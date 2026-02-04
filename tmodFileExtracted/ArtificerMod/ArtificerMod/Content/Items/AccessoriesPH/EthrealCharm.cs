using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using ArtificerMod.Common;

namespace ArtificerMod.Content.Items.AccessoriesPH
{
	public class EtherealCharm : ModItem
	{
		public static int ReducedManaCost = 12;
		public static int IncreasedMaxMana = 20;
 		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ReducedManaCost, IncreasedMaxMana);

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<GiftOfChanneling>()] = Type;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<GiftOfChanneling>();
        }

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.accessory = true;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.buyPrice(0, 12, 50, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.manaCost -= 0.12f;
			player.statManaMax2 += 20;
			player.manaFlower = true;
            player.GetModPlayer<ArtificerPlayer>().hideMagicWeps = !hideVisual;
        }

        public override void UpdateVanity(Player player)
        {
			player.GetModPlayer<ArtificerPlayer>().hideMagicWeps = true;
        }
    }
}
