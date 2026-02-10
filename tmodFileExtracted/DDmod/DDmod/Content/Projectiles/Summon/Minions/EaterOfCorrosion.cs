using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class EaterOfCorrosion : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

        }

        public override void SetDefault()
        {
            Main.projPet[Projectile.type] = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.minionSlots = 1;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<EaterOfCorrosionBuff>();
            Speed = 12;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        int[] Body = new int[5];
        Vector2[] Center = new Vector2[5];
        float[] Rotation = new float[5];
        float Time;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            for (int B = Body.Length - 1; B >= 0; B--)
            {
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[0]+MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B] + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B] + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
            }

            /*for (int B = Body.Length - 1; B >= 0; B--)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                GameShaders.Misc["渲染滤镜"].UseOpacity(2);
                GameShaders.Misc["渲染滤镜"].SetShaderTexture(ModContent.Request<Texture2D>("DDmod/Image/远古背景"));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uImageSize1"].SetValue(ModContent.Request<Texture2D>("DDmod/Image/远古背景").Size());
                GameShaders.Misc["渲染滤镜"].UseColor(Color.White);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["renderTargetArea"].SetValue(new Vector2(2160, 2160));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + new Vector2(0, 1));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["position"].SetValue((Projectile.Center-Main.screenPosition + new Vector2(0, -Time)).RotatedBy(MathHelper.PiOver2));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["ImageSize"].SetValue(new Vector2(ModContent.Request<Texture2D>("DDmod/Image/远古背景").Width(), ModContent.Request<Texture2D>("DDmod/Image/远古背景").Height() / 3 / 2));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["upscaleFactor"].SetValue(new Vector2(-0.7F));
                GameShaders.Misc["渲染滤镜"].Apply();
            if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[0], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                /*Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }*/
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public override void Visual()
        {
            Center[0] = Projectile.Center += Projectile.velocity;
            Rotation[0] = Projectile.rotation;
            for (int B = 0; B < Body.Length; B++)
            {
                if (B > 0)
                {
                    float ro = DDHelper.AngleDifference(Rotation[B], Rotation[B-1]);
                    Center[B] -= (Rotation[B - 1]).ToRotationVector2() * Math.Abs(ro) * 10;

                    Vector2 vector = Center[B - 1] - Center[B];
                    Rotation[B] = (float)Math.Atan2(vector.Y, vector.X);
                    float D = (vector.Length() - 22 * Projectile.scale) / vector.Length();
                    Center[B] += vector * D;
                }
            }
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.DProj().Times[1] == 0)
            {
                Projectile.DProj().Times[1] = Main.rand.Next(3, 8);
                Projectile.netUpdate = true;
            }
            int Length = (int)Projectile.DProj().Times[1];
            if (Body.Length != Length)
            {
                Body = new int[Length];
                Center = new Vector2[Length];
                Rotation = new float[Length];
                for (int B = 0; B < Length; B++)
                {
                    Center[B] = Projectile.Center - new Vector2(0.1f);
                }
            }
        }

        public override bool MobileAI()
        {
            if (Projectile.DProj().Times[0] > 0)
            {
                Projectile.DProj().Times[0]--;
            }
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = new Vector2(Main.rand.NextFloat(-200, 200), Main.rand.NextFloat(-200, 200));
            }
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (Projectile.velocity == Vector2.Zero)
                {
                    Projectile.velocity = Projectile.rotation.ToRotationVector2() * 3;
                }
                float Speed = 19 - Projectile.DProj().Times[1];
                Projectile.DProj().Times[3]++;
                if (Projectile.DProj().Times[3] > 0)
                {
                    if (Projectile.velocity.Length()>1 && npc.getRect().Intersects(Projectile.getRect()))
                    {
                        Projectile.DProj().Times[2] *= 0.92F;
                    }
                    else
                    {

                        if (Projectile.DProj().Times[2] < Speed)
                        {
                            Projectile.DProj().Times[2] += Speed / 20;
                        }
                    }
                    Projectile.RotationSpeed(vector.ToRotation(), Speed / 20);
                    if (!DDHelper.SpecifyDirection(Projectile.rotation, vector.ToRotation(), Speed / 20))
                    {
                        Projectile.DProj().Times[2] *= 0.96F;
                    }
                    Projectile.velocity = (Projectile.rotation).ToRotationVector2() * Projectile.DProj().Times[2];
                }
            }
            else
            {
                Vector2 direction = player.Center - Projectile.Center - Projectile.DProj().vector[0];
                direction.Y -= 120f;
                Projectile.velocity = (Projectile.rotation).ToRotationVector2() * (12 - Projectile.DProj().Times[1] + direction.Length() / 128);
                if (player.velocity.Length() > 20 || direction.Length() > 200)
                {
                    float speed = Speed + player.velocity.Length();
                    Projectile.netUpdate = true;
                    if (DistancePlayer > 3000f)
                    {
                        Projectile.Center = player.Center;
                    }
                    direction = direction.PerfectNormalize();
                    direction *= speed;
                    float temp = inertia;
                    Projectile.RotationSpeed(direction.ToRotation(), 0.5F);
                    //Projectile.velocity = (Projectile.velocity.RotatedBy(I) * temp + direction) / (temp + 1);
                }
                if (Projectile.velocity == Vector2.Zero)
                {
                    Projectile.velocity = Projectile.rotation.ToRotationVector2() * 3;
                }
            }
            return false;
        }
        float I = 1;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.DProj().Times[3] > 10)
                Projectile.DProj().Times[3] = -20;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool AttackAI()
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle[] vectors = new Rectangle[Body.Length];
            bool B = false;
            for (int A = 0; A < Body.Length; A++)
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
    }
}