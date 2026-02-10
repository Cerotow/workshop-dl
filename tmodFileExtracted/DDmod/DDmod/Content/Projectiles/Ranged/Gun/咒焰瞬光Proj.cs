using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria;


namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class 咒焰瞬光Proj : ModProjectile
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
            Gun.Y = 8;
            Gun.X = 2;
            Gun2.Y = 14;
            Gun2.X = 2;
        }
        public Vector2 Gun;
        public Vector2 Gun2;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.localAI[0] > 0f)
            {
                Projectile.localAI[0] -= 0.1F;
            }
            else
            {
                Projectile.localAI[0] = 0f;
            }
            bool canShoot = player.controlUseItem && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;

            Vector2 vector = player.Dplayer().MouseWorld;
            if ((vector - Projectile.Center).Length()<100)
            {
                vector += (vector - player.Center).PerfectNormalize() * 100;
            }
            float targetRotation = (vector - player.Center).ToRotation();

            player.ChangeDir((vector.X - player.Center.X) >=0?1:-1);

            if (Projectile.DProj().Back == 1)
            {
                Projectile.rotation = targetRotation;

                Vector2 muzzleOffset = new Vector2(0, Gun.Y * -player.direction).RotatedBy(Projectile.rotation);
                Vector2 muzzlePosition = Projectile.Center + muzzleOffset;

                Projectile.DProj().vector[0] = (vector - muzzlePosition).SafeNormalize(Vector2.UnitX);

                Projectile.HoldProj(player, 10, (Projectile.localAI[0]) * -player.direction, Projectile.DProj().vector[0], 0, 0, Projectile.ai[0] > 0, -1, false);
                Projectile.hide = true;
            }
            else
            {
                Projectile.rotation = targetRotation;

                Vector2 muzzleOffset = new Vector2(0, Gun2.Y * -player.direction).RotatedBy(Projectile.rotation);
                Vector2 muzzlePosition = Projectile.Center + muzzleOffset;

                Projectile.DProj().vector[0] = (vector - muzzlePosition).SafeNormalize(Vector2.UnitX);
                if (Projectile.ai[0] > 0)
                {
                    Projectile.HoldProj(player, 10, (Projectile.localAI[0]) * -player.direction, Projectile.DProj().vector[0], 0, 0, Projectile.ai[0] > 0, -1,false);
                }
                else
                {
                    Projectile.HoldProj(player, 10, (Projectile.localAI[0]) * -player.direction, Projectile.DProj().vector[0], 0, 0, Projectile.ai[0] > 0, 0, false);
                }
            }
            if (canShoot&&(Projectile.DProj().track<10|| player.ownedProjectileCounts[Type]>=2))
            {
                if (Projectile.ai[0] <= 0)
                {
                    火 = 1;
                    player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int Damage, out float KnockBack, out int usedAmmoItemId);

                    IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, usedAmmoItemId);
                    KnockBack = player.GetWeaponKnockback(player.inventory[player.selectedItem], KnockBack);

                    if (Projectile.DProj().Back == 1)
                    {
                        projToShoot =ModContent.ProjectileType<强化咒火弹Proj>();

                        NewProjectile(Source, Projectile.Center + new Vector2(0, Gun.Y * -player.direction).RotatedBy(Projectile.rotation), Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);
                        PlaySound(SoundID.Item41, Projectile.position);
                        if (Main.netMode != 2)
                        {
                            int GoreType = Mod.Find<ModGore>("弹药壳").Type;
                            Gore.NewGore(player.GetSource_Death(), Projectile.Center + new Vector2(0, Gun.Y*-player.direction).RotatedBy(Projectile.rotation), (player.itemRotation).ToRotationVector2() * -4 * player.direction, GoreType, 1.3f);
                        }
                    }
                    else
                    {
                        projToShoot = ModContent.ProjectileType<强化激光弹Proj>();
                        NewProjectile(Source, Projectile.Center + new Vector2(0, Gun2.Y * -player.direction).RotatedBy(Projectile.rotation), Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);
                        PlaySound(SoundID.Item157, Projectile.position);
                        if (Main.netMode != 2)
                        {
                            int GoreType = Mod.Find<ModGore>("弹药壳").Type;
                            Gore.NewGore(player.GetSource_Death(), Projectile.Center + new Vector2(0, Gun2.Y * -player.direction).RotatedBy(Projectile.rotation), (player.itemRotation).ToRotationVector2() * -4 * player.direction, GoreType, 1.3f);
                        }
                    }
                    Projectile.ai[0] += player.HeldItem.useAnimation;
                    if (Projectile.localAI[0] <= 0.4f)
                    {
                        Projectile.localAI[0] += 0.4F;
                    }
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                if (Projectile.ai[0] <= 0)
                {
                    Projectile.Kill();
                }
            }
            火 -= 0.5f;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
        float 火;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D 枪口 = DDTextures.GunFlames.Value;
            Player player = Main.player[Projectile.owner];
            SpriteEffects sprite = 0;
            Vector2 v = -new Vector2(-texture.Width / 4 + 8, texture.Height / 2 - 4);
            if (Main.player[Projectile.owner].direction == -1)
            {
                v = new Vector2(texture.Width / 4 - 8, texture.Height / 2 - 4);
                sprite = SpriteEffects.FlipVertically;
            }
            if (火 > 0)
            {
                Rectangle rectangle = new Rectangle(0, 枪口.Height / 4 * 1, 枪口.Width, 枪口.Height / 4);

                if (Projectile.DProj().Back == 1)
                {
                    if (player.direction == 1)
                    {
                        Vector2 vector = new Vector2(-texture.Width / 2 + 枪口.Width- Gun.X, Gun.Y + rectangle.Height / 2 +1);
                        Main.spriteBatch.Draw(枪口, Projectile.Center - Main.screenPosition, rectangle, new Color(55, 200, 66, 100), Projectile.rotation, vector, Projectile.scale, 0, 0f);
                    }
                    else
                    {
                        Vector2 vector = new Vector2(-texture.Width / 2 + 枪口.Width - Gun.X, -Gun.Y + rectangle.Height / 2 - 1);
                        Main.spriteBatch.Draw(枪口, Projectile.Center - Main.screenPosition, rectangle, new Color(55, 200, 66, 100), Projectile.rotation, vector, Projectile.scale, 0, 0f);
                    }
                }
                else
                {
                    if (player.direction == 1)
                    {
                        Vector2 vector = new Vector2(-texture.Width / 2 + 枪口.Width - Gun2.X, Gun2.Y + rectangle.Height / 2 - 1);
                        Main.spriteBatch.Draw(枪口, Projectile.Center - Main.screenPosition, rectangle, new Color(233, 51, 51, 100), Projectile.rotation, vector, Projectile.scale, 0, 0f);
                    }
                    else
                    {
                        Vector2 vector = new Vector2(-texture.Width / 2 + 枪口.Width - Gun2.X, -Gun2.Y + rectangle.Height / 2 + 1);
                        Main.spriteBatch.Draw(枪口, Projectile.Center - Main.screenPosition, rectangle, new Color(233, 51, 51, 100), Projectile.rotation, vector, Projectile.scale, 0, 0f);
                    }
                }
            }
            else
            {
                Projectile.frame = 0;
            }
            if (Projectile.DProj().Back == 1)
            {
                Rectangle? rectangle = new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height));
               Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, texture.OffsetCenter(2, 1) - v, Projectile.scale, sprite, 0f);
            }
            else
            {
                Rectangle? rectangle = new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, texture.OffsetCenter(2, 1) - v, Projectile.scale, sprite, 0f);
            }

            return false;
        }
    }
}