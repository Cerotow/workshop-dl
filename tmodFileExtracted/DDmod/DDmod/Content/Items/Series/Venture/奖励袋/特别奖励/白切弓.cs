using System;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.Venture.奖励袋.特别奖励
{
	public class 白切弓 : ModItem
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
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileID.IchorArrow;
			Item.shootSpeed = 10f;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = 40;
			Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            ModifyBow.Load(Type, new Modify(), true);
            Item.GetGlobalItem<RangedGlobalItem>().SetString(5, 5, 3, 0, 4,true, new Color(255, 255, 255,100));
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
            CreateRecipe(1).AddIngredient(ModContent.ItemType<白切精华>()).Register();
        }
        public class Modify : ModifyBow
        {
            public override void Skill(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                if (ranged.BowTime > player.itemAnimationMax * 1.8f)
                {
                    ranged.SpecialEffect = true;
                    //远古弓
                    if (ranged.Ammo[0] > 0 && ranged.Ammo[0] != ModContent.ProjectileType<白切神箭>())
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, ModContent.DustType<光球粒子>(),0,0,0,new Color(255,255,255,100))];
                            dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8F, 2.2F);
                            dust.noGravity = true;
                            dust.alpha = 100;
                            dust.scale = 1.2f;
                        }
                        ranged.Ammo[0] = ModContent.ProjectileType<白切神箭>();
                    }
                }
            }
            public override void Shoot(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ranged.BowUsedAmmoItemId[0]);
                float la = ranged.BowTime / player.itemAnimationMax * 6;
                float Charge = ranged.BowTime / player.itemAnimationMax;
                float Magnification = Charge;

                    if (Projectile.owner == Main.myPlayer)
                {
                    //发射
                    int Proj1 = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                    Main.projectile[Proj1].scale = Projectile.scale;
                    Main.projectile[Proj1].Resize((int)(Main.projectile[Proj1].OriginalWidth() * (Main.projectile[Proj1].scale)), (int)(Main.projectile[Proj1].OriginalHeight() * (Main.projectile[Proj1].scale)));
                }
            }
        }
    }
}