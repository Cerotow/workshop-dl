using Terraria.ID;

namespace DDmod.Content.Items.Series.Venture.Level_1
{
    public class 生命木剑 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 32;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Green;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(20,65,20);
            AdventureGearGlobalItem.WeaponsQualityAttribute(Item, new Random().Next(2, 4), new Random().Next(1, 3));
            Item.DItem().UpdatesRequired = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void HoldItem(Player player)
        {
            player.lifeRegen += 5;
        }
        public override void UpdateInventory(Player player)
        {
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            return true;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}