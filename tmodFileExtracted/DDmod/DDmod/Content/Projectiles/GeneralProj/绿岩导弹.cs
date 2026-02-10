using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.绿岩之视;
using DDmod.Content.Particles;
using DDmod.Worlds;
using System.Transactions;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 绿岩导弹 : ModProjectile
    {
        public static Asset<Texture2D> asset;
        public override void Load()
        {
            asset = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/GeneralProj/绿岩标记");
        }
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
            Projectile.scale = 1F;
            Projectile.timeLeft = 180;
            Projectile.hide = false;
        }
        int A;
        public override void AI()
        {
            if (Projectile.DProj().Times[0] ==0)
            {
                Projectile.DProj().Times[0] = Projectile.damage;
                Projectile.damage = 0;
            }
            for (int a = 0; a < 2; a++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(119, 237, 130, 50))];
                dust.noGravity = true;
                dust.scale = Projectile.scale;
                dust.velocity *= -0.5f;
                Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                dust.velocity += -vector * Projectile.scale;
                dust.rotation = Projectile.velocity.ToRotation();
            }
            Projectile.ProjScaleChange();
            if (Projectile.DProj().track == 1)
            {
                PlaySound(SoundID.Item11, Projectile.Center);
            }
            if (Projectile.ai[0] == 1)
            {
                Projectile.tileCollide = Projectile.DProj().track >= 30;
                NPC npc = Main.npc[(int)Projectile.ai[1]];
                if(!npc.CanBeChasedBy())
                {
                    npc = NPCdirection.FindClosest(Projectile.Center, 2000, false);
                    if (npc != null)
                    Projectile.ai[1] = npc.whoAmI;
                    return;
                }
                if (Projectile.DProj().track < 30)
                {
                }
                if (Projectile.DProj().track > 30)
                {
                    Vector2 vector = (npc.Center - Projectile.Center).PerfectNormalize() * 26;
                    Projectile.velocity = (Projectile.velocity * 10 + vector) / (11);
                }
                if (npc != null)
                {
                    if (new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height).Intersects(npc.getRect()))
                    {
                        Projectile.Kill();
                    }
                }
            }
            else
            {

                if (Projectile.DProj().track >20)
                {
                    NPC npc = NPCdirection.FindClosest(Projectile.Center, 1200, false);
                    if (npc != null)
                    {
                        Vector2 vector = (npc.Center - Projectile.Center).PerfectNormalize() * 12;
                        Projectile.velocity = (Projectile.velocity * 20 + vector) / (21);

                        if (new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height).Intersects(npc.getRect()))
                        {
                            Projectile.Kill();
                        }
                    }
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                int a = NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<绿岩导弹爆炸>(), (int)Projectile.DProj().Times[0], 1, Projectile.owner);
                Main.projectile[a].DamageType = Projectile.DamageType;
                Main.projectile[a].scale = Projectile.scale;
            }
            PlaySound(SoundID.Item14, Projectile.Center);
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(119, 237, 130, 50),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(119, 237, 130, 50), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                20,
                10,
                5,
            }), 2, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal static Trailing TrailDrawer;
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
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
            TrailDrawer.Draw(vectors, Projectile.Size * 0.5f- Projectile.velocity.PerfectNormalize()*(20*Projectile.scale) - Main.screenPosition, 88, null,Projectile.scale);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            texture.DrawCentre(Projectile, new Rectangle(0, 0, texture.Width / 2, texture.Height), lightColor, Projectile.scale);
            texture.DrawCentre(Projectile, new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height), Color.White, Projectile.scale);

            if (Projectile.ai[0] == 1)
            {
                texture = asset.Value;
                NPC npc = Main.npc[(int)Projectile.ai[1]];
                if(npc.CanBeChasedBy())
                Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.White, 0, texture.Size()/2, 1, 0, 0f);
            }
             return false;
        }
    }
    public class 绿岩导弹爆炸 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 0;
            Projectile.scale = 1F;
            Projectile.timeLeft = 12;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        int A;
        public override void AI()
        {
            Projectile.ProjScaleChange();
            if (!Projectile.DProj().Bool[0])
            {
                for (int a = 0; a < 60; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(2), 1, 1, ModContent.DustType<速度粒子>(), 0f, 0f, 0, new Color(119, 237, 130, 50), 4f * Projectile.scale)];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(1, 4), Main.rand.NextFloat(1, 4)), (Math.PI * 2 / a) + a, default);
                    dust.velocity = vector * 2 * Projectile.scale;
                    dust.customData = 2 * Projectile.scale;
                    dust.rotation = dust.velocity.ToRotation();
                    dust.noGravity = true;
                }
                for (int a = 0; a < 30; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<冰雾>(), 0f, 0f, -Main.rand.Next(400, 1200), new Color(119, 237, 130, 100) * 0.5f, Main.rand.NextFloat(0.25F, 1F) * Projectile.scale)];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, (Math.PI * 2 / a) + a, default);
                    dust.velocity = vector * Projectile.scale;
                    dust.noGravity = false;
                }
                Projectile.DProj().Bool[0] = true;
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
}