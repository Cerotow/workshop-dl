using DDmod.Content.Buffs.DeBuffs;
using Terraria;

namespace DDmod.Content.Projectiles.Melee.Sword.Hammer
{
    public class TheBreaker : ModProjectile
    {
        public static Asset<Texture2D> Trailing;
        public override void Load()
        {
            Trailing = ModContent.Request<Texture2D>(Texture + "_Trailing");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            // Projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ownerHitCheck = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.scale = 1.3f;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (NPC > 0)
            {
                //手持弹幕
                if (NPC2 < 2)
                {
                    Projectile.HoldProj(player, 38 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, -0.06f * Projectile.DProj().Times[0], false, false);
                }
                else if (NPC2 < 3)
                {
                    Projectile.HoldProj(player, 38 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, -0.08f * Projectile.DProj().Times[0], false, false);
                }
                else
                {
                    Projectile.HoldProj(player, 38 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, -0.01f * Projectile.DProj().Times[0], false, false);
                }
                NPC--;
                if (NPC <= 3)
                {
                    if (NPC2 > 3)
                    {
                        Projectile.Kill();
                        player.itemTime = 0;
                        player.itemAnimation = 0;
                        return false;
                    }
                    else
                    {
                        NPC = 0;
                        NPC2++;
                    }
                }
                Projectile.extraUpdates = 0;
                return false;
            }
            //手持弹幕
            Projectile.HoldProj(player, 38 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.1f * Projectile.DProj().Times[0], false, false);
            //玩家按着左键
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            if (channeling)
            {
                if (Projectile.DProj().Times[4] < 2)
                {
                    Projectile.DProj().Times[4] = 2;
                }
                else if (Projectile.DProj().Times[4] < 8)
                {
                    Projectile.DProj().Times[4] += 0.03f * player.GetTotalAttackSpeed(Projectile.DamageType);
                }
                else if (!Projectile.DProj().Bool[4])
                {
                    SoundStyle sound = SoundID.Item29;
                    sound.Pitch = -0.3f;
                    PlaySound(sound, Projectile.position);
                    Projectile.DProj().Bool[4] = true;
                    for (int A = 0; A < 100; A++)
                    {
                        int Type = 267;
                        Dust dust = Main.dust[NewDust(player.position, player.width, player.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = 1.8f;
                        dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3, 6);
                        dust.color = new Color(99, 74, 187, 0);
                        dust.noLightEmittence = false;
                    }
                }
                Projectile.DProj().Magnification = Projectile.DProj().Times[4]/2;
                //伤害
                Projectile.netUpdate = true;
                Vector2 vector2 = player.RotatedRelativePoint(player.ArmCenter(), true);

                Projectile.ai[0] = -3.5f * player.direction;

                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.DProj().vector[0] = (Main.MouseWorld - vector2).PerfectNormalize();
                    if (Projectile.DProj().vector[0].X > 0)
                    {
                        player.ChangeDir(1);
                    }
                    else
                    {
                        player.ChangeDir(-1);
                    }
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.extraUpdates = 3;
                if (!Projectile.DProj().Bool[3])
                {
                    Projectile.DProj().Bool[3] = true;
                    PlaySound(SoundID.Item1, Projectile.position);
                }
                int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
                Projectile.damage = (int)(damageWithChargeAndStats * Projectile.DProj().Times[4]/2);
                Projectile.netUpdate = true;

                Projectile.HoldSword(player, -player.HeldItem.useAnimation, 12, 3.5f, true, true);
                if (NPC2 > 3)
                {
                    Projectile.Kill();
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    return false;
                }
            }

            Projectile.spriteDirection = Projectile.DProj().Times[0] == 1 ? 0 : 1;
            return false;
        }
        int NPC;
        int NPC2;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            if (target.HasBuff(BuffID.Bleeding))
            {
                modifiers.SourceDamage += 0.2F;
            }
            if (player.velocity.Length() > 8)
            {
                modifiers.SetCrit();
            }
            modifiers.Knockback *= 3;
           NPC = 33;
            float C = 1;

            if (NPC2 == 0)
            {
                modifiers.ModifyHitInfo += (ref HitInfo info)=>
                {
                    info.Knockback = 3;
                };
                C = 1.3f;
            }
            else
            if (NPC2 != 3)
            {
                modifiers.ModifyHitInfo += (ref HitInfo info) =>
                {
                    info.Knockback = 2.8F;
                };
                C = 0.6f;
            }
            for (int a = 0; a < 120; a++)
            {
                target.HitEffect(0, 0);
                var dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 267, 0, 0, 100, default, 1.2f)];
                dust.noGravity = true;
                dust.color = new Color(99, 74, 187, 0);
                float ro = Projectile.rotation;
                if (player.direction == -1)
                {
                    ro = Projectile.rotation + MathHelper.Pi;
                }
                dust.velocity = (ro + MathHelper.PiOver4).ToRotationVector2().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(2, 5)* C;

            }
            target.AddBuff(ModContent.BuffType<EvilEntanglement>(), Main.rand.Next(100, 300));
            SoundStyle sound = SoundID.Item45;
            sound.Pitch = -0F;
            PlaySound(sound, Projectile.position);

