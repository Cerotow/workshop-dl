
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
using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.绿岩;

namespace DDmod.Content.Tiles.绿岩.家具
{
    public class 绿岩门关Tile : DDDoorOff
    {
        public override int Icon => ModContent.ItemType<绿岩门>();
        public override int Dust => ModContent.DustType<绿岩粒子>();
        public override Color Color => new Color(11, 255, 11);
        public override int DoorID => ModContent.TileType<绿岩门开Tile>();
        public override int ItemID => Icon;
        public override Vector3 LightColor => new Vector3(0, 0.15F, 0);
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
        public override void SetDefaults()
        {
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩门关", out int GG);
            Main.tileGlowMask[Type] = (short)GG;
        }
    }
}