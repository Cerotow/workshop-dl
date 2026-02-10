using DDmod.Content.Projectiles.GeneralProj;

namespace DDmod.Content.Projectiles.Magic.Gun
{
    public class 破坏者激光枪Proj : ModProjectile
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
            //projectile.light = 0.50f;
            Main.projFrames[Projectile.type] = 8;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner]; 
            Projectile.DProj().Times[2]--;
            Projectile.HoldProj(player, 24, 0, Vector2.Zero, 0, 0, Projectile.DProj().Times[0] < 10);
            player.heldProj = Projectile.whoAmI;
            bool canShoot = player.channel && player.statMana >= player.ItemMana() && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;
            if (canShoot)
            {
                if (Projectile.ai[0] > player.HeldItem.useAnimation)
                {
                    Projectile.DProj().Times[0]++;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        int A = player.ItemMana();
                        player.statMana -= A;
                    }
                    if (Projectile.DProj().Times[0] < 5)
                    {
                        if (Projectile.owner == Main.myPlayer)
                            Main.projectile[NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.PerfectNormalize() * player.HeldItem.shootSpeed, ModContent.ProjectileType<破坏者激光>(), Projectile.damage, Projectile.knockBack, Projectile.owner)].DamageType = DamageClass.Magic;
                        PlaySound(SoundID.Item158, Projectile.position);

                        Projectile.DProj().Times[2] = 3;
                    }
                    else
                    {
                        if (Projectile.owner == Main.myPlayer)
                        {
                            int a = NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * 24, Projectile.velocity.PerfectNormalize() * player.HeldItem.shootSpeed, ModContent.ProjectileType<魔法破坏者激光束>(), (int)(Projectile.damage * 1.6f), Projectile.knockBack, Projectile.owner, 0, 1, 1.6f);
                            Projectile.DProj().Times[0] = 0;
                        }
                        PlaySound(SoundID.Item157, Projectile.position);
                    }
                    Projectile.ai[0] -= player.HeldItem.useAnimation;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                player.itemTime = 0;
                player.itemAnimation = 0;
                Projectile.Kill();
            }
            Projectile.frame++;
            if(Projectile.frame>32)
            {
                Projectile.frame = 0;
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
            Vector2 Center = Utils.RotatedBy(new Vector2(0, 6), Projectile.rotation, default);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Rectangle rectangle = new Rectangle(0,texture.Height/ Main.projFrames[Projectile.type]*(Projectile.frame/4),texture.Width,texture.Height/ Main.projFrames[Projectile.type]); 

            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation + MathHelper.Pi, rectangle.Size() / 2, Projectile.scale, (SpriteEffects)1, 0f);
            }
            if (Projectile.DProj().Times[2] > 0)
            {

                Texture2D texture2 = DDTextures.Starlight3.Value;
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26, null, new Color(252, 128, 48, 00), 0, texture2.Size() / 2, Projectile.scale * new Vector2(1, 2) / 2, 0, 0);
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26, null, new Color(252, 128, 48, 00), MathHelper.PiOver2, texture2.Size() / 2, Projectile.scale * new Vector2(1, 3) / 2, 0, 0);

            }

            return false;
        }
    }
}