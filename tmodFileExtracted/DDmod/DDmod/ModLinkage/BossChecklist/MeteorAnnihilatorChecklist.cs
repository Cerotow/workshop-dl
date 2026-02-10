using DDmod.Content.Items.Boss.MeteorAnnihilatorItems;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;
using static DDmod.Players.DDPlayer;

namespace DDmod.ModLinkage.BossChecklist
{
	public class MeteorAnnihilatorPlayer : ModPlayer
	{
		int A;
		public bool 正在打游戏 = false;
		public int 正在打游戏2;
		/// <summary>分数</summary>
		public int fraction;
		public int Maxfraction;
		public override void PostUpdateMiscEffects()
		{
			if (Main.netMode != 2&& 正在打游戏2>0)
			{
				操作();
				正在打游戏2--;
			}
		}
		public override void PreUpdate()
		{
			if (Main.netMode != 2&& 正在打游戏2>0)
			{
				if (MeteorAnnihilatorChecklist.Start)
				{
					碰撞();
					发射();
					更新弹幕();
					更新怪物();
					生成();
					更新尸块(MeteorAnnihilatorChecklist.rectangle);
					更新粒子();
				}
			}
		}
		public void 操作()
		{
			Texture2D texture = TextureAssets.Npc[ModContent.NPCType<MeteorAnnihilator>()].Value;
			//操作游戏
			Mini_game game = Player.Dplayer().Mini_game_shortcuts;

            game.Enable = true;
			if (game.Left)
			{
				if (MeteorAnnihilatorChecklist.BossVelocity.X > -5)
				{
					MeteorAnnihilatorChecklist.BossVelocity.X -= 1F;
				}
			}
			if (game.Right)
			{
				if (MeteorAnnihilatorChecklist.BossVelocity.X < 5)
				{
					MeteorAnnihilatorChecklist.BossVelocity.X += 1F;
				}
			}
			if (game.Up)
			{
				if (MeteorAnnihilatorChecklist.BossVelocity.Y > -5)
				{
					MeteorAnnihilatorChecklist.BossVelocity.Y -= 1F;
				}
			}
			if (game.Down)
			{
				if (MeteorAnnihilatorChecklist.BossVelocity.Y < 5)
				{
					MeteorAnnihilatorChecklist.BossVelocity.Y += 1F;
				}
			}
			MeteorAnnihilatorChecklist.BossPosition += MeteorAnnihilatorChecklist.BossVelocity;
			if (MeteorAnnihilatorChecklist.BossPosition.Y < texture.Height / 8)
			{
				MeteorAnnihilatorChecklist.BossPosition.Y = texture.Height / 8;
				MeteorAnnihilatorChecklist.BossVelocity.Y = 0;
			}
			if (MeteorAnnihilatorChecklist.BossPosition.Y > MeteorAnnihilatorChecklist.rectangle.Height - texture.Height / 8+20)
			{
				MeteorAnnihilatorChecklist.BossPosition.Y = MeteorAnnihilatorChecklist.rectangle.Height - texture.Height / 8+20;
				MeteorAnnihilatorChecklist.BossVelocity.Y = 0;

			}
			if (MeteorAnnihilatorChecklist.BossPosition.X < texture.Width / 2-60)
			{
				MeteorAnnihilatorChecklist.BossPosition.X = texture.Width / 2-60;
				MeteorAnnihilatorChecklist.BossVelocity.X = 0;
			}
			if (MeteorAnnihilatorChecklist.BossPosition.X > MeteorAnnihilatorChecklist.rectangle.Width - texture.Width / 2+60)
			{
				MeteorAnnihilatorChecklist.BossPosition.X = MeteorAnnihilatorChecklist.rectangle.Width - texture.Width / 2+60;
				MeteorAnnihilatorChecklist.BossVelocity.X = 0;
			}
			MeteorAnnihilatorChecklist.BossVelocity *= 0.8f;
		}
		public void 碰撞()
		{
			if (MeteorAnnihilatorChecklist.immunity > 0)
			{
				MeteorAnnihilatorChecklist.immunity--;
			}
			Vector2 飞机位置 = MeteorAnnihilatorChecklist.BossPosition;
			if (MeteorAnnihilatorChecklist.immunity <= 0)
			{
				for (int k = 0; k < 200; k++)
				{
					if (ChecklistHelper.NPC[k].active && new Rectangle((int)(飞机位置.X - 5), (int)(飞机位置.Y - 5), 10, 10).Intersects(ChecklistHelper.NPC[k].Rectangle) && ChecklistHelper.NPC[k].Hostile)
					{
						MeteorAnnihilatorChecklist.Life--;
						MeteorAnnihilatorChecklist.immunity = 100;
					}
				}
				for (int k = 0; k < 1000; k++)
				{
					if (ChecklistHelper.Proj[k].active && new Rectangle((int)(飞机位置.X - 5), (int)(飞机位置.Y - 5), 10, 10).Intersects(ChecklistHelper.Proj[k].Rectangle)&& ChecklistHelper.Proj[k].Hostile)
					{
						MeteorAnnihilatorChecklist.Life--;
						MeteorAnnihilatorChecklist.immunity = 100;
					}
				}
			}
			if (MeteorAnnihilatorChecklist.Life <= 0 && !MeteorAnnihilatorChecklist.Death)
			{
				int GoreType = Mod.Find<ModGore>("MeteorAnnihilator1").Type;
				int GoreType2 = Mod.Find<ModGore>("MeteorAnnihilator2").Type;
				int GoreType3 = Mod.Find<ModGore>("MeteorAnnihilator3").Type;
				int GoreType4 = Mod.Find<ModGore>("MeteorAnnihilator4").Type;
				BGore.NewGore(飞机位置, new Vector2(Main.rand.NextFloat(3, 8), -5), GoreType3, ChecklistHelper.流星歼灭者);
				BGore.NewGore(飞机位置, new Vector2(Main.rand.NextFloat(-8, -3), -5), GoreType3, ChecklistHelper.流星歼灭者);
				BGore.NewGore(飞机位置, new Vector2(Main.rand.NextFloat(3, 8), -5), GoreType4, ChecklistHelper.流星歼灭者);
				BGore.NewGore(飞机位置, new Vector2(Main.rand.NextFloat(-8, -3), -5), GoreType4, ChecklistHelper.流星歼灭者);

				BGore.NewGore(飞机位置, new Vector2(Main.rand.NextFloat(-3, 4), -5), GoreType, ChecklistHelper.流星歼灭者);
				BGore.NewGore(飞机位置, new Vector2(Main.rand.NextFloat(-3, 4), -5), GoreType2, ChecklistHelper.流星歼灭者);
				for (int a = 0; a < 225; a++)
				{
					BDust.NewDust(飞机位置, new Vector2(Main.rand.NextFloat(0, 4)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 6, 2.3F, ChecklistHelper.流星歼灭者);
				}
				MeteorAnnihilatorChecklist.immunity = 200;
				MeteorAnnihilatorChecklist.Death = true;
			}
			if (MeteorAnnihilatorChecklist.immunity <= 0 && MeteorAnnihilatorChecklist.Death)
			{
				刷新();
				MeteorAnnihilatorChecklist.Start = false;
			}
		}
		public void 刷新()
		{
			for (int k = 0; k < 200; k++)
			{
				ChecklistHelper.NPC[k] = new BNPC();
			}
			for (int k = 0; k < 1000; k++)
			{
				ChecklistHelper.Proj[k] = new ModLinkage.BossChecklist.BProj();
                ChecklistHelper.Gore[k] = new BGore();
			}
			for (int k = 0; k < 3000; k++)
			{
                ChecklistHelper.Dust[k] = new BDust();
			}
			MeteorAnnihilatorChecklist.BossPosition = new Vector2(180, 400);
			MeteorAnnihilatorChecklist.Life = 3;
			MeteorAnnihilatorChecklist.Death = false;
			Level = 1;
			fraction = 0;
		}
		public int 等级()
		{
			int A = 0;
			int 经验 = 100;
			int 分 = fraction;
			for (int a = 0; a < 100; a++)
			{
				if(分 - 经验>=0)
                {
					A++;
				}
				else
				{
					break;
                }
				int J = (int)(经验 * 1.1F);
				if(J>10000)
                {
					J = 10000;
                }
				经验 += J;
			}
			return A;
        }
		public void 发射()
		{
			if (MeteorAnnihilatorChecklist.Death)
			{
				return;
			}
			if (等级() < 3)
			{
				A++;
				if (A % (20- 等级()*3) == 0)
				{
					Vector2 飞机位置 = MeteorAnnihilatorChecklist.BossPosition;
					BProj.NewProj(飞机位置 + new Vector2(-4, -64), new Vector2(0, -15), 1, 50, ChecklistHelper.流星歼灭者);
					PlaySound(SoundID.Item12);
				}
			}
            else if (等级() < 5)
			{
				A++;
				if (A % (20 - (等级()-2) * 2) == 0)
				{
					Vector2 飞机位置 = MeteorAnnihilatorChecklist.BossPosition;
					BProj.NewProj(飞机位置 + new Vector2(6, -64), new Vector2(0, -15), 1, 25, ChecklistHelper.流星歼灭者);
					BProj.NewProj(飞机位置 + new Vector2(-14, -64), new Vector2(0, -15), 1, 25, ChecklistHelper.流星歼灭者);
					PlaySound(SoundID.Item12);
				}
			}
            else if (等级() < 10)
			{
				A++;
				if (A % (30 - (等级()-5) * 2) == 0)
				{
					Vector2 飞机位置 = MeteorAnnihilatorChecklist.BossPosition;
					BProj.NewProj(飞机位置 + new Vector2(6, -64), new Vector2(0, -15), 1, 25, ChecklistHelper.流星歼灭者);
					BProj.NewProj(飞机位置 + new Vector2(-4, -64), new Vector2(0, -15), 1, 25, ChecklistHelper.流星歼灭者);
					BProj.NewProj(飞机位置 + new Vector2(-14, -64), new Vector2(0, -15), 1, 25, ChecklistHelper.流星歼灭者);
					PlaySound(SoundID.Item12);
				}
			}
		}
		public void 更新弹幕()
		{
			for (int k = 0; k < 1000; k++)
			{
				ChecklistHelper.Proj[k].UpdateProj(k);
				if (ChecklistHelper.Proj[k].active && ChecklistHelper.Proj[k].position.Y < -300)
				{
					ChecklistHelper.Proj[k].active = false;
				}
			}
		}
		public void 更新怪物()
		{
			for (int k = 0; k < 200; k++)
			{
				if (ChecklistHelper.NPC[k].active && ChecklistHelper.NPC[k].position.Y > (MeteorAnnihilatorChecklist.rectangle.Height - ChecklistHelper.NPC[k].height+40))
				{
					ChecklistHelper.NPC[k].active = false;
				}
				ChecklistHelper.NPC[k].UpdateNPC(k);
			}
		}
		public void 更新尸块(Rectangle rect)
		{
			for (int k = 0; k < 1000; k++)
			{
				if (ChecklistHelper.Gore[k].active)
				{
					if (ChecklistHelper.Gore[k].position.Y > (rect.Height * 2))
					{
						ChecklistHelper.Gore[k].active = false;
					}
					else if (ChecklistHelper.Gore[k].position.Y < (-rect.Height * 2))
					{
						ChecklistHelper.Gore[k].active = false;
					}
					else if (ChecklistHelper.Gore[k].position.X > (rect.Width * 2))
					{
						ChecklistHelper.Gore[k].active = false;
					}
					else if (ChecklistHelper.Gore[k].position.X < (-rect.Width * 2))
					{
						ChecklistHelper.Gore[k].active = false;
					}
					ChecklistHelper.Gore[k].UpdateGore(k);
				}
			}
		}
		public void 更新粒子()
		{
			for (int k = 0; k < 3000; k++)
			{
                ChecklistHelper.Dust[k].UpdateDust(k);
			}
		}
		public int Level = 1;
		public int LevelTime;
		
		public void 生成()
		{
			LevelTime++;

			if (Level == 1)
            {
				if (LevelTime%60==0)
				{
					BNPC.NewNPC(new Vector2(Main.rand.NextFloat(40, MeteorAnnihilatorChecklist.rectangle.Width - 100), 0), new Vector2(0, Main.rand.NextFloat(1, 2)), new NPC_3(), ChecklistHelper.流星歼灭者);
					BNPC.NewNPC(new Vector2(Main.rand.NextFloat(40, MeteorAnnihilatorChecklist.rectangle.Width - 100), 0), new Vector2(0, Main.rand.NextFloat(1, 2)), new NPC_1(), ChecklistHelper.流星歼灭者);
				}
			}
			if (Main.rand.NextBool(120))
			{
				float A = Main.rand.NextFloat(1f, 1f);
				int GoreType = Mod.Find<ModGore>("Cloud_" + Main.rand.Next(22)).Type;

				int Gore = BGore.NewGore(new Vector2(Main.rand.NextFloat(120, MeteorAnnihilatorChecklist.rectangle.Width+80), -100), new Vector2(0, 3 * A), GoreType, A, Main.rand.Next(2), ChecklistHelper.流星歼灭者);
                ChecklistHelper.Gore[Gore].gravity = false;
                ChecklistHelper.Gore[Gore].Norotating = true;
			}
			if (Main.rand.NextBool(840))
			{
				float A = Main.rand.NextFloat(1f, 1f);
				int GoreType = Mod.Find<ModGore>("Cloud_" + Main.rand.Next(22,41)).Type;
				int Gore = BGore.NewGore(new Vector2(Main.rand.NextFloat(120, MeteorAnnihilatorChecklist.rectangle.Width+80), -100), new Vector2(0, 3 * A), GoreType, A, Main.rand.Next(2), ChecklistHelper.流星歼灭者);
                ChecklistHelper.Gore[Gore].gravity = false;
                ChecklistHelper.Gore[Gore].Norotating = true;
			}
		}
	}
	public class MeteorAnnihilatorChecklist : ModSystem
	{
		/// <summary>弹幕位置</summary>
		public static int immunity;
		//飞机爆了
		public static bool Death;
		//游戏开关
		public static bool Start;
		//血量
		public static int Life = 3;
		/// <summary>Boss位置</summary>
		public static Vector2 BossPosition;
		/// <summary> boss移动 </summary>
		public static Vector2 BossVelocity;

