using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist
{
    public class 狱火蛇Checklist : ModSystem
	{
		public float Time;

		public bool DriftBool;
		public float Drift;

		public int[] Body = new int[15];
		public Vector2[] Center = new Vector2[15];
		public float[] Rotation = new float[15];
		public int TI = 3;
		public int Direction = 1;
		public Vector2 Velocity;
		public bool Collide;
		public float T;


		public Vector2[] MeteorProbePosition = new Vector2[5];
		public Vector2[] MeteorProbeVelocity = new Vector2[5];

		public float flame;
		public bool flameBool;

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/地狱Checklist");
		}
		public override void Unload()
        {
			BossChecklistBook = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
			if (TI > 0)
			{
				TI--;
			}
			else
			{
				for (int a = 0; a < Center.Length; a++)
				{
					Center[a] = Vector2.Zero;
					Drift = -2;
					Collide = true;
				}
			}
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

			int bossType = ModContent.NPCType<狱火蛇头>();

			List<int> collection = new List<int>()
			{
				//ModContent.ItemType<DrillingTool>(),
				ModContent.ItemType<狱火晶石>()
			};

			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
		        TI = 60;
				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<狱火蛇头>()].Value;
				Texture2D texture2 = TextureAssets.Npc[ModContent.NPCType<狱火蛇身>()].Value;
				Texture2D texture3 = TextureAssets.Npc[ModContent.NPCType<狱火蛇身2>()].Value;
				Texture2D texture4 = TextureAssets.Npc[ModContent.NPCType<狱火蛇尾>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

				if (Center[0] == Vector2.Zero)
				{
					Center[0] = new Vector2(rect.Width / 2, rect.Height / 1.2f);
				}
				Center[0] += Velocity;
				Velocity = Rotation[0].ToRotationVector2()*5;
				if (Center[0].Y<rect.Height*0.4f)
                {
					if(Collide)
						T = Main.rand.NextFloat(rect.Width * 0.02F, rect.Width * 0.2F);
					Collide = false;
				}
				else if(Center[0].Y > rect.Height * 0.6f)
				{
					if (!Collide)
						T = Main.rand.NextFloat(rect.Width * 0.8F, rect.Width * 0.98F);
					Collide = true;
				}
				if(Collide)
                {
					DDHelper.RotateSpeed(ref Rotation[0], (new Vector2(T, rect.Height *0.2F) - Center[0]).ToRotation(), 0.06f);
                }
				else
				{
					DDHelper.RotateSpeed(ref Rotation[0], (new Vector2(T, rect.Height *0.75F) - Center[0]).ToRotation(), 0.06f);
                }
                float AngleDifference(float a, float b)
                {
                    float diff = (a - b + (float)Math.PI) % (float)(2 * Math.PI) - (float)Math.PI;
                    return diff < -(float)Math.PI ? diff + (float)(2 * Math.PI) : diff;
                }
                for (int B = 0; B < Body.Length; B++)
                {
                    if (B > 0)
                    {
                        float ro = AngleDifference(Rotation[B - 1], Rotation[B]);
                        Center[B] -= (Rotation[B - 1]).ToRotationVector2() * Math.Abs(ro) * 10;

                        Vector2 vector = Center[B - 1] - Center[B];
                        Rotation[B] = (float)Math.Atan2(vector.Y, vector.X);

                        float D = (vector.Length() - 30) / vector.Length();
                        //Center[B] += vector * D;
                        Center[B] = Center[B] + vector * D;

                    }
                }
                for (int B = Body.Length - 1; B >= 0; B--)
				{
					Vector2 vector = new Vector2(rect.X, rect.Y);
					if (B == 0)
					{
						sb.Draw(texture, Center[0] + vector + Rotation[0].ToRotationVector2() * 28, null, color, Rotation[0] + 1.57f, texture.Size() / 2, 1f, 0, 0);
					}
					else if (B < Body.Length - 1)
					{
						if (B % 2 == 1)
						{
							sb.Draw(texture2, Center[B] + vector, null, color, Rotation[B] + 1.57f, texture2.Size() / 2, 1f, 0, 0);
						}
						else
						{
							sb.Draw(texture3, Center[B] + vector, null, color, Rotation[B] + 1.57f, texture3.Size() / 2, 1f, 0, 0);
						}
					}
					else
					{
						sb.Draw(texture4, Center[B] + vector, null, color, Rotation[B] + 1.57f, texture4.Size() / 2, 1f, 0, 0);
					}
				}

				Time += 0.03f;
			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			int summonItem = ModContent.ItemType<火蛇卵>();
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"狱火蛇",
				12.7f,
				() => NPCDowned.欲火蛇,
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