using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist
{
    public class MeteorDiggerChecklist : ModSystem
	{
		public float Time;

		public bool DriftBool;
		public float Drift;

		public int[] Body = new int[10];
		public Vector2[] Center = new Vector2[10];
		public float[] Rotation = new float[10];
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
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/MeteorDiggerChecklist");
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

			int bossType = ModContent.NPCType<MeteorDigger_Head>();

			List<int> collection = new List<int>()
			{
				ModContent.ItemType<DrillingTool>(),
				ModContent.ItemType<SlightlyDamagedCore>()
			};

			string despawnInfo = null;
			//绘制小弟
			void DrawMobs(SpriteBatch sb, Rectangle rect, Color color)
            {
                Vector2 centered = new Vector2(rect.X, rect.Y);

				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<MeteorProbe>()].Value;

				Texture2D VoidStar = DDTextures.VoidStar.Value;
				Texture2D Scanning = DDTextures.Scanning.Value;

				DDHelper.BackAndForth(0, 0.3F, 0.05f, ref flame, ref flameBool);

				for (int T = 0; T < 5; T++)
				{
					MeteorProbePosition[T] += MeteorProbeVelocity[T];
					if (MeteorProbePosition[T].X < 0 || MeteorProbePosition[T].X > rect.Width)
					{
						MeteorProbeVelocity[T].X *= -1;
					}
					if (MeteorProbePosition[T].Y < 0 || MeteorProbePosition[T].Y > rect.Height - 50)
					{
						MeteorProbeVelocity[T].Y *= -1;
					}
					DDHelper.MaxandMinF(ref MeteorProbePosition[T].X, rect.Width, 0);
					DDHelper.MaxandMinF(ref MeteorProbePosition[T].Y, rect.Height - 50, 0);
					if (MeteorProbeVelocity[T] == Vector2.Zero)
					{
						MeteorProbeVelocity[T] = new Vector2(5, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
					}
					if (Main.rand.NextBool(100))
					{
						MeteorProbeVelocity[T] = MeteorProbeVelocity[T].RotatedBy(Main.rand.NextFloat(-0.8F, 0.8F));
					}

					Vector2 vector = centered + MeteorProbeVelocity[T].PerfectNormalize() * -6 + MeteorProbePosition[T];

					sb.Draw(Scanning, centered + MeteorProbePosition[T], null, new Color(0, 255, 0, 0), MeteorProbeVelocity[T].ToRotation() + MathHelper.PiOver4 - MathHelper.PiOver2, new Vector2(0, 0), 0.6F + flame / 3, 0, 0f);
					for (int a = 0; a < 5; a++)
					{
						sb.Draw(VoidStar, vector, new Rectangle?(new Rectangle(0, 0, (int)(VoidStar.Width / 1.7F), VoidStar.Height)), new Color(253, 62, 3, 0) * 1f, MeteorProbeVelocity[T].ToRotation(), VoidStar.Size() / 2, new Vector2(0.5F + flame, 0.285f) * (0.5F + flame), 0, 0f);
					}
					sb.Draw(texture, centered + MeteorProbePosition[T], null, color, MeteorProbeVelocity[T].ToRotation()+MathHelper.PiOver2, texture.Size() / 2, 1, 0, 0f);
                }

            }
            //绘制Boss
            void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				TI = 60;
				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<MeteorDigger_Head>()].Value;
				Texture2D texture2 = TextureAssets.Npc[ModContent.NPCType<MeteorDigger_Body>()].Value;
				Texture2D texture3 = TextureAssets.Npc[ModContent.NPCType<MeteorDigger_Tail>()].Value;
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

                        float D = (vector.Length() - 44) / vector.Length();
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
						sb.Draw(texture2, Center[B] + vector, null, color, Rotation[B] + 1.57f, texture2.Size() / 2, 1f, 0, 0);
					}
					else
					{
						sb.Draw(texture3, Center[B] + vector, null, color, Rotation[B] + 1.57f, texture3.Size() / 2, 1f, 0, 0);
					}
				}

				Time += 0.03f;
			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawMobs(sb, rect, color);
				DrawBoss(sb, rect, color);
                };
				sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
            };
			int summonItem = ModContent.ItemType<AlienRigController>();


			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"流星掘地者",
				3.7f,
				() => NPCDowned.downedMeteorDigger,
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