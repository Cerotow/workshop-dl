using DDmod.Content.Projectiles.Melee.Sword;
using System.Reflection;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.TwinSwords
{
    public class 耀斑双刃Proj : 双刀
    {
        public override void Defaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 40;
            Projectile.extraUpdates = 12;
        }
        int proj = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
        }
        int TZ = 40;
        public override bool PreAI()
        {
            //大小加成
            Player player = Main.player[Projectile.owner];
            float AttackSpeed = -player.HeldItem.useAnimation * (Projectile.extraUpdates+1);

            if (Projectile.ai[1] < 2)
            {
                if(Projectile.ai[2] >=4)
                    Projectile.extraUpdates = 12;
                if (Projectile.ai[2] == 4 && TZ > 0)
                {
                    TZ = 0;
                }
                Projectile.scale = 0;
            }
            else
            {
                if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
                {
                    Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
                    if (Projectile.ai[2] == 2 || Projectile.ai[2] == 3)
                    {
                        Projectile.DProj().Times[0] = -player.direction;
                        Projectile.localAI[0] = 2 * -player.direction;
                        float U = Projectile.localAI[0] + (Projectile.localAI[1] / (AttackSpeed * 3));
                        Projectile.localAI[1]++;
                        if (Projectile.localAI[1] > 0)
                        {
                            Projectile.localAI[1] = 0;
                        }
                        Projectile.ai[0] = -U;
                    }
                }
            }

            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));
            if (Projectile.ai[2] == 0 || Projectile.ai[2] == 2 || Projectile.ai[2] == 4)
            {
                if(TZ>0)
                {
                    TZ--;
                    return false;
                }
                Projectile.HoldProj(player, 32 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0,true, 0.05f * Projectile.DProj().Times[0], false, false,false);
            }
            else
            {
                Projectile.HoldProj(player, 32 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false, false);
            }
            Projectile.HoldSword2(player, AttackSpeed, -1, 11112.2f, true);

            int useTime = (int)(player.HeldItem.useTime / player.GetTotalAttackSpeed(DamageClass.Melee));

            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0)
                {
                    player.velocity = Projectile.DProj().vector[0] * 25;
                    PlaySound(SoundID.Item1, Projectile.position);
                    proj++;
                    if (Projectile.owner == Main.myPlayer)
                    {
                    }
                }
            }
            player.fullRotation = Projectile.rotation;
            if (player.velocity.X==0|| player.velocity.Y==0)
            {
                Projectile.Kill();
                player.velocity = Vector2.Zero;
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
            if (Projectile.scale > 0.5F)
            {
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
        }
        bool DamageNPC;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;
        }

        Color color = new Color(255, 121, 3, 0)*0.7F;
        public Color TrailColor(float completionRatio)
        {
            float trailOpacity = Utils.GetLerpValue(0f, 0.1f, completionRatio, true) * Utils.GetLerpValue(0.7f, 0.58f, completionRatio, true);
            Color startingColor = Color.Lerp(color, color, 0.07f);
            return playerHelper.MulticolorLerp(completionRatio, new Color[]
            {
                startingColor,
            }) * trailOpacity;
        }
        public float TrailWidth(float completionRatio)
        {
            return 12 * Projectile.scale;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["刀光"]);
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.ai[2] == 0 || Projectile.ai[2] == 2|| Projectile.ai[2] == 4)
            {
                vector += new Vector2(10 * Projectile.Player().direction, 0);
            }
            GameShaders.Misc["刀光"].SetShaderTexture(DDTextures.WhitePng);
            GameShaders.Misc["刀光"].UseColor(Projectile.GetAlpha(color * 0.6f));
            GameShaders.Misc["刀光"].UseSecondaryColor(Projectile.GetAlpha(color) * 0.6f);
            GameShaders.Misc["刀光"].Shader.Parameters["FlameColor"].SetValue(Projectile.GetAlpha(color).ToVector3());
            GameShaders.Misc["刀光"].Shader.Parameters["flipped"].SetValue(Projectile.DProj().Times[0] > 0);
            GameShaders.Misc["刀光"].Shader.Parameters["uDarkshade"].SetValue(0);
            GameShaders.Misc["刀光"].Apply(default(DrawData?));
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null);

            GameShaders.Misc["刀光"].SetShaderTexture(DDTextures.Wave);
            GameShaders.Misc["刀光"].UseColor(Projectile.GetAlpha(color));
            GameShaders.Misc["刀光"].UseSecondaryColor(Projectile.GetAlpha(color));
            GameShaders.Misc["刀光"].Shader.Parameters["FlameColor"].SetValue(Projectile.GetAlpha(color).ToVector3());
            GameShaders.Misc["刀光"].Shader.Parameters["flipped"].SetValue(Projectile.DProj().Times[0] > 0);
            GameShaders.Misc["刀光"].Shader.Parameters["uDarkshade"].SetValue(0);
            GameShaders.Misc["刀光"].Apply(default(DrawData?));
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);


            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.spriteDirection == 0)
                {
                    if (Projectile.ai[2] == 0 || Projectile.ai[2] == 2 || Projectile.ai[2] == 4)
                    {
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height)), Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height) / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                else
                {
                    if (Projectile.ai[2] == 0 || Projectile.ai[2] == 2 || Projectile.ai[2] == 4)
                    {
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height)), Color.White, Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height) / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width / 2, texture.Height)), Color.White, Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height) / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
            }
            return false;
        }
        internal Trailing TrailDrawer;
    }
}