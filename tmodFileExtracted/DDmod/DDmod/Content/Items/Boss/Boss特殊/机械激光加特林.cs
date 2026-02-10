using System;
using DDmod.Content.Projectiles.Magic.Gun;
using DDmod.Content.Projectiles.Ranged.Gun;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Boss.Boss特殊
{
	public class 机械激光加特林 : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage =40;
			Item.DamageType = DamageClass.Magic;
			Item.width = 22;
			Item.height = 38;
			Item.useTime = 5;
			Item.useAnimation = 5;
			Item.knockBack = 1;
			Item.mana = 9;
			Item.value = 1545141;
			Item.rare = 6;
			Item.shoot = ModContent.ProjectileType<机械激光加特林Proj>();
			Item.shootSpeed =12;
			Item.autoReuse = true;
			Item.useStyle = 13;
			Item.DItem().DrawRanged = true;

            Item.noMelee = true;
			Item.UseSound = null;
			Item.channel = true;
			Item.noUseGraphic = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 3;
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
			NewProjectile(source, position, velocity, ModContent.ProjectileType<机械激光加特林Proj>(), damage, knockback, player.whoAmI, 0);
			return false;
		}
	}
}