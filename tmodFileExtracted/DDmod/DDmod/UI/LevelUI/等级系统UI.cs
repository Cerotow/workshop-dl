
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.UI;

namespace DDmod.UI.LevelUI
{
    public enum T2DID : short
    {
        初始,
        生命,
        攻击,
        近战攻击,
        远程攻击,
        魔法攻击,
        召唤攻击,
    }
    public class 加成树
    {
        public static List<Asset<Texture2D>> BonusTexture;
        public short ID = 0;
        public Vector2 Position;
        public bool Lock = true;
        /// <summary>
        /// 0第一个技能,1,战士,2射手,3法师,4召唤,5散人
        /// </summary>
        public short ProfessionID = 0;
        /// <summary>
        /// 应该链接的技能
        /// </summary>
        public short[] LevelID;
        /// <summary>
        /// 技能效果
        /// </summary>
        public 加成树(Vector2 vector, T2DID ID, short Profession, params short[] Level)
        {
            Load();
            Position = vector;
            this.ID = ((short)ID);
            ProfessionID = ((short)Profession);
            LevelID = Level;
        }
        public void Load()
        {
            BonusTexture = new List<Asset<Texture2D>>();
            for (int A = 0; A < 2; A++)
            {
                BonusTexture.Add(ModContent.Request<Texture2D>("DDmod/UI/LevelUI/Bonus_" + A));
            }
        }

