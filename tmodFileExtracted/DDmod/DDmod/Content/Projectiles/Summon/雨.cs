
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Summon
{
    public class 雨 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 90;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
            Projectile.alpha = 255;

        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
            Projectile.scale = Projectile.ai[2];
            if (Projectile.scale < 0.5F)
            {
                if (Projectile.timeLeft > 30)
                {
                    Projectile.timeLeft = 30;
                }
            }
            Projectile.ProjScaleChange();
            if (Projectile.timeLeft < 20)
            {
                Projectile.alpha = (int)(255 * (1-Projectile.timeLeft / 20F));
            }
            else
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 40;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.alpha <= 100)
            {
                int num502 = Dust.NewDust(Projectile.Center - new Vector2(4), 0, 0, 154);
                Main.dust[num502].position.X -= 2f;
                Main.dust[num502].alpha = 38;
                Dust dust2 = Main.dust[num502];
                dust2.velocity *= 0.1f;
                dust2 = Main.dust[num502];
                dust2.velocity += -Projectile.oldVelocity * 0.25f;
                Main.dust[num502].scale = 0.95f;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(144, 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.DProj().vector[0] = oldVelocity;
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Color color = Projectile.GetAlpha(lightColor);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height - 2), Projectile.scale, 0, 0f);

            return false;
        }
    }
}