
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Players;
using System.Reflection;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.SwordShield
{
    public class 流星剑盾Proj : 剑盾
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Melee/SwordShield/流星剑盾Proj_Glow");
        }
        public override void Defaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 36;
            Projectile.extraUpdates = 6;
            EffectLength = 20;
            HandheldOffset = 26;
        }
        public override Color color => new Color(233, 86, 3, 40);
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if(Projectile.DProj().Bool[0])
            {
                Projectile.damage *= 3;
                Projectile.knockBack *= 2;
                Projectile.DProj().Magnification *= 3;
                Projectile.DProj().Bool[0] = false;
                Projectile.DProj().Bool[1] = true;
            }
            if (Projectile.DProj().Back == 1)
            {
                Projectile.extraUpdates = 0;
            }
                return true;
        }
        public override void Shoot()
        {

            if (Projectile.DProj().Bool[1])
            {
                for (int a = 0; a < 4; a++)
                {
                    Vector2 ve = new Vector2(Main.rand.Next(10, 300) * player.direction, 600 + (player.Dplayer().MouseWorld.Y - player.Center.Y));
                    NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - ve, ve.PerfectNormalize() * 9, 424 + Main.rand.Next(3), Projectile.damage, 2, Projectile.owner, 1, 1);
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;

            NewProjectile(Projectile.GetSource_FromAI(), target.Center, new Vector2(player.direction * 0.01F, 0), ModContent.ProjectileType<FlameExplosion>(), hit.SourceDamage, hit.Knockback * 2, player.whoAmI);


            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.DProj().Back == -1)
            {
                vector += new Vector2(10 * Projectile.Player().direction, 0);
                DDHelper.BladeTrail(DDTextures.WhitePng, color, 1F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
                DDHelper.BladeTrail(DDTextures.Wave, color, 1F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;
            Vector2 Scale = new Vector2(Projectile.scale);
            //Scale=new Vector2(Projectile.scale*1F,Projectile.scale*0.5F);
            Time += 1;
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.spriteDirection == 0)
                {
                    if (Projectile.DProj().Back == -1)
                    {
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(Glow.Value, Center, new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height)), new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(Glow.Width() / 2, Glow.Height()) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                    }
                    else
                    {
                        float Sc = (float)Time / 100F;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);
                        Sc += 0.33f;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);
                        Sc += 0.33F;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);

                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(Glow.Value, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                else
                {
                    if (Projectile.DProj().Back == -1)
                    {
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height)), lightColor, Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(Glow.Value, Center, new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height)), new Color(255,255,255,0), Projectile.rotation + MathHelper.PiOver2, new Vector2(Glow.Width() / 2, Glow.Height()) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                    }
                    else
                    {
                        float Sc = (float)Time / 100F;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);
                        Sc += 0.33f;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);
                        Sc += 0.33F;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);

                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(Glow.Value, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
    }
}