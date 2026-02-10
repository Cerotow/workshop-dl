using Terraria.Graphics.Effects;
using static Terraria.GameContent.Animations.Actions.Sprites;

namespace DDmod.Worlds
{
    public class 杂物背景 : ModSceneEffect
    {
        public override SceneEffectPriority Priority
        {
            get
            {
                return SceneEffectPriority.Event;
            }
        }
        public override bool IsSceneEffectActive(Player player)
        {
            return true;
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            SkyDowned.Sky("杂物Sky", isActive);
        }
    }
    public class 杂物Sky : CustomSky
    {
        public override void OnLoad()
        {
        }
        public int frame;
        public float frameCounter;
        public override void Update(GameTime gameTime)
        {
            //更新弹幕
            for (int k = 0; k < SkyDowned.Proj.Length; k++)
            {
                if (SkyDowned.Proj[k].active && !Main.gamePaused)
                {
                    SkyDowned.Proj[k].UpdateProj(k);
                }
            }
            if (NPC.downedBoss2)
            {
                if (Main.rand.NextBool(1000))
                {
                    int proj = 0;
                    for (int k = 0; k < SkyDowned.Proj.Length; k++)
                    {
                        if (SkyDowned.Proj[k].type == 2 && SkyDowned.Proj[k].active)
                        {
                            proj++;
                        }
                    }
                    if (proj == 0)
                    {
                        NewDirectionalProj(2, 2, 3, 1, true, true, true, true);
                    }
                }
                if (Main.rand.NextBool(1000))
                {
                    int proj = 0;
                    for (int k = 0; k < SkyDowned.Proj.Length; k++)
                    {
                        if (SkyDowned.Proj[k].type == 3 && SkyDowned.Proj[k].active)
                        {
                            proj++;
                        }
                    }
                    if (proj < 5)
                    {
                        NewDirectionalProj(3, 1, 3, 1, true, true, true, true);
                    }
                }
            }
            if (Main.hardMode && !Main.dayTime && !NPCDowned.鬼牙)
            {
                if (Main.rand.NextBool(1000))
                {
                    int proj = 0;
                    for (int k = 0; k < SkyDowned.Proj.Length; k++)
                    {
                        if (SkyDowned.Proj[k].type == 8 && SkyDowned.Proj[k].active)
                        {
                            proj++;
                        }
                    }
                    if (proj == 0)
                    {
                        NewDirectionalProj(8, 20, 3, 1, true, true, true, true);
                    }
                }
            }
        }
        public static void NewDirectionalProj(int Type, float Speed, int Max, int Min, bool UP, bool Down, bool left, bool right)
        {
            if (Min > Max || Main.gamePaused)
            {
                return;
            }
            int a = Main.rand.Next(Min, Max + 1);
            List<int> B = [];
            if (right)
            {
                B.Add(1);
            }

            if (left)
            {
                B.Add(2);
            }

            if (Down)
            {
                B.Add(3);
            }

            if (UP)
            {
                B.Add(4);
            }

            int r = Main.rand.Next(B);
            float Scale = 0.15F;
            if (a >= 2)
            {
                Scale += 0.05F;
            }
            if (a >= 3)
            {
                Scale += 0.05F;
            }
            if (a >= 4)
            {
                Scale += 0.05F;
            }
            if (a >= 5)
            {
                Scale += 0.05F;
            }
            //左和上起始点
            Vector2 vector = new Vector2(-400);
            //右和下最终点
            Vector2 vector2 = new Vector2(Main.screenWidth+120, Main.screenHeight+120)/ Scale;
            /*
            if (a == 2)
            {
                vector = new Vector2(-400);
                vector2 = new Vector2(10000, 5500);
            }
            if (a == 3)
            {
                vector = new Vector2(-400);
                vector2 = new Vector2(8500, 4500);
            }
            if (a == 4)
            {
                vector = new Vector2(-400);
                vector2 = new Vector2(7000, 4000);
            }
            if (a == 5)
            {
                vector = new Vector2(-400);
                vector2 = new Vector2(6000, 3500);
            }*/

            if (r == 1)
            {
                Vector2 Po = new Vector2(vector2.X, Main.rand.NextFloat(vector.Y, vector2.Y));
                SkyProj.NewProj(Po, new Vector2(-Speed, 0), Type, Speed, a);
            }
            else if (r == 2)
            {
                Vector2 Po = new Vector2(vector.X, Main.rand.NextFloat(vector.Y, vector2.Y));
                SkyProj.NewProj(Po, new Vector2(Speed, 0), Type, Speed, a);
            }
            else if (r == 3)
            {
                Vector2 Po = new Vector2(Main.rand.NextFloat(vector.X, vector2.X), vector2.Y);
                SkyProj.NewProj(Po, new Vector2(0, -Speed), Type, Speed, a);
            }
            else if (r == 4)
            {
                Vector2 Po = new Vector2(Main.rand.NextFloat(vector.X, vector2.X), vector.Y);
                SkyProj.NewProj(Po, new Vector2(0, Speed), Type, Speed, a);
            }
        }
        public static void NewDirectionalProj2(int Type, Vector2 Speed, int Max, int Min, bool UP, bool Down, bool left, bool right)
        {
            if (Min > Max || Main.gamePaused)
            {
                return;
            }
            int a = Main.rand.Next(Min, Max + 1);
            List<int> B = [];
            if (right)
            {
                B.Add(1);
            }

            if (left)
            {
                B.Add(2);
            }

            if (Down)
            {
                B.Add(3);
            }

            if (UP)
            {
                B.Add(4);
            }

            int r = Main.rand.Next(B);
            float Scale = 0.15F;
            if (a >= 2)
            {
                Scale += 0.05F;
            }
            if (a >= 3)
            {
                Scale += 0.05F;
            }
            if (a >= 4)
            {
                Scale += 0.05F;
            }
            if (a >= 5)
            {
                Scale += 0.05F;
            }
            //左和上起始点
            Vector2 vector = new Vector2(-400);
            //右和下最终点
            Vector2 vector2 = new Vector2(Main.screenWidth + 120, Main.screenHeight + 120) / Scale;

            if (r == 1)
            {
                Vector2 Po = new Vector2(vector2.X, Main.rand.NextFloat(vector.Y, vector2.Y));
                SkyProj.NewProj(Po, Speed, Type, Speed.Length(), a);
            }
            else if (r == 2)
            {
                Vector2 Po = new Vector2(vector.X, Main.rand.NextFloat(vector.Y, vector2.Y));
                SkyProj.NewProj(Po, Speed, Type, Speed.Length(), a);
            }
            else if (r == 3)
            {
                Vector2 Po = new Vector2(Main.rand.NextFloat(vector.X, vector2.X), vector2.Y);
                SkyProj.NewProj(Po, Speed, Type, Speed.Length(), a);
            }
            else if (r == 4)
            {
                Vector2 Po = new Vector2(Main.rand.NextFloat(vector.X, vector2.X), vector.Y);
                SkyProj.NewProj(Po, Speed, Type, Speed.Length(), a);
            }
        }

