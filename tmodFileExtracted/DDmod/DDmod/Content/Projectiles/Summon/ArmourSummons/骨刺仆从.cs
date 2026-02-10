using DDmod.Content.Projectiles.Summon.ArmourSummons.Buff;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.ArmourSummons
{
    public class 骨刺仆从 : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefault()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.width = 26;
            Projectile.height = 26;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<血肉仆从Buff>();
            Speed = 16;
            Projectile.DamageType = DamageClass.Default;
            Projectile.extraUpdates = 0;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0.5F;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        /// <summary>旧速度</summary>
        public Vector2[] oldVels;
        public override void Visual()
        {
            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += Projectile.velocity.Length() * 0.03f;
            }
            else
            {
                Projectile.rotation -= Projectile.velocity.Length() * 0.03f;
            }
            HitboxOrientation = Projectile.rotation - MathHelper.PiOver4;

        }
        public override bool MobileAI()
        {
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                vector.DirectPerfectNormalize();
                DDHelper.RotateSpeed(ref Projectile.DProj().Times[0], vector.ToRotation(), 0.1F);
                if (AttackFrame <= 0)
                {
                    if (DDHelper.SpecifyDirection(Projectile.DProj().Times[0], vector.ToRotation(), 0.3F))
                    {
                        if(Projectile.DProj().Times[1]<1)
                        {
                            Projectile.DProj().Times[1] += 0.2F;
                        }
                        Projectile.velocity = Projectile.DProj().Times[0].ToRotationVector2() * Speed * Projectile.DProj().Times[1];
                    }
                    else
                    {
                        if (Projectile.DProj().Times[1] > 0.4)
                        {
                            Projectile.DProj().Times[1] -= 0.02F;
                        }
                        Projectile.velocity = Projectile.DProj().Times[0].ToRotationVector2() * Speed * Projectile.DProj().Times[1];
                    }
                }
                else
                {
                    if (Projectile.DProj().Times[1] > 0.4)
                    {
                        Projectile.DProj().Times[1] -= 0.02F;
                    }
                    Projectile.velocity = Projectile.DProj().Times[0].ToRotationVector2() * Speed * Projectile.DProj().Times[1];

                }
                Projectile.netUpdate = true;
                return false;
            }
            return true;
        }
        public override bool AttackAI()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            AttackFrame = 15;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                Color color = new Color(255, 10, 10, 100);
                color = Lighting.GetColor((int)(Projectile.oldPos[i].X + Projectile.Size.X / 2)/16, (int)(Projectile.oldPos[i].Y + (Projectile.Size.Y / 2))/16, color) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                color.A = (byte)(100 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2));
                Main.EntitySpriteDraw(texture, vector2, null, color, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale, 0);
            }
            Main.EntitySpriteDraw(texture, Center, null, lightColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, 0);

            return false;
        }
    }
}