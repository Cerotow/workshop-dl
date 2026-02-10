using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 精金飞刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
        }
        public override void PostAI()
        {
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 20; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                NewDust(Projectile.Center, 1, 1, 50, projDirection.X, projDirection.Y,0,default,0.75f);
            }

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            NewProjectile(Projectile.GetSource_FromThis(), target.Center,new Vector2(0,-5).RotatedBy(Main.rand.NextFloat(-0.3F,0.3F)), ModContent.ProjectileType<力量之魂>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, target.whoAmI, 1);
        }
    }
    public class 力量之魂 : ModProjectile
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
            Projectile.extraUpdates = 4;
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
                int P = Projectile.owner;
                if (Main.player[P].活着())
                {
                    T = Projectile.owner;
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
                    Color color = new Color(128, 26, 52, 0);
                    int Type = ModContent.DustType<光球粒子>();
                    Dust dust = Main.dust[NewDust(Projectile.Center - Projectile.Size / 4, Projectile.width / 2, Projectile.height / 2, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, color)];
                    dust.noGravity = true;
                    dust.scale = 0.6F;
                    dust.velocity = Vector2.Zero;
                    Vector2 vector = (player.Center - Projectile.Center).PerfectNormalize() * 5;
                    Projectile.velocity = (Projectile.velocity * 20 + vector) / (21);
                    if (player.getRect().Intersects(Projectile.getRect()))
                    {
                        player.AddBuff(ModContent.BuffType<Buffs.PlayerBuffs.力量之魂>(), 300);
                        if (player.Aplayer().精金力量 < 40)
                            player.Aplayer().精金力量 += 2;
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
                    Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Melee/FlyingKnife/精金buff").Value;
                    Color[] colors = DDHelper.GetColors(texture);
                    for (int i = 0; i < colors.Length; i ++)
                    {
                        float x = i % texture.Width;
                        float y = i / texture.Width;
                        if (colors[i] != new Color(0, 0, 0, 0))
                        {
                            Color color = new Color(128, 26, 52, 0);
                            Dust dust = Main.dust[NewDust(Projectile.Player().Center - new Vector2(-0, 50) + (new Vector2(x, y) - new Vector2(texture.Width / 1.5F, texture.Height / 1.5F)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, color, 0.25f)];
                            dust.customData = 0.75F;
                            dust.noGravity = true;
                            dust.velocity = dust.position - (Projectile.Player().Center - new Vector2(-0, 50));
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