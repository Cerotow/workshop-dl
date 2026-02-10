using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.Items.Boss.鬼牙;
using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist
{
    public class 鬼牙Checklist : ModSystem
	{
		public float Time;

		public bool DriftBool;
		public float Drift;

		public int[] Body = new int[6];
		public Vector2[] Center = new Vector2[6];
		public float[] Rotation = new float[6];
		public int TI = 3;
		public int Direction = 1;
		public Vector2 Velocity;
		public bool Collide;
		public float T;
		public float Scale;


		public Vector2[] MeteorProbePosition = new Vector2[5];
		public Vector2[] MeteorProbeVelocity = new Vector2[5];

		public float flame;
		public bool flameBool;

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/鬼牙Checklist");
			for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
			{
                ChecklistHelper.Gore[a] = new BGore();
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

			Texture2D texture = TextureAssets.Npc[ModContent.NPCType<鬼牙头>()].Value;
			Texture2D texture2 = TextureAssets.Npc[ModContent.NPCType<鬼牙身>()].Value;
			Texture2D texture3 = TextureAssets.Npc[ModContent.NPCType<鬼牙尾>()].Value;
			Texture2D textureG = 鬼牙头.Glow.Value;
			Texture2D textureG2 = 鬼牙身.Glow.Value;
			Texture2D textureG3 = 鬼牙尾.Glow.Value;
			Texture2D textureGL = 鬼牙头.Glow2.Value;
			Texture2D textureGL2 = 鬼牙身.Glow2.Value;
			Texture2D textureGL3 = 鬼牙尾.Glow2.Value;
			Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

			if (Center[0] == Vector2.Zero)
			{
				Center[0] = new Vector2(rect.Width / 2, rect.Height / 1.2f);
			}
			Center[0] += Velocity;
			if (Center[0].X > rect.Width * 0.8F)
			{
				float Di = (Center[0].X - rect.Width * 0.8F);
				for (int B = Body.Length - 1; B >= 0; B--)
				{
					Center[B].X -= Di;
				}
			}
			Velocity = Rotation[0].ToRotationVector2() * 10 * Scale;
			if (Center[0].Y < rect.Height * 0.4f + 40 * (1F - Scale))
			{
				Collide = false;
			}
			else if (Center[0].Y > rect.Height * 0.5f - 40 * (1F - Scale))
			{
				Collide = true;
			}
			if (Collide)
			{
				DDHelper.RotateSpeed(ref Rotation[0], -0.8F, 0.06f);
			}
			else
			{
				DDHelper.RotateSpeed(ref Rotation[0], 0.8F, 0.06f);
			}
			for (int B = 0; B < Body.Length; B++)
			{
				if (B > 0)
				{
					if (B < Body.Length - 1)
					{
						Vector2 vector = Center[B - 1] - Center[B];
						Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;

						Center[B] = Center[B - 1] - vector.PerfectNormalize() * 44 * Scale;
						if (Center[B].HasNaNs())
						{
							Center[B] = Vector2.Zero;
						}
						//Center[B] = Center[B - 1] + (Rotation[B] + 1.57f).ToRotationVector2() * 30;
					}
					else
					{
						Vector2 vector = Center[B - 1] - Center[B];
						Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;
						Center[B] = Center[B - 1] - vector.PerfectNormalize() * 56 * Scale;

						if (Center[B].HasNaNs())
						{
							Center[B] = Vector2.Zero;
						}
					}
				}
			}
			for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
			{
				if (ChecklistHelper.Gore[a] != null && ChecklistHelper.Gore[a].post==0)
				{
                    ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.鬼牙);
				}
			}
			color *= T;
			Color color1 = color * 0.5F;
			color1.A = (byte)(255 * T);
			Color color2 = color;
			color2.A = 0;
			for (int B = Body.Length - 1; B >= 0; B--)
			{
				Vector2 vector = new Vector2(rect.X, rect.Y);
				if (Center[B].X > 0 && Center[B].Y > 0 && Center[B].X < rect.Width && Center[B].Y < rect.Height - 50)
				{
					if (B == 0)
					{
						sb.Draw(textureGL, Center[0] + vector, null, color2, Rotation[0] + 1.57f, textureGL.Size() / 2, Scale, 0, 0);
					}
					else if (B < Body.Length - 1)
					{
						sb.Draw(textureGL2, Center[B] + vector, null, color2, Rotation[B], textureGL2.Size() / 2, Scale, 0, 0);
					}
					else
					{
						sb.Draw(textureGL3, Center[B] + vector, null, color2, Rotation[B], textureGL3.Size() / 2, Scale, 0, 0);
					}
				}
			}
			for (int B = Body.Length - 1; B >= 0; B--)
			{
				Vector2 vector = new Vector2(rect.X, rect.Y);
				if (Center[B].X > 0 && Center[B].Y > 0 && Center[B].X < rect.Width && Center[B].Y < rect.Height - 50)
				{
					if (B == 0)
					{
						sb.Draw(texture, Center[0] + vector, null, color1, Rotation[0] + 1.57f, texture.Size() / 2, Scale, 0, 0);
						sb.Draw(textureG, Center[0] + vector, null, color, Rotation[0] + 1.57f, textureG.Size() / 2, Scale, 0, 0);
					}
					else if (B < Body.Length - 1)
					{
						sb.Draw(texture2, Center[B] + vector, null, color1, Rotation[B], texture2.Size() / 2, Scale, 0, 0);
						sb.Draw(textureG2, Center[B] + vector, null, color, Rotation[B], textureG2.Size() / 2, Scale, 0, 0);
					}
					else
					{
						sb.Draw(texture3, Center[B] + vector, null, color1, Rotation[B], texture3.Size() / 2, Scale, 0, 0);
						sb.Draw(textureG3, Center[B] + vector, null, color, Rotation[B], textureG3.Size() / 2, Scale, 0, 0);
					}
				}
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

			int bossType = ModContent.NPCType<鬼牙头>();

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
				if (Time > 600||Main.rand.NextBool(10))
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
					int Gore = BGore.NewGore(new Vector2(rect.Width+16, Main.rand.NextFloat(-300, rect.Height)), new Vector2(-9 * A, 0), GoreType, A, Main.rand.Next(2), ChecklistHelper.鬼牙);
					ChecklistHelper.Gore[Gore].gravity = false;
					ChecklistHelper.Gore[Gore].color = new Color(150,0,15,150)*Main.rand.NextFloat(0.3F,1.2F);
					if (CAIDAN > 0)
					{
						//ChecklistHelper.Gore[Gore].color = new Color(Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256), Main.rand.Next(256));
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
				if (Scale < 0.8F)
					DrawB(sb, rect, color);
				for (int a = 0;a< ChecklistHelper.Gore.Length;a++)
                {
					if(ChecklistHelper.Gore[a]!=null&&ChecklistHelper.Gore[a].post==0)
                    {
                        ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.鬼牙);
					}
                }

				if (Scale < 1&&Scale>=0.8F)
					DrawB(sb, rect, color);
				for (int a = 0; a < ChecklistHelper.Gore.Length; a++)
				{
					if (ChecklistHelper.Gore[a] != null && ChecklistHelper.Gore[a].post==1)
					{
                        ChecklistHelper.Gore[a].Dawn(sb, rect, ChecklistHelper.鬼牙);
					}
				}
				if (Scale >= 1)
				{
					DrawB(sb, rect, color);
				}
				Time++;
				if(Time>700)
                {
					T -= 0.01F;
				}
                else
				{
					T += 0.01F;
				}

				if (TI==0)
				{
					Scale -= 0.1f;
				}
				else
				if (TI==1)
				{
					if (Scale<0.9F)
						Scale += 0.1f;
					else if (Scale > 1F)
						Scale -= 0.1f;
					else
                    {
						Scale = 0.9f;
                    }
				}
				else
				{
					Scale += 0.1f;
				}

				DDHelper.MaxandMinF(ref Scale, 1.1F, 0.7F);
				DDHelper.MaxandMinF(ref T, 1, 0);
				if(Time>900)
                {
					TI = Main.rand.Next(3);
					   Time = 0;
				}
			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			int summonItem = ModContent.ItemType<诡异肉块>();
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"鬼牙",
				13.7f,
				() => NPCDowned.鬼牙,
				bossType,
				new Dictionary<string, object>()
				{
					//["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
					//召唤物
					["spawnItems"] = summonItem,
					//收藏品
					["collectibles"] = collection,
					//绘制
					["customPortrait"] = customBossPortrait,
				}
			);

		}
	}
}