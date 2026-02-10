using DDmod.Content.Dusts;
using DDmod.Content.Items;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Players;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Ranged.Bow
{
    public class Daedalus : BowTemplate
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.hide = false;
        }
        public int A;
        public override bool PreAI()
        {
            //伤害
            Player player = Main.player[Projectile.owner];
            AC(player);
            int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
            Projectile.damage = damageWithChargeAndStats;
            RangedProjectile ranged = Projectile.GetGlobalProjectile<RangedProjectile>();
            ranged.RightClick = player.controlUseTile;
            Item item = Projectile.Player().ActiveItem();
            player.itemTimeMax = item.useAnimation;
            player.itemAnimationMax = item.useAnimation;
            if (player.itemAnimationMax <= 0)
            {
                player.itemTimeMax = item.useAnimation;
                player.itemAnimationMax = item.useAnimation;
            }
            if (item.type <= ItemID.None)
            {
                Projectile.Kill();
                return false;
            }
            RangedGlobalItem rangedItem = item.GetGlobalItem<RangedGlobalItem>();
            Lighting.AddLight(Projectile.Center, rangedItem.BowLight);
            if (!rangedItem.Bow)
            {
                Projectile.Kill();
            }
            Projectile.HoldProj(player, 26, 0, new Vector2(player.Dplayer().MouseWorld.X,player.Center.Y)- player.Center - new Vector2(0,1000), 0, 0, true, 1, false);
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            player.heldProj = Projectile.whoAmI;

            if (!ranged.ShootShop)
            {
                Take--;
                if (Take > 0)
                {
                    float v = MathHelper.PiOver2 + MathHelper.Pi;
                    //if (player.direction == -1) v += MathHelper.Pi;
                    player.PlayerAction().PlayerArmRotation(Projectile.velocity.ToRotation() * player.gravDir + v - player.fullRotation, Player.CompositeArmStretchAmount.Full);
                    player.PlayerAction().PlayerArmRotationBack(Projectile.velocity.ToRotation() * player.gravDir + v - player.fullRotation, Player.CompositeArmStretchAmount.Full);
                    player.ChangeDir(Projectile.direction);
                    Projectile.HoldProj(player, 26, 0, new Vector2(player.Dplayer().MouseWorld.X, player.Center.Y) - player.Center - new Vector2(0, 1000), 0, 0, true, 1, false);
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                }
                else
                {
                    if (player.direction == 1)
                    {
                        Projectile.HoldProj(player, 18, 0, new Vector2(1, 0).RotatedBy(-0.8f + player.fullRotation), 0, 0, true, 1, false);
                    }
                    else
                    {
                        Projectile.HoldProj(player, 18, 0, new Vector2(1, 0).RotatedBy(MathHelper.Pi + 0.8f + player.fullRotation), 0, 0, true, 1, false);
                    }
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    Projectile.Center = player.Center - new Vector2(4 * player.direction, 4 - player.gfxOffY);
                    player.heldProj = -1;
                }
                //射出僵直动画
                if (ranged.BowAnimationTime > 0)
                {
                    ranged.BowAnimationTime--;
                }
                if (!Projectile.DProj().Bool[0])
                {
                    ranged.BowTime -= 8;
                    if (ranged.BowTime < -4)
                    {
                        Projectile.DProj().Bool[0] = true;
                    }
                }
                if (Projectile.DProj().Bool[0])
                {
                    ranged.BowTime += 4f;
                    if (ranged.BowTime > 0)
                    {
                        ranged.BowTime = 0;
                    }
                }
                //僵直完成
                if (ranged.BowTime == 0 && ranged.BowAnimationTime <= 0 && (player.controlUseItem)&& !player.HasBuff(23))
                {
                    Projectile.HoldProj(player, 26, 0, new Vector2(player.Dplayer().MouseWorld.X, player.Center.Y) - player.Center - new Vector2(0, 1000), 0, 0, true, 1, false);
                    ranged.ShootShop = true;
                    ranged.BowAnimationTime = 8;
                    ranged.Ammo[0] = 0;
                    ranged.Ammo[1] = 0;
                    ranged.Ammo[2] = 0;
                }
            }
            else
            {
                player.ChangeDir(Projectile.direction);
                Take = 120;

                //player.bodyFrame.Y = player.bodyFrame.Height * 3;
                //player.itemTime = (int)(player.ActiveItem().useAnimation * (ranged.BowTime / player.itemAnimationMax / 2));
                //player.itemAnimation = (int)(player.ActiveItem().useAnimation * (ranged.BowTime / player.itemAnimationMax / 2));
                float BT = (ranged.BowTime / player.itemAnimationMax / 2);
                Player.CompositeArmStretchAmount amount;
                if (BT < 0.3F)
                {
                    amount = (Player.CompositeArmStretchAmount)0;
                }
                else if (BT < 0.6F)
                {
                    amount = (Player.CompositeArmStretchAmount)3;
                }
                else if (BT < 0.8F)
                {
                    amount = (Player.CompositeArmStretchAmount)2;
                }
                else
                {
                    amount = (Player.CompositeArmStretchAmount)1;
                }
                player.PlayerAction().PlayerArmRotation(Projectile.velocity.ToRotation() * player.gravDir - MathHelper.PiOver2 - player.fullRotation, amount);
                player.PlayerAction().PlayerArmRotationBack(Projectile.velocity.ToRotation() * player.gravDir - MathHelper.PiOver2 - player.fullRotation, Player.CompositeArmStretchAmount.Full);

                if (player.itemTime < 2 || player.itemAnimation < 2)
                {
                    player.itemTime = 2;
                    player.itemAnimation = 2;
                }
                if (ranged.BowTime <= 0)
                {
                    if (ranged.Ammo[0] == 0)
                    {
                        bool ThereAmmo = player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId);

                        if (!ThereAmmo)
                        {
                            ranged.ShootShop = false;
                            ranged.BowTime = 0;
                            Projectile.DProj().Times[2] = 0;
                            ranged.SpecialEffect = false;
                            ranged.Charge = false;
                        }
                        ranged.BowSpeed[0] = speed;
                        ranged.BowDamage[0] = damage;
                        ranged.BowKnockBack[0] = knockBack;
                        ranged.BowUsedAmmoItemId[0] = usedAmmoItemId;
                        A = projToShoot;
                        ranged.Ammo[0] = ModContent.ProjectileType<ArrowofHeaven>();
                        ranged.BowKnockBack[0] = player.GetWeaponKnockback(player.inventory[player.selectedItem], ranged.BowKnockBack[0]);
                    }
                    ranged.BowTime += player.GetTotalAttackSpeed(DamageClass.Ranged);
                }
                else if (ranged.BowTime > 0 && ranged.BowTime < player.itemAnimationMax * 2)
                {
                    ranged.BowTime += player.GetTotalAttackSpeed(DamageClass.Ranged) * 2;

                }
                if (ranged.BowTime > player.itemAnimationMax*2)
                {
                    ranged.BowTime = player.itemAnimationMax*2;
                }
                if ((ranged.BowTime >= player.itemAnimationMax*2 && !ranged.Charge))
                {

                    float la = ranged.BowTime / player.itemAnimationMax * 6;
                    IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ranged.BowUsedAmmoItemId[0]);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        int Proj1 = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * 10, ranged.Ammo[0], (int)(ranged.BowDamage[0]), ranged.BowKnockBack[0], Projectile.owner, 0, 0, 0);
                        Main.projectile[Proj1].scale = Projectile.scale;
                        Main.projectile[Proj1].DProj().Times[0] = A;
                        Main.projectile[Proj1].Resize((int)(Main.projectile[Proj1].OriginalWidth() * (Main.projectile[Proj1].scale)), (int)(Main.projectile[Proj1].OriginalHeight() * (Main.projectile[Proj1].scale)));
                    }
                    ranged.ShootShop = false;
                    ranged.BowTime = 0;
                    PlaySound(SoundID.Item5, Projectile.position);
                    ranged.Ammo[0] = 0;
                    ranged.Ammo[1] = 0;
                    ranged.Ammo[2] = 0;
                    Projectile.DProj().Times[2] = 0;
                    ranged.SpecialEffect = false;
                    ranged.Charge = false;
                }
            }
            if (player.dead)
            {
                Projectile.Kill();
            }
            Projectile.hide = player.heldProj >= 0;
            Projectile.spriteDirection = player.direction == 1 ? 0 : 1;
            if (Projectile.spriteDirection == 1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
            Projectile.netUpdate = true;
            return false;
        }
    }
}