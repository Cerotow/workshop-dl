using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Boss;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.BattlePets.Proj
{
    public class 鬼魂 : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void AI()
        {
            //帧图
            if (Projectile.localAI[0] == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
                Projectile.localAI[0] = 1;
            }
            else
            {
                Projectile.localAI[0]++;
            }
            Projectile.localAI[1]++;
            if (Projectile.localAI[1] >= 4)
            {
                Projectile.frame++;
                if (Projectile.frame >= 3)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.velocity = (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * Projectile.ai[0];
            NPC npc = NPCdirection.FindClosest(Projectile.Center, 1500);
            if (npc != null)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                Projectile.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.04F);
                if (DDHelper.SpecifyDirection(Projectile.rotation, vector.ToRotation() - MathHelper.PiOver2, 0.04F))
                {
                    if (Projectile.ai[0] < 8)
                    {
                        Projectile.ai[0] += 0.3F;
                    }
                }
                else
                {

                    Projectile.ai[0] *= 0.98f;
                }
            }
            else
            {
                if (Projectile.ai[0] < 8)
                {
                    Projectile.ai[0] += 0.3F;
                }
            }
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override void OnKill(int timeLeft)
        {
            SoundStyle sound = SoundID.NPCDeath39;
            sound.MaxInstances = 50;
            sound.Volume = 0.1F;
            sound.Pitch = -1;
            PlaySound(sound, Projectile.Center);
            for (int A = 0; A < 4; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, new Color(214, 241, 241, 0))];
                dust.noGravity = true;
                dust.scale = 0.1f;
                dust.alpha = -5;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                dust.velocity = Vector2.Zero;
                dust.noLightEmittence = false;
                dust.customData = new Vector4(0.1F, 40 * Projectile.scale, 0, 1F);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Size / 2;
            Rectangle rectangle = new Rectangle(0, texture.Height / 3 * Projectile.frame, texture.Width, texture.Height / 3);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color color = new Color(255, 255, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 0.5f;
                Main.spriteBatch.Draw(texture, vector2, rectangle, color, Projectile.oldRot[i], rectangle.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(255, 255, 255, 0), Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);

            return false;

        }
    }
    public class 夺命鬼魂 : 鬼魂
    {

        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[2]==0?null:false;
        }
        public override void AI()
        {
            //帧图
            if (Projectile.localAI[0] == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
                Projectile.localAI[0] = 1;
            }
            else
            {
                Projectile.localAI[0]++;
            }
            Projectile.localAI[1]++;
            if (Projectile.localAI[1] >= 4)
            {
                Projectile.frame++;
                if (Projectile.frame >= 3)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.velocity = (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * Projectile.ai[0];
            if (Projectile.ai[2] == 0)
            {
                NPC npc = NPCdirection.FindClosest(Projectile.Center, 1500);
                if (npc != null)
                {
                    Vector2 vector = npc.Center - Projectile.Center;
                    Projectile.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.04F);
                    if (DDHelper.SpecifyDirection(Projectile.rotation, vector.ToRotation() - MathHelper.PiOver2, 0.04F))
                    {
                        if (Projectile.ai[0] < 8)
                        {
                            Projectile.ai[0] += 0.3F;
                        }
                    }
                    else
                    {

                        Projectile.ai[0] *= 0.98f;
                    }
                }
                else
                {
                    if (Projectile.ai[0] < 8)
                    {
                        Projectile.ai[0] += 0.3F;
                    }
                }
            }
            else
            {
                Player player = Main.player[Player.FindClosest(Projectile.Center,1,1)];
                if (player != null)
                {
                    Vector2 vector = player.Center - Projectile.Center;
                    Projectile.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.04F);
                    if (DDHelper.SpecifyDirection(Projectile.rotation, vector.ToRotation() - MathHelper.PiOver2, 0.04F))
                    {
                        if (Projectile.ai[0] < 8)
                        {
                            Projectile.ai[0] += 0.3F;
                        }
                    }
                    else
                    {

                        Projectile.ai[0] *= 0.98f;
                    }
                }
                else
                {
                    if (Projectile.ai[0] < 8)
                    {
                        Projectile.ai[0] += 0.3F;
                    }
                }
                if(player.getRect().Intersects(Projectile.getRect()))
                {
                    if (player.whoAmI == Main.myPlayer)
                        player.Heal(Projectile.damage/20);
                    Projectile.Kill();
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            Color color = new Color(148, 43, 43, 0);
            SoundStyle sound = SoundID.NPCDeath39;
            sound.MaxInstances = 50;
            sound.Volume = 0.1F;
            if (Projectile.ai[2]==1)
            {
                color = new Color(85, 228, 85, 0);
                sound.Pitch = -1;
                PlaySound(sound, Projectile.Center);
            }
            else
            {
                sound.Pitch = 1;
            }
            PlaySound(sound, Projectile.Center);
            for (int A = 0; A < 4; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, color)];
                dust.noGravity = true;
                dust.scale = 0.1f;
                dust.alpha = -5;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                dust.velocity = Vector2.Zero;
                dust.noLightEmittence = false;
                dust.customData = new Vector4(0.1F, 40 * Projectile.scale, 0, 1F);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.ai[2] = 1;
            Projectile.netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Size / 2;
            Rectangle rectangle = new Rectangle(texture.Width / 2 * (int)Projectile.ai[2], texture.Height / 3 * Projectile.frame, texture.Width / 2, texture.Height / 3);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color color = new Color(255, 255, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 0.5f;
                Main.spriteBatch.Draw(texture, vector2, rectangle, color, Projectile.oldRot[i], rectangle.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(255, 255, 255, 0), Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);

            return false;

        }
    }
}