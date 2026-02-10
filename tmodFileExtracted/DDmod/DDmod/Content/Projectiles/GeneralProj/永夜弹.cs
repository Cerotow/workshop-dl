using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.绿岩之视;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Transactions;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;
using static Terraria.GameContent.Animations.Actions.Sprites;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 永夜弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
            Projectile.scale = 1F;
            Projectile.timeLeft = 360;
            Projectile.hide = false;
        }
        int A;
        public override void AI()
        {
            if (Projectile.DProj().Times[0] == 0)
            {
                Projectile.DProj().Times[0] = Projectile.damage;
                Projectile.damage = 0;
            }
            if (Projectile.ai[0] == 0)
            {
                for (int a = 0; a < 2; a++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(81, 6, 233, 12))];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale;
                    dust.velocity *= -0.5f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                    dust.rotation = Projectile.velocity.ToRotation();
                }
                Projectile.ProjScaleChange();
                Projectile.Track(400, 20, 24, 0);
                NPC npc = NPCdirection.FindClosest(Projectile.Center, 400, false);
                if (npc != null)
                {
                    if (new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height).Intersects(npc.getRect()))
                    {
                        Projectile.Kill();
                    }
                }
            }
            else if (Projectile.ai[0] == 1)
            {
                Projectile.scale = 1.5F;
                for (int a = 0; a < 2; a++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(179, 252, 0, 12))];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale;
                    dust.velocity *= -0.5f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                    dust.rotation = Projectile.velocity.ToRotation();
                }
                Projectile.ProjScaleChange();
                Projectile.Track(400, 20, 24, 0);
                NPC npc = NPCdirection.FindClosest(Projectile.Center, 400, false);
                if (npc != null)
                {
                    if (new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height).Intersects(npc.getRect()))
                    {
                        Projectile.Kill();
                    }
                }

            }
            else if (Projectile.ai[0] == 2)
            {
                Projectile.scale = 0.75F;
                for (int a = 0; a < 2; a++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(81, 6, 233, 12))];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale;
                    dust.velocity *= -0.5f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                    dust.rotation = Projectile.velocity.ToRotation();
                }
                Projectile.ProjScaleChange();
                Projectile.Track(800, 20, 12, 30);
                NPC npc = NPCdirection.FindClosest(Projectile.Center, 400, false);
                if (npc != null)
                {
                    if (new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height).Intersects(npc.getRect()))
                    {
                        Projectile.Kill();
                    }
                }

            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                int a = NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<永夜爆炸>(), (int)Projectile.DProj().Times[0], 1, Projectile.owner, Projectile.ai[0],Projectile.scale);
                Main.projectile[a].DamageType = Projectile.DamageType;
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(81, 6, 233, 12),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(81, 6, 233, 12), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                30,
                25,
                20,
            }), 5, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal static Trailing TrailDrawer;
        internal Color ColorFunction2(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(179, 252, 0, 12),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(179, 252, 0, 12), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction2(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                30,
                25,
                20,
            }), 5, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal static Trailing TrailDrawer2;
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            if (TrailDrawer2 == null)
            {
                TrailDrawer2 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction2), new Trailing.VertexColorFunction(ColorFunction2), null, GameShaders.Misc["贴图拖尾"]);
            }
            Vector2[] vectors = new Vector2[(int)(Projectile.oldPos.Length*Projectile.scale)];
            if(vectors.Length> Projectile.oldPos.Length)
            {
                vectors = new Vector2[Projectile.oldPos.Length];
            }
            for(int a= 0;a<vectors.Length;a++)
            {
                vectors[a] = Projectile.oldPos[a];
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            if (Projectile.ai[0] != 1)
            {
                TrailDrawer.Draw(vectors, Projectile.Size * 0.5f - Main.screenPosition, 88, null, Projectile.scale);
                texture.DrawCentre(Projectile, null, new Color(81, 6, 233, 255), Projectile.scale / 4);
                texture.DrawCentre(Projectile, null, new Color(81, 6, 233, 0), Projectile.scale / 4);
                texture.DrawCentre(Projectile, null, new Color(81, 6, 233, 0), Projectile.scale / 4);
                texture.DrawCentre(Projectile, null, new Color(81, 6, 233, 0), Projectile.scale / 4);
            }
            else
            {
                TrailDrawer2.Draw(vectors, Projectile.Size * 0.5f - Main.screenPosition, 88, null, Projectile.scale);
                texture.DrawCentre(Projectile, null, new Color(179, 252, 0, 255), Projectile.scale / 4);
                texture.DrawCentre(Projectile, null, new Color(179, 252, 0, 0), Projectile.scale / 4);
            }
             return false;
        }
    }
    public class 永夜爆炸 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.DProj().Bool[0] = true;
                if (Projectile.ai[0] != 1)
                {
                    SoundStyle sound = SoundID.Item14;
                    sound.Volume = Projectile.ai[1];
                    sound.MaxInstances = 10;
                    PlaySound(sound, Projectile.Center);
                    NewDustChange4((int)(Projectile.ai[1] * 50), Projectile.position, Projectile.Size, 27, 0, 12 * Projectile.ai[1], true, 1F * (Projectile.ai[1]), 2F * (Projectile.ai[1]), 100, 1000);

                    int Type = ModContent.DustType<速度粒子>(); NewDustChange4((int)(Projectile.ai[1] * 20), Projectile.position, Projectile.Size, Type, 3 * Projectile.ai[1], 6 * Projectile.ai[1], true, 3F * (Projectile.ai[1]), 4F * (Projectile.ai[1]), 100, 1000, new Color(81, 6, 233, 12), 2F);
                }
                else
                {
                    SoundStyle sound = SoundID.Item14;
                    sound.Volume = Projectile.ai[1];
                    sound.MaxInstances = 10;
                    PlaySound(sound, Projectile.Center);
                    NewDustChange4((int)(Projectile.ai[1] * 50), Projectile.position, Projectile.Size, 75, 0, 6 * Projectile.ai[1], true, 1F * (Projectile.ai[1]), 2F * (Projectile.ai[1]), 100, 1000);

                    int Type = ModContent.DustType<速度粒子>(); NewDustChange4((int)(Projectile.ai[1] * 20), Projectile.position, Projectile.Size, Type, 3 * Projectile.ai[1], 6 * Projectile.ai[1], true, 3F * (Projectile.ai[1]), 4F * (Projectile.ai[1]), 100, 1000, new Color(179, 252, 0, 12), 2F);


                }
            }
            if (Projectile.scale < 24 * Projectile.ai[1])
            {
                Projectile.scale += 4 * Projectile.ai[1];
                Projectile.ProjScaleChange();
            }
            else if (Projectile.timeLeft > 2)
            {
                Projectile.timeLeft = 2;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
        }
    }
    public class 永夜 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 40;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (npc != null && npc.CanBeChasedBy())
            {
                if (Projectile.ai[1] == 0)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(81, 6, 233, 12))];
                    dust.noGravity = true;
                    dust.scale = 1.3F;
                    dust.velocity = Projectile.velocity.PerfectNormalize() * 0.01F;
                    dust.rotation = dust.velocity.ToRotation();
                    Projectile.timeLeft = 2;
                    if (new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height).Intersects(npc.getRect()))
                    {
                        npc.AddBuff(ModContent.BuffType<永夜侵袭>(), 300);
                        Projectile.Kill();
                    }
                }
                else
                {

                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(179, 252, 0, 12))];
                    dust.noGravity = true;
                    dust.scale = 1.3F;
                    dust.velocity = Projectile.velocity.PerfectNormalize() * 0.01F;
                    dust.rotation = dust.velocity.ToRotation();
                    Projectile.timeLeft = 2;
                    if (new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height).Intersects(npc.getRect()))
                    {
                        npc.AddBuff(ModContent.BuffType<真永夜侵袭>(), 300);
                        Projectile.Kill();
                    }
                }
                Projectile.Chase(npc, 6, 21);
            }
            else
            {
                Projectile.Kill();
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override void OnKill(int timeLeft)
        {
        }
    }
}