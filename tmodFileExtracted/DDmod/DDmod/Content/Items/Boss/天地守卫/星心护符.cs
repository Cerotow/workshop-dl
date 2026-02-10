using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Projectiles.Pet.MasterPet;
using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;

namespace DDmod.Content.Items.Boss.天地守卫
{
    public class 星心护符 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<SonOfTheLifeGuard>(), ModContent.BuffType<星心之子Buff>());
            Item.width = 28;
            Item.height = 20;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.buyPrice(0, 3, 0, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LifeAmulet>()).AddIngredient(ModContent.ItemType<StarAmulet>()).AddIngredient(ModContent.ItemType<星心之魂>()).Register();
        }
    }
}
