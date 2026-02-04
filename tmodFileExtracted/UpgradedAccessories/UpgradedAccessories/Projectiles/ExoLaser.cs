using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Projectiles {
    public class ExoLaser : ModProjectile {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.MinecartMechLaser;
        public override void SetDefaults() {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 20;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            var texture = Main.projectileTexture[projectile.type];
            var position = projectile.Center + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            var scale = new Vector2(1f, (float)(Math.Sqrt(projectile.ai[0] * projectile.ai[0] + projectile.ai[1] * projectile.ai[1]) / texture.Height));
            var ownerCenter = Main.player[projectile.owner].Center;
            lightColor = Lighting.GetColor((int)ownerCenter.X / 16, (int)ownerCenter.Y / 16);
            spriteBatch.Draw(texture, position, null, projectile.GetAlpha(lightColor), projectile.rotation, texture.Frame().Bottom(), scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void AI() {
            var owner = Main.player[projectile.owner];
            if(!owner.active || owner.dead) {
                projectile.Kill();
                return;
            }
            projectile.alpha = (int)MathHelper.Lerp(0f, 255f, projectile.timeLeft / 20f);
            projectile.rotation = (float)Math.Atan2(projectile.ai[1], projectile.ai[0]) + MathHelper.PiOver2;
            projectile.Center = owner.Center;
        }
    }

    public class ExoLaserImmunity : GlobalNPC {
        public override bool InstancePerEntity => true;

        public int exoLaserImmunity;

        public override void AI(NPC npc) {
            if(exoLaserImmunity > 0) exoLaserImmunity--;
        }
    }
}