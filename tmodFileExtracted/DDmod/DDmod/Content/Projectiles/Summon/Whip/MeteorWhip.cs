namespace DDmod.Content.Projectiles.Summon.Whip
{
    public class MeteorWhip : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.IsAWhip[Projectile.type] = true;
            Projectile.DefaultToWhip();
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            Projectile.ai[0] += 1f;
            GetWhipSettings(Projectile, out var timeToFlyOut, out var _, out var _);
            Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * (Projectile.ai[0] - 1f);
            Projectile.spriteDirection = (!(Vector2.Dot(Projectile.velocity, Vector2.UnitX) < 0f)) ? 1 : (-1);
            if (Projectile.ai[0] >= timeToFlyOut || player.itemAnimation == 0)
            {
                Projectile.Kill();
                return;
            }
            player.heldProj = Projectile.whoAmI;
            player.itemAnimation = player.itemAnimationMax - (int)(Projectile.ai[0] / Projectile.MaxUpdates);
            player.itemTime = 5;
            if (Projectile.ai[0] == (int)(timeToFlyOut / 2f))
            {
                Projectile.WhipPointsForCollision.Clear();
                FillWhipControlPoints(Projectile, Projectile.WhipPointsForCollision);
                Vector2 position = Projectile.WhipPointsForCollision[Projectile.WhipPointsForCollision.Count - 1];
                PlaySound(SoundID.Item153, position);
            }
            float t3 = Projectile.ai[0] / timeToFlyOut;
            float num5 = Utils.GetLerpValue(0.1f, 0.7f, t3, clamped: true) * Utils.GetLerpValue(0.9f, 0.7f, t3, clamped: true);
            if (num5 > 0.1f && Main.rand.NextFloat() < num5 / 2f)
            {
                Projectile.WhipPointsForCollision.Clear();
                FillWhipControlPoints(Projectile, Projectile.WhipPointsForCollision);
                Rectangle r4 = Utils.CenteredRectangle(Projectile.WhipPointsForCollision[Projectile.WhipPointsForCollision.Count - 1], new Vector2(30f, 30f));
                int num6 = NewDust(r4.TopLeft(), r4.Width, r4.Height, 6, 0f, 0f, 100, default, 1.8f);
                Main.dust[num6].noGravity = true;
                Main.dust[num6].velocity.X /= 2f;
                Main.dust[num6].velocity.Y /= 2f;
            }
        }
        public override bool ShouldUpdatePosition()
        {
            Projectile.position = Projectile.Player().position;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            GetWhipSettings(Projectile, out var timeToFlyOut, out var _, out var _);
            if (Projectile.ai[0] >= timeToFlyOut || player.itemAnimation == 0)
            {
                Projectile.Kill();
            }
            List<Vector2> list = new List<Vector2>();
            FillWhipControlPoints(Projectile, list);
            Texture2D value = TextureAssets.FishingLine.Value;
            Rectangle value2 = value.Frame();
            Vector2 origin = new Vector2((float)(value2.Width / 2f), 1f);
            Color originalColor = new Color(252, 160, 28);

            Vector2 value3 = list[0];
            Vector2 scale = default;
            for (int i = 0; i < list.Count - 2; i++)
            {
                Vector2 vector = list[i];
                Vector2 vector2 = list[i + 1] - vector;
                float rotation = vector2.ToRotation() - (float)Math.PI / 2f;
                Color color = Lighting.GetColor(vector.ToTileCoordinates(), originalColor);
                scale = new Vector2(1f, (vector2.Length() + 2f) / value2.Height);
                Main.spriteBatch.Draw(value, value3 - Main.screenPosition, (Rectangle?)value2, color, rotation, origin, scale, 0, 0f);
                value3 += vector2;
            }
            DrawWhip_HeartWhip(Projectile, list);
            return false;
        }
        //绘制鞭子
        public Vector2 DrawWhip_HeartWhip(Projectile projectile, List<Vector2> list)
        {
            Texture2D value = TextureAssets.Projectile[projectile.type].Value;
            Rectangle rectangle = value.Frame(1, 5);
            int height = rectangle.Height;
            rectangle.Height -= 2;
            Vector2 vector = rectangle.Size() / 2f;
            Vector2 vector2 = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                bool flag = true;
                Vector2 origin = vector;
                float scale = 1f;
                //鞭子
                if (i == 0)
                {
                    rectangle.Y = height * 0;
                }
                else if (i == 19)
                {
                    rectangle.Y = height * 4;
                }
                else
                {
                    rectangle.Y = height * (1 + i % 3);
                }
                Vector2 vector3 = list[i];
                Vector2 vector4 = list[i + 1] - vector3;
                if (flag)
                {
                    float rotation = vector4.ToRotation() - (float)Math.PI / 2f;
                    Color color = Lighting.GetColor(vector3.ToTileCoordinates());
                    for (int p = 0; p < 1+i + (i == 19 ? 10 : 0); p++)
                    {
                            Main.spriteBatch.Draw(value, vector2 - Main.screenPosition, (Rectangle?)rectangle, Color.White, rotation, origin, scale, 0, 0f);
                    }
                }
                vector2 += vector4;
            }
            return vector2;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            //Main.player[Projectile.owner].AddBuff(ModContent.BuffType<AcornMarker>(), 300);
            Projectile.damage = (int)(Projectile.damage * 0.8f);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            target.buffImmune[ModContent.BuffType<MeteorMarker>()] = false;
            target.AddBuff(ModContent.BuffType<MeteorMarker>(), 300);
            target.AddBuff(24, 300);
            Data(target);
        }
        public void Data(NPC Projectile)
        {
            for (int A = 0; A < 50; A++)
            {
                Vector2 projDirection = Utils.RotatedBy(new Vector2(0, -Main.rand.NextFloat(2, 3)), Main.rand.NextFloat(0, MathHelper.TwoPi), default);
                int num6 = NewDust(Projectile.Center, 1, 1, 6, 0f, 0f, 100, default, 1.8f);
                Main.dust[num6].velocity = projDirection;
                Main.dust[num6].noGravity = true;

            }
        }
    }
}