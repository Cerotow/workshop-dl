using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.绿岩;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩.家具
{
    public class 绿岩床Tile : DDBed
    {
        public override Color Color => new Color(11, 255, 11);
        public override int Icon => ModContent.ItemType<绿岩床>();
        public override int Dust => ModContent.DustType<绿岩粒子>();
    }
}
