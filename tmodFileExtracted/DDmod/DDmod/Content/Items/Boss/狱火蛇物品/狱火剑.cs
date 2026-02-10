using DDmod.Content.Projectiles.Melee;
using Terraria;

namespace DDmod.Content.Items.Boss.狱火蛇物品
{
    public class 狱火剑 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 108;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useTurn = true;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.crit = 0;
            Item.value = Item.buyPrice(0, 31, 20, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(255, 74, 50, 0)*0.3F;
            Item.DItem().HandheldColor = new Color(255,255,255,150);
            Item.shoot = ModContent.ProjectileType<HellfireBlaze>();
            Item.shootSpeed = 2f;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<地狱之火>(), 300);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}