using DDmod.Content.Projectiles.Melee;
using Terraria;

namespace DDmod.Content.Items.Boss.LifeGuardItems
{
    public class HeartSword : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useTurn = true;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.crit = 0;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.shoot = ModContent.ProjectileType<FragileHeartOfLife>();
            Item.shootSpeed = 20f;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            A = 0;
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        int A;
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (A == 0 && Main.rand.NextBool(5))
            {
                A++;
                for (int i = 0; i < Main.rand.Next(2, 4); i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(new Vector2(Item.shootSpeed / 4), Main.rand.NextFloat(0, MathHelper.TwoPi), default);
                    int A = NewProjectile(player.GetSource_FromAI(), target.Center, projDirection * 0.5f, ModContent.ProjectileType<MeleeHeart>(), hit.SourceDamage, 0f, 0, 0, 0);
                }
            }
            player.AddBuff(ModContent.BuffType<BlessingOfTheHeart>(), 600, false);
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}