using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Magic.Gun;

namespace DDmod.Content.Projectiles.Summon.Minions.Fort
{
    public class 绿岩工程炮台 : Summons
    {
        public override void SetDefault()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            shoot = ModContent.ProjectileType<S绿岩激光束>();
            shootSpeed = 5;
            AttackSpeed = 30;
            IgnoreTile = false;
            Projectile.sentry = true;
            Projectile.tileCollide = true;
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
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.Y = 0;
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool MobileAI()
        {
            Projectile.velocity.X=0;
            if(Projectile.velocity.Y<10)
            {
                Projectile.velocity.Y += 0.25F;
            }
            if(Projectile.spriteDirection==1)
            {
                Projectile.spriteDirection = 0;
            }
            return false;
        }
        public override bool AttackAI()
        {
            Projectile.tileCollide = true;
            Vector2 direction = Vector2.Zero;
            if (npc != null)
            {
                direction = npc.Center - (Projectile.Center - new Vector2(0, 16));
            }
            if(Projectile.frameCounter<80)
            {
                Projectile.frameCounter++;
                Projectile.frame = Projectile.frameCounter / 8;
                return false;
            }
            if (target)
            {
                if (direction.X < 0)
                {
                    Projectile.spriteDirection = 2;
                }
                else
                {
                    Projectile.spriteDirection = 0;
                }
                Projectile.rotation = direction.ToRotation();
                Projectile.ai[0]++;
                DistanceNPC = direction.Length();

                if (Projectile.ai[0] >= AttackSpeed)
                {
                    SoundStyle sound = SoundID.Item158;
                    sound.Pitch = 0;
                    PlaySound(sound, Projectile.position);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 16), direction.PerfectNormalize() * shootSpeed, shoot, Projectile.damage, Projectile.knockBack, Projectile.owner, 1);
                        Projectile.netUpdate = true;
                    }
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
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.frameCounter < 80)
            {
                Rectangle rectangle = new Rectangle(0, texture.Height / 10 * Projectile.frame, texture.Width / 2, texture.Height / 10);

                Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width / 2, Projectile.height)-Main.screenPosition, rectangle, lightColor, 0, new Vector2(rectangle.Width/2, rectangle.Height-2), Projectile.scale, 0, 0f);
            }
            else
            {
                Rectangle rectangle = new Rectangle(texture.Width/2, texture.Height / 10, texture.Width / 2, texture.Height / 10);
                Main.spriteBatch.Draw(texture, Projectile.position +new Vector2(Projectile.width/2,Projectile.height)- Main.screenPosition, rectangle, lightColor, 0, new Vector2(rectangle.Width / 2, rectangle.Height-2), Projectile.scale, 0, 0f);
                rectangle.Y = 0;
                Main.spriteBatch.Draw(texture, Projectile.Center+new Vector2(0,-13) - Main.screenPosition, rectangle, lightColor, Projectile.rotation, rectangle.Size()/2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            }
            return false;
        }
    }
    public class S绿岩激光束 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 15;

        }
        public override void AI()
        {
            if (Projectile.ai[1]==4)
            {

                int a = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<星光粒子>(), 0, 0, 100, new Color(100, 255, 100, 0), 3.5F);
                Main.dust[a].customData = 4002;
                Main.dust[a].velocity = Vector2.Zero;
            }
            if (Projectile.ai[1]++ > 3)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(100, 255, 100, 0), Projectile.ai[0] * 2)];
                dust.velocity = Vector2.Zero;
                dust.rotation = Projectile.velocity.ToRotation();
                dust.customData = 1F;
            }
            Projectile.DProj().Magnification = Projectile.ai[0];
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 30; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(100, 255, 100, 0),2);
                Main.dust[dust].velocity = Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(1, 4);
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
                Main.dust[dust].customData = 1 + Main.dust[dust].DustAI(0);
            }
        }
    }
}