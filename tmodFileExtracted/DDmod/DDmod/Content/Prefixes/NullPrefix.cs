namespace DDmod.Content.Prefixes
{
    public class TalismanPrefix : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override float RollChance(Item item)
        {
            return 1f;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override string Name => "妈妈";
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
        }

        public override void ModifyValue(ref float valueMult)
        {
        }

        public override void Apply(Item item)
        {
        }
    }
    public class 超级无敌宇宙第一大帝 : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Melee;

        public override float RollChance(Item item)
        {
            return 0f;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 114514;
            useTimeMult *= 0.1F;
            critBonus += 100;
        }

        public override void ModifyValue(ref float valueMult)
        {
        }

        public override void Apply(Item item)
        {
        }
    }
    public class 神级 : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override float RollChance(Item item)
        {
            return 0f;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 1.2f;
            critBonus += 8;
            knockbackMult *= 1.2f;
        }

        public override void ModifyValue(ref float valueMult)
        {
        }

        public override void Apply(Item item)
        {
        }
    }
    public class 神级2 : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override float RollChance(Item item)
        {
            return 0f;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 1.25f;
            knockbackMult *= 1.05f;
        }

        public override void ModifyValue(ref float valueMult)
        {
        }

        public override void Apply(Item item)
        {
        }
    }
    public class 恶魔 : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override float RollChance(Item item)
        {
            return 0f;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 1.2f;
            critBonus += 8;
        }

        public override void ModifyValue(ref float valueMult)
        {
        }

        public override void Apply(Item item)
        {
        }
    }
    public class 传奇 : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Melee;

        public override float RollChance(Item item)
        {
            return 0f;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 1.2f;
            knockbackMult *= 1.2f;
            scaleMult *= 1.15f;
            useTimeMult *= 0.85F;
            critBonus += 8;
        }

        public override void ModifyValue(ref float valueMult)
        {
        }

        public override void Apply(Item item)
        {
        }
    }
    public class 虚幻 : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Ranged;

        public override float RollChance(Item item)
        {
            return 0f;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 1.2f;
            knockbackMult *= 1.2f;
            shootSpeedMult *= 1.15f;
            useTimeMult *= 0.85F;
            critBonus += 8;
        }

        public override void ModifyValue(ref float valueMult)
        {
        }

        public override void Apply(Item item)
        {
        }
    }
    public class 神话 : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Magic;

        public override float RollChance(Item item)
        {
            return 0f;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 1.2f;
            knockbackMult *= 1.2f;
            manaMult *= 0.85f;
            useTimeMult *= 0.85F;
            critBonus += 8;
        }

        public override void ModifyValue(ref float valueMult)
        {
        }

        public override void Apply(Item item)
        {
        }
    }
}