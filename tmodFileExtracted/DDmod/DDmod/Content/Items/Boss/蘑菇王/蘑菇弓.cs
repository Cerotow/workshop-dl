using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Bow;

namespace DDmod.Content.Items.Boss.蘑菇王
{
    public class 蘑菇弓 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.crit = 5;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = 10;
            Item.shootSpeed = 10f;
            //ModifyBow.Load(Type, new Modify(), true);
            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            Item.GetGlobalItem<RangedGlobalItem>().StringDown += 2;
            Item.GetGlobalItem<RangedGlobalItem>().StringUP += 2;
            Item.GetGlobalItem<RangedGlobalItem>().Offset += 2;
            Item.GetGlobalItem<RangedGlobalItem>().SetArrows(new int[] { 1 }, ModContent.ProjectileType<蘑菇箭>(),100);
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            if (Main.netMode != 2)
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
}