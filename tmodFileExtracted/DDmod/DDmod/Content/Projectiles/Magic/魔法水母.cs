using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;

namespace DDmod.Content.Projectiles.Magic
{
    public class 魔法水母 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.scale = 1;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Projectile.scale = Projectile.ai[0];
            Projectile.ProjScaleChange();
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = Main.rand.Next(1, 3);
                //NewDustChange4((int)(20 * Projectile.ai[0]), Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<光圈粒子>(), 2 * Projectile.ai[0], 3 * Projectile.ai[0], true,  Projectile.ai[0], Projectile.ai[0], 100, 0, color(), 1 + new Dust().DustAI(1));

                    Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, color())];
                    dust.noGravity = true;
                    dust.scale = 0.1f;
                    dust.alpha = -5;
                    dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                    dust.velocity = Vector2.Zero;
                    dust.customData = new Vector4(0.04F, 10/Projectile.scale, 0, 2F);
                GlobalDust.DustPlayerOwner[dust.dustIndex] = Projectile.Player().whoAmI;

                Projectile.localAI[2] = Projectile.velocity.Length();
                Projectile.rotation = Projectile.velocity.ToRotation();
                SoundStyle sound = new SoundStyle(DDHelper.Sound(1, "溅水"));
                sound.MaxInstances = 50;
                sound.Volume = 0.2F;
                sound.Pitch = 0.3f;
                PlaySound(sound,Projectile.Center);
            }
            //帧图
            Projectile.localAI[1]++;
            if (Projectile.localAI[1] >= 4 * 8)
            {
                Projectile.localAI[1] = 0;
            }
            Projectile.velocity = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * Projectile.localAI[2] * Projectile.ai[0];

            Projectile.localAI[2] *= 0.94F;

            if (Projectile.localAI[1] == 2 * 8)
            {
                Projectile.localAI[2] = 12;
            }
            NPC npc = NPCdirection.FindClosest(Projectile.Center, 1500);
            if (npc != null)
            {
                Vector2 vector = npc.Center-Projectile.Center;
                Projectile.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, 0.04F * Projectile.ai[0]);

            }
        }
        Color color()
        {
            if(Projectile.ai[1]==1)
            {
                return new Color(198, 86, 246,50);
            }
            if(Projectile.ai[1]==2)
            {
                return new Color(69,151,223,50);
            }
            return Color.White;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Rectangle rectangle = new Rectangle(0, texture.Height / 4 * ((int)Projectile.localAI[1] / 8), texture.Width, texture.Height / 4);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color2 = Projectile.GetAlpha(color()) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(texture, vector2, rectangle, color2, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(50,50,50,255), Projectile.rotation, rectangle.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, color(), Projectile.rotation, rectangle.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, color(), Projectile.rotation, rectangle.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            SoundStyle sound = new SoundStyle(DDHelper.Sound(1, "溅水"));
            sound.MaxInstances = 50;
            sound.Volume = 0.4F;
            sound.Pitch = 1;
            PlaySound(sound, Projectile.Center);
            for (int A = 0; A < 2; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, color())];
                dust.noGravity = true;
                dust.scale = 0.1f;
                dust.alpha = -5;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                dust.velocity = Vector2.Zero;
                dust.customData = new Vector4(0.1F, 40 * Projectile.scale, 0, 2F);
            }//NewDustChange4((int)(20 * Projectile.ai[0]), Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 2 * Projectile.ai[0], 3 * Projectile.ai[0], true, Projectile.ai[0], Projectile.ai[0], 100, 0, color(), 1+new Dust().DustAI(1));
        }
    }
}