using DDmod.Content;
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
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace DDmod.UI.ItemUI.背包
{
    internal class BackpackStrengtheningUI : UIState
    {
        public static bool Visible = false;

        public static BackpackUI[] bar2 = new BackpackUI[40];
        public static int InventoryBar =-1;
        public static int InventoryBar2;
        public override void OnInitialize()
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>("DDmod/UI/ItemUI/背包/物品UI");
            for (int a = 0; a < bar2.Length; a++)
            {
                bar2[a] = new BackpackUI(texture, a);
                //设置进度条宽度
                bar2[a].Width.Set(bar2[a].SlotBackTexture.Width(), 0f);
                //设置进度条高度
                bar2[a].Height.Set(bar2[a].SlotBackTexture.Height(), 0f);
                //设置进度条距离所属ui部件的最左端的距离
                bar2[a].Left.Set(Main.screenWidth / 2 + texture.Width() * 1.05F * (a % 10 - 5), 0f);
                //设置进度条距离所属ui部件的最顶端的距离
                bar2[a].Top.Set(Main.screenHeight / 2 + texture.Height() * 1.05F * (1 + a / 10), 0f);
                //将进度条注册入面板中，这个物品框的坐标将以面板的坐标为基础计算
                Append(bar2[a]);
            }
            base.OnInitialize();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //调用SetValue方法更新进度条的值
            //bar.DrawAdvBox(spriteBatch,400,400, ModContent.GetTexture("PVZ/NPCs/金剑").Width, ModContent.GetTexture("PVZ/NPCs/金剑").Height, Color.White, ModContent.GetTexture("PVZ/NPCs/金剑"),new Vector2(0));
        }

        public static bool CanAddItem(Item item, Item[] items, out int slot)
        {
            slot = -1;
            for (int A = 0; A < Main.LocalPlayer.inventory[BackpackStrengtheningUI.InventoryBar].GetGlobalItem<BackpackUIItem>().ContainedItems; A++)
            {
                if (items[A].type == item.type && items[A].maxStack > items[A].stack)
                {
                    slot = A;
                    return true;
                }
            }
            for (int A = 0; A < Main.LocalPlayer.inventory[BackpackStrengtheningUI.InventoryBar].GetGlobalItem<BackpackUIItem>().ContainedItems; A++)
            {
                if (items[A].type == 0)
                {
                    slot = A;
                    return true;
                }
            }
            return false;
        }
        public static bool CanTakeItem(Item item, Item[] items, out int slot)
        {
            slot = -1;
            if (item.type == 71 || item.type == 72 || item.type == 73 || item.type == 74)
            {
                for (int A = 50; A < 53; A++)
                {
                    if (items[A].type == item.type && items[A].maxStack > items[A].stack)
                    {
                        slot = A;
                        return true;
                    }
                }
                for (int A = 50; A < 53; A++)
                {
                    if (items[A].type == 0)
                    {
                        slot = A;
                        return true;
                    }
                }
            }
            if (item.ammo>0)
            {
                for (int A = 54; A < 57; A++)
                {
                    if (items[A].type == item.type && items[A].maxStack > items[A].stack)
                    {
                        slot = A;
                        return true;
                    }
                }
                for (int A = 54; A < 57; A++)
                {
                    if (items[A].type == 0)
                    {
                        slot = A;
                        return true;
                    }
                }
            }
            for (int A = 0; A < 49; A++)
            {
                if (items[A].type == item.type && items[A].maxStack > items[A].stack)
                {
                    slot = A;
                    return true;
                }
            }
            for (int A = 0; A < 49; A++)
            {
                if (items[A].type == 0)
                {
                    slot = A;
                    return true;
                }
            }
            return false;
        }
        public override void Update(GameTime gameTime)
        {
            Player player = Main.player[Main.myPlayer];
            if (InventoryBar ==-1 || player.inventory[InventoryBar].type == 0)
            {
                InventoryBar = -1;
                Visible = false;
                return;
            }
            Item[] items = player.inventory[InventoryBar].GetGlobalItem<BackpackUIItem>().ContainedItem;
            if (Main.LocalPlayer.inventory[InventoryBar].GetGlobalItem<BackpackUIItem>().ContainedItems == 0 || items.Length == 0 || !Main.playerInventory)
            {
                InventoryBar = -1;
                Visible = false;
                return;
            }
            Asset<Texture2D> texture = ModContent.Request<Texture2D>("DDmod/UI/ItemUI/背包/物品UI");

            for (int a = 0; a < items.Length; a++)
            {
                //  设置进度条宽度
                bar2[a].Width.Set(bar2[a].SlotBackTexture.Width(), 0f);
                //设置进度条高度
                bar2[a].Height.Set(bar2[a].SlotBackTexture.Height(), 0f);
                //设置进度条距离所属ui部件的最左端的距离
                bar2[a].Left.Set(888- texture.Width() + texture.Width() * 1.05F * (a % 10 - 5), 0f);
                //设置进度条距离所属ui部件的最顶端的距离
                bar2[a].Top.Set(60 + texture.Height() * 1.05F * (1 + a / 10), 0f);
            }
            /// <summary>
            /// 框内物品
            /// </summary>
            for (int a = 0; a < bar2.Length; a++)
            {
                bar2[a].Update(gameTime);
            }
        }
    }
}
