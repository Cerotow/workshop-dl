using DDmod.Content.Dusts;
using Terraria;
namespace DDmod.Content.Projectiles.Boss
{
    public class Boss陨石弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            //ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            //ProjectileID.Sets.TrailCacheLength[Projectile.type] = 80;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 1500;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 8;
        }
        public override void AI()
        {
            if ((Projectile.localAI[1] >= 1 && Projectile.localAI[0] == 0)|| Projectile.ai[0]>0)
            {
                Projectile.localAI[0]++;
            }
            int Type = ModContent.DustType<速度粒子>();

            if (Projectile.ai[0] > 0)
            {
                Projectile.extraUpdates = 0;
                if (Projectile.localAI[0] == 1)
                {
                    Color color = new Color(252, 128, 48, 50);
                    if (Projectile.ai[0] > 0)
                    {
                        Projectile.scale = Projectile.ai[0]; color = new Color(50, 255, 57, 50);
                    }
                    SoundStyle sound = new SoundStyle(DDHelper.Sound(1, "大炮"));
                    sound.Volume = Projectile.scale;
                    sound.MaxInstances = 10;
                    PlaySound(sound, Projectile.Center);
                    NewDustChange4((int)(Projectile.scale * 100), Projectile.position, Projectile.Size, Type, 10 * Projectile.scale, 20 * Projectile.scale, true, 4F * (Projectile.scale), 8F * (Projectile.scale), 100, 1000, color, Projectile.scale * 5);
                    Projectile.scale *= 6f;
                    Projectile.timeLeft = 3;
                    Projectile.localAI[0]++;
                }
            }
            else
            {
                Projectile.scale = 0.5F;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0)
            {
                for (float A = 0; A < Projectile.scale; A += 0.5f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(252, 128, 48, 50))];
                    dust.noGravity = true;
                    dust.scale *= 2f * Projectile.scale;
                    dust.velocity *= 0.1f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                    dust.rotation = Projectile.velocity.ToRotation();
                }
            }
            Projectile.ProjScaleChange();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreKill(int timeLeft)
        {
            return true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if(Projectile.localAI[1]==0)
            {
                Vector2 vector = Projectile.velocity;
                for (float A = 0; A < 40; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center- new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(252, 128, 48, 50))];
                    dust.noGravity = true;
                    dust.scale *= Main.rand.NextFloat(1F, 4);
                    dust.velocity = vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * Main.rand.NextFloat(12, 44);
                    Vector2 v = vector.PerfectNormalize();
                    dust.rotation = dust.velocity.ToRotation();
                }
            }
         //   Projectile.localAI[1]++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.DProj().track <= 1)
            {
                return false;
            }
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null,Color.White*0.3f, 0, Vector2.Zero, Projectile.Size / 2, 0, 0f);
            if (Projectile.localAI[0] == 0)
            {
                SpriteEffects spriteEffects = (SpriteEffects)1;
                if (Projectile.direction == 1)
                {
                    spriteEffects = 0;
                }
                Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
                Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/4, spriteEffects, 0f);

                for (int i = 0; i < 80; i++)
                {
                    Vector2 vector2 = Projectile.Center-Projectile.velocity.PerfectNormalize()*i*10 - Main.screenPosition;
                    Color color =new Color(255,255,255,0)*0.6f * ((80 - i) / (float)80);
                    //Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vector2, null, color * 0.6f, Projectile.rotation, ModContent.Request<Texture2D>("DDmod/Image/VoidStar").Size() / 2, Projectile.scale / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/4* ((80 - i) / (float)80), spriteEffects, 0f);
                }
                for (int i = 0; i < 2; i++)
                {
                    //Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition + new Vector2(2* Projectile.scale).RotatedBy(MathHelper.TwoPi / 3*i + Projectile.timeLeft / 10), null, new Color(255, 255, 255, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/4, spriteEffects, 0f);
                }
            }
            return false;
        }
    }
}