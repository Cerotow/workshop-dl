using System;
using DDmod.Content.Projectiles.Melee.TwinSwords;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Melee.TwinSwords
{
	public class 耀斑双刃 : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 84;
			Item.DamageType = DamageClass.Melee;
			Item.width = 22;
			Item.height = 38;
			Item.useAnimation = 6;
			Item.useTime = Item.useAnimation;
			Item.knockBack = 2;
			Item.value = 100;
			Item.rare = 2;
			Item.shoot = ModContent.ProjectileType<耀斑双刃Proj>();
			Item.shootSpeed = 5;
			Item.autoReuse = true;
			Item.useStyle = 13;
			Item.scale = 1F;
			Item.noMelee = true;
			Item.UseSound = null;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.DItem().Twin = true;
		}

		public override void SetStaticDefaults()
		{
		}
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if(Time++>60)
            {
				Time = 0;
				R = 0;
			}
		}
		public override void UpdateInventory(Player player)
		{
			if (Time++ > 60)
			{
				Time = 0;
				R = 0;
			}
		}
		public override void HoldItem(Player player)
		{
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
		}
        public override bool MeleePrefix()
        {
            return true;
        }
		byte R = 0;
		byte Time = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Time = 0; R = 0;
			if (!player.PlayerAction().ThereShield2)
			{
				if(R==0)
				{
					NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 0);
					NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, player.HeldItem.useAnimation / 2, 0, 1);
					R = 1;
				}
				else
				if (R == 1)
				{
					NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 0,2);
					NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, player.HeldItem.useAnimation / 2, 0, 3);
					R = 2;
				}
				else
				if (R >= 2)
				{
					NewProjectile(source, position, velocity, type, damage*2, knockback*6, player.whoAmI, 0, 0,4);
					NewProjectile(source, position, velocity, type, damage*2, knockback*6, player.whoAmI, player.HeldItem.useAnimation / 2, 0, 5);
					R = 0;
				}
			}
			else
			{
				if (R >= 2)
                {
					R = 0;
                }
				if (R == 0)
				{
					NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, player.HeldItem.useAnimation / 2, 0, 1);
					R = 1;
				}
				else
				if (R == 1)
				{
					NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, player.HeldItem.useAnimation / 2, 0, 3);
					R = 0;
				}
			}
			return false;
		}
	}
}