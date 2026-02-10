using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Items.Boss.流星破坏者
{
    public class 破坏者巨刃 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 144;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.useTurn = true;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.crit = 0;
            Item.value = Item.buyPrice(0, 26, 60, 0);
            Item.rare = 6;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(251, 127, 25, 0) * 0.6F;
            Item.shootSpeed = 8f;
            Item.autoReuse = true;
            Item.scale = 1.1F;
            Item.shootSpeed = 7;
            Item.GetGlobalItem<MeleeGlobalItem>().ColorL = true;
        }
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 8, false));
            //DisplayName.SetDefault("Haste of Hades");
            //DisplayName.AddTranslation(7, "哈迪斯之刃");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            r = false;
            return true;
        }
        bool r;
        public override void OnHitNPC(Player player, NPC target, HitInfo hit, int damageDone)
        {
            if (!r)
            {
                for (int a = 0; a < Main.rand.Next(2, 4 + 1); a++)
                {
                    NewProjectile(player.GetSource_FromAI(), target.Center, new Vector2(Main.rand.NextFloat(4, 7), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<流星锯刃>(), damageDone/4, 1, ai0: target.whoAmI);
                }
                NewDustChange4(40, target.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 1, 8, true, 2F, 4F, 100, 1000, new Color(251, 127, 25, 0), 2);
                r = true;
            }
        }
        public override bool AltFunctionUse(Player player)
        {
            return false;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}