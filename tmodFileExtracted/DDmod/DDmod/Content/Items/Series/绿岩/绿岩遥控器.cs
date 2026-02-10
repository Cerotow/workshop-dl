using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using Terraria.ID;

namespace DDmod.Content.Items.Series.绿岩
{
    public class 绿岩遥控器 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.knockBack = 3f;
            Item.mana = 10; // mana cost
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = 5;
            Item.value = Item.buyPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<绿岩侦察机Buff>();
            Item.shoot = ModContent.ProjectileType<绿岩侦察机>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            Projectile projectile = NewProjectileDirect(source, Main.MouseWorld, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 12).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
    }
}