using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;

namespace DDmod.Content.Projectiles.Melee
{
    public class 圣金剑 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.alpha = 255;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            DDHelper.BackAndForth(2, 3, 0.1f, ref Projectile.ai[0], ref Projectile.DProj().Bool[0]);

            if (Projectile.alpha == 0 && Main.rand.NextBool(10))
            {
                int Type = ModContent.DustType<速度粒子>();
                Dust dust = Main.dust[NewDust(Projectile.position - Projectile.velocity.PerfectNormalize() * 20, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = Projectile.scale * Main.rand.NextFloat(0.6F, 1.6F);
                dust.velocity = Projectile.velocity / 4;
                dust.rotation = dust.velocity.ToRotation();
                dust.customData = 0.5f;
                dust.color = new Color(255, 249, 59, 4);
            }
            Projectile.ai[1]++;
            if (Projectile.ai[2] <= -10000)
            {
                if (Projectile.localAI[0] == 0)
                {
                    Projectile.scale *= 2F;
                    Projectile.ProjScaleChange();
                    Projectile.penetrate = -1;
                    Projectile.localAI[0] = 1;
                    Projectile.velocity *= 2;
                }
                if (Projectile.alpha > 0 && Projectile.ai[1] > 5)
                {
                    Projectile.alpha -= 20;
                }
                else if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
            else if (Projectile.ai[2] > -1000)
            {
                if (Projectile.ai[2] < 0)
                {
                    Projectile.ai[2]++;
                }
                if (Projectile.ai[2] >= 0 && Projectile.ai[1] > 5)
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
                else
                {
                    if (Projectile.ai[2] > -10000)
                    {
                        if (Projectile.oldPosition != Vector2.Zero)
                            Projectile.position -= Projectile.velocity;
                        Projectile.position += Projectile.Player().Dplayer().PrePosition;
                    }
                }
                if (Projectile.localAI[0] == 0)
                {
                    Projectile.scale *= 0.75F;
                    Projectile.ProjScaleChange();
                    Projectile.localAI[0] = 1;
                }
                if (Projectile.ai[2] < 1)
                {
                    Projectile.Track(500, 20, 22);
                }
            }
            else
            {

                if ( Projectile.ai[1] > 5)
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
        }
        public override void OnKill(int timeLeft)
        {
            int Type = ModContent.DustType<速度粒子>();
            for (int A = 0; A < Projectile.scale * 17; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = Projectile.scale * 2.8f;
                dust.velocity = Projectile.scale * Main.rand.NextVector2Unit() * Main.rand.NextFloat(6.2f, 12f);
                dust.rotation = dust.velocity.ToRotation();
                dust.color = new Color(255, 249, 59, 4);
                dust.customData = 2;
                dust.noLightEmittence = false;
            }
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 0.4f;
            PlaySound(sound, Projectile.position);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(Projectile.velocity.X!=oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            /*for (int a = 0; a < 1; a++)
            {
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 10, ModContent.ProjectileType<LightsBaneCut>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
            }*/
            if (Projectile.ai[2] > -1000)
            {
                Projectile.ai[2]++;
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0,Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(255, 249, 59, 4) * 0.3F;
            Main.projectile[A].localAI[0] = Projectile.scale;
            Main.projectile[A].scale = Projectile.scale;
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
            Color color = Projectile.GetAlpha(new Color(255, 249, 59, 0))*0.5F;
            color.A = 0;
            Color color2 = Projectile.GetAlpha(new Color(120, 120, 120, 100));
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int a = 0; a < Projectile.ai[0]; a++)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color oldcolor = color2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / Projectile.ai[0]);
                    Main.spriteBatch.Draw(texture, vector2, null, oldcolor*0.5f, Projectile.oldRot[i], new Vector2(texture.Width, 0), Projectile.scale * Projectile.ai[0] / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                    oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / Projectile.ai[0]);
                    Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.oldRot[i], new Vector2(texture.Width, 0), Projectile.scale * Projectile.ai[0] / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color2 * 0.4f, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.scale * Projectile.ai[0] / 2, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.scale * Projectile.ai[0] / 2, spriteEffects, 0f);
         }
            return false;
        }
    }
}