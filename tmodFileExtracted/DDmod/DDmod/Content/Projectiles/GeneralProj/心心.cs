using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 心心 : ModProjectile
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
                return Projectile.DProj().Times[0];
            }
            set
            {
                Projectile.DProj().Times[0] = value;
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
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y * -1, Projectile.velocity.X * -1) - MathHelper.Pi / 2;
            if (Projectile.ai[0] == 0)
            {
                if (Projectile.scale < 2)
                {
                    Projectile.scale += 0.05F;
                }
                else
                {
                    Projectile.scale = 2;
                }
            }
            Projectile.ProjScaleChange();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange2(20, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 3* Projectile.scale, 10* Projectile.scale, true, Projectile.scale * 2, Projectile.scale * 3, 0, new Color(255, 100, 100, 0));
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                NewDustChange2(2, Projectile.oldPos[i] + Projectile.Size / 2 - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 1, true, Projectile.scale / 2, Projectile.scale, 0, new Color(255, 100, 100, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length));

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
            Asset<Texture2D> Glow = DDTextures.VoidStar;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Color color = new Color(255, 100, 100, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(Glow.Value, vector2, null, color, Projectile.rotation, Glow.Size() / 2, Projectile.scale * 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 2);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, vector, Projectile.scale, spriteEffects, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 100, 100, 0), Projectile.rotation, Glow.Size() / 2, Projectile.scale * 0.6f, spriteEffects, 0f);

            return false;
        }
    }
}