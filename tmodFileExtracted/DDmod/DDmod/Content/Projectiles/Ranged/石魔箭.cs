
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 石魔箭 : ModProjectile
    {
        public Asset<Texture2D> Glow;
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;

            DDGlobalProjectile.ScaleGlow[Projectile.type] = 1;
            DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255, 255, 255, 255);
            DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Ranged/石魔箭_Glow");
        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
            if(Projectile.ai[0]==0)
            {
                Projectile.ai[0]++;
                Projectile.velocity /= 2;
            }
            for (int i = 0; i < Projectile.velocity.Length(); i+=3)
            {
                Vector2 projDirection = -Projectile.velocity.PerfectNormalize();
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4)-Projectile.velocity.PerfectNormalize()*(2+i), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(255, 255, 56, 0))];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.customData = 1.6F;
                dust.alpha = 100;
                dust.scale = 0.6F;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 15; i++)
                {
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0,8).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<石像弹>(), Projectile.damage / 2, 0, Projectile.owner, 1);
                } 
            }
            for (int i = 0; i < 30; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(255, 255, 56, 0))];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.customData = 2;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(0.8F,1.3F);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            DDHelper.绘制偏移头部(texture, Projectile, lightColor, MathHelper.Pi);
            if(Projectile.ai[0]==1)
            DDHelper.绘制偏移头部(texture, Projectile, Color.White, MathHelper.Pi, DDGlobalProjectile.Glow[Projectile.type].Value,0,1);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, Projectile.Size/2, 0, 0f);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 Pvelocity = Projectile.velocity.PerfectNormalize();
            projHitbox.X += (int)(Pvelocity.X * 14);
            projHitbox.Y += (int)(Pvelocity.Y * 14);
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            return null;
        }
    }
}
