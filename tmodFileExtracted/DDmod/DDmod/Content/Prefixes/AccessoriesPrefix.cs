namespace DDmod.Content.Prefixes
{
    public class AccessoriesPrefix : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;
       public float Damage = 0;
       public int Defense = 0;
       public int Crit = 0;
       public float MoveSpeed = 0;
       public float MeleeSpeed = 0;

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
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult += (Damage * 11F) + (Defense * 0.11F) + (Crit * 0.11F) + (MoveSpeed * 11F) + (MeleeSpeed * 11F);
        }
        public override void ApplyAccessoryEffects(Player player)
        {
            if (Damage > 0)
            {
                player.GetDamage(DamageClass.Generic) += Damage;
            }
            if (Defense > 0)
            {
                player.statDefense += Defense;
            }
            if (Crit > 0)
            {
                player.GetCritChance(DamageClass.Generic) += Crit;
            }
            if (MoveSpeed > 0)
            {
                player.moveSpeed += MoveSpeed;
            }
            if (MeleeSpeed > 0)
            {
                player.GetAttackSpeed(DamageClass.Melee) += MeleeSpeed;
            }
        }
        public override void Apply(Item item)
        {
            item.AccessoryItem().Damage = Damage;
            item.AccessoryItem().Defense = Defense;
            item.AccessoryItem().Crit = Crit;
            item.AccessoryItem().MoveSpeed = MoveSpeed;
            item.AccessoryItem().MeleeSpeed = MeleeSpeed;
        }
    }
    public class 猛烈 : AccessoriesPrefix
    {
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            Damage = 0.05F;
        }
    }
    public class 洞悉 : AccessoriesPrefix
    {
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
           Crit = 5;
        }
    }
    public class 庇佑 : AccessoriesPrefix
    {
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            Defense = 5;
        }
    }
    public class 奔流 : AccessoriesPrefix
    {
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            MoveSpeed = 0.05f;
        }
    }
    public class 疯狂 : AccessoriesPrefix
    {
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            MeleeSpeed = 0.05F;
        }
    }
}