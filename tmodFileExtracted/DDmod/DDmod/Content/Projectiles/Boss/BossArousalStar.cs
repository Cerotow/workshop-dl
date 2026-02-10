using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class BossArousalStar : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/星星光效2");
        }
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 0;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.localAI[1] = 1;
        }
        int A;
        public override void AI()
        {
            Projectile.localAI[2] += Projectile.velocity.Length();
            if(Projectile.localAI[2]>=2000)
            {
                Projectile.localAI[1] -= 0.05F;
                if(Projectile.localAI[1]<=0)
                {
                    Projectile.Kill();
                }
            }
            Projectile.rotation += Projectile.velocity.X * 0.01F;
            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.01F;
            }
            else
            {
                Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.01F;
            }
            if (Projectile.ai[0] == 0)
            {
                if (!Projectile.DProj().Bool[0])
                {
                    Projectile.DProj().Bool[0] = true;
                    Projectile.scale = 0.1F;
                }
                if (Projectile.scale < 1 + Projectile.ai[1])
                {
                    Projectile.scale += 0.05F;
                }
            }
            if (Projectile.ai[0] == 1)
            {
                if (!Projectile.DProj().Bool[0])
                {
                    Projectile.DProj().Bool[0] = true;
                    Projectile.scale = 0.1F;
                    Projectile.DProj().vector[0] = Projectile.velocity;
                    if (Projectile.ai[2] >= MathHelper.Pi)
                    {
                        Projectile.ai[2] -= MathHelper.TwoPi;
                    }
                    if (Projectile.ai[2] <= -MathHelper.Pi)
                    {
                        Projectile.ai[2] += MathHelper.TwoPi;
                    }
                }
                Projectile.velocity = Projectile.DProj().vector[0].RotatedBy(Projectile.ai[2]);
                if (Math.Abs(Projectile.ai[2]) < 0.1F)
                {
                    Projectile.ai[2] = 0;
                }
                if (Projectile.ai[2] > 0)
                {
                    Projectile.ai[2] -= 0.05F;
                }
                if (Projectile.ai[2] < 0)
                {
                    Projectile.ai[2] += 0.05F;
                }
                if (Projectile.scale < 1 + Projectile.ai[1])
                {
                    Projectile.scale += 0.05F;
                }
            }
            if (Projectile.ai[0] == 2)
            {
                if (!Projectile.DProj().Bool[0])
                {
                    Projectile.DProj().Bool[0] = true;
                    Projectile.scale = 0.1F;
                }
                if (Projectile.scale < 1 + Projectile.ai[1])
                {
                    Projectile.scale += 0.05F;
                }
                Projectile.velocity *= 1.01F;
            }
            Projectile.ProjScaleChange();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if(target.ActiveItem().DamageType!=DamageClass.Magic)
            modifiers.SourceDamage *= 1.5F;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.Dplayer().ManaKao += 100*20;
        }
        public override void OnKill(int timeLeft)
        {
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
            Color color = new Color(0, 100, 255, 55) * Projectile.localAI[1];
            Color color2 = new Color(255, 255, 255, 255) * Projectile.localAI[1];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Main.spriteBatch.Draw(Glow.Value, vector2, null, new Color(100, 100, 100, 255)* Projectile.localAI[1] * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                Main.spriteBatch.Draw(Glow.Value, vector2, null, color * 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, color2, Projectile.rotation, vector, Projectile.scale, spriteEffects, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, color, Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4, spriteEffects, 0f);

            return false;
        }
    }
}