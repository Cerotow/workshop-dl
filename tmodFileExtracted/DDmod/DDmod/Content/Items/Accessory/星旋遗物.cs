using DDmod.Content.Projectiles.GeneralProj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Accessory
{
    public class 星旋遗物 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(4, 5));
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 10;
            Item.accessory = true;
        }
        int R = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Ranged) += 0.25F;
            player.GetArmorPenetration(DamageClass.Ranged) += 15;
        }

    }
}
