using DDmod.Content;
using DDmod.Content.Items;
using DDmod.Content.Tiles.EquipTiles;
using DDmod.Modkey;
using DDmod.UI.ItemUI.背包;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DDmod.UI.ItemUI
{
    internal class StrengtheningUI : UIState
    {
        public static bool Visible = false;
        public static Vector2 Location;
        public static Point16 Tile;
        UIImageButton button;

        public static StrengthenUI[] StrengthenFrame = new StrengthenUI[3];
        public static StrengthenUI ItemFrame;
        private UIText text;
        public static int Synthesis;
        public override void OnInitialize()
        {
            for (int a = 1; a <= 3; a++)
            {
                StrengthenFrame[a-1] = new StrengthenUI(ModContent.Request<Texture2D>("DDmod/UI/ItemUI/强化石UI"), a);
                //设置进度条宽度
                StrengthenFrame[a - 1].Width.Set(StrengthenFrame[a - 1].SlotBackTexture.Width(), 0f);
                //设置进度条高度
                StrengthenFrame[a - 1].Height.Set(StrengthenFrame[a - 1].SlotBackTexture.Height(), 0f);
                //设置进度条距离所属ui部件的最左端的距离
                StrengthenFrame[a - 1].Left.Set(Location.X - Main.screenPosition.X - ModContent.Request<Texture2D>("DDmod/UI/ItemUI/强化石UI").Width() / 2, 0f);
                //设置进度条距离所属ui部件的最顶端的距离
                StrengthenFrame[a - 1].Top.Set(Location.Y - Main.screenPosition.Y - 150, 0f);
                Append(StrengthenFrame[a - 1]);
            }
            ItemFrame = new StrengthenUI(ModContent.Request<Texture2D>("DDmod/UI/ItemUI/强化物品UI"),0);
            //设置进度条宽度
            ItemFrame.Width.Set(ItemFrame.SlotBackTexture.Width(), 0f);
            //设置进度条高度
            ItemFrame.Height.Set(ItemFrame.SlotBackTexture.Height(), 0f);
            //设置进度条距离所属ui部件的最左端的距离
            ItemFrame.Left.Set(Location.X - Main.screenPosition.X - ModContent.Request<Texture2D>("DDmod/UI/ItemUI/强化物品UI").Width() / 2, 0f);
            //设置进度条距离所属ui部件的最顶端的距离
            ItemFrame.Top.Set(Location.Y - Main.screenPosition.Y - 150, 0f);

            text = new UIText("0/0", 0.8f);
            text.Width.Set(50, 0f);
            text.Height.Set(50, 0f);
            text.TextColor = new Color(219, 130, 255);

            //用tr原版图片实例化一个图片按钮
            button = new UIImageButton(ModContent.Request<Texture2D>("DDmod/UI/ItemUI/强化按钮"));
            //设置按钮距宽度
            button.Width.Set(50f, 0f);
            //设置按钮高度
            button.Height.Set(24f, 0f);
            //设置按钮距离所属ui部件的最左端的距离
            button.Left.Set(Location.X - Main.screenPosition.X - ModContent.Request<Texture2D>("DDmod/UI/ItemUI/强化按钮").Width() / 2, 0f);
            //设置按钮距离所属ui部件的最顶端的距离
            button.Top.Set(Location.Y - Main.screenPosition.Y - 100, 0f);
            //注册一个事件，这个事件将会在按钮按下时被激活
            button.OnLeftClick += CloseButton_OnClick;
            //将进度条注册入面板中，这个物品框的坐标将以面板的坐标为基础计算
            Append(button);
            Append(ItemFrame);
            ItemFrame.Append(text);
            base.OnInitialize();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //调用SetValue方法更新进度条的值
            //StrengthenFrame.DrawAdvBox(spriteBatch,400,400, ModContent.GetTexture("PVZ/NPCs/金剑").Width, ModContent.GetTexture("PVZ/NPCs/金剑").Height, Color.White, ModContent.GetTexture("PVZ/NPCs/金剑"),new Vector2(0));
        }
        public static bool CanAddItem(Item item, Item items, bool Main =false)
        {
            
            if (Main && (item.damage>=10|| RecipesSystem.SpecialMainMaterial[item.type])&& !item.IsArmor()&& !item.accessory)
            {
                return !RecipesSystem.SpecialMainMaterial[items.type];
            }
            else if(!Main&& items.type==0)
            {
                return true;
            }
            return false;
        }
        public override void Update(GameTime gameTime)
        {
            /// <summary>
            /// 框内物品
            /// </summary>
			TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
            StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(StrengtheningUI.Tile.X, StrengtheningUI.Tile.Y, tileData.Width, tileData.Height, 18);
            Item ContainedItem = tepowerCellFactory.items;
            for (int a = 1; a <= 3; a++)
            {
                StrengthenFrame[a - 1].Update(gameTime);
            }
            ItemFrame.Update(gameTime);


            //设置按钮距离所属ui部件的最左端的距离
            button.Left.Set(ItemFrame.GetDimensions().X, 0f);
            //设置按钮距离所属ui部件的最顶端的距离
            button.Top.Set(ItemFrame.GetDimensions().Y + 46, 0f);
            if(ModkeySetup.StrengtheningKey.JustPressed)
            {
                button.LeftClick(new UIMouseEvent(button,new Vector2(Main.mouseX,Main.mouseY)));
            }

            text.Top.Set(8, 0f);
            text.Left.Set(50, 0f);
            if (Timer != 0)
            {
                Timer--;
                text.Top.Set(4 + Main.rand.NextFloat(-2, 2), 0f);
                text.Left.Set(50 + Main.rand.NextFloat(-2, 2), 0f);
            }
            ItemFrame.Append(text);

            Player player = Main.player[Main.myPlayer];
            if ((Main.LocalPlayer.Center - Location).Length() > 100 || !Main.playerInventory)
            {
                Visible = false;
                Main.LocalPlayer.Dplayer().STPosition = Point16.Zero;
                DDmod.SyncData(DDType.PlayerData, Main.myPlayer, -1, Main.myPlayer);
                //Item item2 = ItemFrame.ContainedItem.Clone();

                //Item item_2 = player.GetItem(player.whoAmI, item2, GetItemSettings.InventoryUIToInventorySettings);


                //int num2 = Item.NewItem(player.GetSource_FromAI(), (int)player.position.X, (int)player.position.Y, player.width, player.height, item_2.type, item_2.stack, false, (int)item2.prefix, true, false);
                //Main.item[num2].newAndShiny = false;
                //ItemFrame.ContainedItem = new Item();
                //ItemFrame.ContainedItem.SetDefaults(0, true);
            }
            bool Sp = true;
            for (int a = 0; a < 3; a++)
            {
                if (ContainedItem.type > 0 && tepowerCellFactory.FortifiedStone[a].type > 0)
                {
                    if (ContainedItem.GetGlobalItem<StrengthenGlobalItem>().Level >= 0 && tepowerCellFactory.FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().Probability[ContainedItem.GetGlobalItem<StrengthenGlobalItem>().Level] > 0)
                    {
                        Sp = false;
                    }
                    else if(tepowerCellFactory.FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().Probability[0] > 0)
                    {

                        Sp = false;
                    }
                }
            }
            if (ContainedItem.type == 0)
            {
                text.SetText(Language.GetTextValue("Mods.DDmod.StrengtheningUI.请放入装备或特殊主材料"));
                U = false;
            }
            else if((ContainedItem.damage==0||ContainedItem.accessory)&& !RecipesSystem.SpecialMainMaterial[ContainedItem.type])
            {
                text.SetText(Language.GetTextValue("Mods.DDmod.StrengtheningUI.装备或特殊主材料不正确"));
            }
            else if(ContainedItem.damage<10 && !RecipesSystem.SpecialMainMaterial[ContainedItem.type]&& !ContainedItem.IsArmor())
            {
                text.SetText(Language.GetTextValue("Mods.DDmod.StrengtheningUI.伤害小于10不予强化"));
            }
            else if (RecipesSystem.SpecialMainMaterial[ContainedItem.type]&& Sp)
            {
                if (RecipesSystem.Material.ContainsKey(ContainedItem.type))
                {
                    RecipesSystem.Material.TryGetValue(ContainedItem.type, out Item[] item);
                    bool A = false;
                    bool B = false;
                    bool C = false;
                    List<Item> items = new List<Item>();
                    items.Add(item[0]);
                    items.Add(item[1]);
                    items.Add(item[2]);
                    for (int a = 0; a < 3; a++)
                    {
                        if (tepowerCellFactory.FortifiedStone[a].type == item[0].type && tepowerCellFactory.FortifiedStone[a].stack >= item[0].stack)
                        {
                            A = true;
                            items.Remove(item[0]);
                        }
                        if (tepowerCellFactory.FortifiedStone[a].type == item[1].type && tepowerCellFactory.FortifiedStone[a].stack >= item[1].stack)
                        {
                            B = true;
                            items.Remove(item[1]);
                        }
                        if (tepowerCellFactory.FortifiedStone[a].type == item[2].type && tepowerCellFactory.FortifiedStone[a].stack >= item[2].stack)
                        {
                            C = true;
                            items.Remove(item[2]);
                        }
                    }
                    if (items.Count > 0)
                    {
                        string T = "";
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (i != 0)
                            {
                                T += ",";
                            }
                            T += items[i].Name + "x" + items[i].stack;
                        }
                        text.SetText(Language.GetTextValue("Mods.DDmod.StrengtheningUI.特殊主材料合成缺少") + ": " + T);
                    }
                    if (A && B && C)
                    {
                        text.SetText(Language.GetTextValue("Mods.DDmod.StrengtheningUI.可以合成") + item[3].Name);
                    }
                }
            }
            else if (!ContainedItem.IsArmor())
            {
                int Rand = 0;
                for (int a = 0; a < 3; a++)
                {
                    if (tepowerCellFactory.FortifiedStone[a].type != 0)
                    {
                        if (ContainedItem.GetGlobalItem<StrengthenGlobalItem>().Level >= 0)
                        {
                            Rand += tepowerCellFactory.FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().Probability[ContainedItem.GetGlobalItem<StrengthenGlobalItem>().Level];
                        }
                        else
                        {
                            if (tepowerCellFactory.FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>().Probability[0] > 0)
                            {
                                Rand = 100;
                            }
                        }
                    }
                }
                if (Rand > 100) Rand = 100;
                if (ContainedItem.damage > 0)
                {
                    text.SetText(Language.GetTextValue("Mods.DDmod.StrengtheningUI.强化成功率") + Rand + "%");
                }
            }
            else
            {
                string Text = Language.GetTextValue("Mods.DDmod.StrengtheningUI.融合成功率");
                List<EnchantmentItem> Enchantments = new List<EnchantmentItem>();
                for (int a = 0; a < 3; a++)
                {
                    if (tepowerCellFactory.FortifiedStone[a].type != 0)
                    {
                        StrengthenGlobalItem strengthen = tepowerCellFactory.FortifiedStone[a].GetGlobalItem<StrengthenGlobalItem>();
                        for (int E = 0; E < 2; E++)
                        {
                            if(strengthen.EnchantmentItemType[E]>0)
                            Enchantments.Add(new EnchantmentItem(strengthen.EnchantmentItemType[E], strengthen.MinE[E], strengthen.MaxE[E], strengthen.Enchantment[E]));

                        }
                    }
                }
                for (int a = 0; a < Enchantments.Count; a++)
                {
                    for (int b = a+1; b < Enchantments.Count; b++)
                    {
                        if (!Enchantments[a].SC&&Enchantments[a].EnchantmentItemType== Enchantments[b].EnchantmentItemType && Enchantments[a].Min== Enchantments[b].Min&&Enchantments[a].Max== Enchantments[b].Max)
                        {
                            Enchantments[a].Read += Enchantments[b].Read;
                            Enchantments[b].SC = true;
                        }
                    }
                }
                for (int a = 0; a < Enchantments.Count; a++)
                {
                    if (!Enchantments[a].SC)
                    {
                        Text += Enchantments[a].Text();
                    }
                }
                text.SetText(Text);
            }
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
            public int Read;
            /// <summary>
            /// 删除
            /// </summary>
            public bool SC;
            /// <summary>
            /// 文本
            /// </summary>
            public string Text()
            {
                return "\n" + StrengthenGlobalItem.EnchantmentText(EnchantmentItemType, out bool B, out bool R) + ((Max - 1 == 0) ? "" : (R ? " -" : " +")) + "(" + ((Max - 1 == 0) ? Language.GetTextValue("Mods.DDmod.properties.唯一") : ((Max - 1 == Min&&false) ? (Min + (B ? "%" : "")) : (Min + (B ? "%" : "") + "~" + (Max - 1) + (B ? "%" : "")))) + ")" + Read + "%";
            }
            public EnchantmentItem(int ET,int Min,int Max,int Read)
            {
                this.EnchantmentItemType = ET;
                this.Min = Min;
                this.Max = Max;
                this.Read = Read;
            }
        }
        bool U;
        int Timer = 10;
        private void CloseButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
            StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(StrengtheningUI.Tile.X, StrengtheningUI.Tile.Y, tileData.Width, tileData.Height, 18);
            if (tepowerCellFactory.items.type == 0)
            {
                return;
            }
            else if (tepowerCellFactory.items.damage == 0 && !tepowerCellFactory.items.IsArmor() && !RecipesSystem.SpecialMainMaterial[tepowerCellFactory.items.type])
            {
                return;
            }
            else if (tepowerCellFactory.items.damage < 10 && !tepowerCellFactory.items.IsArmor() && !RecipesSystem.SpecialMainMaterial[tepowerCellFactory.items.type])
            {
                return;
            }
            tepowerCellFactory.Start = true;
            //Visible = false;
            //Main.LocalPlayer.Dplayer().STPosition = Point16.Zero;
            DDmod.SyncData(DDType.PlayerData, Main.myPlayer, -1, Main.myPlayer);
            U = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = DDmod.Instance.GetPacket(256);
                //写入要发的包
                packet.Write((byte)DDType.TalismanTE2);
                packet.WriteVector2(new Vector2(StrengtheningUI.Tile.X, StrengtheningUI.Tile.Y));
                packet.Write(true);
                packet.Write(-1);
                //发出去
                packet.Send(-1, Main.myPlayer);
            }
            return;
        }
    }
}
