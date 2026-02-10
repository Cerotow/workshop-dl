
using DDmod.Content.NPCs.EliteMonster;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace DDmod.Content.Achievements;
public class 战宠出战 : ModAchievement
{

    public CustomFlagCondition Condition { get; private set; }
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
        Condition = AddCondition("战宠出战");
    }

    public override void Unload()
    {
    }
}