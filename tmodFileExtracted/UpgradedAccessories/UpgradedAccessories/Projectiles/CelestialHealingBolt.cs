using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.ID;
using System.IO;

namespace UpgradedAccessories.Projectiles {
    public class CelestialHealingBolt : ModProjectile {

        private static readonly Color purple = new Color(62, 59, 101);
        private static readonly Color pink = new Color(255, 209, 213);
        public override void SetDefaults() {
            projectile.width = 8;
            projectile.height = 8;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.timeLeft = 600;
        }

        public override bool CanDamage() {
            return false;
        }

        public override void AI() {
            if(!UpgradedAccessories.thoriumLoaded) {
                projectile.Kill();
                return;
            }
            var owner = Main.player[projectile.owner];
            var target = Main.player[(int)projectile.ai[0]];
            if(!target.active || target.dead || (owner.team != 0 && owner.team != target.team)) {
                projectile.Kill();
                return;
            }
            if(projectile.timeLeft <= 570) {
                var dirVec = target.Center - projectile.Center;
                dirVec.Normalize();
                dirVec *= 0.8f;
                projectile.velocity += dirVec;
            }
            projectile.velocity *= 0.95f;
            for(int i = 0; i < 3; i++) {
                var dust = Dust.NewDustDirect(projectile.position, 8, 8, 261, newColor: purple);
                dust.noGravity = true;
                dust = Dust.NewDustDirect(projectile.position, 8, 8, 261, newColor: pink);
                dust.noGravity = true;
            }
            if(Main.myPlayer == projectile.owner) {
                foreach(var player in Main.player) {
                    if(!player.active || player.dead || player.whoAmI == projectile.owner) continue;
                    if(player.statLife < player.statLifeMax2 || player.statMana < player.statManaMax2 || player.whoAmI == (int)projectile.ai[0] || (owner.team != 0 && owner.team != player.team)) {
                        if(projectile.getRect().Intersects(player.getRect())) {
                            int heal = 4;
                            if(player.GetModPlayer<MyPlayer>().balance) heal += 4;
                            heal += (owner.GetModPlayer<ThoriumPlayer>().healBonus + player.GetModPlayer<ThoriumPlayer>().healReceiveBonus) / 2;
                            HandleCelestialHealing(Main.myPlayer, player.whoAmI, heal, null);
                            projectile.Kill();
                            return;
                        }
                    }
                }
            }
        }

        public static void HandleCelestialHealing(int whoAmI, int target, int amount, BinaryReader reader) {
            if(Main.netMode == NetmodeID.MultiplayerClient) {
                if(Main.myPlayer == whoAmI) {
                    var packet = UpgradedAccessories.GetNetMessagePacket(UpgradedAccessories.NetworkMessageID.CELESTIAL_HEALING);
                    packet.Write(target);
                    packet.Write(amount);
                    packet.Send();
                } else {
                    if(reader == null) {
                        UpgradedAccessories.Instance.Logger.Error("reader null for HandleCelestialHealing");
                        return;
                    }
                    target = reader.ReadInt32();
                    amount = reader.ReadInt32();
                }
            } else if(Main.netMode == NetmodeID.Server) {
                if(reader == null) {
                    UpgradedAccessories.Instance.Logger.Error("reader null for HandleCelestialHealing");
                    return;
                }
                target = reader.ReadInt32();
                amount = reader.ReadInt32();
                var packet = UpgradedAccessories.GetNetMessagePacket(UpgradedAccessories.NetworkMessageID.CELESTIAL_HEALING);
                packet.Write(target);
                packet.Write(amount);
                packet.Send(ignoreClient:whoAmI);
            }
            if(target > -1 && amount > 0) {
                var player = Main.player[target];
                player.statLife += amount;
                if(player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
                CombatText.NewText(player.getRect(), CombatText.HealLife, amount);
                player.statMana += amount;
                if(player.statMana > player.statManaMax2) player.statMana = player.statManaMax2;
                CombatText.NewText(player.getRect(), CombatText.HealMana, amount);
            }
        }
    }
}