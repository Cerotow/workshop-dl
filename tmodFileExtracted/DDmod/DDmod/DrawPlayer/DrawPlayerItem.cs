using DDmod.Content;
using DDmod.Content.Items;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Content.Items.农场.水壶;
using DDmod.Content.Projectiles.Talisman;
using DDmod.Players;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.UI;

namespace DDmod.DrawPlayer
{
    //绘制盾牌拿在手上
    public class DrawShieldHand : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.CaptureTheGem);
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            return drawInfo.shadow == 0f && !drawPlayer.dead && drawPlayer.PlayerAction().Action && drawPlayer.PlayerAction().ShowWeapons;
        }
        protected override void Draw(ref PlayerDrawSet drawinfo)
        {
            Player player = drawinfo.drawPlayer;
            bool Speed = player.Dplayer().Speed;
            bool MaxSpeed = player.Dplayer().MaxSpeed;
            Item item = player.inventory[player.selectedItem];
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (!DDPlayer.UseBoomerang(player.ActiveItem(), player))
            {
                return;
            }
            if (/*player.itemAnimation == 0*/player.Dplayer().ShieldDefense==0 && item.type > 0 && player.ActiveItem().holdStyle == 0&& item.GetGlobalItem<MeleeGlobalItem>().SwordandShield && !player.PlayerAction().SwimStatus)
            {
                DrawData draw;

                Color color = Color.White;
                Color LightColor = Lighting.GetColor((int)(player.Center.X / 16), (int)(player.Center.Y / 16), color);
                float scale = player.GetAdjustedItemScale(item);

                Texture2D texture = TextureAssets.Item[player.inventory[player.selectedItem].type].Value;
                if (item.DItem().Twin)
                {
                    Main.instance.LoadProjectile(item.shoot);
                    texture = TextureAssets.Projectile[item.shoot].Value;

                }
                if (texture == null)
                {
                    return;
                }
                float frame = (Main.itemAnimations[item.type] == null) ? 1 : Main.itemAnimations[item.type].FrameCount;
                float LargeWeapons = 0;
                //大型武器
                if (texture != null)
                {
                    LargeWeapons = new Vector2(texture.Width, texture.Height / frame).Length() * player.GetAdjustedItemScale(item);
                    if (item.DItem().Twin)
                    {
                        LargeWeapons = new Vector2(texture.Width / 2, texture.Height).Length() * player.GetAdjustedItemScale(item);
                    }
                }
                Vector2 vec = player.MountedCenter - Main.screenPosition + new Vector2(-6 * player.direction, 0);
                if (player.portableStoolInfo.IsInUse)
                {
                    //vec.Y += player.portableStoolInfo.HeightBoost / 2;
                }
                Rectangle? sourceRect = new Rectangle?((Main.itemAnimations[item.type] == null) ? Utils.Frame(texture, 1, 1, 0, 0, 0, 0) : Main.itemAnimations[item.type].GetFrame(texture, -1));
                if (item.DItem().Twin)
                {
                    sourceRect = new Rectangle?(Utils.Frame(texture, 2, 1, 0, 0, 0, 0));
                }
                ItemSlot.GetItemLight(ref LightColor, ref scale, item, false);
                LightColor = player.GetImmuneAlpha(item.GetAlpha(LightColor) * player.stealth, 0f);
                if (item.DItem().HandheldColor != new Color(0, 0, 0, 0))
                {
                    LightColor = item.DItem().HandheldColor;
                }
                //让物品放在手中间
                Vector2 origin = new Vector2(texture.Width / 4, texture.Height / frame / 2);
                //旋转
                float Rotation = -player.Aplayer().ArmSwing * player.direction + 0.5F * player.direction;
                //距离
                float Distance = texture.Height/2 / frame / 2;

                // -1默认
                int HandheldStyle = -1;
                Distance += item.DItem().DrawDistance;
                //如果武器是拿在手上的
                if (player.PlayerAction().WeaponType < HandheldWeaponType.Back)
                {
                    if (player.inventory[player.selectedItem].DamageType == DamageClass.Melee)
                    {
                        //如果在坐骑上或者在没跑路
                        if (!Speed || player.mount.Active || player.mount.Cart)
                        {
                            vec += (-player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                            
                            Rotation += item.DItem().DrawRot * player.direction;
                            vec.Y += drawinfo.drawPlayer.gfxOffY;
                            vec += Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / 56];
                            draw = new DrawData(texture, vec.Floor(), sourceRect, LightColor, Rotation, origin, scale, drawinfo.playerEffect, 0);
                            drawinfo.DrawDataCache.Add(draw);
                            DDHelper.drawGlow(0, item, player, vec.Floor(), sourceRect, Rotation, origin, scale, drawinfo.playerEffect, drawinfo);

                        }
                        else
                        {
                            if (!player.Aplayer().Ninja)
                            {
                                if (LargeWeapons > 100 && player.PlayerAction().WeaponType == HandheldWeaponType.Hand)
                                {
                                    vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    Rotation = MathHelper.Pi + 0.65F * player.direction - player.fullRotation;
                                    for (int a = 0; a < 3; a++)
                                    {
                                        int i = (int)(((player.Center.X + player.width / 2 * player.direction) - texture.Width * 1.5f * scale * player.direction) / 16 + Main.rand.Next((int)(-1 * scale), (int)(1 * scale)));
                                        int j = (int)((player.position.Y + player.height) / 16);
                                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType != 5)
                                        {
                                            Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, Main.tile[i, j])];
                                            obj.velocity = new Vector2(0, -1);
                                            obj.position -= new Vector2(0, 8);
                                            obj.scale = 1.2f;
                                        }
                                        else if (!Main.tile[i, j].HasTile && Main.tile[i, j].LiquidAmount > 0)
                                        {
                                            if (Main.tile[i, j].LiquidType == 0)
                                            {
                                                int t = 33;
                                                Dust obj = Main.dust[NewDust(new Vector2(i, j) * 16, 16, 1, t)];
                                                obj.velocity = new Vector2(0, -2);
                                                obj.scale = 2.2f;
                                            }
                                        }
                                    }
                                    if (player.velocity.Y != 0)
                                    {
                                        player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                                    }
                                }
                                else
                                {
                                    vec += (-player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                }
                            }
                            else
                            {
                                if (LargeWeapons > 100 && player.PlayerAction().WeaponType == HandheldWeaponType.Hand)
                                {
                                    vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    Rotation = MathHelper.Pi + 0.65F * player.direction - player.fullRotation;

                                    for (int a = 0; a < 3; a++)
                                    {
                                        int i = (int)(((player.Center.X + player.width / 2 * player.direction) - texture.Width * 1.5f * item.scale * player.direction) / 16 + Main.rand.Next((int)(-1 * item.scale), (int)(1 * item.scale)));
                                        int j = (int)((player.position.Y + player.height) / 16);
                                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType != 5)
                                        {
                                            Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, Main.tile[i, j])];
                                            obj.velocity = new Vector2(0, -1);
                                            obj.position -= new Vector2(0, 8);
                                            obj.scale = 1.2f;
                                        }
                                        else if (!Main.tile[i, j].HasTile && Main.tile[i, j].LiquidAmount > 0)
                                        {
                                            if (Main.tile[i, j].LiquidType == 1)
                                            {
                                                int t = 33;
                                                Dust obj = Main.dust[NewDust(new Vector2(i, j), 16, 1, t)];
                                                obj.velocity = new Vector2(0, -1);
                                                obj.position -= new Vector2(0, 8);
                                                obj.scale = 1.2f;
                                            }
                                        }
                                    }
                                    if (player.velocity.Y != 0)
                                    {
                                        player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                                    }
                                }
                                else
                                {
                                    if (HandheldStyle == -1)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance - 2);
                                        Rotation = (MathHelper.PiOver2 + 1) * player.direction;
                                    }
                                }
                            }
                            Rotation += item.DItem().DrawRot * player.direction;
                            vec.Y += drawinfo.drawPlayer.gfxOffY;
                            vec += Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / 56];
                            draw = new DrawData(texture, vec.Floor(), sourceRect, LightColor, Rotation, origin, scale, drawinfo.playerEffect, 0);

                            drawinfo.DrawDataCache.Add(draw);
                            DDHelper.drawGlow(0, item, player, vec.Floor(), sourceRect, Rotation, origin, scale, drawinfo.playerEffect, drawinfo);
                        }
                    }
                }
            }
            return;
        }
    }
    //绘制武器拿在手上
    public class DrawWeaponHand : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.HeldItem);
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            return drawInfo.shadow == 0f && !drawPlayer.dead && drawPlayer.PlayerAction().Action && drawPlayer.PlayerAction().ShowWeapons;
        }
        protected override void Draw(ref PlayerDrawSet drawinfo)
        {
            Player player = drawinfo.drawPlayer;
            bool Speed = player.Dplayer().Speed;
            bool MaxSpeed = player.Dplayer().MaxSpeed;
            Item item = player.inventory[player.selectedItem];
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            bool TypeBool = item.type == 4760|| (item.type > 0&&item.DItem().NoDraw);
            if (!DDPlayer.UseBoomerang(player.ActiveItem(), player))
            {
                return;
            }
            if (player.itemAnimation == 0 && item.type > 0 && !TypeBool && !item.GetGlobalItem<MeleeGlobalItem>().SwordandShield && !item.GetGlobalItem<RangedGlobalItem>().Bow && player.ActiveItem().holdStyle == 0 && !player.PlayerAction().SwimStatus)
            {
                DrawData draw;

                Color color = Color.White;
                Color LightColor = Lighting.GetColor((int)(player.Center.X / 16), (int)(player.Center.Y / 16), color);
                float scale = player.GetAdjustedItemScale(item);

                Texture2D texture = TextureAssets.Item[player.inventory[player.selectedItem].type].Value;
                if (item.DItem().Twin)
                {
                    Main.instance.LoadProjectile(item.shoot);
                    texture = TextureAssets.Projectile[item.shoot].Value;
                }
                if (texture == null)
                {
                    return;
                }
                float frame = (Main.itemAnimations[item.type] == null) ? 1 : Main.itemAnimations[item.type].FrameCount;
                float LargeWeapons = 0;
                //大型武器
                if (texture != null)
                {
                    LargeWeapons = new Vector2(texture.Width, texture.Height / frame).Length() * player.GetAdjustedItemScale(item);
                    if (item.DItem().Twin)
                    {
                        LargeWeapons = new Vector2(texture.Width / 2, texture.Height).Length() * player.GetAdjustedItemScale(item);
                    }
                }
                Vector2 vec = player.MountedCenter - Main.screenPosition + new Vector2(-6 * player.direction, 0);
                if (player.portableStoolInfo.IsInUse)
                {
                    //vec.Y += player.portableStoolInfo.HeightBoost / 2;
                }
                Rectangle? sourceRect = new Rectangle?((Main.itemAnimations[item.type] == null) ? Utils.Frame(texture, 1, 1, 0, 0, 0, 0) : Main.itemAnimations[item.type].GetFrame(texture, -1));
                if (item.DItem().Twin)
                {
                    sourceRect = new Rectangle?(Utils.Frame(texture, 2, 1, 0, 0, 0, 0));
                }
                ItemSlot.GetItemLight(ref LightColor, ref scale, item, false);
                LightColor = player.GetImmuneAlpha(item.GetAlpha(LightColor) * player.stealth, 0f);
                if (item.DItem().HandheldColor != new Color(0, 0, 0, 0))
                {
                    LightColor = item.DItem().HandheldColor;
                }
                if (item.GetGlobalItem<水壶>().kettle)
                {
                    Rectangle value = new Rectangle(0, 0, texture.Width, texture.Height / 2);
                    if (item.GetGlobalItem<水壶>().WaterVolume == 0)
                    {
                        value.Y = texture.Height / 2;
                    }
                    sourceRect = value;
                }
                //让物品放在手中间
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frame / 2);
                //旋转
                float Rotation = -player.Aplayer().ArmSwing * player.direction + 0.5F * player.direction;
                //距离
                float Distance = texture.Height / frame / 2;

                // -1默认
                int HandheldStyle = -1;
                //不属于任何职业而且使用方式是1
                if (HandheldStyle == -1)
                {
                    if (item.DamageType == DamageClass.Default && item.useStyle == 1)
                    {
                        HandheldStyle = 0;
                    }
                }
                if (HandheldStyle == -1)
                {
                    //绘制近战武器
                    if (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed)
                    {
                        if (item.useStyle > 0 && (item.useStyle !=5||ItemID.Sets.Spears[item.type]))
                        {
                            HandheldStyle = 1;
                        }
                    }
                }
                if (HandheldStyle == -1)
                {
                    //法师武器
                    if (item.DamageType == DamageClass.Magic)
                    {
                        if (Item.staff[item.type] || item.useStyle == 1 || item.GetGlobalItem<MagicGlobalItem>().Handheld)
                        {
                            HandheldStyle = 1;
                        }
                        else
                        if (item.useStyle == 5)
                        {
                            HandheldStyle = 3;
                        }
                    }
                }
                if (HandheldStyle == -1)
                {
                    //远程武器
                    if (item.DamageType == DamageClass.Ranged && item.maxStack == 1)
                    {
                        HandheldStyle = 3;
                    }
                }
                if (HandheldStyle == -1)
                {
                    //绘制召唤武器
                    if (item.DamageType == DamageClass.Summon || item.DamageType == DamageClass.SummonMeleeSpeed)
                    {
                        if (item.useStyle > 0 && (item.useStyle != 5 || Item.staff[item.type]))
                        {
                            HandheldStyle = 1;
                        }
                        if (!Item.staff[item.type] && item.useStyle == 5)
                        {
                            HandheldStyle = 2;
                        }
                    }
                }
                if (item.DItem().DrawMelee)
                {
                    HandheldStyle = 1;
                }
                if (item.DItem().DrawRanged)
                {
                    HandheldStyle = 3;
                }
                if (item.GetGlobalItem<水壶>().kettle)
                {
                    HandheldStyle = 4;
                }
                //绘制飞刀
                if (DGlobalItem.FlyingKnife[item.type])
                {
                    HandheldStyle = 2;
                }
                //绘制飞盘
                if (DGlobalItem.Frisbee[item.type])
                {
                    HandheldStyle = 1;
                }
                if(item.DItem().DrawDefaults)
                {
                    HandheldStyle = -1;
                }
                //使用方式为1的物品
                if (HandheldStyle == 0)
                {
                    origin = new Vector2(2, texture.Height / frame - 2);
                    Distance = 10;
                    if (player.direction < 0)
                    {
                        origin = new Vector2(texture.Width - 2, texture.Height / frame - 2);
                    }
                }
                //剑类物品的手持方式
                if (HandheldStyle == 1)
                {
                    origin = new Vector2(6, texture.Height / frame - 6);
                    Distance = 10;
                    if (player.direction < 0)
                    {
                        origin = new Vector2(texture.Width - 6, texture.Height / frame - 6);
                        if (item.DItem().Twin)
                        {
                            origin = new Vector2(texture.Width / 2 - 6, texture.Height / frame - 6);
                        }
                    }
                    Rotation = -player.Aplayer().ArmSwing * player.direction + 0.5F * player.direction;
                }
                //飞刀类物品的手持方式
                if (HandheldStyle == 2)
                {
                    origin = new Vector2(texture.Width / 2, texture.Height / frame - 2);
                    Distance = 10;
                    if (player.direction < 0)
                    {
                        origin = new Vector2(texture.Width / 2, texture.Height / frame - 2);
                        Rotation -= MathHelper.PiOver4;
                    }
                    else
                    {

                        Rotation += MathHelper.PiOver4;
                    }
                }

                //枪类物品的手持方式
                if (HandheldStyle == 3)
                {
                    origin = new Vector2(player.direction == -1 ? (texture.Width - texture.Width * 0.3F) : (texture.Width * 0.3F), texture.Height / frame / 2);
                    if (item.DItem().Twin)
                    {
                        origin = new Vector2(player.direction == -1 ? texture.Width / 2 - (texture.Width / 2 * 0.3F) : (texture.Width / 2 * 0.3F), texture.Height / frame / 2);
                    }
                    Rotation = -player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2 * player.direction;
                    Distance = texture.Height / frame / 2;
                    //origin -= Main.DrawPlayerItemPos(1, item.type).RotatedBy(Rotation) * player.direction;
                }
                //水壶
                if (HandheldStyle == 4)
                {
                    origin = new Vector2(2, texture.Height / frame / 2);
                    Distance = texture.Width/2;
                    Rotation = MathHelper.PiOver4 * player.direction;
                    if (player.direction < 0)
                    {
                        origin = new Vector2(texture.Width-2, texture.Height / frame / 2);
                        Rotation += MathHelper.PiOver4;
                    }
                    else
                    {

                        Rotation -= MathHelper.PiOver4;
                    }
                }
                Distance += item.DItem().DrawDistance;
                if (player.direction < 0)
                {
                    origin.X += item.DItem().DrawVec.X;
                }
                else
                {
                    origin.X -= item.DItem().DrawVec.X;
                }
                origin.Y += item.DItem().DrawVec.Y;
                if(HandheldStyle!=1)
                {

                    if (LargeWeapons > 50)
                    {
                        if (Speed&& player.PlayerAction().WeaponType==HandheldWeaponType.Hand)
                        {
                            player.PlayerAction().WeaponType = HandheldWeaponType.preBack;
                        }
                    }
                }
                //如果武器是拿在手上的
                if (player.PlayerAction().WeaponType < HandheldWeaponType.Back)
                {
                    //if (player.inventory[player.selectedItem].DamageType == DamageClass.Melee)
                    {
                        //如果在坐骑上或者在没跑路
                        if (!Speed || player.mount.Active || player.mount.Cart)
                        {
                            //展示时翻转
                            if (player.PlayerAction().Display && HandheldStyle == 0)
                            {
                                Rotation += 1F * player.direction;
                            }
                            //展示时翻转
                            if (player.PlayerAction().Display && HandheldStyle == 1)
                            {
                                Rotation += 2F * player.direction;
                            }
                            if (HandheldStyle == 4)
                            {
                                vec += (-1.4f * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                            }
                            else
                            {
                                vec += (-player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                            }
                            Rotation += item.DItem().DrawRot * player.direction;
                            vec.Y += drawinfo.drawPlayer.gfxOffY;
                            vec += Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / 56];
                            draw = new DrawData(texture, vec.Floor(), sourceRect, LightColor, Rotation, origin, scale, drawinfo.playerEffect, 0);
                            drawinfo.DrawDataCache.Add(draw);
                            DDHelper.drawGlow(0, item, player, vec.Floor(), sourceRect, Rotation, origin, scale, drawinfo.playerEffect, drawinfo);

                        }
                        else
                        {
                            if (!player.Aplayer().Ninja)
                            {
                                if (LargeWeapons > 100&& player.PlayerAction().WeaponType == HandheldWeaponType.Hand)
                                {
                                    player.bodyFrame.Y = 0;
                                    vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance);
                                    Rotation = MathHelper.Pi + 0.65F * player.direction - player.fullRotation;
                                    for (int a = 0; a < 3; a++)
                                    {
                                        int i = (int)(((player.Center.X + player.width / 2 * player.direction) - (texture.Width+ item.DItem().DrawVec.X*2) * 1.5f * scale * player.direction) / 16 + Main.rand.Next((int)(-1 * scale), (int)(1 * scale)));
                                        int j = (int)((player.position.Y + player.height) / 16);
                                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType != 5)
                                        {
                                            Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, Main.tile[i, j])];
                                            if (item.type == 121)
                                            {
                                                obj.type = 6;
                                            }
                                            if (item.type == ModContent.ItemType<火山长剑>())
                                            {
                                                if (Main.rand.NextBool(8))
                                                    obj.type = 6;
                                            }
                                            if (item.type == ModContent.ItemType<ShadowFlameSwordItem>())
                                            {
                                                obj.type = 27;
                                            }
                                            obj.velocity = new Vector2(0, -1);
                                            obj.position -= new Vector2(0, 8);
                                            obj.scale = 1.2f;
                                        }
                                        else if (!Main.tile[i, j].HasTile && Main.tile[i, j].LiquidAmount > 0)
                                        {
                                            if (Main.tile[i, j].LiquidType == 0)
                                            {
                                                int t = 33;
                                                if (item.type == 121)
                                                {
                                                    t = 31;
                                                    PlaySound(SoundID.LiquidsWaterLava, new Vector2(i, j) * 16);
                                                }
                                                if (item.type == ModContent.ItemType<火山长剑>())
                                                {
                                                    if (Main.rand.NextBool(8))
                                                    {
                                                        t = 31;
                                                        PlaySound(SoundID.LiquidsWaterLava, new Vector2(i, j) * 16);
                                                    }
                                                }
                                                Dust obj = Main.dust[NewDust(new Vector2(i, j) * 16, 16, 1, t)];
                                                if (item.type == ModContent.ItemType<ShadowFlameSwordItem>())
                                                {
                                                    obj.type = 27;
                                                }
                                                obj.velocity = new Vector2(0, -2);
                                                obj.scale = 2.2f;
                                            }
                                        }
                                    }
                                    if (player.velocity.Y != 0)
                                    {
                                        player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                                    }
                                }
                                else
                                {
                                    if (HandheldStyle == 4)
                                    {
                                        vec += (-1.4f * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    }
                                    else
                                    {
                                        vec += (-player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    }
                                    //Rotation = -player.Aplayer().ArmSwing * player.direction + 0.5F * player.direction;
                                    if (HandheldStyle == 3)
                                    {
                                        Rotation = -player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2 * player.direction;
                                    }
                                }
                            }
                            else
                            {
                                if (LargeWeapons > 100 && player.PlayerAction().WeaponType == HandheldWeaponType.Hand)
                                {
                                    player.bodyFrame.Y = 0;
                                    vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    Rotation = MathHelper.Pi + 0.65F * player.direction - player.fullRotation;

                                    for (int a = 0; a < 3; a++)
                                    {
                                        int i = (int)(((player.Center.X + player.width / 2 * player.direction) - (texture.Width + item.DItem().DrawVec.X * 2) * 1.5f * item.scale * player.direction) / 16 + Main.rand.Next((int)(-1 * item.scale), (int)(1 * item.scale)));
                                        int j = (int)((player.position.Y + player.height) / 16);
                                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType != 5)
                                        {
                                            Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, Main.tile[i, j])];
                                            if (item.type == 121)
                                            {
                                                obj.type = 6;
                                            }
                                            if (item.type == ModContent.ItemType<火山长剑>())
                                            {
                                                if (Main.rand.NextBool(8))
                                                    obj.type = 6;
                                            }
                                            if (item.type == ModContent.ItemType<ShadowFlameSwordItem>())
                                            {
                                                obj.type = 27;
                                            }
                                            obj.velocity = new Vector2(0, -1);
                                            obj.position -= new Vector2(0, 8);
                                            obj.scale = 1.2f;
                                        }
                                        else if (!Main.tile[i, j].HasTile && Main.tile[i, j].LiquidAmount > 0)
                                        {
                                            if (Main.tile[i, j].LiquidType == 0)
                                            {
                                                int t = 33;
                                                if (item.type == 121)
                                                {
                                                    t = 31;
                                                    PlaySound(SoundID.LiquidsWaterLava, new Vector2(i, j) * 16);
                                                }
                                                if (item.type == ModContent.ItemType<火山长剑>())
                                                {
                                                    if (Main.rand.NextBool(8))
                                                    {
                                                        t = 31;
                                                        PlaySound(SoundID.LiquidsWaterLava, new Vector2(i, j) * 16);
                                                    }
                                                }
                                                Dust obj = Main.dust[NewDust(new Vector2(i, j) * 16, 16, 1, t)];
                                                if (item.type == ModContent.ItemType<ShadowFlameSwordItem>())
                                                {
                                                    obj.type = 27;
                                                }
                                                obj.velocity = new Vector2(0, -2);
                                                obj.scale = 2.2f;
                                            }
                                        }
                                    }
                                    if (player.velocity.Y != 0)
                                    {
                                        player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                                    }
                                }
                                else
                                {
                                    if (HandheldStyle == -1)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance - 2);
                                        Rotation = (MathHelper.PiOver2 + 1) * player.direction;
                                    }
                                    if (HandheldStyle == 0)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance - 2);
                                        Rotation = (MathHelper.PiOver2 + 1) * player.direction;
                                    }
                                    if (HandheldStyle == 1)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance + 2);
                                        Rotation = MathHelper.PiOver4 * player.direction;
                                        if (player.velocity.Y == 0)
                                        {
                                            Rotation -= player.fullRotation - 0.3F * player.direction;
                                            //如果玩家速度大于8
                                            if (MaxSpeed)
                                            {
                                                Rotation += player.fullRotation;
                                            }
                                        }
                                    }
                                    if (HandheldStyle == 2)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance + 2);
                                        Rotation = MathHelper.PiOver2 * player.direction;
                                    }
                                    if (HandheldStyle == 3)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                        //如果玩家速度大于8
                                        if (Speed)
                                        {
                                            Rotation = MathHelper.PiOver2 * player.direction + 1f * player.direction;
                                        }
                                    }
                                    if (HandheldStyle == 4)
                                    {
                                        vec += (-1.4f * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    }
                                }
                            }
                            Rotation += item.DItem().DrawRot * player.direction;
                            vec.Y += drawinfo.drawPlayer.gfxOffY;
                            vec += Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / 56];
                            draw = new DrawData(texture, vec.Floor(), sourceRect, LightColor, Rotation, origin, scale, drawinfo.playerEffect, 0);

                            drawinfo.DrawDataCache.Add(draw);
                            DDHelper.drawGlow(0, item, player, vec.Floor(), sourceRect, Rotation, origin, scale, drawinfo.playerEffect, drawinfo);
                        }
                    }
                }
            }
            else if (player.ActiveItem().type == 186 && player.itemAnimation == 0)
            {
                DrawData draw;

                Color color = Color.White;
                Color LightColor = Lighting.GetColor((int)(player.Center.X / 16), (int)(player.Center.Y / 16), color);
                float scale = player.GetAdjustedItemScale(item);
                Texture2D texture = TextureAssets.Item[player.inventory[player.selectedItem].type].Value;
                if (texture == null)
                {
                    return;
                }
                Vector2 vec = player.MountedCenter - Main.screenPosition;

                ItemSlot.GetItemLight(ref LightColor, ref scale, item, false);
                LightColor = player.GetImmuneAlpha(item.GetAlpha(LightColor) * player.stealth, 0f);

                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                vec.Y += drawinfo.drawPlayer.gfxOffY;
                vec += Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / 56];

                draw = new DrawData(texture, vec.Floor() + new Vector2(0, -40).RotatedBy(-player.fullRotation) + new Vector2(4 * player.direction, -10), null, LightColor, -MathHelper.PiOver4 * player.direction - player.fullRotation, origin, player.inventory[player.selectedItem].scale, drawinfo.playerEffect, 0);
                drawinfo.DrawDataCache.Add(draw);
            }
            return;
        }
    }
    public class DrawWeaponHand2 : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.SolarShield);
            //return new BeforeParent(PlayerDrawLayers.MountFront);
            //return new BeforeParent(PlayerDrawLayers.MountBack);
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            return false && drawInfo.shadow == 0f && !drawPlayer.dead && drawPlayer.PlayerAction().Action && drawPlayer.PlayerAction().ShowWeapons;

        }

        protected override void Draw(ref PlayerDrawSet drawinfo)
        {
            /*Player player = drawinfo.drawPlayer;
            bool Speed = false;
            if (Math.Abs(player.velocity.X) > 5.4F)
            {
                Speed = true;

            }
            //最大速度
            bool MaxSpeed = false;
            if (Math.Abs(player.velocity.X) > 8.4F)
            {
                MaxSpeed = true;
            }
            Item item = player.inventory[player.selectedItem];
            if (item.type > 0 && !item.DItem().Twin)
            {
                return;
            }
            if (player.PlayerAction().ThereShield2)
            {
                player.PlayerAction().ThereShield2 = false;
                return;
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            bool TypeBool = item.type == 4760;
            if (player.itemAnimation == 0 && item.type > 0 && !TypeBool && !item.GetGlobalItem<RangedGlobalItem>().Bow && player.ActiveItem().holdStyle == 0 && !player.PlayerAction().SwimStatus)
            {
                DrawData draw;

                Color color = Color.White;
                Color LightColor = Lighting.GetColor((int)(player.Center.X / 16), (int)(player.Center.Y / 16), color);
                float scale = player.GetAdjustedItemScale(item);

                Texture2D texture = TextureAssets.Projectile[item.shoot].Value;
                Main.instance.LoadProjectile(item.shoot);
                if (texture == null)
                {
                    return;
                }
                float frame = (Main.itemAnimations[item.type] == null) ? 1 : Main.itemAnimations[item.type].FrameCount;
                float LargeWeapons = 0;
                //大型武器
                if (texture != null)
                {
                    LargeWeapons = new Vector2(texture.Width / 2, texture.Height / frame).Length() * player.GetAdjustedItemScale(item);

                }
                Vector2 vec = player.MountedCenter - Main.screenPosition + new Vector2(-6 * player.direction, 0) + new Vector2(10 * player.direction, 0);

                if (player.portableStoolInfo.IsInUse)
                {
                    vec.Y += player.portableStoolInfo.HeightBoost / 2;
                }
                Rectangle? sourceRect = new Rectangle?((Main.itemAnimations[item.type] == null) ? Utils.Frame(texture, 2, 1, 1, 0, 0, 0) : Main.itemAnimations[item.type].GetFrame(texture, -1));

                ItemSlot.GetItemLight(ref LightColor, ref scale, item, false);
                LightColor = player.GetImmuneAlpha(item.GetAlpha(LightColor) * player.stealth, 0f);
                if (item.DItem().HandheldColor != new Color(0, 0, 0, 0))
                {
                    LightColor = item.DItem().HandheldColor;
                }
                //让物品放在手中间
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frame / 2);
                float Rotation = player.Aplayer().ArmSwing * player.direction + 0.5F * player.direction;

                if (player.PlayerAction().Display)
                {
                    Rotation = 0.5F * player.direction;
                }
                float Distance = texture.Height / frame / 2;
                //绘制近战武器
                if (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed || (item.DamageType == DamageClass.Ranged && item.maxStack > 1))
                {
                    if (item.useStyle > 0 && (item.useStyle != 5 || ItemID.Sets.Spears[item.type]))
                    {
                        origin = new Vector2(2, texture.Height / frame - 2);
                        Distance = 8;
                        if (player.direction < 0)
                        {
                            origin = new Vector2(texture.Width / 2 - 2, texture.Height / frame - 2);
                        }
                    }
                }
                //法师武器
                if (item.DamageType == DamageClass.Magic)
                {
                    if (Item.staff[item.type] || item.useStyle == 1 || item.GetGlobalItem<MagicGlobalItem>().Handheld)
                    {
                        origin = new Vector2(2, texture.Height / frame - 2);
                        Distance = 8;
                        if (player.direction < 0)
                        {
                            origin = new Vector2(texture.Width - 2, texture.Height / frame - 2);
                        }
                    }
                    else
                    if (item.useStyle == 5)
                    {
                        origin = new Vector2(player.direction == -1 ? texture.Width - 12 : 12, texture.Height / frame / 2);
                        Distance = texture.Height / frame / 2;
                        Rotation = -player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2 * player.direction;
                        if (LargeWeapons > 50)
                        {
                            if (Speed)
                            {
                                player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                            }
                        }
                    }
                }
                //远程武器
                if (item.DamageType == DamageClass.Ranged && item.maxStack == 1)
                {
                    origin = new Vector2(player.direction == -1 ? texture.Width / 2 - 16 : 16, texture.Height / frame / 2);

                    if (item.useAmmo == AmmoID.Arrow)
                    {
                        Distance = texture.Width / 2;
                    }
                    else
                    {
                        Distance = texture.Height / frame / 2;
                    }
                    Rotation = player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2 * player.direction;
                    if (LargeWeapons > 50)
                    {
                        if (Speed)
                        {
                            player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                        }
                    }
                }
                //不属于任何职业而且使用方式是1
                if (item.DamageType == DamageClass.Default && item.useStyle == 1)
                {
                    origin = new Vector2(2, texture.Height / frame - 2);
                    Distance = 8;
                    if (player.direction < 0)
                    {
                        origin = new Vector2(texture.Width - 2, texture.Height / frame - 2);
                    }
                }
                //如果武器是拿在手上的
                if (player.PlayerAction().WeaponType < HandheldWeaponType.Back)
                {
                    //if (player.inventory[player.selectedItem].DamageType == DamageClass.Melee)
                    {
                        //如果在坐骑上或者在走路
                        if (!Speed || player.mount.Active || player.mount.Cart)
                        {
                            //如果物品不能使用
                            if (item.useStyle == 0)
                            {
                                Rotation = player.Aplayer().ArmSwing * player.direction;
                            }
                            //展示时翻转
                            vec += (player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                            if (player.PlayerAction().Display)
                            {
                                vec -= (player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                vec += (MathHelper.PiOver2).ToRotationVector2() * Distance;
                            }
                            draw = new DrawData(texture, vec.Floor(), sourceRect, LightColor, Rotation, origin, scale, drawinfo.playerEffect, 0);
                            drawinfo.DrawDataCache.Add(draw);
                            DDHelper.drawGlow(0, item, player, vec.Floor(), sourceRect, Rotation, origin, scale, drawinfo.playerEffect, drawinfo);

                        }
                        else
                        {
                            Rotation = +0.8F * player.direction;
                            if (item.useStyle == 0)
                            {
                                Rotation = 1 * player.direction;
                                Distance -= 2;
                            }
                            if (!player.Aplayer().Ninja)
                            {
                                if (LargeWeapons > 100)
                                {
                                    vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    Rotation = MathHelper.Pi + 0.65F * player.direction - player.fullRotation;
                                    for (int a = 0; a < 3; a++)
                                    {
                                        int i = (int)(((player.Center.X + player.width / 2 * player.direction) - texture.Width * 1.5f * item.scale * player.direction) / 16 + Main.rand.Next((int)(-1 * item.scale), (int)(1 * item.scale)));
                                        int j = (int)((player.position.Y + player.height) / 16);
                                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType != 5)
                                        {
                                            Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, Main.tile[i, j])];
                                            obj.velocity = new Vector2(0, -1);
                                            obj.position -= new Vector2(0, 8);
                                            obj.scale = 1.2f;
                                        }
                                        else if (!Main.tile[i, j].HasTile && Main.tile[i, j].LiquidAmount > 0)
                                        {
                                            if (Main.tile[i, j].LiquidType == 0)
                                            {
                                                int t = 33;
                                                Dust obj = Main.dust[NewDust(new Vector2(i, j), 16, 1, t)];
                                                obj.velocity = new Vector2(0, -2);
                                                obj.scale = 2.2f;
                                            }
                                        }
                                    }
                                    if (player.velocity.Y != 0)
                                    {
                                        player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                                    }
                                }
                                else
                                {
                                    vec += (player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    Rotation = player.Aplayer().ArmSwing * player.direction + 0.5F * player.direction;
                                    if (player.velocity.Y != 0)
                                    {
                                        vec.Y -= 20;
                                        Rotation += 1 * player.direction;
                                    }
                                    if (DGlobalItem.FlyingKnife[item.type])
                                    {
                                        if (player.direction < 0)
                                        {
                                            Rotation -= MathHelper.PiOver4;
                                        }
                                        else
                                        {

                                            Rotation += MathHelper.PiOver4;
                                        }
                                    }
                                    if (item.useStyle == 5 && !ItemID.Sets.Spears[item.type])
                                    {
                                        Rotation = player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2 * player.direction;
                                    }
                                }
                            }
                            else
                            {
                                if (LargeWeapons > 100)
                                {
                                    vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    Rotation = MathHelper.Pi + 0.65F * player.direction - player.fullRotation;

                                    for (int a = 0; a < 3; a++)
                                    {
                                        int i = (int)(((player.Center.X + player.width / 2 * player.direction) - texture.Width * 1.5f * scale * player.direction) / 16 + Main.rand.Next((int)(-1 * scale), (int)(1 * scale)));
                                        int j = (int)((player.position.Y + player.height) / 16);
                                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType != 5)
                                        {
                                            Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, Main.tile[i, j])];
                                            if (item.type == 121)
                                            {
                                                obj.type = 6;
                                            }
                                            if (item.type == ModContent.ItemType<ShadowFlameSwordItem>())
                                            {
                                                obj.type = 27;
                                            }
                                            obj.velocity = new Vector2(0, -1);
                                            obj.position -= new Vector2(0, 8);
                                            obj.scale = 1.2f;
                                        }
                                        else if (!Main.tile[i, j].HasTile && Main.tile[i, j].LiquidAmount > 0)
                                        {
                                            if (Main.tile[i, j].LiquidType == 1)
                                            {
                                                int t = 33;
                                                if (item.type == 121)
                                                {
                                                    t = 6;
                                                }
                                                Dust obj = Main.dust[NewDust(new Vector2(i, j), 16, 1, t)];
                                                if (item.type == ModContent.ItemType<ShadowFlameSwordItem>())
                                                {
                                                    obj.type = 27;
                                                }
                                                obj.velocity = new Vector2(0, -1);
                                                obj.position -= new Vector2(0, 8);
                                                obj.scale = 1.2f;
                                            }
                                        }
                                    }
                                    if (player.velocity.Y != 0)
                                    {
                                        player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                                    }
                                }
                                else
                                {
                                    if (item.useStyle != 5 || Item.staff[item.type] || item.GetGlobalItem<MagicGlobalItem>().Handheld)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance + 2);
                                        if (player.velocity.Y == 0)
                                        {
                                            Rotation -= player.fullRotation - 0.3F * player.direction;
                                            //如果玩家速度大于8
                                            if (MaxSpeed)
                                            {
                                                Rotation += player.fullRotation;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //长矛
                                        if (ItemID.Sets.Spears[item.type])
                                        {
                                            vec -= ((MathHelper.PiOver2 - 1.3F) * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance * item.width / 10;

                                            if (player.velocity.Y != 0)
                                            {
                                                player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                                            }
                                        }
                                        vec += (1f * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                        if (player.velocity.Y == 0)
                                        {
                                            Rotation = 1f * player.direction + MathHelper.PiOver2 * player.direction;
                                        }
                                    }
                                }
                            }
                            draw = new DrawData(texture, vec.Floor(), sourceRect, LightColor, Rotation, origin, scale, drawinfo.playerEffect, 0);

                            drawinfo.DrawDataCache.Add(draw);
                            DDHelper.drawGlow(0, item, player, vec.Floor(), sourceRect, Rotation, origin, scale, drawinfo.playerEffect, drawinfo);
                        }
                    }
                }
            }*/
            Player player = drawinfo.drawPlayer;
            bool Speed = player.Dplayer().Speed;
            bool MaxSpeed = player.Dplayer().MaxSpeed;
            Item item = player.inventory[player.selectedItem];
            if (item.type > 0 && !item.DItem().Twin)
            {
                return;
            }
            if (player.PlayerAction().ThereShield2)
            {
                player.PlayerAction().ThereShield2 = false;
                return;
            }
            if (!DDPlayer.UseBoomerang(player.ActiveItem(), player))
            {
                return;
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            bool TypeBool = item.type == 4760 || (item.type > 0 && item.DItem().NoDraw);
            if (player.itemAnimation == 0 && item.type > 0 && !TypeBool && !item.GetGlobalItem<RangedGlobalItem>().Bow && player.ActiveItem().holdStyle == 0 && !player.PlayerAction().SwimStatus)
            {
                DrawData draw;

                Color color = Color.White;
                Color LightColor = Lighting.GetColor((int)(player.Center.X / 16), (int)(player.Center.Y / 16), color);
                float scale = player.GetAdjustedItemScale(item);

                Texture2D texture = TextureAssets.Item[player.inventory[player.selectedItem].type].Value;
                if (item.DItem().Twin)
                {
                    Main.instance.LoadProjectile(item.shoot);
                    texture = TextureAssets.Projectile[item.shoot].Value;
                }
                if (texture == null)
                {
                    return;
                }
                float frame = (Main.itemAnimations[item.type] == null) ? 1 : Main.itemAnimations[item.type].FrameCount;
                float LargeWeapons = 0;
                //大型武器
                if (texture != null)
                {
                    LargeWeapons = new Vector2(texture.Width, texture.Height / frame).Length() * player.GetAdjustedItemScale(item);
                    if (item.DItem().Twin)
                    {
                        LargeWeapons = new Vector2(texture.Width / 2, texture.Height).Length() * player.GetAdjustedItemScale(item);
                    }
                }
                Vector2 vec = player.MountedCenter - Main.screenPosition + new Vector2(-6 * player.direction, 0) + new Vector2(10 * player.direction, 0);
                if (player.portableStoolInfo.IsInUse)
                {
                    //vec.Y += player.portableStoolInfo.HeightBoost / 2;
                }
                Rectangle? sourceRect = new Rectangle?((Main.itemAnimations[item.type] == null) ? Utils.Frame(texture, 1, 1, 0, 0, 0, 0) : Main.itemAnimations[item.type].GetFrame(texture, -1));
                if (item.DItem().Twin)
                {
                    sourceRect = new Rectangle?(Utils.Frame(texture, 2, 1, 1, 0, 0, 0));
                }
                ItemSlot.GetItemLight(ref LightColor, ref scale, item, false);
                LightColor = player.GetImmuneAlpha(item.GetAlpha(LightColor) * player.stealth, 0f);
                if (item.DItem().HandheldColor != new Color(0, 0, 0, 0))
                {
                    LightColor = item.DItem().HandheldColor;
                }
                //让物品放在手中间
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frame / 2);

                //旋转
                float Rotation = player.Aplayer().ArmSwing * player.direction + 0.5F * player.direction;
                //距离
                float Distance = texture.Height / frame / 2;

                // -1默认
                int HandheldStyle = -1;
                //不属于任何职业而且使用方式是1
                if (HandheldStyle == -1)
                {
                    if (item.DamageType == DamageClass.Default && item.useStyle == 1)
                    {
                        HandheldStyle = 0;
                    }
                }
                if (HandheldStyle == -1)
                {
                    //绘制近战武器
                    if (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed)
                    {
                        if (item.useStyle > 0 && (item.useStyle != 5 || ItemID.Sets.Spears[item.type]))
                        {
                            HandheldStyle = 1;
                        }
                    }
                }
                if (HandheldStyle == -1)
                {
                    //法师武器
                    if (item.DamageType == DamageClass.Magic)
                    {
                        if (Item.staff[item.type] || item.useStyle == 1 || item.GetGlobalItem<MagicGlobalItem>().Handheld)
                        {
                            HandheldStyle = 1;
                        }
                        else
                        if (item.useStyle == 5)
                        {
                            HandheldStyle = 3;
                        }
                    }
                }
                if (HandheldStyle == -1)
                {
                    //远程武器
                    if (item.DamageType == DamageClass.Ranged && item.maxStack == 1)
                    {
                        HandheldStyle = 3;
                    }
                }
                if (HandheldStyle == -1)
                {
                    //绘制召唤武器
                    if (item.DamageType == DamageClass.Summon || item.DamageType == DamageClass.SummonMeleeSpeed)
                    {
                        if (item.useStyle > 0 && (item.useStyle != 5 || Item.staff[item.type]))
                        {
                            HandheldStyle = 1;
                        }
                        if (!Item.staff[item.type] && item.useStyle == 5)
                        {
                            HandheldStyle = 2;
                        }
                    }
                }
                if (item.DItem().DrawMelee)
                {
                    HandheldStyle = 1;
                }
                if (item.DItem().DrawRanged)
                {
                    HandheldStyle = 3;
                }
                //绘制飞刀
                if (DGlobalItem.FlyingKnife[item.type])
                {
                    HandheldStyle = 2;
                }
                //使用方式为1的物品
                if (HandheldStyle == 0)
                {
                    origin = new Vector2(2, texture.Height / frame - 2);
                    Distance = 8;
                    if (player.direction < 0)
                    {
                        origin = new Vector2(texture.Width - 2, texture.Height / frame - 2);
                    }
                    if (player.PlayerAction().Display)
                    {
                        Rotation = -player.Aplayer().ArmSwing/2 * player.direction + 0.5F * player.direction;
                        Rotation += MathHelper.PiOver2 * player.direction;
                    }
                }
                //剑类物品的手持方式
                if (HandheldStyle == 1)
                {
                    origin = new Vector2(2, texture.Height / frame - 2);
                    Distance = 8;
                    if (player.direction < 0)
                    {
                        origin = new Vector2(texture.Width / 2 - 2, texture.Height / frame - 2);

                    }
                    Rotation = player.Aplayer().ArmSwing * player.direction + 0.5F * player.direction;
                    if (player.PlayerAction().Display)
                    {
                        Rotation = -player.Aplayer().ArmSwing/2 * player.direction + 0.5F * player.direction;
                        Rotation += MathHelper.PiOver2 * player.direction;
                    }
                }
                //飞刀类物品的手持方式
                if (HandheldStyle == 2)
                {
                    origin = new Vector2(texture.Width / 2, texture.Height / frame - 2);
                    Distance = 8;
                    if (player.direction < 0)
                    {
                        origin = new Vector2(texture.Width / 2, texture.Height / frame - 2);
                        Rotation += MathHelper.PiOver4;
                    }
                    else
                    {

                        Rotation -= MathHelper.PiOver4;
                    }
                }

                //枪类物品的手持方式
                if (HandheldStyle == 3)
                {
                    origin = new Vector2(player.direction == -1 ? texture.Width / 2 - (texture.Width / 2 * 0.3F) : (texture.Width / 2 * 0.3F), texture.Height / frame / 2);
                    
                    Distance = texture.Height / frame / 2;
                    Rotation = player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2 * player.direction;
                    if (player.PlayerAction().Display)
                    {
                        Rotation = -player.Aplayer().ArmSwing/2 * player.direction + MathHelper.PiOver2 * player.direction;
                    }
                    if (LargeWeapons > 50)
                    {
                        if (Speed)
                        {
                            player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                        }
                    }
                }
                //如果武器是拿在手上的
                if (player.PlayerAction().WeaponType < HandheldWeaponType.Back)
                {
                    //if (player.inventory[player.selectedItem].DamageType == DamageClass.Melee)
                    {
                        //如果在坐骑上或者在没跑路
                        if (!Speed || player.mount.Active || player.mount.Cart)
                        {
                            if (player.PlayerAction().Display)
                            {
                                vec += ((-player.Aplayer().ArmSwing/2) * player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance);
                            }
                            else
                            {

                                vec += (player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                            }
                                Rotation += item.DItem().DrawRot * player.direction;

                            vec.Y += drawinfo.drawPlayer.gfxOffY;
                            vec += Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / 56];
                            draw = new DrawData(texture, vec.Floor(), sourceRect, LightColor, Rotation, origin, scale, drawinfo.playerEffect, 0);
                            drawinfo.DrawDataCache.Add(draw);
                            DDHelper.drawGlow(0, item, player, vec.Floor(), sourceRect, Rotation, origin, scale, drawinfo.playerEffect, drawinfo);

                        }
                        else
                        {
                            if (!player.Aplayer().Ninja)
                            {
                                if (LargeWeapons > 100 && player.PlayerAction().WeaponType == HandheldWeaponType.Hand)
                                {
                                    vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    Rotation = MathHelper.Pi + 0.65F * player.direction - player.fullRotation;
                                    for (int a = 0; a < 3; a++)
                                    {
                                        int i = (int)(((player.Center.X + player.width / 2 * player.direction) - texture.Width * 1.5f * scale * player.direction) / 16 + Main.rand.Next((int)(-1 * scale), (int)(1 * scale)));
                                        int j = (int)((player.position.Y + player.height) / 16);
                                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType != 5)
                                        {
                                            Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, Main.tile[i, j])];
                                            if (item.type == 121)
                                            {
                                                obj.type = 6;
                                            }
                                            if (item.type == ModContent.ItemType<火山长剑>())
                                            {
                                                if (Main.rand.NextBool(8))
                                                    obj.type = 6;
                                            }
                                            if (item.type == ModContent.ItemType<ShadowFlameSwordItem>())
                                            {
                                                obj.type = 27;
                                            }
                                            obj.velocity = new Vector2(0, -1);
                                            obj.position -= new Vector2(0, 8);
                                            obj.scale = 1.2f;
                                        }
                                        else if (!Main.tile[i, j].HasTile && Main.tile[i, j].LiquidAmount > 0)
                                        {
                                            if (Main.tile[i, j].LiquidType == 0)
                                            {
                                                int t = 33;
                                                if (item.type == 121)
                                                {
                                                    t = 31;
                                                    PlaySound(SoundID.LiquidsWaterLava, new Vector2(i, j) * 16);
                                                }
                                                if (item.type == ModContent.ItemType<火山长剑>())
                                                {
                                                    if (Main.rand.NextBool(8))
                                                    {
                                                        t = 31;
                                                        PlaySound(SoundID.LiquidsWaterLava, new Vector2(i, j) * 16);
                                                    }
                                                }
                                                Dust obj = Main.dust[NewDust(new Vector2(i, j) * 16, 16, 1, t)];
                                                if (item.type == ModContent.ItemType<ShadowFlameSwordItem>())
                                                {
                                                    obj.type = 27;
                                                }
                                                obj.velocity = new Vector2(0, -2);
                                                obj.scale = 2.2f;
                                            }
                                        }
                                    }
                                    if (player.velocity.Y != 0)
                                    {
                                        player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                                    }
                                }
                                else
                                {
                                    vec += (player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    if (player.velocity.Y != 0)
                                    {
                                        vec.Y -= 20;
                                        Rotation += 1 * player.direction;
                                    }
                                    //Rotation = -player.Aplayer().ArmSwing * player.direction + 0.5F * player.direction;
                                    if (HandheldStyle == 3)
                                    {
                                        Rotation = player.Aplayer().ArmSwing * player.direction + MathHelper.PiOver2 * player.direction;
                                    }
                                }
                            }
                            else
                            {
                                if (LargeWeapons > 100 && player.PlayerAction().WeaponType == HandheldWeaponType.Hand)
                                {
                                    vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                    Rotation = MathHelper.Pi + 0.65F * player.direction - player.fullRotation;

                                    for (int a = 0; a < 3; a++)
                                    {
                                        int i = (int)(((player.Center.X + player.width / 2 * player.direction) - texture.Width * 1.5f * item.scale * player.direction) / 16 + Main.rand.Next((int)(-1 * item.scale), (int)(1 * item.scale)));
                                        int j = (int)((player.position.Y + player.height) / 16);
                                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType != 5)
                                        {
                                            Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, Main.tile[i, j])];
                                            if (item.type == 121)
                                            {
                                                obj.type = 6;
                                            }
                                            if (item.type == ModContent.ItemType<ShadowFlameSwordItem>())
                                            {
                                                obj.type = 27;
                                            }
                                            obj.velocity = new Vector2(0, -1);
                                            obj.position -= new Vector2(0, 8);
                                            obj.scale = 1.2f;
                                        }
                                        else if (!Main.tile[i, j].HasTile && Main.tile[i, j].LiquidAmount > 0)
                                        {
                                            if (Main.tile[i, j].LiquidType == 1)
                                            {
                                                int t = 33;
                                                if (item.type == 121)
                                                {
                                                    t = 6;
                                                }
                                                Dust obj = Main.dust[NewDust(new Vector2(i, j), 16, 1, t)];
                                                if (item.type == ModContent.ItemType<ShadowFlameSwordItem>())
                                                {
                                                    obj.type = 27;
                                                }
                                                obj.velocity = new Vector2(0, -1);
                                                obj.position -= new Vector2(0, 8);
                                                obj.scale = 1.2f;
                                            }
                                        }
                                    }
                                    if (player.velocity.Y != 0)
                                    {
                                        player.PlayerAction().WeaponType = HandheldWeaponType.Back;
                                    }
                                }
                                else
                                {
                                    if (HandheldStyle == -1)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance - 2);
                                        Rotation = (MathHelper.PiOver2 + 1) * player.direction;
                                    }
                                    if (HandheldStyle == 0)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance - 2);
                                        Rotation = (MathHelper.PiOver2 + 1) * player.direction;
                                    }
                                    if (HandheldStyle == 1)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance + 2);
                                        Rotation = MathHelper.PiOver4 * player.direction;
                                        if (player.velocity.Y == 0)
                                        {
                                            Rotation -= player.fullRotation - 0.3F * player.direction;
                                            //如果玩家速度大于8
                                            if (MaxSpeed)
                                            {
                                                Rotation += player.fullRotation;
                                            }
                                        }
                                    }
                                    if (HandheldStyle == 2)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * (Distance + 2);
                                        Rotation = MathHelper.PiOver2 * player.direction;
                                    }
                                    if (HandheldStyle == 3)
                                    {
                                        vec += (player.direction + MathHelper.PiOver2).ToRotationVector2() * Distance;
                                        //如果玩家速度大于8
                                        if (Speed)
                                        {
                                            Rotation = MathHelper.PiOver2 * player.direction + 1f * player.direction;
                                        }
                                    }
                                }
                            }
                            Rotation += item.DItem().DrawRot * player.direction;
                            vec.Y += drawinfo.drawPlayer.gfxOffY;
                            vec += Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / 56];
                            draw = new DrawData(texture, vec.Floor(), sourceRect, LightColor, Rotation, origin, scale, drawinfo.playerEffect, 0);

                            drawinfo.DrawDataCache.Add(draw);
                            DDHelper.drawGlow(0, item, player, vec.Floor(), sourceRect, Rotation, origin, scale, drawinfo.playerEffect, drawinfo);
                        }
                    }
                }
            }
            return;
        }
    }
    //绘制武器放在背上
    public class DrawWeaponBack : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.BackAcc);
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            return drawInfo.shadow == 0f && !drawPlayer.dead && drawPlayer.PlayerAction().Action && drawPlayer.PlayerAction().ShowWeapons;
        }
        protected override void Draw(ref PlayerDrawSet drawinfo)
        {
            Player player = drawinfo.drawPlayer;
            Item item = player.inventory[player.selectedItem];
            bool TypeBool = item.type == 4760;
            if (!DDPlayer.UseBoomerang(player.ActiveItem(), player))
            {
                return;
            }
            if (player.itemAnimation == 0 && player.ActiveItem().type > 0 && !TypeBool && !player.ActiveItem().GetGlobalItem<RangedGlobalItem>().Bow && player.ActiveItem().holdStyle == 0)
            {
                DrawData draw;
                Color color = Color.White;
                Color LightColor = Lighting.GetColor((int)(player.Center.X / 16), (int)(player.Center.Y / 16), color);
                float scale = player.GetAdjustedItemScale(item);
                Texture2D texture = TextureAssets.Item[player.inventory[player.selectedItem].type].Value;
                if (texture == null)
                {
                    return;
                }
                Vector2 vec = player.MountedCenter - Main.screenPosition;

                Rectangle? sourceRect = new Rectangle?((Main.itemAnimations[item.type] == null) ? Utils.Frame(texture, 1, 1, 0, 0, 0, 0) : Main.itemAnimations[item.type].GetFrame(texture, -1));
                float frame = (Main.itemAnimations[item.type] == null) ? 1 : Main.itemAnimations[item.type].FrameCount;
                ItemSlot.GetItemLight(ref LightColor, ref scale, item, false);
                LightColor = player.GetImmuneAlpha(item.GetAlpha(LightColor) * player.stealth, 0f);

                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frame / 2);

                float Rotation = player.bodyRotation + MathHelper.Pi;
                SpriteEffects sprite = drawinfo.playerEffect;
                if ((item.useStyle == 5 || item.DamageType == DamageClass.Ranged || item.DItem().DrawRanged || (item.DamageType == DamageClass.Magic&& !Item.staff[item.type])) && !Item.staff[item.type] && !ItemID.Sets.Spears[item.type])
                {
                    Rotation -= MathHelper.PiOver4 * player.direction;
                }
                if (item.useAmmo == AmmoID.Arrow)
                {
                    Rotation += MathHelper.Pi;
                }
                if (player.PlayerAction().WeaponType == HandheldWeaponType.Back || player.PlayerAction().SwimStatus)
                {
                    if (player.PlayerAction().SwimStatus || item.damage > 0)
                    {
                        if (item.DItem().HandheldColor != new Color(0, 0, 0, 0))
                        {
                            LightColor = item.DItem().HandheldColor;
                        }
                        Rotation += item.DItem().DrawRot * player.direction;
                        vec.Y += drawinfo.drawPlayer.gfxOffY;
                        vec += Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / 56];
                        draw = new DrawData(texture, vec.Floor(), sourceRect, LightColor, Rotation, origin, scale, sprite, 0);
                        drawinfo.DrawDataCache.Add(draw);
                        int A = item.DItem().TwinGlow; 
                        item.DItem().TwinGlow = 0;
                        DDHelper.drawGlow(0, item, player, vec.Floor(), sourceRect, Rotation, origin, scale, drawinfo.playerEffect, drawinfo);
                        item.DItem().TwinGlow = A;
                    }
                }
            }
            return;
        }
    }
}
