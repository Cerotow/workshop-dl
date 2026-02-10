using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.Strengthen;
using Terraria;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 星尘共鸣Buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.Dnpc().BossPhysique)
            {
                npc.Dnpc().MoveSpeed *= 0.75f;
            }
            else
            {
                npc.Dnpc().MoveSpeed *= 0.25f;
            }
            int A;
            if (Main.rand.NextBool(5))
            {
                A = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<星光粒子>(), 0, 0, 100, new Color(40, 185, 255, 0), Main.rand.NextFloat(0.75F, 1.75F));
            }
            A = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(40, 185, 255, 0), Main.rand.NextFloat(0.5F, 1.25F));
            Main.dust[A].customData = -1002;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetDamage(DamageClass.Summon) += 0.2F;
            if(Main.rand.NextBool(5))
            {
                int A = Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<星光粒子>(), 0, 0, 100, new Color(40, 185, 255, 0),Main.rand.NextFloat(0.75F,1.75F));
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<星尘核心Proj>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
