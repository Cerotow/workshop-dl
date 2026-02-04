using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using UpgradedAccessories.Buffs;
using Terraria.GameInput;
using System.IO;
using UpgradedAccessories.Projectiles;
using System.Collections.Generic;
using UpgradedAccessories.Items.Celestial;
using Terraria.World.Generation;

namespace UpgradedAccessories {
    public class MyPlayer : ModPlayer {
        public bool vortexScope;
        public bool solarFlareGlove;
        public bool nebulaFlower;
        public bool stardustScroll;
        public bool lunarGlove;
        public bool celestialChild;
        public bool shootingStarSoundStudio;
        public bool exoskeleton;
        public bool vengeance;
        public bool spectreLeggings;
        public bool intimidation;
        public bool threatening;
        public bool omniscient;
        public bool trueSoC;
        public bool gaussPack;
        public bool lunarBoots;
        public bool spaceSuit;

        public int reflect;
        public int vortexArrow1 = -1;
        public int vortexArrow2 = -1;
        public bool lifeSurge;
        public bool manaSurge;
        public int nebulaCooldown;
        public bool nebulaBoost;
        public int nebulaLifeCounter;
        public int nebulaManaCounter;
        public bool balance;
        public int celestialHealingBoltCooldown;
        public bool yinyangCursor;
        public int yinyangAura = -1;
        public int shootingStarGrogBarrel = -1;
        public int grogBarrelCooldown;
        public bool drunk;
        public int drunkEnruanceTimer;
        public bool rechargeShield;
        public int shield;
        public float shieldCounter;
        public int stardustShieldGuardian = -1;
        public int handClapCooldown;
        public int lunaticSpeedCounter;
        public bool gaussHover;
        public float exoLifeSteal;
        public bool smallStep;
        public bool giantLeap;
        public bool wasAirborne;
        public bool lastBuffWasTakeoff;

        public int allCirt;

        public static int ShieldMax(Player player) {
            return (player.maxMinions + 1) * 10;
        }
        public static float ShieldRate(Player player) {
            return (float)player.GetModPlayer<MyPlayer>().shield / ShieldMax(player);
        }

