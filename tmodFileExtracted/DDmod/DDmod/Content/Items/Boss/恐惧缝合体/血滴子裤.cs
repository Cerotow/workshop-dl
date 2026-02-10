using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Items.Series.苦难;

namespace DDmod.Content.Items.Boss.恐惧缝合体
{
	[AutoloadEquip(EquipType.Legs)]
	public class 血滴子裤 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 80);
            Item.rare = 4;
			Item.defense = 5;
        }
        public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.2f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<恐惧肉块>(), 8).AddIngredient(ModContent.ItemType<苦难之魂>(), 1).AddTile(TileID.Anvils).Register();
        }
    }
}