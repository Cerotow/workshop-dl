using DDmod.Content.Projectiles.Melee.ball;
using DDmod.Content.Projectiles.Summon.Whip;
using Terraria;

namespace DDmod.Content.Items.Boss.狱火蛇物品
{
    public class 狱火链刃 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.autoReuse = true;
            Item.useStyle = 13;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.width = 18;
            Item.height = 18;
            Item.shoot = ModContent.ProjectileType<狱火链刃Proj>();
            Item.knockBack = 1;
            Item.shootSpeed = 10;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noUseGraphic = true;
            Item.value = Item.buyPrice(0, 31, 20, 0);
            Item.rare = 8;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 2;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}