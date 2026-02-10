using DDmod.Content.Items.Tiles.晶凝;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.晶凝
{
	public class 晶凝椅Tile : DDChair
    {
        public override int Icon => ModContent.ItemType<晶凝椅>();
        public override int Dust => 12;
        public override Color Color => new Color(255, 100, 100);
    }
}
