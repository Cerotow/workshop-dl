
using DDmod.Content.NPCs.EliteMonster;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace DDmod.Content.Achievements;
public class 完美伙伴 : ModAchievement
{

    public CustomFlagCondition Condition { get; private set; }
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
        Condition = AddCondition("完美伙伴");
    }

    public override void Unload()
    {
    }
}