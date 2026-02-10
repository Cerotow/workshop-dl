using DDmod.Players;
using Terraria;

namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class 流星炮Proj : Gun
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            NoAmmo = true;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => 0;
        public override int GunRecoilMaxTime => 12;
        public override void AI()
        {
            if (Projectile.frame > 0)
            {
                if (Projectile.frameCounter++ > 4)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if(Projectile.frame>=4)
                {
                    Projectile.frame = 0;
                }
            }
        }
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            PlaySound(new SoundStyle(DDHelper.Sound(1, "大炮")), Projectile.position);
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos+ Projectile.velocity.PerfectNormalize() * 20, Projectile.velocity.PerfectNormalize() * speed, ModContent.ProjectileType<陨石弹>(), Damage, KnockBack, Projectile.owner);
            
            Projectile.frame = 1;
            player.velocity -= Projectile.velocity.PerfectNormalize() * 4;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = new Rectangle(0, texture.Height / 4 * Projectile.frame, texture.Width, texture.Height / 4);
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation + MathHelper.Pi, rectangle.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
                Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation + MathHelper.Pi, rectangle.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
            }

            return false;
        }
    }
}