
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Items.Boss.蘑菇王
{
    [AutoloadEquip(EquipType.Head)]
    public class 蘑菇王面具 : ModItem
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
    }
}