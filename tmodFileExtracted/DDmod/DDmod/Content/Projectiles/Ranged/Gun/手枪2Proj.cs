namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class 手枪2Proj : ModProjectile
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
        }

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
            if (Projectile.DProj().Back == 1)
            {
                Projectile.HoldProj(player, 10, (Projectile.localAI[0]) * -player.direction, Vector2.Zero, 0, 0, Projectile.ai[0] > 0, -1);
                Projectile.hide = true;
            }
            else
            {
                if (Projectile.ai[0] > 0)
                {
                    Projectile.HoldProj(player, 10, (Projectile.localAI[0]) * -player.direction, Vector2.Zero, 0, 0, Projectile.ai[0] > 0, -1);
                }
                else
                {
                    Projectile.HoldProj(player, 10, (Projectile.localAI[0]) * -player.direction, Vector2.Zero, 0, 0, Projectile.ai[0] > 0, 0);
                }
            }
            if (canShoot)
            {
                if (Projectile.ai[0] <= 0)
                {
                    火 = 1;
                    player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int Damage, out float KnockBack, out int usedAmmoItemId);

                    IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, usedAmmoItemId);
                    KnockBack = player.GetWeaponKnockback(player.inventory[player.selectedItem], KnockBack);

                    NewProjectile(Source, Projectile.Center - new Vector2(0, 12 * player.direction).RotatedBy(Projectile.rotation), Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);
                    PlaySound(SoundID.Item41, Projectile.position);
                    if (Main.netMode != 2)
                    {
                        int GoreType = Mod.Find<ModGore>("弹药壳").Type;
                        Gore.NewGore(player.GetSource_Death(), Projectile.Center - new Vector2(0, 12 * player.direction).RotatedBy(Projectile.rotation), (player.itemRotation).ToRotationVector2() * -4 * player.direction, GoreType, 1.3f);
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
            Vector2 Center = Utils.RotatedBy(new Vector2(0, 6), Projectile.rotation, default);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D 枪口 = DDTextures.GunFlames.Value;
            Player player = Main.player[Projectile.owner];
            if (火 > 0)
            {
                Rectangle rectangle = new Rectangle(0, 枪口.Height / 4 * 1, 枪口.Width, 枪口.Height / 4);
                Vector2 vector = new Vector2(-texture.Width / 4 - rectangle.Width / 2 + 2, rectangle.Height / 2 +14 * player.direction);
                if (player.direction == 1)
                {
                    Main.spriteBatch.Draw(枪口, Projectile.Center - Main.screenPosition, rectangle, new Color(255, 255, 255, 255), Projectile.rotation, vector, Projectile.scale, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(枪口, Projectile.Center - Main.screenPosition, rectangle, new Color(255, 255, 255, 255), Projectile.rotation, vector, Projectile.scale, 0, 0f);
                }
            }
            else
            {
                Projectile.frame = 0;
            }
            
            if (Main.player[Projectile.owner].direction == 1)
            {
                Rectangle? rectangle = new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, texture.OffsetCenter(2, 1) - new Vector2(6, -4), Projectile.scale, 0, 0f);
            }
            else
            {
                Rectangle? rectangle = new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation + MathHelper.Pi, texture.OffsetCenter(2, 1) + new Vector2(6, 4), Projectile.scale, (SpriteEffects)1, 0f);
            }

            return false;
        }
    }
}