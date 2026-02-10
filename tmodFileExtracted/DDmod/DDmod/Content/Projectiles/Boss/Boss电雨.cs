
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss电雨 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft =300;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;

        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void AI()
        {
            Projectile.scale = Projectile.ai[2];
            Projectile.ProjScaleChange();
            if(Projectile.alpha>0)
            {
                Projectile.alpha -= 20;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 4 == 0)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != 2)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(-Projectile.DProj().vector[0].PerfectNormalize(), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 4.2F);
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 226)];
                    dust.velocity = projDirection;
                    dust.alpha = 100;
                    dust.scale = Projectile.scale* Main.rand.NextFloat(0.25F, 0.5F);
                }

                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                sound.Pitch = 1f;
                sound.Volume = .25f;
                sound.MaxInstances = 20;
                PlaySound(sound, Projectile.position);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(144, 180);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.DProj().vector[0] = oldVelocity;
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Color color = Projectile.GetAlpha(Color.White);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);

            return false;
        }
    }
}
