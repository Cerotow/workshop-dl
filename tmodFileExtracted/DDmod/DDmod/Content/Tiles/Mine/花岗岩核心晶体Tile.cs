using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using DDmod.Worlds;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;
using DDmod.Content.Items.Series.Star;
using DDmod.Content.Dusts;

namespace DDmod.Content.Tiles.Mine
{
    public class 花岗岩核心晶体Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.ChecksForMerge[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 255;
            Main.tileMergeDirt[Type] = false;
            Main.tileMerge[Type][368] = true;
            Main.tileMerge[368][Type] = true;
            Main.tileShine[Type] = 800;
            Main.tileLighted[Type] = true;
            DustType = 226;
            HitSound = SoundID.Tink;


            LocalizedText modTranslation = CreateMapEntryName();
            // modTranslation.SetDefault("Mana Ore");
            //modTranslation.AddTranslation(7, "星矿");

            AddMapEntry(new Color(0, 155, 255), modTranslation);
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<StarMine>();
            MinPick = 50;
        }
        public override void PostSetupTileMerge()
        {
            base.PostSetupTileMerge();
        }
        public override bool KillSound(int i, int j, bool fail)
        {
            if (!fail)
            {
                PlaySound(SoundID.Shatter, new Vector2(i, j) * 16);
            }
            return base.KillSound(i, j, fail);
        }
        public Asset<Texture2D> TileTexture;

        public string T => "DDmod/Content/Tiles/Mine/花岗岩核心晶体Tile_Glow";

        public override void Load()
        {
            if (!Main.dedServ)
            {
                TileTexture = ModContent.Request<Texture2D>(T);
            }
        }
        public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
        {
           WorldGen.TileMergeAttempt(-2, 368, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            //TileMergeAttempt(-2, TileID.Sets.Dirt, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
            return base.TileFrame(i, j, ref resetFrame, ref noBreak);
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
            
            Main.spriteBatch.Draw(TileTexture.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);

        }
    }
}