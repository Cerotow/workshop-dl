using DDmod.Helper;

namespace DDmod.Content.Dusts
{
    public class 光球粒子 : ModDust
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
                if(vector.Length()<1&& vector.Length()!=0)
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
                dust.customData = A;
            }
            if (dust.customData is float)
            {
                if((float)dust.customData % 1000 == 0)
                {
                    dust.customData= (float)dust.customData +1;
                }
                if (Math.Abs((float)dust.customData) < dust.DustAI(1))
                {
                    dust.scale -= 0.03F * (Math.Abs((float)dust.customData) % 1000);
                    if (dust.scale < 0.01F)
                    {
                        dust.active = false;
                    }
                }
                else if (Math.Abs((float)dust.customData) < dust.DustAI(2))
                {
                    dust.scale -= 0.03F * (Math.Abs((float)dust.customData) % 1000);
                    dust.velocity *= 0.98F;
                    if (dust.scale < 0.01F)
                    {
                        dust.active = false;
                    }
                }
                else if (Math.Abs((float)dust.customData) < dust.DustAI(3))
                {
                    dust.scale += 0.03F * (Math.Abs((float)dust.customData) % 1000);
                    dust.alpha += (int)(5 * (Math.Abs((float)dust.customData) % 1000));

                    if (dust.alpha > 255)
                    {
                        dust.active = false;
                    }
                }
                else if (Math.Abs((float)dust.customData) < dust.DustAI(4))
                {
                    if (dust.alpha == 0)
                    {
                        dust.alpha = 255;
                    }
                    dust.alpha -= (int)(5 * (Math.Abs((float)dust.customData) % 1000));
                    dust.scale -= 0.03F * ((Math.Abs((float)dust.customData) % 1000));
                    if (dust.scale < 0.01F)
                    {
                        dust.active = false;
                    }
                }
                else if (Math.Abs((float)dust.customData) < dust.DustAI(5))
                {
                    dust.scale -= 0.03F * (Math.Abs((float)dust.customData) % 1000);
                    dust.velocity *= 1.02F * (Math.Abs((float)dust.customData) % 1000);
                    if (dust.scale < 0.01F)
                    {
                        dust.active = false;
                    }
                }
                else if (Math.Abs((float)dust.customData) < dust.DustAI(6))
                {
                    dust.scale += 0.03F * (Math.Abs((float)dust.customData) % 1000);
                    dust.velocity *= 0.98F * (Math.Abs((float)dust.customData) % 1000);
                    dust.alpha += (int)(5 + (Math.Abs((float)dust.customData) % 1000));

                    if (dust.alpha > 255)
                    {
                        dust.active = false;
                    }
                }
                else if (Math.Abs((float)dust.customData) < dust.DustAI(7))
                {
                    dust.scale -= 0.03F * (Math.Abs((float)dust.customData) % 1000);
                    dust.velocity = dust.velocity.RotatedBy(Main.rand.NextFloat(-0.1F, 0.1F));
                    if (dust.scale < 0.01F)
                    {
                        dust.active = false;
                    }
                }
                else
                if (Math.Abs((float)dust.customData) < dust.DustAI(8))
                {
                    if (dust.velocity.Y > -5)
                    {
                        dust.velocity.Y -= 0.05F;
                    }
                    dust.scale -= 0.03F * (Math.Abs((float)dust.customData) % 1000);
                    if (dust.scale < 0.01F)
                    {
                        dust.active = false;
                    }
                }
                else if (Math.Abs((float)dust.customData) < dust.DustAI(9))
                {
                    dust.scale -= 0.01F * (Math.Abs((float)dust.customData) % 1000);
                    dust.velocity *= 0.96F;
                    if (dust.color.A < 255)
                    {
                        dust.color.A += (byte)(3 * (float)dust.customData);
                    }
                    if (dust.scale < 0.01F)
                    {
                        dust.active = false;
                    }
                }
                else
                if (Math.Abs((float)dust.customData) < dust.DustAI(10))
                {
                    if (dust.velocity.Y < 5)
                    {
                        dust.velocity.Y += 0.05F;
                    }
                    dust.velocity.X *= 0.96F;
                    dust.scale -= 0.03F * (Math.Abs((float)dust.customData) % 1000);
                    if (dust.scale < 0.01F)
                    {
                        dust.active = false;
                    }
                }
                else
                {
                    if(dust.alpha>255)
                    {
                        dust.alpha = 255;
                    }
                    if(dust.alpha<0)
                    {
                        dust.alpha = 0;
                    }
                    if (dust.velocity != Vector2.Zero)
                    {
                        if (dust.alpha == 0)
                        {
                            dust.alpha = 255;
                        }
                        
                        dust.alpha -= (int)(5 * (Math.Abs((float)dust.customData) % 1000));
                        dust.scale -= 0.03F * ((Math.Abs((float)dust.customData) % 1000));
                        if (dust.scale < 0.01F)
                        {
                            dust.velocity = Vector2.Zero;
                        }
                    }
                    else
                    {
                        dust.alpha += (int)(1 * (Math.Abs((float)dust.customData) % 1000));
                        dust.scale += 0.02F * ((Math.Abs((float)dust.customData) % 1000));
                        if (dust.alpha >= 255)
                        {
                            dust.active = false;
                        }
                    }
                }
            }
            dust.position += dust.velocity;
            return false;
        }
    }
}
