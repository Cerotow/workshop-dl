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
	public class 夜光蘑菇王Checklist : ModSystem
	{
		public float Time;
		public float Rotation;
		//Boss位置和boss移动
		public Vector2 BossPosition;
		public Vector2 BossVelocity;

		//背景
		public bool DriftBool;
		public float Drift;
		//帧
		public int frame;
		public float frameCounter;

		public bool Bool;

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/蘑菇地Checklist");
		}
		public override void Unload()
		{
			BossChecklistBook = null;
            for (int a = 0; a < 6; a++)
            {
                Time2[a] = Main.rand.NextFloat(-0.1F, 0.1F);
            }
        }
        public bool[] Bool2 = new bool[6];
        public float[] Time2 = new float[6];

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

			int bossType = ModContent.NPCType<夜光蘑菇王>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				for (int a = 0; a < 200; a++)
				{
					if(ChecklistHelper.Dust[a]==null)
                    {
                        ChecklistHelper.Dust[a] = new BDust();
					}
                    ChecklistHelper.Dust[a].UpdateDust(a);
                    ChecklistHelper.Dust[a].Dawn(sb,rect, ChecklistHelper.夜光蘑菇王);
				}
				BossPosition.X =  rect.Width / 2;
                BossPosition.Y = rect.Height - 84;

                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<夜光蘑菇王>()].Value;
				if (Bool)
				{
					DDHelper.BackAndForth(-8, 4, 2F, ref Time, ref Bool);
					float I = 0;
					for (int a = 0; a < 14; a++)
					{
						if (a > 3)
						{
							I += 0.2F * a;
							float l = 7 * a;
							int A = BDust.NewDust(BossPosition + new Vector2(l, Time * I), new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9)), ModContent.DustType<光球粒子>(), scale: (1 - a / 20f) + 1F, ChecklistHelper.夜光蘑菇王);

                            ChecklistHelper.Dust[A].color = new Color(74, 189, 226);
                            ChecklistHelper.Dust[A].Extraspeed = -6;

							l = -7 * a;
							int A2 = BDust.NewDust(BossPosition + new Vector2(l, Time * I), new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9)), ModContent.DustType<光球粒子>(), scale: (1 - a / 20f) + 1F, ChecklistHelper.夜光蘑菇王);

                            ChecklistHelper.Dust[A2].color = new Color(74, 189, 226);
                            ChecklistHelper.Dust[A2].Extraspeed = -6;
						}
					}
				}
				else
				{
					DDHelper.BackAndForth(-8, 8, 0.4f, ref Time, ref Bool);
					float I = 0;
					for (int a = 0; a < 14; a++)
					{
						if (a > 3)
						{
							I += 0.2F * a;
							float l = 7 * a;
							int A = BDust.NewDust(BossPosition  + new Vector2(l, Time * I), new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9)), ModContent.DustType<光球粒子>(), scale: (1 - a / 20f) + 1F, ChecklistHelper.夜光蘑菇王);

                            ChecklistHelper.Dust[A].color = new Color(74, 189, 226);
                            ChecklistHelper.Dust[A].Extraspeed = -6;

							l = -7 * a;
							int A2 = BDust.NewDust(BossPosition  + new Vector2(l, Time * I), new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9)), ModContent.DustType<光球粒子>(), scale: (1 - a / 20f) + 1F, ChecklistHelper.夜光蘑菇王);

                            ChecklistHelper.Dust[A2].color = new Color(74, 189, 226);
                            ChecklistHelper.Dust[A2].Extraspeed = -6;
						}
					}
				}
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

				Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;
				sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 11 * frame, texture.Width/2, texture.Height / 11)), new Color(74, 189, 226,0), Rotation, new Vector2(texture.Width/2, texture.Height / 11) / 2, 1.3f*1.05F,SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 11 * frame, texture.Width/2, texture.Height / 11)), new Color(74, 189, 226, 0), Rotation, new Vector2(texture.Width/2, texture.Height / 11) / 2, 1.3f*1.1F, SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 11 * frame, texture.Width/2, texture.Height / 11)), new Color(74, 189, 226, 0), Rotation, new Vector2(texture.Width/2, texture.Height / 11) / 2, 1.3f*1.15F, SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 11 * frame, texture.Width / 2, texture.Height / 11)), Color.White, Rotation, new Vector2(texture.Width / 2, texture.Height / 11) / 2, 1.3f, SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 11 * frame, texture.Width / 2, texture.Height / 11)), new Color(74, 189, 226, 0), Rotation, new Vector2(texture.Width / 2, texture.Height / 11) / 2, 1.3f, SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 11 * frame, texture.Width / 2, texture.Height / 11)), new Color(74, 189, 226, 0), Rotation, new Vector2(texture.Width / 2, texture.Height / 11) / 2, 1.3f, SpriteEffects.FlipHorizontally, 0);

				DDHelper.MaxandMinF(ref BossPosition.X, rect.Width - 20, 20);
				DDHelper.MaxandMinF(ref BossPosition.Y, rect.Height - 96, 20);

                for (int a = 0; a < 6; a++)
                {
                    DDHelper.BackAndForth(-0.1F, 0.1F, 0.01F, ref Time2[a], ref Bool2[a]);
                }
                texture = TextureAssets.Npc[ModContent.NPCType<史莱菇>()].Value;
                sb.Draw(texture, vector + new Vector2(54, 44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time2[0], 1 - Time2[0]), SpriteEffects.FlipHorizontally, 0);
                sb.Draw(texture, vector + new Vector2(80, 44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time2[1], 1 - Time2[1]), SpriteEffects.FlipHorizontally, 0);
                sb.Draw(texture, vector + new Vector2(110, 44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time2[2], 1 - Time2[2]), SpriteEffects.FlipHorizontally, 0);
                sb.Draw(texture, vector - new Vector2(50, -44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time2[3], 1 - Time2[3]), SpriteEffects.FlipHorizontally, 0);
                sb.Draw(texture, vector - new Vector2(80, -44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time2[4], 1 - Time2[4]), SpriteEffects.FlipHorizontally, 0);
                sb.Draw(texture, vector - new Vector2(106, -44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation, new Vector2(texture.Width, texture.Height) / 2, new Vector2(1 + Time2[5], 1 - Time2[5]), SpriteEffects.FlipHorizontally, 0);

            }
            var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};

            int summonItem = ModContent.ItemType<发光的蘑菇汤>();
            bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"夜光蘑菇王",
				2.5f,
				() => NPCDowned.夜光蘑菇王,
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