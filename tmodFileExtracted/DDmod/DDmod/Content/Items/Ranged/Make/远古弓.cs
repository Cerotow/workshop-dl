using System;
using DDmod.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.Make
{
	public class 远古弓 : ModItem
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
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = 2;
			Item.shoot = ProjectileID.IchorArrow;
			Item.shootSpeed = 10f;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = 40;
			Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            ModifyBow.Load(Type, new Modify(), true);
            Item.GetGlobalItem<RangedGlobalItem>().SetString(5, 5, 3, 0, 6,true, new Color(122, 141, 207));
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
        public class Modify : ModifyBow
        {
            public override void Skill(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                if (ranged.BowTime > player.itemAnimationMax * 1.8f)
                {
                    ranged.SpecialEffect = true;
                    //远古弓
                    if (ranged.Ammo[0] > 0 && ranged.Ammo[0] != ModContent.ProjectileType<远古魔法箭>())
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 172)];
                            dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8F, 2.2F);
                            dust.noGravity = true;
                            dust.alpha = 100;
                            dust.scale = 1.8f;
                        }
                        ranged.Ammo[0] = ModContent.ProjectileType<远古魔法箭>();
                    }
                }
            }
            public override void Shoot(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ranged.BowUsedAmmoItemId[0]);
                float la = ranged.BowTime / player.itemAnimationMax * 6;
                float Charge = ranged.BowTime / player.itemAnimationMax;
                if (Charge > 1 && rangedItem.Skill == 0) Charge *= 1.5f;
                float Magnification = Charge;

                if (ranged.BowTime / player.itemAnimationMax > 1.8F)
                {
                    for (int a = -1; a <= 1; a += 2)
                    {
                        float R = Main.rand.NextFloat(150, 250);
                        Vector2 Center = Projectile.Center - Projectile.velocity.PerfectNormalize().RotatedBy(a) * R;
                        Vector2 vector = (player.Dplayer().MouseWorld - Center).PerfectNormalize() * ranged.BowSpeed[0] * Charge;
                        for (int i = 0; i < 40; i++)
                        {
                            Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.8F, 4.2F);
                            Dust dust = Main.dust[NewDust(Center - new Vector2(4), 1, 1, 172)];
                            dust.velocity = projDirection;
                            dust.noGravity = true;
                            dust.alpha = 100;
                            dust.scale = 1.5f;
                        }
                        int Proj = NewProjectileChange(Source, Center, vector / 2, ModContent.ProjectileType<远古能量>(), (int)(ranged.BowDamage[0] * Charge) / 2, ranged.BowKnockBack[0] * Charge, Projectile.owner, 1, 0, Magnification);
                        Main.projectile[Proj].DProj().Times[0] = R;
                        Main.projectile[Proj].extraUpdates = 4;
                    }
                }
                else if (!ranged.Charge)
                {
                    float R = Main.rand.NextFloat(150, 250);
                    Vector2 Center = Projectile.Center - Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2)) * R;
                    Vector2 vector = (player.Dplayer().MouseWorld - Center).PerfectNormalize() * ranged.BowSpeed[0] * Charge;
                    for (int i = 0; i < 40; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.8F, 4.2F);
                        Dust dust = Main.dust[NewDust(Center - new Vector2(4), 1, 1, 172)];
                        dust.velocity = projDirection;
                        dust.noGravity = true;
                        dust.alpha = 100;
                        dust.scale = 1.5f;
                    }
                    int Proj = NewProjectileChange(Source, Center, vector / 2, ModContent.ProjectileType<远古能量>(), (int)(ranged.BowDamage[0] * Charge) / 2, ranged.BowKnockBack[0] * Charge, Projectile.owner, 2, 0, Magnification);
                    Main.projectile[Proj].DProj().Times[0] = R - vector.Length() * 3;
                }

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