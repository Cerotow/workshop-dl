namespace DDmod.Content.Dusts
{
    public class 花岗岩电光粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
            dust.customData = new Vector2[20];
        }
        public override bool Update(Dust dust)
        {
            if (!dust.noGravity)
            {
                if (dust.velocity.Y < 10)
                {
                    dust.velocity.Y += 0.2F;
                }
            }
            int V = (int)dust.velocity.Length()/2;

            for (int A = 0; A < V; A++)
            {
                dust.position += dust.velocity.PerfectNormalize() * 2;
                Vector2[] P = (Vector2[])dust.customData;
                P[0] = dust.position;
                for (int i = P.Length - 1; i > 0; i--)
                {
                    P[i] = P[i - 1];

                }
                dust.customData = P;
            }
            dust.scale -= 0.03f;
            if (dust.scale <= 0.01F)
                dust.active = false;

            return false;
        }

        public override bool PreDraw(Dust dust)
        {
            Vector2[] P = (Vector2[])dust.customData;

            Main.spriteBatch.Draw(DDTextures.VoidStar.Value, dust.position - Main.screenPosition, null, new Color(100, 255, 100, 0), 0, DDTextures.VoidStar.Size() / 2, dust.scale / 8, 0, 0);
            for (int i = 0; i < P.Length; i++)
            {
                Main.spriteBatch.Draw(DDTextures.VoidStar.Value, P[i] - Main.screenPosition, null, new Color(100, 200, 255, 0), 0, DDTextures.VoidStar.Size() / 2, dust.scale / 8 * ((P.Length - i) / (float)P.Length), 0, 0);

            }
            return false;
        }
    }
}
