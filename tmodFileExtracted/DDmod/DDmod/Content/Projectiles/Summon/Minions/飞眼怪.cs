using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 飞眼怪 : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public static Asset<Texture2D> Asset;
        public override void Load()
        {
            Asset = ModContent.Request<Texture2D>(Texture + "E");
        }
        public override void SetDefault()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.minionSlots = 0.5f;
            Defaults(ModContent.BuffType<飞眼怪Buff>(), 12, 21, 800, 0, 0, 0, 0);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        Vector2 V;
        Vector2 V2;
        public override void Visual()
        {
            V -= (V - V2)/60;
        }
        public override bool MobileAI()
        {
            if (Projectile.ai[2] >= 5)
            {
                Vector2 vector = player.Center - Projectile.Center;
                V2 = vector;
                Projectile.SmoothVelocity(vector.PerfectNormalize() * Speed, inertia);
                if(vector.Length()<10)
                {
                    if (player.whoAmI == Main.myPlayer)
                        player.Heal((int)Projectile.ai[2]);
                    Projectile.ai[2] -= (int)Projectile.ai[2];
                }
            }
            else
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                V2 = vector;
                if (vector.Length() > 100)
                {
                    Projectile.SmoothVelocity(vector.PerfectNormalize() * Speed, inertia);
                }
                else
                {
                    if (Projectile.velocity.Length() < Speed)
                    {
                        Projectile.velocity *= 1.02F;
                    }
                }
            }
            else
            {
                Vector2 vector = player.Center - Projectile.Center;
                V2 = Projectile.velocity.PerfectNormalize() * 200;
                if (vector.Length() > 100)
                {
                    Projectile.SmoothVelocity(vector.PerfectNormalize() * Speed, inertia);
                }
                else
                {
                    if (Projectile.velocity.Length() < Speed)
                    {
                        Projectile.velocity *= 1.02F;
                    }
                }
            }
            return false;
        }
        public override bool AttackAI()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.ai[2] += damageDone / 40F;
            Projectile.netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float R = Projectile.ai[2] / 5;
            if (R > 1) R = 1;

            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D textureGlow = DDTextures.VoidStar.Value;

            Main.EntitySpriteDraw(textureGlow, v, null, new Color(255, 100, 100) * R, 1, textureGlow.Size() / 2, 0.8f, SpriteEffects.None, 0);
            if (R >= 1)
            {

                Main.EntitySpriteDraw(textureGlow, v, null, new Color(255, 100, 100) * R, 1, textureGlow.Size() / 2, 0.8f, SpriteEffects.None, 0);
            }
                Main.EntitySpriteDraw(texture, v, null, lightColor, 0, texture.Size() / 2, Projectile.scale, 0, 0);
            texture = Asset.Value;
            Vector2 V = this.V/10;
            if(V.Length()>8)
            {
                V = V.PerfectNormalize() * 8;
            }
                Main.EntitySpriteDraw(texture, v + V, null, lightColor, V.ToRotation(), texture.Size() / 2, Projectile.scale*new Vector2((1 - V.Length() / 8)*0.25f + 0.75F, 1), 0, 0);

            return false;
        }
    }
}