
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using Terraria;

namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class 绿岩箭Proj : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
        }

        public override void SetStaticDefaults()
        {
            //ProjectileID.Sets.DontCancelChannelOnKill[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[2]++;
            if (Projectile.ai[2]>=30)
            {
                
                for (int a = 0; a < 20; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(101, 255, 101, 50), 2f);
                    Main.dust[dust].velocity = Projectile.oldVelocity.RotatedBy(Main.rand.NextFloat(-0.4F,0.4F)) * Main.rand.NextFloat(0.1F, 0.5f);
                    Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].customData = 1 + Main.dust[dust].DustAI(0);
                }
                if (Main.myPlayer==Projectile.owner)
                {
                    NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center,Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0F,0.3F)),ModContent.ProjectileType<绿岩箭幻象>(),Projectile.damage/3,0,-1,0,0.33F);
                    NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center,Projectile.velocity.RotatedBy(Main.rand.NextFloat(0F, 0.3F)), ModContent.ProjectileType<绿岩箭幻象>(),Projectile.damage/3,0,-1,0,0.33F);
                }
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 20; A++)
            {
                int a = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<绿岩粒子>(), 0, 0, 100);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Color color = new Color(101, 255, 101, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], new Vector2(texture.Width) / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center-Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width) / 2, Projectile.scale, 0, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.Center-Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width) / 2, Projectile.scale, 0, 0f);


            return false;
        }
    }
    public class 绿岩箭幻象 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            //ProjectileID.Sets.DontCancelChannelOnKill[Projectile.type] = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.Track(600,20,18,0);
            Projectile.ai[0]--;
            Projectile.DProj().Magnification = Projectile.ai[1];
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 20; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(51, 255, 51, 50), 1f);
                Main.dust[dust].velocity = -Projectile.oldVelocity.RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f);
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
                Main.dust[dust].customData = 1 + Main.dust[dust].DustAI(0);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] >= 0)
            {
                return false;
            }

                Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            Main.spriteBatch.Draw(texture, Projectile.Center-Main.screenPosition, null, Color.White * Projectile.ai[1], Projectile.rotation, new Vector2(texture.Width) / 2, Projectile.scale, 0, 0f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Color color = new Color(101, 255, 101, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], new Vector2(texture.Width) / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
            }

            return false;
        }
    }
}
