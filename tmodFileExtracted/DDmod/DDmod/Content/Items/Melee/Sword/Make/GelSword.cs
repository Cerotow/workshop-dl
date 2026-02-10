using DDmod.Content.Projectiles.Melee;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.Sword.Make
{
    public class GelSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime =20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8f;
            Item.crit = 100;
            Item.value = Item.buyPrice(0, 0, 0, 20);
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Gel, 24).AddTile(TileID.WorkBenches).Register();
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}