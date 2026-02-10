using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Boss.特殊
{
	public class 海爵之咬 : ModItem
	{
		public override void SetStaticDefaults() 
		{
		}

		public override void SetDefaults() 
		{
			Item.damage = 95;
			Item.DamageType = DamageClass.Melee;
			Item.width = 50;
			Item.height = 52;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6.5f;
			Item.crit = 4;
			Item.value = 66666;
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.noMelee = false;
			Item.shoot = ModContent.ProjectileType<猪鲨>();
			Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(23, 147, 234, 0) * 0.3F;
			Item.DItem().HandheldColor = Color.White;
			Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 2;
			Item.shootSpeed = 15f;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(2))
			{
				Projectile.NewProjectile(player.GetSource_FromAI(),hitbox.X, hitbox.Y, player.direction*2, 0, ProjectileID.FlaironBubble, Item.damage / 2, 1, player.whoAmI);
			}
		}
		public override void OnHitNPC(Player player, NPC target, HitInfo hit, int damageDone)
		{
			//NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(2f, 0), ModContent.ProjectileType<海啸>(), Projectile.damage / 12, Projectile.knockBack / 4);
			int A= NewProjectile(player.GetSource_FromAI(), target.Center, new Vector2(0f, 0), ModContent.ProjectileType<海啸>(), damageDone / 12, hit.Knockback / 4, -1, 0, 1);
			Main.projectile[A].DamageType = DamageClass.Melee;
			//NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(-2f, 0), ModContent.ProjectileType<海啸>(), Projectile.damage / 12, Projectile.knockBack / 4);

		}
        public override bool MeleePrefix()
		{
			return true;
		}
	}
}