
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class ArrowofHeaven : ModProjectile
    {
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.arrow = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
            DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Ranged/ArrowofHeaven_Glow");
            DDGlobalProjectile.ScaleGlow[Projectile.type] =4F;
            DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255,255,255,0);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.Player().Center.Y - Projectile.Center.Y > 800&& Projectile.ai[0]==0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    for (int a = 0; a < 3; a++)
                    {
                        Vector2 vector = Projectile.Player().Dplayer().MouseWorld - new Vector2(0, Projectile.Player().Dplayer().MouseWorld.Y - Projectile.Center.Y).RotatedBy(Main.rand.NextFloat(-0.6F, 0.6F));
                        Vector2 vector2 = (Projectile.Player().Dplayer().MouseWorld - vector).PerfectNormalize() * 9;
                        if (Projectile.DProj().Times[0] == 1)
                        {
                            Projectile.DProj().Times[0] = Type;
                        }
                        Projectile projectile = Main.projectile[NewProjectile(Projectile.GetSource_FromAI(), vector, vector2, (int)Projectile.DProj().Times[0], Projectile.damage, Projectile.knockBack, Projectile.owner)];
                        if (Projectile.DProj().Times[0] == Type)
                        {
                            projectile.ai[0] = 1;
                        }
                    }
                    for (int a = 0; a < 3; a++)
                    {
                        Vector2 vector = Projectile.Player().Dplayer().MouseWorld - new Vector2(0, Projectile.Player().Dplayer().MouseWorld.Y - Projectile.Center.Y).RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F));
                        //Vector2 vector2 = new Vector2(0,9);
                        Vector2 vector2 = (Projectile.Player().Dplayer().MouseWorld - vector).PerfectNormalize() * 9;
                        NewProjectile(Projectile.GetSource_FromAI(), vector, vector2, Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 1);
                    }
                }
                Projectile.Kill();
            }
            if (!Projectile.tileCollide&& Projectile.ai[0] != 0)
            {
                Projectile.tileCollide = Projectile.Center.Y > Projectile.Player().Center.Y;
                if (Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Player().position, Projectile.Player().width, Projectile.Player().height))
                {
                    Projectile.tileCollide = true;
                }
            }
            if (Projectile.ai[0] == 1)
            {
                Projectile.penetrate = 1;
                Dust dust;
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 4;
                    Vector2 vector = -Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0.3F, 0.5F)) * Main.rand.NextFloat(0.25F, 0.75F) * 3;
                    dust = Main.dust[NewDust(Projectile.PreviousCenter() + vector.PerfectNormalize() * 8 + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(135, 214, 227, 0))];
                    dust.noGravity = true;
                    dust.velocity = vector;
                    dust.alpha = 100;
                    dust.scale = 1;
                    dust.customData = 1;
                    dust.rotation = dust.velocity.ToRotation();
                    GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;
                    vector = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(0.3F, 0.5F)) * Main.rand.NextFloat(0.25F, 0.75F) * 3;
                    dust = Main.dust[NewDust(Projectile.PreviousCenter() + vector.PerfectNormalize() * 8 + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(135, 214, 227, 0))];
                    dust.noGravity = true;
                    dust.velocity = vector;
                    dust.alpha = 100;
                    dust.scale = 1;
                    dust.customData = 1;
                    dust.rotation = dust.velocity.ToRotation();
                    GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;

                }

                dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(135, 214, 227, 0))];
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.alpha = 100;
                dust.scale = 2;
                dust.customData = 1;
                dust.rotation = Projectile.velocity.ToRotation();
            }
            else
            {
                Vector2 vector = -Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0.3F, 0.5F)) * Main.rand.NextFloat(0.25F, 0.75F)*3;
                Dust dust = Main.dust[NewDust(Projectile.PreviousCenter() + vector.PerfectNormalize() * 8 + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(135, 214, 227, 0))];
                dust.noGravity = true;
                dust.velocity = vector;
                dust.alpha = 100;
                dust.scale = 2;
                dust.customData = 1;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;
                vector = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(0.3F, 0.5F)) * Main.rand.NextFloat(0.25F, 0.75F) * 3;
                dust = Main.dust[NewDust(Projectile.PreviousCenter() + vector.PerfectNormalize()*8 + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(135, 214, 227, 0))];
                dust.noGravity = true;
                dust.velocity = vector;
                dust.alpha = 100;
                dust.scale = 2;
                dust.customData = 1;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;

                dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(135, 214, 227, 0))];
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.alpha = 100;
                dust.scale = 3;
                dust.customData = 1;
                dust.rotation = Projectile.velocity.ToRotation();
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.position = Projectile.oldPosition;
                Projectile.velocity = Projectile.oldVelocity;
                for (int a = 0; a < 8; a++)
                {
                    Vector2 vector = -Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0, 0.5F)) * Main.rand.NextFloat(0.1F, 0.75F) * 4;
                    Dust dust = Main.dust[NewDust(Projectile.Center + vector.PerfectNormalize() * 8 + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(135, 214, 227, 0))];
                    dust.noGravity = true;
                    dust.velocity = vector;
                    dust.alpha = 100;
                    dust.scale = Main.rand.NextFloat(2F, 3.8F);
                    dust.customData = 1.8f;
                    dust.rotation = dust.velocity.ToRotation();
                    vector = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, 0.5F)) * Main.rand.NextFloat(0.1F, 0.75F) * 4;
                    dust = Main.dust[NewDust(Projectile.Center + vector.PerfectNormalize() * 8 + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(135, 214, 227, 0))];
                    dust.noGravity = true;
                    dust.velocity = vector;
                    dust.alpha = 100;
                    dust.scale = Main.rand.NextFloat(2F, 3.8F);
                    dust.customData = 1.8f;
                    dust.rotation = dust.velocity.ToRotation();
                }
            }
            else
            NewDustChange(30, Projectile.Center, Vector2.Zero, ModContent.DustType<轻语矢粒子>(), 0, 4,true,1F,100);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            Color color = DDGlobalProjectile.GlowColor[Projectile.type];
            color.A = 0;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                color = DDGlobalProjectile.GlowColor[Projectile.type] * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(DDGlobalProjectile.Glow[Projectile.type].Value, vector2, null, color, Projectile.rotation + MathHelper.PiOver2, DDGlobalProjectile.Glow[Projectile.type].Size() / 2, Projectile.scale / DDGlobalProjectile.ScaleGlow[Projectile.type], 0, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White , Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, 0, 0f);

            return false;
        }
        internal  Trailing TrailDrawer;
    }
}
