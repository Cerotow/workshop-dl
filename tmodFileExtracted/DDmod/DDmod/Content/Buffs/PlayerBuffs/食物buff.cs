using DDmod.Content.Items;
using DDmod.Content.Items.农场;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 食物buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            for (int A = 0; A < player.GetModPlayer<FoodPlayer>().FoodBuff.Length; A++)
            {
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A] != null && player.GetModPlayer<FoodPlayer>().FoodBuff[A].type > 0 && !player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff && player.GetModPlayer<FoodPlayer>().FoodBuff[A].Precedence)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].BuffUpdate();
                }
            }
            for (int A = 0; A < player.GetModPlayer<FoodPlayer>().FoodBuff.Length; A++)
            {
                if (player.GetModPlayer<FoodPlayer>().FoodBuff[A] != null && player.GetModPlayer<FoodPlayer>().FoodBuff[A].type > 0 && !player.GetModPlayer<FoodPlayer>().FoodBuff[A].DeBuff && !player.GetModPlayer<FoodPlayer>().FoodBuff[A].Precedence)
                {
                    player.GetModPlayer<FoodPlayer>().FoodBuff[A].BuffUpdate();
                }
            }
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            Player player = Main.LocalPlayer;
            FoodPlayer food = player.GetModPlayer<FoodPlayer>();
            string text = "";
            for (int A = 0; A < player.GetModPlayer<FoodPlayer>().FoodBuff.Length; A++)
            {
                FoodBuff buff = food.FoodBuff[A];
                int Time = buff.Time / 60;
                string T = Time + Language.GetTextValue("Mods.DDmod.Tooltips.Second");
                if (buff != null && buff.type > 0)
                {
                    if (!buff.DeBuff)
                    {
                        if (buff.type == FoodBuff.夜视)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.夜视") + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.发光)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.发光") + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.伤害)
                        {
                            string Damage = "";
                            if (buff.damageType == DamageClass.Generic)
                            {
                                Damage = Language.GetTextValue("Mods.DDmod.properties.伤害");
                            }
                            if (buff.damageType == DamageClass.Melee)
                            {
                                Damage = Language.GetTextValue("Mods.DDmod.properties.近战");
                            }
                            if (buff.damageType == DamageClass.Summon)
                            {
                                Damage = Language.GetTextValue("Mods.DDmod.properties.召唤");
                            }
                            if (buff.damageType == DamageClass.Ranged)
                            {
                                Damage = Language.GetTextValue("Mods.DDmod.properties.远程");
                            }
                            if (buff.damageType == DamageClass.Magic)
                            {
                                Damage = Language.GetTextValue("Mods.DDmod.properties.魔法");
                            }
                            text += Damage + " +" + (buff.Ratio * 100).ToString("F1") + "% " + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.防御)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.防御") + " +" + buff.Ratio + "" + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.防御2)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.防御") + " +" + (buff.Ratio * 100).ToString("F1") + "%" + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.移速)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.移动速度") + " +" + (buff.Ratio * 100).ToString("F1") + "%" + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.生命回复)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.生命回复") + " +" + buff.Ratio + "" + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.生命)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.生命") + " +" + buff.Ratio + "" + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.生命2)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.生命") + " +" + (buff.Ratio * 100).ToString("F1") + "%" + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.魔力回复)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.魔力回复") + " +" + buff.Ratio + "" + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.魔力)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.魔力") + " +" + buff.Ratio + "" + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.魔力2)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.魔力") + " +" + (buff.Ratio * 100).ToString("F1") + "%" + "(" + T + ")" + "\n";
                        }
                        if (buff.type == FoodBuff.挖掘)
                        {
                            text += Language.GetTextValue("Mods.DDmod.properties.挖掘速度") + " +" + (buff.Ratio * 100).ToString("F1") + "%" + "(" + T + ")" + "\n";
                        }
                    }
                }
            }
            tip = text;
        }
    }
}