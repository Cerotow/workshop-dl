using DDmod.Content.Projectiles.Melee.ball;
using DDmod.Content.Tiles.Mine;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Boss.Boss特殊
{
	public class 史莱姆链球 : ModItem
	{
		public override void SetDefaults()
        {
            Item.damage = 33;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.scale = 1f;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 1;
            Item.shoot = ModContent.ProjectileType<史莱姆链球Proj>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 2;
            Item.DItem().DrawDefaults = true;
        }

		public override void SetStaticDefaults()
		{
		}

	}
}
