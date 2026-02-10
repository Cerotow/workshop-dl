using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee
{
    public class 鬼齿 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Carrots");
           //DisplayName.AddTranslation(7, "胡萝卜");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 220;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.scale = 1.1F;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 30)
            {
                if (Projectile.velocity.Y < 10)
                {
                    Projectile.velocity.Y += 0.1f;
                }
            }

        }
        public override void OnKill(int timeLeft)
        {
            int Type = ModContent.DustType<光球粒子>();
            for (int A = 0; A < 30; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(220, 0, 25, 0))];
                dust.noGravity = true;
                dust.scale = 1f;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2f, 3f);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.NextBool(5))
            {
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
                target.AddBuff(30, Main.rand.Next(100, 300));
            }
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
            Color color = lightColor;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color oldcolor = new Color(220, 0, 25, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale * 1.3f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale * 1.3f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale * 1.3f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(220, 0, 25, 0), Projectile.rotation, new Vector2(texture.Width, texture.Height)/2, Projectile.scale*1.3f, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(220, 0, 25, 0), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale*1.3f, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(220, 0, 25, 0), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale*1.3f, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, spriteEffects, 0f);

            return false;
        }
    }
}