using DDmod.Content.Projectiles.Melee;

namespace DDmod.Content.Items.Melee.Sword
{
    public class 火山长剑 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 98;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.scale = 1.3F;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(255, 121, 3)*0.3F;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}