            player.Dplayer().PlayerShake(4, 8);
            modifiers.SourceDamage += C-1;

        }


        public override bool? CanDamage()
        {
            if (Projectile.ai[0] < 1 && Projectile.ai[0] > -1 && NPC == 0)
            {
                return null;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            SpriteEffects spriteEffects = 0;
            if (player.direction == -1)
            {
                spriteEffects = (SpriteEffects)1;
            }
            float ro;
            for (int A = 0; A < Projectile.DProj().Times[4] / 4; A++)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    ro = Projectile.oldRot[i];
                    if (player.direction == -1)
                    {
                        ro = Projectile.oldRot[i] + MathHelper.PiOver2;
                    }
                    Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12;
                    Color oldcolor = new Color(40, 40, 40, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                    Main.spriteBatch.Draw(Trailing.Value, vector2, null, oldcolor, ro, Trailing.Size() / 2, Projectile.scale, spriteEffects, 0f);
                }
            }
            ro = Projectile.rotation;
            if (player.direction == -1)
            {
                ro = Projectile.rotation + MathHelper.PiOver2;
            }
            Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition + Projectile.Size / 2 - Projectile.velocity.PerfectNormalize() * 12, null, Color.White, ro, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);

            for (int A = 0; A < Projectile.DProj().Times[4] / 4; A++)
            {
                Main.spriteBatch.Draw(Trailing.Value, Projectile.position - Main.screenPosition + Projectile.Size / 2 - Projectile.velocity.PerfectNormalize() * 12, null, new Color(55, 0, 0, 0), ro, Trailing.Size() / 2, Projectile.scale, spriteEffects, 0f);
            }
            if (!Projectile.DProj().Bool[3])
            {
                DDHelper.DrawExpanded(Main.GameViewMatrix.TransformationMatrix, Main.spriteBatch, player.Center - Main.screenPosition + new Vector2(0, -50), 0.5f, Projectile.DProj().Times[4] / 8 * 0.5f, new Color(82, 76, 128), new Color(82, 76, 128), 10F, 0.6f);
                DynamicSpriteFontExtensionMethods.DrawString(
                Main.spriteBatch,
                FontAssets.MouseText.Value,
                (int)(Projectile.DProj().Times[4] / 8 * 100) + "%",
                player.Center - Main.screenPosition - new Vector2(0, 38),
                new Color(82, 76, 128) * 2, 0f,
                ChatManager.GetStringSize(FontAssets.MouseText.Value, (int)(Projectile.DProj().Times[4] / 8 * 100) + "%", Vector2.One) / 2,
                0.75F, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}