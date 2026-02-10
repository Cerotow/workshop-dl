namespace DDmod.Content.Dusts
{
    public class 星光粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.rotation = 0;
            dust.frame = new Rectangle(0, 0, 72, 72);
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.velocity *= 0.92F;
            if (dust.velocity.Length() < 0.1F)
            {
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
                    if ((float)dust.customData < 1000)
                    {
                        dust.scale -= 0.1F * Math.Abs((float)dust.customData);
                    }
                    else
                    {
                        dust.scale -= 0.1F * (int)Math.Abs((float)dust.customData/1000);
                    }
                }
                if (dust.scale < 0.1f)
                {
                    dust.active = false;
                }
            }
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.color;
        }
        public override bool PreDraw(Dust dust)
        {
            if (dust.customData is int)
            {
                float A = (int)dust.customData;
                dust.customData = new object();
                dust.customData = A;
            }
            float FP = 1;
            if (dust.customData is float)
            {
                if((float)dust.customData>1000)
                FP = (float)dust.customData%1000;
            }
            ModDust modDust = DustLoader.GetDust(dust.type);
            Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetAlpha(dust.color), dust.rotation, modDust.Texture2D.Size() / 2, dust.scale / 2 * new Vector2(0.25F* FP, 1) * 0.75f, 0, 0f);
            Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetAlpha(dust.color), dust.rotation + MathHelper.PiOver2, modDust.Texture2D.Size() / 2, dust.scale / 2 * new Vector2(0.25F * FP, 1)*0.75f, 0, 0f);
            return false;
        }
    }
}
