using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Magic;
using DDmod.NoContent.Config;
using DDmod.Players;

namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class 晶凝蓄能炮Proj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            int Damage = Projectile.damage;
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            if (!Projectile.DProj().Bool[1])
            {
                Projectile.localAI[2] = (player.Dplayer().MouseWorld - player.Center).ToRotation();
                Projectile.DProj().Bool[1] = true;
            }
            Projectile.velocity = Projectile.localAI[2].ToRotationVector2();
            if (!channeling)
            {
                if (Projectile.ai[2] > 0)
                {
                    if (Projectile.ai[2] <= 10)
                    {
                        Projectile.localAI[2] += -0.4F * player.direction * (1 - Projectile.ai[2] / 10) * (Projectile.ai[0]/ player.ActiveItem().useAnimation);
                    }
                    else
                    {
                        Projectile.localAI[2] += -0.1F * player.direction * (1 - Projectile.ai[2] / 10) * (Projectile.ai[0] / player.ActiveItem().useAnimation);

                    }
                }
                Projectile.ai[2]++;
            }
            else
            {
                DDHelper.RotateSpeed(ref Projectile.localAI[2], (player.Dplayer().MouseWorld - player.Center).ToRotation() + Projectile.ai[2], 0.05F);
            }

            Projectile.HoldProj(player, 24, 0, Projectile.localAI[2].ToRotationVector2(), 0, 0, Projectile.ai[2]<30,direction: Projectile.ai[2] ==0);

            Projectile.damage= Damage;

            if (!Projectile.DProj().Bool[0])
            {
                if(Main.myPlayer==Projectile.owner)
                {
                    NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center,Projectile.velocity, ModContent.ProjectileType<RangedArousalHeart>(),Projectile.damage,Projectile.knockBack,-1,-1,Projectile.damage);
                }
                Projectile.DProj().Bool[0] = true;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public float T1 = 0;
        public float T2 = 0.33f;
        public float T3 = 0.66f;
        public float H2 = 1;
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            if (Projectile.Player().direction==-1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation+MathHelper.Pi, texture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);

            }

            return false;
        }
    }
}