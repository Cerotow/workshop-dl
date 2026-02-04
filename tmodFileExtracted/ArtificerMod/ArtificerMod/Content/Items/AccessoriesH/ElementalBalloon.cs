using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Items.AccessoriesH
{
	[AutoloadEquip(EquipType.Balloon)]
	public class ElementalBalloon : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.accessory = true;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(0, 50, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetJumpState(ExtraJump.CloudInABottle).Enable();
			player.GetJumpState(ExtraJump.BlizzardInABottle).Enable();
			player.GetJumpState(ExtraJump.SandstormInABottle).Enable();
			player.GetJumpState(ExtraJump.FartInAJar).Enable();
			player.GetJumpState(ExtraJump.TsunamiInABottle).Enable();
			player.jumpBoost = true;
			player.noFallDmg = true;
			player.hasLuck_LuckyHorseshoe = true;
			player.honeyCombItem = Item;
		}

		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			if (equippedItem.wingSlot >= 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.HorseshoeBundle)
				.AddIngredient(ItemID.FartInABalloon)
				.AddIngredient(ItemID.SharkronBalloon)
				.AddIngredient(ItemID.HoneyBalloon)
				.AddIngredient(ItemID.SoulofFlight, 20)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
