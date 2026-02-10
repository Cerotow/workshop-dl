using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.天地守卫;
using DDmod.Content.NPCs.Boss.星心守卫;
using DDmod.Worlds;
using StructureHelper;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DDmod.Content.Tiles.流星;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Melee.Sword.Make;
using DDmod.Content.Items.Melee.FlyingKnife.Make;
using DDmod.Content.Items.Summon;
using StructureHelper.API;
using DDmod.Content.Items.Melee.SwordShield;
using DDmod.Content.Items.Melee.Sword;
using Terraria.WorldBuilding;
using DDmod.Content.Tiles.绿岩;

namespace DDmod.Content.Items
{
    public class 黎明Mod内部功能启动器 : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 3200;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
            /*
            Main.NewText(Main.maxTilesX / 2);
            Main.NewText((int)Main.LocalPlayer.position.X/16);
            Main.NewText((int)Main.LocalPlayer.position.Y/16);
            Main.NewText((int)(Main.worldSurface * 0.5f) + 20);*/
        }

        public override bool? UseItem(Player player)
        {
            //Main.hardMode = !Main.hardMode;
            DDWorld.晨曦 = !DDWorld.晨曦;
            DDWorld.开发者模式 = true;
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
            }
            Point16 po = new Point16((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16);
            Vector2 vector = new Vector2(Main.MouseWorld.X, (int)Main.MouseWorld.Y);
            /*for (int A = 0; A < 5; A++)
            {
                DNPC.NewNPCs(player.GetSource_FromAI(), vector + new Vector2(Main.rand.NextFloat(-100, 100), 0), 111,0);
                DNPC.NewNPCs(player.GetSource_FromAI(), vector + new Vector2(Main.rand.NextFloat(-100, 100), 0), 79,0);
            }*/
            //NPCDowned.MeteorTower = false;
           // Main.NewText(NPCDowned.MeteorTower);
            WorldGen.dropMeteor();
            return true;
            int Y = 0;
            for (int b = po.Y; b > 0; b--)
            {
                if (Main.tile[po.X, po.Y + b].LiquidAmount > 0)
                {
                    po = new Point16((int)Main.MouseWorld.X / 16 + 60, (int)Main.MouseWorld.Y / 16);
                }
                if (!Main.tile[po.X, po.Y + b].HasTile)
                {
                    Y = b;
                    break;
                }
            }
            for (int b = po.Y; b < Main.maxTilesY; b++)
            {
                if (Main.tile[po.X, Y + b].HasTile)
                {
                    Y = b -26;
                    break;
                }
            }
            Point16 point = new Point16(po.X, po.Y-26);
            GenerateStructure("Worlds/绿岩实验室", point, DDmod.Instance);
            return true;
            /*
            //向导的小屋
            Point16 po = new Point16((int)Main.MouseWorld.X/16, (int)Main.MouseWorld.Y/16);
            int Y = 0;
            for (int b = po.Y; b > 0; b--)
            {
                if (Main.tile[po.X, po.Y + b].LiquidAmount > 0)
                {
                    po = new Point16((int)Main.MouseWorld.X / 16+60, (int)Main.MouseWorld.Y / 16);
                }
                if (!Main.tile[po.X, po.Y + b].HasTile)
                {
                    Y = b;
                    break;
                }
            }
            for (int b = po.Y; b < Main.maxTilesY; b++)
            {
                if (Main.tile[po.X, Y + b].HasTile)
                {
                    Y = b - 10;
                    break;
                }
            }
           Point16  point = new Point16(po.X, Y);
            for (int a = 0; a <= 38; a++)
            {
                for (int b = 0; b < 11; b++)
                {
                    WorldGen.KillTile(point.X + a, point.Y + b);
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
            MeteorBox(point.X + 19, point.Y + 8);//NoBoss 最后一个箱子,如果没有boss召唤物强行生成一个
            void MeteorBox(int x, int y)
            {
                int PlacementSuccess = WorldGen.PlaceChest(x, y, (ushort)21, false, 0);
                if (PlacementSuccess >= 0)
                {

                    Random ran = new();
                    Chest chest = Main.chest[PlacementSuccess];
                    int Citem = 0;
                    //chest.item[Citem].SetDefaults(ModContent.ItemType<可疑外星蓝图>(), false);
                    //chest.item[Citem].stack = 1;
                    //Citem++;
                    if (Main.rand.NextBool(20))
                    {
                        chest.item[Citem].SetDefaults(ModContent.ItemType<木制剑盾>(), false);
                        chest.item[Citem].stack = 1;
                        Citem++;
                    }
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
                NetMessage.SendObjectPlacement(-1, x, y, 21, 0, 0, -1, -1);
            }*/
            /*
            if (Main.netMode != 1)
            {
                //WorldGen.dropMeteor();
            }
            int R = Main.rand.Next(3) + 1;
            int I2 = (int)(Main.MouseWorld.X / 16) + (Main.rand.NextBool(2) ? -160 : 80);
            if (R == 1)
            {
                I2 = (int)(Main.MouseWorld.X / 16) + 80;
            }
            int J2 = (int)(Main.MouseWorld.Y / 16) - 100;
            for (int a = 0; a < 200; a++)
            {
                if (Main.tile[I2, J2].HasTile && Main.tileSolid[Main.tile[I2, J2].TileType])
                {
                    break;
                }
                J2++;
            }
            int H = 36;
            if (R == 2)
            {
                H= 48;
            }
            if (R == 3)
            {
                H = 44;
            }
            GenerateStructure("Worlds/流星塔"+R, new Point16(I2, J2 - H-1), DDmod.Instance);
            int L = 62;
            if (R == 2)
            {
                L = 59;
            }
            if (R == 3)
            {
                L = 71;
            }
            for (int a = 0; a < L; a++)
            {
                for (int b = -1; b < 30; b++)
                {
                    Tile tile = Main.tile[I2 + a, J2 + b];
                    if (tile.HasTile && Main.tileSolid[tile.TileType])
                    {
                        tile.Slope = 0;
                        tile.IsHalfBlock = false;
                        WorldGen.SquareTileFrame(I2 + a, J2 + b, true);
                        break;
                    }
                    tile.TileType = (ushort)370;
                    tile.HasTile = true;
                    WorldGen.SquareTileFrame(I2 + a, J2 + b, true);
                }
            }
            if(R==1)
            {
                for (int a =-8; a < 0; a++)
                {
                    for (int b = -3; b < 30; b++)
                    {
                        Tile tile = Main.tile[I2 + a, J2 + b];
                        if (tile.HasTile && Main.tileSolid[tile.TileType])
                        {
                            tile.Slope = 0;
                            tile.IsHalfBlock = false;
                            WorldGen.SquareTileFrame(I2 + a, J2 + b, true);
                            break;
                        }
                        tile.TileType = (ushort)370;
                        tile.HasTile = true;
                        WorldGen.SquareTileFrame(I2 + a, J2 + b, true);
                    }
                }
            }
            J2 -= H + 1;
            bool Boss = false;
            if (R == 1)
            {
                //WorldGen.KillTile(I2 + 10, J2 + 33);
                MeteorBox(I2 + 10, J2 + 33,ref Boss);
                //WorldGen.KillTile(I2 + 26, J2 + 33);
                MeteorBox(I2 + 26, J2 + 33, ref Boss);
                //WorldGen.KillTile(I2 + 39, J2 + 26);
                MeteorBox(I2 + 39, J2 + 26, ref Boss);
                //WorldGen.KillTile(I2 + 39, J2 + 21);
                MeteorBox(I2 + 39, J2 + 21, ref Boss, true);
            
            }
            else if (R == 2)
            {
                MeteorBox(I2 + 20, J2 + 37, ref Boss);
                MeteorBox(I2 + 26, J2 + 44, ref Boss);
                MeteorBox(I2 + 29, J2 + 44, ref Boss);
                MeteorBox(I2 + 35, J2 + 37, ref Boss,true);

            }
            else if (R == 3)
            {
                MeteorBox(I2 + 22, J2 + 41, ref Boss);
                MeteorBox(I2 + 31, J2 + 22, ref Boss);
                MeteorBox(I2 + 36, J2 + 22, ref Boss);
                MeteorBox(I2 + 45, J2 + 41, ref Boss, true);

            }
            //NoBoss 最后一个箱子,如果没有boss召唤物强行生成一个
            void MeteorBox(int x, int y,ref bool Boss,bool NoBoss = false)
            {
                int PlacementSuccess = WorldGen.PlaceChest(x, y, (ushort)21, false, 49);
                if (PlacementSuccess >= 0)
                {

                    Random ran = new();
                    Chest chest = Main.chest[PlacementSuccess];
                    chest.name = Language.GetTextValue("Mods.DDmod.Level.草原") + Language.GetTextValue("Mods.DDmod.Level.通关宝箱");
                    int Citem = 0;
                    chest.item[Citem].SetDefaults(ModContent.ItemType<可疑外星蓝图>(), false);
                    chest.item[Citem].stack = 1;
                    Citem++;
                    chest.item[Citem].SetDefaults(117, false);
                    chest.item[Citem].stack = ran.Next(3, 8);
                    Citem++;
                    if (Main.rand.NextBool(2))
                    {
                        chest.item[Citem].SetDefaults(Main.rand.NextBool(20) ? ModContent.ItemType<流星剑盾>() : Main.rand.Next(new int[] { ModContent.ItemType<MeteorSword>(), 127, ModContent.ItemType<流星投刀>(), ModContent.ItemType<MeteorDroneController>() }), false);
                        chest.item[Citem].stack = 1;
                        Citem++;
                    }
                    else
                    {
                        chest.item[Citem].SetDefaults(Main.rand.NextBool(5) ? 197 : Main.rand.Next(new int[] {198,199,200,201,202,203,4258,204 }), false);
                        chest.item[Citem].stack = 1;
                        Citem++;
                    }
                    chest.item[Citem].SetDefaults(117, false);
                    chest.item[Citem].stack = ran.Next(3, 8);
                    Citem++;
                    if (!Boss&&(Main.rand.NextBool(4)|| NoBoss))
                    {
                        chest.item[Citem].SetDefaults(ModContent.ItemType<AlienRigController>(), false);
                        chest.item[Citem].stack = 1;
                        Citem++;
                        Boss = true;
                    }
                    chest.item[Citem].SetDefaults(117, false);
                    chest.item[Citem].stack = ran.Next(3, 8);
                    Citem++;
                    chest.item[Citem].SetDefaults(117, false);
                    chest.item[Citem].stack = ran.Next(3, 8);
                    Citem++;
                }
                NetMessage.SendObjectPlacement(-1, x, y, 21, 0, 0, -1, -1);
            }*/
            Main.NewText(1);
            //WorldGen.PlaceTile(I2, J2, 370);
            return true;
        }
    }
}