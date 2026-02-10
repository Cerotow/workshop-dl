
using DDmod.Content.NPCs.EliteMonster;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace DDmod.Content.Achievements;
public class 强化台 : ModAchievement
{

    public CustomFlagCondition Condition { get; private set; }
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Explorer);
        Condition = AddCondition("强化台");
    }
    public override void OnCompleted(Achievement achievement)
    {
        base.OnCompleted(achievement);
    }
    public override void Unload()
	{
    }
    public override Position GetDefaultPosition() => new After("OOO_SHINY");

}
