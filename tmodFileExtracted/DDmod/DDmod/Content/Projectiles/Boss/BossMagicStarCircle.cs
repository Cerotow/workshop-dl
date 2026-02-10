using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using Terraria;

namespace DDmod.Content.Projectiles.Boss
{
    public class BossMagicStarCircle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.02F;
        }
        public override void AI()
        {
            if (Projectile.ai[2] == 0)
            {
                Player player = Main.player[Main.npc[(int)Projectile.ai[0]].target];
                Projectile.ai[1]++;
                if (Projectile.ai[1] < 30)
                {
                    Projectile.scale += 0.05F;
                    Projectile.rotation += 0.1f;
                    if (Projectile.rotation > MathHelper.Pi)
                    {
                        Projectile.rotation -= MathHelper.TwoPi;
                    }
                    Projectile.Center += player.Dplayer().PrePosition;
                }
                else if (Projectile.ai[1] < 200)
                {
                    if (Projectile.localAI[0]<2)
                        Projectile.localAI[0] += 0.05f;

                    Projectile.RotationSpeed((player.Center - Projectile.Center).ToRotation(), 0.1F);
                    Projectile.Center += player.Dplayer().PrePosition;

                    if (DDHelper.SpecifyDirection(Projectile.rotation, (player.Center - Projectile.Center).ToRotation(), 0.1f) && Projectile.ai[1] < 199 && Projectile.ai[1] > 50)
                    {
                        Projectile.ai[1] = 199;
                    }
                }
                else if (Projectile.ai[1] == 200)
                {
                    NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2().PerfectNormalize() * 15, ModContent.ProjectileType<BossStar2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
                else if (Projectile.ai[1] == 210)
                {
                    NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2().PerfectNormalize() * 15, ModContent.ProjectileType<BossStar2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
                else if (Projectile.ai[1] == 220)
                {
                    NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2().PerfectNormalize() * 15, ModContent.ProjectileType<BossStar2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
                else if (Projectile.ai[1] > 220)
                {
                    Projectile.scale -= 0.01f;
                    if (Projectile.scale < 0.02F)
                    {
                        Projectile.Kill();
                    }
                }
            }
            else if (Projectile.ai[2] == 1)
            {
                Player player = Main.player[Main.npc[(int)Projectile.ai[0]].target];
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.ai[1]++;
                if (Projectile.ai[1] < 7)
                {
                    Projectile.scale += 0.2F;
                    Projectile.Center += player.Dplayer().PrePosition;
                }
                else if (Projectile.ai[1] < 30)
                {
                    if (Projectile.localAI[0] < 2)
                        Projectile.localAI[0] += 0.05f;
                    Projectile.Center += player.Dplayer().PrePosition;
                }
                else if (Projectile.ai[1] == 30)
                {
                   NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2().PerfectNormalize() * 10, ModContent.ProjectileType<BossArousalStar>(), Projectile.damage, Projectile.knockBack, Main.myPlayer,2,0.25F);
                }
                else
                {
                    Projectile.scale -= 0.04f;
                    if (Projectile.scale < 0.02F)
                    {
                        Projectile.Kill();
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.localAI[1] += 0.1F;
            DDHelper.MaxandMinF(ref Projectile.localAI[0], 2, 1);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D texture2 = DDTextures.VoidStar.Value;
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Color color2 = Projectile.GetAlpha(new Color(0, 100, 255, 0));
            DDHelper.Compression(texture2, new Color(0, 150, 255)*0.5F, 0, 1.5F, new Vector2(Projectile.localAI[0] * 2, 1.5F), 0, 0, BlendState.Additive);
            Main.spriteBatch.Draw(texture2, vector2, null, color2, Projectile.rotation, texture2.Size() / 2, new Vector2(Projectile.scale , Projectile.scale)*2, 0, 0f);
            Main.spriteBatch.Draw(texture2, vector2, null, color2, Projectile.rotation, texture2.Size() / 2, new Vector2(Projectile.scale , Projectile.scale)*2, 0, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            DDHelper.Compression(texture, new Color(0, 50, 255), 0, 5, new Vector2(Projectile.localAI[0] * 2, 1), 0, Projectile.localAI[1], BlendState.Additive);
            Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale, Projectile.scale), SpriteEffects.None, 0);
               Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            
            return false;
        }
    }
}