using Terraria.Graphics.Capture;

namespace DDmod.Content.Biome
{
    public class HallowBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override int Music => DDSystem.Music(1, "夜晚神圣地");

        public override bool IsBiomeActive(Player player) {
			return player.ZoneHallow && !Main.dayTime &&(player.position.Y< Main.worldSurface*16||player.ZoneDirtLayerHeight);
		}
		public override void SetStaticDefaults()
		{
		}
	}
}
