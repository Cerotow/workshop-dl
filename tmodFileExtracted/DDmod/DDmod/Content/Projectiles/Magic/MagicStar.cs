using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Magic
{
    public class MagicStar : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/星星光效");
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 3;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation += 0.3f;
            if (Projectile.DProj().track > 30)
            {
                NPC result = null;
                float num = 1000;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy() && npc.Center.Y - Projectile.Center.Y > 0)
                    {
                        float num2 = (Projectile.Center- npc.Center).Length();
                        if (!(num <= num2))
                        {
                            if (Collision.CanHitLine(Projectile.Center, 1, 1, npc.position, npc.width, npc.height))
                            {
                                num = num2;
                                result = npc;
                            }
                        }
                    }
                }
                    if (result != null && result.active)
                    {
                        Projectile.Chase(result, 8, 21);
                    }
                if (Projectile.ai[0] < 5)
                    Projectile.ai[0] += 0.1F;
                //Projectile.velocity.Y = Projectile.ai[0];
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 8; a++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), 0f, 0f, 0, default, 1f)];
                dust.color = new Color(0, 0, 255, 0);
                dust.noGravity = false;
                dust.velocity*=Main.rand.NextFloat(0.5F,1.5F);
                dust.velocity -= Projectile.velocity/2;
                dust.scale = Main.rand.NextFloat(0.5F, 0.8F);
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
            Color color;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition;
                color = new Color(0, 50, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/星星光效"), vector2, null, color, Projectile.rotation, ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/星星光效").Size() / 2, Projectile.scale * 1.2f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }

            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 2);
            color = new Color(0, 100, 255, 0);
            Main.spriteBatch.Draw(Glow.Value, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, color, Projectile.rotation * 1.1f, Glow.Size() / 2, Projectile.scale * 1.1f, spriteEffects, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, color, Projectile.rotation * 1.1f, Glow.Size() / 2, Projectile.scale * 1.1f, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, Color.White, Projectile.rotation, vector, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}