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
using DDmod.Content.Projectiles.Summon.Whip;

namespace DDmod.Content.Items.Melee.SwordShield
{
    public class 绿岩剑盾 : Swordshield
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
            Item.defense = 20;
            Item.value = 5000;
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩剑盾", out int GG);
            Item.glowMask = (short)GG;
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<绿岩剑盾Proj>();
            Item.shootSpeed = 5;
            Endurance = 0.6f;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 1;
            Item.DItem().TwinGlow = 3;
        }
    }
}