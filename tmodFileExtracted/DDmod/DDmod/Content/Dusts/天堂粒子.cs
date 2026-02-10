namespace DDmod.Content.Dusts
{
    public class 天堂粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 0, 0);
            dust.noGravity = true;
        }

        public override bool Update(Dust dust)
        {
            Color color = new Color(255, 153, 183, 155);
            Lighting.AddLight(dust.position, color.ToVector3());
            dust.scale -= 0.01F;
            if(dust.scale<0)
            {
                dust.active = false;
            }
            return true;
        }
    }
}
