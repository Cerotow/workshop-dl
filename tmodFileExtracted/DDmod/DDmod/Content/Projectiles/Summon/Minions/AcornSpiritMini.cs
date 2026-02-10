using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class AcornSpiritMini : Summons
    {

        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Glow2;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Summon/Minions/AcornSpiritMini_Glow");
            Glow2 = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Summon/Minions/AcornSpiritMini_Glow2");
        }
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<AcornSpiritBuff>(), 12, 21, 800, ModContent.ProjectileType<SummonNaturalLight>(), 15, 60, 200);
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
            Projectile.rotation = Projectile.velocity.X * 0.03f;
            Lighting.AddLight(Projectile.Center, new Vector3(20, 255, 20) * (0.003F * Projectile.ai[0] / 50));
        }
        public override bool AttackAI()
        {
            if (!target)
            {
                if (Projectile.ai[0] < AttackSpeed)
                {
                    Projectile.ai[0]++;
                }
            }
            else
            {
                Vector2 direction = npc.Center - Projectile.Center;
                direction = direction.PerfectNormalize();
                if (Projectile.ai[0] == 0)
                {
                    Projectile.velocity = direction * -13;
                }
            }
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // Texture2D texture = DDTextures.VoidStar.Value
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D texture2 = Glow.Value;
            Vector2 vector = Projectile.Size / 2;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color2 = Projectile.GetAlpha(new Color(0, 255, 0, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(texture2, vector2, null, color2, Projectile.rotation, texture2.Size() / 2, (Projectile.ai[0] / 50) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                Main.spriteBatch.Draw(texture2, vector2, null, color2, Projectile.rotation, texture2.Size() / 2, (Projectile.ai[0] / 50) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                Main.spriteBatch.Draw(texture2, vector2, null, color2, Projectile.rotation, texture2.Size() / 2, (Projectile.ai[0] / 50) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.position - Main.screenPosition + vector, null, lightColor, Projectile.rotation, texture.Size()/2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0);
            Main.EntitySpriteDraw(Glow2.Value, Projectile.position - Main.screenPosition + vector, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}