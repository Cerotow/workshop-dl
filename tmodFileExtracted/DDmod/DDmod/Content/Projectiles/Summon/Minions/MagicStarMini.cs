using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class MagicStarMini : Summons
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
            Projectile.width = 34;
            Projectile.height = 36;
            Projectile.minionSlots = 1;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<MagicStarBuff>();
            Speed = 18;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.GetGlobalProjectile<SummonProjectile>().RotateAI = true;
        }
        public override void Visual()
        {
            Projectile.rotation += 0.08f;
        }
        public override bool MobileAI()
        {
            if (AttackFrame > 0)
            {
                target = false;
            }
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                Projectile.velocity = vector.PerfectNormalize() * Speed;
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
            Projectile.netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D textureGlow = DDTextures.VoidStar.Value;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + texture.Size() / 2;
                Color color = Utils.MultiplyRGBA(new Color(255, 255, 255, 0), new Color(0, 150, 255)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.EntitySpriteDraw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1.2f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0);
                Main.EntitySpriteDraw(textureGlow, vector2, null, Utils.MultiplyRGBA(new Color(255, 255, 255, 0), new Color(0, 100, 255)) * 0.4f, 1, textureGlow.Size() / 2, 0.8f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, v, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0);

            Main.EntitySpriteDraw(textureGlow, v, null, Utils.MultiplyRGBA(new Color(255, 255, 255, 0), new Color(0, 100, 255)) * 0.4f, 1, textureGlow.Size() / 2, 0.8f, SpriteEffects.None, 0);

            return false;
        }
    }
}