        private Vector2 PO;
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            //背景绘制更新
            for (int k = 0; k < SkyDowned.Proj.Length; k++)
            {
                if (SkyDowned.Proj[k].active)
                {
                    if (SkyDowned.Proj[k].Depth > minDepth && SkyDowned.Proj[k].Depth < maxDepth)
                    {
                        SkyDowned.Proj[k].Dawn(spriteBatch);
                        continue;
                    }
                    /*
                    if (SkyDowned.Proj[k].Layer == 1)
                    {
                        if (min > 5&& maxDepth<5)
                        {
                            SkyDowned.Proj[k].Dawn(spriteBatch);
                        }
                    }
                    if (SkyDowned.Proj[k].Layer == 2)
                    {
                        Main.NewText("Min" + minDepth);
                        Main.NewText("Max" + maxDepth);
                        if (min > 4 && maxDepth < 4)
                        {
                            SkyDowned.Proj[k].Dawn(spriteBatch);
                        }
                    }
                    if (SkyDowned.Proj[k].Layer == 3)
                    {
                        if (min > 3 && maxDepth < 3)
                        {
                            SkyDowned.Proj[k].Dawn(spriteBatch);
                        }
                    }
                    if (SkyDowned.Proj[k].Layer == 4)
                    {
                        if (min > 2 && maxDepth < 2)
                        {
                            SkyDowned.Proj[k].Dawn(spriteBatch);
                        }
                    }
                    if (SkyDowned.Proj[k].Layer == 5)
                    {
                        if (min < 0)
                        {
                            SkyDowned.Proj[k].Dawn(spriteBatch);
                        }
                    }
                    */
                }
            }

            //SkyDowned.BackgroundColor = new Color(0, 0, 0, 255);
            if (SkyDowned.BackgroundColor != new Color(0, 0, 0, 0))
            {
                if (minDepth < 0)
                {
                    AL = 1;
                    Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, SkyDowned.BackgroundColor * AL, 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 0, 0);
                    BackgroundColor = SkyDowned.BackgroundColor;
                    SkyDowned.BackgroundColor = new Color(0, 0, 0, 0);
                }
            }
            else if (AL > 0)
            {
                if (minDepth < 0)
                {
                    AL -= 0.02f;
                    Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, BackgroundColor * AL, 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 0, 0);
                    SkyDowned.BackgroundColor = new Color(0, 0, 0, 0);
                }
            }
            else
            {
                AL = 0;
            }
        }

        private float AL = 0;
        private Color BackgroundColor;

        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }
        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }
        public override void Reset()
        {
            isActive = false;
        }
        public override bool IsActive()
        {
            return isActive;
        }
        private bool isActive;
    }

}
