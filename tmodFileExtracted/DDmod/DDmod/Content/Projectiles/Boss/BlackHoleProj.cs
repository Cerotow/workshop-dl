using DDmod.Content.Particles;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class BlackHoleProj : ModProjectile
    {
        public static Asset<Texture2D> Effect;
        public override void Load()
        {
            Effect = ModContent.Request<Texture2D>("DDmod/Image/旋转特效");
        }
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Black hole");
           //DisplayName.AddTranslation(7, "黑洞");
        }
        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }
        int A;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.RotationSpeed((Main.player[Player.FindClosest(Projectile.Center, 1, 1)].Center - Projectile.Center).ToRotation(), 0.003f);
                if (Projectile.scale >= 1 && Projectile.ai[1] <= 1200)
                {
                    if (DDHelper.SpecifyDirection(Projectile.rotation, (Main.player[Player.FindClosest(Projectile.Center, 1, 1)].Center - Projectile.Center).ToRotation(), 1.57f))
                    {
                        for (int A = 0; A < 1000; A++)
                        {
                            /*Projectile projectile= Main.projectile[A];
                            if (projectile.active&&projectile.type != Projectile.type)
                            {
                                Vector2 vector = projectile.Center - Projectile.Center;
                                vector -= Projectile.rotation.ToRotationVector2().PerfectNormalize() * 20;
                                if (vector.Length() < 150)
                                {
                                    projectile.velocity = vector.PerfectNormalize() * (-vector.Length()/20);
                                }
                            }*/
                            if (A < 255)
                            {
                                Player player = Main.player[A];
                                if (player.active && !player.dead)
                                {
                                    Vector2 vector = player.Center - Projectile.Center;
                                    vector -= Projectile.rotation.ToRotationVector2().PerfectNormalize() * 20;
                                    if (vector.Length() < 150)
                                    {
                                        player.velocity.Y = 0;
                                        player.position += vector.PerfectNormalize() * -3;
                                    }
                                }
                            }
                            /*if(A<200)
                            {
                                NPC npc = Main.npc[A];
                                if (npc.active)
                                {
                                    Vector2 vector = npc.Center - Projectile.Center;
                                    vector -= Projectile.rotation.ToRotationVector2().PerfectNormalize() * 20;
                                    if (vector.Length() < 150)
                                    {
                                        npc.velocity = vector.PerfectNormalize() * (-vector.Length() / 20);
                                    }
                                }
                            }*/
                        }
                    }
                }
                Projectile.timeLeft = 2;
                if (Projectile.ai[1] == 0)
                {
                    Projectile.scale = 0.02F;
                    Projectile.rotation = (Main.player[Player.FindClosest(Projectile.Center, 1, 1)].Center - Projectile.Center).ToRotation();
                }
                Projectile.ai[1]++;
                if (Projectile.ai[1] <= 1200)
                {
                    if (Projectile.scale < 1)
                    {
                        Projectile.scale += 0.003F;
                    }
                    else
                    {
                        if (Projectile.ai[1] <= 1100)
                        {
                            if (Projectile.ai[1] % 3 == 0)
                            {
                                Vector2 Center = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-Projectile.scale * Projectile.width, Projectile.scale * Projectile.width) / 2), Projectile.rotation, default);
                                Vector2 Vector = Utils.RotatedBy((Projectile.rotation).ToRotationVector2().PerfectNormalize(), Main.rand.NextFloat(-1F, 1F), default);
                                DDParticle.RequestParticleSpawn(ParticleType.blackHole, new ParticleOrchestraSettings
                                {
                                    PositionInWorld = (Projectile.Center + Center) + Vector * 150,
                                    MovementVector = Vector * -3
                                });
                            }
                        }
                    }
                }
                else
                {

                    if (Projectile.scale <= 0.01f)
                    {
                        Projectile.active = false;
                    }
                    Projectile.scale -= 0.01F;
                }
            }
            else if (Projectile.ai[0] == 1)
            {
                Projectile.RotationSpeed((Main.player[Player.FindClosest(Projectile.Center, 1, 1)].Center - Projectile.Center).ToRotation(), 0.003f);
                Projectile.timeLeft = 2;
                if (Projectile.ai[1] == 0)
                {
                    Projectile.scale = 0.02F;
                    Projectile.rotation = (Main.player[Player.FindClosest(Projectile.Center, 1, 1)].Center - Projectile.Center).ToRotation();
                }
                Projectile.ai[1]++;
                if (Projectile.ai[1] <= 500)
                {
                    if (Projectile.scale < 0.5F)
                    {
                        Projectile.scale += 0.003F;
                    }
                    else
                    {
                        if (Projectile.ai[1] % 150 == 0)
                        {
                            Vector2 Center = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-Projectile.scale * Projectile.width, Projectile.scale * Projectile.width) / 3), Projectile.rotation, default);
                            int NPC = NewNPCs(Projectile.GetSource_FromThis(), Projectile.Center + Center, 5, 0);
                            Main.npc[NPC].velocity = (Projectile.rotation).ToRotationVector2().PerfectNormalize() * 2;
                        }
                        if (Projectile.ai[1] % 3 == 0)
                        {
                            Vector2 Center = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-Projectile.scale * Projectile.width, Projectile.scale * Projectile.width) / 2), Projectile.rotation, default);
                            Vector2 Vector = Utils.RotatedBy((Projectile.rotation).ToRotationVector2().PerfectNormalize(), Main.rand.NextFloat(-1F, 1F), default);
                            DDParticle.RequestParticleSpawn(ParticleType.blackHole, new ParticleOrchestraSettings
                            {
                                PositionInWorld = (Projectile.Center + Center),
                                MovementVector = Vector * 3
                            });
                        }

                    }
                }
                else
                {
                    if (Projectile.scale <= 0.01f)
                    {
                        Projectile.active = false;
                    }
                    Projectile.scale -= 0.01F;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.localAI[0] -= 0.1f;
            Projectile.localAI[1] += 0.1f;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = Projectile.Center - Main.screenPosition;
            DDHelper.Compression(texture, new Color(255, 0, 0) * 0.9f, 0, 255, new Vector2(3, 1), 0, Projectile.localAI[0], BlendState.Additive);
            Main.spriteBatch.Draw(Effect.Value, vector, null, new Color(255, 0, 0) * 0.9f, Projectile.rotation, Effect.Size() / 2, Projectile.scale / 3, SpriteEffects.None, 0);

            DDHelper.Compression(texture, new Color(255, 0, 0) * 0.9f, 0, 255, new Vector2(3, 1), 0, Projectile.localAI[1], BlendState.Additive);
            Main.spriteBatch.Draw(Effect.Value, vector, null, new Color(255, 0, 0) * 0.9f, Projectile.rotation, Effect.Size() / 2, Projectile.scale / 3, SpriteEffects.None, 0);

            DDHelper.Compression(texture, new Color(255, 255, 255) * 0.9f, 0, 255, new Vector2(3, 1), 0, 0, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(texture, vector, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1.3f, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}