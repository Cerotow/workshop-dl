namespace DDmod.Content.Dusts
{
    public class 冰雾 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity = Vector2.Zero;
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }

        public override bool Update(Dust dust)
        {
            if (dust.alpha>255)
            {
                dust.active = false;
            }
            if(dust.customData==null)
            {
                dust.color *= Main.rand.NextFloat(0.2F,1);
                dust.customData = 1;
            }
            if (dust.velocity.X > 0)
            {
                //dust.rotation += dust.velocity.Length() * 0.03F;
            }
            else
            {
                //dust.rotation -= dust.velocity.Length() * 0.03F;
            }
            if(dust.scale<0.6F)
            {
                dust.scale += 0.6F;
            }
            dust.alpha+=10;
            dust.velocity.Y *= 0.96f;
            if(dust.velocity.Length()>4)
            {
                dust.velocity = dust.velocity*0.92F;
            }
            Point tiles = new Point((int)dust.position.X / 16, (int)dust.position.Y / 16);

            if (tiles.X > 0 && tiles.Y > 0 && tiles.Y < Main.maxTilesY && tiles.X < Main.maxTilesX)
            {
                if (Main.tile[tiles.X, tiles.Y].WallType == 0 || WorldGen.DefaultTreeWallTest(Main.tile[tiles.X, tiles.Y].WallType))
                {
                    if (dust.velocity.X < Main.windSpeedCurrent / 2)
                    {
                        dust.velocity.X += 0.03F;
                    }
                    if (dust.velocity.X > Main.windSpeedCurrent / 2)
                    {
                        dust.velocity.X -= 0.03F;
                    }
                }
                else
                {
                    dust.velocity.X *= 0.98f;
                }
            }
            dust.position += dust.velocity;
            return false;
        }
    }
}
