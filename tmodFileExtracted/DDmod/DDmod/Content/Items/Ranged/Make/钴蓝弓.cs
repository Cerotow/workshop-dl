using System;
using DDmod.Content.Projectiles;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Bow;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.Make
{
	public class 钴蓝弓 : ModItem
	{
		public override void Load()
		{
		}
		public override void SetDefaults()
		{
			Item.damage = 26;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 38;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 4;
            Item.shoot = ProjectileID.IchorArrow;
			Item.shootSpeed = 8f;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = 40;
			Item.GetGlobalItem<RangedGlobalItem>().Bow = true;

            ModifyBow.Load(Type, new Modify(),true);
			Item.GetGlobalItem<RangedGlobalItem>().SetString(5,5,3,0,6,false, new Color(100, 206, 249));
		}

		public override void SetStaticDefaults()
		{

            if (Main.netMode != 2)
				DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.CobaltBar, 10).AddTile(TileID.Anvils).Register();
        }
        public class Modify : ModifyBow
        {
            public override bool SetArrows(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                return true;
            }
            public override void PreModifyArrow(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged, ref float speed, ref int damage, ref float knockBack, ref int usedAmmoItemId, ref int projToShoot)
            {
            }
            /// <summary>
            ///修改箭后
            /// </summary>
            public override void PostModifyArrow(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged, ref float speed, ref int damage, ref float knockBack, ref int usedAmmoItemId, ref int projToShoot, ref float KnockBack)
            {

            }
            /// <summary>
            ///蓄力时更新
            /// </summary>
            public override void ChargeUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
            }
            /// <summary>
            /// 射箭但不是蓄力时更新
            /// </summary>  
            public override void NoChargeUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();

            }
            /// <summary>
            /// 蓄力完成后技能
            /// </summary>  
            public override void Skill(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
            }

            /// <summary>
            /// 🐍
            /// </summary>  
            public override void Shoot(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ranged.BowUsedAmmoItemId[0]);
                float la = ranged.BowTime / player.itemAnimationMax * 6;
                float Charge = ranged.BowTime / player.itemAnimationMax;
                float Magnification = Charge;
                if(ranged.Charge)
                {
                    Magnification *= 1.5F;
                }
                Vector2 Center = Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8);
                if (Projectile.owner == Main.myPlayer)
                {
                    if (ranged.Ammo[0] > 0)
                    {
                        //发射
                        int Proj = NewProjectileChange(Source, Center, Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Magnification), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                        Main.projectile[Proj].scale = Projectile.scale;
                        Main.projectile[Proj].Resize((int)(Main.projectile[Proj].OriginalWidth() * (Main.projectile[Proj].scale)), (int)(Main.projectile[Proj].OriginalHeight() * (Main.projectile[Proj].scale)));
                    }
                }
            }
            /// <summary>
            /// 在最后更新
            /// </summary>  
            public override void PostUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
            }
        }
    }
}