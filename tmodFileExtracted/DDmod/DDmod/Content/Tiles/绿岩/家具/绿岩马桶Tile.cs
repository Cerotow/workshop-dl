using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.晶凝;
using DDmod.Content.Items.Tiles.绿岩;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩.家具
{
    public class 绿岩马桶Tile : DDToilet
    {
        public override int Icon => ModContent.ItemType<绿岩马桶>();
        public override int Dust => ModContent.DustType<绿岩粒子>();
        public override Color Color => new Color(11, 255, 11);
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
    }
}

