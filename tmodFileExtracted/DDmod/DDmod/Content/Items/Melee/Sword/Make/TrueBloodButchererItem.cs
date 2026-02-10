using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Items.Melee.Sword.Make
{
    public class TrueBloodButchererItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 1;
            Item.shoot = ModContent.ProjectileType<TrueBloodButcherer>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            Vector2 vector2 = player.RotatedRelativePoint(player.ArmCenter(), true);
            float AttackSpeed = -player.HeldItem.useAnimation;
            int A = NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI);
            Main.projectile[A].DProj().Times[0] = player.direction;
            Main.projectile[A].localAI[0] = 2 * player.direction;
            float U = Main.projectile[A].localAI[0] + (Main.projectile[A].localAI[1] / (AttackSpeed * 3));
            U = -U;
            Main.projectile[A].localAI[1]++;
            if (Main.projectile[A].localAI[1] > 0)
            {
                Main.projectile[A].localAI[1] = 0;
            }
            Main.projectile[A].ai[0] = U;
            if (Main.projectile[A].owner == Main.myPlayer)
            {
                Main.projectile[A].DProj().vector[0] = (Main.MouseWorld - vector2).PerfectNormalize();
                Main.projectile[A].netUpdate = true;
            }
            int B = NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI);
            Main.projectile[B].DProj().Times[0] = -player.direction;
            Main.projectile[B].localAI[0] = 2 * -player.direction;
            U = Main.projectile[B].localAI[0] + (Main.projectile[B].localAI[1] / (AttackSpeed * 3));
            U = -U;
            Main.projectile[B].localAI[1]++;
            if (Main.projectile[B].localAI[1] > 0)
            {
                Main.projectile[B].localAI[1] = 0;
            }
            Main.projectile[B].ai[0] = U;
            if (Main.projectile[B].owner == Main.myPlayer)
            {
                Main.projectile[B].DProj().vector[0] = (Main.MouseWorld - vector2).PerfectNormalize();
                Main.projectile[B].netUpdate = true;
            }
            return false;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            //CreateRecipe(1).AddIngredient(ItemID.Amethyst, 12).AddTile(TileID.Anvils).Register();
        }
    }
}