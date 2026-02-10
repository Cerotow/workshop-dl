using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.Items.农场;

namespace DDmod.Content.Items.Boss.MiniBoss
{
	public class 恶魔之血 : ModItem
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
			Item.buffType = ModContent.BuffType<恶魔力量Buff>();
			Item.buffTime = 600;

            Item.healLife = 30;
            Item.potion = true;
            Item.DItem().PotionCD = DDHelper.Second(80);
        }
        public override void GetHealLife(Player player, bool quickHeal, ref int healValue)
        {
			healValue = player.statLifeMax2 / 100;
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.DrinkParticleColors[Item.type] = new Color[] {
                new Color(155, 0, 0),
            };
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}
        public override bool? UseItem(Player player)
        {
			恶魔力量Buff.A = 1;
            return base.UseItem(player);
        }
    }
}