using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class InfluxWaver : ModProjectile
    {
        public override string Texture => "DDmod/Content/Projectiles/Melee/SwordWave5";
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 250;
            Projectile.extraUpdates = 8;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation();
            if(Projectile.ai[0]==0)
            {
                float r = Main.rand.NextFloat(0.4F, 0.7F);
                NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity.RotatedBy(r) *4, 451, (int)(Projectile.damage * 0.25F), Projectile.knockBack / 2, Projectile.owner, 0, 0);
                NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity.RotatedBy(-r) *4, 451, (int)(Projectile.damage * 0.25F), Projectile.knockBack / 2, Projectile.owner, 0, 0);
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 40)
            {
                Projectile.Center = player.Center + Projectile.velocity.PerfectNormalize() * 50;
                Projectile.ai[1] = 0;
            }
            else if (Projectile.timeLeft > 10)
            {
                if (Projectile.ai[1] < 1)
                {
                    Projectile.ai[1] += 0.1F;
                }
                else
                {
                    Projectile.ai[1] = 1;
                }
                Projectile.extraUpdates = 8;
                for (int a = -1; a <= 1; a++)
                {
                    Vector2 vector = new(Projectile.position.X, Projectile.position.Y);
                    vector += Projectile.velocity.RotatedBy(MathHelper.PiOver2).PerfectNormalize() * (Projectile.width * a);
                    if (Main.rand.NextBool(20))
                    {
                        int D = NewDust(vector, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(97, 200, 225, 0), Main.rand.NextFloat(1.2F, 1.8F));
                        Main.dust[D].velocity = -Projectile.velocity * Main.rand.NextFloat(0.5F, 2.8F);
                        Main.dust[D].rotation = Projectile.velocity.ToRotation();
                        Main.dust[D].customData = 0.7F;
                    }
                }
            }
            else
            {
                Projectile.extraUpdates = 0;
                Projectile.ai[1] -= 0.05f;
                if (Projectile.ai[1] > 0)
                    Projectile.timeLeft = 2;
            }
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = Projectile.scale;
            }
            if (Projectile.localAI[1] == 0)
            {
                Projectile.localAI[1] = Projectile.damage;
            }
            Projectile.scale = 1.3F;
            Projectile.damage = (int)(Projectile.localAI[1]*(Projectile.ai[1]));
            Projectile.ProjScaleChange();
            return false;
        }
        
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            /*
            if(Projectile.ai[1]<1)
            {
                Projectile.ai[1]++;
                //NewProjectile(Projectile.GetSource_FromThis(), target.Center, new Vector2(10, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 451, (int)(Projectile.damage * 0.75F), Projectile.knockBack / 2, Projectile.owner, 0, 0);
            }*/
            if (Projectile.timeLeft > 10)
            {
                Projectile.timeLeft = 130;
            }

            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(97, 200, 225, 0) * 0.3F;
            Main.projectile[A].localAI[0] =1;
            Main.projectile[A].localAI[1] = 1;
            Main.projectile[A].scale = 1;

            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            if (Projectile.ai[1]<=0)
            {
                return false;
            }
                return null;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int a = -1; a <= 1;a++)
            {
                Vector2 vector = new (projHitbox.X, projHitbox.Y);
                vector += Projectile.velocity.RotatedBy(MathHelper.PiOver2).PerfectNormalize()*(projHitbox.Width*a);
                if (new Rectangle((int)vector.X, (int)vector.Y, projHitbox.Width, projHitbox.Height).Intersects(targetHitbox))
                {
                    return new bool?(true);
                } 
            }
            return new bool?(false);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(97, 200, 225, 120);
            color *= Projectile.ai[1];
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                for (int a = 0; a < Projectile.velocity.Length();a++)
                {
                    Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2)*0.2f;
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[i]+ Projectile.velocity.PerfectNormalize()*a + Projectile.Size / 2 - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
                } 
            }
            Main.spriteBatch.Draw(texture, Projectile.Center- Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center- Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center- Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            color.A = 0;
            return false;
        }
    }
}