using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items;
using DDmod.Content.Items.Melee.Sword;
using DDmod.NoContent.Config;
using DDmod.Players;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.UI;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class 哈迪斯之刃 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 700;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.ownerHitCheck = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            Projectile.scale = 0.1f;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];

            Projectile.MeleeProj().oldVels2 = 60;
            Projectile.extraUpdates = 9;
            Main.projFrames[Type] = 9;
            Projectile.noEnchantments = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        int proj = 0;
        float SprintChop = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(Ti);
            writer.Write(Special);
            writer.Write(Special2);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
            Ti = reader.ReadInt32();
            Special = reader.ReadBoolean();
            Special2 = reader.ReadBoolean();
        }
        public Vector2[] oldPos = new Vector2[20];
        public float ro = 0;
        public bool roT;
        public bool Special = false;
        public bool Special2 = false;
        public int Ti = 0;
        public override bool PreAI()
        {
            Item item = Projectile.Player().ActiveItem();
            //大小加成
            Player player = Main.player[Projectile.owner];
            if (SprintChop == 0)
            {
                if (player.dashDelay < 0)
                {
                    if (Math.Abs(player.velocity.X) > 5)
                    {
                        SprintChop = Math.Abs(player.velocity.X) / 5;
                        //player.ChangeDir(player.Center.X - player.Dplayer().MouseWorld.X > 0 ? 1 : -1);
                    }
                }
                if (SprintChop == 0)
                {
                    SprintChop = -1;
                }
            }
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(TextureAssets.Item[item.type].Width() * Projectile.scale) / 10, (int)(TextureAssets.Item[item.type].Size().Length() / Main.projFrames[Type] / 2 * Projectile.scale * 1.2f));

            if (player.controlUseTile&& !Special && !Special2&& !player.HasBuff(ModContent.BuffType<SpecialAttackCD>()))
            {
                Special = true;
                player.AddBuff(ModContent.BuffType<SpecialAttackCD>(), 600);
                Projectile.DProj().vector[2] = player.Center;
                Projectile.netUpdate = true;
            }
            if (!Special)
            {
                Projectile.HoldProj(player, 46 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                Special2 = true;
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, 2.5f, true);
                Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            }
            else
            {
                player.velocity = new Vector2(0, 0.0001F);
                Projectile.HoldProj(player, 46 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[1], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                Projectile.ai[1] = 12;
                Projectile.localAI[1] = 1;
                Ti++;
                if (Ti == 100)
                {
                    //Projectile.DPoroj().vector[0] = (Projectile.DPoroj().vector[2] - player.Center).PerfectNormalize();
                }
                if (Ti >= 100)
                {
                    float sp = 0;
                    if (!roT)
                    {
                        ro += 0.06F;
                        if(ro>MathHelper.Pi)
                        {
                            ro = 0;
                            roT = true;
                        }

                        Projectile.DProj().vector[1] = Projectile.DProj().vector[0].RotatedBy(ro) * 6;
                    }
                    else
                    {
                        Projectile.DProj().vector[0] = (Projectile.DProj().vector[2] - player.Center).PerfectNormalize();
                        sp = (Projectile.DProj().vector[2] - player.Center).Length() / 100;
                        if (sp < 5)
                        {
                            sp = 5;
                        }
                        Projectile.DProj().vector[1] = Projectile.DProj().vector[0] * sp;
                    }
                    player.position += Projectile.DProj().vector[1];
                    if ((Projectile.DProj().vector[2] - player.Center).Length() < 5)
                    {
                        Projectile.Kill();
                    }
                }
                else
                {
                    Projectile.DProj().vector[1] = Projectile.DProj().vector[0] * 8;
                    player.position += Projectile.DProj().vector[1];
                }
                player.dashDelay = 5;
                Projectile.spriteDirection = Projectile.DProj().vector[1].X > 0 ? 0 : 1;
                player.ChangeDir(Projectile.DProj().vector[1].X > 0 ? 1 : -1);
                player.Dplayer().Bossperspective(Projectile.DProj().vector[2], 1, false,0);
                Projectile.velocity = Vector2.Zero;
                DDPlayer.移动玩家(player, 30, true);

            }
            //player.ChangeDir(player.Center.X - player.Dplayer().MouseWorld.X > 0 ? -1 : 1);
            int useTime = (int)(player.HeldItem.useTime / player.GetTotalAttackSpeed(DamageClass.Melee));
            if (Projectile.localAI[1] >= 0 && Projectile.ai[1] == 2 && Projectile.DProj().Times[2] < useTime)
            {
                Projectile.ai[1] = 3;
                PlaySound(SoundID.Item1, Projectile.position);
            }
            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0&&!Special)
                {
                    proj++;
                    NewProjectile(Projectile.GetSource_FromAI(), player.Center, Projectile.DProj().vector[0] * 3, ModContent.ProjectileType<哈迪斯剑气>(), Projectile.damage, 5, Projectile.owner,ai2:1.3F);
                }
            }
            for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(4 * Projectile.scale))
            {
                if (Main.rand.NextBool(10) && Projectile.localAI[1] >= 1 && Projectile.Player().magmaStone)
                {
                    int Type = 6;
                    Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 3;
                    dust.scale = Main.rand.NextFloat(1F, 2F);
                }
            }
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref HitModifiers modifiers)
        {

            Player player = Projectile.Player();
            Item item = Projectile.Player().ActiveItem();
            if (SprintChop > 0)
            {
                modifiers.FinalDamage += SprintChop;
                //damage += (int)(damage * SprintChop);
                if (target.knockBackResist != 0)
                    modifiers.Knockback += SprintChop;
            }
            if (Special)
            {
                modifiers.FinalDamage += 5;
            }
            float armorPenetrationPercent = 0f;

            if (item.type == 5129 && target.isLikeATownNPC)
            {
                armorPenetrationPercent = 1f;
                if (target.type == 18)
                    modifiers.TargetDamageMultiplier *= 2;
            }

            modifiers.ArmorPenetration += player.GetWeaponArmorPenetration(item);
            modifiers.ScalingArmorPenetration += armorPenetrationPercent;
            ItemLoader.ModifyHitNPC(item, Projectile.Player(), target, ref modifiers);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Projectile.Player();
            Item item = Projectile.Player().ActiveItem();

            int num6 = Main.DamageVar(damageDone, player.luck);

            Type T = player.GetType();
            var m = T.GetMethod("ApplyNPCOnHitEffects", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] o = new object[7];
            o[0] = player.ActiveItem();
            o[1] = new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, Projectile.width, Projectile.height);
            o[2] = Projectile.damage;
            o[3] = Projectile.knockBack;
            o[4] = target.whoAmI;
            o[5] = num6;
            o[6] = damageDone;
            m.Invoke(player, o);

            Projectile.netUpdate = true;
            Projectile.Player().StatusToNPC(item.type, target.whoAmI);
            if (target.life > 5)
                Projectile.Player().OnHit(target.Center.X, target.Center.Y, target);
            ItemLoader.OnHitNPC(item, Projectile.Player(), target,hit,damageDone);
            if (ModContent.GetInstance<DDConfigClient>().SwordHit)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/Sword");
                sound.Pitch = 0;
                PlaySound(sound, target.position);
            }
            if (player.ActiveItem().type == ItemID.LightsBane)
            {
                return;
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = item.GetGlobalItem<MeleeGlobalItem>().Color; ;
            if (SprintChop > 0)
            {
                Main.projectile[A].scale += SprintChop / 2;
                if (target.knockBackResist != 0)
                {
                    target.velocity.Y -= 3;
                }
                Main.LocalPlayer.Dplayer().PlayerShake(5, 10);
                player.immune = true;
                player.hurtCooldowns[1] = 30;
                player.immuneTime = 30;
                player.immuneNoBlink = true;
                if (Projectile.Player().dashType != 2)
                {
                    Projectile.Player().velocity.X = (target.Center - player.Center).PerfectNormalize().X * -SprintChop;
                    if (Projectile.Player().velocity.Y != 0)
                    {
                        Projectile.Player().velocity.Y = -SprintChop;
                    }
                }
                SprintChop = -1;
            }
        }

        Color color = new Color(0, 0, 0, 0);
        public float TWidth()
        {
            return TextureAssets.Projectile[Type].Size().Length() /Main.projFrames[Type]/ 2 * Projectile.scale-2;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
                DDmod.SyncData(DDType.PlayerCenter, Projectile.Player().whoAmI, -1, Projectile.Player().whoAmI);
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            Item item = Projectile.Player().ActiveItem();
            Player player = Projectile.Player();
            color = item.GetGlobalItem<MeleeGlobalItem>().Color;
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }
            DDHelper.BladeTrail(DDTextures.WhitePng, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
            DDHelper.BladeTrail(DDTextures.Wave, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (Special)
            {
                if (!Main.gamePaused)
                {
                    oldPos[0] = Projectile.position;
                    for (int i = oldPos.Length - 1; i > 0; i--)
                    {
                        oldPos[i] = oldPos[i - 1];
                    }
                }
                if (TrailDrawer2 == null)
                {
                    TrailDrawer2 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
                }
                GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
                GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
                TrailDrawer2.Draw(oldPos, Projectile.Size * 0.5f - Main.screenPosition- Projectile.DProj().vector[1].PerfectNormalize() * 44, 104, null);

            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                Texture2D texture = TextureAssets.Projectile[Type].Value;
                Vector2 Center = Projectile.Center - Main.screenPosition;

                Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Type]));
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Type]) / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width, texture.Height / Main.projFrames[Type]) / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                if (Special)
                {
                    Center -= Projectile.DProj().vector[1].PerfectNormalize() * 44;
                    Color color2 = color;
                    color2.A = 0;
                    texture = DDTextures.GlowEffect.Value;
                    Main.spriteBatch.Draw(texture, Center, null, color2, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale / 1.5F, (SpriteEffects)Projectile.spriteDirection, 0f);
                    texture = DDTextures.VoidStar.Value;
                    Main.spriteBatch.Draw(texture, Center, null, color2, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale * 1.2F, (SpriteEffects)Projectile.spriteDirection, 0f);
                    color2 = new Color(255 - color.R, 255 - color.G, 255 - color.B, 0);
                    Main.spriteBatch.Draw(texture, Center, null, color2, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale * 1.2F / 2, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
        internal Trailing TrailDrawer2;
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(0, 230, 255),
                new Color(0, 210, 255),
                new Color(0, 190, 255),
                new Color(0, 170, 255),
                new Color(0, 150, 255),
                new Color(0, 130, 255),
                new Color(0, 110, 255),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(0, 100, 255), (float)Math.Pow((double)completionRatio, 1.0)); ;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 50f * Projectile.scale, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
    }
    public class 哈迪斯剑气 : ModProjectile
    {
        public override string Texture => "DDmod/Content/Projectiles/Melee/SwordWave5";
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1300;
            Projectile.extraUpdates = 12;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.timeLeft < 25)
            {
                Projectile.extraUpdates = 0;
            }
            else if (Projectile.timeLeft > 1260)
            {
                Projectile.Center = player.Center + Projectile.velocity.PerfectNormalize() * 50;
            }
            /*
            if(Projectile.localAI[2]==0)
            {
                Projectile.localAI[2] = Main.rand.NextFloat(0.7F, 1.6F);
                Projectile.netUpdate = true;
            }*/
            float A = 1;
            if (Projectile.timeLeft < 25)
            {
                A = (float)Projectile.timeLeft / 25;
            }
            else if (Projectile.timeLeft > 1260)
            {
                A = 0;
            }
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = Projectile.scale;
            }
            if (Projectile.localAI[1] == 0)
            {
                Projectile.localAI[1] = Projectile.damage;
            }
            Projectile.scale = Projectile.ai[2];
            Projectile.damage = (int)(Projectile.localAI[1] * (A));
            Projectile.ProjScaleChange();
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[1] < 4)
            {
                Projectile.ai[1]++;
            }
            else
            {
                if (Projectile.timeLeft > 25)
                {
                    Projectile.timeLeft = 25;
                }
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(0, 50, 205,150);
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int a = -1; a <= 1; a++)
            {
                Vector2 vector = new(projHitbox.X, projHitbox.Y);
                vector += Projectile.velocity.RotatedBy(MathHelper.PiOver2).PerfectNormalize() * (projHitbox.Width * a);
                if (new Rectangle((int)vector.X, (int)vector.Y, projHitbox.Width, projHitbox.Height).Intersects(targetHitbox))
                {
                    return new bool?(true);
                }
            }
            return new bool?(false);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(0, 50, 205, 150);
            if (Projectile.ai[0] == 0)
            {
                if (Projectile.timeLeft < 25)
                {
                    color *= (float)Projectile.timeLeft / 25;
                }
                else if (Projectile.timeLeft > 1260)
                {
                    color *= 0;
                }
            }
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(2.4F, 0.8f), 0, 0);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(2.4F, 0.8f), 0, 0);
            color.A = 0;
            return false;
        }
    }
}