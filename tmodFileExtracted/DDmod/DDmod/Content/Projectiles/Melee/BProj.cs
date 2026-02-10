namespace DDmod.Content.Projectiles.Melee
{
    public class BProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("True Excalibur");
           //DisplayName.AddTranslation(7, "真神剑");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 900;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.alpha += 255;
            Projectile.scale =1f;
            Main.projFrames[Type] = 4;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            return;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 12)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 100;
                }
                else if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
            Projectile.Track(500, 20, 30, 10);
            Projectile.ProjScaleChange();
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 5 == 0)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 0.4f;
            PlaySound(sound, Projectile.position);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            //Projectile.scale = 1f;
            Projectile.DProj().color = new Color(110,10,210,255);
            Color color = Projectile.DProj().color * 1f;
            Projectile.DProj().Times[4] -= 0.1F;
            int l =30;
            int rl = 15;
            int he = 10;
            float RO = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            RO = 0;
            Texture2D texture = TextureAssets.Projectile[657].Value;
             //texture = DDTextures.Circle[2].Value;
            Texture2D texture2 = DDTextures.VoidStar.Value;
            Vector2 v = Projectile.Center - Main.screenPosition;
            for (int a = -l; a < l; a++)
            {
                float U = a;
                if (U > 0)
                {
                    U = -U;
                    //U = Math.Abs((float)U);
                }
                else
                {
                    U = Math.Abs((float)U);
                }
                color.A = (byte)(55 * (1 - Math.Abs((float)a) / l));
                Main.spriteBatch.Draw(texture, v - new Vector2(0, he * a).RotatedBy(RO), null, color * (1 - Math.Abs((float)a) / l), (float)a / rl + Projectile.DProj().Times[4], texture.Size() / 2, Projectile.scale * (2F - U / l) * new Vector2(1, 1F), 0, 0f);

                Main.spriteBatch.Draw(texture, v - new Vector2(0, he * a).RotatedBy(RO), null, color * (1 - Math.Abs((float)a) / l), (float)-a / rl - Projectile.DProj().Times[4], texture.Size() / 2, Projectile.scale * (2F - U / l) * new Vector2(1, 1F), 0, 0f);

                /*
                if (a == -l / 4)
                {
                    Color color2 = new Color(212, 192, 100, 255);
                    Main.spriteBatch.Draw(texture2, v - new Vector2(0, he * a * (1F - U / l) / 2).RotatedBy(RO), null, color2, 0, texture2.Size() / 2, Projectile.scale * new Vector2(1, 1F), 0, 0f);
                    Main.spriteBatch.Draw(texture2, v - new Vector2(0, he * a * (1F - U / l) / 2).RotatedBy(RO), null, color2, 0, texture2.Size() / 2, Projectile.scale * new Vector2(1, 1F), 0, 0f);
                    Main.spriteBatch.Draw(texture2, v - new Vector2(0, he * a * (1F - U / l) / 2).RotatedBy(RO), null, color2, 0, texture2.Size() / 2, Projectile.scale * new Vector2(1, 1F), 0, 0f);

                }*/
            }
            /*
            return false;
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.GetAlpha(new Color(236, 200, 19));
            color.A = 0;
            Vector2 vector = Projectile.Size/2;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition,new Rectangle?(new Rectangle(0,texture.Height/4*Projectile.frame,texture.Width,texture.Height/4)), Color.White, Projectile.rotation, new Vector2(texture.Width/2, texture.Height/8), Projectile.scale, spriteEffects, 0f);
            */
            return false;
        }
    }
}