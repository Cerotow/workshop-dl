using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using DDmod.Worlds;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;
using DDmod.Content.Items.Series.Heart;
using DDmod.Content.Items.Tiles.晶凝;

namespace DDmod.Content.Tiles.晶凝
{
	public class 晶凝石块Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			DustType = 12;
			HitSound = SoundID.Tink;

			LocalizedText modTranslation = CreateMapEntryName();

			AddMapEntry(new Color(200, 20, 20), modTranslation);
		}
        public override bool KillSound(int i, int j, bool fail)
        {
			if (!fail)
			{
				PlaySound(SoundID.Shatter, new Vector2(i, j) * 16);
			}
            return base.KillSound(i, j, fail);
        }
    }
}