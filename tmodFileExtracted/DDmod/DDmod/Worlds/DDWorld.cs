using DDmod.Content.Items.Ammo;
using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Items.Magic;
using DDmod.Content.Items.Melee.FlyingKnife.Make;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Melee.SwordShield;
using DDmod.Content.Items.Melee.Tool.Hoe;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Ranged.Make.Gun;
using DDmod.Content.Items.Series.杂物;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Items.Series.钢;
using DDmod.Content.Items.Summon;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Content.Items.农场.水壶;
using DDmod.Content.Items.农场.种子;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.TownNPC;
using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.农场;
using DDmod.Content.Tiles.杂物块.Walls;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Tiles.绿岩.家具;
using DDmod.Content.Tiles.草;
using DDmod.NoContent.Config;
using Iced.Intel;
using Microsoft.Build.Evaluation;
using StructureHelper;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static DDmod.Content.NPCs.Boss.LifeGuardLes.LifeGuard;
using static DDmod.Content.NPCs.Boss.MeteorDigger.MeteorDigger_Head;
using static DDmod.Content.NPCs.Boss.StarGuardBulan.StarGuard;

namespace DDmod.Worlds
{
    public class DDWorld : ModSystem
    {
        public static 锄土[] 土 = new 锄土[1000];
        public static void Read(BinaryReader reader)
        {
            Point16 vector = new Point16(reader.ReadInt16(), reader.ReadInt16());
            int r = reader.ReadInt16();
            土[r].tiles = vector;
            土[r].Who = r;
            土[r].DampTime = reader.ReadInt32();
            if (Main.netMode == 2)
            {
                ModPacket packet = DDmod.Instance.GetPacket(256);
                //写入要发的包
                packet.Write((byte)DDType.锄地);
                packet.Write(vector.X);
                packet.Write(vector.Y);
                packet.Write((short)r);
                packet.Write(土[r].DampTime);
                //发出去
                packet.Send(-1, -1);
            }
            //NetMessage.SendData(MessageID.WorldData);
        }
        public override void Load()
        {
            if (Main.netMode != 2)
            {
                asset[0] = TextureAssets.Npc[35];
                asset[1] = TextureAssets.Npc[36];
                asset[2] = TextureAssets.BoneArm;
            }
        }

