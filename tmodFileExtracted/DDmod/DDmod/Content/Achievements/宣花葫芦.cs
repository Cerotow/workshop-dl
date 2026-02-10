
using DDmod.Content.NPCs.EliteMonster;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace DDmod.Content.Achievements;
public class 宣花葫芦 : ModAchievement
{

    public CustomFlagCondition Condition { get; private set; }
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
        Condition = AddCondition("宣花葫芦");
        
    }

    public override void Unload()
    {
    }

}