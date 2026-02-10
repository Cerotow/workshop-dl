
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using DDmod.Content.Items.Tiles.晶凝;

namespace DDmod.Content.Tiles.晶凝
{
	public class 晶凝门关Tile : DDDoorOff
    {
        public override int Icon => ModContent.ItemType<晶凝门>();
        public override int Dust => 12;
        public override Color Color => new Color(255, 100, 100);
        public override int DoorID => ModContent.TileType<晶凝门开Tile>();
        public override int ItemID => Icon;
    }
}