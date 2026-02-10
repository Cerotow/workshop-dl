
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Items.Boss.鬼牙
{
    [AutoloadEquip(EquipType.Head)]
    public class 鬼牙面具 : ModItem
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