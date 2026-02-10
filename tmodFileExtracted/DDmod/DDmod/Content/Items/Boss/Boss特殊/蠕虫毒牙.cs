using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Melee;
using Terraria;

namespace DDmod.Content.Items.Boss.Boss特殊
{
    public class 蠕虫毒牙 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useTurn = true;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.crit = 0;
            Item.value = Item.buyPrice(0, 8, 20, 0);
            Item.rare = 4;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.shoot = ModContent.ProjectileType<魔唾液>();
            Item.shootSpeed = 8f;
            Item.autoReuse = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 3;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(71, 99, 27, 100)*2;
            Item.GetGlobalItem<MeleeGlobalItem>().Color2 = new Color(44, 71, 1, 255);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int P = NewProjectile(source, position, velocity, type, damage / 4, knockback / 4, -1, 0, 0, Main.rand.NextFloat(0.5F, 0.75F));
            Main.projectile[P].DamageType = DamageClass.Melee;
            for (int a = 0; a < Main.rand.Next(5) + 1; a++)
            {
                P = NewProjectile(source, position, velocity.RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * Main.rand.NextFloat(0.6f, 1F), type, damage / 4, knockback / 4,-1,0,0,Main.rand.NextFloat(0.5F,0.75F));
                Main.projectile[P].DamageType = DamageClass.Melee;
            }
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.HasBuff(ModContent.BuffType<蠕虫毒牙Buff>()))
            {
                target.AddBuff(ModContent.BuffType<蠕虫毒牙Buff>(), 120, false);
            }
            else
            {
                target.AddBuff(ModContent.BuffType<蠕虫毒牙Buff>(), 120+ target.buffTime[target.FindBuffIndex(ModContent.BuffType<蠕虫毒牙Buff>())], false);
            }
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}