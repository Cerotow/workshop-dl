
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic
{
    public class 魔法叶 : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft =180;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.ArmorPenetration = 15;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 255;

        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.DProj().vector[0] = player.Dplayer().MouseWorld;
            /*
            if(DDHelper.SpecifyDirection(Projectile.velocity.ToRotation(),( Projectile.DProj().vector[0] - Projectile.Center).ToRotation(),0.2F))
            {
                Projectile.DProj().track = -114514;
            }*/
            if (Projectile.DProj().track==60)
            {
                Vector2 vector = (Projectile.DProj().vector[0] - Projectile.Center).PerfectNormalize() * 15;
                /*
                Projectile.velocity = (Projectile.velocity * 20 + vector) / (20 + 1);*/
                Projectile.velocity = vector;
                for (int i = 0; i < 30; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 2.2F);
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<光球粒子>())];
                    dust.velocity = projDirection;
                    dust.color = new Color(0, 255, 0, 155);
                    dust.noGravity = true;
                    dust.alpha = 100;
                    dust.scale = 1.2f;
                    dust.customData = 2;
                }

            }
            //Projectile.Track(500,12,15,40);
            Projectile.ProjScaleChange();
            if (Projectile.alpha>0)
            {
                Projectile.alpha -= 5;
            }
                Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 40)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.2f;
            }
            for (int i = 0; i < 15; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<光球粒子>())];
                dust.velocity = projDirection;
                dust.color = new Color(0,255,0,155);
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 0.8f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Glow.Value;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Color color = new Color(0, 255, 0, 0)*(1-(float)Projectile.alpha/255);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
            }
            texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center- Main.screenPosition, null, Color.White * (1 - (float)Projectile.alpha / 255), Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            return false;
        }
    }
}
