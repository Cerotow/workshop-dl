using DDmod.Content.Dusts;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 魔法围巾 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 5;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.scale = 1f;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public Vector2[] oldPos = new Vector2[12];
        public Vector2[] velocities = new Vector2[12];
        private float[] segmentLengths = new float[11];
        private float swingTimer; // 摆动计时器
        private Vector2 lastPlayerVelocity;
        private bool initialized;

        public override void AI()
        {
            Projectile.timeLeft = 5;
            Projectile.Center = Projectile.Player().Center+(Projectile.Player().position- Projectile.Player().oldPosition)- new Vector2(0, 6).RotatedBy(-Projectile.Player().fullRotation);
            oldPos[0] = Projectile.position;
            float S = (Projectile.Player().velocity.X - Main.windSpeedCurrent * 3000);
            S = Math.Abs(S);
            Vector2 v2 = oldPos[oldPos.Length - 1] - oldPos[0];

            // 保留你的摆动系统
            DDHelper.BackAndForth(-S / 2, S / 2, S / 10, ref Projectile.ai[0], ref Projectile.DProj().Bool[0]);
            Projectile.ai[1] = 1;

            Projectile.ai[2]++;
            Color color = new Color(255, 30, 30, 50) * 0.3f;
            if (Projectile.ai[2] / 600 < 1)
            {
                color = new Color(255, 30, 30, 50) * 0.3f;
            }
            else if (Projectile.ai[2] / 600 < 2)
            {
                color = new Color(30, 255, 30, 50) * 0.3f;
            }
            else if (Projectile.ai[2] / 600 < 3)
            {
                color = new Color(255, 175, 30, 50) * 0.3f;
            }
            else if (Projectile.ai[2] / 600 < 4)
            {
                color = new Color(30, 30, 255, 50) * 0.3f;
            }
            else
            {
                Projectile.ai[2] = 0;
            }
            for (int i = oldPos.Length - 1; i > 0; i--)
            {
                Vector2 vector = oldPos[i] - oldPos[i - 1];

                // 先计算目标偏移量，但不直接应用
                Vector2 targetOffset = new Vector2(
                    Main.windSpeedCurrent * 3000 * 0.03f ,
                    Projectile.ai[0] * S / 20 * 0.3f + (float)Math.Sin(Projectile.Player().Dplayer().PlayerTimes * 0.2f + i) * 0.8f
                ) * Projectile.ai[1];

                targetOffset.X -= 1F * Projectile.Player().direction;
                targetOffset.Y += 80 / (v2.Length() + 1);

                // 限制单帧最大位移
                float maxFrameMovement = 8f; // 每帧最大移动距离
                if (targetOffset.Length() > maxFrameMovement)
                {
                    targetOffset = Vector2.Normalize(targetOffset) * maxFrameMovement;
                }

                // 计算新位置
                Vector2 newPos = oldPos[i - 1] + targetOffset;

                // 最终长度约束（更严格）
                Vector2 finalSegment = newPos - oldPos[i - 1];
                float maxSegmentLength = 10f;
                if (finalSegment.Length() > maxSegmentLength)
                {
                    finalSegment = Vector2.Normalize(finalSegment) * maxSegmentLength;
                    newPos = oldPos[i - 1] + finalSegment;
                }

                oldPos[i] = newPos;
            }
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾3"]);
            }

            GameShaders.Misc["贴图拖尾3"].SetShaderTexture(TextureAssets.Projectile[Projectile.type]);
            //GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(0);
            //for(int A=0;A<30;A++)
            TrailDrawer.Draw(oldPos, Projectile.Size * 0.5f - Main.screenPosition , 20, null,1,1);
            return false;
        }
        internal static Trailing TrailDrawer;
        internal Color ColorFunction(float completionRatio)
        {
            return Color.White;
        }
        internal float WidthFunction(float completionRatio)
        {
            return 15;
        }
    }
}