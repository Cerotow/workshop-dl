using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Buffs.AbilityAccPH
{
	public class DarkCurse : ModBuff
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<DarkCurseNPC>().debuffActive = true;

            if (Main.rand.NextBool(10)) // Adapted from On Fire! debuff
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Shadowflame,
                    npc.velocity.X, npc.velocity.Y, 0, default, 1f);
                dust.noGravity = true;
                dust.velocity.Y += Main.rand.NextFloat(2f, 4f);
            }
        }
	}

    public class DarkCurseNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool debuffActive;

        public override void ResetEffects(NPC npc)
        {
            debuffActive = false;
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (debuffActive)
            {
                modifiers.FinalDamage *= 0.9f;
            }
        }
    }

    // Handles Dark Curse lowering projectile damage
    public class DarkCruseGlobalProj : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		private bool powerStolen = false;

		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return !entity.friendly && entity.hostile;
		}

		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			if (source is EntitySource_Parent parent && parent.Entity is NPC npc)
			{
				if (npc.TryGetGlobalNPC(out DarkCurseNPC globalNPC))
				{
                    if (globalNPC.debuffActive)
                    {
						powerStolen = true;
					}
				}
			}
		}

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (powerStolen)
            {
				modifiers.FinalDamage *= 0.9f;
            }
        }
    }
}