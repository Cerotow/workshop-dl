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
using DDmod.Worlds;
using DDmod.Content.Tiles.农场;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.ModLoader;
using DDmod.Content.Tiles.花岗岩基地;
using StructureHelper;
using DDmod.Content.Tiles.流星;
using DDmod.Content;
using Terraria.ObjectData;
using DDmod.Content.Tiles;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Melee.SwordShield;
using DDmod.Content.Items.Melee.Sword.Make;
using DDmod.Content.Items.Melee.FlyingKnife.Make;
using DDmod.Content.Items.Summon;
using DDmod.Content.Items.Boss.流星破坏者;
using Terraria.ModLoader.IO;
using Terraria.IO;
using Terraria.GameContent.UI.States;
using System;
using Microsoft.CodeAnalysis;
using Terraria.ModLoader.UI;
using Terraria.Localization;

namespace DDmod.DDOn
{
    public class UIButton : UIElement
    {
        private Asset<Texture2D> _texture;
        private float _visibilityActive = 1f;
        private float _visibilityInactive = 0.4f;
        private Asset<Texture2D> _borderTexture;

        public UIButton(Asset<Texture2D> texture)
        {
            _texture = texture;
            Width.Set(_texture.Width(), 0f);
            Height.Set(_texture.Height(), 0f);
        }

        public void SetHoverImage(Asset<Texture2D> texture)
        {
            _borderTexture = texture;
        }

