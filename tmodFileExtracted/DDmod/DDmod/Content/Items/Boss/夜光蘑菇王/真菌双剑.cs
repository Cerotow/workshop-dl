using System;
using DDmod.Content.Projectiles.Melee.TwinSwords;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DDmod.Content.Buffs.DeBuffs;

namespace DDmod.Content.Items.Boss.夜光蘑菇王
{
	public class 真菌双剑 : Twinswords
    {
        public override void Set()
        {
            Item.damage = 28;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 12;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<真菌双剑Proj>();
            Item.shootSpeed = 5;
        }
	}
}