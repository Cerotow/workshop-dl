namespace DDmod.Content.Dusts
{
    public class 樱花粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.rotation = 0;
            dust.frame = new Rectangle(0, 20*Main.rand.Next(3), 20, 20);
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X *0.03F;
            dust.alpha += 3;
            if (dust.alpha > 255)
            {
                dust.active = false;
            }
            dust.velocity.X *= 0.98F;
            int A = 10;
            if (Main.LocalPlayer.ZoneSandstorm)
            {
                A += 100;
            }
            if (Main.windSpeedCurrent * A > 0)
            {
                if (dust.velocity.X < Main.windSpeedCurrent * A)
                {
                    dust.velocity.X += 0.05F;
                }
            }
            if (Main.windSpeedCurrent * A < 0)
            {
                if (dust.velocity.X > Main.windSpeedCurrent * A)
                {
                    dust.velocity.X -= 0.05F;
                }
            }
            if(dust.velocity.Y<1)
            {
                dust.velocity.Y += 0.02F;
            }
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color(255 - dust.alpha, 255 - dust.alpha, 255 - dust.alpha, 255 - dust.alpha);
        }
    }
}
