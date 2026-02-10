using DDmod.Content.Items;
using Terraria.Graphics.Shaders;
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Players;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using static AssGen.Assets;

namespace DDmod.Content.Projectiles.Melee.SwordShield
{
    public abstract class 剑盾 : ModProjectile
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
            Projectile.scale = 1f;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.noEnchantments = true;
            Defaults();
            
        }
        public int Gedang = 0;
        public int proj = 0;
        public int TZ = 30;
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

        public int Time = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(TZ);
            writer.Write(Time);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
            TZ = reader.ReadInt32();
            Time = reader.ReadInt32();
        }
        public virtual void Dust()
        {

        }
        public virtual void Shoot()
        {
        }
        public virtual void Trigger()
        {
            PlaySound(SoundID.Item1, Projectile.position);
        }
        public virtual void SpecialShoot()
        {

        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if(Projectile.DProj().Back == 1)
            {
                overPlayers.Add(index);
            }
        }
        public void SpecialAttack(int AttackTime,int CD = 120)
        {
        }
        public override void AI()
        {
            //大小加成
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }

            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

            //盾牌
            if (Projectile.DProj().Back == 1)
            {
                int itemTime = player.itemTime;
                int itemAnimation = player.itemAnimation;
                Projectile.HoldProj(player, 8, 0, new Vector2(1 * player.direction, 0.3F), MathHelper.PiOver2, 0, true, 0, false, false, false);
                player.PlayerAction().PlayerArmRotation(Projectile.velocity.ToRotation()-MathHelper.PiOver2,0);

                Projectile.rotation = -MathHelper.PiOver2;
                if (player.controlUseTile)
                {
                    player.Dplayer().ShieldDefense = 5;
                }
                player.itemTime = itemTime + 2;
                player.itemAnimation = itemAnimation + 2;
                if (Projectile.owner==Main.myPlayer&&player.Dplayer().ShieldDefense==0)
                {
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    Projectile.Kill();
                    if(player.Dplayer().ShieldCD<30)
                    player.Dplayer().ShieldCD = 30;
                }
                Projectile.ai[1] = 3; Projectile.DProj().Times[0] = 1;
                if (player.direction==-1)
                {
                    Projectile.DProj().Times[0] = -1;
                }
                Projectile.rotation = 0;
            }
            //剑
            else
            {
                Projectile.HoldProj(player, HandheldOffset * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false, false);
                float AttackSpeed = -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1);
                if (Attack > 0)
                {
                    AttackSpeed = Attack;
                }
                Projectile.HoldSword2(player, AttackSpeed, -1, range, true, true);
                player.PlayerAction().PlayerArmRotation(player.Aplayer().ArmSwing * player.direction, 0);
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
        }
        public float TWidth()
        {
            return EffectLength;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            byte A = this.color.A;
            Color color = Lighting.GetColor((int)Projectile.Player().Center.X/16, (int)Projectile.Player().Center.Y / 16, this.color);
            color.A = A;
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.DProj().Back == -1)
            {
                vector += new Vector2(10 * Projectile.Player().direction, 0);

                DDHelper.BladeTrail(DDTextures.WhitePng, color, 1F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
                DDHelper.BladeTrail(DDTextures.Wave, color, 1F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;
             Vector2 Scale=new Vector2(Projectile.scale);
            //Scale=new Vector2(Projectile.scale*1F,Projectile.scale*0.5F);
            Time += 1;
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.spriteDirection == 0)
                {
                    if (Projectile.DProj().Back == -1)
                    {
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                    }
                    else
                    {
                        float Sc = (float)Time / 100F;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);
                        Sc += 0.33f;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);
                        Sc += 0.33F;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);

                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                else
                {
                    if (Projectile.DProj().Back == -1)
                    {
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height)), lightColor, Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                    }
                    else
                    {
                        float Sc = (float)Time / 100F;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);
                        Sc += 0.33f;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);
                        Sc += 0.33F;
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor * 0.4F * (1 - Sc % 1), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale * (1 + Sc % 1), (SpriteEffects)Projectile.spriteDirection, 0f);

                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
            }
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {          return base.CanHitNPC(target);
        }

    }
}