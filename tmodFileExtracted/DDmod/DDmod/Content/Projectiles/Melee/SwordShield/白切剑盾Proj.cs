
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Players;
using System.Reflection;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.SwordShield
{
    public class 白切剑盾Proj : 剑盾
    {
        public override void Defaults()
        {
            Projectile.width = 26;
            Projectile.height = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 40;
            Projectile.extraUpdates = 6;
            EffectLength = 14;
            HandheldOffset = 26;
        }
        public override Color color => new Color(200, 200, 200,200);
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if(Projectile.DProj().Bool[0])
            {
                Projectile.damage *= 3;
                Projectile.knockBack *= 1.5f;
                Projectile.DProj().Magnification *= 3;
                Projectile.DProj().Bool[1] = true;
                Projectile.DProj().Bool[0] = false;
            }
            return true;
        }
        public override void Shoot()
        {
            if (Projectile.DProj().Bool[1])
            {
                int a = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0] * 8, ModContent.ProjectileType<SwordWave>(), Projectile.damage*3, 3f, Projectile.owner,-3,0.3F,0.8F);
                Main.projectile[a].DProj().color = color;
                Main.projectile[a].DProj().Magnification *= 9;
                Main.projectile[a].timeLeft = 400;
            }
            else
            {
                int a = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0] * 4, ModContent.ProjectileType<SwordWave>(), Projectile.damage/2, 1f, Projectile.owner,0F,0.3F);
                Main.projectile[a].DProj().color = color;
                Main.projectile[a].DProj().Magnification *= 0.5f;
                Main.projectile[a].timeLeft = 200;
            }
            }
        
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;


            Projectile.netUpdate = true;
        }


        public override void OnKill(int timeLeft)
        {
        }
    }
}