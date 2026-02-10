using System;
using DDmod.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.Make
{
	public class 仙人掌弓 : ModItem
	{
		public override void Load()
		{
		}
		public override void SetDefaults()
		{
			Item.damage = 4;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 38;
			Item.useTime = 17;
			Item.useAnimation = 17;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(0, 0, 3, 0);
			Item.rare = 2;
			Item.shoot = ProjectileID.IchorArrow;
			Item.shootSpeed = 9f;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = 40;
			Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
			Item.GetGlobalItem<RangedGlobalItem>().StringColor = new Color(211, 212, 186);
			Item.GetGlobalItem<RangedGlobalItem>().StringOffset = 14;
			Item.GetGlobalItem<RangedGlobalItem>().StringUP = 6;
			Item.GetGlobalItem<RangedGlobalItem>().StringDown = 6;
		}

		public override void SetStaticDefaults()
		{
			if (Main.netMode != 2)
				DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
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
			CreateRecipe(1).AddIngredient(276, 50).AddTile(18).Register();
		}
	}
}