        public void SetImage(Asset<Texture2D> texture)
        {
            _texture = texture;
            Width.Set(_texture.Width(), 0f);
            Height.Set(_texture.Height(), 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            if (Mode == 0)
            {
                spriteBatch.Draw(_texture.Value, dimensions.Position(), Color.White * (base.IsMouseHovering ? _visibilityActive : _visibilityInactive));
                if (_borderTexture != null && base.IsMouseHovering)
                    spriteBatch.Draw(_borderTexture.Value, dimensions.Position(), Color.White);
            }
            else
            {
                spriteBatch.Draw(_texture.Value, dimensions.Position(), Color.White);
                spriteBatch.Draw(_texture.Value, dimensions.Position(), new Color(255, 100, 100, 0));
                if (_borderTexture != null && base.IsMouseHovering)
                    spriteBatch.Draw(_borderTexture.Value, dimensions.Position(), new Color(255,255,255,0));
            }
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
        }

        public void SetVisibility(float whenActive, float whenInactive)
        {
            _visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
            _visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
        }
    }
    internal static class DDmodOn
    {
        public static int Start;
        public static void Load()
        {
            ProjectileOn.Load();
            NPCOn.Load();
            PlayerOn.Load();
            Terraria.On_WorldGen.oceanDepths += oceanDepths;
            Terraria.On_Main.DrawDust += Main_DrawDust;
            Terraria.On_Dust.NewDust += Dust_NewDust;
            Terraria.On_Main.UpdateAudio_DecideOnNewMusic += UpdateAudio_DecideOnNewMusic;
            Terraria.On_WorldGen.KillTile += KillTile;
            Terraria.On_Main.DoDraw += DoDraw;
            Terraria.On_Main.DrawInfernoRings += DrawInfernoRings;
            Terraria.On_Rain.Update += Rain_Update;
            Terraria.On_Collision.SolidCollision_Vector2_int_int += SolidCollision;
            Terraria.On_Collision.SolidCollision_Vector2_int_int_bool += SolidCollision2;
            Terraria.On_Collision.TileCollision += TileCollision;
            Terraria.On_Main.DrawNPCHeadBoss += DrawNPCHeadBoss;
            //Terraria.On_WorldGen.TileFrame += TileFrame;
            Terraria.On_WorldGen.UpdateWorld_GrassGrowth += UpdateWorld_GrassGrowth;
            Terraria.On_WorldGen.meteor += meteor;
            Terraria.UI.On_ItemSlot.LeftClick_ItemArray_int_int += LeftClick;
            Terraria.On_Wiring.HitSwitch += HitSwitch;
            Terraria.On_WorldGen.PlaceChestDirect += PlaceChestDirect;

            On_AWorldListItem.GetDifficulty += GetDifficulty;

            On_UIWorldCreation.FinishCreatingWorld += FinishCreatingWorld;

            On_UIWorldCreation.MakeInfoMenu += MakeInfoMenu;

            method = typeof(Main).GetMethod("get_UnderworldLayer", BindingFlags.Static | BindingFlags.Public);
            MonoModHooks.Add(method, MyUnderLayer);
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate int orig_UnderworldLayer();
        [EditorBrowsable(EditorBrowsableState.Never)]
        public delegate int hook_UnderworldLayer(orig_UnderworldLayer orig);
        public static int MyUnderLayer()
        {
            if (SWSystem.UnderworldHeight>=0)
            {
                return SWSystem.UnderworldHeight;
            }
            return Main.maxTilesY - 200;
        }
        public static MethodBase method;
        public static void FinishCreatingWorld(On_UIWorldCreation.orig_FinishCreatingWorld orig, UIWorldCreation uI)
        {
            orig(uI);
            if(Mode==1)
            {
                DDWorld.晨曦 = true;
            }
        }
        public static void MakeInfoMenu(On_UIWorldCreation.orig_MakeInfoMenu orig, UIWorldCreation uI,UIElement uIElement)
        {
            orig(uI, uIElement);
            Mode = 0;
            /*
            UIPanel uIPanel = new UIPanel
            {
                Width = StyleDimension.FromPixels(60f),
                Height = StyleDimension.FromPixels(60f),
                Left = StyleDimension.FromPixels(0f),
                Top = StyleDimension.FromPixels(0f),
                BackgroundColor = new Color(233, 43, 79) * 0.8f
            };*/
            UIButton button = new UIButton(ModContent.Request<Texture2D>("DDmod/晨曦模式"));

            button.Width.Set(68,0);
            button.Height.Set(68,0);
            //设置按钮距离所属ui部件的最左端的距离
            button.Left.Set(260, 0.5F);
            //设置按钮距离所属ui部件的最顶端的距离
            button.Top.Set(210, 0F);

            //uIPanel.SetPadding(0f);
            button.OnLeftMouseDown += Element_OnLeftMouseDown;
            button.OnMouseOver += Button_OnMouseOver;
            button.OnMouseOut += Button_OnMouseOut;
            uI.Append(button);
            //uIElement.Append(element);
            void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
            {
                UIText S = (UIText)DDHelper.FieldReflection(uI.GetType(), "_descriptionText", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(uI);
                S.SetText(Language.GetText("Mods.DDmod.CreateWorld.晨曦提示"));
            }
            void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
            {
                UIText S = (UIText)DDHelper.FieldReflection(uI.GetType(), "_descriptionText", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(uI);
                S.SetText(Language.GetText("UI.WorldDescriptionDefault"));
            }
        }


        public static byte Mode = 0;
        private static void Element_OnLeftMouseDown(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Mode == 0)
            {
                Mode = 1;
                SoundEngine.PlaySound(DDHelper.SoundStyle(1,"点"));
            }
            else
            {
                Mode = 0;
                SoundStyle sound = DDHelper.SoundStyle(1, "点");
                sound.Pitch = -1;
                SoundEngine.PlaySound(sound);
            }
        }


        public static void GetDifficulty(On_AWorldListItem.orig_GetDifficulty orig, AWorldListItem item,out string Text,out Color color)
        {
            orig(item, out Text, out color);

            bool A = item.Data.TryGetHeaderData<DDWorld>(out TagCompound data);
            if(A)
            {
                bool I = data.GetBool("晨曦");
                if(I)
                {
                    Text += "+"+ Language.GetText("Mods.DDmod.CreateWorld.晨曦").Value;
                    color = new Color(255, 0, 255);
                }
            }
        }
        public static void PlaceChestDirect(On_WorldGen.orig_PlaceChestDirect orig, int x, int y, ushort type, int style, int id)
        {
            if (type == ModContent.TileType<绿岩存储仓Tile>())
            {
                Tile tile = Main.tile[x, y];
                Chest.CreateChest(x - 2, y - 2, id);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        tile = Main.tile[x + i - 2, y + j - 2];
                        if (tile == null)
                            tile = new Tile();

                        tile.HasTile = true;
                        tile.TileFrameX = (short)(18 * i+18*4* style);
                        tile.TileFrameY = (short)(18 * j);
                        tile.TileType = type;
                        tile.IsHalfBlock = false;
                    }
                }
            }
            else
                orig(x, y, type, style, id);
        }
        public static void HitSwitch(On_Wiring.orig_HitSwitch orig, int i, int j)
        {
            DDSystem.Wiredens = new(i, j);
            orig(i, j);
            ModTile modTile = TileLoader.GetTile(Main.tile[i, j].TileType);
            if (modTile !=null&& modTile.Mod == DDmod.Instance)
            {
                TileObjectData tileData = TileObjectData.GetTileData(Main.tile[i, j].TileType, 0, 0);
                if (tileData != null)
                {
                    Wiring.TripWire(i, j, tileData.Width, tileData.Height);
                }
                else
                {
                    Wiring.TripWire(i, j, 1, 1);
                }
                Tile tile = Main.tile[i, j];
                TileHelp.HitSwitch(i,j);
            }
        }
        public static void LeftClick(On_ItemSlot.orig_LeftClick_ItemArray_int_int orig, Item[] inv, int context = 0, int slot = 0)
        {
            Main.LocalPlayer.Dplayer().Itemslot = slot;
            if (Main.LocalPlayer.TPlayer().UseTalisman&& inv[slot].type>0 && inv[slot].Titem().Talisman)
            {
                return;
            }
            orig(inv, context, slot);

        }
        public static bool meteor(On_WorldGen.orig_meteor orig, int i, int j, bool ignorePlayers = false)
        {
            //流星塔大小
            //1:62x36y
            //2:59x48y
            //2:71x44y
            bool M = orig(i, j, ignorePlayers);
            if (M && !NPCDowned.MeteorTower)
            {
                NPCDowned.MeteorTower = true;
                int R = Main.rand.Next(3) + 1;
                int I2 = i + (Main.rand.NextBool(2) ? -100 : 50);
                if (R == 1)
                {
                    I2 = i + 80;
                }
                int J2 = j - 200;
                for (int a = 0; a < 300; a++)
                {
                    if (I2 < 0)
                    {
                        I2 = 0;
                    }
                    if (J2 < 0)
                    {
                        J2 = 0;
                    }
                    if (Main.tile[I2, J2].HasTile && Main.tileSolid[Main.tile[I2, J2].TileType])
                    {
                        break;
                    }
                    J2++;
                }
                int H = 36;
                if (R == 2)
                {
                    H = 48;
                }
                if (R == 3)
                {
                    H = 44;
                }
                GenerateStructure("Worlds/流星塔" + R, new Point16(I2, J2 - H - 1), DDmod.Instance);
                int L = 62;
                if (R == 2)
                {
                    L = 59;
                }
                if (R == 3)
                {
                    L = 71;
                }
                for (int a = 0; a < L; a++)
                {
                    for (int b = -1; b < 30; b++)
                    {
                        Tile tile = Main.tile[I2 + a, J2 + b];
                        if (tile.HasTile && Main.tileSolid[tile.TileType])
                        {
                            tile.Slope = 0;
                            tile.IsHalfBlock = false;
                            WorldGen.SquareTileFrame(I2 + a, J2 + b, true);
                            break;
                        }
                        tile.TileType = (ushort)370;
                        tile.HasTile = true;
                        WorldGen.SquareTileFrame(I2 + a, J2 + b, true);
                    }
                }
                if (R == 1)
                {
                    for (int a = -8; a < 0; a++)
                    {
                        for (int b = -3; b < 30; b++)
                        {
                            Tile tile = Main.tile[I2 + a, J2 + b];
                            if (tile.HasTile && Main.tileSolid[tile.TileType])
                            {
                                tile.Slope = 0;
                                tile.IsHalfBlock = false;
                                WorldGen.SquareTileFrame(I2 + a, J2 + b, true);
                                break;
                            }
                            tile.TileType = (ushort)370;
                            tile.HasTile = true;
                            WorldGen.SquareTileFrame(I2 + a, J2 + b, true);
                        }
                    }
                }
                J2 -= H + 1;
                bool Boss = false;
                if (R == 1)
                {
                    //WorldGen.KillTile(I2 + 10, J2 + 33);
                    MeteorBox(I2 + 10, J2 + 33, ref Boss);
                    //WorldGen.KillTile(I2 + 26, J2 + 33);
                    MeteorBox(I2 + 26, J2 + 33, ref Boss);
                    //WorldGen.KillTile(I2 + 39, J2 + 26);
                    MeteorBox(I2 + 39, J2 + 26, ref Boss);
                    //WorldGen.KillTile(I2 + 39, J2 + 21);
                    MeteorBox(I2 + 39, J2 + 21, ref Boss, true);

                }
                else if (R == 2)
                {

                    MeteorBox(I2 + 20, J2 + 37, ref Boss);
                    MeteorBox(I2 + 26, J2 + 44, ref Boss);
                    MeteorBox(I2 + 29, J2 + 44, ref Boss);
                    MeteorBox(I2 + 35, J2 + 37, ref Boss, true);

                }
                else if (R == 3)
                {
                    MeteorBox(I2 + 22, J2 + 41, ref Boss);
                    MeteorBox(I2 + 31, J2 + 22, ref Boss);
                    MeteorBox(I2 + 36, J2 + 22, ref Boss);
                    MeteorBox(I2 + 45, J2 + 41, ref Boss, true);

                }
                //NoBoss 最后一个箱子,如果没有boss召唤物强行生成一个
                void MeteorBox(int x, int y, ref bool Boss, bool NoBoss = false)
                {
                    Tile tile = Main.tile[x, y];
                    tile.HasTile = false;
                    tile = Main.tile[x+1, y];
                    tile.HasTile = false;
                    tile = Main.tile[x, y-1];
                    tile.HasTile = false;
                    tile = Main.tile[x+1, y-1];
                    tile.HasTile = false;

                    int PlacementSuccess = WorldGen.PlaceChest(x, y, (ushort)21, false, 49);
                    if (PlacementSuccess >= 0)
                    {

                        Random ran = new();
                        Chest chest = Main.chest[PlacementSuccess];
                        //chest.name = Language.GetTextValue("ItemName.MeteoriteChest");
                        int Citem = 0;
                        chest.item[Citem].SetDefaults(ModContent.ItemType<可疑外星蓝图>(), false);
                        chest.item[Citem].stack = 1;
                        Citem++;
                        if (Main.rand.NextBool(2))
                        {
                            chest.item[Citem].SetDefaults(Main.rand.NextBool(20) ? ModContent.ItemType<流星剑盾>() : Main.rand.Next(new int[] { ModContent.ItemType<MeteorSword>(), 127, ModContent.ItemType<流星投刀>(), ModContent.ItemType<MeteorDroneController>() }), false);
                            chest.item[Citem].stack = 1;
                            chest.item[Citem].Prefix(-1);
                            Citem++;
                        }
                        else
                        {
                            chest.item[Citem].SetDefaults(Main.rand.NextBool(5) ? 197 : Main.rand.Next(new int[] { 198, 199, 200, 201, 202, 203, 4258, 204 }), false);
                            chest.item[Citem].stack = 1;
                            chest.item[Citem].Prefix(-1);
                            Citem++;
                        }
                        chest.item[Citem].SetDefaults(ModContent.ItemType<流星电池>(), false);
                        chest.item[Citem].stack = ran.Next(1, 4);
                        Citem++;
                        if (!Boss && (Main.rand.NextBool(4) || NoBoss))
                        {
                            chest.item[Citem].SetDefaults(ModContent.ItemType<AlienRigController>(), false);
                            chest.item[Citem].stack = 1;
                            Citem++;
                            Boss = true;
                        }
                        if (Main.rand.NextBool(3))
                        {
                            chest.item[Citem].SetDefaults(117, false);
                            chest.item[Citem].stack = ran.Next(3, 9);
                            Citem++;
                        }
                        else
                        {
                            chest.item[Citem].SetDefaults(116, false);
                            chest.item[Citem].stack = ran.Next(6, 13);
                            Citem++;

                        }
                        if (Main.rand.NextBool(3))
                        {
                            chest.item[Citem].SetDefaults(117, false);
                            chest.item[Citem].stack = ran.Next(3, 9);
                            Citem++;
                        }
                        else
                        {
                            chest.item[Citem].SetDefaults(116, false);
                            chest.item[Citem].stack = ran.Next(6, 13);
                            Citem++;

                        }
                    }
                    NetMessage.SendObjectPlacement(-1, x, y, 21, 0, 0, -1, -1);
                }
            }
            return M;
        }
        public static void UpdateWorld_GrassGrowth(On_WorldGen.orig_UpdateWorld_GrassGrowth orig, int i, int j, int minI, int maxI, int minJ, int maxJ, bool underground)
        {
            bool T = NPC.downedMechBossAny;
            try
            {
                if (NPCDowned.觉醒星心双子)
                {
                    NPC.downedMechBossAny = true;
                }
                else
                {
                    NPC.downedMechBossAny = false;
                }
                orig(i, j, minI, maxI, minJ, maxJ, underground);
            }
            finally
            {
                NPC.downedMechBossAny = T;
            }
        }
        public static void TileFrame(On_WorldGen.orig_TileFrame orig, int i, int j, bool resetFrame = false, bool noBreak = false)
        {
            //orig(i, j, resetFrame, noBreak);
            /*
            Tile tile = Main.tile[i, j];
            if (Main.rand.NextBool(2)&&tile.TileType == ModContent.TileType<花岗岩科技砖Tile>())
            {
                tile.TileFrameY += 90;
            }*/
        }
        public static void DrawNPCHeadBoss(On_Main.orig_DrawNPCHeadBoss orig, Entity theNPC, byte alpha, float headScale, float rotation, SpriteEffects effects, int bossHeadId, float x, float y)
        {
            Main.BossNPCHeadRenderer.DrawWithOutlines(theNPC, bossHeadId, new Vector2(x, y), Main.npc[theNPC.whoAmI].GetAlpha(Color.White), rotation, headScale, effects);
        }
        public static Vector2 TileCollision(On_Collision.orig_TileCollision orig, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
        {
            Collision.up = false;
            Collision.down = false;
            Vector2 result = Velocity;
            Vector2 vector = Velocity;
            Vector2 vector2 = Position + Velocity;
            Vector2 vector3 = Position;
            int value = (int)(Position.X / 16f) - 1;
            int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
            int value3 = (int)(Position.Y / 16f) - 1;
            int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
            int num = -1;
            int num2 = -1;
            int num3 = -1;
            int num4 = -1;
            int num5 = Utils.Clamp(value, 0, Main.maxTilesX - 1);
            value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
            value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
            value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
            float num6 = (value4 + 3) * 16;
            Vector2 vector4 = default(Vector2);
            for (int i = num5; i < value2; i++)
            {
                for (int j = value3; j < value4; j++)
                {
                    if (Main.tile[i, j] == null || !Main.tile[i, j].HasTile || !Main.tile[i, j].HasUnactuatedTile || (!Main.tileSolid[Main.tile[i, j].TileType] && (!Main.tileSolidTop[Main.tile[i, j].TileType] || Main.tile[i, j].TileFrameY != 0)))
                    {
                        continue;
                    }

                    vector4.X = i * 16;
                    vector4.Y = j * 16;
                    int num7 = 16;
                    if (Main.tile[i, j].IsHalfBlock)
                    {
                        vector4.Y += 8f;
                        num7 -= 8;
                    }

                    if (!(vector2.X + (float)Width > vector4.X) || !(vector2.X < vector4.X + 16f) || !(vector2.Y + (float)Height > vector4.Y) || !(vector2.Y < vector4.Y + (float)num7))
                    {
                        continue;
                    }

                    bool flag = false;
                    bool flag2 = false;
                    if (Main.tile[i, j].Slope > (SlopeType)2)
                    {
                        if (Main.tile[i, j].Slope == (SlopeType)3 && vector3.Y + Math.Abs(Velocity.X) >= vector4.Y && vector3.X >= vector4.X)
                        {
                            flag2 = true;
                        }

                        if (Main.tile[i, j].Slope == (SlopeType)4 && vector3.Y + Math.Abs(Velocity.X) >= vector4.Y && vector3.X + (float)Width <= vector4.X + 16f)
                        {
                            flag2 = true;
                        }
                    }
                    else if (Main.tile[i, j].Slope > 0)
                    {
                        flag = true;
                        if (Main.tile[i, j].Slope == (SlopeType)1 && vector3.Y + (float)Height - Math.Abs(Velocity.X) <= vector4.Y + (float)num7 && vector3.X >= vector4.X)
                        {
                            flag2 = true;
                        }

                        if (Main.tile[i, j].Slope == (SlopeType)2 && vector3.Y + (float)Height - Math.Abs(Velocity.X) <= vector4.Y + (float)num7 && vector3.X + (float)Width <= vector4.X + 16f)
                        {
                            flag2 = true;
                        }
                    }

                    if (flag2)
                    {
                        continue;
                    }

                    if (vector3.Y + (float)Height <= vector4.Y)
                    {
                        Collision.down = true;
                        if ((!(Main.tileSolidTop[Main.tile[i, j].TileType] && fallThrough) || !(Velocity.Y <= 1f || fall2)) && num6 > vector4.Y)
                        {
                            num3 = i;
                            num4 = j;
                            if (num7 < 16)
                            {
                                num4++;
                            }

                            if (num3 != num && !flag)
                            {
                                result.Y = vector4.Y - (vector3.Y + (float)Height) + ((gravDir == -1) ? (-0.01f) : 0f);
                                num6 = vector4.Y;
                            }
                        }
                    }
                    else if (vector3.X + (float)Width <= vector4.X && !Main.tileSolidTop[Main.tile[i, j].TileType])
                    {
                        if (i >= 1 && Main.tile[i - 1, j] == null)
                        {
                            Tile tile = Main.tile[i - 1, j];
                            tile = default(Tile);
                        }

                        if (i < 1 || (Main.tile[i - 1, j].Slope != (SlopeType)2 && Main.tile[i - 1, j].Slope != (SlopeType)4))
                        {
                            num = i;
                            num2 = j;
                            if (num2 != num4)
                            {
                                result.X = vector4.X - (vector3.X + (float)Width);
                            }

                            if (num3 == num)
                            {
                                result.Y = vector.Y;
                            }
                        }
                    }
                    else if (vector3.X >= vector4.X + 16f && !Main.tileSolidTop[Main.tile[i, j].TileType])
                    {
                        if (Main.tile[i + 1, j] == null)
                        {
                            Tile tile = Main.tile[i + 1, j];
                            tile = default(Tile);
                        }

                        if (Main.tile[i + 1, j].Slope != (SlopeType)1 && Main.tile[i + 1, j].Slope != (SlopeType)3)
                        {
                            num = i;
                            num2 = j;
                            if (num2 != num4)
                            {
                                result.X = vector4.X + 16f - vector3.X;
                            }

                            if (num3 == num)
                            {
                                result.Y = vector.Y;
                            }
                        }
                    }
                    else if (vector3.Y >= vector4.Y + (float)num7 && !Main.tileSolidTop[Main.tile[i, j].TileType])
                    {
                        Collision.up = true;
                        num3 = i;
                        num4 = j;
                        result.Y = vector4.Y + (float)num7 - vector3.Y + ((gravDir == 1) ? 0.01f : 0f);
                        if (num4 == num2)
                        {
                            result.X = vector.X;
                        }
                    }
                }
            }

            return result;
        }
        public static bool SolidCollision(Terraria.On_Collision.orig_SolidCollision_Vector2_int_int orig, Vector2 Position, int Width, int Height)
        {
            int value = (int)(Position.X / 16f) - 1;
            int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
            int value3 = (int)(Position.Y / 16f) - 1;
            int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
            int num = Utils.Clamp(value, 0, Main.maxTilesX - 1);
            value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
            value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
            value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
            Vector2 vector = default(Vector2);
            for (int i = num; i < value2; i++)
            {
                for (int j = value3; j < value4; j++)
                {
                    if (Main.tile[i, j] != null && Main.tile[i, j].HasTile && Main.tile[i, j].HasUnactuatedTile && Main.tileSolid[Main.tile[i, j].TileType] && !Main.tileSolidTop[Main.tile[i, j].TileType])
                    {
                        vector.X = i * 16;
                        vector.Y = j * 16;
                        int num2 = 16;
                        if (Main.tile[i, j].IsHalfBlock)
                        {
                            vector.Y += 8f;
                            num2 -= 8;
                        }
                        if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num2)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public static bool SolidCollision2(Terraria.On_Collision.orig_SolidCollision_Vector2_int_int_bool orig, Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
        {
            int value = (int)(Position.X / 16f) - 1;
            int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
            int value3 = (int)(Position.Y / 16f) - 1;
            int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
            int num = Utils.Clamp(value, 0, Main.maxTilesX - 1);
            value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
            value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
            value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
            Vector2 vector = default(Vector2);
            for (int i = num; i < value2; i++)
            {
                for (int j = value3; j < value4; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile == null || !tile.HasTile|| !Main.tile[i, j].HasUnactuatedTile)
                    {
                        continue;
                    }

                    bool flag = Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];
                    if (acceptTopSurfaces)
                    {
                        flag |= (Main.tileSolidTop[tile.TileType] && tile.TileFrameY == 0);
                    }

                    if (flag)
                    {
                        vector.X = i * 16;
                        vector.Y = j * 16;
                        int num2 = 16;
                        if (tile.IsHalfBlock)
                        {
                            vector.Y += 8f;
                            num2 -= 8;
                        }

                        if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num2)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public static void Rain_Update(Terraria.On_Rain.orig_Update orig, Rain rain)
        {
            if (Main.gamePaused)
                return;

            rain.position += rain.velocity;
            if (Main.gameMenu)
            {
                if (rain.position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 2000f)
                    rain.active = false;
            }
            else if (Collision.SolidCollision(rain.position, 2, 2) || rain.position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 100f || Collision.WetCollision(rain.position, 2, 2))
            {
                Vector2 Position = rain.position / 16.0f;
                for (int a = 0; a < 2; a++)
                {
                    for (int b = 0; b < 2; b++)
                    {
                        if (Position.X + a > 0 && Position.X + a < Main.maxTilesX && Position.Y + 1 + b > 0 && Position.Y + 1 + b < Main.maxTilesY)
                        {
                            if (Main.tile[(int)Position.X + a, (int)Position.Y + 1 + b].TileType == ModContent.TileType<锄过的土块>())
                            {
                                if (锄土.FindFirstTile(new Point16((int)Position.X + a, (int)Position.Y + 1 + b), out int type) >= 0)
                                {
                                    if (Main.netMode != 1)
                                    {
                                        DDWorld.土[type].DampTime = DDHelper.Second(300);

                                    }
                                    if (Main.netMode == NetmodeID.MultiplayerClient)
                                    {
                                        ModPacket packet = DDmod.Instance.GetPacket(256);
                                        //写入要发的包
                                        packet.Write((byte)DDType.锄地);
                                        packet.Write((short)(Position.X + a));
                                        packet.Write((short)(Position.Y + 1 + b));
                                        packet.Write((short)type);
                                        packet.Write(DDHelper.Second(300));
                                        //发出去
                                        packet.Send(-1, -1);
                                    }
                                }
                            }
                        }
                    }
                }
                rain.active = false;
                if ((float)Main.rand.Next(100) < Main.gfxQuality * 100f)
                {
                    int num = Dust.NewDust(rain.position - rain.velocity, 2, 2, Dust.dustWater());
                    Main.dust[num].position.X -= 2f;
                    Main.dust[num].position.Y += 2f;
                    Main.dust[num].alpha = 38;
                    Main.dust[num].velocity *= 0.1f;
                    Main.dust[num].velocity += -rain.velocity * 0.025f;
                    Main.dust[num].velocity.Y -= 2f;
                    Main.dust[num].scale = 0.6f;
                    Main.dust[num].noGravity = true;
                }
            }
        }
        public static void KillTile(Terraria.On_WorldGen.orig_KillTile orig, int i,int j, bool fail, bool effectOnly, bool noItem)
        {
            if(!SWSystem.ForbidVandalism)
            orig(i, j,fail,effectOnly,noItem);
        }
        static bool dian;
        public static void DoDraw(Terraria.On_Main.orig_DoDraw orig, Main main, GameTime gameTime)
        {
            orig(main, gameTime);
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);
            for (int a = 0; a < DustSystem.UIDustDraws.Length; a++)
            {
                if (DustSystem.UIDustDraws[a] != null && DustSystem.UIDustDraws[a].active && DustSystem.UIDustDraws[a].Special)
                {
                    DustSystem.UIDustDraws[a].Update();
                    DustSystem.UIDustDraws[a].Draw(Main.spriteBatch);
                }
            }
            if (DDTextures.WhitePng != null)
            {
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, new Color(255, 255, 0, 0) * (ModContent.GetInstance<DDConfigClient>().EyeProtection / 3), 0, Vector2.Zero, new Vector2(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height) /2, 0, 0);
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, new Color(0, 0, 0, 255) * (1 - (float)ModContent.GetInstance<DDConfigClient>().Brightness / 100), 0, Vector2.Zero, new Vector2(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height) /2, 0, 0);
            }
            if (Main.myPlayer == Main.LocalPlayer.whoAmI && !Main.gameInactive && ModContent.GetInstance<DDConfigClient>().ClickEffects)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !dian)
                {
                    for (int a = 0; a < 6; a++)
                    {
                        int r = UIDustDraw.NewDust(new(Mouse.GetState().X, Mouse.GetState().Y), 2, Main.mouseColor, new Vector2(Main.rand.NextFloat(1, 1.4F)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.UIScale, Main.rand.NextFloat(3.4F, 5) * Main.UIScale);
                        if (r >= 0)
                        {
                            DustSystem.UIDustDraws[r].Gravity = false;
                            DustSystem.UIDustDraws[r].ScaleSpeed = 0.2F*Main.UIScale;
                            DustSystem.UIDustDraws[r].Special = true;
                        }
                    }
                    for (int a = 0; a < 20; a++)
                    {
                        int r = UIDustDraw.NewDust(new(Mouse.GetState().X, Mouse.GetState().Y), 2, Main.mouseColor, new Vector2(1).RotatedBy(MathHelper.TwoPi/20*a) * Main.UIScale, 4 * Main.UIScale);
                        if (r >= 0)
                        {
                            DustSystem.UIDustDraws[r].Gravity = false;
                            DustSystem.UIDustDraws[r].ScaleSpeed = 0.2F*Main.UIScale;
                            DustSystem.UIDustDraws[r].Special = true;
                        }
                    }
                    dian = true;
                }
                if (Mouse.GetState().LeftButton != ButtonState.Pressed)
                {
                    dian = false;
                }
            }
            Main.spriteBatch.End();
        }
        public static float MaxFilter = 0;
        public static float Filterincrease = 0;
        public static float FilterValue = 0;
        public static float DrownFilterValue = 0;
        public static Color color;
        public static void Filter(Color c,float Max,float Speed)
        {
            color = c;
            MaxFilter = Max;
            Filterincrease = Speed;
        }
        public static void DrawInfernoRings(Terraria.On_Main.orig_DrawInfernoRings orig, Main main)
        {
            orig(main);
            Main.spriteBatch.End();
            //以后会用上的技术
            /*
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            if (Main.LocalPlayer.Dplayer().HA>0)
            {
                int M = 85;
                Vector2 screenPosition = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                Rectangle value = new Rectangle(0, 0, 32, 32);
                for (int i = (int)screenPosition.X / 16 - M; i < (int)screenPosition.X / 16 + M; i++)
                {
                    for (int j = (int)screenPosition.Y / 16 - M; j < (int)screenPosition.Y / 16 + M; j++)
                    {
                        if (i <= 0 || j <= 0 || i >= Main.maxTilesX || j >= Main.maxTilesY)
                        {
                            continue;
                        }
                        Tile tile = Main.tile[i, j];
                        value.X = tile.WallFrameX;
                        value.Y = tile.WallFrameY;
                        if (tile.WallType != 0)
                        {
                            Main.spriteBatch.Draw(DDTextures.WhitePng.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - 4, j * 16 - (int)Main.screenPosition.Y - 6), null, new Color(16, 14, 36, 255) * Main.LocalPlayer.Dplayer().HA, 0f, Vector2.Zero, 12f, SpriteEffects.None, 0f);
                        }
                    }
                }
            }
            if (Main.LocalPlayer.Dplayer().HA2>0)
            {
                int M = 85;
                Vector2 screenPosition = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                Rectangle value = new Rectangle(0, 0, 32, 32);
                for (int i = (int)screenPosition.X / 16 - M; i < (int)screenPosition.X / 16 + M; i++)
                {
                    for (int j = (int)screenPosition.Y / 16 - M; j < (int)screenPosition.Y / 16 + M; j++)
                    {
                        if (i <= 0 || j <= 0 || i >= Main.maxTilesX || j >= Main.maxTilesY)
                        {
                            continue;
                        }
                        Tile tile = Main.tile[i, j];
                        value.X = tile.WallFrameX;
                        value.Y = tile.WallFrameY;
                        if (tile.WallType != Main.tile[(int)Main.LocalPlayer.Center.X / 16, (int)Main.LocalPlayer.Center.Y / 16].WallType|| tile.WallType == 0)
                        {
                            Main.spriteBatch.Draw(DDTextures.WhitePng.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - 4, j * 16 - (int)Main.screenPosition.Y - 6), null, new Color(16, 14, 36, 255) * Main.LocalPlayer.Dplayer().HA2, 0f, Vector2.Zero, 12f, SpriteEffects.None, 0f);
                        }
                    }
                }
            }
            if (Main.tile[(int)Main.LocalPlayer.Center.X / 16, (int)Main.LocalPlayer.Center.Y / 16].WallType == 0 && Main.LocalPlayer.Dplayer().HA < 1)
            {
                Main.LocalPlayer.Dplayer().HA += 0.05F;
            }
            else
            {
                if (Main.tile[(int)Main.LocalPlayer.Center.X / 16, (int)Main.LocalPlayer.Center.Y / 16].WallType != 0)
                {
                    if (Main.LocalPlayer.Dplayer().HA > 0)
                    {
                        Main.LocalPlayer.Dplayer().HA -= 0.05F;
                    }
                }
            }
            Main.LocalPlayer.Dplayer().HA2 = 1- Main.LocalPlayer.Dplayer().HA;
            Main.spriteBatch.End();
            */
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);
            if (Main.LocalPlayer.whoAmI == Main.myPlayer)
            {
                if (!Main.gamePaused)
                {
                    if (FilterValue < MaxFilter)
                    {
                        FilterValue += Filterincrease;
                        if(FilterValue > MaxFilter)
                        {
                            FilterValue = MaxFilter;

                        }
                    }
                    else if (MaxFilter != 0 || FilterValue > MaxFilter)
                    {
                        FilterValue = MaxFilter;
                    }
                    else if (FilterValue > 0)
                    {
                        FilterValue -= Filterincrease;
                    }

                    if (FilterValue < 0)
                    {
                        FilterValue = 0;
                        Filterincrease = 0;
                    }
                }
                if (DDTextures.WhitePng != null && FilterValue > 0)
                {
                    if (!Main.gamePaused)
                    {
                        MaxFilter = 0;
                    }
                    Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, color * FilterValue, 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 0, 0);
                }
                /*
                if (DDTextures.WhitePng != null)
                {
                    //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, new Color(0,0,0,255), 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 0, 0);
                }*/
            }
            /*
            if (Collision.DrownCollision(Main.LocalPlayer.position, Main.LocalPlayer.width, Main.LocalPlayer.height, Main.LocalPlayer.gravDir))
            {
                if (DrownFilterValue < 1)
                {
                    DrownFilterValue += 0.02F;
                }
            }
            else
            {
                if (DrownFilterValue > 0)
                {
                    DrownFilterValue -= 0.02F;
                }
            }

            if (DrownFilterValue > 0)
            {
                Color Color = new Color(0, 20, 255, 0) * 0.1F * DrownFilterValue;
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, Color * 4, 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 0, 0);
                for (int a = 0; a < 4; a++)
                {
                    for (int b = 0; b < 4; b++)
                    {
                        Main.spriteBatch.Draw(DDTextures.Wave.Value, new Vector2(DDTextures.Wave.Width() * 2 * a, DDTextures.Wave.Height() * 2 * b - (float)Main.time % (DDTextures.Wave.Height() * 2)), null, Color, 0, Vector2.Zero, 2, 0, 0);
                    }
                }
            }*/
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            if (Main.LocalPlayer.whoAmI == Main.myPlayer)
            {
                if (狱火蛇背景.狱火 && Math.Abs(Main.LocalPlayer.position.Y - Main.UnderworldLayer * 16) < 300)
                {
                    Main.spriteBatch.Draw(DDTextures.限制框.Value, new Vector2(Main.LocalPlayer.Center.X, Main.UnderworldLayer * 16) - Main.screenPosition, null, new Color(253, 62, 3, 0) * (1 - Math.Abs(Main.LocalPlayer.position.Y - Main.UnderworldLayer * 16) / 300), 0, new Vector2(DDTextures.限制框.Width() / 2, 0), new Vector2(1, 0.5F), 0, 0);
                    Main.spriteBatch.Draw(DDTextures.限制框.Value, new Vector2(Main.LocalPlayer.Center.X, Main.UnderworldLayer * 16) - Main.screenPosition, null, new Color(253, 62, 3, 0) * (1 - Math.Abs(Main.LocalPlayer.position.Y - Main.UnderworldLayer * 16) / 300), 0, new Vector2(DDTextures.限制框.Width() / 2, 0), new Vector2(1, 0.5F), 0, 0);
                }
            }
            /*
            for (int a = 0; a < 200; a++)
            {
                NPC npc = Main.npc[a];
                if (npc.active)
                {
                    if (NPCLoader.PreDraw(npc, Main.spriteBatch, Main.screenPosition, Color.White))
                    {
                        DDHelper.MethodReflection(main.GetType(), "DrawNPCDirect_Inner", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, new object[] { Main.spriteBatch, npc, false, Main.screenPosition, Color.White });

                    }
                    NPCLoader.PostDraw(npc, Main.spriteBatch, Main.screenPosition, Color.White);
                }
            }
            for (int a = 0; a < 1000; a++)
            {
                Projectile projectile = Main.projectile[a];
                if (projectile.active)
                {
                    Color color = Color.White;
                    if (ProjectileLoader.PreDraw(projectile, ref color))
                    {
                        //DDHelper.MethodReflection(main.GetType(), "DrawNPCDirect_Inner", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(main, new object[] { Main.spriteBatch, projectile, false, Main.screenPosition, Color.White });
                    }
                    ProjectileLoader.PostDraw(projectile, Color.White);
                }
            }*/
            }
        public static bool oceanDepths(Terraria.On_WorldGen.orig_oceanDepths orig, int x,int y)
        {
            bool Bool = orig(x, y);
            if(SWSystem.ForbidBeach)
            {
                Bool = false;
            }
            return Bool;
        }
        public static void UpdateAudio_DecideOnNewMusic(Terraria.On_Main.orig_UpdateAudio_DecideOnNewMusic orig, Main Main)
        {
            if (Music <= 0)
            {
                orig(Main);
                bool Boss = false;
                for (int j = 0; j < 200; j++)
                {
                    NPC npc = Main.npc[j];
                    Rectangle rectangle = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
                    Rectangle value = new Rectangle((int)(npc.position.X + (npc.width / 2)) - 5000, (int)(npc.position.Y + (npc.height / 2)) - 5000, 10000, 10000);
                    if (rectangle.Intersects(value))
                    {
                        if (npc.active && npc.boss)
                        {
                            //Boss = true;
                        }

                    }
                }
                if (!Boss)
                {
                    if (!Main.gameMenu && Main.LocalPlayer.Dplayer() is DDPlayer player && player.Music > 0)
                    {
                        Main.newMusic = player.Music;
                        player.Music = 0;
                    }
                    for (int j = 0; j < 200; j++)
                    {
                        NPC npc = Main.npc[j];
                        Rectangle rectangle = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
                        Rectangle value = new Rectangle((int)(npc.position.X + (npc.width / 2)) - 5000, (int)(npc.position.Y + (npc.height / 2)) - 5000, 10000, 10000);
                        if (rectangle.Intersects(value))
                        {
                            if (npc.active && npc.ModNPC != null && npc.ModNPC.Music >= 0)
                            {
                                Main.newMusic = npc.ModNPC.Music;
                                break;
                            }
                        }
                    }
                }
                if (!Main.gameMenu && Main.LocalPlayer.Dplayer() is DDPlayer player2 && player2.MusicVital && player2.Music > 0)
                {
                    Main.newMusic = player2.Music;
                    player2.Music = 0;
                    player2.MusicVital = false;
                }
            }
            else
            {
                if (Music > 0)
                {
                    Main.newMusic = Music;
                    if (MusicTime <= 0)
                    {
                        Music = 0;
                    }
                    else
                    {
                        MusicTime--;
                    }
                }
                else
                {
                    MusicTime = 0;
                }
            }
        }
        public static int Music;
        public static int MusicTime;
        //反射粒子生成
        public static int Dust_NewDust(Terraria.On_Dust.orig_NewDust orig, Vector2 Position, int Width, int Height, int Type, float SpeedX = 0f, float SpeedY = 0f, int Alpha = 0, Color newColor = default(Color), float Scale = 1f)
        {
            if (Main.gameMenu)
                return 6000;

            if (Main.rand == null)
                Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);

