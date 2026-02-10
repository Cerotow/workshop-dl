using DDmod.Content.Dusts;
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

namespace DDmod.Content.Tiles.绿岩.家具
{
    public class 绿岩工程灯Tile : DDFloorLamp
    {
		public static Asset<Texture2D> flame;
        public override Vector3 LightColor => new Color(100, 255, 100).ToVector3() * 1.2F;
        public override void SetDefaults()
        {
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩工程灯", out int GG);
            Main.tileGlowMask[Type] = (short)GG;
        TileObjectData.newTile.CoordinateHeights = [16, 16, 18];
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
    }
}
