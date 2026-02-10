using DDmod.Content.Projectiles.Melee;
using System;
using Terraria.GameContent.UI;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.Sword.NPCLoot
{
    public class 黄金大剑 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = 6;
            Item.scale = 1F;
            Item.shoot = ModContent.ProjectileType<圣金剑>();
            Item.shootSpeed = 12;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.珍宝;
            
            //Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(255, 121, 3)*0.3F;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(type==0)
                return false;
            bool overFlowing;
            long num = Utils.CoinsCount(out overFlowing, player.inventory, 58, 57, 56, 55, 54);
            if (num > 3000000)
            {
                num = 3000000;
            }
            player.PayCurrency((long)(num * 0.05F));
            if(num<100)
            {
                return false;
            }
            else
            if(num<10000)
            {
                NewProjectile(source,position,velocity,ModContent.ProjectileType<圣金剑>(),damage/2,knockback,-1,0,0,-5000);
                return false;
            }
            else
            if(num<1000000)
            {
                for (int A = -2; A <= 2; A++)
                {
                    NewProjectile(source, position+ velocity.RotatedBy(0.2F * A * -player.direction)*3, velocity.RotatedBy(0.2F*A*-player.direction), ModContent.ProjectileType<圣金剑>(), damage/4, knockback, -1,0,0,1*(A-2));
                }
                return false;
            }
            else
            {
                for (int A = -2; A <= 2; A++)
                {
                    NewProjectile(source, position + velocity.RotatedBy(0.2F * A * -player.direction) * 3, velocity.RotatedBy(0.2F*A * -player.direction), ModContent.ProjectileType<圣金剑>(), damage/4, knockback, -1, 0, 0, 1 * (A - 2));
                    if(A ==0)
                    {
                        NewProjectile(source, position, velocity, ModContent.ProjectileType<圣金剑>(), damage, knockback*2, -1,0,0,-10000);
                    }
                }
                return false;
            }
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            bool overFlowing;
            long num = Utils.CoinsCount(out overFlowing, player.inventory, 58, 57, 56, 55, 54);
            if (num > 3000000)
            {
                num = 3000000;
            }
            damage *=  1+ num / 3000000F*2;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}