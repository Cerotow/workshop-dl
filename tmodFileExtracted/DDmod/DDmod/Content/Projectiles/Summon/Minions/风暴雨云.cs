using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using DDmod.Sync;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 风暴计数器 : Summons
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<风暴雨云Buff>(), 12, 21, 800,0, 10, 7, 150);
            Projectile.width = 22;
            Projectile.height = 36;
            Projectile.minionSlots = 1;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            /// <summary> 感觉有点针对召唤师了 </summary> ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void Visual()
        {
        }
        public override bool MobileAI()
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
        public override bool AttackAI()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        { return false;
        }
    }

    public class 风暴雨云 : Summons
    {

        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<风暴雨云Buff>(), 12, 21, 800, ModContent.ProjectileType<雨>(), 10, 7, 150);
            Projectile.width = 22;
            Projectile.height = 36;
            Projectile.minionSlots = 0;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            /// <summary> 感觉有点针对召唤师了 </summary> ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override bool CheckActive()
        {
            if (Minibuff != 0)
            {
                if (player.dead || !player.active)
                {
                    player.ClearBuff(Minibuff);

                    return false;
                }

                if (player.HasBuff(Minibuff))
                {
                    Projectile.timeLeft = 2;
                }
            }
            if (Projectile.DProj().track>3&& Projectile.ai[2] == 0)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[2] < player.ownedProjectileCounts[ModContent.ProjectileType<风暴计数器>()])
            {
                Projectile.ai[2] = player.ownedProjectileCounts[ModContent.ProjectileType<风暴计数器>()];
                Projectile.DProj().Bool[0] = false;
            }
            else
            {
                Projectile.ai[2] = player.ownedProjectileCounts[ModContent.ProjectileType<风暴计数器>()];
            }
            return true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void Visual()
        {
            
            if (!Projectile.DProj().Bool[0])
            {
                if (Projectile.ai[2] == 4 || Projectile.ai[2] == 7)
                {
                    for (float A = 0; A < 22; A++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 226, Projectile.oldVelocity.X, Projectile.oldVelocity.Y)];
                        dust.noGravity = true;
                        dust.scale = Projectile.scale * 1.25F;
                        dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(1, 4);
                    }
                }
                else
                {

                    for (float A = 0; A < 22; A++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 154, Projectile.oldVelocity.X, Projectile.oldVelocity.Y)];
                        dust.noGravity = true;
                        dust.scale = Projectile.scale * 1.25F;
                        dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(1, 4);
                    }
                }
                Projectile.DProj().Bool[0] = true;
            }
            Projectile.scale = 1 + (Projectile.ai[2] - 1) / 10;
            if(Projectile.scale>=1.5F)
            {
                Projectile.scale = 1.5F;
            }
            Projectile.frameCounter++;
            Projectile.frame = Projectile.ai[2] <= 6 ? Projectile.frameCounter / 6 % 4 : Projectile.frameCounter / 6 % 5;
        }
        public override bool MobileAI()
        {
            Speed = (int)(12 * (1 + (Projectile.ai[2] - 1) / 2));
            if (Speed>24)
            {
                Speed = 24;
            }
            if (!target)
            {
                Vector2 direction = player.Center - new Vector2(0, 50) - Projectile.Center;
                float SP = direction.Length() / 4;
                if (SP >= Speed)
                {
                    SP = Speed;
                }
                if (direction.Length() > 600 || TL)
                {
                    SP = Speed * 8;
                    Projectile.netUpdate = true;
                    if (DistancePlayer > 3000f)
                    {
                        Projectile.Center = player.Center;
                    }
                    TL = true;
                    if (direction.Length() < 120)
                    {
                        TL = false;
                    }
                }
                if(Projectile.velocity.Length()>Speed)
                {
                    Projectile.velocity *= 0.92F;
                }
                Projectile.velocity = (Projectile.velocity * inertia + direction.PerfectNormalize() * SP) / (inertia + 1);
            }
            else
            {
                Vector2 vector = new Vector2(-npc.width / 2, 0);
                vector.Y += Projectile.ai[2] <= 6 ? 50 : 120;
                Vector2 direction = npc.position - vector + (Projectile.ai[2] <= 6?npc.velocity*8:npc.velocity * 20) - Projectile.Center;
                float SP = direction.Length() / 4;
                if (SP >= Speed)
                {
                    SP = Speed;
                }
                Projectile.velocity = (Projectile.velocity * inertia + direction.PerfectNormalize() * SP) / (inertia + 1);
            }
            return false;
        }
        public override bool AttackAI()
        {
            Projectile.damage = (int)(Projectile.damage * (1+(Projectile.ai[2]-1)/2));
            shoot = Projectile.ai[2] <= 3 ? ModContent.ProjectileType<雨>() : ModContent.ProjectileType<电雨>();
            if (!target)
            {
                if (Projectile.ai[0] < AttackSpeed)
                {
                    Projectile.ai[0]++;
                }
                if (Projectile.ai[0] >= AttackSpeed/2 && DistancePlayer < 200)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Main.projectile[NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(Main.rand.NextFloat(-10, 10) * Projectile.scale, 0), new Vector2(0, shootSpeed/3), shoot, 0, Projectile.knockBack, Projectile.owner,0,0,0.35F)].originalDamage = Projectile.originalDamage;
                    }
                    Projectile.ai[0] = 0;
                }
            }
            else
            {
                Projectile.ai[0]++;
                Vector2 direction = npc.position - Projectile.Center;
                DistanceNPC = direction.Length();
                if (Projectile.ai[0] >= AttackSpeed && DistanceNPC < 300)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Main.projectile[NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(Main.rand.NextFloat(-10, 10) * Projectile.scale, 0), new Vector2(0, shootSpeed), shoot, Projectile.damage, Projectile.knockBack, Projectile.owner,0,0,0.75F* Projectile.scale)].originalDamage = Projectile.originalDamage;
                    }
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;
                    if (Projectile.ai[2] > 6 && Main.rand.NextBool(3))
                    {
                        int A = Main.projectile[NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(Main.rand.NextFloat(-10, 10) * Projectile.scale, 10), new Vector2(0, shootSpeed), ModContent.ProjectileType<闪电>(), Projectile.damage, Projectile.knockBack, Projectile.owner,2,0.5F,200)].originalDamage = Projectile.originalDamage;
                        Main.projectile[A].DamageType = DamageClass.Summon;
                    }
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            int frame = 5;
            int framex = Projectile.ai[2] <= 3 ? 0 : Projectile.ai[2] <= 6 ? 1 : 2;
            Rectangle rectangle = new(texture.Width / 3 * framex, texture.Height / frame * Projectile.frame, texture.Width / 3, texture.Height / frame);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rectangle, Projectile.GetAlpha(lightColor), Projectile.rotation, rectangle.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0);
            Main.EntitySpriteDraw(Glow.Value, Projectile.Center - Main.screenPosition, rectangle, Projectile.GetAlpha(Color.White), Projectile.rotation, rectangle.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0);
            return false;
        }
    }
}