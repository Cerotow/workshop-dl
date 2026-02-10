using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Accessory
{
    public class 远古魔戒 : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2+=40;
            player.manaRegen += 2;
        }

    }
}
