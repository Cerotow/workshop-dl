using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class BlizzardStaff : ModProjectile
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
            int r = 0;
            for (int A = 0; A < 1000; A++)
            {
                Projectile proj = Main.projectile[A];
                if (proj.active && proj.owner == Projectile.owner && proj.type == Projectile.type)
                {
                    r++;
                }
            }
            if (r > 1)
            {
                Projectile.Kill();
            }
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
                    if (projectile.type>0&& projectile.active && projectile.owner == Projectile.owner && projectile.DProj().Bool[1] && projectile.type == ModContent.ProjectileType<BlizzardFormation>())
                    {
                        p++;
                    }
                }
                if (!Projectile.DProj().Bool[2])
                {
                    Projectile.ai[1] = 0;
                }
                if (p == 0&& Projectile.ai[1]>0.25F)
                {
                    int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - new Vector2(Main.rand.NextFloat(-80, 80), Main.rand.NextFloat(100, 160)), Projectile.velocity * 12, ModContent.ProjectileType<BlizzardFormation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
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
                                int Type = 197;
                                for (int a = 0; a < 80; a++)
                                {
                                    int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.35F / 2, player.height / 2), (int)(texture.Width * 0.35F), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].scale = 3f;
                                    Main.dust[dust].velocity = new Vector2((Main.dust[dust].position.X - player.Center.X) / 10, -Main.rand.NextFloat(2, 12));
                                    Main.dust[dust].noLightEmittence = false;
                                    Main.dust[dust].customData = dust;
                                    GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
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
                        int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Center-new Vector2(Main.rand.NextFloat(-1000,1000)+ (Main.windSpeedTarget * 600), 600), new Vector2(Main.windSpeedTarget*10, 16), 337, Projectile.damage, 2, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[proj].netUpdate = true;
                        for(int a = 0;a<3;a++)
                        {
                            Vector2 ve = new Vector2(Main.rand.NextFloat(10,300) * player.direction, 600+ (player.Dplayer().MouseWorld.Y-player.Center.Y));
                            NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - ve, ve.PerfectNormalize()*16, 337, Projectile.damage, 2, Projectile.owner, Projectile.whoAmI);
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
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition-new Vector2(0,10), null, new Color(255,255,255, 0) * 0.5f, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition - new Vector2(0, 10), null, new Color(255,255,255, 0) * 0.5f, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition - new Vector2(0, 10), null, new Color(255,255,255, 0), Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition - new Vector2(0, 10), null, new Color(255,255,255, 0), Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(0, 10), null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition - new Vector2(0, 10), null, new Color(255,255,255, 0) * 0.5f, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition - new Vector2(0, 10), null, new Color(255,255,255, 0) * 0.5f, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition - new Vector2(0, 10), null, new Color(255,255,255, 0), Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition - new Vector2(0, 10), null, new Color(255,255,255, 0), Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(0, 10), null, Color.White, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)(-1), 0f);
            }
            return false;
        }

    }
    public class BlizzardFormation : ModProjectile
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
                    if (projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<BlizzardStaff>())
                    {
                        projId = projectile.whoAmI;
                        break;
                    }
                }
                if (projId >= 0)
                {
                    if(Projectile.DProj().Bool[3])
                    {

                        Projectile.Kill();
                        return;
                    }
                    Projectile.ai[1] = Main.projectile[projId].ai[1];
                }
                else
                {
                    Projectile.ai[1] -= 0.02f;
                    if (Projectile.ai[1] < 0.01F)
                    {
                        Projectile.Kill();
                    }
                    Projectile.DProj().Bool[3] = true;
                }
                Lighting.AddLight(player.Center, new Color(24,191,238).ToVector3() * Projectile.ai[1]);
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (!Projectile.DProj().Bool[1])
            {
                for (int A = -50; A < 50; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position+ Projectile.velocity.PerfectNormalize() * A, 68, 68, 6, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 2.4f;
                    dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 2f) * new Vector2(2f, 2F)).RotatedBy(Projectile.rotation);
                    dust.noLightEmittence = false;
                }
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.DProj().Times[3] < 420)
            {
                modifiers.SourceDamage += 2;
                if (target.knockBackResist > 0)
                {
                    target.velocity = Projectile.velocity.PerfectNormalize() * 10;
                }
            }
            if (target.velocity.Y != 0)
            {
                modifiers.Knockback *= 0;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.DProj().Bool[1])
            {
                if (target.knockBackResist > 0)
                {
                    target.velocity = (target.Center - Projectile.Center).PerfectNormalize() * 8;
                }
            }
            target.AddBuff(24, 180);
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Projectile.Player();
            Projectile.DProj().Times[0] += 0.05F;
            Texture2D texture2 = DDTextures.Circle[9].Value;
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Color color2 = Projectile.GetAlpha(new Color(24,191,238, 0));
            Texture2D texture = DDTextures.VoidStar.Value;
            Texture2D texture3 = DDTextures.Scanning2.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Color color = Projectile.GetAlpha(new Color(24,191,238, 0));

            if (Projectile.DProj().Bool[1])
            {
                Projectile.DProj().Times[1] += 0.02F;
                vector = player.Center + new Vector2(0, player.height / 2).RotatedBy(player.fullRotation) - Main.screenPosition;
                Texture2D Starlight = DDTextures.Starlight.Value;
                texture = DDTextures.冻结_Glow.Value;
                Main.EntitySpriteDraw(texture, vector + new Vector2(0, 6), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), new Color(255, 255, 255, 0), player.fullRotation, new Vector2(texture.Width / 2, texture.Height - 40), new Vector2(1F, 1F) * Projectile.ai[1] / 2, 0, 0);

                DDHelper.Compression(texture2, color2, player.fullRotation + MathHelper.PiOver2, Projectile.Opacity, new Vector2(8, 1), Projectile.direction, Projectile.DProj().Times[1], BlendState.Additive);

                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1] / 3, 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                return false;
            }
            //Main.EntitySpriteDraw(texture, vector2,null, color2, 0f, Utils.Size(texture) * 0.5f, 1, 0, 0);

            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
    }
}