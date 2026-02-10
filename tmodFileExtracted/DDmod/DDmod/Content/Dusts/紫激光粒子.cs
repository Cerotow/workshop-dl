namespace DDmod.Content.Dusts
{
    public class 紫激光粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale *= 2;
        }

        public override bool Update(Dust dust)
        {
            dust.scale -= 0.03f;
            Lighting.AddLight(dust.position, 0.66f*0.2f, 0.49f * 0.2f, 2.55f*0.2f);
            return true;
        }
    }
}
