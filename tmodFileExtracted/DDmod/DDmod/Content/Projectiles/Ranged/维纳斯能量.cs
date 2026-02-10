
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 维纳斯能量 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 2;

        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
            if (Projectile.ai[2] < Projectile.velocity.Length() * (Projectile.extraUpdates + 1))
            {
                Projectile.ai[2] += Projectile.velocity.Length() / 10 * (Projectile.extraUpdates + 1);
            }
            else
            {
                Projectile.ai[2] = Projectile.velocity.Length() * (Projectile.extraUpdates + 1);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange(20,Projectile.Center-new Vector2(4),Vector2.Zero,ModContent.DustType<光球粒子>(),0,3,Scale:0.75F,color:new Color(0,255,0,155));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.Starlight3.Value;
            for (int A = 0; A < Projectile.ai[2]/2; A++)
            {
                Main.spriteBatch.Draw(texture,Projectile.Center-Projectile.velocity.PerfectNormalize()*A*5-Main.screenPosition,null,new Color(0,255,0,155),Projectile.rotation,texture.Size()/2,Projectile.scale/2*new Vector2(0.25F,2),0,0);
            }
            return false;
        }
    }
}
