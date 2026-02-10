using DDmod.Content.Dusts;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Ranged;

namespace DDmod.Content.Items.Series.绿岩
{
    public class 绿岩弓 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            if (Main.netMode != 2)
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
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
            //Item.GetGlobalItem<RangedGlobalItem>().PrePostConvertArrows = new int[] { 1 };
            //Item.GetGlobalItem<RangedGlobalItem>().PostConvertArrows = ModContent.ProjectileType<仙人掌箭>();
            //Item.GetGlobalItem<RangedGlobalItem>().ConvertProbability = 100;
            Item.GetGlobalItem<RangedGlobalItem>().SetString(4, 4, 3, 0, 8, false, new Color(119, 237, 130, 0));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 12).AddTile(16).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
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

                    int T = NewProjectile(player.GetSource_FromAI(), Projectile.Center, Projectile.velocity.PerfectNormalize()* 4 * Charge, ModContent.ProjectileType<绿岩能量>(), (int)(ranged.BowDamage[0] * Charge), 0, -1, -1, 2, 1);
                    Main.projectile[T].DamageType = DamageClass.Ranged;
                }
            }
        }
    }
}