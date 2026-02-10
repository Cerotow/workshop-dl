using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.IO;
using StructureHelper;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.Default;
using DDmod.Worlds;
using DDmod.UI.HunterQuests;
using DDmod.SubworldLibraryWorld.草原;
using DDmod.NoContent.Config;
using System.Text.RegularExpressions;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.SubworldLibraryWorld
{
    public class SWGlobalInfoDisplay : GlobalInfoDisplay
    {
        public override bool? Active(InfoDisplay currentDisplay)
        {
            return base.Active(currentDisplay);
        }
        public override void ModifyDisplayParameters(InfoDisplay currentDisplay, ref string displayValue, ref string displayName, ref Color displayColor, ref Color displayShadowColor)/* tModPorter ModifyDisplayValue, ModifyDisplayName, and ModifyDisplayColor are all combined into ModifyDisplayParameters now. */
        {
            if (currentDisplay == InfoDisplay.DepthMeter)
            {
                string Name = "";
                switch (SWSystem.depth)
                {
                    case SWSystem.DepthMeterType.天空:
                        Name = Language.GetTextValue("GameUI.LayerSpace");
                        break;
                    case SWSystem.DepthMeterType.地表:
                        Name = Language.GetTextValue("GameUI.LayerSurface");
                        break;
                    case SWSystem.DepthMeterType.地下:
                        Name = Language.GetTextValue("GameUI.LayerUnderground");
                        break;
                    case SWSystem.DepthMeterType.洞穴:
                        Name = Language.GetTextValue("GameUI.LayerCaverns");
                        break;
                    case SWSystem.DepthMeterType.地狱:
                        Name = Language.GetTextValue("GameUI.LayerUnderworld");
                        break;
                }
                if (SWSystem.depth != SWSystem.DepthMeterType.禁用)
                {
                    displayValue = Name;
                }
            }
            if (currentDisplay == InfoDisplay.DPSMeter)
            {
                Main.player[Main.myPlayer].checkDPSTime();
                long dPS = Main.player[Main.myPlayer].Dplayer().getDPS();
                if (dPS != 0)
                {
                    displayColor = new Color(200, 200, 200);
                }
                string Text = dPS.ToString();
                if (ModContent.GetInstance<DDConfigClient>().DpsStreamline == DDConfigClient.Dps.InternationalStandard)
                {
                    if (dPS >= 1000000000)
                    {
                        if (Text.Length >= 9)
                        {
                            Text = Text.Substring(0, Text.Length - 9)
                                          + "."
                                          + Text.Substring(Text.Length - 9);
                            Text = Text.Substring(0, Text.Length - 8) + "B";
                        }
                    }
                    else if (dPS >= 1000000)
                    {
                        if (Text.Length >= 6)
                        {
                            Text = Text.Substring(0, Text.Length - 6)
                                          + "."
                                          + Text.Substring(Text.Length - 6);
                            Text = Text.Substring(0, Text.Length - 5) + "M";
                        }
                    }
                    else if (dPS >= 1000)
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
                    if (dPS >= 1000000000000)
                    {
                        if (Text.Length >= 12)
                        {
                            Text = Text.Substring(0, Text.Length - 12)
                                          + "."
                                          + Text.Substring(Text.Length - 12);
                            Text = Text.Substring(0, Text.Length - 11) + "万亿";
                        }
                    }
                    else if (dPS >= 100000000)
                    {
                        if (Text.Length >= 8)
                        {
                            Text = Text.Substring(0, Text.Length - 8)
                                          + "."
                                          + Text.Substring(Text.Length - 8);
                            Text = Text.Substring(0, Text.Length - 7) + "亿";
                        }
                    }
                    else if (dPS >= 10000)
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
                    if (dPS >= 1000000000000)
                    {
                        Text = ReplaceNumbersWithChinese("" + dPS / 1000000000000) + "万亿";
                        string T = ReplaceNumbersWithChinese("" + (dPS % 1000000000000) / 100000000000);
                        if (dPS < 100000000000000 && T.Length >= 1 && T.Substring(0, 1) != "零" && T.Substring(0, 1) != "十")
                        {
                            Text += T.Substring(0, 1);
                        }
                    }
                    else if (dPS >= 100000000)
                    {
                        Text = ReplaceNumbersWithChinese("" + dPS / 100000000) + "亿";
                        string T = ReplaceNumbersWithChinese("" + (dPS % 100000000) / 10000000);
                        if (dPS < 10000000000 && T.Length >= 1 && T.Substring(0, 1) != "零" && T.Substring(0, 1) != "十")
                        {
                            Text += T.Substring(0, 1);
                        }
                    }
                    else if (dPS >= 10000)
                    {
                        Text = ReplaceNumbersWithChinese(""+dPS/10000)+"万";
                        string T = ReplaceNumbersWithChinese("" + (dPS%10000) / 1000);
                        if(dPS <1000000&& T.Length>=1&& T.Substring(0, 1)!= "零"&& T.Substring(0, 1) != "十")
                        {
                            Text += T.Substring(0, 1);
                        }
                    }
                    else
                    {
                        Text = ReplaceNumbersWithChinese(Text);
                    }
                }
                displayValue = ((dPS != 0) ? Language.GetTextValue("GameUI.DPS", Text) : Language.GetTextValue("GameUI.NoDPS"));
            }
        }
        public static string ReplaceNumbersWithChinese(string input)
        {
            var numMap = new Dictionary<char, string>
        {
            {'0', "零"},
            {'1', "一"},
            {'2', "二"},
            {'3', "三"},
            {'4', "四"},
            {'5', "五"},
            {'6', "六"},
            {'7', "七"},
            {'8', "八"},
            {'9', "九"}
        };

            var sb = new StringBuilder();
            var currentNumber = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    currentNumber.Append(c);
                }
                else
                {
                    if (currentNumber.Length > 0)
                    {
                        string numberStr = currentNumber.ToString();
                        string chineseNumber = ConvertNumberToChinese(numberStr);
                        sb.Append(chineseNumber);
                        currentNumber.Clear();
                    }
                    sb.Append(c);
                }
            }

            if (currentNumber.Length > 0)
            {
                string numberStr = currentNumber.ToString();
                string chineseNumber = ConvertNumberToChinese(numberStr);
                sb.Append(chineseNumber);
            }

            return sb.ToString();
        }

        public static string ConvertNumberToChinese(string numberStr)
        {
            if (numberStr == "0") return "零";

            string[] units = { "", "十", "百", "千", "万" };
            var result = new StringBuilder();

            for (int i = 0; i < numberStr.Length; i++)
            {
                char digit = numberStr[i];
                int digitValue = digit - '0';

                if (digitValue == 0)
                {
                    if (i < numberStr.Length - 1 && numberStr[i + 1] != '0')
                    {
                        result.Append("零");
                    }
                }
                else
                {
                    string chineseDigit = new Dictionary<int, string>
                {
                    {1, "一"}, {2, "二"}, {3, "三"}, {4, "四"},
                    {5, "五"}, {6, "六"}, {7, "七"}, {8, "八"}, {9, "九"}
                }[digitValue];

                    int unitIndex = numberStr.Length - i - 1;
                    if (unitIndex < units.Length)
                    {
                        result.Append(chineseDigit + units[unitIndex]);
                    }
                    else
                    {
                        result.Append(chineseDigit); // 超出单位范围（如百万）
                    }
                }
            }

            if (result.Length >= 2 && result.ToString().StartsWith("一十"))
            {
                result.Remove(0, 1);
            }

            return result.ToString();
        }
    }
    public class SWTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            return !SWSystem.ForbidVandalism;
        }
        public override bool CanExplode(int i, int j, int type)
        {
            return !SWSystem.ForbidVandalism;
        }
        public override bool CanPlace(int i, int j, int type)
        {
            return !SWSystem.ForbidVandalism;
        }
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);
        }
    }
    public class SWPlayer : ModPlayer
    {
        /// <summary>
        /// 子世界死亡
        /// </summary>
        public bool SWDead = true;
        public override void ResetEffects()
        {
            if (!SWSystem.TrueSubworld)
            {
                SWDead = false;
            }
        }
        public override void PreUpdate()
        {
            //给需要更新的物品使用背包更新
            if (Player.chest > -1 && Player == Main.LocalPlayer)
            {
                foreach (Item item in Main.chest[Player.chest].item)
                {
                    if (item.type > 0 && item.DItem().UpdatesRequired)
                    {
                        ItemLoader.UpdateInventory(item, Player);
                    }
                }
            }
            if(SWSystem.TrueSubworld&&Player.dead)
            {
                SWDead = true;
            }
            if (SWDead)
            {
                Player.respawnTimer = 6 * 60;
                if (Player.controlMount && Main.LocalPlayer == Player)
                {
                    SubworldSystem.Exit();
                }
                if (Main.LocalPlayer.GetModPlayer<SWPlayer>().SWDead)
                {
                    if (Player.controlLeft)
                    {
                        Player.position -= new Vector2(24, 0);
                        if(Player.position.X<0)
                        {
                            Player.position.X = 0;
                        }
                    }
                    if (Player.controlRight)
                    {
                        Player.position += new Vector2(24, 0);
                        if (Player.position.X > Main.maxTilesX*16)
                        {
                            Player.position.X = Main.maxTilesX * 16;
                        }
                    }
                    if (Player.controlUp)
                    {
                        Player.position -= new Vector2(0, 24);
                        if (Player.position.Y < 0)
                        {
                            Player.position.Y = 0;
                        }
                    }
                    if (Player.controlDown)
                    {
                        Player.position += new Vector2(0, 24);
                        if (Player.position.Y > Main.maxTilesY * 16)
                        {
                            Player.position.Y = Main.maxTilesY * 16;
                        }
                    }
                }
            }
        }
        public override bool CanUseItem(Item item)
        {
            if (SWSystem.ForbidVandalism)
            {
                int type = item.type;
                if (type == 166 || type == 232|| type == 235 || type == 3115 || type == 3196 || type == 4423 || type == 4824 || type == 4825 || type == 4826 ||
                    type == 4827||type == 4908||type == 4909||type == 167||type == 2896||type == 3547)
                {
                    return false;
                }
                if(item.createTile>=0)
                {
                    return false;
                }
            }
            return base.CanUseItem(item);
        }
    }
    public class SWSystem : ModSystem
    {
        /// <summary>
        /// 地图路径
        /// </summary>
        public static string path;

        /// <summary>
        /// 到了子世界
        /// </summary>
        public static bool TrueSubworld = false;
        /// <summary>
        /// 禁止破坏世界
        /// </summary>
        public static bool ForbidVandalism = false;
        /// <summary>
        /// 禁止生成怪物
        /// </summary>
        public static bool ForbidSpawn = false;
        /// <summary>
        /// 
        /// </summary>
        public enum DepthMeterType : byte
        {
            禁用,天空,地表,地下,洞穴,地狱
        }
        public static DepthMeterType depth;
        /// <summary>
        /// 禁止海洋
        /// </summary>
        public static bool ForbidBeach = false;
        /// <summary>
        /// 地狱高度
        /// </summary>
        public static int UnderworldHeight = -1;
        /// <summary>
        /// 退出关卡判定
        /// </summary>
        public static int LevelUnlocked;
        //副本生成
        public static int SWNewNPCs(IEntitySource spawnSource, Vector2 position, int Type, int Start, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0, int Target = 255)
        {
            int R = NewNPCs(spawnSource, position.X, position.Y, Type, Start, ai0, ai1, ai2, ai3, Target);
            Main.npc[R].Dnpc().Copy = true;
            return R;
        }
        public static int SWNewNPCs(IEntitySource spawnSource, float x, float y, int Type, int Start, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0, int Target = 255)
        {
            int R = NewNPC(spawnSource, (int)x, (int)y, Type, Start, ai0, ai1, ai2, ai3, Target);
            Main.npc[R].Dnpc().Copy = true;
            return R;
        }
        public override void OnWorldLoad()
        {
            if (TrueSubworld)
            {
                TrueSubworld = false;
                LevelUnlocked=180;
            }
            ForbidVandalism = false;
            ForbidSpawn = false;
            ForbidBeach = false;
            UnderworldHeight = -1;
            depth = DepthMeterType.禁用;
            HunterQuestPanel.ChallengeWorld.Clear();
            HunterQuestPanel.WorldLock.Clear();
            HunterQuestPanel.WorldLockText.Clear();
        }
        public override void OnWorldUnload()
        {
            HunterQuestPanel.ChallengeWorld.Clear();
            HunterQuestPanel.WorldLock.Clear();
            HunterQuestPanel.WorldLockText.Clear();
        }
        public override void PreUpdateTime()
        {
            if (!TrueSubworld)
            {
                path = Main.worldPathName;
            }
            if(SWSystem.LevelUnlocked>=0)
            {
                SWSystem.LevelUnlocked--;
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = TrueSubworld;
            flags[1] = ForbidVandalism;
            flags[2] = ForbidSpawn;
            flags[3] = ForbidBeach;
            writer.Write(flags);

            writer.Write(UnderworldHeight);

        }

        public override void NetReceive(BinaryReader reader)
        {
            
            BitsByte flags = reader.ReadByte();
            TrueSubworld = flags[0];
            ForbidVandalism = flags[1];
            ForbidSpawn = flags[2];
            ForbidBeach = flags[3];
            UnderworldHeight = reader.ReadInt32();
        }
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.GetModPlayer<SWPlayer>().SWDead)
            {
                Color color = new Color(222, 40, 40, 150);
                string text = Language.GetTextValue("Mods.DDmod.world.dead1");
                Vector2 origin = ChatManager.GetStringSize(FontAssets.DeathText.Value, text, Vector2.One, 0) / 2;
                DynamicSpriteFontExtensionMethods.DrawString(
                spriteBatch,
                FontAssets.DeathText.Value,
                text,
                new Vector2(Main.screenWidth/2, Main.screenHeight/2) + new Vector2(0, 100),
                color, 0f,
                origin,
                1, SpriteEffects.None, 0f);
                text = Language.GetTextValue("Mods.DDmod.world.dead2");
                Vector2 origin2 = ChatManager.GetStringSize(FontAssets.DeathText.Value, text, Vector2.One, 0) / 2;
                DynamicSpriteFontExtensionMethods.DrawString(
                spriteBatch,
                FontAssets.DeathText.Value,
                text,
                new Vector2(Main.screenWidth/2, Main.screenHeight/2) + new Vector2(0, 150),
                color, 0f,
                origin2,
                1, SpriteEffects.None, 0f);
            }
        }
        /// <summary>
        /// 地表高度,禁止爆破,禁止自然生成,禁止海洋,调整地狱高度(0默认禁用地狱)
        /// </summary>
        public static void Basicinfo(int worldSurface, bool ForbidVandalism = true, bool ForbidSpawn = true, bool ForbidBeach = true, int UnderworldHeight = 0)
        {
            SWSystem.TrueSubworld = true;
            Main.worldSurface = worldSurface;
            SWSystem.ForbidVandalism = ForbidVandalism;
            SWSystem.ForbidSpawn = ForbidSpawn;
            SWSystem.ForbidBeach = ForbidBeach;
            SWSystem.UnderworldHeight = UnderworldHeight;
            if (UnderworldHeight==0)
            {
                SWSystem.UnderworldHeight = Main.maxTilesY;
            }
        }
        public static void ReadSave()
        {
            string path = SWSystem.path;
            path = Path.ChangeExtension(path, ".twld");

            if (!FileUtilities.Exists(path, false))
                return;

            byte[] buf = FileUtilities.ReadAllBytes(path, false);

            if (buf[0] != 0x1F || buf[1] != 0x8B)
            {
                return;
            }
            var From = TagIO.FromStream(new MemoryStream(buf));
            var list = From.GetList<TagCompound>("modData");
            foreach (var tag in list)
            {
                if (ModContent.TryFind(tag.GetString("mod"), tag.GetString("name"), out ModSystem system))
                {
                    try
                    {
                        system.LoadWorldData(tag.GetCompound("data"));
                    }
                    catch (Exception e)
                    {
                        throw new CustomModDataException(system.Mod,
                            "Error in reading custom world data for " + system.Mod.Name, e);
                    }
                }
                else
                {
                    //ModContent.GetInstance<UnloadedSystem>().SaveWorldData(tag);
                }
            }
        }
        /// <summary>
        /// 白,绿,蓝,紫,橙,红(注意,数字越大概率越高)
        /// 当前概率除概率总和等于百分比概率
        /// 建议概率总和为100
        /// </summary>
        public static int RandNext(int num,int num2,int num3,int num4,int num5,int num6)
        {
            int R = new Random().Next(num+num2+num3+num4+num5+num6);

            int P = num;
            //白
            if (R < num)
            {
                return 0;
            }
            P += num2;
            //绿
            if (R < P)
            {
                return 1;
            }
            P += num3;
            //蓝
            if (R < P)
            {
                return 2;
            }
            P += num4;
            //紫
            if (R < P)
            {
                return 3;
            }
            P += num5;
            //橙
            if (R < P)
            {
                return 4;
            }
            P += num6;
            //红
            if (R < P)
            {
                return 5;
            }
            return -1;
        }
        static Vector2 PX;
        static Vector2 PY;
        static bool SP;
        public static void ScreenPosition(Player Player, ref float X, ref float Y,bool RestrictPlayer = true, float MaxPositionX=-1,float MinPositionX=-1, float MaxPositionY = -1, float MinPositionY = -1)
        {
            SP = true;
            if (MaxPositionX > -1 && MinPositionX > -1)
            {
                for (int a = 0; a < 200; a++)
                {
                    NPC npc = Main.npc[a];
                    if (npc.active&& npc.CanBeChasedBy())
                    {
                        if (npc.position.X > MaxPositionX - npc.width)
                        {
                            npc.position.X = MaxPositionX - npc.width;
                        }
                        if (npc.position.X < MinPositionX)
                        {
                            npc.position.X = MinPositionX;
                        }
                    }
                }
                PX = new Vector2(MinPositionX, MaxPositionX);
                if (RestrictPlayer)
                {
                    if (Player.position.X > MaxPositionX - Player.width)
                    {
                        Player.position.X = MaxPositionX - Player.width;
                    }
                    if (Player.position.X < MinPositionX)
                    {
                        Player.position.X = MinPositionX;
                    }
                }
                if (Player.position.X + Main.screenWidth / 2 / Main.GameZoomTarget > MaxPositionX)
                {
                    X = MaxPositionX - Main.screenWidth / 2 / Main.GameZoomTarget;
                }
                else if (Player.position.X - Main.screenWidth / 2 / Main.GameZoomTarget < MinPositionX)
                {
                    X = MinPositionX + Main.screenWidth / 2 / Main.GameZoomTarget;
                }
                else
                {
                    X = Player.position.X;
                }
            }
            if (MaxPositionY > -1&& MinPositionY>-1)
            {
                PY = new Vector2(MinPositionY, MaxPositionY);
                if (RestrictPlayer)
                {
                    if (Player.position.Y > MaxPositionY - Player.height)
                    {
                        Player.position.Y = MaxPositionY - Player.height;
                        Player.Aplayer().Stand = 2;
                    }
                    if (Player.position.Y < MinPositionY)
                    {
                        Player.position.Y = MinPositionY;
                        Player.velocity.Y = 0;
                    }
                }
                if (Player.position.Y + Main.screenWidth / 2 / Main.GameZoomTarget > MaxPositionY)
                {
                    Y = MaxPositionY - Main.screenWidth / 2 / Main.GameZoomTarget;
                }
                else if (Player.position.Y - Main.screenWidth / 2 / Main.GameZoomTarget < MinPositionY)
                {
                    Y = MinPositionY + Main.screenWidth / 2 / Main.GameZoomTarget;
                }
                else
                {
                    Y = Player.position.Y;
                }
            }
        }
        public override void PostDrawTiles()
        {
            if (SP)
            {
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                Main.spriteBatch.Draw(DDTextures.限制框.Value, new Vector2(PX.X, Main.LocalPlayer.Center.Y) - Main.screenPosition, null, new Color(45, 153, 255, 0) * (1 - Math.Abs(Main.LocalPlayer.Center.X - PX.X) / 300), MathHelper.PiOver2, new Vector2(DDTextures.限制框.Width() / 2, 0), new Vector2(0.5F, 1F), 0, 0);
                Main.spriteBatch.Draw(DDTextures.限制框.Value, new Vector2(PX.Y, Main.LocalPlayer.Center.Y) - Main.screenPosition, null, new Color(45, 153, 255, 0) * (1 - Math.Abs(Main.LocalPlayer.Center.X- PX.Y) / 300), -MathHelper.PiOver2, new Vector2(DDTextures.限制框.Width() / 2, 0), new Vector2(0.5F, 1F), 0, 0);
                SP = false;
                Main.spriteBatch.End();
            } 
        }
    }
    public class SWNPC : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if(SWSystem.ForbidSpawn)
            {
                spawnRate = 114514;
                maxSpawns = -114514;
            }
        }
        public override bool CheckActive(NPC npc)
        {
            if (SWSystem.ForbidSpawn)
            {
                return false;
            }
                return base.CheckActive(npc);
        }
    }
}