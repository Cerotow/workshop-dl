using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Content.NPCs.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DDmod.ModLinkage.BossChecklist
{
    public class 恐惧缝合体Checklist : ModSystem
	{

		public bool DriftBool;
		public float Drift;

        //帧
        public int frame;
        public float frameCounter;

        //Boss位置和boss移动
        public Vector2 BossPosition;
        public Vector2 BossVelocity;

        public Vector2[] monsterPosition = new Vector2[5];
        public int[] Mframe = new int[5];
        public float[] MframeCounter = new float[5];

        public float flame;
		public bool flameBool;

        public bool[] Bool = new bool[6];
        public float[] Time = new float[6];

        public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/血月Checklist");
			for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
			{
                ChecklistHelper.Gore[a] = new BGore();
			}
			for(int a = 0;a<6;a++)
			{
				Time[a] = Main.rand.NextFloat(-20, 20);
            }
		}
		public override void Unload()
        {
			BossChecklistBook = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
		}
		public void DrawB(SpriteBatch sb, Rectangle rect, Color color)
		{

			Texture2D texture = TextureAssets.Npc[ModContent.NPCType<恐惧缝合体>()].Value;
			Texture2D texture2 = TextureAssets.Npc[ModContent.NPCType<恐惧滋生体>()].Value;
			Vector2 centered = new Vector2(rect.X, rect.Y);

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
				DDHelper.BackAndForth(-20, 20, 0.5F, ref Time[a], ref Bool[a]);
            }
            BossPosition = new Vector2(rect.Width / 2, rect.Height / 2f+ Time[5]) + centered;
            float Rotation = 0.4f;
			sb.Draw(texture, BossPosition, new Rectangle?(new Rectangle(0, texture.Height / 5 * frame, texture.Width, texture.Height / 4)), color, Rotation, new Vector2(texture.Width, texture.Height / 5) / 2, 1.3f, SpriteEffects.FlipHorizontally, 0);

			int fx = 0;
			int fy = 0;
			if (frame != 0 && frame != 4)
			{
				fy = 2;
			}
			if (frame == 2)
			{
				fx = 6;
			}
			if (frame == 3)
			{
				fx = 4;
			}
			if (frame == 4)
			{
				fx = -2;
			}
			Color color1 = color;
			texture = 恐惧缝合体.YJ.Value;
			sb.Draw(texture, (BossPosition + (new Vector2(-(22 + fx), -29) * 1.3f).RotatedBy(Rotation)) + new Vector2(8, 0), null, color, Rotation, new Vector2(texture.Width, texture.Height) / 2, 1.3F, 0, 0f);
			color *= 0.75F;
			color.A = 255;
			sb.Draw(texture, (BossPosition + (new Vector2(29, 17 + fy) * 1.3F).RotatedBy(Rotation)) + new Vector2(8, 0), null, color, Rotation, new Vector2(texture.Width, texture.Height) / 2, 1.3F * 0.75F, 0, 0f);
			texture = 恐惧缝合体.YR.Value;
			sb.Draw(texture, BossPosition, new Rectangle?(new Rectangle(0, texture.Height / 5 * frame, texture.Width, texture.Height / 4)), color1, Rotation, new Vector2(texture.Width, texture.Height / 5) / 2, 1.3f, SpriteEffects.FlipHorizontally, 0);

			monsterPosition[0] = new Vector2(rect.Width / 2 - 130, rect.Height / 2f - 60 + Time[0]) + centered;
			monsterPosition[1] = new Vector2(rect.Width / 2 - 40, rect.Height / 2f - 100 + Time[1]) + centered;
			monsterPosition[2] = new Vector2(rect.Width / 2 + 20, rect.Height / 2f - 130 + Time[2]) + centered;
			monsterPosition[3] = new Vector2(rect.Width / 2 + 70, rect.Height / 2f - 110 + Time[3]) + centered;
			monsterPosition[4] = new Vector2(rect.Width / 2 + 130, rect.Height / 2f - 60 + Time[4]) + centered;
			for (int a = 0; a < 5; a++)
            {
                MframeCounter[a]++;
                if (MframeCounter[a] >= 6)
                {
                    MframeCounter[a] = 0;
                    Mframe[a]++;
                }
                if (Mframe[a] >= 4)
                {
                    Mframe[a] = 0;
                }
                texture2 = TextureAssets.Npc[ModContent.NPCType<恐惧滋生体>()].Value;
                sb.Draw(texture2, monsterPosition[a], new Rectangle?(new Rectangle(0, texture2.Height / 4 * Mframe[a], texture2.Width, texture2.Height / 4)), color1, Rotation, new Vector2(texture2.Width, texture2.Height / 4) / 2, 1f, SpriteEffects.FlipHorizontally, 0);
				texture2 = 恐惧滋生体.YJ.Value;
                Vector2 ve = Vector2.Zero;
                if (Mframe[a] ==  3)
                {
                    ve.X -= 2;
                }
                if (Mframe[a] == 1)
                {
                    ve.Y -= 2;
                }
                sb.Draw(texture2, monsterPosition[a] + (new Vector2(ve.X, 10 + ve.Y)).RotatedBy(Rotation) + new Vector2(4, 0), null, color1, Rotation, texture2.Size() / 2, 1f, SpriteEffects.FlipHorizontally, 0);
            }
			if (frame >= 2)
			{
				if (!flameBool)
				{
					for (int a = 0; a < 30; a++)
					{
						int A = BDust.NewDust(BossPosition- rect.TopLeft() - (new Vector2(-26, 19) * 1.3F).RotatedBy(Rotation), new Vector2(0, 1), 5, scale: 1.8F, ChecklistHelper.恐惧缝合体);
                        ChecklistHelper.Dust[A].velocity = new Vector2(1, -1).RotatedBy(Rotation).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(1, 3);
					}
					flameBool = true;

                }
			}
			else
			{
				flameBool = false;

            }
            for (int a = 0; a < 200; a++)
            {
                if (ChecklistHelper.Dust[a] == null)
                {
                    ChecklistHelper.Dust[a] = new BDust();
                }
                ChecklistHelper.Dust[a].UpdateDust(a);
                ChecklistHelper.Dust[a].Dawn(sb, rect, ChecklistHelper.恐惧缝合体);
            }
        }
        int CAIDAN;
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

			int bossType = ModContent.NPCType<恐惧缝合体>();

			List<int> collection = new List<int>()
			{
			};

			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				if (CAIDAN>0)
                {
					CAIDAN--;
				}
				if (Main.rand.NextBool(10000))
				{
					CAIDAN = 3000;
				}
				if (Main.rand.NextBool(10))
				{
					float A = Main.rand.NextFloat(1f, 1f);
					int GoreType = Mod.Find<ModGore>("Cloud_" + Main.rand.Next(22)).Type;
					if(Main.rand.NextBool(300))
                    {
						GoreType = Mod.Find<ModGore>("Cloud_" + Main.rand.Next(22,41)).Type;
					}
					if(CAIDAN>0)
					{
						GoreType = Mod.Find<ModGore>("Cloud_" + Main.rand.Next(22, 41)).Type;
					}
					int Gore = BGore.NewGore(new Vector2(rect.Width+16, Main.rand.NextFloat(-300, rect.Height)), new Vector2(-9 * A, 0), GoreType, A, Main.rand.Next(2), ChecklistHelper.恐惧缝合体);
                    ChecklistHelper.Gore[Gore].gravity = false;
                    ChecklistHelper.Gore[Gore].color = new Color(150,0,15,150)*Main.rand.NextFloat(0.3F,1.2F);
					if (CAIDAN > 0)
					{
						//鬼牙Checklist.Gore[Gore].color = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256));
					}
                    ChecklistHelper.Gore[Gore].Norotating = true;
				}
				for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
				{
					if (ChecklistHelper.Gore[a] != null)
					{
						if (ChecklistHelper.Gore[a].active)
						{
							if (ChecklistHelper.Gore[a].position.Y > (rect.Height * 2))
							{
                                ChecklistHelper.Gore[a].active = false;
							}
							else if (ChecklistHelper.Gore[a].position.Y < (-rect.Height * 2))
							{
                                ChecklistHelper.Gore[a].active = false;
							}
							else if (ChecklistHelper.Gore[a].position.X > (rect.Width * 2))
							{
                                ChecklistHelper.Gore[a].active = false;
							}
							else if (ChecklistHelper.Gore[a].position.X < (-rect.Width * 2))
							{
                                ChecklistHelper.Gore[a].active = false;
							}
                            ChecklistHelper.Gore[a].UpdateGore(a);
						}
					}
				}
				for (int a = 0;a< ChecklistHelper.Gore.Length;a++)
                {
					if(ChecklistHelper.Gore[a]!=null&& ChecklistHelper.Gore[a].post==0)
                    {
                        ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.恐惧缝合体);
					}
                }

			    DrawB(sb, rect, color);
				for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
				{
					if (ChecklistHelper.Gore[a] != null && ChecklistHelper.Gore[a].post==1)
					{
                        ChecklistHelper.Gore[a].Dawn(sb, rect,ChecklistHelper.恐惧缝合体);
					}
				}
			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			int summonItem = 4271;
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
                "恐惧缝合体",
				3.7f,
				() => NPCDowned.恐惧缝合体,
				bossType,
				new Dictionary<string, object>()
				{
					//["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
					//召唤物
					["spawnItems"] = summonItem,
					//收藏品
					//["collectibles"] = collection,
					//绘制
					["customPortrait"] = customBossPortrait,
				}
			);

		}
	}
}