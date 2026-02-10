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
	public class 炼狱头颅Checklist : ModSystem
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
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/地狱Checklist");
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

			int bossType = ModContent.NPCType<炼狱头颅>();
			string despawnInfo = null;
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				BossPosition.X =  rect.Width / 2;
                BossPosition.Y =  rect.Height *0.5f+ Time;
                DDHelper.BackAndForth(-20F, 20F, 0.1F, ref Time, ref Bool);

                Texture2D texture = TextureAssets.Npc[ModContent.NPCType<炼狱头颅>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y) + rect.Size() / 2;

				frame2++;

                if (frame2 % 120 == 0)
				{
					frameCounter = 24;
					for (int r = 0; r <4; r++)
					{
						int A = BDust.NewDust(BossPosition - new Vector2(0, -32 * (1.3f)), Vector2.Zero, ModContent.DustType<光圈粒子>(), scale: 0.01F, ChecklistHelper.炼狱头颅);
						ChecklistHelper.Dust[A].color = new Color(254, 86, 4, 0);
						ChecklistHelper.Dust[A].alpha = -1;
						ChecklistHelper.Dust[A].customData = new Vector4(2f, 40, 0, 1F);
					}
                }
                if (frameCounter > 0)
                {
                    frameCounter--;

                }
                if (frameCounter > 0)
                {
                    frame = 1;
                }
                else
                {
                    frame = 2;
                }
                Vector2 vector = new Vector2(rect.X, rect.Y) + BossPosition;
				texture = 炼狱头颅.Glow.Value;
                sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 3 * frame, texture.Width, texture.Height / 3)), new Color(254, 86, 4, 0), Rotation, new Vector2(texture.Width, texture.Height / 3) / 2, 1.3f / 4, SpriteEffects.None, 0);
                texture = TextureAssets.Npc[ModContent.NPCType<炼狱头颅>()].Value;
                sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, texture.Height / 3 * frame, texture.Width, texture.Height / 3)), color, Rotation, new Vector2(texture.Width, texture.Height / 3) / 2, 1.3f, SpriteEffects.None, 0);


                for (int a = 0; a < 200; a++)
                {
                    if (ChecklistHelper.Dust[a] == null)
                    {
                        ChecklistHelper.Dust[a] = new BDust();
                    }
                    ChecklistHelper.Dust[a].UpdateDust(a);
                    ChecklistHelper.Dust[a].Dawn(sb, rect, ChecklistHelper.炼狱头颅);
                }

            }
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                    DrawBoss(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			int summonItem = ModContent.ItemType<恶魔护符>();

            bossChecklistMod.Call(
                "LogMiniBoss",
                Mod,
                "炼狱头颅",
				5.4f,
				() => NPCDowned.炼狱头颅,
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