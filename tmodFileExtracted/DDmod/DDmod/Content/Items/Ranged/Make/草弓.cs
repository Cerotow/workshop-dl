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
	public class 草弓 : ModItem
	{
		public override void Load()
		{
		}
		public override void SetDefaults()
		{
			Item.damage = 13;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 38;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.rare = 2;
			Item.shoot = ProjectileID.IchorArrow;
			Item.shootSpeed = 9f;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = 40;
			Item.GetGlobalItem<RangedGlobalItem>().Bow = true;

            ModifyBow.Load(Type, new Modify(),true);
			Item.GetGlobalItem<RangedGlobalItem>().SetString(5,5,3,0,6,false, new Color(184, 104, 101));
		}

		public override void SetStaticDefaults()
		{

            if (Main.netMode != 2)
				DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
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
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(210, 10).AddIngredient(331, 12).AddTile(TileID.Anvils).Register();
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
                if (Charge > 1 && rangedItem.Skill == 0) Charge = 1.5f;
                float Magnification = Charge;

                Vector2 Center = Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8);
                if (Projectile.owner == Main.myPlayer)
                {
                    if (ranged.BowTime / player.itemAnimationMax > 1.8F)
                    {
                        Vector2 vector = (player.Dplayer().MouseWorld - Center).PerfectNormalize() * ranged.BowSpeed[0] * Charge;
                        Center -= new Vector2(0, 10 * 3).RotatedBy(Projectile.rotation);
                        for (int a = -2; a <= 2; a++)
                        {
                            Center += new Vector2(0, 10).RotatedBy(Projectile.rotation);
                            NewProjectileChange(Source, Center - new Vector2(Math.Abs(a) * 8, 0).RotatedBy(Projectile.rotation), vector / 2, ModContent.ProjectileType<叶箭>(), (int)(ranged.BowDamage[0] * Charge) / 4, ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                        }
                        ranged.Ammo[0] = 0;
                    }
                    else if (!ranged.Charge)
                    {

                        Vector2 vector = (player.Dplayer().MouseWorld - Center).PerfectNormalize() * ranged.BowSpeed[0] * Charge;
                        Center += new Vector2(0, Main.rand.NextFloat(-20, 20)).RotatedBy(Projectile.rotation);

                        NewProjectileChange(Source, Center, vector.PerfectNormalize() * 14, ModContent.ProjectileType<叶箭>(), (int)(ranged.BowDamage[0] * Charge) / 4, ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                    }
                    if (ranged.Ammo[0] > 0)
                    {
                        //发射
                        int Proj = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
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