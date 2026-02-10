using DDmod.Content.Items.Melee.Sword;
using Terraria;

namespace DDmod.Content.Dusts
{
    /// <summary>
    /// Vector4
    /// 1.变大速度
    /// 2.透明速度
    /// 3.变大延迟
    /// 4.乘颜色
    /// </summary>
    public class 光圈粒子 : ModDust
    {
        public override string Texture => "DDmod/Image/Round2";
        public override void OnSpawn(Dust dust)
        {
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            dust.noLightEmittence = false;
        }

        public override bool Update(Dust dust)
        {
            
            if (dust.customData == null)
            {
                dust.customData = new Vector4(1, 1, 1,1);
            }
            if (dust.customData is int)
            {
                float A = (int)dust.customData;
                dust.customData = new object();
                dust.customData = new Vector4(A, A, 0, 1);
            }
            if (dust.customData is float)
            {
                float A = (float)dust.customData;
                dust.customData = new object();
                dust.customData = new Vector4(A, A, 0, 1);
            }
            if (dust.customData is Vector3)
            {
                Vector3 vector = (Vector3)dust.customData;
                dust.customData = new object();
                dust.customData = new Vector4(vector, 1);
            }
            if (dust.customData is Vector4)
            {
                Vector4 vector = (Vector4)dust.customData;
                if (vector.Z <= 0)
                {
                    dust.scale += 0.1F * vector.X;
                    dust.scale += 0.2F * vector.X;
                    if (dust.alpha >= 0)
                    {
                        dust.alpha += (int)(1 * vector.Y);
                    }
                    else
                    {
                        dust.alpha++;
                    }
                    if (dust.alpha > 255)
                    {
                        dust.active = false;
                    }
                }
                else
                {
                    vector.Z--;
                    dust.customData = vector;
                }
            }
            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            Texture2D Round = DDTextures.Round3.Value;
            Vector2 origin = Round.Size() / 2;
            float scale = dust.scale;
            float A = (1F - dust.alpha / 255F);
            if (A < 0|| dust.alpha<0)
            {
                A = 0;
            }
            if (A > 1)
            {
                A = 1;
            }
            Vector4 vector = (Vector4)dust.customData;
            Main.spriteBatch.Draw(Round, dust.position - Main.screenPosition, null, dust.color*A* vector.W, dust.rotation, origin, scale, 0, 0);

            /*
            Texture2D Perlin = DDTextures.Perlin2.Value;
            Vector2 origin = Perlin.Size() / 2;
            float scale = dust.scale;
            float A = (1F - dust.alpha / 255F);
            if (A<0)
            {
                A = 0;
            }
            if (A>1)
            {
                A = 1;
            }
            dust.color.A = 0;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            DDHelper.AnnularShaders(A*12, dust.color, Main.LocalPlayer.Dplayer().Times/10);
            Main.spriteBatch.Draw(Perlin, dust.position - Main.screenPosition, null, dust.color, dust.rotation, origin, scale, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            */
            return false;
        }
    }
}