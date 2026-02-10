
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Items.Boss.天雷怒云
{
    public class 引雷针 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare =5;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<引雷针Proj>();
            Item.shootSpeed = 0;
            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;
        }
        public override bool CanUseItem(Player player)
        {
            return !AnyNPCs(ModContent.NPCType<NPCs.Boss.天雷怒云.天雷怒云>())&& Main.raining;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position.Y -= 50;
            NewProjectile(player.GetSource_FromAI(),position,velocity,Item.shoot,0,0,-1);
            type = 0;
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddRecipeGroup(RecipeGroupID.IronBar, 2).AddIngredient(520, 5).AddIngredient(521, 5).Register();
        }
    }
}
