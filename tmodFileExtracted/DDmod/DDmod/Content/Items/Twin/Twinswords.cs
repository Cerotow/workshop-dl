using System;
using DDmod.Content.Projectiles.Melee.TwinSwords;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DDmod.Content.Buffs.DeBuffs;

namespace DDmod.Content.Items
{
	public abstract class Twinswords : ModItem
	{
		public virtual void Set()
		{

		}
		/// <summary>
		/// 重击伤害倍率
		/// </summary>
		public virtual float SpecialMagnification => 3;
		/// <summary>
		/// 第三刀倍率
		/// </summary>
		public virtual float Magnification => 2;
		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;
			Item.useStyle = 13;
			Item.noMelee = true;
			Item.UseSound = null;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.DItem().Twin = true;
			Item.GetGlobalItem<MeleeGlobalItem>().SpecialAttack = true;
			Set();
            Item.useAnimation = Item.useTime;

        }
        public override bool MeleePrefix()
        {
            return true;
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
			if (Main.mouseItem.type == Item.type && Time++ > 60)
			{
				Time = 0;
				R = 0;
			}
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
					int proj = NewProjectile(source, position, velocity, type, (int)(damage* SpecialMagnification), knockback * SpecialMagnification, player.whoAmI, 0, 0, R);
					Main.projectile[proj].DProj().Magnification = SpecialMagnification;
					Main.projectile[proj].DProj().Bool[0] = true;
					Main.projectile[proj].netUpdate = true;

					int proj2 = NewProjectile(source, position, velocity, type, (int)(damage * SpecialMagnification), 0, player.whoAmI, 0, 0, R);
					Main.projectile[proj2].DProj().Back = -1;
					Main.projectile[proj2].DProj().Other = proj;
					Main.projectile[proj].DProj().Other = proj2;
					Main.projectile[proj2].DProj().Magnification = SpecialMagnification;
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
						damage = (int)(damage*Magnification);
						knockback *= Magnification;
					}
					int proj = NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 0, R);
					if (R == 2) Main.projectile[proj].DProj().Magnification = Magnification;
					Main.projectile[proj].netUpdate = true;

					int proj2 = NewProjectile(source, position, velocity, type, damage, 0, player.whoAmI, proj, 0, R);
					Main.projectile[proj2].DProj().Back = -1;
					Main.projectile[proj2].DProj().Other = proj;
                    Main.projectile[proj2].DProj().Bool[1] = true;
                    Main.projectile[proj].DProj().Other = proj2;
					if (R == 2) Main.projectile[proj2].DProj().Magnification = Magnification;
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
	}
}