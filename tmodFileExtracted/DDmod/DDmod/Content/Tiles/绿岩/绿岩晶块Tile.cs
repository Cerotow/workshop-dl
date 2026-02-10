using System;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.绿岩;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩晶块Tile : DDCrystals
    {
        public override int Sound => 0;
        public override int Dust => ModContent.DustType<绿岩粒子>();
        public override Color Color => new Color(100, 255, 100, 0);
        public override Vector3 LightColor => new Color(100, 255, 100).ToVector3()*0.5F;
        public override int Glow => -1;
        public override int Style => 9;
        public override void SetDefaults()
        {
            RegisterItemDrop(ModContent.ItemType<绿岩晶石>());
        }

    }
}
