using System;
using DDmod.Content.Items.Series.钢;
using DDmod.Content.Projectiles.Ranged.Gun;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.Make.Gun
{
	public class 叶绿枪 : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 38;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 38;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = 6;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.shootSpeed = 8;
			Item.autoReuse = true;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.UseSound = null;
			Item.useAmmo = AmmoID.Bullet;

            Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.Gun.叶绿枪Proj>();
            Item.channel = true;
            Item.noUseGraphic = true;
        }

		public override void SetStaticDefaults()
		{
		}
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
		}
		public override void UpdateInventory(Player player)
		{
		}
		public override void HoldItem(Player player)
		{
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2?(new Vector2(-2f, -2f));
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return player.itemTime > 0;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(1006, 12).AddIngredient(ModContent.ItemType<钢锭>(), 6).AddIngredient(324, 1).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}