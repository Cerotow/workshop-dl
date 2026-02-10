using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.GeneralProj;

namespace DDmod.Content.Projectiles.Magic.Gun
{
    public class 机械激光加特林Proj : ModProjectile
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
            Projectile.DProj().Times[2]-=0.5F;

            Vector2 vector = player.Dplayer().MouseWorld;
            if ((vector - Projectile.Center).Length() < 100)
            {
                vector += (vector - player.Center).PerfectNormalize() * 100;
            }
            float targetRotation = (vector - player.Center).ToRotation();

            player.ChangeDir((vector.X - player.Center.X) >= 0 ? 1 : -1);

                Projectile.rotation = targetRotation;

                Vector2 muzzleOffset = new Vector2(0, 14 * player.direction).RotatedBy(Projectile.rotation);

                Projectile.DProj().vector[0] = (vector - Projectile.Center).SafeNormalize(Vector2.UnitX);
                Projectile.HoldProj(player, 20, 0, Projectile.DProj().vector[0], 0, 0, Projectile.DProj().Times[0] < 10);
            Projectile.position += muzzleOffset;

            bool canShoot = player.channel && player.statMana >= player.ItemMana() && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;
            if (Projectile.DProj().Times[0] == 0)
            {
                Projectile.DProj().Times[0] = player.HeldItem.useAnimation * 8;
            }
            if (Projectile.DProj().Times[0] > player.HeldItem.useAnimation)
            {
                Projectile.DProj().Times[0] -= player.HeldItem.useAnimation / 60F;
            }
            else
            {
                Projectile.DProj().Times[0] = player.HeldItem.useAnimation;
            }
            if (canShoot)
            {
                if (Projectile.ai[2]>=5)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        int A = player.ItemMana();
                        player.statMana -= A;
                    }
                    Projectile.ai[2] -= 5;
                }
                if (Projectile.ai[0] > Projectile.DProj().Times[0])
                {

                    Projectile.ai[2]++;
                    if (Projectile.owner == Main.myPlayer)
                        Main.projectile[NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.04F, 0.04F)) * player.HeldItem.shootSpeed*Main.rand.NextFloat(0.75F,1), ModContent.ProjectileType<机械激光>(), Projectile.damage, Projectile.knockBack, Projectile.owner)].DamageType = DamageClass.Magic;
                    PlaySound(SoundID.Item157, Projectile.position);

                    Projectile.DProj().Times[2] = 3;

                    Projectile.ai[0] -= Projectile.DProj().Times[0];

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
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Rectangle rectangle = new Rectangle(0,texture.Height/5 * ((Projectile.frame/3)%5),texture.Width,texture.Height/5);
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, (SpriteEffects)2, 0f);
            }
            Texture2D texture2 = DDTextures.Starlight3.Value;
            if (Projectile.DProj().Times[2] > 0)
            {
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 40, null, new Color(255, 100, 100, 00), 0, texture2.Size() / 2, Projectile.scale * new Vector2(1, 2) / 4 * Projectile.DProj().Times[2], 0, 0);
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 40, null, new Color(255, 100, 100, 00), MathHelper.PiOver2, texture2.Size() / 2, Projectile.scale * new Vector2(1, 2) / 4 * Projectile.DProj().Times[2], 0, 0);
            }
            return false;
        }
    }
}