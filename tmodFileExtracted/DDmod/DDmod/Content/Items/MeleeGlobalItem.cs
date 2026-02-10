using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Content.Projectiles.Melee.Sword.Axe;
using DDmod.Content.Projectiles.Melee.Sword.Hammer;
using DDmod.Content.Tiles.农场;
using DDmod.NoContent.Config;
using DDmod.Players;
using DDmod.Worlds;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader.Config;
using Terraria.ObjectData;
using static Terraria.Player;

namespace DDmod.Content.Items
{
    public class MeleeGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool IsCloneable => true; 
        /// <summary>
        /// 锄头
        /// </summary>
        public bool Hoe;
        public bool SpecialAttack;
        public bool RightSwitch;
        public bool TrueMelee;
        public bool ModifyMelee;
        public bool ModifyMelee2;
        /// <summary>
        /// 剑盾
        /// </summary>
        public bool SwordandShield;
        //双刀减伤
        public float Endurance;
        public Color Color= new Color(0, 0, 0, 0);
        //附属颜色
        public Color Color2= new Color(0, 0, 0, 0);
        public bool? ColorL;
        public float EffectLength = -1;

        /// <summary>
        /// 45°斜着的近战武器
        /// </summary>
        public bool DiagonalWeapon;
        /// <summary>
        /// 回旋镖
        /// </summary>
        public bool Boomerang;
        /// <summary>
        /// 飞盘
        /// </summary>
        public bool Chakram;
        public int OriginalHammer(Item item2)
        {
            return ContentSamples.ItemsByType.TryGetValue(item2.type, out Item item) ? item.hammer : 0;

        }
        public int OriginalAxe(Item item2)
        {
            return ContentSamples.ItemsByType.TryGetValue(item2.type, out Item item) ? item.axe : 0;

        }
        public int OriginalPick(Item item2)
        {
            return ContentSamples.ItemsByType.TryGetValue(item2.type, out Item item) ? item.pick : 0;

        }
        public static int FlyingKnife;
        public static int SummonLamp;
        public override void Load()
        {
            FlyingKnife = ItemLoader.RegisterUseStyle(Mod, "飞刀");
            SummonLamp = ItemLoader.RegisterUseStyle(Mod, "召唤灯");
        }
        public override void SetDefaults(Item item)
        {
                bool Rework = ModContent.GetInstance<DDConfigServer>().MeleeRework2;
            //短剑
            if (item.type is 6 or 3483 or 3489 or 3495 or 3501 or 3507
                or 3513 or 3519 or 4463)
            {
                item.DamageType = DamageClass.Melee;
            }
            if (item.type is 4788 or 4789 or 4790)
                item.DItem().DrawMelee = true;
            //骨剑
            if (item.type == 1166)
            {
                item.shoot = 21;
                item.shootSpeed = 6;
                item.autoReuse = true;
                item.useTime = item.useAnimation;
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.稀有;
            }
            //僵尸手
            if (item.type == 1304)
            {
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.稀有;
            }
            //链刃
            if (item.type == 1325)
            {
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.稀有;
            }
            //蝙蝠刃
            if (item.type == 5097)
            {
                EffectLength = 34;
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.稀有;

            }
            //狂星之怒
            if (item.type == 3065)
            {
                EffectLength = 32;
            }
            //种子弯刀
            if (item.type == 3018)
            {
                EffectLength = 38;
            }
            //种子弯刀
            if (item.type == 3013)
            {
                EffectLength = 18;
            }
            //钉锤
            if (item.type == 5094)
            {
                EffectLength = 29;
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.罕见;
            }
            //种子弯刀
            if (item.type == 3027)
            {
                EffectLength = 28;
            }
            if (item.type == 1571||item.type == 486||item.type == 3368||item.type == 4144)
            {
                item.DItem().DrawMelee = true;
            }
            if (item.type == 1297||item.type == 1314)
            {
                item.DItem().DrawRanged = true;
            }
            if (item.type == 2424)
            {
                item.DItem().DrawRot = -MathHelper.PiOver4;
                item.DItem().DrawDistance = 24;
            }
            if(item.type is 55 or 119 or 284
                or 670 or 1324 or 4764)
            {
                Boomerang = true;
            }
            if(item.type is ItemID.ThornChakram or 561 or 1918)
            {
                Chakram = true;
                item.channel = true;
                item.useStyle = 13;
            }
            if (Chakram)
            {
                item.DItem().DrawRot = MathHelper.PiOver2;
                item.DItem().DrawDistance = -12;
            }
            if (Boomerang)
            {
                item.DItem().DrawRot = MathHelper.PiOver2;
                item.DItem().DrawDistance = -4;
            }
            //光束剑
            if (item.type == 723)
            {
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.稀有;
                item.useTime = item.useAnimation;
                item.shoot = ModContent.ProjectileType<光束剑>();
                Color = new Color(255, 155, 50,100);
                ColorL = true;
            }
            if (item.DamageType == DamageClass.Melee && item.useStyle == 1 && !item.noMelee && ModContent.GetInstance<DDConfigServer>().MeleeRework)
            {
                DiagonalWeapon = true;
                if (item.axe == 0 && item.hammer == 0 && item.pick == 0 && item.type != 1786 && item.type != 213 && !item.GetGlobalItem<MeleeGlobalItem>().Hoe)
                {
                    if (item.shoot == 0&& item.type != ItemID.BloodButcherer)
                    {
                        item.shoot = ModContent.ProjectileType<GlobalSword>();
                        TrueMelee = true;
                    }
                    if (item.type != ModContent.ItemType<A>() && item.type != ModContent.ItemType<B>())
                    {
                        item.useTime = item.useAnimation;
                    }
                    item.UseSound = null;
                    //item.scale += 0.2f;
                    item.damage += (int)(item.damage * 0.2f);
                    item.useTurn = false;
                    TakeProj(item);
                    ModifyMelee = true;
                }
            }
            if (!ModContent.GetInstance<DDConfigServer>().MeleeRework2)
            {
                return;
            }
            //血肉锤
            if (item.type == ItemID.FleshGrinder)
            {
                RightSwitch = true;
            }
            //暗夜战斧
            if (item.type == ItemID.WarAxeoftheNight)
            {
                RightSwitch = true;
            }
            //暗夜战斧
            if (item.type == ItemID.TheBreaker)
            {
                RightSwitch = true;
            }
            if(item.axe != 0 || item.hammer != 0 || item.pick != 0)
            {
                if (!item.noMelee && item.useStyle == 1)
                {
                    DiagonalWeapon = true;
                }
            }
            //蓝相位剑
            if (item.type == ItemID.BluePhaseblade)
            {
                PhaseBlade(item, ModContent.ProjectileType<BluePhaseblade>());
                item.damage = (int)(item.damage * 0.65f);
            }
            //红相位剑
            if (item.type == ItemID.RedPhaseblade)
            {
                PhaseBlade(item, ModContent.ProjectileType<RedPhaseblade>());
                item.damage = (int)(item.damage * 0.5f);
            }
            //绿相位剑
            if (item.type == ItemID.GreenPhaseblade)
            {
                PhaseBlade(item, ModContent.ProjectileType<GreenPhaseblade>());
                item.damage = (int)(item.damage * 0.65f);
            }
            //紫相位剑
            if (item.type == ItemID.PurplePhaseblade)
            {
                PhaseBlade(item, ModContent.ProjectileType<PurplePhaseblade>());
            }
            //白相位剑
            if (item.type == ItemID.WhitePhaseblade)
            {
                PhaseBlade(item, ModContent.ProjectileType<WhitePhaseblade>());
                item.damage = (int)(item.damage * 0.5f);
            }
            //黄相位剑
            if (item.type == ItemID.YellowPhaseblade)
            {
                PhaseBlade(item, ModContent.ProjectileType<YellowPhaseblade>());
            }
            //橙相位剑
            if (item.type == ItemID.OrangePhaseblade)
            {
                PhaseBlade(item, ModContent.ProjectileType<OrangePhaseblade>());
                item.damage = (int)(item.damage * 0.25f);
            }
            //蓝晶光刃
            if (item.type == ItemID.BluePhasesaber)
            {
                PhaseBlade(item, ModContent.ProjectileType<BluePhasesaber>());
                item.damage = (int)(item.damage * 0.65f);
            }
            //红晶光刃
            if (item.type == ItemID.RedPhasesaber)
            {
                PhaseBlade(item, ModContent.ProjectileType<RedPhasesaber>());
                item.damage = (int)(item.damage * 0.5f);
            }
            //绿晶光刃
            if (item.type == ItemID.GreenPhasesaber)
            {
                PhaseBlade(item, ModContent.ProjectileType<GreenPhasesaber>());
                item.damage = (int)(item.damage * 0.65f);
            }
            //紫晶光刃
            if (item.type == ItemID.PurplePhasesaber)
            {
                PhaseBlade(item, ModContent.ProjectileType<PurplePhasesaber>());
            }
            //白晶光刃
            if (item.type == ItemID.WhitePhasesaber)
            {
                PhaseBlade(item, ModContent.ProjectileType<WhitePhasesaber>());
                item.damage = (int)(item.damage * 0.5f);
            }
            //黄晶光刃
            if (item.type == ItemID.YellowPhasesaber)
            {
                PhaseBlade(item, ModContent.ProjectileType<YellowPhasesaber>());
            }
            //橙晶光刃
            if (item.type == ItemID.OrangePhasesaber)
            {
                PhaseBlade(item, ModContent.ProjectileType<OrangePhasesaber>());
                item.damage = (int)(item.damage * 0.25f);
            }
            //毁灭刃
            if (item.type == ItemID.BreakerBlade)
            {
                item.shoot = ModContent.ProjectileType<BreakerBlade>();
                item.useTime = item.useAnimation;
                item.scale += 1;
                TakeProj(item);
            }
            //魔光剑
            if (item.type == ItemID.LightsBane && Rework)
            {
                item.shoot = ModContent.ProjectileType<LightsBane>();
                item.shootSpeed = 7;
                item.autoReuse = true;
                item.scale += 0.2f;
                item.useTime = item.useAnimation;
                DiagonalWeapon = true;
                item.UseSound = null;
                //item.scale += 0.2f;
                item.damage += (int)(item.damage * 0.2f);
                item.useTurn = false;
                item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(99, 74, 187, 255);
                TakeProj(item);
                ModifyMelee = true;
            }
            if (item.type == 3827)
            {
                EffectLength = 48;
            }
            //星怒
            if (item.type == 65)
            {
                item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(237, 63, 133, 50);
            }
            //血猩屠刀
            if (item.type == ItemID.BloodButcherer && Rework)
            {
                item.shoot = ModContent.ProjectileType<BloodButcherer>();
                item.shootSpeed = 13;
                item.autoReuse = true;
                item.scale += 0.2f;
                item.useTime = item.useAnimation;
                DiagonalWeapon = true;
                item.UseSound = null;
                EffectLength = 36;
                //item.scale += 0.2f;
                item.damage += (int)(item.damage * 0.2f);
                item.useTurn = false;
                item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(25, 25, 25, 255);
                item.GetGlobalItem<MeleeGlobalItem>().Color2 = new Color(200, 0, 0, 255) ;
                TakeProj(item);
                ModifyMelee = true;
            }
            //炽焰巨剑
            if (item.type == ItemID.FieryGreatsword)
            {
                if (ModContent.GetInstance<DDConfigServer>().MeleeRework2)
                {
                    item.shoot = ModContent.ProjectileType<Blaze>();
                    item.shootSpeed = 16;
                    item.scale += 0.2f;
                    item.useTime = item.useAnimation;
                    item.UseSound = null;
                    //item.useAnimation = 45;
                    TakeProj(item);
                }
            }
            //草剑
            if (item.type == ItemID.BladeofGrass)
            {
                item.shoot = ModContent.ProjectileType<BladeofGrass>();
                item.scale += 0.2f;
                item.UseSound = null;
                item.useTime = 16;
                item.useAnimation = 10;
                TakeProj(item);
            }
            //妖刀村正
            if (item.type == ItemID.Muramasa)
            {
                item.shoot = ModContent.ProjectileType<Muramasa>();
                item.scale += 0.1f;
                item.useTime = item.useAnimation / 2;
                item.useAnimation /= 2;
                item.UseSound = null;
                TakeProj(item);
            }
            //泰拉魔刃
            if (item.type == 4144)
            {
                item.shoot = ModContent.ProjectileType<泰拉魔刃Proj>();
                item.useTime = item.useAnimation=20;
                item.useAnimation/=2;
                item.knockBack = 3F;
                item.UseSound = null;
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.珍宝;
                TakeProj(item);
            }
            //永夜之刃
            if (item.type == ItemID.NightsEdge)
            {
                item.shoot = ModContent.ProjectileType<NightsEdge>();
                item.useTime = item.useAnimation;
                item.scale += 0.2f;
                item.UseSound = null;
                item.knockBack += 4;
                item.damage = 18;
                TakeProj(item);
            }
            //真永夜之刃
            if (item.type == ItemID.TrueNightsEdge)
            {
                item.shoot = ModContent.ProjectileType<TrueNightsEdge>();
                item.useTime = item.useAnimation;
                item.scale += 0.2f;
                item.knockBack += 4;
                item.UseSound = null;
                item.damage = 28;
                TakeProj(item);
            }
            //神圣剑
            if (item.type == ItemID.Excalibur)
            {
                item.shoot = ModContent.ProjectileType<Excalibur>();
                item.useTime = item.useAnimation * 2;
                item.scale += 0.1f;
                item.UseSound = null;
                item.damage = 52;
                TakeProj(item);
            }
            //真神圣剑
            if (item.type == ItemID.TrueExcalibur)
            {
                item.shoot = ModContent.ProjectileType<TrueExcalibur>();
                item.useTime = item.useAnimation * 2;
                item.scale += 0.1f;
                item.UseSound = null;
                item.damage = 62;
                TakeProj(item);
            }
            //风暴长矛
            if (item.type == 4061)
            {
                item.damage = 8;
            }
            //泰拉刃
            if (item.type == ItemID.TerraBlade)
            {
                ItemID.Sets.BonusAttackSpeedMultiplier[item.type] = 1;
                item.shoot = ModContent.ProjectileType<TerraBlade>();
                item.useTime = item.useAnimation;
                item.scale += 0.1f;
                item.UseSound = null;
                item.damage = 38;
                SpecialAttack = true;
                item.DItem().DrawVec -= new Vector2(12);
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.泰拉级;
                TakeProj(item);
            }
            //武士刀
            if (item.type == ItemID.Katana)
            {
                item.shoot = ModContent.ProjectileType<Katana>();
                item.useTime = item.useAnimation;
                item.UseSound = null;
                SpecialAttack = true;
                TakeProj(item);
            }
            //僵尸手
            if (item.type == ItemID.ZombieArm)
            {
                item.shoot = ModContent.ProjectileType<ZombieArm>();
                item.useTime = item.useAnimation;
                item.UseSound = null;
                SpecialAttack = true;
                TakeProj(item);
            }
            //暗影焰飞刀
            if (item.type == ItemID.ShadowFlameKnife|| item.type == 3543||item.type == 1569||item.type == 1571||item.type == 4956)
            {
                item.damage = (int)(item.damage *0.75f);
                item.DamageType = DamageClass.Melee;
            }
            //血叉
            if (item.type == ItemID.TheRottedFork)
            {
                item.useStyle = ItemUseStyleID.Rapier;
                item.autoReuse = true;
                item.useAnimation = 100;
                item.useTime = 100;
            }
            if (item.type == 2880)
            {
                item.damage = 100;
                item.shoot = ModContent.ProjectileType<InfluxWaver>();
                item.shootSpeed = 2;
                item.useAnimation = 10;
                item.useTime = 10;
            }
            if (item.type == 1227)
            {
                SpecialAttack = true;
            }
            if (item.type is 55 or 284 or 670 or 4764 or 5298)
            {
                item.channel = true;
                item.damage *= 3;
                
            }
            if (item.type is 119)
            {
                item.channel = true;
                item.damage = (int)(item.damage*1.5f);
                
            }
                //特殊攻击的武器
                if (item.type is ItemID.IronShortsword or ItemID.PlatinumShortsword or 3489
                or 3495 or 3501 or 3507
                or 3513 or 3519 or 4463)
            {
                DiagonalWeapon = true;
                SpecialAttack = true;
                ModifyMelee = false;
            }
        }
        public bool Use;
        public void TakeProj(Item item)
        {
            item.noMelee = true;
            item.useStyle = ItemUseStyleID.Rapier;
            item.noUseGraphic = true;
            item.channel = true;
            item.autoReuse = true;
            item.useTurn = false;
            ModifyMelee = false;
            ModifyMelee2 = true;
            DiagonalWeapon = true;
            item.shootsEveryUse = false;
        }

