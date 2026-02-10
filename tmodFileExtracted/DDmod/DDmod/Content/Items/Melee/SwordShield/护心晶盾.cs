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
    public class 护心晶盾 : Swordshield
    {
        public override void Load()
        {
        }
        public override void Unload()
        {
        }
        public override void Set()
        {
            Item.damage = 38;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 15;
            Item.knockBack = 3;
            Item.defense = 16;
            Item.value = 114514;
            Item.rare = 6;
            Item.shoot = ModContent.ProjectileType<护心晶盾Proj>();
            Item.shootSpeed = 5;
            Endurance = 0.65f;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 3;
        }
    }
}