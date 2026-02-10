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
	public class 远古幽魂Checklist : ModSystem
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
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/地牢Checklist");
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

			int bossType = ModContent.NPCType<远古幽魂>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				BossPosition.X =  rect.Width / 2;
                BossPosition.Y =  rect.Height *0.5f+ Time+20;
                DDHelper.BackAndForth(-20F, 20F, 0.1F, ref Time, ref Bool);

                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<远古幽魂>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

                frameCounter++;
                if (frameCounter > 8)
                {
					frameCounter = 0;
                    frame++;
                }
                if (frame >=7)
                {
					frame = 0;
                }
                Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;
				Color color2 = color;
				color2 *= 0.6F;
				color2.B += 100;
				color2.A = color.A;


                sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 19 * frame, texture.Width, texture.Height / 19)), color2*0.8F, Rotation, new Vector2(texture.Width, texture.Height / 19) / 2, 1.3f, SpriteEffects.None, 0);

                texture = 远古幽魂.Glow.Value;
                sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 19 * frame, texture.Width, texture.Height / 19)), color * 0.8F, Rotation, new Vector2(texture.Width, texture.Height / 19) / 2, 1.3f, SpriteEffects.None, 0);
                


            }
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                    DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};

            int summonItem = ModContent.ItemType<幽魂药水>();

            bossChecklistMod.Call(
                "LogMiniBoss",
                Mod,
                "远古幽魂",
				5.1f,
				() => NPCDowned.远古幽魂,
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