using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UpgradedAccessories.Buffs;

namespace UpgradedAccessories.Projectiles {
    public class CelestialYinyang : ModProjectile {
        public override void SetDefaults() {
            projectile.width = 256;
            projectile.height = 256;
            projectile.timeLeft = 9999;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }

        private const float RADIUS_SQ = 16384f; // 128 * 128
        private const float ROTATION_SPEED = (float)Math.PI / 150f;

        public override bool CanDamage() {
            return false;
        }

        public override Color? GetAlpha(Color lightColor) {
            return Color.White * 0.7f;
        }

        public override void AI() {
            var owner = Main.player[projectile.owner];
            if(Main.netMode == NetmodeID.Server && !owner.active) { // user logged off
                projectile.Kill();
                return;
            }
            if(Main.myPlayer == projectile.owner) {
                if(owner.dead || projectile.identity != owner.GetModPlayer<MyPlayer>().yinyangAura) {
                    projectile.Kill();
                    return;
                }
                projectile.Center = Main.MouseWorld;
                projectile.netUpdate = true;
            }
            projectile.timeLeft = 9999;
            projectile.rotation += ROTATION_SPEED;
            var balance = ModContent.BuffType<Balance>();
            foreach(var player in Main.player) {
                if(player.whoAmI == projectile.owner || !player.active || player.dead || (owner.team != 0 && owner.team != player.team)) continue;
                if(Vector2.DistanceSquared(projectile.Center, player.Center) < RADIUS_SQ && GetBuffDuration(player, balance) < 300 ) {
                    player.AddBuff(balance, 600);
                }
            }
        }

        private static int GetBuffDuration(Player player, int type) {
            var index = player.FindBuffIndex(type);
            if(index < 0) return 0;
            return player.buffTime[index];
        }

        public override void Kill(int timeLeft) {
            Main.player[projectile.owner].GetModPlayer<MyPlayer>().yinyangAura = -1;
        }
    }
}