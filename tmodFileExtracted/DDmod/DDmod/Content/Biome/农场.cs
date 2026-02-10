using DDmod.Content.Tiles.农场;
using Terraria.Graphics.Capture;

namespace DDmod.Content.Biome
{
	public class 农场System : ModSystem
	{
		public int 土块;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
		{
			土块 = tileCounts[ModContent.TileType<锄过的土块>()];
		}
	}
	public class 农场Player : ModPlayer
	{
		public bool 农场;
		//0早晨,1下午,2晚上,3凌晨
		//public int type;
		public override void ResetEffects()
        {
			农场 = ModContent.GetInstance<农场System>().土块 >= 20;
			/*
			if(Main.dayTime)
            {
				if (Main.time < DDHelper.Second(450))
                {
					type = 0;
                }
                else
                {
					type = 1;
				}
            }
            else
            {

				if (Main.time < DDHelper.Second(270))
				{
					type = 2;
				}
                else
				{
					type = 3;
				}

			}*/
			
		}


		public override void PreUpdate()
		{
		}
    }
	public class 农场 : ModBiome
	{
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;


		public override int Music => DDSystem.Music(1, "农场");
        /*{
			
			get
			{
				return Main.LocalPlayer.GetModPlayer<农场Player>().type switch
				{
					0 => MusicLoader.GetMusicSlot(Mod, "NoContent/Music/原野之花(早晨)"),
					1 => MusicLoader.GetMusicSlot(Mod, "NoContent/Music/原野之花(下午)"),
					2 => MusicLoader.GetMusicSlot(Mod, "NoContent/Music/原野之花(夜晚)"),
					_ => MusicLoader.GetMusicSlot(Mod, "NoContent/Music/原野之花(凌晨)"),
				};
			}
		}*/
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override string MapBackground => BackgroundPath;

		public override bool IsBiomeActive(Player player) {
			return player.GetModPlayer<农场Player>().农场;
		}
		public override void SetStaticDefaults()
		{
		}
	}
}
