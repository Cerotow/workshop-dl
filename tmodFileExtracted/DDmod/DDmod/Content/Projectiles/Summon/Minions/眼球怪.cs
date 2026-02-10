using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 眼球怪 : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefault()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.minionSlots = 1;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<眼球怪Buff>();
            Speed = 8;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.extraUpdates = 1;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0.5F;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        /// <summary>旧速度</summary>
        public Vector2[] oldVels;
        public override void Visual()
        {
            Projectile.RotationSpeed(Projectile.DProj().vector[0].ToRotation(), 0.1F);
            HitboxOrientation = -Projectile.rotation;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 2)
            {
                Projectile.frame = 0;
            }

        }
        public override bool MobileAI()
        {
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                vector.Normalize();
                if (AttackFrame <= 0)
                {
                    Projectile.DProj().vector[0] = vector;
                }
                if(AttackFrame >0|| DDHelper.SpecifyDirection(Projectile.rotation, Projectile.DProj().vector[0].ToRotation(),0.1F))
                {
                    Projectile.velocity = Projectile.rotation.ToRotationVector2() * Speed;
                }
                else
                {
                    Projectile.velocity = Projectile.rotation.ToRotationVector2() * Speed/2;
                }
                Projectile.netUpdate = true;
                return false;
            }
            Projectile.DProj().vector[0] = Projectile.velocity;
            return true;
        }
        public override bool AttackAI()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = target.Center - Projectile.Center;
            vector.DirectPerfectNormalize();
            Projectile.velocity = Projectile.velocity.PerfectNormalize() * 4;
            AttackFrame = 30;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;
            SpriteEffects sprite = 0;

            if (Projectile.velocity.X > 0)
            {
                sprite = SpriteEffects.FlipVertically;
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                Color color = lightColor * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.EntitySpriteDraw(texture, vector2, new Rectangle(0, texture.Height / 3 * Projectile.frame, texture.Width, texture.Height / 3), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / 3) / 2, Projectile.scale, sprite, 0);
            }
            Main.EntitySpriteDraw(texture, Center, new Rectangle(0, texture.Height / 3 * Projectile.frame, texture.Width, texture.Height / 3), lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / 3) / 2f, Projectile.scale, sprite, 0f);

            return false;
        }
    }
}