namespace DDmod.Content.Projectiles.Melee
{
    public class HellfireBlaze : ModProjectile
    {
        public override string Texture => "DDmod/Content/Projectiles/Melee/SwordWave2";
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.scale /= 2;
            Projectile.alpha = 255;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        int[] Body = new int[14];
        Vector2[] Center = new Vector2[12];
        int Length = 4;
        public override bool PreAI()
        {
            Projectile.extraUpdates = 0;
            Player player = Main.player[Projectile.owner];
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.scale *= player.GetAdjustedItemScale(player.ActiveItem());
                Projectile.ProjScaleChange();
                Projectile.DProj().Bool[0] = true;
                Projectile.velocity *= player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 1.02f;
            for (int A = 0; A <= Length; A++)
            {
                Center[A] = Projectile.Center - (new Vector2(12 * Math.Abs(A - Length / 2), (A - Length / 2) * 20).RotatedBy(Projectile.rotation) * (Projectile.scale * 2));
                for (int a = 1; a < Projectile.velocity.Length(); a += 6)
                {
                    if (Main.rand.NextBool(4))
                    {
                        int Type = 6;
                        Dust dust = Main.dust[NewDust(Center[A] - Projectile.Size / 2 - Projectile.velocity.PerfectNormalize() * a, Projectile.height, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = -Projectile.velocity/10*(Projectile.scale*2);
                        dust.scale = 2f* Projectile.scale;
                    }
                }
            }
            if (Projectile.timeLeft > 170)
            {
                //Projectile.position += player.velocity;
            }
            if (Projectile.timeLeft == 170)
            {
                Projectile.velocity *= 5;
            }
                return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 1; i < 60; i++)
            {
                for (int A = 0; A <= Length; A++)
                {
                    Center[A] = Projectile.Center - (new Vector2(12 * Math.Abs(A - Length / 2), (A - Length / 2) * 20).RotatedBy(Projectile.rotation) * (Projectile.scale * 2));
                    int Type = 6;
                    Dust dust = Main.dust[NewDust(Center[A] - Projectile.Size / 4, Projectile.height/2, Projectile.height/2, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.velocity = new Vector2(Main.rand.NextFloat(0, 2.28F)).RotatedBy(Main.rand.NextFloat(0,6.28F)) * (Projectile.scale * 2);
                    dust.scale = 1.5f * (Projectile.scale * 2);
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<地狱之火>(), 300);
            Projectile.damage = (int)(Projectile.damage*0.75F);
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle[] vectors = new Rectangle[Length + 1];
            bool B = false;
            for (int A = 0; A <= Length; A++)
            {
                Vector2 Size = Projectile.Size / 2;
                vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                if (vectors[A].Intersects(targetHitbox))
                {
                    B = true;
                }
            }
            return new bool?(B);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 170)
            {
                return false;
            }
                Texture2D texture = TextureAssets.Projectile[Type].Value;
            Color color = new Color(253, 62, 3, 0);
            if (Projectile.timeLeft < 60)
            {
                color *= (float)Projectile.timeLeft / 60;
            }
            else if (Projectile.timeLeft > 150)
            {
                color *= 1 - (float)(Projectile.timeLeft - 150) / 20;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition-Projectile.velocity.PerfectNormalize()*28, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition-Projectile.velocity.PerfectNormalize()*28, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition-Projectile.velocity.PerfectNormalize()*28, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0);
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                color = new Color(255, 74, 50, 0);
                if (Projectile.timeLeft < 60)
                {
                    color *= (float)Projectile.timeLeft / 60;
                }
                else if (Projectile.timeLeft > 150)
                {
                    color *= 1 - (float)(Projectile.timeLeft - 150) / 20;
                }
                //Projectile.oldPos[i] = Projectile.position - Projectile.velocity.PerfectNormalize() * i*8;
                for (int a = 1; a < Projectile.velocity.Length(); a+=2)
                {
                    Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 28 + Projectile.velocity.PerfectNormalize()* a, null, oldcolor*0.25f, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0);
                }
            }
            return false;    
            Rectangle[] vectors = new Rectangle[Length + 1];
            bool B = false;
            for (int A = 0; A <= Length; A++)
            {
                Vector2 Size = Projectile.Size / 2;
                vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Center[A] - Main.screenPosition, null, Color.White*0.6F,0, DDTextures.WhitePng.Size()/2, Size, 0, 0);
            }
        }
    }
}