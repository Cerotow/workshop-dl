using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace DDmod.Content.Items.农场.食物
{
	public class 玉米 : ModItem
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
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item2;
			Item.GetGlobalItem<FoodGlobalItem>().移速buff(0.2f, DDHelper.Second(300));
			Item.GetGlobalItem<FoodGlobalItem>().Food = true;
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.useTurn = true;
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.DrinkParticleColors[Item.type] = new Color[] {
				new Color(223, 212, 63)
			};
			
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[] {
				new Color(223, 212, 63)
			};

			ItemID.Sets.IsFood[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}
	}
}