        public static bool IsRunningAwayFrom(Player player, Vector2 target) {
            if(player.velocity.LengthSquared() < 1f) return false;
            var angle = Util.AngleBetween(player.velocity, target - player.Center);
            return angle > MathHelper.PiOver2 || angle < -MathHelper.PiOver2;
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers) {
            if(stardustScroll && shield > 0) {
                var layer = new PlayerLayer("UpgradedAccessories", "StardustShield", (info) => {
                    var rotation = info.drawPlayer.fullRotation;
                    var mountOffset = info.drawPlayer.Center - info.drawPlayer.MountedCenter;

                    var xOffset = mountOffset.X + (info.drawPlayer.width / 2 + 10) * info.drawPlayer.direction;
                    var yOffset = mountOffset.Y - (info.drawPlayer.height / 2 + 10);
                    var offsetVec = new Vector2(xOffset, yOffset).RotatedBy(rotation);
                    var shieldWorldX = info.position.X + info.drawPlayer.width / 2f + offsetVec.X;
                    var shieldWorldY = info.position.Y + info.drawPlayer.height / 2f + offsetVec.Y;

                    var opacity = 1 - info.shadow;
                    var brightness = Lighting.Brightness((int)(shieldWorldX / 16f), (int)(shieldWorldY / 16f));

                    var colorAlpha = opacity * info.drawPlayer.stealth;
                    var colorRGB = colorAlpha * brightness;


                    Main.playerDrawData.Add(new DrawData(UpgradedAccessories.stardustShield, new Vector2(shieldWorldX - Main.screenPosition.X, shieldWorldY - Main.screenPosition.Y), null,
                        new Color(colorRGB, colorRGB, colorRGB, colorAlpha), rotation,
                        new Vector2(UpgradedAccessories.stardustShield.Width / 2, UpgradedAccessories.stardustShield.Height / 2), ShieldRate(info.drawPlayer), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0));
                });
                layers.Add(layer);
            }
        }
        public override bool ConsumeAmmo(Item weapon, Item ammo) {
            if(vortexScope && Main.rand.Next(5) == 0) return false; // 20% chance to not consume ammo
            return true;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) {
            if(item.melee) {
                if(solarFlareGlove) target.AddBuff(BuffID.Daybreak, 600);
                if(drunk) drunkEnruanceTimer = 30;
            }
            if(trueSoC && crit) target.AddBuff(BuffID.BetsysCurse, 600);
            else if(omniscient && crit) target.AddBuff(BuffID.Ichor, 600);

            if(nebulaFlower && item.magic) {
                if(nebulaCooldown == 0) {
                    nebulaCooldown = 30;
                    switch(Main.rand.Next(3)) {
                        case 0:
                            Item.NewItem(target.getRect(), ModContent.ItemType<NebulaBooster>());
                            break;
                        case 1:
                            Item.NewItem(target.getRect(), ItemID.Heart);
                            break;
                        case 2:
                            Item.NewItem(target.getRect(), ItemID.Star);
                            break;
                    }
                }
            }
            if(shootingStarSoundStudio && Util.IsThoriumBardItem(item)) {
                if(shootingStarGrogBarrel < 0) {
                    if(grogBarrelCooldown == 0) {
                        shootingStarGrogBarrel = Projectile.NewProjectile(player.Center + new Vector2(0, -50), Vector2.Zero, ModContent.ProjectileType<ShootingStarGrogBarrel>(), 0, 0f, player.whoAmI);
                    }
                } else {
                    var ssgb = Main.projectile[shootingStarGrogBarrel];
                    ssgb.ai[0] += damage;
                    ssgb.timeLeft = 600;
                }
            }
            if(lunarGlove && item.thrown && handClapCooldown == 0 && target.CanBeChasedBy()) {
                var projDamage = (int)Math.Round(100f * (player.thrownDamage + player.allDamage) * player.thrownDamageMult * player.allDamageMult);
                var p = Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ModContent.ProjectileType<HandClap>(), projDamage, 0, player.whoAmI, target.whoAmI);
                p.scale = 0f;
                handClapCooldown = 120;
            }
            if(!player.HasBuff(BuffID.MoonLeech) && target.lifeMax > 5) {
                if(drunk && item.ranged && player.lifeSteal > 0) {
                    var healAmount = (int)(damage * 0.03f);
                    if(healAmount > 0) {
                        player.statLife += healAmount;
                        if(player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
                        player.lifeSteal -= healAmount;
                        player.HealEffect(healAmount);
                    }
                }
                if(spectreLeggings && item.magic && player.lifeSteal > 0f) {
                    var healAmount = (int)(damage * 0.01f);
                    if(healAmount > 0) {
                        player.statLife += healAmount;
                        if(player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
                        player.lifeSteal -= healAmount;
                        player.HealEffect(healAmount);
                    }
                }
                if(exoskeleton && exoLifeSteal > 0f) {
                    var healAmount = (int)(damage * 0.01f);
                    if(healAmount > 0) {
                        player.statLife += healAmount;
                        if(player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
                        exoLifeSteal -= healAmount;
                        player.HealEffect(healAmount);
                    }
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) {
            if(proj.melee) {
                if(solarFlareGlove) target.AddBuff(BuffID.Daybreak, 600);
                if(drunk) drunkEnruanceTimer = 30;
            }
            if(trueSoC && crit) target.AddBuff(BuffID.BetsysCurse, 600);
            else if(omniscient && crit) target.AddBuff(BuffID.Ichor, 600);

            if(nebulaFlower && proj.magic) {
                if(nebulaCooldown == 0) {
                    nebulaCooldown = 30;
                    switch(Main.rand.Next(3)) {
                        case 0:
                            Item.NewItem(target.getRect(), ModContent.ItemType<NebulaBooster>());
                            break;
                        case 1:
                            Item.NewItem(target.getRect(), ItemID.Heart);
                            break;
                        case 2:
                            Item.NewItem(target.getRect(), ItemID.Star);
                            break;
                    }
                }
            }
            if(shootingStarSoundStudio && Util.IsThoriumBardProjectile(proj)) {
                if(shootingStarGrogBarrel < 0) {
                    if(grogBarrelCooldown == 0) {
                        shootingStarGrogBarrel = Projectile.NewProjectile(player.Center + new Vector2(0, -50), Vector2.Zero, ModContent.ProjectileType<ShootingStarGrogBarrel>(), 0, 0f, player.whoAmI);
                    }
                } else {
                    var ssgb = Main.projectile[shootingStarGrogBarrel];
                    ssgb.ai[0] += damage;
                    ssgb.timeLeft = 600;
                }
            }

            if(lunarGlove && (proj.thrown || UpgradedAccessories.IsCalamityRogueProjectile(proj)) && handClapCooldown == 0 && target.CanBeChasedBy()) {
                var projDamage = (int)Math.Round(100f * (player.thrownDamage + player.allDamage) * player.thrownDamageMult * player.allDamageMult);
                var p = Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ModContent.ProjectileType<HandClap>(), projDamage, 0, player.whoAmI, target.whoAmI);
                p.scale = 0f;
                handClapCooldown = 120;
            }

            if(!player.HasBuff(BuffID.MoonLeech) && target.lifeMax > 5) {
                if(drunk && proj.ranged && player.lifeSteal > 0) {
                    var healAmount = (int)Math.Round(damage * 0.03);
                    if(healAmount > 0) {
                        player.statLife += healAmount;
                        if(player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
                        player.lifeSteal -= healAmount;
                        player.HealEffect(healAmount);
                    }
                }
                if(spectreLeggings && proj.magic && player.lifeSteal > 0) {
                    var healAmount = (int)Math.Round(damage * 0.01);
                    if(healAmount > 0) {
                        player.statLife += healAmount;
                        if(player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
                        player.lifeSteal -= healAmount;
                        player.HealEffect(healAmount);
                    }
                }
                if(exoskeleton && exoLifeSteal > 0) {
                    var healAmount = (int)Math.Round(damage * 0.01);
                    if(healAmount > 0) {
                        player.statLife += healAmount;
                        if(player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
                        exoLifeSteal -= healAmount;
                        player.HealEffect(healAmount);
                    }
                }
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot) {
            if(intimidation) {
                var npcScore = npc.damage + npc.defense + npc.lifeMax / 4;
                var playerScore = player.statDefense * 2 + player.statLifeMax2 / 2;
                if(npcScore < playerScore) return false;
            }
            return true;
        }

        public override bool CanBeHitByProjectile(Projectile proj) {
            if(!proj.friendly && proj.hostile && Main.rand.Next(100) < reflect) {
                var dirVec = proj.Center - player.Center;
                dirVec.Normalize();
                proj.velocity -= 2 * Vector2.Dot(proj.velocity, dirVec) * dirVec;
                proj.friendly = true;
                proj.hostile = false;
                return false;
            }
            if(intimidation) {
                var projScore = proj.damage * 2;
                if(Main.expertMode) projScore = (int)(projScore * Main.expertDamage);
                var playerScore = player.statDefense + player.statLifeMax2 / 10;
                if(projScore < playerScore) return false;
            }
            return true;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            double tempDamage = damage;
            bool dirty = false;
            if(giantLeap || smallStep) {
                if(WorldUtils.Find(proj.Center.ToTileCoordinates(), Searches.Chain(new Searches.Down(8), new Conditions.IsSolid()), out var _)) { // aerial bane is 12 tile check
                    if(giantLeap) {
                        tempDamage *= 1.1;
                        dirty = true;
                    }
                } else {
                    if(smallStep) {
                        tempDamage *= 1.1;
                        dirty = true;
                    }
                }
            }
            if(drunk && proj.minion && Main.rand.Next(10) == 0) crit = true;

            if(dirty) damage = (int)Math.Round(tempDamage);
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit) {
            double tempDamage = damage;
            bool dirty = false;
            if(giantLeap || smallStep) {
                if(WorldUtils.Find(target.Center.ToTileCoordinates(), Searches.Chain(new Searches.Down(6), new Conditions.IsSolid()), out var _)) { // less strict for direct melee hits
                    if(giantLeap) {
                        tempDamage *= 1.1;
                        dirty = true;
                    }
                } else {
                    if(smallStep) {
                        tempDamage *= 1.1;
                        dirty = true;
                    }
                }
            }
            if(dirty) damage = (int)Math.Round(tempDamage);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit) {
            double tempDamage = damage;
            bool dirty = false;
            if(solarFlareGlove) {
                tempDamage *= 0.9; // reduces damage from projectiles by 10%
                dirty = true;
            }
            if(trueSoC && !IsRunningAwayFrom(player, proj.Center)) {
                tempDamage *= 0.5;
                dirty = true;
            }
            if(drunkEnruanceTimer > 0) {
                tempDamage *= 0.92;
                dirty = true;
            }
            if(dirty) damage = (int)Math.Round(tempDamage);
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit) {
            double tempDamage = damage;
            bool dirty = false;
            if(trueSoC && !IsRunningAwayFrom(player, npc.Center)) {
                tempDamage *= 0.5;
                dirty = true;
            }
            if(drunkEnruanceTimer > 0) {
                tempDamage *= 0.92;
                dirty = true;
            }
            if(dirty) damage = (int)Math.Round(tempDamage);
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit) {
            if(player.whoAmI == Main.myPlayer) {
                if(solarFlareGlove) {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<SuperNovaFlare>(), (int)Math.Round(damage * 2d), 1f, player.whoAmI);
                }
                if(vengeance) {
                    int bees = 2; // 1 in vanilla
                    if(Main.rand.NextBool()) bees++; // only 1/3 chance in vanilla
                    if(Main.rand.NextBool()) bees++;
                    if(Main.rand.NextBool()) bees++;
                    for(int i = 0; i < bees; i++) {
                        Projectile.NewProjectile(player.Center, Main.rand.NextVector2Unit() * 12f, ModContent.ProjectileType<Beesile>(), 80, 1f, player.whoAmI, 60f);
                    }
                    for(int i = 0; i < 3; i++) { // 3 in vanilla
                        float x = player.position.X + Main.rand.Next(-400, 400); // left and right of the player
                        float y = player.position.Y - Main.rand.Next(500, 800); // way above the player
                        float speedX = player.position.X + (player.width / 2) - x + Main.rand.Next(-100, 101); // varying x speed
                        float speedY = player.position.Y + (player.height / 2) - y;
                        float speedMul = 23 / (float)Math.Sqrt(speedX * speedX + speedY * speedY); // normalize then multiply by 23
                        Projectile.NewProjectile(x, y, speedX * speedMul, speedY * speedMul, ModContent.ProjectileType<RevengeStar>(), Damage: 80, KnockBack: 6f, player.whoAmI, i, player.position.Y);
                    }
                }
            }
        }
        public override void PreUpdate() {
            if(lunaticSpeedCounter > 0) lunaticSpeedCounter--;
            if(nebulaCooldown > 0) nebulaCooldown--;
            if(handClapCooldown > 0) handClapCooldown--;
            if(celestialHealingBoltCooldown > 0) celestialHealingBoltCooldown--;
            if(grogBarrelCooldown > 0) grogBarrelCooldown--;
            if(drunkEnruanceTimer > 0) drunkEnruanceTimer--;
            if(Main.expertMode) {
                if(exoLifeSteal > 90f) {
                    exoLifeSteal += 0.6f;
                } else {
                    exoLifeSteal = 90f;
                }
            } else {
                if(exoLifeSteal > 110f) {
                    exoLifeSteal += 0.8f;
                } else {
                    exoLifeSteal = 110f;
                }
            }
            StardustShield();
        }

        public override void PostUpdateRunSpeeds() {
            if(player.velocity.Y == 0 && player.dashDelay >= 0 && !player.mount.Active) {
                if(smallStep) {
                    player.maxRunSpeed *= 1.1f;
                    player.runAcceleration *= 1.2f;
                }
                if(lunarBoots) {
                    var minSpeed = (player.accRunSpeed + player.maxRunSpeed) / 2;
                    if((player.controlLeft && player.velocity.X > -player.accRunSpeed && player.velocity.X < -minSpeed) ||
                        (player.controlRight && player.velocity.X < player.accRunSpeed && player.velocity.X > minSpeed)) {
                        if(player.runSoundDelay == 0) {
                            Main.PlaySound(SoundID.Run, player.position);
                            player.runSoundDelay = 9;
                        }
                        for(int i = 0; i < 4; i++) {
                            var dust = Main.dust[Dust.NewDust(new Vector2(player.position.X - 4, player.position.Y), player.width + 8, player.height,
                                DustID.BubbleBlock, player.velocity.X * -0.5f, player.velocity.Y * 0.5f, 0, Main.DiscoColor, 1)];
                            dust.noGravity = true;
                        }
                    }
                    player.maxRunSpeed = player.accRunSpeed;
                }
            }
            if(trueSoC) {
                if(player.dash == 0) { // vanilla behavior : bottommost dash accessory gets the final effect
                    if(player.dashDelay > 0) {
                        player.dashDelay--;
                    } else if(player.dashDelay < 0) {
                        for(int i = 0; i < 4; i++) {
                            var dust = Dust.NewDustDirect(new Vector2(player.position.X, player.position.Y + 4f), player.width, player.height - 8, 6, Alpha: 100, newColor: Color.IndianRed, Scale: 1.7f);
                            dust.velocity *= 0.1f;
                            dust.scale *= 1f + Main.rand.Next(20) * 0.01f;
                            dust.noGravity = true;
                            if(Main.rand.Next(2) == 0) {
                                dust.fadeIn = 0.5f;
                            }
                        }
                        player.vortexStealthActive = false;
                        var lowFrictionMinSpeed = 14;
                        var runSpeed = Math.Max(player.accRunSpeed, player.maxRunSpeed);
                        if(player.velocity.X > lowFrictionMinSpeed || player.velocity.X < -lowFrictionMinSpeed) { // low friction on very high speed
                            player.velocity.X *= 0.985f; // friction
                        } else if(player.velocity.X > runSpeed || player.velocity.X < -runSpeed) { // higher friction on lower speed
                            player.velocity.X *= 0.94f; // more friction
                        } else { // slowed down to run speed, dash end
                            player.dashDelay = 20;
                            if(player.velocity.X > 0) {
                                player.velocity.X = runSpeed;
                            } else if(player.velocity.X < 0) {
                                player.velocity.X = -runSpeed;
                            }
                        }
                    } else {
                        if(player.dashTime > 0) player.dashTime--; // positive dash time : right dash
                        else if(player.dashTime < 0) player.dashTime++; // negative dash time : left dash
                        var direction = 0;
                        if(player.controlRight && player.releaseRight) {
                            if(player.dashTime > 0) {
                                direction = 1;
                                player.dashTime = 0;
                            } else {
                                player.dashTime = 15;
                            }
                        } else if(player.controlLeft && player.releaseLeft) {
                            if(player.dashTime < 0) {
                                direction = -1;
                                player.dashTime = 0;
                            } else {
                                player.dashTime = -15;
                            }
                        }
                        if(direction != 0) {
                            player.velocity.X = 21.9f * direction;
                            var tile1 = (player.Center + new Vector2(direction * player.width / 2 + 2, player.gravDir * player.height / -2 + player.gravDir * 2)).ToTileCoordinates();
                            var tile2 = (player.Center + new Vector2(direction * player.width / 2 + 2, 0)).ToTileCoordinates();
                            if(WorldGen.SolidOrSlopedTile(tile1.X, tile1.Y) || WorldGen.SolidOrSlopedTile(tile2.X, tile2.Y)) {
                                player.velocity.X /= 2;
                            }
                            player.dashDelay = -1;
                            for(int i = 0; i < 20; i++) {
                                var dust = Dust.NewDustDirect(player.position, player.width, player.height, 6, Alpha: 100, newColor: Color.IndianRed, Scale: 2f);
                                dust.position.X += Main.rand.Next(-5, 6);
                                dust.position.Y += Main.rand.Next(-5, 6);
                                dust.velocity *= 0.2f;
                                dust.scale *= 1f + Main.rand.Next(20) * 0.01f;
                                dust.noGravity = true;
                                dust.fadeIn = 0.5f;
                            }
                        }
                    }
                }
            }
        }

        public override void PreUpdateMovement() {
            if(lunarBoots && player.controlDown && player.controlJump && player.wingTime > 0) {
                player.velocity.Y = 1E-05f;
                player.wingTime += 0.5f;
            }
            if(!gaussPack || player.mount.Active || player.gravDir == -1 || Util.AnyBossAlive()) gaussHover = false;
            if(gaussHover) {
                player.rocketBoots = 0;
                player.wings = 0;
                player.wingsLogic = 0;
                player.wingTime = 0;
                player.fallStart = (int)(player.position.Y / 16);
                var xDir = 0;
                var yDir = 0;
                if(player.controlRight) {
                    player.velocity.X = 15;
                    xDir = -1;
                } else if(player.controlLeft) {
                    player.velocity.X = -15;
                    xDir = 1;
                } else {
                    player.velocity.X = 0;
                }
                if(player.controlUp || player.controlJump) {
                    player.velocity.Y = -15;
                    yDir = 1;
                } else if(player.controlDown) {
                    if(player.velocity.Y != player.gravity) player.velocity.Y = 15; // if on ground, don't change the velocity
                    yDir = -1;
                } else {
                    if(player.velocity.Y != player.gravity) player.velocity.Y = 1E-05f; // if on ground, dont' change the velocity
                }
                if(xDir != 0 || yDir != 0) {
                    if(player.rocketDelay2 <= 0) {
                        Main.PlaySound(SoundID.Item24, player.position);
                        player.rocketDelay2 = 15;
                    }
                    for(int i = 0; i < 4; i++) {
                        Dust.NewDust(new Vector2(player.Center.X + player.width / 2 * xDir - 10, player.Center.Y + player.height / 2 * yDir - 10),
#pragma warning disable ChangeMagicNumberToID // Change magic numbers into appropriate ID values
                        20, 20, 16, 5 * xDir, 5 * yDir);
#pragma warning restore ChangeMagicNumberToID // Change magic numbers into appropriate ID values
                    }
                }
            }
        }

        public override void PostUpdateBuffs() {
            int nebulaSynergy = 0;
            if(lifeSurge) nebulaSynergy++;
            if(manaSurge) nebulaSynergy++;
            if(nebulaBoost) nebulaSynergy++;
            if(lifeSurge) {
                if(nebulaSynergy == 1) {
                    nebulaLifeCounter += 1;
                } else if(nebulaSynergy == 2) {
                    nebulaLifeCounter += 2;
                } else {
                    nebulaLifeCounter += 4;
                }
                if(nebulaLifeCounter > 30) {
                    nebulaLifeCounter -= 30;
                    if(player.statLife < player.statLifeMax2) player.statLife++;
                }
            }
            if(manaSurge) {
                if(nebulaSynergy == 1) {
                    nebulaManaCounter += 1;
                } else if(nebulaSynergy == 2) {
                    nebulaManaCounter += 2;
                } else {
                    nebulaManaCounter += 4;
                }
                if(nebulaManaCounter > 15) {
                    nebulaManaCounter -= 15;
                    if(player.statMana < player.statManaMax2) player.statMana++;
                }
            }
            if(nebulaBoost) {
                if(nebulaSynergy == 1) {
                    player.magicDamage += 0.08f;
                } else if(nebulaSynergy == 2) {
                    player.magicDamage += 0.16f;
                } else {
                    player.magicDamage += 0.32f;
                }
            }
        }


        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff) {
            if(gaussPack) {
                wallSpeedBuff = true;
                tileSpeedBuff = true;
                tileRangeBuff = true;
            }
        }
        public override void PostUpdateMiscEffects() {
            if(Main.myPlayer == player.whoAmI) {
                VortexArrow();
                StardustShieldGuardian();
                CelestialChild();
            }
            Exoskeleton();
            SpaceSuit();
            if(gaussHover) player.allDamageMult *= 0.05f;
        }

        private void VortexArrow() {
            if(vortexScope) {
                if(vortexArrow1 < 0) {
                    vortexArrow1 = Projectile.NewProjectile(player.Center, Vector2.UnitX.RotatedByRandom(Math.PI) * 10, ModContent.ProjectileType<VortexArrow>(), Projectiles.VortexArrow.GetDamage(player), 0f, player.whoAmI, 1f);
                }
                if(vortexArrow2 < 0) {
                    vortexArrow2 = Projectile.NewProjectile(player.Center, Vector2.UnitX.RotatedByRandom(Math.PI) * 10, ModContent.ProjectileType<VortexArrow>(), Projectiles.VortexArrow.GetDamage(player), 0f, player.whoAmI, 2f);
                }
            } else {
                vortexArrow1 = -1;
                vortexArrow2 = -1;
            }
        }

        private void StardustShield() {
            if(stardustScroll && !player.dead) {
                var shieldMax = ShieldMax(player);
                if(shield < shieldMax) {
                    float rechargeRate = player.minionDamage * player.minionDamageMult;
                    if(rechargeShield) {
                        rechargeRate *= 3;
                    }
                    shieldCounter += rechargeRate;
                    var shieldToAdd = (int)(shieldCounter / 20f);
                    if(shieldToAdd > 0 && shield < shieldMax) {
                        var canFill = shieldMax - shield;
                        if(shieldToAdd < canFill) {
                            shield += shieldToAdd;
                        } else {
                            shield = shieldMax;
                            if(Main.myPlayer == player.whoAmI) Main.PlaySound(SoundID.MaxMana, player.Center);
                        }
                        shieldCounter %= 20;
                    }
                }
            } else {
                shield = 0;
                shieldCounter = 0;
            }
        }

        private void CelestialChild() {
            if(celestialChild && celestialHealingBoltCooldown == 0) {
                foreach(var other in Main.player) {
                    if(other.whoAmI == player.whoAmI || !other.active || other.dead || (player.team != 0 && player.team != other.team)) continue;
                    var dirVec = other.Center - player.Center;
                    dirVec.Normalize();
                    dirVec *= 8f;
                    Projectile.NewProjectile(player.Center, dirVec, ModContent.ProjectileType<CelestialHealingBolt>(), 0, 0, player.whoAmI, other.whoAmI);
                    celestialHealingBoltCooldown = 240;
                }
            }
            if(yinyangCursor) {
                if(yinyangAura < 0) {
                    yinyangAura = Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<CelestialYinyang>(), 0, 0, player.whoAmI);
                }
            } else {
                yinyangAura = -1;
            }
        }

        private void StardustShieldGuardian() {
            if(stardustScroll) {
                if(stardustShieldGuardian < 0) {
                    stardustShieldGuardian = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<StardustShieldGuardian>(), Projectiles.StardustShieldGuardian.GetDamage(player), Projectiles.StardustShieldGuardian.GetKnockback(player), player.whoAmI, -2);
                }
            } else {
                stardustShieldGuardian = -1;
            }
        }

        private void Exoskeleton() {
            if(exoskeleton) {
                var damageBoost = player.velocity.Length() / 200f;
                if(damageBoost > 0.1f) damageBoost = 0.1f; // max 10% boost at 20 speed
                player.allDamage += damageBoost;
                if(Main.myPlayer == player.whoAmI) {
                    var laserDamage = (int)Math.Round(60 * (1f + damageBoost * 20f)); // max 200% boost at 20 speed
                    var laserRange = 300f * (1f + damageBoost * 10f); // max 100% boost at 20 speed
                    foreach(var npc in Main.npc) {
                        if(!npc.CanBeChasedBy() || npc.dontTakeDamage || npc.GetGlobalNPC<ExoLaserImmunity>().exoLaserImmunity > 0 || Vector2.Distance(npc.Center, player.Center) > laserRange
                            || !Collision.CanHitLine(npc.position, npc.width, npc.height, player.Center, 0, 0)) continue;
                        npc.GetGlobalNPC<ExoLaserImmunity>().exoLaserImmunity = 10;
                        var dirVec = npc.position + npc.Size * Main.rand.NextVector2Square(0f, 1f) - player.Center;
                        var exoLaser = Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<ExoLaser>(), laserDamage, 0f, player.whoAmI, dirVec.X, dirVec.Y);
                        exoLaser.Damage();
                        exoLaser.damage = 0;
                        exoLaser.Center = player.Center;
                    }
                }
            }
        }

        private void SpaceSuit() {
            if(spaceSuit) {
                bool isAirborne = player.velocity.Y != 0f;
                if(isAirborne != wasAirborne) {
                    if(isAirborne) { // player just took off
                        if(!lastBuffWasTakeoff && !smallStep) { // last buff was landing and the player doesn't have small step buff
                            player.AddBuff(ModContent.BuffType<GiantLeap>(), 600);
                            lastBuffWasTakeoff = true;
                        }
                    } else {
                        if(lastBuffWasTakeoff && !giantLeap) { // last buff was take off and the player doesn't have giant leap buff
                            player.AddBuff(ModContent.BuffType<SmallStep>(), 600);
                            lastBuffWasTakeoff = false;
                        }
                    }
                    wasAirborne = isAirborne;
                }
            }
        }

        public override void ResetEffects() {
            vortexScope = false;
            solarFlareGlove = false;
            nebulaFlower = false;
            stardustScroll = false;
            lunarGlove = false;
            exoskeleton = false;
            vengeance = false;
            spectreLeggings = false;
            intimidation = false;
            threatening = false;
            omniscient = false;
            trueSoC = false;
            gaussPack = false;
            lunarBoots = false;
            reflect = 0;
            lifeSurge = false;
            manaSurge = false;
            nebulaBoost = false;
            rechargeShield = false;
            balance = false;
            celestialChild = false;
            yinyangCursor = false;
            shootingStarSoundStudio = false;
            drunk = false;
            smallStep = false;
            giantLeap = false;
            spaceSuit = false;

            allCirt = 0;
        }

        public override void GetWeaponCrit(Item item, ref int crit) {
            crit += allCirt;
        }

        public static void HandleGaussHover(int whoAmI, BinaryReader reader) {
            Player player;
            if(Main.netMode == NetmodeID.SinglePlayer) {
                player = Main.LocalPlayer;
            } else if(Main.netMode == NetmodeID.Server) {
                if(reader == null) {
                    UpgradedAccessories.Instance.Logger.Error("reader null for HandleGaussHover");
                    return;
                }
                player = Main.player[whoAmI];
                var packet = UpgradedAccessories.GetNetMessagePacket(UpgradedAccessories.NetworkMessageID.GAUSS_HOVER);
                packet.Write(whoAmI);
                packet.Send(ignoreClient: whoAmI);
            } else {
                if(Main.myPlayer == whoAmI) {
                    player = Main.LocalPlayer;
                    UpgradedAccessories.GetNetMessagePacket(UpgradedAccessories.NetworkMessageID.GAUSS_HOVER).Send();
                } else {
                    if(reader == null) {
                        UpgradedAccessories.Instance.Logger.Error("reader null for HandleGaussHover");
                        return;
                    }
                    player = Main.player[reader.ReadInt32()];
                }
            }
            var mp = player.GetModPlayer<MyPlayer>();
            mp.gaussHover = !mp.gaussHover;
        }

        public override void ProcessTriggers(TriggersSet triggersSet) {
            if(gaussPack && !player.mount.Active && player.gravDir != -1 && UpgradedAccessories.gaussHover.JustPressed && !Util.AnyBossAlive()) {
                HandleGaussHover(player.whoAmI, null);
            }
        }
    }
}
