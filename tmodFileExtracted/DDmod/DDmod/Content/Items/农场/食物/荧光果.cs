using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.Dusts;

namespace DDmod.Content.Items.农场.食物
{
	public class 荧光果 : ModItem
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
			Item.GetGlobalItem<FoodGlobalItem>().发光buff(DDHelper.Second(300),new Color(0,255,255).ToVector3());
			Item.GetGlobalItem<FoodGlobalItem>().Food = true;
			Item.GetGlobalItem<FoodGlobalItem>().SeniorFood = true;
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.useTurn = true;
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.IsFood[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}
        public override bool? UseItem(Player player)
        {
			player.GetModPlayer<FoodPlayer>().color = new Color(0, 255, 255, 0);
            return base.UseItem(player);
        }
        public override void HoldItemFrame(Player player)
		{
            base.HoldItemFrame(player);
        }
    }
}