using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.NPCs.Boss.LifeGuardLes;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist
{
    public class LifeGuardChecklist : ModSystem
	{
		public float Time;
		public int frame;
		public float frameCounter;
		public Vector2[] LifeWarriorPosition = new Vector2[5];
		public Vector2[] LifeWarriorVelocity = new Vector2[5];

		public bool DriftBool;
		public float Drift;

		public static Asset<Texture2D> BossChecklistBook;
		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/LifeGuardChecklist");
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
			//绘制Boss
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<LifeGuard>()].Value;
				Vector2 centered = new Vector2(rect.X, rect.Y + Drift) + rect.Size() / 2;
				DDHelper.BackAndForth(-10, 10, 0.25F, ref Drift, ref DriftBool);
				//绘制能量体大小
				sb.Draw(LifeGuard.Glow.Value, centered, null, new Color(255, 50, 50, 0), 0, LifeGuard.Glow.Size() / 2, 1.6f, 0, 0f);
				sb.Draw(LifeGuard.Glow.Value, centered, null, new Color(0, 105, 105, 0), 0, LifeGuard.Glow.Size() / 2, 1.15f, 0, 0f);

				Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / 5 * (frame % 5), texture.Width / 2, texture.Height / 5));
				sb.Draw(texture, centered, rectangle, color, 0, new Vector2(texture.Width / 2, texture.Height / 5) / 2, 1, 0, 0);
				frameCounter++;
				if (frameCounter % 4 == 0)
				{
					frameCounter = 0;
					frame++;
				}
				if (frame > 5) frame = 0;
				Time += 0.1f;
			}
			//绘制护卫
			void DrawMobs(SpriteBatch sb, Rectangle rect, Color color)
			{
				Vector2 centered = new Vector2(rect.X, rect.Y + Drift) + rect.Size() / 2;
				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<LifeServant>()].Value;
				for (int a = 0; a < 5; a++)
				{
					Vector2 vector = centered + new Vector2(40).RotatedBy(MathHelper.TwoPi / 5 * a + Time);
					sb.Draw(texture, vector, null, color, (vector - centered).ToRotation() + MathHelper.PiOver2, texture.Size() / 2, 1, 0, 0);
				}
			}
			//绘制护卫
			void DrawMobs2(SpriteBatch sb, Rectangle rect, Color color)
			{
				Vector2 centered = new Vector2(rect.X, rect.Y + Drift) + rect.Size() / 2;
				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<LifeWarrior>()].Value;
				for (int a = 0; a < 5; a++)
				{
					LifeWarriorPosition[a] += LifeWarriorVelocity[a];
					if (LifeWarriorPosition[a].X < 0 || LifeWarriorPosition[a].X > rect.Width)
					{
						LifeWarriorVelocity[a].X *= -1;
					}
					if (LifeWarriorPosition[a].Y < 0 || LifeWarriorPosition[a].Y > rect.Height - 50)
					{
						LifeWarriorVelocity[a].Y *= -1;
					}
					DDHelper.MaxandMinF(ref LifeWarriorPosition[a].X, rect.Width, 0);
					DDHelper.MaxandMinF(ref LifeWarriorPosition[a].Y, rect.Height - 50, 0);
					if (LifeWarriorVelocity[a] == Vector2.Zero)
					{
						LifeWarriorVelocity[a] = new Vector2(5, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
					}
					if (Main.rand.NextBool(100))
					{
						LifeWarriorVelocity[a] = LifeWarriorVelocity[a].RotatedBy(Main.rand.NextFloat(-0.8F, 0.8F));
					}
					Vector2 vector = new Vector2(rect.X, rect.Y) + LifeWarriorPosition[a];
					Rectangle? rectangle2 = new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height));
					sb.Draw(texture, vector, rectangle2, color, LifeWarriorVelocity[a].ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width, texture.Height) / 2, 1, 0, 0);
				}
			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);

                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			var customBossPortrait2 = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
				DrawMobs(sb, rect, color);

                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};
			var customBossPortrait3 = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
                DrawBoss(sb, rect, color);
				DrawMobs(sb, rect, color);
				DrawMobs2(sb, rect, color);

                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
			};


			int bossType = ModContent.NPCType<LifeGuard>();

			List<int> collection = new List<int>()
			{
				ModContent.ItemType<LifeGuardTrophyItem>(),
				ModContent.ItemType<LifeGuardRelic>(),
				ModContent.ItemType<LifeAmulet>()
			};

			string despawnInfo = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.DespawnMessage").Value;

			int summonItem = ModContent.ItemType<SuspiciousHeartStone>();
			//第一阶段
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"生命守卫1",
				0.1f,
				() => NPCDowned.downedLifeGuard,
				bossType,
				new Dictionary<string, object>()
				{
					["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName1"),
					//召唤物
					["spawnItems"] = summonItem,
					//收藏品
					["collectibles"] = collection,
					//绘制
					["customPortrait"] = customBossPortrait,
					//脱战消息
					["despawnMessage"] = despawnInfo,
				}
			);
			//第二阶段
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"生命守卫2",
				1.1f,
				() => NPCDowned.downedLifeGuard2,
				bossType,
				new Dictionary<string, object>()
				{
					["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
					//召唤物
					["spawnItems"] = summonItem,
					//收藏品
					["collectibles"] = collection,
					//绘制
					["customPortrait"] = customBossPortrait2,
					//脱战消息
					["despawnMessage"] = despawnInfo,
				}
			);
			//第三阶段
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"生命守卫3",
				3.1f,
				() => NPCDowned.downedLifeGuard3,
				bossType,
				new Dictionary<string, object>()
				{
					["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName3"),
					//召唤物
					["spawnItems"] = summonItem,
					//收藏品
					["collectibles"] = collection,
					//绘制
					["customPortrait"] = customBossPortrait3,
					//脱战消息
					["despawnMessage"] = despawnInfo,
				}
			);

		}
	}
}