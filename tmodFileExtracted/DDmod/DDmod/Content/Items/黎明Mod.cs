using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.天地守卫;
using DDmod.Content.NPCs.Boss.星心守卫;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items
{
	public class 黎明Mod : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 3200;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = false;
		}

		public override bool? UseItem(Player player)
        {
			if (!DDSystem.English&&player.whoAmI==Main.myPlayer)
			{
				Utils.OpenToURL("https://space.bilibili.com/417426564");
				if(player.altFunctionUse==2)
				{
                    Utils.OpenToURL("https://afdian.com/a/xidong1119");
                }
			}
			return true;
        }
        public override bool AltFunctionUse(Player player)
		{
			return true;
		}
    }
}