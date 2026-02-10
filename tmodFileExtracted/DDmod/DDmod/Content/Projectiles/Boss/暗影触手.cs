using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.先祖咒魂;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Magic.Staff;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Boss
{
    public class 暗影触手基座 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft =1750;
            Projectile.extraUpdates = 4;
            Projectile.penetrate = -1;
            Projectile.scale = 1F;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 10)
            {
                Projectile.DProj().Bool[4] = true;
            }
            if (Projectile.DProj().Bool[4])
            {
                Projectile.timeLeft = 2;
                Projectile.position = Projectile.oldPosition;
                Projectile.scale -= 0.01f;
                if (Projectile.scale <= 0.2f)
                {
                    Projectile.Kill();
                }
                return;
            }
            Projectile.scale = Projectile.ai[1];
            Projectile.velocity = Vector2.Zero;
            if(Projectile.ai[1]==0)
            {
                NewDustChange2(250, Projectile.Center - new Vector2(4), Vector2.Zero, 27, 0, 15, true, 0.3F, 2F, 0);
            }
            if (Projectile.ai[1] < 1)
            {
                Projectile.ai[1] += 0.004F;
            }
            else
            {
                Projectile.ai[0]++;
                int A = (int)Projectile.ai[0] / 60;
                if (Projectile.ai[0] % 60==0&& Projectile.ai[0]<=300)
                {
                    if (Main.netMode != 1)
                    {
                            NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.RotatedBy(MathHelper.TwoPi / 5 * A + Projectile.ai[2]), ModContent.ProjectileType<暗影触手>(), Projectile.damage, 0, -1, 0, Main.rand.Next(12, 24));
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.VoidStar.Value;
            Color color = new Color(81, 6, 233, 255);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color) * Projectile.scale, 0, texture.Size() / 2, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(new Color(181, 106, 233, 105)) * Projectile.scale, 0, texture.Size() / 2, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(new Color(181, 106, 233, 105)) * Projectile.scale, 0, texture.Size() / 2, Projectile.scale, 0, 0);

            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
    }

    public class 暗影触手 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1200;
            Projectile.extraUpdates = 4;
            Projectile.penetrate = -1;
            Projectile.scale = 1.5F;
            Projectile.localAI[0] = -1000;
        }
        private int[] Body;
        private Vector2[] Center;
        private Vector2[] Velocity;
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(81, 6, 233, 0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(81, 6, 233, 0), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 40f, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
        internal static Trailing TrailDrawer;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Body == null)
            {
                return false;
            }
            Texture2D texture = DDTextures.VoidStar.Value;
            Color color = new Color(81, 6, 233, 255);
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.6f);
            Vector2 vector = Projectile.Size / 2;

            Vector2[] v = new Vector2[(int)Projectile.DProj().Times[1]];
            for (int B = 0; B < v.Length; B++)
            {
                v[B] = Projectile.Center + Center[B];
            }
            TrailDrawer.Draw(v, -Main.screenPosition, 60, null, Projectile.scale,1.5F);
            /*
            for (int B = 0; B < v.Length; B++)
            {
                Rectangle[] vectors = new Rectangle[Body.Length];
                v[B] = Projectile.Center + Center[B];
                vector = Projectile.Size * (1.3f - (B) / (float)Body.Length);
                Vector2 Size = vector / 2;
                vectors[B] = new Rectangle((int)(v[B].X - Size.X), (int)(v[B].Y - Size.Y), (int)vector.X, (int)vector.Y);
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, v[B] - Size - Main.screenPosition, null, Color.White * 0.75F, 0, Vector2.Zero, vector / 2, 0, 0);
            }*/
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.ShadowFlame, 300);
        }
        float A1;
        float A2;
        float A3;
        float A4;
        bool B1;
        bool B2;
        bool B3;
        bool B4;
        public override void AI()
        {
            if (Projectile.localAI[0] == -1000)
            {
                Projectile.DProj().Bool[0] = Main.rand.NextBool(2);
                Projectile.localAI[0] = Projectile.velocity.ToRotation();
                if (Projectile.ai[2] == 0)
                {
                    Projectile.ai[2] = 1.5F;
                }
            }
            Projectile.scale = Projectile.ai[2];
            Projectile.alpha = 0;
            if (Projectile.timeLeft < 100)
            {
                if (Projectile.DProj().Times[1] > 0)
                {
                    if (Projectile.DProj().Bool[3])
                    {
                        Projectile.DProj().Times[1] -= 0.05F * Projectile.scale;
                    }
                    else
                    {
                        Projectile.DProj().Times[1] -= 0.2F * Projectile.scale;
                    }
                    if (Projectile.DProj().Times[1] < 0)
                    {
                        Projectile.DProj().Times[1] = 0;
                    }
                }
            }
            if (Projectile.timeLeft < 10)
            {
                Projectile.DProj().Bool[4] = true;
            }
            if (Projectile.DProj().Bool[4])
            {
                Projectile.timeLeft = 2;
                Projectile.position = Projectile.oldPosition;
                if (Projectile.DProj().Times[1]<1F)
                {
                    Projectile.Kill();
                    return;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            bool A = DDHelper.BackAndForth(-1f, 1f, 0.015F * Projectile.DProj().Times[3], ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0], false);
            if (A && Projectile.DProj().Times[3] == 1)
            {
                Projectile.DProj().Times[3] = -1f;
            }
            if (Projectile.DProj().Times[3] < 1F)
            {
                Projectile.DProj().Times[3] += 1F / 50;
            }
            else
            {
                Projectile.DProj().Times[3] = 1f;
            }
            Projectile.velocity = Projectile.localAI[0].ToRotationVector2().RotatedBy(Projectile.DProj().Times[0]);

            if (Projectile.timeLeft < 10)
            {
                Projectile.DProj().Bool[4] = true;
            }
            else
            {

                //长度
                int Length = (int)Projectile.ai[1];
                if (Body==null||Body.Length != Length)
                {
                    Body = new int[Length];
                    Center = new Vector2[Length];
                    Velocity = new Vector2[Length];
                }
                Center[0] = Vector2.Zero;
                Velocity[0] = Projectile.velocity;
                for (int B = 0; B < Body.Length; B++)
                {
                    if (B > 0)
                    {
                        Vector2 vector = Center[B - 1] - Center[B];
                        Velocity[B] = Velocity[B - 1] + Velocity[B - 1].RotatedBy(Projectile.DProj().Times[0]) / 20;
                        float Distance;
                        if (vector.Length() == 0)
                        {
                            Distance = 0;
                        }
                        else
                        {
                            Distance = (vector.Length() - (20 * Projectile.scale)) / vector.Length();
                        }
                        Center[B] += vector * Distance + Velocity[B - 1].PerfectNormalize() * Length / 12 * Projectile.scale;
                    }
                }
                if (Projectile.DProj().Times[1] < Body.Length)
                {
                    Projectile.DProj().Times[1] += 0.04F * Projectile.scale;
                }
                DDHelper.MaxandMinF(ref Projectile.DProj().Times[1], Body.Length, 0);
            }
            if (Projectile.ai[0] > 0)
            {
                //Projectile.scale = 1;
                //Projectile.ProjScaleChange();
                Projectile.hide = true;
                NPC npc = Main.npc[(int)Projectile.ai[0] - 1];
                if (npc.type != ModContent.NPCType<先祖咒魂>() || !npc.active)
                {
                    Projectile.Kill();
                    return;
                }
                if (npc.oldPosition != Vector2.Zero)
                {
                    Projectile.Center = npc.Center;
                }
                if (!Projectile.DProj().Bool[4] && npc.Dnpc().Bool[4])
                {
                    Projectile.DProj().Bool[4] = true;
                    Projectile.DProj().Bool[3] = true;
                }
            }
            //Projectile.netUpdate = true;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Body == null)
            {
                return false;
            }
            Rectangle[] vectors = new Rectangle[Body.Length];
            bool B = false;
            Vector2[] v = new Vector2[Center.Length];
            for (int A = 0; A < Projectile.DProj().Times[1] - 3; A++)
            {
                v[A] = Projectile.Center + Center[A];
                Vector2 vector = Projectile.Size * (1.3f - (A) / (float)Body.Length);
                Vector2 Size = vector / 2;
                vectors[A] = new Rectangle((int)(v[A].X - Size.X), (int)(v[A].Y - Size.Y), (int)vector.X, (int)vector.Y);
                if (vectors[A].Intersects(targetHitbox))
                {
                    B = true;
                    break;
                }
            }
            return new bool?(B);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.ai[0] > 0)
                behindNPCs.Add(Projectile.whoAmI);
        }
    }
}