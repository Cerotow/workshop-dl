using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.NPCs.Boss.天地守卫;
using DDmod.Content.NPCs.Boss.蘑菇王;
using DDmod.Content.Biome;

namespace DDmod.Content.Items.农场.食物.料理
{
	public class 奇怪的蘑菇汤 : ModItem
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
            Item.GetGlobalItem<FoodGlobalItem>().生命buff(20, DDHelper.Second(600));
            Item.GetGlobalItem<FoodGlobalItem>().生命回复buff(2,DDHelper.Second(600));
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.useTurn = true;
		}

		public override void SetStaticDefaults()
        {
            ItemID.Sets.DrinkParticleColors[Item.type] = new Color[] {
                new Color(237, 160, 69),
                new Color(254, 67, 58)
            };
            ItemID.Sets.IsFood[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override bool? UseItem(Player player)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<蘑菇王>())&&(player.ZoneForest|| player.GetModPlayer<农场Player>().农场))
            {
                if (Main.netMode != 1)
                    NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<蘑菇王>());
                else
                    NetMessage.SendData(61, -1, -1, null, player.whoAmI, ModContent.NPCType<蘑菇王>());
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(356, 1).AddIngredient(5, 3).AddIngredient(ModContent.ItemType<赤红蕈菌>(), 1).AddTile(TileID.CookingPots).Register();
        }
    }
}