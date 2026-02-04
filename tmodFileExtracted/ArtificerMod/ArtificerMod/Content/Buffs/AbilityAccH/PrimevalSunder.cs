using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace ArtificerMod.Content.Buffs.AbilityAccH
{
	public class PrimevalSunder : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<PrimevalSunderNPC>().debuffActive = true;

            if (!GeneralBossCheck(npc) && npc.realLife == -1 && npc.aiStyle != NPCAIStyleID.Worm && !npc.immortal)
            {
                npc.velocity.X *= 0.9f;
            }
			
			if(Main.rand.NextBool(10))
			{
				int dustType = Main.rand.NextBool(3) ? DustID.Poisoned : DustID.JunglePlants;
				Dust dust = Dust.NewDustDirect(npc.position + new Vector2(0, npc.height / 4f), npc.width, (int)(npc.height * 0.75f), dustType, 0f, -3f, 50, default, 1.1f);
				dust.noGravity = true;
			}
        }

        public static bool GeneralBossCheck(NPC npc)
        {
            if (npc.boss)
            {
                return true;
            }

            // Boss parts
            if (npc.type == NPCID.PrimeCannon || npc.type == NPCID.PrimeSaw || npc.type == NPCID.PrimeVice || npc.type == NPCID.PrimeLaser || npc.type == NPCID.SkeletronHand
                || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail
                || npc.type == NPCID.WallofFleshEye || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight || npc.type == NPCID.GolemHead
                || npc.type == NPCID.Creeper || npc.type == NPCID.PlanterasTentacle || npc.type == NPCID.MoonLordHand || npc.type == NPCID.MoonLordHead
                || npc.type == NPCID.PirateShipCannon || npc.type == NPCID.MartianSaucerTurret || npc.type == NPCID.MartianSaucerCannon)
            {
                return true;
            }

            // Mini bosses, event bosses, and the like
            if (npc.type == NPCID.DD2DarkMageT1 || npc.type == NPCID.DD2DarkMageT3 || npc.type == NPCID.DD2OgreT2 || npc.type == NPCID.DD2OgreT3 || npc.type == NPCID.DD2Betsy
                || npc.type == NPCID.PirateShip || npc.type == NPCID.MourningWood || npc.type == NPCID.Pumpking || npc.type == NPCID.PumpkingBlade
                || npc.type == NPCID.Everscream || npc.type == NPCID.SantaNK1 || npc.type == NPCID.IceQueen
                || npc.type == NPCID.LunarTowerSolar || npc.type == NPCID.LunarTowerNebula || npc.type == NPCID.LunarTowerStardust || npc.type == NPCID.LunarTowerVortex
                || npc.type == NPCID.DungeonGuardian || npc.type == NPCID.BloodNautilus || npc.type == NPCID.HeadlessHorseman || npc.type == NPCID.Paladin
                || npc.type == NPCID.SandElemental || npc.type == NPCID.IceGolem || npc.type == NPCID.GoblinShark)
            {
                return true;
            }

            return false;
        }
    }

    public class PrimevalSunderNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool debuffActive;

        public override void ResetEffects(NPC npc)
        {
            debuffActive = false;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if(debuffActive)
            {
                modifiers.ArmorPenetration += 30;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (debuffActive)
            {
                drawColor.R = (byte)(drawColor.R * 0.9f);
                drawColor.B = (byte)(drawColor.G * 0.8f);
            }
        }
    }
}