namespace DDmod.Content.Dusts
{
    public class 速度粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, 0, 72, 72);
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            if(dust.noGravity)
            dust.velocity *= 0.92F;
            if (dust.customData == null)
            {
                dust.customData = 1f;
            }
            if (dust.customData is int)
            {
                float A = (int)dust.customData;
                dust.customData = new object();
                dust.customData = A;
            }
            if (dust.customData is float)
            {
                dust.scale -= 0.1F* Math.Abs((float)dust.customData);
            }
            if (dust.scale < 0.1f)
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
            Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetAlpha(dust.color), dust.rotation + MathHelper.PiOver2, modDust.Texture2D.Size() / 2, dust.scale / 2 * new Vector2(0.25F, 1), 0, 0f);
            return false;
        }
    }
}
