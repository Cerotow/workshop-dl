using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Projectiles.Summon.Whip;
using Terraria;

namespace DDmod.Content.Items.Boss.绿岩之视
{
    public class 绿岩鞭 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            DGlobalItem.DefaultToWhip(Item, ModContent.ProjectileType<绿岩鞭Proj>(),22, 1, 9.5f, 20);
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 0, 21, 0);
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
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 24).AddIngredient(ModContent.ItemType<绿岩电池>(), 5).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
    }
}