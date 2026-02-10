using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.Heart;
using DDmod.Content.Items.Series.Star;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.Mine
{
    public class StarIngotTile : ModTile
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
            DustType = ModContent.DustType<魔力水晶粒子>();
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(0, 0, 222));
            Main.tileSolid[Type] = true;
            Main.tileShine[Type] = 800;


            LocalizedText modTranslation = CreateMapEntryName();
            // modTranslation.SetDefault("Star Bar");
            //modTranslation.AddTranslation(7, "星锭");
            AddMapEntry(new Color(255, 0, 0), modTranslation);
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<StarIngot>();
        }
    }
}