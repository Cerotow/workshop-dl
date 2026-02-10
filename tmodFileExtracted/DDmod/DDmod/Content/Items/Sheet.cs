namespace DDmod.Content.Items
{
    [AutoloadEquip(EquipType.Wings)]
    public class Sheet : ModItem
    {

        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 9f, 2.5f);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }
        public override void UpdateVanity(Player player)
        {
            player.AccPlayer().wingslot = Item.type;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(0, -1f, 0f, true);
            if (player.AccPlayer().wingslot == 0)
            {
                player.AccPlayer().wingslot = Item.type;
            }
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f; // Falling glide speed
            ascentWhenRising = 0.15f; // Rising speed
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.135f;
        }
    }
}