		/// <summary>帧</summary>
		public static int frame;
		public static float frameCounter;
		public static Rectangle rectangle;


		public Vector2[] oldPos = new Vector2[18];

		public static Asset<Texture2D> BossChecklistBook;

		public override void Load()
		{
			BossChecklistBook = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/天空Checklist");
		}
		public override void Unload()
		{
			BossChecklistBook = null;
		}

        NPC npc = new NPC();
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

			int bossType = ModContent.NPCType<MeteorAnnihilator>();
			//绘制Boss和小怪
			void DrawBoss(SpriteBatch sb, Rectangle rect, Color color)
			{
				for (int k = 0; k < 1000; k++)
				{
					if (ChecklistHelper.Gore[k].post==0)
					{
                        ChecklistHelper.Gore[k].Dawn(sb,rect, ChecklistHelper.流星歼灭者);
					}
				}
				rectangle = rect;
				for (int k = 0; k < 200; k++)
				{
                    ChecklistHelper.NPC[k].Dawn(sb, rect, ChecklistHelper.流星歼灭者);
				}
				Texture2D texture = TextureAssets.Npc[ModContent.NPCType<MeteorAnnihilator>()].Value;
				Texture2D 尾气 = MeteorAnnihilator.尾气.Value;
				Vector2 Position = new Vector2(rect.X, rect.Y);

				frameCounter++;
				if (frameCounter > 2)
				{
					frameCounter = 0;
					frame++;
				}
				if (frame >= 5)
				{
					frame = 0;
				}
				Vector2 vector = Position + BossPosition;
				if (immunity % 10 <= 5 && !Death)
				{
					/*
					sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 4)), Color.White, 0, new Vector2(texture.Width, texture.Height / 4) / 2, 1, 0, 0);
					sb.Draw(尾气, vector, new Rectangle?(new Rectangle(尾气.Width / 5 * frame, 0, 尾气.Width / 5, 尾气.Height / 4)), Color.White, 0, new Vector2(尾气.Width / 5, 尾气.Height / 4) / 2 - new Vector2(0, 44), 1, 0, 0);
					*/
					if (npc.type != ModContent.NPCType<MeteorAnnihilator>())
					{
						npc.SetDefaults(ModContent.NPCType<MeteorAnnihilator>());
						npc.scale = 0.8F;
                        npc.width = (int)(92 * npc.scale);
                        npc.height = (int)(92* npc.scale);
                    }
                    npc.Center = vector-new Vector2(0,8 * npc.scale);
                    npc.velocity = BossVelocity;

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
                    NPCLoader.PreDraw(npc, sb, Vector2.Zero, Color.White);
					NPCLoader.PostDraw(npc, sb, Vector2.Zero, Color.White);

                    Texture2D VoidStar = DDTextures.GlowEffect.Value;
					Texture2D Starlight = DDTextures.Starlight.Value;
					Main.EntitySpriteDraw(Starlight, vector, null, new Color(20, 205, 20, 0) * 0.5F, 0, Starlight.Size() / 2, 0.3F, 0, 0);
					Main.EntitySpriteDraw(Starlight, vector, null, new Color(235, 50, 235, 0) * 0.5F, 0, Starlight.Size() / 2, 0.15F, 0, 0);
					Main.EntitySpriteDraw(VoidStar, vector, null, new Color(20, 205, 20, 0) * 0.4F, 0, VoidStar.Size() / 2, 0.15F, 0, 0);
					Main.EntitySpriteDraw(Starlight, vector, null, new Color(20, 205, 20, 0) * 0.5F, 0, Starlight.Size() / 2, 0.3F, 0, 0);
					Main.EntitySpriteDraw(Starlight, vector, null, new Color(235, 50, 235, 0) * 0.5F, 0, Starlight.Size() / 2, 0.15F, 0, 0);
					Main.EntitySpriteDraw(VoidStar, vector, null, new Color(20, 205, 20, 0) * 0.4F, 0, VoidStar.Size() / 2, 0.15F, 0, 0);
				}
				texture = TextureAssets.Heart.Value;
				vector = Position;
				vector.Y += rect.Height - 20;
				for (int a = 0; a < Life; a++)
				{
					vector.X += 20;
					sb.Draw(texture, vector, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, 0, new Vector2(texture.Width, texture.Height) / 2, 1, 0, 0);

				}
			}
			//绘制弹幕
			void DrawMobs(SpriteBatch sb, Rectangle rect, Color color)
			{
				for (int k = 0; k < 1000; k++)
				{
                    ChecklistHelper.Proj[k].Dawn(sb, rect, ChecklistHelper.流星歼灭者);
					if (ChecklistHelper.Gore[k].post==1)
					{
                        ChecklistHelper.Gore[k].Dawn(sb, rect, ChecklistHelper.流星歼灭者);
					}
				}
				for (int k = 0; k < 3000; k++)
				{
                    ChecklistHelper.Dust[k].Dawn(sb, rect, ChecklistHelper.流星歼灭者);

				}
				float S = 1;
				Vector2 vector = ChatManager.GetStringSize(FontAssets.DeathText.Value, "开始游戏",Vector2.One, 0);
				if (new Rectangle(rect.X, rect.Y + rect.Height / 10, (int)vector.X, (int)vector.Y).Intersects(new Rectangle(Main.mouseX, Main.mouseY, 1, 1)))
				{
					S = 1.2F;
					if (Main.mouseLeft)
					{
						if (!Start)
						{
							Start = true;
							for (int k = 0; k < 200; k++)
							{
								ChecklistHelper.NPC[k] = new BNPC();
							}
							for (int k = 0; k < 1000; k++)
							{
								ChecklistHelper.Proj[k] = new ModLinkage.BossChecklist.BProj();
                                ChecklistHelper.Gore[k] = new BGore();
							}
							for (int k = 0; k < 3000; k++)
							{
                                ChecklistHelper.Dust[k] = new BDust();
							}
						}
						/*
								else if (immunity > 0 && Death)
								{
									Utils.OpenToURL("https://space.bilibili.com/417426564?spm_id_from=333.1007.0.0");
									immunity = 1000;
									Death = false;
									Life = 5;
								}*/
					}
				}
				if (Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().fraction > Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().Maxfraction)
				{
					Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().Maxfraction = Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().fraction;
				}
				if (Death)
				{
					Vector2 origin = ChatManager.GetStringSize(FontAssets.DeathText.Value, "你的得分是:" + Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().fraction, Vector2.One, 0);
					sb.DrawString(
					FontAssets.DeathText.Value,
					"你的得分是:" + Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().fraction,
					new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2),
					Color.White, 0f,
					origin / 2,
					1, SpriteEffects.None, 0f);
				}
				else if (Start)
				{
					int 经验 = 100;
					for (int a = 0; a < 100; a++)
					{
						if (Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().fraction - 经验 < 0)
						{
							break;
						}
						int J = (int)(经验 * 1.1F);
						if (J > 10000)
						{
							J = 10000;
						}
						经验 += J;
					}
					string Text = "当前分数:" + Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().fraction;
					Vector2 origin = ChatManager.GetStringSize(FontAssets.MouseText.Value, Text, Vector2.One, 0);
					sb.DrawString(
					FontAssets.MouseText.Value,
					Text,
					new Vector2(rect.X + rect.Width, rect.Y + rect.Height),
					Color.White, 0f,
					new Vector2(origin.X, origin.Y),
					1, SpriteEffects.None, 0f);

					Text = "升级需要分数:" + 经验 + "等级:" + (Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().等级() + 1);
					sb.DrawString(
					FontAssets.MouseText.Value,
					Text,
					new Vector2(rect.X, rect.Y + rect.Height / 8),
					Color.White, 0f,
					Vector2.Zero,
					1, SpriteEffects.None, 0f);
				}
				else
				{
					Vector2 origin = ChatManager.GetStringSize(FontAssets.MouseText.Value, "历史最高分数:" + Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().Maxfraction, Vector2.One, 0);
					sb.DrawString(
					FontAssets.MouseText.Value,
					"历史最高分数:" + Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().Maxfraction,
					new Vector2(rect.X + rect.Width, rect.Y + rect.Height),
					Color.White, 0f,
					new Vector2(origin.X, origin.Y),
					1, SpriteEffects.None, 0f);
				}
				if (!Start)
				{
					sb.DrawString(
					FontAssets.DeathText.Value,
					"开始游戏",
					new Vector2(rect.X, rect.Y + rect.Height / 10),
					Color.White, 0f,
					Vector2.Zero,
					S, SpriteEffects.None, 0f);
				}
				/*
				if (immunity>0&&Death)
				{
					sb.DrawString(
					FontAssets.DeathText.Value,
					"点击观看广告复活",
					new Vector2(rect.X, rect.Y + rect.Height / 10),
					Color.White, 0f,
					Vector2.Zero,
					S, SpriteEffects.None, 0f);
				}*/
			}
			var customBossPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
			{
				Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().正在打游戏2 = 10;
                var BossDraw = (SpriteBatch sb, Rectangle rect, Color color) => {
        		DrawBoss(sb, rect, color);
        		DrawMobs(sb, rect, color);
                };
                sb.BossChecklistDraw(BossChecklistBook, rect, color, BossDraw);
            };
			int summonItem = ModContent.ItemType<奇怪的控制器>();


			bossChecklistMod.Call(
				"LogBoss",
				Mod,
				"流星歼灭者",
				5.7f,
				() => NPCDowned.downedMeteorAnnihilator,
				bossType,
				new Dictionary<string, object>()
				{
					//["displayName"] = Language.GetText("Mods.DDmod.NPCs.LifeGuard.BossChecklistIntegration.EntryName2"),
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