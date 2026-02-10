
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Boss
{
    /// <summary>
    /// ai[0]X位置
    /// ai[1]X速度
    /// </summary>
    public class Boss电珠 : ModProjectile
    {
        public static Asset<Texture2D> asset;
        public override void Load()
        {
            asset = ModContent.Request<Texture2D>(Texture + "链条");
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void AI()
        {
            Projectile.ProjScaleChange();
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 5;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 4 == 0)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }
            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha = 255 - (int)(Projectile.timeLeft / 60F * 255);
            }
            if (!Projectile.DProj().Bool[0])
            {
                //启用第二套ai
                if (Projectile.ai[1] != 0)
                {
                    Projectile.DProj().Bool[1] = true;
                }
                Projectile.DProj().Bool[0] = true;
            }
            //附属弹幕位置
            if (!Projectile.DProj().Bool[1])
            {
                Projectile.ai[1]++;

                if (Projectile.DProj().vector[0] == Vector2.Zero)
                {
                    Projectile.DProj().vector[0] = Projectile.Center;
                }
                if (Projectile.DProj().vector[1] == Vector2.Zero)
                {
                    Projectile.DProj().vector[1] = Projectile.velocity;
                }
                if (Projectile.ai[1] <= 60)
                {
                    Projectile.velocity *= 0.98F;
                    Projectile.DProj().vector[1] *= 0.98F;
                }
                else if (Projectile.ai[1] <= Projectile.ai[2])
                {
                    Projectile.velocity = Projectile.ai[0].ToRotationVector2() * (Projectile.ai[1] - Projectile.ai[2]);
                    Projectile.DProj().vector[1] = -Projectile.velocity;
                }
                else
                {
                    Projectile.velocity *= 0.98F;
                    Projectile.DProj().vector[1] *= 0.98F;
                }
                Projectile.DProj().vector[0] += Projectile.DProj().vector[1];
            }
            else
            {

                Projectile.ai[1]++;

                if (Projectile.DProj().vector[0] == Vector2.Zero)
                {
                    Projectile.DProj().vector[0] = Projectile.Center;
                    Projectile.DProj().vector[0] = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                }
                if (Projectile.DProj().vector[1] == Vector2.Zero)
                {
                    Projectile.DProj().vector[1] = Projectile.velocity* Projectile.ai[2];
                }
                Projectile.velocity *= 0.98F;
                Projectile.DProj().vector[1] *= 0.98F;
                Projectile.DProj().vector[0] += Projectile.DProj().vector[1];
            }
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            projHitbox.X = (int)Projectile.DProj().vector[0].X-20;
            projHitbox.Y = (int)Projectile.DProj().vector[0].Y-20;
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            Vector2 vector = (Projectile.DProj().vector[0] - Projectile.Center);
            projHitbox.Width = 16;
            projHitbox.Height = 16;
            for (int a = 0; a < vector.Length() / 16; a++)
            {
                Vector2 vector2 = Projectile.Center + vector.PerfectNormalize() * 16 * a;
                projHitbox.X = (int)vector2.X-8;
                projHitbox.Y = (int)vector2.Y-8;
                if (projHitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Color color = Projectile.GetAlpha(Color.White);
            color.A = (byte)(150 * (1F - Projectile.alpha / 255F));
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);

            Main.spriteBatch.Draw(texture, Projectile.DProj().vector[0] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);

            Vector2 vector = (Projectile.DProj().vector[0] - Projectile.Center);
            /*
            texture = DDTextures.VoidStar.Value;
            for (int a = 0; a < vector.Length(); a += 6)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center+ vector.PerfectNormalize()*a - Main.screenPosition, null, new Color(0,155,255,0) * (1F - Projectile.alpha / 255F), vector.ToRotation(), texture.Size()/2, new Vector2(2,1)*Projectile.scale/8, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center+ vector.PerfectNormalize()*a - Main.screenPosition, null, new Color(0,155,255,0).Opposite() * (1F - Projectile.alpha / 255F), vector.ToRotation(), texture.Size()/2, new Vector2(2, 1) * Projectile.scale/16, 0, 0f);

            }*/
            texture = asset.Value;
            Rectangle rectangle = new Rectangle(texture.Width/4 * Projectile.frame,0,texture.Width/4,texture.Height);
            for (int a = 0; a < vector.Length() / texture.Height-1; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center + vector.PerfectNormalize() * a * texture.Height - Main.screenPosition, rectangle, Color.White * (1F - Projectile.alpha / 255F), vector.ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width / 8, 0), 1, 0, 0f);

            }
            rectangle.Height = (int)(vector.Length() % texture.Height);
            Main.spriteBatch.Draw(texture, Projectile.Center + vector.PerfectNormalize() * ((int)(vector.Length() / texture.Height)) * texture.Height - Main.screenPosition, rectangle, Color.White * (1F - Projectile.alpha / 255F), vector.ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width / 8, 0), 1, 0, 0f);

            Main.spriteBatch.Draw(texture, Projectile.DProj().vector[0] - Main.screenPosition, null, new Color(69, 94, 223, 150) * (1F - Projectile.alpha / 255F), Projectile.rotation, texture.Size() / 2, Projectile.scale / 8, 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.DProj().vector[0] - Main.screenPosition, null, new Color(69, 94, 223, 0).Opposite() * (1F - Projectile.alpha / 255F), Projectile.rotation, texture.Size() / 2, Projectile.scale / 16, 0, 0f);
            return false;
        }
    }
}