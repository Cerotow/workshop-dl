using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.农场.食物.料理;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.Boss.夜光蘑菇王;
using DDmod.Content.NPCs.Boss.蘑菇王;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist
{
	public class 蘑菇王Checklist : ModSystem
	{
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

		public bool[] Bool = new bool[6];
		public float[] Time = new float[6];

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			for(int a = 0;a<6;a++)
			{
				Time[a] = Main.rand.NextFloat(-0.1F,0.1F);
            }
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/草地Checklist");
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

			int bossType = ModContent.NPCType<蘑菇王>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				BossPosition.X =  rect.Width / 2;
                BossPosition.Y = rect.Height - 92;

                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<蘑菇王>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

				frameCounter++;
				if (frameCounter >= 6)
				{
					frameCounter = 0;
					frame++;
				}
				if (frame >= 5)
				{
					frame = 0;
				}

                for (int a = 0; a < 6; a++)
                {
                    DDHelper.BackAndForth(-0.1F, 0.1F, 0.01F, ref Time[a], ref Bool[a]);
                }
                Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;
				sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 11 * frame, texture.Width / 2, texture.Height / 11)), color, Rotation, new Vector2(texture.Width / 2, texture.Height / 11) / 2, 1.3f, SpriteEffects.FlipHorizontally, 0);
                texture = TextureAssets.Npc[ModContent.NPCType<蘑菇怪>()].Value;
                sb.Draw(texture, vector+new Vector2(54,44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1+Time[0],1- Time[0]), SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector + new Vector2(80, 44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time[1], 1 - Time[1]), SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector + new Vector2(110, 44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time[2], 1 - Time[2]), SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector - new Vector2(50, -44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time[3], 1 - Time[3]), SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector - new Vector2(80, -44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time[4], 1 - Time[4]), SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector - new Vector2(106, -44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time[5], 1 - Time[5]), SpriteEffects.FlipHorizontally, 0);

			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			int summonItem = ModContent.ItemType<奇怪的蘑菇汤>();

            bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"蘑菇王",
				1.2f,
				() => NPCDowned.蘑菇王,
				bossType,
				new Dictionary<string, object>()
                {
                    //召唤物
                    ["spawnItems"] = summonItem,
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