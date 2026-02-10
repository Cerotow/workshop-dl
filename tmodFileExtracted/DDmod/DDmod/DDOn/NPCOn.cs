using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.NPCs.TownNPC;
using DDmod.NoContent.Config;
using DDmod.SubworldLibraryWorld;
using DDmod.Worlds;
using System.Reflection;
using Terraria.GameInput;

namespace DDmod.DDOn
{
    internal static class NPCOn
    {
        public static void Load()
        {
            //Terraria.On_NPC.VanillaHitEffect += NPC_VanillaHitEffect;
            Terraria.On_NPC.HitEffect_HitInfo += NPC_HitEffect;
            Terraria.On_NPC.ScaleStats += NPC_ScaleStats;
            Terraria.On_NPC.StrikeNPC_HitInfo_bool_bool += NPC_StrikeNPC;
            Terraria.On_NPC.UpdateNPC_Inner += UpdateNPC_Inner;
            Terraria.On_NPC.UpdateCollision += UpdateCollision;
            Terraria.On_NPC.CanBeChasedBy += CanBeChasedBy;
            //NPC进入岩浆
            Terraria.On_NPC.Collision_LavaCollision += Collision_LavaCollision;
            Terraria.On_Main.DrawNPCChatBubble += DrawNPCChatBubble;
            Terraria.On_Main.HoverOverNPCs += HoverOverNPCs;
            Terraria.On_NPC.NPCLoot += NPC_NPCLoot;
        }
        //鼠标文本
        public static void HoverOverNPCs(On_Main.orig_HoverOverNPCs orig, Main Main, Rectangle mouseRectangle)
        {
            Player player = Main.player[Main.myPlayer];
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.ShowNameOnHover)
                {
                    continue;
                }

                if (!(npc.active & (npc.shimmerTransparency == 0f || npc.CanApplyHunterPotionEffects())))
                {
                    continue;
                }

                int type = npc.type;
                Main.LoadNPC(type);
                npc.position += npc.netOffset;
                Microsoft.Xna.Framework.Rectangle value = new Microsoft.Xna.Framework.Rectangle((int)npc.Bottom.X - npc.frame.Width / 2, (int)npc.Bottom.Y - npc.frame.Height, npc.frame.Width, npc.frame.Height);
                if (npc.type >= 87 && npc.type <= 92)
                {
                    value = new Microsoft.Xna.Framework.Rectangle((int)(npc.position.X + npc.width * 0.5 - 32.0), (int)(npc.position.Y + npc.height * 0.5 - 32.0), 64, 64);
                }

                NPCLoader.ModifyHoverBoundingBox(npc, ref value);

                bool flag = mouseRectangle.Intersects(value);
                bool flag2 = flag || (Main.SmartInteractShowingGenuine && Main.SmartInteractNPC == i);
                if (flag2 && ((npc.type != 85 && npc.type != 341 && npc.type != 629 && npc.aiStyle != 87) || npc.ai[0] != 0f) && npc.type != 488)
                {
                    if (npc.type == 685)
                    {
                        player.cursorItemIconEnabled = true;
                        player.cursorItemIconID = 327;
                        player.cursorItemIconText = "";
                        player.noThrow = 2;
                        if (!player.dead)
                        {
                            PlayerInput.SetZoom_MouseInWorld();
                            if (Main.mouseRight && Main.npcChatRelease)
                            {
                                Main.npcChatRelease = false;
                                if (PlayerInput.UsingGamepad)
                                {
                                    player.releaseInventory = false;
                                }

                                bool TryFreeingElderSlime()
                                {
                                    Player player = Main.player[Main.myPlayer];
                                    short type = 327;
                                    bool inVoidBag = false;
                                    int num = player.FindItemInInventoryOrOpenVoidBag(type, out inVoidBag);
                                    if (num == -1)
                                    {
                                        return false;
                                    }

                                    Item item = null;
                                    item = ((!inVoidBag) ? player.inventory[num] : player.bank4.item[num]);
                                    if (--item.stack <= 0)
                                    {
                                        item.TurnToAir();
                                    }

                                    Recipe.FindRecipes();
                                    return true;
                                }
                                if (player.talkNPC != i && !player.tileInteractionHappened && TryFreeingElderSlime())
                                {
                                    NPC.TransformElderSlime(i);
                                    SoundEngine.PlaySound(SoundID.Unlock);
                                }
                            }
                        }
                    }
                    else
                    {
                        bool flag3 = Main.SmartInteractShowingGenuine && Main.SmartInteractNPC == i;
                        bool vanillaCanChat = false;
                        if (npc.townNPC || npc.type == 105 || npc.type == 106 || npc.type == 123 || npc.type == 354 || npc.type == 376 || npc.type == 579 || npc.type == 453 || npc.type == 589)
                        {
                            vanillaCanChat = true;
                        }

                        if (NPCLoader.CanChat(npc) ?? vanillaCanChat)
                        {
                            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int)(player.position.X + player.width / 2 - Player.tileRangeX * 16), (int)(player.position.Y + player.height / 2 - Player.tileRangeY * 16), Player.tileRangeX * 16 * 2, Player.tileRangeY * 16 * 2);
                            Microsoft.Xna.Framework.Rectangle value2 = new Microsoft.Xna.Framework.Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                            if (rectangle.Intersects(value2))
                            {
                                flag3 = true;
                            }
                        }

                        if (player.ownedProjectileCounts[651] > 0)
                        {
                            flag3 = false;
                        }

                        if (flag3 && !player.dead)
                        {
                            PlayerInput.SetZoom_MouseInWorld();
                            Main.HoveringOverAnNPC = true;
                            Main.currentNPCShowingChatBubble = i;
                            if (Main.mouseRight && Main.npcChatRelease)
                            {
                                Main.npcChatRelease = false;
                                if (PlayerInput.UsingGamepad)
                                {
                                    player.releaseInventory = false;
                                }

                                if (player.talkNPC != i && !player.tileInteractionHappened)
                                {
                                    Main.CancelHairWindow();
                                    Main.SetNPCShopIndex(0);
                                    Main.InGuideCraftMenu = false;
                                    player.dropItemCheck();
                                    Main.npcChatCornerItem = 0;
                                    player.sign = -1;
                                    Main.editSign = false;
                                    player.SetTalkNPC(i);
                                    Main.playerInventory = false;
                                    player.chest = -1;
                                    Recipe.FindRecipes();
                                    Main.npcChatText = npc.GetChat();
                                    SoundEngine.PlaySound(SoundID.Chat);
                                }
                            }
                        }

