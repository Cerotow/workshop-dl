using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic
{
    public class VioletLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1F;
            Projectile.timeLeft = 1600;
            Projectile.extraUpdates = 10;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 10;
        }
        public override void AI()
        {
            if (Projectile.ai[2] == 1)
            {
                Projectile.scale = 0.4F;
                Projectile.ProjScaleChange();
                Projectile.extraUpdates = 2;
                Projectile.ai[2]++;
            }
            if (Projectile.ai[1] == 1)
            {
                Projectile.timeLeft = 300;
                Projectile.extraUpdates = Projectile.timeLeft;
                Projectile.ai[1]++;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            /*if(Projectile.DPoroj().vector[0]==Vector2.Zero)
            {
                Projectile.DPoroj().vector[0] = Projectile.velocity.PerfectNormalize();
                Projectile.velocity /= 20;
            }
            if (Projectile.ai[0] % 5 == 0)
            {
                Vector2 vector1 = Utils.RotatedBy(Projectile.velocity.PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-0.1F,0.1F), default(Vector2));
                Projectile.velocity = vector1;
            }*/
            if (Projectile.ai[0] > 2)
            {
                for (int A = 0; A < 2; A++)
                {
                    int Type = ModContent.DustType<紫激光粒子>();
                    Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale;
                    dust.velocity = Vector2.Zero;
                    dust.customData = Projectile.scale;
                    dust.GetAlpha(new Color(0, 0, 0, 0));
                    dust.rotation = Projectile.rotation;
                }
            }
            Projectile.ai[0]++;
        }
        public override void OnKill(int timeLeft)
        {
            int Type = ModContent.DustType<紫激光粒子>();
            for (int A = 0; A < 30 * Projectile.scale; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = Projectile.scale;
                dust.customData = Projectile.scale;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3.5f, 7.5f);
                dust.rotation = Projectile.rotation;
            }

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            for (int A = 0; A < 2; A++)
            {
                if (Projectile.owner == Main.myPlayer && Projectile.ai[2] == 0)
                {
                    int R = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(2, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), Projectile.type, Projectile.damage / 2, Projectile.knockBack, Projectile.owner, 0, 0, 1);
                    Main.projectile[R].timeLeft = 60;
                    Main.projectile[R].penetrate = 10;
                }
            }
            int Type = ModContent.DustType<紫激光粒子>();
            for (int A = 0; A < 10 * Projectile.scale; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = Projectile.scale;
                dust.customData = Projectile.scale;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f, 2.5f);
                dust.rotation = Projectile.rotation;
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
                Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(66, 49, 255, 0);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.oldPos[0] + vector - Main.screenPosition, null, new Color(66, 49, 255, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[Projectile.oldPos.Length - 1] + vector - Main.screenPosition, null, new Color(255, 155, 0, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 1.5F, spriteEffects, 0f);
            }
            return false;
        }
    }
}