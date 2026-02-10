
using DDmod.Content.Dusts;

namespace DDmod
{
	public class GlobalDust : ModSystem
	{
		public static GlobalDust dust;

		public static bool[] DustPreTile = new bool[6001];

		public static int[] DustPlayerOwner = new int[6001];
		public static int[] DustNPCOwner = new int[6001];
		public static int[] DustProjectileOwner = new int[6001];

		public static float[] DustAI = new float[6001];
		public static Vector2[] DustVector = new Vector2[6001];

		public static bool BossSurvival;
		public override void Load()
		{
		}
		public override void Unload()
		{
		}

		public override void PreUpdateDusts()
		{
			for (int i = 0; i < Main.maxDustToDraw; i++)
			{
				Dust dust = Main.dust[i];
				if (dust.active)
                {
					if (GlobalDust.DustPlayerOwner[dust.dustIndex] > -1 && Main.player[GlobalDust.DustPlayerOwner[dust.dustIndex]].active)
					{
						Player player = Main.player[GlobalDust.DustPlayerOwner[dust.dustIndex]];
						dust.position += player.Dplayer().PrePosition;
					}
					if (GlobalDust.DustNPCOwner[dust.dustIndex] > -1 && Main.npc[GlobalDust.DustNPCOwner[dust.dustIndex]].active)
					{
						NPC npc = Main.npc[GlobalDust.DustNPCOwner[dust.dustIndex]];
						dust.position += npc.position - npc.oldPosition;
					}
					if (GlobalDust.DustProjectileOwner[dust.dustIndex] > -1)
					{
						if (Main.projectile[GlobalDust.DustProjectileOwner[dust.dustIndex]].active)
						{
							Projectile projectile = Main.projectile[GlobalDust.DustProjectileOwner[dust.dustIndex]];
							//dust.position += projectile.position - projectile.oldPosition;
							dust.position += projectile.DProj().PrePosition;
						}
						if(!Main.projectile[GlobalDust.DustProjectileOwner[dust.dustIndex]].active)
						{
							GlobalDust.DustProjectileOwner[dust.dustIndex] = -1;
                        }
					}
					if (dust.type == 172)
					{
						Lighting.AddLight(dust.position, new Color(0, 55, 255).ToVector3());
                    }
                    if (dust.type == 18 && !dust.noGravity)
                    {
                        if (dust.velocity.Y < 10)
                        {
                            dust.velocity.Y += 0.2F;
                        }
                        Point tiles = new Point((int)dust.position.X / 16, (int)dust.position.Y / 16);

                        if (tiles.X > 0 && tiles.Y > 0 && tiles.Y < Main.maxTilesY && tiles.X < Main.maxTilesX)
                        {
                            if (Main.tile[tiles.X, tiles.Y].WallType == 0 || WorldGen.DefaultTreeWallTest(Main.tile[tiles.X, tiles.Y].WallType))
                            {
                                if (dust.velocity.X < Main.windSpeedCurrent)
                                {
                                    dust.velocity.X += 0.03F;
                                }
                                if (dust.velocity.X > Main.windSpeedCurrent)
                                {
                                    dust.velocity.X -= 0.03F;
                                }
                            }
                            else
                            {
                                dust.velocity.X *= 0.98f;
                            }
                            dust.velocity = Collision.TileCollision(dust.position, dust.velocity, (int)(4 * dust.scale), (int)(4 * dust.scale));
                            if (Collision.SolidCollision(dust.position, (int)(4 * dust.scale), (int)(4 * dust.scale)))
                            {
                                dust.position.Y -= 3;
                            }
                        }
                        else
                        {
                            dust.active = false;
                        }
                    }
                    //火焰粒子和咒火粒子
                    if ((dust.type == 6 || dust.type == 75 || dust.type == 27 || dust.type == 92 || dust.type == 135) && !dust.noGravity)
					{
						dust.velocity.Y -= 0.05f;
						/*Tile tile = Main.tile[(int)dust.position.X/16, (int)dust.position.Y/16];
						if (tile.TileType == 147||tile.TileType == 163||tile.TileType == 164||tile.TileType == 161||tile.TileType == 200)
						{
							tile.LiquidType = 0;
							tile.LiquidAmount = 255;
							tile.TileType = 0;
							tile.HasTile = false;
							WorldGen.SquareTileFrame((int)dust.position.X / 16, (int)dust.position.Y / 16, true);
						}
						if (tile.WallType == 40 || tile.WallType == 71)
						{
							tile.LiquidType = 0;
							tile.LiquidAmount = 255;
							tile.WallType = 0;
							WorldGen.SquareWallFrame((int)dust.position.X / 16, (int)dust.position.Y / 16, true);
						}
						if(tile.LiquidAmount>=150)
						{
							dust.active = false;
						}
						if (tile.TileType == 116||tile.TileType == 112||tile.TileType == 53||tile.TileType == 234||(tile.TileType>=396&&tile.TileType<=404))
						{
							tile.TileType = 54;
							WorldGen.SquareTileFrame((int)dust.position.X / 16, (int)dust.position.Y / 16, true);
						}
						if (tile.WallType == 187 || tile.WallType == 200|| tile.WallType == 201|| tile.WallType == 202)
						{
							tile.WallType = 21;
							WorldGen.SquareWallFrame((int)dust.position.X / 16, (int)dust.position.Y / 16, true);
						}*/
					}
				}
			}
		}
	}
}