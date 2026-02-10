namespace DDmod.Content.Dusts
{
    public class 枯萎粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale *= 1;
            dust.noGravity = true;
        }

        public override bool Update(Dust dust)
        {
            dust.frame = new Rectangle(0, 10 * Main.rand.Next(3), 10, 10);
            Lighting.AddLight(dust.position, 2.55f * 0.2f,0, 0);
            return true;
        }
    }
}
