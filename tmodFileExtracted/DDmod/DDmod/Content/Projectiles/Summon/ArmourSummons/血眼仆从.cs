using DDmod.Content.Projectiles.Summon.ArmourSummons.Buff;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.ArmourSummons
{
    public class 血眼仆从 : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefault()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.width = 26;
            Projectile.height = 26;
            IgnoreTile = false;
            Projectile.DamageType = DamageClass.Default;
            Projectile.extraUpdates = 0;
            Defaults(ModContent.BuffType<血肉仆从Buff>(), 12, 21, 1200, ModContent.ProjectileType<血箭>(), 15, 30, 300);
        }
        /// <summary>旧速度</summary>
        public Vector2[] oldVels;
        public override void Visual()
        {
            Projectile.rotation = Projectile.velocity.X * 0.03f;
            HitboxOrientation = Projectile.rotation - MathHelper.PiOver4;
            Projectile.spriteDirection = 0;
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (vector.X < 0)
                {
                    Projectile.spriteDirection = 1;
                }
            }
            else
            {
                if (Projectile.velocity.X < 0)
                {
                    Projectile.spriteDirection = 1;
                }
            }

        }
        public override bool MobileAI()
        {
            Vector2 vector = player.Center - Projectile.Center - new Vector2(0, 40);
            float SP = vector.Length() / 4;
            if (SP < 3 && SP > 0.5F) SP = 3; else if (SP < 0.5F) SP = 0;
            vector.DirectPerfectNormalize();
            Projectile.velocity = (Projectile.velocity * 20 + vector * SP) / 21;
            if (DistanceNPC > 3000)
            {
                Projectile.Center = player.Center - new Vector2(0, 40);
            }
            Projectile.rotation = Projectile.velocity.X * 0.03F;

            return false;
        }
        public override bool AttackAI()
        {
            if (target)
            {
                Projectile.ai[0]++;
                Vector2 direction = npc.Center - Projectile.Center;
                DistanceNPC = direction.Length();
                direction = direction.PerfectNormalize();
                if (Projectile.ai[0] >= AttackSpeed)
                {
                    int A = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center-new Vector2(0,10).RotatedBy(Projectile.rotation), direction * shootSpeed, shoot, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[A].DamageType = DamageClass.Default;
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;
                    Projectile.velocity -= direction * 5;
                }
            }
            else
            {
                Projectile.ai[0] = 0;
            }
            return false;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects sprite = 0;
            float Rot = Projectile.rotation;
            if (Projectile.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;
            Rectangle rectangle = new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                Color color = new Color(255, 10, 10, 100);
                color = Lighting.GetColor((int)(Projectile.oldPos[i].X + Projectile.Size.X / 2) / 16, (int)(Projectile.oldPos[i].Y + (Projectile.Size.Y / 2)) / 16, color) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                color.A = (byte)(100 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2));
                if (Projectile.spriteDirection == 1)
                {
                    Main.EntitySpriteDraw(texture, vector2, rectangle, color, Projectile.oldRot[i] + MathHelper.Pi, rectangle.Size() / 2, Projectile.scale, sprite);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, vector2, rectangle, color, Projectile.oldRot[i], rectangle.Size() / 2, Projectile.scale, sprite);
                }
            }
            Main.EntitySpriteDraw(texture, Center, rectangle, lightColor, Rot, rectangle.Size() / 2f, Projectile.scale, sprite);

            return false;
        }
    }
}