        public void PhaseBlade(Item item, int shoot)
        {
            item.noMelee = true;
            item.useAnimation = 100;
            item.useTime = 100;
            item.useStyle = ItemUseStyleID.Rapier;
            item.shoot = shoot;
            item.noUseGraphic = true;
            item.channel = true;
            item.autoReuse = true;
            item.useTurn = false;
            ModifyMelee = false;
            DiagonalWeapon = true;
            item.shootsEveryUse = false;
        }
        public override void HoldItem(Item item, Player player)
        {
            if (!ModContent.GetInstance<DDConfigServer>().MeleeRework)
            {
                return;
            }
            if (item.pick > 0||item.axe>0||item.hammer>0)
            {
                bool flag18 = player.position.X / 16f - Player.tileRangeX - item.tileBoost <= Player.tileTargetX && (player.position.X + player.width) / 16f + Player.tileRangeX + item.tileBoost - 1f >= Player.tileTargetX && player.position.Y / 16f - Player.tileRangeY - item.tileBoost <= Player.tileTargetY && (player.position.Y + player.height) / 16f + Player.tileRangeY + item.tileBoost - 2f >= Player.tileTargetY;
                if (player.noBuilding)
                {
                    flag18 = false;
                }
                if (flag18)
                {
                    Point point = new Point(Player.tileTargetX, Player.tileTargetY);
                    Tile tile = Main.tile[point];
                    if (player.toolTime == 0 && player.itemAnimation > 0 && player.controlUseItem)
                    {
                        //镐子
                        if (tile.HasTile && !Main.tileAxe[tile.TileType] && !Main.tileHammer[tile.TileType])
                        {
                            //猩红镐
                            if (item.type == ItemID.DeathbringerPickaxe && Main.rand.NextBool(2))
                            {
                                NewProjectile(player.GetSource_FromAI(), new Vector2(Player.tileTargetX, Player.tileTargetY) * 16, Vector2.Zero, ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, 1);
                            }
                            //腐化镐
                            if (item.type == ItemID.NightmarePickaxe && Main.rand.NextBool(2))
                            {
                                for (int A = 0; A < 30; A++)
                                {
                                    int Type = 267;
                                    Dust dust = Main.dust[NewDust(new Vector2(Player.tileTargetX, Player.tileTargetY) * 16 + new Vector2(8), 1, 1, Type, 0,0, 100, default)];
                                    dust.noGravity = true;
                                    dust.scale = 1.2f;
                                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 3);
                                    dust.color = new Color(99, 74, 187, 0);
                                    dust.noLightEmittence = false;
                                }
                                for (int a = 0; a < 3; a++)
                                {
                                    player.PickTile(Player.tileTargetX, Player.tileTargetY, item.pick);
                                }
                            }
                        }
                        //斧头
                        if (tile.HasTile && Main.tileAxe[tile.TileType] && !Main.tileHammer[tile.TileType])
                        {
                            //暗夜斧
                            if (item.type == 45 && Main.rand.NextBool(2))
                            {
                                for (int A = 0; A < 30; A++)
                                {
                                    int Type = 267;
                                    Dust dust = Main.dust[NewDust(new Vector2(Player.tileTargetX, Player.tileTargetY) * 16 + new Vector2(8), 1, 1, Type, 0,0, 100, default)];
                                    dust.noGravity = true;
                                    dust.scale = 1.2f;
                                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 3);
                                    dust.color = new Color(99, 74, 187, 0);
                                    dust.noLightEmittence = false;
                                }
                                for (int a = 0; a < 3; a++)
                                {
                                    player.PickTile(Player.tileTargetX, Player.tileTargetY, item.pick);
                                }
                            }
                        }
                    }
                }
            }
        }
        public override bool? UseItem(Item item, Player player)
        {
            bool flag18 = player.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX && (player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX && player.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY && (player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY;
            if (player.noBuilding)
            {
                flag18 = false;
            }
            if (flag18)
            {
                if (Hoe)
                {
                    Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
                    player.PickTile(Player.tileTargetX, Player.tileTargetY, 1);
                    if (tile.HasTile && tile.TileType == 0)
                    {
                        int T = 0;
                        for (int a = 0; a < DDWorld.土.Length; a++)
                        {
                            if (Main.tile[DDWorld.土[a].tiles.X, DDWorld.土[a].tiles.Y].TileType == ModContent.TileType<锄过的土块>())
                            {
                                T++;
                            }
                        }
                        if (T < 1000)
                        {
                            tile.TileType = (ushort)ModContent.TileType<锄过的土块>();
                            /*
                            if (TileObject.CanPlace(Player.tileTargetX, Player.tileTargetY, ModContent.TileType<锄过的土块>(), 0, 1, out TileObject objectData))
                            {
                                TileObject.Place(objectData);
                            }
                            NetMessage.SendObjectPlacment(-1, Player.tileTargetX, Player.tileTargetY, ModContent.TileType<锄过的土块>(), 0, 0, -1, -1);

                            ModContent.GetInstance<菜TE>().Hook_AfterPlacement(Player.tileTargetX, Player.tileTargetY, ModContent.TileEntityType<菜TE>(),0,0,0);
                            TileEntity.PlaceEntityNet(Player.tileTargetX, Player.tileTargetY, ModContent.TileEntityType<菜TE>());
                             */
                            锄土.New(new Point16(Player.tileTargetX, Player.tileTargetY));
                        }
                        WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
                        NetMessage.SendTileSquare(player.whoAmI, Player.tileTargetX, Player.tileTargetY, 1);
                        return true;
                    }
                    if (tile.HasTile && tile.TileType == (ushort)ModContent.TileType<锄过的土块>())
                    {
                        tile.TileType = 0;
                        WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
                        NetMessage.SendTileSquare(player.whoAmI, Player.tileTargetX, Player.tileTargetY, 1);
                        return true;
                    }
                    return true;
                }
            }
                return base.UseItem(item, player);
        }
        public override void UpdateInventory(Item item, Player player)
        {
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (!ModContent.GetInstance<DDConfigServer>().MeleeRework)
            {
                return base.CanUseItem(item, player);
            }
            if (item.type == 5298)
            {
                return DDPlayer.UseBoomerang(item, player);
            }
            //血肉锤
            if (item.type is ItemID.FleshGrinder or ItemID.WarAxeoftheNight or ItemID.TheBreaker)
            {
                NetMessage.SendData(5, -1, -1, null, player.whoAmI, player.selectedItem, Main.player[Main.myPlayer].inventory[player.selectedItem].prefix);
                if (player.altFunctionUse != 0)
                {
                    return false;
                }
            }
            return base.CanUseItem(item, player);
        }
        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
        }
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            //血猩斧
            if (item.type == ItemID.BloodLustCluster)
            {
                if (target.HasBuff(BuffID.Bleeding))
                {
                    modifiers.SourceDamage += 0.2F;
                }
                target.AddBuff(30, Main.rand.Next(100, 300));
            }
            //血猩屠刀
            if (item.type == ItemID.BloodButcherer)
            {
                if (target.HasBuff(BuffID.Bleeding))
                {
                    modifiers.SourceDamage += 0.2F;
                }
                target.AddBuff(30, 300);
            }
            

        }
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!ModContent.GetInstance<DDConfigServer>().MeleeRework2)
            {
                return;
            }
            //魔光剑
            if (item.type == ItemID.LightsBane)
            {
                for (int a = 0; a < 1; a++)
                {
                    Vector2 vector = Main.rand.NextVector2Unit() * 20;
                    NewProjectile(player.GetSource_FromAI(), target.Center + vector, -vector / 10, 974, hit.Damage / 2, hit.Knockback / 2, player.whoAmI, hit.Crit?item.scale*2:item.scale, 1);
                }
            }
        }
        public override void PostReforge(Item item)
        {
        }
        public override bool CanShoot(Item item, Player player)
        {
            return base.CanShoot(item, player);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (!ModContent.GetInstance<DDConfigServer>().MeleeRework2)
            {
                return;
            }
            if (item.type == ItemID.MoltenFury)
            {
                if (type == 2)
                {
                    type = 41;
                }
            }
            if (item.type == ItemID.FieryGreatsword)
            {
                if (player.heldProj < 0)
                {
                    type = 0;
                    NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(item, 0), player.Center, Vector2.Zero, ModContent.ProjectileType<FieryGreatsword>(), damage, knockback, player.whoAmI, player.whoAmI, 1);
                }
            }
            if (ModifyMelee)
            {
                if (!TrueMelee)
                {
                    if (player.heldProj < 0)
                    {
                        type = 0;
                        //性奴
                        if(item.type == 65)
                        {
                            damage /= 2;
                        }
                        //死神镰刀
                        if (item.type == ItemID.DeathSickle)
                        {
                            SpecialAttack = true;
                            NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(item, 0), player.Center, Vector2.Zero, ModContent.ProjectileType<DeathSickle>(), damage, knockback, player.whoAmI, player.whoAmI, 1);
                        }
                        else
                        {
                            NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(item,0), player.Center, Vector2.Zero, ModContent.ProjectileType<GlobalSword>(), damage, knockback, player.whoAmI, player.whoAmI,1);
                        }
                    }
                }
                else
                {
                    if (player.heldProj >= 0)
                    {
                        type = 0;
                    }
                }
            }
            if (item.type == ItemID.FieryGreatsword)
            {
                damage /= 4;
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == 65)
            {
                if (Main.rand.NextBool(4))
                {
                    position = new Vector2(player.Center.X - Main.rand.Next(600) * player.direction, player.Center.Y - 800);
                    float SP = velocity.Length();
                    velocity = (Main.MouseWorld - position).PerfectNormalize() * SP;
                    NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, Main.MouseWorld.Y);
                }
                else
                {
                    position = new Vector2(player.Center.X - Main.rand.Next(600) * player.direction, player.Center.Y - 800);
                    float SP = velocity.Length();
                    velocity = (Main.MouseWorld - position).PerfectNormalize() * SP;
                    NewProjectile(source, position, velocity, type, damage/2, knockback, player.whoAmI, 0, Main.MouseWorld.Y,-1);

                }
                return false;
            }
            if (item.type == 1166)
            {
                for (int a = 0; a < Main.rand.Next(1, 4); a++)
                {
                    Vector2 Pvelocity = Utils.RotatedBy(velocity, Main.rand.NextFloat(-0.5F, 0.5F), default);
                    int A = NewProjectile(source, position, Pvelocity, type, damage / 5, knockback, player.whoAmI, 0f, 0f);
                }
                return false;
            }
            if (!ModContent.GetInstance<DDConfigServer>().MeleeRework2)
            {
                return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            }
            if (item.type == ItemID.LightsBane)
            {
                velocity = (player.Dplayer().MouseWorld - player.Center).PerfectNormalize() * 7;

                NewProjectile(source, player.MountedCenter, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);

                return false;
            }
            if (item.type is ItemID.NightsEdge or ItemID.TrueNightsEdge or ItemID.TrueExcalibur or ItemID.TerraBlade)
            {
                NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);

                return false;
            }
            if (item.type == 190)
            {
                damage = player.GetWeaponDamage(item);
                NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);

                return false;
            }


            if (item.type is ItemID.BluePhaseblade or ItemID.GreenPhaseblade or ItemID.BluePhasesaber or ItemID.GreenPhasesaber)
            {
                for (int a = 0; a < 2; a++)
                {
                    Vector2 Pvelocity = Utils.RotatedBy(velocity, MathHelper.TwoPi / 2 * a, default);
                    int A = NewProjectile(source, position, Pvelocity, type, damage, knockback, player.whoAmI, 0f, 0f);
                    Main.projectile[A].DProj().Times[4] = MathHelper.TwoPi / 2 * a;
                }
                return false;
            }
            if (item.type is ItemID.RedPhaseblade or ItemID.WhitePhaseblade or ItemID.RedPhasesaber or ItemID.WhitePhasesaber)
            {
                for (int a = 0; a < 3; a++)
                {
                    Vector2 Pvelocity = Utils.RotatedBy(velocity, MathHelper.TwoPi / 3 * a, default);
                    int A = NewProjectile(source, position, Pvelocity, type, damage, knockback, player.whoAmI, 0f, 0f);
                    Main.projectile[A].DProj().Times[4] = MathHelper.TwoPi / 3 * a;
                }
                return false;
            }
            if (item.type is ItemID.OrangePhaseblade or ItemID.OrangePhasesaber)
            {
                for (int a = 0; a < 5; a++)
                {
                    Vector2 Pvelocity = Utils.RotatedBy(velocity, MathHelper.TwoPi / 5 * a, default);
                    int A = NewProjectile(source, position, Pvelocity, type, damage, knockback, player.whoAmI, 0f, 0f);
                    Main.projectile[A].DProj().Times[4] = MathHelper.TwoPi / 5 * a;
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(item.shoot);
            writer.Write(item.shootSpeed);
            writer.Write(item.useStyle);
            writer.Write(item.noUseGraphic);
            writer.Write(item.noMelee);
            writer.Write(item.channel);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            item.shoot = reader.ReadInt32();
            item.shootSpeed = reader.ReadFloat();
            item.useStyle = reader.ReadInt32();
            item.noUseGraphic = reader.ReadBoolean();
            item.noMelee = reader.ReadBoolean();
            item.channel = reader.ReadBoolean();
        }
        public void Switch(Item item, Player player, int Shoot, float ShootSpeed)
        {
            if (item.shoot == ProjectileID.None)
            {
                item.hammer = 0;
                item.axe = 0;
                item.pick = 0;
                item.shoot = Shoot;
                item.shootSpeed = ShootSpeed;
                item.useStyle = ItemUseStyleID.Rapier;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.channel = true;
                CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), new Color(200, 100, 255), Language.GetTextValue("Mods.DDmod.ItemTips.BattleMode"), false, false);
                
            }
            else
            {
                item.hammer = OriginalHammer(item);
                item.axe = OriginalAxe(item);
                item.pick = OriginalPick(item);
                item.shoot = ProjectileID.None;
                item.shootSpeed = 0;
                item.useStyle = ItemUseStyleID.Swing;
                item.noUseGraphic = false;
                item.noMelee = false;
                item.channel = false;
                CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), new Color(200, 100, 255), Language.GetTextValue("Mods.DDmod.ItemTips.ToolMode"), false, false);
            }
        }


        public override bool AltFunctionUse(Item item, Player player)
        {
            if (!ModContent.GetInstance<DDConfigServer>().MeleeRework)
            {
                if (SpecialAttack)
                {
                    if (!player.HasBuff(ModContent.BuffType<SpecialAttackCD>()) || item.type == ItemID.ZombieArm)
                    {
                        return true;
                    }
                }
                return base.AltFunctionUse(item, player);
            }
            //血肉锤
            if (item.type == ItemID.FleshGrinder)
            {
                Switch(item, player, ModContent.ProjectileType<FleshGrinder>(), 10);
                return true;
            }
            //暗夜战斧
            if (item.type == ItemID.WarAxeoftheNight)
            {
                Switch(item, player, ModContent.ProjectileType<WarAxeoftheNight>(), 10);
                return true;
            }
            //暗夜战斧
            if (item.type == ItemID.TheBreaker)
            {
                Switch(item, player, ModContent.ProjectileType<TheBreaker>(), 10);
                return true;
            }
            if (SpecialAttack)
            {
                if(!player.HasBuff(ModContent.BuffType<SpecialAttackCD>())|| item.type == ItemID.ZombieArm)
                {
                    return true;
                }
            }
            return base.AltFunctionUse(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (SpecialAttack)
            {
                R += 1f;
                tooltips.Add(new TooltipLine(Mod, "特殊武器", Language.GetTextValue("Mods.DDmod.Tooltips.Rightattack"))
                {
                    OverrideColor = new Color(255, 255, 255)
                });
            }
            if (RightSwitch)
            {
                R += 1f;
                tooltips.Add(new TooltipLine(Mod, "特殊武器", Language.GetTextValue("Mods.DDmod.Tooltips.RightTransform"))
                {
                    OverrideColor = new Color(255, 255, 255)
                });
            }
        }
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (item.type == 757)
            {
                spriteBatch.Draw(TextureAssets.Item[item.type].Value, position, frame, drawColor, 0, origin, scale*1.3F, 0, 0);
                return false;
            }
            return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (item.type == 757)
            {
                Texture2D texture = TextureAssets.Item[item.type].Value;
                spriteBatch.Draw(texture, item.position+new Vector2(item.width/2,item.height)-Main.screenPosition, null, alphaColor, rotation, new Vector2(texture.Width/2, texture.Height-10), scale, 0, 0);
                return false;
            }
            return base.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override Color? GetAlpha(Item item, Color lightColor)
        {
            if(item.type==757)
            {
                return Color.White;
            }
            return null;
        }
        public static float R;
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "特殊武器" && line.Mod == "DDmod")
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
                R += 3f;
                GameShaders.Misc["渲染滤镜"].UseOpacity(2);
                GameShaders.Misc["渲染滤镜"].SetShaderTexture(ModContent.Request<Texture2D>("DDmod/Image/heatmap"));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uImageSize1"].SetValue(ModContent.Request<Texture2D>("DDmod/Image/heatmap").Size()*5);
                GameShaders.Misc["渲染滤镜"].UseColor(Color.White);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uColor2"].SetValue(Color.White.ToVector3() * 0.75f);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["renderTargetArea"].SetValue(new Vector2(5, 5));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uWorldPosition"].SetValue(Vector2.Zero);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["position"].SetValue(new Vector2(R, 0));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["ImageSize"].SetValue(ModContent.Request<Texture2D>("DDmod/Image/heatmap").Size() *5);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["upscaleFactor"].SetValue(new Vector2(-0.7F));
                GameShaders.Misc["渲染滤镜"].Apply();

                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), Color.White, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
                //return false;
            }
            return true;
        }
        public float swingAngle = -1;
        /// 飞刀使用动画
        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            if(item.useStyle==FlyingKnife)
            {
                if (player.ItemAnimationJustStarted && player.whoAmI == Main.myPlayer)
                {
                    player.Dplayer().swingAngle = ((player.Dplayer().MouseWorld - player.MountedCenter) * new Vector2(1, player.gravDir)).ToRotation();
                }
                float percentDone = 1 - (float)player.itemAnimation / player.itemAnimationMax;
                if (percentDone < 0.5F)
                {
                    float currentAngle = 4 * (percentDone*2 - 0.9F) * player.direction;
                    player.PlayerAction().PlayerArmRotation(player.Dplayer().swingAngle - MathHelper.PiOver2 + currentAngle, Player.CompositeArmStretchAmount.Full);
                }
                else
                {
                    float currentAngle = 4 * (0.1F+ percentDone/10) * player.direction;
                    player.PlayerAction().PlayerArmRotation(player.Dplayer().swingAngle - MathHelper.PiOver2 + currentAngle, Player.CompositeArmStretchAmount.Full);
                }
                /*
                if (player.direction > 0)
                {
                    player.itemRotation = currentAngle+MathHelper.PiOver2;
                }
                else
                {
                    player.itemRotation = currentAngle;
                }

                player.itemLocation = player.MountedCenter;
                player.FlipItemLocationAndRotationForGravity();
                */
            }
        }
        public override void UseItemFrame(Item item, Player player)
        {
            if (item.useStyle == FlyingKnife)
                player.bodyFrame.Y = player.bodyFrame.Height;
        }

    }
}