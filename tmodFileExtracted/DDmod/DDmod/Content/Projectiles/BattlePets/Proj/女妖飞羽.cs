using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Boss;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.BattlePets.Proj
{
    public class 女妖飞羽 : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] =8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 500;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<飞羽破甲>(),600);
        }
        public override void OnKill(int timeLeft)
        {

            NewDustChange2(20, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, false, 0.2F, 0.5F, 0, new Color(155, 138, 153, 255));
            NewDustChange2(10, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 3, 6, true, 1F, 2, 0, new Color(155, 138, 153, 255));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, 0, 0f);

            for (int a = 0; a < Projectile.oldPos.Length; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, null, lightColor * 0.5f * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), Projectile.oldRot[a], texture.Size() / 2, Projectile.scale, 0, 0);
            }
            return false;
        }
    }
}