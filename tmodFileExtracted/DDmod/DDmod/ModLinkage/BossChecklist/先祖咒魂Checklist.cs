using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Boss.先祖咒魂;
using DDmod.Content.Items.Boss.天地守卫;
using DDmod.Content.Items.农场.食物.料理;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.Boss.先祖咒魂;
using DDmod.Content.NPCs.Boss.夜光蘑菇王;
using DDmod.Content.NPCs.Boss.天地守卫;
using DDmod.Content.NPCs.Boss.蘑菇王;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist
{
	public class 先祖咒魂Checklist : ModSystem
	{
		public float Rotation;
		//Boss位置和boss移动
		public Vector2 BossPosition;
		public Vector2 BossVelocity;

		//背景
		public bool DriftBool;
		public float Drift;
		//帧
		public int frame;
		public float frameCounter;

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/暗影火Checklist");
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

			int bossType =  ModContent.NPCType<先祖咒魂>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				Texture2D texture = TextureAssets.Npc[bossType].Value;
				Texture2D texture2 = 先祖咒魂.Glow.Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;
				BossPosition.X = rect.Width / 2;
				BossPosition.Y = rect.Height / 2;
				Color Gcolor = Color.White;

                frame  = (int)(frameCounter++/4)%8;
				Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;
				Rectangle rectangle = new Rectangle(0, texture2.Height / 8 * frame, texture2.Width, texture2.Height / 8);

                sb.Draw(texture2, vector, rectangle, Gcolor, Rotation, rectangle.Size() / 2, 1F, 0, 0);
                rectangle = new Rectangle(0, texture.Height / 8 * frame, texture.Width, texture.Height / 8);
                sb.Draw(texture, vector, rectangle, color, Rotation, rectangle .Size()/ 2, 1f, 0, 0);

			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			int summonItem = ModContent.ItemType<被诅咒的替死玩偶>();

            bossChecklistMod.Call(
				"LogBoss",
				Mod,
                "先祖咒魂",
				11.1f,
				() => NPCDowned.先祖咒魂,
				bossType,
				new Dictionary<string, object>()
                {
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