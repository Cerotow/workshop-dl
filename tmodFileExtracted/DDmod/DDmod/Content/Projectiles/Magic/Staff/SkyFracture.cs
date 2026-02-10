using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class SkyFracture : ModProjectile
    {

        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
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
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 40;
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Melee);
            writer.Write(Chop);
            writer.Write(Mana);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Melee = reader.ReadBoolean();
            Chop = reader.ReadBoolean();
            Mana = reader.ReadBoolean();
        }
        public bool Melee;
        public bool Chop;
        public bool Mana;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.CritChance = player.GetWeaponCrit(player.ActiveItem());
            Projectile.damage = player.GetWeaponDamage(player.ActiveItem());
            if (player.ActiveItem().type != ItemID.SkyFracture)
            {
                Projectile.Kill();
            }
            Projectile.MeleeProj().SwordHitbox = Melee;
            Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);
            if (player.controlUseItem && player.statMana >= player.ItemMana() && !Melee&& !player.HasBuff(23)&& !player.HasBuff(BuffID.Silenced))
            {
                Projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector).PerfectNormalize();
                Projectile.netUpdate = true;
                Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize();

                Projectile.DProj().Times[1] = Projectile.DProj().vector[0].ToRotation();
                Projectile.HoldProj(player, 24, Projectile.DProj().Times[1], new Vector2(1, 0), MathHelper.PiOver4, 0.25F, true);

                if (Projectile.ai[1] < 1)
                {
                    Projectile.ai[1] += 0.02f;
                    Projectile.ai[0] = 0;
                }
                else if (Projectile.ai[0] > player.ActiveItem().useAnimation && Main.myPlayer == Projectile.owner)
                {
                    Projectile.ai[0] += player.GetTotalAttackSpeed(Projectile.DamageType);
                    int A = player.ItemMana();
                    player.statMana -= A;
                    Projectile.ai[0] -= player.ActiveItem().useAnimation;
                    for (int a = 0; a < 2; a++)
                    {
                        int proj = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - Projectile.velocity.PerfectNormalize() * 50 + new Vector2(Main.rand.NextFloat(-50, 10), Main.rand.NextFloat(-60, 60)).RotatedBy(Projectile.rotation - MathHelper.PiOver4), Projectile.velocity * 12, ModContent.ProjectileType<SkyFractureFormation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
            }
            else if (((player.controlUseTile&&!player.Dplayer().NoRight) || Melee) && !player.HasBuff(23) && !player.HasBuff(BuffID.Silenced))
            {
                Melee = true;
                Projectile.height = (int)(46 * Projectile.ai[1]);
                if ((player.controlUseTile || Projectile.ai[1] < 2) && !Chop)
                {
                    player.ChangeDir((player.Dplayer().MouseWorld.X - vector.X) > 0 ? 1 : -1);
                    Projectile.localAI[0] = 2.1F * player.direction;
                    float A = Projectile.localAI[0] + (Projectile.localAI[1] / (-player.HeldItem.useAnimation * (Projectile.extraUpdates+1) * 3));
                    A = -A;
                    Projectile.localAI[1]++;
                    if (Projectile.localAI[1] > 0)
                    {
                        Projectile.localAI[1] = 0;
                    }
                    Projectile.ai[0] = A;

                    Projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector).PerfectNormalize();
                    Projectile.netUpdate = true;
                    if (Projectile.ai[1] < 5)
                    {
                        Projectile.ai[1] += 0.05f;
                    }
                    Projectile.oldPos[0] = Vector2.Zero;
                    // Projectile.DPoroj().Times[0] = player.direction;
                    Projectile.HoldProj(player, 40 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0, false, false);
                    //Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, 2.1f, false);
                }
                else
                {
                    if (!Chop)
                    {
                        if (Main.myPlayer == Projectile.owner&& Projectile.ai[1]>=5)
                        {
                            int A =NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld, Vector2.One.RotatedBy(Main.rand.NextFloat(0,MathHelper.TwoPi)), ModContent.ProjectileType<CrackedSky> (), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                            Main.projectile[A].DProj().color = color;
                        }
                        Chop = true;
                    }
                    Projectile.extraUpdates = 6;
                    Projectile.HoldProj(player, 40 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                    Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), -1, 2.1f, false);
                    if (player.itemAnimation == 0)
                    {
                        Melee = false;
                        Projectile.velocity = Vector2.Zero;
                    }
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.height = 40;
                Chop = false;
                Projectile.extraUpdates = 0;
                Projectile.ai[1] = 0;
                Projectile.ai[0] = 0;
                if (player.direction == 1)
                {
                    Projectile.HoldProj(player, 18, 0, new Vector2(1, 0).RotatedBy(player.fullRotation + MathHelper.Pi), 0, 0, true, 1, false);
                }
                else
                {
                    Projectile.HoldProj(player, 18, 0, new Vector2(1, 0).RotatedBy(player.fullRotation + MathHelper.PiOver2), 0, 0, true, 1, false);
                }
                player.itemTime = 0;
                player.itemAnimation = 0;
                Projectile.Center = player.Center - new Vector2(4 * player.direction, 4 - player.gfxOffY);
                player.heldProj = -1;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Melee)
            {
                modifiers.SourceDamage *= 1 +Projectile.ai[1];
            }

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;
            SoundStyle sound = SoundID.Item29;
            sound.Pitch = 0.5f;
            SoundEngine.PlaySound(sound, target.Center);
            if (!Mana)
            {
                player.statMana += damageDone * 2;
                player.ManaEffect(damageDone * 2);
                Mana = true;
                Projectile.netUpdate = true;
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
        Color color = new Color(0, 129, 221, 0);
        public float TWidth()
        {
            Projectile.MeleeProj().oldVels2 = 25 * Projectile.ai[1] + (8 * (5 - Projectile.ai[1]));
            return 16 * Projectile.ai[1] * Projectile.scale;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if(player.heldProj==-1)
            {
                return false;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            if (Chop)
            {
                Vector2 vector = Projectile.Player().Center;
                if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
                {
                    vector = Projectile.MeleeProj().oldPlayer;
                }
                DDHelper.BladeTrail(DDTextures.WhitePng, color, 1F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, TWidth());
                DDHelper.BladeTrail(DDTextures.Wave, color, 1F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, TWidth());
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            if (player.direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * 0.5f, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * 0.5f, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                }
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)(-1), 0f);
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * 0.5f, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * 0.5f, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                }
            }
            return false;
        }

    }
    public class SkyFractureFormation : ModProjectile
    {
        public override string Texture => "DDmod/Image/Circle8";
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
            Projectile.velocity = Vector2.Zero;
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.Center - player.Center;
            }
            if (!Projectile.DProj().Bool[0])
            {

                Projectile.Center = player.Center + Projectile.DProj().vector[0];
                Projectile.rotation = (Projectile.Player().Dplayer().MouseWorld - Projectile.Center).ToRotation();
                Projectile.scale += 0.025f;
                if (Projectile.scale > 0.6F)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 12, 660, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                    }
                    int Type = 180;
                    for (int A = 0; A < 50; A++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.position - Projectile.velocity.PerfectNormalize() * Projectile.height, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = 1.8f;
                        dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 3f) * new Vector2(0.2f, 1F)).RotatedBy(Projectile.rotation);
                        dust.color = new Color(99, 74, 187, 0);
                        dust.noLightEmittence = false;
                    }

                    Projectile.DProj().Bool[0] = true;
                }
            }
            else
            {
                Projectile.scale -= 0.05f;
                if (Projectile.scale < 0.01F)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[0] += 0.2F;
            Texture2D texture2 = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Color color2 = Projectile.GetAlpha(new Color(30, 129, 221, 0)) * 0.66F;
            Texture2D texture = DDTextures.VoidStar.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Color color = Projectile.GetAlpha(new Color(30, 129, 221, 0));
            Main.spriteBatch.Draw(texture, vector, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale / 3, Projectile.scale), 0, 0f);
            Main.spriteBatch.Draw(texture, vector, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale / 3, Projectile.scale), 0, 0f);

            DDHelper.Compression(texture2, color, Projectile.rotation, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);

            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
    public class CrackedSky : ModProjectile
    {
        public override string Texture => "DDmod/Image/VoidStar";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.width = 8;
            Projectile.height = 90;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 1120;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.scale = 0.01f;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * (0.2f + Main.npc[(int)Projectile.ai[0]].Size.Length() / 100)));
            if (Projectile.velocity.Length() > 0.1f)
                Projectile.velocity = Projectile.velocity.PerfectNormalize() * 0.01F;
            Projectile.timeLeft = 20;
            Projectile.DProj().Times[0]++;
            if (Projectile.DProj().Times[0] < 180)
            {
                if (Projectile.scale < 3F)
                {
                    Projectile.scale += 0.15f;
                }
            }
            else
            {
                Projectile.scale -= 0.05f;
                if (Projectile.scale < 0.05F)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(0, 129, 221,0);
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            if (Projectile.ai[1] >= 2)
            {
                return false;
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            GameShaders.Misc["渲染滤镜"].UseOpacity(0.4f);
            GameShaders.Misc["渲染滤镜"].SetShaderTexture(DDTextures.远古背景2);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["uImageSize1"].SetValue(DDTextures.远古背景2.Size());
            GameShaders.Misc["渲染滤镜"].UseColor(Projectile.DProj().color*0.95f);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["uColor2"].SetValue(Projectile.DProj().color.ToVector3() * 0.95f);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["renderTargetArea"].SetValue(new Vector2(DDTextures.远古背景2.Width(), DDTextures.远古背景2.Height()*6));
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["position"].SetValue(Main.LocalPlayer.Center*4);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["ImageSize"].SetValue(new Vector2(DDTextures.远古背景2.Width(), DDTextures.远古背景2.Height() * 6));
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["upscaleFactor"].SetValue(new Vector2(-1.7F));
            GameShaders.Misc["渲染滤镜"].Apply();
            for (int a = 0; a < 3; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 1.4f, Projectile.rotation, texture.Size() / 2, new Vector2(1.6f + Main.npc[(int)Projectile.ai[0]].Size.Length() / 100, Projectile.scale / 8) * 1.5f, spriteEffects, 0f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), 0, default);
            float num = 0f;
            return new bool?(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center - Pvelocity * 150, Projectile.Center + Pvelocity * 150, 10*Projectile.scale, ref num));
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.ai[1] == 1 && Projectile.damage <= 0)
            {
                overPlayers.Add(index);
            }
        }
    }
}