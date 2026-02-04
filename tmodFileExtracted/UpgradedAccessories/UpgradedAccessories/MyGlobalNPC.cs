using Terraria;
using Terraria.ModLoader;

namespace UpgradedAccessories {
    public class MyGlobalNPC : GlobalNPC{
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
            if (player.GetModPlayer<MyPlayer>().gaussPack) {
                spawnRate *= 10;
                maxSpawns /= 10;
            }
        }
    }
}
