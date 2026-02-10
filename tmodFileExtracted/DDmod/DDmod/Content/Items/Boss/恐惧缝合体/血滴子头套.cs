using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.OrnamentProjectile;
using DDmod.Content.Projectiles.Summon.ArmourSummons;
using DDmod.Content.Projectiles.Summon.ArmourSummons.Buff;
using DDmod.Content.Items.Series.苦难;

namespace DDmod.Content.Items.Boss.恐惧缝合体
{
	[AutoloadEquip(EquipType.Head)]
	public class 血滴子头套 : ModItem
	{
		public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 50);
			Item.rare = 4;
			Item.defense = 8;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
			return body.type == ModContent.ItemType<血滴子盔甲>() && legs.type == ModContent.ItemType<血滴子裤>();
        }
		public override void UpdateVanitySet(Player player)
		{
			int a = NewDust(player.position, player.width, player.height-4, 5, 0, 0, 100, default, Main.rand.NextFloat(0.4F, 1.2F));
			Main.dust[a].velocity = Vector2.Zero;
			Main.dust[a].rotation = player.velocity.ToRotation();
		}
		public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 25;
        }
		public override void UpdateArmorSet(Player player)
        {
		    player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.血滴子套");
			player.statLifeMax2 += 50;
			player.AddBuff(ModContent.BuffType<血肉仆从Buff>(),5);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<骨刺仆从>()] < 1)
            {
                NewProjectile(player.GetSource_FromAI(), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<骨刺仆从>(), 15, 0f, player.whoAmI, 0, 0);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<血眼仆从>()] < 1)
            {
                NewProjectile(player.GetSource_FromAI(), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<血眼仆从>(), 25, 0f, player.whoAmI, 0, 0);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<血嘴仆从>()] < 1)
            {
                NewProjectile(player.GetSource_FromAI(), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<血嘴仆从>(), 10, 0f, player.whoAmI, 0, 0);
            }
            player.Aplayer().血滴子套 = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<恐惧肉块>(), 10).AddIngredient(ModContent.ItemType<苦难之魂>(), 1).AddTile(TileID.Anvils).Register();
        }
    }
}