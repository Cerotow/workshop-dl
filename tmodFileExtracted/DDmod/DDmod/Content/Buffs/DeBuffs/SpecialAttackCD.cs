namespace DDmod.Content.Buffs.DeBuffs
{
    public class SpecialAttackCD : ModBuff
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("SpecialAttackCD");
           //DisplayName.AddTranslation(7, "特殊攻击CD");
          //Description.SetDefault("Weapon Skill Cooldown");
           //Description.AddTranslation(7, "武器技能冷却");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex]==2)
            {
                SoundStyle sound = SoundID.Item4;
                sound.Pitch = -0.5F;
                PlaySound(sound);
            }
        }
    }
}