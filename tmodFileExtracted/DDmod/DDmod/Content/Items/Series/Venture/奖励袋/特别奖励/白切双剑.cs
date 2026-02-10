using System;
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee.TwinSwords;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.Venture.奖励袋.特别奖励
{
	public class 白切双剑 : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Melee;
			Item.width = 22;
			Item.height = 38;
			Item.useAnimation = 16;
			Item.useTime = Item.useAnimation;
			Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<白切双剑Proj>();
			Item.shootSpeed = 5;
			Item.autoReuse = true;
			Item.useStyle = 13;
			Item.scale = 1F;
			Item.noMelee = true;
			Item.UseSound = null;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.DItem().Twin = true;
			Item.GetGlobalItem<MeleeGlobalItem>().SpecialAttack = true;
		}

		public override void SetStaticDefaults()
		{
		}
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (Time++ > 60)
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
        byte R = 0;
        byte Time = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Time = 0;
            if (!player.PlayerAction().ThereShield2)
            {
                if (!player.HasBuff(ModContent.BuffType<SpecialAttackCD>()) && player.controlUseTile)
                {
                    R = 3;
                    int proj = NewProjectile(source, position, velocity, type, damage * 3, knockback * 3, player.whoAmI, 0, 0, R);
                    Main.projectile[proj].DProj().Magnification = 3;
                    Main.projectile[proj].DProj().Bool[0] = true;
                    Main.projectile[proj].netUpdate = true;

                    int proj2 = NewProjectile(source, position, velocity, type, damage * 3, 0, player.whoAmI, 0, 0, R);
                    Main.projectile[proj2].DProj().Back = -1;
                    Main.projectile[proj2].DProj().Other = proj;
                    Main.projectile[proj].DProj().Other = proj2;
                    Main.projectile[proj2].DProj().Magnification = 3;
                    Main.projectile[proj2].DProj().Bool[0] = true;
                    Main.projectile[proj2].netUpdate = true;
                    R = 0;
                }
                else
                {
                    if (R > 2)
                    {
                        R = 0;
                    }
                    if (R == 2)
                    {
                        damage *= 2;
                        knockback *= 2;
                    }
                    int proj = NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 0, R);
                    if (R == 2) Main.projectile[proj].DProj().Magnification = 2;
                    Main.projectile[proj].netUpdate = true;

                    int proj2 = NewProjectile(source, position, velocity, type, damage, 0, player.whoAmI, proj, 0, R);
                    Main.projectile[proj2].DProj().Back = -1;
                    Main.projectile[proj2].DProj().Other = proj;
                    Main.projectile[proj2].DProj().Bool[1] = true;
                    Main.projectile[proj].DProj().Other = proj2;
                    if (R == 2) Main.projectile[proj2].DProj().Magnification = 2;
                    Main.projectile[proj2].netUpdate = true;
                    R++;
                }
            }
            else
            {
                if (R >= 2)
                {
                    R = 0;
                }
                NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, player.HeldItem.useAnimation / 2, 0, R);
                R++;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<白切精华>()).Register();
        }
    }
}