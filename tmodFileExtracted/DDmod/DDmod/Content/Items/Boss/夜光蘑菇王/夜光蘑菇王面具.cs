
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Items.Boss.夜光蘑菇王
{
    [AutoloadEquip(EquipType.Head)]
    public class 夜光蘑菇王面具 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.rare = ItemRarityID.Orange;
            Item.vanity= true;
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            DDSystem.Instance.DDEquipGlow.TryGetValue("夜光蘑菇王面具", out int GG);
            glowMask = GG;
            glowMaskColor = Color.White;
        }
    }
}