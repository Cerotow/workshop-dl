namespace DDmod.Content.Projectiles.Summon.Minions.Fort
{
    public class FanTower : Summons
    {
        public override void SetDefault()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            shoot = ModContent.ProjectileType<Fan>();
            shootSpeed = 5;
            AttackSpeed = 50;
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
            Projectile.rotation += 0.22f;
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
            if (target)
            {
                Projectile.ai[0]++;
                DistanceNPC = direction.Length();

                if (Projectile.ai[0] >= AttackSpeed)
                {
                    NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction.PerfectNormalize() * shootSpeed, shoot, Projectile.damage, Projectile.knockBack, Projectile.owner);
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
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Summon/Minions/Fort/FanTower2");
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition + vector, null, Color.White, 0, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition + vector, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            return false;
        }
    }
}