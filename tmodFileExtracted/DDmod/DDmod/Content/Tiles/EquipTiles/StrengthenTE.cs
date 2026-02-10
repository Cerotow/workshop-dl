using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ObjectData;
using Terraria.ModLoader.Default;
using DDmod.UI.ItemUI;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items;
using DDmod.NoContent.Config;

namespace DDmod.Content.Tiles.EquipTiles
{
	public class StrengthenTE : ModTileEntity
	{
		public Item items;
		public Item[] FortifiedStone;
		public byte t = 4;
		public bool t2;
		public bool Start;
		public float Revolve;
		public byte Distance = 50;
		public byte RevolveTime = 0;
		public int Level = 1;
		public int SyntheticEffects = -1;
		public int SyntheticEffects2 = -1;
		public StrengthenTE()
		{
			items = DDmod.NewItem.Clone();
			FortifiedStone = new Item[] { DDmod.NewItem.Clone(), DDmod.NewItem.Clone(), DDmod.NewItem.Clone() };
		}
		public override bool IsTileValidForEntity(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile.HasTile && tile.TileFrameX == 0 && tile.TileFrameY == 0;
		}
		public Vector2 Center
		{
			get
			{
				return Utils.ToWorldCoordinates(Position, 32f, 32f);
			}
		}
		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
		{
			TileObjectData tileData = TileObjectData.GetTileData(type, style, 0);
			Tile t = Main.tile[i, j];
			i -= (int)t.TileFrameX % (tileData.Width * 16) / 16;
			j -= (int)t.TileFrameY % (tileData.Height * 16) / 16;
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, tileData.Width, tileData.Height, 0);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, (float)j, (float)base.Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i, j);
		}
		public override void OnNetPlace()
		{
			//NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
		}
		public override void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
		{
		}
		public class EnchantmentItem
		{
			/// <summary>
			/// 附魔种类
			/// </summary>
			public int EnchantmentItemType;
			/// <summary>
			/// 最小值
			/// </summary>
			public int Min;
			/// <summary>
			/// 最大值
			/// </summary>
			public int Max;
			/// <summary>
			/// 概率
			/// </summary>
			public int MinRand;
			/// <summary>
			/// 最大概率
			/// </summary>
			public int MaxRand;
			public EnchantmentItem(int ET, int Min, int Max, int MinRand, int MaxRand)
			{
				this.EnchantmentItemType = ET;
				this.Min = Min;
				this.Max = Max;
				this.MinRand = MinRand;
				this.MaxRand = MaxRand;
			}
		}
		public override void Update()
		{
            if (Start)
			{
				if (t < 150)
				{
					t += (byte)DDConfigServer.Instance.Strengthen;
                    items.position = new Vector2(Position.X, Position.Y) * 16 - new Vector2(-24, (float)t / 5);
				}
				else
                {
                    if (Distance <= 0)
					{
						Distance = 0;
						if (RevolveTime == 0)
						{
							if (Recipe() == 0)
							{
								if(items.type != 0 && items.IsArmor())
								{
									List<EnchantmentItem> Enchantments = new List<EnchantmentItem>();
									bool B = false;
									int T = 0;
									for (int a = 0; a < 3; a++)
									{
										if (FortifiedStone[a].type != 0)
										{
											for (int E = 0; E < 2; E++)
											{
												Enchantments.Add(new EnchantmentItem(FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().EnchantmentItemType[E], FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().MinE[E], FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().MaxE[E], T, T += (FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().Enchantment[E]-1)));
												T++;
											}
											if (FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().EnchantmentItemType[0] > 0||FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().EnchantmentItemType[1] > 0)
											{
												FortifiedStone[a].SetDefaults(0);
											}
											else
											{
												int num2 = Item.NewItem(new EntitySource_TileEntity(this), items.position, FortifiedStone[a].type);
												Main.item[num2] = FortifiedStone[a].Clone();
												Main.item[num2].position = FortifiedStone[a].position;
												Main.item[num2].velocity = new Vector2(0, -10).RotatedBy(0.8F * (a - 1));
												Main.item[num2].newAndShiny = false;
												FortifiedStone[a].SetDefaults(0);
											}
										}
									}
									int R = Main.rand.Next(100);
									for (int a = 0; a < Enchantments.Count; a++)
									{
										if (R>=Enchantments[a].MinRand&& R<= Enchantments[a].MaxRand)
										{
											items.GetGlobalItem<StrengthenGlobalItem>().EnchantmentType = Enchantments[a].EnchantmentItemType;

                                            if (Enchantments[a].Max == 1)
                                            {
												items.GetGlobalItem<StrengthenGlobalItem>().Value = 0;
											}
											else
                                            {
												items.GetGlobalItem<StrengthenGlobalItem>().Value = Main.rand.Next(Enchantments[a].Min, Enchantments[a].Max);
												if(items.GetGlobalItem<StrengthenGlobalItem>().Value==0)
                                                {
                                                    break;
                                                }
                                            }
                                            B = true;
											break;
										}
									}
									if (B)
									{
										SyntheticEffects = 6;
										SyntheticEffects2 = 6;
									}
									else
									{
										SyntheticEffects = 5;
										SyntheticEffects2 = 5;
									}
								}
								else
								if (items.type != 0 && items.damage > 0)
								{
									int Rand = 0;
									for (int a = 0; a < 3; a++)
									{
										if (FortifiedStone[a].type != 0)
										{
											if (items.GetGlobalItem<StrengthenGlobalItem>().Level >= 0)
											{
												Rand += FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().Probability[items.GetGlobalItem<StrengthenGlobalItem>().Level];
												if (FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().Probability[items.GetGlobalItem<StrengthenGlobalItem>().Level] > 0)
												{
													FortifiedStone[a].SetDefaults(0);
												}
												else
												{
													int num2 = Item.NewItem(new EntitySource_TileEntity(this), items.position, FortifiedStone[a].type);
													Main.item[num2] = FortifiedStone[a].Clone();
													Main.item[num2].position = FortifiedStone[a].position;
													Main.item[num2].velocity = new Vector2(0, -10).RotatedBy(0.8F * (a - 1));
													Main.item[num2].newAndShiny = false;
													FortifiedStone[a].SetDefaults(0);
												}
											}
											else
                                            {
                                                if (FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().Probability[0] > 0)
                                                {
                                                    Rand += 100;
                                                    FortifiedStone[a].SetDefaults(0);
                                                }
                                                else
                                                {
                                                    int num2 = Item.NewItem(new EntitySource_TileEntity(this), items.position, FortifiedStone[a].type);
                                                    Main.item[num2] = FortifiedStone[a].Clone();
                                                    Main.item[num2].position = FortifiedStone[a].position;
                                                    Main.item[num2].velocity = new Vector2(0, -10).RotatedBy(0.8F * (a - 1));
                                                    Main.item[num2].newAndShiny = false;
                                                    FortifiedStone[a].SetDefaults(0);
                                                }

                                            }
										}
									}
									if (Main.rand.Next(100) < Rand)
									{
										SyntheticEffects = 0;
										SyntheticEffects2 = 0;
										items.GetGlobalItem<StrengthenGlobalItem>().Strengthen(items, false);
									}
									else
									{
										SyntheticEffects = 5;
										SyntheticEffects2 = 5;
									}
								}
                                else
                                {
									for (int a = 0; a < 3; a++)
									{
										int num2 = Item.NewItem(new EntitySource_TileEntity(this), items.position, FortifiedStone[a].type);
										Main.item[num2] = FortifiedStone[a].Clone();
										Main.item[num2].position = FortifiedStone[a].position;
										Main.item[num2].velocity = new Vector2(0, -10).RotatedBy(0.8F * (a - 1));
										Main.item[num2].newAndShiny = false;
										FortifiedStone[a].SetDefaults(0);

									}
								}
							}
                        }
                        RevolveTime += (byte)DDConfigServer.Instance.Strengthen;
                        if (RevolveTime > 60)
						{
							Start = false;
                        }
                    }
					else
					{
						Distance -= (byte)DDConfigServer.Instance.Strengthen;
					}
				}
				Revolve += 0.2f* DDConfigServer.Instance.Strengthen;
				for (int a = 0; a < 3; a++)
				{
					Vector2 vector = items.position + new Vector2(Distance, 0).RotatedBy(MathHelper.TwoPi / 3 * a + Revolve) * new Vector2(1.8f, 1F) - FortifiedStone[a].position;
					float r = vector.Length();
					if (r < 3)
					{
						r = 3;
					}
					else if (r > 10)
					{
						r = 10;
					}
					FortifiedStone[a].velocity = (FortifiedStone[a].velocity * 0 + vector.PerfectNormalize() * r) / 1;
					FortifiedStone[a].velocity.Y *= 0.95F;
					FortifiedStone[a].position += FortifiedStone[a].velocity;
				}
			}
			else
			{
				RevolveTime = 0;
				if (t < 50)
				{
					DDHelper.BackAndForthInt(0, 50, 1, ref t, ref t2);
				}
				else
				{
					t -=2;
                }
				Distance = 50;

				items.position = new Vector2(Position.X, Position.Y) * 16 - new Vector2(-24, (float)t/5);
				for (int a = 0; a < 3; a++)
				{
					Vector2 vector = new Vector2(-24 + (a - 1) * 34, 38 + (float)t / 5);
					if (a == 0)
					{
						vector.X += 34;
					}
					if (a == 1)
					{
						vector.X += 34;
						vector.Y -= 20;
					}
					if (a == 2)
					{
						vector.X -= 68;
						vector.Y -= 20;
					}
					FortifiedStone[a].position = new Vector2(Position.X, Position.Y) * 16 - vector;
					FortifiedStone[a].velocity = Vector2.Zero;
				}
			}
			if (Main.netMode == NetmodeID.Server&& (Start))
			{
				NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
			}
			if (Main.netMode != 1)
			{
				if (SyntheticEffects2 != -1)
				{
					NewProjectile(new EntitySource_TileEntity(this), items.position - new Vector2(0, 10), Vector2.Zero, ModContent.ProjectileType<LightNight>(), 0, 0, -1, SyntheticEffects2, 0);
					SyntheticEffects2 = -1;
				}
			}
		}
		public int Recipe()
		{
			//永夜之刃
			if (RecipesSystem.SpecialMainMaterial[items.type])
			{
				RecipesSystem.Material.TryGetValue(items.type, out Item[] item);
				bool A = false;
				bool B = false;
				bool C = false;
				for (int a = 0; a < 3; a++)
				{
					if (FortifiedStone[a].type == item[0].type && FortifiedStone[a].stack >= item[0].stack)
					{
						A = true;
					}
					if (FortifiedStone[a].type == item[1].type && FortifiedStone[a].stack >= item[1].stack)
					{
						B = true;
					}
					if (FortifiedStone[a].type == item[2].type && FortifiedStone[a].stack >= item[2].stack)
					{
						C = true;
					}
				}
				if (A && B && C)
				{
					SyntheticEffects = RecipesSystem.SyntheticEffects[items.type];
					SyntheticEffects2 = RecipesSystem.SyntheticEffects[items.type];
					items.SetDefaults(item[3].type);
					for (int a = 0; a < 3; a++)
					{
						if (FortifiedStone[a].type == item[0].type)
						{
							FortifiedStone[a].stack -= item[0].stack;
						}
						if (FortifiedStone[a].type == item[1].type)
						{
							FortifiedStone[a].stack -= item[1].stack;
						}
						if (FortifiedStone[a].type == item[2].type)
						{
							FortifiedStone[a].stack -= item[2].stack;
						}
						if (FortifiedStone[a].stack > 0 && FortifiedStone[a].maxStack > 1)
						{
							int num2 = Item.NewItem(new EntitySource_TileEntity(this), items.position, FortifiedStone[a].type);
							Main.item[num2] = FortifiedStone[a].Clone();
							Main.item[num2].position = FortifiedStone[a].position;
							Main.item[num2].velocity = new Vector2(0, -10).RotatedBy(0.8F * (a - 1));
							Main.item[num2].newAndShiny = false;
						}
						FortifiedStone[a].SetDefaults(0);
					}
					if (SyntheticEffects == 4)
					{
						Level = 2;
					}
					return item[3].type;
				}
				else
				{
					return 0;
				}
			}

			return 0;
		}
		public override void OnKill()
		{
		}
		public override void SaveData(TagCompound tag)
		{
			tag.Add("TalismanItems", items);
			tag.Add("FortifiedStone", FortifiedStone);

		}
		public override void LoadData(TagCompound tag)
		{
			items = tag.Get<Item>("TalismanItems");
			if (tag.Get<Item[]>("FortifiedStone").Length==3)
			{
				FortifiedStone = tag.Get<Item[]>("FortifiedStone");
			}
		}
		public void WriteItem(Item item, BinaryWriter writer)
		{
			writer.WriteVector2(item.position);
			if (!ModNet.AllowVanillaClients)
			{
				ItemIO.Send(item, writer, writeStack: true);
				return;
			}

			writer.Write((ushort)item.netID);
			writer.Write((ushort)item.stack);
			writer.Write(item.prefix);
		}

		public void ReadItem(Item item, BinaryReader reader)
		{
			item.position = reader.ReadVector2();
			if (!ModNet.AllowVanillaClients)
			{
				ItemIO.Receive(item, reader, readStack: true);
				return;
			}

			int defaults = reader.ReadUInt16();
			int stack = reader.ReadUInt16();
			int pre = reader.ReadByte();

			item.SetDefaults(defaults);
			item.stack = stack;
			item.Prefix(pre);
		}
		public override void NetSend(BinaryWriter writer)
		{
			WriteItem(items, writer);
		    WriteItem(FortifiedStone[0], writer);
		    WriteItem(FortifiedStone[1], writer);
		    WriteItem(FortifiedStone[2], writer);
			writer.Write(t);
			writer.Write(t2);
			writer.Write(Revolve);
			writer.Write(Distance);
			writer.Write(RevolveTime);
			writer.Write(Level);
			writer.Write(SyntheticEffects);
		}
		public override void NetReceive(BinaryReader reader)
		{
			ReadItem(items, reader);
		    ReadItem(FortifiedStone[0], reader);
		    ReadItem(FortifiedStone[1], reader);
		    ReadItem(FortifiedStone[2], reader);
			t = reader.ReadByte();
			t2 = reader.ReadBoolean();
			Revolve = reader.ReadFloat();
			Distance = reader.ReadByte();
			RevolveTime = reader.ReadByte();
            Level = reader.ReadInt32();
			SyntheticEffects = reader.ReadInt32();

		}
	}
}
