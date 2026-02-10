using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.GeneralProj;
using Terraria;
using Terraria.Graphics.Shaders;


namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class 大地之锋Proj : ModProjectile
    {
        public static Asset<Texture2D> texture;
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 112;
            Projectile.aiStyle = 161;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 360;
            Projectile.hide = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.MeleeProj().DaggerDashDistance = 50;
            Projectile.localAI[2] = 0;
        }
        public override void PostAI()
        {
            Projectile.position += Projectile.velocity.PerfectNormalize() * 18;
            Player player = Main.player[Projectile.owner];
            if(Projectile.DProj().Bool[0])
            {
                Projectile.MeleeProj().ExtraLength = 48;
            }
            if (Projectile.localAI[2]<1)
            {
                Projectile.localAI[2] += 0.1F;
            }
            if (Projectile.ai[2] < 15)
            {
                Projectile.DProj().Times[0] += 3F;
            }
            if (Projectile.owner == Main.myPlayer)
            {
                
                if (Projectile.DProj().Bool[4])
                {
                    Projectile.DProj().Times[0] = 0;
                    /*
                    Vector2 vector = Projectile.velocity.PerfectNormalize() * 2;
                    
                    Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, vector*4, ModContent.ProjectileType<MeleeArousalHeart>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 1)];
                    Projectile.DProj().Bool[4] = false;
                    Projectile.netUpdate = true;
                    */
                    Projectile.DProj().Bool[4] = false;
                    Projectile.localAI[2] = 0;
                    //Projectile.ai[1] = Main.rand.NextFloat(-0.6F, 0.6F);
                }
            }
            Projectile.extraUpdates = 3;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? CanDamage()
        {
            return base.CanDamage();
        }
        bool R = true;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.DProj().Bool[0]&& R)
            {
                R = false;
                byte[] LifeTextures =
        [
            0,0,1,1,1,0,1,1,1,0,0,
            0,1,0,0,0,1,0,0,0,1,0,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            0,1,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,0,0,1,0,0,
            0,0,0,1,0,0,0,1,0,0,0,
            0,0,0,0,1,0,1,0,0,0,0,
            0,0,0,0,0,1,0,0,0,0,0,

        ];
                for (int a = 0; a < LifeTextures.Length; a++)
                {
                    if (LifeTextures[a] == 1)
                    {
                        Vector2 vector = new Vector2(a % 11, a / 11);
                        Vector2 velocity = (vector - (new Vector2(10) / 2));
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), target.Center, velocity , ModContent.ProjectileType<MeleeArousalHeart>(), (int)(Projectile.damage*0.2f), Projectile.knockBack, Projectile.owner, 0, 0, 0.2f)];
                    }
                }

            }
        }
        int A;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 Center;
            Center = Projectile.Center - Main.screenPosition;
            Texture2D Projtexture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture = DDTextures.Starlight3.Value;
            SpriteEffects sprite = 0;
            float ro = Projectile.rotation;
            if (Projectile.DProj().vector[0].X < 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
                ro += MathHelper.PiOver2;
            }
            Main.spriteBatch.Draw(Projtexture, Center, new Rectangle?(new Rectangle(0, 0, Projtexture.Width, Projtexture.Height)), lightColor, ro - MathHelper.PiOver4, new Vector2(Projtexture.Width / 2, Projtexture.Height / 2), Projectile.scale, sprite, 0f);

            Main.spriteBatch.Draw(texture, Center+Projectile.velocity.PerfectNormalize() * (4 + Projectile.DProj().Times[0]), null, new Color(255,100,100,0)* Projectile.localAI[2], Projectile.rotation, texture.Size()/2, Projectile.scale*1.25f * new Vector2(0.5F, 1.25F), sprite, 0f);
            Main.spriteBatch.Draw(texture, Center + Projectile.velocity.PerfectNormalize() * (4+ Projectile.DProj().Times[0]), null, new Color(255,100,100,0)* Projectile.localAI[2], Projectile.rotation, texture.Size()/2, Projectile.scale * 1.25f*new Vector2(0.5F,1.25F), sprite, 0f);

            return false;
        }
    }
}