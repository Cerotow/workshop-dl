using DDmod.Content.Items;
using Terraria.Graphics.Shaders;
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Players;

namespace DDmod.Content.Projectiles.Melee.TwinSwords
{
    public abstract class 双刀 : ModProjectile
    {
        public virtual void Defaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.ownerHitCheck = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            Projectile.scale = 1f;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.noEnchantments = true;
            Projectile.extraUpdates = 6;
            Defaults();
        }
        public byte proj = 0;
        public byte TZ = 30;
        /// <summary>
        /// 刀光长度
        /// </summary>
        public float EffectLength = 10;
        /// <summary>
        /// 偏移,越大武器越远
        /// </summary>
        public float HandheldOffset = 30;
        /// <summary>
        /// 攻击扇形范围
        /// </summary>
        public float range = 1.5f;
        public int Attack = 0;
        public virtual Color color => new Color(0, 0, 0, 0);
        public Player player => Projectile.Player();

        public short Time = 0;
        public byte npc = 255;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(TZ);
            writer.Write(Time);
            writer.Write(npc);
            writer.Write(X);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadByte();
            TZ = reader.ReadByte();
            Time = reader.ReadInt16();
            npc = reader.ReadByte();
            X = reader.ReadBoolean();
        }
        public virtual void Dust()
        {
            return;
            for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(4 * Projectile.scale))
            {
                Lighting.AddLight(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), color.ToVector3());
                Lighting.AddLight(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), color.ToVector3());
                if (Main.rand.NextBool(30) && Projectile.localAI[1] >= 1)
                {
                    int Type = 6;
                    Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 2;
                    dust.scale = 1.5f;
                }
            }
        }
        public virtual void Shoot()
        {
        }
        public virtual void PreSwing()
        {
            //第二刀
            if (Projectile.ai[2] == 1)
            {
                Projectile.MeleeProj().Anti = true;
            }
            //第三刀
            if (Projectile.ai[2] == 2)
            {
                Projectile.extraUpdates = 12;
                if (Projectile.DProj().Back == -1 && TZ > 0)
                {
                    Projectile.MeleeProj().Anti = true;
                    TZ = 0;
                }
            }
            //前斩
            if (Projectile.ai[2] == 3)
            {
                Projectile.extraUpdates = 12;
                if (Projectile.DProj().Back == -1 && TZ > 0)
                {
                    Projectile.MeleeProj().Anti = true;
                    TZ = 0;
                }
            }
            Projectile.scale = 0;
        }
        public virtual void Trigger()
        {
            PlaySound(SoundID.Item1, Projectile.position);
        }
        public virtual void SpecialShoot()
        {

        }
        public bool X;
        public void SpecialAttack(int AttackTime,int CD = 120)
        {
            X = false;
            if (Projectile.DProj().track == 40 && Projectile.DProj().Bool[0] && Projectile.DProj().Back == 1)
            {
                player.velocity = Vector2.Zero;
            }
            Projectile.DProj().Times[1]++;
            if (Projectile.ai[2] == 3)
            {
                if (Projectile.DProj().Times[1] < AttackTime && Projectile.DProj().Bool[0])
                {
                    Projectile.ai[0] = 0;
                }
                else
                {
                    Projectile.DProj().Times[1] = AttackTime;
                    if (!Projectile.DProj().Bool[2])
                    {

                        player.velocity = -Projectile.DProj().vector[2].PerfectNormalize() * 4;
                        player.velocity.Y -= 2;
                        Projectile.DProj().Bool[1] = false;
                        Projectile.DProj().Bool[2] = true;
                    }
                }
                if (Projectile.DProj().Back == 1)
                {
                    Main.projectile[Projectile.DProj().Other].DProj().Times[1] = Projectile.DProj().Times[1];
                }
            }
            if (Projectile.DProj().Bool[1] && Projectile.DProj().Back == 1)
            {
                player.fullRotation = 0;
                Time++;
                if (Time > 16)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        SpecialShoot();
                    }
                    Time = 1;
                    //弹反
                    player.velocity = -Projectile.DProj().vector[2].PerfectNormalize() * 6;
                    player.velocity.Y -= 2;
                    Projectile.DProj().Bool[1] = false;
                    Projectile.DProj().Bool[3] = true;
                }
                else
                {
                    player.dashDelay = 5;
                    player.velocity.Y = -0.001f;
                    player.velocity.X = -0.001f;
                    player.position += Collision.TileCollision(player.position, Projectile.DProj().vector[2].PerfectNormalize() * 26 / (Projectile.extraUpdates + 1), player.width, player.height, true, true);
                    player.Aplayer().NoGravity = 2;
                    DDPlayer.移动玩家(player, 10, true);
                }
            }
            if (Projectile.DProj().Bool[0] && Projectile.DProj().Times[1] > 30)
            {
                if (Projectile.DProj().Back == 1)
                {
                    if (!player.HasBuff(ModContent.BuffType<SpecialAttackCD>()))
                    {
                        player.AddBuff(ModContent.BuffType<SpecialAttackCD>(), CD);
                    }
                    if (Projectile.owner == Main.myPlayer && Projectile.DProj().vector[2] == Vector2.Zero)
                    {
                        Projectile.DProj().MouseWorld = Main.MouseWorld;
                        Projectile.DProj().vector[2] = Projectile.DProj().MouseWorld - player.Center;
                        Projectile.netUpdate = true;
                    }
                    if (Math.Abs(Projectile.ai[0]) == 0)
                    {
                        player.dashDelay = 5;
                        player.velocity = Projectile.DProj().vector[2].PerfectNormalize() * 26 / (Projectile.extraUpdates + 1);
                        player.position += Collision.TileCollision(player.position, Projectile.DProj().vector[2].PerfectNormalize() * 26 / (Projectile.extraUpdates + 1), player.width, player.height, true, true);
                        player.Aplayer().NoGravity = 2;
                        DDPlayer.移动玩家(player, 30, true);
                    }
                }
                if (Math.Abs(Projectile.ai[0]) == 0)
                    X = true;
            }
        }
        public Vector2 ExtraLength;
        public override void AI()
        {

            if (Projectile.ai[1] < 2)
            {
                PreSwing();
            }
            else
            {
                //大小加成
                if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
                {
                    Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
                }
            }

            Projectile.Resize((int)((Projectile.OriginalWidth()+ ExtraLength.X) * Projectile.scale), (int)((Projectile.OriginalHeight() + ExtraLength.Y) * Projectile.scale));
            if (Projectile.DProj().Bool[0])
            {
                Projectile.width = (int)(Projectile.width * 1.5F);
                Projectile.height = (int)(Projectile.height * 1.5F) ;
            }
            if (Projectile.DProj().Back == 1)
            {
                if (TZ > 0)
                {
                    TZ--;
                    return;
                }
                Projectile.HoldProj(player, HandheldOffset * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false, false);
            }
            else
            {
                Projectile.HoldProj(player, HandheldOffset * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false, false);
            }
            float AttackSpeed = -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1);
            if (Attack>0)
            {
                AttackSpeed = Attack;
            }

            if (range > MathHelper.PiOver2)
            {
                Projectile.HoldSword2(player, AttackSpeed, 120, range, true, true);
            }
            else
            {
                Projectile.HoldSword2(player, AttackSpeed, -1, range, true, true);
            }


            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0)
                {
                    Trigger();
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Shoot();
                    }
                    proj++;
                }
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            Dust();
            if (X)
            {
                for (int A = 0; A < Projectile.MeleeProj().oldVels.Length; A++)
                {
                    Projectile.MeleeProj().oldVels[A] = Vector2.Zero;
                }
            }
        }
        public virtual float TWidth()
        {
            return EffectLength * Projectile.scale;
        }
        public virtual void Draw(Texture2D texture,Vector2 Center, Color lightColor)
        {

        }
        public bool BladeGlow = false;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            Color color = this.color;
            if (!BladeGlow)
            {
                color = Lighting.GetColor((int)Projectile.Player().Center.X / 16, (int)Projectile.Player().Center.Y / 16, this.color);
                color.A = this.color.A;
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.DProj().Back == -1)
            {
                vector += new Vector2(10 * Projectile.Player().direction, 0);
            }
            DDHelper.BladeTrail(DDTextures.WhitePng, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
            DDHelper.BladeTrail(DDTextures.Wave, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);


            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;
             Vector2 Scale=new Vector2(Projectile.scale);
            //Scale=new Vector2(Projectile.scale*1F,Projectile.scale*0.5F);
            Rectangle rectangle = new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height);
            Rectangle rectangle2 = new Rectangle(0, 0, texture.Width / 2, texture.Height);
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                Draw(texture, Center, lightColor);
                if (Projectile.spriteDirection == 0)
                {
                    if (Projectile.DProj().Back == -1)
                    {
                        Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, Projectile.rotation, rectangle.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, Center, rectangle2, lightColor, Projectile.rotation, rectangle2.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                else
                {
                    if (Projectile.DProj().Back == -1)
                    {
                        Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, Projectile.rotation + MathHelper.PiOver2, rectangle.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, Center, rectangle2, lightColor, Projectile.rotation + MathHelper.PiOver2, rectangle2.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                if(TextureGlow!=null)
                {
                    lightColor = Color.White;
                    texture = TextureGlow;
                    if (Projectile.spriteDirection == 0)
                    {
                        if (Projectile.DProj().Back == -1)
                        {
                            Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, Projectile.rotation, rectangle.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                        }
                        else
                        {
                            Main.spriteBatch.Draw(texture, Center, rectangle2, lightColor, Projectile.rotation, rectangle2.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        }
                    }
                    else
                    {
                        if (Projectile.DProj().Back == -1)
                        {
                            Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, Projectile.rotation + MathHelper.PiOver2, rectangle.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                        }
                        else
                        {
                            Main.spriteBatch.Draw(texture, Center, rectangle2, lightColor, Projectile.rotation + MathHelper.PiOver2, rectangle2.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        }
                    }
                }
            }
            return false;
        }
        public virtual Texture2D TextureGlow => null;
        public override bool? CanHitNPC(NPC target)
        {          return base.CanHitNPC(target);
        }
    }
}