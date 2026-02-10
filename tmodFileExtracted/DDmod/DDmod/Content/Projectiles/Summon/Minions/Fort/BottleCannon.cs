namespace DDmod.Content.Projectiles.Summon.Minions.Fort
{
    public class BottleCannon : Summons
    {
        public override void SetDefault()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            shoot = ModContent.ProjectileType<FireBall>();
            shootSpeed = 12;
            AttackSpeed = 12;
            IgnoreTile = false;
            Projectile.sentry = true;
            Projectile.minion = false;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            //ProjectileID.Sets.SentryShot[Projectile.type] = true;
            //Main.projPet[Projectile.type] = true;
            //ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            /// <summary> 感觉有点针对召唤师了 </summary> ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void Visual()
        {
            Vector2 direction = Vector2.Zero;
            if (npc != null)
            {
                direction = npc.Center - Projectile.Center;
            }

            Projectile.RotationSpeed(direction.ToRotation() + MathHelper.PiOver2, 0.1F);
        }
        public override bool MobileAI()
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
        public override bool AttackAI()
        {
            Vector2 direction = Vector2.Zero;
            if (npc != null)
            {
                direction = npc.Center - Projectile.Center;
            }
            if (target && DDHelper.SpecifyDirection(Projectile.rotation, direction.ToRotation() + MathHelper.PiOver2, 0.1f))
            {
                Projectile.ai[0]++;
                DistanceNPC = direction.Length();

                if (Projectile.ai[0] >= AttackSpeed)
                {
                    NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * shootSpeed, shoot, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.netUpdate = true;
                    Projectile.ai[0] = 0;
                }
            }
            else
            {
                Projectile.ai[0] = 0;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Summon/Minions/Fort/BottleCannon2");
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition + vector, null, Color.White, 0, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition + vector, null, Color.White, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale + Projectile.ai[0] / 30, Projectile.scale - Projectile.ai[0] / 30), (SpriteEffects)Projectile.spriteDirection, 0f);
            return false;
        }
    }
}