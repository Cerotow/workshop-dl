using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.晶凝;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩.家具
{
    public class 绿岩水槽Tile : DDSink
    {

        public override int Dust => ModContent.DustType<绿岩粒子>();
        public override Color Color => new Color(11, 255, 11);
        public override Vector3 LightColor => new Vector3(0, 0.15F, 0);
        public override void SetDefaults()
        {
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 20 };
            TileObjectData.newTile.DrawYOffset-=2;
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩水槽", out int GG);
            Main.tileGlowMask[Type] = (short)GG;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
    }
}