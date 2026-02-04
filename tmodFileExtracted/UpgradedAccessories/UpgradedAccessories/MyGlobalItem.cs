using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UpgradedAccessories.Buffs;
using UpgradedAccessories.Projectiles;

namespace UpgradedAccessories {
    public class MyGlobalItem : GlobalItem {
        public override bool OnPickup(Item item, Player player) {
            if(player.GetModPlayer<MyPlayer>().nebulaFlower) {
                if(item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane) {
                    player.AddBuff(ModContent.BuffType<LifeSurge>(), 300);
                }else if(item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum) {
                    player.AddBuff(ModContent.BuffType<ManaSurge>(), 300);
                }
            }
            return true;
        }
    }
}
