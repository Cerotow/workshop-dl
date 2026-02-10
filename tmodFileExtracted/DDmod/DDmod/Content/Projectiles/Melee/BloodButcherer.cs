using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee
{
    public class BloodButcherer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 40;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.alpha = 255;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 4)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 20;
                }
                else if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
            float V = Math.Abs(Projectile.velocity.Y)*0.04F;
            if (Projectile.velocity.X <0) V = -V;
            Projectile.rotation += Projectile.velocity.X * 0.04f + V;
        }
        public override void OnKill(int timeLeft)
        {
            int Type = ModContent.DustType<光球粒子>();
            for (int A = 0; A <40; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(118, 14, 18, 255))];
                dust.noGravity = true;
                dust.scale = 1f;
                dust.customData= dust.DustAI(1)+1;
                dust.velocity = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(1f, 1.5f) * (Projectile.velocity.Length() / 3));
            }
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 0.4f;
            PlaySound(sound, Projectile.position);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            float lifeStoled = damageDone * 0.01f;
            if (lifeStoled < 1)
            {
                lifeStoled = 1;
            }
            if (target.HasBuff(30)) lifeStoled *= 2;
            if ((int)lifeStoled > 0 && !player.moonLeech && target.type != NPCID.TargetDummy && target.life > 2)
            {
                NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, lifeStoled);
            }
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(30, Main.rand.Next(100,300));
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            if(Projectile.velocity.X<0)
            {
                spriteEffects = (SpriteEffects)1;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.GetAlpha(new Color(118, 14, 18, 255));
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale , spriteEffects, 0f);
            }
            for (int T = 0; T < 10; T++)
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            }
            return false;
        }
    }
}