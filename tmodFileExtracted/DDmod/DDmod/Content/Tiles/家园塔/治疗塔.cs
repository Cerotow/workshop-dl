using System;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Tiles;
using DDmod.Content.Tiles.农场;
using DDmod.UI.ItemUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.家园塔
{
    public class 治疗塔 : ModTile
    {
        public const int FrameWidth = 18 * 2;
        public const int FrameHeight = 18 * 2;
        public static Asset<Texture2D> GTexture;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                GTexture = ModContent.Request<Texture2D>("DDmod/Content/Tiles/家园塔/治疗珠");
            }
        }
        public override void Unload()
        {
            GTexture = null;
        }
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
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            家园塔TE advancedEntity = ModContent.GetInstance<家园塔TE > ();
            advancedEntity.items = new Item[] { new Item(1), new Item(2) };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(advancedEntity.Hook_AfterPlacement, -1, 0, false);
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
            Tile tile = Main.tile[i, j];
            TileObjectData tileData = TileObjectData.GetTileData(ModContent.TileType<治疗塔>(), 0, 0);
            家园塔TE tepowerCellFactory = playerHelper.FindTileEntity2<家园塔TE>(i, j, tileData.Width, tileData.Height, 18);
            if (tepowerCellFactory != null)
            {
                for (int a = 0; a < 3; a++)
                {
                    if (tepowerCellFactory.items[a].type != 0)
                    {
                        Item ContainedItem = tepowerCellFactory.items[a];
                        int num2 = Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ContainedItem.type, ContainedItem.stack, false, (int)ContainedItem.prefix, true, false);
                        Main.item[num2] = ContainedItem.Clone();
                        Main.item[num2].position = new Vector2(i * 16 + 16, j * 16);
                        Main.item[num2].newAndShiny = false;
                    }
                }
            }
            ModContent.GetInstance<家园塔TE>().Kill(i, j);
            //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<StrengthenPlatformItem>());
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
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (drawData.tileFrameX % FrameWidth == 0 && drawData.tileFrameY % FrameHeight == 0)
            {
                Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
                bool B = true;
                for (int A = 0; A < DDTileDawnSystem.家园塔Draw.Count; A++)
                {
                    if (DDTileDawnSystem.家园塔Draw[A] == new Point16(i, j))
                    {
                        B = false;
                    }
                }
                if (B)
                    DDTileDawnSystem.家园塔Draw.Add(new Point16(i, j));
            }
        }
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Main.tile[i,j].TileType, 0, 0);
            家园塔TE tepowerCellFactory = playerHelper.FindTileEntity2<家园塔TE>(i, j, tileData.Width, tileData.Height, 18);

            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
            {
                offScreen = Vector2.Zero;
            }

            Point p = new Point(i, j);
            Tile tile = Main.tile[p.X, p.Y];
            if (tile == null || !tile.HasTile)
            {
                return;
            }
            return;

            Texture2D texture = GTexture.Value;
            Texture2D texture2 = DDTextures.MiniVoidStar.Value;
            Vector2 worldPos = p.ToWorldCoordinates(16f, 64f);

            float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 5f);
            Vector2 drawPos = worldPos + offScreen - Main.screenPosition + new Vector2(0f, -66f) + new Vector2(0f, offset);
            spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, texture.Size()/2, 1, 0, 0f);
            if(tepowerCellFactory.active&& tepowerCellFactory.ConsumptionMana< tepowerCellFactory.Mana)
                spriteBatch.Draw(texture2, drawPos, null, new Color(100,255,100,0), 0f, texture2.Size()/2, 0.5F* tepowerCellFactory.Glow, 0, 0f);

            Lighting.AddLight(new Vector2(i+1,j-1)* 16 + new Vector2(0f, 12+offset), new Color(100, 255, 100, 0).ToVector3()/2);
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            
            return true;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
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
            Tile t = Main.tile[i, j];
            TileObjectData tileData = TileObjectData.GetTileData(Main.tile[i, j].TileType, 0, 0);
            int sheetSquare = 18;
            int left = i - t.TileFrameX % (tileData.Width * sheetSquare) / sheetSquare;
            int top = j - t.TileFrameY % (tileData.Height * sheetSquare) / sheetSquare;
            Main.LocalPlayer.GetModPlayer<家园塔Player>().塔 = new Point16(left, top);
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
                player.cursorItemIconID = 0;

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