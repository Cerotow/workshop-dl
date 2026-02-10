using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class CrystalSerpent : ModProjectile
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
                    if (projectile.type>0&& projectile.active && projectile.owner == Projectile.owner && projectile.DProj().Bool[1] && projectile.type == ModContent.ProjectileType<CrystalSerpentFormation>())
                    {
                        p++;
                    }
                }
                if (p == 0&& Projectile.ai[1]>0.25F)
                {
                    int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - new Vector2(Main.rand.NextFloat(-80, 80), Main.rand.NextFloat(100, 160)), Projectile.velocity * 12, ModContent.ProjectileType<CrystalSerpentFormation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
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
                    Projectile.Center -= new Vector2(Projectile.DProj().Times[2] / 5, Projectile.DProj().Times[2]+6);
                }
                else
                {
                    Projectile.HoldProj(player, 12, Projectile.DProj().Times[1], new Vector2(1, 0), -MathHelper.PiOver4 + MathHelper.Pi + player.fullRotation, 0.25F, true);
                    Projectile.Center -= new Vector2(-Projectile.DProj().Times[2] / 5, Projectile.DProj().Times[2]+6);
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
                                //int Type = 254;
                                int Type = ModContent.DustType<光球粒子>();
                                for (int a = 0; a < 300; a++)
                                {
                                    int dust = NewDust(player.Center + new Vector2(0, player.height / 2), 0, 0, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(248, 131, 228, 50));
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].scale = 2f;
                                    Main.dust[dust].velocity = new Vector2(0,-Main.rand.NextFloat(0, 12)).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2,MathHelper.PiOver2));
                                    Main.dust[dust].noLightEmittence = false;
                                    Main.dust[dust].customData = 2;
                                    GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
                                }
                            }

                            for (int a = 0; a < 3; a++)
                            {
                                Vector2 Pos = player.position;
                                int proj = NewProjectile(Projectile.GetSource_FromThis(), Pos, new Vector2(0, -0.01F), ModContent.ProjectileType<CrystalSerpentGhosting>(), 0, 2, Projectile.owner, Projectile.whoAmI,a);
                                Main.projectile[proj].netUpdate = true;
                            }
                        }
                        Projectile.ai[1] += 0.05f;
                        Projectile.ai[0] = 0;
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
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition+new Vector2(0,10), null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, 10), null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)(1), 0f);
            }
            return false;
        }

    }
    public class CrystalSerpentFormation : ModProjectile
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
                    if (projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<CrystalSerpent>())
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
                Lighting.AddLight(player.Center, new Color(248,131,228).ToVector3() * Projectile.ai[1]);
                /*if (Main.netMode != 2)
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
                }*/
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
            Texture2D texture2 = DDTextures.Circle[8].Value;
            Color color2 = Projectile.GetAlpha(new Color(248,131,228, 0));
            Texture2D texture3 = DDTextures.Scanning2.Value;
            Vector2 vector;
            Color color = Projectile.GetAlpha(new Color(248,131,228, 0));

            if (Projectile.DProj().Bool[1])
            {
                Projectile.DProj().Times[1] += 0.02F;
                vector = player.Center + new Vector2(0, player.height / 2).RotatedBy(player.fullRotation) - Main.screenPosition;
                Main.spriteBatch.Draw(texture3, vector - new Vector2(0, 10).RotatedBy(player.fullRotation), null, color * 0.7f, player.fullRotation, texture3.Size() / 2, new Vector2(1.2F, 1) * 0.75F * Projectile.ai[1], 0, 0f);
                Texture2D Starlight = DDTextures.GlowEffect.Value;
                DDHelper.BackAndForth(0.8F, 1.2F, 0.04F, ref Projectile.localAI[0], ref Projectile.DProj().Bool[2]);
                Main.spriteBatch.Draw(Starlight, vector - new Vector2(0, 40).RotatedBy(player.fullRotation), null, new Color(248,131,228, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(0.7f, 0.3f) * Projectile.ai[1] * Projectile.localAI[0], 0, 0f);

                Main.EntitySpriteDraw(texture2, player.MountedCenter- Main.screenPosition, null, color2, Projectile.DProj().Times[1], Utils.Size(texture2) * 0.5f, Projectile.ai[1], 0, 0);

                DDHelper.Compression(texture2, color2, player.fullRotation + MathHelper.PiOver2, Projectile.Opacity, new Vector2(8, 1), Projectile.direction, Projectile.DProj().Times[1], BlendState.Additive);

                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.ai[1], 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
    }
    public class CrystalSerpentGhosting : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Crystal Serpent Ghosting");
           //DisplayName.AddTranslation(7, "水晶蛇虚影");
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 100;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 3;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.scale = 1.5F;
        }
        int[] Body = new int[7];
        Vector2[] Center = new Vector2[7];
        float[] Rotation = new float[7];
        Vector2[] Velocity = new Vector2[7];
        internal Color ColorFunction(float completionRatio)
        {
            return new Color(248, 131, 228, 0);
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            float SC = MathHelper.Lerp(0, 180f * Projectile.scale, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
            if(SC>30)
            {
                SC = 30;
            }
            return SC;
        }
        internal Trailing TrailDrawer;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2 = DDTextures.Starlight3.Value;
            Color color = new Color(248,131,228, 50)*(1F-(float)Projectile.alpha/255);
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(0.1f);
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Player player = Main.player[Projectile.owner];
            TrailDrawer.Draw(Center, -Main.screenPosition, 100, null,1, (1F - (float)Projectile.alpha / 255));
            TrailDrawer.Draw(Center, -Main.screenPosition, 100, null,1, (1F - (float)Projectile.alpha / 255));

            if ( Center[Body.Length -3] != Vector2.Zero)
            {
                if (player.direction == 1)
                {
                    if (Projectile.frame > 0)
                    {
                        Main.EntitySpriteDraw(texture, Center[Body.Length - 3] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2)), color, Rotation[Body.Length - 3] + MathHelper.PiOver2, new Vector2(texture.Width, texture.Height / 2) / 2, Projectile.scale / 1.5F, 0, 0);
                        Main.EntitySpriteDraw(texture, Center[Body.Length - 3] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2)), color, Rotation[Body.Length - 3] + MathHelper.PiOver2, new Vector2(texture.Width, texture.Height / 2) / 2, Projectile.scale / 1.5F, 0, 0);

                        Main.EntitySpriteDraw(texture2, Center[Body.Length - 3] - Main.screenPosition - new Vector2(-4, 11).RotatedBy(Rotation[Body.Length - 3] + MathHelper.PiOver2)*Projectile.scale/1.5F, null, new Color(0, 183, 255,255) * (1F - (float)Projectile.alpha / 255), Rotation[Body.Length - 3], texture2.Size() / 2, Projectile.scale * new Vector2(0.2F, 0.3F), 0, 0);
                        Main.EntitySpriteDraw(texture2, Center[Body.Length - 3] - Main.screenPosition - new Vector2(-4, 11).RotatedBy(Rotation[Body.Length - 3] + MathHelper.PiOver2) * Projectile.scale / 1.5F, null, new Color(0, 183, 255,0) * (1F - (float)Projectile.alpha / 255), Rotation[Body.Length - 3], texture2.Size() / 2, Projectile.scale * new Vector2(0.2F, 0.3F), 0, 0);
                    }
                    else
                    {
                        Main.EntitySpriteDraw(texture, Center[Body.Length - 3] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation[Body.Length - 3] + MathHelper.PiOver2, new Vector2(texture.Width, texture.Height / 2) / 2, Projectile.scale / 1.5F, 0, 0);
                        Main.EntitySpriteDraw(texture, Center[Body.Length - 3] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation[Body.Length - 3] + MathHelper.PiOver2, new Vector2(texture.Width, texture.Height / 2) / 2, Projectile.scale / 1.5F, 0, 0);

                        Main.EntitySpriteDraw(texture2, Center[Body.Length - 3] - Main.screenPosition-new Vector2(-4,6).RotatedBy(Rotation[Body.Length - 3] + MathHelper.PiOver2) * Projectile.scale / 1.5F, null, new Color(0,183, 255,255) * (1F - (float)Projectile.alpha / 255), Rotation[Body.Length - 3], texture2.Size() / 2, Projectile.scale*new Vector2(0.2F,0.3F), 0, 0);
                        Main.EntitySpriteDraw(texture2, Center[Body.Length - 3] - Main.screenPosition-new Vector2(-4,6).RotatedBy(Rotation[Body.Length - 3] + MathHelper.PiOver2) * Projectile.scale / 1.5F, null, new Color(0,183, 255,0) * (1F - (float)Projectile.alpha / 255), Rotation[Body.Length - 3], texture2.Size() / 2, Projectile.scale*new Vector2(0.2F,0.3F), 0, 0);
                    }
                }
                else
                {
                    if (Projectile.frame > 0)
                    {
                        Main.EntitySpriteDraw(texture, Center[Body.Length - 3] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2)), color, Rotation[Body.Length - 3] + MathHelper.PiOver2 - MathHelper.Pi, new Vector2(texture.Width, texture.Height / 2) / 2, Projectile.scale / 1.5F, (SpriteEffects)1, 0);
                        Main.EntitySpriteDraw(texture, Center[Body.Length - 3] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2)), color, Rotation[Body.Length - 3] + MathHelper.PiOver2 - MathHelper.Pi, new Vector2(texture.Width, texture.Height / 2) / 2, Projectile.scale / 1.5F, (SpriteEffects)1, 0);

                        Main.EntitySpriteDraw(texture2, Center[Body.Length - 3] - Main.screenPosition - new Vector2(-4, -11).RotatedBy(Rotation[Body.Length - 3] + MathHelper.PiOver2) * Projectile.scale / 1.5F, null, new Color(0, 183, 255, 255) * (1F - (float)Projectile.alpha / 255), Rotation[Body.Length - 3], texture2.Size() / 2, Projectile.scale * new Vector2(0.2F, 0.3F), (SpriteEffects)1, 0);
                        Main.EntitySpriteDraw(texture2, Center[Body.Length - 3] - Main.screenPosition - new Vector2(-4, -11).RotatedBy(Rotation[Body.Length - 3] + MathHelper.PiOver2) * Projectile.scale / 1.5F, null, new Color(0, 183, 255, 0) * (1F - (float)Projectile.alpha / 255), Rotation[Body.Length - 3], texture2.Size() / 2, Projectile.scale * new Vector2(0.2F, 0.3F), (SpriteEffects)1, 0);
                    }
                    else
                    {
                        Main.EntitySpriteDraw(texture, Center[Body.Length - 3] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation[Body.Length - 3] + MathHelper.PiOver2 - MathHelper.Pi, new Vector2(texture.Width, texture.Height / 2) / 2, Projectile.scale / 1.5F, (SpriteEffects)1, 0);
                        Main.EntitySpriteDraw(texture, Center[Body.Length - 3] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color, Rotation[Body.Length - 3] + MathHelper.PiOver2 - MathHelper.Pi, new Vector2(texture.Width, texture.Height / 2) / 2, Projectile.scale / 1.5F, (SpriteEffects)1, 0);

                        Main.EntitySpriteDraw(texture2, Center[Body.Length - 3] - Main.screenPosition - new Vector2(-4, -6).RotatedBy(Rotation[Body.Length - 3] + MathHelper.PiOver2) * Projectile.scale / 1.5F, null, new Color(0, 183, 255, 255) * (1F - (float)Projectile.alpha / 255), Rotation[Body.Length - 3], texture2.Size() / 2, Projectile.scale * new Vector2(0.2F, 0.3F), (SpriteEffects)1, 0);
                        Main.EntitySpriteDraw(texture2, Center[Body.Length - 3] - Main.screenPosition - new Vector2(-4, -6).RotatedBy(Rotation[Body.Length - 3] + MathHelper.PiOver2) * Projectile.scale / 1.5F, null, new Color(0, 183, 255, 0) * (1F - (float)Projectile.alpha / 255), Rotation[Body.Length - 3], texture2.Size() / 2, Projectile.scale * new Vector2(0.2F, 0.3F), (SpriteEffects)1, 0);
                    }
                }
            }
            /*for (int B = (int)(Projectile.DPoroj().Times[1] - 1); B >= 0; B--)
            {
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[0], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length), 0, 0);
                    // Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[0], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length/2), 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[B], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length), 0, 0);
                    //Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[B], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length/2), 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[B], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length / 2), 0, 0);
                }
            }*/
            for (int B = 0; B < Projectile.DProj().Times[1]; B++)
            {
                if (!Main.gamePaused && !Projectile.DProj().Bool[4])
                {
                    Center[B] += player.Dplayer().PrePosition;
                }
            }

            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.CritChance = player.GetWeaponCrit(player.ActiveItem());
            Projectile.damage = player.GetWeaponDamage(player.ActiveItem());
            if (!player.controlUseItem || Projectile.DProj().Bool[4])
            {
                Projectile.DProj().Bool[4] = true;
                Projectile.velocity *= 0.001f;
                Projectile.alpha += 3;
                if (Projectile.alpha>=255)
                {
                    Projectile.Kill();
                }
                return;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.Center = player.MountedCenter;
            DDHelper.BackAndForth(-0.8f, 0.8f, (0.001F * (Projectile.ai[1]+1) + 0.001F) * player.GetTotalAttackSpeed(Projectile.DamageType), ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0]);

            Projectile.velocity = new Vector2(0, -1).RotatedBy(Projectile.DProj().Times[0]+0.5f*player.direction);
            if (Projectile.timeLeft < 120)
            {
                Projectile.alpha += 3;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                //长度
                int Length = 10;
                if (Body.Length != Length)
                {
                    Body = new int[Length];
                    Center = new Vector2[Length];
                    Rotation = new float[Length];
                    Velocity = new Vector2[Length];
                    for (int B = 0; B < Length; B++)
                    {
                        //Center[B] = Projectile.Center - new Vector2(0.1f);
                    }
                }
                Center[0] = Projectile.Center;
                Rotation[0] = Projectile.rotation;
                Velocity[0] = Projectile.velocity;
                for (int B = 0; B < Body.Length; B++)
                {
                    if (B > 0 && B < Projectile.DProj().Times[1])
                    {
                        Vector2 vector = Center[B - 1] - Center[B];
                        if (B == Body.Length -5)
                        {
                            Velocity[B] = (player.Dplayer().MouseWorld - Center[B]).PerfectNormalize();
                        }
                        else
                        {
                            Velocity[B] = Velocity[B - 1];
                        }
                        Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;
                        float Distance = (vector.Length() - ((15+Projectile.ai[1] % 3 * 5) * Projectile.scale)) / vector.Length();
                        Center[B] += vector * Distance + Velocity[B - 1].PerfectNormalize() * 4 + Projectile.velocity.PerfectNormalize();
                    }
                }
                if (Main.projectile[(int)Projectile.ai[0]].active)
                {
                    Projectile.timeLeft = 122;
                }
                if (Projectile.DProj().Times[1] < Body.Length)
                {
                    Projectile.DProj().Times[1] += 0.1f;
                }
                if (Center[9] != Vector2.Zero)
                {
                    if (Projectile.localAI[0] > player.ActiveItem().useAnimation)
                    {
                        Projectile.DProj().Times[3]++;
                        Projectile.localAI[0] -= player.ActiveItem().useAnimation;
                        if (Main.myPlayer == Projectile.owner&& Projectile.DProj().Times[3] % 3 == Projectile.ai[1] && player.controlUseTile)
                        {
                            int A = player.ItemMana();
                            player.statMana -= A;
                            int proj =NewProjectile(Projectile.GetSource_FromThis(), Center[Body.Length -3], Velocity[Body.Length -3] * 19, 521, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                            Main.projectile[proj].DProj().vector[0] = player.Dplayer().MouseWorld;
                            Main.projectile[proj].netUpdate = true;
                            Projectile.DProj().Times[2]=60;
                            Projectile.netUpdate = true;
                        }
                    }
                    else
                    {
                        Projectile.localAI[0] += player.GetTotalAttackSpeed(Projectile.DamageType)/ (Projectile.extraUpdates+1);
                    }
                }
                DDHelper.MaxandMinF(ref Projectile.DProj().Times[1], Body.Length, 0);
            }
            Projectile.DProj().Times[2]--;
            Projectile.frame = (int)Projectile.DProj().Times[2];
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
    }
}