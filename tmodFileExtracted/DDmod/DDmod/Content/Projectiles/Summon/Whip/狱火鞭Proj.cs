using DDmod.Content.Dusts;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Summon.Whip
{
    public class 狱火鞭Proj : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.IsAWhip[Projectile.type] = true;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10;
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
            if (Main.rand.NextBool(1))
            {
                Projectile.WhipPointsForCollision.Clear();
                FillWhipControlPoints(Projectile, Projectile.WhipPointsForCollision);
                Rectangle r4 = Utils.CenteredRectangle(Projectile.WhipPointsForCollision[Projectile.WhipPointsForCollision.Count - 1], new Vector2(30f, 30f));
                int num6 = NewDust(r4.TopLeft(), r4.Width, r4.Height, 6, 0f, 0f, 100, default, 2.5f);
                Main.dust[num6].noGravity = true;
                Main.dust[num6].velocity.X /= 2f;
                Main.dust[num6].velocity.Y /= 2f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            FillWhipControlPoints(Projectile, list);
            Texture2D value = TextureAssets.FishingLine.Value;
            Rectangle value2 = value.Frame();
            Vector2 origin = new Vector2((float)(value2.Width / 2f), 1f);
            Color originalColor = new Color(254, 62, 3,0);
            Vector2 value3 = list[0];
            for (int i = 0; i < list.Count - 2; i++)
            {
                Vector2 vector = list[i];
                Vector2 vector2 = list[i + 1] - vector;
                float rotation = vector2.ToRotation() - (float)Math.PI / 2f;
                Color color =  originalColor;
                Vector2 scale = new Vector2(1f, (vector2.Length() + 2f) / value2.Height);
                Main.spriteBatch.Draw(value, value3 - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 6, (Rectangle?)value2, color, rotation, origin, scale, 0, 0f);
                value3 += vector2;
            }
            DrawWhip_HeartWhip(Projectile, list);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            Projectile.position = Projectile.Player().position;
            return false;
        }
        public Vector2 DrawWhip_HeartWhip(Projectile projectile, List<Vector2> list)
        {
            Texture2D value = TextureAssets.Projectile[projectile.type].Value;
            Rectangle rectangle = value.Frame(1, 5);
            int height = rectangle.Height;
            Vector2 vector = rectangle.Size() / 2f;
            Vector2 vector2 = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                bool flag = true;
                Vector2 origin = vector;
                float scale = 1f;
                if (i == 0)
                {
                    rectangle.Y = 0;
                }
                else if (i<=5)
                {
                    rectangle.Y = height;
                }
                else if (i <= 10)
                {
                    rectangle.Y = height * 2;
                }
                else if (i < list.Count - 2)
                {
                    rectangle.Y = height * 3;
                }
                else
                {
                    rectangle.Y = height * 4;
                }
                Vector2 vector3 = list[i];
                Vector2 vector4 = list[i + 1] - vector3;
                if (flag)
                {
                    float rotation = vector4.ToRotation() - (float)Math.PI / 2f;
                    Color color = Color.White;
                    Main.spriteBatch.Draw(value, vector2 - Main.screenPosition+Projectile.velocity.PerfectNormalize()*6, (Rectangle?)rectangle, color, rotation, origin, scale, 0, 0f);
                }
                vector2 += vector4;
            }
            return vector2; 
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.9F);
            target.AddBuff(ModContent.BuffType<地狱之火>(), 300);
            if (!Projectile.DProj().Bool[0])
            {
                Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
                target.AddBuff(ModContent.BuffType<狱火爆炸>(), 300);
                Projectile.DProj().Bool[0] = true;
            }
        }
    }
}