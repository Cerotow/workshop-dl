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
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Tiles.农场;

namespace DDmod.Content.Tiles.家园塔
{
	public class 家园塔TE : ModTileEntity
	{
		public int type;
		public Item[] items;
		public float Time = 0;
		public int Level = 1;
		public float Glow;
		public bool active;
		public int Mana;
		public int Mana2;
		public int ConsumptionMana;
		public int MaxMana;
		public 家园塔TE()
		{
			items = new Item []{ DDmod.NewItem.Clone(), DDmod.NewItem.Clone(), DDmod.NewItem.Clone() };
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
		public void Tiletype(Tile tile)
		{
			if(tile.TileType==ModContent.TileType<治疗塔>())
            {
				ConsumptionMana = 5;
			}

		}
		public void UIItem()
        {
		    Point16 point = Main.LocalPlayer.GetModPlayer<家园塔Player>().塔;

			if (point == Position)
			{
				if (Main.netMode != 2)
				{
					float Scale = 0.8F;
					Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/Tiles/家园塔/UI").Value;
					Vector2 vector = new Vector2(point.X, point.Y) * 16 - new Vector2(-20 * Scale, 86);
					float X = items.Length * (texture.Width * 1.1F * Scale) / 2;
					for (int a = 0; a < items.Length; a++)
					{
						Vector2 po = new Vector2((int)(vector.X + (texture.Width * 1.1F * Scale) * a - X), vector.Y);
						Vector2 vector2 = new Vector2(po.X + texture.Width / 2, po.Y + texture.Height / 2);
						if (new Rectangle((int)DDTileDawnSystem.MouseWorld.X, (int)DDTileDawnSystem.MouseWorld.Y, 1, 1).Intersects(new Rectangle((int)vector2.X - (int)(texture.Size().X * Scale) / 2, (int)vector2.Y - (int)(texture.Size().Y * Scale) / 2, (int)(texture.Size().X * Scale), (int)(texture.Size().Y * Scale))))
						{
							if (Main.LocalPlayer.itemAnimation == 0)
							{
								if ((Main.mouseLeft && !dian) || Main.mouseRight)
								{
									Click(ref items[a]);
									dian = true;
								}
								if (!Main.mouseLeft)
								{
									dian = false;
								}
							}
						}
					}
				}
			}
		}
		public override void Update()
		{
			UIItem();
			Tile tile = Main.tile[Position.X, Position.Y];
			Tiletype(tile);
			TileObjectData tileData = TileObjectData.GetTileData(tile.TileType, 0, 0);
			家园塔TE tepowerCellFactory = playerHelper.FindTileEntity2<家园塔TE>(Position.X, Position.Y, tileData.Width, tileData.Height, 18);
			int npcs = 0;
			Vector2 Po = new Vector2(Position.X, Position.Y) * 16;
			int PlayerID = Player.FindClosest(Po, 1, 1);
			Player player = Main.player[PlayerID];
			Vector2 vector = player.Center - Po;
			active = false;
			if (vector.Length() > 1600)
			{
				return;
			}
			MaxMana = 200;
			if (!DDSystem.BossSurvival)
			{
				for (int a = 0; a < 200; a++)
				{
					if (Main.npc[a].active && Main.npc[a].townNPC && (Main.npc[a].Center - Po).Length() < 1600)
					{
						npcs++;
						if (npcs >= 2)
						{
							active = true;
							break;
						}
					}
				}
			}
			if (Glow > 1)
			{
				Glow -= 0.05F;
			}
			else
			{
				Glow = 1;
			}
			if (!active)
			{
				return;
			}
			Time++;
			if (Time > 60&& player.statLife!=player.statLifeMax2)
			{
				float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 5f);
				Po += new Vector2(16f, offset - 4);
				vector = player.Center - Po;
				if (vector.Length() < 800 && Mana > 5)
				{
					Glow = 2;
					int A = NewProjectile(new EntitySource_TileEntity(tepowerCellFactory), Po, new Vector2(0, -4), ModContent.ProjectileType<ForestBullets>(), 0, 0, Main.myPlayer);
					Main.projectile[A].npcProj = true;
					Main.projectile[A].noDropItem = true;
					Mana -= ConsumptionMana;
					Time = 0;
				}
			}
			for (int a = 0; a < items.Length; a++)
			{
				if (items[a].type == 109 && MaxMana - Mana >= 100)
				{
					items[a].stack--;
					if (items[a].stack == 0)
					{
						items[a].SetDefaults(0);
					}
					Mana += 100;

					CombatText.NewText(new Rectangle((int)Position.X * 16 + 16, (int)Position.Y * 16, 1, 1), new Color(0, 50, 255, 150), "100", false, false);
				}
			}
		}
		bool dian;
		int ItemTime;
		public void Click(ref Item item)
		{
			//开启背包
			Main.playerInventory = true;
			if (Main.mouseRight)
			{
				//如果可以拿起物品
				if (Main.mouseRight && item.type > 0 && item.maxStack > 1)
				{
					if (Main.mouseItem.type == 0)
					{
						//触发放物品声音
						SoundEngine.PlaySound(new SoundStyle("DDmod/NoContent/Sounds/Items/背包"));
						if (item.stack > 1)
						{
							Main.mouseItem = item.Clone();
							Main.mouseItem.stack = 1;
							item.stack--;
						}
						else
						{
							Main.mouseItem = item.Clone();
							item = DDmod.NewItem.Clone();
							item.SetDefaults(0, true);
						}
					}
					else if (Main.mouseItem.type == item.type)
					{
						SoundEngine.PlaySound(new SoundStyle("DDmod/NoContent/Sounds/Items/点"));
						if (item.stack > 1)
						{
							Main.mouseItem.stack++;
							item.stack--;
						}
						else
						{
							Main.mouseItem.stack++;
							item = DDmod.NewItem.Clone();
							item.SetDefaults(0, true);
						}
					}
				}
				return;
			}
			//当鼠标没物品，框里有物品的时候
			if ((Main.mouseItem.type == 0 && item.type != 0))
			{
				//拿出物品
				Main.mouseItem = item.Clone();
				item = DDmod.NewItem.Clone();
				item.SetDefaults(0, true);
			}
			//当鼠标有物品，框里没物品的时候
			else if (Main.mouseItem.type != 0 && item.type == 0)
			{
				//放入物品
				item = Main.mouseItem.Clone();
				Main.mouseItem = DDmod.NewItem.Clone();
				Main.mouseItem.SetDefaults(0, true);

			}
			//当鼠标和框都有物品时
			else if (Main.mouseItem.type != 0 && item.type != 0)
			{
				//如果框里的物品和鼠标的相同
				if (Main.mouseItem.type == item.type)
				{
					//框里的物品数量加上鼠标物品数量
					item.stack += Main.mouseItem.stack;
					//如果框里物品数量大于数量上限
					if (item.stack > item.maxStack)
					{
						//计算鼠标物品数量，并将框内物品数量修改为数量上限
						var exceed = item.stack - item.maxStack;
						item.stack = item.maxStack;
						Main.mouseItem.stack = exceed;
					}
					//反之
					else
					{
						//清空鼠标物品
						Main.mouseItem = DDmod.NewItem.Clone();
					}
				}
				//如果可以放入物品也能拿出物品
				else
				{
					//交换框内物品和鼠标物品
					var tmp = Main.mouseItem.Clone();
					Main.mouseItem = item;
					item = tmp;
				}
			}
			//反之
			else
			{
				//中断函数
				return;
			}
			//触发放物品声音
			SoundEngine.PlaySound(new SoundStyle("DDmod/NoContent/Sounds/Items/背包"));

		}
		public static void Tile(SpriteBatch spriteBatch, Point16 point)
		{
			Tile tile = Main.tile[point.X, point.Y];
			TileObjectData tileData = TileObjectData.GetTileData(tile.TileType, 0, 0);
			if (tileData == null)
			{
				DDTileDawnSystem.家园塔Draw.Remove(point);
				return;
			}
			家园塔TE tepowerCellFactory = playerHelper.FindTileEntity2<家园塔TE>(point.X, point.Y, tileData.Width, tileData.Height, 18);
			if (tepowerCellFactory == null)
			{
				DDTileDawnSystem.家园塔Draw.Remove(point);
				return;
			}
			Texture2D texture = 治疗塔.GTexture.Value;
			Texture2D texture2 = DDTextures.MiniVoidStar.Value;
			Vector2 worldPos = point.ToWorldCoordinates(16f, 64f);

			float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 5f);
			Vector2 drawPos = worldPos - Main.screenPosition + new Vector2(0f, -66f) + new Vector2(0f, offset);
			spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, texture.Size() / 2, 1, 0, 0f);
			if (tepowerCellFactory.active && tepowerCellFactory.ConsumptionMana < tepowerCellFactory.Mana)
				spriteBatch.Draw(texture2, drawPos, null, new Color(100, 255, 100, 0), 0f, texture2.Size() / 2, 0.5F * tepowerCellFactory.Glow, 0, 0f);

