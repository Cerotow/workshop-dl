using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 土灵 : Summons
    {

        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
        }
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<土灵Buff>(), 8, 21, 800, ModContent.ProjectileType<土>(), 12, 30, 150);
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
            Projectile.rotation = Projectile.velocity.X * 0.03f;
            if (!Projectile.DProj().Bool[0])
            {
                for (float A = 0; A < 22; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center-new Vector2(4), 1, 1, 0, Projectile.oldVelocity.X, Projectile.oldVelocity.Y)];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale*1.25F;
                    dust.velocity =Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*Main.rand.NextFloat(1,4);
                }
                Projectile.DProj().Bool[0] = true;
            }
            Projectile.frameCounter++;
            Projectile.frame = (int)Projectile.frameCounter / 6 % 4;
        }
        public override bool AttackAI()
        {
            if (!target)
            {
                if (Projectile.ai[0] < AttackSpeed)
                {
                    Projectile.ai[0]++;
                }
                Projectile.spriteDirection = 0;
                if (Projectile.velocity.X < 0)
                {
                    Projectile.spriteDirection = 1;
                }
            }
            else
            {
                Projectile.ai[0]++;
                Vector2 direction = npc.Center+npc.velocity/10 - Projectile.Center;
                DistanceNPC = direction.Length();
                direction = direction.PerfectNormalize();
                if (Projectile.ai[0] >= AttackSpeed && DistanceNPC<200)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Main.projectile[NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction.RotatedBy(Main.rand.NextFloat(-0.1F,0.1F)) * shootSpeed, shoot, Projectile.damage, Projectile.knockBack, Projectile.owner)].originalDamage = Projectile.originalDamage;
                    }
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;
                }

                direction = npc.Center - Projectile.Center;
                direction = direction.PerfectNormalize();
                if (Projectile.ai[0] == 0)
                {
                    Projectile.velocity = -direction * 4;
                    for (float A = 0; A < 22; A++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 0, Projectile.oldVelocity.X, Projectile.oldVelocity.Y)];
                        dust.noGravity = true;
                        dust.scale = Projectile.scale;
                        dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(2);
                    }
                }
                Projectile.spriteDirection = 0;

                if (direction.X < 0)
                {
                    Projectile.spriteDirection = 1;
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Rectangle rectangle = new(0,texture.Height/4*Projectile.frame,texture.Width,texture.Height/4);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, rectangle.Size()/2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0);
            Main.EntitySpriteDraw(Glow.Value, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0);
            return false;
        }
    }
}