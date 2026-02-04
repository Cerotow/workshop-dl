using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace UpgradedAccessories.Gores {
    public class BeesileGore : ModGore {

        public override bool Update(Gore gore) {
            gore.velocity.Y += 0.6f;
            var collisionVelocity = Collision.TileCollision(gore.position, gore.velocity, 16, 16, true);
            if(gore.velocity != collisionVelocity) {
                collisionVelocity *= 0.8f;
            }
            gore.velocity = collisionVelocity;
            if(gore.velocity.Y == 0) gore.rotation += gore.velocity.X * 0.1f;
            else gore.rotation = gore.velocity.ToRotation() + MathHelper.PiOver2;
            gore.velocity *= 0.98f;
            gore.position += gore.velocity;
            return false;
        }
    }
}