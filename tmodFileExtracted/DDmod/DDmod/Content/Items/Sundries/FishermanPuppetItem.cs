using System;
using Terraria.ID;

namespace DDmod.Content.Items.Sundries
{
    public class FishermanPuppetItem : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item2;
            Item.autoReuse = false;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }
        public override bool? UseItem(Player player)
        {
            int T = ModContent.NPCType<FishermanPuppet>();
            if (Main.netMode == 0)
            {
                int A = NewNPC(player.GetSource_FromAI(), (int)player.Dplayer().MouseWorld.X / 16 * 16, (int)player.Dplayer().MouseWorld.Y / 16 * 16, T, 0);
                Main.npc[A].defense = 0;
            }
            else
            if (Main.netMode == 1 && player.whoAmI == Main.myPlayer)
            {
                int A = NewNPC(player.GetSource_FromAI(), (int)player.Dplayer().MouseWorld.X / 16 * 16, (int)player.Dplayer().MouseWorld.Y / 16 * 16, T, 0);
                if (A < 200)
                {
                    Main.npc[A].defense = 0;
                    NetMessage.SendData(MessageID.FishOutNPC, -1, -1, null, (int)player.Dplayer().MouseWorld.X / 16, (int)player.Dplayer().MouseWorld.Y / 16, T);
                }
            }
            
            return true;
        }
        public override void AddRecipes()
        {
        }
    }
}
