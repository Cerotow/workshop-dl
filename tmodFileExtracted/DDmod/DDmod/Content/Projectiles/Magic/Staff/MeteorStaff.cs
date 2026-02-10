using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class MeteorStaff : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hide = true;
            Projectile.DProj().Times[2] = 20;
        }
        public override void Load()
        {
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.CritChance = player.GetWeaponCrit(player.ActiveItem());
            Projectile.damage = player.GetWeaponDamage(player.ActiveItem());
            Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);
            if (player.controlUseItem && player.statMana >= player.ItemMana())
            {
                player.Dplayer().Realm = 10;
                int p = 0;
                for (int a = 0; a < 1000; a++)
                {
                    Projectile projectile = Main.projectile[a];
                    if (projectile.type>0&& projectile.active && projectile.owner == Projectile.owner && projectile.DProj().Bool[1] && projectile.type == ModContent.ProjectileType<MeteorFormation>())
                    {
                        p++;
                    }
                }
                if (p == 0&& Projectile.ai[1]>0.25F)
                {
                    int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - new Vector2(Main.rand.NextFloat(-80, 80), Main.rand.NextFloat(100, 160)), Projectile.velocity * 12, ModContent.ProjectileType<MeteorFormation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                    Main.projectile[proj].DProj().Bool[1] = true;
                    Main.projectile[proj].netUpdate = true;
                }

                if(player.Dplayer().MouseWorld.X - vector.X>0)
                {
                    Projectile.DProj().vector[0] = new Vector2(1, 0);
                }
                else
                {
                    Projectile.DProj().vector[0] = new Vector2(-1, 0);
                }
                Projectile.netUpdate = true;
                Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize();

                Projectile.DProj().Times[1] = Projectile.DProj().vector[0].ToRotation();
                if (player.Dplayer().MouseWorld.X - vector.X > 0)
                {
                    Projectile.HoldProj(player, 12, Projectile.DProj().Times[1], new Vector2(1, 0), -MathHelper.PiOver4 + player.fullRotation, 0.25F, true);
                    Projectile.Center -= new Vector2(Projectile.DProj().Times[2] / 5, Projectile.DProj().Times[2] + 6);
                }
                else
                {
                    Projectile.HoldProj(player, 12, Projectile.DProj().Times[1], new Vector2(1, 0), -MathHelper.PiOver4 + MathHelper.Pi + player.fullRotation, 0.25F, true);
                    Projectile.Center -= new Vector2(-Projectile.DProj().Times[2] / 5, Projectile.DProj().Times[2] + 6);
                }
                //if (Projectile.ai[1] < 0.5)
                {
                    float v = 0;
                    if (player.direction == -1) v = 3.14f;
                    player.itemRotation = Projectile.velocity.ToRotation() * player.gravDir + v - player.fullRotation-MathHelper.PiOver4*(Projectile.DProj().Times[2]/15) * player.direction;
                }
                //player.heldProj = -1;
                if (Projectile.DProj().Times[2] <= 0)
                {
                    if (Projectile.ai[1] < 1)
                    {
                        if (Projectile.ai[1] == 0.25)
                        {
                            SoundStyle sound = SoundID.Item14;
                            sound.Pitch = -1;
                            PlaySound(sound, Projectile.Center);
                            if (Main.netMode != 2)
                            {
                                Texture2D texture = DDTextures.VoidStar.Value;
                                int Type = 6;
                                for (int a = 0; a < 80; a++)
                                {
                                    int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.5F / 2, player.height / 2), (int)(texture.Width * 0.5F), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].scale = 3f;
                                    Main.dust[dust].velocity = new Vector2((Main.dust[dust].position.X - player.Center.X) / 10, -Main.rand.NextFloat(2, 12));
                                    Main.dust[dust].noLightEmittence = false;
                                    Main.dust[dust].customData = dust;
                                    GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
                                }
                            }
                            
                            for (int a = -6; a <= 6; a++)
                            {
                                Vector2 Pos = player.position + new Vector2(player.width / 2+400*a, -1000);
                                if (a != 0)
                                {
                                    int proj = NewProjectile(Projectile.GetSource_FromThis(), Pos, new Vector2(0, -0.01F), ModContent.ProjectileType<SmallMeteors>(), 0, 2, Projectile.owner, Projectile.whoAmI,a);
                                    Main.projectile[proj].DProj().Times[2] = 2;
                                    Main.projectile[proj].DProj().Times[3] = -600;
                                    Main.projectile[proj].DProj().Bool[4] = true;
                                    Main.projectile[proj].netUpdate = true;
                                }
                            }
                        }
                        Projectile.ai[1] += 0.05f;
                        Projectile.ai[0] = 0;
                    }
                    else if (Projectile.ai[0] > player.ActiveItem().useAnimation && Main.myPlayer == Projectile.owner)
                    {
                        Projectile.ai[0] += player.GetTotalAttackSpeed(Projectile.DamageType);
                        int A = player.ItemMana();
                        player.statMana -= A;
                        Projectile.ai[0] -= player.ActiveItem().useAnimation;

                        if (Main.rand.NextBool(3))
                        {
                            if (Main.rand.NextBool(5))
                            {
                                int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Center - new Vector2(Main.rand.Next(-1000, 1000) + (1000 * player.direction), 600), new Vector2(10 * player.direction, 12), ModContent.ProjectileType<Meteor>(), Projectile.damage, 2, Projectile.owner, 2, 2);
                                Main.projectile[proj].netUpdate = true;
                            }
                            else
                            {
                                int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Center - new Vector2(Main.rand.Next(-1000, 1000) + (1000 * player.direction), 600), new Vector2(10 * player.direction, 9), 424 + Main.rand.Next(3), Projectile.damage*3, 2, Projectile.owner, 1, 1);
                                Main.projectile[proj].netUpdate = true;
                            }
                        }
                        //for (int a = 0; a < 1; a++)
                        {
                            Vector2 ve = new Vector2(Main.rand.Next(10, 300) * player.direction, 600 + (player.Dplayer().MouseWorld.Y - player.Center.Y));
                            NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - ve, ve.PerfectNormalize() *9, 424+Main.rand.Next(3), Projectile.damage, 2, Projectile.owner,1,1);
                        }
                    }
                }
                else
                {
                    Projectile.DProj().Times[3]++;
                    if (Projectile.DProj().Times[3] > 20)
                    {
                        Projectile.DProj().Times[2] -= 4;
                    }
                }
            }
            else
            {
                Projectile.Kill();
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            if (player.direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)(-1), 0f);
            }
            return false;
        }

    }
    public class MeteorFormation : ModProjectile
    {
        public override string Texture => "DDmod/Image/Circle8";
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            if (Projectile.DProj().Bool[1])
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.width = 120;
                Projectile.height = 240;
                Projectile.Center = player.Center;
                Projectile.position.Y -= 100;
                int projId = -1;
                for (int a = 0; a < 1000; a++)
                {
                    Projectile projectile = Main.projectile[a];
                    if (projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<MeteorStaff>())
                    {
                        projId = projectile.whoAmI;
                        break;
                    }
                }
                if (projId >= 0)
                {
                    Projectile.ai[1] = Main.projectile[projId].ai[1];
                }
                else
                {
                    Projectile.ai[1] -= 0.02f;
                    if (Projectile.ai[1] < 0.01F)
                    {
                        Projectile.Kill();
                    }
                }
                Lighting.AddLight(player.Center, new Color(253, 62, 3).ToVector3() * Projectile.ai[1]);
                if (Main.netMode != 2)
                {
                    Texture2D texture = DDTextures.VoidStar.Value;
                    int Type = 6;
                    int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.5F / 2 * Projectile.ai[1], player.height / 2), (int)(texture.Width * 0.5F * Projectile.ai[1]), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].scale = Main.rand.NextFloat(0.6F, 0.8F);
                    Main.dust[dust].velocity = new Vector2(0, -Main.rand.NextFloat(1, 3));
                    Main.dust[dust].noLightEmittence = false;
                    Main.dust[dust].customData = dust;
                    Main.dust[dust].position -= player.velocity;
                    GlobalDust.DustPlayerOwner[dust] = player.whoAmI;

                }
            }
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        { 
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Projectile.Player();
            Projectile.DProj().Times[0] += 0.05F;
            Texture2D texture2 = DDTextures.Circle[9].Value;
            Color color2 = Projectile.GetAlpha(new Color(253, 62, 3, 0));
            Texture2D texture3 = DDTextures.Scanning2.Value;
            Vector2 vector;
            Color color = Projectile.GetAlpha(new Color(253, 62, 3, 0));

            if (Projectile.DProj().Bool[1])
            {
                Projectile.DProj().Times[1] += 0.02F;
                vector = player.Center + new Vector2(0, player.height / 2).RotatedBy(player.fullRotation) - Main.screenPosition;
                Main.spriteBatch.Draw(texture3, vector - new Vector2(0, 10).RotatedBy(player.fullRotation), null, color * 0.7f, player.fullRotation, texture3.Size() / 2, new Vector2(1.2F, 1) * 0.75F * Projectile.ai[1], 0, 0f);
                Texture2D Starlight = DDTextures.GlowEffect.Value;
                DDHelper.BackAndForth(0.8F, 1.2F, 0.04F, ref Projectile.localAI[0], ref Projectile.DProj().Bool[2]);
                Main.spriteBatch.Draw(Starlight, vector - new Vector2(0, 40).RotatedBy(player.fullRotation), null, new Color(253, 62, 3, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(0.7f, 0.3f) * Projectile.ai[1] * Projectile.localAI[0], 0, 0f);

                DDHelper.Compression(texture2, color2, player.fullRotation + MathHelper.PiOver2, Projectile.Opacity, new Vector2(8, 1), Projectile.direction, Projectile.DProj().Times[1], BlendState.Additive);

                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.5F * Projectile.ai[1] / 2, 0, 0);
                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.5F * Projectile.ai[1] / 2, 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
    }
}