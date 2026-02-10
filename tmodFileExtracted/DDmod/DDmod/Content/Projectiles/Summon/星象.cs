using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Summon
{
    public class 星象 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 190;
            Projectile.extraUpdates = 60;
            Projectile.localAI[0] = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1120;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[1] < 60)
            {

                Projectile.ai[1] = 60;
            }
                return false;
        }
        int A;
        Vector2[] oldPos;
        public override void AI()
        {
            Projectile.scale = 1.2F;
            if (oldPos==null|| oldPos.Length!=60)
            {
                oldPos = new Vector2[60];
            }
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity;
                Projectile.DProj().Bool[0] = true;
                
            }
            int V = 6;
            Projectile.ai[1]++;
            if (!Projectile.DProj().Bool[0])
            {
                if (Projectile.ai[0] >= V)
                {
                    Projectile.ai[2] = -Main.rand.NextFloat(-0.8F, 0.8F);
                }
                else
                {

                    if (Projectile.ai[0] % 2 == 0)
                    {
                        Projectile.ai[2] = Main.rand.NextFloat(0.1F, 0.8F);
                    }
                    else
                    {
                        Projectile.ai[2] = -Main.rand.NextFloat(0.1F, 0.8F);
                    }
                }
                Projectile.DProj().Bool[0] = true;
            }
            if (Projectile.ai[1] < 60)
            {
                Projectile.velocity = Projectile.DProj().vector[0].RotatedBy(Projectile.ai[2]);
                /*
                int d = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(0, 155, 255, 155), 0.5F);
                Main.dust[d].customData = 0.3F;
                Main.dust[d].velocity = Vector2.Zero;
                */
                oldPos[0] = Projectile.position;
                for (int i = oldPos.Length - 1; i > 0; i--)
                {
                    oldPos[i] = oldPos[i - 1];
                }
            }
            else
            if (Projectile.ai[0] < V && Main.myPlayer == Projectile.owner)
            {
                NPC npc = NPCdirection.FindClosest(Projectile.Center, 120, false);
                if (npc == null)
                {
                    int p = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, Type, Projectile.damage, Projectile.knockBack, -1, Projectile.ai[0] + 1);
                    Main.projectile[p].DProj().vector[0] = Projectile.DProj().vector[0];
                }
                else
                {
                    //for (int a = 0; a < V - Projectile.ai[0]; a++)
                    {
                        int p = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, Type, Projectile.damage, Projectile.knockBack, -1, Projectile.ai[0] + 1);
                        Main.projectile[p].DProj().vector[0] = (npc.Center - Projectile.Center).PerfectNormalize() * 2;
                    }
                }
                Projectile.extraUpdates = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[0] = V;
            }
            else
            {
                Projectile.extraUpdates = 0;
                Projectile.velocity = Vector2.Zero;

            }
            if(Projectile.ai[1]>60)
            {
                Projectile.localAI[0] -= 0.1F;
                if(Projectile.localAI[0]<0)
                {
                    Projectile.Kill();
                }
            }
            Projectile.rotation = Projectile.DProj().vector[0].RotatedBy(Projectile.ai[2]).ToRotation() + MathHelper.Pi;
            //Projectile.rotation += 0.1f;

            Projectile.ProjScaleChange();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnKill(int timeLeft)
        {
            return;
            for (int a = 0; a < 6; a++)
            {
                DDParticle.RequestParticleSpawn(ParticleType.Star, new ParticleOrchestraSettings
                {
                    PositionInWorld = Projectile.Center,
                    MovementVector = Main.rand.NextVector2Unit() * 3
                });
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D textureGlow = DDTextures.VoidStar.Value;

            if (oldPos != null && oldPos.Length == 60)
            {
                for (int i = 0; i < oldPos.Length; i++)
                {
                    Vector2 vector2 = oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = new Color(0, 90, 255, 0);
                    Main.spriteBatch.Draw(textureGlow, vector2, null, color * Projectile.localAI[0], Projectile.rotation, textureGlow.Size() / 2, Projectile.scale/16, 0, 0f);
                    Main.spriteBatch.Draw(textureGlow, vector2, null, color.Opposite() * Projectile.localAI[0], Projectile.rotation, textureGlow.Size() / 2, Projectile.scale/32, 0, 0f);
                }
            }
            Main.spriteBatch.Draw(texture, v, null, new Color(0, 90, 255, 0) * Projectile.localAI[0], Projectile.rotation, texture.Size() / 2, Projectile.scale/4, 0, 0f);
            Main.spriteBatch.Draw(texture, v, null, new Color(0, 90, 255, 0) * Projectile.localAI[0], Projectile.rotation, texture.Size() / 2, Projectile.scale/4, 0, 0f);
            Main.spriteBatch.Draw(texture, v, null, new Color(0, 90, 255, 0) * Projectile.localAI[0], Projectile.rotation, texture.Size() / 2, Projectile.scale/4, 0, 0f);
            Main.spriteBatch.Draw(texture, v, null, new Color(0, 90, 255, 0).Opposite() * Projectile.localAI[0], Projectile.rotation, texture.Size() / 2, Projectile.scale/6, 0, 0f);
            Main.spriteBatch.Draw(texture, v, null, new Color(0, 90, 255, 0).Opposite() * Projectile.localAI[0], Projectile.rotation, texture.Size() / 2, Projectile.scale/6, 0, 0f);
            Main.spriteBatch.Draw(texture, v, null, new Color(0, 90, 255, 0).Opposite() * Projectile.localAI[0], Projectile.rotation, texture.Size() / 2, Projectile.scale/6, 0, 0f);
            return false;
        }
    }
}