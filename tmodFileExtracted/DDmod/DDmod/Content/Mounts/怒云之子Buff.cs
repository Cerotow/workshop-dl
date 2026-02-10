using Terraria;
using Terraria.ModLoader;

namespace DDmod.Content.Mounts
{
	public class 怒云之子Buff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<怒云之子>(), player);
			player.buffTime[buffIndex] = 10;
			player.noKnockback = true;
        }
	}
}
