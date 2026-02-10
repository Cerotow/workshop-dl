using System;
using DDmod.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.Venture.奖励袋.特别奖励
{
	public class 樱之弓 : ModItem
	{
		public override void Load()
		{
		}
		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 38;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.rare = 2;
			Item.shoot = ProjectileID.IchorArrow;
			Item.shootSpeed = 7f;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = 40;
			Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
			Item.GetGlobalItem<RangedGlobalItem>().StringUP = 6;
			Item.GetGlobalItem<RangedGlobalItem>().StringDown = 8;
			Item.GetGlobalItem<RangedGlobalItem>().StringOffset = 7;
			Item.GetGlobalItem<RangedGlobalItem>().YOffset = 1;
			Item.GetGlobalItem<RangedGlobalItem>().Offset = 4;
			Item.GetGlobalItem<RangedGlobalItem>().StringColor = new Color(255,100,153);
			Item.GetGlobalItem<RangedGlobalItem>().PrePostConvertArrows = new int[] { 1 };
			Item.GetGlobalItem<RangedGlobalItem>().PostConvertArrows = ModContent.ProjectileType<樱之箭>();
		}
		public static Asset<Texture2D> asset;
		public override void SetStaticDefaults()
		{
			if (Main.netMode != 2)
			{
				DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
				asset = ModContent.Request<Texture2D>(Texture + "_E");
			}
		}
        public override void UpdateInventory(Player player)
		{
		}
        public override void HoldItem(Player player)
        {
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
		public override void AddRecipes()
		{
		}
	}
}