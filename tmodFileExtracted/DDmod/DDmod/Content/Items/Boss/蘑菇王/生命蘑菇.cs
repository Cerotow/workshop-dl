using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace DDmod.Content.Items.Boss.蘑菇王
{
	public class 生命蘑菇 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 30;
			Item.rare = 3;
			Item.consumable = true;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item2;
			Item.value = Item.buyPrice(0, 3,0, 0);
			Item.useTurn = true;
			Item.expert = true;
        }
        public override void GetHealLife(Player player, bool quickHeal, ref int healValue)
        {
			
        }
        public override bool? UseItem(Player player)
        {
			player.Dplayer().MushroomsLife = true;
			return true;
        }
        public override void SetStaticDefaults()
		{
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[] {
				new Color(237, 160, 69),
				new Color(201, 45, 45),
				new Color(195, 105, 39),
			};
			ItemID.Sets.IsFood[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}
	}
}