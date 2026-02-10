using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 白切灵剑Proj : Summons
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.width = 10;
            Projectile.height = 30;
            Projectile.minionSlots = 1;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<白切灵剑Buff>();
            Speed = 8;
            Projectile.DamageType = DamageClass.Summon;
            RectangularHitbox = true;
            Projectile.extraUpdates = 1;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0.5F;
            Projectile.GetGlobalProjectile<SummonProjectile>().RotateAI = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.frame = Main.rand.Next(3);
            Projectile.scale = 1.1F;
        }
        /// <summary>旧速度</summary>
        public Vector2[] oldVels;
        public override void Visual()
        {
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (Projectile.DProj().Times[0] <= 0)
                {
                    Projectile.RotationSpeed(vector.ToRotation(), 0.1F);
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            HitboxOrientation = Projectile.rotation;

        }
        public override bool MobileAI()
        {
            if (Projectile.DProj().Times[0] > 0)
            {
                Projectile.DProj().Times[0]--;
            }
            else
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                vector.Normalize();
                Projectile.velocity = Projectile.rotation.ToRotationVector2() * Speed;

                Projectile.netUpdate = true;
            }
            else
            {
                return true;

            }
            return false;
        }
        public override bool AttackAI()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            AttackFrame = 20;
            Projectile.DProj().Times[0] = 20;
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(255, 255, 255, 100) * 0.4F;
            Main.projectile[A].scale = 0.3F;
        }
        Color color = new Color(255, 255, 255, 0);
        public Color TrailColor(float completionRatio)
        {
            float trailOpacity = Utils.GetLerpValue(0f, 0.1f, completionRatio, true) * Utils.GetLerpValue(0.7f, 0.58f, completionRatio, true);
            Color startingColor = Color.Lerp(color, color, 0.07f);
            return playerHelper.MulticolorLerp(completionRatio, new Color[]
            {
                startingColor,
            }) * trailOpacity;
        }
        public float TrailWidth(float completionRatio)
        {
            return 13 * Projectile.scale;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;
            Rectangle rectangle = new Rectangle(texture.Width / 3 * Projectile.frame, 0, texture.Width / 3, texture.Height);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size/2;
                Main.EntitySpriteDraw(texture, vector2, rectangle, color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2), Projectile.oldRot[i] + MathHelper.PiOver4, rectangle.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0);
            }
            Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, Projectile.rotation + MathHelper.PiOver4, rectangle.Size() / 2f, Projectile.scale, 0, 0f);

            return false;
        }
    }
}