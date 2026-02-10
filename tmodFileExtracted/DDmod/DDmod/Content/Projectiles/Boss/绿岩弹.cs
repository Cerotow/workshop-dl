using DDmod.Content.Dusts;
using Terraria;
namespace DDmod.Content.Projectiles.Boss
{
    public class 绿岩弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 100;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (Projectile.localAI[1] >= 1 && Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
            }
            int Type = ModContent.DustType<速度粒子>();
            if (Projectile.timeLeft <= 3)
            {
                Projectile.localAI[0]++;
            }
            if (Projectile.localAI[0] == 1)
            {
                SoundStyle sound = new SoundStyle(DDHelper.Sound(1, "大炮"));
                sound.Volume = Projectile.scale;
                sound.MaxInstances = 10;
                PlaySound(sound, Projectile.Center);
                NewDustChange4((int)(Projectile.scale * 100), Projectile.position, Projectile.Size, Type, 10* Projectile.scale, 20 * Projectile.scale, true, 4F * (Projectile.scale), 8F * (Projectile.scale), 100,1000, new Color(119, 237, 130, 50),Projectile.scale*5);
                Projectile.scale *= 3f;
                Projectile.timeLeft = 3;
                Projectile.localAI[0]++;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0)
            {
                for (float A = 0; A < Projectile.scale; A += 0.5f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(119, 237, 130, 50))];
                    dust.noGravity = true;
                    dust.scale *= 2f * Projectile.scale;
                    dust.velocity *= 0.1f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                    dust.rotation = Projectile.velocity.ToRotation();
                }
                if (Projectile.timeLeft <3)
                {
                    Projectile.localAI[0]++;
                }
            }
            Projectile.ProjScaleChange();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.localAI[0] == 0)
                Projectile.localAI[0]++;
            Projectile.tileCollide = false;
            return Projectile.localAI[0] == 2;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreKill(int timeLeft)
        {
            if (Projectile.localAI[0] == 0)
                Projectile.localAI[0]++;
            return Projectile.localAI[0] == 2;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.localAI[1]++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
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

                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color =new Color(255,255,255,0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    //Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vector2, null, color * 0.6f, Projectile.rotation, ModContent.Request<Texture2D>("DDmod/Image/VoidStar").Size() / 2, Projectile.scale / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
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