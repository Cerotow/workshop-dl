using DDmod.Content.Dusts;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Summon.Whip
{
    public class HeartWhip : ModProjectile
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
                int num6 = NewDust(r4.TopLeft(), r4.Width, r4.Height, ModContent.DustType<爱心粒子>(), 0f, 0f, 100, default, 1.5f);
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
            List<Vector2> list = new List<Vector2>();
            FillWhipControlPoints(Projectile, list);
            Texture2D value = TextureAssets.FishingLine.Value;
            Rectangle value2 = value.Frame();
            Vector2 origin = new Vector2((float)(value2.Width / 2f), 1f);
            Color originalColor = new Color(194, 74, 91);

            Vector2 value3 = list[0];
            for (int i = 0; i < list.Count - 2; i++)
            {
                Vector2 vector = list[i];
                Vector2 vector2 = list[i + 1] - vector;
                float rotation = vector2.ToRotation() - (float)Math.PI / 2f;
                Color color = Lighting.GetColor(vector.ToTileCoordinates(), originalColor);
                Vector2 scale = new Vector2(1f, (vector2.Length() + 2f) / value2.Height);
                Main.spriteBatch.Draw(value, value3 - Main.screenPosition, (Rectangle?)value2, color, rotation, origin, scale, 0, 0f);
                value3 += vector2;
            }
            DrawWhip_HeartWhip(Projectile, list);
            return false;
        }
        public Vector2 DrawWhip_HeartWhip(Projectile projectile, List<Vector2> list)
        {
            Texture2D value = TextureAssets.Projectile[projectile.type].Value;
            Rectangle rectangle = value.Frame(1, 3);
            int height = rectangle.Height;
            rectangle.Height -= 2;
            Vector2 vector = rectangle.Size() / 2f;
            Vector2 vector2 = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                bool flag = true;
                Vector2 origin = vector;
                float scale = 1f;
                switch (i)
                {
                    case 0:
                        origin.Y -= 4f;
                        break;
                    case 19:
                        rectangle.Y = height * 2;
                        scale = 1.1f;
                        break;
                    default:
                        rectangle.Y = height;
                        scale = 0.8f;
                        break;
                }
                Vector2 vector3 = list[i];
                Vector2 vector4 = list[i + 1] - vector3;
                if (flag)
                {
                    float rotation = vector4.ToRotation() - (float)Math.PI / 2f;
                    Color color = Lighting.GetColor(vector3.ToTileCoordinates());
                    Main.spriteBatch.Draw(value, vector2 - Main.screenPosition, (Rectangle?)rectangle, color, rotation, origin, scale, 0, 0f);
                }
                vector2 += vector4;
            }
            return vector2; 
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Data(target);
        }
        bool R;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].AddBuff(ModContent.BuffType<BlessingOfTheHeart>(), 300);
            Projectile.damage /= 2;
            if (!R)
            {
                Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
                target.buffImmune[ModContent.BuffType<BlessingOfTheHeart>()] = false;
                target.AddBuff(ModContent.BuffType<BlessingOfTheHeart>(), 300);
                R = true;
            }
        }
        public static void Data(NPC Projectile)
        {
            float scale = 1;
            float velocity = 0.5f;
            Vector2 projDirection = Utils.RotatedBy(new Vector2(0, -2 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection - Vector2.Zero) * velocity;

            Vector2 projDirection2 = Utils.RotatedBy(new Vector2(1 * scale, -3 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection2, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection2 - Vector2.Zero) * velocity;

            Vector2 projDirection3 = Utils.RotatedBy(new Vector2(-1 * scale, -3 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection3, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection3 - Vector2.Zero) * velocity;

            Vector2 projDirection4 = Utils.RotatedBy(new Vector2(+2 * scale, -4 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection4, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection4 - Vector2.Zero) * velocity;

            Vector2 projDirection5 = Utils.RotatedBy(new Vector2(-2 * scale, -4 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection5, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection5 - Vector2.Zero) * velocity;

            Vector2 projDirection6 = Utils.RotatedBy(new Vector2(+3 * scale, -4 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection6, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection6 - Vector2.Zero) * velocity;

            Vector2 projDirection7 = Utils.RotatedBy(new Vector2(-3 * scale, -4 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection7, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection7 - Vector2.Zero) * velocity;

            Vector2 projDirection8 = Utils.RotatedBy(new Vector2(+4 * scale, -3 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection8, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection8 - Vector2.Zero) * velocity;

            Vector2 projDirection9 = Utils.RotatedBy(new Vector2(-4 * scale, -3 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection9, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection9 - Vector2.Zero) * velocity;

            Vector2 projDirection10 = Utils.RotatedBy(new Vector2(+4 * scale, -2 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection10, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection10 - Vector2.Zero) * velocity;

            Vector2 projDirection11 = Utils.RotatedBy(new Vector2(-4 * scale, -2 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection11, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection11 - Vector2.Zero) * velocity;

            Vector2 projDirection12 = Utils.RotatedBy(new Vector2(+4 * scale, -1 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection12, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection12 - Vector2.Zero) * velocity;

            Vector2 projDirection13 = Utils.RotatedBy(new Vector2(-4 * scale, -1 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection13, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection13 - Vector2.Zero) * velocity;

            Vector2 projDirection14 = Utils.RotatedBy(new Vector2(+3 * scale, 0), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection14, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection14 - Vector2.Zero) * velocity;

            Vector2 projDirection15 = Utils.RotatedBy(new Vector2(-3 * scale, 0), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection15, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection15 - Vector2.Zero) * velocity;

            Vector2 projDirection16 = Utils.RotatedBy(new Vector2(+2 * scale, +1 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection16, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection16 - Vector2.Zero) * velocity;

            Vector2 projDirection17 = Utils.RotatedBy(new Vector2(-2 * scale, +1 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection17, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection17 - Vector2.Zero) * velocity;

            Vector2 projDirection18 = Utils.RotatedBy(new Vector2(+1 * scale, +2 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection18, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection18 - Vector2.Zero) * velocity;

            Vector2 projDirection19 = Utils.RotatedBy(new Vector2(-1 * scale, +2 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection19, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection19 - Vector2.Zero) * velocity;

            Vector2 projDirection20 = Utils.RotatedBy(new Vector2(0, +3 * scale), 0, default);
            Main.dust[NewDust(Projectile.Center + projDirection20, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = (projDirection20 - Vector2.Zero) * velocity;
            TryGetActiveSound(PlaySound(SoundID.Item4, Projectile.position), out var Sound);
            Sound.Sound.Pitch = 1.5f;
        }
    }
}