using DDmod.Content.NPCs.Boss.LifeGuardLes;

namespace DDmod.Content.Items.Boss.LifeGuardItems
{
    public class SuspiciousHeartStone : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = Item.CommonMaxStack;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return !AnyNPCs(ModContent.NPCType<LifeGuard>());
        }

        public override bool? UseItem(Player player)
        {

            int T = ModContent.NPCType<LifeGuard>();
            Vector2 vector = player.Center + new Vector2(1000 * player.direction, 1000);
            if (Main.netMode == 0)
            {
                int A = NewNPC(player.GetSource_FromAI(), (int)vector.X / 16 * 16, (int)vector.Y / 16 * 16, T, 0);
            }
            else
            if (Main.netMode == 1 && player.whoAmI == Main.myPlayer)
            {
                int A = NewNPC(player.GetSource_FromAI(), (int)vector.X / 16 * 16, (int)vector.Y / 16 * 16, T, 0);
                if (A < 200)
                {
                    NetMessage.SendData(MessageID.FishOutNPC, -1, -1, null, (int)vector.X / 16, (int)vector.Y / 16, T);
                }
            }
            PlaySound(SoundID.Roar, player.position);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.LifeCrystal).AddTile(TileID.WorkBenches).Register();
        }
    }
}
    