using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class MeteorFighter : Summons
    {

        public static Asset<Texture2D> NPCtexture;
        public override void Load()
        {
            NPCtexture = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Summon/Minions/MeteorFighter");
        }
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<MeteorFighterBuff>(), 12, 21, 800, 20, 15, 25, 300);
            Projectile.width = 20;
            Projectile.height = 20;
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
            Lighting.AddLight(Projectile.Center, new Color(233, 66, 4).ToVector3());

            Lighting.AddLight(Projectile.Center, new Color(0, 255, 0).ToVector3() * 0.25f);
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
                    int A = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * shootSpeed, shoot, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[A].DamageType = DamageClass.Summon;
                    Main.projectile[A].minion= true;
                    Main.projectile[A].originalDamage = Projectile.originalDamage;
                    Main.projectile[A].scale /= 1.5f;
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = -4;
                    PlaySound(SoundID.Item157, Projectile.position);
                }
            }
            else
            {
                Projectile.ai[0] = 0;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Texture2D Starlight = DDTextures.Starlight.Value;
            Texture2D texture = NPCtexture.Value;

            Vector2 vector = Projectile.Center - Main.screenPosition + ((Projectile.rotation - MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * -24);
            Main.EntitySpriteDraw(VoidStar, vector, new Rectangle?(new Rectangle(0, 0, (int)(VoidStar.Width / 1.7F), VoidStar.Height)), new Color(233, 66, 4, 0) * 0.8f, Projectile.rotation - MathHelper.PiOver2, VoidStar.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2.5F) / 1.5F * Projectile.localAI[0], 0, 0);
            Main.EntitySpriteDraw(VoidStar, vector, new Rectangle?(new Rectangle(0, 0, (int)(VoidStar.Width / 1.7F), VoidStar.Height)), new Color(233, 66, 4, 0) * 0.8f, Projectile.rotation - MathHelper.PiOver2, VoidStar.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2.5F) / 1.5F * Projectile.localAI[0], 0, 0);
            Main.EntitySpriteDraw(VoidStar, vector, new Rectangle?(new Rectangle(0, 0, (int)(VoidStar.Width / 1.7F), VoidStar.Height)), new Color(233, 66, 4, 0).Opposite() * 0.8f, Projectile.rotation - MathHelper.PiOver2, VoidStar.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2.5F) / 3F * Projectile.localAI[0], 0, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, (SpriteEffects)1, 0);
            if (Projectile.ai[0] < 0)
            {
                vector = Projectile.Center - Main.screenPosition + ((Projectile.rotation - MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * 16);
                Main.EntitySpriteDraw(VoidStar, vector, null, new Color(0, 255, 0, 0) * 0.5f, Projectile.rotation - MathHelper.PiOver2, VoidStar.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2) / 1.5F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(0, 255, 0, 0), Projectile.rotation - MathHelper.PiOver2, Starlight.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2) / 1.5F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(255, 0, 255, 0), Projectile.rotation - MathHelper.PiOver2, Starlight.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 2) / 3, 0, 0);
            }
            return false;
        }
    }
}