using DDmod.Content.Projectiles.Melee.ball;
using DDmod.Content.Tiles.Mine;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.Heart
{
	public class BrokenHeartItem : ModItem
	{
		public override void SetDefaults()
        {
            Item.damage = 28;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.scale = 1f;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 1;
            Item.shoot = ModContent.ProjectileType<BrokenHeart>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
        }

		public override void SetStaticDefaults()
		{
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<HeartIngot>(),12).AddTile(TileID.Anvils).Register();
		}
	}
}
