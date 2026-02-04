using Terraria;
using Terraria.ModLoader;

namespace UpgradedAccessories.Gores {
    public class GrogMistGore : ModGore {
        public override bool Update(Gore gore) {
            gore.alpha += 3;
            if(gore.alpha > 250) {
                gore.active = false;
            } else {
                gore.velocity *= 0.98f;
                gore.position += gore.velocity;
            }
            return false;
        }
    }
}