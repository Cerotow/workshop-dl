using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.海幽浮王;
using DDmod.Content.Particles;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss水球 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;

        }
        int A;
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            Player player = Main.player[npc.target];
            Vector2 vector = player.Center - Projectile.Center;
            if (npc == null || npc.type != ModContent.NPCType<海幽浮王>() || !npc.active)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[1] <= 2 && npc.Dnpc().Stage != 0)
            {
                Projectile.Kill();
            }
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water);
            DDHelper.BackAndForth(-0.1F, 0.1F, 0.03F, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0], false);
            if (Projectile.alpha > 120)
            {
                if (Projectile.localAI[2] == 0)
                {
                    Projectile.localAI[2] = Projectile.damage;
                    Projectile.damage = 0;
                }
                if (Projectile.ai[1] <= 2)
                {
                    NewDustChange(2, Projectile.position, Projectile.Size, 33, 1, 4, false, Main.rand.NextFloat(0.5F, 1F) * Projectile.scale);
                }
                else
                {
                    NewDustChange(2, Projectile.position, Projectile.Size, ModContent.DustType<光球粒子>(), 1, 4, false, Main.rand.NextFloat(0.5F, 1F) * Projectile.scale, 100, new Color(30, 150, 255));
                }
                if (Projectile.ai[1] == 0)
                {
                    Projectile.alpha -= 2;
                    Projectile.position += npc.position - npc.oldPosition;
                }
                if (Projectile.ai[1] == 1)
                {
                    Projectile.alpha -= 1;
                    Projectile.DProj().vector[1] = npc.Center;
                    Projectile.localAI[0] += 0.08F;
                    Projectile.Center = Projectile.DProj().vector[1] + new Vector2(0, Projectile.ai[2]).RotatedBy(MathHelper.TwoPi / Projectile.DProj().vector[0].Y * Projectile.DProj().vector[0].X + Projectile.localAI[0]);
                }
                if (Projectile.ai[1] == 2)
                {
                    Projectile.alpha -= 1;
                    Projectile.DProj().vector[1] = npc.Center;
                    Projectile.localAI[0] -= 0.04F;
                    Projectile.Center = Projectile.DProj().vector[1] + new Vector2(0, Projectile.ai[2]).RotatedBy(MathHelper.TwoPi / Projectile.DProj().vector[0].Y * Projectile.DProj().vector[0].X + Projectile.localAI[0]);
                }
                if (Projectile.ai[1] == 3)
                {
                    Projectile.alpha -= 2;
                    Projectile.position += npc.position - npc.oldPosition;
                }
                if (Projectile.ai[1] == 4)
                {
                    Projectile.alpha -= 1;
                    Projectile.DProj().vector[1] = npc.Center;
                    Projectile.localAI[0] += 0.08F;
                    Projectile.Center = Projectile.DProj().vector[1] + new Vector2(0, Projectile.ai[2]).RotatedBy(MathHelper.TwoPi / Projectile.DProj().vector[0].Y * Projectile.DProj().vector[0].X + Projectile.localAI[0]);
                }
                if (Projectile.ai[1] == 5)
                {
                    Projectile.alpha -= 1;
                    Projectile.DProj().vector[1] = npc.Center;
                    Projectile.localAI[0] -= 0.04F;
                    Projectile.Center = Projectile.DProj().vector[1] + new Vector2(0, Projectile.ai[2]).RotatedBy(MathHelper.TwoPi / Projectile.DProj().vector[0].Y * Projectile.DProj().vector[0].X + Projectile.localAI[0]);
                }
            }
            else
            {
                Projectile.damage = (int)Projectile.localAI[2];
                if (Projectile.ai[1] <= 2)
                {
                    if (Projectile.wet)
                    {
                        Projectile.Kill();
                    }

                    Projectile.tileCollide = true;
                    Projectile.alpha = 0;
                    if (!Projectile.DProj().Bool[1])
                    {
                        NewDustChange(30, Projectile.position, Projectile.Size, 33, 4, 8, false, Main.rand.NextFloat(1F, 2F) * Projectile.scale);
                        if (Projectile.ai[1] == 0)
                        {
                            Projectile.velocity = new Vector2(vector.PerfectNormalize().X * 12, -Main.rand.NextFloat(10, 18));
                        }
                        if (Projectile.ai[1] == 1)
                        {
                            vector = player.Center - npc.Center;
                            Projectile.velocity = new Vector2(vector.PerfectNormalize().X * 18, -18);
                        }
                        if (Projectile.ai[1] == 2)
                        {
                            vector = player.Center - npc.Center;
                            Projectile.velocity = new Vector2(vector.PerfectNormalize().X * 18, -18);
                        }
                        Projectile.DProj().Bool[1] = true;
                    }
                    if (Projectile.ai[1] == 1)
                    {
                        Projectile.DProj().vector[1] += Projectile.velocity;
                        Projectile.localAI[0] += 0.08F;
                        Projectile.Center = Projectile.DProj().vector[1] + new Vector2(0, Projectile.ai[2]).RotatedBy(MathHelper.TwoPi / Projectile.DProj().vector[0].Y * Projectile.DProj().vector[0].X + Projectile.localAI[0]);
                    }
                    if (Projectile.ai[1] == 2)
                    {
                        Projectile.DProj().vector[1] += Projectile.velocity;
                        Projectile.localAI[0] -= 0.04F;
                        Projectile.Center = Projectile.DProj().vector[1] + new Vector2(0, Projectile.ai[2]).RotatedBy(MathHelper.TwoPi / Projectile.DProj().vector[0].Y * Projectile.DProj().vector[0].X + Projectile.localAI[0]);
                    }
                    if (Projectile.velocity.Y < 8)
                    {
                        Projectile.velocity.Y += 0.1F;
                    }
                    Projectile.velocity.X *= 0.98F;
                }
                else
                {
                    Projectile.alpha = 0;
                    if (!Projectile.DProj().Bool[1])
                    {
                        NewDustChange(15, Projectile.position, Projectile.Size, ModContent.DustType<光球粒子>(), 4, 8, false, Main.rand.NextFloat(1F, 2F) * Projectile.scale, 100, new Color(30, 150, 255));
                        NewDustChange(15, Projectile.position, Projectile.Size, ModContent.DustType<光球粒子>(), 4, 8, false, Main.rand.NextFloat(1F, 2F) * Projectile.scale, 100, new Color(110, 250, 255));
                        if (Projectile.ai[1] == 3)
                        {
                            Projectile.velocity = vector.PerfectNormalize() * 24;
                        }
                        if (Projectile.ai[1] == 4)
                        {
                            vector = player.Center - npc.Center;
                            Projectile.velocity = vector.PerfectNormalize() * 24;
                        }
                        if (Projectile.ai[1] == 5)
                        {
                            vector = player.Center - npc.Center;
                            Projectile.velocity = vector.PerfectNormalize() * 24;
                        }
                        Projectile.DProj().Bool[1] = true;
                    }
                    Projectile.velocity *= 0.98F;
                }
            }
            Projectile.ProjScaleChange();
            Projectile.position -= Projectile.velocity/2;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[1] <= 2)
            {
                NewDustChange(60, Projectile.position, Projectile.Size, 33, 1, Projectile.velocity.Length() / 2, false, Main.rand.NextFloat(1F, 2F) * Projectile.scale);
            }
            else
            {
                NewDustChange(30, Projectile.position, Projectile.Size, ModContent.DustType<光球粒子>(), 1, Projectile.velocity.Length() / 2, false, Main.rand.NextFloat(1F, 2F) * Projectile.scale, 100, new Color(30, 150, 255));
                NewDustChange(30, Projectile.position, Projectile.Size, ModContent.DustType<光球粒子>(), 1, Projectile.velocity.Length() / 2, false, Main.rand.NextFloat(1F, 2F) * Projectile.scale, 100, new Color(110, 250, 255));

            }
            SoundStyle sound = SoundID.SplashWeak;
            sound.Pitch = -1F;
            SoundEngine.PlaySound(sound  , Projectile.position);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2 = DDTextures.VoidStar.Value;
            Vector2 v = Projectile.Center - Main.screenPosition;
            
            if (Projectile.ai[1] > 2)
                lightColor = Color.White;
            Main.spriteBatch.Draw(texture, v, null, lightColor*(1F- Projectile.alpha/255F), Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1 - Projectile.DProj().Times[0], 1 + Projectile.DProj().Times[0]), 0, 0f);

            if (Projectile.ai[1] > 2)
            {
                Main.spriteBatch.Draw(texture2, v, null, new Color(30, 150, 255) * (1F - Projectile.alpha / 255F), Projectile.rotation, texture2.Size() / 2, Projectile.scale / 2 * new Vector2(1 - Projectile.DProj().Times[0], 1 + Projectile.DProj().Times[0]), 0, 0f);
                Main.spriteBatch.Draw(texture, v, null, new Color(30, 150, 255, 0) * (1F - Projectile.alpha / 255F), Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1 - Projectile.DProj().Times[0], 1 + Projectile.DProj().Times[0]), 0, 0f);
                for (int a = 0; a < Projectile.oldPos.Length; a++)
                {
                    Vector2 vector = Projectile.oldPos[a] + Projectile.Size / 2;
                    Main.spriteBatch.Draw(texture2, vector - Main.screenPosition, null, new Color(30, 150, 255) * (1F - Projectile.alpha / 255F) * 0.4f * (1 - (float)a / Projectile.oldPos.Length), Projectile.rotation, new Vector2(texture2.Width, texture2.Height) / 2, Projectile.scale / 2 * new Vector2(1 - Projectile.DProj().Times[0], 1 + Projectile.DProj().Times[0]) * (1 - (float)a / Projectile.oldPos.Length / 2), 0, 0f);

                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition, null, new Color(30, 150, 255, 0) * (1F - Projectile.alpha / 255F) * 0.4f * (1 - (float)a / Projectile.oldPos.Length), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale * new Vector2(1 - Projectile.DProj().Times[0], 1 + Projectile.DProj().Times[0]) * (1 - (float)a / Projectile.oldPos.Length / 2), 0, 0f);

                }
            }
            
            return false;
        }
    }
}