using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;
using ArtificerMod.Common;

namespace ArtificerMod.Content.Items.AccessoriesPH
{
    [AutoloadEquip(EquipType.Waist)]
    public class WeatherBottle : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.accessory = true;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.buyPrice(0, 15, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetJumpState(ExtraJump.SandstormInABottle).Enable();
            player.GetJumpState(ExtraJump.BlizzardInABottle).Enable();
            player.GetJumpState(ExtraJump.CloudInABottle).Enable();
        }

		public override void AddRecipes()
		{
			CreateRecipe()
                .AddIngredient(ItemID.SandstorminaBottle)
                .AddIngredient(ItemID.BlizzardinaBottle)
                .AddIngredient(ItemID.CloudinaBottle)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
        }
	}
}
