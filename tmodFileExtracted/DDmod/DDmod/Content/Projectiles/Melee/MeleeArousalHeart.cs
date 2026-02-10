using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Melee
{
    public class MeleeArousalHeart : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/爱心光效2");
        }
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
        }
        int A;
        public override void AI()
        {
            if(Projectile.alpha>0)
            {
                Projectile.alpha -= 10;
            }
            Projectile.localAI[1] = 1 - (Projectile.alpha / 255F);
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.ProjScaleChange();
            Projectile.Track(420,20,13,10);
            if(Projectile.velocity.Length()<13)
            {
                Projectile.velocity *= 1.02F;
            }

        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.DProj().track >= 10)
                return null;
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return true;
            return Projectile.alpha<=0;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }

        public override void OnKill(int timeLeft)
        {
            float scale = 5;
            float velocity = 2;
            Vector2 projDirection = Utils.RotatedBy(new Vector2(0, -2 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection - Vector2.Zero) * velocity;

            Vector2 projDirection2 = Utils.RotatedBy(new Vector2(1 * scale, -3 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection2, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection2 - Vector2.Zero) * velocity;

            Vector2 projDirection3 = Utils.RotatedBy(new Vector2(-1 * scale, -3 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection3, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection3 - Vector2.Zero) * velocity;

            Vector2 projDirection4 = Utils.RotatedBy(new Vector2(+2 * scale, -4 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection4, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection4 - Vector2.Zero) * velocity;

            Vector2 projDirection5 = Utils.RotatedBy(new Vector2(-2 * scale, -4 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection5, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection5 - Vector2.Zero) * velocity;

            Vector2 projDirection6 = Utils.RotatedBy(new Vector2(+3 * scale, -4 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection6, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection6 - Vector2.Zero) * velocity;

            Vector2 projDirection7 = Utils.RotatedBy(new Vector2(-3 * scale, -4 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection7, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection7 - Vector2.Zero) * velocity;

            Vector2 projDirection8 = Utils.RotatedBy(new Vector2(+4 * scale, -3 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection8, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection8 - Vector2.Zero) * velocity;

            Vector2 projDirection9 = Utils.RotatedBy(new Vector2(-4 * scale, -3 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection9, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection9 - Vector2.Zero) * velocity;

            Vector2 projDirection10 = Utils.RotatedBy(new Vector2(+4 * scale, -2 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection10, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection10 - Vector2.Zero) * velocity;

            Vector2 projDirection11 = Utils.RotatedBy(new Vector2(-4 * scale, -2 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection11, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection11 - Vector2.Zero) * velocity;

            Vector2 projDirection12 = Utils.RotatedBy(new Vector2(+4 * scale, -1 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection12, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection12 - Vector2.Zero) * velocity;

            Vector2 projDirection13 = Utils.RotatedBy(new Vector2(-4 * scale, -1 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection13, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection13 - Vector2.Zero) * velocity;

            Vector2 projDirection14 = Utils.RotatedBy(new Vector2(+3 * scale, 0), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection14, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection14 - Vector2.Zero) * velocity;

            Vector2 projDirection15 = Utils.RotatedBy(new Vector2(-3 * scale, 0), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection15, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection15 - Vector2.Zero) * velocity;

            Vector2 projDirection16 = Utils.RotatedBy(new Vector2(+2 * scale, +1 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection16, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection16 - Vector2.Zero) * velocity;

            Vector2 projDirection17 = Utils.RotatedBy(new Vector2(-2 * scale, +1 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection17, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection17 - Vector2.Zero) * velocity;

            Vector2 projDirection18 = Utils.RotatedBy(new Vector2(+1 * scale, +2 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection18, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection18 - Vector2.Zero) * velocity;

            Vector2 projDirection19 = Utils.RotatedBy(new Vector2(-1 * scale, +2 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection19, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection19 - Vector2.Zero) * velocity;

            Vector2 projDirection20 = Utils.RotatedBy(new Vector2(0, +3 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection20, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection20 - Vector2.Zero) * velocity;

            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 1.5F; PlaySound(sound, Projectile.position);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)2;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];


            Color color = new Color(255, 50, 50, 55) * Projectile.localAI[1];
            Color color2 = new Color(255, 255, 255, 255) * Projectile.localAI[1];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Main.spriteBatch.Draw(Glow.Value, vector2, null, new Color(100, 100, 100, 255) * Projectile.localAI[1] * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                Main.spriteBatch.Draw(Glow.Value, vector2, null, color * 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4, spriteEffects, 0f);

            return false;
        }
    }
}