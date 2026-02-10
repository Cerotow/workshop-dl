using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.ID;

namespace DDmod.Content.Items.Series.ShadowFlame
{
    public class ShadowFury : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            if (Main.netMode != 2)
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.crit = 0;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot =2;
            Item.shootSpeed = 10f;

            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            ModifyBow.Load(Type, new Modify暗影之怒(), true);
            Item.GetGlobalItem<RangedGlobalItem>().SetArrows(new int[] { 1 }, ModContent.ProjectileType<ShadowWoodArrow>(), 100);
            Item.GetGlobalItem<RangedGlobalItem>().SetString(7,7,Offset:8,StringColor: new Color(196, 136, 251, 0));
        }
        public override bool CanShoot(Player player)
        {
            return base.CanShoot(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.MoltenFury).AddIngredient(ModContent.ItemType<ShadowFlame>()).AddIngredient(154, 24).AddTile(TileID.Anvils).Register();
        }
    }
    public class Modify暗影之怒 : ModifyBow
    {
        public override bool SetArrows(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            if (!ranged.Charge)
            {
                return false;
            }
            return base.SetArrows(item, Projectile, rangedItem, ranged);
        }
    }
}