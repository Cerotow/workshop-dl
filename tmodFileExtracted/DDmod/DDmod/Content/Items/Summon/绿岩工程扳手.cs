using DDmod.Content.Projectiles.Summon.Minions.Fort;

namespace DDmod.Content.Items.Summon
{
    public class 绿岩工程扳手 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<绿岩工程炮台>();
            Item.shootSpeed = 2;
            Item.sentry = true;
        }
        public override bool CanUseItem(Player player)
        {
            Vector2 vector = Vector2.Zero;
            for (int A = 0; A < 100; A++)
            {
                if (DDHelper.SolidTile((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16 + A)))
                {
                    vector = new Vector2((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16 + A-1));
                    break;
                }
            }
            if(vector==Vector2.Zero)
            {
                return false;
            }
            for (int A = 0; A < 1000; A++)
            {
                Projectile projectile = Main.projectile[A];
                if (projectile.active && projectile.type == Item.shoot && (projectile.Center - vector*16).Length() < 30)
                {
                    return false;
                }
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vector = Vector2.Zero;
            for (int A = 0; A < 100; A++)
            {
                if (DDHelper.SolidTile((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16 + A)))
                {
                    vector = new Vector2((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16 + A - 1));
                    break;
                }
            }
            Projectile projectile = NewProjectileDirect(source, vector*16, Vector2.Zero, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;
            player.UpdateMaxTurrets();
            return false;
        }
    }
}