                        if (flag && !player.mouseInterface)
                        {
                            player.cursorItemIconEnabled = false;
                            string text = npc.GivenOrTypeName;
                            int num = i;
                            if (npc.realLife >= 0)
                            {
                                num = npc.realLife;
                            }

                            if (Main.npc[num].lifeMax > 1 && !Main.npc[num].dontTakeDamage)
                            {
                                text = text + ": " + Main.npc[num].life + "/" + Main.npc[num].lifeMax;
                            }
                            if (Main.npc[num].Dnpc().Battlepet)
                            {
                                text += "\n" + Language.GetTextValue("Mods.DDmod.BattlePetText.可捕捉宠物");
                            }
                            if (Main.npc[num].SuperArmor)
                            {
                                text += "\n" + Language.GetTextValue("Mods.DDmod.properties.绝对防御");
                            }
                            else
                            {
                                text += "\n" + Language.GetTextValue("Mods.DDmod.properties.防御") + ": " + Main.npc[num].defense;
                            }

                            if (Main.npc[num].Exp().Exp > 0 || Main.npc[num].realLife != -1)
                            {
                                if (Main.npc[num].realLife == -1)
                                {
                                    text += "\nExp: " + Main.npc[num].Exp().Exp;
                                }
                                else
                                {
                                    text += "\nExp: " + Main.npc[Main.npc[num].realLife].Exp().Exp;
                                }

                            }
                            Main.MouseTextHackZoom(text);
                            Main.mouseText = true;
                            npc.position -= npc.netOffset;
                            break;
                        }

