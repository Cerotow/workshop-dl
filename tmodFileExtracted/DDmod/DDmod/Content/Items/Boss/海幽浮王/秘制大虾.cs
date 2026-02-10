using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.NPCs.Boss.星心守卫;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.Content.Items.Boss.海幽浮王
{
	public class 秘制大虾 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 20;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = 3200;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.consumable = true;
			Item.bait = 33;
		}
        public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            base.ModifyFishingLine(bobber, ref lineOriginOffset, ref lineColor);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(2316).AddIngredient(4608).AddTile(TileID.WorkBenches).Register();
        }
	}
}