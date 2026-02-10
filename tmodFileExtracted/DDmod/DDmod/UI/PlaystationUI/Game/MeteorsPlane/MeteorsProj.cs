using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Sundries;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Modkey;
using DDmod.ModLinkage.BossChecklist;
using DDmod.Players;
using DDmod.Sync;
using DDmod.UI.HunterQuests;
using DDmod.UI.ItemUI;
using DDmod.UI.ItemUI.背包;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Collections;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.UI;
using static DDmod.Players.DDPlayer;

namespace DDmod.UI.PlaystationUI.Game.MeteorsPlane
{
    public class MeteorsProj
    {
        public int GameType;
        public string Text = "DDmod/UI/PlaystationUI/Game/MeteorsPlane/Proj/Proj_";
        public static Asset<Texture2D>[] Texture;
        public MeteorsProj(int GameType)
        {
            this.GameType = GameType;
            Load();
            invinc = new int[200];
        }
        public void Load()
        {
            int T = 6;
            Texture = new Asset<Texture2D>[T + 1];
            for (int a = 0; a < Texture.Length; a++)
            {
                Texture[a] = ModContent.Request<Texture2D>(Text + a);
            }
        }
        public Vector2[] oldVels;
        public bool Banvelocity;
        public Vector2 velocity;
        public Vector2 Position;
        public Vector2 Size;
        public bool Active;
        public bool Hostile;
        public bool Friendly;
        public int WhoamI;
        public int Damage;
        public bool CanDamage = true;
        public int Penetrate = 1;
        public int timeLeft = 3600;
        public int Type = 0;
        public int Alpha = 0;
        public float Visibility => 1-((float)Alpha/255);
        public float rotation;
        public Vector2 Scale = new Vector2(0.5F);
        public Rectangle Rect => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        public int AIType = 0;
        public int[] Times = new int[3]; 
        public float[] ai = new float[3]; 
        public bool[] Bool = new bool[3];
        public int[] invinc;
        public void AI()
        {
            if (Type == 1)
            {
                rotation = velocity.ToRotation() + MathHelper.PiOver2;
                if (Alpha > 0)
                {
                    Alpha -= 20;
                }
                else
                {
                    Alpha = 0;
                }
                Position = Position + Size / 2;
                Scale = new Vector2(ai[0]);
                Size *= Scale;
                Position = Position - Size / 2;

            }
            if (Type == 2)
            {
                Position = PlaystationSystem.Playstation[GameType].player[(int)ai[0]].Centre()-Size/2;
                if (!Bool[0] && Alpha > 0)
                {
                    Alpha -= 60;
                }
                else
                {
                    Bool[0] = true;
                    Alpha += 60;
                    if(Alpha>255)
                    {
                        Kill();
                    }
                }
            }
            if (Type == 3)
            {
                if (Alpha > 0)
                {
                    Alpha -= 20;
                }
                else
                {
                    Alpha = 0;
                }
                MeteorsPlayer player = PlaystationSystem.Playstation[GameType].player[(int)ai[0]];
                //Position += player.velocity;
                Vector2 vector = player.Position - Position;
                Times[0]++;
                if (Times[0] >=15)
                {
                    MeteorsProj.NewProj(GameType, Position + Size/2, new Vector2(0, -10), 1, Damage/2,0.25F);
                    SoundStyle sound = SoundID.Item12;
                    sound.Volume = 0.1F;
                    PlaySound(sound);
                    Times[0] = 0;
                }
                if (vector.Length() > 30)
                {
                    velocity = (velocity * 20 + vector.PerfectNormalize() * 4) / 21;
                }
                else
                {
                    if(velocity==Vector2.Zero)
                    {
                        velocity = new Vector2(0, -2);
                    }
                    else
                    if(velocity.Length()<4)
                    {
                        velocity *= 1.1F;
                    }
                    else
                    {
                        velocity *= 0.9f;
                    }
                }
                for (int a = 0; a < PlaystationSystem.Playstation[GameType].proj.Length; a++)
                {
                    if (PlaystationSystem.Playstation[GameType].proj[a] == null)
                    {
                        PlaystationSystem.Playstation[GameType].proj[a] = new MeteorsProj(GameType);
                    }
                    if (PlaystationSystem.Playstation[GameType].proj[a].Active)
                    {
                        if (PlaystationSystem.Playstation[GameType].proj[a].ai[0] == ai[0] && PlaystationSystem.Playstation[GameType].proj[a].Type == 3)
                        {
                            Vector2 vector2 = (PlaystationSystem.Playstation[GameType].proj[a].Position + PlaystationSystem.Playstation[GameType].proj[a].Size / 2 - (Position + Size / 2));
                            if (vector2.Length() < Size.Length() / 2)
                            {
                                velocity -= vector2.PerfectNormalize()*5;
                            } }
                    }
                }
                timeLeft = 5;
            }
            if (Type == 4)
            {
                rotation = velocity.ToRotation() + MathHelper.PiOver2;
                if (Alpha > 0)
                {
                    Alpha -= 20;
                }
                else
                {
                    Alpha = 0;
                }

            }
            if (Type == 5)
            {
                rotation = velocity.ToRotation() + MathHelper.PiOver2;
                    MeteorsNPC result = null;
                    float num = 500;
                for (int i = 0; i < 200; i++)
                {
                    MeteorsNPC npc = PlaystationSystem.Playstation[GameType].npc[i];
                    if (npc.Active)
                    {
                        float num2 = ((Position + Size / 2) - npc.Position + npc.Size / 2).Length();
                        if (!(num <= num2))
                        {
                            num = num2;
                            result = npc;
                        }
                    }
                }
                if(result!=null)
                {
                    Vector2 vector = result.Position + result.Size / 2-(Position + Size / 2);
                    velocity = (velocity * 20 + vector.PerfectNormalize() * 8) / 21;

                }
                 if (Alpha > 0)
                {
                    Alpha -= 20;
                }
                else
                {
                    Alpha = 0;
                }

            }
            if (Type == 6)
            {
                if (Alpha > 0)
                {
                    Alpha -= 20;
                }
                else
                {
                    Alpha = 0;
                }

                if (Position.X < 0 ||
                    Position.X + Size.X > PlaystationSystem.ScreenSize.X)
                {
                    velocity.X = -velocity.X;
                }

                if (Position.Y < 0 ||
                    Position.Y + Size.Y > PlaystationSystem.ScreenSize.Y)
                {
                    velocity.Y = -velocity.Y;
                }

            }
        }
        public void Setdefault()
        {
            //激光
            if (Type == 1)
            {
                Size = new Vector2(10, 10);
                Alpha = 255;
                Friendly = true;
                timeLeft = 300;
            }
            //无延迟激光
            if (Type == 2)
            {
                Size = new Vector2(10, 10);
                Alpha = 255;
                Friendly = true;
                timeLeft = 300;
                Penetrate = -1;
                Banvelocity = true;

            }
            //无人机
            if (Type == 3)
            {
                Size = new Vector2(10, 10);
                Alpha = 255;
                Friendly = true;
                timeLeft = 300;
                Penetrate = -1;
                CanDamage = false;
            }
            //爆破弹
            if (Type == 4)
            {
                Size = new Vector2(14, 14);
                Alpha = 255;
                Friendly = true;
                Scale = new Vector2(0.6F);
                timeLeft = 300;
            }
            //导弹
            if (Type == 5)
            {
                Size = new Vector2(18, 18);
                Alpha = 255;
                Friendly = true;
                timeLeft = 300;
                Penetrate = 1;
                oldVels = new Vector2[12];
            }
            //反弹弹
            if (Type == 6)
            {
                Size = new Vector2(10, 10);
                Alpha = 255;
                Friendly = true;
                timeLeft = 300;
                oldVels = new Vector2[12];
            }
            Size *= Scale;
        }
        public void Kill()
        {
            if (Type == 1)
            {
                for (int a = 0; a < 15; a++)
                {
                    int P = MeteorsDust.NewDust(GameType, Position + Size / 2, new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*Scale, ModContent.DustType<光球粒子>(), Main.rand.NextFloat(0.5F, 2f) * Scale.X);
                    PlaystationSystem.Playstation[GameType].dust[P].color = new Color(255, 157, 0, 0);
                    PlaystationSystem.Playstation[GameType].dust[P].Extraspeed = Scale.X*2;
                }
            }
            if (Type == 5)
            {
                for (int a = 0; a < 15; a++)
                {
                    int P = MeteorsDust.NewDust(GameType, Position + Size / 2, new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*Scale, ModContent.DustType<光球粒子>(), Main.rand.NextFloat(0.5F, 2f) * Scale.X);
                    PlaystationSystem.Playstation[GameType].dust[P].color = new Color(252, 160, 28, 155);
                    PlaystationSystem.Playstation[GameType].dust[P].Extraspeed = Scale.X*2;
                }
            }
            if (Type == 6)
            {
                for (int a = 0; a < 15; a++)
                {
                    int P = MeteorsDust.NewDust(GameType, Position + Size / 2, new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*Scale, ModContent.DustType<光球粒子>(), Main.rand.NextFloat(0.5F, 2f) * Scale.X);
                    PlaystationSystem.Playstation[GameType].dust[P].color = new Color(255, 157, 0, 0);
                    PlaystationSystem.Playstation[GameType].dust[P].Extraspeed = Scale.X*2;
                }
            }
            if (Type == 4)
            {
                for (int A = 0; A < 5; A++)
                {
                    MeteorsProj.NewProj(GameType, Position+Size/2, velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*2, 1, Damage/2,0.5F);

                }
                for (int a = 0; a < 15; a++)
                {
                    int P = MeteorsDust.NewDust(GameType, Position + Size / 2, new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Scale, ModContent.DustType<光球粒子>(), Main.rand.NextFloat(1F, 3f) * Scale.X);
                    PlaystationSystem.Playstation[GameType].dust[P].color = new Color(255, 157, 0, 0);
                    PlaystationSystem.Playstation[GameType].dust[P].Extraspeed = Scale.X * 2;
                }
            }
                Active = false;
        }
        public void Update()
        {
            timeLeft--;
            if (timeLeft < 0)
            {
                Kill();
            }
            AI();
            for(int a=0;a< PlaystationSystem.Playstation[GameType].npc.Length; a++)
            {
                if(invinc[a]>0)
                {
                    invinc[a]--;
                }
            }
            if (oldVels != null && oldVels.Length > 0)
            {
                oldVels[0] = Position;
                for (int i = oldVels.Length - 1; i > 0; i--)
                {
                    oldVels[i] = oldVels[i - 1];
                }
            }
            if (!Banvelocity)
            {
                Position += velocity;
            }
            if (Position.X < -500||
                Position.X > PlaystationSystem.ScreenSize.X + 500||
               Position.Y < -500 ||
              Position.Y > PlaystationSystem.ScreenSize.Y + 500)
            {
                Active = false;
            }
        }
        public bool? ModifyCollide(Rectangle rectangle)
        {
            if(!CanDamage)
            {
                return false;
            }
            if(Type==2)
            {
                Rectangle Rect = this.Rect;
                for (int a = 0; a < 100; a++)
                {
                    Rect.Y -= (int)Size.Y;
                    if (rectangle.Intersects(Rect))
                    {
                        return true;
                    }
                }
            }
            return null;
        }
        public bool Collide(MeteorsNPC npc)
        {
            bool? C = ModifyCollide(npc.Rect);
            if (C != null)
            {
                return C.Value;
            }
            if (npc.Rect.Intersects(Rect))
            {
                return true;
            }
            return false;
        }
        public bool Collide(MeteorsPlayer player)
        {
            bool? C = ModifyCollide(player.Rect);
            if (C != null)
            {
                return C.Value;
            }
            if (player.Rect.Intersects(Rect))
            {
                return true;
            }
            return false;
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(252, 160,28 ),
                new Color(252, 160,28 ),
                new Color(252, 160,28 ),
                new Color(248, 66,5 ),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(248, 66, 5), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                10,
                20,
                30,
                20,
            }), 10, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal Trailing TrailDrawer;
        public void Draw(SpriteBatch spriteBatch, Vector2 ScreenPos)
        {
            Texture2D texture = Texture[0].Value;
            if (Type < Texture.Length)
            {
                texture = Texture[Type].Value;
            }
            if (Type == 2)
            {
                Vector2 vector = Position;
                vector.Y -= (int)Size.Y * 5;
                for (int a = 5; a < 100; a++)
                {
                        spriteBatch.Draw(texture, vector + ScreenPos, null, Color.White * Visibility, 0, Vector2.Zero, Scale, 0, 0);
                    vector.Y -= (int)Size.Y;
                }
                spriteBatch.Draw(DDTextures.Starlight3.Value, Position + Size / 2 - new Vector2(0, (int)Size.Y * 5) + ScreenPos, null, new Color(255, 157, 100, 0) * Visibility, 0, DDTextures.Starlight3.Size() / 2, Scale * new Vector2(1F,1.5F), 0, 0);
                spriteBatch.Draw(DDTextures.Starlight3.Value, Position + Size / 2 - new Vector2(0, (int)Size.Y * 5) + ScreenPos, null, new Color(255, 157, 100, 0) * Visibility, MathHelper.PiOver2, DDTextures.Starlight3.Size() / 2, Scale * new Vector2(0.5F, 2F), 0, 0);
                return;
            }
            if (Type == 3)
            {
                if (!Bool[1])
                {
                    Times[1]++;
                    if(Times[1]>30)
                    {
                        Bool[1] = true;
                    }
                }
                else
                {
                    Times[1]--;
                    if (Times[1] < 20)
                    {
                        Bool[1] = false;
                    }
                }
                spriteBatch.Draw(DDTextures.VoidStar.Value, Position + Size / 2 + ScreenPos, new Rectangle(0,DDTextures.VoidStar.Height()/2, DDTextures.VoidStar.Width(), DDTextures.VoidStar.Height()/2), new Color(233, 127, 5, 0) * Visibility, 0, new Vector2(DDTextures.VoidStar.Width()/2, 0), Scale * new Vector2(0.5F, 1F * Times[1]/15)* Scale, 0, 0);
                spriteBatch.Draw(DDTextures.VoidStar.Value, Position + Size / 2 + ScreenPos, new Rectangle(0,DDTextures.VoidStar.Height()/2, DDTextures.VoidStar.Width(), DDTextures.VoidStar.Height()/2), new Color(233, 127, 5, 0) * Visibility, 0, new Vector2(DDTextures.VoidStar.Width()/2, 0), Scale * new Vector2(0.5F, 1F * Times[1]/15)* Scale, 0, 0);

                spriteBatch.Draw(texture, Position + Size / 2 + ScreenPos, null, Color.White * Visibility, rotation, texture.Size() / 2, Scale, 0, 0);
                    return;
            }
            if (Type == 5)
            {
                if (TrailDrawer == null)
                {
                    TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"],1);
                }
                GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
                GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
                TrailDrawer.Draw(oldVels, Size * 0.5f+ScreenPos, 102, null,Scale.X, Visibility, spriteBatch);
                Main.spriteBatch.Draw(texture, Position+Size/ 2 + ScreenPos, null, Color.White * Visibility, rotation, new Vector2(texture.Width) / 2, Scale, 0, 0f);
                return;
            }
            if (Type == 6)
            {

                spriteBatch.Draw(texture, Position + Size / 2 + ScreenPos, null, Color.White * Visibility, rotation, texture.Size() / 2, Scale, 0, 0);
                for (int a = 0; a < oldVels.Length; a++)
                {
                    spriteBatch.Draw(texture, oldVels[a] + Size / 2 + ScreenPos, null, Color.White * Visibility* (1F-(float)a/oldVels.Length), rotation, texture.Size() / 2, Scale, 0, 0);
                }
                return;
            }
            spriteBatch.Draw(texture, Position+Size/2 + ScreenPos, null, Color.White * Visibility, rotation, new Vector2(texture.Width/2), Scale, 0, 0);

            //spriteBatch.Draw(DDTextures.WhitePng.Value, Position + ScreenPos, null, Color.White, 0, Vector2.Zero, Size/2, 0, 0);

        }
        public static int NewProj(int GameType,Vector2 Position, Vector2 velocity, int Type, int Damage,float ai0 = 0,float ai1= 0,float ai2=0)
        {
            for (int a = 0; a < PlaystationSystem.Playstation[GameType].proj.Length; a++)
            {
                if (PlaystationSystem.Playstation[GameType].proj[a] == null)
                {
                    PlaystationSystem.Playstation[GameType].proj[a] = new MeteorsProj(GameType);
                }
                if (!PlaystationSystem.Playstation[GameType].proj[a].Active)
                {
                    PlaystationSystem.Playstation[GameType].proj[a] = new MeteorsProj(GameType);
                    PlaystationSystem.Playstation[GameType].proj[a].Type = Type;
                    PlaystationSystem.Playstation[GameType].proj[a].Setdefault();
                    PlaystationSystem.Playstation[GameType].proj[a].Position = Position - PlaystationSystem.Playstation[GameType].proj[a].Size / 2;
                    PlaystationSystem.Playstation[GameType].proj[a].velocity = velocity;
                    PlaystationSystem.Playstation[GameType].proj[a].Damage = Damage;
                    PlaystationSystem.Playstation[GameType].proj[a].WhoamI = a;
                    PlaystationSystem.Playstation[GameType].proj[a].ai[0] = ai0;
                    PlaystationSystem.Playstation[GameType].proj[a].ai[1] = ai1;
                    PlaystationSystem.Playstation[GameType].proj[a].ai[2] = ai2;
                    PlaystationSystem.Playstation[GameType].proj[a].Active = true;
                    return a;
                }
            }
            return 0;
        }
        public static int MinionNewProj(int GameType,int MaxMinion,Vector2 Position, int Type, int Damage,float ai0 = 0,float ai1= 0)
        {
            int Shoot = 0;
            for (int a = 0; a < PlaystationSystem.Playstation[GameType].proj.Length; a++)
            {
                if (PlaystationSystem.Playstation[GameType].proj[a] == null)
                {
                    PlaystationSystem.Playstation[GameType].proj[a] = new MeteorsProj(GameType);
                }
                if (PlaystationSystem.Playstation[GameType].proj[a].Active)
                {
                    if (PlaystationSystem.Playstation[GameType].proj[a].ai[0]==ai0 && PlaystationSystem.Playstation[GameType].proj[a].Type == 3)
                    {
                        Shoot++;
                    }
                }
            }
            if (Shoot< MaxMinion)
            {
                for (int a = 0; a < PlaystationSystem.Playstation[GameType].proj.Length; a++)
                {
                    if (!PlaystationSystem.Playstation[GameType].proj[a].Active)
                    {
                        PlaystationSystem.Playstation[GameType].proj[a] = new MeteorsProj(GameType);
                        PlaystationSystem.Playstation[GameType].proj[a].Type = Type;
                        PlaystationSystem.Playstation[GameType].proj[a].Setdefault();
                        PlaystationSystem.Playstation[GameType].proj[a].Position = Position - PlaystationSystem.Playstation[GameType].proj[a].Size / 2;
                        PlaystationSystem.Playstation[GameType].proj[a].velocity = Vector2.Zero;
                        PlaystationSystem.Playstation[GameType].proj[a].Damage = Damage;
                        PlaystationSystem.Playstation[GameType].proj[a].WhoamI = a;
                        PlaystationSystem.Playstation[GameType].proj[a].ai[0] = ai0;
                        PlaystationSystem.Playstation[GameType].proj[a].ai[1] = ai1;
                        PlaystationSystem.Playstation[GameType].proj[a].ai[2] = Shoot;
                        PlaystationSystem.Playstation[GameType].proj[a].Active = true;
                        return a;
                    }
                }
            }
            return -1;
        }
    }
}