
using DDmod.Content.Items.Pet;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.ID;
using static Terraria.Player;
using StructureHelper;
using DDmod.UI.HunterQuests;
using DDmod.SubworldLibraryWorld;
using DDmod.SubworldLibraryWorld.草原;
using DDmod.UI.ItemUI.背包;
using DDmod.Helper;
using DDmod.Content.NPCs.Boss.狱火蛇;
using Microsoft.Xna.Framework.Input;
using DDmod.Content.Items.农场;
using static Terraria.Collision;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.NPCs.Boss.StarGuardBulan;
using Terraria.WorldBuilding;
using Terraria.GameContent.UI;
using Terraria.GameContent.Events;
using System.Reflection;
using DDmod.Content.Dusts;
using log4net.Core;
using DDmod.Content.Items.Boss.海幽浮王;
using DDmod.Content.NPCs.Boss.海幽浮王;
using Terraria.Map;
using DDmod.Content.Items;
using System.Linq;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Melee.SwordShield;
using Terraria.ModLoader.Core;
using DDmod.Content.Projectiles.Pet.MasterPet;
using DDmod.Content.Projectiles.BattlePets;
using DDmod.Content.Items.Melee.Sword;
using static DDmod.Content.NPCs.Boss.LifeGuardLes.LifeGuard;
using DDmod.UI.BattlePetUI.技能;
using Microsoft.CodeAnalysis.Differencing;
using System;
using DDmod.Content.Projectiles.GeneralProj;
using Terraria.ObjectData;
using DDmod.Content.Tiles.EquipTiles;
using Terraria.ModLoader;
using DDmod.Content.Tiles;
using Terraria.UI.Gamepad;
using DDmod.Content.Items.Sundries;
using static AssGen.Assets;
using Terraria.Graphics.Capture;
using DDmod.Content.Items.Boss.天雷怒云;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Mounts;
using DDmod.Content.Projectiles.BattlePets.Proj;
using DDmod.Content.Achievements;
using DDmod.Content.Items.Boss.Boss特殊;
using Terraria.IO;
using Terraria.UI;
namespace DDmod.Players
{
    //限量购买引用
    public class PlayerShop
    {
        public int ItemType;
        public int Stack;
        public int MaxStack;
        public PlayerShop(int type, int stack)
        {
            ItemType = type;
            MaxStack = stack;
            Stack = MaxStack;
        }

        public void Save(TagCompound tag, int T)
        {
            tag.Add("PlayerStack" + T, Stack);
        }

        // 加载逻辑
        public void Load(TagCompound tag, int T)
        {
            Stack = tag.GetInt("PlayerStack" + T);
        }
    }
    public class DDPlayer : ModPlayer
    {
        public void Sync(byte Type)
        {
            Mod mod = DDmod.Instance;
            ModPacket packet = mod.GetPacket(256);
            packet.Write((byte)DDType.PlayersWorld);
            packet.Write((byte)Player.whoAmI);
            packet.Write(Type);
            packet.Write(NPCDowned.绿岩刷怪);
            packet.Write(DDWorld.诅咒之火);
            packet.Send(-1, Player.whoAmI);
        }
        public BattlePets[] Bpets = new BattlePets[20];
        public int FightPets = -1;
        public int ownedFightPets = -1;
        public int Itemslot = 0;
        public Vector2 FightPetsUIPo = Vector2.Zero;
        public float HA = 0;
        public float HA2 = 0;
        /// <summary>
        /// 该不该右键使用武器
        /// </summary>
        public bool NoRight => Main.myPlayer == Player.whoAmI && (Player.tileInteractionHappened || Player.mouseInterface || CaptureManager.Instance.Active || Main.HoveringOverAnNPC || Main.SmartInteractShowingGenuine);

        /// <summary> 流星配方 </summary>
        public bool MeteorRecipe = false;
        /// <summary> 绿岩配方 </summary>
        public bool GreenstoneRecipe = false;
        /// <summary> 绿岩开关 </summary>
        public bool GreenstoneOrgan = false;

        /// <summary> 启用盾牌 </summary>
        public byte ShieldDefense = 0;
        /// <summary> 启用盾牌时间 </summary>
        public byte ShieldDefenseTime = 0;
        /// <summary> 启用盾牌CD </summary>
        public byte ShieldCD = 0;
        /// <summary> 启用盾牌CD </summary>
        public float ShieldEndurance = 0;
        /// <summary> 格挡成功后强化攻击 </summary>
        public byte Block = 0;
        /// <summary> 控制屏幕的弹幕 </summary>
        public int control = -1;
        /// <summary> 是否绘制玩家 </summary>
        public bool PDraw;
        /// <summary> 禁止攻击(帧) </summary>
        public int ForbiddenToAttack;
        /// <summary> 闪避机会 </summary>
        public float DodgeChance;
        /// <summary> 一个用于增长的变量() </summary>
        public float Rotate;
        /// <summary> 提升最大魔力上限 </summary>
        public int PlayerMana;
        /// <summary>
        /// 临时弹药
        /// </summary>
        public int Ammo;
        /// <summary> 隐藏玩家 </summary>
        public bool Hide;
        public int Hide2;
        /// <summary> 玩家计时器(多用途) </summary>
        public float PlayerTimes;
        /// <summary> 飞行时间 </summary>
        public float wingTimeMax;
        /// <summary> 飞行时间 </summary>
        public int wingTimeMax2;
        /// <summary> 暴击伤害 </summary>
        public float CritDamage = 0;
        /// <summary> 受击给予buff </summary>
        public List<int> HitBuff = new List<int>();
        /// <summary> 哥布林伤害 </summary>
        public float GoblinsEndurance;

        /// <summary> 吸血CD </summary>
        public int VampireCD;
        /// <summary> 领域 </summary>
        public int Realm;

        /// <summary> 生命蘑菇 </summary>
        public bool MushroomsLife;
        /// <summary> 魔法菇 </summary>
        public bool MushroomsMana;

        /// <summary> 鼠标 </summary>
        public Vector2 MouseWorld;
        public float swingAngle;
        public float gfxOffY;
        /// <summary>
        /// 近战大小
        /// </summary>
        public float MeleeScale;
        /// <summary> 延迟计时器 </summary>
        public int DelayTime;
        public int DelayTime2;
        /// <summary> 当前延迟 </summary>
        public int Delay;
        /// <summary> 魔力上限 </summary>
        public int statManaMax;
        /// <summary> 传送的地方 </summary>
        public Vector2 PreLocation;
        /// <summary> 玩家播放音乐 </summary>
        public int Music = -1;
        /// <summary> 由先播放 </summary>
        public bool MusicVital = false;
        /// <summary> 幽灵 </summary>
        public bool phantom = false;
        /// <summary> 奇幻花 </summary>
        public bool FantasyFlowers = false;
        /// <summary> 手持动画 </summary>
        public bool ProjAnimation = false;
        /// <summary> 召唤能量 </summary>
        public int SummonEnergy;

