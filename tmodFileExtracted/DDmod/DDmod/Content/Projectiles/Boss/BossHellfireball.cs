using Terraria;
namespace DDmod.Content.Projectiles.Boss
{
    public class BossHellfireball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 100;
            Projectile.penetrate = -1;
            Projectile.scale = 2;
            Projectile.width /= 2;
            Projectile.height /= 2;
        }
        public override void AI()
        {
            if (Projectile.localAI[1] >= 1 && Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
            }
            int Type = 6;
            if (Projectile.localAI[0] == 1)
            {
                NewDustChange((int)(Projectile.scale * 70), Projectile.position, Projectile.Size, Type, 2 * Projectile.scale, 5 * Projectile.scale, true, 1.5F * (Projectile.scale / 2), 100);
                Projectile.scale *= 3f;
                Projectile.timeLeft = 3;
                Projectile.localAI[0]++;
            }
            if (Projectile.ai[1]!=0)
            {
                Projectile.scale = Projectile.ai[1];
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0)
            {
                for (float A = 0; A < Projectile.scale; A ++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale *= 1.5f;
                    dust.velocity *= 0.1f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                }
                if (Projectile.timeLeft <3)
                {
                    Projectile.localAI[0]++;
                }
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 120)
            {
                Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                if (Projectile.ai[2] == 2)
                {
                    Projectile.velocity = (Projectile.velocity * 40 + (player.Center - Projectile.Center).PerfectNormalize() * 30) / 41;
                }
                else
                {

                    Projectile.velocity = (Projectile.velocity * 20 + (player.Center - Projectile.Center).PerfectNormalize() * 15) / 21;
                }
            }
            else
            {
                if (Projectile.velocity.Length() < 20)
                {
                    Projectile.velocity += Projectile.velocity.PerfectNormalize();
                }
            }
            if (Projectile.localAI[0] >= 1) Projectile.velocity = Vector2.Zero;
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
            Projectile.localAI[1]++;
            if (Projectile.localAI[0] == 0)
            {
                int Type = 6;
                for (float A = 0; A < Projectile.scale; A += 0.01f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale *= 1 * (Projectile.scale / 2);
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f * Projectile.scale, 2.5f * Projectile.scale);
                }
            }
            target.AddBuff(BuffID.OnFire, 300);
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
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(253, 62, 3, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/5, spriteEffects, 0f);

                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(192, 74, 90, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);

                    if (Projectile.ai[2] == 2)
                    {
                        color = new Color(253, 202,100, 120) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    }
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/5 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                for (int i = 0; i < 2; i++)
                {
                    if (Projectile.ai[2] == 2)
                        Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition + new Vector2(2 * Projectile.scale).RotatedBy(MathHelper.TwoPi / 3 * i + Projectile.timeLeft / 10), null, new Color(253, 182, 100, 120), Projectile.rotation, texture.Size() / 2, Projectile.scale / 5, spriteEffects, 0f);
                    else
                        Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition + new Vector2(2 * Projectile.scale).RotatedBy(MathHelper.TwoPi / 3 * i + Projectile.timeLeft / 10), null, new Color(253, 62, 3, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 5, spriteEffects, 0f);
                }
            }
            return false;
        }
    }
}