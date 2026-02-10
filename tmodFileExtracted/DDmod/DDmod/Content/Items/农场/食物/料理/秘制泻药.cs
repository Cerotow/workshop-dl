using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace DDmod.Content.Items.农场.食物.料理
{
	public class 秘制泻药 : ModItem
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
            Item.GetGlobalItem<FoodGlobalItem>().Food = true;
            Item.GetGlobalItem<FoodGlobalItem>().SeniorFood = true;
            Item.GetGlobalItem<FoodGlobalItem>().移速buff(-0.8f,DDHelper.Second(666));
			Item.GetGlobalItem<FoodGlobalItem>().伤害buff(-0.8f, DDHelper.Second(666),DamageClass.Generic);
			//Item.GetGlobalItem<FoodGlobalItem>().Food = true;
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.useTurn = true;
			Item.buffType = ModContent.BuffType<拉肚子>();
			Item.buffTime = DDHelper.Second(666)*60;

        }

		public override void SetStaticDefaults()
        {
            ItemID.Sets.DrinkParticleColors[Item.type] = new Color[] {
                new Color(57, 60, 226),
                new Color(133, 131, 195),
                new Color(182, 175, 130)
            };
            ItemID.Sets.IsFood[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}
	}
}