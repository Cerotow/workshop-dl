using DDmod.Content.Tiles.农场;
using DDmod.Content.Tiles.绿岩;
using DDmod.Worlds;
using Terraria.Graphics.Capture;

namespace DDmod.Content.Biome
{
	public class 绿岩实验室System : ModSystem
	{
		public int Style;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
		{
            Style = tileCounts[ModContent.TileType<绿岩格网块Tile>()]+ tileCounts[ModContent.TileType<绿岩砖Tile>()];
		}
	}
	public class 绿岩实验室Player : ModPlayer
	{
		public bool Biome;
		public override void ResetEffects()
		{
			Point16 point = new Point16((int)(Player.Center.X / 16), (int)(Player.Center.Y / 16));
			if ((Main.tile[point.X, point.Y].WallType == ModContent.WallType<绿岩砖墙Tile>() || Main.tile[point.X, point.Y].WallType == ModContent.WallType<绿岩格网墙Tile>() || Main.tile[point.X, point.Y].WallType == ModContent.WallType<绿岩墙Tile>())&& point.Y>DDWorld.GreenRockLab.Y+60)
			{
				Biome = ModContent.GetInstance<绿岩实验室System>().Style >= 20;
			}
			else
			{
				Biome = false;

            }
		}

		public override void PreUpdate()
		{
			if(Biome)
             Player.Dplayer().ForbidSpawn = 10;
        }
    }
	public class 绿岩实验室 : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override int Music => DDSystem.Music(1, "绿岩实验室");
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override void MapBackgroundColor(ref Color color)
        {
			color = Color.White;
        }
        public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override string MapBackground => BackgroundPath;

		public override bool IsBiomeActive(Player player)
		{
			return player.GetModPlayer<绿岩实验室Player>().Biome;
		}
		public override void SetStaticDefaults()
		{
		}
	}
}
