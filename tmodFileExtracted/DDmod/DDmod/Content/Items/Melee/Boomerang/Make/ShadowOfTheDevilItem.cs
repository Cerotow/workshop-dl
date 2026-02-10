using DDmod.Content.Projectiles.Melee.Boomerang;
using DDmod.Players;

namespace DDmod.Content.Items.Melee.Boomerang.Make
{
    public class ShadowOfTheDevilItem : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 39;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 30;
            Item.noUseGraphic = true;
            Item.consumable = false;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<ShadowOfTheDevilProj>();
            Item.shootSpeed = 10f;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.GetGlobalItem<MeleeGlobalItem>().Boomerang = true;
            Item.DItem().Boomerang = 1;
            Item.channel = true;
            Item.autoReuse = true;
        }
        public override bool CanUseItem(Player player)
        {
            return DDPlayer.UseBoomerang(Item, player);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.DemoniteBar, 12).AddIngredient(ItemID.ShadowScale, 20).AddTile(TileID.Anvils).Register();
        }
    }
}
