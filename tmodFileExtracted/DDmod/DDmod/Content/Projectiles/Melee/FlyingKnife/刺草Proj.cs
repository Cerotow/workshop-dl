using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 刺草Proj : 飞刀Proj
    {
        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            Projectile.timeLeft = 330;
            Projectile.width = 10;
            Projectile.height = 10;
        }
        public override void PostAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.NextBool(50))
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<孢子>(), Projectile.damage / 2, 0, player.whoAmI);
                }
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0]%4==0)
            {
                NewDustChange(2, Projectile.Center - new Vector2(4), Vector2.Zero, 44, 0, 4, true, 1.2f);
            }
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            NewDustChange(30, Projectile.Center, Vector2.Zero, 44, 0, 25, true, 1.5f);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);

        }
    }
    public class 孢子 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 120;
            Projectile.scale = 1F;
        }
        public override void PostAI()
        {
            Projectile.Track(400, 20, 3, 20);
            Projectile.rotation += 0.01F;
            if (Projectile.timeLeft < 50)
            {
                Projectile.alpha = (int)(255 - 255* ((float)Projectile.timeLeft/50));
            }
            else
            {
                Projectile.alpha = 0;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);

        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, Main.rand.Next(569,572), Projectile.damage, 0, Projectile.owner);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[567].Value;
            Main.instance.LoadProjectile(567);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, texture.OffsetCenter(1, 1), Projectile.scale, 0, 0f);
            return false;
        }
    }
}