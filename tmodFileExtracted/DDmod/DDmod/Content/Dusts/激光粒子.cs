namespace DDmod.Content.Dusts
{
    public class 激光粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale *= 2;
            dust.customData = null;
            GlobalDust.DustPlayerOwner[dust.dustIndex] = -1;
        }

        public override bool Update(Dust dust)
        {
            if (!dust.noGravity)
            {
                if (dust.velocity.Y < 5)
                {
                    dust.velocity.Y += 0.01F;

                }
            }
            if (!dust.noLight)
            {
                Vector3 vector = new Vector3(dust.color.R, dust.color.G, dust.color.B) * 0.001f * dust.scale * (1f - (dust.alpha / 255));
                if (vector.Length() < 1 && vector.Length() != 0)
                {
                    vector.Normalize();
                }
                Lighting.AddLight(dust.position, vector);
            }
            if (dust.customData == null)
            {
                dust.customData = 1;
            }
            if (dust.customData is int)
            {
                float A = (int)dust.customData;
                dust.customData = new object();
                dust.customData = A;
            }
            if (dust.customData is float)
            {
                dust.scale -= 0.1F * Math.Abs((float)dust.customData);
            }
            if (dust.scale < 0.01F)
            {
                dust.active = false;
            }
            dust.position += dust.velocity;
            return false;
        }
    }
}
