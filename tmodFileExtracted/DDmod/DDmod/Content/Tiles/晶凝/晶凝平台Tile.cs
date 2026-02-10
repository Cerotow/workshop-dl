using DDmod.Content.Items.Tiles.晶凝;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.晶凝
{
	public class 晶凝平台Tile : DDPlatforms
    {
        public override int Dust => 12;
        public override Color Color => new Color(255, 100, 100);
    }
}