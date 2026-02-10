using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.GeneralProj;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class 冰霜弹Proj : ModProjectile
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
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
            AIType = 14;
            Projectile.GetGlobalProjectile<RangedProjectile>().BulletProj = true;
        }
        public override void AI()
        {
            Projectile.ai[2] += Projectile.velocity.Length();
            if (Projectile.ai[2]>50)
            {
                for (int a = 0; a < 1; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(0, 155, 255, 100), 1.4F);
                    Main.dust[dust].velocity = Vector2.Zero;
                    Main.dust[dust].rotation = Projectile.velocity.ToRotation();
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].customData = 2;
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<Frozen>(), (int)Main.rand.NextFloat(120, 300));

        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            float A = Projectile.oldVelocity.Length() * 0.5F * (Projectile.extraUpdates + 1);
            if (A > 45)
            {
                A = 45;
            }
            Projectile.position = Projectile.oldPosition;
            for (int a = 0; a < 5; a++)
            {
                int dust = NewDust(Projectile.position+Projectile.Size/2 - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(0, 155, 255, 100), Main.rand.NextFloat(0.75F, 1.4f));
                Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 1f) * A;
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
                Main.dust[dust].customData = 1 + Main.dust[dust].DustAI(0);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
            //子弹
            Texture2D texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition + new Vector2(Projectile.width / 2, 0), null, Projectile.GetAlpha(new Color(99, 74, 187, 200)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, Projectile.DProj().Times[0]), 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition + new Vector2(Projectile.width / 2, 0), null, Projectile.GetAlpha(new Color(100, 100, 100, 0)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, Projectile.DProj().Times[0] / 8), 0, 0f);
            
            return false;
        }
    }
}