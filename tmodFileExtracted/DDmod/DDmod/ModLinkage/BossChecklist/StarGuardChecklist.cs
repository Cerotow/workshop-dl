using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;

namespace DDmod.ModLinkage.BossChecklist
{
    public class StarGuardChecklist : ModSystem
	{
		public float Time;
		public int ProjTime;
		public float Rotation;
		public Vector2[] StarPosition = new Vector2[5];
		public Vector2[] StarVelocity = new Vector2[5];

		public bool DriftBool;
		public float Drift;
		public Vector2[] oldPos = new Vector2[5];

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/StarGuardChecklist");
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
			int bossType = ModContent.NPCType<StarGuard>();

			List<int> collection = new List<int>()
			{
				ModContent.ItemType<StarguardRelic>(),
				ModContent.ItemType<StarguardTrophyItem>(),
				ModContent.ItemType<StarAmulet>()
			};

            string despawnInfo = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.DespawnMessage").Value;
            //绘制Boss
            void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<StarGuard>()].Value;
				Texture2D texture2 = StarGuard.Glow.Value;
				Vector2 centered = new Vector2(rect.X, rect.Y + Drift) + rect.Size() / 2;
				DDHelper.BackAndForth(-10, 10, 0.25F, ref Drift, ref DriftBool);
				sb.Draw(texture2, centered, null, new Color(0, 100, 255, 0), Rotation, texture2.Size() / 2, 1, 0, 0);
				sb.Draw(texture2, centered, null, new Color(0, 100, 255, 0), Rotation, texture2.Size() / 2, 1, 0, 0);
				sb.Draw(texture2, centered, null, new Color(255, 155, 0, 0), Rotation, texture2.Size() / 2, 0.5F, 0, 0);
				sb.Draw(texture, centered, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), color, Rotation,new Vector2(texture.Width / 2, texture.Height) / 2, 1, 0, 0);

				Time += 0.03f;
				Rotation += 0.1f;
			}
			//绘制弹幕
			void DrawMobs(SpriteBatch sb, Rectangle rect, Color color)
			{
				Main.instance.LoadProjectile(ModContent.ProjectileType<BossStar>());

				ProjTime++;
				Vector2 centered = new Vector2(rect.X, rect.Y + Drift) + rect.Size() / 2;
				Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<BossStar>()].Value;
				Texture2D texture2 = BossStar.Glow.Value;
				if (ProjTime > 120)
				{
					for (int a = 0; a < 5; a++)
					{
						StarPosition[a] = Rotation.ToRotationVector2().RotatedBy(MathHelper.TwoPi / 5 * a) * 30 + rect.Size() / 2;
						StarVelocity[a] = Rotation.ToRotationVector2().RotatedBy(MathHelper.TwoPi / 5 * a) * 7;
					}
					ProjTime = 0;
				}
				for (int a = 0; a < 5; a++)
				{
					StarPosition[a] += StarVelocity[a];
					StarVelocity[a].Y += 0.2f;
					if (StarVelocity[a].Y > 10) StarVelocity[a].Y = 10;
					StarVelocity[a].X *= 0.9f;
					Vector2 vector = new Vector2(rect.X, rect.Y) + StarPosition[a];
					sb.Draw(texture2, vector, null, new Color(0, 100, 255, 0), Rotation, texture2.Size() / 2, 1, 0, 0f);
					sb.Draw(texture2, vector, null, new Color(0, 100, 255, 0), Rotation, texture2.Size() / 2, 1, 0, 0f);
					sb.Draw(texture, vector, null, color, Rotation, texture.Size() / 2, 1, 0, 0);

				}
			}
			//绘制护卫
			void DrawMobs2(SpriteBatch sb, Rectangle rect, Color color)
			{
				Vector2 centered = new Vector2(rect.X, rect.Y + Drift) + rect.Size() / 2;
				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<ServantOfTheStars>()].Value;
				Texture2D texture2 = ServantOfTheStars.Glow.Value;

				for (int a = 0; a < 5; a++)
				{
					Vector2 vector = centered + new Vector2(40 + Drift).RotatedBy(MathHelper.TwoPi / 5 * a + Time);

					sb.Draw(texture2, vector, null, new Color(0, 100, 255, 0), (vector - centered).ToRotation() + Rotation, texture2.Size() / 2, 1, 0, 0);
					sb.Draw(texture2, vector, null, new Color(0, 100, 255, 0), (vector - centered).ToRotation() + Rotation, texture2.Size() / 2, 1, 0, 0);
					sb.Draw(texture2, vector, null, new Color(255, 155, 0, 0), (vector - centered).ToRotation() + Rotation, texture2.Size() / 2, 0.5F, 0, 0);
					sb.Draw(texture, vector, null, color, (vector - centered).ToRotation() + Rotation, texture.Size() / 2, 1, 0, 0);
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

			int summonItem = ModContent.ItemType<SuspiciousStarStone>();
			//第一阶段
			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"星辰守卫1",
				0.1f,
				() => NPCDowned.downedStarGuard,
				bossType,
				new Dictionary<string, object>()
				{
					["displayName"] = Language.GetText("Mods.DDmod.NPCs.StarGuard.BossChecklistIntegration.EntryName1"),
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
				"星辰守卫2",
				1.1f,
				() => NPCDowned.downedStarGuard2,
				bossType,
				new Dictionary<string, object>()
				{
					["displayName"] = Language.GetText("Mods.DDmod.NPCs.StarGuard.BossChecklistIntegration.EntryName2"),
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
				"星辰守卫3",
				3.1f,
				() => NPCDowned.downedStarGuard3,
				bossType,
				new Dictionary<string, object>()
				{
					["displayName"] = Language.GetText("Mods.DDmod.NPCs.StarGuard.BossChecklistIntegration.EntryName3"),
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