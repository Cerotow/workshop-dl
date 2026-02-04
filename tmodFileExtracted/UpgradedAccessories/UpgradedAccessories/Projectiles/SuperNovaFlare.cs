

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Projectiles {
    public class SuperNovaFlare : ModProjectile {

        public override void SetDefaults() {
            projectile.width = 48;
            projectile.height = 48;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.timeLeft = 40;
            projectile.penetrate = -1;
            projectile.light = 0.4f;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }
        public override bool CanDamage() {
            return projectile.timeLeft == 0;
        }
        private const float ROTATION_SPEED = (float)Math.PI / 20f;
        private const float PULL_RADIUS = 10000f;
        public override void AI() { // 40 frame lifetime
            projectile.scale = projectile.timeLeft / 40f;
            var ownerPlayer = Main.player[projectile.owner];
            var spinSpeedMult = (float)ownerPlayer.statLifeMax2 / (ownerPlayer.statLife + ownerPlayer.statLifeMax2); // spin faster as the owner's health becomes lower
            projectile.rotation += ROTATION_SPEED * spinSpeedMult;
            foreach(var npc in Main.npc) {
                if(npc.IsHittableHostile() && !npc.IsBoss() && Vector2.DistanceSquared(projectile.Center, npc.Center) < PULL_RADIUS) {
                    npc.velocity += Vector2.Normalize(projectile.Center - npc.Center) * 0.5f;
                    npc.velocity *= 0.96f;
                }
            }
        }

        public override void Kill(int timeLeft) {
            projectile.scale = 1f;
            projectile.position.X += projectile.width / 2f;
            projectile.position.Y += projectile.height / 2f;
            projectile.width = 160;
            projectile.height = 160;
            projectile.position.X -= projectile.width / 2f;
            projectile.position.Y -= projectile.height / 2f;
            projectile.Damage();
            Main.PlaySound(SoundID.Item14, projectile.Center);
            var radius = 80;
            for(int i = 0; i < 30; i++) {
                var dust = Dust.NewDustPerfect(projectile.Center + Util.InCircleRandomVector2(radius), 6, Alpha: 200, Scale: 3.7f);
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust = Dust.NewDustPerfect(projectile.Center + Util.InCircleRandomVector2(radius), 6, Alpha: 100, Scale: 1.5f);
                dust.noGravity = true;
                dust.velocity *= 2f;
                dust.fadeIn = 2.5f;
                if(i < 2) {
                    var gore = Gore.NewGoreDirect(projectile.position, Vector2.Zero, Main.rand.Next(61, 64)); // vanilla does some math to determine the new gore's position
                    gore.position = projectile.Center + Util.InCircleRandomVector2(radius); // but then immediately overwrite it with a new value
                    gore.velocity.X = gore.velocity.X * 0.3f + Main.rand.NextFloat(-0.5f, 0.5f);
                    gore.velocity.Y = gore.velocity.Y * 0.3f + Main.rand.NextFloat(-0.5f, 0.5f);
                }
                if(i < 4) {
                    Dust.NewDustPerfect(projectile.Center + Util.InCircleRandomVector2(radius), 31, Alpha: 100, Scale: 1.5f);
                }
                if(i < 10) {
                    dust = Dust.NewDustPerfect(projectile.Center + Util.InCircleRandomVector2(radius), 6, Scale: 2.7f);
                    dust.noGravity = true;
                    dust.velocity *= 3f;
                    dust = Dust.NewDustPerfect(projectile.Center + Util.InCircleRandomVector2(radius), 31, Scale: 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 3f;
                }
            }
        }
    }
}