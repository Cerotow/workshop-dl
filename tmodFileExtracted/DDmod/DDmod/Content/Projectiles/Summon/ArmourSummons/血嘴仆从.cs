using DDmod.Content.Projectiles.Summon.ArmourSummons.Buff;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.ArmourSummons
{
    public class 血嘴仆从 : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefault()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<血肉仆从Buff>();
            Speed = 20;
            Projectile.DamageType = DamageClass.Default;
            
            Projectile.extraUpdates = 0;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0.5F;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        /// <summary>旧速度</summary>
        public Vector2[] oldVels;
        public float NPColdRot;
        public bool Yaozhu;
        public override void Visual()
        {
            HitboxOrientation = Projectile.rotation - MathHelper.PiOver4;
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 10 == 0)
            {
                Projectile.frame++;
            }
            Projectile.spriteDirection = 0;
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (vector.Length()>300)
                {
                    if(Projectile.frame>=2)
                    {
                        Projectile.frame = 0;
                    }
                }
                else
                {
                    if (Projectile.frame >= 3)
                    {
                        Projectile.frame = 3;
                    }
                }
                Projectile.rotation = Projectile.velocity.ToRotation();
                if(Projectile.velocity.X<0)
                {
                    Projectile.spriteDirection=1;
                }
                if (npc.getRect().Intersects(Projectile.getRect()))
                {
                    Projectile.rotation = vector.ToRotation();
                    if (vector.X < 0)
                    {
                        Projectile.spriteDirection = 1;
                    }
                    Projectile.frame = 4;
                    Projectile.Center = npc.Center-vector.RotatedBy(npc.rotation- NPColdRot);
                }
                NPColdRot = npc.rotation;
            }
            else
            {
                if (Projectile.frame >= 2)
                {
                    Projectile.frame = 0;
                }
                Projectile.rotation = Projectile.velocity.ToRotation();
                if (Projectile.velocity.X < 0)
                {
                    Projectile.spriteDirection = 1;
                }
            }

        }
        public override bool MobileAI()
        {
            target = false;
            if (player.HasMinionAttackTargetNPC)
            {
                npc = null;
                npc = Main.npc[player.MinionAttackTargetNPC];
                if (IgnoreTile || (Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height) && !Projectile.sentry))
                {
                    if (Vector2.Distance(npc.Center, Projectile.Center) < SearchRange) target = true;
                }
                else if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && Vector2.Distance(npc.Center, Projectile.Center) < SearchRange)
                {
                    target = true;
                }
            }

            if (!target)
            {
                npc = NPCdirection.FindClosest(Projectile.Center, SearchRange, IgnoreTile);
                if (npc!=null)
                target = true;
            }
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                vector.DirectPerfectNormalize();
                Projectile.velocity = (Projectile.velocity * 20 + vector * Speed)/21;
                if(Yaozhu)
                {
                    Projectile.velocity = Vector2.Zero;
                    Vector2 r = npc.position - npc.oldPosition;
                    if (r.Length() < 48)
                    {
                        Projectile.position += r;
                    }
                }
                if(npc.getRect().Intersects(Projectile.getRect()))
                {
                    Yaozhu = true;
                }
                else
                {
                    Yaozhu = false;
                }
                Projectile.netUpdate = true;
                return false;
            }
            Yaozhu = false;
            return true;
        }
        public override bool AttackAI()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            AttackFrame = 5;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects sprite = 0;
            float Rot = Projectile.rotation;
            if(Projectile.spriteDirection==1)
            {
                Rot += MathHelper.Pi;
                sprite = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;
            Rectangle rectangle = new Rectangle(0, texture.Height / Main.projFrames[Projectile.type]*Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                Color color = new Color(255, 10, 10, 100);
                color = Lighting.GetColor((int)(Projectile.oldPos[i].X + Projectile.Size.X / 2) / 16, (int)(Projectile.oldPos[i].Y + (Projectile.Size.Y / 2)) / 16, color) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                color.A = (byte)(100 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2));
                if (Projectile.spriteDirection == 1)
                {
                    Main.EntitySpriteDraw(texture, vector2, rectangle, color, Projectile.oldRot[i]+ MathHelper.Pi, rectangle.Size() / 2, Projectile.scale, sprite);
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