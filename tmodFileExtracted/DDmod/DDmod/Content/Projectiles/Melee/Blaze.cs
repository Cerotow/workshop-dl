namespace DDmod.Content.Projectiles.Melee
{
    public class Blaze : ModProjectile
    {
        public override string Texture => "DDmod/Content/Projectiles/Melee/SwordWave";
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 40;
            Projectile.extraUpdates = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        int[] Body = new int[14];
        Vector2[] Center = new Vector2[16];
        int Length = 6;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if(!Projectile.DProj().Bool[0])
            {
                Projectile.scale *= player.GetAdjustedItemScale(player.ActiveItem());
                Projectile.ProjScaleChange();
                Projectile.DProj().Bool[0] = true;
                Projectile.velocity *= player.GetAdjustedItemScale(player.ActiveItem())*1.25f;
            }
            if (Projectile.timeLeft > 30)
            {
                Projectile.velocity *= 0.94F;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.velocity *= 0.97f;
            for (int A = 0; A <= Length; A++)
            {
                Center[A] = Projectile.Center - (new Vector2(12 * Math.Abs(A - Length / 2), (A - Length / 2) * 20).RotatedBy(Projectile.rotation));
                if (Main.rand.NextBool(4))
                {
                    int Type = 6;
                    Dust dust = Main.dust[NewDust(Center[A] - Projectile.Size / 2, Projectile.height, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.velocity = Projectile.velocity;
                    dust.scale = 1f;
                }
            }

            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(24, 300);
            Projectile.damage /= 2;
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity * 0.001f;
            if (Projectile.timeLeft > 60)
            {
                Projectile.timeLeft = 60;
            }
                return false;
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
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Color color = new Color(253, 62, 3, 80)*1.5f;
            if (Projectile.timeLeft < 20)
            {
                color *= (float)Projectile.timeLeft / 20;
            }
            else if (Projectile.timeLeft < 30)
            {
                color *= 1-(float)(Projectile.timeLeft-19) / 19f;
            }
            else
            {
                color *= 0;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition-Projectile.velocity.PerfectNormalize()*20, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0);
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition - Projectile.velocity - Projectile.velocity.PerfectNormalize() * 20, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale , 0, 0);
            }
            return false;
        }
    }
}