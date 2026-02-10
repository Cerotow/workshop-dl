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
    public class 橡果灵灯 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 8;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 180;
            Item.useAnimation = 180;
            Item.useStyle = 0;
            Item.holdStyle = 0;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item82;
            Item.autoReuse = true;
            Item.shootSpeed = 4;
            Item.shoot = 0;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.MagicItem().charging = 3;
            Item.MagicItem().ExtraMana = 3;
            Item.DItem().NoDraw = true;
            Item.DItem().SummonLamp = true;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<橡果灵灯Proj>()] == 0)
            {
                NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<橡果灵灯Proj>(), 0, Item.knockBack, -1, 0, 0);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            //CreateRecipe(1).AddIngredient(3459, 18).AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}