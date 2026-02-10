using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class HallowedStaff : ModProjectile
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
                int p = 0;
                for (int a = 0; a < 1000; a++)
                {
                    Projectile projectile = Main.projectile[a];
                    if (projectile.type>0&& projectile.active && projectile.owner == Projectile.owner && projectile.DProj().Bool[1] && projectile.type == ModContent.ProjectileType<HallowedFormation>())
                    {
                        p++;
                    }
                }
                if (p == 0)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - new Vector2(Main.rand.NextFloat(-80, 80), Main.rand.NextFloat(100, 160)), Projectile.velocity * 12, ModContent.ProjectileType<HallowedFormation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[proj].DProj().Bool[1] = true;
                        Main.projectile[proj].netUpdate = true;
                    }
                }

                Projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector).PerfectNormalize();
                Projectile.netUpdate = true;
                Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize();

                Projectile.DProj().Times[1] = Projectile.DProj().vector[0].ToRotation();
                Projectile.HoldProj(player, 40, Projectile.DProj().Times[1], new Vector2(1, 0), MathHelper.PiOver4, 0.25F, true);
                //player.heldProj = -1;

                if (Projectile.ai[1] < 1)
                {
                    Projectile.ai[1] += 0.02f;
                    Projectile.ai[0] = 0;
                }
                else if (Projectile.ai[0] > player.ActiveItem().useAnimation && Main.myPlayer == Projectile.owner)
                {
                    Projectile.ai[0] += player.GetTotalAttackSpeed(Projectile.DamageType);
                    int A = player.ItemMana();
                    Projectile.ai[0] -= player.ActiveItem().useAnimation;
                    player.statMana -= A;
                    for (int a = 0; a < 1; a++)
                    {
                        int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - new Vector2(Main.rand.NextFloat(-280, 280), Main.rand.NextFloat(300, 460)), Projectile.velocity * 20, ModContent.ProjectileType<HallowedFormation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[proj].netUpdate = true;
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
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, Projectile.scale/4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, Projectile.scale/4, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, Projectile.scale/4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, Projectile.scale/4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, Projectile.scale/4, 0, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.scale/4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.scale/4, (SpriteEffects)(-1), 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(236, 220, 19, 0) * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.scale/4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(236, 220, 19, 0) * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.scale/4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(236, 220, 19, 0) * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.scale/4, (SpriteEffects)(-1), 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)(-1), 0f);
            }
            return false;
        }

    }
    public class HallowedFormation : ModProjectile
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
            if (Projectile.DProj().Bool[1])
            {
                int projId = -1;
                for (int a = 0; a < 1000; a++)
                {
                    Projectile projectile = Main.projectile[a];
                    if (projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<HallowedStaff>())
                    {
                        projId = projectile.whoAmI;
                        break;
                    }
                }
                if(projId>=0)
                Projectile.ai[0] = projId;
                Projectile.Center = player.Center;
                if (Main.projectile[(int)Projectile.ai[0]].active && Main.projectile[(int)Projectile.ai[0]].type == ModContent.ProjectileType<HallowedStaff>())
                {
                    Projectile.ai[1] = Main.projectile[(int)Projectile.ai[0]].ai[1];
                }
                else
                {
                    Projectile.ai[1] -= 0.05f;
                    if (Projectile.ai[1] < 0.01F)
                    {
                        Projectile.Kill();
                    }
                }
                Lighting.AddLight(player.Center, new Color(236, 220, 19).ToVector3() * Projectile.ai[1]);
                if (Main.netMode != 2)
                {
                    Texture2D texture = DDTextures.VoidStar.Value;
                    int Type = 57;
                    Dust dust = Main.dust[NewDust(player.Center + new Vector2(-texture.Width * 0.35F / 2 * Projectile.ai[1], player.height / 2), (int)(texture.Width * 0.35F * Projectile.ai[1]), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 1.2f;
                    dust.velocity = new Vector2(0, -5F);
                    dust.color = new Color(99, 74, 187, 0);
                    dust.noLightEmittence = false;

                }
            }
            else
            {
                if (!Projectile.DProj().Bool[0])
                {
                        Projectile.rotation = (Projectile.Player().Dplayer().MouseWorld - Projectile.Center).ToRotation();
                    Projectile.scale += 0.05f;
                    if (Projectile.scale > 1F)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            if (!player.Aplayer().HolyEnergy)
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 12, ModContent.ProjectileType<魔法神圣弹>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI, 2);
                            else
                            {
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2().RotatedBy(Main.rand.NextFloat(-0.5F,0.5F)) * 12, ModContent.ProjectileType<魔法神圣弹>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI, 1);
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2().RotatedBy(Main.rand.NextFloat(-0.5F,0.5F)) * 12, ModContent.ProjectileType<魔法神圣弹>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI, 1);
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2().RotatedBy(Main.rand.NextFloat(-0.5F,0.5F)) * 12, ModContent.ProjectileType<魔法神圣弹>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI, 1);
                            }
                        }
                        int Type = 57;
                        for (int A = 0; A < 80; A++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.position - Projectile.velocity.PerfectNormalize() * Projectile.height, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                            dust.noGravity = true;
                            dust.scale = 1.8f;
                            dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 5f)*new Vector2(0.2f,1F)).RotatedBy(Projectile.rotation);
                            dust.color = new Color(99, 74, 187, 0);
                            dust.noLightEmittence = false;
                        }

                        Projectile.DProj().Bool[0] = true;
                    }
                }
                else
                {
                    Projectile.scale -= 0.1f;
                    if (Projectile.scale < 0.01F)
                    {
                        Projectile.Kill();
                    }
                }
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Projectile.Player();
            Projectile.DProj().Times[0] += 0.05F;
            Texture2D texture2 = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Color color2 = Projectile.GetAlpha(new Color(236, 220, 19, 0));
            Texture2D texture = DDTextures.VoidStar.Value;
            Texture2D texture3 = DDTextures.Scanning2.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Color color = Projectile.GetAlpha(new Color(236, 220, 19, 0));
            Main.spriteBatch.Draw(texture, vector, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale / 3, Projectile.scale), 0, 0f);
            Main.spriteBatch.Draw(texture, vector, null, color * 0.5f, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale / 3, Projectile.scale), 0, 0f);

            Main.spriteBatch.Draw(texture3, vector + Projectile.rotation.ToRotationVector2() * (10 * Projectile.scale), null, color, Projectile.rotation + MathHelper.PiOver2, texture3.Size() / 2, new Vector2(Projectile.scale * 1.2F, Projectile.scale), 0, 0f);
            Main.spriteBatch.Draw(texture3, vector + Projectile.rotation.ToRotationVector2() * (10 * Projectile.scale), null, new Color(20, 55, 240, 0), Projectile.rotation + MathHelper.PiOver2, texture3.Size() / 2, new Vector2(Projectile.scale * 1.2F, Projectile.scale) / 2, 0, 0f);

            DDHelper.Compression(texture2, color, Projectile.rotation, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);

            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.DProj().Bool[1])
            {
                Projectile.DProj().Times[1] += 0.02F;
                vector = player.Center + new Vector2(0, player.height / 2).RotatedBy(player.fullRotation) - Main.screenPosition;
                Main.spriteBatch.Draw(texture, vector, null, color, player.fullRotation + MathHelper.PiOver2, texture.Size() / 2, new Vector2(0.75F * Projectile.ai[1] / 3, 0.75F * Projectile.ai[1]), 0, 0f);
                Main.spriteBatch.Draw(texture, vector, null, color * 0.5F, player.fullRotation + MathHelper.PiOver2, texture.Size() / 2, new Vector2(0.75F * Projectile.ai[1] / 3, 0.75F * Projectile.ai[1]), 0, 0f);
                Main.spriteBatch.Draw(texture3, vector - new Vector2(0, 10).RotatedBy(player.fullRotation), null, color * 0.7f, player.fullRotation, texture3.Size() / 2, new Vector2(1.2F, 1) * 0.75F * Projectile.ai[1], 0, 0f);
                Texture2D Starlight = DDTextures.Starlight.Value;
                DDHelper.BackAndForth(0.8F, 1.2F, 0.04F, ref Projectile.localAI[0], ref Projectile.DProj().Bool[2]);
                Main.spriteBatch.Draw(Starlight, vector - new Vector2(0, 40).RotatedBy(player.fullRotation), null, new Color(236, 200, 19, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * 2 * Projectile.ai[1] * Projectile.localAI[0], 0, 0f);
                Main.spriteBatch.Draw(Starlight, vector - new Vector2(0, 40).RotatedBy(player.fullRotation), null, new Color(20, 55, 240, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * Projectile.ai[1] * Projectile.localAI[0], 0, 0f);

                DDHelper.Compression(texture2, color2, player.fullRotation + MathHelper.PiOver2, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.DProj().Times[1], BlendState.Additive);

                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1], 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
    }

}