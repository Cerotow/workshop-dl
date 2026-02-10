using DDmod.Content.Mounts;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Boss.MeteorAnnihilatorItems
{
	public class 流星遥控手环 : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Meteor Control Bracelet");
		//DisplayName.AddTranslation(7,"流星遥控手环");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 26;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 4;
			Item.value = 50000;
			Item.rare = 11;
			Item.master = true;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.mountType = ModContent.MountType<流星飞行器>();
		}
	}
}