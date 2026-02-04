using Terraria;
using Terraria.ModLoader;

namespace UpgradedAccessories.Projectiles {

    public class HandClap : ModProjectile {
        public override void SetDefaults() {
            projectile.width = 174;
            projectile.height = 210;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 20;
            projectile.thrown = true;
        }

        public override bool CanDamage() {
            return false;
        }

        public override void AI() {
            if(projectile.timeLeft >= 10) {
                var npc = Main.npc[(int)projectile.ai[0]];
                if(!npc.CanBeChasedBy()) {
                    projectile.Kill();
                    return;
                }
                projectile.Center = npc.Center;
                projectile.scale = (20 - projectile.timeLeft) / 10f;
                if(projectile.timeLeft == 10) {
                    if(Main.rand.Next(200) == 0) Main.PlaySound(UpgradedAccessories.clapSpecial, projectile.Center);
                    else Main.PlaySound(UpgradedAccessories.clap, projectile.Center);
                    if(projectile.owner == Main.myPlayer) {
                        var owner = Main.player[projectile.owner];
                        owner.ApplyDamageToNPC(Main.npc[(int)projectile.ai[0]], projectile.damage, projectile.knockBack, 0, Main.rand.Next(100) < owner.thrownCrit);
                    }
                }
            } else {
                projectile.alpha += 25;
            }
        }
    }
}