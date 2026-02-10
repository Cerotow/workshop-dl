using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class 寒冰斩 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 50;
            Projectile.extraUpdates =1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 150;
            Projectile.alpha = 0;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.scale *= 1.3F;
                Projectile.scale *= player.GetAdjustedItemScale(player.ActiveItem());
                Projectile.DProj().Bool[0] = true;
                Projectile.velocity *= player.GetAdjustedItemScale(player.ActiveItem());
            }
            if (Projectile.timeLeft <= 10)
            {
                Projectile.extraUpdates = 1;
                if (Projectile.alpha > 0)
                {
                    Projectile.timeLeft = 6;
                    //Projectile.extraUpdates = 0;
                    Projectile.alpha -= 3;
                    Projectile.velocity *= 0.96F;
                }
            }
            else
            {
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += 50;
                }
                else
                {
                    Projectile.alpha = 255;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = Projectile.scale;
            }
            if (Projectile.localAI[1] == 0)
            {
                Projectile.localAI[1] = Projectile.damage;
            }

            Projectile.damage = (int)(Projectile.localAI[1]*(Projectile.alpha/255F));
            Projectile.ProjScaleChange();
            for (int A = 0; A <= Length; A++)
            {
                Center[A] = Projectile.Center - (new Vector2(Projectile.height/2 * Math.Abs(A - Length / 2)- Projectile.height/2, (A - Length / 2) * Projectile.height).RotatedBy(Projectile.rotation));
            }
            if (Projectile.alpha >= 155)
            {
                Rectangle[] vectors = new Rectangle[Length + 1];
                for (int A = 0; A <= Length; A++)
                {
                    if (Main.rand.NextBool(4))
                    {
                        Vector2 Size = Projectile.Size / 2;
                        vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                        int D = NewDust(vectors[A].TopLeft(), Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(60, 155, 255, 0), Main.rand.NextFloat(1.2F, 2.8F));
                        Main.dust[D].velocity = -Projectile.velocity * Main.rand.NextFloat(0.2F, 0.4F);
                        Main.dust[D].rotation = Projectile.velocity.ToRotation();
                    }
                }
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 10)
            {
                Projectile.timeLeft = 10;
            }
            Projectile.velocity = oldVelocity;
            Projectile.tileCollide = false;
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0]++;
            if (Projectile.ai[0]>=5)
            {
                Projectile.localAI[1] *= 0.8F;
                Projectile.damage = (int)(Projectile.localAI[1] * (Projectile.alpha / 255F));
                if (Projectile.timeLeft > 10)
                {

                    Projectile.timeLeft = 10;
                }
            }
            target.AddBuff(ModContent.BuffType<Freeze>(), 120);
            target.AddBuff(ModContent.BuffType<Frozen>(), 360);
            target.AddBuff(44, 360);
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
                return null;
        }
        int Length = 4;
        Vector2[] Center = new Vector2[5];
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle[] vectors = new Rectangle[Length + 1];
            bool B = false;
            for (int A = 0; A <= Length; A++)
            {
                Vector2 Size = Projectile.Size / 2;
                vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                if (vectors[A].Intersects(targetHitbox))
                {
                    B = true;
                }
            }
            return new bool?(B);
        }
        Vector2[] V => new Vector2[Projectile.oldPos.Length];
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(60, 155, 255, 0);
            Color color2 = new Color(55, 55, 55, 255);
            float A = Projectile.alpha / 255F;
            color *= A;
            color2 *= A;

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            
            for (int i = 1; i < Projectile.oldPos.Length/2; i++)
            {
                Color oldcolor = color * ((Projectile.oldPos.Length/2 - i) / (float)Projectile.oldPos.Length/2 / 2)*0.5F;
                Color oldcolor2 = color2 * ((Projectile.oldPos.Length/2 - i) / (float)Projectile.oldPos.Length/2 / 2) * 0.5F;
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, oldcolor2, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1)/4, 0, 0);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1)/4, 0, 0);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1)/4, 0, 0);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1)/4, 0, 0);

            }
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["普通拖尾"]);
            }
            if (TrailDrawer2 == null)
            {
                TrailDrawer2 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction2), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["普通拖尾"]);
            }
            GameShaders.Misc["普通拖尾"].SetShaderTexture(DDTextures.Perlin2);
            GameShaders.Misc["普通拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            float B = (Projectile.alpha-105F) / 150F;
            if(B<0)
            {
                B = 0;
            }
            if(B>1)
            {
                B = 1;
            }

            color2 = new Color(55, 55, 55, 255);
            color2 *= A;
            Vector2[] V = new Vector2[Projectile.oldPos.Length];
            for (int a = 0; a < V.Length*0.6F; a++)
            {
                V[a] = Projectile.oldPos[a];
            }
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + Projectile.velocity.PerfectNormalize() * (12 * Projectile.scale),188, null, Projectile.scale, B*2);

            TrailDrawer2.Draw(V, Projectile.Size * 0.5f - Main.screenPosition + Projectile.velocity.PerfectNormalize() * (-12 * Projectile.scale)- Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * 44 * Projectile.scale, 128, null, Projectile.scale*0.65F, B * 0.75F);
            
            TrailDrawer2.Draw(V, Projectile.Size * 0.5f - Main.screenPosition + Projectile.velocity.PerfectNormalize() * (-12 * Projectile.scale)+ Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2)* 44 * Projectile.scale, 128, null, Projectile.scale * 0.65F, B * 0.75F);
            Vector2 vector = Projectile.Size / 2;

            //Main.NewText(Projectile.oldPos[Projectile.oldPos.Length / 2 + 2]);
            Main.spriteBatch.Draw(texture, Projectile.position + Projectile.Size / 2 - Main.screenPosition, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1) / 4, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.position + Projectile.Size / 2 - Main.screenPosition, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1) / 4, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.position + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1) / 4, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.position + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1) / 4, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.position + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1) / 4, 0, 0);

            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                //Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1) / 4 * (1-(float)i/ Projectile.oldPos.Length), 0, 0);
            }
            color.A = 0;

            /*
             Rectangle[] vectors = new Rectangle[Length+1];

             for (int r = 0; r <= Length; r++)
             {
                 Vector2 Size = Projectile.Size / 2;
                 vectors[r] = new Rectangle((int)(Center[r].X - Size.X), (int)(Center[r].Y - Size.Y), Projectile.width, Projectile.height);
                 Main.spriteBatch.Draw(DDTextures.WhitePng.Value, new Vector2((int)(Center[r].X - Size.X), (int)(Center[r].Y - Size.Y)) - Main.screenPosition, null, Color.White, 0, Vector2.Zero, new Vector2(Projectile.width, Projectile.height)/2, 0, 0);
             }*/
            return false;
        }
        internal static Trailing TrailDrawer;
        internal static Trailing TrailDrawer2;
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(60, 155, 255, 0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(60, 155, 255, 0), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                70,
                60,
                50,
                40,
                30,
                20,
                10,
            }), 2, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction2(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                30,
                20,
                10,
            }), 0, (float)Math.Pow((double)completionRatio, 1.0));
        }
    }
}