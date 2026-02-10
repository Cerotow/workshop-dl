using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic
{
    public class MagicSapling : ModProjectile
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
            Projectile.width = 18;
            Projectile.height = 36;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 200;
            Projectile.penetrate = -1;
            CooldownSlot = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }
        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                for (int a = 0; a < Projectile.width; a++)
                {
                    NewDust(Projectile.position + new Vector2(a, Projectile.height), 1, 1, ModContent.DustType<生命粒子>(), 0f, 0f, 0, default, 0.5f);
                }
                Projectile.soundDelay = 3;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }

        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(texture.Width / 3 - 8, texture.Height) / 2;
            //Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, new Rectangle ?(new Rectangle((int)(texture.Width / 3 * TelegraphDelay), 0, texture.Width / 3, texture.Height)), new Color(0, 255, 0, 50), Projectile.rotation,new Vector2(texture.Width/3,texture.Height)/ 2, Projectile.scale * 1.4f, 0, 0f);

            Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, new Rectangle?(new Rectangle((int)(texture.Width / 3 * TelegraphDelay), 0, texture.Width / 3, texture.Height)), lightColor, Projectile.rotation, vector, Projectile.scale, 0, 0f);
            return false;
        }
    }
}