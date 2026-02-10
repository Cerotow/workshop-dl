using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Boss.海幽浮王;
using DDmod.Content.Items.Boss.鬼牙;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.Boss.夜光蘑菇王;
using DDmod.Content.NPCs.Boss.海幽浮王;
using DDmod.Content.NPCs.Boss.蘑菇王;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist
{
	public class 海幽浮王Checklist : ModSystem
	{
		public float Rotation;
		public Vector2 ProjPosition;
		public Vector2 ProjVelocity;
		//Boss位置和boss移动
		public Vector2 BossPosition;
		public Vector2 BossVelocity;

		//背景
		public bool DriftBool;
		public float Drift;
		//帧
		public int frame;
		public float frameCounter;

		public bool[] Bool = new bool[6];
		public float[] Time = new float[6];
		public float[] frameC = new float[6];

		public static Asset<Texture2D> BossChecklistBook;
		public static Asset<Texture2D> BossChecklistBook2;

		public override void Load()
		{
			for(int a = 0;a<6;a++)
			{
				Time[a] = Main.rand.Next(0,4);
            }
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/海洋Checklist");
			BossChecklistBook2 = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/海洋Checklist2");
		}
		public override void Unload()
		{
			BossChecklistBook = null;
			BossChecklistBook2 = null;
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

			int bossType = ModContent.NPCType<海幽浮王>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				BossPosition.X =  rect.Width / 2-30;
                BossPosition.Y = rect.Height/2+6;

                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<海幽浮王>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

                frameCounter++;
				if (frameCounter >= 6)
				{
					frameCounter = 0;
                    frame++;
                }
                for (int a = 0; a < 6; a++)
                {
                    frameC[a]++;
                    if (frameC[a] >= 10)
                    {
                        frameC[a] = 0;
                        Time[a]++;
                    }
                }
                if (frame >= 7)
				{
					frame = 0;
                }
                for (int a = 0; a < 6; a++)
                {
                    if (a==0 && Time[a] >= 6)
                    {
                        Time[a] = 0;
                    }
                    if (a != 0 && Time[a] >= 4)
                    {
                        Time[a] = 0;
                    }
                }

                Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;
				sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 7 * frame, texture.Width, texture.Height / 7)), color, Rotation, new Vector2(texture.Width, texture.Height / 7) / 2, 1.3f, SpriteEffects.FlipHorizontally, 0);
                texture = TextureAssets.Npc[ModContent.NPCType<海幽浮>()].Value;
				sb.Draw(texture, vector + new Vector2(74, 40), new Rectangle?(new Rectangle(0, (int)(texture.Height / 6 * Time[0]), texture.Width, texture.Height / 6)), color, Rotation, new Vector2(texture.Width, texture.Height/6) / 2, 1, SpriteEffects.FlipHorizontally, 0);
                texture = TextureAssets.Npc[ModContent.NPCType<小海幽浮>()].Value;
                sb.Draw(texture, vector + new Vector2(110, 44), new Rectangle?(new Rectangle(0, (int)(texture.Height / 4 * Time[1]), texture.Width, texture.Height / 4)), color, Rotation, new Vector2(texture.Width, texture.Height/4) / 2, 1, SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector + new Vector2(140, 44), new Rectangle?(new Rectangle(0, (int)(texture.Height / 4 * Time[2]), texture.Width, texture.Height / 4)), color, Rotation, new Vector2(texture.Width, texture.Height/4) / 2, 1, SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector - new Vector2(50, -44), new Rectangle?(new Rectangle(0, (int)(texture.Height / 4 * Time[3]), texture.Width, texture.Height / 4)), color, Rotation, new Vector2(texture.Width, texture.Height/4) / 2,1, SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector - new Vector2(80, -44), new Rectangle?(new Rectangle(0, (int)(texture.Height / 4 * Time[4]), texture.Width, texture.Height / 4)), color, Rotation, new Vector2(texture.Width, texture.Height/4) / 2,1, SpriteEffects.FlipHorizontally, 0);
				sb.Draw(texture, vector - new Vector2(106, -44), new Rectangle?(new Rectangle(0, (int)(texture.Height / 4 * Time[5]), texture.Width, texture.Height / 4)), color, Rotation, new Vector2(texture.Width, texture.Height/4) / 2, 1, SpriteEffects.FlipHorizontally, 0);

			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw2(BossChecklistBook,BossChecklistBook2, rect, color, BossDraw);
			};

            int summonItem = ModContent.ItemType<秘制大虾>();
            bossChecklistMod.Call(
				"LogBoss",
				Mod,
                "海幽浮王",
				4.6f,
				() => NPCDowned.海幽浮王,
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