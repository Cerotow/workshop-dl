using DDmod.Players;
using Terraria;

namespace DDmod.Content.Projectiles.Melee.Boomerang
{
    public class ShadowOfTheDevilProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.coldDamage = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
        }
        
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            //大小加成
            Projectile.ProjScale();
            bool canShoot = player.channel && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;

            //蓄力
            if ((canShoot|| Projectile.DProj().Times[4] < 1F) && !Projectile.DProj().Bool[4]&& Projectile.ai[0]==0)
            {
                player.heldProj = Projectile.whoAmI;
                if (Projectile.DProj().Times[4] < 1.9f)
                {
                    Projectile.DProj().Times[4] += (1- Projectile.DProj().Times[4] / 1.9f)/30;
                }
                Projectile.DProj().Times[0] = 8 * (Projectile.DProj().Times[4] / 1.9f);
                Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);
                Projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector).PerfectNormalize();
                Vector2 Pvelocity = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize(), 0, default);
                float v = 3.14f;
                if (player.direction == 1) v = -MathHelper.Pi;
                Projectile.HoldProj(player, 0, Pvelocity.ToRotation()-MathHelper.PiOver4* player.direction + MathHelper.PiOver2 - Projectile.DProj().Times[4]*player.direction, new Vector2(1f, 0), MathHelper.Pi, 0, true, 0, false);
                Projectile.damage = (int)(player.GetWeaponDamage(player.ActiveItem()) * (Projectile.DProj().Times[4] / 1.9f));
                Projectile.DProj().Times[3] = new Vector2(0, -1).RotatedBy(Pvelocity.ToRotation() - Projectile.DProj().Times[4] * player.direction).ToRotation();
                player.itemRotation = Projectile.DProj().Times[3];
                player.PlayerAction().PlayerArmRotation(Projectile.DProj().Times[3], 0);
                Projectile.position -= new Vector2(0, 26).RotatedBy(player.itemRotation + v);
                player.ChangeDir(Projectile.DProj().vector[0].X >= 0 ? 1 : -1);
                Projectile.timeLeft = 600;
                player.itemTime = 2;
                player.itemAnimation = 2;
                if (player.velocity.X == 0)
                {
                    player.PlayerAction().PlayerArmRotationBack(-0.1F, 0);
                }
                Projectile.direction = player.direction;
            }
            //放手后自由移动
            else
            {
                Projectile.extraUpdates = 3;
                Projectile.hide = false;
                if (!Projectile.DProj().Bool[4] && Projectile.ai[0] == 0)
                {
                    player.itemTime=player.itemAnimation = 26;
                    player.PlayerAction().ThrowingProj(player.itemTime, 6, 0.6f* (Projectile.DProj().Times[4]/ 1.9f), Projectile.DProj().Times[3]);
                    Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize() * Projectile.DProj().Times[0];
                    Projectile.DProj().Bool[4]=true;
                }
                if (Projectile.soundDelay == 0 && Projectile.ai[0] != 2)
                {
                    Projectile.soundDelay = 16;
                    SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
                }
                Player P = Main.player[Projectile.owner];
                Projectile.ai[1]++;
                if (Projectile.ai[0]!=0&&NPCdirection.HaveGoal(Projectile, 600, 30))
                {
                    Projectile.ai[1] -= 0.6f;
                }
                if (Projectile.ai[1] >= 120 && Projectile.ai[0] == 0)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        for (float i = -0.3f; i <= 0.3f; i += 0.3f)
                        {
                            if (i != 0)
                            {
                                Vector2 perturbedSpeed = Utils.RotatedBy(Projectile.velocity *1.5F, i, default);
                                int A = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, Projectile.type, (int)(Projectile.damage * 0.8f), Projectile.knockBack, Projectile.owner, 2f,60);
                                Main.projectile[A].alpha = 120;
                            }
                        }
                    }
                    Projectile.ai[0] = 1;
                }
                if ((Projectile.ai[1] > 120 && Projectile.ai[0]==0)|| Projectile.ai[1] > 120)
                {
                    Projectile.tileCollide = false;
                    Vector2 vector4 = Vector2.Subtract(P.Center, Projectile.Center);
                    DDHelper.RotateSpeed(ref Projectile.localAI[0], vector4.ToRotation(), Projectile.DProj().Times[0]* 0.01F);


                    Projectile.velocity = Projectile.localAI[0].ToRotationVector2() * Projectile.DProj().Times[0];

                    Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value2 = new Rectangle((int)P.position.X, (int)P.position.Y, P.width, P.height);
                    if (rectangle.Intersects(value2))
                    {
                        Projectile.Kill();
                    }
                }
                else
                {
                    Projectile.localAI[0] = Projectile.velocity.ToRotation();
                    if (Projectile.DProj().Times[0]==0)
                    {
                        Projectile.DProj().Times[0] = Projectile.velocity.Length();
                    }
                    if (Projectile.ai[0] == 2)
                    {
                        NPCdirection.Track(Projectile, 600, 20, Projectile.DProj().Times[0], 30);
                    }
                }
                Projectile.rotation += Projectile.velocity.Length() * 0.05F* Projectile.direction;
            }
            Projectile.netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Rectangle value = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 vector = Projectile.Size / 2;
            if (Projectile.ai[0] == 2)
            {
                lightColor *= 0.4F;
            }
            SpriteEffects sprite = 0;
            if (Projectile.DProj().vector[0].X>0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color = new Color(156, 0, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                if (Projectile.ai[0] == 2)
                {
                    color *= 0.4F;
                }
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(value), color, Projectile.oldRot[i], texture.Size()/2, Projectile.scale, sprite, 0f);
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(value), color, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale, sprite, 0f);
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(value), color, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale, sprite, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(value), lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, sprite, 0f);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.localAI[0] = -oldVelocity.ToRotation();
            if (Projectile.ai[0] == 0)
                Projectile.ai[1] += 120;
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.localAI[0] = (-Projectile.velocity).ToRotation();
            if (Projectile.ai[0] == 0)
                Projectile.ai[1] += 120;
        }
    }
}