
using DDmod.Content.NPCs.EliteMonster;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace DDmod.Content.Achievements;
public class 强化成功 : ModAchievement
{

    public CustomFlagCondition Condition { get; private set; }
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
        Condition = AddCondition("强化成功");
        AchievementsHelper.OnItemPickup += Items;
    }

    public override void Unload()
    {
        AchievementsHelper.OnItemPickup -= Items;
    }

    private void Items(Player player, short itemId, int count)
    {
        if (player.whoAmI != Main.myPlayer || Condition.IsCompleted)
        {
            return;
        }
        for (int A = 0; A < player.inventory.Length; A++)
        {
            if (player.inventory[A].type > 0 && player.inventory[A].GetGlobalItem<StrengthenGlobalItem>().Level > 0)
            {
                Condition.Complete();
                break;
            }
        }
    }
    public override Position GetDefaultPosition() => new After("OOO_SHINY");
    public override IEnumerable<Position> GetModdedConstraints()
    {
        yield return new After(ModContent.GetInstance<强化台>());
    }
}