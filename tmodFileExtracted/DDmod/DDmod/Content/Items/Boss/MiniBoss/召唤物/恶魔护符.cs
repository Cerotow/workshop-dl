using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.Items.农场;

namespace DDmod.Content.Items.Boss.MiniBoss.召唤物
{
    public class 恶魔护符 : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 26;
            Item.height = 30;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 1, 0, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(173, 15).AddIngredient(174, 30).AddTile(TileID.Hellforge).AddCondition(Condition.NearLava).Register();
        }
        bool R = false;
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                if (!R && Main.netMode == 1 && Main.myPlayer == player.whoAmI)
                {
                    for (int i = 0; i < 59; i++)
                    {
                        if (player.inventory[i].type == Item.type)
                        {
                            NetMessage.SendData(5, -1, player.whoAmI, null, player.whoAmI, PlayerItemSlotID.Inventory0 + i);
                        }
                    }
                }
                R = true;
                player.Aplayer().恶魔头颅 = true;

            }
            else
            {
                R = false;
            }
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Item.favorited);
        }
        public override void NetReceive(BinaryReader reader)
        {
            Item.favorited = reader.ReadBoolean();
        }
    }
}