using System;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.绿岩;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩.家具
{
    public class 绿岩小灯管Tile : DDCandle
    {
        public override int Icon => ModContent.ItemType<绿岩小灯管>();
        public override int Dust => ModContent.DustType<绿岩粒子>();
        public override Vector3 LightColor => new Color(100, 255, 100).ToVector3();
        public override void SetDefaults()
        {
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩小灯管", out int GG);
            Main.tileGlowMask[Type] = (short)GG;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
    }
}
