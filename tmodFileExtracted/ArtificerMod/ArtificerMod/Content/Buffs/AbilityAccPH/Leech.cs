using ArtificerMod.Content.Projectiles.AbilityAccPH;
using ArtificerMod.Content.Projectiles.AbilityAccH;
using Terraria;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Buffs.AbilityAccPH
{
	public class Leech : ModBuff
	{
        public override void Update(NPC npc, ref int buffIndex)
        {
			npc.GetGlobalNPC<LeechNPC>().leeched = true;
		}
	}

    public class Leech2 : ModBuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<LeechNPC>().leeched2 = true;
        }
    }

    public class LeechNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool leeched;
        public bool leeched2;

        public override void ResetEffects(NPC npc)
        {
            leeched = leeched2 = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (leeched || leeched2)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                int dmgOverTime = 0;
                foreach (var proj in Main.ActiveProjectiles)
                {
                    if ((proj.type == ModContent.ProjectileType<DarkLeechHook>() || proj.type == ModContent.ProjectileType<LeechingTetherHook>()
                        || proj.type == ModContent.ProjectileType<LifelineHook>())
                        && proj.ai[0] == 1f && proj.ai[1] == npc.whoAmI)
                    {
                        if (proj.type == ModContent.ProjectileType<LifelineHook>())
                        {
                            dmgOverTime += 100;
                        }
                        else
                        {
                            dmgOverTime += 20;
                        }
                    }
                }

                if(dmgOverTime <= 0)
                {
                    dmgOverTime = leeched2 ? 100 : 20;
                }

                npc.lifeRegen -= 2 * dmgOverTime;
                if (damage < dmgOverTime)
                {
                    damage = dmgOverTime;
                }
            } 
        }
    }
}