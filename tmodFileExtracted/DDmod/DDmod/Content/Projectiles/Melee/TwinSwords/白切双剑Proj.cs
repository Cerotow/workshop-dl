using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Players;
using System.Reflection;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.TwinSwords
{
    public class 白切双剑Proj : 双刀
    {
        public override void Defaults()
        {
            Projectile.width = 26;
            Projectile.height = 40;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 42;
            EffectLength = 14;
            HandheldOffset = 30;
        }
        public override Color color => new Color(200, 200, 200,50);
        int r = 0;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            SpecialAttack(210);
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;


            if (Projectile.DProj().Back == -1)
                Projectile.DProj().Bool[1] = true;

            if (Projectile.DProj().Bool[0] && Projectile.DProj().Back == 1)
            {
                Projectile.DProj().Bool[1] = true;
                Projectile.DProj().Bool[0] = false;
            }
            Projectile.netUpdate = true;
            short Jl = (short)(20 - (player.Center - target.Center).Length() / 10);
            if (Jl < 0)
            {
                Time = 0;
            }
            Time += Jl;
        }

        public override void OnKill(int timeLeft)
        {
        }
        public override void SpecialShoot()
        {
            int a = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0] * 4, ModContent.ProjectileType<SwordWave>(), Projectile.damage, 0.2f, Projectile.owner);
            Main.projectile[a].DProj().color = color;
            Main.projectile[a].timeLeft = 200;
        }
        public override void Shoot()
        {
            if (Projectile.ai[2] == 2)
            {
                if (Projectile.DProj().Back == 1)
                {
                    int a = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0] * 4, ModContent.ProjectileType<SwordWave>(), Projectile.damage, 0.2f, Projectile.owner);
                    Main.projectile[a].DProj().color = color;
                    Main.projectile[a].timeLeft = 200;
                }
            }
        }
    }
}