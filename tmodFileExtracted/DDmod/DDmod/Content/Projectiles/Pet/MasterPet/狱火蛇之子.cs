using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Pet.MasterPet
{
    public class 狱火蛇之子 : ModProjectile
    {
        public override void SetStaticDefaults()
        {

            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet);
            Projectile.aiStyle = -1;
            Projectile.scale = 1F;
        }
        int[] Body = new int[7];
        Vector2[] Center = new Vector2[7];
        float[] Rotation = new float[7];

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        public override void PostDraw(Color lightColor)
        {
            int Length = 20;
            if (Body.Length != Length)
            {
                Body = new int[Length];
                Center = new Vector2[Length];
                Rotation = new float[Length];
                for (int B = 0; B < Length; B++)
                {
                    Center[B] = Projectile.Center - new Vector2(0.1f);
                }
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2 = ModContent.Request<Texture2D>(GlowTexture).Value;
            Center[0] = Projectile.Center;
            Rotation[0] = Projectile.rotation;
            for (int B = 0; B < Body.Length; B++)
            {
                if (B > 0)
                {
                    Vector2 vector = Center[B - 1] - Center[B];
                    Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;
                    
                    float Distance = (vector.Length() - (12 * Projectile.scale)) / vector.Length();
                    if (B % 2 == 1)
                    {
                        Distance = (vector.Length() - (10 * Projectile.scale)) / vector.Length();
                    }
                        if (B==1)
                    {
                        Distance = (vector.Length() - (18 * Projectile.scale)) / vector.Length();
                    }
                    Center[B] = Center[B] + vector * Distance;
                }
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 4)), Projectile.GetAlpha(lightColor), Rotation[0], new Vector2(texture.Width / 2, texture.Height / 8), Projectile.scale*1.2F, 0, 0);
                    Main.EntitySpriteDraw(texture2, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height / 4)), Projectile.GetAlpha(Color.White), Rotation[0], new Vector2(texture2.Width / 2, texture2.Height / 8), Projectile.scale*1.2F, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    if (B % 2 == 1)
                    {
                        Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 4, texture.Width, texture.Height / 4)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 8), Projectile.scale, 0, 0);
                        Main.EntitySpriteDraw(texture2, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture2.Height / 4, texture2.Width, texture2.Height / 4)), Projectile.GetAlpha(Color.White), Rotation[B], new Vector2(texture2.Width / 2, texture2.Height / 8), Projectile.scale, 0, 0);
                    }
                    else
                    {
                        Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 4 * 2, texture.Width, texture.Height / 4)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 8), Projectile.scale, 0, 0);
                        Main.EntitySpriteDraw(texture2, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture2.Height / 4 * 2, texture2.Width, texture2.Height / 4)), Projectile.GetAlpha(Color.White), Rotation[B], new Vector2(texture2.Width / 2, texture2.Height / 8), Projectile.scale, 0, 0);
                    }
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 4 * 3, texture.Width, texture.Height / 4)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 8), Projectile.scale, 0, 0);
                    Main.EntitySpriteDraw(texture2, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture2.Height / 4 * 3, texture2.Width, texture2.Height / 4)), Projectile.GetAlpha(Color.White), Rotation[B], new Vector2(texture2.Width / 2, texture2.Height / 8), Projectile.scale, 0, 0);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            //Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0f);
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            CheckActive(player);

            Movement(player);
        }

        private void CheckActive(Player player)
        {
            if (!player.dead && player.HasBuff(ModContent.BuffType<狱火蛇之子Buff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        private void Movement(Player player)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Vector2 direction = player.Center - Projectile.Center;
            float DistanceNPC = (player.Center - Projectile.Center).Length();
            if (direction.Length() > 400)
            {
                Projectile.ai[1] = 1;
            }
            if (Projectile.ai[1] == 1)
            {
                direction.Y -= 200;
                float speed = 1 + player.velocity.Length();
                Projectile.netUpdate = true;
                if (DistanceNPC > 3000f)
                {
                    Projectile.Center = player.Center;
                }
                if (direction.Length() < 50)
                {
                    Projectile.ai[1] = 0;
                }
                direction = direction.PerfectNormalize();
                direction *= speed;
                Projectile.velocity = (Projectile.velocity * 20 + direction) / (21);
                Projectile.ai[0] = 0;
            }
            else
            {
                Projectile.ai[0]--;
                if (Projectile.ai[0] <= 0)
                {
                    float speed = 5f;
                    Projectile.netUpdate = true;
                    direction.Y -= 200f;
                    if (DistanceNPC > 3000f)
                    {
                        Projectile.Center = player.Center;
                    }
                    if (direction.Length() > 200f)
                    {
                        Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize() * -speed, -1, default);
                        if (Main.rand.NextBool(2)) Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize() * -speed, 1, default);
                        Projectile.velocity = Pvelocity;
                        Projectile.ai[0] = 60;
                    }
                }
                if (Projectile.velocity.Length() < 2)
                {
                    Projectile.velocity = Projectile.rotation.ToRotationVector2() * 4;
                }
            }
        }
    }
}
