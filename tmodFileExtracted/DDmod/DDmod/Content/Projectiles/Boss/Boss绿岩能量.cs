
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss绿岩能量 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 24;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity;
                Projectile.DProj().vector[1] = Projectile.DProj().vector[0];
            }
            Vector2 vector = Projectile.DProj().vector[1];
            Projectile.velocity = vector.PerfectNormalize() * 4;
            /*
                int R = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, new Color(119, 237, 130, 50)*0.5F, 3);
                Main.dust[R].velocity = -Projectile.velocity;
                Main.dust[R].rotation = Projectile.rotation;
                Main.dust[R].customData = 2f;
            */
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == Projectile.ai[0])
            {
                return base.CanHitNPC(target);
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.Starlight3.Value;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size/2;
                Main.spriteBatch.Draw(texture, vector2, null, new Color(119, 237, 130, 50)*0.5F, Projectile.rotation+MathHelper.PiOver2, texture.Size() / 2, Projectile.scale/2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);


            }
            return base.PreDraw(ref lightColor);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            NewDustChange2(40,Projectile.Center-new Vector2(4),Vector2.Zero,ModContent.DustType<速度粒子>(),1,5,true,2,4,100,new Color(119, 237, 130, 50));
            Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
        }
    }
}
