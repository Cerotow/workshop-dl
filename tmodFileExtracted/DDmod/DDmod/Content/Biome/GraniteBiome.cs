using Terraria.Graphics.Capture;
using static Terraria.Graphics.Capture.CaptureBiome;

namespace DDmod.Content.Biome
{
    public class GraniteBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override int Music => DDSystem.Music(1,"花岗岩之地");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

		public override bool IsBiomeActive(Player player) {
			return player.ZoneGranite;
		}
		public override void SetStaticDefaults()
		{
		}
	}
	//地狱
    public class UnderworldBiome : ModBiome
	{
		public override bool IsBiomeActive(Player player) {
			return player.ZoneUnderworldHeight;
		}
	}
	//天空
    public class SkyBiome : ModBiome
	{

        public override bool IsBiomeActive(Player player) {
			return player.ZoneSkyHeight;
		}
		public override void SetStaticDefaults()
		{
		}
	}
}
