using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 夜光蘑菇 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 3;
            Projectile.scale = 1;
            Projectile.timeLeft = 600;
        }
        int A;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(Projectile.velocity.X!=oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.3F;
            }
            if(Projectile.velocity.Y!=oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.3F;
            }
            return base.OnTileCollide(oldVelocity);
        }
        public override void AI()
        {
            Projectile.rotation += 0.1f;
            if (Projectile.ai[0] == 1)
            {
                Projectile.ai[0]++;
                Projectile.velocity.Y /= 2;
            }
            if (Projectile.velocity.Y < 10)
            {

                Projectile.velocity.Y += 0.1F;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 10; A++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 20, 0f, 0f, 0, default(Color), 1f);
                if (Main.rand.NextBool(2))
                {
                    Main.dust[dust].scale = 0.1f;
                    Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.02f;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + texture.Size() / 2;
                Color color = new Color(34, 172, 226,0) *((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            Main.spriteBatch.Draw(texture, v, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            
            return false;
        }
    }
    public class 蘑菇弹 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 2;
            Projectile.scale = 1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
        }
        int A;
        public override void AI()
        {
            if (Projectile.rotation==0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();

                Projectile.DProj().Bool[0] = true;
            }
            DDHelper.BackAndForth(-3, 5, 0.25F, ref Projectile.ai[1], ref Projectile.DProj().Bool[0]);
            Projectile.scale = (5 - Projectile.ai[1]) / 5 + 1.5F;
            Projectile.ProjScaleChange();
            NPC npc = NPCdirection.FindClosest(Projectile.Center,500,false);
            if (npc != null&& Projectile.GetGlobalProjectile<DDGlobalProjectile>().track>30)
            {
                DDHelper.RotateSpeed(ref Projectile.rotation, (npc.Center - Projectile.Center).ToRotation(), 0.05F);
            }
            if (Projectile.ai[1] > 0)
            {
                int A = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<激光粒子>(), 0, 0, newColor: new Color(74, 189, 226)*0.16f, Scale: Projectile.scale / 4);
                Main.dust[A].noGravity = true;
                Main.dust[A].velocity = Vector2.Zero;
                Main.dust[A].rotation = Projectile.velocity.ToRotation();
                Main.dust[A].customData = -2;
                Projectile.velocity = Projectile.rotation.ToRotationVector2().PerfectNormalize() * Projectile.ai[1] * 3;
            }
            else
            {
                int A = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226) * 0.16f, Scale: Projectile.scale);
                Main.dust[A].noGravity = true;
                Main.dust[A].velocity = Vector2.Zero;
                Main.dust[A].rotation = Projectile.velocity.ToRotation();
                Main.dust[A].customData = 8;
                Projectile.velocity = Projectile.rotation.ToRotationVector2().PerfectNormalize() * 0.001f;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}