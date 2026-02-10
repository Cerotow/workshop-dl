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
    public class 木制剑盾 : Swordshield
    {
        public override void Set()
        {
            Item.damage = 8;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 18;
            Item.knockBack = 3;
            Item.defense = 5;
            Item.value = 100;
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<木制剑盾Proj>();
            Item.shootSpeed = 5;
            Endurance = 0.6F;
        }
    }
}