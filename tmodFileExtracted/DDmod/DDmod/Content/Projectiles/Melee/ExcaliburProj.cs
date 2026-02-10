using Terraria;

namespace DDmod.Content.Projectiles.Melee
{
    public class ExcaliburProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.alpha += 255;
            Projectile.scale =1f;
        }
        public override bool? CanDamage()
        {
            return Projectile.localAI[0] > 20&& Projectile.localAI[1]==0;
        }
        public override void AI()
        {
            if (Projectile.Player().Aplayer().HolyEnergy)
            {
                Projectile.scale = 1f;
            }
            else
            {
                Projectile.scale = 0.8f;
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 12)
            {
                if (Projectile.localAI[1] == 0)
                {
                    if (Projectile.alpha > 0)
                    {
                        Projectile.alpha -= 100;
                    }
                    else if (Projectile.alpha < 0)
                    {
                        Projectile.alpha = 0;
                    }
                }
                else
                {
                    if (Projectile.alpha < 255)
                    {
                        Projectile.alpha += 30;
                    }
                    else if (Projectile.alpha > 255)
                    {
                        Projectile.Kill();
                    }
                }
            }
            Projectile.localAI[0]++;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.ai[0] == 0)
            {
                if (Projectile.localAI[0] == 20)
                    Projectile.velocity *= 5f;
            }
            else
            {
                Projectile.position += Main.npc[(int)Projectile.ai[2]].position - Main.npc[(int)Projectile.ai[2]].oldPosition;
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Projectile.oldPos[i] += Main.npc[(int)Projectile.ai[2]].position - Main.npc[(int)Projectile.ai[2]].oldPosition;

                }
                    if (Projectile.localAI[0] == 20)
                    Projectile.velocity *= 5f;
            }
            Projectile.ProjScaleChange();
        }
        public override void OnKill(int timeLeft)
        {
            int Type = 57;
            for (int A = 0; A < 30; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position-Projectile.velocity.PerfectNormalize()*Projectile.height*2, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 0.8f;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2f, 3f);
                dust.color = new Color(236, 200, 89, 0);
                dust.noLightEmittence = false;
            }
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 0.4f;
            PlaySound(sound, Projectile.position);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.localAI[1]++;
            Projectile.velocity = Projectile.velocity * 0.08f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0] < 20)
            {
                Projectile.oldPos[0] = Vector2.Zero;
            }

            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.GetAlpha(new Color(236, 200, 89));
            color.A = 0;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Color oldcolor2 = Projectile.GetAlpha(new Color(100, 100, 100)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor2, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }

            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Projectile.GetAlpha(new Color(100, 100, 100)), Projectile.rotation, new Vector2(texture.Width, 0), Projectile.scale, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.scale, spriteEffects, 0f);

            return false;
        }
    }
}