        public static bool 开发者模式 = false;
        public static bool 诅咒之火 = false;
        public static bool 晨曦 = false;
        public override void OnWorldLoad()
        {
            if (土 == null)
            {
                土 = new 锄土[1000];
            }
            for (int A = 0; A < 土.Length; A++)
            {
                土[A] = new 锄土(0, 0);
            }
            if (Main.netMode != 2 && Main.worldName == "牢大")
            {
                TextureAssets.Npc[35] = ModContent.Request<Texture2D>("DDmod/Textures/牢大");
                TextureAssets.Npc[36] = ModContent.Request<Texture2D>("DDmod/Textures/手掌");
                TextureAssets.BoneArm = ModContent.Request<Texture2D>("DDmod/Textures/牢大手");
            }
            开发者模式 = false;
            诅咒之火 = false;
            晨曦 = false;
        }
        public static Asset<Texture2D>[] asset = new Asset<Texture2D>[3];
        public override void OnWorldUnload()
        {
            if (Main.netMode != 2)
            {
                TextureAssets.Npc[35] = asset[0];
                TextureAssets.Npc[36] = asset[1];
                TextureAssets.BoneArm = asset[2];
            }
            开发者模式 = false;
            诅咒之火 = false;
            晨曦 = false;
        }
        public static Color SunColor;
        public static float SunLight = 1;
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (SunColor != new Color(0, 0, 0, 0))
            {
                backgroundColor = SunColor;
                tileColor = SunColor;
                SunColor = new Color(0, 0, 0, 0);
            }
            if (SunLight != 1)
            {
                byte B = backgroundColor.A;
                byte T = tileColor.A;
                backgroundColor *= SunLight;
                tileColor *= SunLight;
                backgroundColor.A = B;
                tileColor.A = T;
                if (!Main.gamePaused)
                {
                    if (SunLight < 1)
                    {
                        SunLight += 0.05F;
                    }
                    else 
                    {
                        SunLight = 1;
                    }
                }
            }
            base.ModifySunLightColor(ref tileColor, ref backgroundColor);
        }
        public static float SunLightScale = 1;
        public override void ModifyLightingBrightness(ref float scale)
        {
            if (SunLightScale != 1)
            {
                float r = scale;
                scale = SunLightScale;
                if (!Main.gamePaused)
                {
                    if (SunLightScale > r)
                    {
                        SunLightScale -= 0.05F;
                    }
                    else
                    {
                        SunLightScale += 0.05F;
                    }
                    if (Math.Abs(SunLightScale - r) <= 0.06F)
                    {
                        SunLightScale = 1;
                    }
                }
            }
        }
        static Item[] items;
        public Vector2[] itemPo;
        public bool[] ac;
        public int Count = 0;
        public override void SaveWorldHeader(TagCompound tag)
        {
            if (晨曦) tag["晨曦"] = true;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            if (Main.item != null)
            {
                items = Main.item;
                if (itemPo == null || itemPo.Length != items.Length)
                {
                    itemPo = new Vector2[items.Length];
                }
                for (int a = 0; a < itemPo.Length; a++)
                {
                    itemPo[a] = items[a].position;
                }
                if (ac == null || ac.Length != items.Length)
                {
                    ac = new bool[items.Length];
                }
                for (int a = 0; a < ac.Length; a++)
                {
                    ac[a] = items[a].active;
                }
            }
            tag.Add("WorldItems", items);
            tag.Add("WorldItemsPo", itemPo);
            tag.Add("WorldItemsAc", ac);
            tag.Add("GreenRockLabX", GreenRockLab.X);
            tag.Add("GreenRockLabY", GreenRockLab.Y);
            if (土 == null || 土.Length < 1000)
            {
                土 = new 锄土[1000];
            }
            for (int a = 0; a < 土.Length; a++)
            {
                if (土[a] == null)
                {
                    土[a] = new 锄土(0, 0);
                }
                土[a].SaveWorldData(a, tag);
            }
            if (晨曦) tag["晨曦"] = true;
            if (开发者模式) tag["开发者模式"] = true;
            if (诅咒之火) tag["诅咒之火"] = true;

        }
        public override void LoadWorldData(TagCompound tag)
        {
            Main.item = tag.Get<Item[]>("WorldItems");
            for (int a = 0; a < tag.Get<Vector2[]>("WorldItemsPo").Length; a++)
            {
                if (Main.item[a] != null && Main.item[a].type > 0)
                {
                    Main.item[a].position = tag.Get<Vector2[]>("WorldItemsPo")[a];
                }
            }
            for (int a = 0; a < tag.Get<bool[]>("WorldItemsAc").Length; a++)
            {
                if (Main.item[a] != null && Main.item[a].type > 0)
                {
                    Main.item[a].active = tag.Get<bool[]>("WorldItemsAc")[a];
                }
            }
            GreenRockLab = new Point16(tag.Get<short>("GreenRockLabX"), tag.Get<short>("GreenRockLabY"));
            for (int a = 0; a < 土.Length; a++)
            {
                if (土[a] == null)
                {
                    土[a] = new 锄土(0, 0);
                }
                土[a].LoadWorldData(a, tag);
            }
            开发者模式 = tag.ContainsKey("开发者模式");
            诅咒之火 = tag.ContainsKey("诅咒之火");
            晨曦 = tag.ContainsKey("晨曦");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(GreenRockLab.X);
            writer.Write(GreenRockLab.Y);
            for (int a = 0; a < 土.Length; a++)
            {
                土[a].NetSend(writer);
            }
            BitsByte flags = new BitsByte();
            flags[0] = 开发者模式;
            flags[1] = 诅咒之火;
            flags[2] = 晨曦;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            GreenRockLab = new Point16(reader.ReadInt16(), reader.ReadInt16());
            for (int a = 0; a < 土.Length; a++)
            {
                土[a].NetReceive(reader);
            }
            BitsByte flags = reader.ReadByte();
            开发者模式 = flags[0];
            诅咒之火 = flags[1];
            晨曦 = flags[2];
        }
        public override void PreUpdateInvasions()
        {
        }
        public override void PreUpdateTime()
        {
            for (int a = 0; a < 土.Length; a++)
            {
                if (土[a].tiles != Point16.Zero)
                {
                    土[a].Update(a);
                }
            }
        }
        public override void PreUpdateWorld()
        {
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.Add(new 处理部分细节("处理部分细节", 237.4298f));
            tasks.Add(new 天上的武器("正在向箱子添加更多战利品", 237.4298f));
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Oasis"));

            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1,new 小屋("正在生成NPC小屋", 237.4298f));
            }
            /* ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Dungeon"));

             if (ShiniesIndex != -1)
             {
                 tasks.Insert(ShiniesIndex + 1, new 绿岩实验室("正在生成绿岩实验室", 237.4298f));
             }*/

            //ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Statues"));
            ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new 绿岩实验室("正在生成绿岩实验室", 237.4298f));
            }


            for (int a = 0; a < Main.item.Length; a++)
            {
                Main.item[a].active = false;
            }
        }
        public static Point16 GreenRockLab;

    }


    public class 处理部分细节 : GenPass
    {
        public 处理部分细节(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "处理部分细节";
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (Main.rand.NextBool(12))
                    {
                        Tile tile = Main.tile[i, j];
                        if (tile.TileType == 314 && tile.HasTile)
                        {
                            bool R = false;
                            for (int y = 1; y < 20; y++)
                            {
                                Tile tile2 = Main.tile[i, j - y];
                                if (!R && tile2.HasTile && !WorldGen.SolidTile(i, j - y))
                                {
                                    if (tile.TileType != 314)
                                        tile2.HasTile = false;
                                }
                                if (!R && tile2.HasTile)
                                {
                                    tile2.Slope = 0;
                                    WorldGen.PlaceTile(i, j - y + 1, (ushort)42, Main.rand.NextBool(5), style: 2);
                                    R = true;
                                }
                            }
                        }
                    }
                }
            }
            for (int a = 10; a < 38; a++)
            {
                WorldGen.KillTile(小屋.XDpo.X + a, 小屋.XDpo.Y-1);
            }
        }
    }
    public class 天上的武器 : GenPass
    {
        public 天上的武器(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "正在向箱子添加更多战利品";
            List<int> 木箱 = new List<int>();
            List<int> 空岛 = new List<int>();
            List<int> 冰雪 = new List<int>();
            List<int> 地牢 = new List<int>();
            List<int> 地狱暗影箱 = new List<int>();
            List<int> 丛林箱 = new List<int>();

            List<int> 地下金箱 = new List<int>();
            List<int> 洞穴金箱 = new List<int>();
            List<int> 岩浆金箱 = new List<int>();

            for (int a = 0; a < Main.chest.Length; a++)
            {
                if (Main.chest[a] != null && Main.chest[a].x > 0 && Main.chest[a].x < Main.maxTilesX && Main.chest[a].y > 0 && Main.chest[a].y < Main.maxTilesY)
                {
                    Tile tile = Main.tile[Main.chest[a].x, Main.chest[a].y];
                    if (tile.TileType == 21)
                    {
                        if (tile.WallType == 82)
                        {
                            空岛.Add(a);
                        }
                        if (tile.TileFrameX == 36 * 11)
                        {
                            冰雪.Add(a);
                        }
                        if (((tile.WallType >= 94 && tile.WallType <= 105) || (tile.WallType >= 7 && tile.WallType <= 9)))
                        {
                            if (tile.TileFrameX == 36 * 2)
                            {
                                地牢.Add(a);
                            }
                        }
                        else
                        {
                            if (tile.TileFrameX == 0)
                            {
                                木箱.Add(a);
                            }
                        }
                        if (tile.TileFrameX == 36 * 4 && Main.chest[a].y > Main.UnderworldLayer)
                        {
                            地狱暗影箱.Add(a);
                        }
                        if (tile.TileFrameX == 36 * 10)
                        {
                            丛林箱.Add(a);
                        }
                        if (tile.TileFrameX == 36 * 1)
                        {
                            if (Main.chest[a].y >= GenVars.worldSurfaceHigh)
                            {
                                if (Main.chest[a].y <= GenVars.rockLayerHigh)
                                    地下金箱.Add(a);
                                else if (Main.chest[a].y < GenVars.lavaLine)
                                    洞穴金箱.Add(a);
                                else
                                    岩浆金箱.Add(a);

                            }
                        }
                    }
                }
            }
            宝箱(木箱, ModContent.ItemType<WoodSpiritSwordItem>());
            宝箱(空岛, ModContent.ItemType<星空>());
            宝箱(空岛, ModContent.ItemType<奇异的云>());
            宝箱(冰雪, ModContent.ItemType<寒霜飞刀>());
            宝箱(冰雪, ModContent.ItemType<永恒冰晶>());
            宝箱(冰雪, ModContent.ItemType<寒霜冲锋枪>());
            宝箱(地牢, ModContent.ItemType<远古短刀>());
            宝箱(地牢, ModContent.ItemType<远古弓>());
            宝箱(地狱暗影箱, ModContent.ItemType<地狱葫芦>());
            宝箱(丛林箱, ModContent.ItemType<宣花葫芦>());

            if (WorldGen.SavedOreTiers.Copper == TileID.Copper)
            {
                宝箱(地下金箱, ModContent.ItemType<铜制手枪>(), [new Item(ModContent.ItemType<受潮弹>(),Main.rand.Next(50,150))]);
            }
            else
            {
                宝箱(地下金箱, ModContent.ItemType<锡制手枪>(), [new Item(ModContent.ItemType<受潮弹>(), Main.rand.Next(50, 150))]);
            }

            if (WorldGen.SavedOreTiers.Iron == TileID.Iron)
            {
                宝箱(洞穴金箱, ModContent.ItemType<铁制猎枪>(), [new Item(ModContent.ItemType<受潮弹>(),Main.rand.Next(150,350))]);
            }
            else
            {
                宝箱(洞穴金箱, ModContent.ItemType<铅制猎枪>(), [new Item(ModContent.ItemType<受潮弹>(), Main.rand.Next(150, 350))]);
            }

            if (WorldGen.SavedOreTiers.Silver == TileID.Silver)
            {
                宝箱(岩浆金箱, ModContent.ItemType<银制手枪>(), [new Item(97,Main.rand.Next(50,150))]);
            }
            else
            {
                宝箱(岩浆金箱, ModContent.ItemType<钨制手枪>(), [new Item(97, Main.rand.Next(50, 150))]);
            }
        }
        public void 宝箱(List<int> 宝箱数量, int type, Item[] items = null)
        {
            //冰雪宝箱
            if (宝箱数量.Count <= 0)
            {
                return;
            }
            int r = Main.rand.Next(0, 宝箱数量.Count);
            for (int B = 0; B < Main.chest[宝箱数量[r]].item.Length; B++)
            {
                if (Main.chest[宝箱数量[r]].item[B].type <= 0)
                {
                    Main.chest[宝箱数量[r]].item[B].SetDefaults(type);
                    Main.chest[宝箱数量[r]].item[B].Prefix(-1);
                    if (items != null)
                    {
                        for (int C = 0; C < items.Length; C++)
                        {
                            Main.chest[宝箱数量[r]].item[B+1 + C] = items[C];
                        }
                    }
                    宝箱数量.Remove(r);
                    break;
                }
            }
            if(宝箱数量.Count <= 2)
            {
                return;
            }
            int Count = 宝箱数量.Count / 3;
            if (Count == 0)
            {
                Count = 1;
            }
            for (int a = 0; a < Count; a++)
            {
                r = Main.rand.Next(1, 宝箱数量.Count);
                for (int B = 0; B < Main.chest[宝箱数量[r]].item.Length; B++)
                {
                    if (Main.chest[宝箱数量[r]].item[B].type == type)
                    {
                        a--;
                        break;
                    }
                    if (Main.chest[宝箱数量[r]].item[B].type <= 0)
                    {
                        Main.chest[宝箱数量[r]].item[B].SetDefaults(type);
                        Main.chest[宝箱数量[r]].item[B].Prefix(-1);
                        if(items!=null)
                        {
                            for(int C=0;C< items.Length;C++)
                            {
                                Main.chest[宝箱数量[r]].item[B+1+C] = items[C];
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    public class 绿岩实验室 : GenPass
    {
        bool rr;
        public 绿岩实验室(string name, float loadWeight) : base(name, loadWeight)
        {
            rr = false;
            if (name == "正在生成绿岩实验室2")
            {
                rr = true;
            }
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "正在生成绿岩实验室";
            List<Point16> vs = new List<Point16>();
            bool cg = false;
            void GreenRockLab(int L, out List<Point16> point)
            {
                List<Point16> Remove = new List<Point16>();
                if (Main.dungeonX < Main.maxTilesX / 2)
                {
                    for (int a = (int)(Main.maxTilesX * 0.65F); a < Main.maxTilesX * 0.8F; a++)
                    {
                        for (int b = (int)(Main.worldSurface * 0.5f); b < Main.maxTilesY; b++)
                        {
                            if ((Main.tile[a, b].HasTile && (Main.tile[a, b].TileType == 2 || Main.tile[a, b].TileType == 60)))
                            {
                                vs.Add(new Point16(a, b));
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int a = (int)(Main.maxTilesX * 0.2F); a < Main.maxTilesX * 0.35F; a++)
                    {
                        for (int b = (int)(Main.worldSurface * 0.5f); b < Main.maxTilesY; b++)
                        {
                            if ((Main.tile[a, b].HasTile && (Main.tile[a, b].TileType == 2 || Main.tile[a, b].TileType == 60)))
                            {
                                vs.Add(new Point16(a, b));
                                break;
                            }
                        }
                    }
                }
                for (int A = 0; A < vs.Count; A++)
                {
                    int R = 0;
                    for (int b = (int)(Main.worldSurface * 0.5f); b < Main.maxTilesY; b++)
                    {
                        if (Main.tile[vs[A].X + 173, b].HasTile && Main.tileSolid[Main.tile[vs[A].X + 173, b].TileType])
                        {
                            R = b;
                            break;
                        }
                    }
                    if (Math.Abs(vs[A].Y - R) > L)
                    {
                        Remove.Add(vs[A]);
                        continue;
                    }
                    for (int b = (int)(Main.worldSurface * 0.5f); b < Main.maxTilesY; b++)
                    {
                        if (vs[A].X - 12 <= 0 || vs[A].X - 12 >= Main.maxTilesX)
                        {
                            Remove.Add(vs[A]);
                            break;
                        }
                        if (Main.tile[vs[A].X - 12, b].HasTile && Main.tileSolid[Main.tile[vs[A].X - 12, b].TileType])
                        {
                            R = b;
                            break;
                        }
                    }
                    if (Math.Abs(vs[A].Y - R) > L)
                    {
                        Remove.Add(vs[A]);
                    }
                }
                for (int A = 0; A < Remove.Count; A++)
                {
                    vs.Remove(Remove[A]);
                }
                point = vs;
            }
            for (int A = 0; A < Main.maxTilesY; A++)
            {
                GreenRockLab(A, out List<Point16> PO);
                if (PO.Count > 0)
                {
                    vs = PO;
                    break;
                }
            }
            Point16 point = Main.rand.Next(vs);
            point = new Point16(point.X, point.Y - 26);
            if (rr)
            {
                point = DDWorld.GreenRockLab;
            }
            DDWorld.GreenRockLab = point;
            GenerateStructure("Worlds/绿岩实验室", point, DDmod.Instance);

            List<int> ints = [ModContent.ItemType<绿岩飞刀>(), ModContent.ItemType<绿岩狙击枪>(), ModContent.ItemType<绿岩聚能炮>(), ModContent.ItemType<绿岩工程扳手>()];

            MeteorBox(point.X + 122, point.Y + 82, false);
            MeteorBox(point.X + 149, point.Y + 82, false);
            MeteorBox(point.X + 11, point.Y + 85, false, 1);
            MeteorBox(point.X + 93, point.Y + 146, false, 2);
            MeteorBox(point.X + 103, point.Y + 146, false, 2);
            MeteorBox(point.X + 111, point.Y + 127, false, 3);
            MeteorBox(point.X + 119, point.Y + 127, false, 3);
            MeteorBox(point.X + 118, point.Y + 167, true, 4);
            WorldGen.PlaceObject(point.X + 123, point.Y + 127, 304, true, 0, 0, -1, -1);

            WorldGen.PlaceObject(point.X + 66, point.Y + 164, ModContent.TileType<绿岩干扰器Tile>(), true, 0, 0, -1, -1);
            for (int A = 0; A < 172; A++)
            {
                for (int B = 1; B < 40; B++)
                {
                    Tile tile = Main.tile[A + point.X, point.Y - B];
                    tile.TileType = 0;
                    tile.HasTile = false;
                    tile.WallType = 0;
                }
            }

            for (int A = 31; A < 160; A++)
            {
                int T = 273;
                for (int B = 33; B < 84; B++)
                {
                    Tile tile = Main.tile[A + point.X, B + point.Y];
                    if ((!tile.HasTile || !Main.tileSolid[tile.TileType]) && tile.WallType != ModContent.WallType<淡蓝钢墙Tile>() && tile.WallType != ModContent.WallType<绿岩砖墙Tile>() && tile.WallType != ModContent.WallType<绿岩格网墙Tile>())
                    {
                        tile.TileType = (ushort)T;
                        tile.HasTile = true;
                    }
                    else
                    {
                        Main.LocalPlayer.PickTile(A + point.X, B + point.Y, 1);
                        T = tile.TileType;
                    }
                }

            }
            for (int A = 31; A < 160; A++)
            {
                int W = 147;
                for (int B = 33; B < 84; B++)
                {
                    Tile tile = Main.tile[A + point.X, B + point.Y];
                    Tile tile2 = Main.tile[A + point.X + 1, B + point.Y];
                    Tile tile3 = Main.tile[A + point.X - 1, B + point.Y];
                    Tile tile4 = Main.tile[A + point.X, B + point.Y + 1];
                    Tile tile5 = Main.tile[A + point.X, B + point.Y - 1];

                    //if (tile.HasTile&&tile.TileType==273&&tile2.TileType==273&&tile3.TileType==273&&tile4.TileType==273&&tile5.TileType==273)
                    if (tile2.HasTile && Main.tileSolid[tile2.TileType] && tile3.HasTile && Main.tileSolid[tile3.TileType] && tile4.HasTile && Main.tileSolid[tile4.TileType] && tile5.HasTile && Main.tileSolid[tile5.TileType])
                    {
                        if (tile.HasTile && tile.TileType == 273)
                        {
                            tile.WallType = 147;
                        }
                        else
                        {
                            if (tile.WallType > 0)
                            {
                                W = tile.WallType;
                            }
                            if (Main.tileSolid[tile.TileType] && tile.HasTile)
                            {
                                tile.WallType = (ushort)W;
                            }
                        }
                        tile.Slope = 0;
                        tile.IsHalfBlock = false;
                    }
                }

            }
            for (int A = 100; A < Main.maxTilesX - 100; A++)
            {
                for (int B = 100; B < Main.maxTilesY - 100; B++)
                {
                    Tile tile = Main.tile[A, B];
                    Tile tile2 = Main.tile[A + 1, B];
                    if (tile.HasTile && tile2.HasTile && Main.rand.NextBool(10))
                    {
                        if (tile.TileType == ModContent.TileType<绿岩砖Tile>() || tile.TileType == ModContent.TileType<绿岩格网块Tile>() || tile.TileType == ModContent.TileType<绿岩平台Tile>())
                        {
                            Tile tile3 = Main.tile[A, B - 1];
                            Tile tile4 = Main.tile[A, B - 2];
                            Tile tile5 = Main.tile[A + 1, B - 1];
                            Tile tile6 = Main.tile[A + 1, B - 2];
                            if (!tile3.HasTile && !tile4.HasTile && !tile5.HasTile && !tile6.HasTile)
                            {
                                int style = Main.rand.Next(0, 3);
                                WorldGen.PlaceObject(A, B - 1, ModContent.TileType<绿岩罐子Tile>(), true, style, 0, -1, -1);

                                if (Main.rand.NextBool(2))
                                {
                                    tile3.TileFrameY += (short)(36);
                                    tile4.TileFrameY += (short)(36);
                                    tile5.TileFrameY += (short)(36);
                                    tile6.TileFrameY += (short)(36);
                                }
                            }
                        }
                    }
                }
            }

            void MeteorBox(int x, int y, bool R, int Type = 0)
            {
                int A = ModContent.TileType<绿岩箱Tile>();
                if (R)
                {
                    A = ModContent.TileType<绿岩存储仓Tile>();
                }
                int PlacementSuccess = WorldGen.PlaceChest(x, y, (ushort)A, false, R ? 1 : 0);
                if (PlacementSuccess >= 0)
                {
                    Chest chest = Main.chest[PlacementSuccess];

                        int Citem = 0;
                    if (Type == 0)
                    {
                        chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩钥匙>(), false);
                        chest.item[Citem].stack = 1;
                        Citem++;

                    }
                    if (Type == 1)
                    {
                        chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩门禁卡>(), false);
                        chest.item[Citem].stack = 1;
                        Citem++;
                    }
                    if (Type == 3)
                    {
                        chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩信号增幅器>(), false);
                        chest.item[Citem].stack = 1;
                        Citem++;
                    }
                    if (Type == 4)
                    {
                        chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩设计图>(), false);
                        chest.item[Citem].stack = 1;
                        Citem++;
                    }
                    if (Type != 4)
                    {

                        if (Type != 0)
                        {
                            if (ints.Count > 0)
                            {
                                int item = Main.rand.Next(ints);
                                chest.item[Citem].SetDefaults(item, false);
                                chest.item[Citem].Prefix(-1);
                                chest.item[Citem].stack = 1;
                                ints.Remove(item);
                                Citem++;
                            }
                            else
                            {
                                int item = Main.rand.Next([ModContent.ItemType<绿岩飞刀>(), ModContent.ItemType<绿岩狙击枪>(), ModContent.ItemType<绿岩聚能炮>(), ModContent.ItemType<绿岩工程扳手>()]);
                                chest.item[Citem].SetDefaults(item, false);
                                chest.item[Citem].Prefix(-1);
                                chest.item[Citem].stack = 1;
                                Citem++;
                            }
                        }
                        chest.item[Citem].SetDefaults(1922, false);
                        chest.item[Citem].stack = Main.rand.Next(10, 31);
                        Citem++;
                        if (Main.rand.NextBool(3))
                        {
                            chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩锭>(), false);
                            chest.item[Citem].stack = Main.rand.Next(4, 11);
                            Citem++;
                        }
                        else
                        {
                            chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩晶石>(), false);
                            chest.item[Citem].stack = Main.rand.Next(10, 21);
                            Citem++;
                        }
                        if (Main.rand.NextBool(2))
                        {
                            chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩弹>(), false);
                            chest.item[Citem].stack = Main.rand.Next(80, 161);
                            Citem++;
                        }
                        else
                        {
                            chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩箭>(), false);
                            chest.item[Citem].stack = Main.rand.Next(80, 161);
                            Citem++;
                        }
                        if (Main.rand.NextBool(2))
                        {
                            chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩砖>(), false);
                            chest.item[Citem].stack = Main.rand.Next(80, 161);
                            Citem++;
                        }
                        else
                        {
                            chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩格网块>(), false);
                            chest.item[Citem].stack = Main.rand.Next(80, 161);
                            Citem++;
                        }
                    }
                    else
                    {
                        chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩剑盾>(), false);
                        chest.item[Citem].Prefix(-1);
                        chest.item[Citem].stack = 1;
                        Citem++;
                        chest.item[Citem].SetDefaults(1922, false);
                        chest.item[Citem].stack = Main.rand.Next(30, 81);
                        Citem++;
                        chest.item[Citem].SetDefaults(ModContent.ItemType<钢锭>(), false);
                        chest.item[Citem].stack = Main.rand.Next(5, 13);
                        Citem++;
                        chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩锭>(), false);
                        chest.item[Citem].stack = Main.rand.Next(15, 31);
                        Citem++;
                        chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩砖>(), false);
                        chest.item[Citem].stack = Main.rand.Next(180, 361);
                        Citem++;
                        chest.item[Citem].SetDefaults(ModContent.ItemType<绿岩格网块>(), false);
                        chest.item[Citem].stack = Main.rand.Next(180, 361);
                        Citem++;
                    }
                }
            }
        }
    }

    public class 小屋 : GenPass
    {
        /// <summary>
        /// 向导小屋位置
        /// </summary>
        public static Point16 XDpo;
        public 小屋(string name, float loadWeight) : base(name, loadWeight)
        {
        }
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "正在生成NPC小屋";
            Point16 point = new Point16(0, 0);
            //猎人的小屋
            List<Point16> vs = new List<Point16>();
            for (int a = 0; a < Main.maxTilesX; a++)
            {
                for (int b = 0; b < Main.maxTilesY; b++)
                {
                    if (Main.tile[a, b].LiquidAmount>10)
                    {
                        break;
                    }
                    else
                    if (Main.tile[a, b].HasTile)
                    {
                        if (Main.tile[a, b].TileType == 147)
                        {
                            vs.Add(new Point16(a, b - 13));
                        }
                        break;
                    }
                }
            }
            point = Main.rand.Next(vs);
            for (int a = 0; a <= 17; a++)
            {
                for (int b = -1; b < 20; b++)
                {
                    WorldGen.KillTile(a, b);
                }
            }
            GenerateStructure("Worlds/猎人小屋", point, DDmod.Instance);
            int L = 8;
            for (int a = -L; a <= L; a++)
            {
                for (int b = 0; b < 20; b++)
                {
                    Tile tile = Main.tile[point.X + a + L, point.Y + 13 + b];
                    if (tile.HasTile && tile.TileType != 5 && b > 1)
                    {
                        break;
                    }
                    if (!tile.HasTile || tile.TileType == 5)
                    {
                        tile.TileType = 147;
                        tile.Slope = 0;
                        tile.HasTile = true;
                    }
                }
                for (int b = 0; b < 20; b++)
                {
                    Tile tile = Main.tile[point.X + a + L, point.Y - b];
                    if (tile.HasTile || tile.TileType == 5)
                    {
                        tile.TileType = 147;
                        tile.Slope = 0;
                        tile.HasTile = false;
                    }
                }
            }
            NPC.NewNPC(new EntitySource_WorldGen(), point.X * 16, point.Y * 16, ModContent.NPCType<HunterSlime2>());
            //向导的小屋
            vs = new List<Point16>();
            Point16 po = new Point16(Main.maxTilesX / 2 + 30, (int)(Main.worldSurface * 0.5f));
            int Y = 0;
            for (int b = po.Y; b < Main.maxTilesY; b++)
            {
                if (Main.tile[po.X, po.Y + b].LiquidAmount > 0)
                {
                    po = new Point16(Main.maxTilesX / 2 + 60, (int)(Main.worldSurface * 0.5f));
                }
                if (Main.tile[po.X, Y+b].HasTile)
                {
                    Y = b-9;
                    break;
                }
            }
            XDpo = point = new Point16(po.X, Y);
            for (int a = 0; a < 38; a++)
            {
                for (int b = 0; b < 11; b++)
                {
                    WorldGen.KillTile(point.X+a, point.Y+b);
                }
            }
            GenerateStructure("Worlds/向导小屋", point, DDmod.Instance);
            for (int a = 0; a < 38; a++)
            {
                for (int b = 0; b < Main.maxTilesY; b++)
                {
                    Tile tile = Main.tile[point.X + a, point.Y + 11 + b];
                    if (tile.HasTile && Main.tileSolid[tile.TileType])
                    {
                        tile.Slope = 0;
                        tile.IsHalfBlock = false;
                        WorldGen.SquareTileFrame(point.X + a, point.Y + 11 + b, true);
                        break;
                    }
                    tile.TileType = 0;
                    tile.HasTile = true;
                    WorldGen.SquareTileFrame(point.X + a, point.Y + 11 + b, true);
                }
            }
            MeteorBox(point.X + 19, point.Y + 8);
            void MeteorBox(int x, int y)
            {
                int PlacementSuccess = WorldGen.PlaceChest(x, y, (ushort)21, false, 0);
                if (PlacementSuccess >= 0)
                {

                    Random ran = new();
                    Chest chest = Main.chest[PlacementSuccess];
                    int Citem = 0;
                    if (Main.rand.NextBool(20))
                    {
                        chest.item[Citem].SetDefaults(ModContent.ItemType<木制剑盾>(), false);
                        chest.item[Citem].Prefix(-1);
                        chest.item[Citem].stack = 1;
                        Citem++;
                    }
                    chest.item[Citem].SetDefaults(Main.rand.NextBool(2) ? ModContent.ItemType<向导的纸条>() : ModContent.ItemType<向导的纸条2>(), false);
                    chest.item[Citem].stack = 1;
                    Citem++;
                    chest.item[Citem].SetDefaults(ModContent.ItemType<木锄头>(), false);
                    chest.item[Citem].stack = 1;
                    Citem++;
                    chest.item[Citem].SetDefaults(ModContent.ItemType<木水壶>(), false);
                    chest.item[Citem].stack = 1;
                    Citem++;
                    chest.item[Citem].SetDefaults(ModContent.ItemType<胡萝卜种子>(), false);
                    chest.item[Citem].stack = Main.rand.Next(3,11);
                    Citem++;
                    chest.item[Citem].SetDefaults(ModContent.ItemType<生菜种子>(), false);
                    chest.item[Citem].stack = Main.rand.Next(3,11);
                    Citem++;
                    chest.item[Citem].SetDefaults(ModContent.ItemType<玉米种子>(), false);
                    chest.item[Citem].stack = Main.rand.Next(2,6);
                    Citem++;
                    chest.item[Citem].SetDefaults(271, false);
                    chest.item[Citem].stack = 1;
                    Citem++;
                    chest.item[Citem].SetDefaults(269, false);
                    chest.item[Citem].stack = 1;
                    Citem++;
                    chest.item[Citem].SetDefaults(270, false);
                    chest.item[Citem].stack = 1;
                    Citem++;
                    chest.item[Citem].SetDefaults(149, false);
                    chest.item[Citem].stack = ran.Next(2, 6);
                    Citem++;
                    chest.item[Citem].SetDefaults(9, false);
                    chest.item[Citem].stack = ran.Next(60, 120);
                    Citem++;
                    chest.item[Citem].SetDefaults(8, false);
                    chest.item[Citem].stack = ran.Next(10, 20);
                    Citem++;
                    chest.item[Citem].SetDefaults(27, false);
                    chest.item[Citem].stack = ran.Next(4, 8);
                    Citem++;
                    chest.item[Citem].SetDefaults(28, false);
                    chest.item[Citem].stack = ran.Next(3, 8);
                    Citem++;
                    chest.item[Citem].SetDefaults(110, false);
                    chest.item[Citem].stack = ran.Next(3, 8);
                    Citem++;
                }
            }

        }
    }
}