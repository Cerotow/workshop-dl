using DDmod.Content.Dusts;
using DDmod.Modkey;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class GhostFireLantern : Talismans
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] =11;
        }
        public override bool MobileAI()
        {
            Projectile.frame = 0;
            return base.MobileAI();
        }
        public override void PreUse()
        {
            if(Projectile.frame<=0)
            {
                Projectile.frame = 1;
            }
            Projectile.frameCounter++;
            if(Projectile.frameCounter>=8)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame>= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 1;
                }
            }
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().Times[0]++;
            //遍历npc
            for (int A = 0; A < 200; A++)
            {
                NPC npc = Main.npc[A];
                if (npc.CanBeChasedBy(Projectile) && Projectile.DProj().Times[0] % 10 == 0)
                {
                    //让玩家可以踩npc
                    if (DDHelper.CircleInsertRectangle(npc.Hitbox, Projectile.Center, 256  * 0.3F))
                    {
                        npc.SimpleStrikeNPC(Projectile.damage, 0, Main.rand.Next(100) < Projectile.CritChance);
                    }
                }
            }
            Vector2 vector = player.Dplayer().MouseWorld - Projectile.Center;
            float S = vector.Length();
            if (S > 20) S = 20;
            Projectile.velocity = (Projectile.velocity*20+vector.PerfectNormalize() * S)/21;
            if (S < 0.1F)
            {
                Projectile.velocity = Vector2.Zero;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.X * 0.05F;
            Projectile.spriteDirection = -player.direction;

            SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == 1|| (Projectile.DProj().Bool[0]&&Projectile.velocity.X<0))
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]);
            Color color = new Color(23, 147, 234, 0);
            Vector2 vector = Projectile.Size / 2;
            Texture2D Perlin = DDTextures.Perlin.Value;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, 1, spriteEffects, 0f);
            if (Projectile.DProj().Bool[0])
            {
                Lighting.AddLight(Projectile.Center, new Color(96, 255, 255).ToVector3());
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                Projectile.ai[0] += 0.02f;
                Vector2 origin = Perlin.Size() / 2;
                DDHelper.AnnularShaders(2, color, Projectile.ai[0]);
                Main.spriteBatch.Draw(Perlin, Projectile.position + vector - Main.screenPosition, null, new Color(96, 255, 255), 0, origin, 0.3f, 0, 0);
                Main.spriteBatch.Draw(Perlin, Projectile.position + vector - Main.screenPosition, null, new Color(96, 255, 255), MathHelper.TwoPi/5, origin, 0.3f, 0, 0);
                Main.spriteBatch.Draw(Perlin, Projectile.position + vector - Main.screenPosition, null, new Color(96, 255, 255), MathHelper.TwoPi / 5*2, origin, 0.3f, 0, 0);
                Main.spriteBatch.Draw(Perlin, Projectile.position + vector - Main.screenPosition, null, new Color(96, 255, 255), MathHelper.TwoPi / 5*3, origin, 0.3f, 0, 0);
                Main.spriteBatch.Draw(Perlin, Projectile.position + vector - Main.screenPosition, null, new Color(96, 255, 255), MathHelper.TwoPi / 5*4, origin, 0.3f, 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            else
            {
                Lighting.AddLight(Projectile.Center, new Color(96, 255, 255).ToVector3()*0.4f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                Projectile.ai[0] += 0.2f;
                Vector2 origin = Perlin.Size() / 2;
                DDHelper.AnnularShaders(4, color, Projectile.ai[0]);
                Main.spriteBatch.Draw(Perlin, Projectile.position + vector - Main.screenPosition, null, new Color(96, 255, 255), -MathHelper.PiOver2, origin, 0.3f*0.4F, 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
    }
}