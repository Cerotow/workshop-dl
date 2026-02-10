using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 星云幻梦Proj : ModProjectile
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
            Projectile.DProj().Times[2] = 40;
            Projectile.DProj().Times[4] = 1;
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            //Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
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
            Projectile.frame++;
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
                    if (projectile.type>0&& projectile.active && projectile.owner == Projectile.owner && projectile.DProj().Bool[1] && !projectile.DProj().Bool[3] && projectile.type == ModContent.ProjectileType<星云幻梦ProjFormation>())
                    {
                        p++;
                    }
                    if (projectile.active && projectile.aiStyle == 7 && projectile.owner == Projectile.owner)
                    {
                        projectile.Kill();
                    }
                }
                if (p == 0 && Projectile.ai[1] > 0.25F)
                {
                    int proj = NewProjectile(Projectile.GetSource_FromThis(), player.position, new Vector2(0, -10), ModContent.ProjectileType<星云幻梦ProjFormation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                    Main.projectile[proj].DProj().Bool[1] = true;
                    Main.projectile[proj].netUpdate = true;
                    for (int a = 0; a < 3; a++)
                    {
                        proj = NewProjectile(Projectile.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<星云之眼>(), Projectile.damage, 0, Projectile.owner, Main.rand.NextFloat(1000), Main.rand.NextFloat(MathHelper.TwoPi));
                        Main.projectile[proj].netUpdate = true;
                    }
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
                    Projectile.Center -= new Vector2(Projectile.DProj().Times[2] / 5, Projectile.DProj().Times[2]/2 + 12);
                }
                else
                {
                    Projectile.HoldProj(player, 12, Projectile.DProj().Times[1], new Vector2(1, 0), -MathHelper.PiOver4 + MathHelper.Pi + player.fullRotation, 0.25F, true);
                    Projectile.Center -= new Vector2(-Projectile.DProj().Times[2] / 5, Projectile.DProj().Times[2]/2 + 12);
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
                            player.statMana -= 2;
                        }
                    }
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
                                int Type = 255;
                                for (int a = 0; a < 80; a++)
                                {
                                    int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.35F / 2, player.height / 2), (int)(texture.Width * 0.35F), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].scale = 2.1f;
                                    Main.dust[dust].velocity = new Vector2((Main.dust[dust].position.X-player.Center.X)/10, -Main.rand.NextFloat(2,18));
                                    Main.dust[dust].noLightEmittence = false;
                                    Main.dust[dust].customData = dust;
                                    GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
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
                        else if(!player.Dplayer().NoRight)
                        {
                            Projectile.DProj().Times[4] = 3;
                            SoundStyle sound = SoundID.Item29;
                            sound.Pitch = 0;
                            PlaySound(sound, Projectile.Center);
                            int A = player.ItemMana();
                            player.statMana -= A;
                            Projectile.ai[0] -= player.ActiveItem().useAnimation;
                            if (Main.myPlayer == Projectile.owner)
                            {
                                for (int a = 0; a < 3; a++)
                                {
                                    int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - new Vector2(0, Main.rand.NextFloat(150, 200)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), Projectile.velocity * 20, ModContent.ProjectileType<星云幻梦ProjFormation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
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
                        Projectile.DProj().Times[2] -= 10;
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
            Color color = new Color(255, 105, 206, 0);
            Rectangle rectangle = new Rectangle(0,0,texture.Width,texture.Height/5);
            if (Projectile.DProj().Times[2] <= 0)
            {
                rectangle = new Rectangle(0, texture.Height / 5 * ((Projectile.frame / 4) % 4 + 1), texture.Width, texture.Height / 5);
            }
            if (player.direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), rectangle, Color.White, Projectile.rotation + MathHelper.PiOver2, rectangle.Size() / 2, Projectile.scale, (SpriteEffects)(1), 0f);
            }
            //Texture2D Starlight = DDTextures.Starlight.Value;
            //DDHelper.BackAndForth(0.4F, 0.6F, 0.04F, ref Sc, ref ScB);
            //Main.spriteBatch.Draw(Starlight, Projectile.position + new Vector2(Projectile.width/2, -6).RotatedBy(player.fullRotation) - Main.screenPosition, null, color, player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * 2 * Projectile.ai[1] * Sc* Projectile.DProj().Times[4], 0, 0f);
            //Main.spriteBatch.Draw(Starlight, Projectile.position + new Vector2(Projectile.width/2, -6).RotatedBy(player.fullRotation) - Main.screenPosition, null, color, player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * Projectile.ai[1] * Sc* Projectile.DProj().Times[4], 0, 0f);

            return false;
        }
        float Sc;
        bool ScB;

    }
    public class 星云幻梦ProjFormation : ModProjectile
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
                    if (projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<星云幻梦Proj>())
                    {
                        projId = projectile.whoAmI;
                        break;
                    }
                }
                if (projId >= 0&& !Projectile.DProj().Bool[3])
                {
                    Projectile.ai[1] = Main.projectile[projId].ai[1];
                    if (Projectile.ai[1] > 0.8F)
                    {
                        Projectile.tileCollide = true;
                    }
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
                Lighting.AddLight(player.Center, new Color(255, 105, 206).ToVector3() * Projectile.ai[1]);
                if (Main.netMode != 2)
                {
                    Texture2D texture = DDTextures.VoidStar.Value;
                    int Type = 255;
                    int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.35F / 2 * Projectile.ai[1], player.height / 2), (int)(texture.Width * 0.35F * Projectile.ai[1]), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].scale = Main.rand.NextFloat(0.8F,1.2F);
                    Main.dust[dust].velocity = new Vector2(0, Main.rand.NextFloat(1,3));
                    Main.dust[dust].noLightEmittence = false;
                    Main.dust[dust].customData = dust;
                    Main.dust[dust].position -= player.velocity;
                }
                void Velocity()
                {
                    Projectile.velocity *= 0.9F;
                    Main.projectile[(int)Projectile.ai[0]].Center += Projectile.velocity;
                    if (player.controlJump || player.controlUp)
                    {
                        if (Projectile.velocity.Y > -10)
                        {
                            Projectile.velocity.Y -= 1F;
                        }
                    }
                    if (player.controlDown)
                    {
                        if (Projectile.velocity.Y < 10)
                        {
                            Projectile.velocity.Y += 1F;
                        }
                    }
                    if (player.controlLeft)
                    {
                        if (Projectile.velocity.X > -10)
                        {
                            Projectile.velocity.X -= 1F;
                        }
                    }
                    if (player.controlRight)
                    {
                        if (Projectile.velocity.X < 10)
                        {
                            Projectile.velocity.X += 1F;
                        }
                    }
                    player.Aplayer().NoGravity = 2;
                    Projectile.width = player.width;
                    Projectile.height = player.height;
                    player.position = Projectile.position;
                    player.velocity = Projectile.velocity;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                if (!Projectile.DProj().Bool[0])
                {
                    Projectile.scale += 0.05f;
                    if (Projectile.scale > 0.75F)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Projectile projectile;
                            /*
                            for (int a = 0; a < 3; a++)
                            {
                                projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.rotation.ToRotationVector2() * 16, Projectile.rotation.ToRotationVector2().RotatedBy(Main.rand.NextFloat(-1.2F,1.2F)) * 30, ModContent.ProjectileType<爆裂荆棘>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, Main.rand.Next(4, 8), 0.5F)];
                                projectile.CritChance = Projectile.CritChance;
                            }*/
                            projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.rotation.ToRotationVector2() * 16, Projectile.rotation.ToRotationVector2() * player.ActiveItem().shootSpeed, ModContent.ProjectileType<星云火焰>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, Main.rand.Next((int)(3 + player.ActiveItem().shootSpeed/4), (int)(3+ player.ActiveItem().shootSpeed/2)), 0.5F)];
                            projectile.CritChance = Projectile.CritChance;
                        }
                        Projectile.DProj().Bool[0] = true;
                    }
                    else
                    {
                        NPC npc = NPCdirection.FindClosest(Projectile.Center, 500, false);
                        if (npc == null)
                        {
                            Projectile.rotation = (Projectile.Player().Dplayer().MouseWorld - Projectile.Center).ToRotation();
                        }
                        else
                        {
                            Projectile.rotation = (npc.Center - Projectile.Center).ToRotation();

                        }
                    }
                }
                else
                {
                    Projectile.DProj().Times[2]++;
                    if (Projectile.DProj().Times[2] > 80)
                    {
                        Projectile.scale -= 0.05f;
                        if (Projectile.scale < 0.01F)
                        {
                            int Type = 255;
                            for (int A = 0; A < 20; A++)
                            {
                                Dust dust = Main.dust[NewDust(Projectile.position - Projectile.velocity.PerfectNormalize() * Projectile.height, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 200, default)];
                                dust.noGravity = true;
                                dust.scale = 1.8f;
                                dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 5f) * new Vector2(0.2f, 1F)).RotatedBy(Projectile.rotation);
                                dust.noLightEmittence = false;
                            }
                            Projectile.Kill();
                        }
                    }
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
            Texture2D texture2 = DDTextures.Circle[12].Value;
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Color color2 = Projectile.GetAlpha(new Color(255, 105, 206, 0));
            Texture2D texture = DDTextures.VoidStar.Value;
            Texture2D texture3 = DDTextures.Scanning2.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Color color = Projectile.GetAlpha(new Color(255, 105, 206, 0));
            Main.spriteBatch.Draw(texture, vector, null, color2, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale / 3, Projectile.scale), 0, 0f);
            Main.spriteBatch.Draw(texture, vector, null, color2 * 0.5f, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale / 3, Projectile.scale), 0, 0f);
            if (Projectile.DProj().Times[2] < 80)
            {
                Main.spriteBatch.Draw(texture3, vector + Projectile.rotation.ToRotationVector2() * (10 * Projectile.scale), null, color2, Projectile.rotation + MathHelper.PiOver2, texture3.Size() / 2, new Vector2(Projectile.scale * 1.2F, Projectile.scale), 0, 0f);
                Main.spriteBatch.Draw(texture3, vector + Projectile.rotation.ToRotationVector2() * (10 * Projectile.scale), null, new Color(255, 105, 206, 0), Projectile.rotation + MathHelper.PiOver2, texture3.Size() / 2, new Vector2(Projectile.scale * 1.2F, Projectile.scale) / 2, 0, 0f);
            }
            DDHelper.Compression(texture2, color, Projectile.rotation, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);

            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.DProj().Bool[1])
            {
                color2 = Projectile.GetAlpha(new Color(255, 105, 206, 0));
                Projectile.DProj().Times[1] += 0.02F;
                vector = player.Center + new Vector2(0, player.height / 2+ player.gfxOffY).RotatedBy(player.fullRotation) - Main.screenPosition;
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