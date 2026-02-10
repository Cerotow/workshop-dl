using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using Terraria;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class ShadowSkull : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

        }
        public override void SetDefault()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.minionSlots = 1;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<ShadowSkullBuff>();
            Speed = 12;
            Projectile.DamageType = DamageClass.Summon;
            Main.projFrames[Projectile.type] = 6;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 1;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void Visual()
        {
            if (!target)
                Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 5)
            {
                Projectile.frame = 0;
            }
            Projectile.DProj().Times[1]++;
            if (Projectile.DProj().Times[1]%5==0)
            {
                Dust dust = Main.dust[NewDust(Projectile.position - Projectile.rotation.ToRotationVector2() * 0, Projectile.width, Projectile.height, 27)];
                dust.scale = 0.8f;
                dust.velocity = Projectile.rotation.ToRotationVector2().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * -5;
            }
        }
        public override bool MobileAI()
        {
            if (Projectile.DProj().Times[0] > 0)
            {
                Projectile.DProj().Times[0]--;
            }
            Projectile.DProj().Bool[1] = false;
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (vector.X > 0)
                {
                    vector.X -= 300;
                }
                else
                {
                    vector.X += 300;
                }
                if (vector.Length() > 300)
                {
                    float Sp = Speed* vector.Length()/20;
                    if(Sp<2)
                    {
                        Sp = 2;
                    }
                    if (Sp > 10)
                    {
                        Sp = 10;
                    }
                    vector.DirectPerfectNormalize();
                    Projectile.velocity = (Projectile.velocity * 20 + vector * Sp) / 21;
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
                else
                {
                    Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0;

                    float Sp = Speed * vector.Length() / 20;
                    if (Sp < 2)
                    {
                        Sp = 2;
                    }
                    if (Sp > 10)
                    {
                        Sp = 10;
                    }
                    float Length = 50;
                    Projectile.ai[0]++;
                    if (vector.Length() < Length && Projectile.DProj().Times[2] <= 3&& Projectile.ai[0] > 60)
                    {
                        Vector2 vector2 = npc.Center - Projectile.Center;
                        vector2.DirectPerfectNormalize();
                        Projectile.velocity = new Vector2(vector2.X * 20, 0);
                        Projectile.rotation = Projectile.velocity.ToRotation();
                        Projectile.DProj().Times[2]++;
                        Projectile.ai[0] = 0;
                    }
                    else if(Projectile.ai[0]>60|| Projectile.DProj().Times[2] > 3)
                    {
                        vector.DirectPerfectNormalize();
                        Projectile.velocity = (Projectile.velocity * 20 + vector * (10)) / 21;

                        Projectile.DProj().Bool[1] = true;
                        if (Projectile.DProj().Times[2] > 3)
                        {

                            Projectile.DProj().Times[3]++;
                            if (Projectile.DProj().Times[3] == 60&&Projectile.owner==Main.myPlayer)
                            {
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (npc.Center - Projectile.Center).PerfectNormalize() * 12, ModContent.ProjectileType<ShadowWave>(), Projectile.damage * 2, 0, Projectile.owner);
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (npc.Center - Projectile.Center).PerfectNormalize() * 10, ModContent.ProjectileType<ShadowWave>(), Projectile.damage * 2, 0, Projectile.owner);
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (npc.Center - Projectile.Center).PerfectNormalize() * 8, ModContent.ProjectileType<ShadowWave>(), Projectile.damage * 2, 0, Projectile.owner);
                            }
                            if (Projectile.DProj().Times[3] >= 240)
                            {
                                Projectile.DProj().Times[2] = 0;
                                Projectile.DProj().Times[3] = 0;
                            }
                        }
                    }
                }
                if(Projectile.DProj().Times[1]%180==0)
                {
                }
                Projectile.netUpdate = true;
            }
            else
            {
                Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 1;
                return true;
            }
            return false;
        }
        public override bool AttackAI()
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.ShadowFlame, 210);
            }
        }
        int Di;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.DProj().Bool[1])
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (vector.X > 0)
                {
                    Projectile.direction = 1;
                }
                else
                {
                    Projectile.direction = -1;
                }
            }
            //Projectile.direction = Di;

           Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / 6 * Projectile.frame, texture.Width, texture.Height / 6));
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                Color color = Color.White.MultiplyRGBA(new Color(81, 6, 233, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.EntitySpriteDraw(texture, vector2, rectangle, color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 12), Projectile.scale * 1.2f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), (SpriteEffects)Projectile.direction, 0);
                Main.EntitySpriteDraw(texture, vector2, rectangle, color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 12), Projectile.scale * 1.2f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), (SpriteEffects)Projectile.direction, 0);
            }
            Main.EntitySpriteDraw(texture, v, rectangle, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 12), Projectile.scale, (SpriteEffects)Projectile.direction, 0);
            return false;
        }
    }
}