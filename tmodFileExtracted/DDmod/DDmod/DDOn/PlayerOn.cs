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
using DDmod.AccessorySlot;
using Microsoft.CodeAnalysis.Differencing;
using Terraria.IO;
using Terraria.Net;
using DDmod.Content.Tiles.绿岩;

namespace DDmod.DDOn
{

    internal static class PlayerOn
    {
        public static void Load()
        {
            Terraria.On_Player.Update += Player_Update;
            Terraria.On_Player.UpdateBiomes += Player_UpdateBiomes;
            Terraria.DataStructures.On_PlayerDrawLayers.DrawPlayer_09_Wings += DrawPlayer_Wings;
            Terraria.DataStructures.On_PlayerDrawLayers.DrawPlayer_13_Leggings += DrawPlayer_Leggings;
            Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.DrawPlayers += DrawPlayers;
            Terraria.DataStructures.On_PlayerDrawSet.BoringSetup_2 += BoringSetup_2;
            Terraria.On_Player.PlayerFrame += PlayerAction.PlayerFrame;
            //绘制玩家绳索
            Terraria.DataStructures.On_PlayerDrawLayers.DrawPlayer_24_Pulley += DrawPlayer_Pulley;
            Terraria.On_Player.addDPS += addDPS;
            Terraria.On_Player.getDPS += getDPS;
            Terraria.On_Player.GetAdjustedItemScale += Player_GetAdjustedItemScale;
            Terraria.On_Player.Heal += Player_Heal;
            On_Player.ApplyPotionDelay += ApplyPotionDelay;
            On_Player.KeyDoubleTap += KeyDoubleTap;
            Terraria.On_Player.UpdateVisibleAccessory += UpdateVisibleAccessory;
            Terraria.On_Player.ApplyEquipFunctional += ApplyEquipFunctional;
        }
        public static void ApplyEquipFunctional(On_Player.orig_ApplyEquipFunctional orig, Player player, Item item, bool moddad)
        {
            if (!Main.gameMenu && player.ActiveItem().type > 0 && player.ActiveItem().DItem().Twin && item.shieldSlot >= 0)
            {
                return;
            }
            orig(player, item, moddad);
        }
        public static void UpdateVisibleAccessory(On_Player.orig_UpdateVisibleAccessory orig, Player player, int itemSlot, Item item,bool moddad)
        {
            if(!Main.gameMenu&& player.ActiveItem().type>0 && player.ActiveItem().DItem().Twin&&item.shieldSlot>=0)
            {
                return;
            }
            orig(player,itemSlot,item,moddad);
        }
        public static void KeyDoubleTap(On_Player.orig_KeyDoubleTap orig, Player player, int KeyDir)
        {
            int num = 0;
            if (Main.ReversedUpDownArmorSetBonuses)
                num = 1;

            if (KeyDir != num&&KeyDir<1)
                return;

            PlayerLoader.ArmorSetBonusActivated(player);

            if (player.setVortex && !player.mount.Active)
                player.vortexStealthActive = !player.vortexStealthActive;

            if (player.setForbidden)
            {
                player.MinionRestTargetAim();
                if (!player.setForbiddenCooldownLocked)
                    player.CommandForbiddenStorm();
            }

            //PlayerLoader.SetControls(player);
            if ((KeyDir == 0 && !player.controlDown) || (KeyDir == 1 && !player.controlUp) || (KeyDir == 2 && !player.controlRight) || (KeyDir == 3 && !player.controlLeft))
            {
                KeyDir = -1;
            }

            orig(player, KeyDir);
            /*
            int num = 0;
            if (Main.ReversedUpDownArmorSetBonuses)
                num = 1;
            */
            AttributesPlayer.KeyDoubleTap(player, KeyDir);
        }
        public static void ApplyPotionDelay(On_Player.orig_ApplyPotionDelay orig, Player player, Item sItem)
        {
            orig(player, sItem);
            if(sItem.DItem().PotionCD>0)
            {
                if (player.HasBuff(21) && player.buffTime[player.FindBuffIndex(21)] > (int)(sItem.DItem().PotionCD* PhilosopherStoneDurationMultiplier))
                {
                    player.buffTime[player.FindBuffIndex(21)] = (int)(sItem.DItem().PotionCD* PhilosopherStoneDurationMultiplier);
                }
            }
        }
        public static void Player_Heal(Terraria.On_Player.orig_Heal orig, Player player, int Life)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                player.HealEffect(Life);
                player.statLife += Life;
                //if (player.statLife > player.statLifeMax2)
                    //player.statLife = player.statLifeMax2;
            }
            else
            {
                DDmod.SyncData(DDType.PlayerHeal, player.whoAmI, player.whoAmI, -1, (short)Life);
            }
        }
        public static void HandleHeal(Mod mod, BinaryReader reader)
        {
            byte player = reader.ReadByte();
            int Life = reader.ReadInt16();
            if (player == Main.myPlayer)
            {
                Main.player[player].HealEffect(Life, true);
                Main.player[player].statLife += Life;
            }
            if (Main.netMode == 2)
            {
                DDmod.SyncData(DDType.PlayerHeal, player, player, -1, (short)Life);
            }
        }

        public static int getDPS(Terraria.On_Player.orig_getDPS  orig, Player player)
        {
            return 0;
        }
        public static void addDPS(Terraria.On_Player.orig_addDPS orig, Player player, int dmg)
        {
            if (player.dpsStarted)
            {
                player.dpsLastHit = DateTime.Now;
                player.Dplayer().dpsDamage += dmg;
                player.dpsEnd = DateTime.Now;
            }
            else
            {
                player.dpsStarted = true;
                player.dpsStart = DateTime.Now;
                player.dpsEnd = DateTime.Now;
                player.dpsLastHit = DateTime.Now;
                player.Dplayer().dpsDamage = dmg;
            }
        }
        public static void Player_UpdateBiomes(Terraria.On_Player.orig_UpdateBiomes orig, Player player)
        {
            orig(player);
        }
        //玩家绘制
        public static void BoringSetup_2(Terraria.DataStructures.On_PlayerDrawSet.orig_BoringSetup_2 orig, ref PlayerDrawSet playerDraw, Player player, List<DrawData> drawData, List<int> dust, List<int> gore, Vector2 drawPosition, float shadowOpacity, float rotation, Vector2 rotationOrigin)
        {
            orig(ref playerDraw, player, drawData, dust, gore, drawPosition, shadowOpacity, rotation, rotationOrigin);
            float head = (player.Dplayer().MouseWorld - player.Center).ToRotation();
            if (player.direction < 0)
            {
                head = -(player.Dplayer().MouseWorld - player.Center).ToRotation() + MathHelper.Pi;
                if (head > MathHelper.Pi)
                {
                    head -= MathHelper.TwoPi;
                }
            }
            if (Main.gameMenu)
            {
                head = 0;
            }
            //player.RotationSpeed(player.fullRotation, 0);
            DDHelper.MaxandMinF(ref head, 0.3f, -0.6f);
            if (!player.sleeping.isSleeping && !player.sitting.isSitting && player.PlayerAction().headRotation&&player.gravDir==1)
            {
                player.headRotation = head * player.direction;
                if (player.headRotation.ToRotationVector2().Y > 0)
                {
                    if (player.direction==1)
                    {
                        player.headPosition.Y += player.headRotation.ToRotationVector2().Y * 2;
                    }
                    else
                    {
                        player.headPosition.X = 0;
                    }
                }
                else
                {
                    if (player.direction == -1)
                    {
                        player.headPosition.Y += -player.headRotation.ToRotationVector2().Y * 2;
                    }
                    else
                    {
                        player.headPosition.X = 0;
                    }
                }
                if (player.headRotation.ToRotationVector2().Y > 0)
                {
                    //player.headPosition.Y -= player.headRotation.ToRotationVector2().Y * 2;
                }
            }
            else
            {
                player.headRotation = 0;
            }
        }
        public static void DrawPlayers(Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.orig_DrawPlayers orig, LegacyPlayerRenderer legacy, Camera camera, IEnumerable<Player> players)
        {

            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vector2> vectors = new List<Vector2>();
            foreach (Player drawPlayer in players)
            {
                vectors.Add(drawPlayer.position);
                if (drawPlayer.Dplayer().PDraw)
                {
                    drawPlayer.Dplayer().PDraw = false;
                    Main.spriteBatch.End();
                    return;
                }
                Color LightColor = Lighting.GetColor((int)(drawPlayer.Center.X / 16), (int)(drawPlayer.Center.Y / 16), Color.White);
                Color immuneAlpha = drawPlayer.GetImmuneAlpha(LightColor, drawPlayer.immuneAlpha);
                PlayerDraw.PreDraw(camera.SpriteBatch, drawPlayer, LightColor, immuneAlpha);
            }
            Main.spriteBatch.End();
            orig(legacy, camera, players);

            foreach (Player drawPlayer in players)
            {
               // drawPlayer.position = vectors[drawPlayer.whoAmI];
            }

            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            
            foreach (Player drawPlayer in players)
            {
                   Color LightColor = Lighting.GetColor((int)(drawPlayer.Center.X / 16), (int)(drawPlayer.Center.Y / 16), Color.White);
                Color immuneAlpha = drawPlayer.GetImmuneAlpha(LightColor, drawPlayer.immuneAlpha);
                PlayerDraw.PostDraw(camera.SpriteBatch, drawPlayer, LightColor, immuneAlpha);
            }
            Main.spriteBatch.End();
        }
        //绘制翅膀
        public static void DrawPlayer_Wings(Terraria.DataStructures.On_PlayerDrawLayers.orig_DrawPlayer_09_Wings orig, ref PlayerDrawSet drawSet)
        {
            drawSet.drawPlayer.AccPlayer().wing = drawSet.drawPlayer.wings;
            if (drawSet.drawPlayer.AccPlayer().wingslot == 0)
            {
                orig(ref drawSet);
            }
        }
        //绘制后背手前面
        public static void DrawPlayer_Leggings(Terraria.DataStructures.On_PlayerDrawLayers.orig_DrawPlayer_13_Leggings orig, ref PlayerDrawSet drawSet)
        {
            DrawWeaponHand2 hand = new DrawWeaponHand2();
            Player drawPlayer = drawSet.drawPlayer;
            if (drawSet.shadow == 0f && !drawPlayer.dead && drawPlayer.PlayerAction().Action && drawPlayer.PlayerAction().ShowWeapons)
            {
                hand.DrawWithTransformationAndChildren(ref drawSet);
            }
            orig(ref drawSet);
        }
        public static void DrawPlayer_Pulley(Terraria.DataStructures.On_PlayerDrawLayers.orig_DrawPlayer_24_Pulley orig, ref PlayerDrawSet drawinfo)
        {
            if (drawinfo.drawPlayer.pulley && drawinfo.drawPlayer.itemAnimation == 0)
            {
                if (drawinfo.drawPlayer.pulleyDir == 2)
                {
                    int num = -25;
                    int num2 = 0;
                    float rotation = 0f;
                    DrawData item = new DrawData(TextureAssets.Pulley.Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2) - (float)(9 * drawinfo.drawPlayer.direction)) + num2 * drawinfo.drawPlayer.direction, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 2f * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir)), new Rectangle(0, TextureAssets.Pulley.Height() / 2 * drawinfo.drawPlayer.pulleyFrame, TextureAssets.Pulley.Width(), TextureAssets.Pulley.Height() / 2), drawinfo.colorArmorHead, rotation, new Vector2(TextureAssets.Pulley.Width() / 2, TextureAssets.Pulley.Height() / 4), 1f, drawinfo.playerEffect, 0);
                    drawinfo.DrawDataCache.Add(item);
                }
                else
                {
                    int num3 = -26;
                    int num4 = 10;
                    float rotation2 = 0.35f * (float)(-drawinfo.drawPlayer.direction);
                    DrawData item = new DrawData(TextureAssets.Pulley.Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2) - (float)(9 * drawinfo.drawPlayer.direction)) + num4 * drawinfo.drawPlayer.direction, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 2f * drawinfo.drawPlayer.gravDir + (float)num3 * drawinfo.drawPlayer.gravDir)), new Rectangle(0, TextureAssets.Pulley.Height() / 2 * drawinfo.drawPlayer.pulleyFrame, TextureAssets.Pulley.Width(), TextureAssets.Pulley.Height() / 2), drawinfo.colorArmorHead, rotation2, new Vector2(TextureAssets.Pulley.Width() / 2, TextureAssets.Pulley.Height() / 4), 1f, drawinfo.playerEffect, 0);
                    drawinfo.DrawDataCache.Add(item);
                }
            }
        }
        public static void Player_Update(Terraria.On_Player.orig_Update orig, Player player, int I)
        {
            if (Start>0 || WorldGen.IsGeneratingHardMode)
            {
                player.releaseInventory = false;
                player.immune = true;
                player.immuneTime = 180;
                player.maxMinions = 9999;
                player.Dplayer().Playerperspective();
            }
            else
            {
                orig(player, I);
            }
        }
        public static float Player_GetAdjustedItemScale(Terraria.On_Player.orig_GetAdjustedItemScale orig, Player player, Item item)
        {
            float scale = item.scale;
            if (item.DamageType == DamageClass.Melee)
            {
                if (player.Dplayer().MeleeScale >= 1.5)
                {
                    player.Dplayer().MeleeScale = 1.5F;
                }
                scale *= player.Dplayer().MeleeScale;
            }
            CombinedHooks.ModifyItemScale(player, item, ref scale);
            return scale;
        }
    }
}