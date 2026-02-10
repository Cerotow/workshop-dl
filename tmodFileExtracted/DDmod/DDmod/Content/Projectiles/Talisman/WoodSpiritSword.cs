using DDmod.Modkey;

namespace DDmod.Content.Projectiles.Talisman
{
    public class WoodSpiritSword : Talismans
    {
        public override void Set()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            Projectile.width = 60;
            Projectile.height = 20;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override void PreUse()
        {
            //Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            Projectile.ai[0]++;
            //前进
            if (Projectile.ai[0] < 60)
            {
                Projectile.velocity = new Vector2(Projectile.localAI[0], 0);
            }
            else
            {
                //折返
                Projectile.velocity = new Vector2(-Projectile.localAI[0], 0);
            }
            if(Projectile.ai[0]>=114514)
            {
                Projectile.velocity *= 0.0001f;
                Projectile.tileCollide = false;
            }
            else
            {

                Projectile.tileCollide = true;
            }
            if(Projectile.velocity.X!=0)
            {
                if(Projectile.velocity.X>0)
                {
                    Projectile.spriteDirection = 1;
                }
                else
                {
                    Projectile.spriteDirection = 0;
                }
            }
            //遍历玩家
            for (int A = 0; A < 255; A++)
            {
                Player player = Main.player[A];
                //能踩的碰撞箱
                Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, 8);
                //玩家脚碰撞箱
                Rectangle playerRectangle = new Rectangle((int)(player.position.X), (int)player.position.Y + player.height -4, player.width, 4);
                //假如碰撞箱相交
                if (rectangle.Intersects(playerRectangle))
                {
                    //遍历npc
                    for (int T = 0; T < 200; T++)
                    {
                        NPC npc = Main.npc[T];
                        Rectangle projRrectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                        Rectangle npcRectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.height, npc.height);
                        if (npc.CanBeChasedBy(Projectile))
                        {
                            //如果玩家在剑上面,剑碰到npc给予玩家无敌
                            if (projRrectangle.Intersects(npcRectangle))
                            {
                                player.immune = true;
                                player.immuneNoBlink = true;
                                player.immuneTime = 2;
                            }
                        }
                    }
                    //检测玩家有没有按下
                    if (!player.controlDown)
                    {
                        //调整Y位置
                        player.position.Y = Projectile.position.Y - player.height + 4;
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
        public override bool Use()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = player.direction;
            Projectile.rotation = MathHelper.Pi+MathHelper.PiOver4+0.2F;
            if (Projectile.owner == Main.myPlayer && player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD && ModkeySetup.TalismanKey.JustPressed && !player.HasBuff(23))
            {
                Projectile.ai[0] = 0;
                Projectile.DProj().Bool[0] = true;
                Projectile.localAI[0] = Projectile.Player().direction * 5;
                player.TPlayer().TalismanTimes = player.TPlayer().MaxTalismanTimes;
                Projectile.Center = Projectile.Player().Center;
                Projectile.netUpdate = true;

            }
            return false;
        }
        public override void End()
        {
            Projectile.ai[0] = 0;
        }
        public override bool? CanDamage()
        {
            return Projectile.DProj().Bool[0];
        }
      
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0.01f;
            if (Projectile.ai[0] >= 1000)
            {
                Projectile.ai[0] = 114514;
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.ai[0] = 1000;
            }
            Projectile.netUpdate = true;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;

            SpriteEffects spriteEffects = 0;
            float ro =Projectile.rotation;
            if (Projectile.DProj().Bool[0])
            {
                if (Projectile.spriteDirection == 1)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    ro += MathHelper.PiOver2;
                }
            }
            else
            {
                if (Projectile.spriteDirection == 1)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    ro -= MathHelper.PiOver2+0.4F;
                }
            }
            Color color = new Color(0, 215, 0, 0);
            if (player.TPlayer().TalismanTimes>0)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                    Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture, vector2, null, oldcolor * 0.5f, ro, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
                }
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, ro, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);

            if (player.TPlayer().TalismanTimes <= 100&& (player.TPlayer().TalismanCD>= player.TPlayer().MaxTalismanCD||Projectile.DProj().Bool[0]))
            {
                DDHelper.BackAndForth(0, 1, 0.1f, ref Projectile.DProj().Times[2], ref Projectile.DProj().Bool[2]);
                Main.spriteBatch.Draw(texture, vector, null, color * Projectile.DProj().Times[2], ro, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            }
            else
            {
                Projectile.DProj().Times[2] = 1;
            }
            return false;
        }
    }
}