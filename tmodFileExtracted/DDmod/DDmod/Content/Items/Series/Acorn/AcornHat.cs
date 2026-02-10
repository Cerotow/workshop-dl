namespace DDmod.Content.Items.Series.Acorn
{
    [AutoloadEquip(EquipType.Head)]
    public class AcornHat : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 2;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AcornBreastplate>() && legs.type == ModContent.ItemType<AcornLeggings>();
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 1;
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.Acorn2");
            player.statDefense += 2;
            player.GetDamage(DamageClass.Summon) += 0.05f;
            player.lifeRegen += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<SoulOfNature>(), 12).AddIngredient(ItemID.Acorn, 8).AddTile(TileID.WorkBenches).Register();
        }
    }
}