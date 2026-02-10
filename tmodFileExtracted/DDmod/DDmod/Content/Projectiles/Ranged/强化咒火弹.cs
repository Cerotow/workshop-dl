using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.GeneralProj;
using Microsoft.Xna.Framework.Graphics;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 强化咒火弹Proj : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
            AIType = 14;
            Projectile.GetGlobalProjectile<RangedProjectile>().BulletProj = true;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            if (Projectile.ai[2] == 0)
            {
                float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
                if (A > 45)
                {
                    A = 45;
                }
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4) + Projectile.velocity.PerfectNormalize() * 34, 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(96, 248, 2, 0), 2.2F);
                    Main.dust[dust].velocity = Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                    Main.dust[dust].noGravity = true;
                }
            }
            Projectile.ai[2] += Projectile.velocity.Length();
            if (Projectile.ai[2] > 50)
            {
                for (int a = 0; a < 1; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(96, 248, 2, 0), 2.2F);
                    Main.dust[dust].velocity = Vector2.Zero;
                    Main.dust[dust].rotation = Projectile.velocity.ToRotation();
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].customData = 2;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(39, 300);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.position = Projectile.oldPosition;
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnKill(int timeLeft)
        {
            float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
            if (A > 45)
            {
                A = 45;
            }
           int R =  Projectile.NewProjectileChange(Projectile.Center, Projectile.velocity.PerfectNormalize() * 0.01F, ModContent.ProjectileType<咒火爆炸>(), Projectile.damage * 2, 3, -1, 0, 1F, 0, 2);
            if(R>=0)
            {
                Main.projectile[R].DamageType = DamageClass.Ranged;
            }
            for (int a = 0; a < 5; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(96, 248, 2, 0), 2.2F);
                Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
            }
        }
    }
    public class 强化激光弹Proj : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 8;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
            AIType = 14;
            Projectile.GetGlobalProjectile<RangedProjectile>().BulletProj = true;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            if (Projectile.ai[2]==0)
            {
                int a = NewDust(Projectile.Center - new Vector2(4)+ Projectile.velocity.PerfectNormalize()*40, 1, 1, ModContent.DustType<星光粒子>(), 0, 0, 100, new Color(223, 51, 51, 0), 5.5F);
                Main.dust[a].customData = 4002;
                Main.dust[a].velocity = Vector2.Zero;
            }
            Projectile.ai[2] += Projectile.velocity.Length();
            if (Projectile.ai[2] > 50)
            {
                for (int a = 0; a <3; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.光球粒子>(), 0, 0, 0, new Color(223, 51, 51, 0), 0.75F);
                    Main.dust[dust].velocity = Vector2.Zero;
                    Main.dust[dust].position -= Projectile.velocity/3*a;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].customData = -3;
                    dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(223, 51, 51, 0), 2.2F);
                    Main.dust[dust].velocity = Vector2.Zero;
                    Main.dust[dust].rotation = Projectile.velocity.ToRotation();
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].customData = 2;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            float A = Projectile.oldVelocity.Length();
            if (A > 45)
            {
                A = 45;
            }
            for (int a = 0; a < 10; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.光球粒子>(), 0, 0, 0, new Color(223, 51, 51, 0), 1.2F);
                Main.dust[dust].velocity = Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 1f) * A/2;
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
                Main.dust[dust].customData = -1.5f;
            }
            int y = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<星光粒子>(), 0, 0, 100, new Color(223, 51, 51, 0), 5.5F);
            Main.dust[y].customData = 4002;
            Main.dust[y].velocity = Vector2.Zero;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.position = Projectile.oldPosition;
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnKill(int timeLeft)
        {
            float A = Projectile.oldVelocity.Length() * 0.15F * (Projectile.extraUpdates + 1);
            if (A > 45)
            {
                A = 45;
            }
            for (int a = 0; a < 20; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.光球粒子>(), 0, 0, 0, new Color(223, 51, 51, 0), 1.2F);
                Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0F, 0.3f) * A;
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
                Main.dust[dust].customData = -3;
            }
        }
    }
}