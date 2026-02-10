using DDmod.Content.Projectiles.OrnamentProjectile;

namespace DDmod.Content.Items.Boss.狱火蛇物品
{
    public class 狱火戒 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.expert = true;
            //Item.canBePlacedInVanityRegardlessOfConditions = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lavaImmune = true;
            player.buffImmune[24] = true;
            player.buffImmune[ModContent.BuffType<地狱之火>()] = true;
            player.buffImmune[ModContent.BuffType<炼狱之火>()] = true;
            if(Main.tile[(int)player.Center.X / 16, (int)(player.Center.Y) / 16].LiquidAmount>0&& Main.tile[(int)player.Center.X / 16, (int)(player.Center.Y) / 16].LiquidType==1)
            {
                player.GetDamage(DamageClass.Generic) += 0.3F;
            }
            if (!hideVisual && Main.netMode != 2 && Main.myPlayer == player.whoAmI)
            {
                Point point = new Point((int)player.position.X / 16, (int)(player.position.Y + player.height) / 16);
                Tile tile = Main.tile[point.X, point.Y];
                Tile tile2 = Main.tile[point.X + 1, point.Y];
                if (tile.LiquidAmount > 30&&tile.LiquidType==0&&!tile.HasTile)
                {
                    tile.HasTile = true;
                    tile.TileType = 56;
                    tile.LiquidAmount = 0;
                    WorldGen.SquareTileFrame(point.X, point.Y, true);
                    NewDustChange(30, new Vector2(point.X, point.Y)*16, new Vector2(16), 6, 1, 3);
                    NetMessage.SendTileSquare(player.whoAmI, point.X, point.Y, 1, 1);

                }
                if(tile2.LiquidAmount > 30 && tile2.LiquidType==0&&!tile2.HasTile)
                {
                    tile2.HasTile = true;
                    tile2.TileType = 56;
                    tile2.LiquidAmount = 0;
                    WorldGen.SquareTileFrame(point.X+1, point.Y, true);
                    NewDustChange(30, new Vector2(point.X+1, point.Y) * 16, new Vector2(16), 6, 1, 3);
                    NetMessage.SendTileSquare(player.whoAmI, point.X+1, point.Y, 1, 1);
                }
            }
        }
    }
}
