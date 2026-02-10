using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 秘银飞刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void PostAI()
        {
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[1] == 0)
            {
                PlaySound(SoundID.Dig, Projectile.position);
                for (int i = 0; i < 20; i++)
                {
                    Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                    NewDust(Projectile.position, 1, 1, 49, projDirection.X, projDirection.Y, 0, default, 1f);
                }
            }

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5))
            {
                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-4, -8)), ModContent.ProjectileType<秘银能量>(), Projectile.damage / 2, 0, Projectile.owner, target.whoAmI);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, Projectile.height / 2);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height - Projectile.height / 2);
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, RO, Origia, Projectile.scale, sprite, 0f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                RO = Projectile.oldRot[i];
                if (Projectile.velocity.X < 0)
                {
                    RO -= Rotation2;
                }
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i]+Projectile.Size/2 - Main.screenPosition, null, lightColor*0.5F * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), RO, Origia, Projectile.scale, sprite, 0f);
            }
            return false;
        }
    }
    public class 秘银能量 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1250;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
        }
        int T = -1;
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void AI()
        {
            if (T == -1)
            {
                int P = Player.FindClosest(Projectile.Center, 1, 1);
                if (Main.player[P].活着())
                {
                    T = P;
                }
                else
                {
                    Projectile.ai[0]++;
                    if (Projectile.ai[0] > 3)
                    {
                        Projectile.Kill();
                    }
                }
            }
            else
            {
                Player player = Main.player[T];
                Projectile.ai[1]++;
                if (player.活着())
                {
                    Color color = new Color(89, 194, 201, 0);
                    if (Main.rand.NextBool(2))
                    {
                        color = new Color(157, 210, 114,0);
                    }
                    int Type = ModContent.DustType<光球粒子>();
                    Dust dust = Main.dust[NewDust(Projectile.Center - Projectile.Size / 4, Projectile.width / 2, Projectile.height / 2, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, color)];
                    dust.noGravity = true;
                    dust.scale = 0.5F;
                    dust.velocity = Vector2.Zero;
                    Vector2 vector = (player.Center - Projectile.Center).PerfectNormalize() * 5;
                    Projectile.velocity = (Projectile.velocity * 20 + vector) / (21);
                    if (player.getRect().Intersects(Projectile.getRect()))
                    {
                        player.AddBuff(ModContent.BuffType<迅捷之力>(), 300);
                        Projectile.Kill();
                    }
                }
                else
                {
                    T = -1;
                    Projectile.Kill();
                }
            }

        }
        public override void OnKill(int timeLeft)
        {
            if (T != -1)
            {
                if (Main.netMode != 2)
                {
                    Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Melee/FlyingKnife/秘银Buff").Value;
                    Color[] colors = DDHelper.GetColors(texture);
                    for (int i = 0; i < colors.Length; i+=2)
                    {
                        float x = i % texture.Width;
                        float y = i / texture.Width;
                        if (colors[i] != new Color(0, 0, 0, 0))
                        {
                            Color color = new Color(89, 194, 201, 0);
                            if (Main.rand.NextBool(2))
                            {
                                color = new Color(157, 210, 114, 0);
                            }
                            Dust dust = Main.dust[NewDust(Projectile.Player().Center - new Vector2(-10, 50) + (new Vector2(x, y) - new Vector2(texture.Width / 1.5F, texture.Height / 1.5F)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, color, 0.25f)];
                            dust.customData = 0.75F;
                            dust.noGravity = true;
                            dust.velocity = dust.position - (Projectile.Player().Center-new Vector2(-10,50));
                            dust.velocity *= 0.05F;
                        }
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}