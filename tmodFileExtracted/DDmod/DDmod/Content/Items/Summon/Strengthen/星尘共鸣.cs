using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.Content.Projectiles.Summon.Minions.Strengthen;
using DDmod.NoContent.Config;
using Terraria;
using static AssGen.Assets;

namespace DDmod.Content.Items.Summon.Strengthen
{
    public class 星尘共鸣 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 150;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = 13;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 30, 20, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 4;
            Item.shoot = ModContent.ProjectileType<星尘共鸣Proj>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.DItem().DrawMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, 0, player.whoAmI, 0f, 0f);
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(3459, 18).AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}