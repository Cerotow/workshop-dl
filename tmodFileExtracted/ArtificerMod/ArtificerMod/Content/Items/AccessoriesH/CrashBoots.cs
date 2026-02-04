using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using ArtificerMod.Common;
using ArtificerMod.Content.Items.AccessoriesPH;

namespace ArtificerMod.Content.Items.AccessoriesH
{
	[AutoloadEquip(EquipType.Shoes)]
	public class CrashBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.accessory = true;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.buyPrice(0, 20, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// Disable other dashes
			player.dashType = 1;

            if (player.controlDown && !player.mount.Active && !player.PortalPhysicsEnabled)
            {
                player.maxFallSpeed *= 1.7f;
            }
            player.GetModPlayer<ArtificerPlayer>().heavyShoes = true;
            player.extraFall += 30;
            player.buffImmune[BuffID.Burning] = true;
            player.noKnockback = true;
        }

		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			if (equippedItem.type == ModContent.ItemType<HeavyBoots>() || equippedItem.type == ModContent.ItemType<DynamicBoots>() || equippedItem.type == ModContent.ItemType<PowerBoots>() ||
                incomingItem.type == ModContent.ItemType<HeavyBoots>() || incomingItem.type == ModContent.ItemType<DynamicBoots>() || incomingItem.type == ModContent.ItemType<PowerBoots>())
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<PowerBoots>()
				.AddIngredient(ItemID.Tabi)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
