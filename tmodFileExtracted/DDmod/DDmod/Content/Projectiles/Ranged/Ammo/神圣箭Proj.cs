
using DDmod.Content.Dusts;
using Terraria;

namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class 神圣箭Proj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            //ProjectileID.Sets.DontCancelChannelOnKill[Projectile.type] = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 5;
                Dust dust = Main.dust[NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 57)];
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1f;
                dust.color = new Color(236, 200, 89, 0);
            }
            if (Projectile.ai[0] == 0)
            {
                Projectile.velocity.Y += 0.1f;

                Projectile.velocity *= 0.995f;
                if (Projectile.DProj().track >= 30)
                {
                    NPC npc = NPCdirection.FindClosest(Projectile.Center, 500, false);
                    if (npc != null)
                    {
                        Projectile.ai[0] = 1;
                        Projectile.ai[1] = npc.whoAmI;
                        int a = NewDust(Projectile.Center+Projectile.velocity - new Vector2(4), 1, 1, ModContent.DustType<星光粒子>(), 0, 0, 100, new Color(236, 200, 89, 0), 7F);
                        Main.dust[a].customData = 4002;
                        Main.dust[a].velocity = Vector2.Zero;
                    }
                }
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }
            }
            else
            {
                Projectile.Track(500, 0, 16, 0, false, (int)Projectile.ai[1]);
                Projectile.extraUpdates = 1;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            int a = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<星光粒子>(), 0, 0, 100, new Color(236, 200, 89, 0), 3.5F);
            Main.dust[a].customData = 4002;
            Main.dust[a].velocity = Vector2.Zero;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                for (int a = 0; a < 1; a++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(236, 200, 89, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                    if (Projectile.ai[0] > 30)
                    {
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], new Vector2(texture.Width, 0) / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], new Vector2(texture.Width, texture.Height) / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                    }
                }
            }
            return false;
        }
    }
}
