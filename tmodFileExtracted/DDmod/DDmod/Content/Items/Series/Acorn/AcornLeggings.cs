namespace DDmod.Content.Items.Series.Acorn
{
    [AutoloadEquip(EquipType.Legs)]
    public class AcornLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 1;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<SoulOfNature>(), 10).AddIngredient(ItemID.Acorn, 12).AddTile(TileID.WorkBenches).Register();
        }
    }
}