        /// <summary> 距离上一步的位置 </summary>
        public Vector2 PrePosition;
        /// <summary>
        /// 快走
        /// </summary>
        public bool Speed = false;
        /// <summary>
        /// 跑步
        /// </summary>
        public bool MaxSpeed = false;
        /// <summary>
        /// 禁止生成怪物
        /// </summary>
        public bool ForbidSpawnNPC = false;

        public int ManaKao;
        public int LifeKao;
        /// <summary>
        /// 委托币
        /// </summary>
        public int AdventureCoins;
        public int AdventureCoins2;
        public int AdventureCoins3;
        public int AdventureCoins4;
        public int AdventureCoins5;
        public static bool BowUse;
        public override void OnEnterWorld()
        {
            base.OnEnterWorld();
        }
        public byte ForbidSpawn = 10;
        public override void ResetEffects()
        {
            HitBuff = new List<int>();
            wingTimeMax = 0;
            wingTimeMax2 = 0;
            CritDamage = 0;
            if (ForbiddenToAttack > 0)
            {
                Player.noItems = true;
                ForbiddenToAttack--;
            }
            if (!Player.HasBuff(ModContent.BuffType<星心考验>()))
            {
                ManaKao = 0;
                LifeKao = 0;
            }
            DodgeChance = 0;
            PlayerMana = 0;
            if (VampireCD > 0)
            {
                VampireCD--;
            }
            else
            {
                VampireCD = 0;
            }
            //领域
            if (Realm > 0)
            {
                Realm--;
            }
            else
            {
                Realm = 0;
            }
            Player.Dplayer().ForbidSpawnNPC = ForbidSpawn != 0;
            if (ForbidSpawn > 0)
                ForbidSpawn--;

            GoblinsEndurance = 0;
            MeleeScale = 1;
            phantom = false;
            if (Player.ActiveItem().holdStyle == 0 || Player.ActiveItem().fishingPole == 0)
                HYF = false;
        }
        private HurtTile GetHurtTile()
        {
            HurtTile result = Collision.HurtTiles(Player.position, Player.width, (!Player.mount.Active || !Player.mount.Cart) ? Player.height : (Player.height - 16), Player);
            if (result.type >= 0)
                return result;

            foreach (Point touchedTile in Player.TouchedTiles)
            {
                Tile tile = Main.tile[touchedTile.X, touchedTile.Y];
                if (tile != null && tile.HasTile && tile.HasTile && !TileID.Sets.Suffocate[tile.TileType] && Collision.CanTileHurt(tile.TileType, touchedTile.X, touchedTile.Y, Player))
                {
                    Collision.HurtTile result2 = default;
                    result2.type = tile.TileType;
                    result2.x = touchedTile.X;
                    result2.y = touchedTile.Y;
                    return result2;
                }
            }

            return result;
        }
        public bool Map = false;
        public Mini_game Mini_game_shortcuts = new Mini_game();
        public class Mini_game
        {
            /// <summary> 小游戏启动中 </summary>
            public bool Enable = false;
            /// <summary> 上 </summary>
            public bool Up = false;
            public int UpTime;
            /// <summary> 下 </summary>
            public bool Down = false;
            public int DownTime;
            /// <summary> 左 </summary>
            public bool Left = false;
            public int LeftTime;
            /// <summary> 右 </summary>
            public bool Right = false;
            public int RightTime;
            /// <summary> 跳 </summary>
            public bool Jump = false;
            public int JumpTime;
            /// <summary>
            /// 退出游戏
            /// </summary>
            public bool Exit = false;
            public void ResetControls()
            {
                Up = false;
                Down = false;
                Left = false;
                Right = false;
                Exit = false;
                Jump = false;
            }
            public void SetControls(Player Player)
            {
                Up = Player.controlUp;
                if (Up)
                {
                    UpTime++;
                }
                else UpTime = 0;
                Down = Player.controlDown;
                if (Down)
                {
                    DownTime++;
                }
                else DownTime = 0;
                Left = Player.controlLeft;
                if (Left)
                {
                    LeftTime++;
                }
                else LeftTime = 0;
                Right = Player.controlRight;
                if (Right)
                {
                    RightTime++;
                }
                else RightTime = 0;
                Exit = Player.controlInv;
                Jump = Player.controlJump;
                if (Jump) { JumpTime++; } else JumpTime = 0;
            }
        }


        public override void SetControls()
        {
            if (Mini_game_shortcuts.Enable)
            {
                Mini_game_shortcuts.ResetControls();
                Mini_game_shortcuts.SetControls(Player);
                ResetControls();
                Mini_game_shortcuts.Enable = false;
            }
            base.SetControls();
        }
        void ResetControls()
        {
            Player.controlUp = false;
            Player.controlLeft = false;
            Player.controlDown = false;
            Player.controlRight = false;
            Player.controlJump = false;
            Player.controlUseItem = false;
            Player.controlUseTile = false;
            Player.controlThrow = false;
            Player.controlInv = false;
            Player.controlHook = false;
            Player.controlTorch = false;
            Player.controlSmart = false;
            Player.controlMount = false;
            Player.controlQuickHeal = false;
            Player.controlQuickMana = false;
            Player.controlCreativeMenu = false;
            Player.mapStyle = false;
            Player.mapAlphaDown = false;
            Player.mapAlphaUp = false;
            Player.mapFullScreen = false;
            Player.mapZoomIn = false;
            Player.mapZoomOut = false;
        }

        public static bool UseBoomerang(Item sItem, Player player)
        {
            if (sItem.type > 0 && sItem.DItem().Boomerang > 0)
            {
                if (player.ownedProjectileCounts[sItem.shoot] >= sItem.DItem().Boomerang)
                {
                    return false;
                }
            }
            return true;
        }
        public override void Load()
        {
            BattlePets.Load();
            if (shops == null || shops.Length == 0)
            {
                shops = [new PlayerShop(2267, 20), new PlayerShop(2268, 20), new PlayerShop(ModContent.ItemType<白色委托币>(), 15), new PlayerShop(ModContent.ItemType<绿色委托币>(), 15), new PlayerShop(ModContent.ItemType<蓝色委托币>(), 15)];
            }
        }
        //海幽浮王上钩了其他钩子不能上
        public bool HYF = false;
        public override bool? CanConsumeBait(Item bait)
        {
            if (HYF)
            {
                HYF = false;
                return true;
            }
            return null;
        }

        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            int baitItemType = attempt.playerFishingConditions.BaitItemType;
            if (baitItemType == ModContent.ItemType<秘制大虾>())
            {
                if (Main.rand.NextBool(4) && (attempt.X < 380 || attempt.X > Main.maxTilesX - 380))
                {
                    int a = Main.rand.Next(new int[] { 2436, 2437, 2438 });
                    itemDrop = a;
                }
                if ((attempt.X < 380 || attempt.X > Main.maxTilesX - 380) && attempt.waterTilesCount > 1000 && !HYF && !NPC.AnyNPCs(ModContent.NPCType<海幽浮王>()))
                {
                    HYF = true;
                    Player.displayedFishingInfo = Language.GetTextValue("GameUI.FishingWarning");
                    npcSpawn = ModContent.NPCType<海幽浮王>();
                    itemDrop = 0;
                    int num = -1;
                    for (int i = 54; i < 58; i++)
                    {
                        if (Player.inventory[i].stack > 0 && Player.inventory[i].bait > 0)
                        {
                            num = i;
                            break;
                        }
                    }

                    if (num == -1)
                    {
                        for (int j = 0; j < 50; j++)
                        {
                            if (Player.inventory[j].stack > 0 && Player.inventory[j].bait > 0)
                            {
                                num = j;
                                break;
                            }
                        }
                    }

                    if (num <= -1)
                        return;

                    Item item = Player.inventory[num];
                    if (CombinedHooks.CanConsumeBait(Player, item) ?? true)
                    {
                        item.stack--;
                        if (item.stack <= 0)
                            item.SetDefaults();
                    }
                }
                return;
            }
        }
        public Point16 STPosition;

