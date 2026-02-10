using DDmod.Content.Items.Tiles.晶凝;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.晶凝
{
	public class 晶凝心灯Tile : DDFloorLamp
    {
		public static Asset<Texture2D> flame;
        public override Color Color => new Color(255, 100, 100);
        public override Vector3 LightColor => new Color(255, 100, 100, 0).ToVector3()*1.2F;
        public override Texture2D flameTexture => flame.Value;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                flame = ModContent.Request<Texture2D>("DDmod/Content/Tiles/晶凝/晶凝心灯Tile_Flame");
            }
        }
    }
}
