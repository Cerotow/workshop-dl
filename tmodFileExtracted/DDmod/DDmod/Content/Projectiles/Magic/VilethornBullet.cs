using Terraria;

namespace DDmod.Content.Projectiles.Magic
{
    public class VilethornBullet : ModProjectile
    {
        public float TelegraphDelay
        {
            get
            {
                return Projectile.ai[0];
            }
            set
            {
                Projectile.ai[0] = value;
            }
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (Projectile.localAI[1] >= 1 && Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
            }
            int Type = 46;
            if (Projectile.localAI[0] == 1)
            {
                for (int a = 1; a < Projectile.scale * 4; a++)
                {
                    if (Projectile.ai[1] == 0)
                    {
                        Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), Main.rand.NextFloat(-0.4F, 0.4F)* Projectile.scale, default);
                        //Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, Pvelocity * 30, 7, (int)(Projectile.damage / Projectile.scale / 1.5f), Projectile.knockBack, Projectile.owner, 0, 1, 1)];
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, Pvelocity * 30, ModContent.ProjectileType<魔刺>(), (int)(Projectile.ai[0] / 2), Projectile.knockBack, Projectile.owner, 0, Main.rand.Next((int)(4 + Projectile.velocity.Length()/4), (int)(8 + Projectile.velocity.Length()/2)), 0.5F)];
                        projectile.CritChance = Projectile.CritChance;
                    }
                }
                NewDustChange((int)(Projectile.scale * 100), Projectile.position, Projectile.Size, Type, 2 * Projectile.scale, 5 * Projectile.scale, true, 1.5F * (Projectile.scale / 2), 100);
                Projectile.scale *= 2f;
                Projectile.timeLeft = 3;
                Projectile.velocity = Vector2.Zero;
                Projectile.localAI[0]++;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0)
            {
                for (float A = 0; A < Projectile.scale; A += 0.5f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale *= 1.25f;
                    dust.velocity *= 0.1f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                }
            }
            Projectile.ProjScaleChange();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.localAI[0] == 0)
                Projectile.localAI[0]++;
            Projectile.tileCollide = false;
            Projectile.velocity = oldVelocity;
            return Projectile.localAI[0] == 2;
        }
        public override void OnKill(int timeLeft)
        {
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
                int Type = 46;
                for (float A = 0; A < Projectile.scale; A += 0.01f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale *= 1 * (Projectile.scale / 2);
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f * Projectile.scale, 2.5f * Projectile.scale);
                    dust.color = new Color(155, 200, 100, 100);
                }
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if (width > 24)
            {
                width = 24;
            }
            if (height > 24)
            {
                height = 24;
            }
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
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
                Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(150, 200, 100, 200) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 1f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            }
            return false;
        }
    }
}