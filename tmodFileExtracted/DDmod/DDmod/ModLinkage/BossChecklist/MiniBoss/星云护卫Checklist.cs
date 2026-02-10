using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MiniBoss.召唤物;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.EliteMonster.四柱护卫;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
	public class 星云护卫Checklist : ModSystem
	{
		public float Time;
		public bool Bool;
		public float Rotation;
        public Vector2[] ProjPos = new Vector2[60];
		public float[] ProjScale = new float[60];
		public int[] ProjTime = new int[60];
        //Boss位置和boss移动
        public Vector2 BossPosition;
		public Vector2 BossVelocity;

		//背景
		public bool DriftBool;
		public float Drift;
		//帧
		public int frame;

		public int Break;

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/星云Checklist");
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

			int bossType = ModContent.NPCType<星云护卫>();
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
            {
                BossPosition.X = rect.Width / 2;
                BossPosition.Y = rect.Height * 0.5f + Time;
                DDHelper.BackAndForth(-20F, 20F, 0.1F, ref Time, ref Bool);

                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<星云护卫>()].Value;
                Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;
                frame++;
                Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;

                Rectangle rectangle = new Rectangle(0, texture.Height / 6 * (frame / 6 % 6), texture.Width/2, texture.Height / 6);

                sb.Draw(texture, vector, rectangle, color, Rotation, rectangle.Size()/2, 1.15f, SpriteEffects.None, 0);
                texture = 星云护卫.Zu.Value;
                sb.Draw(texture, vector, rectangle, color, Rotation, rectangle.Size()/2, 1.15f, SpriteEffects.None, 0);
                texture = 星云护卫.Glow.Value;
                sb.Draw(texture, vector, rectangle, new Color(255, 255, 255, 0), Rotation, rectangle.Size() / 2, 1.15f, SpriteEffects.None, 0);

            }
            //绘制弹幕
            void DrawMobs(SpriteBatch sb, Rectangle rect, Color color)
            {
                int[] Po = new int[] { (int)(rect.Width * 0.13F), (int)(rect.Width * 0.9F), (int)(rect.Width * 0.43F) };
                Texture2D texture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Beam").Value;
                sb.Draw(texture, new Vector2(Po[0],rect.Height )+ rect.TopLeft(), null, Color.White*0.3F, 0, texture.Size(),new Vector2(0.3F,0.5F), 0, 0);
                sb.Draw(texture, new Vector2(Po[1], rect.Height) + rect.TopLeft(), null, Color.White * 0.5F, 0, texture.Size(), new Vector2(0.5F, 0.5F), 0, 0);
                sb.Draw(texture, new Vector2(Po[2], rect.Height) + rect.TopLeft(), null, Color.White * 0.4F, 0, texture.Size(), new Vector2(0.4F, 0.5F), 0, 0);
                for (int A = 0; A < ProjPos.Length; A++)
                {
                    if (A < 20)
                    {
                        Draw(sb, A, rect, (int)(Po[0]-35*0.3F), 0.6F);

                    }
                    else if (A < 40)
                    {

                        Draw(sb, A, rect, (int)(Po[1] - 35 * 0.5F), 1F);
                    }
                    else if (A < 60)
                    {

                        Draw(sb, A, rect, (int)(Po[2] - 35 * 0.4F), 0.8F);
                    }
                }
            }
			void Draw(SpriteBatch sb, int A, Rectangle rect, int vector,float Sc)
            {
                Texture2D texture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Rock_" + ProjTime[A]).Value;
                if (ProjPos[A] == Vector2.Zero)
                {
                    ProjScale[A] = Main.rand.NextFloat(0.8F, 1F);
                    ProjPos[A] = new Vector2(Main.rand.NextFloat(-20, 10) * Sc + vector, rect.Height + 100 - (rect.Height + 200) / 20 * (A%20));
                    ProjTime[A] = Main.rand.Next(3);
                }
                ProjPos[A].Y-= Sc;

                sb.Draw(texture, ProjPos[A] + rect.TopLeft(), null, Color.White * (ProjPos[A].Y >= 200?1: (ProjPos[A].Y/200)), 0, Vector2.Zero, ProjScale[A] * Sc, 0, 0);
                if (ProjPos[A].Y < -100)
                {
                    ProjScale[A] = Main.rand.NextFloat(0.8F, 1F);
                    ProjPos[A].X =Main.rand.NextFloat(-20,10) * Sc + vector;
                    ProjPos[A].Y += rect.Height + 100;
                    ProjTime[A] = Main.rand.Next(3);
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

            int summonItem = ModContent.ItemType<群星符星云>();

            bossChecklistMod.Call(
        "LogMiniBoss",
        Mod,
        "星云护卫",

               17.93f,
        () => NPCDowned.星云护卫,
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