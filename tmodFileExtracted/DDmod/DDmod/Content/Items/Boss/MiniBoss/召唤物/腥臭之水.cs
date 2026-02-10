using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.Items.农场;

namespace DDmod.Content.Items.Boss.MiniBoss.召唤物
{
	public class 腥臭之水 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.consumable = true;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item3;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.useTurn = true;
			Item.buffType = ModContent.BuffType<腥臭Buff>();
			Item.buffTime = DDHelper.Second(300);
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.DrinkParticleColors[Item.type] = new Color[] {
                new Color(150,20,10,150),
            };
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(126, 1).AddIngredient(1330, 3).AddIngredient(880, 4).AddTile(TileID.Bottles).Register();
        }
    }
}