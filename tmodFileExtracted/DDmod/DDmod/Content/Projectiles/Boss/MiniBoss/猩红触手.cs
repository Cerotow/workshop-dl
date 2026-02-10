using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.先祖咒魂;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Magic.Staff;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class 猩红触手 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1200;
            Projectile.extraUpdates = 3;
            Projectile.penetrate = -1;
            Projectile.scale = 1.5F;
            Projectile.localAI[0] = -1000;
        }
        private int[] Body;
        private Vector2[] Center;
        private Vector2[] Velocity;
        internal Color ColorFunction(float completionRatio)
        {
            // 定义颜色数组
            Color[] colorArray = [
    new Color(21, 0, 0, 200),
    new Color(51, 8, 0, 150),
    new Color(91, 19, 11, 100),
    new Color(253, 152, 0, 50),
];

            // 根据completionRatio选择颜色
            float segment = 1f / (colorArray.Length - 1);
            int index = (int)(completionRatio / segment);
            float lerpFactor = (completionRatio % segment) / segment;

            Color drawColor = Color.Lerp(colorArray[index], colorArray[index + 1], lerpFactor);
            return drawColor;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 60f, widthRatio);
        }
        internal static Trailing TrailDrawer;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Body == null)
            {
                return false;
            }
            Texture2D texture = DDTextures.VoidStar.Value;
            Color color = new Color(81, 6, 233, 255);
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾2"]);
            }
            GameShaders.Misc["贴图拖尾2"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾2"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            Vector2 vector = Projectile.Size / 2;

            Vector2[] v = new Vector2[(int)Projectile.DProj().Times[1]];
            for (int B = 0; B < v.Length; B++)
            {
                v[B] = Projectile.Center + Center[B].RotatedBy(Projectile.localAI[1]);
            }
            TrailDrawer.Draw(v, -Main.screenPosition,80, null, Projectile.scale,1.5F);
            /*
            for (int B = 0; B < v.Length; B++)
            {
                Rectangle[] vectors = new Rectangle[Body.Length];
                v[B] = Projectile.Center + Center[B];
                vector = Projectile.Size * (1.3f - (B) / (float)Body.Length);
                Vector2 Size = vector / 2;
                vectors[B] = new Rectangle((int)(v[B].X - Size.X), (int)(v[B].Y - Size.Y), (int)vector.X, (int)vector.Y);
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, v[B] - Size - Main.screenPosition, null, Color.White * 0.75F, 0, Vector2.Zero, vector / 2, 0, 0);
            }*/
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(30, 300);
            target.AddBuff(69, 300);
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == -1000)
            {
                Projectile.DProj().Bool[0] = Projectile.velocity.X >= 0;
                Projectile.localAI[0] = Projectile.velocity.ToRotation();
                if (Projectile.ai[2] == 0)
                {
                    Projectile.ai[2] = 1.5F;
                }
            }
            Projectile.scale = Projectile.ai[2];
            Projectile.alpha = 0;
            for (int TT = 0; TT < 3; TT++)
            {
                if (Projectile.timeLeft < 100)
                {
                    if (Projectile.DProj().Times[1] > 0)
                    {
                        if (Projectile.DProj().Bool[3])
                        {
                            Projectile.DProj().Times[1] -= 0.05F * Projectile.scale;
                        }
                        else
                        {
                            Projectile.DProj().Times[1] -= 0.2F * Projectile.scale;
                        }
                        if (Projectile.DProj().Times[1] < 0)
                        {
                            Projectile.DProj().Times[1] = 0;
                        }
                    }
                }
                if (Projectile.timeLeft < 10)
                {
                    Projectile.DProj().Bool[4] = true;
                }
                if (Projectile.DProj().Bool[4])
                {
                    Projectile.timeLeft = 2;
                    Projectile.position = Projectile.oldPosition;
                    if (Projectile.DProj().Times[1] < 1F)
                    {
                        Projectile.Kill();
                        return;
                    }
                }
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                if ((Projectile.DProj().Bool[0] && Projectile.velocity.X > 0) || (!Projectile.DProj().Bool[0] && Projectile.velocity.X < 0))
                {
                    float R = 1.5f;

                    // 根据当前位置计算速度系数（中间快，边界慢）
                    float normalizedPos = Math.Abs(Projectile.DProj().Times[0]) / R; // 0到1，0=中心，1=边界
                    float speedMultiplier = 1f - (float)Math.Pow(normalizedPos, 1); // 二次曲线减速

                    float currentSpeed = 0.02F * MathHelper.Clamp(speedMultiplier, 0.2f, 1f);
                    bool A = DDHelper.BackAndForth(-R, R, currentSpeed, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0], false);
                }
                else
                {
                    float R = 1.5f;
                    float normalizedPos = Math.Abs(Projectile.DProj().Times[0]) / R;
                    float speedMultiplier = 1f - (float)Math.Pow(normalizedPos, 1);
                    float currentSpeed = 0.02F * MathHelper.Clamp(speedMultiplier, 0.2f, 1f);
                    bool A = DDHelper.BackAndForth(-R, R, currentSpeed, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0], false);
                }
                Projectile.velocity = Projectile.localAI[0].ToRotationVector2().RotatedBy(Projectile.DProj().Times[0]);

                if (Projectile.timeLeft < 10)
                {
                    Projectile.DProj().Bool[4] = true;
                }
                else
                {

                    //长度
                    int Length = (int)Projectile.ai[1];
                    if (Body == null || Body.Length != Length)
                    {
                        Body = new int[Length];
                        Center = new Vector2[Length];
                        Velocity = new Vector2[Length];
                    }

                    Center[0] = Vector2.Zero;
                    Velocity[0] = Projectile.velocity; 
                    for (int B = 0; B < Body.Length; B++)
                    {
                        if (B > 0)
                        {
                            Vector2 vector = Center[B - 1] - Center[B];

                            // 恢复原来的惯性速度计算
                            Velocity[B] = Velocity[B - 1] + Velocity[B - 1].RotatedBy(Projectile.DProj().Times[0]) / ((1F - B / Body.Length) * 40);

                            float Distance;
                            if (vector.Length() == 0)
                            {
                                Distance = 0;
                            }
                            else
                            {
                                Distance = (vector.Length() - (24 * Projectile.scale)) / vector.Length();
                            }

                            // 恢复原来的惯性位移
                            Center[B] += vector * Distance + Velocity[B - 1].PerfectNormalize() * Length / 12 * Projectile.scale;
                        }
                    }
                    if (Projectile.DProj().Times[1] < Body.Length)
                    {
                        Projectile.DProj().Times[1] += 0.04F * Projectile.scale;
                    }
                    DDHelper.MaxandMinF(ref Projectile.DProj().Times[1], Body.Length, 0);
                }
                if (Projectile.ai[0] > 0)
                {
                    //Projectile.scale = 1;
                    //Projectile.ProjScaleChange();
                    Projectile.hide = true;
                    NPC npc = Main.npc[(int)Projectile.ai[0] - 1];
                    if (npc.type != ModContent.NPCType<克苏鲁心脏>() || !npc.active)
                    {
                        Projectile.Kill();
                        return;
                    }
                    Projectile.timeLeft = 102;
                    if (npc.oldPosition != Vector2.Zero)
                    {
                        Projectile.Center = npc.Center;
                    }
                    if (!Projectile.DProj().Bool[4] && npc.Dnpc().Bool[4])
                    {
                        Projectile.DProj().Bool[4] = true;
                        Projectile.DProj().Bool[3] = true;
                    }
                    Projectile.localAI[1] = npc.rotation;
                }
                //Projectile.netUpdate = true;
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Body == null)
            {
                return false;
            }
            Vector2[] v = new Vector2[(int)Projectile.DProj().Times[1]];
            for (int B = 0; B < v.Length; B++)
            {
                Rectangle[] vectors = new Rectangle[Body.Length];
                v[B] = Projectile.Center + Center[B];
                Vector2 vector = Projectile.Size * (1.3f - (B) / (float)Body.Length);
                Vector2 Size = vector / 2;
                vectors[B] = new Rectangle((int)(v[B].X - Size.X), (int)(v[B].Y - Size.Y), (int)vector.X, (int)vector.Y);

                if (targetHitbox.Intersects(new Rectangle((int)(v[B].X - Size.X), (int)(v[B].Y - Size.Y), (int)vector.X, (int)vector.Y)))
                {
                    return true;
                }
            }
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.ai[0] > 0)
                behindNPCs.Add(Projectile.whoAmI);
        }
    }
}