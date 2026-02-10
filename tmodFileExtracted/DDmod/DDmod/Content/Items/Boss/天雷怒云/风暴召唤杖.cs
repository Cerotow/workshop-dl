using DDmod.Content.Buffs.PlayerBuffs;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace DDmod.Content.Items.Boss.天雷怒云
{
    public class 风暴召唤杖 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 0;

        }
        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.buffType = ModContent.BuffType<风暴雨云Buff>();
            Item.shoot = ModContent.ProjectileType<风暴雨云>();
            Item.shootSpeed = 2;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            if (player.ownedProjectileCounts[Item.shoot] == 0)
            {
                Projectile projectile = NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<风暴计数器>(), 0, knockback, Main.myPlayer, 0, 0, 1);
                projectile.originalDamage = Item.damage / 2;
                projectile = NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, Main.myPlayer,0,0,1);
                projectile.originalDamage = Item.damage;
            }
            else
            {
                Projectile projectile = NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<风暴计数器>(), 0, knockback, Main.myPlayer, 0, 0, 1);
                projectile.originalDamage = Item.damage/2;
            }
            /*
            else
            {
                float A = 0;
                for (int i = 0; i < 1000; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == player.whoAmI)
                    {
                        A += proj.minionSlots;
                    }
                }
                for (int i = 0; i < 1000; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == player.whoAmI && proj.type == Item.shoot)
                    {
                        if (A<player.maxMinions)
                        {
                            proj.ai[2]++;
                            proj.originalDamage += Item.damage / 2;
                            proj.DProj().Bool[0] = false;
                        }
                    }
                }
            }*/
            return false;
        }
    }
}
