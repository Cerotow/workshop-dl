using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 流星破坏球 : Summons
    {
        static Asset<Texture2D> asset;
        public override void SetDefault()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Defaults(ModContent.BuffType<流星破坏球Buff>(), 12, 21, 800, ModContent.ProjectileType<破坏者激光>(), 12, 30, 300);
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.minionSlots = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void Load()
        {
            asset = ModContent.Request<Texture2D>(Texture + "_Glow");
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 7;
            /// <summary> 感觉有点针对召唤师了 </summary> ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return Projectile.ai[1] > 0;
        }
        public override void Visual()
        {
            Dust.NewDust(Projectile.Center, 20, 20, ModContent.DustType<光球粒子>());
            Lighting.AddLight(Projectile.Center, new Color(0, 255, 0).ToVector3() * 0.25f);
            if (Projectile.ai[1] < 0)
            {
                if (target)
                {
                    Projectile.rotation = (npc.Center - Projectile.Center).ToRotation();
                }
                else
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
                Projectile.frameCounter++;
                Projectile.frame = (Projectile.frameCounter / 5) % 7;
            }
            else
            {
                if (Projectile.velocity.X > 0)
                {

                    Projectile.rotation += Projectile.velocity.X * 0.03F + Math.Abs(Projectile.velocity.Y * 0.03F);
                }
                else
                {
                    Projectile.rotation += Projectile.velocity.X * 0.03F - Math.Abs(Projectile.velocity.Y * 0.03F);
                }
                Projectile.frame = (int)(Projectile.ai[1] / 5);
                if (Projectile.frame >= BS/5-1)
                {
                    Projectile.frame = BS/5-1;
                }
            }
        }
        public int BS = 35;
        public override bool AttackAI()
        {
            Projectile.ai[2]--;
            if (target)
            {
                Projectile.ai[0]++;
                Vector2 direction = npc.Center - Projectile.Center;
                DistanceNPC = direction.Length();
                Projectile.ai[1]++;
                if (Projectile.ai[1] < 0)
                {
                    direction = npc.Center + npc.Dnpc().oldVelocity* (DistanceNPC/40) - Projectile.Center;
                    direction = direction.PerfectNormalize();
                    if (Projectile.ai[0] >= AttackSpeed && Projectile.ai[0] % 5 == 0)
                    {
                        if (Projectile.ai[0] >= AttackSpeed + 15)
                        {
                            Projectile.ai[0] = 0;
                        }
                        else
                        {
                            Projectile.ai[2] = 3;
                            int A = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * shootSpeed, shoot, Projectile.damage / 3, Projectile.knockBack, Projectile.owner, 0, 0, 1);
                            Main.projectile[A].DamageType = DamageClass.Summon;
                            Main.projectile[A].DProj().Magnification /=3;
                            Main.projectile[A].minion = true;
                            Main.projectile[A].originalDamage = Projectile.originalDamage / 3;
                            Projectile.netUpdate = true;
                            SoundStyle sound = SoundID.Item157;
                            sound.MaxInstances = 20;
                            sound.Volume = 0.5F;
                            PlaySound(sound, Projectile.position);
                        }
                    }
                }
                else
                {
                    direction = direction.PerfectNormalize();
                    if (Projectile.ai[1] > 240)
                    {
                        Projectile.ai[1] = -240;
                    }
                    if (Projectile.ai[1] < BS)
                    {
                        Projectile.velocity *= 0.96F;
                    }
                    else
                    {
                        if ((npc.Center - Projectile.Center).Length() > 60)
                        {
                            Projectile.velocity = (Projectile.velocity * 20 + direction * 21) / 21;
                        }
                        else
                        {
                            if (Projectile.velocity.Length() < 18)
                                Projectile.velocity = direction * 21;
                        }
                    }

                }
                Projectile.spriteDirection = 0;
                if (direction.X < 0)
                {
                    Projectile.spriteDirection = -1;
                }
            }
            else
            {
                Projectile.ai[1] = -240;
                Projectile.spriteDirection = 0;
                if (Projectile.velocity.X < 0)
                {
                    Projectile.spriteDirection = -1;
                }
                if (Projectile.ai[0] < AttackSpeed)
                {
                    Projectile.ai[0]++;
                }
            }
            return false;
        }
        public override bool MobileAI()
        {
            return Projectile.ai[1] < 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Texture2D Starlight = DDTextures.Starlight3.Value;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Color color = new Color(251, 127, 25, 0);
            SpriteEffects sprite = 0;
            float rotation = Projectile.rotation;
            if (Projectile.spriteDirection == -1)
            {
                sprite = SpriteEffects.FlipHorizontally;
                rotation += MathHelper.Pi;
            }
            Rectangle rectangle = new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width / 2, texture.Height / Main.projFrames[Projectile.type]);
            if (Projectile.ai[1] >= 0)
            {
                rectangle.X = texture.Width / 2;
            }
            if (Projectile.ai[1] >= BS)
            {

                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    float rot = Projectile.oldRot[i];
                    if (Projectile.spriteDirection == -1)
                    {
                        sprite = SpriteEffects.FlipHorizontally;
                        rot += MathHelper.Pi;
                    }
                    Main.EntitySpriteDraw(texture, Projectile.oldPos[i]+Projectile.Size/2 - Main.screenPosition, rectangle, color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), rot, rectangle.Size() / 2, Projectile.scale, sprite, 0);
                }
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, rotation, rectangle.Size() / 2, Projectile.scale, sprite, 0);
            Main.EntitySpriteDraw(asset.Value, Projectile.Center - Main.screenPosition, rectangle, Color.White, rotation, rectangle.Size() / 2, Projectile.scale, sprite, 0);

            if (Projectile.ai[2] > 0)
            {
                Main.EntitySpriteDraw(Starlight, Projectile.Center + Projectile.rotation.ToRotationVector2()*2 - Main.screenPosition, null, color * 0.8f, rotation - MathHelper.PiOver2, Starlight.Size() / 2, new Vector2(Projectile.scale / 2, Projectile.scale), 0, 0);
                Main.EntitySpriteDraw(Starlight, Projectile.Center + Projectile.rotation.ToRotationVector2() * 2 - Main.screenPosition, null, color * 0.8f, rotation, Starlight.Size() / 2, new Vector2(Projectile.scale / 2, Projectile.scale), 0, 0);

            }
            return false;
        }
    }
}