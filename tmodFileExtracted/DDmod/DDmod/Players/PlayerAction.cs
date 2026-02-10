
using DDmod.Content.Items;
using DDmod.Content.Items.农场.水壶;
using DDmod.Content.Projectiles;
using DDmod.Modkey;
using DDmod.NoContent.Config;
using System.Reflection;
using Terraria;
using tModPorter;
using static Terraria.Player;

namespace DDmod.Players
{
    public static class PH
    {
        public static PlayerAction PlayerAction(this Player player)
        {
            return player.GetModPlayer<PlayerAction>();
        }
    }
    public class PlayerAction : ModPlayer
    {
        public static void PlayerFrame(Terraria.On_Player.orig_PlayerFrame orig, Player player)
        {
            //游戏暂停时不动
            if (!Main.gamePaused&&player.PlayerAction().Action)
            {
                if(player.Dplayer().ProjAnimation)
                {
                    //player.itemAnimation = 10;
                }
                if (!player.PlayerAction().ShowWeapons)
                {
                    player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                }
                //不许转动
                bool RO = false;
                if (player.Dplayer().Realm > 0 || player.pulley || (player.ActiveItem().type>0 && player.ActiveItem().holdStyle != 0))
                {
                    RO = true;
                }
                //不使用手臂动作
                bool ArmAnimation = true;
                if (player.Dplayer().Realm > 0 || player.pulley || (player.ActiveItem().type > 0 && player.ActiveItem().holdStyle != 0))
                {
                    ArmAnimation = false;
                    player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                }
                bool Speed = player.Dplayer().Speed;
                bool MaxSpeed = player.Dplayer().MaxSpeed;

                //翻转
                if (player.itemAnimation == 0 && !player.sleeping.isSleeping && !player.sitting.isSitting && player.PlayerAction().headRotation && !player.pulley && player.ActiveItem().holdStyle == 0)
                {
                    player.ChangeDir(player.Dplayer().MouseWorld.X - player.Center.X > 0 ? 1 : -1);
                }
                Texture2D texture = null;
                if (Main.netMode != 2)
                {
                   texture = TextureAssets.Item[player.inventory[player.selectedItem].type].Value;
                }
                float WeaponsSize =0;
                //大型武器改变走路
                if (texture != null)
                {
                    float frame = (Main.itemAnimations[player.ActiveItem().type] == null) ? 1 : Main.itemAnimations[player.ActiveItem().type].FrameCount;
                    WeaponsSize = new Vector2(texture.Width, texture.Height / frame).Length() * player.GetAdjustedItemScale(player.ActiveItem());
                    if (player.ActiveItem().type > 0&&player.ActiveItem().DItem().Twin)
                    {
                        WeaponsSize = new Vector2(texture.Width / 2, texture.Height).Length() * player.GetAdjustedItemScale(player.ActiveItem());
                    }
                }
                if (player.ActiveItem().type == 946 || player.ActiveItem().type == 4707)
                {
                    if (player.velocity.Y != 0)
                    {
                        if (player.PlayerAction().MoveEffects)
                        {
                            float velocity = player.velocity.X * -0.1F;
                            if (velocity < -1)
                            {
                                velocity = -1;
                            }
                            else if (velocity > 1F)
                            {
                                velocity = 1;
                            }
                            player.RotationSpeed(velocity, 0.05f);
                        }
                    }
                    else
                    {
                        player.RotationSpeed(0, 0.05f);
                    }
                }
                int X = (int)(player.position.X / 16);
                int X2 = X + (int)(player.width / 16);
                //坐骑
                if (!player.mount.Active && !player.mount.Cart&&!player.pulley&& ArmAnimation)
                {
                    //绳子
                    if ((Main.tile[X2 - 1, (int)((player.position.Y + player.height) / 16)].TileType == 213 && Main.tile[X2, (int)((player.position.Y + player.height) / 16)].TileType == 213) ||
                    (Main.tile[X2 - 1, (int)((player.position.Y + player.height) / 16)].TileType == 214 && Main.tile[X2, (int)((player.position.Y + player.height) / 16)].TileType == 214) ||
                    (Main.tile[X2 - 1, (int)((player.position.Y + player.height) / 16)].TileType == 353 && Main.tile[X2, (int)((player.position.Y + player.height) / 16)].TileType == 353) ||
                    (Main.tile[X2 - 1, (int)((player.position.Y + player.height) / 16)].TileType == 365 && Main.tile[X2, (int)((player.position.Y + player.height) / 16)].TileType == 365) ||
                    (Main.tile[X2 - 1, (int)((player.position.Y + player.height) / 16)].TileType == 356 && Main.tile[X2, (int)((player.position.Y + player.height) / 16)].TileType == 366))
                    {
                        if (!player.controlJump && !player.controlDown && (player.itemAnimation == 0)&&(player.ActiveItem().type==0|| player.ActiveItem().GetGlobalItem<水壶>().kettle) && !player.PlayerAction().EnableArmRotation)
                        {
                            player.RotationSpeed(0, 0.05f);
                            player.position.Y = (int)((player.position.Y + player.height) / 16 - 2) * 16 - 4;
                            player.SetCompositeArmFront(true, 0, MathHelper.PiOver2 * player.direction + player.Aplayer().ArmSwing);
                            player.SetCompositeArmBack(true, 0, -MathHelper.PiOver2 * player.direction + player.Aplayer().ArmSwing);
                            if (player.Aplayer().ArmSwing < 0.03F && !player.Aplayer().ArmSwingBegin)
                            {
                                player.Aplayer().ArmSwingMax = Main.rand.NextFloat(-0.3F, 0.3F);
                                player.Aplayer().ArmSwingBegin = true;
                            }
                            if (player.Aplayer().ArmSwingBegin)
                            {
                                if (player.Aplayer().ArmSwing < player.Aplayer().ArmSwingMax)
                                {
                                    player.Aplayer().ArmSwing += 0.01F;
                                }
                                else
                                {
                                    player.Aplayer().ArmSwingBegin = false;
                                }
                            }
                            else
                            {
                                player.Aplayer().ArmSwing -= 0.01F;
                            }
                            player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                            player.Aplayer().Stand = 2;
                        }
                    }
                    else
                    //走路
                    if (Math.Abs(player.velocity.X) > 0 && !Speed && player.velocity.Y == 0 && !player.PlayerAction().SwimStatus)
                    {
                        //玩家准备拿着武器放在背后
                        if (player.PlayerAction().WeaponType == HandheldWeaponType.preBack)
                        {
                            if (player.itemAnimation == 0)
                                player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing * player.direction);
                            float SP = 0.1F;
                            DDHelper.BackAndForth(0.1F, 4.4F, SP, ref player.Aplayer().ArmSwing, ref player.Aplayer().ArmSwingBegin);
                            if (player.Aplayer().ArmSwing > 4F)
                            {
                                //放在背后
                                player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                            }
                        }
                        else
                        {
                            if (player.itemAnimation == 0)
                            {
                                player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing * player.direction);
                                if (player.ActiveItem().type > 0 && player.ActiveItem().GetGlobalItem<水壶>().kettle)
                                    player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -1.4f * player.direction);
                            }
                            if (!player.PlayerAction().ThereShield)
                            {
                                player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, player.Aplayer().ArmSwing * player.direction);
                            }
                            float SP = Math.Abs(player.velocity.X) / 20;
                            float Swing = 1f;
                            //手持物品时放慢速度
                            if (player.ActiveItem().type != 0 && player.PlayerAction().WeaponType == 0 && DDPlayer.UseBoomerang(player.ActiveItem(), player))
                            {
                                SP /= 4;
                                Swing /= 4;
                                if (!player.PlayerAction().ThereShield)
                                {
                                    if (player.ActiveItem().type > 0 && !player.ActiveItem().DItem().Twin)
                                    {
                                        player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, player.Aplayer().ArmSwing * 4 * player.direction);
                                    }
                                    else
                                    {
                                        player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, player.Aplayer().ArmSwing * player.direction);
                                    }
                                }
                            }
                            else
                            {
                                if (!player.PlayerAction().ThereShield)
                                    player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, player.Aplayer().ArmSwing * player.direction);
                            }
                            DDHelper.BackAndForth(-Swing, Swing, SP, ref player.Aplayer().ArmSwing, ref player.Aplayer().ArmSwingBegin);
                        }
                        //忍者跑
                        /**/
                    }
                    //跑步
                    else if (Speed && !player.PlayerAction().SwimStatus)
                    {

                        if(player.itemAnimation==0&&!player.Dplayer().ProjAnimation)
                           player.ChangeDir(player.velocity.X > 0 ? 1 : -1);
                        player.Dplayer().ProjAnimation = false;
                        //player.fullRotation = player.velocity.X * 0.05f;
                        //忍者套
                        if (player.Aplayer().Ninja)
                        {
                            //忍者跑步手放在后面
                            //player.Aplayer().ArmSwing = MathHelper.PiOver2 * player.direction - player.direction;
                            if (player.itemAnimation == 0)
                            {
                                player.SetCompositeArmFront(true, 0, MathHelper.PiOver2 * player.direction - 1f * player.direction);
                                if (player.ActiveItem().type > 0 && player.ActiveItem().GetGlobalItem<水壶>().kettle)
                                    player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -1.4f * player.direction);
                            }
                            if (!player.PlayerAction().ThereShield)
                                player.SetCompositeArmBack(true, 0, MathHelper.PiOver2 * player.direction - 1f * player.direction);
                        }
                        else
                        {
                            //检测手持模式
                            if (player.PlayerAction().WeaponType == 0 || player.PlayerAction().WeaponType == HandheldWeaponType.Back)
                            {
                                //如果是大型贴图就拖着走
                                if (WeaponsSize > 100 && player.PlayerAction().WeaponType == 0)
                                {
                                    //手放后面拖着武器走
                                    if (player.itemAnimation == 0 && player.ActiveItem().type > 0)
                                        player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, 1 * player.direction);
                                }
                                else
                                {
                                    //正常走路
                                    if (player.itemAnimation == 0 && player.ActiveItem().type > 0)
                                    {
                                        player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing * player.direction);
                                        if (player.ActiveItem().type > 0 && player.ActiveItem().GetGlobalItem<水壶>().kettle)
                                            player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -1.4f * player.direction);
                                    }
                                }
                                //如果玩家不在空中
                                if (player.velocity.Y == 0)
                                {
                                    float SP = Math.Abs(player.velocity.X) / 20;
                                    float Swing = 1.8f;
                                    //手持物品时放慢拿武器的手
                                    if (player.ActiveItem().type != 0 && player.PlayerAction().WeaponType == 0 && DDPlayer.UseBoomerang(player.ActiveItem(), player))
                                    {
                                        SP /= 4;
                                        Swing /= 4;
                                        if (!player.PlayerAction().ThereShield)
                                        {
                                            if (player.ActiveItem().type > 0 && !player.ActiveItem().DItem().Twin)
                                            {
                                                player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, player.Aplayer().ArmSwing * 4 * player.direction);
                                            }
                                            else
                                            {
                                                player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, player.Aplayer().ArmSwing * player.direction);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!player.PlayerAction().ThereShield && player.ActiveItem().type > 0)
                                            player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, player.Aplayer().ArmSwing * player.direction);
                                    }
                                    //手臂摆动
                                    DDHelper.BackAndForth(-Swing, Swing, SP, ref player.Aplayer().ArmSwing, ref player.Aplayer().ArmSwingBegin);
                                }
                                else
                                {
                                    player.Aplayer().ArmSwing = -0.2f;
                                }
                            }
                            //玩家准备拿着武器放在背后
                            if (player.PlayerAction().WeaponType == HandheldWeaponType.preBack)
                            {
                                if (player.itemAnimation == 0)
                                    player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing * player.direction);
                                float SP = 0.1F;
                                DDHelper.BackAndForth(0.1F, 4.4F, SP, ref player.Aplayer().ArmSwing, ref player.Aplayer().ArmSwingBegin);
                                if (player.Aplayer().ArmSwing > 4F)
                                {
                                    //放在背后
                                    player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                                }
                            }
                        }
                    }
                    //站着不动
                    else if (player.itemAnimation == 0 && player.PlayerAction().WeaponType != HandheldWeaponType.Back)
                    {
                        //游泳把武器放在背后
                        if ((player.PlayerAction().SwimStatus && player.PlayerAction().WeaponType < HandheldWeaponType.Back))
                        {
                            player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                        }
                        //启动展示物品
                        if (player.PlayerAction().ShowKey && !player.PlayerAction().Display)
                        {
                            player.PlayerAction().Display = true;
                            player.PlayerAction().DisplayTime = 0;
                            player.PlayerAction().ShowKey = false;
                        }
                        //展示物品
                        if (player.PlayerAction().Display)
                        {
                            player.PlayerAction().WeaponTypeTimes = 0;
                            player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing * player.direction);
                            if (player.ActiveItem().type>0&& player.ActiveItem().DItem().Twin&& !player.PlayerAction().ThereShield)
                            {
                                player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing / 2 * player.direction);
                            }
                            float SP = 0.3F;
                            DDHelper.BackAndForth(-0.1F, 3F, SP, ref player.Aplayer().ArmSwing, ref player.Aplayer().ArmSwingBegin);
                            if (player.Aplayer().ArmSwing >= 2.5F)
                            {
                                if (player.PlayerAction().DisplayTime == 0)
                                {
                                    if (player.ActiveItem().type > 0)
                                    {
                                        CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), new Color(255, 255, 0), (DDSystem.English?"I have":"我有") + player.inventory[player.selectedItem].stack + (DDSystem.English ? "" : "个:") + player.inventory[player.selectedItem].AffixName(), false, false);
                                    }
                                    else
                                    {
                                        CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), new Color(255, 255, 0), DDSystem.English ? "I don't have anything on my hands" : "我手上什么都没有", false, false);
                                    }
                                }
                                if (player.PlayerAction().DisplayTime < 60)
                                {
                                    player.Aplayer().ArmSwing = 2.5F;
                                    player.PlayerAction().DisplayTime++;
                                    player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing * player.direction);
                                    if (player.ActiveItem().type >0&& player.ActiveItem().DItem().Twin && !player.PlayerAction().ThereShield)
                                    {
                                        player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing/2 * player.direction);
                                    }
                                }
                                else
                                {
                                    player.PlayerAction().Display = false;
                                    player.PlayerAction().WeaponType = 0;
                                }
                            }
                        }
                        //玩家手持武器
                        else if (player.PlayerAction().WeaponType == 0)
                        {
                            //武器大一点才会放在背后
                            if (WeaponsSize > 50)
                            {
                                player.PlayerAction().WeaponTypeTimes++;
                            }
                            else
                            {
                                player.PlayerAction().WeaponTypeTimes = 0;
                                player.PlayerAction().WeaponType = 0;
                            }
                            if (player.PlayerAction().WeaponTypeTimes > 600)
                            {
                                player.PlayerAction().WeaponType = HandheldWeaponType.preBack;
                            }
                            if (player.ActiveItem().type > 0)
                            {
                                player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing * player.direction);
                                if (player.ActiveItem().type > 0 && player.ActiveItem().GetGlobalItem<水壶>().kettle)
                                    player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -1.4f * player.direction);
                                float SP = 0.003F;
                                float Swing = 0.1f;
                                DDHelper.BackAndForth(-Swing, Swing, SP, ref player.Aplayer().ArmSwing, ref player.Aplayer().ArmSwingBegin);
                                if (!player.PlayerAction().ThereShield)
                                {
                                    if (player.ActiveItem().type>0&&player.ActiveItem().DItem().Twin)
                                    {
                                        player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, player.Aplayer().ArmSwing* player.direction);
                                    }
                                }
                            }
                        }
                        else if (player.PlayerAction().WeaponType == HandheldWeaponType.preBack)
                        {
                            if (WeaponsSize <= 50)
                            {
                                player.PlayerAction().WeaponType = 0;
                            }
                            player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing * player.direction);
                            if (player.ActiveItem().type > 0 && player.ActiveItem().DItem().Twin)
                            {
                                player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, player.Aplayer().ArmSwing * player.direction);
                            }
                            float SP = 0.1F;
                            DDHelper.BackAndForth(0.1F, 4.4F, SP, ref player.Aplayer().ArmSwing, ref player.Aplayer().ArmSwingBegin);
                            if (player.Aplayer().ArmSwing > 4F)
                            {
                                player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                            }
                        }
                    }
                    //手上没拿物品
                    else
                    {
                        //武器大一点才会放在背后
                        if (WeaponsSize <= 50)
                        {
                            player.PlayerAction().WeaponType = 0;
                        }
                        if (player.itemAnimation == 0)
                        {
                            if (player.PlayerAction().ShowKey)
                            {
                                player.PlayerAction().Display = true;
                                player.PlayerAction().DisplayTime = 0;
                                player.PlayerAction().WeaponType = 0;
                                player.PlayerAction().ShowKey = false;
                            }
                        }
                        player.Aplayer().ArmSwing = 0;
                    }
                    if (player.sleeping.isSleeping)
                    {
                        player.PlayerAction().WeaponType = HandheldWeaponType.Ground;
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<PlayerItem>()] == 0 && player.inventory[player.selectedItem].damage > 0 && player.inventory[player.selectedItem].useStyle != 0)
                        {
                            NewProjectile(player.GetSource_FromAI(), player.Center, new Vector2(Main.rand.NextFloat(0.3f, 0.5f) * player.direction, Main.rand.NextFloat(-8, -5)), ModContent.ProjectileType<PlayerItem>(), 0, 0, player.whoAmI);
                        }
                    }
                    else if (player.PlayerAction().WeaponType == HandheldWeaponType.Ground)
                    {
                        player.PlayerAction().WeaponType = HandheldWeaponType.preBack;
                    }
                    if (player.itemAnimation != 0 || player.velocity.Length() != 0)
                    {
                        player.PlayerAction().Display = false;
                    }
                    //跳跃翻转
                    if (!player.PlayerAction().SwimStatus && player.Aplayer().Ninja && player.Aplayer().NinjaJump && !RO && !player.PlayerAction().EnableArmRotation&&(player.wingTimeMax==0||player.velocity.Y==0||(player.wingTime==0&&!player.controlJump)))
                    {
                        //手持大型武器不翻转
                        if (WeaponsSize <= 100&& !(player.ActiveItem().type > 0 && player.ActiveItem().GetGlobalItem<水壶>().kettle))
                        {
                            if (((player.velocity.Y != 0 && player.velocity.Length() > 0.1F) || MaxSpeed) && player.itemAnimation == 0)
                            {
                                player.fullRotation += 0.5f * player.direction;
                            }
                            else if (player.velocity.Y != 0)
                            {
                                player.fullRotation = player.velocity.X * 0.05F;
                            }
                        }
                    }
                    if(player.wingTimeMax != 0 && player.velocity.Y!=0)
                    {
                        //if (player.itemAnimation == 0 && !(player.ActiveItem().type>0 && player.ActiveItem().GetGlobalItem<RangedGlobalItem>().Bow && player.heldProj != -1))
                            //player.ChangeDir(player.velocity.X > 0 ? 1 : -1);
                        //player.fullRotation = Math.Abs(player.velocity.X) * 0.2F*player.direction;
                        //DDHelper.MaxandMinF(ref player.fullRotation, 0.8f, -0.8f);
                    }
                    player.Aplayer().Ninja = false;
                }
                else
                {
                    /*
                    if (player.PlayerAction().WeaponType == 0&&player.itemAnimation==0)
                    {
                        player.PlayerAction().WeaponType = HandheldWeaponType.preBack;
                        if(player.pulley)
                        {
                            player.PlayerAction().WeaponType = HandheldWeaponType.Back;

                        }
                    }
                    else*/
                    if (player.PlayerAction().WeaponType == HandheldWeaponType.preBack)
                    {
                        player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing * player.direction);
                        float SP = 0.1F;
                        DDHelper.BackAndForth(0.1F, 4.4F, SP, ref player.Aplayer().ArmSwing, ref player.Aplayer().ArmSwingBegin);
                        if (player.Aplayer().ArmSwing > 4F)
                        {
                            player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                        }
                    }
                    else if (player.PlayerAction().WeaponType == HandheldWeaponType.Hand)
                    {
                        if (player.ActiveItem().type > 0&& player.PlayerAction().WeaponType == 0 && player.itemAnimation == 0)
                        {
                            player.SetCompositeArmFront(true, (CompositeArmStretchAmount)0, -player.Aplayer().ArmSwing * player.direction);
                            player.SetCompositeArmBack(true, (CompositeArmStretchAmount)0, player.Aplayer().ArmSwing * player.direction);
                            float SP = 0.003F;
                            DDHelper.BackAndForth(-0.1F, 0.1F, SP, ref player.Aplayer().ArmSwing, ref player.Aplayer().ArmSwingBegin);
                        }
                    }
                }
            }
            if((player.ActiveItem().type > 0 && player.Dplayer().ProjAnimation))
            {
                player.PlayerAction().WeaponType = HandheldWeaponType.Back;
            }
            //芦苇
            if (player.ActiveItem().type == 186 && player.itemAnimation == 0)
            {
                player.PlayerAction().PlayerArmRotation(-1*player.direction * player.gravDir - MathHelper.PiOver2 * player.direction, 0);
            }
            orig(player);
            //坐骑
            if (!player.mount.Active && !player.mount.Cart)
            {
                //游泳
                if (player.PlayerAction().SwimStatus && player.itemAnimation == 0&&player.ActiveItem().holdStyle==0)
                {
                    player.ChangeDir(player.velocity.X > 0 ? 1 : -1);
                    if (player.controlUp || player.controlDown || player.controlLeft || player.controlRight)
                    {
                        player.SetCompositeArmFront(true, (CompositeArmStretchAmount)3, (MathHelper.PiOver2 * player.direction) + player.Dplayer().PlayerTimes / 6 * player.direction);
                        if (!player.PlayerAction().ThereShield)
                            player.SetCompositeArmBack(true, (CompositeArmStretchAmount)3, -MathHelper.PiOver2 * player.direction + player.Dplayer().PlayerTimes / 6 * player.direction);
                    }
                    else
                    {
                            player.SetCompositeArmFront(true, (CompositeArmStretchAmount)3, 0);
                        if (!player.PlayerAction().ThereShield)
                            player.SetCompositeArmBack(true, (CompositeArmStretchAmount)3, 0);
                    }
                }
            }
            //前面的手旋转
            if (player.PlayerAction().EnableArmRotation)
            {
                player.SetCompositeArmFront(player.PlayerAction().EnableArmRotation, player.PlayerAction().amount, player.PlayerAction().ArmRotation);

                player.PlayerAction().EnableArmRotation = false;
            }
            //后面的手旋转
            if (player.PlayerAction().EnableArmRotationBack)
            {
                if (!player.PlayerAction().ThereShield)
                    player.SetCompositeArmBack(player.PlayerAction().EnableArmRotationBack, player.PlayerAction().amountBack, player.PlayerAction().ArmRotationBack);

                player.PlayerAction().EnableArmRotationBack = false;
            }
            player.PlayerAction().ThereShield = false;
            //player.RotationSpeed(player.fullRotation, 0f);

            if (player.Aplayer().NoGravity > 0)
            {
                if (player.Dplayer().Realm > 0)
                {
                    player.legFrame.Y = 0;
                    player.legFrameCounter = 0;
                }
            }
        }
        public void PlayerArmRotation(float Rotation, CompositeArmStretchAmount StretchAmount)
        {
            EnableArmRotation = true;
            ArmRotation = Rotation;
            amount = StretchAmount;
        }
        public void PlayerArmRotationBack(float Rotation, CompositeArmStretchAmount StretchAmount)
        {
            EnableArmRotationBack = true;
            ArmRotationBack = Rotation;
            amountBack = StretchAmount;
        }
        /// <summary> 启用手臂旋转 </summary>
        public bool EnableArmRotation;
        /// <summary> 手臂旋转 </summary>
        public float ArmRotation;
        /// <summary> 旋转样式 </summary>
        public CompositeArmStretchAmount amount;
        /// <summary> 启用手臂旋转 </summary>
        public bool EnableArmRotationBack;
        /// <summary> 手臂旋转 </summary>
        public float ArmRotationBack;
        /// <summary> 旋转样式 </summary>
        public CompositeArmStretchAmount amountBack;
        /// <summary> 手持武器 </summary>
        public HandheldWeaponType WeaponType;
        /// <summary> 手持武器计时器 </summary>
        public int WeaponTypeTimes;
        /// <summary> 展示物品 </summary>
        public bool Display;
        /// <summary> 展示物品时间 </summary>
        public int DisplayTime;
        /// <summary> 展示按键 </summary>
        public bool ShowKey;
        /// <summary> 展示武器 </summary>
        public bool ShowWeapons;
        /// <summary> 有盾牌 </summary>
        public bool ThereShield;
        public bool ThereShield2;
        /// <summary> 投掷 </summary>
        public int Throwing;
        /// <summary> 投掷旋转时间 </summary>
        public int ThrowingTime;
        /// <summary> 投掷旋转速度 </summary>
        public float ThrowingSpeed;
        /// <summary> 投掷旋转 </summary>
        public float ThrowingRotation;
        /// <summary> 投掷最大时间,投掷旋转时间,投掷旋转速度,当前手臂 </summary>
        public void ThrowingProj(int Time,int ROTime,float Speed,float Rotation)
        {
            Throwing = Time;
            ThrowingTime = ROTime;
            ThrowingSpeed = Speed;
            ThrowingRotation = Rotation;
        }
        //同步
        public static void PlayerConfig(Mod mod, BinaryReader reader)
        {
            byte player = reader.ReadByte();
            PlayerAction playerAction = Main.player[player].PlayerAction(); 
            BitsByte actionFlags = reader.ReadByte();
            playerAction.headRotation = actionFlags[0];
            playerAction.Somersault = actionFlags[1];
            playerAction.Swim = actionFlags[2];
            playerAction.Action = actionFlags[3];
            playerAction.MoveEffects = actionFlags[4];
            playerAction.ShowKey = actionFlags[5];
            playerAction.ShowWeapons = actionFlags[6];
            for (byte a=0;a<255;a++)
            {
                Player player1 = Main.player[a];
                if(player1.active&&a!= player)
                {
                    player1.Dplayer().Config = true;
                }
            }
            //用服务器发
            if (Main.netMode == NetmodeID.Server)
            {
                DDmod.SyncData(DDType.Config, player, -1, player);
            }
        }
        /// <summary> 打拳 </summary>
        public int R;
        public override void PreUpdate()
        {
            if (Player.ActiveItem().type == 0&&!Player.dead&& Player.itemAnimation==0)
            {
                R--;
                PickTime--;
                if (Player.controlUseItem && R < -8)
                {
                    if (R < 0)
                    {
                        R = 8;
                    }
                }
                float v = MathHelper.PiOver2 + MathHelper.Pi;
                if (Player.controlUseItem)
                {
                    bool 双手 = true;
                    if (Math.Abs(Main.MouseWorld.X / 16 - Player.Center.X / 16) < 3 + Player.blockRange && Math.Abs(Main.MouseWorld.Y / 16 - Player.Center.Y / 16) < 3 + Player.blockRange )
                    {
                        if (Main.tile[(int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16].HasTile)
                        {
                            if (PickTime <= 0)
                            {
                                Player.PickTile((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, 3);
                                Player.itemAnimation = 30;
                                Player.itemTime = 30;
                                PickTime = 8;
                            }
                            双手 = false;
                        }
                    }
                    for (int a = 0; a < 200; a++)
                    {
                        NPC npc = Main.npc[a];
                        if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                        {
                            Vector2 vector = Player.MountedCenter + (Player.Dplayer().MouseWorld - Player.MountedCenter).PerfectNormalize() * 16;
                            if (npc.getRect().Intersects(new Rectangle((int)vector.X - 8, (int)vector.Y - 8, 16, 16)))
                            {
                                if (npc.immune[Player.whoAmI] <= 0)
                                {
                                    npc.SimpleStrikeNPC(5, Player.direction , false,1, DamageClass.Melee);
                                    npc.immune[Player.whoAmI] = 8;
                                }
                                双手 = true;
                            }
                        }
                    }
                    Player.PlayerAction().PlayerArmRotation((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.None);
                    if(双手)
                    Player.PlayerAction().PlayerArmRotationBack((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.None);
                    if (R > 6)
                    {
                        Player.PlayerAction().PlayerArmRotation((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.None);
                        if (双手)
                            Player.PlayerAction().PlayerArmRotationBack((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.None);
                    }
                    else if (R > 4)
                    {
                        Player.PlayerAction().PlayerArmRotation((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.Quarter);
                        if (双手)
                            Player.PlayerAction().PlayerArmRotationBack((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.None);
                    }
                    else if (R > 2)
                    {
                        Player.PlayerAction().PlayerArmRotation((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.ThreeQuarters);
                        if (双手)
                            Player.PlayerAction().PlayerArmRotationBack((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.None);
                    }
                    else if (R > 0)
                    {
                        Player.PlayerAction().PlayerArmRotation((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.Full);
                        if (双手)
                            Player.PlayerAction().PlayerArmRotationBack((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.None);
                    }
                    else if (R > -2)
                    {
                        Player.PlayerAction().PlayerArmRotation((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.ThreeQuarters);
                        if (双手)
                            Player.PlayerAction().PlayerArmRotationBack((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.None);
                    }
                    else if (R > -4)
                    {
                        Player.PlayerAction().PlayerArmRotation((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.Quarter);
                        if (双手)
                            Player.PlayerAction().PlayerArmRotationBack((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.Quarter);
                    }
                    else if (R > -6)
                    {
                        Player.PlayerAction().PlayerArmRotation((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.None);
                        if (双手)
                            Player.PlayerAction().PlayerArmRotationBack((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.ThreeQuarters);
                    }
                    else if (R > -8)
                    {
                        Player.PlayerAction().PlayerArmRotation((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.None);
                        if (双手)
                            Player.PlayerAction().PlayerArmRotationBack((Player.Dplayer().MouseWorld - Player.ArmCenter()).ToRotation() * Player.gravDir + v - Player.fullRotation, Player.CompositeArmStretchAmount.Full);
                    }
                }
            }
            if (Throwing>0)
            {
                Throwing--;
                if (ThrowingTime > 0)
                {
                    ThrowingRotation += ThrowingSpeed * Player.direction;
                    ThrowingTime--;
                }
                Player.itemRotation = ThrowingRotation;
                Player.PlayerAction().PlayerArmRotation(ThrowingRotation, 0);
            }

            if (Main.myPlayer==Player.whoAmI&&ModkeySetup.ShowKey.JustPressed)
            {
                if (!ShowKey)
                {
                    ShowKey = true;
                    DDmod.SyncData(DDType.Config, Main.myPlayer, -1, Main.myPlayer);
                }
            }
            Roll();
        }
        //挖掘冷却
        int PickTime;
        public override void ResetEffects()
        {
            WaterWings = false;
            Jump = false;
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.velocity.Y == 0)
            {
                Player.Aplayer().NinjaJump = false;
                if (Player.controlJump)
                {
                    Player.Aplayer().NinjaJump = true;
                }
            }
            else if (Player.velocity.Y < 0)
            {
                if (Player.controlJump)
                {
                    Player.Aplayer().NinjaJump = true;
                }
            }
            if(Math.Abs(Player.velocity.X)>8)
            {
                Player.Aplayer().NinjaJump = true;
            }
            /*if (Player.controlDown)
           {
               Player.RotationSpeed(Player.direction * MathHelper.PiOver2, 0.2f);
               Player.position.Y += Player.height;
               Player.height = 42 - 20;
               Player.width = 42;
               Player.position.Y -= Player.height;
               Player.gfxOffY = -8;
               Player.maxRunSpeed *= 0.2f;
           }
           else
           {
               Player.height = 42;
               Player.width = 20;
           }*/
            //检测脚底有没有水
            bool ThereWater = false;
            bool ThereWater2 = false;
            int TW = 0;
                for (int a = 0; a < 20; a++)
                {
                    for (int b = 0; b < 3; b++)
                    {
                        Point point = new Point((int)(Player.position.X / 16), (int)((Player.position.Y + Player.height)) / 16 + a + b);
                        Tile tile = Main.tile[point];
                        Point point2 = new Point((int)(Player.position.X / 16) + 1, (int)((Player.position.Y + Player.height)) / 16 + a + b);
                        Tile tile2 = Main.tile[point2];
                        if (DDHelper.SolidTile(tile) || DDHelper.SolidTile(tile2))
                        {
                            ThereWater = false;
                            ThereWater2 = true;
                            break;
                        }
                        if (tile.LiquidAmount > 155 && tile2.LiquidAmount >= 155)
                        {
                            if (!DDHelper.SolidTile(tile) && !DDHelper.SolidTile(tile2))
                            {
                                TW++;
                            }
                        }
                    }
                    if (TW >= 12 || ThereWater2)
                    {
                        break;
                    }
                }
                if (TW >= 12)
                {
                    ThereWater = true;
                }
            //Player.RotationSpeed(Player.fullRotation, 0f);
            if (Player.Dplayer().Realm > 0 || Player.pulley)
            {
                Player.RotationSpeed(0, 0.15f);
            }

            if ((!Player.Aplayer().Ninja || Math.Abs(Player.velocity.X) <= 10)||Player.itemAnimation!=0)
            {
                if (!Collision.DrownCollision(Player.position, Player.width, Player.height, Player.gravDir))
                {
                    if (!Player.mount.Active && !Player.mount.Cart && !Player.pulley)
                    {
                        if (Player.controlJump && Player.wingTime > 0)
                        {
                            Player.RotationSpeed(0, 0.05f);
                        }
                    }
                }
                if (!Swim && (Player.HasBuff(103) && !ThereWater))
                {
                    Player.RotationSpeed(0, 0.2f);
                }
            }
            /*if (Player.pulley)
            {
                int X = (int)(Player.position.X / 16);
                int X2 = X + (int)(Player.width / 16);
                if(Player.controlJump)
                {
                    Player.velocity.Y = -117;
                }
                if (Player.controlRight)
                {
                    if (Main.tile[X2 + 1, (int)(Player.position.Y / 16) - 1].TileType == 213 || Main.tile[X2 + 1, (int)(Player.position.Y / 16) - 1].TileType == 214)
                    {
                        Player.position.X += 10;
                        Player.controlLeft = false;
                        Player.controlUp = true;
                        Player.pulleyDir = 2;
                    }
                    Player.direction = 1;
                }
                if (Player.controlLeft)
                {
                    if (Main.tile[X - 1, (int)(Player.position.Y / 16) - 1].TileType == 213 || Main.tile[X - 1, (int)(Player.position.Y / 16) - 1].TileType == 214)
                    {
                        Player.position.X -= 10;
                        Player.controlLeft = false;
                        Player.controlUp = true;
                        Player.pulleyDir = 2;
                    }
                    Player.direction = -1;
                }
            }*/
            if ((Collision.DrownCollision(Player.position, Player.width, Player.height, Player.gravDir) && (!Player.mount.Active && !Player.mount.Cart)) && !WaterWings)
            {
                if (!Swim && ThereWater&& Player.controlJump)
                {
                    Player.controlJump = false;
                    Jump = true;
                }
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                if (headRotation != ModContent.GetInstance<DDConfigClient>().Head)
                {
                    headRotation = ModContent.GetInstance<DDConfigClient>().Head;
                    DDmod.SyncData(DDType.Config, Main.myPlayer, -1, Main.myPlayer);
                }
                if (Somersault != ModContent.GetInstance<DDConfigClient>().Somersault)
                {
                    Somersault = ModContent.GetInstance<DDConfigClient>().Somersault;
                    DDmod.SyncData(DDType.Config, Main.myPlayer, -1, Main.myPlayer);
                }
                if (Swim != ModContent.GetInstance<DDConfigClient>().Swim)
                {
                    Swim = ModContent.GetInstance<DDConfigClient>().Swim;
                    DDmod.SyncData(DDType.Config, Main.myPlayer, -1, Main.myPlayer);
                }
                if (Action != ModContent.GetInstance<DDConfigClient>().Action)
                {
                    Action = ModContent.GetInstance<DDConfigClient>().Action;
                    DDmod.SyncData(DDType.Config, Main.myPlayer, -1, Main.myPlayer);
                }
                if (MoveEffects != ModContent.GetInstance<DDConfigClient>().MoveEffects)
                {
                    MoveEffects = ModContent.GetInstance<DDConfigClient>().MoveEffects;
                    DDmod.SyncData(DDType.Config, Main.myPlayer, -1, Main.myPlayer);
                }
                if (ShowWeapons != ModContent.GetInstance<DDConfigClient>().ShowWeapons)
                {
                    ShowWeapons = ModContent.GetInstance<DDConfigClient>().ShowWeapons;
                    DDmod.SyncData(DDType.Config, Main.myPlayer, -1, Main.myPlayer);
                }
                if(Player.Dplayer().Config)
                {
                    DDmod.SyncData(DDType.Config, Main.myPlayer, -1, Main.myPlayer);
                    DDmod.SyncData(DDType.PlayerFood, Player.whoAmI,-1,Main.myPlayer);
                    DDmod.SyncData(DDType.Battlepets, Main.myPlayer, -1, Main.myPlayer);
                    Player.Dplayer().Config = false;
                }
            }

        }
        public bool Jump;
        /// <summary> 水中翅膀 </summary>
        public bool WaterWings;
        /// <summary> 处于游泳状态 </summary>
        public bool SwimStatus;
        /// <summary> 游泳速度 </summary>
        public Vector2 SwimSpeed;
        /// <summary> 摔伤 </summary>
        public bool fall = false;

        /// <summary> 头转动 </summary>
        public bool headRotation;
        /// <summary> 忽略 </summary>
        public bool Somersault;
        /// <summary> 游泳 </summary>
        public bool Swim;
        /// <summary> 动作 </summary>
        public bool Action;
        /// <summary> 移动前倾 </summary>
        public bool MoveEffects;
        /// <summary> 翻滚 </summary>
        public  void Roll()
        {
            //进入水中给予buff
            if (Collision.DrownCollision(Player.position, Player.width, Player.height, Player.gravDir) && !Swim)
            {
                Player.AddBuff(103, 600);
                fall = false;
            }
            //检测脚下有没有水
            bool ThereWater = false;
            bool ThereWater2 = false;
            int TW = 0;
            for (int a = 0; a < 20; a++)
            {
                for (int b = 0; b < 3; b++)
                {
                    Point point = new Point((int)(Player.position.X / 16), (int)((Player.position.Y + Player.height)) / 16 + a + b);
                    Tile tile = Main.tile[point];
                    Point point2 = new Point((int)(Player.position.X / 16) + 1, (int)((Player.position.Y + Player.height)) / 16 + a + b);
                    Tile tile2 = Main.tile[point2];
                    if (DDHelper.SolidTile(tile) || DDHelper.SolidTile(tile2))
                    {
                        ThereWater = false;
                        ThereWater2 = true;
                        break;
                    }
                    if (tile.LiquidAmount > 155 && tile2.LiquidAmount >= 155 && tile.LiquidType == 0 && tile2.LiquidType == 0)
                    {
                        if (!DDHelper.SolidTile(tile) && !DDHelper.SolidTile(tile2))
                        {
                            TW++;
                        }
                    }
                }
                if (TW >= 12 || ThereWater2)
                {
                    break;
                }
            }
            if (TW >= 12)
            {
                ThereWater = true;
            }
            //假如翻滚到一定程度不给攻击
            if (Player.fullRotation > 1 && Player.fullRotation < 5.28)
            {
                //ForbiddenToAttack = 3;
            }
            if (ThereWater && (Player.wingTime == 0 || Player.HasBuff(103)) && !Swim && Player.Dplayer().Realm <= 0&& Math.Abs(Player.velocity.Y)>0.1F&& !Player.canFloatInWater)
            {
                if ((!Player.waterWalk && !Player.waterWalk2) || SwimStatus)
                {
                    if(!Player.mount.Active && !Player.mount.Cart && !Player.pulley)
                    Player.RotationSpeed(Player.velocity.ToRotation() + MathHelper.PiOver2, 0.1f);
                    fall = false;
                }
            }
            else if(!Player.mount.Active && !Player.mount.Cart && !Player.pulley)
            {
                if (!fall && !Player.sleeping.isSleeping && !Player.sitting.isSitting)
                {
                    if (Player.PlayerAction().MoveEffects)
                    {
                        float R = Player.velocity.X * 0.03f;
                        if (R > 1F)
                        {
                            R = 1F;
                        }
                        if (R < -1F)
                        {
                            R = -1F;
                        }
                        if (!Player.Aplayer().Ninja || Player.velocity.Y == 0)
                        {
                            Player.RotationSpeed(R, 0.1f);
                        }
                    }
                    else
                    {
                        Player.RotationSpeed(0, 0.1f);
                    }
                }
                if (Player.sleeping.isSleeping)
                {
                    if (Player.direction == 1)
                    {
                        Player.fullRotation = -MathHelper.PiOver2;
                    }
                    else
                    {
                        Player.fullRotation = MathHelper.PiOver2;
                    }
                }
            }
            else
            {
                Player.fullRotation  = 0;
            }
            bool Flipper = Player.GetJumpState<FlipperJump>().Enabled;
            if (!Collision.DrownCollision(Player.position, Player.width, Player.height, Player.gravDir))
            {
                SwimStatus = false;
                SwimSpeed = Player.velocity;
                //启用忽略和摔伤
                if(Player.ActiveItem().holdStyle>0)
                {
                    Somersault = false;
                }
                if (Player.velocity.Y != 0 && Player.controlUp && Somersault && !Player.HasBuff(103) && Player.Dplayer().Realm <= 0)
                {
                    float SP = Player.velocity.X * 0.03f;
                    if (SP > 0.12F) SP = 0.12F;
                    if (SP < -0.12F) SP = -0.12F;
                    Player.fullRotation += SP;
                    fall = true;
                }
                //假如落水或者抓绳子就不会摔伤
                if (Player.pulley|| Player.Aplayer().Ninja)
                {
                    fall = false;
                }
                for (int a = 0; a < 1000; a++)
                {
                    Projectile projectile = Main.projectile[a];
                    if (projectile.active && projectile.aiStyle == 7 && projectile.owner == Player.whoAmI)
                    {
                        fall = false;
                        break;
                    }
                }
                //落地
                if (Player.velocity.Y == 0)
                {
                    if (Player.Dplayer().PrePosition.Y > 0 && !Player.sleeping.isSleeping && !Player.sitting.isSitting && !Player.controlDown && (!Player.Aplayer().Ninja || Math.Abs(Player.velocity.X) < 8 || Player.itemAnimation != 0))
                    {
                        if (Math.Abs(Player.fullRotation) >= 0.3F || Player.velocity.Length() == 0)
                        {
                            Player.RotationSpeed(0, 0.2f);
                        }
                        if (Math.Abs(Player.fullRotation) > 1 && Math.Abs(Player.fullRotation) < 5.28 && fall)
                        {
                            Player.statDefense = DefenseStat.Default;
                            int damage = 50;
                            if (Math.Abs(Player.fullRotation) > 2 && Math.Abs(Player.fullRotation) < 4.28 && fall)
                            {
                                damage *= 2;
                            }
                            if (Player.whoAmI == Main.myPlayer)
                            {
                                Player.AddBuff(160, damage * 3);
                                if (!DDSystem.English)
                                {
                                    Player.Hurt(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.DDmod.PlayerKill.Kill1", "")), damage, 0);
                                }
                                else
                                {
                                    Player.Hurt(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.DDmod.PlayerKill.Kill1", Player.name)), damage, 0);
                                }
                            }
                        }
                    }
                    fall = false;
                }
            }
            else if (Player.velocity.Length() > 0.1F && (!Player.mount.Active && !Player.mount.Cart) &&Player.dashDelay == 0)
            {
                SwimStatus = false;
                if (!Swim && Player.Aplayer().IgnoreWater == 0 && ThereWater)
                {
                    SwimStatus = true;
                    Player.wingTime = Player.wingTimeMax;
                    //如果进入了水就不启用摔伤和游泳
                    fall = false;
                    if (Player.Dplayer().Realm <= 0)
                    {
                        Player.RotationSpeed(Player.velocity.ToRotation() + MathHelper.PiOver2, 0.1f);
                    }
                    //最大速度
                    float Speed = 8 + (Flipper ? 2 : 0);
                    if (Player.accMerman) Speed = 12;
                    //加速度
                    float addSpeed = 0.1F + ( Flipper  ? 0.1F : 0) + (Player.accMerman ? 0.1F : 0);
                    //向上
                    if (Player.controlUp && SwimSpeed.Y > -Speed)
                    {
                        SwimSpeed.Y -= addSpeed * ( Flipper  ? 4 : 3);
                    }
                    Speed = 5 + ( Flipper  ? 5 : 0);
                    //向下
                    if (Player.controlDown && SwimSpeed.Y < Speed / 2)
                    {
                        SwimSpeed.Y += addSpeed;
                    }
                    //向左
                    if (Player.controlLeft && SwimSpeed.X > -Speed)
                    {
                        SwimSpeed.X -= addSpeed;
                    }
                    //向右
                    if (Player.controlRight && SwimSpeed.X < Speed)
                    {
                        SwimSpeed.X += addSpeed;
                    }
                    if (Player.swimTime < 10)
                    {
                        Player.swimTime = 30;
                    }
                    Player.velocity = SwimSpeed;
                   
                    if (Player.controlJump && SwimSpeed.Length() < 8)
                    {
                        SwimSpeed *= 1.02f;
                    }
                    if (Player.canFloatInWater && !Player.controlDown)
                    {
                        SwimSpeed.Y -= 0.1F;
                        Player.RotationSpeed(0, 0.2f);
                    }
                }
                else
                {
                    SwimSpeed = Player.velocity;
                }
            }
            else
            {
                if (!Swim)
                {
                    Player.wingTime = Player.wingTimeMax;
                    SwimSpeed = Player.velocity;
                    //开始游泳
                    if (Player.controlUp && (!Player.mount.Active && !Player.mount.Cart))
                    {
                        Player.velocity.Y -= 0.3F * ( Flipper  ? 4 : 1);
                    }
                }
            }
            //翻转中心
            if (Player.fullRotation != 0 && (!Player.mount.Active && !Player.mount.Cart))
            {
                Player.fullRotationOrigin = Player.Size / 2;
            }
            //坐骑
            if (Player.mount.Active || Player.mount.Cart)
            {
                SwimStatus = false;
                //Player.fullRotation = 0;
            }
        }
    }
    public enum HandheldWeaponType : int
    {
        Hand,
        preBack,
        Back,
        Ground,
    }
}