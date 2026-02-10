using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MiniBoss.召唤物;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.EliteMonster.四柱护卫;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
	public class 日耀护卫Checklist : ModSystem
	{
		public float Time;
		public bool Bool;
		public float Rotation;
        public Vector2[] ProjPos = new Vector2[10];
		public float[] ProjScale = new float[10];
        public int[] ProjTime = new int[10];
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
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/日耀Checklist");
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

			int bossType = ModContent.NPCType<日耀护卫>();
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
            {
                BossPosition.X = rect.Width / 2;
                BossPosition.Y = rect.Height * 0.5f + Time;
                DDHelper.BackAndForth(-20F, 20F, 0.1F, ref Time, ref Bool);

                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<日耀护卫>()].Value;
                Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;
                frame++;
                Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;

                Rectangle rectangle = new Rectangle(0, texture.Height / 5 * (frame / 6 % 5), texture.Width, texture.Height / 5);

                sb.Draw(texture, vector, rectangle, color, Rotation, rectangle.Size()/2, 1.3f, SpriteEffects.None, 0);
				rectangle.Y *= 2;
				rectangle.Width *= 2;
				rectangle.Height *= 2;
                texture = 日耀护卫.Glow.Value;
                sb.Draw(texture, vector, rectangle, new Color(255, 255, 255, 0), Rotation, rectangle.Size() / 2, 1.3f / 2, SpriteEffects.None, 0);

            }
            //绘制弹幕
            void DrawMobs(SpriteBatch sb, Rectangle rect, Color color)
			{
				for (int A = 0; A < ProjPos.Length; A++)
				{
					if (ProjPos[A] == Vector2.Zero)
					{
						ProjPos[A].Y = -Main.rand.Next(10,200);
						ProjPos[A].X = rect.X + Main.rand.Next(rect.Width * 2);
						ProjScale[A] = Main.rand.NextFloat(0.3F, 1F);
					}
					ProjTime[A]++;
					ProjPos[A] += new Vector2(-5,5) * ProjScale[A];
					Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;
					Texture2D texture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Meteor").Value;
					Rectangle rectangle = new Rectangle(0, texture.Height / 4 * (ProjTime[A] / 6 % 4), texture.Width, texture.Height / 4);
					sb.Draw(texture, ProjPos[A], rectangle, Color.White, 0, Vector2.Zero, ProjScale[A], 0, 0);
					if (ProjPos[A].Y >= rect.Y + rect.Height * 2)
                    {
                        ProjPos[A].Y = -10;
                        ProjPos[A].X = rect.X + Main.rand.Next(rect.Width * 2);
                        ProjScale[A] = Main.rand.NextFloat(0.3F, 1F);
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

            int summonItem = ModContent.ItemType<群星符日耀>();

            bossChecklistMod.Call(
        "LogMiniBoss",
        Mod,
        "日耀护卫",
               17.91f,
        () => NPCDowned.日耀护卫,
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