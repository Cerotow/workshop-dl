namespace DDmod.Content.Dusts
{
    public class 拉长粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.rotation = 0;
            dust.frame = new Rectangle(0, 0, 2, 2);
            dust.alpha = 255;
            dust.noLightEmittence = false;
        }
        public override bool Update(Dust dust)
        {
            dust.rotation = dust.velocity.ToRotation();
            dust.position += dust.velocity;
            dust.velocity *= 0.92F;
            if (dust.velocity.Length() < 0.1F)
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
            Color color = dust.color;
            if(dust.noLightEmittence)
            {
                color = Lighting.GetColor(new Point((int)dust.position.X / 16, (int)dust.position.Y / 16), dust.color);
            }
            Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, color, dust.rotation, 
                new Vector2(0, modDust.Texture2D.Height() / 2), new Vector2(Math.Abs(dust.velocity.Length() * 2), dust.scale), 0, 0f);
            return false;
        }
    }
}
