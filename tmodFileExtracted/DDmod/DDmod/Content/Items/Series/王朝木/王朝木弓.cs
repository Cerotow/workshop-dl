using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.ID;

namespace DDmod.Content.Items.Series.王朝木
{
    public class 王朝木弓 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            if (Main.netMode != 2)
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.crit = 0;
            Item.value = Item.buyPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.LightRed;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot =2;
            Item.shootSpeed = 10f;

            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
        }
        public override bool CanShoot(Player player)
        {
            return base.CanShoot(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.DynastyWood, 6).AddTile(TileID.WorkBenches).Register();
        }
    }
}