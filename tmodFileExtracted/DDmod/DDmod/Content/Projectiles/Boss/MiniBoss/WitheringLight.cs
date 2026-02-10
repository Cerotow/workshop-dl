using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class WitheringLight : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.scale = 1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(255, 20, 20) * 0.003F);
            Projectile.ProjScaleChange();
            int PlayerID = Player.FindClosest(Projectile.Center, 1, 1);
            Player player = Main.player[PlayerID];
            Vector2 vector = player.Center - Projectile.Center;
            if (Projectile.ai[0] == 0)
            {
                Projectile.scale = 0.5F;
                if (Math.Abs(player.Center.Y) - Math.Abs(Projectile.Center.Y) < 10)
                {
                    Projectile.ai[0]--;
                    vector.DirectPerfectNormalize();
                    Projectile.velocity.Y = 0;
                    Projectile.velocity.X = Projectile.velocity.PerfectNormalize().X * 10;
                }
            }
            if (Projectile.ai[0] == 1)
            {
                Projectile.scale = 0.5F;
                Projectile.velocity.Y += 0.2f;
            }
            if (Projectile.ai[0] == 2)
            {
                Projectile.scale = 1.3F;
                Projectile.velocity.Y += 0.2f;
            }
            if (Projectile.ai[0] == 3)
            {
                Projectile.scale = 0.4F;
                if (Projectile.velocity.Length() < 5)
                {
                    if (Projectile.ai[1] > 10)
                    {
                        Projectile.velocity = (Projectile.velocity * 100 + vector.PerfectNormalize() * 15) / 101;
                    }
                    else
                    {
                        Projectile.ai[1]++;
                    }
                }
            }
            if (Projectile.ai[0] == 4)
            {
                if(Projectile.scale==1)
                {
                    Projectile.scale = Main.rand.NextFloat(0.1f, 0.3f);
                }
                if(Projectile.timeLeft>60)
                {
                    Projectile.timeLeft = 60;
                }
                Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            float Length = Projectile.velocity.Length() / (Projectile.height / 4);
            for (float l = 0; l < Length; l++)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                    Color color2 = Projectile.GetAlpha(new Color(255, 0, 0, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                    color2 = Projectile.GetAlpha(new Color(0, 150, 150, 0)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity / Length * l, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 2f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                }
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                for (int A = 0; A < 0.25F * (Projectile.oldPos.Length - i); A++)
                {
                    int Type = ModContent.DustType<枯萎粒子>();
                    Dust dust = Main.dust[NewDust(Projectile.oldPos[i] + Projectile.Size / 2, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 0.07f * (Projectile.oldPos.Length - i);
                    dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
                }
            }
            if (Projectile.ai[0] == 2)
            {
                for (int a = 0; a < 12; a++)
                {
                    Projectile projectile = Main.projectile[NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -2).RotatedBy(MathHelper.TwoPi / 12 * a), Type, Projectile.damage / 3, 1, Main.myPlayer, 3)];
                }
            }
            if (Projectile.ai[0] == 4)
            {
                SoundStyle sound = SoundID.NPCDeath39;
                sound.Pitch = -1f;
                PlaySound(sound, Projectile.Center);
            }
        }
    }
}