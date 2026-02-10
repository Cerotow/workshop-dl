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
using System.Collections.ObjectModel;
using Terraria.GameInput;

namespace DDmod.UI.ItemUI.背包
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mouseItem"></param>
    /// <returns></returns>
    public delegate bool CheckPutSlotCondition(Item mouseItem);
    public delegate void ExchangeItemHandler(UIElement target);
    public class BackpackUIItem : GlobalItem
    {
        /// <summary>
        /// 框内物品
        public override bool InstancePerEntity => true;
        public Item[] ContainedItem = new Item[40];
        public int ContainedItems = 0;
        /// <summary>
        /// 抽奖战利品
        /// </summary>
        public int[] LootProbability = new int[5];
        public Dictionary<int, Item[]> Loot = new Dictionary<int, Item[]>();
        public int RandNext(int[] I)
        {
            int Probability = 0;
            for(int A= 0;A< I.Length;A++)
            {
                Probability += I[A];
            }
            int R = new Random().Next(Probability);

            int P = 0;
            int L = 0;
            for (int A = 0; A < I.Length; A++)
            {
                P += I[A];
                if (R < P)
                {
                    return L;
                }
                L++;
            }
            return -1;
        }
        public override void SetDefaults(Item item)
        {
            for (int a = 0; a < ContainedItem.Length; a++)
            {
                ContainedItem[a] = DDmod.NewItem.Clone();
            }
        }
        int CD = 10;
        public override bool CanRightClick(Item item)
        {
            if (ContainedItems > 0 && CD <= 0)
            {
                int a = 0;
                foreach (Item i in Main.LocalPlayer.inventory)
                {
                    if (item == i)
                    {
                        if (BackpackStrengtheningUI.InventoryBar != a || !BackpackStrengtheningUI.Visible)
                        {
                            BackpackStrengtheningUI.InventoryBar = a;
                            for (int r = 0; r < ContainedItem.Length; r++)
                            {
                                if (ContainedItem[r].type != 0)
                                {
                                    Main.instance.LoadItem(ContainedItem[r].type);
                                }
                            }
                            BackpackStrengtheningUI.Visible = true;
                            break;
                        }
                        else
                        {
                            BackpackStrengtheningUI.Visible = false;
                        }
                        break;
                    }
                    a++;
                }
                CD = 10;
            }
            return false;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            CD--;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Loot.Count > 0)
            {
                if (Main.SmartCursorIsUsed)
                {
                    tooltips.Add(new TooltipLine(Mod, "袋子", "以下物品随机之一,子物品概率互相相等")
                    {
                        OverrideColor = new Color(255, 255, 255)
                    });
                    int Probability = 0;
                    for (int A = 0; A < Loot.Count; A++)
                    {
                        Probability += LootProbability[A];
                    }
                    for (int a = 1; a < Loot.Count+1; a ++)
                    {
                        string itemText = "";
                        for (int R = 0; R < Loot[a].Length; R++)
                        {
                            itemText += "   [i:" + Loot[a][R].type + "] X " + Loot[a][R].stack;
                        }
                        tooltips.Add(new TooltipLine(Mod, "袋子", "lv."+(a)+"  "+((float)LootProbability[a - 1] / Probability*100).ToString("F2") + "%" + itemText)
                        {
                            OverrideColor = new Color(255, 255, 255)
                        });
                    }
                }
                else
                {
                    tooltips.Add(new TooltipLine(Mod, "袋子", Language.GetTextValue("Mods.DDmod.Tooltips.抽奖"))
                    {
                        OverrideColor = new Color(255, 255, 255)
                    });
                }
            }
            if (ContainedItems <= 0)
            {
                return;
            }
            tooltips.Add(new TooltipLine(Mod, "袋子", Language.GetTextValue("Mods.DDmod.Tooltips.Bags"))
            {
                OverrideColor = new Color(255, 255, 255)
            });
            if (Main.SmartCursorIsUsed)
            {
                for (int a = 0; a < ContainedItems; a += 5)
                {
                    string S = "";
                    for (int b = 0; b < 5; b++)
                    {
                        if ((a / 5) * 5 + b < ContainedItems)
                        {
                            if (ContainedItem[a + b].type != 0)
                            {
                                S += "[i:" + ContainedItem[a + b].type + "]x" + ContainedItem[a + b].stack + "  ";
                            }
                            else
                            {
                                S += "空" + "  ";
                            }
                        }
                    }
                    tooltips.Add(new TooltipLine(Mod, "物品", S)
                    {
                        OverrideColor = new Color(255, 255, 255)
                    });
                }
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod, "袋子", Language.GetTextValue("Mods.DDmod.Tooltips.Bags2"))
                {
                    OverrideColor = new Color(255, 255, 255)
                });
            }
        }
        //public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        //{
        //return base.PreDrawTooltipLine(item, line, ref yOffset);
        // }

        public void WriteItem(Item item, BinaryWriter writer)
        {
            ItemIO.Send(item, writer, writeStack: true);
        }

        public void ReadItem(Item item, BinaryReader reader)
        {
            ItemIO.Receive(item, reader, readStack: true);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            if (ContainedItems > 0)
            {
                for (int a = 0; a < 40; a++)
                {
                    WriteItem(ContainedItem[a], writer);
                }
            }
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            if (ContainedItems > 0)
            {
                for (int a = 0; a < 40; a++)
                {
                    ReadItem(ContainedItem[a], reader);
                }
            }
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            for(int A = 0;A< ContainedItems;A++)
            {
                if(ContainedItem[A].type==5537)
                {
                    ContainedItem[A].SetDefaults(0);
                }
            }
            if (ContainedItems > 0)
                tag.Add("ContainedItem", ContainedItem);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            ContainedItem = tag.Get<Item[]>("ContainedItem");
        }
    }

    public class BackpackUI : UIElement
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
        /// <summary>
        /// 第几号物品栏
        /// </summary>
        public int InventoryID { get; set; }
        public BackpackUI(Asset<Texture2D> texture,int T) : base()
        {
            Scale = 1f;
            Opacity = 1f;
            CanPutInSlot = null;
            SlotBackTexture = texture;
            DrawColor = new Color(0x3f, 0x41, 0x97) * 0.75f;
            CornerSize = new Vector2(10, 10);
            Tooltip = "";
            InventoryID = T;
        }

        public override void Update(GameTime gameTime)
        {
            if (BackpackStrengtheningUI.InventoryBar == -1 || Main.LocalPlayer.inventory[BackpackStrengtheningUI.InventoryBar].type == 0 || Main.LocalPlayer.inventory[BackpackStrengtheningUI.InventoryBar].GetGlobalItem<BackpackUIItem>().ContainedItems == 0)
            {
                return;
            }
            BackpackUIItem gitem = Main.LocalPlayer.inventory[BackpackStrengtheningUI.InventoryBar].GetGlobalItem<BackpackUIItem>();
            if (gitem.ContainedItems > InventoryID)
            {
                if (ContainsPoint(Main.MouseScreen) && Main.LocalPlayer.itemAnimation == 0)
                {
                    if (ItemSlot.ShiftInUse)
                    {
                        Main.cursorOverride = 8;
                    }
                    if ((Main.mouseLeft && !dian) || Main.mouseRight)
                    {
                        if (ItemSlot.ShiftInUse&& BackpackStrengtheningUI.CanTakeItem(Main.LocalPlayer.inventory[BackpackStrengtheningUI.InventoryBar].GetGlobalItem<BackpackUIItem>().ContainedItem[InventoryID], Main.LocalPlayer.inventory,out int slot))
                        {
                            Click(Main.LocalPlayer.inventory[slot],slot);
                        }
                        else
                        {
                            Click();
                        }
                        dian = true;
                    }
                    if (!Main.mouseLeft)
                    {
                        dian = false;
                    }
                    Main.LocalPlayer.mouseInterface = true;
                }
            }
            if (!Main.mouseRight)
            {
                Time = 30;
                Time2 = 0;
            }
            base.Update(gameTime);

        }
        bool dian;
        float Time;
        float Time2;

        public void Click(Item item = null,int slot=-1)
        {
            /// <summary>
            /// 框内物品
            /// </summary>
            BackpackUIItem gitem = Main.LocalPlayer.inventory[BackpackStrengtheningUI.InventoryBar].GetGlobalItem<BackpackUIItem>();
            //开启背包
            //Main.playerInventory = true;
            if(Main.mouseRight)
            {
                Time-=0.5F;
                if(Time2>0)
                {
                    Time2--;
                    return;
                }
                //如果可以拿起物品
                if (CanTakeOutSlot == null || CanTakeOutSlot(gitem.ContainedItem[InventoryID]))
                {
                    if (Main.mouseRight && gitem.ContainedItem[InventoryID].type > 0 && gitem.ContainedItem[InventoryID].maxStack > 1)
                    {
                        if (Main.mouseItem.type == 0)
                        {
                            //触发放物品声音
                            SoundEngine.PlaySound(new SoundStyle("DDmod/NoContent/Sounds/Items/背包"));
                            if (gitem.ContainedItem[InventoryID].stack > 1)
                            {
                                Main.mouseItem = gitem.ContainedItem[InventoryID].Clone();
                                Main.mouseItem.stack = 1;
                                gitem.ContainedItem[InventoryID].stack--;
                            }
                            else
                            {
                                Main.mouseItem = gitem.ContainedItem[InventoryID].Clone();
                                //gitem.ContainedItem[InventoryID] = new Item();
                                gitem.ContainedItem[InventoryID].TurnToAir(true);
                            }
                        }
                        else if (Main.mouseItem.type == gitem.ContainedItem[InventoryID].type)
                        {
                            SoundEngine.PlaySound(new SoundStyle("DDmod/NoContent/Sounds/Items/点"));
                            if (gitem.ContainedItem[InventoryID].stack > 1)
                            {
                                Main.mouseItem.stack++;
                                gitem.ContainedItem[InventoryID].stack--;
                            }
                            else
                            {
                                Main.mouseItem.stack++;
                                //gitem.ContainedItem[InventoryID] = new Item();
                                gitem.ContainedItem[InventoryID].TurnToAir(true); ;
                            }
                        }
                        Time2 = Time;
                        //调用委托
                        OnPickItem?.Invoke(this);
                    }
                }
                return;
            }
            bool R = false;
            if (item == null&& slot <0)
            {
                R = true;
                item = Main.mouseItem;
            }
            //当鼠标没物品，框里有物品的时候
            if ((item.type == 0 && gitem.ContainedItem[InventoryID].type != 0))
            {
                //如果可以拿起物品
                if (CanTakeOutSlot == null || CanTakeOutSlot(gitem.ContainedItem[InventoryID]))
                {
                    //拿出物品
                    item = gitem.ContainedItem[InventoryID].Clone();
                    //gitem.ContainedItem[InventoryID] = new Item();
                    gitem.ContainedItem[InventoryID].TurnToAir(true);
                    if(slot>=0)
                    {
                        Main.LocalPlayer.inventory[slot] = item;
                    }
                    //调用委托
                    OnPickItem?.Invoke(this);
                }
            }
            //当鼠标有物品，框里没物品的时候
            else if (item.type != 0 && gitem.ContainedItem[InventoryID].type == 0)
            {
                //如果可以放入物品
                if (item.GetGlobalItem<BackpackUIItem>().ContainedItems > 0)
                {
                    return;
                }
                if ((CanPutInSlot == null || CanPutInSlot(item)))
                {
                    //放入物品
                    gitem.ContainedItem[InventoryID] = item.Clone();
                    //item = new Item();
                    item.TurnToAir(true);
                }
            }
            //当鼠标和框都有物品时
            else if (item.type != 0 && gitem.ContainedItem[InventoryID].type != 0)
            {
                //如果不能放入物品
                if (!(CanPutInSlot == null || CanPutInSlot(item)) || item.GetGlobalItem<BackpackUIItem>().ContainedItems > 0)
                {
                    //中断函数
                    return;
                }

                //如果框里的物品和鼠标的相同
                if (item.type == gitem.ContainedItem[InventoryID].type)
                {
                    if (slot >= 0)
                    {
                        //框里的物品数量加上鼠标物品数量
                        item.stack += gitem.ContainedItem[InventoryID].stack;
                        //如果框里物品数量大于数量上限
                        if (item.stack > item.maxStack)
                        {
                            //计算鼠标物品数量，并将框内物品数量修改为数量上限
                            var exceed = item.stack - item.maxStack;
                            item.stack = item.maxStack;
                            gitem.ContainedItem[InventoryID].stack = exceed;
                        }
                        //反之
                        else
                        {
                            //清空鼠标物品
                            //item = new Item();
                            gitem.ContainedItem[InventoryID].TurnToAir(true);
                        }
                    }
                    else
                    {
                        //框里的物品数量加上鼠标物品数量
                        gitem.ContainedItem[InventoryID].stack += item.stack;
                        //如果框里物品数量大于数量上限
                        if (gitem.ContainedItem[InventoryID].stack > gitem.ContainedItem[InventoryID].maxStack)
                        {
                            //计算鼠标物品数量，并将框内物品数量修改为数量上限
                            var exceed = gitem.ContainedItem[InventoryID].stack - gitem.ContainedItem[InventoryID].maxStack;
                            gitem.ContainedItem[InventoryID].stack = gitem.ContainedItem[InventoryID].maxStack;
                            item.stack = exceed;
                        }
                        //反之
                        else
                        {
                            //清空鼠标物品
                            //item = new Item();
                            item.TurnToAir(true);
                        }

                    }
                }
                //如果可以放入物品也能拿出物品
                else if ((CanPutInSlot == null || CanPutInSlot(item))
                    && (CanTakeOutSlot == null || CanTakeOutSlot(gitem.ContainedItem[InventoryID])))
                {
                    //交换框内物品和鼠标物品
                    var tmp = item.Clone();
                    item = gitem.ContainedItem[InventoryID];
                    gitem.ContainedItem[InventoryID] = tmp;
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

            //调用委托
            PostExchangeItem?.Invoke(this);
        }
        protected override void DrawSelf(SpriteBatch sb)
        {
            if (BackpackStrengtheningUI.InventoryBar == -1 ||Main.LocalPlayer.inventory[BackpackStrengtheningUI.InventoryBar].type == 0 || Main.LocalPlayer.inventory[BackpackStrengtheningUI.InventoryBar].GetGlobalItem<BackpackUIItem>().ContainedItems == 0)
            {
                return;
            }
            BackpackUIItem gitem = Main.LocalPlayer.inventory[BackpackStrengtheningUI.InventoryBar].GetGlobalItem<BackpackUIItem>();
            if (gitem.ContainedItems <= InventoryID)
            {
                return;
            }
            /// <summary>
            /// 框内物品
            /// </summary>
            Item ContainedItem = gitem.ContainedItem[InventoryID];

            //调用原版的介绍绘制
            if (ContainsPoint(Main.MouseScreen) && ContainedItem.type != 0)
            {
                Main.hoverItemName = ContainedItem.Name;
                Main.HoverItem = ContainedItem.Clone();
            }
            //获取当前UI部件的信息
            var DrawRectangle = GetDimensions();
            //绘制物品框
            DrawAdvBox(sb, (int)DrawRectangle.X, (int)DrawRectangle.Y,
                (int)DrawRectangle.Width, (int)DrawRectangle.Height,
                Color.White, SlotBackTexture.Value, CornerSize, Scale);

            if (ContainedItem.type != 0)
            {
                var frame = Main.itemAnimations[ContainedItem.type] != null ? Main.itemAnimations[ContainedItem.type].GetFrame(TextureAssets.Item[ContainedItem.type].Value) : TextureAssets.Item[ContainedItem.type].Frame(1, 1, 0, 0);
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
                if(ItemLoader.PreDrawInInventory(ContainedItem, sb, vector,frame, ContainedItem.GetAlpha(Color.White), Color.White * Opacity, size / 2, texScale * Scale))
                sb.Draw(TextureAssets.Item[ContainedItem.type].Value, vector, new Rectangle?(frame), Color.White * Opacity, 0, size/2, texScale * Scale, 0, 0);
                //绘制物品左下角那个代表数量的数字
                if (ContainedItem.stack > 1)
                {
                    sb.DrawString(FontAssets.MouseText.Value, ContainedItem.stack.ToString(), new Vector2(DrawRectangle.X + 10, DrawRectangle.Y + DrawRectangle.Height/2), Color.White * Opacity, 0f, Vector2.Zero, Scale*0.75F, SpriteEffects.None, 0f);
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
