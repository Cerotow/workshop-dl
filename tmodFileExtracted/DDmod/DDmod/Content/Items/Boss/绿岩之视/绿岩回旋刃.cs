using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Projectiles.Melee.Boomerang.Chakram;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Players;
using Terraria;

namespace DDmod.Content.Items.Boss.绿岩之视
{
    public class 绿岩回旋刃 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(0, 0, 40, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 8;
            Item.shoot = ModContent.ProjectileType<绿岩回旋刃Proj>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.GetGlobalItem<MeleeGlobalItem>().Chakram = true;
            Item.DItem().Boomerang = 1;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DGlobalItem.Frisbee[Item.type] = true;
        }
        public override bool CanUseItem(Player player)
        {
            return DDPlayer.UseBoomerang(Item, player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 24).AddIngredient(ModContent.ItemType<绿岩电池>(), 5).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
    }
}