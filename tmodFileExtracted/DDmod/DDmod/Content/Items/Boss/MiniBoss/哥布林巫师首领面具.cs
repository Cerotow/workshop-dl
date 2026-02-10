
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Items.Boss.MiniBoss
{
    [AutoloadEquip(EquipType.Head)]
    public class 哥布林巫师首领面具 : ModItem
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
        }
    }
}