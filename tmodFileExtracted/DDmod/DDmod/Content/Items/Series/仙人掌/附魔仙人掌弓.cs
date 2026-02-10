using DDmod.Content.Dusts;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Projectiles.Ranged;

namespace DDmod.Content.Items.Series.仙人掌
{
    public class 附魔仙人掌弓 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            if (Main.netMode != 2)
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.crit = 10;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<仙人掌箭>();
            Item.shootSpeed = 7f;
            Item.useAmmo = 40;
            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            ModifyBow.Load(Type, new Modify(), true);
            Item.GetGlobalItem<RangedGlobalItem>().PrePostConvertArrows = new int[] { 1 };
            Item.GetGlobalItem<RangedGlobalItem>().PostConvertArrows = ModContent.ProjectileType<仙人掌箭>();
            Item.GetGlobalItem<RangedGlobalItem>().ConvertProbability = 100;
            Item.GetGlobalItem<RangedGlobalItem>().SetString(7, 7, 3, 0, 8, false, new Color(0, 255, 0, 0));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<仙人掌弓>()).AddIngredient(ModContent.ItemType<仙人掌之魂>(), 4).AddTile(16).Register();
        }
        public class Modify : ModifyBow
        {
            public override void Skill(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
            }
            public override void Shoot(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
            {
                Player player = Projectile.Player();
                IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ranged.BowUsedAmmoItemId[0]);
                float la = ranged.BowTime / player.itemAnimationMax * 6;
                float Charge = ranged.BowTime / player.itemAnimationMax;
                if (Charge > 1 && rangedItem.Skill == 0) Charge *= 1.5f;
                float Magnification = Charge;

                if (Projectile.owner == Main.myPlayer)
                {
                    //发射
                    int Proj1 = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                    Main.projectile[Proj1].scale = Projectile.scale;
                    Main.projectile[Proj1].Resize((int)(Main.projectile[Proj1].OriginalWidth() * (Main.projectile[Proj1].scale)), (int)(Main.projectile[Proj1].OriginalHeight() * (Main.projectile[Proj1].scale)));

                }
            }
        }
    }
}