using System;
using DDmod.Content.Items.Tiles.晶凝;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.晶凝
{
	public class 晶凝烛台Tile : DDCandelabra
    {
        public override int Dust => 12;
        public override Vector3 LightColor => new Color(255, 100, 100).ToVector3()*1.5F;
    }
}
