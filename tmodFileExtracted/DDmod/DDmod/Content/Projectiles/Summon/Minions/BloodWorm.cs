using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using Terraria;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class BloodWorm : Summons
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
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.minionSlots = 1;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<BloodWormBuff>();
            Speed = 12;
            Projectile.DamageType = DamageClass.Summon;
            Main.projFrames[Projectile.type] = 2;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void Visual()
        {

            if (!target)
                Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;
            Projectile.frameCounter++;
            if(Projectile.frameCounter>10)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if(Projectile.frame>1)
            {
                Projectile.frame = 0;
            }
        }
        public override bool MobileAI()
        {
            if (Projectile.DProj().Times[0] > 0)
            {
                Projectile.DProj().Times[0]--;
            }
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (vector.Length() > 300)
                {
                    vector.DirectPerfectNormalize();
                    Projectile.velocity = (Projectile.velocity * 20 + vector * Speed) / 21;
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                }
                else
                {
                    Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0;

                    Projectile.ai[0]+=2;
                    float Sp = Speed;
                    float Length = 150;
                    float Length2 = 150;
                    if (Projectile.DProj().Bool[0])
                    {
                        Projectile.ai[1]++;
                        if (Projectile.ai[1] > 300)
                        {
                            Projectile.ai[1] = 0;
                            Projectile.DProj().Bool[0] = false;
                            Projectile.netUpdate = true;
                        }
                        Sp *= 1.2f;
                        Projectile.ai[0]+=2;
                        Length -= 50;
                        Length2 -= 50;
                    }
                    if (Projectile.ai[0] > 40 && Projectile.ai[0] < 60)
                    {
                        Projectile.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, 0.3f);
                    }
                    if (vector.Length() > Length && Projectile.ai[0] > 60)
                    {
                        Projectile.velocity = vector.PerfectNormalize() * Sp *3;
                        Projectile.ai[0] = 0;
                    }
                    else if (vector.Length() > Length2)
                    {
                        Projectile.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, 0.3f);
                        vector.DirectPerfectNormalize();
                        Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * Sp) / 21;
                    }
                    else
                    {
                        vector.DirectPerfectNormalize();
                        Projectile.velocity = (Projectile.velocity * 20 + -vector.PerfectNormalize() * Sp) / 21;
                    }
                }
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
            if (Projectile.DProj().Bool[0])
            {
                modifiers.SourceDamage *= 1.3F;
            }
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(10))
            {
                Projectile.DProj().Bool[0] = true;
                Projectile.netUpdate = true;
            }
            if (Projectile.DProj().Bool[0]&& Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Bleeding, 180);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / 2 * Projectile.frame, texture.Width, texture.Height / 2));
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                Color color = Color.White.MultiplyRGBA(new Color(255, 20, 20, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.EntitySpriteDraw(texture, vector2, rectangle, color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 4), Projectile.scale * 1.2f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0);
            }
            Main.EntitySpriteDraw(texture, v, rectangle, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 4), Projectile.scale, 0, 0);
            if(Projectile.DProj().Bool[0])
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Color.White.MultiplyRGBA(new Color(255, 20, 20, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                    Main.EntitySpriteDraw(texture, vector2, rectangle, color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 4), Projectile.scale * 1.5f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0);
                }
            }

            return false;
        }
    }
}