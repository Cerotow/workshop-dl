using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace ArtificerMod.Content.Items.AccessoriesH
{
	[AutoloadEquip(EquipType.Wings)]
	public class NovaHologliderG : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 9f, 2.5f);

            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<NovaHologlider>()] = Type;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<NovaHologlider>();
        }

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.accessory = true;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(0, 40, 0, 0);
		}
	}
}
