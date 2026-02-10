using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
    public class GoblinSorcererChieftainChecklist : ModSystem
	{
		float R = 0;
		bool RB = false;

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/MiniBoss/GoblinSorcererChieftainChecklist");
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

			int bossType = ModContent.NPCType<GoblinSorcererChieftain>();

			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
				var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                    Vector2 centered = new Vector2(rect.X, rect.Y+36) + rect.Size() / 2;
                    Texture2D texture = TextureAssets.Npc[ModContent.NPCType<GoblinSorcererChieftain>()].Value;
                    sb.Draw(texture, centered + new Vector2(0, 84), new Rectangle?(new Rectangle(0, texture.Height / 7 * 6, texture.Width/2, texture.Height / 7)), Color.White, 0, new Vector2(texture.Width/2, texture.Height / 7) / 2, 1, (SpriteEffects)1, 0);


                    DDHelper.BackAndForth(0.8F, 1F, 0.005F, ref R, ref RB);
                    texture = DDTextures.VoidStar.Value;
                    for (int a = 0; a < 50; a++)
                    {
                        sb.Draw(texture, centered, null, new Color(81, 6, 233, 0), 0, texture.Size() / 2, R * ((float)a / 50), 0, 0);
                    }
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);

			};
			List<int> collection = new List<int>()
			{
			};
			
			bossChecklistMod.Call(
				"LogMiniBoss",
				Mod,
				"哥布林首领",
				3.2f,
				() => NPCDowned.downedGoblinSorcererChieftain,
				bossType,
				new Dictionary<string, object>()
				{
					//["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
					//收藏品
					["collectibles"] = collection,
					//绘制
					["customPortrait"] = customBossPortrait,
				}
			);

		}
	}
}