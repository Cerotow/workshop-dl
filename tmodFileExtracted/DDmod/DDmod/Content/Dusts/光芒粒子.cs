using DDmod.Content.Items.Melee.Sword;

namespace DDmod.Content.Dusts
{
    public class 光芒粒子 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity = Vector2.Zero;
            dust.customData = null;
            dust.noGravity = true;
        }

        public override bool Update(Dust dust)
        {
            if (!dust.noGravity)
            {
                if (dust.velocity.Y < 5)
                {
                    dust.velocity.Y += 0.1F;
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
                dust.customData = 1f;
            }
            if (dust.customData is int)
            {
                float A = (int)dust.customData;
                dust.customData = new object();
                dust.customData = new Vector3(A, 1, 1);
            }
            if (dust.customData is float)
            {
                float A = (float)dust.customData;
                dust.customData = new object();
                dust.customData = new Vector3(A,1,1) ;
            }
            if (dust.customData is not Vector3)
            {
                dust.customData = new object();
                dust.customData = new Vector3(1,1,1);
            }
            else
            {
                Vector3 vector = (Vector3)dust.customData;
                dust.scale += 0.03F * (Math.Abs((float)vector.X) % 1000);
                dust.alpha += (int)(3F * (Math.Abs((float)vector.Y) % 1000));
                if (dust.alpha>255)
                {
                    dust.active = false;
                }
            }
            dust.position += dust.velocity;
            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            Vector2 vector = new Vector2(DDTextures.GlowEffect.Width(), DDTextures.GlowEffect.Height()) / 2;
            Vector2 vector2 = new Vector2(DDTextures.VoidStar.Width(), DDTextures.VoidStar.Height()) / 2;
            float A = dust.alpha;
            if (A < 0)
            {
                A = 0;
            }
            Color color = dust.color * (1 - A / 255F);
            Main.spriteBatch.Draw(DDTextures.VoidStar.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.VoidStar.Width(), DDTextures.VoidStar.Height())), color, dust.rotation, vector2, dust.scale / 2, 0, 0f);
            Main.spriteBatch.Draw(DDTextures.GlowEffect.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.GlowEffect.Width(), DDTextures.GlowEffect.Height())), color, dust.rotation, vector, dust.scale / 4, 0, 0f);
            color = dust.color.Opposite();
            color.A = 0;
            if (dust.customData is float && (float)dust.customData < 0)
            {
                Main.spriteBatch.Draw(DDTextures.GlowEffect.Value, dust.position - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, DDTextures.GlowEffect.Width(), DDTextures.GlowEffect.Height())), color, dust.rotation, vector, dust.scale / 8, 0, 0f);
            }
            return false;
        }
    }
}
