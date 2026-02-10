using System;
using DDmod.Content.Projectiles.Ranged.Gun;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Boss.Boss特殊
{
	public class 咒焰瞬光 : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 38;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.knockBack = 1;
			Item.value = 1145141;
			Item.rare = 6;
			Item.shoot = ModContent.ProjectileType<咒焰瞬光Proj>();
			Item.shootSpeed = 5;
			Item.autoReuse = true;
			Item.useStyle = 13;
			Item.noMelee = true;
			Item.UseSound = null;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.useAmmo = AmmoID.Bullet;
			Item.DItem().Twin = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 2;
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
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return player.itemTime > 0;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			NewProjectile(source, position, velocity, ModContent.ProjectileType<咒焰瞬光Proj>(), damage, knockback, player.whoAmI, 0);
			int proj = NewProjectile(source, position, velocity, ModContent.ProjectileType<咒焰瞬光Proj>(), damage, knockback, player.whoAmI, 4);
			Main.projectile[proj].DProj().Back = -1;
			return false;
		}
	}
}