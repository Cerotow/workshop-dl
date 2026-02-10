using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class MeteorYshapedDrone : Summons
    {
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<MeteorYshapedDroneBuff>(), 12, 21, 1200, ModContent.ProjectileType<SGreenLaser>(), 4, 8, 200);
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.minionSlots = 1;
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            /// <summary> 感觉有点针对召唤师了 </summary> ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void Visual()
        {
            Projectile.rotation = Projectile.velocity.X * 0.03F;
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 6 == 0)
            {
                Projectile.frame++;
            }
            Projectile.frame%=6;
            Lighting.AddLight(Projectile.Center, new Color(0, 255, 0).ToVector3() * 0.25f);
        }
        public override bool MobileAI()
        {
            if (target)
            {
                Vector2 direction = npc.Center - Projectile.Center;
                if (direction.Y<100)
                {
                    Projectile.velocity.Y -= 1;
                }
                if (direction.Length() > KeepDistance)
                {
                    direction.Normalize();
                    Projectile.velocity = (Projectile.velocity * inertia + direction * Speed) / (inertia + 1);
                }
                else
                {
                    direction.Normalize();
                    Projectile.velocity = (Projectile.velocity * inertia + -direction * Speed) / (inertia + 1);
                }
                return false;
            }
            return true;
        }
        public override bool AttackAI()
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
                npc = null;
                npc = NPCdirection.FindClosest(Projectile.Center, SearchRange, IgnoreTile);

                if (npc == null && !Projectile.sentry)
                {
                    npc = NPCdirection.FindClosest(player.Center, SearchRange, IgnoreTile);
                }
                if (npc != null)
                {
                    target = true;
                }
            }
            if (target)
            {
                Projectile.ai[0]++;
                Vector2 direction = npc.Center - Projectile.Center;
                Projectile.DProj().vector[0] = direction.PerfectNormalize();
                Projectile.ai[1] = direction.Length() + 2;
                DistanceNPC = direction.Length();
                direction = direction.PerfectNormalize();
                bool Bool = true;
                for (int a = 0; a < 1000; a++)
                {
                    Projectile Proj = Main.projectile[a];
                    if (Proj.active && Proj.owner == Projectile.owner && Proj.type == ModContent.ProjectileType<SGreenLaser>() && (Proj.ai[0] == Projectile.projUUID))
                    {
                        Bool = false;
                        break;
                    }
                }
                if (Bool && Projectile.ai[1] < 400)
                {
                    int A = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * shootSpeed, shoot, Projectile.damage, 0, Projectile.owner, Projectile.projUUID, Projectile.ai[1]);
                    Main.projectile[A].DamageType = DamageClass.Summon;
                    Main.projectile[A].minion = true;
                    Main.projectile[A].originalDamage = Projectile.originalDamage;
                    Main.projectile[A].scale /= 1.5f;
                    Projectile.netUpdate = true;
                    //PlaySound(SoundID.Item157, Projectile.position);
                }
            }
            else
            {
                Projectile.ai[1] = 0;
                Projectile.ai[0] = 0;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Texture2D Starlight = DDTextures.Starlight.Value;
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 vector = Projectile.Center - Main.screenPosition + ((Projectile.rotation - MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * -6);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, (SpriteEffects)1, 0);

            if (target)
            {
                vector = Projectile.Center - Main.screenPosition + new Vector2(0,14).RotatedBy(Projectile.rotation);
                Main.EntitySpriteDraw(VoidStar, vector, null, new Color(194, 37, 15, 0) * 0.5f*Projectile.DProj().Times[0], Projectile.rotation, VoidStar.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2)*1.5F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(194, 37, 15, 0) * Projectile.DProj().Times[0], Projectile.rotation, Starlight.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2)*1.5F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(255, 255, 255, 0) * Projectile.DProj().Times[0], Projectile.rotation, Starlight.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2), 0, 0);
            }
            return false;
        }
    }
}