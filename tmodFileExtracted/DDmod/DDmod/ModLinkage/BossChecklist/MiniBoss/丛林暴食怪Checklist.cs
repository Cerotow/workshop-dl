using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MiniBoss.召唤物;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.Boss.夜光蘑菇王;
using DDmod.Content.NPCs.Boss.蘑菇王;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;
using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
	public class 丛林暴食怪Checklist : ModSystem
	{
		public float Rotation;
		//Boss位置和boss移动
		public Vector2 BossPosition;
		public Vector2 BossVelocity;

		//小弟位置和小弟移动
		public Vector2[] MiniPosition = new Vector2[5];
		public Vector2[] MiniVelocity = new Vector2[5];

        float[] MiniBossT = new float[5];
        bool[] MiniBossB = new bool[5];

        //帧
        public int Bossframe;
		public float BossframeCounter;

        //帧
        public int[] frame = new int[5];
		public float[] frameCounter =new float[5];

        public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/丛林Checklist");
		}
		public override void Unload()
		{
			BossChecklistBook = null;
		}
        float BossT;
        bool BossB;
        public void MiniAI(Vector2 vector,int i)
        {
            frameCounter[i]++;
            if (frameCounter[i] >= 8)
            {
                frameCounter[i] = 0;
                frame[i]++;
            }
            if (frame[i] >= 4)
            {
                frame[i] = 0;
            }
            Vector2 Distance = vector - MiniPosition[i];
            if (MiniBossT[i] < 0)
            {
                if (MiniVelocity[i].X > -0.3)
                {
                    MiniVelocity[i].X -= 0.1F;
                }
                MiniBossT[i] += Math.Abs(MiniVelocity[i].X);
                if (MiniBossT[i] > 0)
                {
                    MiniBossT[i] = 0;
                }
            }
            else if (MiniBossT[i] > 0)
            {
                if (MiniVelocity[i].X < 0.3)
                {
                    MiniVelocity[i].X += 0.1F;
                }
                MiniBossT[i] -= Math.Abs(MiniVelocity[i].X);
                if (MiniBossT[i] < 0)
                {
                    MiniBossT[i] = 0;
                }
            }
            if (!MiniBossB[i])
            {
                if (MiniVelocity[i].Y > -0.3F)
                {
                    MiniVelocity[i].Y -= 0.1F;
                }
            }
            else if (Distance.Y > 0)
            {
                if (MiniVelocity[i].Y < 0.3)
                {
                    MiniVelocity[i].Y += 0.1F;
                }
            }
            if (Distance.Length() > 160 || Math.Abs(Distance.X) > 120)
            {
                MiniVelocity[i] = Distance.PerfectNormalize() * 2;
            }
            if (Distance.Y > 120)
            {
                MiniBossB[i] = true;
            }
            if (Distance.Y < 80)
            {
                MiniBossB[i] = false;
                MiniBossT[i] = Main.rand.NextFloat(-10, 10);
            }
            MiniPosition[i] += MiniVelocity[i];
        }
        public void BossAI(Vector2 vector)
        {
            BossframeCounter++;
            if (BossframeCounter >= 8)
            {
                BossframeCounter = 0;
                Bossframe++;
            }
            if (Bossframe >= 3)
            {
                Bossframe = 0;
            }
            Vector2 Distance = vector - BossPosition;
            Rotation = Distance.ToRotation() - MathHelper.PiOver2;
            if (BossT < 0)
            {
                if (BossVelocity.X > -0.3)
                {
                    BossVelocity.X -= 0.1F;
                }
                BossT += Math.Abs(BossVelocity.X);
                if (BossT > 0)
                {
                    BossT = 0;
                }
            }
            else if (BossT > 0)
            {
                if (BossVelocity.X < 0.3)
                {
                    BossVelocity.X += 0.1F;
                }
                BossT -= Math.Abs(BossVelocity.X);
                if (BossT < 0)
                {
                    BossT = 0;
                }
            }
            if (!BossB)
            {
                if (BossVelocity.Y > -0.3F)
                {
                    BossVelocity.Y -= 0.1F;
                }
            }
            else if (Distance.Y > 0)
            {
                if (BossVelocity.Y < 0.3)
                {
                    BossVelocity.Y += 0.1F;
                }
            }
            if (Distance.Length() > 600||Math.Abs(Distance.X)>120)
            {
                BossVelocity = Distance.PerfectNormalize() * 2;
            }
            if (Distance.Y > 180)
            {
                BossB = true;
            }
            if (Distance.Y < 80)
            {
                BossB = false;
                BossT = Main.rand.NextFloat(-3, 3);
            }
            BossPosition += BossVelocity;
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

			int bossType = ModContent.NPCType<丛林暴食怪>();
			string despawnInfo = null;
			
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				Vector2 gen = new Vector2(rect.X, rect.Y) + rect.Size() / 2+new Vector2(0,120);
				if(BossPosition== Vector2.Zero)
                {
                    BossPosition = rect.Size() / 2;
                }
                BossAI(gen- new Vector2(rect.X, rect.Y));
                for(int A = 0;A<5;A++)
                {

                    MiniAI(gen - new Vector2(rect.X, rect.Y),A);
                }
                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<丛林暴食怪>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

                Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;


                Asset<Texture2D> L = 丛林暴食怪根.ML;
                Asset<Texture2D> L2 = 丛林暴食怪根.ML2;
                for (int A = 0; A < 5; A++)
                {
                    Vector2 Distance = gen - new Vector2(rect.X, rect.Y) - MiniPosition[A];

                    vector = new Vector2(rect.X, rect.Y) + MiniPosition[A];
                    float MiniLen = (gen - vector).Length();
                    for (int W = 0; W < MiniLen / L.Height(); W++)
                    {
                        if (W < MiniLen / L.Height() - 1)
                        {
                            Vector2 P = gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W);
                            Main.spriteBatch.Draw(L.Value, gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W), new Rectangle?(new Rectangle(0, 0, (int)(L.Width()), L.Height())), color, (vector - gen).ToRotation() - MathHelper.PiOver2, new Vector2(L.Width() / 2, 10), 1, 0, 0f);
                            Main.spriteBatch.Draw(L2.Value, gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W - 2), new Rectangle?(new Rectangle(0, 0, (int)(L2.Width()), L2.Height())), color, (vector - gen).ToRotation() - MathHelper.PiOver2, new Vector2(L2.Width() / 2, 16), 1, 0, 0f);
                        }
                        else
                        {
                            Vector2 P = gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W);
                            Main.spriteBatch.Draw(L.Value, gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W), new Rectangle?(new Rectangle(0, 0, (int)(L.Width()), (int)(L.Height() - (L.Height() - (vector - gen).Length() % L.Height())))), color, (vector - gen).ToRotation() - MathHelper.PiOver2, new Vector2(L.Width() / 2, 10), 1, 0, 0f);
                            Main.spriteBatch.Draw(L2.Value, gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W - 2), new Rectangle?(new Rectangle(0, 0, (int)(L2.Width()), (int)(L2.Height() - (L2.Height() - (vector - gen).Length() % L2.Height())))), color, (vector - gen).ToRotation() - MathHelper.PiOver2, new Vector2(L2.Width() / 2, 16), 1, 0, 0f);
                        }
                    }
                    texture = TextureAssets.Npc[ModContent.NPCType<小丛林暴食怪>()].Value;
                    sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 5 * frame[A], texture.Width, texture.Height / 5)), color, Distance.ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width, texture.Height / 5) / 2, 1, SpriteEffects.None, 0);

                }
                vector = new Vector2(rect.X, rect.Y) + BossPosition;
                L = 丛林暴食怪根.L;
                L2 = 丛林暴食怪根.L2;
                float Len = (gen - vector).Length();
                for (int W = 0; W < Len / L.Height(); W++)
                {
                    if (W < Len / L.Height() - 1)
                    {
                        Vector2 P = gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W);
                        Main.spriteBatch.Draw(L.Value, gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W), new Rectangle?(new Rectangle(0, 0, (int)(L.Width()), L.Height())), color, (vector - gen).ToRotation() - MathHelper.PiOver2, new Vector2(L.Width() / 2, 10), 1, 0, 0f);
                        Main.spriteBatch.Draw(L2.Value, gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W - 4), new Rectangle?(new Rectangle(0, 0, (int)(L2.Width()), L2.Height())), color, (vector - gen).ToRotation() - MathHelper.PiOver2, new Vector2(L2.Width() / 2, 16), 1, 0, 0f);
                    }
                    else
                    {
                        Vector2 P = gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W);
                        Main.spriteBatch.Draw(L.Value, gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W), new Rectangle?(new Rectangle(0, 0, (int)(L.Width()), (int)(L.Height() - (L.Height() - (vector - gen).Length() % L.Height())))), color, (vector - gen).ToRotation() - MathHelper.PiOver2, new Vector2(L.Width() / 2, 10), 1, 0, 0f);
                        Main.spriteBatch.Draw(L2.Value, gen + (vector - gen).PerfectNormalize() * (8 + L.Height() * W - 4), new Rectangle?(new Rectangle(0, 0, (int)(L2.Width()), (int)(L2.Height() - (L2.Height() - (vector - gen).Length() % L2.Height())))), color, (vector - gen).ToRotation() - MathHelper.PiOver2, new Vector2(L2.Width() / 2, 16), 1, 0, 0f);
                    }
                }
                texture = TextureAssets.Npc[ModContent.NPCType<丛林暴食怪>()].Value;
                sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 6 * Bossframe, texture.Width, texture.Height / 6)), color, Rotation, new Vector2(texture.Width, texture.Height / 6) / 2, 1, SpriteEffects.None, 0);

                texture = TextureAssets.Npc[ModContent.NPCType<丛林暴食怪根>()].Value;
                sb.Draw(texture, gen,null, color, 0, texture.Size() / 2, 1, SpriteEffects.None, 0);


            }
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                    DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};

            int summonItem = ModContent.ItemType<丛林香水>();

                    bossChecklistMod.Call(
                "LogMiniBoss",
                Mod,
                "丛林暴食怪",
				3.4f,
				() => NPCDowned.丛林暴食怪,
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