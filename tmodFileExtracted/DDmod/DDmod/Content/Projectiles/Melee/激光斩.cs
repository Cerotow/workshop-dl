using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class 激光斩 : ModProjectile
    {
        public override string Texture => "DDmod/Content/Projectiles/Melee/SwordWave5";
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 100;
            Projectile.extraUpdates = 4;
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
                //NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity.RotatedBy(r) *4, 451, (int)(Projectile.damage * 0.25F), Projectile.knockBack / 2, Projectile.owner, 0, 0);
                //NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity.RotatedBy(-r) *4, 451, (int)(Projectile.damage * 0.25F), Projectile.knockBack / 2, Projectile.owner, 0, 0);
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
                Projectile.extraUpdates = 4;
                for (int a = -1; a <= 1; a++)
                {
                    Vector2 vector = new(Projectile.position.X, Projectile.position.Y);
                    vector += Projectile.velocity.RotatedBy(MathHelper.PiOver2).PerfectNormalize() * (Projectile.width * a);
                    if (Main.rand.NextBool(10))
                    {
                        int D = NewDust(vector, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(255, 50, 50, 0), Main.rand.NextFloat(1.2F, 1.8F));
                        Main.dust[D].velocity = -Projectile.velocity * Main.rand.NextFloat(0.5F, 2.8F);
                        Main.dust[D].rotation = Projectile.velocity.ToRotation();
                        Main.dust[D].customData = 0.7F;
                    }
                }
            }
            else
            {
                Projectile.extraUpdates = 0;
                Projectile.ai[1] -= 0.04f;
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
            Projectile.scale = 1F;
            if(Projectile.ai[2]==1)
            {
                Projectile.scale = 1.5F;

            }
            if(Projectile.ai[2]==2)
            {
                Projectile.scale = 1.25F;

            }
            Projectile.damage = (int)(Projectile.localAI[1]*(Projectile.ai[1]));
            Projectile.ProjScaleChange();
            return false;
        }
        
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().Ecolor = new Color(255, 10, 10, 255);
            Main.projectile[A].DProj().Ecolor2 = new Color(200, 200, 200, 0);

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
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity / 4;
            if (Projectile.ai[0] >= 40&& Projectile.ai[1]>=1)
            {
                if (Projectile.timeLeft > 10)
                {
                    Projectile.timeLeft = 10;
                }
            }
            return false;
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
            Color color;
            Texture2D texture = TextureAssets.Projectile[Type].Value; 
            color = new Color(255, 150, 150, 150) * Projectile.ai[1];
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                for (int a = 0; a < Projectile.velocity.Length();a++)
                {
                    Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2)*0.2f;
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[i]+ Projectile.velocity.PerfectNormalize()*a + Projectile.Size / 2 - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
                }
            }
            color = new Color(255, 100, 100, 255) * Projectile.ai[1];
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            color.A = 0;
            return false;
        }
    }
}