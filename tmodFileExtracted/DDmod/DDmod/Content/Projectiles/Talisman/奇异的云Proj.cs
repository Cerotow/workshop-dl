using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Modkey;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class 奇异的云Proj : Talismans
    {
        public static Asset<Texture2D> texture2;
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Eternal ice crystals");
           //DisplayName.AddTranslation(7, "奇异的云");
        }
        public override void Load()
        {
            texture2 = ModContent.Request<Texture2D>(Texture+"2");
        }
        public override void PreUse()
        {
            Projectile.velocity.X = Main.windSpeedCurrent;
            Projectile.DProj().vector[0].X+= Main.windSpeedCurrent;
            //遍历玩家
            for (int A = 0; A < 255; A++)
            {
                Player player = Main.player[A];
                //能踩的碰撞箱
                Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y+36, Projectile.width, 8);
                //玩家脚碰撞箱
                Rectangle playerRectangle = new Rectangle((int)(player.position.X), (int)player.position.Y + player.height - 16, player.width, 16);
                //假如碰撞箱相交
                if (rectangle.Intersects(playerRectangle))
                {
                    player.noFallDmg = true;
                    //检测玩家有没有按下
                    if (!player.controlDown)
                    {
                        if (player.velocity.Y > 2)
                        {
                            for (int I = 0; I < 60; I++)
                            {
                                Dust dust = Main.dust[NewDust(player.Center + new Vector2(-10, player.height / 2), 20, 1, 16, 0, 0, 0, Color.White)];
                                dust.noGravity = true;
                                dust.scale = 1.2F;
                                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(0, 2) * (player.velocity.Y/4);
                            }
                            Projectile.velocity.Y = player.velocity.Y;
                            player.velocity.Y *= -0.8f;
                        }
                        else
                        {
                            if(player.velocity.X!=0)
                            {
                                Dust dust = Main.dust[NewDust(player.Center + new Vector2(-10, player.height / 2-4), 20, 1, 16, 0, 0, 0, Color.White)];
                                dust.noGravity = true;
                                dust.scale = 1.6F;
                                dust.velocity = -player.velocity/4;
                            }
                            //调整Y位置
                            player.position.Y = Projectile.position.Y + 36 - player.height + 4;
                            //踩
                            if (player.velocity.Y >= 0)
                            {
                                player.Aplayer().Stand = 2;
                                int Width = 0;
                                if (Projectile.velocity.X > 0)
                                {
                                    Width = player.width;
                                }
                                //检测方块
                                if (!WorldGen.SolidTile((int)(player.position.X + Width) / 16, (int)player.position.Y / 16) && !WorldGen.SolidTile((int)(player.position.X + Width) / 16, (int)player.position.Y / 16 + 1) && !WorldGen.SolidTile((int)(player.position.X + Width) / 16, (int)player.position.Y / 16 + 2))
                                {
                                    player.position += Projectile.velocity;
                                }
                                else
                                {
                                    if (Projectile.velocity.X < 0)
                                    {
                                        player.position.X = (int)(player.Center.X / 16) * 16;
                                    }
                                    else
                                    {
                                        player.position.X = (int)(player.Center.X / 16) * 16 - 4;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Projectile.DProj().vector[1].Y < Projectile.position.Y)
            {
                Projectile.velocity.Y -=  (Projectile.position.Y- Projectile.DProj().vector[1].Y)/20;
            }
            if (Projectile.DProj().vector[1].Y > Projectile.position.Y)
            {
                Projectile.velocity.Y +=  (Projectile.DProj().vector[1].Y-Projectile.position.Y)/50;
            }
            Projectile.velocity.Y *= 0.9F;
            if (Projectile.DProj().vector[0]!= Projectile.Center&&!Projectile.DProj().Bool[1])
            {
                NewDustChange(60, Projectile.Center, Vector2.Zero, 16, 0.1F, 5, Scale: Main.rand.NextFloat(1.2F, 1.8F));
                Projectile.Center = Projectile.DProj().vector[0];
                Projectile.DProj().vector[1].Y = Projectile.Center.Y;
                NewDustChange(120, Projectile.position, Projectile.Size, 16, 0.1F, 5, Scale: Main.rand.NextFloat(2.2F, 2.8F));
                Projectile.DProj().Bool[1] = true;
            }
        }
        public override void End()
        {
            NewDustChange(120, Projectile.position, Projectile.Size, 16, 0.1F, 5, Scale: Main.rand.NextFloat(2.2F, 2.8F));
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.DProj().Bool[1] = false;
        }
        public override void ExtraUse()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().vector[0] = player.Center + new Vector2(0, 200);
            Projectile.width = 256;
            Projectile.height = 86;
            Projectile.DProj().Bool[1] = false;
            Projectile.DProj().vector[1].Y = Projectile.DProj().vector[0].Y;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.X * 0.05F;

            SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == 1|| (Projectile.DProj().Bool[0]&&Projectile.velocity.X<0))
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Size / 2;
            if (Projectile.DProj().Bool[0])
            {
                Projectile.spriteDirection = 0;
                DDHelper.BackAndForth(0, 0.1F, 0.002F, ref Projectile.DProj().Times[2], ref Projectile.DProj().Bool[3]);
                Main.spriteBatch.Draw(texture2.Value, Projectile.position + vector - Main.screenPosition, null, lightColor, Projectile.rotation, texture2.Size() / 2, new Vector2(1 + Projectile.DProj().Times[2], 1 - Projectile.DProj().Times[2]), spriteEffects, 0f);
                Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, new Color(0, 25, 40, 0));
                color.A = 0;
                Main.spriteBatch.Draw(texture2.Value, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture2.Size() / 2, new Vector2(1 + Projectile.DProj().Times[2], 1 - Projectile.DProj().Times[2]), spriteEffects, 0f);
            }
            else
            {
                Projectile.spriteDirection = -player.direction;
                DDHelper.BackAndForth(0, 0.2F, 0.005F, ref Projectile.DProj().Times[2],ref Projectile.DProj().Bool[3]);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, new Vector2(1+ Projectile.DProj().Times[2], 1-Projectile.DProj().Times[2]), spriteEffects, 0f);
            }
            return false;
        }
    }
}