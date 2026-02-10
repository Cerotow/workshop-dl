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

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
	public class 突变噬魂怪Checklist : ModSystem
	{
		public float Rotation;
		public Vector2 ProjPosition;
		public Vector2 ProjVelocity;
		//Boss位置和boss移动
		public Vector2 BossPosition;
		public Vector2 BossVelocity;

		//帧
		public int frame;
		public int frame2;
		public float frameCounter;

        public bool Bool;
        public float Time;

        public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/腐化Checklist");
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

			int bossType = ModContent.NPCType<突变噬魂怪>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				BossPosition.X =  rect.Width / 2;
                BossPosition.Y =  rect.Height *0.5f;

                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<突变噬魂怪>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;
				frameCounter++;
                if (frameCounter > 5)
                {
					frameCounter = 0;
                    frame++;
                }
                if (frame>2)
                {
                    frame = 0;
                }
				Rotation = 2F;
                Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;
				color *= 0.75F;
				color.A = 255;
                sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 3 * frame, texture.Width, texture.Height / 3)), color, Rotation, new Vector2(texture.Width, texture.Height / 3) / 2, 1f, SpriteEffects.None, 0);


            }
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                    DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			Func<bool> func = () => !WorldGen.crimson || Main.drunkWorld || ModLoader.TryGetMod("BothEvils", out Mod mod);

            int summonItem = ModContent.ItemType<腐臭之水>();
            bossChecklistMod.Call(
                "LogMiniBoss",
                Mod,
                "突变噬魂怪",
				2.4f,
				() => NPCDowned.突变噬魂怪,
				bossType,
				new Dictionary<string, object>()
				{
                    //["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
                    //收藏品
                    //["collectibles"] = collection,
                    ["spawnItems"] = summonItem,
                    //绘制
                    ["customPortrait"] = customBossPortrait,
                    ["availability"] = func,
                }
			);

		}
	}
}