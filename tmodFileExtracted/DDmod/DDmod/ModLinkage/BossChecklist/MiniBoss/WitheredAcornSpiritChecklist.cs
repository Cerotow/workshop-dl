using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
	[AutoloadBossHead]
	public class WitheredAcornSpiritChecklist : ModSystem
	{
		public float Time;
		public int ProjTime;
		public float Rotation;
		public Vector2 ProjPosition;
		public Vector2 ProjVelocity;
		//Boss位置和boss移动
		public Vector2 BossPosition;
		public Vector2 BossVelocity;

		//背景
		public bool DriftBool;
		public float Drift;
		//帧
		public int frame;
		public float frameCounter;


		public Vector2[] oldPos = new Vector2[18];

		public int Break;

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/MiniBoss/WitheredAcornSpiritChecklist");
		}
		public override void Unload()
		{
			BossChecklistBook = null;
		}

		public override void PostSetupContent()
		{
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
			{
				return;
			}
			if (bossChecklistMod.Version < new Version(1, 6))
			{
				return;
			}

			int bossType = ModContent.NPCType<WitheredAcornSpirit>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<WitheredAcornSpirit>()].Value;
				Texture2D texture2 = WitheredAcornSpirit.Glow.Value;
				Texture2D texture3 = WitheredAcornSpirit.Glow2.Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

				BossPosition += BossVelocity;
				if (Break == 0)
				{
					Drift += 0.01F;
					frameCounter++;
					if (frameCounter > 10)
					{
						frameCounter = 0;
						frame++;
					}
					if (frame >= 6)
					{
						frame = 0;
					}
					BossVelocity = (BossVelocity * 20 + (rect.Size() / 2 - BossPosition).PerfectNormalize() * 10) / 21;
					Rotation = BossVelocity.X * 0.03F;
				}
				else
				{
					Break--;
					BossVelocity.Y += 0.2F;

					if (BossPosition.Y > rect.Height - 66)
					{
						BossVelocity.Y = 0;
					}
					if (BossPosition.X > rect.Width - 20 || BossPosition.X < 20)
					{
						BossVelocity.X = 0;
					}
					frame = 3;
					Rotation += BossVelocity.X * 0.03F;
				}
				Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;
				sb.Draw(texture3, vector, null, new Color(255, 0, 0,0), Rotation, texture3.Size() / 2, Drift, 0, 0);

				sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 6 * frame, texture.Width, texture.Height / 6)), color, Rotation, new Vector2(texture.Width, texture.Height / 6) / 2, 1, 0, 0);
				if(Break>0)
				sb.Draw(texture2, vector, new Rectangle?(new Rectangle(0, texture.Height / 6 * frame, texture.Width, texture.Height / 6)), new Color(25,25,25), Rotation, texture2.Size() / 2, Drift, 0, 0);

				DDHelper.MaxandMinF(ref BossPosition.X, rect.Width-20, 20);
				DDHelper.MaxandMinF(ref BossPosition.Y, rect.Height - 66, 20);
			}
			//绘制弹幕
			void DrawMobs(SpriteBatch sb, Rectangle rect, Color color)
			{
				for (int i = oldPos.Length - 1; i > 0; i--)
				{
					oldPos[i] = oldPos[i - 1];
				}
				oldPos[0] = ProjPosition;
				if (Break == 0)
				{
					ProjTime++;
				}
				ProjPosition += ProjVelocity;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;
				Texture2D texture = DDTextures.VoidStar.Value;
				if (ProjTime > 120)
				{
					ProjPosition = BossPosition;
					ProjVelocity = new Vector2(0, 1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 15;
					BossVelocity -= ProjVelocity / 2;
					ProjTime = 0;
					Drift = 0;
					Break = 120;
				}
				float Length = ProjVelocity.Length() / (10);
				for (float l = 0; l < Length; l++)
				{
					for (int i = 0; i < oldPos.Length; i++)
					{
						Vector2 vector = new Vector2(rect.X, rect.Y) + oldPos[i];
						Color color2 = new Color(255, 0, 0, 0) * ((oldPos.Length - i) / (float)oldPos.Length / 2f);
						sb.Draw(texture, vector - ProjVelocity / Length * l, null, color2, Rotation, texture.Size() / 2, 0.5f * ((oldPos.Length - i) / (float)oldPos.Length), SpriteEffects.None, 0);
						sb.Draw(texture, vector - ProjVelocity / Length * l, null, color2, Rotation, texture.Size() / 2, 0.5f * ((oldPos.Length - i) / (float)oldPos.Length), SpriteEffects.None, 0);
						color2 = new Color(0, 150, 150, 0) * ((oldPos.Length - i) / (float)oldPos.Length / 2f);
						sb.Draw(texture, vector - ProjVelocity / Length * l, null, color2, Rotation, texture.Size() / 2, 0.25f * ((oldPos.Length - i) / (float)oldPos.Length), SpriteEffects.None, 0);
					}
				}
			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
				DrawMobs(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
            };

			bossChecklistMod.Call(
				"LogMiniBoss",
				Mod,
				"枯萎橡果",
				0.5f,
				() => NPCDowned.downedWitheredAcornSpirit,
				bossType,
				new Dictionary<string, object>()
				{
					//["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
					//收藏品
					//["collectibles"] = collection,
					//绘制
					["customPortrait"] = customBossPortrait,
				}
			);

		}
	}
}