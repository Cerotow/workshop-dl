using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Talisman;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.Boss.夜光蘑菇王;
using DDmod.Content.NPCs.Boss.蘑菇王;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
	public class 超级蓝史莱姆Checklist : ModSystem
	{
		public float Rotation;
		public Vector2 ProjPosition;
		public Vector2 ProjVelocity;
		//Boss位置和boss移动
		public Vector2 BossPosition;
		public Vector2 BossVelocity;

		public bool Bool;
		public float frameCounter;

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/草地Checklist");
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

			int bossType = ModContent.NPCType<超级蓝史莱姆>();
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				BossPosition.X =  rect.Width / 2;
				BossPosition.Y =  rect.Height-92;

				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<超级蓝史莱姆>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;


                DDHelper.BackAndForth(-0.1F, 0.1F, 0.02F, ref frameCounter, ref Bool);
                Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;
				Texture2D texture2 = TextureAssets.Item[ModContent.ItemType<LegendaryGelItem>()].Value;

                sb.Draw(texture2, vector - new Vector2(0, -18- frameCounter*24), null, color, Rotation, new Vector2(texture2.Width/2, texture2.Height/2), 1, SpriteEffects.None, 0);

				sb.Draw(texture, vector - new Vector2(0, -44), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color*0.75f, Rotation, new Vector2(texture.Width/2, texture.Height), new Vector2(1.3F + frameCounter, 1.3F - frameCounter), SpriteEffects.None, 0);

			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};

			bossChecklistMod.Call(
                "LogMiniBoss",
                Mod,
				"超级蓝史莱姆",
				0.01f,
				() => NPCDowned.超级蓝史莱姆,
				bossType,
				new Dictionary<string, object>()
				{
					//["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
					//收藏品
					//["collectibles"] = collection,
					//绘制
					["customPortrait"] = customBossPortrait,
				}
			);

		}
	}
}