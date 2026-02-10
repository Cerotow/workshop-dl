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
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.Content.Tiles.EquipTiles
{
    public class StrengthenPlatform : ModTile
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
            DustType = ModContent.DustType<生命粒子>();
            MinPick = 50;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            StrengthenTE advancedEntity = ModContent.GetInstance<StrengthenTE>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(advancedEntity.Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
            // modTranslation.SetDefault("Strengthen Platform");
            //modTranslation.AddTranslation(7,"强化台");
            AddMapEntry(new Color(27, 168, 255), CreateMapEntryName());
            AdjTiles = new int[]
            {
                14
            }; 
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
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
                    Main.item[num2].position = new Vector2(i * 16+16, j * 16 - 16 - (float)tepowerCellFactory.t / 5);
                    Main.item[num2].newAndShiny = false;
                }
                for (int a = 0; a < 3; a++)
                {
                    if (tepowerCellFactory.FortifiedStone[a].type != 0)
                    {
                        Vector2 vector = new Vector2((a - 1) * 34, 38 + (float)tepowerCellFactory.t / 5);
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
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, 1, 0f, 0f, 1, new Color(100, 130, 150), 1f);
            return false;
        }
        public override bool CanPlace(int i, int j)
        {
            return true;
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
                if (DDSystem.SpecialDraw != null)
                {
                    for (int a = 0; a < DDSystem.SpecialDraw.Count; a++)
                    {
                        if (DDSystem.SpecialDraw[a] == new Point16(i, j))
                        {
                            uu = true;
                        }
                    }
                    for (int a = 0; a < DDSystem.SpecialDraw.Count; a++)
                    {
                        if (DDSystem.SpecialDraw[a] != new Point16(0, 0))
                        {
                            TT++;
                        }
                    }
                }
                if (!uu)
                {
                    DDSystem.SpecialDraw.Add(new Point16(i, j));
                    if (DDSystem.SpecialDraw != null)
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
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = (tile.TileFrameY == 36) ? 18 : 16;
            //Main.spriteBatch.Draw(TextureAssets.Tile[Type].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
            if (tile.TileFrameY == 0 && tile.TileFrameX == 0)
            {
                TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<StrengthenPlatform>(), 0, 0);
                StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(i, j, tileData.Width, tileData.Height, 18);
            }
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
            StrengthenTE tepowerCellFactory = playerHelper.FindTileEntity2<StrengthenTE>(i, j, tileData.Width, tileData.Height, 18);
            if (tepowerCellFactory != null)
            {
                int A = -1;
                for (int a = 0; a < 255; a++)
                {
                    if (Main.player[a].active && Main.player[a].Dplayer().STPosition == new Point16(tepowerCellFactory.Position.X, tepowerCellFactory.Position.Y))
                    {
                        A = a;
                        break;
                    }
                }
                if (A == -1)
                {
                    Main.LocalPlayer.Dplayer().STPosition = new Point16(tepowerCellFactory.Position.X, tepowerCellFactory.Position.Y);
                    DDmod.SyncData(DDType.PlayerData, Main.myPlayer, -1, -1);
                }
                else
                {
                    Main.NewText(Language.GetTextValue("Mods.DDmod.StrengtheningUI.使用中", Main.player[A].name));
                }
            }
            else
            {
                Main.NewText(playerHelper.FindTileEntity2<StrengthenTE>(i, j, tileData.Width, tileData.Height, 18) == null);
            }
            return true;
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];

            player.cursorItemIconID = -1;
            string defaultName = TileLoader.DefaultContainerName(tile.TileType,tile.TileFrameX,tile.TileFrameY)/* tModPorter Note: new method takes in FrameX and FrameY */;
            if (player.cursorItemIconText == defaultName)
            {
                player.cursorItemIconID = ModContent.ItemType<StrengthenPlatformItem>();

                player.cursorItemIconText = "";
            }

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }
    }
}