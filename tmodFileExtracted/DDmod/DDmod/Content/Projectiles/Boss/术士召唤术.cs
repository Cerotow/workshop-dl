using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.先祖咒魂;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics.Metrics;

namespace DDmod.Content.Projectiles.Boss
{/// <summary>
/// Projectile.ai[0]
/// 1召唤混沌球
/// 2召唤旋转混沌球,会变成小怪
/// </summary>
    public class 术士召唤术 : ModProjectile
    {

        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.scale = 1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1800;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Projectile.ProjScaleChange();
            if (Projectile.scale < Projectile.ai[1])
            {
                Projectile.scale += 0.2F;
            }
            else
            {
                Projectile.scale = Projectile.ai[1];
            }
            Projectile.ai[2]--;
            if (Projectile.ai[2] <= 0)
            {
                Projectile.velocity = Vector2.Zero;
                NewDustChange2((int)(5 * Projectile.scale / 10), Projectile.Center - new Vector2(4), Vector2.Zero, 27, 0, Projectile.scale / 2, true, 0.3F, 2F, 0);
            }
            else
            {
                NewDustChange2(5, Projectile.Center - new Vector2(4), Vector2.Zero, 27, 0, 0, true, 0.3F, 2F, 0);
            }
            if (Projectile.ai[0] == 0)
            {
                if (Projectile.localAI[0]++ > 180)
                {
                    if (Main.netMode != 1)
                    {
                        Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (player.Center - Projectile.Center).PerfectNormalize() * 12, ModContent.ProjectileType<混沌球>(), Projectile.damage, 0, -1, 0, 0.4F);

                        Projectile.Kill();
                    }
                }
            }
            if (Projectile.ai[0] == 100)
            {
                Main.LocalPlayer.Dplayer().Bossperspective(Projectile.Center, 80, false, 0.1F);
                if (Projectile.localAI[2] == 0)
                {
                    if (Projectile.localAI[0]++ > 380)
                    {
                        if (Main.netMode != 1)
                        {
                            Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                            NewNPCs(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(0, 60), ModContent.NPCType<先祖咒魂>(), 0);
                        }
                        Projectile.localAI[2] = 1;
                        NewDustChange2(150, Projectile.Center - new Vector2(4), Vector2.Zero, 27, 0, 40, true, 0.3F, 10F, 0);
                    }
                    if (Projectile.localAI[0] == 30)
                    {
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<暗影触手基座>(), Projectile.damage, 0, -1, 0, 0, Main.rand.NextFloat(MathHelper.TwoPi));
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<暗影触手基座>(), Projectile.damage, 0, -1, 0, 0, Main.rand.NextFloat(MathHelper.TwoPi));
                    }
                    if (Projectile.localAI[0] == 150)
                    {
                        for (int A = 0; A < 12; A++)
                        {
                            NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), ModContent.ProjectileType<暗影触手>(), Projectile.damage, 0, -1, 0, Main.rand.NextFloat(18, 26), Main.rand.NextFloat(2F, 2.5F));
                        }
                    }
                }
                else
                {
                    Projectile.ai[1] -= 3F;
                    if (Projectile.ai[1] <= 0)
                    {
                        Projectile.Kill();
                    }
                }
                if (Projectile.localAI[1] < 100)
                {
                    Projectile.localAI[1]++;
                }
                else
                    DDWorld.SunColor = new Color(211, 35, 221, 200);
                DDWorld.SunLightScale = 0.75F * (Projectile.localAI[1] / 100) + (1F - Projectile.localAI[1] / 100);
                DDWorld.SunLight = 0.05F * (Projectile.localAI[1] / 100) + (1F - Projectile.localAI[1] / 100);

                Projectile.rotation += 0.1F;
            }
            /*
            if (Projectile.ai[0] == 1)
            {
                if (Projectile.localAI[0]++ > 180)
                {
                    if (Main.netMode != 1)
                    {
                        int D = Main.rand.Next([-1, 1]);
                        for (int A = 0; A < 8; A++)
                        {
                            NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, 2).RotatedBy(MathHelper.TwoPi / 8 * A), ModContent.ProjectileType<混沌球>(), 30, 0, -1, D, 0.4F);
                        }
                        Projectile.Kill();
                    }
                }
            }*/
        }
        public override bool? CanDamage()
        {
                return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.Circle[9].Value;
            if (Projectile.ai[0] == 100)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(81, 81, 81, 255), Projectile.rotation, texture.Size() / 2, Projectile.scale/50, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/50, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/50, 0, 0f);
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 0)
            {
                NewDustChange(40, Projectile.Center - new Vector2(4), Vector2.Zero, 27, 0, Projectile.scale/2, true, 2, 0);
            }
        }
    }
    public class 术士召唤术2 : 术士召唤术
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.scale = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1800;
            Projectile.aiStyle = -1;
        }
    }
    public class 混沌球 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.scale = 1F;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 900;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Projectile.ProjScaleChange();
            Projectile.scale = Projectile.ai[1];
            int A = NewDust(Projectile.Center - new Vector2(4), 0, 0, 27, 0, 0, 100, default, Main.rand.NextFloat(0.3F, 2F));
            Main.dust[A].velocity = -Projectile.velocity / 10;
            Main.dust[A].noGravity = true;
            if (Math.Abs(Projectile.ai[0]) == 1)
            {
                Projectile.extraUpdates = 0;
                Projectile.ai[2]++;
                Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0] / 15) * 1.005F;
                if (Projectile.velocity.Length() > 16)
                {
                    Projectile.velocity = Projectile.velocity.PerfectNormalize() * 16;
                }
                if (Projectile.ai[2] > 150)
                {
                    if (Main.netMode != 1)
                    {
                        NewNPCs(Projectile.GetSource_FromAI(), Projectile.Center, 472, 0);
                    }
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[0] == 2)
            {
                Projectile.velocity *= 0.96F;
                if (Projectile.velocity.Length() < 0.1F)
                {
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[0] == 3)
            {
                if (Projectile.velocity.Length() < 16)
                    Projectile.velocity *= 1.02F;
            }
            if (Math.Abs(Projectile.ai[0]) == 4)
            {
                Projectile.extraUpdates = 0;
                Projectile.ai[2]++;
                Projectile.velocity = Projectile.velocity.RotatedBy(0.0666F) * 1.005F;
                if (Projectile.velocity.Length() > 16)
                {
                    Projectile.velocity = Projectile.velocity.PerfectNormalize() * 16;
                }
                if (Projectile.ai[2] > 150)
                {
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[0] == 5)
            {
                if (Projectile.ai[2]++ < 120)
                {
                    Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                    Vector2 vector = player.Center - Projectile.Center;
                    Projectile.velocity = (Projectile.velocity * 50 + vector.PerfectNormalize()*12) / 51;
                }
                else
                {
                    if(Projectile.velocity.Length()<12)
                    {
                        Projectile.velocity *= 1.02F;
                    }
                }
            }
            if (Projectile.ai[0] == 6)
            {
                if (Projectile.ai[2]++ < 120)
                {
                    Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                    Vector2 vector = player.Center - Projectile.Center;
                    Projectile.velocity = (Projectile.velocity * 80 + vector.PerfectNormalize()*8) / 81;
                }
                else
                {
                    if (Projectile.ai[2] >= 240)
                    {
                        Projectile.Kill();

                    }
                        if (Projectile.velocity.Length()<8)
                    {
                        Projectile.velocity *= 1.02F;
                    }
                }
            }
            if (Projectile.ai[0] == 7)
            {
                if (Projectile.ai[2]++ >= 10)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(81, 6, 233, 0);
            Texture2D texture = DDTextures.VoidStar.Value;
            Vector2 vector = Projectile.Size / 2;
            int L = Projectile.oldPos.Length;
            for (int i = 0; i < L; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color Trailcolor = color * ((L - i) / (float)L / 2f);
                Main.spriteBatch.Draw(texture, vector2, null, Trailcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((L - i) / (float)L), 0, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, Trailcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((L - i) / (float)L), 0, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(132, 166, 21, 0) * 0.75F, Projectile.rotation, texture.Size() / 2, Projectile.scale/2, 0, 0f);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White*0.4F, 0, Vector2.Zero, Projectile.Size/2, 0, 0f);



            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 30; a++)
            {
                int A = NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0, 0, 100, default, Main.rand.NextFloat(0.3F, 2F));
                Main.dust[A].velocity = (Main.dust[A].position - new Vector2(4) - Projectile.Center) / 5;
                Main.dust[A].customData = 1F;
                Main.dust[A].noGravity = true;
            }
            if (Projectile.ai[0] == 2)
            {
                if (Main.netMode != 1)
                {
                    int R = Main.rand.Next(3, 6);
                    float W = Main.rand.NextFloat(MathHelper.TwoPi / R);
                    for (int A=0;A<R;A++)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center, Vector2.One.RotatedBy(MathHelper.TwoPi / R * A+ W)  * 2, ModContent.ProjectileType<混沌球>(), 30, 0, -1, 3, 0.4f);
                }

            }
        }
    }
}