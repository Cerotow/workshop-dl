using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;
using ArtificerMod.Common;

namespace ArtificerMod.Content.Items.AccessoriesPH
{
	[AutoloadEquip(EquipType.Shoes)]
	public class AquaticBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.accessory = true;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.buyPrice(0, 20, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.accFlipper = true;
			player.waterWalk2 = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.WaterWalkingBoots)
                .AddIngredient(ItemID.Flipper)
                .AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
