using DDmod.Content.Tiles.农场;
using DDmod.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.农场.水壶
{
    public class 水滴 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("水滴");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            Main.projFrames[Projectile.type] = 8;
        }
        bool A;
        public override bool? CanCutTiles()
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            if (!A)
            {
                Vector2 Position = Projectile.position / 16.0f;
                for (int a = 0; a < Projectile.width / 16 + 2; a++)
                {
                    for (int b = 0; b < Projectile.height / 16 + 2; b++)
                    {
                        if (Main.tile[(int)Position.X + a, (int)Position.Y + 1 + b].TileType == ModContent.TileType<锄过的土块>())
                        {
                            if (锄土.FindFirstTile(new Point16((int)Position.X + a, (int)Position.Y + 1 + b), out int type) >= 0)
                            {
                                if (Main.netMode != 1)
                                {
                                    DDWorld.土[type].DampTime = DDHelper.Second(300);
                                }
                                
                                if (Main.netMode == NetmodeID.MultiplayerClient)
                                {
                                    ModPacket packet = DDmod.Instance.GetPacket(256);
                                    //写入要发的包
                                    packet.Write((byte)DDType.锄地);
                                    packet.Write((short)(Position.X + a));
                                    packet.Write((short)(Position.Y + 1 + b));
                                    packet.Write((short)type);
                                    packet.Write(DDHelper.Second(300));
                                    //发出去
                                    packet.Send(-1, -1);
                                }
                            }
                        }
                    }
                }
                Projectile.position.Y += 4;
                A = true;
            }
            return false;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.05f;
            Projectile.velocity.X *= 0.97f;

            Projectile.frameCounter++;
            if (Projectile.wet)
            {
                Projectile.Kill();
            }
            if (!A)
            {
                if (Projectile.frameCounter > 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame >= 3)
                {
                    Projectile.frame = 0;
                    return;
                }
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                if (Projectile.frameCounter > 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame >= 8)
                {
                    Projectile.Kill();
                    return;
                }
                //TileLoader.RandomUpdate((int)Position.X,(int)Position.Y, (int)Main.tile[(int)Position.X, (int)Position.Y].type);
                //TileLoader.RandomUpdate((int)Position.X,(int)Position.Y+1, (int)Main.tile[(int)Position.X, (int)Position.Y+1].type);

            }
            Projectile.ProjScale(-0.3F);
        }
        public override bool PreDraw(ref Color drawColor)
        {
            Player player = Main.player[Projectile.owner];
            SpriteEffects spriteEffects = (SpriteEffects)((Projectile.direction == -1) ? 1 : 0);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            drawColor.A = 200;
            if (spriteEffects == (SpriteEffects)1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 8 * Projectile.frame, texture.Width, texture.Height / 8)), drawColor, Projectile.rotation, new Vector2((float)(texture.Width /2), (float)(texture.Height / 16)), Projectile.scale, spriteEffects, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 8 * Projectile.frame, texture.Width, texture.Height / 8)), drawColor, Projectile.rotation, new Vector2((float)(texture.Width / 2), (float)(texture.Height / 16)), Projectile.scale, spriteEffects, 0f);
            }
            return false;
        }
    }
}