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

namespace DDmod.UI.抽奖UI
{
    internal class 抽奖UI : UIState
    {
        public static bool Visible = false;

        public static 抽奖UI信息[] bar2 = new 抽奖UI信息[10];
        public static int InventoryBar = -1;
        public float Quantity = 1;
        public Item[] Item;
        public int[] Quality;
        public static 抽奖UI instance;
        public static void Loot(int Quantity, int type,Item item, int Quality = 1)
        {
            for (int a = 0; a < bar2.Length; a++)
            {
                bar2[a].看到物品 = false;
                bar2[a].翻牌 = -1;
                bar2[a].position = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 120);
                bar2[a].XP = 0;
            }
            if (Quantity>10)
            {
                Quantity = 10;
            }
            抽奖UI.instance.Quantity = Quantity;
            抽奖UI.instance.Item[type] = item;
            抽奖UI.instance.Quality[type] = Quality;
        }
        public override void OnInitialize()
        {
            for (int a = 0; a < bar2.Length; a++)
            {
                bar2[a] = new 抽奖UI信息(a);
                //设置进度条宽度
                bar2[a].Width.Set(60, 0f);
                //设置进度条高度
                bar2[a].Height.Set(60, 0f);
                //设置进度条距离所属ui部件的最左端的距离
                bar2[a].Left.Set(Main.screenWidth / 2 + 60 * 1.05F * (a % 10 - 5), 0f);
                //设置进度条距离所属ui部件的最顶端的距离
                bar2[a].Top.Set(Main.screenHeight / 2 + 60 * 1.05F * (1 + a / 10), 0f);
                //将进度条注册入面板中，这个物品框的坐标将以面板的坐标为基础计算
                Append(bar2[a]);
            }
            instance = this;
            base.OnInitialize();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //调用SetValue方法更新进度条的值
            //bar.DrawAdvBox(spriteBatch,400,400, ModContent.GetTexture("PVZ/NPCs/金剑").Width, ModContent.GetTexture("PVZ/NPCs/金剑").Height, Color.White, ModContent.GetTexture("PVZ/NPCs/金剑"),new Vector2(0));
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.player[Main.myPlayer];
            for (int a = 0; a < Quantity; a++)
            {
                bar2[a].Update(gameTime);
            }
            if (!Main.playerInventory|| Main.SmartCursorIsUsed)
            {
                Visible = false;


                for (int a = 0; a < Item.Length; a++)
                {
                    if (Item[a]!=null&& Item[a].type != 0)
                    {
                        Item item_2 = player.GetItem(player.whoAmI, Item[a],GetItemSettings.LootAllSettings);
                        int num2 = Terraria.Item.NewItem(player.GetSource_Loot(), (int)player.position.X, (int)player.position.Y, player.width, player.height, item_2.type, item_2.stack, false, (int)Item[a].prefix, true, false);
                        Main.item[num2].newAndShiny = false;
                        if (Main.netMode == 1)
                            NetMessage.SendData(21, -1, -1, null, num2);
                        Item[a] = DDmod.NewItem.Clone();
                        Item[a].SetDefaults(0, true);
                    }
                }
            }
        }
    }
}
