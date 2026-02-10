using System;
using DDmod.Content.Projectiles.Melee.TwinSwords;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DDmod.Content.Buffs.DeBuffs;

namespace DDmod.Content.Items.Melee.TwinSwords
{
	public class 叶木双剑 : Twinswords
    {
		public override void Set()
		{
            Item.damage = 8;
            Item.width = 22;
            Item.height = 38;
			Item.useTime = 18;
            Item.knockBack = 3;
            Item.value = 100;
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<叶木双剑Proj>();
            Item.shootSpeed = 5;
        }
	}
}