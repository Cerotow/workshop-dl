using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Boss.天地守卫;
using DDmod.Content.Items.农场.食物.料理;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.Boss.夜光蘑菇王;
using DDmod.Content.NPCs.Boss.天地守卫;
using DDmod.Content.NPCs.Boss.蘑菇王;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist
{
	public class 星心守卫Checklist : ModSystem
	{
		public float Rotation;
		//Boss位置和boss移动
		public Vector2[] BossPosition = new Vector2[2];
		public Vector2[] BossVelocity = new Vector2[2];

		//背景
		public bool DriftBool;
		public float Drift;
		//帧
		public int[] frame =  new int[2];
		public float[] frameCounter= new float[2];

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/星心守卫Checklist");
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

			List<int> bossType = new List<int>() { ModContent.NPCType<苍穹守卫>(), ModContent.NPCType<大地守卫>() };
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<苍穹守卫>()].Value;
				Texture2D texture2 = 苍穹守卫.Glow.Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;
				for (int a = 0; a < 2; a++)
                {
                    BossPosition[a].X = rect.Width / 2+60;
                    BossPosition[a].Y = rect.Height / 2;
					Color Gcolor = new Color(0, 100, 255, 0);
                    if (a==1)
					{
                        texture = TextureAssets.Npc[ModContent.NPCType<大地守卫>()].Value;
                        texture2 = 大地守卫.Glow.Value;
                        BossPosition[a].X = rect.Width / 2-60;
                        Gcolor = new Color(255, 100, 100, 0);
                    }


					frameCounter[a]++;
					if (frameCounter[a] >= 6)
					{
						frameCounter[a] = 0;
						frame[a]++;
					}
					if (frame[a] >= 8)
					{
						frame[a] = 0;
					}

					Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition[a];
					sb.Draw(texture2, vector, null, Gcolor, Rotation, new Vector2(texture2.Width, texture2.Height) / 2, 0.25F, SpriteEffects.FlipHorizontally, 0);
					sb.Draw(texture2, vector, null, Gcolor.Opposite()*0.25F, Rotation, new Vector2(texture2.Width, texture2.Height) / 2, 0.25F, SpriteEffects.FlipHorizontally, 0);
					sb.Draw(texture2, vector, null, Gcolor.Opposite()*0.5F, Rotation, new Vector2(texture2.Width, texture2.Height) / 2, 0.2F, SpriteEffects.FlipHorizontally, 0);
					sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 8 * frame[a], texture.Width, texture.Height / 8)), color, Rotation, new Vector2(texture.Width, texture.Height /8) / 2, 1f, SpriteEffects.FlipHorizontally, 0);

				}
			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			int summonItem = ModContent.ItemType<星心石>();

            bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"天地守卫",
				11.2f,
				() => NPCDowned.觉醒星心双子,
				bossType,
				new Dictionary<string, object>()
                {
                    //召唤物
                    ["spawnItems"] = summonItem,
                    ["displayName"] = Language.GetText("Mods.DDmod.NPCs.天地守卫.EntryName"),
                    //收藏品
                    //["collectibles"] = collection,
                    //绘制
                    ["customPortrait"] = customBossPortrait,
				}
			);

		}
	}
}