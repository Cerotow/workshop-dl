namespace DDmod.Content.Dusts
{
    public class 光子拖尾粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.rotation = 0;
            dust.frame = new Rectangle(0, 0, 2, 2);
        }
        public override bool Update(Dust dust)
        {
            dust.rotation = dust.velocity.ToRotation();
            dust.position += dust.velocity;
            dust.velocity *= 0.92F;
            if (dust.velocity.Length() < 0.1F)
            {
                dust.scale -= 0.1F;
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
            ModDust modDust = DustLoader.GetDust(dust.type);
            Main.spriteBatch.Draw(modDust.Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.GetAlpha(dust.color), dust.rotation, new Vector2(0, modDust.Texture2D.Height()/2), new Vector2(Math.Abs(dust.velocity.Length()*2),dust.scale), 0, 0f);
            Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition+dust.velocity * 4, null, dust.GetAlpha(dust.color), dust.rotation, DDTextures.MiniVoidStar.Size()/2, dust.scale/5, 0, 0f);
            Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, dust.position - Main.screenPosition + dust.velocity * 4, null, dust.GetAlpha(dust.color.Opposite()), dust.rotation, DDTextures.MiniVoidStar.Size()/2, dust.scale/8, 0, 0f);
            return false;
        }
    }
}
