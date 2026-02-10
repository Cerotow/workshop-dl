using DDmod.Players;

namespace DDmod.Content.Projectiles.Melee.Spear.Throwing
{
    public abstract class 投矛 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
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
            Player player = Main.player[Projectile.owner];
            return 旋转 >= 0.6f;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            //大小加成
            Projectile.ProjScale();
            bool canShoot = player.channel && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;
            if(player.controlUseTile)
            {
                canShoot = false;
            }
            if ((canShoot || 旋转 < 0.6f)&& 手==0)
            {
                if (player.dead)
                {
                    Projectile.Kill();
                }
                player.heldProj = Projectile.whoAmI;
                if (旋转 < 0.6f)
                {
                    旋转 += 0.10f;
                }
                //确定方向和位置
                Vector2 Pvelocity = (player.Dplayer().MouseWorld - player.Center).PerfectNormalize();
                if((player.Dplayer().MouseWorld - player.Center).Length()>40)
                {
                    Pvelocity = (player.Dplayer().MouseWorld - Projectile.Center).PerfectNormalize();
                }
                Projectile.DProj().vector[0] = Pvelocity;

                float v = 0;
                if (player.direction == -1) v = 3.14f;

                if (player.mount.Active)
                {
                    Projectile.Center = player.RotatedRelativePoint(player.MountedCenter - new Vector2(4 * player.direction, 0) + (player.itemRotation + v).ToRotationVector2() * 14, reverseRotation: false, addGfxOffY: false) + new Vector2(0, player.gfxOffY) + Pvelocity * 26;

                }
                else
                {
                    Projectile.Center = player.MountedCenter - new Vector2(4*player.direction, 0) + (player.itemRotation+ v).ToRotationVector2()*14 + new Vector2(0, player.gfxOffY) + Pvelocity * 26;

                }

                Projectile.rotation = Pvelocity.ToRotation() + MathHelper.PiOver2;
                手旋转 = new Vector2(0, -1).RotatedBy(Pvelocity.ToRotation() - 旋转 * player.direction).ToRotation();
                player.itemRotation = 手旋转;
                player.PlayerAction().PlayerArmRotation(手旋转 - MathHelper.PiOver2 * player.direction, 0);

                player.ChangeDir(Pvelocity.X >= 0 ? 1 : -1);

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
                Projectile.ownerHitCheck = false;
                Projectile.tileCollide = true;
                Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize() * 7;
                Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;
                if (手 == 0)
                {
                    player.itemTime = 26;
                    player.itemAnimation = 26;
                    player.PlayerAction().ThrowingProj(26, 6, 0.5F, 手旋转);
                    手++;
                }
            }
            Projectile.netUpdate = true;
        }
        public int 手;
        public float 手旋转;
        public float 旋转;
        public override void OnKill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            SpriteEffects spriteEffects = 0;
            if (player.direction == -1)
            {
                spriteEffects = (SpriteEffects)1;
            }
            float ro = Projectile.rotation + MathHelper.PiOver4;
            if (player.direction == -1)
            {
                ro = Projectile.rotation - MathHelper.PiOver4;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), ro, new Vector2(texture.Width-Projectile.width / 2, Projectile.height / 2), Projectile.scale, spriteEffects, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), ro, new Vector2(Projectile.width/2,Projectile.height/2), Projectile.scale, spriteEffects, 0f);
            }
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White * 0.5F, 0, Vector2.Zero, Projectile.Size / 2, 0, 0f);
            return false;
        }
    }
}