using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using ReLogic.Graphics;
using System.Media;
using Terraria.Audio;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using DDmod.Content.Tiles.EquipTiles;
using Terraria.ID;
using static AssGen.Assets;
using DDmod.UI.ItemUI.背包;

namespace DDmod.UI.ItemUI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mouseItem"></param>
    /// <returns></returns>
    public delegate bool CheckPutSlotCondition(Item mouseItem);
    public delegate void ExchangeItemHandler(UIElement target);
    public class StrengthenUI : UIElement
    {
        /// <summary>
        /// 框贴图
        /// </summary>
        public Asset<Texture2D> SlotBackTexture { get; set; }
        /// <summary>
        /// 是否可以放置物品
        /// </summary>
        public CheckPutSlotCondition CanPutInSlot { get; set; }
        /// <summary>
        /// 是否可以拿去物品
        /// </summary>
        public CheckPutSlotCondition CanTakeOutSlot { get; set; }
        /// <summary>
        /// 框的绘制的拐角尺寸
        /// </summary>
        public Vector2 CornerSize { get; set; }
        /// <summary>
        /// 绘制颜色
        /// </summary>
        public Color DrawColor { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Tooltip { get; set; }
        /// <summary>
        /// 更改物品时调用
        /// </summary>
        public event ExchangeItemHandler PostExchangeItem;
        /// <summary>
        /// 玩家拿取物品时调用
        /// </summary>
        public event ExchangeItemHandler OnPickItem;
        /// <summary>
        /// 大小
        /// </summary>
        public float Scale { get; set; }
        /// <summary>
        /// 透明度
        /// </summary>
        public float Opacity { get; set; }
        public int type { get; set; }
        public Item items
        {
            get
            {
                TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
                StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(StrengtheningUI.Tile.X, StrengtheningUI.Tile.Y, tileData.Width, tileData.Height, 18);
                if (type >= 1&& type <=3)
                {
                    return tepowerCellFactory.FortifiedStone[type-1];
                }
                else
                {
                    return tepowerCellFactory.items;
                }
            }
            set
            {
                TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
                StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(StrengtheningUI.Tile.X, StrengtheningUI.Tile.Y, tileData.Width, tileData.Height, 18);
                if (type >= 1 && type <= 3)
                {
                    tepowerCellFactory.FortifiedStone[type - 1] = value;
                }
                else
                {
                    tepowerCellFactory.items = value;
                }
            }
        }
        public StrengthenUI(Asset<Texture2D> texture,int type) : base()
        {
            Scale = 1f;
            Opacity = 1f;
            //items = new Item();
            CanPutInSlot = null;
            SlotBackTexture = texture;
            DrawColor = new Color(0x3f, 0x41, 0x97) * 0.75f;
            CornerSize = new Vector2(10, 10);
            Tooltip = "";
            this.type = type;
        }

        public override void Update(GameTime gameTime)
        {
            Time--;
            if (ContainsPoint(Main.MouseScreen)&&Main.LocalPlayer.itemAnimation==0)
            {
                if (ItemSlot.ShiftInUse)
                {
                    Main.cursorOverride = 8;
                }
                if (Main.mouseLeft && !dian)
                {
                    if (ItemSlot.ShiftInUse && BackpackStrengtheningUI.CanTakeItem(items, Main.LocalPlayer.inventory, out int slot))
                    {
                        Click(Main.LocalPlayer.inventory[slot], slot);
                    }
                    else
                    {
                        Click();
                    }
                    //Click();
                    dian = true;
                }
                if (!Main.mouseLeft)
                {
                    dian = false;
                }
                Main.LocalPlayer.mouseInterface = true;
            }
            if (type == 0)
            {
                Left.Set(80, 0f);
                Top.Set(320, 0f);
                Width.Set(50, 0);
                Height.Set(50, 0);
            }
            if (type == 1)
            {
                Left.Set(86, 0f);
                Top.Set(282, 0f);
                Width.Set(38, 0);
                Height.Set(38, 0);
            }
            if (type == 2)
            {
                Left.Set(48, 0f);
                Top.Set(286, 0f);
                Width.Set(38, 0);
                Height.Set(38, 0);
            }
            if (type == 3)
            {
                Left.Set(122, 0f);
                Top.Set(286, 0f);
                Width.Set(38, 0);
                Height.Set(38, 0);
            }
            base.Update(gameTime);

        }
        bool dian;

        public static void WriteItem(Item item, BinaryWriter writer)
        {
            ItemIO.Send(item, writer, writeStack: true);
        }
        public static void ReadItem(Item item, BinaryReader reader)
        {
            ItemIO.Receive(item, reader, readStack: true);
        }
        int Time;
        public void Click(Item item = null, int slot = -1)
        {
            /// <summary>
            /// 框内物品
            /// </summary>
			TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
            StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(StrengtheningUI.Tile.X, StrengtheningUI.Tile.Y, tileData.Width, tileData.Height, 18);

            bool R = false;
            if (item == null && slot < 0)
            {
                R = true;
                item = Main.mouseItem;
            }
            //当鼠标没物品，框里有物品的时候
            if (item.type == 0 && items.type != 0)
            {
                //如果可以拿起物品
                if (CanTakeOutSlot == null || CanTakeOutSlot(items))
                {
                    //拿出物品
                    item = items.Clone();
                    items.TurnToAir(true);

                    if (slot >= 0)
                    {
                        Main.LocalPlayer.inventory[slot] = item;
                    }
                    //调用委托
                    OnPickItem?.Invoke(this);
                }
            }
            //当鼠标有物品，框里没物品的时候
            else if (item.type != 0 && items.type == 0)
            {
                //如果可以放入物品
                if (CanPutInSlot == null || CanPutInSlot(item))
                {
                    //放入物品
                    if (item.GetGlobalItem<StrengthenGlobalItem>().Probability[0] > 0 || (item.GetGlobalItem<StrengthenGlobalItem>().EnchantmentItemType[0] > 0 && tepowerCellFactory.items.damage <= 0))
                    {
                        items = item.Clone();
                        items.stack = 1;
                        item.stack--;
                    }
                    else
                    {
                        items = item.Clone();
                        item.TurnToAir(true);
                    }
                }
            }
            //当鼠标和框都有物品时
            else if (item.type != 0 && items.type != 0)
            {
                //如果不能放入物品
                if (!(CanPutInSlot == null || CanPutInSlot(item)))
                {
                    //中断函数
                    return;
                }
                if ((item.GetGlobalItem<StrengthenGlobalItem>().Probability[0] > 0 || item.GetGlobalItem<StrengthenGlobalItem>().EnchantmentItemType[0] > 0) && (item.type == items.type || item.stack > 1))
                {
                    if (slot >= 0)
                    {
                        //如果框里的物品和鼠标的相同
                        if (item.type == items.type)
                        {
                            item.stack += items.stack;
                            //如果框里物品数量大于数量上限
                            if (item.stack > item.maxStack)
                            {
                                //计算鼠标物品数量，并将框内物品数量修改为数量上限
                                var exceed = item.stack - item.maxStack;
                                item.stack = item.maxStack;
                                items.stack = exceed;
                            }
                            //反之
                            else
                            {
                                //清空鼠标物品
                                items.TurnToAir(true);
                            }
                        }
                    }
                    else
                    {
                        //如果框里的物品和鼠标的相同
                        if (item.type == items.type)
                        {
                            item.stack += items.stack;
                            //如果框里物品数量大于数量上限
                            if (item.stack > item.maxStack)
                            {
                                //计算鼠标物品数量，并将框内物品数量修改为数量上限
                                var exceed = item.stack - item.maxStack;
                                item.stack = item.maxStack;
                                items.stack = exceed;
                            }
                            //反之
                            else
                            {
                                //清空鼠标物品
                                items.TurnToAir(true);
                            }
                        }
                    }
                }
                else
                //如果框里的物品和鼠标的相同
                if (item.type == items.type)
                {
                    if (slot >= 0)
                    {
                        //框里的物品数量加上鼠标物品数量
                        item.stack += items.stack;
                        //如果框里物品数量大于数量上限
                        if (item.stack > item.maxStack)
                        {
                            //计算鼠标物品数量，并将框内物品数量修改为数量上限
                            var exceed = item.stack - item.maxStack;
                            item.stack = item.maxStack;
                            items.stack = exceed;
                        }
                        //反之
                        else
                        {
                            //清空鼠标物品
                            items.TurnToAir(true);
                        }
                    }
                    else
                    {

                        //框里的物品数量加上鼠标物品数量
                        items.stack += item.stack;
                        //如果框里物品数量大于数量上限
                        if (items.stack > items.maxStack)
                        {
                            //计算鼠标物品数量，并将框内物品数量修改为数量上限
                            var exceed = items.stack - items.maxStack;
                            items.stack = items.maxStack;
                            item.stack = exceed;
                        }
                        //反之
                        else
                        {
                            //清空鼠标物品
                            item.TurnToAir(true);
                        }
                    }
                }
                //如果可以放入物品也能拿出物品
                else if ((CanPutInSlot == null || CanPutInSlot(item))
                    && (CanTakeOutSlot == null || CanTakeOutSlot(items)))
                {
                    //交换框内物品和鼠标物品
                    var tmp = item.Clone();
                    item = items;
                    items = tmp;
                    if (slot >= 0)
                    {
                        Main.LocalPlayer.inventory[slot] = item;
                    }
                }
            }
            //反之
            else
            {
                //中断函数
                return;
            }
            if (R)
            {
                Main.mouseItem = item;
            }

            //触发放物品声音
            SoundEngine.PlaySound(new SoundStyle("DDmod/NoContent/Sounds/Items/背包"));
            Write();
            //调用委托
            PostExchangeItem?.Invoke(this);
        }
        public void Write()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = DDmod.Instance.GetPacket(256);
                //写入要发的包
                packet.Write((byte)DDType.TalismanTE);
                packet.WriteVector2(new Vector2(StrengtheningUI.Tile.X, StrengtheningUI.Tile.Y));
                packet.Write((byte)type);
                WriteItem(items, packet);
                //发出去
                packet.Send(-1, Main.myPlayer);
            }
        }
        public static void ReadTalismanTE(Mod mod, BinaryReader reader)
        {
            Vector2 point = reader.ReadVector2();
            byte type = reader.ReadByte();
            TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
            StrengthenTE tepowerCellFactory = (StrengthenTE)playerHelper.FindTileEntity((int)point.X, (int)point.Y, tileData.Width, tileData.Height, 18);
            if (type == 0)
            {
                ReadItem(tepowerCellFactory.items, reader);
                Send(tepowerCellFactory.items);
            }
            else if (type == 1)
            {
                ReadItem(tepowerCellFactory.FortifiedStone[0], reader);
                Send(tepowerCellFactory.FortifiedStone[0]);
            }
            else if (type == 2)
            {
                ReadItem(tepowerCellFactory.FortifiedStone[1], reader);
                Send(tepowerCellFactory.FortifiedStone[1]);
            }
            else if (type == 3)
            {
                ReadItem(tepowerCellFactory.FortifiedStone[2], reader);
                Send(tepowerCellFactory.FortifiedStone[2]);
            }
            void Send(Item items)
            {
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(MessageID.TileEntitySharing, number: tepowerCellFactory.ID, number2: tepowerCellFactory.Position.X, number3: tepowerCellFactory.Position.Y);
                    ModPacket packet = DDmod.Instance.GetPacket(256);
                    //写入要发的包
                    packet.Write((byte)DDType.TalismanTE);
                    packet.WriteVector2(new Vector2(point.X, point.Y));
                    packet.Write(type);
                    WriteItem(items, packet);
                    //发出去
                    packet.Send(-1, Main.myPlayer);
                }
            }
        }
        public static void ReadTalismanTE2(Mod mod, BinaryReader reader)
        {
            Vector2 point = reader.ReadVector2();
            TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
            StrengthenTE tepowerCellFactory = (StrengthenTE)playerHelper.FindTileEntity((int)point.X, (int)point.Y, tileData.Width, tileData.Height, 18);
            tepowerCellFactory.Start = reader.ReadBoolean();
            tepowerCellFactory.SyntheticEffects = reader.ReadInt32();
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            /// <summary>
            /// 框内物品
            /// </summary>
			TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
            StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(StrengtheningUI.Tile.X, StrengtheningUI.Tile.Y, tileData.Width, tileData.Height, 18);

            //调用原版的介绍绘制
            if (ContainsPoint(Main.MouseScreen) && items.type != 0)
            {
                Main.hoverItemName = items.Name;
                Main.HoverItem = items.Clone();
            }
            //获取当前UI部件的信息
            var DrawRectangle = GetDimensions();
            //绘制物品框
            DrawAdvBox(sb, (int)DrawRectangle.X, (int)DrawRectangle.Y,
                (int)DrawRectangle.Width, (int)DrawRectangle.Height,
                Color.White, SlotBackTexture.Value, CornerSize, Scale);

            if (items.type != 0)
            {
                var frame = Main.itemAnimations[items.type] != null ? Main.itemAnimations[items.type].GetFrame(TextureAssets.Item[items.type].Value) : TextureAssets.Item[items.type].Frame(1, 1, 0, 0);
                var size = frame.Size();
                var texScale = 1f;
                if (type == 0)
                {
                }
                if (size.X > DrawRectangle.Width*0.4f || size.Y > DrawRectangle.Height * 0.4f)
                {
                    texScale = size.X > size.Y ? size.X / DrawRectangle.Width : size.Y / DrawRectangle.Height;
                    texScale = 0.7f / texScale;
                    size *= texScale;
                }
                //绘制物品贴图
                sb.Draw(TextureAssets.Item[items.type].Value, new Vector2(DrawRectangle.X + DrawRectangle.Width / 2 - (size.X) / 2,
                    DrawRectangle.Y + DrawRectangle.Height / 2 - (size.Y) / 2), new Rectangle?(frame), Color.White * Opacity, 0, Vector2.Zero, texScale * Scale, 0, 0);
                //绘制物品左下角那个代表数量的数字
                if (items.stack > 1)
                {
                    sb.DrawString(FontAssets.MouseText.Value, items.stack.ToString(), new Vector2(DrawRectangle.X + 10, DrawRectangle.Y + DrawRectangle.Height - 20), Color.White * Opacity, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                }
            }
        }
        /// <summary>
        /// 绘制物品框
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="c"></param>
        /// <param name="img"></param>
        /// <param name="size4"></param>
        /// <param name="scale"></param>
        public void DrawAdvBox(SpriteBatch sp, int x, int y, int w, int h, Color c, Texture2D img, Vector2 size4, float scale = 1f)
        {
            var box = img;
            var nw = (int)(w * scale);
            var nh = (int)(h * scale);
            x += (w - nw) / 2;
            y += (h - nh) / 2;
            w = nw;
            h = nh;
            var width = (int)size4.X;
            var height = (int)size4.Y;
            if (w < size4.X)
            {
                w = width;
            }
            if (h < size4.Y)
            {
                h = width;
            }
            sp.Draw(box, new Rectangle(x, y, width, height), new Rectangle(0, 0, width, height), c);
            sp.Draw(box, new Rectangle(x + width, y, w - width * 2, height), new Rectangle(width, 0, box.Width - width * 2, height), c);
            sp.Draw(box, new Rectangle((x + w) - width, y, width, height), new Rectangle(box.Width - width, 0, width, height), c);
            sp.Draw(box, new Rectangle(x, y + height, width, h - height * 2), new Rectangle(0, height, width, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle(x + width, y + height, w - width * 2, h - height * 2), new Rectangle(width, height, box.Width - width * 2, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle((x + w) - width, y + height, width, h - height * 2), new Rectangle(box.Width - width, height, width, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle(x, (y + h) - height, width, height), new Rectangle(0, box.Height - height, width, height), c);
            sp.Draw(box, new Rectangle(x + width, (y + h) - height, w - width * 2, height), new Rectangle(width, box.Height - height, box.Width - width * 2, height), c);
            sp.Draw(box, new Rectangle((x + w) - width, (y + h) - height, width, height), new Rectangle(box.Width - width, box.Height - height, width, height), c);
        }
    }
}
