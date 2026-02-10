using DDmod.Content.Items.Melee.Sword;
using Terraria;

namespace DDmod.Content.Dusts
{
    public class 血水粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 10*Main.rand.Next(3), 10, 10);
        }
        public override bool Update(Dust dust)
        {
            if (dust.firstFrame && !ChildSafety.Disabled)
            {
                if (Main.rand.NextBool(2))
                {
                    dust.firstFrame = false;
                    dust.type = 16;
                    dust.scale = Main.rand.NextFloat() * 1.6f + 0.3f;
                    dust.color = Color.Transparent;
                    dust.frame.X = 10 * dust.type;
                    dust.frame.Y = 10 * Main.rand.Next(3);
                    dust.shader = null;
                    dust.customData = null;
                    int num2 = dust.type / 100;
                    dust.frame.X -= 1000 * num2;
                    dust.frame.Y += 30 * num2;
                    dust.noGravity = true;
                }
                else
                {
                    dust.active = false;
                }
            }
            if (dust.noGravity)
            {
                dust.scale -= dust.scale / 50;
            }
            dust.scale -= 0.03f;
            if (dust.scale <= 0.2F)
            {
                dust.active = false;
                return false;
            }
            if (!dust.noGravity)
            {
                /*
                if (GlobalDust.DustPlayerOwner[dust.dustIndex] > -1)
                {
                    Player player = Main.player[GlobalDust.DustPlayerOwner[dust.dustIndex]];
                    dust.velocity.Y *= 0.2f;
                    dust.velocity.X *= 0.92f;
                    dust.scale += 0.008f;
                    if (!new Rectangle((int)dust.position.X, (int)dust.position.Y, (int)(4 * dust.scale), (int)(4 * dust.scale)).Intersects(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height-4)))
                    {
                        GlobalDust.DustPlayerOwner[dust.dustIndex] = -1;
                    }
                }
                else
                {
                    for (int a = 0; a < 255; a++)
                    {
                        Player player = Main.player[a];
                        if (player == null || !player.active || player.dead)
                        {
                            continue;
                        }
                        if (new Rectangle((int)dust.position.X, (int)dust.position.Y, (int)(4 * dust.scale), (int)(4 * dust.scale)).Intersects(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height - 4)))
                        {
                            GlobalDust.DustPlayerOwner[dust.dustIndex] = a;
                            break;
                        }
                    }
                }*/
                if (dust.velocity.Y < 10)
                {
                    dust.velocity.Y += 0.2F;
                }
                Point tiles = new Point((int)dust.position.X / 16, (int)dust.position.Y / 16);
                
                if (tiles.X > 0 && tiles.Y > 0 && tiles.Y < Main.maxTilesY  && tiles.X < Main.maxTilesX )
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
            else
            {
                dust.velocity *= 0.92F;
            }
            dust.position += dust.velocity;
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            
            return new Color(lightColor.R - dust.alpha, lightColor.G - dust.alpha, lightColor.B - dust.alpha, lightColor.A - dust.alpha);
        }
    }
}
