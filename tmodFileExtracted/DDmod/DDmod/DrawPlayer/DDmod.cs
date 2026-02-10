using DDmod.Content;
using DDmod.Content.Items;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Talisman;
using DDmod.Players;
using Terraria.Graphics.Shaders;

namespace DDmod.DrawPlayer
{
    public static class PlayerDraw
    {
        static Color RingColor = Color.White;
        static float Speed = 0;
        static float RO = 1f;
        static Color SetColor(Color color, Color color2, float V)
        {

            int cInt = (int)((color2.R - color.R) * V);
            if (cInt > 0)
            {
                color.R += (byte)cInt;
            }
            else
            {
                color.R -= (byte)-cInt;
            }
            cInt = (int)((color2.G - color.G) * V);
            if (cInt > 0)
            {
                color.G += (byte)cInt;
            }
            else
            {
                color.G -= (byte)-cInt;
            }
            cInt = (int)((color2.B - color.B) * V);
            if (cInt > 0)
            {
                color.B += (byte)cInt;
            }
            else
            {
                color.B -= (byte)-cInt;
            }
            return color;
        }
        //绘制在玩家后面层
        public static void PreDraw(SpriteBatch spriteBatch, Player player, Color LightColor, Color immuneAlpha)
        {
            RO = 1f;
            AttributesPlayer modPlayer = player.Aplayer();
            DrawDDPlayer DDPlayer = player.GetModPlayer<DrawDDPlayer>();
            DDPlayer.ShieldTimes += 0.01F;
            Vector2 drawPos = player.MountedCenter - Main.screenPosition + new Vector2(0, player.gfxOffY);
            if (!player.dead)
            {
                if (player.GetModPlayer<TalismanPlayer>().type == TalismanPlayer.混元伞 && (player.TPlayer().Shield > 0 || player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit > 0) && player.GetModPlayer<DrawDDPlayer>().HunyuanShieldSc < 0.5F)
                {
                    float quotient = (0.5F) + ((float)player.TPlayer().Shield / player.TPlayer().MaxShield);
                    if (player.TPlayer().Shield <= 0)
                    {
                        quotient = 0.5F - player.GetModPlayer<DrawDDPlayer>().HunyuanShieldSc;
                    }
                    quotient = Utils.Clamp(quotient, 0f, 1f);
                    Color color = new Color(100, 255, 100, 0) * quotient * 0.65F;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    Asset<Texture2D> texture = DDTextures.远古背景;
                    float sc = 0.18F + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldSc / 4;
                    DDHelper.测试shieldShaders(1 + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit / 10, color, DDPlayer.ShieldTimes * 2, texture.Size(), false);
                    spriteBatch.Draw(DDTextures.WhitePng.Value, drawPos, null, Color.Red, 0.785f, DDTextures.WhitePng.Size() / 2, texture.Width() / 2 * sc, SpriteEffects.FlipHorizontally, 0);

                    int S = 2;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(2 + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit / 10, color, DDPlayer.ShieldTimes / 2 - (RO / S * a * 1.2F), texture.Size(), false);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, -MathHelper.PiOver2, texture.Size() / 2, sc, 0, 0);
                        DDHelper.测试shieldShaders(2 + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit / 10, color, DDPlayer.ShieldTimes * 2 - (RO / S * a), texture.Size(), false);
                        //spriteBatch.Draw(DDTextures.光晕.Value, drawPos, null, new Color(255, 180, 15, 0), MathHelper.PiOver2 - MathHelper.PiOver2, DDTextures.光晕.Size() / 2, 0.23F, 0, 0);
                        spriteBatch.Draw(DDTextures.EnergyShieldLight.Value, drawPos, null, color * 2, MathHelper.PiOver4, DDTextures.EnergyShieldLight.Size() / 2, sc * 1.33F, SpriteEffects.FlipHorizontally, 0);
                        // spriteBatch.Draw(texture.Value, drawPos, null, Color.Red, MathHelper.PiOver4 - MathHelper.PiOver2, texture.Size() / 2, 0.2F, 0, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);


                }
                else if (player.Aplayer().HolyShield > 0)
                //神圣盾
                {
                    float quotient = 1;
                    Color color = new Color(236, 200, 10, 0) * quotient * 0.25F;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    Asset<Texture2D> texture = DDTextures.化石;
                    float sc = 0.8f;
                    DDHelper.测试shieldShaders(1, color, DDPlayer.ShieldTimes * 2, texture.Size(), false);
                    spriteBatch.Draw(DDTextures.WhitePng.Value, drawPos, null, Color.Red, 0.785f, DDTextures.WhitePng.Size() / 2, texture.Width() / 2 * sc, SpriteEffects.FlipHorizontally, 0);

                    int S = 2;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(2, color, DDPlayer.ShieldTimes * 2 - (RO / S * a), texture.Size(), false);
                        spriteBatch.Draw(texture.Value, drawPos, null, color * 2, MathHelper.PiOver2, texture.Size() / 2, sc, SpriteEffects.FlipHorizontally, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                }
                if (player.HasBuff(ModContent.BuffType<固若金汤Buff>()))
                {

                    Color color = new Color(64, 202, 251, 255);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                    Asset<Texture2D> texture = DDTextures.Wave;
                    float sc = 0.26f;
                    int S = 2;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(2, color*0.25F, DDPlayer.ShieldTimes /2 - (RO / S * a), texture.Size(), false);
                        spriteBatch.Draw(texture.Value, drawPos, null, color * 2, MathHelper.PiOver2, texture.Size() / 2, sc, SpriteEffects.FlipHorizontally, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                }
                if (player.HasBuff(ModContent.BuffType<魂怒Buff>()))
                {

                    Color color = new Color(148, 13, 13, 255);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                    Asset<Texture2D> texture = DDTextures.Perlin2;
                    float sc = 0.26f;
                    int S = 3;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(10, color * 0.25F, DDPlayer.ShieldTimes / 2 - (RO / S * a), texture.Size(), false);
                        spriteBatch.Draw(texture.Value, drawPos, null, color * 2, MathHelper.PiOver2, texture.Size() / 2, sc, SpriteEffects.FlipHorizontally, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                }
                    //神圣戒指buff
                if (player.HasBuff(ModContent.BuffType<守护>()))
                {
                    float quotient = 1;
                    quotient = Utils.Clamp(quotient, 0f, 1f);
                    Color color = new Color(255, 216, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    Asset<Texture2D> texture = DDTextures.护盾;
                    float sc = 0.15F;

                    int S = 3;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(4 + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit / 10, color, DDPlayer.ShieldTimes - (RO / S * a), texture.Size(), false, true, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color,0, texture.Size() / 2, sc * new Vector2(1F, 1.3f), SpriteEffects.FlipHorizontally, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3F), SpriteEffects.FlipHorizontally, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                }
                //真神圣戒指buff
                if (player.HasBuff(ModContent.BuffType<真守护>()))
                {
                    Speed += 0.03F;
                    if (Speed >= 3)
                    {
                        Speed = 0;
                    }
                    if (Speed < 1)
                    {
                        Color c = new Color(255, 216, 0);
                        Color c2 = new Color(255, 85, 116);

                        RingColor = SetColor(c, c2, Speed);

                    }
                    else if (Speed < 2)
                    {
                        Color c = new Color(255, 85, 116);
                        Color c2 = new Color(35, 238, 238);
                        RingColor = SetColor(c, c2, Speed - 1);
                    }
                    else
                    {
                        Color c = new Color(35, 238, 238);
                        Color c2 = new Color(255, 216, 0);
                        RingColor = SetColor(c, c2, Speed - 2);
                    }
                    float quotient = 1;
                    quotient = Utils.Clamp(quotient, 0f, 1f);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    Asset<Texture2D> texture = DDTextures.护盾;
                    float sc = 0.15F;
                    int S = 3;

                    Color color = RingColor;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(4, color, DDPlayer.ShieldTimes - (RO / S * a), texture.Size(), false, true, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3f), SpriteEffects.FlipHorizontally, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3F), SpriteEffects.FlipHorizontally, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                }
                //泰拉戒指
                if (player.HasBuff(ModContent.BuffType<泰拉守护>()))
                {
                    float quotient = 1;
                    quotient = Utils.Clamp(quotient, 0f, 1f);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    Asset<Texture2D> texture = DDTextures.护盾;
                    float sc = 0.15F;

                    int S = 3;
                    Color color = new Color(255, 216, 0);
                    for (int a = 0; a < S; a++)
                    {
                        color = new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 12);
                        DDHelper.测试shieldShaders(4 + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit / 10, color, DDPlayer.ShieldTimes - (RO / S * a), texture.Size(), false, true, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3f), SpriteEffects.FlipHorizontally, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3F), SpriteEffects.FlipHorizontally, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                }
            }
        }
        public static float ro = 0;
        //绘制在玩家前面层
        public static void PostDraw(SpriteBatch spriteBatch, Player player, Color LightColor, Color immuneAlpha)
        {

            AttributesPlayer modPlayer = player.Aplayer();
            DrawDDPlayer DDPlayer = player.GetModPlayer<DrawDDPlayer>();
            Item item = player.ActiveItem();
            if (player.heldProj >= 0)
            {
                //海啸和空灾
                if (item.type == 2624 || item.type == 3859)
                {
                    Projectile Projectile = Main.projectile[player.heldProj];
                    if (Projectile.TryGetGlobalProjectile<RangedProjectile>(out var gproj))
                    {
                        RangedProjectile ranged = gproj;
                        RangedGlobalItem rangedItem = item.GetGlobalItem<RangedGlobalItem>();
                        Texture2D texture;
                        Color light;
                        if (item.type == 2624)
                        {
                            light = LightColor * (1 - player.immuneAlpha / 255F);
                            texture = DDTextures.BowE2624.Value;
                            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(0, rangedItem.YOffset).RotatedBy(Projectile.rotation), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), light, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), ranged.Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                        }
                        if (item.type == 3859)
                        {
                            light = Color.White * (1 - player.immuneAlpha / 255F);
                            texture = DDTextures.BowE3859.Value;
                            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(0, rangedItem.YOffset).RotatedBy(Projectile.rotation), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), light, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), ranged.Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                        }
                    }
                }
            }
            Vector2 drawPos = player.MountedCenter - Main.screenPosition + new Vector2(0, player.gfxOffY);
            if (!player.dead)
            {
                if (player.HasBuff(ModContent.BuffType<守护>()))
                {
                    float quotient = 1;
                    quotient = Utils.Clamp(quotient, 0f, 1f);
                    Color color = new Color(255, 216, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    Asset<Texture2D> texture = DDTextures.护盾;
                    float sc = 0.15F;

                    int S = 3;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(4 + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit / 10, color, DDPlayer.ShieldTimes - (RO / S * a), texture.Size(), false, false, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3f), 0, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3f), 0, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                }
                if (player.HasBuff(ModContent.BuffType<泰拉守护>()))
                {
                    float quotient = 1;
                    quotient = Utils.Clamp(quotient, 0f, 1f);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    Asset<Texture2D> texture = DDTextures.护盾;
                    float sc = 0.15F;

                    int S = 3;
                    Color color = new Color(255, 216, 0);
                    for (int a = 0; a < S; a++)
                    {
                        color = new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 12); ;
                        DDHelper.测试shieldShaders(4 + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit / 10, color, DDPlayer.ShieldTimes - (RO / S * a), texture.Size(), false, false, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3f), 0, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3f), 0, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                }
                if (player.HasBuff(ModContent.BuffType<真守护>()))
                {
                    float quotient = 1;
                    quotient = Utils.Clamp(quotient, 0f, 1f);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    Asset<Texture2D> texture = DDTextures.护盾;
                    float sc = 0.15F;

                    int S = 3;
                    Color color = RingColor;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(4, color, DDPlayer.ShieldTimes - (RO / S * a), texture.Size(), false, false, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3f), 0, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc * new Vector2(1F, 1.3f), 0, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                }


                if (player.HasBuff(ModContent.BuffType<魂怒Buff>()))
                {
                    Color color = new Color(148, 13, 13, 255);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                    Asset<Texture2D> texture = DDTextures.Perlin2;
                    float sc = 0.26f;
                    int S = 3;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(10, color * 0.25F, DDPlayer.ShieldTimes / 2 - (RO / S * a), texture.Size(), false, false, 0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, MathHelper.PiOver2, texture.Size() / 2, sc, 0, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
   }
                if (player.HasBuff(ModContent.BuffType<固若金汤Buff>()))
                {
                    Color color = new Color(64, 202, 251, 255);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                    Asset<Texture2D> texture = DDTextures.Wave;
                    float sc = 0.26f;
                    int S = 2;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(2, color*0.25F, DDPlayer.ShieldTimes / 2 - (RO / S * a), texture.Size(), false, false,0);
                        spriteBatch.Draw(texture.Value, drawPos, null, color, MathHelper.PiOver2, texture.Size() / 2, sc, 0, 0);
                    }
                    texture = DDTextures.护盾3;
                    float T = player.Dplayer().PlayerTimes / 40 % 1;
                    DDHelper.测试shieldShaders(3, color * 0.75f * (1F - T), 0.995f, texture.Size(), false, false, 0);
                    spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, 0.25f + T / 10, 0, 0);
                    DDHelper.测试shieldShaders(3, color, 0.995f, texture.Size(), false, false, 0);
                    spriteBatch.Draw(texture.Value, drawPos, null, color * 0.75f, 0, texture.Size() / 2, 0.25f, 0, 0);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
   }
                if (player.GetModPlayer<TalismanPlayer>().type == TalismanPlayer.混元伞 && (player.TPlayer().Shield > 0 || player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit > 0) && player.GetModPlayer<DrawDDPlayer>().HunyuanShieldSc < 0.5F)
                {
                    float quotient = ((float)player.TPlayer().Shield / player.TPlayer().MaxShield);
                    float Life = Utils.Clamp(quotient, 0f, 1f);
                    quotient += 0.5f;
                    quotient = Utils.Clamp(quotient, 0f, 1f);
                    if (Life > 0)
                    {
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                        int C = (int)(DDTextures.Shield.Width() * Life);
                        int I = (int)(255 * (1 - Life));
                        int N = (int)(255 * Life);
                        spriteBatch.Draw(DDTextures.ShieldValue.Value, drawPos - new Vector2(0, 60), new Rectangle?(new Rectangle(0, 0, C, DDTextures.Shield.Height())), new Color(I, N, 0), 0f, DDTextures.ShieldValue.Size() / 2, 1, 0, 0);
                        spriteBatch.Draw(DDTextures.Shield.Value, drawPos - new Vector2(0, 60), null, Color.White, 0f, DDTextures.Shield.Size() / 2, 1, 0, 0);
                    }
                    if (player.TPlayer().Shield <= 0)
                    {
                        quotient = 0.5F - player.GetModPlayer<DrawDDPlayer>().HunyuanShieldSc;
                    }
                    Color color = new Color(100, 255, 100, 0) * quotient * 0.65F;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    Asset<Texture2D> texture = DDTextures.远古背景;
                    float sc = 0.18F + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldSc / 4;
                    DDHelper.测试shieldShaders(1 + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit / 10, color, DDPlayer.ShieldTimes * 2, texture.Size(), false, false);
                    spriteBatch.Draw(DDTextures.WhitePng.Value, drawPos, null, Color.Red, 0.785f, DDTextures.WhitePng.Size() / 2, texture.Width() / 2 * sc, 0, 0);

                    int S = 2;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(2 + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit / 10, color, DDPlayer.ShieldTimes / 2 - (RO / S * a * 1.2F), texture.Size(), false, false); ;
                        spriteBatch.Draw(texture.Value, drawPos, null, color, -MathHelper.PiOver2, texture.Size() / 2, sc, 0, 0);
                        DDHelper.测试shieldShaders(2 + player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit / 10, color, DDPlayer.ShieldTimes * 2 - (RO / S * a), texture.Size(), false, false);
                        //spriteBatch.Draw(DDTextures.光晕.Value, drawPos, null, new Color(255, 180, 15, 0), MathHelper.PiOver2 - MathHelper.PiOver2, DDTextures.光晕.Size() / 2, 0.23F, 0, 0);
                        spriteBatch.Draw(DDTextures.EnergyShieldLight.Value, drawPos, null, color*2, MathHelper.PiOver4, DDTextures.EnergyShieldLight.Size() / 2, sc * 1.33F, 0, 0);
                        // spriteBatch.Draw(texture.Value, drawPos, null, Color.Red, MathHelper.PiOver4 - MathHelper.PiOver2, texture.Size() / 2, 0.2F, 0, 0);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                    {
                        Vector2 Pos = drawPos - new Vector2(0, 36);
                        spriteBatch.Draw(HunyuanPearlUmbrella.Umbrella.Value, Pos, null, Color.White, 0f, HunyuanPearlUmbrella.Umbrella.Size() / 2, 1, 0, 0);
                    }
                }
                else if (player.Aplayer().HolyShield > 0)
                {
                    float quotient = 1;
                    float Life = Utils.Clamp(player.Aplayer().HolyShield / 100f, 0f, 1f);
                    Color color = new Color(236, 200, 10, 0) * quotient * 0.25F;
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                    if (Life > 0)
                    {
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                        int C = (int)(DDTextures.Shield.Width() * Life);
                        int I = (int)(255 * (1 - Life));
                        int N = (int)(255 * Life);
                        spriteBatch.Draw(DDTextures.ShieldValue.Value, drawPos - new Vector2(0, 60), new Rectangle?(new Rectangle(0, 0, C, DDTextures.Shield.Height())), new Color(I, N, 0), 0f, DDTextures.ShieldValue.Size() / 2, 1, 0, 0);
                        spriteBatch.Draw(DDTextures.Shield.Value, drawPos - new Vector2(0, 60), null, Color.White, 0f, DDTextures.Shield.Size() / 2, 1, 0, 0);
                    }
                    Asset<Texture2D> texture = DDTextures.化石;
                    float sc = 0.8f;
                    DDHelper.测试shieldShaders(1, color, DDPlayer.ShieldTimes * 2, texture.Size(), false, false);
                    spriteBatch.Draw(DDTextures.WhitePng.Value, drawPos, null, Color.Red, 0.785f, DDTextures.WhitePng.Size() / 2, texture.Width() / 2 * sc, 0, 0);

                    int S = 2;
                    for (int a = 0; a < S; a++)
                    {
                        DDHelper.测试shieldShaders(2, color, DDPlayer.ShieldTimes * 2 - (RO / S * a), texture.Size(), false, false);
                        spriteBatch.Draw(texture.Value, drawPos, null, color * 2, MathHelper.PiOver2, texture.Size() / 2, sc, 0, 0);
                    }
                    color = new Color(236, 200, 10, 0) * quotient;
                    DDHelper.测试shieldShaders(4, color, 0.995f, texture.Size(), false, false, 0);

                    texture = DDTextures.护盾;
                    spriteBatch.Draw(texture.Value, drawPos, null, color, 0, texture.Size() / 2, sc / 3, 0, 0);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                }

                if (modPlayer.Crimson)
                {
                    Texture2D Perlin = DDTextures.Perlin.Value;

                    Color color = new Color(255, 30, 30);

                    Vector2 origin = Perlin.Size() / 2;
                    float scale = player.Aplayer().CrimsonTime;

                    DDPlayer.CrimsonwTimes += 0.06f;
                    DDHelper.AnnularShaders(5, color, DDPlayer.CrimsonwTimes);
                    spriteBatch.Draw(Perlin, drawPos, null, color, -MathHelper.PiOver2, origin, scale, 0, 0);
                    spriteBatch.Draw(Perlin, drawPos, null, color, -MathHelper.PiOver2, origin, scale / 1.25f, 0, 0);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                }



            }
        }
    }
    public class DrawDDPlayer : ModPlayer
    {
        //血猩计时器
        public float CrimsonwTimes;
        //盾计时器
        public float ShieldTimes= 15;
        //混元伞受击
        public float HunyuanShieldHit;
        //受击
        public float HunyuanShieldSc;
        //神圣盾受击
        public float HallowedHit;
    }
}
