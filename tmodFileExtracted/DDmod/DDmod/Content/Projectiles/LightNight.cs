namespace DDmod.Content.Projectiles.Melee
{
    /// <summary>
    /// 0,强化成功,1,永夜之刃,2.原版永夜3,原版神圣 4,泰拉,5强化失败
    /// </summary>
    public class LightNight : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 114;
            Projectile.penetrate = -1;
            Projectile.idStaticNPCHitCooldown = 30;
            Projectile.scale = 0.01f;
        }
        public override void AI()
        {
            Projectile.timeLeft = 114;
            if (Projectile.ai[0] == 0|| Projectile.ai[0] == 6)
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.scale += 0.1f;
                    if (Projectile.scale > 1)
                    {
                        Projectile.ai[1] = 1;
                    }
                }
                else
                {
                    Projectile.scale -= 0.05F;
                    if (Projectile.scale <= 0.01F)
                    {
                        Projectile.Kill();
                    }
                }
                Projectile.Resize((int)(80 * (Projectile.scale)), (int)(80 * (Projectile.scale)));
                if (Projectile.ai[0] == 0)
                {
                    if (Projectile.localAI[0] == 0)
                        CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(0, 255, 0), Language.GetTextValue("Mods.DDmod.StrengtheningUI.强化成功"));
                    Projectile.localAI[0] = 1;
                }
            }
            else
            if (Projectile.ai[0] == 1|| Projectile.ai[0] == 2|| Projectile.ai[0] == 3)
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.scale += 0.25f;
                    if (Projectile.scale > 3)
                    {
                        Projectile.ai[1] = 1;
                    }
                }
                else
                {
                    Projectile.scale -= 0.1F;
                    if (Projectile.scale <= 0.01F)
                    {
                        Projectile.Kill();
                    }
                }
                Projectile.Resize((int)(80 * (Projectile.scale)), (int)(80 * (Projectile.scale)));
            }
            else
            if (Projectile.ai[0] == 4)
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.scale += 0.25f;
                    if (Projectile.scale > 3)
                    {
                        Projectile.ai[1] = 1;
                    }
                }
                else
                {
                    Projectile.localAI[1]+=15;
                    if (Projectile.localAI[1] > 600)
                    {
                        Projectile.scale -= 0.1F;
                        if (Projectile.scale <= 0.01F)
                        {
                            Projectile.Kill();
                        }
                    }
                }
                Projectile.Resize((int)(80 * (Projectile.scale)), (int)(80 * (Projectile.scale)*6));
            }
            else
            if (Projectile.ai[0] == 5)
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.scale += 0.1f;
                    if (Projectile.scale > 0.4f)
                    {
                        Projectile.ai[1] = 1;
                    }
                }
                else
                {
                    Projectile.scale -= 0.05F;
                    if (Projectile.scale <= 0.01F)
                    {
                        Projectile.Kill();
                    }
                }
                Projectile.Resize((int)(80 * (Projectile.scale)), (int)(80 * (Projectile.scale)));
                if (Projectile.localAI[0] == 0)
                    CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(255, 0, 0), Language.GetTextValue("Mods.DDmod.StrengtheningUI.强化失败"));
                Projectile.localAI[0] = 1;
            }
            Projectile.rotation += 0.02F;
        }

        public override void OnKill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(219, 130, 255, 0)
                ;
            if (Projectile.ai[0] ==1)
            {
                color = new Color(81, 6, 233, 0);
            }
            if (Projectile.ai[0] ==2)
            {
                color = new Color(36, 208, 2, 0);
            }
            if (Projectile.ai[0] ==3)
            {
                color = new Color(56, 187, 255, 0)*0.5f;
            }
            if (Projectile.ai[0] ==4)
            {
                color = new Color(107, 255, 118, 0);
            }
            if (Projectile.ai[0] ==5)
            {
                color = new Color(255, 0, 0, 0);
            }
            Texture2D texture = DDTextures.GlowEffect.Value;
            Texture2D texture2 = DDTextures.GlowTrail2.Value;
            Texture2D texture3 = DDTextures.GlowTrail2.Value;

            if (Projectile.ai[0] < 3|| Projectile.ai[0] == 5|| Projectile.ai[0] == 6)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width , texture.Height) / 2, Projectile.scale, 0, 0f);

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width , texture.Height) / 2, Projectile.scale, 0, 0f);
                texture = DDTextures.VoidStar.Value;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width / Main.projFrames[Projectile.type], texture.Height) / 2, Projectile.scale * 2, 0, 0f);
            }
            else if(Projectile.ai[0] == 3)
            {
                Color color2 = new Color(215, 53, 203, 0)*0.5f;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, 0, 0f);

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color2, Projectile.rotation+1, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, 0, 0f);

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, 0, 0f);

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color2, Projectile.rotation+1, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, 0, 0f);
                texture = DDTextures.VoidStar.Value;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width / Main.projFrames[Projectile.type], texture.Height) / 2, Projectile.scale * 2, 0, 0f);

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color2, Projectile.rotation+1, new Vector2(texture.Width / Main.projFrames[Projectile.type], texture.Height) / 2, Projectile.scale * 2, 0, 0f);
            }
            else if (Projectile.ai[0] ==4)
            {
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition - new Vector2(0, Projectile.localAI[1] % (texture.Width/2)*6), new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color, MathHelper.PiOver2, new Vector2(texture2.Width, texture2.Height/2), new Vector2(6, Projectile.scale * 0.3F), 0, 0f);
                Main.spriteBatch.Draw(texture3, Projectile.Center - Main.screenPosition - new Vector2(0, Projectile.localAI[1] % (texture.Width/2)*6), new Rectangle?(new Rectangle(0, 0, texture3.Width, texture3.Height)), color, MathHelper.PiOver2, new Vector2(texture3.Width, texture3.Height/2), new Vector2(6, Projectile.scale * 0.3F), 0, 0f);
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition + new Vector2(0, texture2.Width * 6-(int)Projectile.localAI[1] % texture2.Width * 6), new Rectangle?(new Rectangle(0, 0, (int)Projectile.localAI[1] % texture2.Width, texture2.Height)), color, MathHelper.PiOver2, new Vector2(texture2.Width, texture2.Height/2), new Vector2(6, Projectile.scale * 0.3F), 0, 0f);
                Main.spriteBatch.Draw(texture3, Projectile.Center - Main.screenPosition+new Vector2(0, texture3.Width * 6-(int)Projectile.localAI[1] % texture3.Width * 6), new Rectangle?(new Rectangle(0, 0, (int)Projectile.localAI[1] % texture3.Width, texture3.Height)), color, MathHelper.PiOver2, new Vector2(texture3.Width, texture3.Height/2), new Vector2(6, Projectile.scale * 0.3F), 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale / 2, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale / 2, 0, 0f);
                texture = DDTextures.VoidStar.Value;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width , texture.Height) / 2, Projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, Projectile.rotation, new Vector2(texture.Width , texture.Height) / 2, Projectile.scale, 0, 0f);
            }
            return false;
        }
    }
}