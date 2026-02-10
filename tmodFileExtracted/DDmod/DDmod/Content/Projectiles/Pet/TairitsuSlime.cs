using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;
using DDmod.Content.Projectiles.Pet.PetBuff;

namespace DDmod.Content.Projectiles.Pet
{
    public class TairitsuSlime : ModProjectile
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            Main.projFrames[Projectile.type] = 8;
            Projectile.netImportant = true;
            Projectile.width = 46;
            Projectile.height = 32;
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
        int Tailframe;
        int Tailframe2;
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/TairitsuSlime_Tail");
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(4* (Projectile.spriteDirection == 0 ? -1 : 1), -2), new Rectangle?(new Rectangle(0, texture.Height / 4 * Tailframe, texture.Width, texture.Height / 4)), lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / 4) / 2, Projectile.scale, (SpriteEffects)(Projectile.spriteDirection == 0 ? 0 : 1), 0);
            texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition - new Vector2(0, 2), new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)(Projectile.spriteDirection == 0 ? 0 : 1), 0);


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
        //起飞绘制
        public float T1 = 0;
        public float T2 = 0.33f;
        public float T3 = 0.66f;
        public float H2 = 1;
        //跳跃
        int dt;
        bool dtq;

        //睡觉
        int SJ;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] == 1)
            {
                T1 += 0.033f;
                T2 += 0.033f;
                T3 += 0.033f;
                if (T1 > 1f) T1 = 0;
                if (T2 > 1f) T2 = 0;
                if (T3 > 1f) T3 = 0;
                for (int a = 0; a < 3; a++)
                {
                    Vector2 projDirection = Utils.RotatedBy(new Vector2(0, Projectile.height / 2), Projectile.rotation, default);
                    Main.EntitySpriteDraw(DDTextures.Circle[7].Value, Projectile.Center + projDirection - Main.screenPosition, null, new Color(80, 222, 232, 0) * (1 - T1), Projectile.rotation, DDTextures.Circle[7].Size() / 2, new Vector2(Projectile.scale * 0.8f, 0.2f) * T1, 0, 0);
                    Main.EntitySpriteDraw(DDTextures.Circle[7].Value, Projectile.Center + projDirection - Main.screenPosition, null, new Color(80, 222, 232, 0) * (1 - T2), Projectile.rotation, DDTextures.Circle[7].Size() / 2, new Vector2(Projectile.scale * 0.8f + 0.01F, 0.21f) * T2, 0, 0);
                    Main.EntitySpriteDraw(DDTextures.Circle[7].Value, Projectile.Center + projDirection - Main.screenPosition, null, new Color(80, 222, 232, 0) * (1 - T3), Projectile.rotation, DDTextures.Circle[7].Size() / 2, new Vector2(Projectile.scale * 0.8f + 0.02F, 0.22f) * T3, 0, 0);
                }
            }
            return false;
        }
        public override void AI()
        {
            Tailframe2++;
            if (Tailframe2 > 8)
            {
                Tailframe2 = 0;
                DDHelper.BackAndForthInt(0, 2, 1, ref Tailframe, ref Projectile.DProj().Bool[0]);
            }
            Lighting.AddLight(Projectile.Center - new Vector2(0, 4), new Color(210, 55, 25).ToVector3() * 0.5F);
            Player player = Main.player[Projectile.owner];
            CheckActive(player);

            Vector2 vector = (player.Center - new Vector2(100 * player.direction, 0)) - Projectile.Center;
            Vector2 vector2 = vector;

            if (Projectile.frameCounter == 0)
            {
                Projectile.frame = 0;
            }
            else if (Projectile.velocity.Y < 0)
            {
                Projectile.frame = 1;
            }
            else if (Projectile.velocity.Y > 0)
            {
                Projectile.frame = 3;
            }
            else if (Projectile.frameCounter > 4)
            {
                Projectile.frame = 3;
            }
            else
            {
                Projectile.frame = 5;
            }
            Projectile.tileCollide = (Projectile.ai[1] == 0 && Projectile.velocity.Y > 0) || Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, player.position, player.width, player.height) && player.Center.Y - Projectile.Center.Y < 100;
            Projectile.rotation = 0;
            if (Projectile.ai[1] == 1 || Projectile.velocity == Vector2.Zero)
            {
                Projectile.ai[0]++;
                if (Projectile.ai[0] < 8)
                {
                    Projectile.frame = 4;
                }
                else
                {
                    Projectile.frame = 0;
                }
                if (Projectile.ai[0] > 24) Projectile.ai[0] = 8;
            }
            else
            {
                Projectile.ai[0] = 0;
            }
            if (Projectile.velocity == Vector2.Zero)
            {
                if (Projectile.DProj().Times[0] > 0)
                {
                    Projectile.frame = 7;
                }
                if (Projectile.owner == Main.myPlayer && (Projectile.Center - Main.MouseWorld).Length() < 20 && player.controlUseTile && Projectile.DProj().Times[0] < -120)
                {
                    for (int A = 0; A < 5; A++)
                    {
                        int Dust = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<爱心粒子>());
                        Main.dust[Dust].velocity = new Vector2(0, -2).RotatedBy(Main.rand.NextFloat(-0.4F, 0.4F));
                    }
                    Projectile.DProj().Times[0] = 60;
                    SJ = 0;
                    Projectile.netUpdate = true;
                }
                else
                {
                    Projectile.DProj().Times[0]--;
                    if (Projectile.DProj().Times[0] == 0)
                    {
                        Projectile.frame = 0;
                        Projectile.netUpdate = true;
                    }
                }
                if (player.sleeping.FullyFallenAsleep && Projectile.frame != 7)
                {
                    Projectile.frame = 6;
                    SJ++;
                    if (SJ % 100 == 0)
                    {
                        CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(80,222,232, 255) * 0.4f, "ZZZ..", false, false);
                    }
                }
            }
            else
            {
                if (player.sleeping.FullyFallenAsleep)
                {
                    SJ++;
                    Projectile.frame = 6;
                    if (SJ % 100 == 0)
                    {
                        CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(80,222,232, 255) * 0.4f, "ZZZ..", false, false);
                    }
                }
            }
            if (Projectile.ai[1] == 1)
            {
                if (player.dead || !player.HasBuff(ModContent.BuffType<TairitsuSlimeBuff>())) return;
                vector = player.Center - new Vector2(0, Projectile.height / 2 + player.height / 2 + 50) - Projectile.Center;
                vector2 = vector;
                if (vector != Vector2.Zero) vector.Normalize();
                float sp = vector2.Length() / 10;
                if (sp > 15) sp = 15;
                if (sp < 0.5F) sp = 0;
                vector *= sp;
                Projectile.velocity = vector;
                if (Projectile.velocity.X > 0)
                {
                    Projectile.spriteDirection = 0;
                }
                else
                {
                    Projectile.spriteDirection = -1;
                }
                if (player.velocity.Y == 0 && Projectile.ai[1] == 1)
                {
                    for (int a = 0; a < 7; a++)
                    {
                        if (Main.tile[(int)((player.Center.X - 100 * player.direction) / 16), (int)((player.Center.Y + Projectile.height) / 16) + a].HasTile)
                        {
                            Projectile.ai[1] = 0;
                        }
                    }
                }
                Projectile.rotation = Projectile.velocity.X * 0.03f;
            }

            float Center = (player.Center.X - 100 * player.direction) - Projectile.Center.X;

            if (Center < 0) Center = -Center;

            if (vector != Vector2.Zero) vector.Normalize();

            if ((vector2.X > 50 || vector2.X < -50) || (vector2.Y > 140 || vector2.Y < -140))
            {
                if (Projectile.velocity.Y == 0 && Projectile.velocity.X == 0 && Projectile.ai[0] > 8)
                {
                    Projectile.velocity.Y = -6 - dt;
                    if (Center > 30)
                    {
                        Projectile.velocity.X = vector.X * 6;
                    }
                }
            }

            if (Projectile.velocity.Y != 0)
            {
                Projectile.frameCounter = 8;
                if (Projectile.ai[1] == 0)
                {
                    Projectile.velocity.Y += 0.4f;
                }
                if (Projectile.velocity.X > 0)
                {
                    Projectile.spriteDirection = 0;
                }
                else
                {
                    Projectile.spriteDirection = -1;
                }
            }
            else
            {
                if (Projectile.ai[0] < 4)
                {
                    Projectile.frame = 3;
                }
                else if (Projectile.ai[0] < 8)
                {
                    Projectile.frame = 4;
                }
                if ((vector2.X > 50 || vector2.X < -50 || vector2.Y > 140 || vector2.Y < -140) && Projectile.velocity.X == 0)
                {
                    if (Projectile.ai[0] > 8)
                    {
                        dt++;
                    }
                }
                else
                {
                    Projectile.velocity.X = 0;
                    dt = 0;
                }
                if (Projectile.frameCounter > 0)
                {
                    Projectile.frameCounter--;
                }
                if (player.Center.X > Projectile.Center.X)
                {
                    Projectile.spriteDirection = 0;
                }
                else
                {
                    Projectile.spriteDirection = -1;
                }
            }

            if (Center <= 50)
            {
                Projectile.velocity.X *= 0.9f;
            }
            if (vector2.Length() > 500 || vector2.Y < -140)
            {
                Projectile.ai[1] = 1;
            }
            if (vector2.Length() > 2000)
            {
                Projectile.Center = player.Center;
            }
        }
        private void CheckActive(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<TairitsuSlimeBuff>()))
            {
                Projectile.timeLeft = 2;
            }
            else if (!player.dead)
            {
                Projectile.Kill();
            }
        }
    }
}
