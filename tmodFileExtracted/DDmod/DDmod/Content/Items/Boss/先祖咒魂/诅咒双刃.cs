using System;
using DDmod.Content.Projectiles.Melee.TwinSwords;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DDmod.Content.Buffs.DeBuffs;

namespace DDmod.Content.Items.Boss.先祖咒魂
{
	public class 诅咒双刃 : Twinswords
    {
        public override float SpecialMagnification => 0.75F;
        public override float Magnification => 1.25F;
        public override void Set()
        {
            Item.damage = 82;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 12;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 12, 20, 0);
            Item.rare = 7;
            DDSystem.Instance.DDEquipGlow.TryGetValue("诅咒双刃", out int GG);
            Item.glowMask = (short)GG;
            Item.shoot = ModContent.ProjectileType<诅咒双刃Proj>();
            Item.shootSpeed = 5;
            Item.DItem().TwinGlow = 4;
        }
	}
}