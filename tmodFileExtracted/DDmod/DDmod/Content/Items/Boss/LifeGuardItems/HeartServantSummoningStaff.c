using Microsoft.Xna.Framework;

using Terraria.ID;
using Terraria.ModLoader;

namespace slime.Items.Boss.愤怒双子
{
	public class 水晶仆从召唤杖 : ModItem
	{

		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.summon = true;
			Item.mana = 10;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 8000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item44;
			Item.shoot = Mod.Find<ModProjectile>("水晶仆从").Type;
		}

		public override void SetStaticDefaults()
		{
		//DisplayName.SetDefault("水晶仆从召唤杖");
		//Tooltip.SetDefault("召唤水晶仆从为你而战");
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.altFunctionUse != 2;
		}
	}
}
