using System;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.Make
{
	public class 花岗岩能量喷射器 : ModItem
	{
		public override void SetStaticDefaults() 
		{
		}

		public override void SetDefaults() 
		{
			Item.damage = 30;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1;
			Item.value = 1000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item34;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<花岗岩激光>();
			Item.shootSpeed = 12f;
			Item.channel = true;
			Item.noUseGraphic = true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2?(new Vector2(-10f, 0f));
		}
		public override void HoldItem(Player player)
        {
			//player.armorPenetration += 50;
		}
	}
}