using System;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.晶凝;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩.家具
{
    public class 绿岩吊灯Tile : DDChandelier
    {
        public override int Dust => ModContent.DustType<绿岩粒子>();
        public override Vector3 LightColor => new Color(100, 255, 100).ToVector3() * 2;
    }
}
