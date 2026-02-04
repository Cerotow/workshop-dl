

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Projectiles {
    public class VortexArrow : ModProjectile {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults() {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 9999;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.light = 0.4f;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
        }
        public override bool? CanCutTiles() {
            return false;
        }

        private const float MAX_ROTATION_SPEED = (float)Math.PI / 30f;
        private const float MAX_TARGET_DISTANCE_SQ = 360000f; // 600 * 600
        private const float MAX_OWNER_DISTANCE_SQ = 2250000f; // 1500 * 1500
        private const float WANDER_DISTANCE_SQ = 6400f; // 80 * 80
        private const float FAST_RETURN_DISTANCE_SQ = 640000f; // 800 * 800
        private const float TP_TO_OWNER_DISTANCE_SQ = 4000000f; // 2000 * 2000

        public static int GetDamage(Player player) {
            return (int)Math.Round(50 * (player.rangedDamage + player.allDamage) * player.rangedDamageMult * player.allDamageMult);
        }

        public override void AI() {
            var owner = Main.player[projectile.owner];
            if(Main.netMode == NetmodeID.Server && !owner.active) {
                projectile.Kill();
                return;
            }
            if(Main.myPlayer == projectile.owner) {
                if(owner.dead) {
                    projectile.Kill();
                    return;
                }
                if(projectile.ai[0] == 1) {
                    if(owner.GetModPlayer<MyPlayer>().vortexArrow1 != projectile.identity) {
                        projectile.Kill();
                        return;
                    }
                } else if(projectile.ai[0] == 2) {
                    if(owner.GetModPlayer<MyPlayer>().vortexArrow2 != projectile.identity) {
                        projectile.Kill();
                        return;
                    }
                } else {
                    projectile.Kill();
                    return;
                }
            }
            projectile.damage = GetDamage(owner);
            Vector2 target = Vector2.Zero;
            var targetDistanceSQ = MAX_TARGET_DISTANCE_SQ;
            foreach(var npc in Main.npc) {
                if(npc.CanBeChasedBy()) {
                    if(Vector2.DistanceSquared(npc.Center, owner.Center) > MAX_OWNER_DISTANCE_SQ) continue;
                    var distanceSQ = Vector2.DistanceSquared(projectile.Center, npc.Center);
                    if(distanceSQ < targetDistanceSQ) {
                        target = npc.Center;
                        targetDistanceSQ = distanceSQ;
                    }
                }
            }
            var fastReturn = false;
            if(target == Vector2.Zero) { // npcs can't be at 0,0
                // wander around the player
                var distanceSQ = Vector2.DistanceSquared(owner.Center, projectile.Center);
                if(distanceSQ > TP_TO_OWNER_DISTANCE_SQ) {
                    target = owner.Center;
                    projectile.Center = owner.Center + Util.InCircleRandomVector2(80f);
                } else if(distanceSQ > FAST_RETURN_DISTANCE_SQ) {
                    target = owner.Center;
                    fastReturn = true;
                } else if(distanceSQ > WANDER_DISTANCE_SQ) {
                    target = owner.Center;
                } else {
                    target = owner.Center + Util.InCircleRandomVector2(80f);
                }
            }
            var targetVec = target - projectile.Center;
            if(targetVec.X == 0 && targetVec.Y == 0) targetVec = projectile.velocity;

            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.Clamp(Util.AngleBetween(projectile.velocity, targetVec), -MAX_ROTATION_SPEED, MAX_ROTATION_SPEED));

            if(fastReturn) {
                targetVec.Normalize();
                targetVec *= 10;
                projectile.position += targetVec;
            }
            projectile.timeLeft = 9999;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void Kill(int timeLeft) {
            var owner = Main.player[projectile.owner];
            if(projectile.ai[0] == 1) {
                owner.GetModPlayer<MyPlayer>().vortexArrow1 = -1;
            } else if(projectile.ai[0] == 2) {
                owner.GetModPlayer<MyPlayer>().vortexArrow2 = -1;
            }
        }
    }
}