using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DDmod.Content.Projectiles.Ranged
{
    public class BloodBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blood Bullet");
            //DisplayName.AddTranslation(7, "血猩弹");
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates =2;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.aiStyle = 1;
            AIType = 14;
        }
        public override void AI()
        {
            if (Projectile.ai[2] < Projectile.velocity.Length() * (Projectile.extraUpdates + 1))
            {
                Projectile.ai[2] += Projectile.velocity.Length() / 10 * (Projectile.extraUpdates + 1);
            }
            else
            {
                Projectile.ai[2] = Projectile.velocity.Length() * (Projectile.extraUpdates + 1);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            float lifeStoled = damageDone * 0.1f;
            if (lifeStoled < 1)
            {
                lifeStoled = 1;
            }
            if ((int)lifeStoled > 0 && !player.moonLeech && target.type != NPCID.TargetDummy && target.life > 2)
            {
                NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, 0f, 0f, ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, lifeStoled);
            }
        }
        public override void OnKill(int timeLeft)
        {
            int Type = 5;
            for (int A = 0; A < 10; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1f;
                dust.velocity = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (-Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 3));
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //子弹
            float A = Projectile.ai[2] * 1.5F;
            if (A > 90)
            {
                A = 90;
            }
            Texture2D texture = DDTextures.WhitePng.Value;
            //Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition, null, Color.White, 0, Vector2.Zero, Projectile.Size / 2 * new Vector2(1, 1), 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(100, 0, 0, 255)), Projectile.rotation, new Vector2(texture.Width/2,0), new Vector2(Projectile.scale, A), 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(255, 0, 0, 255)), Projectile.rotation, new Vector2(texture.Width / 2,0), new Vector2(Projectile.scale, A / 8), 0, 0f);

            return false;
        }
    }
}