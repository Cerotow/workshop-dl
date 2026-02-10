using DDmod.Content.Items.Sundries;
using DDmod.Modkey;
using DDmod.Players;
using DDmod.UI.HunterQuests;
using DDmod.UI.ItemUI;
using DDmod.UI.ItemUI.背包;
using System.Collections;
using Terraria.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace DDmod.UI.BattlePetUI.技能
{
    public class Skill
    {
        public static string Text(int Type,BattlePets pets)
        {
            switch (Type)
            {
                case 1:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.强攻");
                case 2:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.狂暴");
                case 3:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.嗜血");
                case 4:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.嗜血幻象");
                case 5:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.节能");
                case 6:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.飞羽");
                case 7:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.撕裂");
                case 8:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.治愈神羽");
                case 9:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.久战骸骨");
                case 10:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.三重颅骨");
                case 11:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.冥思");
                case 12:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.寒霜颅骨");
                case 13:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.寒霜领域");
                case 14:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.寒霜庇护");
                case 15:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.装甲");
                case 16:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.无畏冲锋");
                case 17:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.坚韧盔甲");
                case 18:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.固若金汤", pets.Defense);
                case 19:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.狱炎");
                case 20:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.狱火焚身");
                case 21:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.激怒");
                case 22:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.地狱幻影");
                case 23:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.地狱武士");
                case 24:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.地狱颅骨");
                case 25:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.地狱装甲");
                case 26:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.赤红炎舞");
                case 27:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.暗咒");
                case 28:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.夺魂愈疗");
                case 29:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.魂怒共鸣");
                case 30:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.魂怒反噬");
                default:
                    return Language.GetTextValue("Mods.DDmod.BattlePetSkill.未知");
            }
        }
        public static void Update(int Type,BattlePets pets)
        {
            if(Type==1)
            {
                pets.AddDamage += 0.2f;
            }
            if(Type== 11)
            {
                pets.AddLife -= 0.3f;
                pets.AddDefense -= 0.3f;
                pets.AddDamage += 0.2f;
            }
            if(Type== 15)
            {
                pets.AddLife += 0.2f;
                pets.AddDefense += 0.2f;
                pets.AddDamage += 0.1f;
            }
            if(Type== 17)
            {
                pets.Endurance+=0.25F;
            }
            if(Type== 19)
            {
                pets.AddLife += 0.35f;
                pets.AddDefense -= 0.1f;
                pets.AddDamage += 0.2f;
            }
            if(Type== 23)
            {
                pets.AddDefense += 0.1f;
                pets.AddDamage += 0.1f;
            }
            if (Type == 25)
            {
                pets.Endurance += 0.10F;
            }
            if (Type == 27)
            {
                pets.Endurance += 0.10F;
            }
        }
        public static void OnHitNPC(int Type, BattlePets pets, NPC target, HitInfo hit, int damageDone)
        {
            if (Type == 3)
            {

            }
        }
    }
}