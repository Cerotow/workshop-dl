namespace DDmod.Content.Projectiles.Magic
{
    public class BloodBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter>6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 3)
            {
                Projectile.frame = 0;
            }
            if (Projectile.localAI[1] >= 1 && Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
            }
            if (Projectile.localAI[0] == 1)
            {
                NewDustChange((int)(Projectile.scale * 100), Projectile.position, Projectile.Size, Type, 2 * Projectile.scale, 5 * Projectile.scale, true, 1.5F * (Projectile.scale / 2), 100);
                Projectile.scale *= 3.5f;
                Projectile.timeLeft = 3;
                Projectile.localAI[0]++;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0)
            {
                for (float A = 0; A < Projectile.scale; A += 0.5f)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 5)];
                    dust.noGravity = true;
                    dust.scale *= Projectile.scale/2;
                    dust.velocity = -Projectile.velocity / 5;
                }
            }
            Projectile.ProjScaleChange();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.localAI[0] == 0)
                Projectile.localAI[0]++;
            Projectile.tileCollide = false;
            return Projectile.localAI[0] == 2;
        }
        public override void OnKill(int timeLeft)
        {
            for (float A = 0; A < 30* Projectile.scale; A ++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center, 1,1, 5,0,0,100)];
                dust.noGravity = false;
                dust.scale *= 2f;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 2f * Projectile.scale);
            }
            PlaySound(SoundID.NPCDeath1, Projectile.Center);
        }
        public override bool PreKill(int timeLeft)
        {
            if (Projectile.localAI[0] == 0)
                Projectile.localAI[0]++;
            return Projectile.localAI[0] == 2;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.localAI[1]++;
            if (Projectile.localAI[0] == 0)
            {
                Projectile.damage /= 2;
            }
            Player player = Main.player[Projectile.owner];
            if (Projectile.localAI[1] < 4)
            {
                float lifeStoled = damageDone * 0.04f;
                if (lifeStoled < 1)
                {
                    return;
                }
                if (lifeStoled > player.statLifeMax2 / 50)
                {
                    lifeStoled = player.statLifeMax2 / 50;
                }
                if (target.HasBuff(30)) lifeStoled *= 2;
                if ((int)lifeStoled > 0 && !player.moonLeech && target.type != NPCID.TargetDummy && target.life > 2)
                {
                    player.Dplayer().VampireCD = 60;
                    NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, lifeStoled);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0] == 0)
            {
                SpriteEffects spriteEffects = (SpriteEffects)1;
                if (Projectile.direction == 1)
                {
                    spriteEffects = 0;
                }
                Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
                Vector2 vector = Projectile.Size / 2;
                Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type]*Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]));
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, rectangle, lightColor, Projectile.rotation,new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]/2)/2, Projectile.scale, spriteEffects, 0f);
            }
            return false;
        }
    }
}