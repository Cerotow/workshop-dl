using DDmod.Content.Projectiles.GeneralProj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Accessory
{
    public class 火山戒指 : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        int R = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            NPC npc = NPCdirection.FindClosest(player.Center, 500, false);
            R--;
            if (npc != null && R <= 0)
            {
                R = 360;
                NewProjectile(player.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<火山爆炸>(), 60, 0);
            }
        }

    }
}
