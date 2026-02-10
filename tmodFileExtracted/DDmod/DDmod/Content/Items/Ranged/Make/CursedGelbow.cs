using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.Make
{
	public class CursedGelbow : ModItem
	{
		public override void Load()
		{
			DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
		}
		public override void SetDefaults()
		{
			Item.damage = 42;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 38;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(0, 0, 1, 0);
			Item.rare = 6;
			Item.shoot = ProjectileID.IchorArrow;
			Item.shootSpeed = 8f;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.useAmmo = 40;
			Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            Item.GetGlobalItem<RangedGlobalItem>().PrePostConvertArrows = new int[] { 1 };
			Item.GetGlobalItem<RangedGlobalItem>().PostConvertArrows = ProjectileID.CursedArrow;
            Item.GetGlobalItem<RangedGlobalItem>().SetString(5, 5, 1, 0, 2);
        }

		public override void SetStaticDefaults()
		{
			if (Main.netMode != 2)
				DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
		}
		int Time;
        public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Time++;
			if (Time > 900)
			{
				Item.SetDefaults(0);
			}
			Dust dust =Main.dust[NewDust(Item.position, Item.width, Item.height, 75, 0,0,0,default,Main.rand.NextFloat(0.8F,1.5F))];
			dust.noGravity = false;
		}
        public override void UpdateInventory(Player player)
		{
			Time++;
			if (Time > 900)
			{
				Item.SetDefaults(0);
			}
		}
        public override void HoldItem(Player player)
        {
			player.AddBuff(BuffID.CursedInferno,2);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			if(type==1)
            {
				type = 103;
			}
			position += velocity.PerfectNormalize() * 10;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}