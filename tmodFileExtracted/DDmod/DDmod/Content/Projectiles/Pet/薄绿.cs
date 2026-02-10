using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;
using DDmod.Content.Projectiles.Pet.PetBuff;
using static Terraria.GameContent.Bestiary.BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;

namespace DDmod.Content.Projectiles.Pet
{
    public class 薄绿 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Tairitsu Slime");
           //DisplayName.AddTranslation(7, "对立史莱姆");
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            Main.projFrames[Projectile.type] = 9;
            Projectile.netImportant = true;
            Projectile.width = 24;
            Projectile.height = 44;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.scale = 1.15f;
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.frame = reader.ReadInt32();
            SJ = reader.ReadInt32();
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.frame);
            writer.Write(SJ);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        public override void PostDraw(Color lightColor)
        { 
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.ai[1] == 1)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition;
                    Color color = lightColor * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)*0.5f;
                    Main.spriteBatch.Draw(texture, vector2 + new Vector2(0, 2), new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)(Projectile.spriteDirection == 0 ? 0 : 1), 0f);
                }
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, 2), new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)(Projectile.spriteDirection == 0 ? 0 : 1), 0);


            /*if (Projectile.ai[1] != 1)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition;
                    Color color = new Color(132, 0, 255, 150) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture, vector2 - new Vector2(0, 2), new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)(Projectile.spriteDirection == 0 ? 0 : 1), 0f);
                }
            }*/
        }
        //开场白
        public bool HB;
        //跳跃
        int dt;
        bool dtq;

        //睡觉
        int SJ;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CheckActive(player);
            Movement(player);
            Vector2 vector = (player.Center - new Vector2(100 * player.direction, 0)) - Projectile.Center;
            //左边有物块
            //bool TileCollision = Collision.SolidCollision(new Vector2(Projectile.position.X-1, (Projectile.position.Y)), 1, Projectile.height);
            if (Projectile.velocity.X==0)
            {
                Projectile.spriteDirection = 0;
                if(player.direction==-1)
                {
                    Projectile.spriteDirection = 1;
                }
            }
            else
            {
                Projectile.spriteDirection = 0;
                if (Projectile.velocity.X<0)
                {
                    Projectile.spriteDirection = 1;
                }
            }

            if (Projectile.ai[1] == 0)
            {
                Times += Math.Abs(Projectile.velocity.X);
                if (Times > 15)
                {
                    Times -= 15;
                    Projectile.frame++;
                }
                if (Projectile.frame >= 5)
                {
                    Projectile.frame = 1;
                }
                Projectile.rotation = 0;
                if (Math.Abs(Projectile.velocity.X) <= 0.01f)
                {
                    Projectile.frame = 0;
                    Projectile.rotation = 0;
                }
            }
            else
            {
                Times += 3;
                if (Times > 15)
                {
                    Times -= 15;
                    Projectile.frame++;
                }
                if (Projectile.frame >= 9)
                {
                    Projectile.frame = 5;
                }
            }
        }
        float Times;
        private void CheckActive(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<薄绿Buff>()))
            {
                Projectile.timeLeft = 2;
            }
            else if (!player.dead)
            {
                Projectile.Kill();
            }
        }
        private bool Movement(Player player)
        {
            Vector2 direction = player.Center - Projectile.Center;
            float Speed = Math.Abs(direction.X) / 50;
            if (Speed > 12)
            {
                Speed = 12;
            }
            else if (Speed < 5f)
            {
                Speed = 5f;
            }
            if (Projectile.velocity.X > 0)
            {
                //Projectile.spriteDirection = -1;
            }
            else
            {
                //Projectile.spriteDirection = 0;
            }
            bool TileCollision = Collision.SolidCollision(new Vector2(Projectile.position.X-1, (Projectile.position.Y)), Projectile.width+2, Projectile.height);
            if (Projectile.ai[1] == 0)
            {
                direction.X -= 80 * player.direction;
                if (Projectile.velocity.Y == 0)
                {
                    if (direction.Y < -100 || (Math.Abs(direction.X) > 5 && Projectile.velocity.X == 0&& TileCollision))
                    {
                        Projectile.velocity.Y = -8;
                    }
                }
                if (Projectile.velocity.Y < 10) Projectile.velocity.Y += 0.4F;
                if ((direction.Y < -150 || Math.Abs(direction.X) > 500))
                {
                    Projectile.ai[1] = 1;
                }
                direction = direction.PerfectNormalize();
                Projectile.velocity.X = direction.X * Speed;

                if (Math.Abs(Projectile.velocity.X) < 2)
                {
                    Projectile.velocity.X = 0;
                    if (player.direction == 1)
                    {
                        //Projectile.spriteDirection = -1;
                    }
                    else
                    {
                        //Projectile.spriteDirection = 0;
                    }
                }
                Projectile.tileCollide = true;
                if (direction.Y > Projectile.height / 2)
                {
                    Projectile.tileCollide = false;
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.X * 0.03F;
                float DistanceNPC = (player.Center - Projectile.Center).Length();
                if(Main.rand.NextBool(5) && player.velocity.Length()>0.5F)
                {
                    int a = Dust.NewDust(Projectile.position,Projectile.width,Projectile.height,ModContent.DustType<速度粒子>(),0,0,100,new Color(155,155,155,50),Main.rand.NextFloat(1,2));
                    Main.dust[a].velocity = -Projectile.velocity/4;
                    Main.dust[a].rotation = Main.dust[a].velocity.ToRotation();
                }
                if (player.velocity.Y == 0)
                {
                    direction = player.Center - Projectile.Center;
                    if (direction.Length()>400|| Collision.CanHitLine(Projectile.Center, 1, 1, player.position, player.width, player.height))
                    {
                        Projectile.tileCollide = false;
                    }
                    direction.X -= 80 * player.direction;
                    if (Projectile.velocity.Length() < 2)
                    {
                        if (player.direction == 1)
                        {
                           // Projectile.spriteDirection = -1;
                        }
                        else
                        {
                            //Projectile.spriteDirection = 0;
                        }
                    }
                    Projectile.velocity = (Projectile.velocity * 20 + direction.PerfectNormalize() * 12) / 21;

                    Projectile.rotation = Projectile.velocity.X * 0.05f;
                    if (player.velocity.Y == 0 && (Math.Abs(direction.Y) < 5 || Math.Abs(direction.X) < 5))
                    {
                        for (int a = 0; a < 7; a++)
                        {
                            if (Main.tile[(int)((player.Center.X) / 16), (int)((Projectile.Center.Y) / 16) + a].HasTile)
                            {
                                Projectile.ai[1] = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (player.velocity.Length() > 20 || direction.Length() > 300)
                    {
                        direction = player.Center - new Vector2(0, 100) - Projectile.Center;
                        float speed = Speed + player.velocity.Length();
                        Projectile.netUpdate = true;
                        if (DistanceNPC > 3000f)
                        {
                            Projectile.Center = player.Center;
                        }
                        direction = direction.PerfectNormalize();
                        direction *= speed;
                        float temp = 60;
                        Projectile.velocity = (Projectile.velocity * temp + direction) / (temp + 1);
                    }
                    else
                    {
                        float speed = 2f;
                        Projectile.netUpdate = true;
                        direction.Y -= 350f;
                        if (DistanceNPC > 3000f)
                        {
                            Projectile.Center = player.Center;
                        }
                        if (Projectile.velocity.Length() <= 5)
                        {
                            Projectile.velocity.X *= 1.02f;
                        }
                        if (direction.Length() > 300f)
                        {
                            direction.Normalize();
                            direction *= speed;
                            float temp = 10;
                            Projectile.velocity = (Projectile.velocity * temp + direction) / (temp + 1);
                        }
                    }
                }
            }
            return true;
        }
    }
}
