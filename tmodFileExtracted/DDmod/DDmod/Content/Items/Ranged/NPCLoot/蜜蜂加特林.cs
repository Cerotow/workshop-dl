using System;
using DDmod.Content.Items.Series.钢;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Sync;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.NPCLoot
{
    public class 蜜蜂加特林 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 3;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 2, 25, 0);
            Item.rare = 6;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.shootSpeed = 8;
            Item.autoReuse = true;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.UseSound = null;
            Item.useAmmo = AmmoID.Bullet;

            Item.shoot = ModContent.ProjectileType<蜜蜂加特林Proj>();
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.罕见;
            Item.GetGlobalItem<RangedGlobalItem>().NoConsumesAmmo = 57;
        }
        public override void SetStaticDefaults()
        {
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
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
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return player.itemTime > 0;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 0f, 0f, Item.type);
            return false;
        }
        public override void AddRecipes()
        {
        }
    }
}