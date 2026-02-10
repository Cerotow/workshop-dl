namespace DDmod.Content.Projectiles.RemoteControl
{
    public class MeteorExcavator : ModProjectile
    {
        public static Asset<Texture2D> Bar;
        public static Asset<Texture2D> Fill;
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Bar = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/RemoteControl/陨石挖掘机能量条");
                Fill = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/RemoteControl/陨石挖掘机能量");
                Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/RemoteControl/MeteorExcavator_Glow");
            }
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 150;
            Projectile.DProj().Detect = true;
        }
        public override void SetStaticDefaults()
        {
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadFloat();
            Projectile.localAI[1] = reader.ReadFloat();
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.dead)
            {
                Projectile.Kill();
            }
            Projectile.timeLeft = 5;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[1]++;
            Projectile.localAI[1]--;
            player.Dplayer().control = Projectile.whoAmI;
            player.itemTime = 5;
            player.itemAnimation = 5;
            Vector2 vector = player.Dplayer().MouseWorld - Projectile.Center;
            vector.DirectPerfectNormalize();
            if (player.controlUseItem && Projectile.ai[1] < 1800)
            {
                float A = 20f;
                Projectile.rotation = vector.ToRotation() + MathHelper.PiOver2;
                Projectile.tileCollide = true;
                if (player.controlUseTile)
                {
                    Projectile.ai[1]++;
                    A = 3;
                    if (Projectile.soundDelay == 0)
                    {
                        Projectile.soundDelay = 5;
                        PlaySound(SoundID.Item22, Projectile.position);
                    }
                    Vector2[] Tile = new Vector2[7];
                    Vector2[] TilePos = new Vector2[7];
                    Vector2 DigPos = vector * 16;
                    bool UseDust = false;
                    for (int a = 0; a < Tile.Length; a++)
                    {
                        TilePos[0] = Utils.RotatedBy(new Vector2(-16f, -8f), Projectile.rotation, default);
                        TilePos[1] = Utils.RotatedBy(new Vector2(0f, -8f), Projectile.rotation, default);
                        TilePos[2] = Utils.RotatedBy(new Vector2(16f, -8f), Projectile.rotation, default);
                        TilePos[3] = Utils.RotatedBy(new Vector2(-16f, 8f), Projectile.rotation, default);
                        TilePos[4] = Utils.RotatedBy(new Vector2(0f, 8f), Projectile.rotation, default);
                        TilePos[5] = Utils.RotatedBy(new Vector2(16f, 8f), Projectile.rotation, default);

                        Tile[a] = (Projectile.Center + TilePos[a] + DigPos) / 16;
                        if ((!Main.tileHammer[Main.tile[(int)Tile[a].X, (int)Tile[a].Y].TileType]) && (Main.tile[(int)Tile[a].X, (int)Tile[a].Y].HasTile))
                        {
                            UseDust = true;
                        }
                        if (!Main.tileHammer[Main.tile[(int)Tile[a].X, (int)Tile[a].Y].TileType])
                        {
                            if (Main.LocalPlayer.Aplayer().破坏者核心装置)
                            {
                                Main.player[Projectile.owner].PickTile((int)Tile[a].X, (int)Tile[a].Y, 205);
                                Main.player[Projectile.owner].PickTile((int)Tile[a].X, (int)Tile[a].Y, 205);
                                Main.player[Projectile.owner].PickTile((int)Tile[a].X, (int)Tile[a].Y, 205);
                            }
                            else
                            {
                                Main.player[Projectile.owner].PickTile((int)Tile[a].X, (int)Tile[a].Y, player.HeldItem.pick);
                            }
                        }
                    }
                    if (UseDust)
                    {
                        Vector2[] DustSpeed = new Vector2[2];
                        for (int a = 0; a < 3; a++)
                        {
                            Vector2 dustPos = vector * 30;

                            DustSpeed[0] = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 10), Main.rand.NextFloat(2, 10)), Projectile.rotation, default);
                            DustSpeed[1] = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(-10, -2), Main.rand.NextFloat(2, 10)), Projectile.rotation, default);
                            for (int D = 0; D < 2; D++)
                            {
                                Dust dust = Main.dust[NewDust(Projectile.Center + dustPos + Projectile.velocity, 4, 4, 6, 0f, 0f, 0, default, 1f)];
                                dust.scale *= 2f + Main.rand.Next(10) * 0.1f;
                                dust.noGravity = true;
                                dust.velocity = DustSpeed[D];
                            }
                        }
                    }
                }
                vector *= A;
                Projectile.velocity = (Projectile.velocity * 20 + vector) / 21;
            }
            else
            {
                vector = player.Center - Projectile.Center;
                float A = 20f;
                vector = vector.PerfectNormalize() * A;
                Projectile.velocity = (Projectile.velocity * 20 + vector) / 21;
                Projectile.tileCollide = false;

                Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                Rectangle value2 = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                if (rectangle.Intersects(value2))
                {
                    Projectile.Kill();
                }
            }
            foreach (Item item in Main.item)
            {
                Vector2 ItemVector = Projectile.Center - item.Center;
                Vector2 ItemPlayer = player.Center - Projectile.Center;
                if (ItemVector.Length() < 150)
                {
                    //item.velocity = ItemVector;
                    item.position += ItemVector.PerfectNormalize() * 5;
                }
                if (ItemVector.Length() < 10 && ItemPlayer.Length() > 10)
                {
                    for (int D = 0; D < ItemPlayer.Length(); D += 5)
                    {
                        Dust dust = Main.dust[NewDust(item.position + ItemPlayer.PerfectNormalize() * D, 1, 1, 6, 0f, 0f, 0, default, 1f)];
                        dust.scale = 0.8f;
                        dust.noGravity = true;
                        dust.velocity = Vector2.Zero;
                    }
                    item.Center = player.Center;
                }
            }
            Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0.4f;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            DDHelper.BackAndForth(0, Projectile.velocity.Length() / 40, Projectile.velocity.Length()/1200, ref Projectile.localAI[0], ref Projectile.DProj().Bool[0]);

            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition + ((Projectile.rotation - MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * -10);

            for (int a = 0; a < 5; a++)
            {
                Main.spriteBatch.Draw(VoidStar, vector, new Rectangle?(new Rectangle(0, 0, (int)(VoidStar.Width / 2F), VoidStar.Height)), new Color(253, 62, 3, 0) * 1f, Projectile.rotation - MathHelper.PiOver2, VoidStar.Size() / 2, new Vector2(Projectile.scale * (0.25F + Projectile.localAI[0]), 0.2F), 0, 0f);
            }
            double quotient = 1 - (Projectile.ai[1] / 1800);
            quotient = Utils.Clamp(quotient, 0f, 1f);
            int C = (int)(Fill.Width() * quotient);

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(10, 255, 10, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(10, 255, 10, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(10, 255, 10, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            int I = (int)(255 * (1 - quotient));
            int N = (int)(255 * quotient);

            Main.spriteBatch.Draw(Fill.Value, Projectile.Center - Main.screenPosition - new Vector2(Fill.Width() / 2, 80), new Rectangle?(new Rectangle(0, 0, C, Fill.Height())), new Color(I, N, 0), 0, Vector2.Zero, new Vector2(1F,1),0,0);

            Main.spriteBatch.Draw(Bar.Value, Projectile.Center - Main.screenPosition - new Vector2(Bar.Width() / 2, 80), Color.White);
            return false;
        }
    }
}