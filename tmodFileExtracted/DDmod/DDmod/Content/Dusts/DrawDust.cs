using DDmod.NoContent.Config;
using DDmod.Sync;

namespace DDmod.Content.Dusts
{
    public static class DrawDust
    {
        public static bool PreModDrawdust(Dust dust, Color alpha, float scale)
        {
            ModDust modDust = DustLoader.GetDust(dust.type);
            if (dust.type == ModContent.DustType<星星粒子>())
            {
                if (dust.customData == null)
                {
                    Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;

                    Color color = new Color(50, 50, 50, 255) * (1 - dust.alpha / 255F);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                    color = new Color(0, 100, 255, 50) * (1 - dust.alpha / 255F);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                    Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetAlpha(Color.White * 0.75f), dust.rotation, modDust.Texture2D.Size() / 2, scale, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                }
                else
                {
                    Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;

                    Color color = new Color(50, 50, 50, 255) * (1 - dust.alpha / 255F);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                    color = new Color(0, 100, 255, 50) * (1 - dust.alpha / 255F);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                    Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetAlpha(Color.White*0.75f), dust.rotation, modDust.Texture2D.Size() / 2, scale, 0, 0f);
                    Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetAlpha(new Color(100,100,0,0)), dust.rotation, modDust.Texture2D.Size() / 2, scale, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                }
                return false;
            }
            if (dust.type == ModContent.DustType<爱心粒子>())
            {
                int r = dust.alpha;
                if(r<0)
                {
                    r = 0;

                }
                if(r>255)
                {
                    r = 255;

                }
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;

                Color color = new Color(50, 50, 50, 255) * (1 - r / 255F);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
                color = new Color(255, 50, 50, 0) * (1 - r / 255F);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
                Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetAlpha(Color.White), dust.rotation, dust.frame.Size() / 2, scale, 0, 0f);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
                return false;
            }
            if (dust.type == ModContent.DustType<光球粒子>())
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                if(dust.alpha<0)
                {
                    dust.alpha = 0;
                }
                Color color = dust.color* (1-dust.alpha/255F);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                color = dust.color.Opposite() * (1 - dust.alpha / 255F);
                color.A=0;
                if (dust.customData is float&& (float)dust.customData < 0)
                {
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 6, 0, 0f);
                }
                if (dust.customData is int && (int)dust.customData < 0)
                {
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 6, 0, 0f);
                }
                return false;
            }
            if (dust.type == ModContent.DustType<冰雾>())
            {
                Vector2 vector = modDust.Texture2D.Size()/2;
                Color color = Lighting.GetColor((int)dust.position.X/ 16, (int)dust.position.Y/16, dust.color);
                if (dust.noLightEmittence)
                {
                    color = dust.color;
                }
                color *=0.8F;
                int A = dust.alpha;
                if(A<80)
                {
                    A = 80;
                }
                Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, modDust.Texture2D.Width(), modDust.Texture2D.Height())), color*(1-(float)A / 255)*0.5f, dust.rotation, vector, scale / 8, 0, 0f);
                //Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, modDust.Texture2D.Width(), modDust.Texture2D.Height())), color*(1-(float)A / 255)*0.5f, dust.rotation, vector, scale / 8, 0, 0f);

                //Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position+ (vector.RotatedBy(1 + dust.rotation) /20*dust.scale) - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, modDust.Texture2D.Width(), modDust.Texture2D.Height())), color*(1-(float)A / 255)*0.5f, dust.rotation+1, vector, scale / 8, 0, 0f);
               // Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position + (vector.RotatedBy(2.2F+ dust.rotation) / 16 * dust.scale) - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, modDust.Texture2D.Width(), modDust.Texture2D.Height())), color*(1-(float)A / 255)*0.5f, dust.rotation+3, vector, scale / 8, 0, 0f);
               // Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position + (vector.RotatedBy(4.2F+ dust.rotation) / 16 * dust.scale) - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, modDust.Texture2D.Width(), modDust.Texture2D.Height())), color*(1-(float)A / 255)*0.5f, dust.rotation+3, vector, scale / 8, 0, 0f);
                //Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position + (vector.RotatedBy(5.2F + dust.rotation) / 18 * dust.scale) - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, modDust.Texture2D.Width(), modDust.Texture2D.Height())), color*(1-(float)A / 255)*0.5f, dust.rotation+5, vector, scale / 8, 0, 0f);
                return false;
            }
            if (dust.type == ModContent.DustType<激光粒子>())
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = dust.color;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, new Vector2(2, scale) / 1.5f, 0, 0f);
                color = dust.color.Opposite();
                if (dust.customData is float && (float)dust.customData > 0)
                {
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, new Vector2(2, scale) / 5, 0, 0f);
                }
                return false;
            }
            return true;
        }
        public static void ModDrawdust(Dust dust, Color alpha, float scale)
        {
            ModDust modDust = DustLoader.GetDust(dust.type);
            if (dust.type == ModContent.DustType<天堂粒子>())
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height) / 2;
                Color color = new Color(255, 153, 183, 120);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height)), color, dust.rotation, vector, scale, 0, 0f);
                color = new Color(255, 153, 183, 0).Opposite() * 0.1F;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height)), color, dust.rotation, vector, scale, 0, 0f);
            }
            if (dust.type == ModContent.DustType<绿激光粒子>())
            {
                float S = 2;
                if (dust.customData is float)
                {
                    S = (float)dust.customData * 2;
                }
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(96, 248, 2);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, new Vector2(S, scale) / 2, 0, 0f);
                color = new Color(255 - 96, 255 - 248, 255 - 2);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, new Vector2(S, scale) / 5, 0, 0f);
            }
            if (dust.type == ModContent.DustType<紫激光粒子>())
            {
                float S = 2;
                if (dust.customData is float)
                {
                    S = (float)dust.customData*2;
                }
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(66, 49, 255);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, new Vector2(S, scale) / 1.5f, 0, 0f);
                color = new Color(255 - 150, 255 -150, 0);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, new Vector2(S, scale) / 5, 0, 0f);
            }
            if (!ModContent.GetInstance<DDConfigClient>().DustLightEffect)
            {
                return;
            }
            if (dust.type == ModContent.DustType<生命粒子>())
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height) / 2;
                Color color = new Color(0, 188, 0, 0);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height)), color, dust.rotation, vector, scale / 2, 0, 0f);
                color = new Color(255, 68, 255, 0) * 0.1f;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height)), color, dust.rotation, vector, scale / 2, 0, 0f);
            }
            if (dust.type == ModContent.DustType<枯萎粒子>())
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height) / 2;
                Color color = new Color(188, 0, 0, 0);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height)), color, dust.rotation, vector, scale / 2, 0, 0f);
                color = new Color(68, 255, 255, 0) * 0.1f;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height)), color, dust.rotation, vector, scale / 2, 0, 0f);
            }
            if (dust.type == ModContent.DustType<格挡粒子>())
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height) / 2;
                Color color = new Color(255, 250, 112, 0);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height)), color, dust.rotation, vector, scale / 2, 0, 0f);
            }
            if (dust.type == ModContent.DustType<轻语矢粒子>())
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height) / 2;
                Color color = new Color(135, 214, 227, 0);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Value.Width, DDTextures.MiniVoidStar.Value.Height)), color, dust.rotation, vector, scale / 2, 0, 0f);
            }
        }
        public static void Drawdust(Dust dust, Color alpha, float scale)
        {
            if (!ModContent.GetInstance<DDConfigClient>().DustLightEffect)
            {
                return;
            }   
            //蝙蝠粒子
            if (dust.type == 195)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = dust.GetAlpha(new Color(20, 20, 20));
                
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
            }
            //火焰粒子
            if (dust.type == 6)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(253, 62, 3,80)* ((float)(255F - dust.alpha) / 255f);
                
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
            }
            //神圣栗子
            if (dust.type ==  57)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(255, 255, 104);
                color.A = 200;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color.Opposite()*0.25F, dust.rotation, vector, scale / 4, 0, 0f);
            }
            //高温粒子
            if (dust.type == 162)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(212, 131, 11);
                if (dust.frame.Y==50)
                {
                    color = new Color(224, 79, 0);
                }
                else if (dust.frame.Y ==40)
                {
                    color = new Color(254, 246, 37);
                }
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2+0.1f, 0, 0f);
            }
            if (dust.type == 35)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = dust.GetAlpha(new Color(253, 62, 3));
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2, 0, 0f);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2.5f, 0, 0f);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 2.5f, 0, 0f);
            }
            if (dust.type == 135)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = dust.GetAlpha(new Color(0, 186, 242));
                color.A = 255;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
            }
            if ( dust.type == 92)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(0, 136, 242);
                color.A = 100;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 3, 0, 0f);
            }

            if (dust.type == 226)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(0, 186, 242);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
                color = new Color(255, 70, 15);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 8, 0, 0f);
            }
            if (dust.type == 228)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(255, 255, 128);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
            }
            if (dust.type == 172)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(0, 55, 255);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
            }
            //暗影束
            if (dust.type == 173)
            {
                for (int a = 0; a < 3; a++)
                {
                    Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                    Color color = new Color(106, 0, 198);
                    color.A = 0;
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color * scale, dust.rotation, vector, scale/4+0.2F, 0, 0f);
                    color = new Color(255 - 106, 255, 255 - 198)*0.5F;
                    color.A = 0;
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color * scale, dust.rotation, vector, scale/8+0.1F, 0, 0f);
                }
            }
            //咒火粒子
            if (dust.type == 75)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(96, 248, 2);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
            }
            //暗影焰
            if (dust.type == 27)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(81, 6, 233);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
            }
            //灵液
            if (dust.type == 170)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(253, 152, 0);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
            }
            if (dust.type == 160)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(2, 254, 201);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale, 0, 0f);
                Color color2 = new Color(253, 1, 54);
                color2.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color2, dust.rotation, vector, scale / 1.5f, 0, 0f);
            }
            if (dust.type == 70)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(132, 64, 205);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
            }
            //咒火粒子
            if (dust.type == 74)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(96, 248, 2);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);
            }
            //利刃台风水粒子
            if (dust.type == 217)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = Lighting.GetColor((int)(dust.position.X / 16), (int)(dust.position.Y / 16), new Color(9, 173, 191) * 0.5f);
                color = new Color(9, 173, 191);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), new Color(50,50,50,255), dust.rotation, vector, scale / 4, 0, 0f);
                color.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale / 4, 0, 0f);

            }
            //月总粒子
            if (dust.type == 229)
            {
                Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
                Color color = new Color(20, 233, 201);
                color.A = 0;
                Color color2 = new Color(225, 22, 54)*0.8f;
                color2.A = 0;
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, dust.rotation, vector, scale/3, 0, 0f);
                Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color2, dust.rotation, vector, scale/6, 0, 0f);

            }
        }
    }
}