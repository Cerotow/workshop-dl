using DDmod.Content.Items.Tiles.晶凝;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.晶凝
{
    public class 晶凝床Tile : DDBed
    {
        public override Color Color => new Color(255, 100, 100);
        public override int Icon => ModContent.ItemType<晶凝床>();
        public override int Dust => 12;
	}
}
