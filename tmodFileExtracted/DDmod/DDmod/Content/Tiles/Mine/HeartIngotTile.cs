using DDmod.Content.Items.Series.Heart;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.Mine
{
    public class HeartIngotTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            DustType = 12;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(222, 0, 0));
            Main.tileSolid[Type] = true;
            Main.tileShine[Type] = 800;

            LocalizedText modTranslation = CreateMapEntryName();
            // modTranslation.SetDefault("Heart Bar");
            //modTranslation.AddTranslation(7, "心锭");
            AddMapEntry(new Color(255, 0, 0), modTranslation);
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<HeartIngot>();
        }
    }
}