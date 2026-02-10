using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 钴蓝小刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            Projectile.penetrate = 1;
        }
        public override bool PreAI()
        {
            return base.PreAI();
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<钴蓝能量>(), Projectile.damage / 2, 0, Projectile.owner, target.whoAmI);
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 20; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                NewDust(Projectile.position, 1, 1, 48, projDirection.X, projDirection.Y, 0, default, 1f);
            }
        }
    }
    public class 钴蓝能量 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Cobalt energy");
           //DisplayName.AddTranslation(7, "钴蓝能量");
        }
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1250;
            Projectile.extraUpdates = 40;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
        }
        int T = -1;
        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI == T;
        }
        public override void AI()
        {
            if (T == -1)
            {
                NPC npc = NPCdirection.FindClosest(Projectile.Center, 250, true, Main.npc[(int)Projectile.ai[0]]);
                if (npc != null)
                {
                    T = npc.whoAmI;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            else
            {
                NPC npc = Main.npc[T];
                Projectile.ai[1]++;
                if (npc != null)
                {
                    if (Projectile.ai[1] % 6 == 0)
                    {
                        int Type = ModContent.DustType<光球粒子>();
                        Dust dust = Main.dust[NewDust(Projectile.Center - Projectile.Size / 4, Projectile.width / 2, Projectile.height / 2, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, new Color(46, 143, 189, 120))];
                        dust.noGravity = true;
                        dust.scale = 0.5F;
                        dust.velocity = new Vector2(Main.rand.NextFloat(0, 1)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                    }
                    Projectile.Chase(npc, 2, 800);
                }
                else
                {
                    Projectile.Kill();
                }
            }

        }
        public override void OnKill(int timeLeft)
        {
            if (T != -1)
            {
                for (int a = 0; a < 30; a++)
                {
                    int Type = ModContent.DustType<光球粒子>();
                    Dust dust = Main.dust[NewDust(Projectile.Center - Projectile.Size / 4, Projectile.width / 2, Projectile.height / 2, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, new Color(46, 143, 189, 120))];
                    dust.noGravity = true;
                    dust.scale = 0.8F;
                    dust.velocity = new Vector2(Main.rand.NextFloat(0, 2)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}