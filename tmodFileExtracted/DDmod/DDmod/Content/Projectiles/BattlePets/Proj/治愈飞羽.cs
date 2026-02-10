using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Boss;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.BattlePets.Proj
{
    public class 治愈飞羽 : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] =10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1200;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;
            Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
            if (!player.dead)
            {
                if (Projectile.DProj().track > 60)
                {
                    Vector2 Govector = player.position - Projectile.Center;
                    Projectile.velocity = (Projectile.velocity * 20 + Govector.PerfectNormalize() * 12) / 21;
                }
                if (player.getRect().Intersects(Projectile.getRect()))
                {
                    if (player.whoAmI == Main.myPlayer)
                        player.Heal(player.statLifeMax2 / 100 + 1);
                    Projectile.Kill();
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange2(20, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, false, 0.2F, 0.5F, 0, new Color(255, 238, 53, 100));
            NewDustChange2(10, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 3, 6, true, 1F, 2, 0, new Color(255, 238, 53, 100));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, 0, 0f);

            lightColor = new Color(255, 238, 53,50);
            for (int a = 0; a < Projectile.oldPos.Length; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, null, lightColor * 0.5f * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), Projectile.oldRot[a], texture.Size() / 2, Projectile.scale, 0, 0);
            }
            return false;
        }
    }
}