
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Players;
using System.Reflection;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.TwinSwords
{
    public class 叶木双剑Proj : 双刀
    {
        public override void Defaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 36;
            EffectLength = 10;
            HandheldOffset = 26;
        }
        public override Color color => new Color(127, 92, 69,180);
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            SpecialAttack(100);
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
            short Jl = (short)(20 - (player.Center - target.Center).Length() / 10);
            if (Jl < 0)
            {
                Jl = 0;
            }
            Time += Jl;

            Projectile.netUpdate = true;
        }


        public override void OnKill(int timeLeft)
        {
        }
    }
}