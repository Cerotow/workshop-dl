using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using DDmod.Worlds;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;
using DDmod.Content.Items.Series.Heart;
using DDmod.Content.Items.Tiles.晶凝;
using System.Reflection;
using Terraria.GameContent.Events;
using DDmod.Content.Tiles.家园塔;
using DDmod.Content.Dusts;
using DDmod.Content.Tiles.农场.果树;

namespace DDmod.Content.Tiles.农场
{
	public class DDTileDawn
	{
		public Point tiles;
		public int type;
		public int Time;
		public int Max;
		public float rotation;
		public float rotation2;
		public bool rotationB;
		public float Erotation;
		public float Erotation2;
		public bool ErotationB;
		public Point16 Sice;
		public bool variation;
		public bool Wind;
		/// <summary>
		/// 玩家经过
		/// </summary>
		public int PlayerElapse;
		public DDTileDawn(int i, int j, int type, int width, int height, int Time, int Max, bool variation = false,bool Wind =true)
		{
			tiles = new Point(i, j);
			Sice = new Point16(width, height);
			this.variation = variation;
			this.Wind = Wind;
			this.type = type;
			this.Time = Time;
			this.Max = Max;
		}
		public void Update()
		{
			//Main.windSpeedCurrent =1f;

            float WindSpeed = Main.windSpeedCurrent / 4;

            if (Erotation2 != 0)
            {
                Erotation2 -= 0.01F * (0.5F + Math.Abs(Erotation2));
                Erotation2 = MathHelper.Clamp(Erotation2, 0f, 1.6f);

                if (Erotation2 > 0)
                {
                    float minSwing, maxSwing;
                    if (Math.Abs(Main.windSpeedCurrent) > 0.1f)
                    {
                        minSwing = WindSpeed > 0 ? -Erotation2 : -Erotation2 / 2;
                        maxSwing = WindSpeed > 0 ? Erotation2 / 2 : Erotation2;
                    }
                    else
                    {
                        minSwing = -Erotation2;
                        maxSwing = Erotation2;
                    }

                    float swingSpeed = 0.04f * (0.5F + Erotation2);
                    DDHelper.BackAndForth(minSwing, maxSwing, swingSpeed, ref Erotation, ref ErotationB, false);

                    if (Math.Abs(Erotation2) < 0.02f)
                    {
                        Erotation = 0;
                    }
                }
            }

            if (Erotation2 == 0 && Math.Abs(Erotation) > 0.03f)
            {
                DDHelper.BackAndForth(0, 0, 0.03f, ref Erotation, ref ErotationB, false);
            }
            else if (Erotation2 == 0)
            {
                Erotation = 0;
            }
            if (Wind && (Main.tile[tiles.X, tiles.Y + Sice.Y - 1].WallType == 0 || WorldGen.DefaultTreeWallTest(Main.tile[tiles.X, tiles.Y + Sice.Y - 1].WallType)))
			{
				if (WindSpeed > 0)
				{
					if (WindSpeed > 0.4F)
					{
						WindSpeed = 0.4F;
					}
					if (Main.windSpeedCurrent > 0.1)
					{
						if (Math.Abs(Erotation) < Math.Abs(Erotation2) * 0.4f)
						{
							ErotationB = false;
						}
					}
					Erotation2 += Math.Abs(Main.windSpeedCurrent) / 50;
					DDHelper.BackAndForth(WindSpeed / 4, WindSpeed / 2, Math.Abs(WindSpeed) / 40, ref rotation, ref rotationB, false);
				}
				else if (WindSpeed < 0)
				{
					if (WindSpeed < -0.4F)
					{
						WindSpeed = -0.4F;
					}
					if (Main.windSpeedCurrent < -0.1)
					{
						if (Math.Abs(Erotation) < Math.Abs(Erotation2) * 0.4f)
						{
							ErotationB = true;
						}
					}
					Erotation2 += Math.Abs(Main.windSpeedCurrent) / 50;
					DDHelper.BackAndForth(WindSpeed / 2, WindSpeed / 4, Math.Abs(WindSpeed) / 40, ref rotation, ref rotationB, false);
				}
				else
				{

					if (Math.Abs(rotation) > 0.03F)
					{
						DDHelper.BackAndForth(0, 0, 0.03F, ref rotation, ref rotationB, false);
                    }
                    else
                    {
                        rotation = 0;

                    }
                }
			}
			else
			{

				if (Math.Abs(rotation) > Math.Abs(Main.windSpeedCurrent) / 150)
				{
						DDHelper.BackAndForth(0, 0, Math.Abs(Main.windSpeedCurrent) / 150, ref rotation, ref rotationB, false);
				}
				else
				{
					rotation = 0;

				}
			}
			if (Wind)
			{
				if (PlayerElapse > 0)
				{
					PlayerElapse--;
					rotation2 += 0.02F;
					if (rotation2 > 0.3F)
					{
						PlayerElapse = 0;
					}
				}
				else if (PlayerElapse < 0)
				{
					PlayerElapse++;
					rotation2 -= 0.02F;
					if (rotation2 < -0.3F)
					{
						PlayerElapse = 0;
					}
				}
				if (PlayerElapse == 0)
				{
					if (rotation2 > 0)
					{
						rotation2 -= 0.02F;
					}
					else if (rotation2 < 0)
					{
						rotation2 += 0.02f;
					}
					if (Math.Abs(rotation2) < 0.02F)
					{
						rotation2 = 0;
					}
				}
			}
			for (int a = 0; a < 255; a++)
			{
				Player player = Main.player[a];
				Vector2 vector = player.position + new Vector2(player.width / 2, player.height);
				if (new Point((int)(vector.X / 16), (int)(vector.Y / 16) - (Sice.Y)) == tiles)
				{
					if (Math.Abs(player.velocity.X) > 2)
					{
						if (player.velocity.X > 0)
						{
							PlayerElapse = 10;
							ErotationB = false;
							Erotation -= 0.1F;
						}
						else if (player.velocity.X < 0)
						{
							PlayerElapse = -10;
							ErotationB = true;
							Erotation += 0.1F;
						}
						Erotation2 = 2;
					}
				}
			}
		}
	}
	public class DDTileDawnSystem : ModSystem
	{
        public override void Load()
		{
			Terraria.On_Main.DoDraw_WallsTilesNPCs += DoDraw_WallsTilesNPCs;
        }
        public static float MaxFilter = 0;
        public static float Filterincrease = 0;
        public static float FilterValue = 0;
        public static float DrownFilterValue = 0;
        public static Color color;
        public static void Filter(Color c, float Max, float Speed)
        {
            color = c;
            MaxFilter = Max;
            Filterincrease = Speed;
        }
        
