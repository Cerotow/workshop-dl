using DDmod.Content.Projectiles.OrnamentProjectile;

namespace DDmod.Content.Items.Boss.流星破坏者
{
    public class 破坏者核心装置 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 8, false));
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.expert = true;
            Item.defense = 4;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.1F;
            player.GetCritChance(DamageClass.Generic) += 8;
            player.moveSpeed += 0.2F;
            player.Aplayer().破坏者核心装置 = true;


        }
    }
}
