using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Sundries;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Content.Projectiles.Melee;
using DDmod.Modkey;
using DDmod.ModLinkage.BossChecklist;
using DDmod.Players;
using DDmod.Sync;
using DDmod.UI.HunterQuests;
using DDmod.UI.ItemUI;
using DDmod.UI.ItemUI.背包;
using System.Collections;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using static DDmod.Players.DDPlayer;

namespace DDmod.UI.PlaystationUI.Game.MeteorsPlane
{
    public class MeteorsPlayer
    {
        public int GameType;
        public int HitTime;
        public int HitTime2;
        public int MaxMinions = 0; 
        public MeteorsPlayer(int GameType)
        {
            this.GameType = GameType;
        }
        public Vector2 velocity;
        public Vector2 Scale = new Vector2(0.5F);
        public  Vector2 Size()
        {
            return new Vector2(20)* Scale;
        }
        public Vector2 Position;
        public int MaxLife = 10;
        public int Life = 10;
        public Vector2 Centre()
        {
            return Position+Size() / 2;
        }
        public Rectangle Rect => new Rectangle((int)Position.X, (int)Position.Y,(int)Size().X, (int)Size().Y);
        public int[] ai = new int[3];
        public void Collide(MeteorsProj proj)
        {
            if (proj.Active && proj.Collide(PlaystationSystem.Playstation[GameType].player[Main.myPlayer]))
            {
                Hit(proj.Damage);
            }
        }
        public void Hit(int Damage)
        {
            HitTime = 30;
            Life -= Damage;
            if (Life <= 0)
            {
                Kill();
            }
        }
        public void Kill()
        {
            for (int a = 0; a < 120; a++)
            {
                int P = MeteorsDust.NewDust(GameType, Position + Size() / 2, new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.DustType<光球粒子>(), Main.rand.NextFloat(0.2F, 2f));
                PlaystationSystem.Playstation[GameType].dust[P].color = new Color(255, 157, 0, 0);
            }
        }
        public void Initialize()
        {
            HitTime = 60;
        }
        public void 控制飞机(bool Left, bool Right, bool Up, bool Down)
        {
            if (Left)
            {
                if (velocity.X > -5)
                {
                    velocity.X -= 0.5F;
                }
                else
                {
                    velocity.X = -5;
                }
            }
            if (Right)
            {
                if (velocity.X < 5)
                {
                    velocity.X += 0.5F;
                }
                else
                {
                    velocity.X = 5;
                }
            }
            if (Up)
            {
                if (velocity.Y > -5)
                {
                    velocity.Y -= 0.5F;
                }
                else
                {
                    velocity.Y = -5;
                }
            }
            if (Down)
            {
                if (velocity.Y < 5)
                {
                    velocity.Y += 0.5F;
                }
                else
                {
                    velocity.Y = 5;
                }
            }
            Position += velocity;
            velocity *= 0.8F;

            if(Position.X<-Size().X / 2)
            {
                Position.X = -Size().X / 2;
            }
            if (Position.X > ((MeteorsPlaneMian)PlaystationSystem.Playstation[GameType]).ScreenSize.X - Size().X/2)
            {
                Position.X = ((MeteorsPlaneMian)PlaystationSystem.Playstation[GameType]).ScreenSize.X - Size().X/2;
            }
            if (Position.Y < -Size().Y / 2)
            {
                Position.Y = -Size().Y / 2;
            }
            if (Position.Y > ((MeteorsPlaneMian)PlaystationSystem.Playstation[GameType]).ScreenSize.Y- Size().Y/2-40)
            {
                Position.Y = ((MeteorsPlaneMian)PlaystationSystem.Playstation[GameType]).ScreenSize.Y - Size().Y/2-40;
            }
        }
        public void 碰撞()
        {
            if (Life > 0)
            {
                if (HitTime <= 0)
                {
                    for (int a = 0; a < PlaystationSystem.Playstation[GameType].proj.Length; a++)
                    {
                        if(PlaystationSystem.Playstation[GameType].proj[a].Active&& PlaystationSystem.Playstation[GameType].proj[a].Hostile)
                        Collide(PlaystationSystem.Playstation[GameType].proj[a]);
                    }
                    for (int a = 0; a < PlaystationSystem.Playstation[GameType].npc.Length; a++)
                    {
                        if (PlaystationSystem.Playstation[GameType].npc[a].Active && PlaystationSystem.Playstation[GameType].npc[a].Rect.Intersects(Rect))
                        {
                            Hit(PlaystationSystem.Playstation[GameType].npc[a].Damage);
                        }
                    }
                }
            }
            if (HitTime > 0)
            {
                HitTime2++;
                HitTime--;
            }
            else
            {
                HitTime2 = 0;
            }
        }
        public int 等级()
        {
            return 0;
        }
        public int ODamage = 20;
        /// <summary>
        /// 0:普通  1:激光  2:爆破弹  4:反弹
        /// </summary>
        public int ShootType = 2;
        /// <summary>
        /// 导弹数量
        /// </summary>
        public int Missile = 0;
        
        public void 发射()
        {
            ShootType = 1;
            Missile = 3;
            MaxMinions = 3;
            int Damage = ODamage;
            int Attackspeed = 15;
            int Shoot = 1;
            float ShootSpeed = -10;
            Vector2 vector = new Vector2(0, ShootSpeed);
            Vector2 Centre =this.Centre();
            if (PlaystationSystem.Playstation[GameType].Start&&Life>0)
            {
                ai[0]++;
                ai[1]++;
                if (ShootType == 0)
                {
                    Shoot = 1;
                }
                else if (ShootType == 1)
                {
                    Damage /= 2;
                    Shoot = 2;
                }
                else if (ShootType == 2)
                {
                    vector /= 2;
                    Attackspeed *= 2;
                    Damage *= 2;
                    Shoot = 4;
                }
                else if (ShootType == 3)
                {
                    if (Shoot <= 1)
                    {
                        vector = vector.RotatedBy(Main.rand.NextFloat(-0.6F, 0.6F)) / 2;
                        Centre -= new Vector2(0, 24);
                        Shoot = 6;
                    }
                }
                if (ai[0] >= Attackspeed)
                {
                    if (ShootType == 0)
                    {
                        MeteorsProj.NewProj(GameType, Centre, vector, Shoot, Damage,0.5F);
                    }
                    else
                    {

                        MeteorsProj.NewProj(GameType, Centre, vector, Shoot, Damage);
                    }
                    SoundStyle sound = SoundID.Item12;
                    sound.Volume = 0.3F;
                    PlaySound(sound);
                    ai[0] = 0;
                }
                if (ai[1] >= Attackspeed*3)
                {
                    ai[1] = 0;
                    if(Missile>0)
                    {
                        for(int A=0;A< Missile; A++)
                        {
                            MeteorsProj.NewProj(GameType, this.Centre(), new Vector2(0, ShootSpeed).RotatedBy(Main.rand.NextFloat(-1,1))/3, 5, Damage*2);
                            
                        }
                    }
                }

                if(MaxMinions>0)
                {
                    MeteorsProj.MinionNewProj(GameType,MaxMinions, this.Centre(), 3, ODamage,Main.myPlayer);
                }
            }
        }
    }
}