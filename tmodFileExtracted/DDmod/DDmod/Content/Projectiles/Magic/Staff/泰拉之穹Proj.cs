using DDmod.Content.Dusts;
using DDmod.Content.Items.Magic.Staff.Make;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.GeneralProj;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 泰拉之穹Proj : ModProjectile
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
            Projectile.localNPCHitCooldown = 20;
            Projectile.hide = true;
            Projectile.DProj().Times[2] = 20;
        }
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> QTexture;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
            QTexture = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Magic/Staff/泰拉之球");
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        List<NPC> npcs = new List<NPC>();
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            npcs = new List<NPC>();
            for (int a = 0; a < 5 + Projectile.DProj().Times[4]; a++)
            {
                NPC npc = NPCdirection.FindClosest2(Qposition, 1000, false, npcs);
                if(npc!=null)
                {
                    npcs.Add(npc);
                }
            }
            if (Projectile.DProj().Times[4] > 0)
            {
                Projectile.DProj().Times[4] -= 0.01F;
                if (Projectile.DProj().Times[4] >= 1.9f)
                {
                    player.Dplayer().Bossperspective(player.Center, 1, false, 3 - Projectile.DProj().Times[4]+0.1F);
                }
            }
            else
            {
                Projectile.DProj().Times[4] = 0f;
            }
            Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);
            if (player.controlUseItem && player.statMana >= player.ItemMana())
            {
                player.Dplayer().Realm = 10;
                int p = -1;
                for (int a = 0; a < 1000; a++)
                {
                    Projectile projectile = Main.projectile[a];
                    if (projectile.type > 0 && projectile.active && projectile.owner == Projectile.owner && !projectile.DProj().Bool[3] && projectile.type == ModContent.ProjectileType<泰拉之穹Formation>())
                    {
                        p= projectile.whoAmI;
                    }
                    if (projectile.active && projectile.aiStyle == 7 && projectile.owner == Projectile.owner)
                    {
                        projectile.Kill();
                    }
                }
                if (p == -1 && Projectile.ai[1] > 0.25F)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        int proj = NewProjectile(Projectile.GetSource_FromThis(), player.position, new Vector2(0, -10), ModContent.ProjectileType<泰拉之穹Formation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[proj].DProj().Bool[1] = true;
                        Main.projectile[proj].netUpdate = true;
                    }
                }

                if (player.Dplayer().MouseWorld.X - vector.X > 0)
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
                    player.itemRotation = Projectile.velocity.ToRotation() * player.gravDir + v - player.fullRotation - MathHelper.PiOver4 * (Projectile.DProj().Times[2] / 15) * player.direction;
                }
                //player.heldProj = -1;
                if (Projectile.DProj().Times[2] <= 0)
                {
                    if (Projectile.ai[1] >= 0.25 && Main.myPlayer == Projectile.owner)
                    {
                        ManaTime++;
                        if (ManaTime % 30 == 0)
                        {
                            /*
                            for (int a = 0; a < 2; a++)
                            {
                                int proj = NewProjectile(Projectile.GetSource_FromThis(), player.MountedCenter - new Vector2(0, Main.rand.NextFloat(20, 200)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), Vector2.Zero, 316, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                                Main.projectile[proj].DProj().Bool[1] = true;
                                Main.projectile[proj].netUpdate = true;
                            }*/
                            int A = player.ItemMana()/10;
                            if(player.ItemMana()>3&&A<=0)
                            {
                                A = 1;
                            }
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
                            Qposition = Projectile.position + new Vector2(Projectile.width / 2, -24).RotatedBy(player.fullRotation);
                            if (Main.netMode != 2)
                            {
                                Texture2D texture = DDTextures.VoidStar.Value;
                                int Type = ModContent.DustType<光球粒子>();
                                for (int a = 0; a < 100; a++)
                                {
                                    int dust = NewDust(player.Center + new Vector2(0, player.height / 2), 0, 0, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164),0));
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].scale = 2f;
                                    Main.dust[dust].velocity = new Vector2(0, -Main.rand.NextFloat(0, 6)).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2));
                                    Main.dust[dust].noLightEmittence = false;
                                    Main.dust[dust].customData = 2;
                                    GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
                                }
                                Type = ModContent.DustType<星光粒子>();
                                for (int a = 0; a < 30; a++)
                                {
                                    int dust = NewDust(player.Center + new Vector2(0, player.height / 2), 0, 0, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].scale = 1.5f;
                                    Main.dust[dust].velocity = new Vector2(0, -Main.rand.NextFloat(0, 12)).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2));
                                    Main.dust[dust].noLightEmittence = false;
                                    Main.dust[dust].customData = 1;
                                    GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
                                }

                            }
                        }
                        Projectile.ai[1] += 0.05f;
                        Projectile.ai[0] = 0;
                    }
                    else if (Projectile.ai[0] > player.ActiveItem().useAnimation)
                    {
                        int Type = ModContent.DustType<光球粒子>();
                        int Type2 = ModContent.DustType<星光粒子>();
                        if (!player.controlUseTile)
                        {
                            Projectile.ai[0] = player.ActiveItem().useAnimation;
                            if (!Projectile.DProj().Bool[4])
                            {
                                SoundStyle sound = SoundID.Item29;
                                sound.Volume = 1F;
                                sound.Pitch = 0.5f;
                                PlaySound(sound, Projectile.Center);
                                Projectile.DProj().Bool[4] = true;
                                for(int a=0;a<25;a++)
                                {
                                    int dust = NewDust(Projectile.PreviousCenter() - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].scale = Main.rand.NextFloat(1, 2F);
                                    Main.dust[dust].velocity = new Vector2(0, 3).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                    Main.dust[dust].noLightEmittence = false;
                                    Main.dust[dust].customData = 1;
                                    GlobalDust.DustProjectileOwner[dust] = Projectile.whoAmI;
                                }
                                for(int a=0;a<25;a++)
                                {
                                    int dust = NewDust(Projectile.PreviousCenter() - new Vector2(4), 1, 1, Type2, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].scale = Main.rand.NextFloat(1, 2F);
                                    Main.dust[dust].velocity = new Vector2(0, Main.rand.NextFloat(2F, 8F)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                    Main.dust[dust].noLightEmittence = false;
                                    Main.dust[dust].customData = 1;
                                    GlobalDust.DustProjectileOwner[dust] = Projectile.whoAmI;
                                }
                            }
                            if (Main.rand.NextBool(5))
                            {
                                int dust = NewDust(player.position - new Vector2(4), player.width, player.height, Type2, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].scale = Main.rand.NextFloat(1,1.5F);
                                Main.dust[dust].velocity = new Vector2(0, 2).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                Main.dust[dust].noLightEmittence = false;
                                Main.dust[dust].customData = 1;
                                //GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
                            }
                            if (Main.rand.NextBool(2))
                            {
                                int dust = NewDust(Projectile.PreviousCenter() - new Vector2(4), 1, 1, Type2, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].scale = Main.rand.NextFloat(1, 2F);
                                Main.dust[dust].velocity = new Vector2(0, 3).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                Main.dust[dust].noLightEmittence = false;
                                Main.dust[dust].customData = 1;
                                GlobalDust.DustProjectileOwner[dust] = Projectile.whoAmI;
                            }
                        }
                        else if (!Collision.SolidCollision(new Vector2(player.Dplayer().MouseWorld.X-player.Size.X/2, player.Dplayer().MouseWorld.Y-player.Size.Y/2),player.width, player.height)&& !player.Dplayer().NoRight)
                        {
                            Projectile.DProj().Bool[4] = false;
                            SoundStyle sound = SoundID.Item29;
                            sound.Volume = 0.3F;
                            sound.Pitch = 0;
                            PlaySound(sound, Projectile.Center);
                            int A = player.ItemMana();
                            player.statMana -= A;
                            Projectile.ai[0] -= player.ActiveItem().useAnimation;
                            if (Main.myPlayer == Projectile.owner)
                            {
                                //int proj = NewProjectile(Projectile.GetSource_FromThis(), Qposition, Vector2.Zero, ModContent.ProjectileType<泰拉弹>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                                //Main.projectile[proj].netUpdate = true;
                            }
                            for (int a = 0; a < 40; a++)
                            {
                                int dust = NewDust(Main.projectile[p].Center - new Vector2(4), 0, 0, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].scale = 1.5f;
                                Main.dust[dust].velocity = new Vector2(0, Main.rand.NextFloat(2, 5)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                Main.dust[dust].noLightEmittence = false;
                                Main.dust[dust].customData =2;
                            }
                            for (int a = 0; a < 10; a++)
                            {
                                int dust = NewDust(Main.projectile[p].Center - new Vector2(4), 0, 0, Type2, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].scale = 1.5f;
                                Main.dust[dust].velocity = new Vector2(0, Main.rand.NextFloat(2, 5)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                Main.dust[dust].noLightEmittence = false;
                                Main.dust[dust].customData =1;
                            }
                            for (int b = 0; b < (player.Dplayer().MouseWorld - Main.projectile[p].Center).Length() / 10; b++)
                            {
                                for (int a = 0; a < 5; a++)
                                {
                                    int dust = NewDust(Main.projectile[p].Center + ((player.Dplayer().MouseWorld - Main.projectile[p].Center) / (player.Dplayer().MouseWorld - Main.projectile[p].Center).Length() * b * 10) - new Vector2(4), 0, 0, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].scale = 0.6f;
                                    Main.dust[dust].velocity = new Vector2(0, 1).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                    Main.dust[dust].noLightEmittence = false;
                                    Main.dust[dust].customData = 1;
                                }
                                for (int a = 0; a < 1; a++)
                                {
                                    int dust = NewDust(Main.projectile[p].Center + ((player.Dplayer().MouseWorld - Main.projectile[p].Center) / (player.Dplayer().MouseWorld - Main.projectile[p].Center).Length() * b * 10) - new Vector2(4), 0, 0, Type2, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].scale = 0.6f;
                                    Main.dust[dust].velocity = new Vector2(0, 1).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                    Main.dust[dust].noLightEmittence = false;
                                    Main.dust[dust].customData = 0.5f;
                                }
                            }

                            player.Dplayer().Bossperspective(player.Dplayer().MouseWorld, 3, false, 0.2F);
                            Main.projectile[p].Center = player.Dplayer().MouseWorld;

                            for (int a = 0; a < 40; a++)
                            {
                                int dust = NewDust(Main.projectile[p].Center - new Vector2(4), 0, 0, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].scale = 1.5f;
                                Main.dust[dust].velocity = new Vector2(0, Main.rand.NextFloat(2, 5)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                Main.dust[dust].noLightEmittence = false;
                                Main.dust[dust].customData = 2;
                            }
                            for (int a = 0; a < 10; a++)
                            {
                                int dust = NewDust(Main.projectile[p].Center - new Vector2(4), 0, 0, Type2, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].scale = 1.5f;
                                Main.dust[dust].velocity = new Vector2(0, Main.rand.NextFloat(2, 5)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                                Main.dust[dust].noLightEmittence = false;
                                Main.dust[dust].customData = 1;
                            }
                            for (int a = 0; a < 10; a++)
                            {
                                Projectile.NewProjectileChange(Main.projectile[p].Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))* Main.rand.NextFloat(8F, 12F), ModContent.ProjectileType<魔法泰拉弹>(), Projectile.damage,0,-1,Main.rand.NextFloat(0.6F,1F));
                            }
                            Projectile.DProj().Times[4] = 3;
                        }
                        player.Aplayer().Stand2 = 2;
                    }
                    else
                    {
                        player.Aplayer().Stand2 = 2;
                    }
                    if (player.direction == 1)
                    {
                        Qposition += (Projectile.position + new Vector2(2, 0) + new Vector2(Projectile.width / 2, -24).RotatedBy(player.fullRotation) - Qposition) * 0.1f;
                    }
                    else
                    {
                        Qposition += (Projectile.position + new Vector2(Projectile.width / 2, -24).RotatedBy(player.fullRotation) - Qposition) * 0.1f;
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
                player.itemTime = 0;
                player.itemAnimation = 0;
            }
            Projectile.localNPCHitCooldown = (int)player.IteUseAnimation2() / 20;
            Projectile.CritChance = player.GetWeaponCrit(player.ActiveItem());
            Projectile.damage = (int)(player.GetWeaponDamage(player.ActiveItem()) * (Projectile.DProj().Times[4]/2 + 1));
            return false;
        }
        int ManaTime;
        public override bool ShouldUpdatePosition()
        {
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
            Projectile.numHits = 2;
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, settings, Projectile.owner);
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            for(int a =0;a< npcs.Count;a++)
            {
                if (npcs[a].whoAmI==target.whoAmI)
                {
                    return true;
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            ro += 0.1f;
            if (player.direction == 1)
            {
                Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, new Color(255, 255, 255, 0), Projectile.rotation, Glow.Size() / 2, Projectile.scale/4, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, new Color(255,255,255,0), Projectile.rotation + MathHelper.PiOver2, Glow.Size() / 2, Projectile.scale/4, SpriteEffects.FlipHorizontally, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, player.gfxOffY), null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
            }
            Texture2D Starlight = DDTextures.Starlight.Value;
            DDHelper.BackAndForth(0.4F, 0.6F, 0.04F, ref Sc, ref ScB);
            Texture2D Qtexture = QTexture.Value;

            Main.spriteBatch.Draw(Qtexture, Qposition - Main.screenPosition, null, Color.White, player.fullRotation, Qtexture.Size() / 2, Projectile.scale/8, 0, 0f);
            if (npcs.Count > 0)
            {
                Main.spriteBatch.Draw(Starlight, Qposition - Main.screenPosition, null, new Color(173, 255, 170, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * 2 * Projectile.ai[1] * Sc*(1+ Projectile.DProj().Times[4]/2), 0, 0f);
                Main.spriteBatch.Draw(Starlight, Qposition - Main.screenPosition, null, new Color(33, 255, 30, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * Projectile.ai[1] * Sc * (1 + Projectile.DProj().Times[4] / 2), 0, 0f);
                for (int a = 0; a < npcs.Count; a++)
                {
                    Vector2 vector = npcs[a].Center - Qposition;
                   
                    for (int L = 0; L < vector.Length() / 5; L++)
                    {
                        float T = -0.5F;
                        T += ((float)L / (vector.Length() / 5));
                        T = Math.Abs(T);
                        T += 0.2F;
                        T *= 1+Projectile.DProj().Times[4];
                        Main.spriteBatch.Draw(DDTextures.GlowEffect.Value, Qposition+ vector/ (vector.Length() / 5)*L - Main.screenPosition, null, new Color(103, 255, 100, 155) * 0.8F* T, 0, DDTextures.GlowEffect.Size() / 2, 0.08F* (1+Projectile.DProj().Times[4]/10), 0, 0f);
                        Main.spriteBatch.Draw(DDTextures.GlowEffect.Value, Qposition + vector / (vector.Length() / 5) * L - Main.screenPosition, null, new Color(103, 255, 100, 100).Opposite() * 0.5F* T, 0, DDTextures.GlowEffect.Size() / 2, 0.04F* (1+Projectile.DProj().Times[4]/10), 0, 0f);
                    }
                    Main.spriteBatch.Draw(Starlight, npcs[a].Center - Main.screenPosition, null, new Color(173, 255, 170, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * 3 * Projectile.ai[1] * Sc, 0, 0f);
                    Main.spriteBatch.Draw(Starlight, npcs[a].Center - Main.screenPosition, null, new Color(33, 255, 30, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) *1.5F* Projectile.ai[1] * Sc, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.GlowEffect.Value, npcs[a].Center - Main.screenPosition, null, new Color(103, 255, 100, 100), ro, DDTextures.GlowEffect.Size() / 2, 0.35F, 0, 0f);
                }
            }
            return false;
        }
        public Vector2 Qposition;
        float Sc;
        bool ScB;
        float ro;

    }
    public class 泰拉之穹Formation : ModProjectile
    {
        public override string Texture => "DDmod/Image/Circle12";
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.scale = 0.1f;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            Projectile.DProj().Sync = true;
            int projId = -1;
            for (int a = 0; a < 1000; a++)
            {
                Projectile projectile = Main.projectile[a];
                if (projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<泰拉之穹Proj>())
                {
                    projId = projectile.whoAmI;
                    break;
                }
            }
            if (projId >= 0 && !Projectile.DProj().Bool[3])
            {
                Projectile.ai[1] = Main.projectile[projId].ai[1];
                if (Projectile.ai[1] > 0.8F)
                {
                    Projectile.tileCollide = true;
                }
                if (Projectile.ai[2] < 0.3f)
                    Projectile.ai[2] += 0.01f;
                Velocity();
            }
            else
            {
                if (Projectile.ai[2] > 0)
                    Projectile.ai[2] -= 0.03f;
                Projectile.DProj().Bool[3] = true;
                Projectile.ai[1] -= 0.02f;
                if (Projectile.ai[1] < 0.01F)
                {
                    Projectile.Kill();
                }
            }
            if (Main.netMode != 2)
            {
                Texture2D texture = DDTextures.VoidStar.Value;
                int Type = ModContent.DustType<光球粒子>();
                int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.35F / 2 * Projectile.ai[1], player.height / 2), (int)(texture.Width * 0.35F * Projectile.ai[1]), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0));
                Main.dust[dust].noGravity = false;
                Main.dust[dust].scale = Main.rand.NextFloat(0.8F, 1.2F);
                Main.dust[dust].velocity = new Vector2(0, Main.rand.NextFloat(1, 3));
                Main.dust[dust].noLightEmittence = false;
                Main.dust[dust].customData = 1;
                Main.dust[dust].position -= player.velocity;
                GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
            }
            void Velocity()
            {
                Projectile.velocity *= 0.9F;
                Main.projectile[(int)Projectile.ai[0]].Center += Projectile.velocity;
                if (player.controlJump || player.controlUp)
                {
                    if (Projectile.velocity.Y > -20)
                    {
                        Projectile.velocity.Y -= 1F;
                    }
                }
                if (player.controlDown)
                {
                    if (Projectile.velocity.Y < 20)
                    {
                        Projectile.velocity.Y += 1F;
                    }
                }
                if (player.controlLeft)
                {
                    if (Projectile.velocity.X > -20)
                    {
                        Projectile.velocity.X -= 1F;
                    }
                }
                if (player.controlRight)
                {
                    if (Projectile.velocity.X < 20)
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
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        float[] R = new float[8];
        public override bool PreDraw(ref Color lightColor)
        {
            if(R==null||R.Length!=10)
            {
                R = new float[10];
            }
            for (int i = R.Length - 1; i > 0; i--)
            {
                R[i] = R[i - 1];
            }
            Player player = Projectile.Player();
            Projectile.DProj().Times[0] += 0.05F;
            Texture2D texture2 = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.GetAlpha(new Color(63, 255, 60, 255)*0.75f);
            Color color2 = Projectile.GetAlpha(new Color(173, 255, 170, 0) * 0.75f);
            Texture2D texture = DDTextures.VoidStar.Value;
            Texture2D texture3 = DDTextures.Scanning2.Value;
            Vector2 vector = player.Center + new Vector2(0, player.height / 2 + player.gfxOffY).RotatedBy(player.fullRotation) - Main.screenPosition;

            if (Projectile.DProj().Bool[1])
            {
                DDHelper.BackAndForth(-1.2F, 0.6F, 0.05F, ref R[0], ref Projectile.DProj().Bool[1]);
            }
            else
            {
                DDHelper.BackAndForth(-1.2F, 0.6F, 0.2F, ref R[0], ref Projectile.DProj().Bool[1]);
            }
            Projectile.DProj().Times[1] += 0.02F;

            Main.spriteBatch.Draw(texture, vector, null, color2, player.fullRotation + MathHelper.PiOver2, texture.Size() / 2, new Vector2(0.75F * Projectile.ai[1] / 3, 0.75F * Projectile.ai[1]), 0, 0f);
            Main.spriteBatch.Draw(texture, vector, null, color2 * 0.5F, player.fullRotation + MathHelper.PiOver2, texture.Size() / 2, new Vector2(0.75F * Projectile.ai[1] / 3, 0.75F * Projectile.ai[1]), 0, 0f);
            Main.spriteBatch.Draw(texture3, vector - new Vector2(0, 10).RotatedBy(player.fullRotation), null, color2 * 0.7f, player.fullRotation, texture3.Size() / 2, new Vector2(1.2F, 1) * 0.75F * Projectile.ai[1], 0, 0f);

            DDHelper.Compression(texture2, color, player.fullRotation + MathHelper.PiOver2, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.DProj().Times[1], BlendState.Additive);

            Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1], 0, 0);
            Main.EntitySpriteDraw(texture2, vector + new Vector2(0, 5 * Projectile.ai[1]), new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1] * 0.75F, 0, 0);
            Main.EntitySpriteDraw(texture2, vector + new Vector2(0, 10 * Projectile.ai[1]), new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1] * 0.5F, 0, 0);
            Main.EntitySpriteDraw(texture2, vector + new Vector2(0, 15 * Projectile.ai[1]), new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1] * 0.25F, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }

}