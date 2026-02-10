using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Content.Dusts;

namespace DDmod.Content.Tiles.绿岩.家具
{
    public class 绿岩箱Tile : DDChest
    {
        public override int Icon => ModContent.ItemType<绿岩箱>();
        public override int Dust => ModContent.DustType<绿岩粒子>();
        public override Color Color => new Color(11, 255, 11);
        public override Vector3 LightColor => new Vector3(0, 0.15F, 0);
        public override LocalizedText name => Language.GetText("Mods.DDmod.Items.绿岩箱.DisplayName");
        public override void SetDefaults()
        {
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩箱", out int GG);
            Main.tileGlowMask[Type] = (short)GG;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
    }
}