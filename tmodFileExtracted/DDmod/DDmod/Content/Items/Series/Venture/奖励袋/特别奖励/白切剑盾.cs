using System;
using DDmod.Content.Projectiles.Melee.TwinSwords;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee.SwordShield;

namespace DDmod.Content.Items.Series.Venture.奖励袋.特别奖励
{
    public class 白切剑盾 : Swordshield
    {
        public override void Set()
        {
            Item.damage = 24;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 15;
            Item.knockBack = 3;
            Item.defense = 14;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<白切剑盾Proj>();
            Item.shootSpeed = 5;
            Endurance = 0.5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<白切精华>()).Register();
        }
    }
}