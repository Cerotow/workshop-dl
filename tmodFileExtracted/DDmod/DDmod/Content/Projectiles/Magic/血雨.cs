
using DDmod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Terraria.GameContent.Animations.Actions.Sprites;
namespace DDmod.Content.Projectiles.Magic
{
    public class 血雨 : ModProjectile
    {
        public override string Texture => "DDmod/Image/VoidStar";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.timeLeft =1000;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
        }
        public override bool PreAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[2] = Projectile.velocity.Length();
            if (Projectile.ai[2]<1)
            {
                Projectile.ai[2] = 1;
            }
            //Projectile.extraUpdates = ((int)Projectile.ai[2] - 1);
            /*
            Projectile.ai[0]++;
            if (Projectile.ai[0]>30* (Projectile.extraUpdates+1))
            {
                if(Projectile.velocity.Y<12)
                {
                    Projectile.velocity.Y += 0.2F;
                }
                Projectile.velocity.X *= 0.96F;
            }*/
            if(Projectile.ai[1]<1)
            {
                Projectile.ai[1] += 0.1F;
            }
            else
            {
                Projectile.ai[1] = 1;
            }
            Projectile.ProjScale2();
            return false;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5)) target.AddBuff(BuffID.Bleeding, 120);
        }
        public override void OnKill(int timeLeft)
        {
            Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, new Color(51, 3, 3, 50));
            color.A = 50;
            for (int A = 0; A <12; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, (int)Projectile.Size.X, (int)Projectile.Size.Y, ModContent.DustType<光球粒子>(), 0, 0, 0, color)];
                dust.noGravity = false;
                dust.noLight = true;
                dust.scale = Main.rand.NextFloat(0.3F, 0.6F);
                dust.velocity = Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2)) * -Main.rand.NextFloat(1, 5);
                dust.customData = new Dust().DustAI(1) + 1;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.VoidStar.Value;
            Color color;
            int L = 40;
            for (int i = 0; i < L; i++)
            {
                float R = ((L - i) / (float)L);
                Vector2 vector = Projectile.position + Projectile.Size / 2 - Projectile.velocity.PerfectNormalize() * 80 * R * Projectile.ai[1];
                color = Lighting.GetColor((int)vector.X/16, (int)vector.Y/16, new Color(51, 3, 3, 150));
                color.A = 50;
                Main.spriteBatch.Draw(texture, Projectile.position+ Projectile.Size/2 -Projectile.velocity.PerfectNormalize()*80* R* Projectile.ai[1] - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale* (1- R) / 10, 0, 0f);
            }


            return false;
        }
    }
}