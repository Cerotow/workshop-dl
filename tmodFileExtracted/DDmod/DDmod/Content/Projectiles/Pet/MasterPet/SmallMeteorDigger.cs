using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Pet.MasterPet
{
    public class SmallMeteorDigger : ModProjectile
    {
        public static Asset<Texture2D> eyeTexture;
        public override void Load()
        {
            eyeTexture = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/MasterPet/SmallMeteorDigger_Glow");
        }
        public override void SetStaticDefaults()
        {

            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet);
            Projectile.aiStyle = -1;
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
            int Length = 10;
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
            Center[0] = Projectile.Center;
            Rotation[0] = Projectile.rotation;
            for (int B = 0; B < Body.Length; B++)
            {
                if (B > 0)
                {
                    Vector2 vector = Center[B - 1] - Center[B];
                    Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;

                    float Distance = (vector.Length() - (16 * Projectile.scale)) / vector.Length();
                    Center[B] = Center[B] + vector * Distance;
                }
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[0], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                    Main.EntitySpriteDraw(eyeTexture.Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, eyeTexture.Value.Width, eyeTexture.Value.Height / 3)), Projectile.GetAlpha(Color.White), Rotation[0], new Vector2(eyeTexture.Value.Width / 2, eyeTexture.Value.Height / 6), Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                    Main.EntitySpriteDraw(eyeTexture.Value, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, eyeTexture.Value.Height / 3, eyeTexture.Value.Width, eyeTexture.Value.Height / 3)), Projectile.GetAlpha(Color.White), Rotation[B], new Vector2(eyeTexture.Value.Width / 2, eyeTexture.Value.Height / 6), Projectile.scale, 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                    Main.EntitySpriteDraw(eyeTexture.Value, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, eyeTexture.Value.Height / 3 * 2, eyeTexture.Value.Width, eyeTexture.Value.Height / 3)), Projectile.GetAlpha(Color.White), Rotation[B], new Vector2(eyeTexture.Value.Width / 2, eyeTexture.Value.Height / 6), Projectile.scale, 0, 0);
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
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 600)
            {
                //钻头音效
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 20;
                    SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/NPC/钻头");
                    sound.Volume = 0.05f;
                    PlaySound(sound, Projectile.position);
                }
                //火焰
                for (int a = 0; a < 4; a++)
                {
                    Dust dust = Main.dust[NewDust((Projectile.position + Projectile.Size / 4) + (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * (15 + Projectile.velocity.Length()), 1, 1, 6, 0, 0, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 0.9F;
                    dust.velocity = Main.rand.NextVector2Unit(Projectile.rotation + MathHelper.PiOver2, Main.rand.NextFloat(-1.8F, 1.8F)) * Main.rand.NextFloat(2, 4);
                    dust.shader = GameShaders.Armor.GetSecondaryShader(player.miscDyes[0].dye, player);
                }
            }
            if (Projectile.localAI[0] > 700)
            {
                Projectile.localAI[0] = 0;
            }
        }

        private void CheckActive(Player player)
        {
            if (!player.dead && player.HasBuff(ModContent.BuffType<SmallMeteorDiggerBuff>()))
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
