using DDmod.Content;
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.ball;
using DDmod.Content.Projectiles.OrnamentProjectile;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Suit;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Projectiles.Summon.ArmourSummons;
using DDmod.DrawPlayer;
using DDmod.Modkey;
using DDmod.SubworldLibraryWorld;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Light;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace DDmod.Players
{
    public class AttributesPlayer : ModPlayer
    {
        /// <summary> 爱心强化 </summary>
        public bool Heartstrengthening;
        /// <summary> 心之仆从饰品 </summary>
        public bool ServantOfTheHeart;
        /// <summary> 星之守护 </summary>
        public bool GuardianOfTheStar;
        /// <summary> 星之守护强化 </summary>
        public bool GuardianOfTheStar2;
        /// <summary> 星之守护 </summary>
        public int GuardianOfTheStarCD;
        /// <summary> 星之力 </summary>
        public bool StarPower;
        /// <summary> 猩红之怒 </summary>
        public bool Crimson;
        /// <summary> 1召唤,2近战,3射手,4法师</summary>
        public byte 绿岩套;
        public bool 恶魔头颅;
        public byte 绿岩Time;
        public bool 血滴子套;
        public bool 血肉戒指;
        public bool 破坏者核心装置;
        /// <summary> 精金力量buff </summary>
        public int 精金力量;
        public byte 暗影焰力量;
        /// <summary> 碎心 </summary>
        public bool Heartbreaker;
        /// <summary> 发光蘑菇套 </summary>
        public bool GlowingMushroomSet;
        /// <summary> 钴套 </summary>
        public bool CobaltSet;
        public byte CobaltSetCD = 180;
        /// <summary> 钯金套 </summary>
        public bool PalladiumSet;
        public short PalladiumSetCD = 1800;
        /// <summary> 精金套 </summary>
        public bool RefinedGoldSet;
        public short RefinedGoldSetCD = 60;
        /// <summary> 化石套 </summary>
        public bool FossilSet;
        /// <summary> 角斗士套 </summary>
        public bool GladiatorSet;
        /// <summary> 死灵套 </summary>
        public bool UndeadSet = false;
        public bool UndeadSet2 = false;
        /// <summary>
        /// 精金套使用前的速度
        /// </summary>
        public Vector2 RefinedGoldSpeed;
        /// <summary> 鬼牙手环 </summary>
        public bool GhostfangBracelet;
        /// <summary> 流星冲刺 </summary>
        public bool MeteorDash;
        /// <summary> 碎心冷却 </summary>
        public int HeartbreakerTime;
        /// <summary> 黑暗 </summary>
        public int Dark;
        /// <summary> 格挡 </summary>
        public int Resist;
        /// <summary> 暗夜套 </summary>
        public bool ShadowSet;
        /// <summary> 忍者状态 </summary>
        public bool Ninja;
        /// <summary> 忍者跳状态 </summary>
        public bool NinjaJump;
        /// <summary> 无视重力 </summary>
        public int NoGravity;
        /// <summary> 站立 </summary>
        public int Stand;
        /// <summary> 站立 </summary>
        public int Stand2;
        public int Stand3;
        /// <summary> 站立 </summary>
        public int Gravitation;
        /// <summary> 手臂摆动 </summary>
        public float ArmSwing;
        /// <summary> 最大手臂摆动 </summary>
        public float ArmSwingMax;
        /// <summary> 手臂摆动开始</summary>
        public bool ArmSwingBegin;
        /// <summary> Y偏移 </summary>
        public float gfxOffY;
        /// <summary> 无视水 </summary>
        public int IgnoreWater;
        public bool 荆棘戒指;
        public bool 永夜戒指;
        public bool 真永夜戒指;
        public bool 神圣戒指;
        public bool 真神圣戒指;
        public bool 泰拉戒指;
        /// <summary>
        /// 永夜能量
        /// </summary>
        public bool NightEnergy;
        /// <summary>
        /// 真永夜能量
        /// </summary>
        public bool TrueNightEnergy;
        /// <summary>
        /// 神圣能量
        /// </summary>
        public bool HolyEnergy;
        /// <summary>
        /// 真神圣能量
        /// </summary>
        public bool TrueHolyEnergy;
        /// <summary>
        /// 神圣子弹护盾
        /// </summary>
        public float HolyShield;
        public void Sync()
        {
            if (Main.netMode == 1)
            {
                Mod mod = DDmod.Instance;
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.PlayersSuit);
                packet.Write((byte)Player.whoAmI);
                packet.Write(神圣戒指CD);
                packet.Write(GuardianOfTheStarCD);
                packet.Write(PalladiumSetCD);
                packet.Write(RefinedGoldSetCD);
                packet.Send(-1, Player.whoAmI);
            }
        }
        public static void Walk(Player player)
        {
            if (player.velocity.X != 0 && player.velocity.Y == 0f)
            {
                player.Aplayer().legFrameCounter += (double)Math.Abs(player.velocity.X) * 1.3;
                while (player.Aplayer().legFrameCounter > 8.0)
                {
                    player.Aplayer().legFrameCounter -= 8.0;
                    player.Aplayer().R = player.Aplayer().R + player.legFrame.Height;
                }
                if (player.Aplayer().R < player.legFrame.Height * 7)
                {
                    player.Aplayer().R = player.legFrame.Height * 19;
                }
                else if (player.Aplayer().R > player.legFrame.Height * 19)
                {
                    player.Aplayer().R = player.legFrame.Height * 7;
                }
                player.Aplayer().bodyFrameCounter += (double)(Math.Abs(player.velocity.X) * 0.5f);
                while (player.Aplayer().bodyFrameCounter > 8.0)
                {
                    player.Aplayer().bodyFrameCounter -= 8.0;
                    player.Aplayer().Body = player.Aplayer().Body + player.bodyFrame.Height;
                }
                if (player.Aplayer().Body < player.bodyFrame.Height * 7)
                {
                    player.Aplayer().Body = player.bodyFrame.Height * 19;
                }
                if (player.Aplayer().Body > player.bodyFrame.Height * 19)
                {
                    player.Aplayer().Body = player.bodyFrame.Height * 7;
                }
            }
            else
            {
                player.Aplayer().R = 0;
                player.Aplayer().Body = 0;
            }
            if (player.itemTime == 0)
            {
                player.bodyFrame.Y = player.Aplayer().Body;
            }
            player.legFrame.Y = player.Aplayer().R;
        }
        public override void ResetEffects()
        {
            Heartstrengthening = false;
            ServantOfTheHeart = false;
            GuardianOfTheStar = false;
            GuardianOfTheStar2 = false;
            StarPower = false;
            Crimson = false;
            ShadowSet = false;
            Heartbreaker = false;
            GlowingMushroomSet = false;
            MeteorDash = false;
            GhostfangBracelet = false;
            血滴子套 = false;
            绿岩套 = 0;
            破坏者核心装置 = false;
            恶魔头颅 = false;
            血肉戒指 = false;
            荆棘戒指 = false;
            神圣戒指 = false;
            永夜戒指 = false;
            真神圣戒指 = false;
            真永夜戒指 = false;
            泰拉戒指 = false;
            FossilSet = false;
            GladiatorSet = false;
            UndeadSet = false;
            UndeadSet2 = false;
            暗影焰力量 = 0;
            Resist = 0;
            if (NoGravity > 0)
            {
                NoGravity--;
            }
            if (CobaltSet)
            {
                CobaltSet = false;
                if (CobaltSetCD > 0)
                {
                    CobaltSetCD--;
                }
            }
            if (PalladiumSet)
            {
                PalladiumSet = false;
                if (PalladiumSetCD > 0)
                {
                    PalladiumSetCD--;
                }
            }
            if (RefinedGoldSet)
            {
                RefinedGoldSet = false;
            }
            if (RefinedGoldSetCD > 0)
            {
                RefinedGoldSetCD--;
            }
            if (Stand2 > 0)
            {
                if (Main.myPlayer == Player.whoAmI)
                {
                    Stand2--;
                    if (Stand2 == 0)
                    {
                        DDmod.SyncData(DDType.PlayerData, Player.whoAmI, -1, Player.whoAmI);
                    }
                }
            }
            if (Stand3 > 0)
            {
                if (Main.myPlayer == Player.whoAmI)
                {
                    Stand3--;
                    if (Stand3 == 0)
                    {
                        DDmod.SyncData(DDType.PlayerData, Player.whoAmI, -1, Player.whoAmI);
                    }
                }
            }
            if (Stand > 0)
            {
                if (Main.myPlayer == Player.whoAmI)
                {
                    Stand--;
                    if (Stand == 0)
                    {
                        DDmod.SyncData(DDType.PlayerData, Player.whoAmI, -1, Player.whoAmI);
                    }
                }
                if (Player.Aplayer().gfxOffY < 18)
                {
                    Player.position.Y += Player.Aplayer().gfxOffY;
                    Player.Aplayer().gfxOffY = 0;
                }
            }
            else
            {
                Player.position.Y += Player.Aplayer().gfxOffY;
                Player.Aplayer().gfxOffY = 0;
            }
            if (Gravitation > 0)
            {
                Player.gravControl2 = false;
                Player.gravControl = false;
                Player.noFallDmg = true;
                Gravitation--;
            }
            
            if (IgnoreWater > 0)
            {
                IgnoreWater--;
            }
            NightEnergy = !Main.dayTime;
            TrueNightEnergy = !Main.dayTime;
            HolyEnergy = Main.dayTime;
            TrueHolyEnergy = Main.dayTime;
        }
        public Vector3 ShadowColor;
        public double bodyFrameCounter;
        public int Body;
        public double legFrameCounter;
        public int R;
        public float Ve = 0;

        bool a;
        bool dash;
        int dustTime;
        int dashTime;
        public void AddBuffTime(int Type, int Time)
        {
            if (Player.HasBuff(Type))
                Player.buffTime[Player.FindBuffIndex(Type)] += Time;
            else
                Player.AddBuff(Type, Time);
        }
        public override void UpdateEquips()
        {
            //Player.portableStoolInfo.SetStats(0, 0, 0);

            //Player.GetKnockback(DamageClass.Melee) *= 100;
        }
        //永夜戒指CD
        public int YYTime;
        public override void PostUpdateEquips()
        {
            if (Player.hasMagiluminescence && Player.velocity.Y == 0f)
            {
                /*
                Player.runAcceleration *= 1.75f;
                Player.maxRunSpeed *= 1.15f;
                Player.accRunSpeed *= 1.15f;
                Player.runSlowdown *= 1.75f;
                原版的代码
                */
                Player.runAcceleration *= 1.5f / 1.75f;
                Player.maxRunSpeed *= 1.1f / 1.15f;
                Player.accRunSpeed *= 1.1f / 1.15f;
                Player.runSlowdown *= 1.5f / 1.75f;
            }
            if (HolyShield > 100)
            {
                HolyShield = 100;
            }
            if (HolyShield > 0)
            {
                HolyShield -= 0.1F;
                Player.GetDamage(DamageClass.Ranged) += (HolyShield / 100) / 10;
            }
            if (Player.whoAmI == Main.myPlayer)
            {
                if (永夜戒指)
                {
                    YYTime--;
                    NPC npc = NPCdirection.FindClosest(Player.Center, 400,false);
                    if (npc != null && YYTime <= 0)
                    {
                        Vector2 vector = npc.Center - Player.Center;
                        YYTime = 240;
                        NewProjectile(Player.GetSource_FromAI(), Player.Center, vector.PerfectNormalize()*6, ModContent.ProjectileType<永夜弹>(), 60, 10);
                    }
                }
                else if (真永夜戒指)
                {
                    YYTime--;
                    NPC npc = NPCdirection.FindClosest(Player.Center, 400, false);
                    if (npc != null && YYTime <= 0)
                    {
                        YYTime = 240;
                        NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<永夜弹>(), 100, 10,-1,1);
                        for(int a= 0;a<3;a++)
                        {
                            NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(8,0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<永夜弹>(), 30, 3, -1, 2);
                        }
                    }
                }
                else if (泰拉戒指)
                {
                    YYTime--;
                    NPC npc = NPCdirection.FindClosest(Player.Center, 600, false);
                    if (npc != null && YYTime <= 0)
                    {
                        YYTime = 240;
                        NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<泰拉弹>(), 150, 10,-1,0);
                        for(int a= 0;a<6;a++)
                        {
                            NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(8,0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<泰拉弹>(), 40, 3, -1, 1);
                        }
                    }
                }
            }
            if (血肉戒指)
            {
                int r = Player.statDefense / 2;
                Player.statDefense -= r;
                Player.statLifeMax2 += r * 3;
            }
            if (绿岩Time > 0)
            {
                绿岩Time--;
            }
            else
            if (Player.Aplayer().绿岩套 == 3&&Player.itemTime>0&&Player.ActiveItem().DamageType == DamageClass.Ranged)
            {
                绿岩Time = 120;
                if (Main.myPlayer == Player.whoAmI)
                {
                    int proj = NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(0, -10).RotatedBy(Main.rand.NextFloat(-1, 1)), ModContent.ProjectileType<绿岩导弹>(), 80, 1, -1, 0);
                    Main.projectile[proj].scale = 1F;
                }
            }
            if (MeteorDash)
            {
                if (dashTime > 0)
                {
                    dashTime--;
                }
                if (dustTime > 0)
                {
                    if (Player.velocity.X > 0)
                    {
                        NewDustSector2(2, Player.position, Player.Size, new Vector3(-1, 0, 0.2F), ModContent.DustType<速度粒子>(), 10, 30, true, 2, 5, 100, 1000, new Color(253, 102, 3, 0), null, Player.cWings, Player);
                    }
                    else
                    {
                        NewDustSector2(2, Player.position, Player.Size, new Vector3(1, 0, 0.2F), ModContent.DustType<速度粒子>(), 10, 30, true, 2, 5, 100, 1000, new Color(253, 102, 3, 0),null,Player.cWings,Player);
                    }
                    if (dustTime % 10 == 0)
                    {
                        NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(-Player.velocity.X / 2, 5), ModContent.ProjectileType<歼灭者导弹>(), 40, 1, Player.whoAmI);
                        NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(-Player.velocity.X / 2, -5), ModContent.ProjectileType<歼灭者导弹>(), 40, 1, Player.whoAmI);
                    }
                    dustTime--;
                }
                if (dashTime > 0 && dashTime % 5 == 0)
                {
                    NewDustChange3(1, Player.position, Player.Size, ModContent.DustType<速度粒子>(), 2, 6, true, 1, 3, 100, 1000, new Color(253, 102, 3, 0));
                }
            }
            if (GladiatorSet)
            {
                Player.GetDamage(DamageClass.Melee) += 0.075F;
                Player.GetAttackSpeed(DamageClass.Melee) += 0.075F;
                if (Player.statLife <= Player.statLifeMax2*0.2F)
                {
                    Player.GetDamage(DamageClass.Melee) += 0.15F;
                    Player.GetAttackSpeed(DamageClass.Melee) += 0.15F;
                }
            }
            //Main.NewText(player.dashType);
        }
        public override void SetControls()
        {
            Player player = this.Player;
            if (ModkeySetup.SetBonus.JustPressed)
            {
                SetBonus();
            }
        }
        public void SetBonus()
        {

            Player player = this.Player;
            if (player.Aplayer().绿岩套 == 4)
            {
                int Mana = 100;
                if (player.statMana < Mana)
                {
                    player.QuickMana();
                }
                if (player.statMana >= Mana)
                {
                    NewDustSector(120, player.position - new Vector2(30, 0), player.Size + new Vector2(60, 0), new Vector3(0, -1, 0.02F), ModContent.DustType<速度粒子>(), 2, 8, true, 1, 3, 100, 1000, new Color(100, 255, 100, 0));
                    if (Main.myPlayer == player.whoAmI)
                    {
                        player.AddBuff(ModContent.BuffType<绿岩能量Buff>(), 300);

                        player.statMana -= Mana;
                    }
                }
            }
            else
            if (player.Aplayer().RefinedGoldSet && player.velocity.Y != 0 && player.Aplayer().RefinedGoldSetCD == 0)
            {
                player.Aplayer().RefinedGoldSpeed = player.velocity;
                player.Aplayer().RefinedGoldSetCD = 190;
                Player.maxFallSpeed = 10000;
                player.velocity = (player.Dplayer().MouseWorld - player.Center).PerfectNormalize() * 60;
                //player.velocity = player.velocity.PerfectNormalize() * 60;
                if (player.whoAmI == Main.myPlayer)
                {
                    DDmod.SyncData(DDType.PlayerCenter, player.whoAmI, -1, player.whoAmI);
                    DDmod.SyncData(DDType.PlayersSuit, player.whoAmI, -1, player.whoAmI);

                }
            }
        }
        public static void KeyDoubleTap(Player player, int KeyDir)
        {
            //KeyDir
            //0↓,1↑,2→,3←
            int num = 0;
            if (Main.ReversedUpDownArmorSetBonuses)
                num = 1;
            //ModkeySetup.SetBonus.JustPressed
            if (KeyDir == num)
            {
                player.Aplayer().SetBonus();
            }
            if (player.Aplayer().MeteorDash)
                MDash(player, KeyDir);
        }

        
        public override void PostUpdateMiscEffects()
        {
            //Player.jumpHeight += 200;
            //Player.jumpSpeedBoost += 20;
            /*
            int i = 121;
            if (Player.inventory[58].type == 0)
            {
                Player.inventory[58].SetDefaults(i);
                Player.selectedItem = 58;
                
            }
             if(Player.inventory[58].type== i || Main.mouseItem.type == i)
            {
                Main.mouseItem = new Item();
            }*/
            if (Player.whoAmI == Main.myPlayer &&Player.HasBuff(ModContent.BuffType<恶魔力量Buff>())&& 恶魔力量Buff.A++ % 10 == 0)
            {
                int R = Player.statLifeMax2 / 200;
                if(R<1)
                {
                    R = 1;
                }
                Player.Heal(R);
            }
            if (Player.whoAmI == Main.myPlayer && Player.HasBuff(ModContent.BuffType<鬼牙眷顾>()) && Player.Dplayer().PlayerTimes % 20 == 0)
            {
                Player.Heal(3);
            }
            if (Player.statLife > Player.statLifeMax2)
                Player.statLife = Player.statLifeMax2;

        }
        /// <summary>
        /// 获取加成后的最大生命
        /// </summary>
        public int LifeMax;
        public int ManaMax;
        public static void MDash(Player player, int KeyDir)
        {
            if (player.dashType != -1|| player.dashDelay>0|| player.mount.Active)
            {
                return;
            }
            if (KeyDir == 3)
            {
                if (player.Aplayer().dashTime > 0)
                {
                    player.velocity.X = -8;
                    NewDustSector(20, player.position, player.Size, new Vector3(1, 0, 0.4F), ModContent.DustType<速度粒子>(),2, 20, true, 2, 5, 100, 1000, new Color(253, 102, 3, 0));
                }
                else
                {
                    player.velocity.X = -15;
                    player.Aplayer().dustTime = 30;
                    player.Aplayer().dashTime = 300;
                    NewDustSector(100, player.position, player.Size, new Vector3(1, 0, 0.4F), ModContent.DustType<速度粒子>(), 10, 30, true, 2, 5, 100, 1000, new Color(253, 102, 3, 0));
                }
                player.dashDelay = 20;
            }
            else
            if (KeyDir == 2)
            {
                if (player.Aplayer().dashTime > 0)
                {
                    player.velocity.X = 8;
                    NewDustSector(20, player.position, player.Size, new Vector3(-1, 0, 0.4F), ModContent.DustType<速度粒子>(), 2, 20, true, 2, 5, 100, 1000,new Color(253, 102, 3,0));
                }
                else
                {
                    player.velocity.X = 15;
                    player.Aplayer().dustTime = 30;
                    player.Aplayer().dashTime = 300;
                    NewDustSector(100, player.position, player.Size, new Vector3(-1, 0, 0.4F), ModContent.DustType<速度粒子>(), 10, 30, true, 2, 5, 100, 1000, new Color(253, 102, 3, 0));
                }
                player.dashDelay = 20;
            }
        }
        public override void PreUpdate()
        {
            if (NPC.brainOfGravity >= 0 && NPC.brainOfGravity < 200 && Vector2.Distance(Player.Center, Main.npc[NPC.brainOfGravity].Center) < 4000f)
            {
                brainOfGravity = -1;
                Player.Aplayer().Gravitation = 5;
            }
            if (Player.gravControl2 || Player.gravControl)
            {
                Player.gravControl2 = false;
                Player.gravControl = false;
                Player.Aplayer().Gravitation = 5;
            }
            if (Stand > 0)
            {
                Player.gravity = 0;
                if (Player.velocity.Y > 0)
                    Player.velocity.Y = 0;
            }
            if (Stand2 > 0)
            {
                Player.gravity = 0;
                Player.velocity = Vector2.Zero;
                if (Player.controlJump)
                {
                    Player.velocity = new Vector2(0, -0.01F);
                }
                Player.gfxOffY = 0;
            }
            if (Stand3 > 0)
            {
                Player.gravity = 0;
                if (Player.velocity.Y > 0)
                    Player.velocity.Y = 0;
                Player.gfxOffY = 0;
            }
            if (NoGravity > 0)
            {
                Player.gfxOffY = 0;
                Player.gravity = 0;
                if (Player.Dplayer().Realm > 0)
                {
                    Player.velocity.Y = -0.01F;
                }
            }
            //我看了一下你都重力,稍微改进了一下,希望你喜欢
            if (Gravitation > 0)
            {
                if (!Player.controlDown)
                {
                    Player.gravity = 0;
                    Player.noFallDmg = true;
                    if (Player.controlJump || Player.controlUp)
                    {
                        /*
                        if (Player.wingTime <= 0)
                        {
                        }*/
                        if (Player.velocity.Y > -(Player.jumpSpeed + Player.jumpSpeedBoost))
                        {
                            Player.velocity.Y -= (Player.jumpSpeed + Player.jumpSpeedBoost) / 10;
                        }
                    }

                    if (Player.velocity.Y == 0)
                    {
                        Player.velocity.Y = -0.001F;
                    }
                    if (Player.controlLeft)
                    {
                        if (Ve > -Player.maxRunSpeed * 2)
                        {
                            Ve -= 1;
                        }
                    }
                    else if (Player.controlRight)
                    {
                        if (Ve < Player.maxRunSpeed * 2)
                        {
                            Ve += 1;
                        }
                    }
                    if (Player.dashDelay == 0)
                        Player.velocity.X = Ve;
                }
            }
            if (ShadowSet)
            {
                ShadowColor = Lighting.GetColor((int)(Player.Center.X / 16), (int)(Player.Center.Y / 16)).ToVector3();
                //每帧同步
                //StartSync(ShadowColor);
            }
            float ColorV = ShadowColor.X + ShadowColor.Y + ShadowColor.Z;

            //暗影套
            if (ColorV <= 0.5f && ShadowSet && !Player.HasBuff(ModContent.BuffType<Exposed>()))
            {
                if (Dark < 200)
                {
                    Dark += 5;
                }
                Player.noFallDmg = true;
                if (Player.velocity != Vector2.Zero)
                {
                    Player.Dplayer().DodgeChance += 15;
                }
                Player.moveSpeed += 0.35f;
                if (!Main.dayTime)
                {
                    Player.moveSpeed += 0.5f;
                }
                if (!Player.controlDown)
                {
                    if (Player.velocity.Y >= 0)
                    {
                        Player.gravity = 0;
                        Player.velocity.Y = 0;
                    }
                    if (Player.controlJump)
                    {
                        Player.gravity = 0;
                        Player.velocity.Y = -5;
                    }
                }
                Player.immuneAlpha = Player.Aplayer().Dark;
            }
            else
            {
                Dark = 0;
            }
            if (GuardianOfTheStar)
            {
                GuardianOfTheStarCD++;
                if(GuardianOfTheStarCD==6000||(GuardianOfTheStar2&& GuardianOfTheStarCD ==3000))
                {
                    if (Main.myPlayer == Player.whoAmI)
                    {
                        DDmod.SyncData(DDType.PlayersSuit, Main.myPlayer, -1, Main.myPlayer);
                    }
                }
            }
            else
            {
                GuardianOfTheStarCD = 0;
            }
            if (Player.sitting.isSitting)
            {
                if (Main.tile[(int)(Player.Center.X / 16), (int)(Player.Center.Y / 16)].TileType == 497 && Main.tile[(int)(Player.Center.X / 16), (int)(Player.Center.Y / 16)].TileFrameY == 1560)
                {
                    if (Main.rand.NextBool(10))
                    {
                        Vector2 vector = Utils.RotatedBy(new Vector2(0, 1), Main.rand.NextFloat(-1f, 1f), default);
                        int A = NewProjectile(Player.GetSource_FromAI(), (int)(Player.Center.X / 16) * 16 + 10, (int)(Player.Center.Y / 16) * 16 + 20, vector.X, vector.Y, ModContent.ProjectileType<TerraShit>(), 200, 1, Player.whoAmI);
                        Main.projectile[A].tileCollide = false;
                        Main.tile[(int)(Player.Center.X / 16), (int)(Player.Center.Y / 16)].LiquidAmount += 10;
                        WorldGen.SquareTileFrame((int)(Player.Center.X / 16), (int)(Player.Center.Y / 16), true);
                    }
                }
            }
            if (Player.HasBuff(ModContent.BuffType<CrimsonFury>()))
            {
                if (CrimsonTime < 0.5f)
                {
                    CrimsonTime += 0.25f;
                }
                for (int a = 0; a < 200; a++)
                {
                    NPC npc = Main.npc[a];
                    if (npc.CanBeChasedBy())
                    {
                        if (Main.rand.NextBool(10) && DDHelper.CircleInsertRectangle(npc.Hitbox, Player.Center, 128) && CrimsonTime2 <= 0)
                        {
                            CrimsonTime2 = 60;
                            NewProjectile(Player.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MeleeSuckBloodPlayer>(), 30, 0, Player.whoAmI, 0, -1);
                        }
                    }
                }
            }
            else
            {
                if (CrimsonTime > 0)
                {
                    CrimsonTime -= 0.1f;
                }
            }
            if (CrimsonTime2 > 0)
            {
                CrimsonTime2--;
            }
            DDHelper.MaxandMinF(ref CrimsonTime2, 999, 0);
            DDHelper.MaxandMinF(ref CrimsonTime, 999, 0);
            if (!DDSystem.BossSurvival)
            {
                Player.respawnTimer -= 3;
            }
            if (HeartbreakerTime > 0)
            {
                HeartbreakerTime--;
            }
            if (荆棘戒指 && 荆棘戒指CD > 0)
            {
                荆棘戒指CD--;
            }
            if ((永夜戒指 || 真永夜戒指 || 泰拉戒指) && 永夜戒指CD > 0)
            {
                永夜戒指CD--;
            }
            if ((神圣戒指 || 真神圣戒指 || 泰拉戒指))
            {
                if (神圣戒指CD > 0)
                {
                    神圣戒指CD--;
                    if (神圣戒指CD==0&&Main.myPlayer == Player.whoAmI)
                    {
                        DDmod.SyncData(DDType.PlayersSuit, Main.myPlayer, -1, Main.myPlayer);
                    }
                }
                else
                {
                    if (神圣戒指)
                        Player.AddBuff(ModContent.BuffType<守护>(), 2);
                    else if (真神圣戒指)
                        Player.AddBuff(ModContent.BuffType<真守护>(), 2);
                    else
                        Player.AddBuff(ModContent.BuffType<泰拉守护>(), 2);
                }
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                if (GlowingMushroomSet)
                {
                    if (Player.Dplayer().PlayerTimes % 60 == 0)
                    {
                        Player.statMana += 5;
                        Player.ManaEffect(5);
                    }
                }
            }
            if (PalladiumSet)
            {
                if (PalladiumSetCD <= 0)
                {
                    if (Player.whoAmI == Main.myPlayer)
                        Player.Heal(15);
                    NewDustChange5((int)(50), Player.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 1, 3, true, 2, 4, 100, 1000, new Color(247, 183, 51, 100), 5F, Player.whoAmI);
                    NewDustChange5((int)(50), Player.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 1, 3, true, 2, 4, 100, 1000, new Color(239, 90, 49, 100), 5F, Player.whoAmI);
                    for (int A = 0; A < 30; A++)
                    {
                        int num16 = Gore.NewGore(Player.GetSource_FromAI(), Player.position + new Vector2(0, Player.height / 2), Vector2.Zero, 331, (float)Main.rand.Next(40, 121) * 0.01f);
                        Main.gore[num16].sticky = false;
                        Main.gore[num16].velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.Next(300,1000) / 300;
                    }
                    PalladiumSetCD = 1800;
                }
                else if (PalladiumSetCD <= 50)
                {
                    if (神圣戒指CD == 50 && Main.myPlayer == Player.whoAmI)
                    {
                        DDmod.SyncData(DDType.PlayersSuit, Main.myPlayer, -1, Main.myPlayer);
                    }
                    if (PalladiumSetCD % 10 == 0)
                    {
                        if (Player.whoAmI == Main.myPlayer)
                            Player.Heal(3);
                    }
                    for (int a = 0; a < 2; a++)
                    {
                        int A = NewDust(Player.Center - new Vector2(4) + new Vector2(Main.rand.Next(100, 140), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(247, 183, 51, 100), Scale: 1.8F);
                        Main.dust[A].velocity = (Player.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(5, 7);
                        Main.dust[A].noGravity = true;
                        Main.dust[A].customData = 3.5f + Main.dust[A].DustAI(3);
                        GlobalDust.DustPlayerOwner[A] = Player.whoAmI;

                        A = NewDust(Player.Center - new Vector2(4) + new Vector2(Main.rand.Next(100, 140), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(239, 90, 49, 100), Scale: 1.8F);
                        Main.dust[A].velocity = (Player.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(5, 7);
                        Main.dust[A].noGravity = true;
                        Main.dust[A].customData = 3.5f + Main.dust[A].DustAI(3);
                        GlobalDust.DustPlayerOwner[A] = Player.whoAmI;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        int num16 = Gore.NewGore(Player.GetSource_FromAI(), Player.position + new Vector2(0, Player.height / 2), Vector2.Zero, 331, (float)Main.rand.Next(40, 121) * 0.01f);
                        Main.gore[num16].sticky = false;
                        Main.gore[num16].velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.Next(1000) / 500;
                    }
                }
            }

            if (RefinedGoldSetCD > 180)
            {
                NewDustSector(6, Player.position, Player.Size, new Vector3(-Player.velocity.PerfectNormalize(), 0.4F), ModContent.DustType<速度粒子>(), 1, 20, true, 2, 5, 100, 1000, new Color(255, 50, 50, 100));
                NewDustSector(6, Player.position, Player.Size, new Vector3(-Player.velocity.PerfectNormalize()/4, 0.4F), ModContent.DustType<光球粒子>(), 1, 20, true, 1, 2, 100, 1000, new Color(255, 50, 50, 100));
                Player.Aplayer().IgnoreWater = 3;
                Player.Aplayer().NoGravity = 3;
                Player.maxFallSpeed = 10000;
                if (!RefinedGoldSet)
                {
                    RefinedGoldSetCD = 180;
                }
            }
            if (RefinedGoldSet)
            {
                if (RefinedGoldSetCD == 1)
                {
                    NewDustChange4((int)(50), Player.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 3, 6, true, 1, 2, 100, 1000, new Color(255, 50, 50, 100), 5F);
                    NewDustChange4((int)(50), Player.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 5, 10, true, 3, 6, 100, 1000, new Color(255, 50, 50, 100), 5F);
                    SoundStyle sound = SoundID.Item25;
                    sound.Pitch = 0.5F;
                    SoundEngine.PlaySound(sound);
                }
            }
            if (RefinedGoldSetCD == 180)
            {
                if (Player.whoAmI == Main.myPlayer)
                {
                    DDmod.SyncData(DDType.PlayerCenter, Player.whoAmI, -1, Player.whoAmI);
                    DDmod.SyncData(DDType.PlayersSuit, Player.whoAmI, -1, Player.whoAmI);
                    Player.velocity = RefinedGoldSpeed;
                }
            }
        }
        public override void PostUpdate()
        {
            Player.gravDir = 1;
            if (!Player.HasBuff(ModContent.BuffType<力量之魂>()))
            {
                if (精金力量 > 1)
                {
                    Player.AddBuff(ModContent.BuffType<力量之魂>(), 30);
                    精金力量--;
                }
                else
                {
                    精金力量 = 0;
                }
            }
            if (精金力量 > 40)
            {
                精金力量 = 40;
            }
            LifeMax = Player.statLifeMax2;
            ManaMax = Player.statManaMax2;
            //Player.portableStoolInfo.SetStats(26, 26, 26);
        }
        public void Hit(NPC npc, HitInfo hit)
        {
            if (ShadowSet)
            {
                Player.AddBuff(ModContent.BuffType<Exposed>(), 600);
            }
            if (Heartbreaker && HeartbreakerTime <= 0 && Main.rand.NextBool(10))
            {
                NewProjectile(Player.GetSource_FromAI(), npc.Center, new Vector2(0, -7).RotatedBy(Main.rand.NextFloat(-1, 1)), ModContent.ProjectileType<Heartbreaker>(), 0, 0, Player.whoAmI);
                HeartbreakerTime = (300 - hit.Damage / 2) + 100;
            }
            if(CobaltSet&& CobaltSetCD==0)
            {
                CobaltSetCD = 180;
                NewProjectile(Player.GetSource_FromAI(), npc.Center, new Vector2(0, -7).RotatedBy(Main.rand.NextFloat(-1, 1)), ModContent.ProjectileType<钴爆炸>(), 120, 0, Player.whoAmI);
            }
        }
        int 荆棘戒指CD = 0;
        int 永夜戒指CD = 0;
        public short 神圣戒指CD = 0;
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            Hit(target, hit);
            if (FossilSet && Main.rand.NextBool(5))
            {
                int R = damageDone * 2 + 10;
                if(R>300)
                {
                    R = 300;
                }
                target.AddBuff(ModContent.BuffType<Fossil>(), R);
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            Hit(target, hit);
            if (FossilSet&&Main.rand.NextBool(5))
            {
                int R = damageDone * 2 + 10;
                if (R > 300)
                {
                    R = 300;
                }
                target.AddBuff(ModContent.BuffType<Fossil>(), R);
            }
        }
        public void HitBy(ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.Next(100) < Resist)
            {
                modifiers.ModifyHurtInfo += Modifiers_ModifyHurtInfo; ;
                void Modifiers_ModifyHurtInfo(ref Player.HurtInfo info)
                {
                    info.Damage /=2;
                }
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/格挡");
                sound.Pitch = 0;
                PlaySound(sound, Player.position);
                for (int a = 0; a < 10; a++)
                {
                    int A = NewDust(Player.position, Player.width, Player.height, ModContent.DustType<格挡粒子>());
                    Main.dust[A].velocity = (Main.dust[A].position - Player.Center).PerfectNormalize() * Main.rand.NextFloat(2, 3);
                    Main.dust[A].noGravity = true;
                }
                //int A = CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 1, 1), new Color(0, 162, 255, 0) * 0.8f, DDSystem.English ? "Resist!" : "格挡!", false, false);
                //Main.combatText[A].velocity = Vector2.Zero;
            }
        }


        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            HitBy(ref  modifiers);
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            HitBy(ref  modifiers);
        }
        public void OnHitBy(Player.HurtInfo hurtInfo)
        {
            if (荆棘戒指)
            {
                if (荆棘戒指CD <= 0)
                {
                    int T = Main.rand.Next(8, 11);
                    for (int a = 0; a < T; a++)
                    {
                        Vector2 vector = new Vector2(Main.rand.NextFloat(4, 10), 0).RotatedBy(MathHelper.TwoPi / T * a + Main.rand.NextFloat(MathHelper.TwoPi / T / 2));

                        NewProjectile(Player.GetSource_FromAI(), Player.MountedCenter, vector, ModContent.ProjectileType<毒刺>(), 25, 1, Player.whoAmI);
                    }
                    荆棘戒指CD = 120;
                }

            }
            if (永夜戒指)
            {
                if (永夜戒指CD <= 0)
                {
                    for (int a = 0; a < 200; a++)
                    {
                        NPC n = Main.npc[a];
                        Vector2 vector = n.Center - Player.Center;
                        if (n.CanBeChasedBy() && !n.buffImmune[ModContent.BuffType<永夜侵袭>()] && vector.Length() < 500)
                        {
                            NewProjectile(Player.GetSource_FromAI(), Player.MountedCenter, vector.PerfectNormalize() * 3, ModContent.ProjectileType<永夜>(), 0, 0, Player.whoAmI, a);
                        }
                    }
                    永夜戒指CD = 120;
                }
            }
            else if (真永夜戒指)
            {
                if (永夜戒指CD <= 0)
                {
                    for (int a = 0; a < 200; a++)
                    {
                        NPC n = Main.npc[a];
                        Vector2 vector = n.Center - Player.Center;
                        if (n.CanBeChasedBy() && !n.buffImmune[ModContent.BuffType<真永夜侵袭>()] && vector.Length() < 500)
                        {
                            NewProjectile(Player.GetSource_FromAI(), Player.MountedCenter, vector.PerfectNormalize() * 3, ModContent.ProjectileType<永夜>(), 0, 0, Player.whoAmI, a,1);
                        }
                    }
                    永夜戒指CD = 120;
                }
            }
            else if (泰拉戒指)
            {
                if (永夜戒指CD <= 0)
                {
                    for (int a = 0; a < 200; a++)
                    {
                        NPC n = Main.npc[a];
                        Vector2 vector = n.Center - Player.Center;
                        if (n.CanBeChasedBy() && !n.buffImmune[ModContent.BuffType<泰拉侵袭>()] && vector.Length() < 500)
                        {
                            NewProjectile(Player.GetSource_FromAI(), Player.MountedCenter, vector.PerfectNormalize() * 3, ModContent.ProjectileType<泰拉>(), 0, 0, Player.whoAmI, a,1);
                        }
                    }
                    永夜戒指CD = 120;
                }
            }
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            OnHitBy(hurtInfo);
            if (荆棘戒指)
            {
                int D = -1;
                if (npc.position.X + (float)(npc.width / 2) < Player.position.X + (float)(Player.width / 2))
                    D = 1;
                Player.ApplyDamageToNPC(npc, hurtInfo.Damage*2, 2, -D, crit: false);
                npc.AddBuff(20, 600);
            }
        }
        public override void ModifyHitNPC(NPC target, ref HitModifiers modifiers)
        {
            modifiers.CritDamage += Player.Dplayer().CritDamage;
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            OnHitBy(hurtInfo);
        }
        public override void PostHurt(Player.HurtInfo info)
        {
            if (Crimson)
            {
                AddBuffTime(ModContent.BuffType<CrimsonFury>(), info.Damage * 5);
            }
            if (GhostfangBracelet)
            {
                AddBuffTime(ModContent.BuffType<鬼牙眷顾>(), info.Damage * 4);
            }
            if (ServantOfTheHeart)
            {
                Player.AddBuff(ModContent.BuffType<心灵治愈>(), 300);
            }
            if (绿岩套 == 2 && info.Damage >= 15 && Main.myPlayer == Player.whoAmI)
            {
                for (int a = 0; a < info.Damage / 15; a++)
                {
                    int proj = NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(0, -10).RotatedBy(Main.rand.NextFloat(-1,1)), ModContent.ProjectileType<绿岩导弹>(), 40, 1, -1, 0);
                    Main.projectile[proj].scale = 0.5F;
                }
            }
            if (HolyShield > 0 && Player.TPlayer().Shield <= 0)
                HolyShield = 0;
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (UndeadSet || UndeadSet2)
            {
                if (damage < Player.statLifeMax2 && !Player.HasBuff(ModContent.BuffType<不死枯竭>()))
                {
                    Player.statLife = 1;
                    Player.AddBuff(ModContent.BuffType<不死枯竭>(), 3600);
                    if (UndeadSet2)
                        Player.AddBuff(ModContent.BuffType<死灵恩赐>(), 600);
                    playSound = false;
                    SoundStyle sound = SoundID.NPCHit2;
                    sound.Pitch = -2;
                    PlaySound(sound,Player.Center);
                    if (Main.myPlayer==Player.whoAmI)
                    {
                        for(int A=0;A<12;A++)
                        {
                            NewProjectile(Player.GetSource_FromAI(),Player.Center,Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*Main.rand.Next(300,1000)/100+new Vector2(0,-4), 21,60,3);
                        }
                    }
                    return false;
                }
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }
        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            return base.FreeDodge(info);
        }
        public override bool ConsumableDodge(Player.HurtInfo info)
        {
            if (GuardianOfTheStarCD > 6000 || (GuardianOfTheStar2 && GuardianOfTheStarCD > 3000))
            {
                GuardianOfTheStarCD = -30;
                Player.immune = true;
                Player.hurtCooldowns[1] = 60;
                Player.immuneTime = 60;
                return true;
            }
            if (神圣戒指)
            {
                if (神圣戒指CD <= 0)
                {
                    Player.Heal(1+(int)(info.Damage*0.1F));
                    神圣戒指CD = (short)DDHelper.Second(180);
                    Player.immune = true;
                    Player.hurtCooldowns[1] = 60;
                    Player.immuneTime = 60;
                    for(int a= 0;a<50;a++)
                    {
                        NewDust(Player.position,Player.width,Player.height,ModContent.DustType<星光粒子>(),0,0,100,new Color(255, 216, 0,0),Main.rand.NextFloat(0.6F,1.2F));
                    }
                    return true;
                }
            }
            else if (真神圣戒指)
            {
                if (神圣戒指CD <= 0)
                {
                    Player.Heal(1+(int)(info.Damage * 0.3F));
                    神圣戒指CD = (short)DDHelper.Second(180);
                    Player.immune = true;
                    Player.hurtCooldowns[1] = 60;
                    Player.immuneTime = 60;
                    for (int a = 0; a < 50; a++)
                    {
                        NewDust(Player.position, Player.width, Player.height, ModContent.DustType<星光粒子>(), 0, 0, 100, Main.rand.Next(new Color[] { new Color(255, 216, 0, 0) ,new Color(255, 85, 116, 0),new Color(25,238,238, 0) }), Main.rand.NextFloat(0.6F, 1.2F));
                    }
                    return true;
                }
            }
            else if (泰拉戒指)
            {
                if (神圣戒指CD <= 0)
                {
                    Player.Heal(1+(int)(info.Damage * 0.5F));
                    神圣戒指CD = (short)DDHelper.Second(180);
                    Player.immune = true;
                    Player.hurtCooldowns[1] = 60;
                    Player.immuneTime = 60;
                    for (int a = 0; a < 50; a++)
                    {
                        NewDust(Player.position, Player.width, Player.height, ModContent.DustType<星光粒子>(), 0, 0, 100,new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 12), Main.rand.NextFloat(1.2F, 1.8F));
                    }
                    return true;
                }
            }
            /*
            if (Player.TPlayer().Shield <= 0)
            {
                if (HolyShield > 0)
                {
                    for (int a = 0; a < 50; a++)
                    {
                        NewDust(Player.position, Player.width, Player.height, 57, 0, 0, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 12), Main.rand.NextFloat(0.5F, 1F));
                    }
                    return true;
                }
                else
                {
                    for (int a = 0; a < 50; a++)
                    {
                        NewDust(Player.position, Player.width, Player.height, 57, 0, 0, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 12), Main.rand.NextFloat(0.5F, 1F));
                    }
                    HolyShield = 0;
                }
            }*/
            return base.ConsumableDodge(info);
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (ShadowSet)
            {
                Player.AddBuff(ModContent.BuffType<Exposed>(), 600);
            }
            if (HolyShield > 0&&Player.TPlayer().Shield<=0)
            {
                modifiers.SourceDamage -= HolyShield/500;
                for (int a = 0; a < 150; a++)
                {
                   int A= NewDust(Player.position, Player.width, Player.height, 57, 0, 0, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 12), Main.rand.NextFloat(0.5F, 1F));
                    Main.dust[A].velocity = new Vector2(1,0).RotatedBy(MathHelper.TwoPi/50*a);
                    Main.dust[A].velocity *= Main.rand.Next(1, 5);
                }
            }
        }
        public float CrimsonTime;
        public float CrimsonTime2;
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
        }
        public override void ModifyDrawLayerOrdering(IDictionary<PlayerDrawLayer, Position> positions)
        {
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (Player.HasBuff(ModContent.BuffType<CrimsonFury>()))
            {
                r = 1;
                g = 0.1F;
                b = 0.1F;
            }
            if (Player.HasBuff(BuffID.Slimed))
            {
                r = 0;
                g = 0.6F;
                b = 1F;
            }
        }
    }
    public class MagicStarCircleLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.HandOnAcc);
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            AttributesPlayer modPlayer = drawPlayer.Aplayer();
            return drawInfo.shadow == 0f && !drawPlayer.dead && (modPlayer.GuardianOfTheStarCD > 6000 || modPlayer.GuardianOfTheStarCD < 0||(modPlayer.GuardianOfTheStar2 && modPlayer.GuardianOfTheStarCD > 3000)) && modPlayer.GuardianOfTheStar;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Texture2D texture = ModContent.Request<Texture2D>("DDmod/Image/MagicStarCircle", AssetRequestMode.AsyncLoad).Value;
            Vector2 drawPos = drawPlayer.Center - Main.screenPosition + new Vector2(0f, drawPlayer.gfxOffY);
            Color color = new Color(0, 100, 255, 0)*0.25f;
            Vector2 origin = new(texture.Width / 2f, texture.Height / 2f);
            float scale = 1f;
            if (drawPlayer.Aplayer().GuardianOfTheStarCD < 0)
            {
                color *= (float)-drawPlayer.Aplayer().GuardianOfTheStarCD / 30;
                scale += (1 - (float)-drawPlayer.Aplayer().GuardianOfTheStarCD / 30);
            }
            SpriteEffects spriteEffects = (SpriteEffects)((drawPlayer.direction != -1) ? 0 : 1);
            drawInfo.DrawDataCache.Add(new DrawData(texture, drawPos, null, color, (float)drawPlayer.Aplayer().GuardianOfTheStarCD / 20, origin, scale, spriteEffects, 0));
            drawInfo.DrawDataCache.Add(new DrawData(texture, drawPos, null, color, (float)drawPlayer.Aplayer().GuardianOfTheStarCD / 20, origin, scale, spriteEffects, 0));
            drawInfo.DrawDataCache.Add(new DrawData(texture, drawPos, null, color, (float)drawPlayer.Aplayer().GuardianOfTheStarCD / 20, origin, scale, spriteEffects, 0));

            texture = DDTextures.VoidStar.Value;
            origin = new(texture.Width / 2f, texture.Height / 2f);
            drawInfo.DrawDataCache.Add(new DrawData(texture, drawPos, null, color, (float)drawPlayer.Aplayer().GuardianOfTheStarCD / 20, origin, scale, spriteEffects, 0));
        }
    }
}
