using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MiniBoss.召唤物;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.EliteMonster.四柱护卫;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
	public class 星尘护卫Checklist : ModSystem
	{
		public float Time;
        public Vector2[] ProjPos = new Vector2[20];
		public float[] ProjScale = new float[20];
        public int[] ProjTime = new int[20];
        public bool[] ProjBool = new bool[20];
        //Boss位置和boss移动

        public int[] Body = new int[9];
        public Vector2[] Center = new Vector2[9];
        public float[] Rotation = new float[9];
        public int TI = 3;
        public int Direction = 1;
        public Vector2 Velocity;
        public bool Collide;
        public float T;

        //背景
        public bool DriftBool;
		public float Drift;
		//帧
		public int frame;

		public int Break;

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/星尘Checklist");
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

			int bossType = ModContent.NPCType<星尘护卫头>();
            float AngleDifference(float a, float b)
            {
                float diff = (a - b + (float)Math.PI) % (float)(2 * Math.PI) - (float)Math.PI;
                return diff < -(float)Math.PI ? diff + (float)(2 * Math.PI) : diff;
            }
            //绘制Boss
            void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
            {
                TI = 60;
                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<星尘护卫头>()].Value;
                Texture2D texture2 = TextureAssets.Npc[ModContent.NPCType<星尘护卫身>()].Value;
                Texture2D texture3 = TextureAssets.Npc[ModContent.NPCType<星尘护卫尾>()].Value;
                Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;


                if (Center[0] == Vector2.Zero)
                {
                    Center[0] = new Vector2(rect.Width / 2, rect.Height / 1.2f);
                }
                Center[0] += Velocity;
                Velocity = Rotation[0].ToRotationVector2() * 5;
                if (Center[0].Y < rect.Height * 0.4f)
                {
                    if (Collide)
                        T = Main.rand.NextFloat(rect.Width * 0.02F, rect.Width * 0.2F);
                    Collide = false;
                }
                else if (Center[0].Y > rect.Height * 0.6f)
                {
                    if (!Collide)
                        T = Main.rand.NextFloat(rect.Width * 0.8F, rect.Width * 0.98F);
                    Collide = true;
                }
                if (Collide)
                {
                    DDHelper.RotateSpeed(ref Rotation[0], (new Vector2(T, rect.Height * 0.2F) - Center[0]).ToRotation(), 0.06f);
                }
                else
                {
                    DDHelper.RotateSpeed(ref Rotation[0], (new Vector2(T, rect.Height * 0.75F) - Center[0]).ToRotation(), 0.06f);
                }
                for (int B = 0; B < Body.Length; B++)
                {
                    if (B > 0)
                    {
                        float ro = AngleDifference(Rotation[B-1], Rotation[B]);
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
                        sb.Draw(texture, Center[0] + vector + Rotation[0].ToRotationVector2() * 8, null, color, Rotation[0] + MathHelper.PiOver2, texture.Size() / 2, 1f, 0, 0);
                    }
                    else if (B < Body.Length - 1)
                    {
                        sb.Draw(texture2, Center[B] + vector, null, color, Rotation[B]+MathHelper.PiOver2, texture2.Size() / 2, 1f, 0, 0);
                    }
                    else
                    {
                        sb.Draw(texture3, Center[B] + vector, null, color, Rotation[B] + MathHelper.PiOver2, texture3.Size() / 2, 1f, 0, 0);
                    }
                }

                Time += 0.03f;

            }
            //绘制弹幕
            void DrawMobs(SpriteBatch sb, Rectangle rect, Color color)
			{
				for (int A = 0; A < ProjPos.Length; A++)
				{
					if (ProjPos[A] == Vector2.Zero)
					{
						ProjTime[A] = -Main.rand.Next(0, 40);
						ProjScale[A] = Main.rand.NextFloat(0F, 1F);

						ProjPos[A] = new Vector2(Main.rand.NextFloat(-50, rect.Width + 50), Main.rand.NextFloat(-50, rect.Height + 50));
						ProjBool[A] = Main.rand.NextBool(2);

                    }

                    DDHelper.BackAndForth(0F, 1F, 0.03F, ref ProjScale[A], ref ProjBool[A]);

					Texture2D texture = Main.Assets.Request<Texture2D>("Images/Misc/StarDustSky/Star " + (A < 10 ? 0 : 1)).Value;
                    sb.Draw(texture, ProjPos[A]+rect.TopLeft(), null, Color.White, 0, texture.Size()/2, ProjScale[A], 0, 0);
                    if(ProjScale[A]==0&& Main.rand.NextBool(5))
                    {

                        ProjPos[A] = new Vector2(Main.rand.NextFloat(-50, rect.Width + 50), Main.rand.NextFloat(-50, rect.Height + 50));
                    }

				}
			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                    DrawMobs(sb, rect, color);
                    DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
            };

            int summonItem = ModContent.ItemType<群星符星尘>();

            bossChecklistMod.Call(
        "LogMiniBoss",
        Mod,
        "星尘护卫",

               17.94f,
        () => NPCDowned.星尘护卫,
        bossType,
        new Dictionary<string, object>()
        {
            //["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
            //收藏品
            //["collectibles"] = collection,
            //召唤物
            ["spawnItems"] = summonItem,
            //绘制
            ["customPortrait"] = customBossPortrait,
				}
			);

		}
	}
}