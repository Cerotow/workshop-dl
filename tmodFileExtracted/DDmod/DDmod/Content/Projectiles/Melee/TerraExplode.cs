using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class TerraExplode : ModProjectile
    {
        public override string Texture => "DDmod/Image/爆炸";
        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Main.projFrames[Type] = 7;
            Projectile.timeLeft = 25;
            Projectile.scale = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.scale = Projectile.ai[1];
            if (Projectile.timeLeft > 14)
            {
                if (Projectile.ai[0] == 0)
                    Projectile.ai[0] = Projectile.damage;
                Projectile.damage = 0;
                return false;
            }
            else if(Projectile.frameCounter==0)
            {
                for (float A = 0; A < Projectile.scale*2; A += 0.1f)
                {
                    if (A <= Projectile.scale * 0.3F)
                    {
                        for (int r = 0; r < 4; r++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, new Color(106, 248, 106, 0))];
                            dust.noGravity = true;
                            dust.scale = 0.1f;
                            dust.alpha = -5;
                            dust.velocity = Vector2.Zero;
                            dust.customData = new Vector4(Projectile.scale / 2, 40, A * 16,0.3F);
                        }
                    }
                    else
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, new Color(96, 248, 96, 0))];
                        dust.noGravity = true;
                        dust.scale = 5.2f * Projectile.scale;
                        Vector2 vector = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                        dust.velocity = vector * Main.rand.NextFloat(4, 16) * Projectile.scale;
                        dust.rotation = dust.velocity.ToRotation();
                        dust.customData = 2;
                    }
                }
            }
            Projectile.damage = (int)Projectile.ai[0];
            Projectile.timeLeft = 2;
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter / 2;
            Projectile.velocity = Vector2.Zero;
            if (Projectile.frame >= 7)
            {
                SoundStyle sound = SoundID.Item14;
                sound.Pitch = -0.4F;
                PlaySound(sound, Projectile.Center);
                Projectile.Kill();
            }
            Projectile.scale += 0.5F;
            Projectile.ProjScaleChange();
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
            if (Projectile.timeLeft > 14)
            {
                return false;
            }
            Texture2D texture = TextureAssets.Projectile[Type].Value;


            Color color = new Color(96, 248, 96, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 7 * Projectile.frame, texture.Width, texture.Height / 7)), color, 1, new Vector2(texture.Width, texture.Height / 7) / 2, Projectile.scale, 0, 0);
           // color = new Color(83, 255, 40, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 7 * Projectile.frame, texture.Width, texture.Height / 7)), color, 2, new Vector2(texture.Width, texture.Height / 7) / 2, Projectile.scale * 0.8F, 0, 0);
          //  color = new Color(0, 144, 217, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 7 * Projectile.frame, texture.Width, texture.Height / 7)), color, 3, new Vector2(texture.Width, texture.Height / 7) / 2, Projectile.scale * 0.6F, 0, 0);
          //  color = new Color(19, 201, 122, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 7 * Projectile.frame, texture.Width, texture.Height / 7)), color, 4, new Vector2(texture.Width, texture.Height / 7) / 2, Projectile.scale * 0.4F, 0, 0);
          //  color = new Color(54, 249, 152, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 7 * Projectile.frame, texture.Width, texture.Height / 7)), color, 5, new Vector2(texture.Width, texture.Height / 7) / 2, Projectile.scale * 0.2F, 0, 0);
            return false;
        }
    }
}