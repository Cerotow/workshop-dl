namespace DDmod.Content.Dusts
{
    public class 爱心粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = 1f;
            dust.noGravity = true;
            dust.rotation = 0;
            dust.frame = new Rectangle(0, 10 * Main.rand.Next(3), 10, 10);
        }
        public override bool Update(Dust dust)
        {
            if (dust.customData ==null)
            {
                dust.customData = 1F;
            }
            if (dust.customData is int)
            {
                float A = (int)dust.customData;
                dust.customData = new object();
                dust.customData = A;
            }
            if ((float)dust.customData > 1000)
            {
                dust.alpha -= 1 + (int)(15 * ((float)dust.customData-1000));
                if (dust.alpha <= 0)
                {
                    dust.customData = (float)dust.customData - 1000;
                }
            }
            else
            {
                dust.alpha += 1 + (int)(15 * (float)dust.customData);
                if (dust.alpha > 255)
                {
                    dust.active = false;
                }
            }
            if(dust.noGravity)
                dust.velocity.Y -= 0.05f;
            dust.position += dust.velocity;
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color(255 - dust.alpha, 255 - dust.alpha, 255 - dust.alpha, 255 - dust.alpha);
        }
    }
}