            if (Main.gamePaused)
                return 6000;

            if (WorldGen.gen)
                return 6000;

            if (Main.netMode == 2)
                return 6000;
            if (Type == 5)
            {
                Type = ModContent.DustType<血水粒子>();
                if (Main.rand.NextBool(100))
                {
                    Type = ModContent.DustType<冰雾>();
                    newColor = new Color(155, 15, 15, 100);
                    Alpha = Main.rand.Next(-3000,-1000);
                }
            }
            if (Type == 18)
            {
                if (Main.rand.NextBool(100))
                {
                    Type = ModContent.DustType<冰雾>();
                    newColor = new Color(117, 145, 13, 100) ;
                    Alpha = Main.rand.Next(-3000,-1000);
                }
            }

            int num = (int)(400f * (1f - dCount));
            Rectangle rectangle = new Rectangle((int)(Main.screenPosition.X - num), (int)(Main.screenPosition.Y - num), Main.screenWidth + num * 2, Main.screenHeight + num * 2);
            Rectangle value = new Rectangle((int)Position.X, (int)Position.Y, 10, 10);
            if (!rectangle.Intersects(value))
                return 6000;

            int result = 6000;
            for (int i = 0; i < 6000; i++)
            {
                Dust dust = Main.dust[i];
                if (dust.active)
                    continue;

                if (i > Main.maxDustToDraw * 0.9)
                {
                    if (!Main.rand.NextBool(4))
                        return 6000;
                }
                else if (i > Main.maxDustToDraw * 0.8)
                {
                    if (!Main.rand.NextBool(3))
                        return 6000;
                }
                else if (i > Main.maxDustToDraw * 0.7)
                {
                    if (Main.rand.NextBool(2))
                        return 6000;
                }
                else if (i > Main.maxDustToDraw * 0.6)
                {
                    if (Main.rand.NextBool(4))
                        return 6000;
                }
                else if (i > Main.maxDustToDraw * 0.5)
                {
                    if (Main.rand.NextBool(5))
                        return 6000;
                }
                else
                {
                    dCount = 0f;
                }

                int num2 = Width;
                int num3 = Height;
                if (num2 < 5)
                    num2 = 5;

                if (num3 < 5)
                    num3 = 5;

                result = i;
                dust.fadeIn = 0f;
                dust.active = true;
                dust.type = Type;
                dust.noGravity = false;
                dust.color = newColor;
                dust.alpha = Alpha;
                dust.position.X = Position.X + Main.rand.Next(num2 - 4) + 4f;
                dust.position.Y = Position.Y + Main.rand.Next(num3 - 4) + 4f;
                dust.velocity.X = Main.rand.Next(-20, 21) * 0.1f + SpeedX;
                dust.velocity.Y = Main.rand.Next(-20, 21) * 0.1f + SpeedY;
                dust.frame.X = 10 * Type;
                dust.frame.Y = 10 * Main.rand.Next(3);
                dust.shader = null;
                dust.customData = null;
                dust.noLightEmittence = false;

                GlobalDust.DustPlayerOwner[i] = -1;
                GlobalDust.DustNPCOwner[i] = -1;
                GlobalDust.DustProjectileOwner[i] = -1;
                GlobalDust.DustAI[i] = 0;
                GlobalDust.DustVector[i] = Vector2.Zero;
                GlobalDust.DustPreTile[i] = false;
                int num4 = Type;
                while (num4 >= 100)
                {
                    num4 -= 100;
                    dust.frame.X -= 1000;
                    dust.frame.Y += 30;
                }

                dust.frame.Width = 8;
                dust.frame.Height = 8;
                dust.rotation = 0f;
                dust.scale = 1f + Main.rand.Next(-20, 21) * 0.01f;
                dust.scale *= Scale;
                dust.noLight = false;
                dust.firstFrame = true;
                if (dust.type == 228 || dust.type == 279 || dust.type == 269 || dust.type == 135 || dust.type == 6 || dust.type == 242 || dust.type == 75 || dust.type == 169 || dust.type == 29 || (dust.type >= 59 && dust.type <= 65) || dust.type == 158 || dust.type == 293 || dust.type == 294 || dust.type == 295 || dust.type == 296 || dust.type == 297 || dust.type == 298 || dust.type == 302)
                {
                    dust.velocity.Y = Main.rand.Next(-10, 6) * 0.1f;
                    dust.velocity.X *= 0.3f;
                    dust.scale *= 0.7f;
                }

                if (dust.type == 127 || dust.type == 187)
                {
                    dust.velocity *= 0.3f;
                    dust.scale *= 0.7f;
                }

                if (dust.type == 33 || dust.type == 52 || dust.type == 266 || dust.type == 98 || dust.type == 99 || dust.type == 100 || dust.type == 101 || dust.type == 102 || dust.type == 103 || dust.type == 104 || dust.type == 105)
                {
                    dust.alpha = 170;
                    dust.velocity *= 0.5f;
                    dust.velocity.Y += 1f;
                }

                if (dust.type == 41)
                    dust.velocity *= 0f;

                if (dust.type == 80)
                    dust.alpha = 50;


                ModDust modDust = DustLoader.GetDust(dust.type);

                if (modDust != null)
                {
                    dust.frame.X = 0;
                    dust.frame.Y %= 30;
                    modDust.OnSpawn(dust);
                }

                if (dust.type == 34 || dust.type == 35 || dust.type == 152)
                {
                    dust.velocity *= 0.1f;
                    dust.velocity.Y = -0.5f;
                    if (dust.type == 34 && !Collision.WetCollision(new Vector2(dust.position.X, dust.position.Y - 8f), 4, 4))
                        dust.active = false;
                }
                    break;
            }

