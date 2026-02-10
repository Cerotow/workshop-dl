namespace DDmod.Content.Dusts
{
    public class 星星粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.rotation = 0;
            dust.frame = new Rectangle(0, 0, 14, 14);
        }
        public override bool Update(Dust dust)
        {
            //dust.scale += 0.05f;
            dust.rotation += 0.3f;
            if (dust.customData is int)
            {
                float A = (int)dust.customData;
                dust.customData = new object();
                dust.customData = A;
            }
            if (dust.customData is float)
            {
                dust.alpha += (int)(5*(float)dust.customData+1);
            }
            else
            {
                dust.alpha += 5;
            }
            if (dust.alpha > 255)
            {
                dust.active = false;
            }
            if (!dust.noGravity)
            {
                if (dust.velocity.Y < 12)
                {
                    dust.velocity.Y += 0.1f;
                }
            }
            if (dust.customData == null && dust.noGravity)
            {
                dust.velocity.Y -= 0.01f;
            }
            dust.position += dust.velocity;
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color(255 - dust.alpha, 255 - dust.alpha, 255 - dust.alpha, 255 - dust.alpha);
        }
    }
}
