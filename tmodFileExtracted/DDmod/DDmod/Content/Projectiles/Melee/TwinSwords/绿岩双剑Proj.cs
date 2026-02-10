using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Players;
using System.Reflection;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.TwinSwords
{
    public class 绿岩双剑Proj : 双刀
    {
        public override void Defaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 42;
            EffectLength = 14;
            HandheldOffset = 30;
            range = 1.5F;
        }
        public override Color color => new Color(55, 212,116,50);
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[2] == 2)
            {
                range = MathHelper.TwoPi*0.35F;
            }
            SpecialAttack(300);
            return true;
        }
        bool R = false;
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
            short Jl = (short)(20 - (player.Center - target.Center).Length()/10);
            if(Jl < 0)
            {
                Time = 0;
            }
            Time += Jl;
            if(!R)
            {
                NPC npc = NPCdirection.FindClosest(target.Center, 300, false, target);
                if (npc != null)
                {
                    int T = NewProjectile(player.GetSource_FromAI(), target.Center, Vector2.Zero, ModContent.ProjectileType<绿岩能量>(), hit.SourceDamage, 0, -1, npc.whoAmI);
                    Main.projectile[T].DamageType = DamageClass.Melee;
                    Main.projectile[T].DProj().NPCW = new List<byte>() { (byte)target.whoAmI };
                }
                R = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
        }
        public override void SpecialShoot()
        {
            for (int R = -1; R <= 1; R++)
            {
                int a = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].RotatedBy(R * 0.4F) * 4, ModContent.ProjectileType<SwordWave>(), Projectile.damage, 0.2f, Projectile.owner);
                Main.projectile[a].DProj().color = color;
                Main.projectile[a].scale = 0.3F;
                if (R == 0)
                {

                    Main.projectile[a].scale = 0.6F;
                }
            }
        }
        public override void Shoot()
        {
            if (Projectile.ai[2] == 2)
            {
                if (Projectile.DProj().Back == -1)
                {
                    for (int R = -1; R <= 1; R++)
                    {
                        int a = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].RotatedBy(R*0.6F) * 2, ModContent.ProjectileType<SwordWave>(), Projectile.damage, 0.2f, Projectile.owner);

                        Main.projectile[a].extraUpdates  =8;
                        Main.projectile[a].DProj().color = color;
                        Main.projectile[a].scale = 0.2F;
                        if (R == 0)
                        {
                            Main.projectile[a].scale = 0.4F;
                        }
                    }
                }
            }
        }
    }
}