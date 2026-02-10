namespace DDmod.Content.Dusts
{
    public class 生命粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale *= 1;
            dust.noGravity = true;
        }

        public override bool Update(Dust dust)
        {
            dust.frame = new Rectangle(0, 10 * Main.rand.Next(3), 10, 10);
            Lighting.AddLight(dust.position, 0, 2.55f * 0.2f, 0);
            if (GlobalDust.DustPlayerOwner[dust.dustIndex] != -1)
            {
                dust.position += dust.velocity;
                if (dust.velocity.Length() < 0.1F)
                {
                    dust.scale -= 0.2f;
                    if(dust.scale<=0.01F)
                    dust.active = false;
                }
                else
                {
                    if(dust.scale<1F)
                    {
                        dust.scale += 0.05F;
                    }
                    dust.velocity *= 0.95f;
                }
                return false;

            }
            return true;
        }
    }
}
