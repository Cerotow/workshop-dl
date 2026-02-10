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

namespace DDmod.ModLinkage.BossChecklist.MiniBoss
{
	public class 妖精王Checklist : ModSystem
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
        float W = 0;
        float W2 = 0;
        float W3 = 0;

        float W4 = 0;
        float W5 = 0;
        float W6 = 0;
        float W7 = 0;
        float W8 = 0;
        float W9 = 0;
        float W10 = 0;

        bool WB;
        bool WB2;
        bool WB3;

        public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/神圣地Checklist");
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
            void FindFrame()
            {
                //光球
                DDHelper.BackAndForth(1, 1.3F, 0.03F, ref Time, ref Bool);
                //翅膀
                //DDHelper.BackAndForth(-0.8F, 1.8F, 0.0125F, ref NPC.Dnpc().Times[3], ref NPC.Dnpc().Bool[3]);
                DDHelper.BackAndForth(-0.8F, 0.2F, 0.2F, ref W, ref WB);
                if (WB2)
                {
                    DDHelper.BackAndForth(-0.4F, 1.7F, 0.3F, ref W2, ref WB2);
                }
                else
                {
                    DDHelper.BackAndForth(-0.4F, 1.7F, 0.3F, ref W2, ref WB2);
                }
                DDHelper.BackAndForth(0.8F, 2.2F, 0.2F, ref W3, ref WB3);
                W10 = W9;
                W9 = W8;
                W8 = W7;
                W7 = W6;
                W6 = W5;
                W5 = W4;
                W4 = W2;
                Time2 += 0.03f;
            }

            int bossType = ModContent.NPCType<妖精王>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{

                FindFrame();
                BossPosition.X =  rect.Width / 2;
                BossPosition.Y =  rect.Height *0.5f+ Time;
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
                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<妖精王>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

                Vector2 vector = centered+new Vector2(0,40+ Time3);

                float CBSC = 1;
                texture = 妖精王.Wing.Value;

                sb.Draw(texture, vector + 1.15F * new Vector2(8, -8), null, color, W, new Vector2(4, 20), 1.15F * CBSC, 0, 0f);

                texture = 妖精王.WingX.Value;
                sb.Draw(texture, vector + 1.15F * new Vector2(14, 8), null, color, W3, new Vector2(4, 16), 1.15F * CBSC, 0, 0f);
                
                texture = 妖精王.WingD.Value;
                sb.Draw(texture, vector + 1.15F * new Vector2(10, 0), null, color, W2, new Vector2(6, 28), 1.15F * CBSC, 0, 0f);
                sb.Draw(texture, vector + 1.15F * new Vector2(10, 0), null, color * 0.4f, W4, new Vector2(6, 28), 1.15F * CBSC, 0, 0f);
                sb.Draw(texture, vector + 1.15F * new Vector2(10, 0), null, color * 0.3f, W5, new Vector2(6, 28), 1.15F * CBSC, 0, 0f);
                sb.Draw(texture, vector + 1.15F * new Vector2(10, 0), null, color * 0.2f, W6, new Vector2(6, 28), 1.15F * CBSC, 0, 0f);
                sb.Draw(texture, vector + 1.15F * new Vector2(10, 0), null, color * 0.1f, W7, new Vector2(6, 28), 1.15F * CBSC, 0, 0f);

                texture = 妖精王.Wing.Value;

                sb.Draw(texture, vector + 1.15F * new Vector2(-8, -8), null, color, -W, new Vector2(32, 20), 1.15F * CBSC, (SpriteEffects)1, 0f);
                texture = 妖精王.WingX.Value;
                sb.Draw(texture, vector + 1.15F * new Vector2(-14, 8), null, color, -W3, new Vector2(24, 16), 1.15F * CBSC, (SpriteEffects)1, 0f);

                texture = 妖精王.WingD.Value;
                sb.Draw(texture, vector + 1.15F * new Vector2(-10, 0), null, color, -W2, new Vector2(42, 28), 1.15F * CBSC, (SpriteEffects)1, 0f);
                sb.Draw(texture, vector + 1.15F * new Vector2(-10, 0), null, color * 0.4f, -W4, new Vector2(42, 28), 1.15F * CBSC, (SpriteEffects)1, 0f);
                sb.Draw(texture, vector + 1.15F * new Vector2(-10, 0), null, color * 0.3f, -W5, new Vector2(42, 28), 1.15F * CBSC, (SpriteEffects)1, 0f);
                sb.Draw(texture, vector + 1.15F * new Vector2(-10, 0), null, color * 0.2f, -W6, new Vector2(42, 28), 1.15F * CBSC, (SpriteEffects)1, 0f);
                sb.Draw(texture, vector + 1.15F * new Vector2(-10, 0), null, color * 0.1f, -W7, new Vector2(42, 28), 1.15F * CBSC, (SpriteEffects)1, 0f);


                texture = 妖精王.Glow.Value;
                sb.Draw(texture, vector, null, new Color(255, 191, 0, 100), 0, texture.Size() / 2, 1.15F / 4 * Time, 0, 0f);


                texture = DDTextures.Round2.Value;
                float Sc = Time2;
                sb.Draw(texture, vector, null, new Color(255, 191, 0, 100) * (1 - (Sc % 1)), 0, texture.Size() / 2, 1.15F * (Sc % 1), 0, 0f);
                Sc += 0.33f;
                sb.Draw(texture, vector, null, new Color(255, 191, 0, 100) * (1 - (Sc % 1)), 0, texture.Size() / 2, 1.15F * (Sc % 1), 0, 0f);
                Sc += 0.33F;
                sb.Draw(texture, vector, null, new Color(255, 191, 0, 100) * (1 - (Sc % 1)), 0, texture.Size() / 2, 1.15F * (Sc % 1), 0, 0f);

                texture = TextureAssets.Npc[ModContent.NPCType<妖精王>()].Value;
                sb.Draw(texture, vector, null, Color.White, 0, texture.Size() / 2, 1.15F, 0, 0f);

                texture = DDTextures.Starlight3.Value;
                sb.Draw(texture, vector, null, new Color(255, 191, 0, 0), 0, texture.Size() / 2, 1.15F * new Vector2(0.5F, 1) * 0.5f, 0, 0f);
                sb.Draw(texture, vector, null, new Color(0, 0, 255, 0) * 0.5F, 0, texture.Size() / 2, 1.15F * new Vector2(0.5F, 1) * 0.5f, 0, 0f);
                sb.Draw(texture, vector, null, new Color(255, 191, 0, 0), MathHelper.PiOver2, texture.Size() / 2, 1.15F * new Vector2(0.5F, 1) * 0.5f, 0, 0f);
                sb.Draw(texture, vector, null, new Color(0, 0, 255, 0) * 0.5F, MathHelper.PiOver2, texture.Size() / 2, 1.15F * new Vector2(0.5F, 1) * 0.5f, 0, 0f);



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
                "妖精王",
				7.1f,
				() => NPCDowned.妖精王,
				bossType,
				new Dictionary<string, object>()
				{
                    //["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
                    //收藏品
                    //["collectibles"] = collection,
                    ["spawnItems"] = summonItem,
                    //绘制
                    ["customPortrait"] = customBossPortrait,
				}
			);

		}
	}
}