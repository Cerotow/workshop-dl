namespace DDmod.Content.Projectiles.Magic.Gun
{
    public class MeteorLaserGun : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.HoldProj(player, 24, 0, Vector2.Zero, 0, 0, Projectile.DProj().Times[0] < 10);
            player.heldProj = Projectile.whoAmI;
            bool canShoot = player.channel && player.statMana >= player.ItemMana() && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;
            if(Projectile.DProj().Times[2]>0)
            {
                Projectile.DProj().Times[2]--;
            }
            if (canShoot)
            {
                int Use = (int)(player.HeldItem.useAnimation / player.GetAttackSpeed(Projectile.DamageType));
                if (Projectile.ai[0] > player.HeldItem.useTime)
                {
                    if (Projectile.DProj().Times[0] == 0 && Projectile.owner == Main.myPlayer)
                    {
                        int A = player.ItemMana();
                        player.statMana -= A;
                    }
                    Projectile.DProj().Times[0]++;
                    if (Projectile.DProj().Times[0] % (Use/6) == 0)
                    {
                        if (Projectile.DProj().Times[0] <= Use/2)
                        {
                            if (Projectile.owner == Main.myPlayer)
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.PerfectNormalize() * player.HeldItem.shootSpeed, 20, Projectile.damage, Projectile.knockBack, Projectile.owner);
                            PlaySound(SoundID.Item158, Projectile.position);
                            Projectile.DProj().Times[2] = 3;
                        }
                    }
                    if (Projectile.DProj().Times[0] >= Use)
                    {
                        if (Projectile.owner == Main.myPlayer)
                        {
                            int a = NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.PerfectNormalize() * player.HeldItem.shootSpeed, ModContent.ProjectileType<GreenLaser>(), (int)(Projectile.damage * 1.6f), Projectile.knockBack, Projectile.owner, 0, 1, 1.6f);
                        }
                        PlaySound(SoundID.Item157, Projectile.position);
                        Projectile.ai[0] -= Use;
                        Projectile.DProj().Times[0] = 0;
                        Projectile.DProj().Times[2] = -3;
                    }
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                player.itemTime = 0;
                player.itemAnimation = 0;
                Projectile.Kill();
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
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 Center = Utils.RotatedBy(new Vector2(0, 4), Projectile.rotation, default);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Center = Utils.RotatedBy(new Vector2(0, -4), Projectile.rotation, default);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)1, 0f);
            }
            if (Projectile.DProj().Times[2] > 0)
            {

                Texture2D texture2 = DDTextures.Starlight3.Value;
                Main.spriteBatch.Draw(texture2, Projectile.Center - Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26, null, new Color(96, 248, 2, 00), 0, texture2.Size() / 2, Projectile.scale * new Vector2(1, 2) / 2, 0, 0);
                Main.spriteBatch.Draw(texture2, Projectile.Center - Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26, null, new Color(96, 248, 2, 00), MathHelper.PiOver2, texture2.Size() / 2, Projectile.scale * new Vector2(1, 3) / 2, 0, 0);

            }
            return false;
        }
    }
}