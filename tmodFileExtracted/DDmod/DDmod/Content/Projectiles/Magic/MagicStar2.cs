using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Magic
{
    public class MagicStar2 : ModProjectile
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation += 0.1f;

            Projectile.ProjScaleChange();
            if (Projectile.DProj().vector[0] == Vector2.Zero && Projectile.velocity!=Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity;
                if (Projectile.ai[0] == 0)
                {
                    Projectile.DProj().vector[0] *= 0.85f;
                }
            }
            //浮动
            DDHelper.BackAndForth(-Projectile.ai[0], Projectile.ai[0], Projectile.ai[0]/8, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0]);

            Projectile.velocity = Projectile.DProj().vector[0].RotatedBy(Projectile.DProj().Times[0]);
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 10;
            }
            else
            {
                Projectile.alpha = 0;
            }
            NewDustChange4(1, Projectile.position, Projectile.Size, ModContent.DustType<光球粒子>(), 0F, 0F, true, 0.6F, 1.2F,color:new Color(0, 100, 255, 150), Data:1.5f);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange4(10, Projectile.position, Projectile.Size, ModContent.DustType<星星粒子>(), 0F, 4F,false,0.4F,1F,Data:2);
            /*
        for (int a = 0; a < 6; a++)
        {
            DDParticle.RequestParticleSpawn(ParticleType.Star, new ParticleOrchestraSettings
            {
                PositionInWorld = Projectile.Center,
                MovementVector = Main.rand.NextVector2Unit() * 3
            });
        }*/
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D textureGlow = DDTextures.VoidStar.Value;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size/2;
                Color color = new Color(0, 150, 255, 150) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2)*Projectile.Visible();
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                Main.spriteBatch.Draw(textureGlow, vector2, null, new Color(0, 150, 255, 150) * 0.4f * Projectile.Visible(), 1, textureGlow.Size() / 2, Projectile.scale * 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, v, null, Color.White * Projectile.Visible(), Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);

            Main.spriteBatch.Draw(textureGlow, v, null, new Color(0, 150, 255,155) * Projectile.Visible(), 1, textureGlow.Size() / 2,Projectile.scale* 0.6f, SpriteEffects.None, 0);
            return false;
        }
    }
}