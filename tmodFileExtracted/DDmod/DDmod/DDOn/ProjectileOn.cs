using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items;
using DDmod.Content.Projectiles;
using DDmod.DrawPlayer;
using DDmod.NoContent.Config;
using System.Reflection;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.UI;
using Terraria.Utilities;
using static Terraria.Player;
using Terraria.ID;
using DDmod.Players;
using Terraria.Graphics.Effects;
using DDmod.SubworldLibraryWorld;
using SubworldLibrary;
using DDmod.Content.NPCs.TownNPC;
using DDmod.SubworldLibraryWorld.草原;
using System.Runtime.InteropServices;
using System.ComponentModel;
using MonoMod.RuntimeDetour.HookGen;
using static DDmod.DDOn.DDmodOn;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Items.Ammo;

namespace DDmod.DDOn
{
    internal static class ProjectileOn
    {
        public static void Load()
        {
            Terraria.On_Projectile.Update += Projectile_Update;
            Terraria.On_Projectile.Damage += Damage;
            Terraria.On_Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float += NewProj;
            Terraria.On_Projectile.Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks += Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks;
        }
        public static void Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks(Terraria.On_Projectile.orig_Kill_DirtAndFluidProjectiles_RunDelegateMethodPushUpForHalfBricks orig, Projectile projectile, Point pt, float size, Utils.TileActionAttempt plot)
        {
            if (!SWSystem.ForbidVandalism)
            {
                orig(projectile,pt,size,plot);
            }
        }
        public static int NewProj(Terraria.On_Projectile.orig_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float orig, IEntitySource spawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = -1, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f)
        {
            if(Main.myPlayer<255 && Main.LocalPlayer.Dplayer().Ammo==ModContent.ItemType<珍珠木箭>())
            {
                //Main.LocalPlayer.Dplayer().Ammo = 0;
                ai2 = 1;
            }
            return orig(spawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
        }
        public static void Damage(Terraria.On_Projectile.orig_Damage orig,Projectile projectile)
        {
            orig(projectile);
            
        }
        public static void Projectile_Update(Terraria.On_Projectile.orig_Update orig, Projectile projectile, int I)
        {
            if (projectile.type <= 0)
            {
                projectile.active = false;
            }
            if (Start > 0)
            {
                //projectile.active = false;
            }
            else
            {
                if (!projectile.active)
                {
                    return;
                }
                Vector2 vector = projectile.position;
                projectile.DProj().PreviousPosition = projectile.position;
                orig(projectile,I);
                projectile.DProj().PrePosition = (projectile.position - vector);
                //Main.NewText(Projectile.MaxUpdates);
                /*
                if (!projectile.active)
                    return;

                if (Main.netMode == 1 && (ProjectileID.Sets.IsAGolfBall[projectile.type] || projectile.type == 820))
                {
                    int num = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                    int num2 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
                    if (!Main.sectionManager.TileLoaded(num, num2))
                        return;
                }

                projectile.numUpdates = projectile.extraUpdates;
                while (projectile.numUpdates >= 0)
                {
                    projectile.numUpdates--;
                    if (projectile.type == 640 && projectile.ai[1] > 0f)
                    {
                        projectile.ai[1] -= 1f;
                        continue;
                    }

                    if (projectile.position.X <= Main.leftWorld || projectile.position.X + (float)projectile.width >= Main.rightWorld || projectile.position.Y <= Main.topWorld || projectile.position.Y + (float)projectile.height >= Main.bottomWorld)
                    {
                        projectile.active = false;
                        return;
                    }

                    if (!projectile.noEnchantmentVisuals)
                        DDHelper.MethodReflection(projectile.GetType(), "UpdateEnchantmentVisuals", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(projectile, null);

                    if (projectile.numUpdates == -1 && (projectile.minion || projectile.sentry))
                    {
                        Player player = Main.player[projectile.owner];
                        projectile.damage = (int)player.GetTotalDamage(projectile.DamageType).ApplyTo(projectile.originalDamage);
                        projectile.CritChance = (int)(projectile.OriginalCritChance + player.GetTotalCritChance(projectile.DamageType) + 5E-06f);
                        projectile.ArmorPenetration = (int)(projectile.OriginalArmorPenetration + player.GetTotalArmorPenetration(projectile.DamageType) + 5E-06f);
                    }

                    if (projectile.minion && projectile.numUpdates == -1 && projectile.type != 625 && projectile.type != 628)
                    {
                        projectile.minionPos = Main.player[projectile.owner].numMinions;
                        if (Main.player[projectile.owner].slotsMinions + projectile.minionSlots > (float)Main.player[projectile.owner].maxMinions && projectile.owner == Main.myPlayer)
                        {
                            if ((projectile.type == 627 || projectile.type == 626) && projectile.owner == Main.myPlayer)
                            {
                                int byUUID = GetByUUID(projectile.owner, projectile.ai[0]);
                                if (byUUID != -1)
                                {
                                    Projectile projectile2 = Main.projectile[byUUID];
                                    if (projectile2.type != 625)
                                        projectile2.localAI[1] = projectile.localAI[1];

                                    projectile2 = Main.projectile[(int)projectile.localAI[1]];
                                    projectile2.ai[0] = projectile.ai[0];
                                    projectile2.ai[1] = 1f;
                                    projectile2.netUpdate = true;
                                }
                            }

                            projectile.Kill();
                        }
                        else
                        {
                            Main.player[projectile.owner].numMinions++;
                            Main.player[projectile.owner].slotsMinions += projectile.minionSlots;
                        }
                    }

                    float num3 = 1f + Math.Abs(projectile.velocity.X) / 3f;
                    if (projectile.gfxOffY > 0f)
                    {
                        projectile.gfxOffY -= num3 * projectile.stepSpeed;
                        if (projectile.gfxOffY < 0f)
                            projectile.gfxOffY = 0f;
                    }
                    else if (projectile.gfxOffY < 0f)
                    {
                        projectile.gfxOffY += num3 * projectile.stepSpeed;
                        if (projectile.gfxOffY > 0f)
                            projectile.gfxOffY = 0f;
                    }

                    if (projectile.gfxOffY > 16f)
                        projectile.gfxOffY = 16f;

                    if (projectile.gfxOffY < -16f)
                        projectile.gfxOffY = -16f;

                    Vector2 velocity = projectile.velocity;
                    projectile.oldVelocity = projectile.velocity;
                    projectile.whoAmI = I;
                    if (projectile.soundDelay > 0)
                        projectile.soundDelay--;

                    projectile.netUpdate = false;
                    for (int j = 0; j < 255; j++)
                    {
                        if (projectile.playerImmune[j] > 0)
                            projectile.playerImmune[j]--;
                    }

                    if (projectile.usesLocalNPCImmunity)
                    {
                        for (int k = 0; k < 200; k++)
                        {
                            if (projectile.localNPCImmunity[k] > 0)
                                projectile.localNPCImmunity[k]--;
                        }
                    }
                    //奇怪的bug
                    //projectile.oldPosition = projectile.position;
                    //projectile.oldDirection = projectile.direction;
                    projectile.AI();
                    if (projectile.ShouldUseWindPhysics() && (double)projectile.Center.Y < Main.worldSurface * 16.0 && Main.tile[(int)projectile.Center.X / 16, (int)projectile.Center.Y / 16] != null && Main.tile[(int)projectile.Center.X / 16, (int)projectile.Center.Y / 16].WallType == 0 && ((projectile.velocity.X > 0f && Main.windSpeedCurrent < 0f) || (projectile.velocity.X < 0f && Main.windSpeedCurrent > 0f) || Math.Abs(projectile.velocity.X) < Math.Abs(Main.windSpeedCurrent * Main.windPhysicsStrength) * 180f) && Math.Abs(projectile.velocity.X) < 16f)
                    {
                        projectile.velocity.X += Main.windSpeedCurrent * Main.windPhysicsStrength;
                        MathHelper.Clamp(projectile.velocity.X, -16f, 16f);
                    }

                    if (projectile.owner < 255 && !Main.player[projectile.owner].active)
                        projectile.Kill();

                    if (projectile.type == 242 || projectile.type == 302 || projectile.type == 638)
                        projectile.wet = false;

                    if (!projectile.ignoreWater)
                    {
                        bool flag;
                        bool flag2;
                        try
                        {
                            flag = Collision.LavaCollision(projectile.position, projectile.width, projectile.height);
                            flag2 = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
                            if (flag)
                                projectile.lavaWet = true;

                            if (Collision.honey)
                                projectile.honeyWet = true;

                            if (Collision.shimmer)
                                projectile.shimmerWet = true;
                        }
                        catch
                        {
                            projectile.active = false;
                            return;
                        }

                        if (projectile.wet && !projectile.lavaWet)
                        {
                            if (projectile.type == 85 || projectile.type == 15 || projectile.type == 188)
                                projectile.Kill();

                            if (projectile.type == 2)
                            {
                                projectile.type = 1;
                                projectile.light = 0f;
                            }
                        }

                        if (projectile.type == 34)
                        {
                            if (projectile.wet && !projectile.lavaWet)
                                projectile.Kill();

                            if (projectile.lavaWet)
                                flag2 = (projectile.wet = (projectile.lavaWet = false));
                        }

                        if (projectile.type == 80)
                        {
                            flag2 = false;
                            projectile.wet = false;
                            if (flag && projectile.ai[0] >= 0f)
                                projectile.Kill();
                        }

                        if (flag2)
                        {
                            if (projectile.type != 155 && projectile.wetCount == 0 && !projectile.wet)
                            {
                                if (!flag)
                                {
                                    if (projectile.shimmerWet)
                                    {
                                        for (int l = 0; l < 10; l++)
                                        {
                                            int num4 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y + (float)(projectile.height / 2) - 8f), projectile.width + 12, 24, 308);
                                            Main.dust[num4].velocity.Y -= 4f;
                                            Main.dust[num4].velocity.X *= 2.5f;
                                            Main.dust[num4].scale = 1.3f;
                                            Main.dust[num4].noGravity = true;
                                            switch (Main.rand.Next(6))
                                            {
                                                case 0:
                                                    Main.dust[num4].color = new Color(255, 255, 210);
                                                    break;
                                                case 1:
                                                    Main.dust[num4].color = new Color(190, 245, 255);
                                                    break;
                                                case 2:
                                                    Main.dust[num4].color = new Color(255, 150, 255);
                                                    break;
                                                default:
                                                    Main.dust[num4].color = new Color(190, 175, 255);
                                                    break;
                                            }

                                            SoundEngine.PlaySound(SoundID.SplashWeak, projectile.position);
                                        }
                                    }
                                    else if (projectile.honeyWet)
                                    {
                                        for (int l = 0; l < 10; l++)
                                        {
                                            int num4 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y + (float)(projectile.height / 2) - 8f), projectile.width + 12, 24, 152);
                                            Main.dust[num4].velocity.Y -= 1f;
                                            Main.dust[num4].velocity.X *= 2.5f;
                                            Main.dust[num4].scale = 1.3f;
                                            Main.dust[num4].alpha = 100;
                                            Main.dust[num4].noGravity = true;
                                        }

                                        SoundEngine.PlaySound(SoundID.SplashWeak, projectile.position);
                                    }
                                    else
                                    {
                                        for (int m = 0; m < 10; m++)
                                        {
                                            int num5 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y + (float)(projectile.height / 2) - 8f), projectile.width + 12, 24, Dust.dustWater());
                                            Main.dust[num5].velocity.Y -= 4f;
                                            Main.dust[num5].velocity.X *= 2.5f;
                                            Main.dust[num5].scale = 1.3f;
                                            Main.dust[num5].alpha = 100;
                                            Main.dust[num5].noGravity = true;
                                        }

                                        SoundEngine.PlaySound(SoundID.Splash, projectile.position);
                                    }
                                }
                                else
                                {
                                    for (int n = 0; n < 10; n++)
                                    {
                                        int num6 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y + (float)(projectile.height / 2) - 8f), projectile.width + 12, 24, 35);
                                        Main.dust[num6].velocity.Y -= 1.5f;
                                        Main.dust[num6].velocity.X *= 2.5f;
                                        Main.dust[num6].scale = 1.3f;
                                        Main.dust[num6].alpha = 100;
                                        Main.dust[num6].noGravity = true;
                                    }

                                    SoundEngine.PlaySound(SoundID.SplashWeak, projectile.position);
                                }
                            }

                            projectile.wet = true;
                        }
                        else if (projectile.wet)
                        {
                            projectile.wet = false;
                            if (projectile.type == 155)
                            {
                                projectile.velocity.Y *= 0.5f;
                            }
                            else if (projectile.wetCount == 0)
                            {
                                projectile.wetCount = 10;
                                if (!projectile.lavaWet)
                                {
                                    if (projectile.shimmerWet)
                                    {
                                        for (int num9 = 0; num9 < 10; num9++)
                                        {
                                            int num10 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y + (float)(projectile.height / 2) - 8f), projectile.width + 12, 24, 308);
                                            Main.dust[num10].velocity.Y -= 4f;
                                            Main.dust[num10].velocity.X *= 2.5f;
                                            Main.dust[num10].scale = 1.3f;
                                            Main.dust[num10].noGravity = true;
                                            switch (Main.rand.Next(6))
                                            {
                                                case 0:
                                                    Main.dust[num10].color = new Color(255, 255, 210);
                                                    break;
                                                case 1:
                                                    Main.dust[num10].color = new Color(190, 245, 255);
                                                    break;
                                                case 2:
                                                    Main.dust[num10].color = new Color(255, 150, 255);
                                                    break;
                                                default:
                                                    Main.dust[num10].color = new Color(190, 175, 255);
                                                    break;
                                            }

                                            SoundEngine.PlaySound(SoundID.SplashWeak, projectile.position);
                                        }
                                    }
                                    else if (projectile.honeyWet)
                                    {
                                        for (int num7 = 0; num7 < 10; num7++)
                                        {
                                            int num8 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y + (float)(projectile.height / 2) - 8f), projectile.width + 12, 24, 152);
                                            Main.dust[num8].velocity.Y -= 1f;
                                            Main.dust[num8].velocity.X *= 2.5f;
                                            Main.dust[num8].scale = 1.3f;
                                            Main.dust[num8].alpha = 100;
                                            Main.dust[num8].noGravity = true;
                                        }

                                        SoundEngine.PlaySound(SoundID.SplashWeak, projectile.position);
                                    }
                                    else
                                    {
                                        for (int num9 = 0; num9 < 10; num9++)
                                        {
                                            int num10 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y + (float)(projectile.height / 2)), projectile.width + 12, 24, Dust.dustWater());
                                            Main.dust[num10].velocity.Y -= 4f;
                                            Main.dust[num10].velocity.X *= 2.5f;
                                            Main.dust[num10].scale = 1.3f;
                                            Main.dust[num10].alpha = 100;
                                            Main.dust[num10].noGravity = true;
                                        }

                                        SoundEngine.PlaySound(SoundID.Splash, projectile.position);
                                    }
                                }
                                else
                                {
                                    for (int num11 = 0; num11 < 10; num11++)
                                    {
                                        int num12 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y + (float)(projectile.height / 2) - 8f), projectile.width + 12, 24, 35);
                                        Main.dust[num12].velocity.Y -= 1.5f;
                                        Main.dust[num12].velocity.X *= 2.5f;
                                        Main.dust[num12].scale = 1.3f;
                                        Main.dust[num12].alpha = 100;
                                        Main.dust[num12].noGravity = true;
                                    }

                                    SoundEngine.PlaySound(SoundID.SplashWeak, projectile.position);
                                }
                            }
                        }

                        if (!projectile.wet)
                        {
                            projectile.lavaWet = false;
                            projectile.honeyWet = false;
                            projectile.shimmerWet = false;
                        }

                        if (projectile.wetCount > 0)
                            projectile.wetCount--;
                    }
                    if (projectile.shimmerWet)
                        DDHelper.MethodReflection(projectile.GetType(), "Shimmer", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(projectile, null);
                    //原本的位置
                    projectile.oldPosition = projectile.position;
                    projectile.oldDirection = projectile.direction;
                    var A = new object[] { velocity,null,null };
                    DDHelper.MethodReflection(projectile.GetType(), "HandleMovement", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(projectile, A);
                    if ((bool)DDHelper.MethodReflection(projectile.GetType(), "AutomaticallyChangesDirection", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(projectile,null))
                    {
                        if (projectile.velocity.X < 0f)
                            projectile.direction = -1;
                        else
                            projectile.direction = 1;
                    }

                    if (!projectile.active)
                        return;

                    projectile.ProjLight();
                    if (!projectile.npcProj && projectile.friendly && Main.player[projectile.owner].magicQuiver && projectile.extraUpdates < 1 && projectile.arrow)
                        projectile.extraUpdates = 1;

                    if (projectile.type == 2 || projectile.type == 82)
                    {
                        Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100);
                    }
                    else if (projectile.type == 172)
                    {
                        Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 135, 0f, 0f, 100);
                    }
                    else if (projectile.type == 103)
                    {
                        int num13 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 75, 0f, 0f, 100);
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[num13].noGravity = true;
                            Main.dust[num13].scale *= 2f;
                        }
                    }
                    else if (projectile.type == 278)
                    {
                        int num14 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 169, 0f, 0f, 100);
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[num14].noGravity = true;
                            Main.dust[num14].scale *= 1.5f;
                        }
                    }
                    else if (projectile.type == 4)
                    {
                        if (Main.rand.Next(5) == 0)
                            Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 14, 0f, 0f, 150, default(Color), 1.1f);
                    }
                    else if (projectile.type == 5)
                    {
                        int num15;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                num15 = 15;
                                break;
                            case 1:
                                num15 = 57;
                                break;
                            default:
                                num15 = 58;
                                break;
                        }

                        Dust.NewDust(projectile.position, projectile.width, projectile.height, num15, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 150, default(Color), 1.2f);
                    }

                    projectile.Damage();
                    if (projectile.type == 434 && projectile.localAI[0] == 0f && projectile.numUpdates == 0)
                    {
                        projectile.extraUpdates = 1;
                        projectile.velocity = Vector2.Zero;
                        projectile.localAI[0] = 1f;
                        projectile.localAI[1] = 0.9999f;
                        projectile.netUpdate = true;
                    }

                    if (Main.netMode != 1 && (projectile.type == 99 || projectile.type == 655 || projectile.type == 727))
                        Collision.SwitchTiles(projectile.position, projectile.width, projectile.height, projectile.oldPosition, 3);

                    if (ProjectileID.Sets.TrailingMode[projectile.type] == 0)
                    {
                        for (int num16 = projectile.oldPos.Length - 1; num16 > 0; num16--)
                        {
                            projectile.oldPos[num16] = projectile.oldPos[num16 - 1];
                        }

                        projectile.oldPos[0] = projectile.position;
                    }
                    else if (ProjectileID.Sets.TrailingMode[projectile.type] == 1)
                    {
                        if (projectile.frameCounter == 0 || projectile.oldPos[0] == Vector2.Zero)
                        {
                            for (int num17 = projectile.oldPos.Length - 1; num17 > 0; num17--)
                            {
                                projectile.oldPos[num17] = projectile.oldPos[num17 - 1];
                            }

                            projectile.oldPos[0] = projectile.position;
                            if (projectile.velocity == Vector2.Zero && projectile.type == 466)
                            {
                                float num18 = projectile.rotation + (float)Math.PI / 2f + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                                float num19 = (float)Main.rand.NextDouble() * 2f + 2f;
                                Vector2 vector = new Vector2((float)Math.Cos(num18) * num19, (float)Math.Sin(num18) * num19);
                                int num20 = Dust.NewDust(projectile.oldPos[projectile.oldPos.Length - 1], 0, 0, 229, vector.X, vector.Y);
                                Main.dust[num20].noGravity = true;
                                Main.dust[num20].scale = 1.7f;
                            }

                            if (projectile.velocity == Vector2.Zero && projectile.type == 580)
                            {
                                float num21 = projectile.rotation + (float)Math.PI / 2f + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                                float num22 = (float)Main.rand.NextDouble() * 2f + 2f;
                                Vector2 vector2 = new Vector2((float)Math.Cos(num21) * num22, (float)Math.Sin(num21) * num22);
                                int num23 = Dust.NewDust(projectile.oldPos[projectile.oldPos.Length - 1], 0, 0, 229, vector2.X, vector2.Y);
                                Main.dust[num23].noGravity = true;
                                Main.dust[num23].scale = 1.7f;
                            }
                        }
                    }
                    else if (ProjectileID.Sets.TrailingMode[projectile.type] == 2)
                    {
                        for (int num24 = projectile.oldPos.Length - 1; num24 > 0; num24--)
                        {
                            projectile.oldPos[num24] = projectile.oldPos[num24 - 1];
                            projectile.oldRot[num24] = projectile.oldRot[num24 - 1];
                            projectile.oldSpriteDirection[num24] = projectile.oldSpriteDirection[num24 - 1];
                        }

                        projectile.oldPos[0] = projectile.position;
                        projectile.oldRot[0] = projectile.rotation;
                        projectile.oldSpriteDirection[0] = projectile.spriteDirection;
                    }
                    else if (ProjectileID.Sets.TrailingMode[projectile.type] == 3)
                    {
                        for (int num25 = projectile.oldPos.Length - 1; num25 > 0; num25--)
                        {
                            projectile.oldPos[num25] = projectile.oldPos[num25 - 1];
                            projectile.oldRot[num25] = projectile.oldRot[num25 - 1];
                            projectile.oldSpriteDirection[num25] = projectile.oldSpriteDirection[num25 - 1];
                        }

                        projectile.oldPos[0] = projectile.position;
                        projectile.oldRot[0] = projectile.rotation;
                        projectile.oldSpriteDirection[0] = projectile.spriteDirection;
                        float amount = 0.65f;
                        int num26 = 1;
                        for (int num27 = 0; num27 < num26; num27++)
                        {
                            for (int num28 = projectile.oldPos.Length - 1; num28 > 0; num28--)
                            {
                                if (!(projectile.oldPos[num28] == Vector2.Zero))
                                {
                                    if (projectile.oldPos[num28].Distance(projectile.oldPos[num28 - 1]) > 2f)
                                        projectile.oldPos[num28] = Vector2.Lerp(projectile.oldPos[num28], projectile.oldPos[num28 - 1], amount);

                                    projectile.oldRot[num28] = (projectile.oldPos[num28 - 1] - projectile.oldPos[num28]).SafeNormalize(Vector2.Zero).ToRotation();
                                }
                            }
                        }
                    }
                    else if (ProjectileID.Sets.TrailingMode[projectile.type] == 4)
                    {
                        Vector2 vector3 = Main.player[projectile.owner].position - Main.player[projectile.owner].oldPosition;
                        for (int num29 = projectile.oldPos.Length - 1; num29 > 0; num29--)
                        {
                            projectile.oldPos[num29] = projectile.oldPos[num29 - 1];
                            projectile.oldRot[num29] = projectile.oldRot[num29 - 1];
                            projectile.oldSpriteDirection[num29] = projectile.oldSpriteDirection[num29 - 1];
                            if (projectile.numUpdates == 0 && projectile.oldPos[num29] != Vector2.Zero)
                                projectile.oldPos[num29] += vector3;
                        }

                        projectile.oldPos[0] = projectile.position;
                        projectile.oldRot[0] = projectile.rotation;
                        projectile.oldSpriteDirection[0] = projectile.spriteDirection;
                    }
                    else if (ProjectileID.Sets.TrailingMode[projectile.type] == 5)
                    {
                        for (int num34 = projectile.oldPos.Length - 1; num34 > 0; num34--)
                        {
                            projectile.oldPos[num34] = projectile.oldPos[num34 - 1];
                            projectile.oldRot[num34] = projectile.oldRot[num34 - 1];
                            projectile.oldSpriteDirection[num34] = projectile.oldSpriteDirection[num34 - 1];
                        }

                        projectile.oldPos[0] = projectile.position;
                        projectile.oldRot[0] = velocity.ToRotation();
                        projectile.oldSpriteDirection[0] = projectile.spriteDirection;
                    }

                    if (ProjectileID.Sets.IsADD2Turret[projectile.type] && DD2Event.Ongoing)
                        projectile.timeLeft++;

                    projectile.timeLeft--;
                    if (projectile.timeLeft <= 0)
                        projectile.Kill();

                    if (projectile.penetrate == 0)
                        projectile.Kill();

                    if (!projectile.active || projectile.owner != Main.myPlayer)
                        continue;

                    if (projectile.netUpdate2)
                        projectile.netUpdate = true;

                    if (!projectile.active)
                        projectile.netSpam = 0;

                    if (projectile.netUpdate)
                    {
                        if (projectile.netSpam < 60)
                        {
                            projectile.netSpam += 5;
                            NetMessage.SendData(27, -1, -1, null, I);
                            projectile.netUpdate2 = false;
                        }
                        else
                        {
                            projectile.netUpdate2 = true;
                        }
                    }

                    if (projectile.netSpam > 0)
                        projectile.netSpam--;
                }

                projectile.netUpdate = false;*/
            }
        }
    }
}