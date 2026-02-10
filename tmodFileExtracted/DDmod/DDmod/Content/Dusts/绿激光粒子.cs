namespace DDmod.Content.Dusts
{
    public class 绿激光粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
        }

        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position, 0, 2.55f * 0.2f, 0);
            return true;
        }
    }
}
