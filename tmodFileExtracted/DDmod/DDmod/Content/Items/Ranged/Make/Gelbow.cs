using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.Make
{
	public class Gelbow : ModItem
	{
		public override void Load()
		{
			//DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
		}
		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 18;
			Item.height = 54;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(0, 0, 0, 10);
			Item.rare = 2;
			Item.shoot = ProjectileID.IchorArrow;
			Item.shootSpeed = 4f;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = 40;
			Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
			Item.GetGlobalItem<RangedGlobalItem>().SetString(1, 5, 1, 0, 2);
			ModifyBow.Load(Type, new Modify(), true);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gel bow");
		//DisplayName.AddTranslation(7,"凝胶弓");
			if (Main.netMode != 2)
				DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position += velocity.PerfectNormalize() * 10;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Gel, 24).AddTile(TileID.WorkBenches).Register();
		}
		public class Modify : ModifyBow
		{
			public override void PostUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
			{
				if (ranged.Ammo[0] == 2)
				{
					item.SetDefaults(ModContent.ItemType<FireGelbow>());
				}
				if (ranged.Ammo[0] == 103)
				{
					item.SetDefaults(ModContent.ItemType<CursedGelbow>());
				}
				if (ranged.Ammo[0] == 172)
				{
					item.SetDefaults(ModContent.ItemType<FrostBurnGelbow>());
				}
			}
		}
	}
}