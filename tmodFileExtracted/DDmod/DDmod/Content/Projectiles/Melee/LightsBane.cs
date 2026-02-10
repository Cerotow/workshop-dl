using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee
{
    public class LightsBane : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Lights Bane");
           //DisplayName.AddTranslation(7, "光之驱逐");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.alpha += 255;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            DDHelper.BackAndForth(2, 3, 0.1f, ref Projectile.ai[0], ref Projectile.DProj().Bool[0]);

            int Type = ModContent.DustType<速度粒子>();
            Dust dust = Main.dust[NewDust(Projectile.position-new Vector2(10)-Projectile.velocity.PerfectNormalize()*20, Projectile.width+20, Projectile.height+20, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
            dust.noGravity = true;
            dust.scale = Main.rand.NextFloat(0.6F,1.6F);
            dust.velocity = Projectile.velocity/4;
            dust.rotation = dust.velocity.ToRotation();
            dust.color = new Color(99, 74, 187, 255);

            Projectile.ai[1]++;
            if (Projectile.ai[1] > 12)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 20;
                }
                else if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            int Type = ModContent.DustType<速度粒子>();
            for (int A = 0; A < 30; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 2.8f;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2f, 10f);
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();
                dust.color= new Color(99, 74, 187, 255);
                dust.noLightEmittence = false;
            }
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 0.4f;
            PlaySound(sound, Projectile.position);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            /*for (int a = 0; a < 1; a++)
            {
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 10, ModContent.ProjectileType<LightsBaneCut>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
            }*/
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.GetAlpha(new Color(129, 0, 187, 200));
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int a = 0; a < Projectile.ai[0]; a++)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / Projectile.ai[0]);
                    Main.spriteBatch.Draw(texture, vector2, null, oldcolor*0.5f, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.ai[0] / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.4f, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.ai[0] / 2, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.4f, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.ai[0] / 2, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.4f, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.ai[0] / 2, spriteEffects, 0f);
            }
            return false;
        }
    }
}