using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 神庙探针 : Summons
    {
        static Asset<Texture2D> asset;
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<神庙探针Buff>(), 12, 21, 800, ModContent.ProjectileType<S石魔弹>(), 4, 30, 300);
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.minionSlots = 1;
        }
        public override void Load()
        {
            asset = ModContent.Request<Texture2D>(Texture+"_Glow");
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
            if (target)
            {
                Projectile.rotation = (Projectile.Center - npc.Center).ToRotation() - MathHelper.PiOver2;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            if (Projectile.localAI[0] < 0.5f)
            {
                Projectile.localAI[0] = 0.5f;
                Projectile.localAI[1] = 1;
            }
            if (Projectile.localAI[0] > 0.8f)
            {
                Projectile.localAI[0] = 0.8f;
                Projectile.localAI[1] = 0;
            }
            if (Projectile.localAI[1] == 1)
            {
                Projectile.localAI[0] += 0.05f;
            }
            else
            {
                Projectile.localAI[0] -= 0.05f;
            }
            Dust.NewDust(Projectile.Center, 20, 20, ModContent.DustType<光球粒子>());
            Lighting.AddLight(Projectile.Center, new Color(0, 255, 0).ToVector3() * 0.25f);
        }
        public override bool AttackAI()
        {
            Projectile.ai[2]--;
            if (target)
            {
                Projectile.ai[0]++;
                Vector2 direction = npc.Center - Projectile.Center;
                DistanceNPC = direction.Length();
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
                        int A = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * shootSpeed, shoot, Projectile.damage, Projectile.knockBack, Projectile.owner);
                        Main.projectile[A].DamageType = DamageClass.Summon;
                        Main.projectile[A].originalDamage = Projectile.originalDamage;
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
                if (Projectile.ai[0] < AttackSpeed)
                {
                    Projectile.ai[0]++;
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Texture2D Starlight = DDTextures.Starlight3.Value;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Color color = new Color(255, 170, 0, 0);
            Vector2 vector = Projectile.Center - Main.screenPosition + ((Projectile.rotation - MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * -6);
            Main.EntitySpriteDraw(VoidStar, vector, new Rectangle?(new Rectangle(0, 0, (int)(VoidStar.Width / 1.7F), VoidStar.Height)), color * 0.8f, Projectile.rotation - MathHelper.PiOver2, VoidStar.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 3.5F) / 1.5F * Projectile.localAI[0], 0, 0);
            Main.EntitySpriteDraw(VoidStar, vector, new Rectangle?(new Rectangle(0, 0, (int)(VoidStar.Width / 1.7F), VoidStar.Height)), color * 0.8f, Projectile.rotation - MathHelper.PiOver2, VoidStar.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 3.5F) / 1.5F * Projectile.localAI[0], 0, 0);
            Main.EntitySpriteDraw(VoidStar, vector, new Rectangle?(new Rectangle(0, 0, (int)(VoidStar.Width / 1.7F), VoidStar.Height)), color.Opposite() * 0.8f, Projectile.rotation - MathHelper.PiOver2, VoidStar.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 3.5F) / 3F * Projectile.localAI[0], 0, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, (SpriteEffects)1, 0);
            Main.EntitySpriteDraw(asset.Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, (SpriteEffects)1, 0);
            if (Projectile.ai[2]>0)
            {
                Main.EntitySpriteDraw(Starlight, Projectile.Center - Main.screenPosition+ (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2()*10, null, color * 0.8f, Projectile.rotation - MathHelper.PiOver2, Starlight.Size() / 2, new Vector2(Projectile.scale/2, Projectile.scale*1.25F), 0, 0);
                Main.EntitySpriteDraw(Starlight, Projectile.Center - Main.screenPosition + (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 10, null, color * 0.8f, Projectile.rotation, Starlight.Size() / 2, new Vector2(Projectile.scale/2, Projectile.scale), 0, 0);

            }
            return false;
        }
    }
}