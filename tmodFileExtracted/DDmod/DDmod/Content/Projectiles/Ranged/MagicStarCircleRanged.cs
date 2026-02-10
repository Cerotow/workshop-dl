using DDmod.Content.Items;
using DDmod.Content.Projectiles.Ranged.Bow;

namespace DDmod.Content.Projectiles.Ranged
{
    public class MagicStarCircleRanged : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 100;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.02F;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity = Vector2.Zero;
            Projectile.DProj().Times[0] += 0.11f;
            for (int a = 0; a < 1000;a++)
            {
                if (Main.projectile[a].active && Main.projectile[a].type == ModContent.ProjectileType<GlobalBow>() && Main.projectile[a].owner==Projectile.owner)
                {
                    player.heldProj = a;
                    break;
                }
            }
            if (player.heldProj >= 0 && Projectile.ai[1] == 0)
            {
                Projectile.timeLeft = 200;
                if (!player.controlUseTile)
                {
                    Projectile.ai[1] = 1;
                    Projectile.netUpdate = true;
                    return;
                }
                Projectile projectile = Main.projectile[player.heldProj];
                RangedProjectile ranged = projectile.GetGlobalProjectile<RangedProjectile>();
                Item item = Projectile.Player().ActiveItem();
                float Scale = ranged.BowTime / player.itemAnimationMax;
                if (Scale > 1.8F)
                {
                    Scale = 1.8F;
                }
                Projectile.scale = Scale/2;
                if(Projectile.timeLeft<50)
                {
                    Projectile.ai[1] = 2;
                }
                if (player.dead)
                {
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    Projectile.Kill();
                    return;
                }
                Vector2 vector = player.Dplayer().MouseWorld - projectile.Center;
                Projectile.Center = projectile.Center + vector.PerfectNormalize() * 200;
                Projectile.rotation = projectile.rotation;
            }
            else if(Projectile.ai[1] == 1)
            {
                if (Projectile.timeLeft < 50)
                {
                    Projectile.ai[1] = 2;
                }
                Projectile projectile;
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int T = 0; T < 1000; T++)
                    {
                        projectile = Main.projectile[T];
                        if (projectile.active && projectile.type > 0 && projectile.GetGlobalProjectile<RangedProjectile>().copy)
                        {
                            Vector2 vector = projectile.Center - Projectile.Center;
                            if (vector.Length() <= 50)
                            {
                                for (float a = -Projectile.scale; a <= Projectile.scale; a += 0.2f)
                                {
                                    Vector2 Pvelocity = Utils.RotatedBy(projectile.velocity, 0.15F * a, default);

                                    NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Pvelocity, projectile.type, projectile.damage / 2, projectile.knockBack, player.whoAmI, 0f, 0f);
                                }
                                projectile.active = false;
                                Projectile.ai[1] = 2;
                                projectile.GetGlobalProjectile<RangedProjectile>().copy = false;
                                break;
                            }
                            Projectile.netUpdate = true;
                        }
                    }
                }
            }
            else
            {
                Projectile.scale += 0.08f;
                Projectile.alpha += 20;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2 = DDTextures.Circle[9].Value;
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Color color2 = Projectile.GetAlpha(new Color(0, 100, 255, 0)) * 0.66F;
            Texture2D texture = DDTextures.VoidStar.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Color color = Projectile.GetAlpha(new Color(0, 100, 255, 0));
            Main.spriteBatch.Draw(texture, vector, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale/4, Projectile.scale), 0, 0f);

            DDHelper.Compression(texture2, color, Projectile.rotation, Projectile.Opacity, new Vector2(6, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);

            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale/4, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}