using DDmod.Content;
using DDmod.Content.Items;
using DDmod.Content.Items.Melee.Sword.Make;
using DDmod.Content.Items.Series.Venture;
using DDmod.Content.Items.Sundries;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.NPCs.TownNPC;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Tiles.EquipTiles;
using DDmod.Modkey;
using DDmod.ModLinkage.BossChecklist;
using DDmod.NoContent.Config;
using DDmod.Players;
using DDmod.UI.HunterQuests;
using DDmod.UI.LevelUI;
using log4net.Core;
using System.Reflection;
using System.Reflection.Emit;
using Terraria.Chat;
using Terraria.GameContent.UI;
using Terraria.IO;
using Terraria.Map;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;
using Terraria.ObjectData;
using Terraria.UI;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod
{
    public class DDSystem : ModSystem
	{
		public static string HealthBar = "DDmod/UI/血条UI/";
		/// <summary>
		/// 电线窝点
		/// </summary>
		public static Point16 Wiredens;
        public static int AdventureCoins;
		public static int AdventureCoins2;
		public static int AdventureCoins3;
		public static int AdventureCoins4;
		public static int AdventureCoins5;
		//归类血条
		public static void HBar(int type, string name, Vector2 vector = default, string Exname = "",bool Shield = false)
		{
			
			//&&Main.gameMenu && !NPCHealthBar.MaxNPCReset
			if (Main.netMode != 2)
			{
				if (vector != default)
				{
					NPCHealthBar.Headoffset[type] = vector;
				}
				if (NPCHealthBar.Head[type] == null|| NPCHealthBar.Head[type]!= ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/" + Exname + "Head"))
				{
					NPCHealthBar.Head[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/" + Exname + "Head");
					NPCHealthBar.Fill[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/" + Exname + "Fill");
					NPCHealthBar.Mid[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/" + Exname + "Mid");
					NPCHealthBar.Tail[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/" + Exname + "Tail");
					NPCHealthBar.End[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/" + Exname + "End");
					NPCHealthBar.Lock[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/" + Exname + "Lock");
					if (Shield)
					{
						NPCHealthBar.Shield[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/" + Exname + "Shield");
					}
				}
			}
		}
		public static DDSystem Instance;

		public static bool BossSurvival;
		//public static bool InLava;

		public Dictionary<string, int> DDEquipGlow = new Dictionary<string, int>();
		public Dictionary<string, int> DDBossHeld = new Dictionary<string, int>();
		public static int MiniBossMusic = -1;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Type">0不添加,1环境,2肉前boss,3肉后boss</param>
		/// <returns></returns>
		public static int Music(int Type,string name)
		{
			string T = "DDMusicMod/Music/";
			if(Type==1)
			{
				T += "Biome/";

            }
			
			if(Type==2)
			{
				T += "Boss/PreHard/";

            }
			
			if(Type==3)
            {
                T += "Boss/Hard/";

            }

           return MusicLoader.GetMusicSlot(T + name);
        }

        private Type 动态生成寒冰巨剑类(int id)
        {
            AssemblyName 程序集名称 = new AssemblyName("YourModName_DynamicAssembly"); // 动态程序集名称
            AssemblyBuilder 程序集 = AssemblyBuilder.DefineDynamicAssembly(
                name: 程序集名称,
                access: AssemblyBuilderAccess.Run
            );
            ModuleBuilder 模块 = 程序集.DefineDynamicModule("TempModule");

            TypeBuilder 类型构建器 = 模块.DefineType(
                $"DDmod.Content.Items.Melee.Sword.Make.寒冰巨剑_{id}",
                TypeAttributes.Public,
                typeof(寒冰巨剑)
            );

            // 添加字段或逻辑（示例）
            类型构建器.DefineField("剑ID", typeof(int), FieldAttributes.Public);

            return 类型构建器.CreateType();
        }
        public override void Load()
        {
            /*
            for (int i = 1; i <= 10000; i++)
            {
                // 动态生成唯一类名
                Type 动态类型 = 动态生成寒冰巨剑类(i);
                Mod.AddContent((ModItem)Activator.CreateInstance(动态类型));
            }*/
            MiniBossMusic = DDSystem.Music(2, "MiniBoss");
            AdventureCoins = CustomCurrencyManager.RegisterCurrency(new AdventureCoins(ModContent.ItemType<白色委托币>(), 999999L));
            AdventureCoins2 = CustomCurrencyManager.RegisterCurrency(new AdventureCoins2(ModContent.ItemType<绿色委托币>(), 999999L));
            AdventureCoins3 = CustomCurrencyManager.RegisterCurrency(new AdventureCoins3(ModContent.ItemType<蓝色委托币>(), 999999L));
            AdventureCoins4 = CustomCurrencyManager.RegisterCurrency(new AdventureCoins4(ModContent.ItemType<紫色委托币>(), 999999L));
            AdventureCoins5 = CustomCurrencyManager.RegisterCurrency(new AdventureCoins5(ModContent.ItemType<橙色委托币>(), 999999L));
            Instance = this;
			//装备光效贴图
			DDEquipGlowMask.Load(DDEquipGlow);
            TextureAssets.Extra[57] = DDTextures.Nullpng;
            DDOn.DDmodOn.Load();
			//贴图
			DDProjTextures.LoadProjTextures();
			DDItemTextures.LoadItemTextures();

			DDTextures.LoadTextures();
			ModkeySetup.LoadKey(Mod);

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
        public override void PostSetupContent()
        {
        }
        public override void Unload()
		{
			//装备光效贴图
			DDEquipGlowMask.UnLoad(this);
			DDProjTextures.UnloadProjTextures();
			DDItemTextures.UnloadItemTextures();
			DDTextures.UnloadTextures();
			ModkeySetup.UnloadKey();
			NPCHealthBar.MaxNPCReset = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
        }
        public override void LoadWorldData(TagCompound tag)
        {
        }
        public override void PreUpdateTime()
        {
			//手动保存世界
			/*
			if (Main.rand.NextBool(20))
			{
               Task.Run(() =>
                {
                    WorldFile.SaveWorld();

                    // 回到主线程显示完成消息
                    Main.QueueMainThreadAction(() => {
                        Main.NewText("触发");
                    });
                });
                
			}*/
			//InLava = Main.tile[(int)Main.LocalPlayer.Center.X / 16, (int)Main.LocalPlayer.Center.Y / 16].LiquidType == 1 && Main.tile[(int)Main.LocalPlayer.Center.X / 16, (int)Main.LocalPlayer.Center.Y / 16].LiquidAmount > 100;
			bool Boss = false;
			for (int a = 0; a < 200; a++)
			{
				if (Main.npc[a].active&&(Main.npc[a].Dnpc().BossPhysique||Main.npc[a].NPCHB().MiniBoss))
				{
					Boss = true;
					break;
				}
			}
			X = (int)(Main.MouseWorld.X / 16);
			Y = (int)(Main.MouseWorld.Y / 16);
			BossSurvival = Boss;
			if(DDOn.DDmodOn.Start > 0)
            {
				DDOn.DDmodOn.Start--;
			}
			if (SpecialDraw != null)
			{
				for (int A = 0; A < SpecialDraw.Count; A++)
				{
					int i = SpecialDraw[A].X;
					int j = SpecialDraw[A].Y;
					TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
					StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(i, j, tileData.Width, tileData.Height, 18);
					if (tepowerCellFactory != null && tepowerCellFactory.items != null && tepowerCellFactory.items.type != 0)
					{
						if (tepowerCellFactory.SyntheticEffects != -1)
						{
							if (Main.netMode != 2 && tepowerCellFactory.SyntheticEffects == 6)
                            {
                                int R = CombatText.NewText(new Rectangle((int)tepowerCellFactory.items.position.X, (int)tepowerCellFactory.items.Center.Y, 1, 1), new Color(0, 255, 0), tepowerCellFactory.items.GetGlobalItem<StrengthenGlobalItem>().NewText());
								Main.combatText[R].lifeTime = 120;
							}
                            tepowerCellFactory.SyntheticEffects = -1;
							if (Main.netMode == NetmodeID.MultiplayerClient)
							{
								ModPacket packet = DDmod.Instance.GetPacket(256);
								//写入要发的包
								packet.Write((byte)DDType.TalismanTE2);
								packet.WriteVector2(new Vector2(SpecialDraw[A].X, SpecialDraw[A].Y));
								packet.Write(true);
								packet.Write(tepowerCellFactory.SyntheticEffects);
								//发出去
								packet.Send(-1, -1);
							}
						}
					}
				}
			}
		}
		public override void PreUpdateNPCs()
		{
		}
		int rrr;
        public override void PostUpdateWorld()
		{/*
			if (rrr++ % 300 == 0)
			{
				Player.SavePlayer(Main.ActivePlayerFileData);
				Main.NewText(1);
			}*/
			//if (Main.netMode == 2)
            //ChatHelper.BroadcastChatMessage(NetworkText.FromKey("妈妈"), new Color(175, 75, 255));
        }
        public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = BossSurvival;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			BossSurvival = flags[0];
		}
		public void Exp(SpriteBatch spriteBatch)
        {
            Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/升级UI").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("DDmod/Content/升级UI_E").Value;
			EntrustPlayer entrust = Main.LocalPlayer.GetModPlayer<EntrustPlayer>();

            if (entrust.UIPo == Vector2.Zero)
			{
				entrust.UIPo.X = 800;
				entrust.UIPo.Y = 40;
			}
			if (entrust.UIPo.X < 53 || entrust.UIPo.X > Main.screenWidth)
            {
                entrust.UIPo.X = Main.screenWidth / 2;
            }
            if (entrust.UIPo.Y < 16 || entrust.UIPo.Y > Main.screenHeight)
            {
                entrust.UIPo.Y = Main.screenHeight / 2;
            }
            float A = (float)entrust.Experience / entrust.MaxExperience;
			if(!float.IsNormal(A))
			{
				A = 0;
			}
			if(A>1)
			{
				A = 1;
			}
                spriteBatch.Draw(texture2, entrust.UIPo+new Vector2(18,0), new Rectangle(0, 0, (int)(texture2.Width * A), texture2.Height), Color.White, 0, new Vector2(texture2.Width / 2, texture2.Height / 2), 1, 0, 0);

            spriteBatch.Draw(texture, entrust.UIPo, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);
			Color color = new Color(155, 155, 155, 255);
			if(entrust.Level>=5)
			{
                color = new Color(255, 255, 255, 255);
            }
			if(entrust.Level>=10)
			{
                color = new Color(100, 255, 100, 255);
            }
			if(entrust.Level>=20)
			{
                color = new Color(100, 100, 255, 255);
            }
			if(entrust.Level>=30)
			{
                color = new Color(255, 0, 255, 255);
            }
			if(entrust.Level>=40)
			{
                color = new Color(255, 180, 0, 255);
            }
			if(entrust.Level>=50)
			{
                color = new Color(255, 50, 50, 255);
            }

            Vector2 origin = ChatManager.GetStringSize(FontAssets.DeathText.Value, ""+entrust.Level, Vector2.One, 0) / 2;
            ChatManager.DrawColorCodedStringWithShadow(
            spriteBatch,
            FontAssets.DeathText.Value, "" + entrust.Level, entrust.UIPo-new Vector2(43,-3), color, 0, origin, new Vector2(0.35f), 114514);

			string Text;
			if (entrust.Level < entrust.MaxLevel)
			{
				Text = Language.GetTextValue("Mods.DDmod.Entrust.升级进度") + ": " + entrust.Experience + "/" + entrust.MaxExperience +
					"\n+" + entrust.Level * 2 + " " + Language.GetTextValue("Mods.DDmod.properties.生命") +
                "\n" + Language.GetTextValue("Mods.DDmod.UI.右键拖动");
            }
			else
			{
                Text = Language.GetTextValue("Mods.DDmod.Entrust.升级进度") + ": Max" +
                    "\n+" + entrust.Level * 2 + " " + Language.GetTextValue("Mods.DDmod.properties.生命")+
                    "\n"+ Language.GetTextValue("Mods.DDmod.UI.右键拖动");
            }
            if (!entrust.mouseX && new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)entrust.UIPo.X - 59, (int)entrust.UIPo.Y - 20, 118, 40)))
            {
                UICommon.TooltipMouseText(Text);

                entrust.mouse = true;
            }
            if (Main.mouseRight)
            {
                entrust.mouseX = true;
            }

            if (entrust.mouse && entrust.mouseX)
            {
                entrust.UIPo = new Vector2(Main.mouseX, Main.mouseY);
            }
            if (Main.mouseRightRelease)
            {
                entrust.mouse = false;
                entrust.mouseX = false;
            }
        }
		static byte Current = 0;
		public override void PostDrawInterface(SpriteBatch spriteBatch)
		{
            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            if (Main.LocalPlayer.TPlayer().Talisman&& Main.LocalPlayer.TPlayer().MaxTalismanCD>0)
			{
				Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/法宝充能UI").Value;
				spriteBatch.Draw(texture, Main.LocalPlayer.TPlayer().TalismanPo, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 4), 1, 0, 0);
				float A = (float)Main.LocalPlayer.TPlayer().TalismanCD2 / Main.LocalPlayer.TPlayer().MaxTalismanCD;
				if(Main.LocalPlayer.TPlayer().UseTalisman)
                {
					A = (float)Main.LocalPlayer.TPlayer().TalismanTimes2 / Main.LocalPlayer.TPlayer().MaxTalismanTimes;
				}
				spriteBatch.Draw(texture, Main.LocalPlayer.TPlayer().TalismanPo + new Vector2(8, 0), new Rectangle?(new Rectangle(8, texture.Height / 2, (int)((texture.Width - 16) * A), texture.Height / 2)), Color.White*0.75f, 0, new Vector2((texture.Width) / 2, texture.Height / 4), 1, 0, 0);
				if (A >= 1)
				{
					spriteBatch.Draw(texture, Main.LocalPlayer.TPlayer().TalismanPo + new Vector2(8, 0), new Rectangle?(new Rectangle(8, texture.Height / 2, (int)((texture.Width - 16) * A), texture.Height / 2)), new Color(0, 155, 255, 0), 0, new Vector2((texture.Width) / 2, texture.Height / 4), 1, 0, 0);
					spriteBatch.Draw(texture, Main.LocalPlayer.TPlayer().TalismanPo + new Vector2(8, 0), new Rectangle?(new Rectangle(8, texture.Height / 2, (int)((texture.Width - 16) * A), texture.Height / 2)), new Color(0, 155, 255, 0), 0, new Vector2((texture.Width) / 2, texture.Height / 4), 1, 0, 0);
					spriteBatch.Draw(texture, Main.LocalPlayer.TPlayer().TalismanPo + new Vector2(8, 0), new Rectangle?(new Rectangle(8, texture.Height / 2, (int)((texture.Width - 16) * A), texture.Height / 2)), new Color(0, 155, 255, 0), 0, new Vector2((texture.Width) / 2, texture.Height / 4), 1, 0, 0);
				}
				if(ModkeySetup.TalismanKey.Current)
				{
					Current++;

                }
				else
				{
					Current = 0;

                }
				if ((new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)Main.LocalPlayer.TPlayer().TalismanPo.X - 33, (int)Main.LocalPlayer.TPlayer().TalismanPo.Y - 13, 66, 26))|| Current>180))
                {
					for (int a = -1; a <= 1; a += 2)
					{
						for (int b = -1; b <= 1; b += 2)
						{
							DynamicSpriteFontExtensionMethods.DrawString(
						spriteBatch,
						FontAssets.MouseText.Value,
						(int)(A * 100) + "%",
						new Vector2(Main.mouseX + 10 + a, Main.mouseY + 10 + b),
                        new Color(0, 0, 0, 255), 0f,
						Vector2.Zero,
						1F, SpriteEffects.None, 0f);
						}
					}
                    DynamicSpriteFontExtensionMethods.DrawString(
                    spriteBatch,
                    FontAssets.MouseText.Value,
                    (int)(A*100)+"%",
                    new Vector2(Main.mouseX+10, Main.mouseY+10),
                    Color.White, 0f,
                    Vector2.Zero,
                    1F, SpriteEffects.None, 0f);
                    Main.LocalPlayer.TPlayer().mouse = true;
				}
				if (Main.mouseRight)
                {
					Main.LocalPlayer.TPlayer().mouseX = true;
				}

				if (Main.LocalPlayer.TPlayer().mouse&& Main.LocalPlayer.TPlayer().mouseX)
				{
					Main.LocalPlayer.TPlayer().TalismanPo = new Vector2(Main.mouseX, Main.mouseY);
				}
				if (Main.mouseRightRelease)
				{
					Main.LocalPlayer.TPlayer().mouse = false;
					Main.LocalPlayer.TPlayer().mouseX = false;
				}
			}
			Exp(spriteBatch);

            if (Main.netMode == 1 && ModContent.GetInstance<DDConfigClient>().Delay)
			{
				Color color = new Color(0, 255, 0);
				int f = 0;
				if (Main.LocalPlayer.Dplayer().Delay < 10)
				{
					color = new Color(0, 255, 0);
					f = 0;
				}
				else if (Main.LocalPlayer.Dplayer().Delay < 30)
				{
					color = new Color(255, 255, 0);
					f = 1;

				}
				else
				{
					color = new Color(255, 0, 0);
					f = 2;
				}
				spriteBatch.Draw(DDTextures.Wifi.Value, new Vector2(4, Main.screenHeight) - new Vector2(0, 50), new Rectangle?(new Rectangle(DDTextures.Wifi.Width() / 3 * f, 0, DDTextures.Wifi.Width() / 3, DDTextures.Wifi.Height())), Color.White, 0, Vector2.Zero, 1, 0, 0);

				DynamicSpriteFontExtensionMethods.DrawString(
				spriteBatch,
				FontAssets.MouseText.Value,
				Main.LocalPlayer.Dplayer().Delay + " Frame delay",
				new Vector2(6 + DDTextures.Wifi.Width() / 3, Main.screenHeight) - new Vector2(0, 50),
				color, 0f,
				Vector2.Zero,
				1F, SpriteEffects.None, 0f);
            }
            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
        }

        public static bool English => Language.ActiveCulture != GameCulture.FromLegacyId(7);

		int X = (int)(Main.MouseWorld.X / 16);
		int Y = (int)(Main.MouseWorld.Y / 16);

		public static List<Point16> SpecialDraw = new List<Point16>();
		public static List<Point16> 鱼缸Draw = new List<Point16>();
		NPC npc = new NPC();
		NPC[] npc2;
		float[] sc;
		Vector2 vector;
		public override void PostDrawTiles()
		{
			//温馨屋子
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			Player player = Main.player[Main.myPlayer];
			if (player.HeldItem.type == ModContent.ItemType<CozyCabin>())
			{
				Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/Items/Sundries/温馨木屋").Value;
				Main.spriteBatch.Draw(texture, new Vector2(X, Y)*16 - new Vector2(0, texture.Height-16) - Main.screenPosition, null, Color.White*0.6f, 0, Vector2.Zero, 1F, 0, 0);
			}
			if (player.HeldItem.type == ModContent.ItemType<NOCozyCabin>())
			{
				Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/Items/Sundries/不温馨木屋").Value;
				Main.spriteBatch.Draw(texture, new Vector2(X, Y)*16 - new Vector2(0, texture.Height-16) - Main.screenPosition, null, Color.White*0.6f, 0, Vector2.Zero, 1F, 0, 0);
			}
			if (SpecialDraw != null)
			{
				for (int A = 0; A < SpecialDraw.Count; A++)
				{
					int i = SpecialDraw[A].X;
					int j = SpecialDraw[A].Y;
					TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
					StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(i, j, tileData.Width, tileData.Height, 18);
					if (tepowerCellFactory != null && tepowerCellFactory.items != null && tepowerCellFactory.items.type != 0)
					{
						var texScale = 1f;
						Item ContainedItem = tepowerCellFactory.items;
						Main.instance.LoadItem(ContainedItem.type);
						var frame = Main.itemAnimations[ContainedItem.type] != null ? Main.itemAnimations[ContainedItem.type].GetFrame(TextureAssets.Item[ContainedItem.type].Value) : TextureAssets.Item[ContainedItem.type].Frame(1, 1, 0, 0);
						var size = frame.Size();
						Texture2D texture = TextureAssets.Item[ContainedItem.type].Value;
						if (size.X > 30)
						{
							if (size.X > 30)
							{
								texScale *= (float)30 / size.X;
							}
						}
						else
						{
							if (size.Y > 30)
							{
								texScale *= (float)30 / size.Y;
							}
						}
						Main.spriteBatch.Draw(texture, ContainedItem.position - Main.screenPosition, frame, Color.White, 0, new Vector2(size.X/2, size.Y), texScale, 0, 0f);
						for (int a = 0; a < 3; a++)
						{
							texScale = 1f;
							Item ContainedItem2 = tepowerCellFactory.FortifiedStone[a];
							if (ContainedItem2.type != 0)
							{
								Main.instance.LoadItem(ContainedItem2.type);
								var frame2 = Main.itemAnimations[ContainedItem2.type] != null ? Main.itemAnimations[ContainedItem2.type].GetFrame(TextureAssets.Item[ContainedItem2.type].Value) : TextureAssets.Item[ContainedItem2.type].Frame(1, 1, 0, 0);
								var size2 = frame2.Size();
								Texture2D texture2 = TextureAssets.Item[ContainedItem2.type].Value;
								if (size2.X > 30)
								{
									if (size2.X > 30)
									{
										texScale *= (float)30 / size2.X;
									}
								}
								else
								{
									if (size2.Y > 30)
									{
										texScale *= (float)30 / size2.Y;
									}
								}
								if (ContainedItem2.type == ModContent.ItemType<StrengtheningStone>())
								{
									Main.spriteBatch.Draw(DDTextures.GlowEffect.Value, ContainedItem2.position - new Vector2(0, 10) - Main.screenPosition, null, new Color(155, 155, 155, 0), 0, DDTextures.GlowEffect.Size() / 2, 0.15F, 0, 0f);
								}
								if (ContainedItem2.type == ModContent.ItemType<StrengtheningStone2>())
								{
									Main.spriteBatch.Draw(DDTextures.GlowEffect.Value, ContainedItem2.position-new Vector2(0,10) - Main.screenPosition, null, new Color(100, 255, 100, 0), 0, DDTextures.GlowEffect.Size()/2, 0.15F, 0, 0f);
								}
								if (ContainedItem2.type == ModContent.ItemType<StrengtheningStone3>())
								{
									Main.spriteBatch.Draw(DDTextures.GlowEffect.Value, ContainedItem2.position-new Vector2(0,10) - Main.screenPosition, null, new Color(0, 100, 255, 0), 0, DDTextures.GlowEffect.Size()/2, 0.15F, 0, 0f);
								}
								Main.spriteBatch.Draw(texture2, ContainedItem2.position - Main.screenPosition, frame2, Color.White, 0, new Vector2(size2.X / 2, size2.Y), texScale, 0, 0f);
							}
						}
						Texture2D Scanning = DDTextures.Scanning2.Value;
						if (tepowerCellFactory.items.type != 0)
						{
							Color color = new Color(219, 130, 255, 0);
							if (tepowerCellFactory.Level == 2)
                            {
								color = new Color(107, 255, 118, 0);

							}
							Main.spriteBatch.Draw(Scanning, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) - new Vector2(-24, -10), new Rectangle?(new Rectangle(0, 0, Scanning.Width, Scanning.Height)), color, 0, new Vector2(Scanning.Width / 2, Scanning.Height / 2), new Vector2(0.8F, 0.6F), 0, 0f);
							Main.spriteBatch.Draw(Scanning, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) - new Vector2(-24, -10), new Rectangle?(new Rectangle(0, 0, Scanning.Width, Scanning.Height)), color, 0, new Vector2(Scanning.Width / 2, Scanning.Height / 2), new Vector2(0.8F, 0.6F), 0, 0f);

							Main.spriteBatch.Draw(Scanning, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) - new Vector2(-24, -20), new Rectangle?(new Rectangle(0, 0, Scanning.Width, Scanning.Height)), color, 0, new Vector2(Scanning.Width / 2, Scanning.Height / 2 + 10), new Vector2(0.8F, 0.6F + ((float)tepowerCellFactory.t / 5 - 10) / 30), 0, 0f);
							Main.spriteBatch.Draw(Scanning, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) - new Vector2(-24, -20), new Rectangle?(new Rectangle(0, 0, Scanning.Width, Scanning.Height)), color, 0, new Vector2(Scanning.Width / 2, Scanning.Height / 2 + 10), new Vector2(0.8F, 0.6F + ((float)tepowerCellFactory.t / 5 - 10) / 30), 0, 0f);
						}
					}
					if (tepowerCellFactory != null&&tepowerCellFactory.Level == 2)
					{
						Main.spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Content/Tiles/EquipTiles/TerraStrengthenPlatform2").Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y+2)), null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
					}
                    else
					{
						Main.spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Content/Tiles/EquipTiles/StrengthenPlatform2").Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y+2)), null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
					}
					if (tepowerCellFactory == null)
                    {
						SpecialDraw.Remove(new Point16(SpecialDraw[A].X, SpecialDraw[A].Y));
					}
				}
			}
			if (鱼缸Draw != null)
			{
				for (int A = 0; A < 鱼缸Draw.Count; A++)
				{
					int i = 鱼缸Draw[A].X;
					int j = 鱼缸Draw[A].Y;
					Main.spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Content/Tiles/EquipTiles/鱼缸").Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y + 2)), null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
					Main.spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Content/Tiles/EquipTiles/玻璃").Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y + 2)), null, Color.White*0.02f, 0f, Vector2.Zero, 1f, 0, 0f); 

                    for (int r = 0; r < 12; r++)
					{
						if (npc2 == null || npc2.Length == 0|| sc==null || sc.Length != npc2.Length)
						{
							npc2 = new NPC[] { new NPC(), new NPC(), new NPC(), new NPC(), new NPC() };
							sc = new float[npc2.Length];
						}
						else
						{
							for (int a = 0; a < npc2.Length; a++)
							{
								if (npc2[a].type == 0)
								{
									npc2[a].SetDefaults(55);
								}
								if (npc2[a].scale>sc[a])
                                {
									if (npc2[a].scale - sc[a] > 0.001F)
									{
										npc2[a].scale -= 0.001F;
									}
                                    else
                                    {
										npc2[a].scale = sc[a];

									}
								}
								if (npc2[a].scale<sc[a])
                                {
									if (sc[a]-npc2[a].scale > 0.001F)
									{
										npc2[a].scale += 0.001F;
									}
                                    else
                                    {
										npc2[a].scale = sc[a];

									}
								}
								if(sc[a]==0)
								{
									sc[a] = (float)Main.rand.Next(80, 100) / 100;
								}
								int R = 0;
								for (int I = 1; I < 10; I++)
								{
									if ((npc2[a].scale - 0.8F) > 0.2F / 12 * I)
									{
										R++;
									}
								}
							    if (r == R)
								{
									Main.instance.LoadNPC(npc2[a].type);
									npc2[a].wet = true;
									if (npc2[a].position.X < i * 16 + (npc2[a].width  / 2 + 2) * npc2[a].scale)
									{
										npc2[a].position.X = i * 16 + (npc2[a].width  / 2 + 2) * npc2[a].scale;
										npc2[a].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
									}
									if (npc2[a].position.X > (i + 10 - 1) * 16 - (npc2[a].width / 2 + 2) * npc2[a].scale)
									{
										npc2[a].position.X = (i + 10 - 1) * 16 - (npc2[a].width / 2 + 2) * npc2[a].scale;
										npc2[a].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
									}
									if (npc2[a].position.Y < j * 16 + (npc2[a].height / 2 + 2) * npc2[a].scale)
									{
										npc2[a].position.Y = j * 16 + (npc2[a].height / 2 + 2) * npc2[a].scale;
										npc2[a].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
									}
									if (npc2[a].position.Y > (j + 5 - 1) * 16 - (npc2[a].height / 2 + 2) * npc2[a].scale)
									{
										npc2[a].position.Y = (j + 5 - 1) * 16 - (npc2[a].height / 2 + 2) * npc2[a].scale;
										npc2[a].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
									}
									if (Main.rand.NextBool(100))
									{
										npc2[a].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1))/3;
										sc[a] = (float)Main.rand.Next(80, 100) / 100;
									}
									if (npc2[a].velocity.X < 0)
									{
										npc2[a].direction = -1;
									}
									else
									{
										npc2[a].direction = 1;
									}
									npc2[a].position += npc2[a].velocity;
									NPCLoader.FindFrame(npc2[a], TextureAssets.Npc[npc2[a].type].Height() / Main.npcFrameCount[npc2[a].type]);
									if (NPCLoader.PreDraw(npc2[a], Main.spriteBatch, Main.screenPosition, Color.White))
									{
										DDHelper.MethodReflection(Main.instance.GetType(), "DrawNPCDirect_Inner", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Main.instance, new object[] { Main.spriteBatch, npc2[a], false, Main.screenPosition, Color.White });
									}
									NPCLoader.PostDraw(npc2[a], Main.spriteBatch, Main.screenPosition, Color.White);
								}
							}
						}
						Main.spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Content/Tiles/EquipTiles/水").Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y + 2)), null, Color.White * 0.1f, 0f, Vector2.Zero, 1f, 0, 0f);
					}
					Main.spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Content/Tiles/EquipTiles/玻璃").Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y + 2)), null, Color.White * 0.01f, 0f, Vector2.Zero, 1f, 0, 0f);

					if (Main.tile[i, j].TileType != ModContent.TileType<鱼缸>())
					{
						鱼缸Draw.Remove(new Point16(鱼缸Draw[A].X, 鱼缸Draw[A].Y));
					}
				}
			}
			//Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, new Color(255, 155, 155, 0) *0.91f, 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight), 0, 0);
			Main.spriteBatch.End();
		}
	}
}