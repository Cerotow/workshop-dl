using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Boss.克苏鲁之眼
{
	[AutoloadEquip(EquipType.Body)]
	public class 血嘴衣 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}
        public override void UpdateEquip(Player player)
		{
            player.statLifeMax2 += 20;
        }
	}
}