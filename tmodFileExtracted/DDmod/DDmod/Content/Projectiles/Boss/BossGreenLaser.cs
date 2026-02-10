using DDmod.Content.Dusts;
using Terraria;

namespace DDmod.Content.Projectiles.Boss
{
    public class BossGreenLaser : ModProjectile
    {
        public float TelegraphDelay
        {
            get
            {
                return Projectile.ai[0];
            }
            set
            {
                Projectile.ai[0] = value;
            }
        }
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1F;
            Projectile.timeLeft = 1600;
            Projectile.extraUpdates = 0;
            Projectile.penetrate = 3;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.masterMode)
            {
                if (Projectile.DProj().vector[0] == Vector2.Zero)
                {
                    Projectile.DProj().vector[0] = Projectile.velocity.PerfectNormalize();
                }
                if (Projectile.ai[0] % 20 == 0)
                {
                    Vector2 vector1 = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-0.2F, 0.2F), default);
                    Projectile.velocity = vector1;
                    Projectile.netUpdate = true;
                }
            }
            Projectile.ai[0]++;
        }
        public override void OnKill(int timeLeft)
        {
            int Type = ModContent.DustType<绿激光粒子>();
            for (int A = 0; A < 30; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3.5f, 7.5f);
                dust.rotation = Projectile.rotation;
            }

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int Type = ModContent.DustType<绿激光粒子>();
            for (int A = 0; A < 10; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f, 2.5f);
                dust.rotation = Projectile.rotation;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color color = new Color(26, 248, 2, 0);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                color = new Color(255 - 26, 255 - 248, 255 - 2, 0);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 10) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(26, 248, 2, 0), Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2), spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(255 - 26, 255 - 248, 255 - 2, 0), Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 10), spriteEffects, 0f);

            return false;
        }
    }
}