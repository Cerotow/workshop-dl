
using DDmod.Content.NPCs.EliteMonster;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace DDmod.Content.Achievements;
public class 黎明初升 : ModAchievement
{

    public CustomFlagCondition Condition { get; private set; }
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
        Condition = AddCondition("黎明初升");
        Player.Hooks.OnEnterWorld += Items;
    }

    public override void Unload()
    {
        Player.Hooks.OnEnterWorld -= Items;
    }

    private void Items(Player player)
    {
        if (player.whoAmI != Main.myPlayer || Condition.IsCompleted)
        {
            return;
        }
        Condition.Complete();
    }
}