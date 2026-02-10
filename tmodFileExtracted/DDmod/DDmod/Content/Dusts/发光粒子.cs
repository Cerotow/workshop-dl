namespace DDmod.Content.Dusts
{
    public class 发光粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.rotation = 0;
            dust.frame = new Rectangle(0, 0, 60, 60*Main.rand.Next(3));
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.velocity *= 0.92F;
            if (dust.customData == null)
            {
                dust.customData = 1f;
            }
            if (dust.customData is float)
            {
                int A = (int)dust.customData;
                dust.customData = new object();
                dust.customData = A;
            }
            if (dust.customData is int)
            {
                dust.alpha += 1 + Math.Abs((int)dust.customData);
            }
            if (dust.alpha >= 255)
            {
                dust.active = false;
            }
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.color;
        }
        public override bool PreDraw(Dust dust)
        {
            ModDust modDust = DustLoader.GetDust(dust.type);
            Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetAlpha(dust.color), dust.rotation, dust.frame.Size() / 2, dust.scale/2, 0, 0f);
            return false;
        }
    }
}
