using System;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Tiles;
using DDmod.UI.ItemUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.EquipTiles
{
    public class 鱼缸 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[(int)Type] = true;
            Main.tileFrameImportant[(int)Type] = true;
            Main.tileNoAttach[(int)Type] = true;
            Main.tileTable[(int)Type] = true;
            Main.tileLavaDeath[(int)Type] = true;
            Main.tileWaterDeath[(int)Type] = false;
            Main.tileLighted[Type] = true;
            HitSound = SoundID.Dig;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Width = 10;
            TileObjectData.newTile.CoordinateWidth = 16;

            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16 };
            //StrengthenTE advancedEntity = ModContent.GetInstance<StrengthenTE>();
            //TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(advancedEntity.Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
            LocalizedText modTranslation = CreateMapEntryName();
            AddMapEntry(new Color(27, 168, 255), modTranslation);
            AdjTiles = new int[]
            {
                14
            }; 
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            /*
            Tile tile = Main.tile[i, j];
            TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
            StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(i, j, tileData.Width, tileData.Height, 18);
            if (tepowerCellFactory != null)
            {
                if (tepowerCellFactory.items.type != 0)
                {
                    Item ContainedItem = tepowerCellFactory.items;
                    int num2 = Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ContainedItem.type, ContainedItem.stack, false, (int)ContainedItem.prefix, true, false);
                    Main.item[num2] = ContainedItem.Clone();
                    Main.item[num2].position = new Vector2(i * 16+16, j * 16 - 16 - tepowerCellFactory.t);
                    Main.item[num2].newAndShiny = false;
                }
                for (int a = 0; a < 3; a++)
                {
                    if (tepowerCellFactory.FortifiedStone[a].type != 0)
                    {
                        Vector2 vector = new Vector2((a - 1) * 34, 38 + tepowerCellFactory.t);
                        if (a != 1)
                        {
                            vector.Y -= 20;
                        }
                        Item ContainedItem = tepowerCellFactory.FortifiedStone[a];
                        int num2 = Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ContainedItem.type, ContainedItem.stack, false, (int)ContainedItem.prefix, true, false);
                        Main.item[num2] = ContainedItem.Clone();
                        Main.item[num2].position = new Vector2(i * 16+16, j * 16)- vector;
                        Main.item[num2].newAndShiny = false;
                    }
                }
            }
            StrengtheningUI.Visible = false;
            ModContent.GetInstance<StrengthenTE>().Kill(i, j);
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<StrengthenPlatformItem>());*/
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
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.SpecialDraw(i, j, spriteBatch);
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {

        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY == 0 && tile.TileFrameX == 0)
            {
                bool uu = false;
                int TT = 0;
                if (DDSystem.鱼缸Draw != null)
                {
                    for (int a = 0; a < DDSystem.鱼缸Draw.Count; a++)
                    {
                        if (DDSystem.鱼缸Draw[a] == new Point16(i, j))
                        {
                            uu = true;
                        }
                    }
                    for (int a = 0; a < DDSystem.鱼缸Draw.Count; a++)
                    {
                        if (DDSystem.鱼缸Draw[a] != new Point16(0, 0))
                        {
                            TT++;
                        }
                    }
                }
                if (!uu)
                {
                    DDSystem.鱼缸Draw.Add(new Point16(i, j));
                    if (DDSystem.鱼缸Draw != null)
                    {
                        //Array.Resize(ref DDSystem.SpecialDraw, DDSystem.SpecialDraw.Length + 1);
                        //DDSystem.SpecialDraw[DDSystem.SpecialDraw.Length - 1] = new Point16(i, j);
                    }
                    else
                    {
                        //DDSystem.SpecialDraw = new Point16[1];
                       // DDSystem.SpecialDraw[DDSystem.SpecialDraw.Length - 1] = new Point16(i, j);
                    }
                }
            }
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
        }
        public override bool RightClick(int i, int j)
        {
            return true;
        }
        public override void MouseOver(int i, int j)
        {
        }

        public override void MouseOverFar(int i, int j)
        {
        }
    }
}