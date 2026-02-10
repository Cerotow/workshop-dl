

using DDmod.Content.Items.Boss.Boss特殊;
using Terraria;
using static DDmod.NoContent.Config.DDConfigClient;

namespace DDmod.DDLoot
{  /// <summary>
   /// 全部模式掉落规则
   /// </summary>
    public class DropBasedOnCompleteMode : IItemDropRule, INestedItemDropRule
    {
        public IItemDropRule ruleForNormalMode;

        public IItemDropRule ruleForExpertMode;

        public IItemDropRule ruleForMasterMode;
        public bool Boss;
        public List<int> NPCs;

        public List<IItemDropRuleChainAttempt> ChainedRules
        {
            get;
            private set;
        }

        public DropBasedOnCompleteMode(IItemDropRule ruleForNormalMode, IItemDropRule ruleForExpertMode, IItemDropRule ruleForMasterMode, bool? Boss = null, List<int> ints = default)
        {
            if (ints==null)
            {
                ints = [];
            }
            this.ruleForNormalMode = ruleForNormalMode;
            this.ruleForExpertMode = ruleForExpertMode;
            this.ruleForMasterMode = ruleForMasterMode;
            this.NPCs = ints;
            ChainedRules = new List<IItemDropRuleChainAttempt>();
            if (Boss != null)
            {
                this.Boss = Boss.Value;
            }
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (NPCs != null && NPCs.Count > 0)
            {
                for (int N = 0; N < NPCs.Count; N++)
                {
                    if (NPCs[N] == info.npc.type)
                    {
                        continue;
                    }
                    for (int A = 0; A < 200; A++)
                    {
                        if (Main.npc[A].active && Main.npc[A].type == NPCs[N])
                        {
                            return false;
                        }
                    }
                }
            }
            if (Boss)
            {
                if (!info.npc.boss)
                {
                    return false;
                }
            }
            /*
            CommonDrop drop = ruleForMasterMode as CommonDrop;
            if(drop!=null)
            {
                drop.chanceNumerator = drop.chanceDenominator;
                ruleForMasterMode = drop;
            }*/
            if (info.IsMasterMode && ruleForMasterMode.CanDrop(info))
            {
                return true;
            }
            if (info.IsExpertMode && ruleForExpertMode.CanDrop(info))
            {
                return true;
            }
            if (ruleForNormalMode.CanDrop(info))
            {
                return true;
            }
            return false;
        }

        public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
        {
            ItemDropAttemptResult result = default;
            result.State = ItemDropAttemptResultState.DidNotRunCode;
            return result;
        }

        public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
        {
            if (info.IsMasterMode)
            {
                return resolveAction(ruleForMasterMode, info);
            }
            if (info.IsExpertMode)
            {
                return resolveAction(ruleForExpertMode, info);
            }

            return resolveAction(ruleForNormalMode, info);
        }

        public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
        {
            DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
            ratesInfo2.AddCondition(new Conditions.IsMasterMode());
            ruleForMasterMode.ReportDroprates(drops, ratesInfo2);

            DropRateInfoChainFeed ratesInfo3 = ratesInfo.With(1f);
            ratesInfo3.AddCondition(new Expert());
            ruleForExpertMode.ReportDroprates(drops, ratesInfo3);

            DropRateInfoChainFeed ratesInfo4 = ratesInfo.With(1f);
            ratesInfo4.AddCondition(new Conditions.NotExpert());
            ruleForNormalMode.ReportDroprates(drops, ratesInfo4);

            Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
        }

        public class Expert : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                return Main.expertMode && !Main.masterMode;
            }

            public bool CanShowItemDropInUI()
            {
                return Main.expertMode && !Main.masterMode;
            }

            public string GetConditionDescription()
            {
                return Language.GetTextValue("Bestiary_ItemDropConditions.IsExpert");
            }
        }
    }
    /// <summary>
    /// 数组掉落规则
    /// </summary>
    public class FromOptionsNotScaledWithLuckDropRule : IItemDropRule
    {
        public int[] dropIds;

        public int chanceDenominator;

        public int chanceNumerator;

        public int Minquantity;

        public int Maxquantity;

        public List<IItemDropRuleChainAttempt> ChainedRules
        {
            get;
            private set;
        }

        public FromOptionsNotScaledWithLuckDropRule(int chanceDenominator, int chanceNumerator, int Minquantity, int Maxquantity, params int[] options)
        {
            this.chanceDenominator = chanceDenominator;
            this.Minquantity = Minquantity;
            this.Maxquantity = Maxquantity;
            dropIds = options;
            this.chanceNumerator = chanceNumerator;
            ChainedRules = new List<IItemDropRuleChainAttempt>();
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            return true;
        }

        public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
        {
            ItemDropAttemptResult result;
            if (info.rng.Next(chanceDenominator) < chanceNumerator)
            {
                CommonCode.DropItem(info, dropIds[info.rng.Next(dropIds.Length)], info.rng.Next(Minquantity, Maxquantity));
                result = default;
                result.State = ItemDropAttemptResultState.Success;

                return result;
            }

            result = default;
            result.State = ItemDropAttemptResultState.FailedRandomRoll;
            return result;
        }

        public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
        {
            float num = chanceNumerator / (float)chanceDenominator;
            float num2 = num * ratesInfo.parentDroprateChance;
            float dropRate = 1f / dropIds.Length * num2;
            for (int i = 0; i < dropIds.Length; i++)
            {
                drops.Add(new DropRateInfo(dropIds[i], Minquantity, Maxquantity, dropRate, ratesInfo.conditions));
            }

            Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
        }
    }
    /// <summary>
    /// 针对客户端掉落
    /// </summary>
    public class DropLocalPerClient : CommonDrop
    {
        public IItemDropRuleCondition condition;

        public DropLocalPerClient(int itemId, int chanceDenominator=1, int amountDroppedMinimum=1, int amountDroppedMaximum=1, IItemDropRuleCondition optionalCondition=null)
            : base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum)
        {
            condition = optionalCondition;
        }

        public override bool CanDrop(DropAttemptInfo info)
        {
            if (condition != null)
                return condition.CanDrop(info);

            return true;
        }

        public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
        {
            ItemDropAttemptResult result;
            if (info.rng.Next(chanceDenominator) < chanceNumerator)
            {
                DropItemLocalPerClient(info.npc, itemId, info.rng.Next(amountDroppedMinimum, amountDroppedMaximum + 1));
                result = default(ItemDropAttemptResult);
                result.State = ItemDropAttemptResultState.Success;
                return result;
            }

            result = default(ItemDropAttemptResult);
            result.State = ItemDropAttemptResultState.FailedRandomRoll;
            return result;
        }
        public static void DropItemLocalPerClient(NPC npc, int itemId, int stack, bool interactionRequired = true)
        {
            if (itemId <= 0 || itemId >= ItemLoader.ItemCount)
                return;

            if (Main.netMode == 2)
            {
                int num = Item.NewItem(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, itemId, stack, noBroadcast: true, -1);
                Main.timeItemSlotCannotBeReusedFor[num] = 54000;
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && (npc.playerInteraction[i] || !interactionRequired))
                        NetMessage.SendData(90, i, -1, null, num);
                }

                Main.item[num].active = false;
            }
            else
            {
                if (itemId > 0 && itemId < ItemLoader.ItemCount)
                {
                    int itemIndex = CommonCode.DropItem(npc.Hitbox, npc.GetSource_Loot(), itemId, stack,false);
                    CommonCode.ModifyItemDropFromNPC(npc, itemIndex);
                }
            }
        }
    }
}
