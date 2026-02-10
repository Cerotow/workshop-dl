using DDmod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace DDmod.Content.Projectiles.Melee
{
    public class FlameExplosion : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override void AI()
        {
            if(Projectile.ai[0]==0)
            {
                Projectile.ai[0] = 1;
            }
            Projectile.scale = Projectile.ai[0];
            Projectile.ProjScaleChange();
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 80 * Projectile.scale; A++)
            {
                int Type = 6;
                if (Main.rand.NextBool(3))
                {
                    Type = ModContent.DustType<速度粒子>();
                }
                if (Main.rand.NextBool(10))
                {
                    Type = 31;
                }
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 0, 0, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, default)];
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(0.75F, 2f);
                dust.velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 6f) * Projectile.scale;
                if (Type == ModContent.DustType<速度粒子>())
                {
                    dust.color = new Color(233, 86, 3, 0);
                    dust.velocity *= 2;
                }
                dust.rotation = dust.velocity.ToRotation();
            }
            SoundStyle sound = SoundID.Item14;
            sound.Pitch = -0.2f;
            PlaySound(sound, Projectile.position);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(24, 300);
            }
            if (Projectile.ai[1] == 1&& target.knockBackResist>0)
            {
                Vector2 vector = (target.Center - (Projectile.Center+new Vector2(0,20))).PerfectNormalize() * 30* target.knockBackResist;
                target.velocity = vector;
                DDmod.SyncData(DDType.NPCCenter, target.whoAmI, -1, Projectile.owner);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White, 0, Vector2.Zero, Projectile.Size/2, 0, 0);

            return false;
        }
    }
}