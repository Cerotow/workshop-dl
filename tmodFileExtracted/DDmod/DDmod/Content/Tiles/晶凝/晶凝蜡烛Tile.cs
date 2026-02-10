using System;
using DDmod.Content.Items.Tiles.晶凝;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.晶凝
{
	public class 晶凝蜡烛Tile : DDCandle
    {
        public override int Icon => ModContent.ItemType<晶凝蜡烛>();
        public override int Dust => 12;
        public override Color Color => new Color(253, 221, 3, 0);
        public override Vector3 LightColor => new Color(255, 100, 100, 0).ToVector3();
        public override Texture2D flameTexture => flame.Value;
        public static Asset<Texture2D> flame;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                flame = ModContent.Request<Texture2D>("DDmod/Content/Tiles/晶凝/晶凝蜡烛Tile_Flame");
            }
        }
    }
}
