namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class 迷你鲨2Proj : ModProjectile
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
            Projectile.localAI[1] =3.3f;
        }

        public override bool PreAI()
        {
            火 -= 0.5F;
            Player player = Main.player[Projectile.owner];
            Projectile.HoldProj(player, 10, (Projectile.localAI[0]) * -player.direction, Vector2.Zero, 0, 0, Projectile.ai[0] > 0, -1);
            Projectile.localAI[0]*=0.92F;
            bool canShoot = player.channel && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;
            if (canShoot)
            {
                if (Projectile.ai[0] <= 0)
                {
                    火 = 1;
                    player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int Damage, out float KnockBack, out int usedAmmoItemId);

                    IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, usedAmmoItemId);
                    KnockBack = player.GetWeaponKnockback(player.inventory[player.selectedItem], KnockBack);

                    NewProjectile(Source, Projectile.Center+new Vector2(0,4*player.direction).RotatedBy(Projectile.rotation), Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);

                    PlaySound(SoundID.Item11, Projectile.position);
                    if (Main.netMode != 2)
                    {
                        int GoreType = Mod.Find<ModGore>("弹药壳").Type;
                        Gore.NewGore(player.GetSource_Death(), player.Center + (player.itemRotation).ToRotationVector2(), (player.itemRotation).ToRotationVector2() * -4 * player.direction, GoreType, 1.3f);
                    }
                    Projectile.localAI[0] = Main.rand.NextFloat(-0.2F, 0.2F);
                    Projectile.ai[0] += player.HeldItem.useAnimation* Projectile.localAI[1];
                    player.velocity -= Projectile.velocity.PerfectNormalize()*0.6f;
                    if (Projectile.localAI[1] > 1)
                    {
                        Projectile.localAI[1] -= Projectile.localAI[1]/10;
                    }
                    else
                    {
                        Projectile.localAI[1] = 1;
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
                Rectangle rectangle = new Rectangle(0, 枪口.Height/4*1, 枪口.Width,枪口.Height/4);
                Vector2 vector = new Vector2(-texture.Width / 4-rectangle.Width/2+2, rectangle.Height / 2-3*player.direction);
                if (player.direction == 1)
                {
                    Main.spriteBatch.Draw(枪口, Projectile.Center - Main.screenPosition, rectangle, new Color(255,255,255,255) * 火, Projectile.rotation, vector, Projectile.scale, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(枪口, Projectile.Center - Main.screenPosition, rectangle, new Color(255, 255, 255, 255) * 火, Projectile.rotation, vector, Projectile.scale, 0, 0f);
                }
            }

            if (Main.player[Projectile.owner].direction == 1)
            {
                Rectangle? rectangle = new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, texture.OffsetCenter(2, 1) - new Vector2(6, 0), Projectile.scale, 0, 0f);
            }
            else
            {
                Rectangle? rectangle = new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation + MathHelper.Pi, texture.OffsetCenter(2, 1) + new Vector2(6, 0), Projectile.scale, (SpriteEffects)1, 0f);
            }

            return false;
        }
    }
}