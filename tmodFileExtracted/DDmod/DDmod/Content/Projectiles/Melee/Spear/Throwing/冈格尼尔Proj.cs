using DDmod.Players;

namespace DDmod.Content.Projectiles.Melee.Spear.Throwing
{
    public class 冈格尼尔Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            旋转 = -1.4f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            //大小加成
            Projectile.ProjScale();
            bool canShoot = player.channel && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;

            
            if ((canShoot || 旋转 < 0.6f)&& 手==0)
            {
                player.heldProj = Projectile.whoAmI;
                if (旋转 < 0.6f)
                {
                    旋转 += 0.05f;
                }
                Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);
                Projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector).PerfectNormalize();
                Vector2 Pvelocity = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize(), 0, default);
                float v = 0;
                if (player.direction == -1) v = 3.14f;
                Projectile.HoldProj(player, 0, Pvelocity.ToRotation(), new Vector2(0f, -1), MathHelper.Pi, 0, true, 0, false);
                手旋转 = new Vector2(0, -1).RotatedBy(Pvelocity.ToRotation() - 旋转 * player.direction).ToRotation();
                player.itemRotation = 手旋转;
                player.PlayerAction().PlayerArmRotation(手旋转 - MathHelper.PiOver2 * player.direction, 0);
                Projectile.position -= new Vector2(0, 10).RotatedBy(player.itemRotation + MathHelper.PiOver2 + v) - Projectile.DProj().vector[0].PerfectNormalize() * 28;
                player.ChangeDir(Projectile.DProj().vector[0].X >= 0 ? 1 : -1);
                Projectile.timeLeft = 300;
                player.itemTime = 2;
                player.itemAnimation = 2;
                if (player.velocity.X == 0)
                {

                    player.PlayerAction().PlayerArmRotationBack(-0.1F, 0);
                }
            }
            else
            {
                if (旋转 < 0.6f)
                {
                    Projectile.Kill();
                    return;
                }
                Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize() * 20;
                if (手 == 0)
                {
                    player.itemTime = 26;
                    player.itemAnimation = 26;
                    player.PlayerAction().ThrowingProj(26, 6, 0.5F, 手旋转);
                    手++;
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<冈格尼尔弹幕>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                if (player.itemAnimation <= 0)
                {
                    Projectile.Kill();

                }
            }
            Projectile.netUpdate = true;
        }
        int 手;
        float 手旋转;
        float 旋转;
        public override void OnKill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(手>0)
            {
                return false;
            }
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2 = Glow.Value;
            SpriteEffects spriteEffects = 0;
            if (player.direction == -1)
            {
                spriteEffects = (SpriteEffects)1;
            }
            float ro = Projectile.rotation + MathHelper.PiOver4;
            if (player.direction == -1)
            {
                ro = Projectile.rotation - MathHelper.PiOver4;
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, new Color(255, 30, 30,255), ro, texture2.Size()/2, Projectile.scale/4, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), ro, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, new Color(255, 30, 30, 255), ro, texture2.Size() / 2, Projectile.scale/4, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), ro, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            }
            return false;
        }
    }
}