


namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class InfernoFork : AMagicStaff
    {
        public override int Formation => ModContent.ProjectileType<InfernoForkFormation>();
        public override int ProjShoot => ModContent.ProjectileType<InfernoForkFormation>();
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
                int Type = 6;
                for (int a = 0; a < 80; a++)
                {
                    int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.35F / 2, player.height / 2), (int)(texture.Width * 0.35F), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale = 2;
                    Main.dust[dust].velocity = new Vector2((Main.dust[dust].position.X - player.Center.X) / 10, -Main.rand.NextFloat(2, 12));
                    Main.dust[dust].noLightEmittence = false;
                    Main.dust[dust].customData = dust;
                    GlobalDust.DustPlayerOwner[dust] = player.whoAmI;
                }
            }
            for (int a = -2; a <= 2; a++)
            {
                Vector2 Pos = player.position + new Vector2(player.width / 2 + 100 * a, -16);
                for (int Y = 0; Y < 10; Y++)
                {
                    if (!Main.tile[(int)Pos.X / 16, (int)Pos.Y / 16 - Y].HasTile)
                    {
                        Pos = player.position + new Vector2(player.width / 2 + 100 * a, -16 - Y * 16);
                        break;
                    }
                }
                int A = player.ItemMana();
                if (a != 0 && player.statMana>=A)
                {
                    player.statMana -= A;
                    int proj = NewProjectile(Projectile.GetSource_FromThis(), Pos, new Vector2(0, -0.01F), ModContent.ProjectileType<InfernoForkFormation>(), Projectile.damage, 2, Projectile.owner, Projectile.whoAmI);
                    Main.projectile[proj].DProj().Times[2] = 2;
                    Main.projectile[proj].DProj().Times[3] = -600;
                    Main.projectile[proj].DProj().Bool[4] = true;
                    Main.projectile[proj].netUpdate = true;
                }
            }
        }
        public override void Shoot(Player player)
        {
            int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld + new Vector2(Main.rand.NextFloat(-30, 30), 30), new Vector2(0, -10), ModContent.ProjectileType<InfernoForkFormation>(), Projectile.damage, 2, Projectile.owner, Projectile.whoAmI);
            Main.projectile[proj].netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(253, 62, 3, 0);
            if (player.direction == 1)
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, color * 0.5f, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, color * 0.5f, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, color * 0.5f, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, color * 0.5f, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)(-1), 0f);
            }
            return false;
        }

    }
    public class InfernoForkFormation : ModProjectile
    {
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
                    if (projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<InfernoFork>())
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
                    int dust = NewDust(player.Center + new Vector2(-texture.Width * 0.7F / 2 * Projectile.ai[1], player.height / 2), (int)(texture.Width * 0.7F * Projectile.ai[1]), 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].scale = Main.rand.NextFloat(1F, 1.25F);
                    Main.dust[dust].velocity = new Vector2(0, -Main.rand.NextFloat(1, 3));
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
                    Projectile.DProj().Times[2]++;
                    if (Projectile.DProj().Times[2] > 60)
                    {
                        Projectile.DProj().Bool[0] = true;
                    }
                    else if (Projectile.DProj().Times[2] == 1)
                    {
                        Projectile.velocity = (Projectile.Player().Dplayer().MouseWorld - Projectile.Center).PerfectNormalize() * 0.01F;
                    }
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    if (Projectile.DProj().Times[2] > 10)
                    {
                        Projectile.DProj().Times[3] += 40;
                    }
                    int Type = 6;
                    
                    if (Projectile.DProj().Times[3] < 280)
                    {
                        float R = (1 - Projectile.DProj().Times[3] / 280);
                        if (R > 1) R = 1;
                        for (int A = 0; A < 10; A++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center - Projectile.velocity.PerfectNormalize() * 56 - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                            dust.noGravity = true;
                            dust.scale = 1.4f+0.5F* R;
                            dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 2f + 1 * R) * new Vector2(2f, 2F)).RotatedBy(Projectile.rotation);
                            dust.noLightEmittence = false;
                        }
                    }
                    else
                    {
                        for (int A = 0; A < 10; A++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center - Projectile.velocity.PerfectNormalize() * 56 - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                            dust.noGravity = true;
                            dust.scale = 1.4f;
                            dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 2f) * new Vector2(2f, 2F)).RotatedBy(Projectile.rotation);
                            dust.noLightEmittence = false;
                        }
                    }
                    
                        if (Projectile.DProj().Times[3] == 280)
                    {
                        SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/叉");
                        sound.Pitch = 1;
                        PlaySound(sound, Projectile.position);
                        for (int A = 0; A < 50; A++)
                        {
                            float a = Main.rand.NextFloat(-30f, 30f);
                            Dust dust = Main.dust[NewDust(Projectile.Center - Projectile.velocity.PerfectNormalize() * 60-new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                            dust.noGravity = true;
                            dust.scale = 2.3f;
                            dust.position += Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * a;
                            dust.velocity = Projectile.velocity.PerfectNormalize() * Main.rand.NextFloat(10f, 30f) * (1-Math.Abs(a)/90);
                            dust.noLightEmittence = false;
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
        public override void OnKill(int timeLeft)
        {
            if (!Projectile.DProj().Bool[1])
            {
                for (int A = -50; A < 50; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position+ Projectile.velocity.PerfectNormalize() * A, 68, 68, 6, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 1.4f;
                    dust.velocity = (Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, 2f) * new Vector2(2f, 2F)).RotatedBy(Projectile.rotation);
                    dust.noLightEmittence = false;
                }
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!Projectile.DProj().Bool[1])
            {
                if (Projectile.DProj().Times[3] < 420)
                {
                    modifiers.SourceDamage += 2;
                    if (target.knockBackResist > 0)
                    {
                        Vector2 vector = Projectile.velocity.PerfectNormalize() * 2;
                        if (Projectile.DProj().Bool[4])
                        {
                            vector = Projectile.velocity.PerfectNormalize() * 10;
                        }
                        target.velocity = vector;
                        DDmod.SyncData(DDType.NPCCenter, target.whoAmI, -1, Projectile.owner);
                    }
                }
                if (target.velocity.Y != 0)
                {
                    modifiers.Knockback *= 0;
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.DProj().Bool[1])
            {
                if (target.knockBackResist > 0)
                {
                    Vector2 vector = (target.Center - Projectile.Center).PerfectNormalize() * 8;
                    target.velocity = vector;
                    DDmod.SyncData(DDType.NPCCenter, target.whoAmI, -1, Projectile.owner);
                }
            }
            target.AddBuff(323, 180);
        }
        public override bool? CanDamage()
        {
            if (Projectile.DProj().Bool[1])
            {
                Player player = Projectile.Player();
                return !player.HasBuff(BuffID.ManaSickness);

            }
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Projectile.Player();
            Projectile.DProj().Times[0] += 0.05F;
            Texture2D texture2 = DDTextures.Circle[9].Value;
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Color color2 = Projectile.GetAlpha(new Color(253, 62, 3, 0));
            Texture2D texture = DDTextures.VoidStar.Value;
            Texture2D texture3 = DDTextures.Scanning2.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Color color = Projectile.GetAlpha(new Color(253, 62, 3, 0));

            if (Projectile.DProj().Bool[1])
            {
                Projectile.DProj().Times[1] += 0.02F;
                vector = player.Center + new Vector2(0, player.height / 2).RotatedBy(player.fullRotation) - Main.screenPosition;
                Main.spriteBatch.Draw(texture, vector, null, color, player.fullRotation + MathHelper.PiOver2, texture.Size() / 2, new Vector2(0.75F * Projectile.ai[1] / 3, 0.75F * Projectile.ai[1]), 0, 0f);
                Main.spriteBatch.Draw(texture, vector, null, color * 0.5F, player.fullRotation + MathHelper.PiOver2, texture.Size() / 2, new Vector2(0.75F * Projectile.ai[1] / 3, 0.75F * Projectile.ai[1]), 0, 0f);
                Main.spriteBatch.Draw(texture3, vector - new Vector2(0, 10).RotatedBy(player.fullRotation), null, color * 0.7f, player.fullRotation, texture3.Size() / 2, new Vector2(1.2F, 1) * 0.75F * Projectile.ai[1], 0, 0f);
                Texture2D Starlight = DDTextures.Starlight.Value;
                if (!player.HasBuff(BuffID.ManaSickness))
                {
                    DDHelper.BackAndForth(0.8F, 1.2F, 0.04F, ref Projectile.localAI[0], ref Projectile.DProj().Bool[2]);
                    Main.spriteBatch.Draw(Starlight, vector - new Vector2(0, 40).RotatedBy(player.fullRotation), null, color, player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * 2 * Projectile.ai[1] * Projectile.localAI[0], 0, 0f);
                    Main.spriteBatch.Draw(Starlight, vector - new Vector2(0, 40).RotatedBy(player.fullRotation), null, color, player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * Projectile.ai[1] * Projectile.localAI[0], 0, 0f);

                }
                else if(Projectile.localAI[0]>-3)
                {
                    Projectile.localAI[0] -= 0.4F;
                }
                else
                {
                    Projectile.localAI[0] = -3;
                }
                Main.EntitySpriteDraw(texture, vector, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color2, player.fullRotation, Utils.Size(texture) * 0.5f, new Vector2(1.25F, 2 * (1 + Projectile.localAI[0] / 3)) * Projectile.ai[1], 0, 0);
                Main.EntitySpriteDraw(texture, vector, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color2, player.fullRotation, Utils.Size(texture) * 0.5f, new Vector2(1.25F, 2 * (1 + Projectile.localAI[0] / 3)) * Projectile.ai[1], 0, 0);
                Main.EntitySpriteDraw(texture, vector, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), color2, player.fullRotation, Utils.Size(texture) * 0.5f, new Vector2(1.25F, 2 * (1 + Projectile.localAI[0] / 3)) * Projectile.ai[1], 0, 0);

                DDHelper.Compression(texture2, color2, player.fullRotation + MathHelper.PiOver2, Projectile.Opacity, new Vector2(8, 1), Projectile.direction, Projectile.DProj().Times[1], BlendState.Additive);

                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1] / 2, 0, 0);
                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1] / 2, 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                return false;
            }
            color = new Color(253, 255, 0, 0);
            texture = TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.DProj().Times[3] > texture.Width)
            {
                Main.EntitySpriteDraw(texture, vector2, null, new Color(150, 150, 150, 255), Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) * 0.5f, 0.5F, 0, 0);
                Main.EntitySpriteDraw(texture, vector2, null, color, Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) * 0.5f, 0.5F, 0, 0);
            }
            else if (Projectile.DProj().Times[3] > 0)
            {
                Rectangle rectangle = new Rectangle(texture.Width - (int)Projectile.DProj().Times[3], texture.Height - (int)Projectile.DProj().Times[3], (int)Projectile.DProj().Times[3], (int)Projectile.DProj().Times[3]);
                vector2 += Projectile.velocity.PerfectNormalize()*40*0.5F;
                Main.EntitySpriteDraw(texture, vector2, rectangle, new Color(150, 150, 150, 255), Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) * 0.5f, 0.5F, 0, 0);
                Main.EntitySpriteDraw(texture, vector2, rectangle, color, Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) * 0.5f, 0.5F, 0, 0);
            }
            //Main.EntitySpriteDraw(texture, vector2,null, color2, 0f, Utils.Size(texture) * 0.5f, 1, 0, 0);

            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

            if (Projectile.DProj().Bool[1])
            {
                return null;

            }
            if (Projectile.DProj().Times[3] < 280)
            {
                return false;
            }
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            int X = (int)(projHitbox.X + Projectile.velocity.PerfectNormalize().X * 68);
            int Y = (int)(projHitbox.Y + Projectile.velocity.PerfectNormalize().Y * 28);
            if (new Rectangle(X, Y, 68, 68).Intersects(targetHitbox))
            {
                return true;
            }
            X = (int)(projHitbox.X - Projectile.velocity.PerfectNormalize().X * 68);
            Y = (int)(projHitbox.Y - Projectile.velocity.PerfectNormalize().Y * 28);
            if (new Rectangle(X, Y, 68, 68).Intersects(targetHitbox))
            {
                return true;
            }
            return false;
        }
    }
}