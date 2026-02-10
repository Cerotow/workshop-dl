using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class BossHeart : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/爱心光效");
        }
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }
        int A;
        public override void AI()
        {
            /*
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = Main.rand.Next(10, 15);
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, ModContent.DustType<爱心粒子>(), 0f, 0f, 0, default, 1f)];
                dust.color = new Color(255, 0, 0, 0);
                dust.scale = 0.5f;
            }*/
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y * -1, Projectile.velocity.X * -1) - MathHelper.Pi / 2;
            //
            NPC npc = Main.npc[(int)Projectile.ai[1]];
            if (Projectile.ai[0] == 1)
            {
                A++;
                if (A == 60)
                {
                    Projectile.ai[0]++;
                    Vector2 vector = Projectile.Center - npc.Center;
                    vector.Normalize();
                    Projectile.velocity = vector * 7;
                }
                if (A < 60)
                {
                    Vector2 vector = Projectile.Center - npc.Center;
                    vector.Normalize();
                    Projectile.rotation = vector.ToRotation() + MathHelper.Pi / 2;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.penetrate = 0;
            Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 6; a++)
            {
                DDParticle.RequestParticleSpawn(ParticleType.Heart, new ParticleOrchestraSettings
                {
                    PositionInWorld = Projectile.Center,
                    MovementVector = Main.rand.NextVector2Unit(),

                });
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

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Color color = new Color(255, 0, 0, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(Glow.Value, vector2, null, color, Projectile.rotation, Glow.Size() / 2, Projectile.scale * 1.2f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }

            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 2);
            Main.spriteBatch.Draw(Glow.Value, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, new Color(255, 0, 0, 0), Projectile.rotation, Glow.Size() / 2, Projectile.scale * 1.2f, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, Color.White, Projectile.rotation, vector, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}