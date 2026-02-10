using DDmod.Content.Projectiles.GeneralProj;
using Terraria;

namespace DDmod.Content.Projectiles.Magic.Gun
{
    public class 绿岩手提灯Proj : ModProjectile
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
            Projectile.HoldProj(player, 0, 0, Vector2.Zero, 0, 0, Projectile.DProj().Times[0] < 10);
            Vector2 Center = Utils.RotatedBy(new Vector2(12, 8*player.direction), Projectile.rotation, default);
            Projectile.position += Center;
            player.heldProj = Projectile.whoAmI;
            bool canShoot = player.channel && player.statMana >= player.ItemMana() && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;
            if (canShoot)
            {
                if (Projectile.ai[0] > player.HeldItem.useTime)
                {
                    if (Projectile.DProj().Times[0] == 0 && Projectile.owner == Main.myPlayer)
                    {
                        int A = player.ItemMana();
                        player.statMana -= A;
                    }
                    if (Projectile.owner == Main.myPlayer)
                        NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, 4 * player.direction).RotatedBy(Projectile.rotation), Projectile.velocity.PerfectNormalize() * player.HeldItem.shootSpeed, ModContent.ProjectileType<绿岩能量>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -1, -1, 4);

                    Projectile.ai[0] -= player.HeldItem.useTime;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.Kill();
            }
            Lighting.AddLight(Projectile.Center, new Color(119, 237, 130).ToVector3() * 1);
            Vector2 vector = Projectile.Center + new Vector2(0, 4 * player.direction).RotatedBy(Projectile.rotation);
            /*
            Projectile.ai[2] = 70;
            for (int a= 0;a<70;a++)
            {
                if(Projectile.velocity.PerfectNormalize() * player.HeldItem.shootSpeed!= Collision.TileCollision(vector, Projectile.velocity.PerfectNormalize() * player.HeldItem.shootSpeed,1,1))
                {
                    Projectile.ai[2] = a-3;
                    break;
                }
                vector += Projectile.velocity.PerfectNormalize() * player.HeldItem.shootSpeed;
            }*/
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
            Player player = Main.player[Projectile.owner];
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation+MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)1, 0f);
            }
            texture = DDTextures.Scanning2.Value;
            Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(20, 4 * player.direction).RotatedBy(Projectile.rotation) - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), new Color(13, 188, 13, 0), Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2 + new Vector2(0, 12), new Vector2(0.3F, 6) * Projectile.scale, 0, 0f);
            /*
            for (int a = 0; a < 100 * (1 - Projectile.ai[2] / 70F)+2; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(24, 4 * player.direction).RotatedBy(Projectile.rotation) - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), new Color(13, 188, 13, 0), Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2 + new Vector2(0, 12), new Vector2(0.3F, 6 * (Projectile.ai[2] / 70F)) * Projectile.scale, 0, 0f);
            }*/
            return false;
        }
    }
}