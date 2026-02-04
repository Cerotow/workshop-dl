using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Buffs.AbilityAccH
{
	public class CyborgCoreBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 24;
			player.lifeRegen += 8;
			player.runAcceleration *= 0.75f;
			player.maxRunSpeed *= 0.75f;
			player.accRunSpeed *= 0.75f;
			player.runSlowdown *= 1.4f;


			// Dust FX
			Dust dust;
			dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.HealingPlus)];
			dust.noGravity = true;
			dust.shader = GameShaders.Armor.GetSecondaryShader(86, Main.LocalPlayer);
		}
	}
}