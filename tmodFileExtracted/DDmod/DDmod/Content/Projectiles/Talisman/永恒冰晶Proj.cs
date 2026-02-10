using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Modkey;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class 永恒冰晶Proj : Talismans
    {
        public static Asset<Texture2D> Glow;
        public override void SetStaticDefaults()
        {
        }
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
        }
        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().Times[0]++;
            Projectile.ai[0] += 0.005f;
            //遍历npc
            for (int A = 0; A < 200; A++)
            {
                NPC npc = Main.npc[A];
                if (npc.active && npc.CanBeChasedBy())
                {
                    if (DDHelper.CircleInsertRectangle(npc.Hitbox,Projectile.Center, 256 / 2))
                    {
                        npc.AddBuff(ModContent.BuffType<Frozen>(), 180);
                    }
                    if (DDHelper.CircleInsertRectangle(npc.Hitbox, Projectile.Center, 256 / 2*0.2F) && !npc.Dnpc().BossPhysique)
                    {
                        npc.AddBuff(ModContent.BuffType<Freeze>(), 180);
                    }
                }
            }
            if(Projectile.DProj().vector[0]!= Projectile.Center)
            {
                NewDustChange(60, Projectile.Center, Vector2.Zero, 135, 0.1F, 5, Scale: Main.rand.NextFloat(1.2F, 1.8F));
                Projectile.Center = Projectile.DProj().vector[0];
                NewDustChange(60, Projectile.Center, Vector2.Zero, 135, 0.1F, 5, Scale: Main.rand.NextFloat(1.2F, 1.8F));
            }
            Projectile.velocity = Vector2.Zero;
        }
        public override void ExtraUse()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().vector[0] = player.Dplayer().MouseWorld;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.X * 0.05F;
            Projectile.rotation += MathHelper.Pi;
            Projectile.spriteDirection = -player.direction;

            SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == 1|| (Projectile.DProj().Bool[0]&&Projectile.velocity.X<0))
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(0, 155, 255, 0);
            Vector2 vector = Projectile.Size / 2;
            Texture2D Perlin = DDTextures.Perlin2.Value;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            if (Projectile.DProj().Bool[0])
            {
                Lighting.AddLight(Projectile.Center, new Color(96, 255, 255).ToVector3());
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                
                Vector2 origin = Perlin.Size() / 2;
                DDHelper.RotundityShaders(1, color, Projectile.ai[0], 1);
                Main.spriteBatch.Draw(Perlin, Projectile.position + vector - Main.screenPosition, null, color, -MathHelper.PiOver2, origin, 1f, 0, 0);
                DDHelper.RotundityShaders(1, color, Projectile.ai[0]*2, 1);
                Main.spriteBatch.Draw(Perlin, Projectile.position + vector - Main.screenPosition, null, color, -MathHelper.PiOver2, origin, 1f, 0, 0);
                DDHelper.RotundityShaders(1, color, Projectile.ai[0]*3, 1);
                Main.spriteBatch.Draw(Perlin, Projectile.position + vector - Main.screenPosition, null, color, -MathHelper.PiOver2, origin, 1f, 0, 0);
                DDHelper.AnnularShaders(5, color, Projectile.ai[0]);
                Main.spriteBatch.Draw(Perlin, Projectile.position + vector - Main.screenPosition, null, color, -MathHelper.PiOver2, origin, 0.2f, 0, 0);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                Main.spriteBatch.Draw(Glow.Value, Projectile.position + vector - Main.screenPosition, null, new Color(255,255,255,0), Projectile.rotation, Glow.Size() / 2, 0.25f, spriteEffects, 0f);
            }
            return false;
        }
    }
}