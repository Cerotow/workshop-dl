using DDmod.Content.Projectiles.Melee;
using Terraria;

namespace DDmod.Content.Items.Series.Acorn
{
    public class AcornSword : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8f;
            Item.crit = 100;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.shoot = ModContent.ProjectileType<MeleeAcorn>();
            Item.shootSpeed = 5f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
        int T = 0;
        public override void HoldItem(Player player)
        {
            if (T > 0)
            {
                T -= 1;
            }
            if (T <= 0 && player.whoAmI == Main.myPlayer && Main.mouseRight && Main.mouseRightRelease)
            {
                for(int a = 0; a < player.inventory.Length;a++)
                {
                    if(player.inventory[a].type==27)
                    {
                        player.inventory[a].stack--;
                        if(player.inventory[a].stack<=0)
                        {
                            player.inventory[a].SetDefaults(0);
                        }
                        NewProjectileChange(player.GetSource_ItemUse_WithPotentialAmmo(Item, 0), player.Center, new Vector2(0, -6), Item.shoot, player.GetWeaponDamage(Item) * 3, player.GetWeaponKnockback(Item), -1, 0, 0, 3);
                        T = 15;
                        break;
                    }
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<SoulOfNature>(), 12).AddIngredient(ItemID.Acorn, 8).AddTile(TileID.WorkBenches).Register();
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}