using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.ID;

namespace DDmod.Content.Items.Series.Venture.Level_1
{
    public class 青弦 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            if (Main.netMode != 2)
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.crit = 0;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot =2;
            Item.shootSpeed = 10f;
            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            Item.GetGlobalItem<RangedGlobalItem>().SetArrows(new int[] { 1 }, ModContent.ProjectileType<SpiritLeafArrow>(),20);
            Item.GetGlobalItem<RangedGlobalItem>().SetString(5,5,Offset:4);
            AdventureGearGlobalItem.WeaponsQualityAttribute(Item, new Random().Next(3, 6), new Random().Next(2, 5));
        }
        public override bool CanShoot(Player player)
        {
            return base.CanShoot(player);
        }
    }
}