			Lighting.AddLight(new Vector2(point.X + 1, point.Y - 1) * 16 + new Vector2(0f, 12 + offset), new Color(100, 255, 100, 0).ToVector3() / 2);
		}
		public static void ItemDraw()
        {
			Point16 point = Main.LocalPlayer.GetModPlayer<家园塔Player>().塔;
			Tile tile = Main.tile[point.X, point.Y];
			TileObjectData tileData = TileObjectData.GetTileData(tile.TileType, 0, 0);
			if(tileData==null)
			{
				return;
			}
			家园塔TE tepowerCellFactory = playerHelper.FindTileEntity2<家园塔TE>(point.X, point.Y, tileData.Width, tileData.Height, 18);
			if (tepowerCellFactory == null)
			{
				return;
			}
			if (point == Point16.Zero)
			{
				return;
			}
			float Scale = 0.8F;
			Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/Tiles/家园塔/UI").Value;
			//绘制物品框
			Vector2 vector = new Vector2(point.X, point.Y) * 16-new Vector2(-20*Scale,86);
			float X = tepowerCellFactory.items.Length * (texture.Width*1.1F * Scale) / 2;
			for (int a = 0; a < tepowerCellFactory.items.Length; a++)
			{
				Vector2 po = new Vector2((int)(vector.X + (texture.Width*1.1F * Scale) * a - X), vector.Y);
				DrawAdvBox(Main.spriteBatch, (int)po.X, (int)po.Y,
					Color.White, texture, Scale, Main.screenPosition);

				Vector2 vector2 = new Vector2(po.X + texture.Width / 2, po.Y + texture.Height / 2);
				if (new Rectangle((int)DDTileDawnSystem.MouseWorld.X, (int)DDTileDawnSystem.MouseWorld.Y, 1, 1).Intersects(new Rectangle((int)vector2.X - (int)(texture.Size().X * Scale) / 2, (int)vector2.Y - (int)(texture.Size().Y * Scale) / 2, (int)(texture.Size().X * Scale), (int)(texture.Size().Y * Scale))))
				{
					Main.LocalPlayer.mouseInterface = true;
				}
				DrawItem(Main.spriteBatch, tepowerCellFactory.items[a], po, Main.screenPosition);
			}
			if (tepowerCellFactory.Mana2 > tepowerCellFactory.Mana)
			{
				if (tepowerCellFactory.Mana2 - tepowerCellFactory.Mana > 1)
				{
					tepowerCellFactory.Mana2-=1+(tepowerCellFactory.Mana2 - tepowerCellFactory.Mana)/10;
				}
                else
                {
					tepowerCellFactory.Mana2 = tepowerCellFactory.Mana;

				}
			}
			else if (tepowerCellFactory.Mana2 < tepowerCellFactory.Mana)
			{
				if (tepowerCellFactory.Mana - tepowerCellFactory.Mana2 > 1)
				{
					tepowerCellFactory.Mana2+= 1 + (tepowerCellFactory.Mana - tepowerCellFactory.Mana2) / 10;
				}
				else
				{
					tepowerCellFactory.Mana2 = tepowerCellFactory.Mana;

				}
			}
			float quotient = ((float)tepowerCellFactory.Mana2 / tepowerCellFactory.MaxMana);
			quotient = Utils.Clamp(quotient, 0f, 1f);
			int C = (int)(DDTextures.Shield.Width() * quotient);
			int I = (int)(255 * (1 - quotient));
			int N = (int)(255 * quotient);
			Main.spriteBatch.Draw(DDTextures.ShieldValue.Value, new Vector2(point.X+1, point.Y)*16 - new Vector2(0, 20)-Main.screenPosition, new Rectangle?(new Rectangle(0, 0, C, DDTextures.Shield.Height())), new Color(I, N, 0), 0f, DDTextures.ShieldValue.Size() / 2, 1, 0, 0);
			Main.spriteBatch.Draw(DDTextures.Shield.Value, new Vector2(point.X + 1, point.Y)*16 - new Vector2(0, 20) - Main.screenPosition, null, Color.White, 0f, DDTextures.Shield.Size() / 2, 1, 0, 0);
			void DrawAdvBox(SpriteBatch sp, int x, int y, Color c, Texture2D img, float scale, Vector2 screenPosition)
			{
				var box = img;
				sp.Draw(box, new Vector2(x, y) + box.Size() / 2- screenPosition, null, c, 0, box.Size() / 2, scale, 0, 0);
			}
			void DrawItem(SpriteBatch spriteBatch, Item item, Vector2 ItemPo, Vector2 screenPosition)
			{
				if (item.type != 0)
				{
					Main.instance.LoadItem(item.type);
					Rectangle DrawRectangle = new Rectangle((int)ItemPo.X, (int)ItemPo.Y, (int)texture.Size().X, (int)texture.Size().Y);
					var frame = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(TextureAssets.Item[item.type].Value) : TextureAssets.Item[item.type].Frame(1, 1, 0, 0);
					var size = frame.Size();
					var texScale = 0.8f;
					if (DrawRectangle.Width > DrawRectangle.Height)
					{
						if (size.X > DrawRectangle.Width * 0.8F)
						{
							texScale *= (float)DrawRectangle.Width * 0.8F / size.X;
						}
					}
					else
					{
						if (size.Y > DrawRectangle.Height * 0.8F)
						{
							texScale *= (float)DrawRectangle.Height * 0.8F / size.Y;
						}
					}
					//绘制物品贴图
					Vector2 vector = new Vector2(DrawRectangle.X + DrawRectangle.Width / 2, DrawRectangle.Y + DrawRectangle.Height / 2);
					Main.spriteBatch.Draw(TextureAssets.Item[item.type].Value, vector - screenPosition, new Rectangle?(frame), Color.White, 0, size / 2, texScale * Scale, 0, 0);
					//绘制物品左下角那个代表数量的数字
					if (item.stack > 0)
					{
						Main.spriteBatch.DrawString(FontAssets.MouseText.Value, item.stack.ToString(), vector + new Vector2(0, 4) * Scale - screenPosition, Color.White, 0f, Vector2.Zero, Scale * 0.75F, SpriteEffects.None, 0f);
					}
					//调用原版的介绍绘制
					if (new Rectangle((int)DDTileDawnSystem.MouseWorld.X, (int)DDTileDawnSystem.MouseWorld.Y,1,1).Intersects(new Rectangle((int)vector.X- (int)(texture.Size().X * Scale) / 2, (int)vector.Y- (int)(texture.Size().Y * Scale )/ 2, (int)(texture.Size().X * Scale), (int)(texture.Size().Y * Scale))) )
					{
						DDTileDawnSystem.itemText = item;
					}
				}
			}
		}
		public override void OnKill()
		{
		}
		public override void SaveData(TagCompound tag)
		{
			tag.Add("家园塔Items", items);
			tag.Add("家园塔Mana", Mana);

		}
		public override void LoadData(TagCompound tag)
		{
			if (tag.Get<Item[]>("家园塔Items").Length>0)
			{
				items = tag.Get<Item[]>("家园塔Items");
			}
			Mana = tag.Get<int>("家园塔Mana");
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
			for (int a = 0; a < items.Length; a++)
			{
				WriteItem(items[a], writer);
			}
			writer.Write(Time);
		}
		public override void NetReceive(BinaryReader reader)
		{
			for (int a = 0; a < items.Length; a++)
			{
				ReadItem(items[a], reader);
			}
			float T = reader.ReadFloat();
			if (T < 0)
				Time = T;

        }
    }
    public class 家园塔Player : ModPlayer
    {
		public Point16 塔;
        public override void PreUpdate()
		{
			Tile tile = Main.tile[塔.X, 塔.Y];
			TileObjectData tileData = TileObjectData.GetTileData(tile.TileType, 0, 0);
			if (tileData==null)
            {
				塔 = Point16.Zero;
				return;
			}
			家园塔TE tepowerCellFactory = playerHelper.FindTileEntity2<家园塔TE>(塔.X, 塔.Y, tileData.Width, tileData.Height, 18);
			if ((new Vector2(塔.X,塔.Y)-Player.Center/16).Length()>Player.tileRangeX+2||tepowerCellFactory==null)
            {
				塔 = Point16.Zero;
            }
        }
    }

}
