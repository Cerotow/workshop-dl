using DDmod.Content.Buffs.DeBuffs;
using Terraria;

namespace DDmod.Content.NPCs
{
    public class DGlobalNPCBuff : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int BuffDamage;
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (BuffDamage > 0)
            {
                damage = BuffDamage;
            }
            //水和火
            if (npc.HasBuff(103)&&npc.Dnpc().Properties.Fire)
            {
                npc.lifeRegen -= 50;
            }
            //普通流血
            if (npc.HasBuff(30))
            {
                npc.lifeRegen -= 17;
            }
            damage = -npc.lifeRegen / 10;
            if(damage<1)
            {
                damage = 1;
            }
            if (damage == 1 && Math.Abs(npc.lifeRegenCount) >= 240)
            {
                damage = Math.Abs(npc.lifeRegenCount) / 120;
            }
        }

        public override void ResetEffects(NPC npc)
        {
            BuffDamage = 0;
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            //短剑流血
            if (npc.HasBuff(ModContent.BuffType<Bleed>()))
            {
                drawColor = new Color(155, 0, 0);
            }
            //邪恶缠绕
            if (npc.HasBuff(ModContent.BuffType<EvilEntanglement>()))
            {
                drawColor = new Color(65, 32, 149);
            }
            //普通流血
            if (npc.HasBuff(30))
            {
                drawColor = new Color(135, 20, 20);
            }
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if(npc.HasBuff(70))
            {
                modifiers.ModifyHurtInfo += Modifiers_ModifyHurtInfo;
            }
        }

        private void Modifiers_ModifyHurtInfo(ref Player.HurtInfo info)
        {
            info.Damage = (int)(info.Damage * 0.8F);
        }
    }
}