using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;

namespace UpgradedAccessories {
    public static class Util {

        public static bool AnyBossAlive() {
            foreach(var npc in Main.npc) {
                if(npc.active && npc.boss) return true;
            }
            return false;
        }

        public static void SetHermesRocketBoots(Player player, float speedUp = 0.08f, float maxMPH = 34.49f) {
            player.moveSpeed += speedUp;
            player.rocketBoots = 1;
            player.accRunSpeed = maxMPH / 5.11f;
        }

        public static void SetSpectreBoots(Player player, float speedUp = 0.08f, float maxMPH = 34.49f) {
            player.moveSpeed += speedUp;
            player.rocketBoots = 2;
            player.accRunSpeed = maxMPH / 5.11f;
        }

        public static void SetArcticDivingGear(Player player) {
            player.arcticDivingGear = true;
            player.iceSkate = true;
            player.accDivingHelm = true;
            player.accFlipper = true;
            if(player.wet) {
                Lighting.AddLight((int)player.Center.X / 16, (int)player.Center.Y / 16, 0.9f, 0.9f, 0.9f);
            }
        }

        public static void SetLavaWader(Player player, int lavaMax = 420) {
            player.fireWalk = true;
            player.waterWalk = true;
            player.lavaMax += lavaMax;
        }

        public static void SetPaladinShield(Player player) {
            if(player.statLife > player.statLifeMax2 * 0.25f) {
                player.hasPaladinShield = true;
                if(player.whoAmI != Main.myPlayer && player.miscCounter % 10 == 0) {
                    if(player.team == Main.LocalPlayer.team && player.team != 0) {
                        var xDist = player.position.X - Main.LocalPlayer.position.X;
                        var yDist = player.position.Y - Main.LocalPlayer.position.Y;
                        if(xDist * xDist + yDist * yDist < 640000f) { // square of 800
                            Main.LocalPlayer.AddBuff(BuffID.PaladinsShield, 20);
                        }
                    }
                }
            }
        }

        public static void SetCelestialStone(Player player) {
            AllDamageUp(player, 0.1f);
            AllCritUp(player, 2);
            player.statDefense += 4;
            player.meleeSpeed += 0.1f;
            player.pickSpeed += 0.15f;
            player.minionKB += 0.5f;
            player.lifeRegen += 2;
        }

        public static void AllDamageUp(Player player, float amount) {
            player.allDamage += amount;
        }

        public static void AllCritUp(Player player, int amount) {
            player.GetModPlayer<MyPlayer>().allCirt += amount;
        }

        public static Vector2 InCircleRandomVector2(float radius) {
            return Main.rand.NextVector2Unit() * Main.rand.NextFloat(radius);
        }

        public static float AngleBetween(Vector2 from, Vector2 to) {
            double dot = from.X * to.X + from.Y * to.Y;
            double cross = from.X * to.Y - from.Y * to.X;
            return (float)Math.Atan2(cross, dot);
        }

        public static bool IsBoss(this NPC npc) {
            if(npc.boss) return true;
            if(NPCID.Sets.TechnicallyABoss[npc.type]) return true;
            if(npc.realLife < 0) return false;
            return Main.npc[npc.realLife].boss;
        }

        public static bool IsHittableHostile(this NPC npc) {
            return npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage;
        }

        public static Predicate<NPC> CanBeChasedBy = npc => npc.CanBeChasedBy();

        public static NPC ClosestNPC(Vector2 position, float maxDistanceSQ, Predicate<NPC> predicate = null) {
            NPC target = null;
            var targetDistanceSQ = maxDistanceSQ;
            foreach(var npc in Main.npc) {
                if(predicate?.Invoke(npc) ?? true) {
                    var distanceSQ = Vector2.DistanceSquared(npc.Center, position);
                    if(distanceSQ < targetDistanceSQ) {
                        target = npc;
                        targetDistanceSQ = distanceSQ;
                    }
                }
            }
            return target;
        }

        public static Vector2 LerpSpeed(Vector2 velocity, float maxSpeed, float lerpFactor) {
            return velocity * (1 + (maxSpeed - velocity.Length()) * lerpFactor / maxSpeed);
        }

        public static Vector2 RotateTo(Vector2 vector, Vector2 target, float maxRotationSpeed) {
            if(maxRotationSpeed <= 0) return vector;
            var toRotate = MathHelper.Clamp(AngleBetween(vector, target), -maxRotationSpeed, maxRotationSpeed);
            return vector.RotatedBy(toRotate);
        }

        public static bool IsThoriumBardProjectile(Projectile proj) {
            if(UpgradedAccessories.thoriumLoaded && proj.modProjectile != null && proj.modProjectile.mod == UpgradedAccessories.thorium) {
                return proj.modProjectile is ThoriumMod.Projectiles.Bard.BardProjectile;
            } else {
                return false;
            }
        }

        public static bool IsThoriumBardItem(Item item) {
            if(UpgradedAccessories.thoriumLoaded && item.modItem != null && item.modItem.mod == UpgradedAccessories.thorium) {
                return item.modItem is ThoriumMod.Items.BardItem;
            } else {
                return false;
            }
        }
        public static void DrawTextureInUI(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle frame, Color color, float scale) {
            var rect = new Rectangle {
                X = (int)Math.Round(position.X),
                Y = (int)Math.Round(position.Y),
                Width = (int)Math.Round(frame.Width * scale),
                Height = (int)Math.Round(frame.Height * scale)
            };
            spriteBatch.Draw(texture, rect, color);
        }

        public static void DrawTextureInWorld(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color) {
            var rect = new Rectangle {
                X = (int)Math.Round(position.X - Main.screenPosition.X),
                Y = (int)Math.Round(position.Y - Main.screenPosition.Y),
                Width = texture.Width,
                Height = texture.Height
            };
            spriteBatch.Draw(texture, rect, color);
        }
    }
}
