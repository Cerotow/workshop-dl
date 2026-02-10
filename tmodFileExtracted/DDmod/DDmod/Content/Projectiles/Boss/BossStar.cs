using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class BossStar : ModProjectile
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
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/星星光效");
        }
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }
        int A;
        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                /*
                Projectile.soundDelay = Main.rand.Next(10, 15);
                Dust dust = Main.dust[NewDust(Projectile.position,Projectile.width , Projectile.height, ModContent.DustType<星星粒子>(), 0f, 0f, 0, default, 1f)];
                dust.color = new Color(0, 0, 255, 0);
                dust.velocity= Vector2.Zero;
                dust.scale = 0.5f;
                dust.customData = 3;
            */}
            Projectile.rotation += 0.3f;
            if (Projectile.ai[1] == 0)
            {
                Projectile.localAI[1]++;
                Player player = Main.player[Main.npc[(int)TelegraphDelay].target];
                if (Projectile.localAI[1] > 20 && Projectile.localAI[1] < 50)
                {
                    Vector2 vector = player.Center - Projectile.Center;
                    vector += player.velocity * 50;
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 4) / 21;
                }
                if (NPCDowned.downedStarGuard2)
                {
                    if (Projectile.velocity.Y < 4)
                    {
                        Projectile.velocity.Y += 0.1f;
                    }
                    if (Projectile.velocity.Y > 4)
                    {
                        Projectile.velocity.Y =4;
                    }
                    if (player.velocity.Y > 0)
                    {
                        Projectile.velocity.Y += player.Dplayer().PrePosition.Y;
                    }
                    float VX = player.velocity.X;
                    if (VX < 0) VX = -VX;
                    if (VX > 15)
                    {
                        Vector2 vector = player.Center - Projectile.Center;
                        Projectile.velocity.X = (Projectile.velocity.X * 20 + vector.PerfectNormalize().X * 12) / 21;
                    }
                }
            }
            if (Projectile.ai[1] == 1)
            {
                Projectile.extraUpdates = 0;
                Player player = Main.player[Main.npc[(int)TelegraphDelay].target];
                Projectile.velocity = player.Dplayer().PrePosition;
            }
            if (Projectile.ai[1] == 2)
            {
                Projectile.velocity = Vector2.Zero;
                if (Projectile.timeLeft < 10)
                {
                    Projectile.timeLeft = 100;
                    Projectile.ai[1] = 3;
                }
            }
            if (Projectile.ai[1] == 3)
            {
                Vector2 vector = Projectile.DProj().vector[0] - Projectile.Center;
                Projectile.velocity = vector.PerfectNormalize() * -12;
            }
            if (Projectile.ai[1] == 4)
            {
                if (Projectile.localAI[0] < 30)
                {
                    Projectile.localAI[0]++;
                    Projectile.velocity.Y -= 0.2f;
                }
                else
                {
                    Projectile.velocity.Y += 0.1f;
                }
            }
            if (Projectile.ai[1] == 5)
            {
                if (Projectile.localAI[0] < 30)
                {
                    Projectile.localAI[0]++;
                    Projectile.velocity.Y -= 0.2f;
                }
                else
                {
                    Projectile.velocity.Y += 0.1f;
                }
                Projectile.extraUpdates = 4;
            }
            Projectile.ProjScaleChange();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 6; a++)
            {
                DDParticle.RequestParticleSpawn(ParticleType.Star, new ParticleOrchestraSettings
                {
                    PositionInWorld = Projectile.Center,
                    MovementVector = Main.rand.NextVector2Unit()
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


            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 2);
            Color color = new Color(0, 100, 255, 0);
            Color color2 = new Color(255, 255, 255, 255);
            if (Projectile.ai[1] == 2)
            {
                color = new Color(100, 155, 255, 0) * 0f;
                color2 = new Color(100, 155, 255, 0) * 0.3f;
            }
            if (Projectile.ai[1] == 5)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition;
                    Main.spriteBatch.Draw(Glow.Value, vector2, null, color, Projectile.rotation, Glow.Size() / 2, Projectile.scale * 0.4f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                color = new Color(100, 155, 255, 0) * 0f;
                color2 = new Color(100, 155, 255, 0) * 0;
            }
            else
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition;
                    Main.spriteBatch.Draw(Glow.Value, vector2, null, color*0.1f, Projectile.rotation, Glow.Size() / 2, Projectile.scale * 1.1f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
            }
            Main.spriteBatch.Draw(Glow.Value, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, color, Projectile.rotation, Glow.Size() / 2, Projectile.scale * 1.1f, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, color2, Projectile.rotation, vector, Projectile.scale, spriteEffects, 0f);

            return false;
        }
    }
}