                        if (flag2)
                        {
                            npc.position -= npc.netOffset;
                            break;
                        }
                    }
                }

                npc.position -= npc.netOffset;
            }
        }
        //岩浆
        public static bool Collision_LavaCollision(Terraria.On_NPC.orig_Collision_LavaCollision orig, NPC npc)
        {
            bool num = Collision.LavaCollision(npc.position, npc.width, npc.height);
            if (num)
            {
                npc.lavaWet = true;
                if (!npc.lavaImmune && !npc.dontTakeDamage && Main.netMode != 1 && npc.immune[255] == 0)
                {
                    npc.AddBuff(24, 420);
                    npc.immune[255] = 30;
                    int damage = 50;
                    if (npc.Dnpc().Properties.Gel)
                    {
                        damage += 100;
                    }
                    if (npc.Dnpc().Properties.Ice)
                    {
                        damage += 200;
                    }
                    npc.SimpleStrikeNPC(damage, 0);
                    if (Main.netMode == 2 && Main.netMode != 0)
                    {
                        NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, 50f);
                    }
                }
                if (npc.Dnpc().Properties.Fire && npc.life < npc.lifeMax)
                {
                    npc.life++;
                }
            }
            return num;
        }
        public static bool CanBeChasedBy(Terraria.On_NPC.orig_CanBeChasedBy orig, NPC npc, object attacker = null, bool ignoreDontTakeDamage = false)
        {
            /*
            if (npc.active && npc.chaseable && npc.lifeMax > 5 && (!npc.dontTakeDamage || ignoreDontTakeDamage) && !npc.friendly && !npc.Dnpc().Neutrality)
                return !npc.immortal;
            */
            if (npc.active && npc.chaseable && npc.lifeMax > 5 && (!npc.dontTakeDamage || ignoreDontTakeDamage) && !npc.friendly)
            {
                return !npc.immortal;
            }

            return false;
        }
        public static void AI(NPC npc)
        {
            if (npc.Dnpc().velocity != Vector2.Zero)
            {
                npc.velocity = npc.Dnpc().velocity;
                npc.Dnpc().velocity = Vector2.Zero;
                npc.netUpdate = true;
            }
            for (int A = 0; A < 200; A++)
            {
                NPC npcA = Main.npc[A];
                Rectangle npcRectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.height, npc.height);
                Rectangle playerRectangle = new Rectangle((int)npcA.position.X, (int)npcA.position.Y, npcA.width, npcA.height);
                if (npcA.active && npcA.CanBeChasedBy() && npc != npcA && !npcA.friendly)
                {
                    if (playerRectangle.Intersects(npcRectangle) && npc.Dnpc().Control < 2000000)
                    {
                        if (npc.Dnpc().Control >= 110000)
                        {
                            if (npcA.damage > 0)
                            {
                                if (npc.Dnpc().InvincibleFrame[255] == 0)
                                {
                                    npc.SimpleStrikeNPC(npcA.damage, npc.direction, false, 1);
                                    npc.Dnpc().InvincibleFrame[255] = 30;
                                }
                                else
                                {
                                    npc.Dnpc().InvincibleFrame[255]--;
                                }
                            }

                        }
                        else
                        {
                            if (npcA.Dnpc().InvincibleFrame[255] == 0)
                            {
                                if (npcA.damage > 30)
                                {
                                    npc.SimpleStrikeNPC(npcA.damage, npc.direction, false, 0);
                                }
                                else
                                {
                                    npc.SimpleStrikeNPC(30, npc.direction, false, 0);
                                }
                                if (npc.damage > 30)
                                {
                                    npcA.SimpleStrikeNPC(npc.damage, npc.direction, false, npc.velocity.X);
                                    npcA.Dnpc().InvincibleFrame[255] = 30;
                                }
                                else
                                {
                                    npcA.SimpleStrikeNPC(30, npc.direction, false, npc.velocity.X);
                                    npcA.Dnpc().InvincibleFrame[255] = 30;
                                }
                                npc.velocity = new Vector2(npc.velocity.PerfectNormalize().X * -8, -8);
                                npc.Dnpc().Control = 10000000;
                            }
                            else
                            {
                                npcA.Dnpc().InvincibleFrame[255]--;
                            }
                        }
                    }
                }
            }
            if (npc.velocity.Y > 0)
            {
                npc.Dnpc().FallDamage += 0.2f;
            }
            if (npc.velocity.Y == 0 && npc.Dnpc().Control > 200)
            {
                npc.velocity = Vector2.Zero;
                npc.SimpleStrikeNPC((int)npc.Dnpc().FallDamage, npc.direction, false, 0);
                npc.Dnpc().FallDamage = 0;
            }
            if (npc.velocity.Length() <= 0)
            {
                npc.Dnpc().Control = 0;
            }
            else
            {
                npc.Dnpc().Control = 100000;
            }
            if (npc.Dnpc().oldrotation == 0)
            {
                npc.Dnpc().oldrotation = npc.rotation;
            }
            npc.rotation += npc.velocity.X * 0.03F;
            if (Main.player[npc.Dnpc().Player] != null)
            {
                //DDmod.StartSyncNPC(npc, Main.player[npc.Dnpc().Player]);
            }
        }
        public static void UpdateCollision(Terraria.On_NPC.orig_UpdateCollision orig, NPC npc)
        {
            DDHelper.MethodReflection(npc.GetType(), "Collision_WalkDownSlopes", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
            bool lava = (bool)DDHelper.MethodReflection(npc.GetType(), "Collision_LavaCollision", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
            lava = (bool)DDHelper.MethodReflection(npc.GetType(), "Collision_WaterCollision", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { lava });
            if (!npc.wet)
            {
                npc.lavaWet = false;
                npc.honeyWet = false;
                npc.shimmerWet = false;
            }

            if (npc.wetCount > 0)
            {
                npc.wetCount--;
            }

            bool fall = (bool)DDHelper.MethodReflection(npc.GetType(), "Collision_DecideFallThroughPlatforms", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
            npc.oldVelocity = npc.velocity;
            npc.collideX = false;
            npc.collideY = false;
            DDHelper.MethodReflection(npc.GetType(), "FishTransformationDuringRain", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
            npc.GetTileCollisionParameters(out Vector2 cPosition, out int cWidth, out int cHeight);
            Vector2 velocity = npc.velocity;
            velocity.X *= npc.Dnpc().MoveSpeed;
            if (npc.noGravity)
            {
                velocity.Y *= npc.Dnpc().MoveSpeed;
            }
            else if (velocity.Y < 0)
            {
                velocity.Y *= npc.Dnpc().MoveSpeed;
            }
            DDHelper.MethodReflection(npc.GetType(), "ApplyTileCollision", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { fall, cPosition, cWidth, cHeight });
            if (npc.wet)
            {

                if (npc.shimmerWet)
                {
                    DDHelper.MethodReflection(npc.GetType(), "Collision_MoveWhileWet", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { velocity, npc.shimmerMovementSpeed });
                }
                else if (npc.honeyWet)
                {
                    DDHelper.MethodReflection(npc.GetType(), "Collision_MoveWhileWet", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { velocity, npc.honeyMovementSpeed });
                }
                else if (npc.lavaWet)
                {
                    DDHelper.MethodReflection(npc.GetType(), "Collision_MoveWhileWet", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { velocity, npc.lavaMovementSpeed });
                }
                else
                {
                    DDHelper.MethodReflection(npc.GetType(), "Collision_MoveWhileWet", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { velocity, npc.waterMovementSpeed });
                }
            }
            else
            {
                if (Collision.up)
                {
                    npc.velocity.Y = 0.01f;
                }

                if (npc.oldVelocity.X != npc.velocity.X)
                {
                    npc.collideX = true;
                }

                if (npc.oldVelocity.Y != npc.velocity.Y)
                {
                    npc.collideY = true;
                }

                npc.oldPosition = npc.position;
                npc.oldDirection = npc.direction;

                Vector2 velocity2 = npc.velocity;
                velocity2.X *= npc.Dnpc().MoveSpeed;
                if (npc.noGravity)
                {
                    velocity2.Y *= npc.Dnpc().MoveSpeed;
                }
                else if (velocity2.Y < 0)
                {
                    velocity2.Y *= npc.Dnpc().MoveSpeed;
                }
                npc.position += velocity2;
            }

            if (npc.aiStyle == 67)
            {
                DDHelper.MethodReflection(npc.GetType(), "Collision_MoveSnailOnSlopes", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
            }
            else if (npc.type != 72 && npc.type != 247 && npc.type != 248 && (npc.type < 542 || npc.type > 545) && (!NPCID.Sets.BelongsToInvasionOldOnesArmy[npc.type] || !npc.noGravity))
            {
                DDHelper.MethodReflection(npc.GetType(), "Collision_MoveSlopesAndStairFall", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { fall });
                if (npc.townNPC)
                {
                    Collision.StepConveyorBelt(npc, 1f);
                }
            }
        }
        public static void UpdateNPC_Inner(Terraria.On_NPC.orig_UpdateNPC_Inner orig, NPC npc, int i)
        {
            if (npc.type == 0)
            {
                npc.active = false;
            }
            if (offSetDelayTime > 0)
            {
                npc.netOffset *= 0f;
            }
            else if (Main.netMode == 2)
            {
                npc.netOffset *= 0f;
            }
            else if (Main.multiplayerNPCSmoothingRange <= 0)
            {
                npc.netOffset *= 0f;
            }
            else if (npc.netOffset != new Vector2(0f, 0f))
            {
                if (NPCID.Sets.NoMultiplayerSmoothingByType[npc.type])
                {
                    npc.netOffset *= 0f;
                }
                else if (npc.aiStyle >= 0 && npc.aiStyle < NPCLoader.NPCCount && NPCID.Sets.NoMultiplayerSmoothingByAI[npc.aiStyle])
                {
                    npc.netOffset *= 0f;
                }
                else
                {
                    float num = 2f;
                    float num2 = Main.multiplayerNPCSmoothingRange;
                    float num3 = npc.netOffset.Length();
                    if (num3 > num2)
                    {
                        npc.netOffset.Normalize();
                        npc.netOffset *= num2;
                        num3 = npc.netOffset.Length();
                    }

                    num += num3 / num2 * num;
                    Vector2 vector = npc.netOffset;
                    vector.Normalize();
                    vector *= num;
                    npc.netOffset -= vector;
                    if (npc.netOffset.Length() < num)
                        npc.netOffset *= 0f;

                    if (npc.townNPC)
                    {
                        if (Vector2.Distance(npc.position, new Vector2(npc.homeTileX * 16 + 8 - npc.width / 2, (float)(npc.homeTileY * 16 - npc.height) - 0.1f)) < 1f)
                            npc.netOffset *= 0f;

                        if (npc.ai[0] == 25f)
                            npc.netOffset *= 0f;
                    }
                }
            }

            npc.UpdateAltTexture();
            if (npc.type == 368)
                travelNPC = true;

            if (Main.netMode != 2)
            {
                DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_CastLights", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
            }

            DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_TeleportVisuals", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
            DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_CritterSounds", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);

            object[] o = new object[1];
            o[0] = i;
            DDHelper.MethodReflection(npc.GetType(), "TrySyncingUniqueTownNPCData", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, o);
            if (npc.aiStyle == 7 && npc.position.Y > Main.bottomWorld - 640f + npc.height && Main.netMode != 1 && !Main.xMas)
            {
                npc.SimpleStrikeNPC(9999, 0, knockBack: 0, noPlayerInteraction: true);
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(28, -1, -1, null, npc.whoAmI, 9999f);
                }
            }


            if (Main.netMode == 1)
            {
                bool flag = false;
                int num4 = (int)(npc.position.X + npc.width / 2) / 16;
                int num5 = (int)(npc.position.Y + npc.height / 2) / 16;
                flag = !Main.sectionManager.TilesLoaded(num4 - 3, num5 - 3, num4 + 3, num5 + 3);
                if (flag)
                {
                    return;
                }
            }

            DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_BuffFlagsReset", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
            NPCLoader.ResetEffects(npc);
            DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_UpdateGravity", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);

            npc.UpdateNPC_BuffSetFlags();

            DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_SoulDrainDebuff", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);

            DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_BuffClearExpiredBuffs", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);

            DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_BuffApplyDOTs", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);

            DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_BuffApplyVFX", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);

            DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_BloodMoonTransformations", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);

            if (npc.soundDelay > 0)
                npc.soundDelay--;

            if (npc.life <= 0)
            {
                npc.active = false;
                o[0] = i;
                DDHelper.MethodReflection(npc.GetType(), "UpdateNetworkCode", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, o);
                npc.netUpdate = false;
                npc.Dnpc().netUpdate = false;
                npc.justHit = false;
                return;
            }

            npc.oldTarget = npc.target;
            npc.oldDirection = npc.direction;
            npc.oldDirectionY = npc.directionY;
            float num6 = 1f + Math.Abs(npc.velocity.X) / 3f;
            if (npc.gfxOffY > 0f)
            {
                npc.gfxOffY -= num6 * npc.stepSpeed;
                if (npc.gfxOffY < 0f)
                {
                    npc.gfxOffY = 0f;
                }
            }
            else if (npc.gfxOffY < 0f)
            {
                npc.gfxOffY += num6 * npc.stepSpeed;
                if (npc.gfxOffY > 0f)
                {
                    npc.gfxOffY = 0f;
                }
            }

            if (npc.gfxOffY > 16f)
            {
                npc.gfxOffY = 16f;
            }

            if (npc.gfxOffY < -16f)
            {
                npc.gfxOffY = -16f;
            }

            npc.TryPortalJumping();
            npc.IdleSounds();
            if (npc.type > 0 && npc.Dnpc() != null && npc.Dnpc().Control > 0)
            {
                AI(npc);
                npc.Dnpc().Control--;
                npc.frameCounter = 0;
            }
            else if (npc.Dnpc() != null)
            {
                if (npc.realLife > 0 && Main.npc[npc.realLife].whoAmI != npc.whoAmI && Main.npc[npc.realLife].HasBuff(ModContent.BuffType<Fossil>()))
                {
                    npc.AddBuff(ModContent.BuffType<Fossil>(), 2);
                }
                if (npc.Dnpc().NoMove || npc.Dnpc().Control2 > 0)
                {
                    if (npc.Dnpc().Control2 > 0)
                    {
                        npc.Dnpc().Control2--;
                    }
                    for (int a = 0; a < npc.oldPos.Length; a++)
                    {
                        npc.oldPos[a] = Vector2.Zero;
                        npc.oldRot[a] = npc.rotation;
                    }
                    npc.velocity.X = 0;
                    npc.frameCounter = 0;
                }
                else
                {
                    npc.AI();
                }
                if (npc.Dnpc().oldrotation != 0)
                {
                    npc.rotation = npc.Dnpc().oldrotation;
                    npc.rotation = 0;
                    npc.Dnpc().oldrotation = 0;
                }
            }

            DDHelper.MethodReflection(npc.GetType(), "SubAI_HandleTemporaryCatchableNPCPlayerInvulnerability", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
            if (Main.netMode != 2 && npc.extraValue > 0)
            {
                int num7 = 244;
                float num8 = 30f;
                if (npc.extraValue >= 1000000)
                {
                    num7 = 247;
                    num8 *= 0.25f;
                }
                else if (npc.extraValue >= 10000)
                {
                    num7 = 246;
                    num8 *= 0.5f;
                }
                else if (npc.extraValue >= 100)
                {
                    num7 = 245;
                    num8 *= 0.75f;
                }

                if (Main.rand.Next((int)num8) == 0)
                {
                    npc.position += npc.netOffset;
                    int num9 = Dust.NewDust(npc.position, npc.width, npc.height, num7, 0f, 0f, 254, default(Color), 0.25f);
                    Main.dust[num9].velocity *= 0.1f;
                    npc.position -= npc.netOffset;
                }
            }

            for (int j = 0; j < 256; j++)
            {
                if (npc.immune[j] > 0)
                    npc.immune[j]--;
            }

            if (!npc.noGravity && !npc.noTileCollide)
            {
                int num10 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
                int num11 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
                if (WorldGen.InWorld(num10, num11) && Main.tile[num10, num11] == null)
                {
                    DDHelper.FieldReflection(npc.GetType(), "gravity", BindingFlags.NonPublic | BindingFlags.Static).SetValue(npc, 0);
                    npc.velocity.X = 0f;
                    npc.velocity.Y = 0f;
                }
            }

            if (!npc.noGravity || npc.Dnpc().Control > 0 || npc.Dnpc().NoMove || npc.Dnpc().Control2 > 0)
            {
                if (npc.realLife == -1 || (npc.realLife > 0 && Main.npc[npc.realLife].velocity != Vector2.Zero))
                {
                    float a = npc.gravity;
                    npc.velocity.Y += a;
                    if (npc.velocity.Y > npc.maxFallSpeed)
                    {
                        npc.velocity.Y = npc.maxFallSpeed;
                    }
                    if (npc.realLife > 0)
                    {
                        npc.velocity = Main.npc[npc.realLife].velocity;
                    }
                }
            }

            if (npc.velocity.X < 0.005 && npc.velocity.X > -0.005)
            {
                npc.velocity.X = 0f;
            }

            if (Main.netMode != 1 && npc.type != 37 && (npc.friendly || NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[npc.type]))
            {
                if (npc.townNPC)
                {
                    npc.CheckDrowning();
                }

                DDHelper.MethodReflection(npc.GetType(), "CheckLifeRegen", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
                o[0] = NPCID.Sets.AllNPCs;
                DDHelper.MethodReflection(npc.GetType(), "GetHurtByOtherNPCs", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, o);
            }

            if (Main.netMode != 1 && (npcsFoundForCheckActive[210] || npcsFoundForCheckActive[211]) && !NPCID.Sets.HurtingBees[npc.type])
            {
                o[0] = NPCID.Sets.HurtingBees;
                DDHelper.MethodReflection(npc.GetType(), "GetHurtByOtherNPCs", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, o);
            }
            if (npc.Dnpc().Stand > 0)
            {
                npc.Dnpc().Stand--;
            }
            if (!npc.noGravity && npc.velocity.Y > 0)
            {
                if (npc.Dnpc().Stand > 0)
                {
                    npc.velocity.Y = 0;
                    if (Main.player[npc.target].position.Y + Main.player[npc.target].height < npc.position.Y && npc.aiStyle == 3)
                    {
                        npc.velocity.Y = -6f;
                    }
                }
            }

            if (!npc.noTileCollide || npc.Dnpc().Control > 0 || npc.Dnpc().NoMove || npc.Dnpc().Control2 > 0)
            {
                if (npc.realLife > 0)
                {
                    if (Main.npc[npc.realLife].HasBuff(ModContent.BuffType<Fossil>()))
                    {
                        if (npc.velocity == Vector2.Zero)
                        {
                            Main.npc[npc.realLife].velocity = Vector2.Zero;
                        }
                        if (npc.realLife > 0 && Main.npc[npc.realLife].velocity == Vector2.Zero)
                        {
                            npc.velocity = Vector2.Zero;
                        }
                    }
                }
                DDHelper.MethodReflection(npc.GetType(), "UpdateCollision", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
                if (npc.velocity == Vector2.Zero)
                {
                    if (npc.realLife > 0)
                    {
                        Main.npc[npc.realLife].velocity = Vector2.Zero;
                    }
                }
                if (npc.realLife > 0 && Main.npc[npc.realLife].velocity == Vector2.Zero)
                {
                    npc.velocity = Vector2.Zero;
                }
            }
            else
            {
                npc.oldPosition = npc.position;
                npc.oldDirection = npc.direction;
                npc.position += npc.velocity * npc.Dnpc().MoveSpeed;
                if (npc.onFire && npc.boss && Main.netMode != 1 && Collision.WetCollision(npc.position, npc.width, npc.height))
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (npc.buffType[k] == 24)
                        {
                            npc.DelBuff(k);
                        }
                    }
                }
            }

            if (Main.netMode != 1 && !npc.noTileCollide && npc.lifeMax > 1 && Collision.SwitchTiles(npc.position, npc.width, npc.height, npc.oldPosition, 2) && (npc.type == 46 || npc.type == 148 || npc.type == 149 || npc.type == 303 || npc.type == 361 || npc.type == 362 || npc.type == 364 || npc.type == 366 || npc.type == 367 || (npc.type >= 442 && npc.type <= 448) || npc.type == 602 || npc.type == 608 || npc.type == 614))
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 400f;
                npc.ai[2] = 0f;
            }

            npc.FindFrame();
            DDHelper.MethodReflection(npc.GetType(), "UpdateNPC_UpdateTrails", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
            o[0] = i;
            DDHelper.MethodReflection(npc.GetType(), "UpdateNetworkCode", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, o);
            if (npc.active && npc.Dnpc().netUpdate)
            {
                if (Main.netMode == 2)
                {
                    DDmod.SyncData(DDType.NPCLife, npc.whoAmI, -1, -1);
                }
            }
            npc.CheckActive();
            npc.netUpdate = false;
            npc.Dnpc().netUpdate = false;
            npc.justHit = false;
        }
        public static string DamageDamage(long Damage)
        {
            Main.player[Main.myPlayer].checkDPSTime();
            string Text = Damage.ToString();
            if (ModContent.GetInstance<DDConfigClient>().DpsStreamline == DDConfigClient.Dps.InternationalStandard)
            {
                if (Damage >= 1000000000)
                {
                    if (Text.Length >= 9)
                    {
                        Text = Text.Substring(0, Text.Length - 9)
                                      + "."
                                      + Text.Substring(Text.Length - 9);
                        Text = Text.Substring(0, Text.Length - 8) + "B";
                    }
                }
                else if (Damage >= 1000000)
                {
                    if (Text.Length >= 6)
                    {
                        Text = Text.Substring(0, Text.Length - 6)
                                      + "."
                                      + Text.Substring(Text.Length - 6);
                        Text = Text.Substring(0, Text.Length - 5) + "M";
                    }
                }
                else if (Damage >= 1000)
                {
                    if (Text.Length >= 3)
                    {
                        Text = Text.Substring(0, Text.Length - 3)
                                      + "."
                                      + Text.Substring(Text.Length - 3);
                        Text = Text.Substring(0, Text.Length - 2) + "K";
                    }
                }
            }
            else
            if (ModContent.GetInstance<DDConfigClient>().DpsStreamline == DDConfigClient.Dps.ChineseConvention)
            {
                if (Damage >= 1000000000000)
                {
                    if (Text.Length >= 12)
                    {
                        Text = Text.Substring(0, Text.Length - 12)
                                      + "."
                                      + Text.Substring(Text.Length - 12);
                        Text = Text.Substring(0, Text.Length - 11) + "万亿";
                    }
                }
                else if (Damage >= 100000000)
                {
                    if (Text.Length >= 8)
                    {
                        Text = Text.Substring(0, Text.Length - 8)
                                      + "."
                                      + Text.Substring(Text.Length - 8);
                        Text = Text.Substring(0, Text.Length - 7) + "亿";
                    }
                }
                else if (Damage >= 10000)
                {
                    if (Text.Length >= 4)
                    {
                        Text = Text.Substring(0, Text.Length - 4)
                                      + "."
                                      + Text.Substring(Text.Length - 4);
                        Text = Text.Substring(0, Text.Length - 3) + "万";
                    }
                }
            }
            else
            if (ModContent.GetInstance<DDConfigClient>().DpsStreamline == DDConfigClient.Dps.PureChineseCharacters)
            {
                //整活
                if (Damage >= 1000000000000)
                {
                    Text = SWGlobalInfoDisplay.ReplaceNumbersWithChinese("" + Damage / 1000000000000) + "万亿";
                    string T = SWGlobalInfoDisplay.ReplaceNumbersWithChinese("" + (Damage % 1000000000000) / 100000000000);
                    if (Damage < 100000000000000 && T.Length >= 1 && T.Substring(0, 1) != "零" && T.Substring(0, 1) != "十")
                    {
                        Text += T.Substring(0, 1);
                    }
                }
                else if (Damage >= 100000000)
                {
                    Text = SWGlobalInfoDisplay.ReplaceNumbersWithChinese("" + Damage / 100000000) + "亿";
                    string T = SWGlobalInfoDisplay.ReplaceNumbersWithChinese("" + (Damage % 100000000) / 10000000);
                    if (Damage < 10000000000 && T.Length >= 1 && T.Substring(0, 1) != "零" && T.Substring(0, 1) != "十")
                    {
                        Text += T.Substring(0, 1);
                    }
                }
                else if (Damage >= 10000)
                {
                    Text = SWGlobalInfoDisplay.ReplaceNumbersWithChinese("" + Damage / 10000) + "万";
                    string T = SWGlobalInfoDisplay.ReplaceNumbersWithChinese("" + (Damage % 10000) / 1000);
                    if (Damage < 1000000 && T.Length >= 1 && T.Substring(0, 1) != "零" && T.Substring(0, 1) != "十")
                    {
                        Text += T.Substring(0, 1);
                    }
                }
                else
                {
                    Text = SWGlobalInfoDisplay.ReplaceNumbersWithChinese(Text);
                }
            }
            /*
            Text = Damage.ToString();
            if (Text.Length > 3)
            {
                int a = Text.Length %3;
                for (int A = 0; A < Damage.ToString().Length / 3; A++)
                {
                    Text = Text.Insert((a+3 * A) + A, ",");
                }
            }*/
            return Text;
        }
        //打击npc
        public static int NPC_StrikeNPC(Terraria.On_NPC.orig_StrikeNPC_HitInfo_bool_bool orig, NPC npc, HitInfo hit, bool fromNet = false, bool noPlayerInteraction = false)
        {
            bool flag = Main.netMode == 0;

            flag &= !noPlayerInteraction;

            if (!npc.active || npc.life <= 0)
            {
                return 0;
            }

            double num = hit.Damage;
            bool crit = hit.Crit;
            int hitDirection = hit.HitDirection;
            if (hit.InstantKill)
            {
                num = npc.realLife > 0 ? Main.npc[npc.realLife].life : npc.life;
            }

            if (!hit.HideCombatText && !hit.InstantKill && npc.lifeMax > 1 && !npc.HideStrikeDamage && !npc.Dnpc().NoDamage)
            {
                if (npc.friendly)
                {
                    Color color = (crit ? CombatText.DamagedFriendlyCrit : CombatText.DamagedFriendly);
                    string Damages = DamageDamage((long)num);
                    CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color, Damages, crit);
                }
                else
                {
                    Color color2 = (crit ? CombatText.DamagedHostileCrit : CombatText.DamagedHostile);
                    if (fromNet)
                    {
                        color2 = (crit ? CombatText.OthersDamagedHostileCrit : CombatText.OthersDamagedHostile);
                    }

                    string Damages = DamageDamage((long)num);

                    CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, Damages, crit);
                }
            }

            if (npc.realLife >= 0)
            {
                Main.npc[npc.realLife].NPCHB().damage += (int)num;
            }
            else
            {

                npc.NPCHB().damage += (int)num;
            }
            if (num >= 1.0)
            {
                if (flag)
                {
                    npc.PlayerInteraction(Main.myPlayer);
                }

                npc.justHit = true;
                if ((npc.type == 438 || npc.type == 379) && Main.netMode != 1)
                {
                    int num3 = (int)(0f - npc.ai[3] - 1f);
                    if (num3 > -1 && Main.npc[num3].localAI[0] == 0f)
                    {
                        Main.npc[num3].localAI[0] = 1f;
                    }
                }

                if (npc.townNPC)
                {
                    if (npc.aiStyle == 7 && (npc.ai[0] == 3f || npc.ai[0] == 4f || npc.ai[0] == 16f || npc.ai[0] == 17f))
                    {
                        NPC nPC = Main.npc[(int)npc.ai[2]];
                        if (nPC.active)
                        {
                            nPC.ai[0] = 1f;
                            nPC.ai[1] = 300 + Main.rand.Next(300);
                            nPC.ai[2] = 0f;
                            nPC.localAI[3] = 0f;
                            nPC.direction = hitDirection;
                            nPC.netUpdate = true;
                        }
                    }

                    npc.ai[0] = 1f;
                    npc.ai[1] = 300 + Main.rand.Next(300);
                    npc.ai[2] = 0f;
                    npc.localAI[3] = 0f;
                    npc.direction = hitDirection;
                    npc.netUpdate = true;
                }

                if (npc.aiStyle == 8 && Main.netMode != 1)
                {
                    if (npc.type == 172)
                    {
                        npc.ai[0] = 450f;
                    }
                    else if (npc.type == 283 || npc.type == 284)
                    {
                        if (Main.rand.NextBool(2))
                        {
                            npc.ai[0] = 390f;
                            npc.netUpdate = true;
                        }
                    }
                    else if (npc.type == 533)
                    {
                        if (!Main.rand.NextBool(3))
                        {
                            npc.ai[0] = 181f;
                            npc.netUpdate = true;
                        }
                    }
                    else
                    {
                        npc.ai[0] = 400f;
                    }

                    npc.TargetClosest();
                }

                if (npc.aiStyle == 97 && Main.netMode != 1)
                {
                    npc.localAI[1] = 1f;
                    npc.TargetClosest();
                }

                if (npc.type == 371)
                {
                    num = 0.0;
                    npc.ai[0] = 1f;
                    npc.ai[1] = 4f;
                    npc.dontTakeDamage = true;
                }

                if (npc.type == 346 && npc.life >= npc.lifeMax * 0.5 && npc.life - num < npc.lifeMax * 0.5)
                {
                    Gore.NewGore(npc.GetSource_Death(), npc.position, npc.velocity, 517);
                }

                if (npc.type == 184)
                {
                    npc.localAI[0] = 60f;
                }

                if (npc.type == 535)
                {
                    npc.localAI[0] = 60f;
                }

                if (npc.type == 185)
                {
                    npc.localAI[0] = 1f;
                }

                if (!npc.immortal)
                {
                    if (npc.realLife >= 0)
                    {
                        Main.npc[npc.realLife].life -= (int)num;
                        npc.life = Main.npc[npc.realLife].life;
                        npc.lifeMax = Main.npc[npc.realLife].lifeMax;
                    }
                    else
                    {
                        npc.life -= (int)num;
                    }
                }

                if (hit.Knockback > 0f)
                {
                    float num4 = hit.Knockback;
                    if (npc.onFire2)
                    {
                        num4 *= 1.1f;
                    }

                    if (num4 > 8f)
                    {
                        float num5 = num4 - 8f;
                        num5 *= 0.9f;
                        num4 = 8f + num5;
                    }

                    if (num4 > 10f)
                    {
                        float num6 = num4 - 10f;
                        num6 *= 0.8f;
                        num4 = 10f + num6;
                    }

                    if (num4 > 12f)
                    {
                        float num7 = num4 - 12f;
                        num7 *= 0.7f;
                        num4 = 12f + num7;
                    }

                    if (num4 > 14f)
                    {
                        float num8 = num4 - 14f;
                        num8 *= 0.6f;
                        num4 = 14f + num8;
                    }

                    if (num4 > 16f)
                    {
                        num4 = 16f;
                    }

                    if (crit)
                    {
                        num4 *= 1.4f;
                    }

                    int num9 = (int)num * 10;
                    if (Main.expertMode)
                    {
                        num9 = (int)num * 15;
                    }

                    if (num9 > npc.lifeMax)
                    {
                        if (hitDirection < 0 && npc.velocity.X > 0f - num4)
                        {
                            if (npc.velocity.X > 0f)
                            {
                                npc.velocity.X -= num4;
                            }

                            npc.velocity.X -= num4;
                            if (npc.velocity.X < 0f - num4)
                            {
                                npc.velocity.X = 0f - num4;
                            }
                        }
                        else if (hitDirection > 0 && npc.velocity.X < num4)
                        {
                            if (npc.velocity.X < 0f)
                            {
                                npc.velocity.X += num4;
                            }

                            npc.velocity.X += num4;
                            if (npc.velocity.X > num4)
                            {
                                npc.velocity.X = num4;
                            }
                        }

                        if (npc.type == 185)
                        {
                            num4 *= 1.5f;
                        }

                        num4 = (npc.noGravity ? (num4 * -0.5f) : (num4 * -0.75f));
                        if (npc.velocity.Y > num4)
                        {
                            npc.velocity.Y += num4;
                            if (npc.velocity.Y < num4)
                            {
                                npc.velocity.Y = num4;
                            }
                        }
                    }
                    else
                    {
                        if (!npc.noGravity)
                        {
                            npc.velocity.Y = (0f - num4) * 0.75f * npc.knockBackResist;
                        }
                        else
                        {
                            npc.velocity.Y = (0f - num4) * 0.5f * npc.knockBackResist;
                        }

                        npc.velocity.X = num4 * hitDirection * npc.knockBackResist;
                    }
                }

                if ((npc.type == 113 || npc.type == 114) && npc.life <= 0)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].active && (Main.npc[i].type == 113 || Main.npc[i].type == 114))
                        {
                            Main.npc[i].HitEffect(hitDirection, num, hit.InstantKill);
                        }
                    }
                }
                else
                {
                    npc.HitEffect(hit);
                }

                if (npc.HitSound != null)
                {
                    PlaySound(npc.HitSound, npc.position);
                }

                if (npc.realLife >= 0)
                {
                    Main.npc[npc.realLife].checkDead();
                }
                else
                {
                    npc.checkDead();
                }

                return (int)num;
            }

            return 0;
        }
        public static void NPC_ScaleStats(Terraria.On_NPC.orig_ScaleStats orig, NPC npc, int? activePlayersCount, GameModeData gameModeData, float? strengthOverride)
        {
            if (npc.TryGetGlobalNPC<DGlobalNPC>(out DGlobalNPC gnpc))
            {
                gnpc.Properties.SetLevel(npc);
                if (gnpc.LifeUP)
                {
                    if ((!NPCID.Sets.NeedsExpertScaling.IndexInRange(npc.type) || !NPCID.Sets.NeedsExpertScaling[npc.type]) && (npc.lifeMax <= 5 || npc.damage == 0 || npc.friendly || npc.townNPC))
                    {
                        if (npc.lifeMax > 5&& !npc.townNPC&& !npc.friendly)
                        {
                            DGlobalNPC.LIFE(npc);
                        }
                        return;
                    }

                    float num = 1f;
                    if (strengthOverride.HasValue)
                    {
                        num = strengthOverride.Value;
                    }
                    else if (gameModeData.IsJourneyMode)
                    {
                        CreativePowers.DifficultySliderPower power = CreativePowerManager.Instance.GetPower<CreativePowers.DifficultySliderPower>();
                        if (power != null && power.GetIsUnlocked())
                        {
                            num = power.StrengthMultiplierToGiveNPCs;
                        }
                    }

                    float num2 = num;
                    if (gameModeData.IsJourneyMode && Main.getGoodWorld)
                    {
                        num += 1f;
                    }

                    NPCStrengthHelper nPCStrengthHelper = new NPCStrengthHelper(gameModeData, num, Main.getGoodWorld);
                    if (nPCStrengthHelper.IsExpertMode)
                    {
                        bool flag = npc.type >= 0 && NPCID.Sets.ProjectileNPC[npc.type];
                        bool flag2 = !NPCID.Sets.DontDoHardmodeScaling[npc.type];

                        if (flag2 && Main.hardMode && !npc.boss && npc.lifeMax < 1000)
                        {
                            int V = npc.damage + npc.defense + npc.lifeMax / 4;
                            if (V == 0)
                                V = 1;

                            int V2 = 80;
                            if (downedPlantBoss)
                                V2 += 20;

                            if (V < V2)
                            {
                                if(downedPlantBoss)
                                {
                                    if(npc.Dnpc().Properties.Level==0)
                                    {
                                        npc.Dnpc().Properties.Level = 13;
                                    }
                                    if(npc.Dnpc().Properties.Level==1)
                                    {
                                        npc.Dnpc().Properties.Level = 13;
                                    }
                                }
                                else if (Main.hardMode)
                                {
                                    if (npc.Dnpc().Properties.Level == 0)
                                    {
                                        npc.Dnpc().Properties.Level = 7;
                                    }
                                    if (npc.Dnpc().Properties.Level == 1)
                                    {
                                        npc.Dnpc().Properties.Level = 8;
                                    }
                                }
                            }
                        }

                        DDHelper.MethodReflection(npc.GetType(), "ScaleStats_ApplyExpertTweaks", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, null);
                    }

                    npc.GetGlobalNPC<DGlobalNPCExp>().GetExp(npc);
                    DDHelper.MethodReflection(npc.GetType(), "ScaleStats_ApplyGameMode", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, [gameModeData]);
                    if (Main.getGoodWorld && nPCStrengthHelper.ExtraDamageForGetGoodWorld)
                    {
                        npc.damage += npc.damage / 3;
                    }

                    if (nPCStrengthHelper.IsExpertMode)
                    {
                        int num3 = 1;
                        num3 = (npc.statsAreScaledForThisManyPlayers = ((!activePlayersCount.HasValue) ? GetActivePlayerCount() : activePlayersCount.Value));
                        GetStatScalingFactors(num3, out float balance, out float boost);
                        float bossAdjustment = 1f;
                        if (nPCStrengthHelper.IsMasterMode)
                        {
                            bossAdjustment = 0.85f;
                        }

                        DDHelper.MethodReflection(npc.GetType(), "ScaleStats_ApplyMultiplayerStats", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, [num3, balance, boost, bossAdjustment]);
                    }

                    npc.ScaleStats_UseStrengthMultiplier(num);
                    npc.strengthMultiplier = num2;
                    if ((npc.type < 0 || !NPCID.Sets.ProjectileNPC[npc.type]) && npc.lifeMax < 6)
                    {
                        npc.lifeMax = 6;
                    }

                    npc.life = npc.lifeMax;
                    npc.defDamage = npc.damage;
                    npc.defDefense = npc.defense;
                }
                else
                {
                    npc.GetGlobalNPC<DGlobalNPCExp>().GetExp(npc);
                }
                DGlobalNPC.LIFE(npc);
            }
        }
        public static void NPC_NPCLoot(Terraria.On_NPC.orig_NPCLoot orig, NPC npc)
        {
            orig(npc);
            if (npc.type == 23 && Main.hardMode)
            {
                Player closestPlayer = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];
                if (!NPCLoader.PreKill(npc))
                {
                    return;
                }

                DDHelper.MethodReflection(npc.GetType(), "DoDeathEvents_BeforeLoot", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { closestPlayer });
                DDHelper.MethodReflection(npc.GetType(), "NPCLoot_DropItems", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { closestPlayer });
                NPCLoader.OnKill(npc);
                DDHelper.MethodReflection(npc.GetType(), "DoDeathEvents", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { closestPlayer });
                DDHelper.MethodReflection(npc.GetType(), "NPCLoot_DropMoney", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { closestPlayer });
                DDHelper.MethodReflection(npc.GetType(), "NPCLoot_DropHeals", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(npc, new object[] { closestPlayer });
            }
        }
        public static void NPC_VanillaHitEffect(Terraria.On_NPC.orig_VanillaHitEffect orig, NPC npc, int I, double G, bool K)
        {
            if (npc.type > 0 && npc.active && !npc.Dnpc().Deathrattle)
            {
                orig(npc, I, G, K);
            }

        }
        public static void NPC_HitEffect(Terraria.On_NPC.orig_HitEffect_HitInfo orig, NPC npc, HitInfo hit)
        {
            if (npc.type > 0 && npc.active && npc.Dnpc().Deathrattle && npc.life <= 0)
            {
                NPCLoader.HitEffect(npc, hit);
            }
            else
            {
                orig(npc, hit);
            }
        }
        //禁止弹气泡
        private static void DrawNPCChatBubble(Terraria.On_Main.orig_DrawNPCChatBubble orig, int i)
        {
            if (Main.npc[i].type != ModContent.NPCType<HunterSlime>())
            {
                orig(i);
            }
        }
    }
}