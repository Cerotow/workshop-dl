

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Projectiles {

    public class RevengeStar : ModProjectile {
        public override void SetStaticDefaults() {
            Main.projFrames[projectile.type] = 3;
        }
        public override void SetDefaults() {
            projectile.width = 60;
            projectile.height = 60;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 1;
            projectile.light = 0.3f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            projectile.frame = (int)projectile.ai[0];
            return base.PreDraw(spriteBatch, lightColor);
        }

        public override void AI() {
            if(projectile.position.Y > projectile.ai[1]) {
                projectile.tileCollide = true;
            }
            if(projectile.soundDelay == 0) {
                projectile.soundDelay = 20 + Main.rand.Next(40);
                Main.PlaySound(SoundID.Item60, projectile.Center);
            }
            projectile.rotation = projectile.velocity.ToRotation();
            for(int i = 0; i < 6; i++) {
                var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, -projectile.velocity.X, -projectile.velocity.Y, Alpha: 100, Scale: 1.5f);
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft) {
            projectile.position.X += projectile.width / 2f;
            projectile.position.Y += projectile.height / 2f;
            projectile.width = 200;
            projectile.height = 200;
            projectile.position.X -= projectile.width / 2f;
            projectile.position.Y -= projectile.height / 2f;
            projectile.penetrate = -1;
            projectile.Damage();
            Main.PlaySound(SoundID.Item14, projectile.Center);


            var effectSize = 80;
            var effectPosition = projectile.Center - new Vector2(effectSize);

            for(int i = 0; i < 70; i++) {
                var dust = Dust.NewDustDirect(effectPosition, effectSize, effectSize, 6, Alpha: 100, Scale: 3);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust = Dust.NewDustDirect(effectPosition, effectSize, effectSize, 6, Alpha: 100, Scale: 2);
                dust.velocity *= 2f;
                if(i < 3) {
                    var velocityMul = 0.33f;
                    if(i == 1) velocityMul = 0.66f;
                    if(i == 2) velocityMul = 1f;
                    var gore = Main.gore[Gore.NewGore(projectile.Center, default, Main.rand.Next(61, 64))];
                    gore.velocity *= velocityMul;
                    gore.velocity.X += 1;
                    gore.velocity.Y += 1;
                    gore = Main.gore[Gore.NewGore(projectile.Center, default, Main.rand.Next(61, 64))];
                    gore.velocity *= velocityMul;
                    gore.velocity.X -= 1;
                    gore.velocity.Y += 1;
                    gore = Main.gore[Gore.NewGore(projectile.Center, default, Main.rand.Next(61, 64))];
                    gore.velocity *= velocityMul;
                    gore.velocity.X += 1;
                    gore.velocity.Y -= 1;
                    gore = Main.gore[Gore.NewGore(projectile.Center, default, Main.rand.Next(61, 64))];
                    gore.velocity *= velocityMul;
                    gore.velocity.X -= 1;
                    gore.velocity.Y -= 1;
                }
                if(i < 40) {
                    dust = Dust.NewDustDirect(effectPosition, effectSize, effectSize, 31, Alpha: 100, Scale: 2);
                    dust.velocity *= 3f;
                    if(Main.rand.NextBool()) {
                        dust.scale *= 0.5f;
                        dust.fadeIn = 1 + Main.rand.Next(10) * 0.1f;
                    }
                }
            }



        }
    }






    /*public class RevengeStarBlue : ModProjectile {
        public override void SetDefaults() {
            projectile.width = 60;
            projectile.width = 60;
            projectile.tileCollide = false;
            projectile.friendly = true;
        }

        public override void AI() {
            
        }
    }

    public class RevengeStarRed : ModProjectile {
        public override void SetDefaults() {
            projectile.width = 60;
            projectile.width = 60;
            projectile.tileCollide = false;
            projectile.friendly = true;
        }
        public override void AI() {

        }
    }

    public class RevengeStarYellow : ModProjectile {
        public override void SetDefaults() {
            projectile.width = 60;
            projectile.width = 60;
            projectile.tileCollide = false;
            projectile.friendly = true;
        }
        public override void AI() {

        }
    }*/
}