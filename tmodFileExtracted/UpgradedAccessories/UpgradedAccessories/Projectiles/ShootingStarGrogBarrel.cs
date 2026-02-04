using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UpgradedAccessories.Buffs;
using UpgradedAccessories.Gores;

namespace UpgradedAccessories.Projectiles {
    public class ShootingStarGrogBarrel : ModProjectile {
        public override void SetDefaults() {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = false;
        }

        public override bool CanDamage() {
            return false;
        }

        public override Color? GetAlpha(Color lightColor) {
            return Color.White * 0.7f;
        }

        private const float BASE_ROTATION_SPEED = (float)Math.PI / 120f;
        private const float EXPLODE_DAMAGE = 4000f;
        private const float RADIUS_SQ = 640000f; // 800 * 800

        public override void AI() {
            var owner = Main.player[projectile.owner];
            if(Main.netMode == NetmodeID.Server && !owner.active) { // user logged off
                projectile.Kill();
                return;
            }
            if(Main.myPlayer == projectile.owner) {
                if(owner.dead || projectile.identity != owner.GetModPlayer<MyPlayer>().shootingStarGrogBarrel) {
                    projectile.Kill();
                    return;
                }
            }
            if(projectile.ai[0] >= EXPLODE_DAMAGE) {
                Explode();
            } else {
                projectile.rotation += BASE_ROTATION_SPEED * (1 + 2 * projectile.ai[0] / EXPLODE_DAMAGE);
                projectile.Center = owner.Center + new Vector2(0, -50);
            }
        }

        private void Explode() {
            var owner = Main.player[projectile.owner];
            foreach(var player in Main.player) {
                if(!player.active || player.dead || (owner.team != 0 && owner.team != player.team)) continue;
                if(Vector2.DistanceSquared(projectile.Center, player.Center) < RADIUS_SQ) {
                    player.AddBuff(ModContent.BuffType<Drunk>(), 720);
                }
            }
            owner.GetModPlayer<MyPlayer>().grogBarrelCooldown = 200;
            for(int i = 0; i < 30; i++) {
                var gore = Gore.NewGorePerfect(projectile.position, Util.InCircleRandomVector2(2f), mod.GetGoreSlot("Gores/GrogMistGore"));
                gore.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }
            projectile.Kill();
        }

        public override void Kill(int timeLeft) {
            Main.player[projectile.owner].GetModPlayer<MyPlayer>().shootingStarGrogBarrel = -1;
        }
    }
}