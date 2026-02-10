using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Content.Projectiles.OrnamentProjectile;

namespace DDmod.Content.Items.Boss.天雷怒云
{
    public class 雷霆令牌 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.expert = true;
            //Item.canBePlacedInVanityRegardlessOfConditions = true;
        }

        public short A = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            A++;
            if (A >= 600 && A % 5 == 0&&player.whoAmI==Main.myPlayer)
            {
                NPC npc = NPCdirection.FindClosest(player.Center, 500);
                if (npc != null)
                {

                    NewProjectile(player.GetSource_FromAI(), npc.Center.X + Main.rand.Next(-300, 300), player.Center.Y - 600, 0f, 4f, ModContent.ProjectileType<闪电>(), 80, 3f, player.whoAmI, 0, 1, 300);

                }
                else
                {
                    NewProjectile(player.GetSource_FromAI(), player.Center.X + Main.rand.Next(-300, 300), player.Center.Y - 600, 0f, 4f, ModContent.ProjectileType<闪电>(), 80, 3f, player.whoAmI, 0, 1, 300);

                }
                if (A > 650)
                {
                    A = 0;
                }
            }
        }
    }
}