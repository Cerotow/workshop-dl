

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Projectiles {
    public class StardustShieldGuardian : ModProjectile {

        public override void SetDefaults() {
            projectile.width = 58;
            projectile.height = 56;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 9999;
            projectile.minion = true;
            projectile.penetrate = -1;
            //projectile.light = 0.4f;
        }

        private const float OWNER_HEALTH_PANIC = 0.4f;
        private const float OWNER_SHIELD_PANIC = 0.8f;
        private const float MAX_TARGET_DISTANCE_SQ = 360000f; // 600 * 600
        private const float MAX_OWNER_DISTANCE_SQ = 1000000f; // 1000 * 1000
        private const float TP_TO_OWNER_DISTANCE_SQ = 4000000f; // 2000 * 2000
        private const float MAX_BUFF_DISTANCE_SQ = 25500f; // 150 * 150
        private const float LOCK_ON_DISTANCE_SQ = 225f; // 15 * 15
        private const float MAX_SPEED = 30f;

        public static int GetDamage(Player player) {
            return (int)Math.Round(30 * (player.minionDamage + player.allDamage) * player.minionDamageMult * player.allDamageMult);
        }

        public static float GetKnockback(Player player) {
            return 7f * player.minionKB;
        }

        public override void AI() {
            Player owner = Main.player[projectile.owner];
            if(Main.netMode == NetmodeID.Server && !owner.active) {
                projectile.Kill();
                return;
            }
            if(Main.myPlayer == projectile.owner) {
                if(owner.dead || owner.GetModPlayer<MyPlayer>().stardustShieldGuardian != projectile.identity) {
                    projectile.Kill();
                    return;
                }
            }
            projectile.damage = GetDamage(owner);
            projectile.knockBack = GetKnockback(owner);
            var behindOwner = BehindPlayer(owner);
            if((float)owner.statLife / owner.statLifeMax2 < OWNER_HEALTH_PANIC && MyPlayer.ShieldRate(owner) < OWNER_SHIELD_PANIC) { // owner is low and don't have full shield, return first
                projectile.ai[0] = -3; // force stay
                projectile.ai[1] = 0;
            } else {
                var ownerDistanceSQ = Vector2.DistanceSquared(owner.Center, projectile.Center);
                if(ownerDistanceSQ > TP_TO_OWNER_DISTANCE_SQ) { // too far away from the owner, tp and retarget
                    projectile.Center = behindOwner;
                    projectile.ai[0] = -1;
                    projectile.ai[1] = 0;
                } else if(ownerDistanceSQ > MAX_OWNER_DISTANCE_SQ) { // too far away from the owner, return first
                    projectile.ai[0] = -2;
                    projectile.ai[1] = 0;
                } else if(projectile.ai[0] < -1) { // close to the owner but not in panic, retarget
                    projectile.ai[0] = -1;
                    projectile.ai[1] = 0;
                }
            }
            if(projectile.ai[0] <= -2) { // ignore enemies and return to the owner
                GoTo(behindOwner);
            } else if(projectile.ai[0] == -1) { // find new target
                StayAlert(owner, behindOwner);
            }
            if(Vector2.DistanceSquared(owner.Center, projectile.Center) < MAX_BUFF_DISTANCE_SQ && projectile.ai[0] < 0) { // close to the player and not in attack mode
                owner.GetModPlayer<MyPlayer>().rechargeShield = true; // shield recharge buff
                Lighting.AddLight(projectile.Center, 1f, 1f, 2f); // bright light
                projectile.direction = owner.direction; // force direction
            }
            if(projectile.ai[0] >= 0) { // have target
                if(projectile.ai[1] < 0) HandlePushEffect(owner); // already doing push effect
                else PursueEnemy(owner); // get close to the target
            }
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 9999;
        }


        private static Vector2 BehindPlayer(Player player) {
            float xOffset;
            if(player.direction > 0) {
                xOffset = -40;
            } else {
                xOffset = player.width + 40;
            }
            return new Vector2(player.position.X + xOffset, player.position.Y - 50);
        }
        private static bool CanTarget(Player player, NPC npc) {
            return npc.IsHittableHostile() && !npc.IsBoss() && Vector2.Distance(player.Center, npc.Center) < MAX_OWNER_DISTANCE_SQ;
        }
        private void GoTo(Vector2 target) {
            var dirVec = target - projectile.Center;
            var dirVecLength = dirVec.Length();
            if(dirVecLength == 0) return;
            projectile.velocity = dirVec * Math.Min(dirVecLength / 4f, MAX_SPEED) / dirVecLength;
        }

        private void StayAlert(Player player, Vector2 behindOwner) {
            var targetNPC = Util.ClosestNPC(player.Center, MAX_OWNER_DISTANCE_SQ, npc => CanTarget(player, npc) && Vector2.DistanceSquared(projectile.Center, npc.Center) < MAX_TARGET_DISTANCE_SQ);
            if(targetNPC != null) projectile.ai[0] = targetNPC.whoAmI;
            if(projectile.ai[0] == -1) {
                GoTo(behindOwner);
            }
        }

        private void PursueEnemy(Player player) {
            var targetNPC = Main.npc[(int)projectile.ai[0]];
            if(!CanTarget(player, targetNPC)) {
                projectile.ai[0] = -1;
                return;
            }
            var offSet = (targetNPC.width / 2f) + 100;
            if(targetNPC.Center.X > player.Center.X) offSet = -offSet;
            var target = targetNPC.Center + new Vector2(offSet, 0);
            GoTo(target);
            projectile.direction = targetNPC.Center.X > player.Center.X ? 1 : -1;
            if(projectile.ai[1] > 0) {
                projectile.ai[1]--;
                if(projectile.ai[1] < 0) projectile.ai[1] = 0;
            }
            if(Vector2.DistanceSquared(target, projectile.Center) < LOCK_ON_DISTANCE_SQ && projectile.ai[1] == 0) {
                projectile.ai[1] = projectile.direction == 1 ? -1f : -2f;
            }
        }

        private void HandlePushEffect(Player player) {
            var targetNPC = Main.npc[(int)projectile.ai[0]];
            if(!CanTarget(player, targetNPC)) {
                projectile.ai[0] = -1;
                projectile.ai[1] = 0;
                return;
            }
            GoTo(targetNPC.Center);
            projectile.direction = projectile.ai[1] == -1 ? 1 : -1;
        }

        public override bool CanDamage() {
            return projectile.ai[1] < 0;
        }

        public override bool? CanCutTiles() {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            projectile.ai[0] = -1;
            projectile.ai[1] = 25;
        }

        public override void Kill(int timeLeft) {
            Main.player[projectile.owner].GetModPlayer<MyPlayer>().stardustShieldGuardian = -1;
        }
    }
}