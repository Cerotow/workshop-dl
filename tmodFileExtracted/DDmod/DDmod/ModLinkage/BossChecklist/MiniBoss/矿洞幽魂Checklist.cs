using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.Boss.夜光蘑菇王;
using DDmod.Content.NPCs.Boss.蘑菇王;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
	public class 矿洞幽魂Checklist : ModSystem
	{
		public float Rotation;
		public Vector2 ProjPosition;
		public Vector2 ProjVelocity;
		//Boss位置和boss移动
		public Vector2 BossPosition;
		public Vector2 BossVelocity;

		//帧
		public int frame;
		public int frame2;
		public float frameCounter;

        public bool Bool;
        public float Time;

        public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/洞穴Checklist");
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

			int bossType = ModContent.NPCType<矿洞幽魂>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				BossPosition.X =  rect.Width / 2+50;
                BossPosition.Y =  rect.Height *0.5f+ Time;
                DDHelper.BackAndForth(-20F, 20F, 0.1F, ref Time, ref Bool);

                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<矿洞幽魂>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

				frameCounter++;
				if (frameCounter >= 10)
				{
					frameCounter = 0;
					frame++;
				}
				if (frame >= 8)
				{
					frame = 0;
				}
				if(Main.rand.NextBool(4))
                {
                    int A =BDust.NewDust(BossPosition + new Vector2(Main.rand.NextFloat(-20, 20), 30), new Vector2(0, 1), ModContent.DustType<光球粒子>(), scale: 1.2F, ChecklistHelper.矿洞幽魂);
					ChecklistHelper.Dust[A].color = new Color(0, 245, 255, 0);
					ChecklistHelper.Dust[A].Extraspeed = -0.8f;

                }
                Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;
                sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 8 * frame, texture.Width, texture.Height / 8)), color, Rotation, new Vector2(texture.Width, texture.Height / 8) / 2, 1.3f, SpriteEffects.FlipHorizontally, 0);
                color.A = 0;
                sb.Draw(矿洞幽魂.Glow.Value, vector, new Rectangle?(new Rectangle(0, texture.Height / 8 * frame, texture.Width, texture.Height / 8)), color, Rotation, new Vector2(texture.Width, texture.Height / 8) / 2, 1.3f, SpriteEffects.FlipHorizontally, 0);
                sb.Draw(矿洞幽魂.Glow.Value, vector, new Rectangle?(new Rectangle(0, texture.Height / 8 * frame, texture.Width, texture.Height / 8)), color, Rotation, new Vector2(texture.Width, texture.Height / 8) / 2, 1.3f, SpriteEffects.FlipHorizontally, 0);

                for (int a = 0; a < 200; a++)
                {
                    if (ChecklistHelper.Dust[a] == null)
                    {
                        ChecklistHelper.Dust[a] = new BDust();
                    }
                    ChecklistHelper.Dust[a].UpdateDust(a);
                    ChecklistHelper.Dust[a].Dawn(sb, rect, ChecklistHelper.矿洞幽魂);
                }

            }
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                    DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};

			bossChecklistMod.Call(
                "LogMiniBoss",
                Mod,
                "矿洞幽魂",
				1.4f,
				() => NPCDowned.矿洞幽魂,
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