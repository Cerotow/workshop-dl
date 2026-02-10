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
    public class 史莱姆剑盾 : Swordshield
    {
        public override void Set()
        {
            Item.damage = 18;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 18;
            Item.knockBack = 3;
            Item.defense = 3;
            Item.value = 100;
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<史莱姆剑盾Proj>();
            Item.shootSpeed = 5;
            Endurance = 0.4f;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 3;
        }
    }
}