            return result;
        }
        public static void Main_DrawDust(Terraria.On_Main.orig_DrawDust orig, Main Main)
        {
            Rectangle rectangle = new Rectangle((int)Main.screenPosition.X - 500, (int)Main.screenPosition.Y - 50, Main.screenWidth + 1000, Main.screenHeight + 100);
            rectangle = new Rectangle((int)Main.screenPosition.X - 1000, (int)Main.screenPosition.Y - 1050, Main.screenWidth + 2000, Main.screenHeight + 2100);
            Rectangle rectangle2 = rectangle;
            ArmorShaderData armorShaderData = null;
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            for (int i = 0; i < Main.maxDustToDraw; i++)
            {
                Dust dust = Main.dust[i];
                if (!dust.active|| GlobalDust.DustPreTile[dust.dustIndex])
                    continue;

                if ((dust.type >= 130 && dust.type <= 134) || (dust.type >= 219 && dust.type <= 223) || dust.type == 226 || dust.type == 278)
                    rectangle = rectangle2;

                if (new Rectangle((int)dust.position.X, (int)dust.position.Y, 4, 4).Intersects(rectangle))
                {
                    float scale = dust.GetVisualScale();
                    if (dust.shader != armorShaderData)
                    {
                        Main.spriteBatch.End();
                        armorShaderData = dust.shader;
                        if (armorShaderData == null)
                        {
                            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        }
                        else
                        {
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                            dust.shader.Apply(null);
                        }
                    }

                    if (dust.type >= 130 && dust.type <= 134)
                    {
                        float num = Math.Abs(dust.velocity.X) + Math.Abs(dust.velocity.Y);
                        num *= 0.3f;
                        num *= 10f;
                        if (num > 10f)
                            num = 10f;

                        for (int j = 0; j < num; j++)
                        {
                            Vector2 velocity = dust.velocity;
                            Vector2 value = dust.position - velocity * j;
                            float scale2 = dust.scale * (1f - j / 10f);
                            Color color = Lighting.GetColor((int)(dust.position.X + 4.0) / 16, (int)(dust.position.Y + 4.0) / 16);
                            color = dust.GetAlpha(color);
                            Main.spriteBatch.Draw(TextureAssets.Dust.Value, value - Main.screenPosition, dust.frame, color, dust.rotation, new Vector2(4f, 4f), scale2, SpriteEffects.None, 0f);
                        }
                    }
                    else if (dust.type == 278)
                    {
                        float num2 = Math.Abs(dust.velocity.X) + Math.Abs(dust.velocity.Y);
                        num2 *= 0.3f;
                        num2 *= 10f;
                        if (num2 > 10f)
                            num2 = 10f;

                        Vector2 origin = new Vector2(4f, 4f);
                        for (int k = 0; k < num2; k++)
                        {
                            Vector2 velocity2 = dust.velocity;
                            Vector2 value2 = dust.position - velocity2 * k;
                            float scale3 = dust.scale * (1f - k / 10f);
                            Color color2 = Lighting.GetColor((int)(dust.position.X + 4f) / 16, (int)(dust.position.Y + 4f) / 16);
                            color2 = dust.GetAlpha(color2);
                            Main.spriteBatch.Draw(TextureAssets.Dust.Value, value2 - Main.screenPosition, dust.frame, color2, dust.rotation, origin, scale3, SpriteEffects.None, 0f);
                        }
                    }
                    else if (dust.type >= 219 && dust.type <= 223 && dust.fadeIn == 0f)
                    {
                        float num3 = Math.Abs(dust.velocity.X) + Math.Abs(dust.velocity.Y);
                        num3 *= 0.3f;
                        num3 *= 10f;
                        if (num3 > 10f)
                            num3 = 10f;

                        for (int l = 0; l < num3; l++)
                        {
                            Vector2 velocity3 = dust.velocity;
                            Vector2 value3 = dust.position - velocity3 * l;
                            float scale4 = dust.scale * (1f - l / 10f);
                            Color color3 = Lighting.GetColor((int)(dust.position.X + 4.0) / 16, (int)(dust.position.Y + 4.0) / 16);
                            color3 = dust.GetAlpha(color3);
                            Main.spriteBatch.Draw(TextureAssets.Dust.Value, value3 - Main.screenPosition, dust.frame, color3, dust.rotation, new Vector2(4f, 4f), scale4, SpriteEffects.None, 0f);
                        }
                    }
                    else if (dust.type == 264 && dust.fadeIn == 0f)
                    {
                        float num4 = Math.Abs(dust.velocity.X) + Math.Abs(dust.velocity.Y);
                        num4 *= 10f;
                        if (num4 > 10f)
                            num4 = 10f;

                        for (int m = 0; m < num4; m++)
                        {
                            Vector2 velocity4 = dust.velocity;
                            Vector2 value4 = dust.position - velocity4 * m;
                            float scale5 = dust.scale * (1f - m / 10f);
                            Color color4 = Lighting.GetColor((int)(dust.position.X + 4.0) / 16, (int)(dust.position.Y + 4.0) / 16);
                            color4 = dust.GetAlpha(color4) * 0.3f;
                            Main.spriteBatch.Draw(TextureAssets.Dust.Value, value4 - Main.screenPosition, dust.frame, color4, dust.rotation, new Vector2(5f), scale5, SpriteEffects.None, 0f);
                            color4 = dust.GetColor(color4);
                            Main.spriteBatch.Draw(TextureAssets.Dust.Value, value4 - Main.screenPosition, dust.frame, color4, dust.rotation, new Vector2(5f), scale5, SpriteEffects.None, 0f);
                        }
                    }
                    else if ((dust.type == 226 || dust.type == 272) && dust.fadeIn == 0f)
                    {
                        float num5 = Math.Abs(dust.velocity.X) + Math.Abs(dust.velocity.Y);
                        num5 *= 0.3f;
                        num5 *= 10f;
                        if (num5 > 10f)
                            num5 = 10f;

                        for (int n = 0; n < num5; n++)
                        {
                            Vector2 velocity5 = dust.velocity;
                            Vector2 value5 = dust.position - velocity5 * n;
                            float scale6 = dust.scale * (1f - n / 10f);
                            Color color5 = Lighting.GetColor((int)(dust.position.X + 4.0) / 16, (int)(dust.position.Y + 4.0) / 16);
                            color5 = dust.GetAlpha(color5);
                            Main.spriteBatch.Draw(TextureAssets.Dust.Value, value5 - Main.screenPosition, dust.frame, color5, dust.rotation, new Vector2(4f, 4f), scale6, SpriteEffects.None, 0f);
                        }
                    }

                    Color newColor = Lighting.GetColor((int)(dust.position.X + 4.0) / 16, (int)(dust.position.Y + 4.0) / 16);

                    if (dust.type == 6 || dust.type == 15 || (dust.type >= 59 && dust.type <= 64))
                        newColor = Color.White;

                    newColor = dust.GetAlpha(newColor);
                    if (dust.type == 213)
                        scale = 1f;


                        DrawDust.Drawdust(dust, newColor, scale);
                        ModDust modDust = DustLoader.GetDust(dust.type);
                    if (modDust != null)
                    {

                        if (DrawDust.PreModDrawdust(dust, newColor, scale) & modDust.PreDraw(dust))
                        {
                            Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, newColor, dust.rotation, new Vector2(4f, 4f), scale, SpriteEffects.None, 0f);

                            if (dust.color != default)
                            {
                                Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetColor(newColor), dust.rotation, new Vector2(4f, 4f), scale, SpriteEffects.None, 0f);
                            }
                        }
                        if (newColor == Color.Black&&dust.noLightEmittence)
                        {
                            dust.active = false;
                        }
                        DrawDust.ModDrawdust(dust, newColor, scale);
                        continue;
                    }
                    //利刃台风粒子
                    bool AC = (dust.type == 217 || dust.type == 229|| dust.type == 226|| dust.type==57);
                    if (!AC || !ModContent.GetInstance<DDConfigClient>().DustLightEffect)
                    {
                        Main.spriteBatch.Draw(TextureAssets.Dust.Value, dust.position - Main.screenPosition, dust.frame, newColor, dust.GetVisualRotation(), new Vector2(4f, 4f), scale, SpriteEffects.None, 0f);
                        if (dust.color.PackedValue != 0)
                        {
                            Color color6 = dust.GetColor(newColor);
                            if (color6.PackedValue != 0)
                                Main.spriteBatch.Draw(TextureAssets.Dust.Value, dust.position - Main.screenPosition, dust.frame, color6, dust.GetVisualRotation(), new Vector2(4f, 4f), scale, SpriteEffects.None, 0f);
                        }
                    }
                    if (newColor == Color.Black && dust.noLightEmittence)
                        dust.active = false;
                }
                else
                {
                    dust.active = false;
                }
            }

            Main.spriteBatch.End();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            TimeLogger.DetailedDrawTime(25);
        }
    }
}