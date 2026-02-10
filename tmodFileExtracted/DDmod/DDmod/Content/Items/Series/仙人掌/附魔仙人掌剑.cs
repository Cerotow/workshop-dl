using DDmod.Content.Projectiles.Melee;

namespace DDmod.Content.Items.Series.仙人掌
{
    public class 附魔仙人掌剑 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.crit = 2;
            Item.scale = 1.3F;
            Item.value = Item.buyPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        int r = 0;
        public override bool? UseItem(Player player)
        {
            r = 0;
            return base.UseItem(player);
        }
        public override void OnHitNPC(Player player, NPC target, HitInfo hit, int damageDone)
        {
            if (r == 0)
            {
                r++;
                for (int a = 0; a < Main.rand.Next(2, 6); a++)
                {
                    NewProjectile(player.GetSource_FromAI(), target.position + new Vector2(Main.rand.Next(0, target.width), Main.rand.Next(0, target.height)), new Vector2(Main.rand.Next(1, 3)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<仙人掌刺>(), hit.SourceDamage / 4, 0);
                }
            }
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(881).AddIngredient(ModContent.ItemType<仙人掌之魂>(), 4).AddTile(16).Register();
        }
    }
}