using System;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Tiles;
using DDmod.Content.Items.农场.种子;
using DDmod.Content.Items.农场.食物;
using DDmod.UI.ItemUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.农场
{
    public class 胡萝卜Tile : 作物
    {
        public override void SetStaticDefaults()
        {
            
            Main.tileSolidTop[(int)Type] = false;
            Main.tileFrameImportant[(int)Type] = true;
            Main.tileNoAttach[(int)Type] = true;
            Main.tileLavaDeath[(int)Type] = true;
            Main.tileWaterDeath[(int)Type] = false;
            Main.tileLighted[Type] = true;
            HitSound = SoundID.Dig;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16,16};
            菜TE advancedEntity = ModContent.GetInstance<菜TE>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(advancedEntity.Hook_AfterPlacement, -1, 0, false);
            
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				ModContent.TileType<锄过的土块>(),
			};
            TileObjectData.newTile.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(184, 98, 27));
        }
        int Time = DDHelper.Second(300);
        int Max = 3;
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            菜TE tepowerCellFactory = playerHelper.FindTileEntity2<菜TE>(i, j, tileData.Width, tileData.Height, 18);
            int A = tepowerCellFactory.Time / Time;
            if (A > Max)
            {
                A = Max;
            }
            if (A == 0)
            {
                yield return new Item(ModContent.ItemType<胡萝卜种子>());
            }
            if (A == Max)
            {
                if (tepowerCellFactory.variation)
                {
                    yield return new Item(ModContent.ItemType<金胡萝卜>());
                    yield return new Item(ModContent.ItemType<胡萝卜>(), Main.rand.Next(1, 3));
                }
                else
                {
                    yield return new Item(ModContent.ItemType<胡萝卜>(),Main.rand.Next(2,4));
                }
            }
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, 1, 0f, 0f, 1, new Color(100, 130, 150), 1f);
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Draw(i, j, spriteBatch, Time, Max,true, false);
            return false;
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
        }
        public override void MouseOver(int i, int j)
        {
            MouseText(i, j, Time, Max, false);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY == 0 && tile.TileFrameX == 0)
            {
            }
        }
        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            菜TE tepowerCellFactory = playerHelper.FindTileEntity2<菜TE>(i, j, tileData.Width, tileData.Height, 18);
            return true;
        }
    }
}