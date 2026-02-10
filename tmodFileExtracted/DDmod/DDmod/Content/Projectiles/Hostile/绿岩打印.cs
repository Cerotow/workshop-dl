using DDmod.Content.Dusts;
using DDmod.Content.NPCs.IittleMonster.绿岩;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.UI;

namespace DDmod.Content.Projectiles.Hostile
{
    public class 绿岩打印 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orange Laser");
            //DisplayName.AddTranslation(7, "橙激光");
        }
        public static Asset<Texture2D> P;
        public override void Load()
        {
            P = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Hostile/绿岩侦察机Proj");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            Projectile.timeLeft = 5;
            Projectile.ai[0]++;
            if(!NPCDowned.绿岩刷怪)
            {
                for (int A = 0; A < 30; A++)
                {
                    int DU = NewDust(Projectile.Center - new Vector2(4, 4) - new Vector2(40, -14) - new Vector2(48, 44) / 2, 48, 44, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6), Scale: Main.rand.NextFloat(0.75F, 1.25F));
                }
                Projectile.Kill();
                return;
            }
            if (Projectile.ai[0] == 75&& NPCDowned.绿岩刷怪)
            {
                if (Projectile.ai[2] == 0)
                {
                    if (Main.netMode != 1)
                    {
                        int N = DNPC.NewNPCs(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(40, -14), ModContent.NPCType<绿岩侦察机>(), 0, 0, 0, -1);
                        Main.npc[N].velocity.X = -1;
                        Main.npc[N].Dnpc().TETile = new Point16((int)Projectile.DProj().vector[0].X, (int)Projectile.DProj().vector[0].Y);
                    }
                    for (int A = 0; A < 30; A++)
                    {
                        int DU = NewDust(Projectile.Center - new Vector2(4, 4) - new Vector2(40, -14) - new Vector2(48, 44) / 2, 48, 44, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6), Scale: Main.rand.NextFloat(0.75F, 1.25F));
                    }
                }
                else
                {
                    if (Main.netMode != 1)
                    {
                        int N = DNPC.NewNPCs(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(40, 14), ModContent.NPCType<绿岩侦察机>(), 0, 0, 0, 1);
                        Main.npc[N].velocity.X = 1;
                        Main.npc[N].Dnpc().TETile = new Point16((int)Projectile.DProj().vector[0].X, (int)Projectile.DProj().vector[0].Y);
                    }
                    for (int A = 0; A < 30; A++)
                    {
                        int DU = NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(40, 14) - new Vector2(48, 44) / 2, 48, 44, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6), Scale: Main.rand.NextFloat(0.75F, 1.25F));
                    }
                }
            }
            Lighting.AddLight(Projectile.Center, new Color(100, 255, 100).ToVector3() * 1.5F);
            if (Projectile.ai[0] > 75)
            {

                Projectile.ai[1] -= 0.1F;
            }
            else
            {
                if (Projectile.ai[1] < 1)
                {
                    Projectile.ai[1] += 0.1F;
                }
            }

            if (Projectile.ai[2] == 0)
            {
                int D = NewDust(Projectile.Center - new Vector2(0, 19), 1, 30, ModContent.DustType<绿岩粒子>(), -Main.rand.NextFloat(2, 12), Scale: Main.rand.NextFloat(0.75F, 1.25F));
                Main.dust[D].velocity.Y = (Main.dust[D].position.Y + 4 - Projectile.Center.Y) / 4;
                if (Main.rand.NextBool(2))
                {
                    D = NewDust(Projectile.Center - new Vector2(0, 19), 1, 30, ModContent.DustType<绿岩电光粒子>(), -Main.rand.NextFloat(2, 6), Scale: Main.rand.NextFloat(0.75F, 1.25F));
                    Main.dust[D].velocity.Y = (Main.dust[D].position.Y + 4 - Projectile.Center.Y) / 4;
                }
            }
            else
            {
                int D = NewDust(Projectile.Center - new Vector2(0, 19), 1, 30, ModContent.DustType<绿岩粒子>(), Main.rand.NextFloat(2, 12), Scale: Main.rand.NextFloat(0.75F, 1.25F));
                Main.dust[D].velocity.Y = (Main.dust[D].position.Y + 4 - Projectile.Center.Y) / 4;
                if (Main.rand.NextBool(2))
                {
                    D = NewDust(Projectile.Center - new Vector2(0, 19), 1, 30, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(2, 6), Scale: Main.rand.NextFloat(0.75F, 1.25F));
                    Main.dust[D].velocity.Y = (Main.dust[D].position.Y + 4 - Projectile.Center.Y) / 4;
                }
            }
            if (Projectile.ai[0] > 85)
            {
                Projectile.Kill();
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
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
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height / 2);
            if (Projectile.ai[2] == 0)
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(100, 255, 100, 0) * Projectile.ai[1], Projectile.rotation, new Vector2(texture.Width, texture.Height / 2), Projectile.scale, 0, 0f);
                if (Projectile.ai[0] < 75)
                {
                    Rectangle rectangle = new Rectangle(0, 0, P.Width(), P.Height() / 2);
                    Main.spriteBatch.Draw(P.Value, Projectile.position + vector - new Vector2(40, 0) - Main.screenPosition, rectangle, new Color(255, 255, 255, 0) * 0.5F * Projectile.ai[1], Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
                    rectangle = new Rectangle(0, P.Height() / 2, P.Width(), (int)(P.Height() / 2 * (Projectile.ai[0] / 75F)));
                    Main.spriteBatch.Draw(P.Value, Projectile.position + vector - new Vector2(40, P.Height() / 4) - Main.screenPosition, rectangle, lightColor, Projectile.rotation, new Vector2(rectangle.Width / 2, 0), Projectile.scale, 0, 0f);
                }
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(100, 255, 100, 0) * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, new Vector2(texture.Width, texture.Height / 2), Projectile.scale, 0, 0f);
                if (Projectile.ai[0] < 75)
                {
                    Rectangle rectangle = new Rectangle(0, 0, P.Width(), P.Height() / 2);
                    Main.spriteBatch.Draw(P.Value, Projectile.position + vector + new Vector2(40, 0) - Main.screenPosition, rectangle, new Color(255, 255, 255, 0) * 0.5F * Projectile.ai[1], Projectile.rotation, rectangle.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
                    rectangle = new Rectangle(0, P.Height() / 2, P.Width(), (int)(P.Height() / 2 * (Projectile.ai[0] / 75F)));
                    Main.spriteBatch.Draw(P.Value, Projectile.position + vector + new Vector2(40, -P.Height() / 4) - Main.screenPosition, rectangle, lightColor, Projectile.rotation, new Vector2(rectangle.Width / 2, 0), Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
                }
            }
            return false;
        }
    }
}