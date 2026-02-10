namespace DDmod.Content.Dusts
{
    public class 绿岩粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 10*Main.rand.Next(3), 10, 10);
            dust.noGravity = false;
        }
        public override bool Update(Dust dust)
        {
            if (!dust.noGravity)
            {
                if (dust.velocity.Y < 10)
                {
                    dust.velocity.Y += 0.2F;
                }
                dust.velocity.X *= 0.92F;
                dust.velocity = Collision.TileCollision(dust.position, dust.velocity, (int)(4 * dust.scale), (int)(4 * dust.scale));
            }
            else
            {
                dust.velocity *= 0.92F;
            }
            dust.position += dust.velocity;
            dust.scale -= 0.01f;
            if (dust.scale <= 0.5F)
                dust.active = false;
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            
            return new Color(lightColor.R - dust.alpha, lightColor.G - dust.alpha, lightColor.B - dust.alpha, lightColor.A - dust.alpha);
        }
    }
}
