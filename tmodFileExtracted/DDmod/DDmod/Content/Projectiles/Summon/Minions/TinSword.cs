using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class TinSword : Summons
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.width = 10;
            Projectile.height = 28;
            Projectile.minionSlots = 1;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<TinSwordBuff>();
            Speed = 8;
            Projectile.DamageType = DamageClass.Summon;
            RectangularHitbox = true;
            Projectile.extraUpdates = 2;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0.5F;
            Projectile.GetGlobalProjectile<SummonProjectile>().RotateAI = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        /// <summary>旧速度</summary>
        public Vector2[] oldVels;
        public override void Visual()
        {
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                Projectile.rotation = vector.ToRotation() + MathHelper.PiOver4;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            HitboxOrientation = Projectile.rotation - MathHelper.PiOver4;

        }
        public override bool MobileAI()
        {
            if (Projectile.DProj().Times[0] > 0)
            {
                Projectile.DProj().Times[0]--;
            }
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                vector.Normalize();
                if (Projectile.DProj().Times[0] <= 0)
                {
                    Projectile.velocity = (Projectile.velocity *20+ vector * Speed)/21;
                }
                else
                {
                    Projectile.oldPos[0] = Vector2.Zero;
                    Projectile.velocity = vector * -Speed/4;
                }
                Projectile.netUpdate = true;
                return false;
            }
            return true;
        }
        public override bool AttackAI()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            AttackFrame = 60;
            Projectile.DProj().Times[0] = 60;
        }
        Color color = new Color(250, 134, 71, 0);
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

            for (int i = 0; i < Projectile.oldPos.Length; i+=3)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size/2;
                Color color = lightColor * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.EntitySpriteDraw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0);
            }
            Main.spriteBatch.Draw(texture, Center, null, lightColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, 0, 0f);

            return false;
        }
        internal Trailing TrailDrawer;
    }
}