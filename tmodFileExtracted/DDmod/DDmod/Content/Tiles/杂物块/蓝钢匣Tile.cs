using DDmod.Content.Items.Series.Heart;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.杂物块
{
    public class 蓝钢匣Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileTable[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.Table|AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            DustType = -1;
            TileObjectData.addTile(Type);

            LocalizedText modTranslation = CreateMapEntryName();
            AddMapEntry(new Color(222, 222, 222), modTranslation); }
    }
}