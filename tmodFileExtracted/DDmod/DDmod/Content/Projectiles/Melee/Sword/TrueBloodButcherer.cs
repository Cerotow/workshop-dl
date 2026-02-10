using DDmod.Content.Buffs.DeBuffs;
using DDmod.NoContent.Config;
using DDmod.Players;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class TrueBloodButcherer : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            Projectile.width = 20;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 44;
            Projectile.extraUpdates = 6;
            Projectile.DProj().Times[4] = 2F;
        }
        int proj = 0;
        //特殊攻击
        bool Special;
        //是否给Buff
        bool Buff;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(Buff);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
            Buff = reader.ReadBoolean();
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
          
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

            Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, Projectile.DProj().Times[4], true);
            Projectile.HoldProj(player, 50 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
            
            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0)
                {
                    proj++;
                    SoundStyle sound = SoundID.Item1;
                    sound.Pitch = -1F;
                    PlaySound(sound, Projectile.position);
               
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.localAI[1] >= 0)
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
                {
                    if (Main.rand.NextBool(15))
                    {
                        int Type = 5;
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2();
                        dust.scale = 1.3f;
                    }
                }
            }

            int R = 0;
            for(int A = 0;A<Projectile.whoAmI;A++)
            {
                Projectile projectile = Main.projectile[A];
                if(projectile.active&&projectile.owner==Projectile.owner&&projectile.type==Projectile.type)
                {
                    R++;
                }
            }
            float v = 0;
            if (player.direction == -1) v = 3.14f;
            if (R == 0)
            {
                Projectile.position +=new Vector2(24, 6).RotatedBy(Projectile.rotation);
                player.PlayerAction().PlayerArmRotationBack(Projectile.velocity.ToRotation()-MathHelper.PiOver2, Player.CompositeArmStretchAmount.Full);
            }
            if (R == 1)
            {
                Projectile.position +=new Vector2(10,0).RotatedBy(Projectile.rotation- v/2);
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Projectile.Player();
            Item item = Projectile.Player().ActiveItem();

            Projectile.netUpdate = true;

            if (ModContent.GetInstance<DDConfigClient>().SwordHit)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/Sword");
                sound.Pitch = 0;
                PlaySound(sound, target.position);
            }
            Color[] colors = DDHelper.GetColors(TextureAssets.Item[item.type].Value);
            int a = 0;
            Vector4 vector4 = new Vector4(0, 0, 0, 0);
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i] != new Color(0, 0, 0, 0))
                {
                    a++;
                    vector4 += colors[i].ToVector4();
                }
            }
            vector4 /= a * 2;
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(vector4.X, vector4.Y, vector4.Z, vector4.W);
        }

        Color color = new Color(36, 208, 2, 0);
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
            Projectile.MeleeProj().oldVels2 = 76;
            return 30 * Projectile.scale;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Item item = Projectile.Player().ActiveItem();
            Color[] colors = DDHelper.GetColors(TextureAssets.Item[item.type].Value);
            int a = 0;
            Vector4 vector4 = new Vector4(0, 0, 0, 0);
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i] != new Color(0, 0, 0, 0))
                {
                    a++;
                    vector4 += colors[i].ToVector4();
                }
            }
            vector4 /= a * 2;
            color = new Color(vector4.X, vector4.Y, vector4.Z, vector4.W);

            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1]++;
                return false;
            }
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["刀光"]);
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
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

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
        internal Trailing TrailDrawer;
    }
}