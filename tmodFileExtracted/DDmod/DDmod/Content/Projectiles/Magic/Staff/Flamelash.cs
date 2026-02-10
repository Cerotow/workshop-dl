using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class Flamelash : ModProjectile
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
            Projectile.DProj().Times[2] = 0;
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
            if (player.controlUseItem)
            {
                player.Dplayer().Realm = 10;
                int p = 0;
                for (int a = 0; a < 1000; a++)
                {
                    Projectile projectile = Main.projectile[a];
                    if (projectile.type>0&& projectile.active && projectile.owner == Projectile.owner && projectile.DProj().Bool[1] && projectile.type == ModContent.ProjectileType<FlamelashFormation>())
                    {
                        p++;
                    }
                }
                if (p == 0&& Projectile.ai[1]>0.25F)
                {
                    int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - new Vector2(Main.rand.NextFloat(-80, 80), Main.rand.NextFloat(100, 160)), Projectile.velocity * 12, ModContent.ProjectileType<FlamelashFormation>(), Projectile.damage, 0, Projectile.owner, Projectile.whoAmI);
                    Main.projectile[proj].DProj().Bool[1] = true;
                    Main.projectile[proj].netUpdate = true;
                }
                if(TT2>1)
                {
                    TT2 -= 0.2F;
                }
                else
                {
                    TT2 += 0.2f;
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
                player.itemTime = (int)(player.ActiveItem().useAnimation * 0.4f);
                player.itemAnimation = (int)(player.ActiveItem().useAnimation * 0.4f);
                if (player.itemTime < 2)
                {
                    player.itemTime = 2;
                }
                if (player.itemAnimation < 2)
                {
                    player.itemAnimation = 2;
                }
                //if (Projectile.ai[1] < 0.5)
                {
                    float v = 0;
                    if (player.direction == -1) v = 3.14f;
                    player.itemRotation = Projectile.velocity.ToRotation() * player.gravDir + v - player.fullRotation-MathHelper.PiOver4*(Projectile.DProj().Times[2]/15) * player.direction;
                }
                //player.heldProj = -1;
                if (Projectile.DProj().Times[2] >= 20)
                {
                    int pr = 0;
                    int pr2 = 0;
                    for (int a = 0; a < 1000; a++)
                    {
                        Projectile projectile = Main.projectile[a];
                        if (projectile.active && projectile.owner == Projectile.owner && projectile.type == 34 && projectile.ai[1] == 0)
                        {
                            pr++;
                        }
                        if (projectile.active && projectile.owner == Projectile.owner && projectile.type == 34 && projectile.ai[0] == 0 && projectile.ai[1] == 0)
                        {
                            pr2++;
                        }
                    }
                    if(pr>0&&player.controlUseTile)
                    {
                        TT2 = 3.5F;
                        SoundStyle sound = SoundID.Item20;
                        sound.Pitch = -1;
                        PlaySound(sound, Projectile.Center);
                        NewDustChangeRound(100, Projectile.position + new Vector2(Projectile.width / 2, 4).RotatedBy(player.fullRotation), 1, 6, 0, 12,true,2);
                    }
                    if (Projectile.ai[1] < 1)
                    {
                        Projectile.ai[1] += 0.05f;
                        Projectile.ai[0] = 0;
                    }
                    else if (Projectile.ai[0] > player.ActiveItem().useAnimation)
                    {
                        if (pr <= 3)
                        {
                            if (player.statMana >= player.ItemMana())
                            {
                                Projectile.ai[0] += player.GetTotalAttackSpeed(Projectile.DamageType);
                                int A = player.ItemMana();
                                player.statMana -= A;
                                Projectile.ai[0] -= player.ActiveItem().useAnimation;
                                TT2 = 2.5F;
                                if (Main.myPlayer == Projectile.owner)
                                {
                                    SoundStyle sound = SoundID.Item20;
                                    sound.Pitch = 0;
                                    PlaySound(sound, Projectile.Center);
                                    if (pr2 == 0)
                                    {
                                        int proj = NewProjectile(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Projectile.width / 2, 4).RotatedBy(player.fullRotation), Vector2.Zero, 34, Projectile.damage, 1, Projectile.owner, 0);
                                        Main.projectile[proj].netUpdate = true;
                                    }
                                    else
                                    {
                                        int proj = NewProjectile(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Projectile.width / 2, 4).RotatedBy(player.fullRotation), Vector2.Zero, 34, Projectile.damage, 1, Projectile.owner, 1);
                                        Main.projectile[proj].netUpdate = true;
                                    }
                                }
                            }
                        }
                    }
                    if(player.ownedProjectileCounts[34] >3 || player.statMana < player.ItemMana())
                    {
                        if(Projectile.ai[0] > player.ActiveItem().useAnimation)
                        {
                            Projectile.ai[0] = player.ActiveItem().useAnimation;
                        }
                        if(player.statMana < player.ItemMana())
                        {
                            TT2 = 0.6F;
                        }
                    }
                }
                else
                {
                    Projectile.DProj().Times[3]++;
                    if (Projectile.DProj().Times[3] > 20)
                    {
                        Projectile.DProj().Times[2] += 4;
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
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0) * 0.5f, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0) * 0.5f, Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0), Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0), Projectile.rotation, Glow.Size() / 2, Projectile.ai[1] / 4, 0, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * 0.5f, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * 0.5f, Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0), Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0), Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.ai[1] / 4, (SpriteEffects)(-1), 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)(-1), 0f);
            }
            Texture2D Starlight = DDTextures.Starlight.Value;
            DDHelper.BackAndForth(0.4F, 0.8F, 0.04F, ref TT, ref Projectile.DProj().Bool[4]);
            Main.spriteBatch.Draw(Starlight, Projectile.position+ new Vector2(Projectile.width/2, 4).RotatedBy(player.fullRotation) - Main.screenPosition, null, new Color(253, 62, 3, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * 2 * Projectile.ai[1] * (TT* TT2), 0, 0f);
            Main.spriteBatch.Draw(Starlight, Projectile.position + new Vector2(Projectile.width / 2, 4).RotatedBy(player.fullRotation)-Main.screenPosition,null, new Color(2, 95, 30, 0), player.fullRotation, Starlight.Size() / 2, new Vector2(1, 0.5f) * Projectile.ai[1] * (TT* TT2), 0, 0f);

            return false;
        }
        float TT;
        float TT2 = 1;

    }
    public class FlamelashFormation : ModProjectile
    {
        public override string Texture => "DDmod/Image/Circle9";
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
                    if (projectile.active && projectile.owner == Projectile.owner && projectile.type == ModContent.ProjectileType<Flamelash>())
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
                    int Type = 6;
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

                DDHelper.Compression(texture2, color2, player.fullRotation + MathHelper.PiOver2, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.DProj().Times[1], BlendState.Additive);

                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1]/4, 0, 0);
                Main.EntitySpriteDraw(texture2, vector, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, 0.75F * Projectile.ai[1]/4, 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
    }

}