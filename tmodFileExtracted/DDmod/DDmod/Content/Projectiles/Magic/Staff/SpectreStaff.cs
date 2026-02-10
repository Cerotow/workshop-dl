using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class SpectreStaff : ModProjectile
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
            Projectile.DProj().Times[4] = 1;
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
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
                    if (projectile.type>0&& projectile.active && projectile.owner == Projectile.owner && projectile.DProj().Bool[1] && !projectile.DProj().Bool[3] && projectile.type == ModContent.ProjectileType<SpectreFormation>())
                    {
                        p++;
                    }
                    if (projectile.active && projectile.aiStyle == 7 && projectile.owner == Projectile.owner)
                    {
                        projectile.Kill();
                    }
                }
                if (p == 0&& Projectile.ai[1]>0.25F)
                {
                    int proj = NewProjectile(Projectile.GetSource_FromThis(), player.position, new Vector2(0,-10), ModContent.ProjectileType<SpectreFormation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
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
                    if (Projectile.ai[1] >= 0.25 && Main.myPlayer == Projectile.owner)
                    {
                        ManaTime++;
                        if (ManaTime % 30 == 0)
                        {
                            int A = player.ItemMana() / 5;
                            player.statMana -= A;
                        }
                    }
                    if (Projectile.ai[1] < 1)
                    {
                        if (Projectile.ai[1] == 0.25)
                        {
                            SoundStyle sound = SoundID.Item14;
                            sound.Pitch = -1;
                            PlaySound(sound, Projectile.Center);
                            Projectile.alpha = 150;
                            if (Main.netMode != 2)
                            {
                                Texture2D texture = DDTextures.VoidStar.Value;
                                int Type = 175;
                                for (int a = 0; a < 40; a++)
                                {
                                    float Rand = -Main.rand.NextFloat(8, 18);
                                    float Rand2 = Main.rand.NextFloat(-5, 5);
                                    for (int t = 0; t < 5; t++)
                                    {
                                        int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.35F / 2, player.height / 2), (int)(texture.Width * 0.35F), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, default);
                                        Main.dust[dust].noGravity = true;
                                        Main.dust[dust].scale = 2.1f;
                                        Main.dust[dust].velocity = new Vector2(Rand2, Rand);
                                        Main.dust[dust].noLightEmittence = false;
                                        Main.dust[dust].customData = dust;
                                        GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
                                    }
                                }
                            }
                        }
                        Projectile.ai[1] += 0.05f;
                        Projectile.ai[0] = 0;
                    }
                    else if (Projectile.ai[0] > player.ActiveItem().useAnimation)
                    {
                        if (!player.controlUseTile)
                        {
                            Projectile.ai[0] = player.ActiveItem().useAnimation;
                        }
                        else if (!player.Dplayer().NoRight)
                        {
                            Projectile.DProj().Times[4] = 3;
                            SoundStyle sound = SoundID.NPCDeath39;
                            sound.Pitch = -1;
                            PlaySound(sound, Projectile.Center);
                            int A = player.ItemMana();
                            player.statMana -= A;
                            Projectile.ai[0] -= player.ActiveItem().useAnimation;
                            if (Main.myPlayer == Projectile.owner)
                            {
                                //for (int a = 0; a < Main.rand.Next(2, 5); a++)
                                {
                                    Vector2 Rand = new Vector2(Main.rand.NextFloat(-80, 80), Main.rand.NextFloat(-80, 80));
                                    NewDustChangeRound(60, player.Center + Rand, 1, 175, 0, 4, true, 3, 0);
                                    int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Center + Rand, (player.Dplayer().MouseWorld - (player.Center + Rand)).PerfectNormalize() * player.ActiveItem().shootSpeed, 297, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                                    Main.projectile[proj].netUpdate = true;
                                }
                            }
                        }
                        player.Aplayer().Stand2 = 2;
                    }
                    else
                    {
                        player.Aplayer().Stand2 = 2;
                    }
                    if(Projectile.DProj().Times[4]>1)
                    {
                        Projectile.DProj().Times[4] -= 0.1F;
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
        int ManaTime;
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
            Color color = Projectile.GetAlpha(Color.White);
            color.A = 0;
            if (player.direction == 1)
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, color * 0.5f * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, 0.25F, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, color * 0.5f * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, 0.25F, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, color * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, 0.25F, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, color * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2,0.25F, 0, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, Projectile.GetAlpha(Color.White), Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition +new Vector2(0,player.gfxOffY), null, color * 0.5f * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2,0.25F, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, color * 0.5f * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2,0.25F, (SpriteEffects)(-1), 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, color * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2,0.25F, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, color * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2,0.25F, (SpriteEffects)(-1), 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, Projectile.GetAlpha(Color.White), Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)(-1), 0f);
            }
            Texture2D Starlight = DDTextures.Starlight.Value;
            DDHelper.BackAndForth(0.4F, 0.6F, 0.04F, ref Sc, ref ScB);
            Main.spriteBatch.Draw(Starlight, Projectile.position + new Vector2(Projectile.width/2-2, -4).RotatedBy(player.fullRotation) - Main.screenPosition, null, new Color(22, 173, 255, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * 2 * Projectile.ai[1] * Sc* Projectile.DProj().Times[4], 0, 0f);
            Main.spriteBatch.Draw(Starlight, Projectile.position + new Vector2(Projectile.width/2-2, -4).RotatedBy(player.fullRotation) - Main.screenPosition, null, new Color(22, 173, 0, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * Projectile.ai[1] * Sc* Projectile.DProj().Times[4], 0, 0f);

            return false;
        }
        float Sc;
        bool ScB;

    }
    public class SpectreFormation : ModProjectile
    {
        public override string Texture => "DDmod/Image/Circle10";
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.scale = 0.1f;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            if (Projectile.DProj().Bool[1])
            {
                Projectile.DProj().Sync = true;
                int projId = -1;
                for (int a = 0; a < 1000; a++)
                {
                    Projectile projectile = Main.projectile[a];
                    if (projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<SpectreStaff>())
                    {
                        projId = projectile.whoAmI;
                        break;
                    }
                }
                if (projId >= 0&& !Projectile.DProj().Bool[3])
                {
                    Projectile.ai[1] = Main.projectile[projId].ai[1];
                    Velocity();
                }
                else
                {
                    Projectile.DProj().Bool[3] = true;
                    Projectile.ai[1] -= 0.02f;
                    if (Projectile.ai[1] < 0.01F)
                    {
                        Projectile.Kill();
                    }
                }
                Lighting.AddLight(player.Center, new Color(255,255,255).ToVector3() * Projectile.ai[1]);
                void Velocity()
                {
                    Projectile.tileCollide = true;
                    player.immuneAlpha = 150;
                    Projectile.alpha = 150;
                    Projectile.velocity *= 0.9F;
                    player.velocity = Vector2.Zero;
                    Main.projectile[(int)Projectile.ai[0]].Center += Projectile.velocity;
                    if (player.controlJump || player.controlUp)
                    {
                        if (Projectile.velocity.Y > -8)
                        {
                            Projectile.velocity.Y -= 0.75F;
                        }
                    }
                    if (player.controlDown)
                    {
                        if (Projectile.velocity.Y < 8)
                        {
                            Projectile.velocity.Y += 0.75F;
                        }
                    }
                    if (player.controlLeft)
                    {
                        if (Projectile.velocity.X > -8)
                        {
                            Projectile.velocity.X -= 0.75F;
                        }
                    }
                    if (player.controlRight)
                    {
                        if (Projectile.velocity.X < 8)
                        {
                            Projectile.velocity.X += 0.75F;
                        }
                    }
                    player.Aplayer().Stand2 = 2;
                    Projectile.width = player.width;
                    Projectile.height = player.height;
                    player.position = Projectile.position;
                    Projectile.netUpdate = true;
                }
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Projectile.Player();
            Projectile.DProj().Times[0] += 0.05F;
            Texture2D texture2 = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture = DDTextures.VoidStar.Value;
            Texture2D texture3 = DDTextures.Scanning2.Value;
            if (Projectile.DProj().Bool[1])
            {
                Color color2 = Projectile.GetAlpha(new Color(255, 255, 255, 0));
                Projectile.DProj().Times[1] += 0.02F;
                Vector2 vector = player.Center + new Vector2(0, player.height / 2+ player.gfxOffY).RotatedBy(player.fullRotation) - Main.screenPosition;
                Main.spriteBatch.Draw(texture, vector, null, color2, player.fullRotation + MathHelper.PiOver2, texture.Size() / 2, new Vector2(0.75F * Projectile.ai[1] / 3, 0.75F * Projectile.ai[1]), 0, 0f);
                Main.spriteBatch.Draw(texture, vector, null, color2 * 0.5F, player.fullRotation + MathHelper.PiOver2, texture.Size() / 2, new Vector2(0.75F * Projectile.ai[1] / 3, 0.75F * Projectile.ai[1]), 0, 0f);
                Main.spriteBatch.Draw(texture3, vector - new Vector2(0, 10).RotatedBy(player.fullRotation), null, color2 * 0.7f, player.fullRotation, texture3.Size() / 2, new Vector2(1.2F, 1) * 0.75F * Projectile.ai[1], 0, 0f);
                
                DDHelper.Compression(texture2, color2, player.fullRotation + MathHelper.PiOver2, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.DProj().Times[1], BlendState.Additive);

                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1], 0, 0);
                Main.EntitySpriteDraw(texture2, vector + new Vector2(0,5 * Projectile.ai[1]), new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1]*0.75F, 0, 0);
                Main.EntitySpriteDraw(texture2, vector + new Vector2(0,10 * Projectile.ai[1]), new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1]*0.5F, 0, 0);
                Main.EntitySpriteDraw(texture2, vector + new Vector2(0,15*Projectile.ai[1]), new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1]*0.25F, 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
    }

}