        public PlayerShop[] shops = [new PlayerShop(2267, 20), new PlayerShop(2268, 20),
            new PlayerShop(ModContent.ItemType<白色委托币>(), 15), new PlayerShop(ModContent.ItemType<绿色委托币>(), 15), new PlayerShop(ModContent.ItemType<蓝色委托币>(), 15)];

        public override bool CanBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            return true;
        }
        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            Item item2 = shopInventory[Itemslot];
            for (int S = 0; S < shops.Length; S++)
            {
                bool BB = false;

                if (item.type == shops[S].ItemType && item2.shopSpecialCurrency == -1)
                {
                    Main.shopSellbackHelper.Remove(item2);

                }
                if (item2.type == shops[S].ItemType)
                {
                    if (!item2.buyOnce && item2.shopSpecialCurrency == -1)
                    {
                        shops[S].Stack--;
                        item2.stack = shops[S].Stack;
                        if (item2.stack <= 0)
                        {
                            item2.type = 0;
                            BB = true;
                        }
                    }
                }
                for (int A = Itemslot; A < shopInventory.Length; A++)
                {
                    if (BB && A < shopInventory.Length - 1)
                    {
                        shopInventory[A] = shopInventory[A + 1];
                    }
                }
            }
        }
        public override void ModifyZoom(ref float zoom)
        {
            if (Player.inventory[Player.selectedItem].type > 0 && Player.inventory[Player.selectedItem].DItem().Sniper && Main.mouseRight && !Main.SmartCursorIsUsed)
            {
                if (Player.scope)
                {
                    zoom = 1 / 1.25f;
                }
                else
                {
                    zoom = 1 / 1.5f;
                }
            }
            if (Main.SmartCursorIsUsed)
            {
                zoom = 0;
            }
        }
        public override void PostUpdate()
        {
            if (Player.HasBuff(ModContent.BuffType<怒云之子Buff>()))
            {
                Player.position.Y = Player.oldPosition.Y;
                Player.velocity.Y = 0.1F;
            }
            // if (Player.ownedProjectileCounts[ModContent.ProjectileType<魔法围巾>()] == 0)
            {
                // NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<魔法围巾>(), 4, 0, -1);
            }
            //TileID.Sets.TouchDamageBleeding[0] = true;
            ItemID.Sets.StaffMinionSlotsRequired[ModContent.ItemType<风暴召唤杖>()] = Player.ownedProjectileCounts[ModContent.ProjectileType<风暴雨云>()] == 0 ? 1 : 0;
            ItemID.Sets.StaffMinionSlotsRequired[ModContent.ItemType<毁灭控制器>()] = Player.ownedProjectileCounts[ModContent.ProjectileType<机械蠕虫>()] == 0 ? 1 : 0;
            if (ShieldDefense > 0)
            {
                ShieldDefense--;
                /*
                if(ShieldDefense==0)
                {
                    Main.NewText("保存");
                    Player.SavePlayer(Main.ActivePlayerFileData);
                }*/
            }
            if (!SWSystem.TrueSubworld && PreLocation != Vector2.Zero && !HunterQuestsUI.Visible)
            {
                Player.position = PreLocation;
                PreLocation = Vector2.Zero;
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                if (STPosition != Point16.Zero)
                {
                    TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
                    StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>((int)STPosition.X, (int)STPosition.Y, tileData.Width, tileData.Height, 18);
                    if (tepowerCellFactory == null || !tepowerCellFactory.Start)
                    {
                        if (tepowerCellFactory != null)
                        {
                            if (!Main.playerInventory && StrengtheningUI.Visible)
                            {
                                Main.LocalPlayer.Dplayer().STPosition = Point16.Zero;
                                DDmod.SyncData(DDType.PlayerData, Main.myPlayer, -1, Main.myPlayer);
                            }
                            Main.playerInventory = true;
                            StrengtheningUI.Visible = true;
                            ModContent.GetInstance<强化台>().Condition.Complete();
                            StrengtheningUI.Location = new Vector2(tepowerCellFactory.Position.X, tepowerCellFactory.Position.Y) * 16;
                            StrengtheningUI.Tile = tepowerCellFactory.Position;
                        }
                        else
                        {
                            STPosition = Point16.Zero;
                            DDmod.SyncData(DDType.PlayerData, Player.whoAmI, -1, Player.whoAmI);
                        }
                    }
                }
                else
                {
                    StrengtheningUI.Visible = false;
                }
            }
            //HurtTile hurtTile = GetHurtTile();
            //if (hurtTile.type >= 0)
            /*
            if (Main.playerInventory)
            {
                if (Main.mapStyle == 1)
                {
                    Map = true;
                    Main.mapStyle = 0;
                }
            }
            else
            {
                if (Map)
                {
                    Main.mapStyle = 1;
                    Map = false;
                }
            }*/
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.ActiveItem().type > 0 && Player.ActiveItem().GetGlobalItem<MeleeGlobalItem>().SwordandShield)
            {
                Player.statDefense += Player.ActiveItem().defense;
            }
            if (Block > 0)
            {
                Block--;
            }
            if (ShieldCD > 0)
            {
                ShieldCD--;
            }
            if (ShieldDefense > 0)
            {
                if (ShieldDefenseTime < 30)
                {
                    ShieldDefenseTime++;
                }
                if (Math.Abs(Player.velocity.X) > 1)
                {
                    Player.velocity.X *= 0.92F;
                }
                if (Player.velocity.Y < -1)
                {
                    Player.position.Y -= Player.velocity.Y * 0.8F;
                }
                if (Player.Dplayer().ShieldDefense > 0)
                {
                    Player.statDefense += Player.ActiveItem().defense * 2;
                }
            }
            else
            {
                ShieldDefenseTime = 0;
            }
            Player.GetDamage(DamageClass.Summon) += Player.GetTotalAttackSpeed(DamageClass.Summon) - 1;
            Player.GetDamage(DamageClass.SummonMeleeSpeed) -= Player.GetTotalAttackSpeed(DamageClass.Summon) - 1;
            Player.statManaMax2 += PlayerMana + statManaMax;
            if (FantasyFlowers)
            {
                Player.manaCost *= 0.95f;
            }
            Player.wingTimeMax += wingTimeMax2;
            Player.wingTimeMax += (int)(Player.wingTimeMax * wingTimeMax);

            if (SWSystem.TrueSubworld)
            {
                Player.tileRangeX = 3;
                Player.tileRangeY = 3;
            }
            if ((!NPCDowned.downedStarGuard2) && AnyNPCs(ModContent.NPCType<StarGuard>()))
            {
                Player.statMana = 200;
                Player.statManaMax2 = 200;
            }
            if (Player.meleeScaleGlove)
            {
                Player.Dplayer().MeleeScale += 0.1F;
            }
            if (Player.endurance > 0.8F)
            {
                Player.endurance = 0.8F;
            }
            PrePosition = Player.position - Player.oldPosition;

        }
        public override void ModifyHitByNPC(NPC npc, ref HurtModifiers modifiers)
        {
            if (npc.Dnpc().Goblins)
            {
                modifiers.FinalDamage *= 1 - GoblinsEndurance;
            }

            if (ShieldDefenseTime < 30 && ShieldDefense > 0)
            {
                if (npc.velocity.Length() > 2 || Player.velocity.Length() > 2 || Player.velocity.Length() < npc.velocity.Length())
                {
                    NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<格挡粒子Proj>(), 0, 0, -1);
                    float D = npc.Center.X - Player.Center.X;
                    modifiers.FinalDamage *= (1F - ShieldEndurance);
                    if (npc.type != 113)
                    {
                        npc.velocity -= npc.velocity * 1.5f;
                    }
                    npc.SimpleStrikeNPC(Player.GetWeaponDamage(Player.ActiveItem()), D > 0 ? 1 : -1, false, 8, Player.ActiveItem().DamageType, true);
                    npc.netUpdate = true;
                    Block = 180;
                }
            }

            if (phantom && Main.rand.NextBool(10))
            {
                modifiers.ModifyHurtInfo += Modifiers_ModifyHurtInfo;
                void Modifiers_ModifyHurtInfo(ref HurtInfo info)
                {
                    if (info.Damage >= 40)
                    {
                        for (int a = 0; a < 5; a++)
                        {
                            NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(1).RotatedBy(Main.rand.NextFloat(0, MathHelper.Pi)), 177, 100, 2, -1);
                        }
                    }
                }
                modifiers.FinalDamage *= 0.2f;
            }
        }
        public override void PostHurt(HurtInfo info)
        {
            if (Block > 170)
            {
                Player.immune = true;
                Player.immuneTime = 80;
            }

            if (Player.HasBuff(ModContent.BuffType<魂怒Buff>()) && FightPets >= 0)
            {
                BattlePets pets = Bpets[FightPets];
                if (pets != null && pets.Type == Players.BattlePets.牢中灵骷 && pets.Variant == 5)
                {
                    SoundStyle sound = SoundID.Item14;
                    sound.Pitch = -1.5F;
                    PlaySound(sound, Player.Center);
                    for (int A = 0; A < 4; A++)
                    {
                        Dust dust = Main.dust[NewDust(Player.Center + new Vector2(-4, 16), 1, 1, ModContent.DustType<光圈粒子>(), 0, 0, 0, new Color(148, 43, 43, 0))];
                        dust.noGravity = true;
                        dust.scale = 0.1f;
                        dust.alpha = -5;
                        dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                        dust.velocity = Vector2.Zero;
                        dust.noLightEmittence = false;
                        dust.customData = new Vector4(0.2F, 40, 0, 1F);
                    }
                    if (ownedFightPets >= 0)
                    {
                        for (int a = 0; a < 8; a++)
                            NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(Main.rand.NextFloat(4, 8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<夺命鬼魂>(), Main.projectile[ownedFightPets].damage, 3, -1, Main.rand.NextFloat(4, 8));
                    }
                }
                Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<魂怒Buff>())] = 1;
            }

            ShieldCD = 60;
        }
        public override void ModifyHitByProjectile(Projectile proj, ref HurtModifiers modifiers)
        {
            if (ShieldDefenseTime < 30 && ShieldDefense > 0)
            {
                NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<格挡粒子Proj>(), 0, 0, -1);
                proj.penetrate--;
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
                Vector2 D = proj.Center - Player.Center;
                modifiers.FinalDamage *= (1F - ShieldEndurance);
                Block = 180;
            }
            if (phantom && Main.rand.NextBool(10))
            {
                modifiers.ModifyHurtInfo += Modifiers_ModifyHurtInfo;
                void Modifiers_ModifyHurtInfo(ref HurtInfo info)
                {
                    if (info.Damage >= 40)
                    {
                        for (int a = 0; a < 5; a++)
                        {
                            NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(1).RotatedBy(Main.rand.NextFloat(0, MathHelper.Pi)), 177, 100, 2, -1);
                        }
                    }
                }
                modifiers.FinalDamage *= 0.2f;
            }
        }

        public override void OnHitByNPC(NPC npc, HurtInfo hurtInfo)
        {
            for (int a = 0; a < HitBuff.Count; a++)
            {
                npc.AddBuff(HitBuff[a], 300);
                if (HitBuff[a] == 24)
                {
                    NewProjectile(Player.GetSource_FromAI(), npc.Center, Vector2.Zero, 978, 70, 2, -1);
                    NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, 978, 70, 2, -1);
                }
            }
        }
        public override void OnHitByProjectile(Projectile proj, HurtInfo hurtInfo)
        {
            for (int a = 0; a < HitBuff.Count; a++)
            {
                if (HitBuff[a] == 24)
                {
                    NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, 978, 70, 2, -1);
                }
            }
        }
        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (Player == null)
            {
                return;
            }
            if (Hide2 > 0)
            {
                foreach (PlayerDrawLayer draw in PlayerDrawLayerLoader.Layers)
                {
                    if (draw != PlayerDrawLayers.HeldItem)
                        draw.Hide();
                }
                Hide2--;
            }
        }
        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            return null;
        }
        public long dpsDamage = 0;
        public long getDPS()
        {

            TimeSpan timeSpan = Player.dpsEnd - Player.dpsStart;
            float num = (float)timeSpan.Milliseconds / 1000f;
            num += (float)timeSpan.Seconds;
            num += (float)timeSpan.Minutes / 60f;
            if (num >= 3f)
            {
                Player.dpsStart = DateTime.Now;
                Player.dpsStart = Player.dpsStart.AddSeconds(-1.0);
                dpsDamage = (long)(dpsDamage / num);
                timeSpan = Player.dpsEnd - Player.dpsStart;
                num = (float)timeSpan.Milliseconds / 1000f;
                num += (float)timeSpan.Seconds;
                num += (float)timeSpan.Minutes / 60f;
            }
            if (num < 1f)
                num = 1f;

            return (long)(dpsDamage / num);
        }
        public static void SyncMouseWorld(Mod mod, BinaryReader reader, int whoAmI)
        {
            byte player = reader.ReadByte();
            DDPlayer DDPlayer = Main.player[player].Dplayer();
            Vector2 vector = reader.ReadVector2();
            bool RightClick = reader.ReadBoolean();
            //同步一下
            DDPlayer.MouseWorld = vector;
            Main.player[player].controlUseTile = RightClick;
            //用服务器发
            if (Main.netMode == NetmodeID.Server)
            {
                DDmod.SyncData(DDType.MouseWorld, player, -1, player);
            }
        }
        public static void PlayerData(Mod mod, BinaryReader reader, int whoAmI)
        {
            byte player = reader.ReadByte();
            DDPlayer DDPlayer = Main.player[player].Dplayer();
            int Mana = reader.ReadInt16();
            Main.player[player].statMana = Mana;
            float PlayerTimes = reader.ReadFloat();
            DDPlayer.PlayerTimes = PlayerTimes;
            int Level = reader.ReadByte();
            Main.player[player].GetModPlayer<EntrustPlayer>().Level = Level;
            BitsByte actionFlags2 = reader.ReadBitsByte();
            if (actionFlags2[0])
            {
                int Stand = reader.ReadByte();
                Main.player[player].Aplayer().Stand = Stand;
            }
            else
            {
                Main.player[player].Aplayer().Stand = 0;
            }
            if (actionFlags2[1])
            {
                int Stand2 = reader.ReadByte();
                Main.player[player].Aplayer().Stand2 = Stand2;
            }
            else
            {
                Main.player[player].Aplayer().Stand2 = 0;
            }
            if (actionFlags2[2])
            {
                int Stand3 = reader.ReadByte();
                Main.player[player].Aplayer().Stand3 = Stand3;
            }
            else
            {
                Main.player[player].Aplayer().Stand3 = 0;
            }
            int A = 0;
            int B = 0;
            if (actionFlags2[3])
            {
                A = reader.ReadInt16();
            }
            if (actionFlags2[4])
            {
                B = reader.ReadInt16();
            }
            Point16 STP = new Point16(A, B);
            Main.player[player].Dplayer().STPosition = STP;
            //用服务器发
            if (Main.netMode == NetmodeID.Server)
            {
                if (actionFlags2[3] || actionFlags2[4])
                {
                    DDmod.SyncData(DDType.PlayerData, player, -1, -1);
                }
                else
                {
                    DDmod.SyncData(DDType.PlayerData, player, -1, player);
                }
            }
        }
        //同步延迟
        public static void PlayerDelay(Mod mod, BinaryReader reader)
        {
            byte player = reader.ReadByte();

            DDPlayer DDPlayer = Main.player[player].Dplayer();
            DDPlayer.DelayTime = 0;
            //用服务器发
            if (Main.netMode == NetmodeID.Server)
            {
                DDmod.SyncData(DDType.Delay, player, player, -1);
            }
        }
        public override void FrameEffects()
        {
        }
        public override bool PreItemCheck()
        {
            return base.PreItemCheck();
        }
        public override void PostItemCheck()
        {
            base.PostItemCheck();
        }
        int ManaTime;
        int LaTime;
        bool dian;
        bool cb;
        float cbTimes;
        bool CTile = false;
        public List<Point16> TEFrame = new List<Point16>();
        public void AddTE(Point16 point)
        {
            bool R = true;
            for (int A = 0; A < TEFrame.Count; A++)
            {
                if (TEFrame[A] == point)
                {
                    R = false;
                    break;
                }

            }
            if (R)
            {
                TEFrame.Add(point);
            }
        }
        public override void PreUpdate()
        {
            //if (Player.ownedProjectileCounts[ModContent.ProjectileType<灵剑>()] == 0)
            {
                //NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<灵剑>(), 20, 3, -1,0, Player.whoAmI);
            }

            if (Main.netMode != 2 && Main.time == 0 && Main.dayTime)
            {
                for (int S = 0; S < Main.LocalPlayer.Dplayer().shops.Length; S++)
                {
                    PlayerShop op = Main.LocalPlayer.Dplayer().shops[S];
                    op.Stack = op.MaxStack;
                }
            }
            if (Main.netMode == 1)
            {
                for (int a = 0; a < TEFrame.Count; a++)
                {
                    Tile tile = Main.tile[TEFrame[a]];
                    TileObjectData tileData = TileObjectData.GetTileData(tile.TileType, 0, 0);
                    if (tileData == null)
                    {

                        TEFrame.Remove(TEFrame[a]);
                    }
                    else
                    {
                        NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(TEFrame[a].X, TEFrame[a].Y, tileData.Width, tileData.Height, 18);
                        if (tepowerCellFactory != null)
                        {
                            if (tepowerCellFactory.Initiate && tepowerCellFactory.SpawnRunning)
                                tepowerCellFactory.Time++;
                        }
                        else
                        {
                            TEFrame.Remove(TEFrame[a]);
                        }
                    }
                }
            }
            if (FightPets >= 0)
            {
                if (Bpets != null && Bpets[FightPets] != null)
                {
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        if (Player.ownedProjectileCounts[Bpets[FightPets].ProjType] == 0)
                        {
                            NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, Bpets[FightPets].ProjType, Bpets[FightPets].Damage, 3, -1, FightPets, Player.whoAmI);
                        }
                    }
                    Bpets[FightPets].PlayerUpdate(Player, FightPets);
                    if (!Bpets[FightPets].Fight)
                    {
                        FightPets = -1;
                        ownedFightPets = -1;
                    }
                }
                else
                {

                    ownedFightPets = -1;
                }
            }
            else
            {

                ownedFightPets = -1;
            }
            if (Player.sitting.isSitting && Player.sitting.details.IsAToilet)
            {
                if (Player.HasBuff(ModContent.BuffType<拉肚子>()))
                {
                    Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<拉肚子>()));
                    int Time = 0;

                    for (int A = 0; A < Player.GetModPlayer<FoodPlayer>().FoodBuff.Length; A++)
                    {
                        if (Player.GetModPlayer<FoodPlayer>().FoodBuff[A].type > 0)
                        {
                            Time += Player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time;
                            Player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time = 0;
                        }
                    }
                    if (Time > 0)
                    {
                        int num4 = Item.NewItem(Player.GetSource_FromAI(), Player.MountedCenter, Vector2.Zero, 5395, Time / 3600, noBroadcast: false, 0, noGrabDelay: true);
                        if (Main.netMode == 0)
                            Main.item[num4].noGrabDelay = 100;
                        if (Main.netMode == 1)
                            NetMessage.SendData(21, -1, -1, null, num4);
                    }
                }
                else
                {

                    for (int A = 0; A < Player.GetModPlayer<FoodPlayer>().FoodBuff.Length; A++)
                    {
                        if (Player.GetModPlayer<FoodPlayer>().FoodBuff[A].type > 0)
                        {
                            LaTime += 30;
                            Player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time -= 30;
                        }
                    }
                    if (LaTime > 3600)
                    {
                        int num4 = Item.NewItem(Player.GetSource_FromAI(), Player.MountedCenter, Vector2.Zero, 5395, 1, noBroadcast: false, 0, noGrabDelay: true);
                        if (Main.netMode == 0)
                            Main.item[num4].noGrabDelay = 100;
                        if (Main.netMode == 1)
                            NetMessage.SendData(21, -1, -1, null, num4);
                        LaTime -= 3600;
                    }
                }
            }
            /*
            bool TileCollision= Collision.SolidCollision(Player.position, Player.width/2, Player.height);
            bool TileCollision2 = Collision.SolidCollision(Player.position + new Vector2(Player.width / 2, 0), Player.width / 2, Player.height);
            bool TileCollision3= Collision.SolidCollision(Player.position, Player.width, Player.height/2);
            bool TileCollision4= Collision.SolidCollision(Player.position + new Vector2(0, Player.height / 2), Player.width, Player.height / 2);
            if(TileCollision)
            {
                Player.position.X+= Math.Abs(Player.velocity.X) + 1;
                Player.velocity.X = 0;
            }
            if(TileCollision2)
            {
                Player.position.X-= Math.Abs(Player.velocity.X) + 1;
                Player.velocity.X = 0;
            }
            if(TileCollision&& TileCollision2)
            {
                if (TileCollision3)
                {
                    Player.position.Y+= Math.Abs(Player.velocity.Y)+1;
                    Player.velocity.Y = 0;
                }
                if (TileCollision4)
                {
                    Player.position.Y -= Math.Abs(Player.velocity.Y) + 1;
                    Player.velocity.Y = 0;
                }
            }*/

            //粒子翅膀
            /*
            Color color = Main.DiscoColor;
            if (cb)
            {
                DDHelper.BackAndForth(-8, 4, 2F, ref cbTimes, ref cb);
                float I = 0;
                for (int a = 0; a < 12; a++)
                {
                    if (a > 2)
                    {
                        I += 0.3F * a;
                        float l =  10 * a;
                        if (Player.direction == -1)
                            l =  8 * a;
                        int A = NewDust(Player.Center + new Vector2(l, -20 + cbTimes * I), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: color, Scale: (1 - a / 20f) * 2 + 0.8F);
                        Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9));
                        Main.dust[A].noGravity = true;
                        Main.dust[A].customData = 6;
                        GlobalDust.DustPlayerOwner[A] = Player.whoAmI;

                        l = 10 * a;
                        if (Player.direction == 1)
                            l =  -6+8 * a;
                        int A2 = NewDust(Player.Center + new Vector2(-l, -20+cbTimes * I), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: color, Scale: (1 - a / 20f) * 2 + 0.8F);
                        Main.dust[A2].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9));
                        Main.dust[A2].noGravity = true;
                        Main.dust[A2].customData = 6;
                        GlobalDust.DustPlayerOwner[A2] = Player.whoAmI;
                    }
                }
            }
            else
            {
                DDHelper.BackAndForth(-8, 8, 0.4f, ref cbTimes, ref cb);
                float I = 0;
                for (int a = 0; a < 12; a++)
                {
                    if (a > 2)
                    {
                        I += 0.3F * a;
                        float l =  10 * a;
                        if (Player.direction == -1)
                            l =  8 * a;
                        int A = NewDust(Player.Center + new Vector2(l, -10 + cbTimes * I), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: color, Scale: (1 - a / 20f) * 2 + 0.8F);
                        Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9));
                        Main.dust[A].noGravity = true;
                        Main.dust[A].customData = 10;
                        GlobalDust.DustPlayerOwner[A] = Player.whoAmI;

                        l = 10 * a;
                        if (Player.direction == 1)
                            l = 6 + 8 * a;
                        int A2 = NewDust(Player.Center + new Vector2(-l, -10 + cbTimes * I), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: color, Scale: (1 - a / 20f) * 2 + 0.8F);
                        Main.dust[A2].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(3, 9));
                        Main.dust[A2].noGravity = true;
                        Main.dust[A2].customData = 10;
                        GlobalDust.DustPlayerOwner[A2] = Player.whoAmI;
                    }
                }
            }*/

            ManaTime++;
            if (ManaTime >= 20)
            {
                ManaTime = 0;
                Player.statMana++;
            }
            if (Math.Abs(Player.velocity.X) < 4F)
            {
                Speed = false;
            }
            if (Math.Abs(Player.velocity.X) > 5F)
            {
                Speed = true;
            }
            //最大速度
            if (Math.Abs(Player.velocity.X) > 7F)
            {
                MaxSpeed = false;
            }
            if (Math.Abs(Player.velocity.X) > 8F)
            {
                MaxSpeed = true;
            }
            //不用发给服务器
            PlayerTimes++;
            if (Main.myPlayer == Player.whoAmI)
            {
                MouseWorld = Main.MouseWorld;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    bool Net = false;
                    for (int a = 0; a < 255; a++)
                    {
                        if (a != Player.whoAmI)
                        {
                            if (Main.player[a].active && (Main.player[a].Center - Player.Center).Length() < 2000)
                            {
                                Net = true;
                                break;
                            }
                        }
                    }
                    if (Net)
                    {
                        DDmod.SyncData(DDType.MouseWorld, Player.whoAmI, -1, Player.whoAmI);
                    }
                }
            }

            if (ModContent.GetInstance<DDConfigClient>().Delay)
            {
                DelayTime2++;
                DelayTime++;
                if (Main.netMode == 2)
                {
                    DDmod.SyncData(DDType.Delay, Player.whoAmI, Player.whoAmI, -1);
                }
                if (DelayTime2 % 20 == 0)
                {
                    Delay = DelayTime;
                }
            }
            Rotate++;
            if (Main.worldName == "巨石噩梦")
            {
                if (Main.rand.NextBool(100))
                {
                    NewProjectile(Player.GetSource_FromAI(), new Vector2(Player.Center.X + Main.rand.NextFloat(-1000, 1000), Player.Center.Y - 1200), Vector2.Zero, 99, 100, 1);
                }
                if (Main.rand.NextBool(100))
                {
                    NewProjectile(Player.GetSource_FromAI(), new Vector2(Player.Center.X + Main.rand.NextFloat(-1000, 1000), Player.Center.Y - 1200), Vector2.Zero, 727, 100, 1);
                }
                if (Main.rand.NextBool(100))
                {
                    NewProjectile(Player.GetSource_FromAI(), new Vector2(Player.Center.X + Main.rand.NextFloat(-1000, 1000), Player.Center.Y - 1200), Vector2.Zero, 655, 100, 1);
                }
            }
            if (!TimeStops)
            {
                Playerperspective();
            }
        }
        public void Playerperspective()
        {
            //地震时间
            if (Earthquake > 0)
            {
                Earthquake--;
            }
            if (Earthquake <= 0)
            {
                Frequency = 0;
            }
            if (Start)
            {
                if (LockTime > 0f)
                {
                    if (!Main.gamePaused)
                        LockTime -= 1;
                }
                else if (focusTransition >= 0f)
                {
                    if (Times == 0)
                    {
                        Times = 1;
                    }
                }
                else
                {
                    Start = false;
                }
            }
            else
            {
                TimeStops = false;
                if (control >= 0 && !Main.projectile[control].active)
                {
                    control = -1;
                }
                if (control >= 0 && Main.projectile[control].DProj().Detect && Player.whoAmI == Main.myPlayer)
                {
                    control = -1;
                }
            }
        }

        //禁止使用物品
        public override bool CanUseItem(Item item)
        {
            //如果被史莱姆王吞噬
            if (ForbiddenToAttack > 0)
            {
                return false;
            }
            if (!ModContent.GetInstance<DDConfigServer>().ForceMechanism)
            {
                //生命守卫封印着生命水晶
                if (item.type == ItemID.LifeCrystal)
                {
                    if (NPCDowned.downedLifeGuard && Player.statLifeMax < 200)
                    {
                        return true;
                    }
                    else if (NPCDowned.downedLifeGuard2 && Player.statLifeMax < 300)
                    {
                        return true;
                    }
                    else if (NPCDowned.downedLifeGuard3)
                    {
                        return true;
                    }
                    return false;
                }
                //魔力水晶
                if (item.type == ItemID.ManaCrystal)
                {
                    if (NPCDowned.downedStarGuard && Player.statManaMax < 100)
                    {
                        return true;
                    }
                    else if (NPCDowned.downedStarGuard2 && Player.statManaMax < 200)
                    {
                        return true;
                    }
                    else if (NPCDowned.downedStarGuard3)
                    {
                        if (Player.statManaMax >= 200 && Player.statManaMax + statManaMax < 300)
                        {
                            statManaMax += 20;
                            Player.ManaEffect(20);
                            item.consumable = true;
                        }
                        else if (Player.statManaMax + statManaMax >= 300)
                        {
                            item.consumable = false;
                        }
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 任务ui用
        /// </summary>
        public bool EntrustTextPanelUI;
        public override void SaveData(TagCompound tag)
        {
            tag.Add(nameof(Player.statManaMax), Player.statManaMax);
            tag.Add("GreenstoneRecipe", GreenstoneRecipe);
            tag.Add("MeteorRecipe", MeteorRecipe);
            tag.Add("GreenstoneOrgan", GreenstoneOrgan);
            tag.Add("statManaMax2", statManaMax);
            tag.Add("MushroomsLife", MushroomsLife);
            tag.Add("MushroomsMana", MushroomsMana);
            tag.Add("FantasyFlowers", FantasyFlowers);
            tag.Add("AdventureCoins", AdventureCoins);
            tag.Add("AdventureCoins2", AdventureCoins2);
            tag.Add("AdventureCoins3", AdventureCoins3);
            tag.Add("AdventureCoins4", AdventureCoins4);
            tag.Add("AdventureCoins5", AdventureCoins5);
            tag.Add("FightPets", FightPets);
            tag.Add("FightPetsUIPo", FightPetsUIPo);
            tag.Add("EntrustTextPanelUI", EntrustTextPanelUI);

            if (Bpets == null || Bpets.Length != 20)
                Bpets = new BattlePets[20];
            for (int a = 0; a < Bpets.Length; a++)
            {
                if (Bpets[a] == null)
                {
                    Bpets[a] = new BattlePets(0);
                }
                Bpets[a].SaveData(tag, a);
            }
            for (int A = 0; A < shops.Length; A++)
            {
                shops[A].Save(tag, A);
            }
        }
        public override void LoadData(TagCompound tag)
        {
            Player.statManaMax = tag.Get<int>(nameof(Player.statManaMax));
            GreenstoneRecipe = tag.Get<bool>("GreenstoneRecipe");
            MeteorRecipe = tag.Get<bool>("MeteorRecipe");
            GreenstoneOrgan = tag.Get<bool>("GreenstoneOrgan");
            statManaMax = tag.Get<int>("statManaMax2");
            MushroomsLife = tag.Get<bool>("MushroomsLife");
            MushroomsMana = tag.Get<bool>("MushroomsMana");
            FantasyFlowers = tag.Get<bool>("FantasyFlowers");
            AdventureCoins = tag.Get<int>("AdventureCoins");
            AdventureCoins2 = tag.Get<int>("AdventureCoins2");
            AdventureCoins3 = tag.Get<int>("AdventureCoins3");
            AdventureCoins4 = tag.Get<int>("AdventureCoins4");
            AdventureCoins5 = tag.Get<int>("AdventureCoins5");
            FightPets = tag.Get<int>("FightPets");
            FightPetsUIPo = tag.Get<Vector2>("FightPetsUIPo");
            EntrustTextPanelUI = tag.Get<bool>("EntrustTextPanelUI");

            if (Bpets == null || Bpets.Length != 20)
                Bpets = new BattlePets[20];


            for (int a = 0; a < Bpets.Length; a++)
            {
                if (Bpets[a] == null)
                {
                    Bpets[a] = new BattlePets(0);
                }
                Bpets[a].LoadData(tag, a);
            }
            for (int A = 0; A < shops.Length; A++)
            {
                shops[A].Load(tag, A);
            }
        }

        /// <summary> 视角转移(position位置,Time停留时间,TimeStop玩家弹幕时停,Ti视角转移时间) </summary>
        public void Bossperspective(Vector2 position, int Time, bool TimeStop, float Ti = 0.05f)
        {
            if (Player.Distance(position) < 10000f)
            {
                ScreenPosition = position - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                focusTransition = 0f;
                startPoint = Main.screenPosition;
                LockTime = Time;
                Times = Ti;
                Start = true;
                TimeStops = TimeStop;
            }
        }
        //起点
        public Vector2 startPoint;
        //终点
        public Vector2 ScreenPosition;
        //锁定时间
        public int LockTime;
        //开始
        public bool Start;
        public float Times;

        public int Earthquake;

        public float Frequency;

        private float focusTransition;

        public bool TimeStops;

        Vector2 screenPosition;
        public override void ModifyScreenPosition()
        {
            if (WorldGen.IsGeneratingHardMode)
            {
                //困难模式地震
                PlayerShake(5, 12);
            }
            //时间停止
            if (TimeStops)
            {
                DDOn.DDmodOn.Start = 5;
            }
            //TimeStops = Start;
            //开始转移屏幕
            if (Start)
            {
                if (LockTime > 0f)
                {
                    if (focusTransition <= 1f && Times > 0)
                    {
                        Main.screenPosition = Vector2.SmoothStep(startPoint, ScreenPosition, focusTransition += Times);
                    }
                    else
                    {
                        Main.screenPosition = ScreenPosition;
                    }
                    if (Frequency != 0)
                    {
                        Main.screenPosition = ScreenPosition + new Vector2(Main.rand.NextFloat(-Frequency, Frequency), Main.rand.NextFloat(-Frequency, Frequency));
                    }
                }
                else if (focusTransition >= 0f)
                {
                    Main.screenPosition = Vector2.SmoothStep(Player.Center - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), ScreenPosition, focusTransition -= Times);
                }
            }
            else
            {
                //屏幕在玩家位置
                if (Frequency != 0)
                {
                    Main.screenPosition += new Vector2(Main.rand.NextFloat(-Frequency, Frequency), Main.rand.NextFloat(-Frequency, Frequency));

                }
                if (control >= 0 && Main.projectile[control].DProj().Detect && Player.whoAmI == Main.myPlayer)
                {
                    Projectile projectile = Main.projectile[control];
                    if (Frequency != 0)
                    {
                        Main.screenPosition = projectile.Center + new Vector2(Main.rand.NextFloat(-Frequency, Frequency), Main.rand.NextFloat(-Frequency, Frequency)) - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                    }
                    else
                    {
                        Main.screenPosition = projectile.Center - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                    }
                }
            }
        }
        /// <summary> 屏幕抖动使用,VibrationTime震动时间,VibrationFrequency震动频率 </summary>
        public void PlayerShake(int VibrationTime = 0, float VibrationFrequency = 0)
        {
            Earthquake = VibrationTime;
            Frequency = VibrationFrequency * ModContent.GetInstance<DDConfigClient>().VibrationFrequency;
        }
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (mediumCoreDeath)
            {
                return Enumerable.Empty<Item>();

            }
            Player.miscEquips[0].type = ModContent.ItemType<LavaCrown>();
            Player.miscEquips[0].SetDefaults(ModContent.ItemType<LavaCrown>());
            Item item = new Item(ModContent.ItemType<黎明Mod>());
            return new[] {
                item
            };
        }
        public override bool ConsumableDodge(HurtInfo info)
        {
            if (Main.rand.Next(100) < DodgeChance)
            {
                Player.immune = true;
                Player.immuneTime = 60;
                CombatText.NewText(new Rectangle((int)Player.Center.X, (int)Player.Center.Y, 1, 1), new Color(0, 162, 255, 0) * 0.8f, DDSystem.English ? "dodge!" : "闪避!", false, false);
                return true;
            }

            return base.ConsumableDodge(info);
        }
        /// <summary>
        /// 给老子发包
        /// </summary>
        public bool Config;
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (newPlayer)
            {
                for (int A = 0; A < 200; A++)
                {
                    Main.npc[A].netUpdate = true;
                }
                DDmod.SyncData(DDType.Config, Player.whoAmI, toWho, fromWho);
                DDmod.SyncData(DDType.PlayerFood, Player.whoAmI, toWho, fromWho);
                DDmod.SyncData(DDType.Battlepets, Player.whoAmI, toWho, fromWho);
                DDmod.SyncData(DDType.PlayerData, Player.whoAmI, toWho, fromWho);
            }
        }
        public override void ModifyDrawLayerOrdering(IDictionary<PlayerDrawLayer, PlayerDrawLayer.Position> positions)
        {
            base.ModifyDrawLayerOrdering(positions);
        }
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Item item = drawPlayer.ActiveItem();

            if (drawInfo.heldProjOverHand)
            {
                drawInfo.projectileDrawPosition = drawInfo.DrawDataCache.Count;
            }
        }
        public static void 移动玩家(Player Player, int 无敌时间 = 0, bool 移除坐骑 = false)
        {
            //if (Player.whoAmI == Main.myPlayer)
            //DDmod.SyncData(DDType.PlayerCenter, Player.whoAmI, -1, Player.whoAmI);
            if (无敌时间 > 0)
            {
                Player.immune = true;
                Player.hurtCooldowns[1] = 30;
                Player.immuneTime = 30;
                Player.immuneNoBlink = true;
            }
            if (移除坐骑)
            {
                Player.mount.Dismount(Player);
            }
            Player.Aplayer().IgnoreWater = 3;
            for (int a = 0; a < 1000; a++)
            {
                if (Main.projectile[a].active && Main.projectile[a].aiStyle == 7 && Main.projectile[a].owner == Player.whoAmI)
                {
                    Main.projectile[a].Kill();
                }
            }
        }
        public override bool HoverSlot(Item[] inventory, int context, int slot)
        {
            Item item = inventory[slot];
            if (item.IsAir)
                return false;

            if (StrengtheningUI.Visible)
            {
                if (ItemSlot.ShiftInUse && context == ItemSlot.Context.InventoryItem && !item.favorited)
                {
                    Main.cursorOverride = 9;
                    return true;
                }
            }
            if (BackpackStrengtheningUI.Visible)
            {
                if (ItemSlot.ShiftInUse && context == ItemSlot.Context.InventoryItem && !item.favorited)
                {
                    Main.cursorOverride = 9;
                    return true;
                }
            }
            return base.HoverSlot(inventory, context, slot);
        }
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            Item item = inventory[slot];
            if (item.IsAir)
                return false;
            if (StrengtheningUI.Visible)
            {

                if (context == ItemSlot.Context.InventoryItem && !item.favorited)
                {
                    if (StrengtheningUI.ItemFrame.items.type == 0)
                    {
                        StrengtheningUI.ItemFrame.Click(inventory[slot]);
                        return true;
                    }
                    else if(StrengtheningUI.CanAddItem(item, StrengtheningUI.ItemFrame.items, true))
                    {
                        StrengtheningUI.ItemFrame.Click(inventory[slot], slot);
                    }
                    else
                    {
                        for (int A = 0; A < 3; A++)
                        {
                            if (StrengtheningUI.CanAddItem(item, StrengtheningUI.StrengthenFrame[A].items))
                            {
                                StrengtheningUI.StrengthenFrame[A].Click(inventory[slot]);
                                return true;
                            }
                        }
                    }
                }
            }
            if (BackpackStrengtheningUI.Visible)
            {

                if (context == ItemSlot.Context.InventoryItem && !item.favorited && BackpackStrengtheningUI.InventoryBar >= 0)
                {
                    if (BackpackStrengtheningUI.CanAddItem(item, Player.inventory[BackpackStrengtheningUI.InventoryBar].GetGlobalItem<BackpackUIItem>().ContainedItem, out int S))
                    {
                        BackpackStrengtheningUI.bar2[S].Click(inventory[slot]);
                    }
                    return true;
                }
            }
            return base.ShiftClickSlot(inventory, context, slot);
        }
    }
}