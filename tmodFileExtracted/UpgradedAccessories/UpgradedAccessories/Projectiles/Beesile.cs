

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UpgradedAccessories.Gores;

namespace UpgradedAccessories.Projectiles {
    public class Beesile : ModProjectile {

        public override void SetStaticDefaults() {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults() {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 1;
            projectile.timeLeft = 1200;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if(projectile.velocity.X != oldVelocity.X) {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if(projectile.velocity.Y != oldVelocity.Y) {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }
            projectile.timeLeft -= 300;
            return false;
        }

        private const float MAX_TARGET_DISTANCE_SQ = 1440000f; // 1200 * 1200
        private const float MAX_ROTATION_SPEED = (float)Math.PI / 15f;
        private const float MAX_SPEED = 20f;

        public override bool CanDamage() {
            return projectile.ai[0] < 0;
        }

        public override void AI() {
            Vector2 target = Vector2.Zero;
            var targetDistanceSQ = MAX_TARGET_DISTANCE_SQ;
            foreach(var npc in Main.npc) {
                if(npc.CanBeChasedBy()) {
                    var distanceSQ = Vector2.DistanceSquared(npc.Center, projectile.Center);
                    if(distanceSQ < targetDistanceSQ) {
                        target = npc.Center;
                        targetDistanceSQ = distanceSQ;
                    }
                }
            }
            if(projectile.ai[0] > 0) {
                projectile.ai[0]--;
                if(target == Vector2.Zero) target = Main.player[projectile.owner].Center;
                // angle manipulation : the rotation is actually PI/2 more rotated, compensate this by also rotating the directional vector by PI/2
                projectile.rotation += MathHelper.Clamp(Util.AngleBetween(projectile.rotation.ToRotationVector2(), (target - projectile.Center).RotatedBy(MathHelper.PiOver2)) / 10f, -MAX_ROTATION_SPEED, MAX_ROTATION_SPEED);
                projectile.velocity *= 0.95f;
            } else if(projectile.ai[0] == 0) {
                projectile.ai[0] = -1;
                // angle manipulation : the rotation is actually PI/2 more rotated, compensate this by subtracting PI/2 from the rotation
                projectile.velocity = (projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * projectile.velocity.Length();
            } else {
                if(target != Vector2.Zero)
                    projectile.velocity = Util.RotateTo(projectile.velocity, target - projectile.Center, MAX_ROTATION_SPEED);
                projectile.velocity = Util.LerpSpeed(projectile.velocity, MAX_SPEED, 0.25f);
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
        }

        public override void Kill(int timeLeft) {
            if(timeLeft <= 0) {
                Gore.NewGorePerfect(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/BeesileGore"));
            } else {
                projectile.position.X += projectile.width / 2f;
                projectile.position.Y += projectile.height / 2f;
                projectile.width = 120;
                projectile.height = 120;
                projectile.position.X -= projectile.width / 2f;
                projectile.position.Y -= projectile.height / 2f;
                projectile.penetrate = -1;
                projectile.Damage();
                Main.PlaySound(SoundID.Item14, projectile.Center);

                var effectSize = 80;
                var effectPosition = projectile.Center - new Vector2(effectSize);

                for(int i = 0; i < 70; i++) {
                    var dust = Dust.NewDustDirect(effectPosition, effectSize, effectSize, 6, Alpha: 100, Scale: 3);
                    dust.noGravity = true;
                    dust.velocity *= 5f;
                    dust = Dust.NewDustDirect(effectPosition, effectSize, effectSize, 6, Alpha: 100, Scale: 2);
                    dust.velocity *= 2f;
                    if(i < 3) {
                        var velocityMul = 0.33f;
                        if(i == 1) velocityMul = 0.66f;
                        if(i == 2) velocityMul = 1f;
                        var gore = Main.gore[Gore.NewGore(projectile.Center, default, Main.rand.Next(61, 64))];
                        gore.velocity *= velocityMul;
                        gore.velocity.X += 1;
                        gore.velocity.Y += 1;
                        gore = Main.gore[Gore.NewGore(projectile.Center, default, Main.rand.Next(61, 64))];
                        gore.velocity *= velocityMul;
                        gore.velocity.X -= 1;
                        gore.velocity.Y += 1;
                        gore = Main.gore[Gore.NewGore(projectile.Center, default, Main.rand.Next(61, 64))];
                        gore.velocity *= velocityMul;
                        gore.velocity.X += 1;
                        gore.velocity.Y -= 1;
                        gore = Main.gore[Gore.NewGore(projectile.Center, default, Main.rand.Next(61, 64))];
                        gore.velocity *= velocityMul;
                        gore.velocity.X -= 1;
                        gore.velocity.Y -= 1;
                    }
                    if(i < 40) {
                        dust = Dust.NewDustDirect(effectPosition, effectSize, effectSize, 31, Alpha: 100, Scale: 2);
                        dust.velocity *= 3f;
                        if(Main.rand.NextBool()) {
                            dust.scale *= 0.5f;
                            dust.fadeIn = 1 + Main.rand.Next(10) * 0.1f;
                        }
                    }
                }
            }
        }
    }
}