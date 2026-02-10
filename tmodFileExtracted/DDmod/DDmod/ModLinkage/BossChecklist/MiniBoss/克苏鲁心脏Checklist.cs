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
using Newtonsoft.Json.Linq;
using Terraria;

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
	public class 克苏鲁心脏Checklist : ModSystem
	{
		public float Rotation;
		public Vector2 ProjPosition;
		public Vector2 ProjVelocity;
		//Boss位置和boss移动
		public Vector2 BossPosition;
		public Vector2 BossVelocity;

		//帧
		public int frame;
		public float frameCounter;

        public bool Bool;
        public bool Bool2;
        public float Time;
        public float Time2;
        public float Time3;
        public float Time4;

        public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/地下猩红Checklist");
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
            return;
            void FindFrame()
            {
                //光球
                DDHelper.BackAndForth(1, 1.3F, 0.03F, ref Time, ref Bool);
                Time2 += 0.03f;
            }

            int bossType = ModContent.NPCType<克苏鲁心脏>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
            {

                FindFrame();
                Time3 += Time4;
                if (Bool2)
                {
                    if (Time4 < 1)
                    {
                        Time4 += 0.05F;
                    }
                }
                else
                {
                    if (Time4 > -1)
                    {
                        Time4 -= 0.05F;
                    }
                }
                if (Time3 >= 20)
                {
                    Bool2 = false;
                }
                if (Time3 <= -20)
                {
                    Bool2 = true;
                }
                BossPosition.X = rect.Width / 2;
                BossPosition.Y = rect.Height * 0.5f + Time3;
                Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

                Vector2 vector = centered + new Vector2(0, Time3);

                int dust = BDust.NewDust(BossPosition - new Vector2(46) + new Vector2(Main.rand.NextFloat(92), Main.rand.NextFloat(92)), new Vector2(0, 1), 5, scale: Main.rand.NextFloat(0.6F, 1.2F), ChecklistHelper.克苏鲁心脏);
                ChecklistHelper.Dust[dust].color = new Color(150, 100, 100, 255);
                ChecklistHelper.Dust[dust].gravity = true;

                dust = BDust.NewDust(BossPosition - new Vector2(4, 72) - new Vector2(6), new Vector2(2, -4) * Main.rand.NextFloat(0.6F, 1F), 5, scale: Main.rand.NextFloat(0.5F, 1.2F), ChecklistHelper.克苏鲁心脏);
                ChecklistHelper.Dust[dust].color = new Color(150, 100, 100, 255);
                ChecklistHelper.Dust[dust].position += new Vector2(Main.rand.NextFloat(12), Main.rand.NextFloat(12));
                ChecklistHelper.Dust[dust].gravity = true;

                dust = BDust.NewDust(BossPosition - new Vector2(-30, 64) - new Vector2(6), new Vector2(4, -2) * Main.rand.NextFloat(0.6F, 1F), 5, scale: Main.rand.NextFloat(0.75F, 1.4F), ChecklistHelper.克苏鲁心脏);
                ChecklistHelper.Dust[dust].color = new Color(150, 100, 100, 255);
                ChecklistHelper.Dust[dust].position += new Vector2(Main.rand.NextFloat(12), Main.rand.NextFloat(12));
                ChecklistHelper.Dust[dust].gravity =true;


                Texture2D texture = TextureAssets.Npc[bossType].Value;
                frameCounter++;
                frame = 15;
                Rectangle rectangle = new Rectangle(0, texture.Height / 6 * ((int)(frameCounter / 5 % 6)), texture.Width / 2, texture.Height / 6);
                sb.Draw(texture, vector, rectangle, new Color(150, 100, 100, 255), 0, rectangle.Size() / 2, 1.15F, 0, 0f);
                rectangle = rectangle = new Rectangle(texture.Width / 2, texture.Height / 6 * (frame / 5 % 6), texture.Width / 2, texture.Height / 6);
                sb.Draw(texture, vector, rectangle, new Color(150, 100, 100, 255), 0, rectangle.Size() / 2, 1.15F, 0, 0f);
                for (int a = 0; a < 200; a++)
                {
                    if (ChecklistHelper.Dust[a] == null)
                    {
                        ChecklistHelper.Dust[a] = new BDust();
                    }
                    ChecklistHelper.Dust[a].UpdateDust(a);
                    ChecklistHelper.Dust[a].Dawn(sb, rect, ChecklistHelper.克苏鲁心脏);
                }


            }
            var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                    DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			int summonItem = ModContent.ItemType<神圣仙酒>();

            bossChecklistMod.Call(
                "LogMiniBoss",
                Mod,
                "克苏鲁心脏",
				7.4f,
				() => NPCDowned.克苏鲁心脏,
				bossType,
				new Dictionary<string, object>()
				{
                    //["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
                    //收藏品
                    //["collectibles"] = collection,
                    //["spawnItems"] = summonItem,
                    //绘制
                    ["customPortrait"] = customBossPortrait,
				}
			);

		}
	}
}