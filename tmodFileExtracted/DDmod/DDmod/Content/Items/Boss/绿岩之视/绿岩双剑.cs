using System;
using DDmod.Content.Projectiles.Melee.TwinSwords;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DDmod.Content.Buffs.DeBuffs;

namespace DDmod.Content.Items.Boss.绿岩之视
{
    public class 绿岩双剑 : Twinswords
    {
        public override void Set()
        {
            Item.damage = 35;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 9;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<绿岩双剑Proj>();
            Item.shootSpeed = 5;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 3;
        }
    }
}