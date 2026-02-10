using DDmod.AccessorySlot;
using DDmod.Content.Prefixes;
using DDmod.Players;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace DDmod.Content.Items
{
    public class AccessoryGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        /// <summary> 表 </summary>
        public bool Watch;
        /// <summary> 水中翅膀 </summary>
        public bool WaterWings;

        public float Damage = 0;
        public int Defense = 0;
        public int Crit = 0;
        public float MoveSpeed = 0;
        public float MeleeSpeed = 0;

        //默认属性
        public override void SetDefaults(Item item)
        {
            if (item.type is ItemID.CopperWatch or ItemID.SilverWatch or ItemID.GoldWatch
                or ItemID.TinWatch or ItemID.TungstenWatch or ItemID.PlatinumWatch
                or ItemID.GPS or ItemID.PDA or ItemID.CellPhone)
            {
                Watch = true;
                item.accessory = true;
            }
            if (item.type is 2609 or 2494)
            {
                WaterWings = true;
            }
        }
        
        //保存和加载
        public override void LoadData(Item item, TagCompound tag)
        {
        }

        public override void SaveData(Item item, TagCompound tag)
        {
        }
        //同步
        public override void NetSend(Item item, BinaryWriter writer)
        {
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Damage > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "PrefixAccDamage", "+" + (int)(Damage*100) + Lang.tip[39].Value)
                {
                    IsModifier = true
                });
            }
            if (Defense > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "PrefixAccDefense", "+" + Defense + Lang.tip[25].Value)
                {
                    IsModifier = true
                });
            }
            if (Crit > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "PrefixAccCritChance", "+" + Crit + Lang.tip[5].Value)
                {
                    IsModifier = true
                });
            }
            if (MoveSpeed > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "PrefixAccMoveSpeed", "+" + (int)(MoveSpeed*100) + Lang.tip[46].Value)
                {
                    IsModifier = true
                });
            }
            if (MeleeSpeed > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "PrefixAccMeleeSpeed", "+" + (int)(MeleeSpeed*100) + Lang.tip[47].Value)
                {
                    IsModifier = true
                });
            }
        }
        public override bool AllowPrefix(Item item, int pre)
        {
            if (Watch)
            {
                return false;
            }
            return base.AllowPrefix(item, pre);
        }
        public override bool CanReforge(Item item)
        {
            return base.CanReforge(item);
        }
        //重铸
        public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand)
        {
            if (Watch && Main.InReforgeMenu)
            {
                return false;
            }
            return base.PrefixChance(item, pre, rand);
        }
        //可以获得的词条
        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            return base.ChoosePrefix(item, rand);
        }
        //禁止饰品
        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
        {
            if (Watch)
            {
                return modded && slot == AccessorySystem.WatchSlots;
            }
            return base.CanEquipAccessory(item, player, slot, modded);
        }
        public override void UpdateVisibleAccessory(Item item, Player player, bool hideVisual)
        {

            if (item.shieldSlot >= 0)
            {
                player.PlayerAction().ThereShield = true;
               // player.PlayerAction().ThereShield2 = true;
            }
        }
        //饰品加成
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            
            if (item.shieldSlot>=0)
            {
                if (!hideVisual)
                    player.PlayerAction().ThereShield = true;

                //player.PlayerAction().ThereShield2 = true;
            }
            if (!hideVisual && item.type == ItemID.FlurryBoots)
            {
                if (Main.tile[(int)player.position.X / 16, (int)player.position.Y / 16 + 3].TileType == 147)
                {
                    player.moveSpeed += 2f;
                }
                if (Main.netMode != 2 && Main.myPlayer == player.whoAmI)
                {
                    Point point = new Point((int)player.position.X / 16, (int)(player.position.Y + player.height) / 16);
                    Tile tile = Main.tile[point.X, point.Y];
                    Tile tile2 = Main.tile[point.X + 1, point.Y];
                    if (tile.LiquidAmount > 30 && tile.LiquidType == 0 && !tile.HasTile)
                    {
                        tile.HasTile = true;
                        tile.TileType = 162;
                        WorldGen.SquareTileFrame(point.X, point.Y, true);
                        NewDustChange(30, new Vector2(point.X, point.Y) * 16, new Vector2(16), 92, 1, 3);
                        NetMessage.SendTileSquare(player.whoAmI, point.X, point.Y, 1, 1);

                    }
                    if (tile2.LiquidAmount > 30 && tile2.LiquidType == 0 && !tile2.HasTile)
                    {
                        tile2.HasTile = true;
                        tile2.TileType = 162;
                        WorldGen.SquareTileFrame(point.X + 1, point.Y, true);
                        NewDustChange(30, new Vector2(point.X + 1, point.Y) * 16, new Vector2(16), 92, 1, 3);
                        NetMessage.SendTileSquare(player.whoAmI, point.X + 1, point.Y, 1, 1);
                    }
                }
            }
            if (!hideVisual && item.type == 54)
            {
                if (Main.tile[(int)player.position.X / 16, (int)player.position.Y / 16 + 3].TileType == 2 || Main.tile[(int)player.position.X / 16, (int)player.position.Y / 16 + 3].TileType == 60)
                {
                    player.moveSpeed += 1f;
                }
                if (Main.netMode != 2 && Main.myPlayer == player.whoAmI)
                {
                    Point point = new Point((int)player.position.X / 16, (int)(player.position.Y + player.height) / 16);
                    Tile tile = Main.tile[point.X, point.Y];
                    Tile tile2 = Main.tile[point.X + 1, point.Y];
                    if (tile.HasTile)
                    {
                        if (tile.TileType == 0)
                        {
                            tile.TileType = 2;
                            WorldGen.SquareTileFrame(point.X, point.Y, true);
                            NewDustChange(30, new Vector2(point.X, point.Y) * 16, new Vector2(16), 2, 1, 3);
                            NetMessage.SendTileSquare(player.whoAmI, point.X, point.Y, 1, 1);
                        }
                        if (tile.TileType == 59)
                        {
                            tile.TileType = 60;
                            WorldGen.SquareTileFrame(point.X + 1, point.Y, true);
                            NewDustChange(30, new Vector2(point.X + 1, point.Y) * 16, new Vector2(16), 40, 1, 3);
                            NetMessage.SendTileSquare(player.whoAmI, point.X + 1, point.Y, 1, 1);
                        }

                    }
                    if (tile2.HasTile)
                    {
                        if (tile2.TileType == 0)
                        {
                            tile2.TileType = 2;
                            WorldGen.SquareTileFrame(point.X + 1, point.Y, true);
                            NewDustChange(30, new Vector2(point.X + 1, point.Y) * 16, new Vector2(16), 2, 1, 3);
                            NetMessage.SendTileSquare(player.whoAmI, point.X + 1, point.Y, 1, 1);
                        }
                        if (tile2.TileType == 59)
                        {
                            tile2.TileType = 60;
                            WorldGen.SquareTileFrame(point.X + 1, point.Y, true);
                            NewDustChange(30, new Vector2(point.X + 1, point.Y) * 16, new Vector2(16), 40, 1, 3);
                            NetMessage.SendTileSquare(player.whoAmI, point.X + 1, point.Y, 1, 1);
                        }
                    }
                }
            }
            if (WaterWings)
            {
                player.PlayerAction().WaterWings = true;
            }
            /*
            if (item.type == 1131)
            {
                player.gravControl2 = false;

                player.Aplayer().Gravitation = 5;
            }*/
        }
    }
}