using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.NoContent.Config;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DDmod.Content.Items.农场
{
    public class FoodGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public bool Seed;
        public bool 夜视;
        public int 夜视Time;
        public bool 发光;
        public int 发光Time;
        public Vector3 发光颜色;
        public float 伤害;
        public int 伤害Time;
        public DamageClass damage;
        public int 防御;
        public int 防御Time;
        public float 防御2;
        public int 防御2Time;
        public float 移速;
        public int 移速Time;
        public int 生命回复;
        public int 生命回复Time;
        public int 生命;
        public int 生命Time;
        public int 生命2;
        public int 生命2Time;
        public int 魔力回复;
        public int 魔力回复Time;
        public int 魔力;
        public int 魔力Time;
        public int 魔力2;
        public int 魔力2Time;
        public float 挖掘;
        public int 挖掘Time;
        public bool Food;
        public bool SeniorFood;
        public void 夜视buff(int time)
        {
            夜视 = true;
            夜视Time = time;
        }
        public void 发光buff(int time, Vector3 vector)
        {
            发光 = true;
            发光Time = time;
            发光颜色 = vector;
        }
        public void 伤害buff(float Ratio, int time, DamageClass damage)
        {
            伤害 = Ratio;
            this.damage = damage;
            伤害Time = time;
        }
        public void 防御buff(int Ratio, int time)
        {
            防御 = Ratio;
            防御Time = time;
        }
        public void 防御2buff(float Ratio, int time)
        {
            防御2 = Ratio;
            防御2Time = time;
        }
        public void 移速buff(float Ratio, int time)
        {
            移速 = Ratio;
            移速Time = time;
        }
        public void 生命回复buff(int Ratio, int time)
        {
            生命回复 = Ratio;
            生命回复Time = time;
        }
        public void 生命buff(int Ratio, int time)
        {
            生命 = Ratio;
            生命Time = time;
        }
        public void 生命2buff(int Ratio, int time)
        {
            生命2 = Ratio;
            生命2Time = time;
        }
        public void 魔力回复buff(int Ratio, int time)
        {
            魔力回复 = Ratio;
            魔力回复Time = time;
        }
        public void 魔力buff(int Ratio, int time)
        {
            魔力 = Ratio;
            魔力Time = time;
        }
        public void 魔力2buff(int Ratio, int time)
        {
            魔力2 = Ratio;
            魔力2Time = time;
        }
        public void 挖掘buff(float Ratio, int time)
        {
            挖掘 = Ratio;
            挖掘Time = time;
        }
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults(Item item)
        {
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
        public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            return base.PreDrawTooltip(item, lines, ref x, ref y);
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (Food&&player.HasBuff(ModContent.BuffType<拉肚子>()))
            {
                return false;

            }
            if (!SeniorFood && Food&&player.HasBuff(ModContent.BuffType<食物DeBuff>()))
            {
                return false;
            }
                return base.CanUseItem(item, player);
        }
        public override bool? UseItem(Item item, Player player)
        {
            if (Food)
            {
                DDmod.SyncData(DDType.PlayerFood, player.whoAmI,-1,player.whoAmI);
                if (!SeniorFood)
                {
                    for (int A = 0; A < player.GetModPlayer<FoodPlayer>().FoodBuff.Length; A++)
                    {
                        if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].FunctionalBuff)
                        {
                        }
                        else
                        {
                            if (!player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff)
                            {
                                player.GetModPlayer<FoodPlayer>().FoodBuff[A] = new FoodBuff(player, 0, 2);
                            }
                        }
                    }
                }
                if (夜视)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.夜视, 夜视Time))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.夜视, 夜视Time);
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who].FunctionalBuff = true;
                    }
                }
                if (发光)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.发光, 发光Time, Glow: 发光颜色))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.发光, 发光Time, Glow: 发光颜色);
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who].FunctionalBuff = true;
                    }
                }
                if (伤害 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.伤害, 伤害Time, 伤害, damage))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.伤害, 伤害Time, 伤害, false, damage);
                    }
                }
                else if (伤害 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.伤害, 伤害Time, 伤害, damage))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.伤害, 伤害Time, 伤害, true, damage);
                    }
                }
                if (防御 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.防御, 防御Time, 防御))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.防御, 防御Time, 防御);
                    }
                }
                else if (防御 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.防御, 防御Time, 防御))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.防御, 防御Time, 防御, true);
                    }
                }
                if (防御2 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.防御2, 防御2Time, 防御2))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.防御2, 防御2Time, 防御2);
                    }
                }
                else if (防御2 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.防御2, 防御2Time, 防御2))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.防御2, 防御2Time, 防御2, true);
                    }
                }
                if (移速 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.移速, 移速Time, 移速))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.移速, 移速Time, 移速);
                    }
                }
                else if (移速 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.移速, 移速Time, 移速))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.移速, 移速Time, 移速, true);
                    }
                }
                if (生命回复 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.生命回复, 生命回复Time, 生命回复))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.生命回复, 生命回复Time, 生命回复);
                    }
                }
                else if (生命回复 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.生命回复, 生命回复Time, 生命回复))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.生命回复, 生命回复Time, 生命回复, true);
                    }
                }
                if (生命 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.生命, 生命Time, 生命))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.生命, 生命Time, 生命);
                    }
                }
                else if (生命 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.生命, 生命Time, 生命))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.生命, 生命Time, 生命, true);
                    }
                }
                if (生命2 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.生命2, 生命2Time, 生命2))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.生命2, 生命2Time, 生命2);
                    }
                }
                else if (生命2 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.生命2, 生命2Time, 生命2))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.生命2, 生命2Time, 生命2, true);
                    }
                }
                if (魔力回复 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.魔力回复, 魔力回复Time, 魔力回复))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.魔力回复, 魔力回复Time, 魔力回复);
                    }
                }
                else if (魔力回复 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.魔力回复, 魔力回复Time, 魔力回复))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.魔力回复, 魔力回复Time, 魔力回复, true);
                    }
                }
                if (魔力 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.魔力, 魔力Time, 魔力))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.魔力, 魔力Time, 魔力);
                    }
                }
                else if (魔力 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.魔力, 魔力Time, 魔力))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.魔力, 魔力Time, 魔力, true);
                    }
                }
                if (魔力2 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.魔力2, 魔力2Time, 魔力2))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.魔力2, 魔力2Time, 魔力2);
                    }
                }
                else if (魔力2 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.魔力2, 魔力2Time, 魔力2))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.魔力2, 魔力2Time, 魔力2, true);
                    }
                }
                if (挖掘 > 0)
                {
                    if (FoodBuff.FindfirstBuff(player, out int Who, FoodBuff.挖掘, 挖掘Time, 挖掘))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.挖掘, 挖掘Time, 挖掘);
                    }
                }
                else if (挖掘 != 0)
                {
                    if (FoodBuff.FindfirstDeBuff(player, out int Who, FoodBuff.挖掘, 挖掘Time, 挖掘))
                    {
                        player.GetModPlayer<FoodPlayer>().FoodBuff[Who] = new FoodBuff(player, FoodBuff.挖掘, 挖掘Time, 挖掘, true);
                    }
                }
                return true;
            }
            return null;
        }
        public string FoodTime(int time)
        {
            string t = "";
            time /= 60;
            if (time/60>0)
            {
                t += time / 60 + Language.GetTextValue("Mods.DDmod.Tooltips.Minute");
            }
            if (time % 60 != 0)
            {
                t += (time % 60) + Language.GetTextValue("Mods.DDmod.Tooltips.Second");
            }
            return t;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Food)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.食用"))
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            if (夜视)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.夜视") + "  (" + FoodTime(夜视Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            if (发光)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.发光") + "  (" + FoodTime(发光Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            if (伤害 != 0)
            {
                string Damage = "";
                if (damage == DamageClass.Generic)
                {
                    Damage = Language.GetTextValue("Mods.DDmod.properties.伤害");
                }
                if (damage == DamageClass.Melee)
                {
                    Damage = Language.GetTextValue("Mods.DDmod.properties.近战");
                }
                if (damage == DamageClass.Summon)
                {
                    Damage = Language.GetTextValue("Mods.DDmod.properties.召唤");
                }
                if (damage == DamageClass.Ranged)
                {
                    Damage = Language.GetTextValue("Mods.DDmod.properties.远程");
                }
                if (damage == DamageClass.Magic)
                {
                    Damage = Language.GetTextValue("Mods.DDmod.properties.魔法");
                }
                if (伤害 > 0)
                {
                    tooltips.Add(new TooltipLine(Mod, "", Damage +" +" + Math.Abs(伤害 * 100).ToString("F1") + "%" + "  (" + FoodTime(伤害Time) + ")")
                    {
                        OverrideColor = new Color(74, 255, 112)
                    });
                }
                else
                {
                    tooltips.Add(new TooltipLine(Mod, "",  Damage + " -" + Math.Abs(伤害 * 100).ToString("F1") + "%" + "  (" + FoodTime(伤害Time) + ")")
                    {
                        OverrideColor = new Color(192, 74, 90)
                    });
                }
            }
            if (防御 > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.防御") + " +" + Math.Abs(防御) + "  (" + FoodTime(防御Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            else if (防御 != 0)
            {

                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.防御") + " -" + Math.Abs(防御) + "  (" + FoodTime(防御Time) + ")")
                {
                    OverrideColor = new Color(192, 74, 90)
                });
            }
            if (防御2 > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.防御") + " +" + Math.Abs(防御2 * 100).ToString("F1") + "%" + "  (" + FoodTime(防御2Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            else if (防御2 != 0)
            {

                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.防御") + " -" + Math.Abs(防御2 * 100).ToString("F1") + "%" + "  (" + FoodTime(防御2Time) + ")")
                {
                    OverrideColor = new Color(192, 74, 90)
                });
            }
            if (移速 > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.移动速度") + " +" + Math.Abs(移速 * 100).ToString("F1") + "%" + "  (" + FoodTime(移速Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            else if (移速 != 0)
            {

                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.移动速度") + " -" + Math.Abs(移速 * 100).ToString("F1") + "%" + "  (" + FoodTime(移速Time) + ")")
                {
                    OverrideColor = new Color(192, 74, 90)
                });
            }
            if (生命回复 > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.生命回复") + " +" + Math.Abs(生命回复) + "  (" + FoodTime(生命回复Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            else if (生命回复 != 0)
            {

                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.生命回复") + " -" + Math.Abs(生命回复) + "  (" + FoodTime(生命回复Time) + ")")
                {
                    OverrideColor = new Color(192, 74, 90)
                });
            }
            if (生命 > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.生命") + " +" + Math.Abs(生命) + "  (" + FoodTime(生命Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            else if (生命 != 0)
            {

                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.生命") + " -" + Math.Abs(生命) + "  (" + FoodTime(生命Time) + ")")
                {
                    OverrideColor = new Color(192, 74, 90)
                });
            }
            if (生命2 > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.生命") + " +" + Math.Abs(生命2 * 100).ToString("F1") + "%" + "  (" + FoodTime(生命2Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            else if (生命2 != 0)
            {

                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.生命") + " -" + Math.Abs(生命2 * 100).ToString("F1") + "%" + "  (" + FoodTime(生命2Time) + ")")
                {
                    OverrideColor = new Color(192, 74, 90)
                });
            }
            if (魔力回复 > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.魔力回复") + " +" + Math.Abs(魔力回复) + "  (" + FoodTime(魔力回复Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            else if (魔力回复 != 0)
            {

                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.魔力回复") + " -" + Math.Abs(魔力回复) + "  (" + FoodTime(魔力回复Time) + ")")
                {
                    OverrideColor = new Color(192, 74, 90)
                });
            }
            if (魔力 > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.魔力") + " +" + Math.Abs(魔力) + "  (" + FoodTime(魔力Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            else if (魔力 != 0)
            {

                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.魔力") + " -" + Math.Abs(魔力) + "  (" + FoodTime(魔力Time) + ")")
                {
                    OverrideColor = new Color(192, 74, 90)
                });
            }
            if (魔力2 > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.魔力") + " +" + Math.Abs(魔力2 * 100).ToString("F1") + "%" + "  (" + FoodTime(魔力2Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            else if (魔力2 != 0)
            {

                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.魔力") + " -" + Math.Abs(魔力2 * 100).ToString("F1") + "%" + "  (" + FoodTime(魔力2Time) + ")" )
                {
                    OverrideColor = new Color(192, 74, 90)
                });
            }
            if (挖掘 > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.挖掘速度") + " +" + Math.Abs(挖掘 * 100).ToString("F1") + "%" + "  (" + FoodTime(挖掘Time) + ")")
                {
                    OverrideColor = new Color(74, 255, 112)
                });
            }
            else if (挖掘 != 0)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.挖掘速度") + " -" + Math.Abs(挖掘 * 100).ToString("F1") + "%" + "  (" + FoodTime(挖掘Time) + ")")
                {
                    OverrideColor = new Color(192, 74, 90)
                });
            }
            if (Food&& !SeniorFood)//if (SeniorFood)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.properties.食用刷新"))
                {
                    OverrideColor = new Color(174, 55, 242)
                });
            }
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria")
                {
                    if (line.Name == "Placeable")
                    {
                        if (Seed)
                        {
                            line.Text = "";
                        }
                    }
                }
            }
        }
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            return base.PreDrawTooltipLine(item, line, ref yOffset);
        }
    }
    public class FoodPlayer : ModPlayer
    {
        public FoodBuff[] FoodBuff = new FoodBuff[15];
        public int FoodCount => FoodBuff.Length;
        public Color color;
        public void Send()
        {
            for (int a = 0; a < FoodCount; a++)
            {
                FoodBuff[a].Send(a, Mod);
            }
        }
        //同步食物
        public static void PlayerFood(Mod mod, BinaryReader reader)
        {
            int player = reader.ReadInt32();
            int T = reader.ReadInt32();
            FoodPlayer food = Main.player[player].GetModPlayer<FoodPlayer>();
            if (food == null)
            {
                Main.player[player].GetModPlayer<FoodPlayer>().FoodBuff[T] = new FoodBuff(Main.player[player], 0, 2);
            }
            if (food.FoodBuff[T] == null)
            {
                Main.player[player].GetModPlayer<FoodPlayer>().FoodBuff[T] = new FoodBuff(Main.player[player], 0, 2);
            }
            int type = reader.ReadInt32();
            int Time = reader.ReadInt32();
            float Ratio = reader.ReadFloat();
            bool DeBuff = reader.ReadBoolean();
            bool FunctionalBuff = reader.ReadBoolean();
            int damageTypes = reader.ReadInt32();
            Vector3 Glow = new(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
            food.FoodBuff[T].type = type;
            food.FoodBuff[T].Time = Time;
            food.FoodBuff[T].Ratio = Ratio;
            food.FoodBuff[T].DeBuff = DeBuff;
            food.FoodBuff[T].FunctionalBuff = FunctionalBuff;
            food.FoodBuff[T].damageTypes = damageTypes;
            food.FoodBuff[T].Glow = Glow;
            //要和上面对齐
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.PlayerFood);
                packet.Write(player);
                packet.Write(T);
                packet.Write(type);
                packet.Write(Time);
                packet.Write(Ratio);
                packet.Write(DeBuff);
                packet.Write(FunctionalBuff);
                packet.Write(damageTypes);
                packet.Write((int)Glow.X);
                packet.Write((int)Glow.Y);
                packet.Write((int)Glow.Z);
                //前者是发给所有玩家,后者是不用发给我自己
                packet.Send(-1, player);
            }
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (newPlayer)
            {
            }
        }
        public override void PreUpdate()
        {
            if (Player.itemAnimation >= 1 && color != new Color(0, 0, 0, 0))
            {
                NewDustChange(2, Player.Center - new Vector2(Player.direction == 1 ? -0 : 10, 10), new Vector2(6), ModContent.DustType<光球粒子>(), 0F, 1.5F, Scale: 0.3F, color: color);
            }
            else
            {
                color = new Color(0, 0, 0, 0);
            }
            bool De = false;
            bool Bu = false;

            for (int A = 0; A < Player.GetModPlayer<FoodPlayer>().FoodBuff.Length; A++)
            {
                if (Player.GetModPlayer<FoodPlayer>().FoodBuff[A] == null || Player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time <= 0 || Player.dead)
                {
                    if (Player.GetModPlayer<FoodPlayer>().FoodBuff[A] != null && Player.GetModPlayer<FoodPlayer>().FoodBuff[A].type != 0)
                    {
                        DDmod.SyncData(DDType.PlayerFood, Player.whoAmI, -1, Player.whoAmI);
                    }
                    Player.GetModPlayer<FoodPlayer>().FoodBuff[A] = new FoodBuff(Player, 0, 2);
                }
                if (Player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff && Player.GetModPlayer<FoodPlayer>().FoodBuff[A].type > 0)
                {
                    De = true;
                }
                if (!Player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff && Player.GetModPlayer<FoodPlayer>().FoodBuff[A].type > 0)
                {
                    Bu = true;
                }
            }
            if (De)
            {
                Player.AddBuff(ModContent.BuffType<食物DeBuff>(), 2, true);
            }
            if (Bu)
            {
                Player.AddBuff(ModContent.BuffType<食物buff>(), 2, true);
            }
        }
        public override void SaveData(TagCompound tag)
        {
            for (int a = 0; a < FoodBuff.Length; a++)
            {
                if (FoodBuff[a] == null)
                {
                    FoodBuff[a] = new FoodBuff(Player, 0, 2);
                }
                FoodBuff[a].SaveData(a, tag);
            }
            tag.Add("FoodCount", FoodBuff.Length);
        }
        public override void LoadData(TagCompound tag)
        {
            for (int a = 0; a < tag.Get<int>("FoodCount"); a++)
            {
                FoodBuff[a] = new FoodBuff(Player, 0, 2);
            }
            for (int a = 0; a < FoodBuff.Length; a++)
            {
                if (FoodBuff[a] == null)
                {
                    FoodBuff[a] = new FoodBuff(Player, 0, 2);
                }

                FoodBuff[a].LoadData(a, tag);
            }
        }
    }
    public partial class FoodBuff
    {
        public FoodBuff(Player player, int type, int Time, float Ratio = 0, bool DeBuff = false, DamageClass damage = null, Vector3? Glow = null)
        {
            this.player = player;
            this.type = type;
            this.Time = Time;
            this.Ratio = Ratio;
            this.DeBuff = DeBuff;
            this.damageType = damage;
            if (damage == DamageClass.Generic)
            {
                damageTypes = 1;
            }
            if (damage == DamageClass.Melee)
            {
               damageTypes = 2;
            }
            if (damage == DamageClass.Ranged)
            {
                damageTypes = 3;
            }
            if (damage == DamageClass.Magic)
            {
                damageTypes = 4;
            }
            if (damage == DamageClass.Summon)
            {
                damageTypes = 5;
            }
            if (Glow != null)
                this.Glow = Glow.Value;
        }
        public const short 夜视 = 1;
        public const short 伤害 = 2;
        public const short 防御 = 3;
        /// <summary>
        /// 百分比防御
        /// </summary>
        public const short 防御2 = 4;
        public const short 移速 = 5;
        public const short 生命回复 = 6;
        public const short 生命 = 7;
        public const short 生命2 = 8;
        public const short 魔力回复 = 9;
        public const short 魔力 = 10;
        public const short 魔力2 = 11;
        public const short 发光 = 12;
        public const short 挖掘 = 13;
        /// <summary>
        /// 优先计算
        /// </summary>
        public bool Precedence;
        public int type = 0;
        public int Time = 0;
        public float Ratio = 0;
        public bool DeBuff;
        /// <summary>
        /// 功能性buff
        /// </summary>
        public bool FunctionalBuff;
        public Vector3 Glow;
        public DamageClass damageType;
        public int damageTypes;
        public Player player;
        public static bool FindfirstBuff(Player player, out int Who, int type, int Time, float Ratio = 0, DamageClass damage = null, Vector3? Glow = null)
        {
            int BUFF = 0;
            if (damage == null)
            {
               damage = DamageClass.Default;
            }
            Who = 0;
            for (int A = 0; A < player.GetModPlayer<FoodPlayer>().FoodBuff.Length; A++)
            {
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].type == type)
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A] = new FoodBuff(player, 0, 2);
                /*
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].type == 发光 && type == 发光&& !player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time = Time;
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Glow = Glow.Value;
                    return false;
                }
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].type == type &&
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time > Time &&
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Ratio > Ratio &&
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageType == damage && !player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff)
                {
                    return false;
                }
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].type == type &&
                    (player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time < Time || player.GetModPlayer<FoodPlayer>().FoodBuff[A].Ratio < Ratio) &&
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageType == damage && !player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time = Time;
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Ratio = Ratio;
                    return false;
                }*/
                if (damage == DamageClass.Generic)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageTypes = 1;
                }
                if (damage == DamageClass.Melee)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageTypes = 2;
                }
                if (damage == DamageClass.Ranged)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageTypes = 3;
                }
                if (damage == DamageClass.Magic)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageTypes = 4;
                }
                if (damage == DamageClass.Summon)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageTypes = 5;
                }
                if (!player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff)
                {
                    BUFF++;
                }
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].type <= 0 && Who == 0)
                {
                    Who = A;
                }
            }
            return true;
        }
        public static bool FindfirstDeBuff(Player player, out int Who, int type, int Time, float Ratio = 0, DamageClass damage = null)
        {
            Who = 0;
            if (damage == null)
            {
                damage = DamageClass.Default;
            }
            for (int A = 0; A < player.GetModPlayer<FoodPlayer>().FoodBuff.Length; A++)
            {
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].type == type)
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A] = new FoodBuff(player, 0, 2);
                /*
                //buff时间大于食物时间,但是buff效果小于食物效果
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].type == type &&
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time >= Time &&
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Ratio < Ratio &&
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageType == damage && player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time += Time / 10;
                    return false;
                }
                //buff时间小于食物时间,但是buff效果大于食物效果
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].type == type &&
                    (player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time < Time || player.GetModPlayer<FoodPlayer>().FoodBuff[A].Ratio > Ratio) &&
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageType == damage && player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time += Time / 2;
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Ratio = Ratio;
                    return false;
                }
                //buff时间小于食物时间,而buff效果小于食物效果
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].type == type &&
                    (player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time < Time || player.GetModPlayer<FoodPlayer>().FoodBuff[A].Ratio <= Ratio) &&
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageType == damage && player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].Time += Time / 2;
                    return false;
                }*/
                if (damage == DamageClass.Generic)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageTypes = 1;
                }
                if (damage == DamageClass.Melee)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageTypes = 2;
                }
                if (damage == DamageClass.Ranged)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageTypes = 3;
                }
                if (damage == DamageClass.Magic)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageTypes = 4;
                }
                if (damage == DamageClass.Summon)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].damageTypes = 5;
                }
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A].type <= 0 && Who == 0)
                {
                    Who = A;
                }
            }
            return true;
        }
        public void BuffUpdate()
        {
            if (damageTypes == 0)
            {
                damageType = DamageClass.Default;
            }
            if (damageTypes == 1)
            {
                damageType = DamageClass.Generic;
            }
            if (damageTypes == 2)
            {
                damageType = DamageClass.Melee;
            }
            if (damageTypes == 3)
            {
                damageType = DamageClass.Ranged;
            }
            if (damageTypes == 4)
            {
                damageType = DamageClass.Magic;
            }
            if (damageTypes == 5)
            {
                damageType = DamageClass.Summon;
            }
            Precedence = false;
            Time--;
            if (type == 发光)
            {
                Lighting.AddLight(player.Center, Glow);
            }
            if (type == 夜视)
            {
                player.nightVision = true;
            }
            if (type == 伤害)
            {
                player.GetDamage(damageType) += Ratio;
            }
            if (type == 防御)
            {
                player.statDefense += (int)Ratio;
            }
            if (type == 防御2)
            {
                player.statDefense += (int)(player.statDefense * Ratio);
                Precedence = true;
            }
            if (type == 移速)
            {
                player.moveSpeed += Ratio;
            }
            if (type == 生命回复)
            {
                player.lifeRegen += (int)Ratio;
            }
            if (type == 生命)
            {
                player.statLifeMax2 += (int)Ratio;
            }
            if (type == 生命2)
            {
                player.statLifeMax2 += (int)(player.statLifeMax2 * Ratio);
                Precedence = true;
            }
            if (type == 魔力回复)
            {
                player.manaRegen += (int)Ratio;
            }
            if (type == 魔力)
            {
                player.statManaMax2 += (int)Ratio;
            }
            if (type == 魔力2)
            {
                player.statManaMax2 += (int)(Ratio * player.statManaMax2);
                Precedence = true;
            }
            if (type == 挖掘)
            {
                player.pickSpeed -= Ratio;
            }
        }
        public void SaveData(int a, TagCompound tag)
        {
            tag.Add("FoodType" + a, type);
            tag.Add("FoodTime" + a, Time);
            tag.Add("FoodRatio" + a, Ratio);
            tag.Add("FoodDeBuff" + a, DeBuff);
            tag.Add("FoodFunctionalBuff" + a, FunctionalBuff);
            tag.Add("FoodDamageType" + a, damageTypes);
            tag.Add("FoodGlow" + a, Glow);
        }
        public void LoadData(int a, TagCompound tag)
        {
            type = tag.Get<int>("FoodType" + a);
            Time = tag.Get<int>("FoodTime" + a);
            Ratio = tag.Get<float>("FoodRatio" + a);
            DeBuff = tag.Get<bool>("FoodDeBuff" + a);
            FunctionalBuff = tag.Get<bool>("FoodFunctionalBuff" + a);
            damageTypes = tag.Get<int>("FoodDamageTypes" + a);
            Glow = tag.Get<Vector3>("Glow" + a);
        }
        public void Send(int a, Mod mod)
        {
            ModPacket packet = mod.GetPacket(256);
            packet.Write((byte)DDType.PlayerFood);
            packet.Write(player.whoAmI);
            packet.Write(a);
            packet.Write(type);
            packet.Write(Time);
            packet.Write(Ratio);
            packet.Write(DeBuff);
            packet.Write(FunctionalBuff);
            packet.Write(damageTypes);
            packet.Write((int)Glow.X);
            packet.Write((int)Glow.Y);
            packet.Write((int)Glow.Z);
            packet.Send(-1, player.whoAmI);
        }
    }
}