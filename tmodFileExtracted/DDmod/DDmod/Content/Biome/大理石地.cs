using Terraria.Graphics.Capture;

namespace DDmod.Content.Biome
{
    public class 大理石地 : ModBiome
	{
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

		public override int Music => DDSystem.Music(1, "遗弃");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

		public override bool IsBiomeActive(Player player) {
			return player.ZoneMarble;
		}
		public override void SetStaticDefaults()
		{
		}
	}
}
