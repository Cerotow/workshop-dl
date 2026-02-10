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

namespace DDmod.Content.Items.Melee.SwordShield
{
    public class 蜂巢剑盾 : Swordshield
    {
        public override void Set()
        {
            Item.damage = 38;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 15;
            Item.knockBack = 3;
            Item.defense = 10;
            Item.value = 10000;
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<蜂巢剑盾Proj>();
            Item.shootSpeed = 5;
            Endurance = 0.7f;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 3;
        }
    }
}