using DDmod.Content.Dusts;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class 烈阳光束 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] =5;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 30;
            Projectile.timeLeft = 320;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.alpha += 255;
            Projectile.scale = 1f;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Player player = Main.player[Projectile.owner];
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.scale *= player.GetAdjustedItemScale(player.ActiveItem());
                Projectile.DProj().Bool[0] = true;
                Projectile.velocity *= player.GetAdjustedItemScale(player.ActiveItem());
            }
            if (Projectile.ai[0]!=0)
            {
                Projectile.scale = Projectile.ai[0];
            }
            Projectile.ProjScaleChange();

            Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<激光粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 255, new Color(244, 128, 48, 45))];
            dust.noGravity = true;
            dust.rotation = Projectile.velocity.ToRotation();
            dust.scale = Projectile.scale * 1F;
            dust.velocity = Vector2.Zero;
            dust.customData = -1.3F;
            if (Main.rand.NextBool(2))
            {
                Dust dust3 = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 255, new Color(244, 128, 48, 45))];
                dust3.noGravity = true;
                dust3.scale = Projectile.scale * 3;
                dust3.velocity = Projectile.velocity;
                dust3.customData = 2F;
                dust3.rotation = dust3.velocity.ToRotation();
            }
            Lighting.AddLight(Projectile.Center, new Color(100, 255, 100).ToVector3() * 0.3f);
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                //NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, 953, Projectile.damage, 0, -1, 0, 1.35f);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Projectile.velocity;
            for (float A = 0; A < 20; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(244, 128, 48, 45))];
                dust.noGravity = true;
                dust.scale *= Main.rand.NextFloat(2F, 4);
                dust.velocity = vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * Main.rand.NextFloat(6, 24);
                dust.rotation = dust.velocity.ToRotation();
                dust.customData=2;
            }
            Projectile.damage = (int)(Projectile.damage * 0.8F);
            NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, 953, Projectile.damage, 0, -1, 0, 1.35f);

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}