        public void DoDraw_WallsTilesNPCs(Terraria.On_Main.orig_DoDraw_WallsTilesNPCs orig, Main main)
		{
			try
			{
				DDHelper.MethodReflection(main.GetType(), "CacheNPCDraws", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
				DDHelper.MethodReflection(main.GetType(), "CacheProjDraws", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
				DDHelper.MethodReflection(main.GetType(), "DrawCachedNPCs", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, new object[] { Main.instance.DrawCacheNPCsMoonMoon, true });
				DDHelper.MethodReflection(main.GetType(), "DoDraw_WallsAndBlacks", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
                DDHelper.MethodReflection(main.GetType(), "DrawWoF", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
                DGlobalNPC.WallDraw(main);
                DDHelper.MethodReflection(main.GetType(), "DrawBackGore", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
				MoonlordDeathDrama.DrawPieces(Main.spriteBatch);
				MoonlordDeathDrama.DrawExplosions(Main.spriteBatch);
				DDHelper.MethodReflection(main.GetType(), "DrawCachedNPCs", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, new object[] { Main.instance.DrawCacheNPCsBehindNonSolidTiles, true });
				DDHelper.MethodReflection(main.GetType(), "DoDraw_Tiles_NonSolid", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
				DDHelper.MethodReflection(main.GetType(), "DoDraw_Waterfalls", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
			}
			catch (Exception e)
			{
				TimeLogger.DrawException(e);
			}

			菜Draw();
			Main.spriteBatch.End();
			try
			{
				bool detectCreature = Main.player[Main.myPlayer].detectCreature;
				if (!detectCreature)
				{
					DDHelper.MethodReflection(main.GetType(), "DoDraw_DrawNPCsBehindTiles", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
				}
				DDHelper.MethodReflection(main.GetType(), "DoDraw_Tiles_Solid", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);

				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				for (int i = 0; i < Main.maxDustToDraw; i++)
				{
					Dust dust = Main.dust[i];
					if (!dust.active || !GlobalDust.DustPreTile[dust.dustIndex])
						continue;

					float scale = dust.GetVisualScale();
					Color newColor = Lighting.GetColor((int)(dust.position.X + 4.0) / 16, (int)(dust.position.Y + 4.0) / 16);

					newColor = dust.GetAlpha(newColor);
					DrawDust.Drawdust(dust, newColor, scale);
					ModDust modDust = DustLoader.GetDust(dust.type);
					if (modDust != null)
					{

						if (DrawDust.PreModDrawdust(dust, newColor, scale) & modDust.PreDraw(dust))
						{
							Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, newColor, dust.rotation, new Vector2(4f, 4f), scale, SpriteEffects.None, 0f);

							if (dust.color != default)
							{
								Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetColor(newColor), dust.rotation, new Vector2(4f, 4f), scale, SpriteEffects.None, 0f);
							}
						}
						if (newColor == Color.Black)
						{
							dust.active = false;
						}
						DrawDust.ModDrawdust(dust, newColor, scale);
						continue;
					}
				}
				Main.spriteBatch.End();

                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);
                if (Main.LocalPlayer.whoAmI == Main.myPlayer)
                {
                    if (!Main.gamePaused)
                    {
                        if (FilterValue < MaxFilter)
                        {
                            FilterValue += Filterincrease;
                        }
                        else if (MaxFilter != 0 && FilterValue > MaxFilter)
                        {
                            FilterValue = MaxFilter;
                        }
                        else if (FilterValue > 0)
                        {
                            FilterValue -= Filterincrease;
                        }
                        if (FilterValue < 0)
                        {
                            FilterValue = 0;
                            Filterincrease = 0;
                        }
                    }
					
                    if (DDTextures.WhitePng != null && FilterValue > 0)
                    {
                        if (!Main.gamePaused)
                        {
                            MaxFilter = 0;
                        }
                        Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, color * FilterValue, 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 0, 0);
                    }
                }
                Main.spriteBatch.End();


                if (detectCreature)
				{
					DDHelper.MethodReflection(main.GetType(), "DoDraw_DrawNPCsBehindTiles", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
				}
				DDHelper.MethodReflection(main.GetType(), "DrawPlayers_BehindNPCs", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
				DDHelper.MethodReflection(main.GetType(), "DoDraw_DrawNPCsOverTiles", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, null);
				TimeLogger.DetailedDrawReset();
			}
			catch (Exception e2)
			{
				TimeLogger.DrawException(e2);
            }


            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

		}

		public static Item itemText;
		public static Vector2 MouseWorld;
		public override void PostDrawInterface(SpriteBatch spriteBatch)
		{

			if (itemText != null && itemText.type > 0)
			{
				Main.hoverItemName = itemText.Name;
				Main.HoverItem = itemText.Clone();
				itemText = null;
			}
		}
		public void 菜Draw()
		{
			if (SpecialDraw != null)
			{
				for (int A = 0; A < SpecialDraw.Count; A++)
				{
					int i = SpecialDraw[A].tiles.X;
					int j = SpecialDraw[A].tiles.Y;
					TileObjectData tileData = TileObjectData.GetTileData(SpecialDraw[A].type, 0, 0);

                    菜TE Entity = playerHelper.FindTileEntity2<菜TE>(i, j, tileData.Width, tileData.Height, 18);
					Tile tile = Main.tile[i, j];
					if (Entity != null)
					{
						int height = 18;
						int frameXPos = (int)tile.TileFrameX;
						int frameYPos = (int)tile.TileFrameY;
						int Lvevl = Entity.Time / SpecialDraw[A].Time;
						if (Lvevl > SpecialDraw[A].Max)
						{
							Lvevl = SpecialDraw[A].Max;
						}
						if (SpecialDraw[A].variation && Entity.variation)
						{
							Lvevl++;
						}
						frameXPos += 18 * tileData.Width * Lvevl;
						if (Entity.direction < 0)
						{
							frameYPos += 18 * tileData.Height;
						}
						if (SpecialDraw[A].type == ModContent.TileType<荧光果植株>())
						{
							if (Lvevl == 5)
							{
								Rectangle? rectangle = new Rectangle?(new Rectangle(0, 0, 荧光果植株.asset.Width() / 2, 荧光果植株.asset.Height()));
								Color color = Lighting.GetColor(i, j, Color.White);
								Main.spriteBatch.Draw(荧光果植株.asset.Value, new Vector2(i * 16, j * 16) + new Vector2(8 - 4 * Entity.direction, 34) - new Vector2((-13 - (Entity.direction > 0 ? 6 : 0)) * Entity.direction, 20).RotatedBy((SpecialDraw[A].rotation + SpecialDraw[A].rotation2) / 2 * (0.8f + 0.2F * tileData.Height)) + new Vector2(荧光果植株.asset.Width() / 4, 0) - Main.screenPosition, rectangle, color, SpecialDraw[A].Erotation, new Vector2(荧光果植株.asset.Width() / 4, 0), 1f, 0, 0f);
								Main.spriteBatch.Draw(荧光果植株.Glow.Value, new Vector2(i * 16, j * 16) + new Vector2(8 - 4 * Entity.direction, 34) - new Vector2((-13 - (Entity.direction > 0 ? 6 : 0)) * Entity.direction, 20).RotatedBy((SpecialDraw[A].rotation + SpecialDraw[A].rotation2) / 2 * (0.8f + 0.2F * tileData.Height)) + new Vector2(荧光果植株.Glow.Width() / 4, 0) - Main.screenPosition, rectangle, Color.White, SpecialDraw[A].Erotation, new Vector2(荧光果植株.Glow.Width() / 4, 0), 1f, 0, 0f);
								Lighting.AddLight(new Vector2(i * 16, j * 16) + new Vector2(8 - 4 * Entity.direction, 34) - new Vector2(-19, 18).RotatedBy((SpecialDraw[A].rotation + SpecialDraw[A].rotation2) / 2 * (0.8f + 0.2F * tileData.Height)) + new Vector2(荧光果植株.Glow.Width() / 4, 荧光果植株.Glow.Width() / 2), new Color(0, 255, 253).ToVector3() * 0.8F);
							}
							if (Lvevl == 6)
							{
								Rectangle? rectangle = new Rectangle?(new Rectangle(荧光果植株.asset.Width() / 2, 0, 荧光果植株.asset.Width() / 2, 荧光果植株.asset.Height()));
								Color color = Lighting.GetColor(i, j, Color.White);
								Main.spriteBatch.Draw(荧光果植株.asset.Value, new Vector2(i * 16, j * 16) + new Vector2(8 - 4 * Entity.direction, 34) - new Vector2((-13 - (Entity.direction > 0 ? 6 : 0)) * Entity.direction, 18).RotatedBy((SpecialDraw[A].rotation + SpecialDraw[A].rotation2) / 2 * (0.8f + 0.2F * tileData.Height)) + new Vector2(荧光果植株.asset.Width() / 4, 0) - Main.screenPosition, rectangle, color, SpecialDraw[A].Erotation, new Vector2(荧光果植株.asset.Width() / 4, 0), 1f, 0, 0f);
								Main.spriteBatch.Draw(荧光果植株.Glow.Value, new Vector2(i * 16, j * 16) + new Vector2(8 - 4 * Entity.direction, 34) - new Vector2((-13 - (Entity.direction > 0 ? 6 : 0)) * Entity.direction, 18).RotatedBy((SpecialDraw[A].rotation + SpecialDraw[A].rotation2) / 2 * (0.8f + 0.2F * tileData.Height)) + new Vector2(荧光果植株.Glow.Width() / 4, 0) - Main.screenPosition, rectangle, Color.White, SpecialDraw[A].Erotation, new Vector2(荧光果植株.Glow.Width() / 4, 0), 1f, 0, 0f);
								Lighting.AddLight(new Vector2(i * 16, j * 16) + new Vector2(8 - 4 * Entity.direction, 34) - new Vector2(-19, 18).RotatedBy((SpecialDraw[A].rotation + SpecialDraw[A].rotation2) / 2 * (0.8f + 0.2F * tileData.Height)) + new Vector2(荧光果植株.Glow.Width() / 4, 荧光果植株.Glow.Width() / 2), new Color(0, 255, 253).ToVector3() * 0.8F);
							}
							for (int a = 0; a < tileData.Width; a++)
							{
								for (int b = 0; b < tileData.Height; b++)
								{
									Rectangle? rectangle = new Rectangle?(new Rectangle(frameXPos + 18 * a, frameYPos + 18 * b, 16, height));
									Color color = Lighting.GetColor(i + a, j + b, Color.White);
									Main.spriteBatch.Draw(TextureAssets.Tile[SpecialDraw[A].type].Value, new Vector2(i * 16 + 16 * a, j * 16 + 16 * b) + new Vector2(16 * tileData.Width / 2 - 16 * a - 6 * Entity.direction, 16 * (tileData.Height - b) + 1) - Main.screenPosition, rectangle, color, (SpecialDraw[A].rotation / 2 + SpecialDraw[A].rotation2 / 2) * (0.8f + 0.2F * (tileData.Height - b)), new Vector2(16 * tileData.Width / 2 - 16 * a - 6 * Entity.direction, 16 * (tileData.Height - b) - 2), 1f, 0, 0f);
								}
							}
						}
						else
						{
							for (int a = 0; a < tileData.Width; a++)
							{
								for (int b = 0; b < tileData.Height; b++)
								{
									Rectangle? rectangle = new Rectangle?(new Rectangle(frameXPos + 18 * a, frameYPos + 18 * b, 16, height));
									Color color = Lighting.GetColor(i + a, j + b, Color.White);
									float R = (SpecialDraw[A].rotation + SpecialDraw[A].rotation2) * (0.8f + 1.6F * (tileData.Height - b)) / 3;
									float R2 = (SpecialDraw[A].rotation + SpecialDraw[A].rotation2) * (0.7f + 0.7F * (tileData.Height - b)) / 3;

                                    Main.spriteBatch.Draw(TextureAssets.Tile[SpecialDraw[A].type].Value, new Vector2(i * 16 + 16 * a, j * 16 + 16 * b + (SpecialDraw[A].Wind ? (34) : 32)) + new Vector2(16 * tileData.Width / 2 - 16 * a, 16 * (tileData.Height - b)-16)-new Vector2(0,16 * (tileData.Height - b)).RotatedBy(R2) - Main.screenPosition, rectangle, color, R, new Vector2(16 * tileData.Width / 2 - 16 * a, 14), 1f, 0, 0f);
									if(SpecialDraw[A].type == ModContent.TileType<地狱果树>())
                                    {
										Lighting.AddLight(new Vector2(i * 16 + 16 * a, j * 16 + 16 * b), new Color(238, 102, 70).ToVector3() * 0.4F);
										color = Color.White*0.75F;
										color.A = 0;
                                        Main.spriteBatch.Draw(地狱果树.Glow.Value, new Vector2(i * 16 + 16 * a, j * 16 + 16 * b + (SpecialDraw[A].Wind ? (34) : 32)) + new Vector2(16 * tileData.Width / 2 - 16 * a, 16 * (tileData.Height - b) - 16) - new Vector2(0, 16 * (tileData.Height - b)).RotatedBy(R2) - Main.screenPosition, rectangle, color, R, new Vector2(16 * tileData.Width / 2 - 16 * a, 14), 1f, 0, 0f);
                                    }
                                }
							}
						}
					}
					if (!tile.HasTile || tile.TileType != SpecialDraw[A].type)
					{
						SpecialDraw.Remove(SpecialDraw[A]);
					}
				}
			}
		}
		public override void PreUpdateTime()
		{
			MouseWorld=Main.MouseWorld;
			if (Main.netMode != 2)
			{
				if (SpecialDraw != null)
				{
					for (int A = 0; A < SpecialDraw.Count; A++)
					{
						SpecialDraw[A].Update();
					}
				}
			}
		}

		public static List<DDTileDawn> SpecialDraw = new List<DDTileDawn>();
		public static List<Point16> 家园塔Draw = new List<Point16>();
        public override void PostDrawTiles()
		{
			Player player = Main.player[Main.myPlayer];
			if (DDItemTextures.MouseTime<=0)
			{
				TextureAssets.Cursors[0] = DDItemTextures.Mouse[0];
				TextureAssets.Cursors[1] = DDItemTextures.Mouse[1];
				TextureAssets.Cursors[11] = DDItemTextures.Mouse[2];
				TextureAssets.Cursors[12] = DDItemTextures.Mouse[3];
				TextureAssets.CursorRadial = DDItemTextures.Mouse[4];
				TextureAssets.LockOnCursor = DDItemTextures.Mouse[5];
			}
            else
			{
				Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);
				Texture2D texture = DDTextures.收获.Value;
				Texture2D texture2 = DDTextures.收获边框.Value;
				Main.spriteBatch.Draw(texture2, new Vector2(Main.mouseX,Main.mouseY), null, Main.MouseBorderColor, 0f, texture2.Size()/2, Main.UIScale*0.6F, (SpriteEffects)(Main.LocalPlayer.direction == 1 ? 1 : 0), 0);
				Main.spriteBatch.Draw(texture2, new Vector2(Main.mouseX,Main.mouseY), null, Main.MouseBorderColor, 0f, texture2.Size()/2, Main.UIScale*0.6F, (SpriteEffects)(Main.LocalPlayer.direction == 1 ? 1 : 0), 0);
				Main.spriteBatch.Draw(texture, new Vector2(Main.mouseX,Main.mouseY), null, Main.mouseColor, 0f, texture.Size()/2, Main.UIScale*0.5F, (SpriteEffects)(Main.LocalPlayer.direction == 1 ? 1 : 0), 0);
				Main.spriteBatch.End();
				DDItemTextures.MouseTime--;

			}
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
			家园塔TE.ItemDraw();
			if (家园塔Draw!=null)
            {
				for(int A = 0;A< 家园塔Draw.Count;A++)
                {
					家园塔TE.Tile(Main.spriteBatch, 家园塔Draw[A]);
				} 
            }
			Main.spriteBatch.End();
			//温馨屋子
			return;
			for (int A = 0; A < DDWorld.土.Length; A++)
			{
				int i = DDWorld.土[A].tiles.X;
				int j = DDWorld.土[A].tiles.Y;
				Tile tile = Main.tile[i, j];
				if (锄土.FindFirstTile(new Point16(i, j), out int type) >= 0 && DDWorld.土[type].DampTime > 0)
				{
					int height = (tile.TileFrameY == 36) ? 18 : 16;
					Main.spriteBatch.Draw(锄过的土块.asset.Value, new Vector2(i * 16, j * 16)-Main.screenPosition, new Rectangle?(new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height)), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, 0, 0f);
				}
				else
                {
					int height = (tile.TileFrameY == 36) ? 18 : 16;
					Main.spriteBatch.Draw(TextureAssets.Tile[ModContent.TileType<锄过的土块>()].Value, new Vector2(i * 16, j * 16) - Main.screenPosition, new Rectangle?(new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height)), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, 0, 0f);
				}
			}
		}
	}
}