using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class UnholyTrident : AMagicStaff
    {
        public override int Formation => ModContent.ProjectileType<UnholyFormation>();
        public override int ProjShoot => ModContent.ProjectileType<UnholyFormation>();
        public override byte AIStyle => 2;
        public override float ProjShootSpeed => 0;
        public override void Set()
        {
            UseAnimations = 20;
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void FormationEffect(Player player)
        {


            SoundStyle sound = SoundID.Item14;
            sound.Pitch = -1;
            PlaySound(sound, Projectile.Center);
            if (Main.netMode != 2)
            {
                Texture2D texture = DDTextures.VoidStar.Value;
                int Type = 27;
                for (int a = 0; a < 80; a++)
                {
                    int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.35F / 2, player.height / 2), (int)(texture.Width * 0.35F), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale = 2.1f;
                    Main.dust[dust].velocity = new Vector2((Main.dust[dust].position.X - player.Center.X) / 10, -Main.rand.NextFloat(2, 12));
                    Main.dust[dust].noLightEmittence = false;
                    Main.dust[dust].customData = dust;
                    GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
                }
                int A = player.ItemMana();
                if (player.statMana >= A)
                {
                    player.statMana -= A;
                    for (int a = 0; a < 8; a++)
                    {
                        float R = Main.rand.NextFloat(-40, 40);

                        int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Center + new Vector2(R, 0), new Vector2(R / 5, -Main.rand.NextFloat(6, 12)), 114, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[proj].netUpdate = true;
                    }
                    for (int a = -5; a <= 5; a++)
                    {
                        if (a == 0)
                        {
                            continue;
                        }
                        int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Center - new Vector2(80*a * player.direction, -player.height/2), new Vector2(0,-1), ProjShoot, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI, 0, -1);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
            }
        }
        public override void Shoot(Player player)
        {
            int R = Main.rand.Next(200, 500);
            for (int a = -1; a <= 1; a++)
            {
                if (a == 0)
                {
                    continue;
                }
                int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Center - new Vector2(R * player.direction, Main.rand.Next(100,400)* a), Projectile.velocity * ProjShootSpeed, ProjShoot, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI,0, R *2f);
                Main.projectile[proj].netUpdate = true;
            }
            int proj2 = NewProjectile(Projectile.GetSource_FromThis(), player.Center - new Vector2(R * player.direction, 0), Projectile.velocity * ProjShootSpeed, ProjShoot, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI,0, R*2f);
            Main.projectile[proj2].DProj().Bool[2] = true;
            Main.projectile[proj2].netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            if (player.direction == 1)
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0) * 0.5f, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0) * 0.5f, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0), Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0), Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0) * 0.5f, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0) * 0.5f, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0), Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0), Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)(-1), 0f);
            }
            return false;
        }

    }
    public class UnholyFormation : ModProjectile
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
                Projectile.Center = player.Center;
                int projId = -1;
                for (int a = 0; a < 1000; a++)
                {
                    Projectile projectile = Main.projectile[a];
                    if (projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<UnholyTrident>())
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
                Lighting.AddLight(player.Center, new Color(81, 6, 233).ToVector3() * Projectile.ai[1]);
                if (Main.netMode != 2)
                {
                    Texture2D texture = DDTextures.VoidStar.Value;
                    int Type = 27;
                    int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.35F / 2 * Projectile.ai[1], player.height / 2), (int)(texture.Width * 0.35F * Projectile.ai[1]), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].scale = Main.rand.NextFloat(0.5F,0.8F);
                    Main.dust[dust].velocity = new Vector2(0, -Main.rand.NextFloat(1,2));
                    Main.dust[dust].noLightEmittence = false;
                    Main.dust[dust].customData = dust;
                    Main.dust[dust].position -= player.velocity;
                    GlobalDust.DustPlayerOwner[dust] = player.whoAmI;

                }
            }
            else
            {
                if (!Projectile.DProj().Bool[0])
                {
                    if (Projectile.ai[2] > 0)
                    {
                        Projectile.rotation = (Projectile.Player().Dplayer().MouseWorld - Projectile.Center).ToRotation();
                    }
                    else
                    {
                        Projectile.rotation = -MathHelper.PiOver2;
                    }
                    Projectile.scale += 0.05f;
                    if (Projectile.DProj().Bool[2])
                    {
                        Projectile.scale += 0.05f;
                        if (Projectile.scale > 1.5F)
                        {
                            if (Main.myPlayer == Projectile.owner)
                            {
                                int Proj = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 12, 114, Projectile.damage*2, 0, Projectile.owner, Projectile.whoAmI,0, Projectile.ai[2]);
                                Main.projectile[Proj].DProj().Magnification = 2F;
                                Main.projectile[Proj].scale = 1.5F;
                                Main.projectile[Proj].ProjScaleChange();
                                Main.projectile[Proj].netUpdate = true;
                            }
                            int Type = 27;
                            for (int A = 0; A < 160; A++)
                            {
                                Dust dust = Main.dust[NewDust(Projectile.position - Projectile.velocity.PerfectNormalize() * Projectile.height, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                                dust.noGravity = true;
                                dust.scale = 1.8f;
                                dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 5f) * new Vector2(0.4f, 2F)).RotatedBy(Projectile.rotation);
                                dust.color = new Color(99, 74, 187, 0);
                                dust.noLightEmittence = false;
                            }
                            Projectile.DProj().Bool[0] = true;
                        }
                    }
                    else
                    {
                        if (Projectile.scale > 1F)
                        {
                            if (Main.myPlayer == Projectile.owner)
                            {
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 12, 114, Projectile.damage, 0, Projectile.owner, Projectile.whoAmI,0, Projectile.ai[2]);
                            }
                            int Type = 27;
                            for (int A = 0; A < 80; A++)
                            {
                                Dust dust = Main.dust[NewDust(Projectile.position - Projectile.velocity.PerfectNormalize() * Projectile.height, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                                dust.noGravity = true;
                                dust.scale = 1.8f;
                                dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 5f) * new Vector2(0.2f, 1F)).RotatedBy(Projectile.rotation);
                                dust.color = new Color(99, 74, 187, 0);
                                dust.noLightEmittence = false;
                            }
                            Projectile.DProj().Bool[0] = true;
                        }
                    }
                    
                }
                else
                {
                    Projectile.scale -= 0.1f;
                    if (Projectile.DProj().Bool[2])
                    {
                        Projectile.scale -= 0.1f;
                    }
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
            Color color2 = Projectile.GetAlpha(new Color(81, 6, 233, 0));
            Texture2D texture = DDTextures.VoidStar.Value;
            Texture2D texture3 = DDTextures.Scanning2.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Color color = Projectile.GetAlpha(new Color(81, 6, 233, 0));
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
                Main.spriteBatch.Draw(Starlight, vector - new Vector2(0, 40).RotatedBy(player.fullRotation), null, new Color(81, 6, 233, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * 2 * Projectile.ai[1] * Projectile.localAI[0], 0, 0f);
                Main.spriteBatch.Draw(Starlight, vector - new Vector2(0, 40).RotatedBy(player.fullRotation), null, new Color(81, 6, 233, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * Projectile.ai[1] * Projectile.localAI[0], 0, 0f);

                DDHelper.Compression(texture2, color2, player.fullRotation + MathHelper.PiOver2, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.DProj().Times[1], BlendState.Additive);

                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1], 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
    }

}