        public void Update(GameTime gameTime)
        {
            Texture2D texture = BonusTexture[ID].Value;
            Vector2 vector = new Vector2(Main.screenWidth, Main.screenHeight) / 2;
            if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(Position.X - 等级系统UI.Position.X + vector.X), (int)(Position.Y - 等级系统UI.Position.Y + vector.Y), texture.Width, texture.Height)) && 等级系统UI.OldMouse == Vector2.Zero)
            {
                等级系统UI.An = true;
                for (int A = 0; A < LevelID.Length; A++)
                {
                    if (LevelID[A] >= 0&& 等级系统UI.OldMouse==Vector2.Zero)
                    {
                        加成树 Bonus = DDUISystem.Instance.LevelUI.Bonus[LevelID[A]];
                        if (Bonus.Lock == false&&Main.mouseLeft)
                        {
                            Lock = false;
                            break;
                        }
                    }
                }
            }
        }
        public void DrawLink(SpriteBatch spriteBatch)
        {
            Texture2D texture = BonusTexture[ID].Value;
            Vector2 vector = new Vector2(Main.screenWidth, Main.screenHeight) / 2;
            Vector2 Po = Position + texture.Size() / 2 - 等级系统UI.Position + vector;

            for (int A = 0; A < LevelID.Length; A++)
            {
                if (LevelID[A] >= 0)
                {
                    加成树 Bonus = DDUISystem.Instance.LevelUI.Bonus[LevelID[A]];
                    Color color = Color.White;
                    vector = Bonus.Position + (BonusTexture[Bonus.ID].Size() - BonusTexture[ID].Size()) / 2 - Position;
                    texture = DDTextures.WhitePng.Value;
                    Vector2[] vectors = [Po, Po + vector / 5, Po + vector / 5 * 2, Po + vector / 5 * 3, Po + vector / 5 * 4, Po + vector, Po + vector];
                    if (Lock || Bonus.Lock)
                    {
                        color *= 0.3F;
                        color.A = 255;
                        GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.FireEffect2);
                        GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(0f);
                        DDUISystem.Instance.LevelUI.TrailDrawer2.Draw(vectors, Vector2.Zero, 24, null, 1, 0.3F);
                    }
                    else
                    {
                        GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.FireEffect2);
                        GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(-0.6f);
                        DDUISystem.Instance.LevelUI.TrailDrawer.Draw(vectors, Vector2.Zero, 24, null, 1, 1F);
                    }
                    //spriteBatch.Draw(texture, Po, null, color, vector.ToRotation(), new Vector2(0, texture.Height / 2), new Vector2(vector.Length() / 2, 2), 0, 0);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = BonusTexture[ID].Value;
            Vector2 vector = new Vector2(Main.screenWidth, Main.screenHeight) / 2;
            if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(Position.X - 等级系统UI.Position.X + vector.X), (int)(Position.Y - 等级系统UI.Position.Y + vector.Y), texture.Width, texture.Height)) && 等级系统UI.OldMouse == Vector2.Zero)
            {
                if (ID == 0)
                {
                    UICommon.TooltipMouseText("这是你的起点,获得的属性点可以从此处开始衍生");
                }
                if (ID == 1)
                {
                    UICommon.TooltipMouseText("消耗1点属性点激活\n生命上限增加5点");
                }
            }
            Color color = Color.White;
            if (Lock)
            {
                color *= 0.3F;
                color.A = 255;
            }
            Vector2 Po = Position + texture.Size() / 2 - 等级系统UI.Position + vector;
            spriteBatch.Draw(texture, Po, null, color, 0, texture.Size() / 2, 1, 0, 0);
        }
    }
    public class 等级系统UI
    {
        public static bool Visible;
        /// <summary>
        /// 鼠标在技能位置
        /// </summary>
        public static bool An;
        public static Asset<Texture2D> texture;
        public static Vector2 Position = new Vector2(30, 30);
        public List<加成树> Bonus;
        public static Vector2 OldMouse = Vector2.Zero;
        public static Vector2 OldPosition = Vector2.Zero;
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(241, 255, 77, 0),
                new Color(241, 255, 77, 0),
                new Color(255, 144, 0, 0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(255, 144, 0, 0), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal Color ColorFunction2(float completionRatio)
        {
            return new Color(100, 100, 100, 0);
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                10,
                20,
                30,
            }) * MathHelper.Lerp(0f, 1.4f, widthRatio), 30, (float)Math.Pow((double)completionRatio, 1.0));
        }
        public Trailing TrailDrawer;
        public Trailing TrailDrawer2;
        public 等级系统UI()
        {
            Load();
        }

        public void Load()
        {
            texture = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/等级系统UI");


            Bonus = new List<加成树>();
            short S = 0;
            Vector2 Starting = new Vector2(-10, -10);
            Bonus.Add(new 加成树(Starting, T2DID.初始, 0, -1));
            Starting = Vector2.Zero;
            Bonus[0].Lock = false;
            Vector2 Distance = new Vector2(0, -80);
            Vector2 Branch = Vector2.Zero;
            ///加成树1
            Vector2 vector = Starting;
            {
                vector += Distance;
                Bonus.Add(new 加成树(vector, T2DID.生命, 0, 0));
                S = (short)Bonus.Count;
                S--;
                Branch = vector + Distance.RotatedBy(-0.5F);
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                Branch = vector + Distance.RotatedBy(0.5F);
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                vector += Distance;
                S = (short)Bonus.Count;
                S--;
                vector += Distance;
                Bonus.Add(new 加成树(vector, T2DID.生命, 0, S--, S));
            }
            vector = Starting;
            Distance = new Vector2(0, -80).RotatedBy(MathHelper.TwoPi / 5);
            ///加成树2
            {
                vector += Distance;
                Bonus.Add(new 加成树(vector, T2DID.生命, 0, 0));
                S = (short)Bonus.Count;
                S--;

                Branch = vector + Distance.RotatedBy(-0.75F);
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                Branch = vector + Distance;
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                Branch = vector + Distance.RotatedBy(0.75F);
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                vector += Distance;
                S = (short)Bonus.Count;
                S--;
                vector += Distance;
                Bonus.Add(new 加成树(vector, T2DID.生命, 0, S--, S--, S));
            }
            vector = Starting;
            Distance = new Vector2(0, -80).RotatedBy(MathHelper.TwoPi / 5 * 2);
            ///加成树2
            {
                vector += Distance;
                Bonus.Add(new 加成树(vector, T2DID.生命, 0, 0));
                S = (short)Bonus.Count;
                S--;

                Branch = vector + Distance.RotatedBy(-0.75F);
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                Branch = vector + Distance;
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                Branch = vector + Distance.RotatedBy(0.75F);
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                vector += Distance;
                S = (short)Bonus.Count;
                S--;
                vector += Distance;
                Bonus.Add(new 加成树(vector, T2DID.生命, 0, S--, S--, S));
            }
            vector = Starting;
            Distance = new Vector2(0, -80).RotatedBy(MathHelper.TwoPi / 5 * 3);
            ///加成树2
            {
                vector += Distance;
                Bonus.Add(new 加成树(vector, T2DID.生命, 0, 0));
                S = (short)Bonus.Count;
                S--;

                Branch = vector + Distance.RotatedBy(-0.75F);
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                Branch = vector + Distance;
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                Branch = vector + Distance.RotatedBy(0.75F);
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                vector += Distance;
                S = (short)Bonus.Count;
                S--;
                vector += Distance;
                Bonus.Add(new 加成树(vector, T2DID.生命, 0, S--, S--, S));
            }
            vector = Starting;
            Distance = new Vector2(0, -80).RotatedBy(MathHelper.TwoPi / 5 * 4);
            ///加成树2
            {
                vector += Distance;
                Bonus.Add(new 加成树(vector, T2DID.生命, 0, 0));
                S = (short)Bonus.Count;
                S--;

                Branch = vector + Distance.RotatedBy(-0.75F);
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                Branch = vector + Distance;
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                Branch = vector + Distance.RotatedBy(0.75F);
                Bonus.Add(new 加成树(Branch, T2DID.生命, 0, S));
                vector += Distance;
                S = (short)Bonus.Count;
                S--;
                vector += Distance;
                Bonus.Add(new 加成树(vector, T2DID.生命, 0, S--, S--, S));
            }
        }
        
        float UIsc = 0;
        public void Update(GameTime gameTime)
        {
            Visible = false;
            if (Visible)
            {
                if (UIsc < 1)
                {
                    UIsc += 0.1F;
                }
                else
                {
                    UIsc = 1;
                }
            }
            else if (UIsc > 0)
            {
                UIsc -= 0.1f;
            }
            if (UIsc <= 0)
            {
                return;
            }
            An = false;
            for (int A = 0; A < Bonus.Count; A++)
            {
                Bonus[A].Update(gameTime);
            }
            if (!Main.mouseLeft)
            {
                OldMouse = Vector2.Zero;
                OldPosition = Position;
            }
            if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle(Main.screenWidth / 2 - 400, Main.screenHeight / 2 - 250, 800, 500)) || OldMouse != Vector2.Zero)
            {
                if (!An && Main.mouseLeft)
                {
                    if (OldMouse == Vector2.Zero)
                    {
                        OldMouse = new(Main.mouseX, Main.mouseY);
                    }
                    Position = OldPosition + (OldMouse - new Vector2(Main.mouseX, Main.mouseY));
                }
                Main.LocalPlayer.mouseInterface = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(!Visible)
            {
                return;
            }

            spriteBatch.Draw(texture.Value, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, Color.White * 0.75F, 0, new Vector2(texture.Width() / 2, texture.Height() / 2), UIsc, 0, 0);
            spriteBatch.DrawrectBegin(new Rectangle(Main.screenWidth / 2 - 386, Main.screenHeight / 2 - 236, 774, 474), BlendState.AlphaBlend, Main.UIScaleMatrix, out SamplerState anisotropicClamp, out RasterizerState rasterizerState, out Rectangle scissorRectangle);

            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"],1);
            }
            if (TrailDrawer2 == null)
            {
                TrailDrawer2 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction2), null, GameShaders.Misc["贴图拖尾"],1);
            }
            for (int A=0;A< Bonus.Count;A++)
            {
                Bonus[A].DrawLink(spriteBatch);
            }
            for(int A=0;A< Bonus.Count;A++)
            {
                Bonus[A].Draw(spriteBatch);
            }
            spriteBatch.DrawrectEnd(BlendState.AlphaBlend, Main.UIScaleMatrix, anisotropicClamp, rasterizerState, scissorRectangle);
        }
    }
}