namespace DDmod.Content.Projectiles.Melee
{
    public class ShadowBlaze : ModProjectile
    {
        public override string Texture => "DDmod/Content/Projectiles/Melee/SwordWave2";
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height =40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.scale *= 0.8F;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        int[] Body = new int[9];
        int Length = 8;
        Vector2[] Center = new Vector2[9];
        public override bool PreAI()
        {
            Projectile.extraUpdates = 0;
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 0.97f;
            for (int A = 0; A <= Length; A++)
            {
                Center[A] = Projectile.Center - (new Vector2(20 * Math.Abs(A- Length/2), (A- Length/2) * 20).RotatedBy(Projectile.rotation));
                if (Main.rand.NextBool(4))
                {
                    int Type = 27;
                    Dust dust = Main.dust[NewDust(Center[A]-Projectile.Size/2, Projectile.height, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.velocity = Projectile.velocity;
                    dust.scale = 1f;
                }
            }
            if(!Projectile.tileCollide)
            {
                Projectile.velocity *=  0.8f;
            }
            
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(BuffID.ShadowFlame, 300);
            Projectile.velocity *= 0.2f;
            Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Projectile.velocity = oldVelocity;
            Projectile.tileCollide = false;
            if (Projectile.timeLeft > 60)
            {
                Projectile.timeLeft = 60;
            }
            return false;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle[] vectors = new Rectangle[Length+1];
            bool B = false;
            for (int A = 0; A <= Length; A++)
            {
                Vector2 Size = Projectile.Size / 2;
                vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                if (vectors[A].Intersects(targetHitbox))
                {
                    B = true;
                    break;
                }
            }
            return new bool?(B);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Color color = new Color(81, 6, 233, 40);
            float A = 1;
            if (Projectile.timeLeft < 60)
            {
                A *= (float)Projectile.timeLeft / 60;
            }
            else if (Projectile.timeLeft > 160)
            {
                A *= 1 - (float)(Projectile.timeLeft - 160) / 20;
            }
            color *= A;
            /*Rectangle[] vectors = new Rectangle[Length+1];
            for (int A = 0; A <= Length; A++)
            {
                Vector2 Size = Projectile.Size / 2;
                vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, new Vector2((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y)) - Main.screenPosition, null, Color.White, 0, Vector2.Zero, new Vector2(Projectile.width, Projectile.height)/2, 0, 0);
            }*/
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White, 0, Vector2.Zero, new Vector2(Projectile.width, Projectile.height) / 2, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition- Projectile.velocity - Projectile.velocity.PerfectNormalize()*30, null, new Color(100,100,100,255) * A, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(2F, 1.5F), 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition- Projectile.velocity - Projectile.velocity.PerfectNormalize()*30, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(2F, 1.5F), 0, 0);
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition - Projectile.velocity - Projectile.velocity.PerfectNormalize() * 30, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(2F, 1.5F), 0, 0);
            }
            return false;
        }
    }
}