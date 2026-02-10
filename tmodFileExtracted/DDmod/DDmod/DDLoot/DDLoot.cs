namespace DDmod.DDLoot
{
    //NPC
    public static class DDNPCLoot
    {
        /// <summary>
        /// 普通模式掉落(无视幸运)
        /// <para> chanceDenominatorInNormal普通模式掉率</para>
        /// <para>options物品数组</para> 
        /// </summary>
        public static void NormalLoot(this NPCLoot loot, int chanceDenominatorInNormal, params int[] options)
        {
            loot.Add(new DropBasedOnExpertMode(ItemDropRule.NormalvsExpertOneFromOptionsNotScalingWithLuck(chanceDenominatorInNormal, 0, options), ItemDropRule.DropNothing()));
        }
        /// <summary>
        /// 普通模式掉落(无视幸运)
        /// <para>chanceDenominatorInNormal普通模式掉率</para>
        /// <para>item物品</para> 
        /// <para>minimumDropped最小数量</para> 
        /// <para>maximumDropped最大数量</para> 
        /// </summary>
        public static void NormalLoot(this NPCLoot loot, int chanceDenominatorInNormal, int item, int minimumDropped = 1, int maximumDropped = 1)
        {
            loot.Add(new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(item, chanceDenominatorInNormal, minimumDropped, maximumDropped), ItemDropRule.DropNothing()));
        }
        /// <summary>
        /// 基于普通,专家,大师掉落率
        /// <para>item物品 </para>
        /// <para>chanceDenominatorInNormal普通模式掉率</para>
        /// <para>chanceDenominatorInExpert专家模式掉率</para>
        /// <para>chanceDenominatorInMaster大师模式掉率</para> 
        /// </summary>
        public static void CompleteModeLoot(this NPCLoot npcLoot, int item, int chanceDenominatorInNormal, int chanceDenominatorInExpert, int chanceDenominatorInMaster)
        {
            npcLoot.Add(new DropBasedOnCompleteMode(
                chanceDenominatorInNormal == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInNormal),
                chanceDenominatorInExpert == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInExpert),
                chanceDenominatorInMaster == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInMaster)));
        }
        public static void CompleteModeLoot(this NPCLoot npcLoot, int item, int chanceDenominatorInNormal, int chanceDenominatorInExpert, int chanceDenominatorInMaster, int Dropped)
        {
            npcLoot.Add(new DropBasedOnCompleteMode(
                chanceDenominatorInNormal == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInNormal),
                chanceDenominatorInExpert == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInExpert),
                chanceDenominatorInMaster == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInMaster)));
        }
        public static void CompleteModeLoot(this NPCLoot npcLoot, int item, int chanceDenominatorInNormal, int chanceDenominatorInExpert, int chanceDenominatorInMaster, int miniDropped, int maxDropped)
        {
            npcLoot.Add(new DropBasedOnCompleteMode(
                chanceDenominatorInNormal == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInNormal),
                chanceDenominatorInExpert == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInExpert),
                chanceDenominatorInMaster == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInMaster)));
        }
        public static void CompleteModeLoot(this NPCLoot npcLoot, int item, int chanceDenominatorInNormal, int chanceDenominatorInExpert, int chanceDenominatorInMaster, bool? Boss = null, List<int> ints = null)
        {
            npcLoot.Add(new DropBasedOnCompleteMode(
                chanceDenominatorInNormal == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInNormal),
                chanceDenominatorInExpert == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInExpert),
                chanceDenominatorInMaster == 0 ? ItemDropRule.DropNothing() : ItemDropRule.Common(item, chanceDenominatorInMaster), Boss, ints));
        }
        /// <summary>
        /// 特殊武器掉率(0稀有,1史诗,2传说)
        /// </summary>
        /// <param name="npcLoot"></param>
        /// <param name="item"></param>
        /// <param name="chanceDenominatorInNormal"></param>
        /// <param name="chanceDenominatorInExpert"></param>
        /// <param name="chanceDenominatorInMaster"></param>
        public static void SpecialLoot(this NPCLoot npcLoot, int item, int Rarity, bool? Boss = null, List<int> ints = null)
        {
            if (Rarity == 0)
            {
                npcLoot.CompleteModeLoot(item, 30, 20, 10, Boss, ints);
            }
            else if (Rarity == 1)
            {
                npcLoot.CompleteModeLoot(item, 45, 35, 25, Boss, ints);
            }
            else
            {
                npcLoot.CompleteModeLoot(item, 80, 60, 40, Boss, ints);
            }
        }
    }
    //物品
    public static class DDItemLoot
    {
        /// <summary>
        /// 袋子(无视幸运)
        /// <para>item物品</para> 
        /// <para>minimumDropped最小数量</para> 
        /// <para>maximumDropped最大数量</para> 
        /// </summary>
        public static void NormalLoot(this ItemLoot itemLoot, int chanceDenominatorInNormal, int item, int minimumDropped = 1, int maximumDropped = 1)
        {
            itemLoot.Add(new DropBasedOnExpertMode(ItemDropRule.NotScalingWithLuck(item, chanceDenominatorInNormal, minimumDropped, maximumDropped), ItemDropRule.NotScalingWithLuck(item, chanceDenominatorInNormal, minimumDropped, maximumDropped)));
        }
        /// <summary>
        /// 袋子(无视幸运)
        /// <para>chanceDenominator掉率分母</para>
        /// <para>chanceNumerator掉率分子</para>
        /// <para>minimumDropped最少掉落</para> 
        /// <para>maximumDropped最大掉落</para> 
        /// <para>options物品数组</para> 
        /// </summary>
        public static void NormalLoot(this ItemLoot itemLoot, int chanceDenominator, int chanceNumerator, int minimumDropped = 1, int maximumDropped = 1, params int[] options)
        {
            itemLoot.Add(new FromOptionsNotScaledWithLuckDropRule(chanceDenominator, chanceNumerator, minimumDropped, maximumDropped, options));
        }
    }

}