using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.DDOn;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Filters = Terraria.Graphics.Effects.Filters;

namespace DDmod.Worlds
{
    public partial class SkyProj
    {
        public Vector2 velocity;
        public Vector2 position;
        public int width;
        public int height;
        public int type;
        public int Layer;
        //初始大小
        public float Scale = 1;
        //额外大小
        public float EScale = 1;
        public float rotating;
        public float Speed;
        public float[] ai = new float[3];
        public bool[] Bool = new bool[3];
        public Vector2[] vectors = new Vector2[3];
        public int aiStyle = 0;
        public NPC npc;
        /// <summary>
        /// 是不是敌对
        /// </summary>
        public bool Hostile;
        public bool active;
        public float Depth;
        public Vector2 Centre => position + new Vector2(width, height) / 2;
        public Rectangle Rectangle => new Rectangle((int)position.X, (int)position.Y, width, height);
        public int[] ImmuneFrames = new int[200];
        public float Ro = 0;
        public void UpdateProj(int Proj)
        {
            /*
            if (position.Y < -400 || position.Y > Main.screenHeight + 400)
            {
                active = false;
            }
            if (position.X < -400 || position.X > Main.screenWidth + 400)
            {
                active = false;
            }*/
            if (!active)
            {
                return;
            }
            /*
            if (Main.rand.NextBool(200))
            {
                active = false;
            }*/
            if (Math.Abs(position.X-(Main.screenPosition.X)-2000) *Scale > Main.screenWidth*3)
            {
                active = false;
            }
            if (Math.Abs(position.Y - (Main.screenPosition.Y)-2000) *Scale > Main.screenHeight*3)
            {
                active = false;
            }
            if (aiStyle == 1)
            {
                //position.X -= Main.LocalPlayer.velocity.X / 10 * Layer;
                //position.Y -= Main.LocalPlayer.velocity.Y / 10F * Layer;

                velocity = (rotating - MathHelper.PiOver2).ToRotationVector2() * 2 * Layer;
                DDHelper.RotateSpeed(ref rotating, Ro, 0.002F * 2 + 0.02F);
                if (Main.rand.NextBool(300))
                {
                    Ro = Main.rand.NextFloat(0, MathHelper.TwoPi);
                }
            }
            if (aiStyle == 2)
            {
                //position.X -= Main.LocalPlayer.velocity.X / 10 * Layer;
                //position.Y -= Main.LocalPlayer.velocity.Y / 10F * Layer;

                if (Main.rand.NextBool(300))
                {
                    Ro = Main.rand.NextFloat(-2, 2);
                    velocity.X = Ro;
                }
            }
            if (aiStyle == 3)
            {
                for (int B = 0; B < Body.Length; B++)
                {
                    //Centers[B].X -= Main.LocalPlayer.velocity.X / 10 * Layer;
                    //Centers[B].Y -= Main.LocalPlayer.velocity.Y / 10 * Layer;
                }
                //position.X -= Main.LocalPlayer.velocity.X / 10 * Layer;
                //position.Y -= Main.LocalPlayer.velocity.Y / 10F * Layer;
                rotating = velocity.ToRotation()+MathHelper.PiOver2;
                if (vectors[0] == Vector2.Zero)
                {
                    vectors[0] = velocity;
                }
                int Length = Main.rand.Next(18, 28);
                if (type == 5)
                {
                    DDHelper.BackAndForth(-1, 1, 0.05f, ref ai[0], ref Bool[0]);
                    velocity = vectors[0].RotatedBy(ai[0]);
                    Centers[0] = Centre;
                    Rotations[0] = rotating;
                    for (int B = 0; B < Body.Length; B++)
                    {
                        if (B > 0)
                        {
                            Vector2 vector = Centers[B - 1] - Centers[B];
                            Rotations[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;
                            float D = (vector.Length() - 8 * EScale / Scale) / vector.Length();
                            Centers[B] += vector * D;

                        }
                    }
                }
                if (type == 8)
                {
                    if (Main.rand.NextBool(120) && ai[0]==0)
                        ai[0] = Main.rand.Next(-20,21);
                    if(velocity.Length()<3)
                    {
                        velocity *= 1.02F;
                    }
                    else
                    {
                        velocity *= 0.98F;
                    }
                    if(ai[0]>0)
                    {
                        ai[0]--;
                        velocity = velocity.RotatedBy(MathHelper.TwoPi / 100);
                    }
                    else if (ai[0] < 0)
                    {
                        ai[0]++;
                        velocity = velocity.RotatedBy(-MathHelper.TwoPi / 100);
                    }
                    Centers[0] = Centre;
                    Rotations[0] = rotating;
                    for (int B = 0; B < Body.Length; B++)
                    {
                        if (B > 0)
                        {
                            Vector2 vector = Centers[B - 1] - Centers[B];
                            Rotations[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;
                            float D = (vector.Length() - 40*EScale/Scale) / vector.Length();
                            Centers[B] += vector * D;

                        }
                    }
                    Length = Main.rand.Next(10, 14);

                }
                if (Body.Length == 5)
                {
                    Body = new int[Length];
                    Centers = new Vector2[Length];
                    Rotations = new float[Length];
                    for (int B = 0; B < Length; B++)
                    {
                        Centers[B] = Centre - new Vector2(0.1f);
                    }
                }
            }
            if (type == 6)
            {
                Centers[0] = Centre;
                for (int i = Centers.Length - 1; i > 0; i--)
                {
                    Centers[i] = Centers[i - 1];
                }
                ai[1] += Main.rand.NextFloat(8, 10) * Scale;
                if (ai[1] > 200)
                {
                    ai[0] += 0.05F;
                    if (ai[0] > 1)
                    {
                        active = false;
                    }
                }
                rotating += 0.2F;
            }
            if (type == 7)
            {
                rotating = MathHelper.Pi;
                Centers[0] = Centre;
                for (int i = Centers.Length - 1; i > 0; i--)
                {
                    Centers[i] = Centers[i - 1];
                }
                ai[1] += Main.rand.NextFloat(8,10)*Scale;
                if (ai[1] > 300)
                {
                    Vector2 vector = position;
                    EScale += Scale * 0.04F;
                    position = vector;
                    velocity = velocity.PerfectNormalize() * 1.25F;
                    ai[0] += 0.05F;
                    if (ai[0] > 1)
                    {
                        active = false;
                    }
                }
            }
            position += velocity;
            //rotating = velocity.ToRotation() + MathHelper.PiOver2;
        }
        public void Dawn(SpriteBatch sb)
        {
            Color color = Main.ColorOfTheSkies;
            if (!active || type == 0)
            {
                return;
            }
            Vector2 value = Main.screenPosition;
            Vector2 Centre = this.Centre;
            Centre = (Centre - value) * this.Scale;
            float Scale = this.EScale;
            Texture2D texture = ProjTexture[type].Value;
            if (type == 1)
            {
                sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, rotating, new Vector2(texture.Width, texture.Height) / 2, 0.5f * 0.3F, 0, 0);
            }

            if (type == 2 || type == 3)
            {
                if (npc != null)
                {
                    color.A = 200;
                    npc.position = Centre;
                    npc.rotation = rotating;
                    npc.scale = Scale;
                    if (Layer >= 2)
                    {
                        color.A += 10;
                    }
                    if (Layer >= 3)
                    {
                        color.A += 10;
                    }
                    if (Layer >= 4)
                    {
                        color.A += 10;
                    }
                    if (Layer >= 5)
                    {
                        color.A += 10;
                    }
                    NPCLoader.FindFrame(npc, 0);
                    Matrix transformationMatrix = Main.BackgroundViewMatrix.TransformationMatrix;
                    transformationMatrix.Translation -= Main.BackgroundViewMatrix.ZoomMatrix.Translation * new Vector3(1f, Main.BackgroundViewMatrix.Effects.HasFlag(SpriteEffects.FlipVertically) ? (-1f) : 1f, 1f);
                    sb.End();
                    sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, transformationMatrix);
                    NPCLoader.PreDraw(npc, sb, Vector2.Zero, color);
                    NPCLoader.PostDraw(npc, sb, Vector2.Zero, color);
                }
            }
            Color Fcolor = new Color(253, 62, 3, 0);
            if (type == 4)
            {
                sb.Draw(texture, Centre, null, Fcolor, rotating, new Vector2(texture.Width, texture.Height) / 2, Scale/2, 0, 0);
                sb.Draw(texture, Centre, null, Fcolor, rotating, new Vector2(texture.Width, texture.Height) / 2, Scale/3, 0, 0);
            }
            if (type == 5)
            {
                for (int B = Body.Length - 1; B >= 0; B--)
                {
                    Centre = Centers[B];
                    Centre = (Centre - value) * this.Scale;
                    if (B == 0)
                    {
                        texture = TextureAssets.Npc[ModContent.NPCType<狱火小蛇头>()].Value;
                        sb.Draw(texture,Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[0], new Vector2(texture.Width / 2, texture.Height / 2), Scale/4, 0, 0);
                        sb.Draw(texture,Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[0], new Vector2(texture.Width / 2, texture.Height / 2), Scale/4, 0, 0);
                        sb.Draw(texture,Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[0], new Vector2(texture.Width / 2, texture.Height / 2), Scale/4, 0, 0);
                    }
                    
                    else if (B < Body.Length - 4)
                    {
                        texture = TextureAssets.Npc[ModContent.NPCType<狱火小蛇身>()].Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/4, 0, 0);
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/4, 0, 0);
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/4, 0, 0);
                    }
                    else if (B < Body.Length - 3)
                    {
                        texture = TextureAssets.Npc[ModContent.NPCType<狱火小蛇身>()].Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/5, 0, 0);
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/5, 0, 0);
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/5, 0, 0);
                    }
                    else if (B < Body.Length - 2)
                    {
                        texture = TextureAssets.Npc[ModContent.NPCType<狱火小蛇身>()].Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/6, 0, 0);
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/6, 0, 0);
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/6, 0, 0);
                    }
                    else if (B < Body.Length - 1)
                    {
                        texture = TextureAssets.Npc[ModContent.NPCType<狱火小蛇身>()].Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/7, 0, 0);
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/7, 0, 0);
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/7, 0, 0);
                    }
                    else
                    {
                        texture = TextureAssets.Npc[ModContent.NPCType<狱火小蛇尾>()].Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/8, 0, 0);
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/8, 0, 0);
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Fcolor, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale/8, 0, 0);
                    }
                }
            }
            if (type == 6)
            {
                Texture2D texture2 = BossStar.Glow.Value;
                for (int a = 0; a < Centers.Length; a++)
                {
                    Vector2 vector = (Centers[a] - value) * this.Scale;
                    sb.Draw(texture2, vector, null, new Color(0, 100, 255, 0)*(1- ai[0]), rotating, texture2.Size() / 2, Scale * 1.2F * (1 - ((float)a / Centers.Length)), 0, 0);
                }

                
                sb.Draw(texture, Centre, null, Color.White * (1 - ai[0]), rotating, texture.Size() / 2, Scale, 0, 0);
                sb.Draw(texture2, Centre, null, new Color(0, 100, 255, 0) * (1 - ai[0]), rotating, texture2.Size() / 2, Scale * 1.2F, 0, 0);
            }
            if (type == 7)
            {
                Texture2D texture2 = BossHeart.Glow.Value;
                for (int a = 0; a < Centers.Length; a++)
                {
                    Vector2 vector = (Centers[a] - value) * this.Scale;
                    sb.Draw(texture2, vector, null, new Color(255, 50, 50, 0) * (1 - ai[0]), rotating, texture2.Size() / 2, Scale * 1.2F * (1 - ((float)a / Centers.Length)), 0, 0);
                }


                sb.Draw(texture, Centre, null, Color.White * (1 - ai[0]), rotating, texture.Size() / 2, Scale, 0, 0);
                sb.Draw(texture2, Centre, null, new Color(255, 50, 50, 0) * (1 - ai[0]), rotating, texture2.Size() / 2, Scale * 1.2F, 0, 0);
            }
            if (type == 8)
            {
                color.A = 200;
                if (Layer >= 2)
                {
                    color.A += 10;
                }
                if (Layer >= 3)
                {
                    color.A += 10;
                }
                if (Layer >= 4)
                {
                    color.A += 10;
                }
                if (Layer >= 5)
                {
                    color.A += 10;
                }
                for (int B = Body.Length - 1; B >= 0; B--)
                {
                    Centre = Centers[B];
                    Centre = (Centre - value) * this.Scale;
                    if (B == 0)
                    {
                        texture = 鬼牙头.Glow2.Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), new Color(255, 255, 255, 0), Rotations[0], new Vector2(texture.Width / 2, texture.Height / 2), Scale, 0, 0);
                    }
                    else if (B < Body.Length - 1)
                    { texture = 鬼牙身.Glow2.Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), new Color(255, 255, 255, 0), Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale, 0, 0);
                    }
                    else
                    {
                        texture = 鬼牙尾.Glow2.Value; 
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), new Color(255,255,255,0), Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale, 0, 0);
                    }
                }
                for (int B = Body.Length - 1; B >= 0; B--)
                {
                    Centre = Centers[B];
                    Centre = (Centre - value) * this.Scale;
                    if (B == 0)
                    {
                        texture = TextureAssets.Npc[ModContent.NPCType<鬼牙头>()].Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Rotations[0], new Vector2(texture.Width / 2, texture.Height / 2), Scale, 0, 0);

                        texture = 鬼牙头.Glow.Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, Rotations[0], new Vector2(texture.Width / 2, texture.Height / 2), Scale, 0, 0);
                    }
                    else if (B < Body.Length - 1)
                    {
                        texture = TextureAssets.Npc[ModContent.NPCType<鬼牙身>()].Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale, 0, 0);
                        texture = 鬼牙身.Glow.Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale, 0, 0);
                    }
                    else
                    {
                        texture = TextureAssets.Npc[ModContent.NPCType<鬼牙尾>()].Value;
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale, 0, 0);

                        texture = 鬼牙尾.Glow.Value; 
                        sb.Draw(texture, Centre, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, Rotations[B], new Vector2(texture.Width / 2, texture.Height / 2), Scale, 0, 0);
                    }
                }
            }
        }
        int[] Body = new int[5];
        Vector2[] Centers = new Vector2[5];
        float[] Rotations = new float[5];

        public static Asset<Texture2D>[] ProjTexture = new Asset<Texture2D>[100];
        public void Tex(int Type, Asset<Texture2D> asset)
        {
            if (ProjTexture[Type] == null)
                ProjTexture[Type] = asset; 
            
        }
        public void SetDefault(int type)
        {
            Scale = 0.15F;
            EScale = 0.1f;
            if (Layer >= 2)
            {
                Scale += 0.05F;
                EScale = 0.2f;
            }
            if (Layer >= 3)
            {
                Scale += 0.05F;
                EScale = 0.3f;
            }
            if (Layer >= 4)
            {
                Scale += 0.05F;
                EScale = 0.4f;
            }
            if (Layer >= 5)
            {
                Scale += 0.05F;
                EScale = 0.5f;
            }
            if (ProjTexture == null|| ProjTexture.Length<100)
            {
                ProjTexture = new Asset<Texture2D>[100];
            }
            if (type != 0)
                ProjTexture[type] = DDTextures.Nullpng;
            //陨石激光
            if (type == 1)
            {
                if (ProjTexture[type] == null)
                    ProjTexture[type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/BossOrangeLaser");
                width = 7;
                height = 7;
            }
            //陨石歼灭者
            if (type == 2)
            {
                npc = new NPC();
                npc.SetDefaults(ModContent.NPCType<MeteorAnnihilator>());
                aiStyle = 1;
                EScale *= 0.8f;
            }
            //陨石探测器
            if (type == 3)
            {
                npc = new NPC();
                npc.SetDefaults(ModContent.NPCType<MeteorProbe>());
                aiStyle = 1;
                EScale *= 0.8f;
            }
            //火焰粒子
            if (type == 4)
            {
                if (ProjTexture[type] == null)
                    ProjTexture[type] = DDTextures.MiniVoidStar;
                aiStyle = 2;
            }
            //狱火蛇
            if (type == 5)
            {
                if (ProjTexture[type] == null)
                    ProjTexture[type] = DDTextures.MiniVoidStar;
                aiStyle = 3;
            }
            //星星
            if (type == 6)
            {
                if (ProjTexture[type] == null)
                    ProjTexture[type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/BossStar");

                Centers = new Vector2[9];
                Rotations = new float[9];
                EScale *= Main.rand.NextFloat(1.2F, 1.8F);
            }
            //心心
            if (type == 7)
            {
                if (ProjTexture[type] == null)
                    ProjTexture[type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/BossHeart");
                width = height = 22;
                EScale *= Main.rand.NextFloat(0.8F, 1F);
            }
            //鬼牙
            if (type == 8)
            {
                if (ProjTexture[type] == null)
                    ProjTexture[type] = DDTextures.MiniVoidStar;
                aiStyle = 3;
            }
        }
        public static int NewProj(Vector2 position, Vector2 velocity, int type,float Speed, int Layer)
        {
            for (int k = 0; k < SkyDowned.Proj.Length; k++)
            {
                if (!SkyDowned.Proj[k].active)
                {
                    SkyDowned.Proj[k] = new SkyProj();
                    SkyDowned.Proj[k].Layer = Layer;
                    if(SkyDowned.Proj[k].Layer>=5)
                    {
                        SkyDowned.Proj[k].Depth = Main.rand.NextFloat(0, 2);
                    }
                    if(SkyDowned.Proj[k].Layer==4)
                    {
                        SkyDowned.Proj[k].Depth = Main.rand.NextFloat(2, 3);
                    }
                    if(SkyDowned.Proj[k].Layer==3)
                    {
                        SkyDowned.Proj[k].Depth = Main.rand.NextFloat(3, 4);
                    }
                    if(SkyDowned.Proj[k].Layer==2)
                    {
                        SkyDowned.Proj[k].Depth = Main.rand.NextFloat(4, 5);
                    }
                    if(SkyDowned.Proj[k].Layer==1)
                    {
                        SkyDowned.Proj[k].Depth = Main.rand.NextFloat(5, 6);
                    }
                    SkyDowned.Proj[k].SetDefault(type);

                    SkyDowned.Proj[k].position = position + Main.screenPosition;
                    SkyDowned.Proj[k].velocity = velocity;
                    SkyDowned.Proj[k].type = type;
                    SkyDowned.Proj[k].Speed = Speed;
                    SkyDowned.Proj[k].Ro = Main.rand.NextFloat(0, MathHelper.TwoPi);
                    SkyDowned.Proj[k].active = true;
                    return k;
                }
            }
            return 0;
        }
    }
}
