using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.蘑菇王;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Summon.Whip
{
    public class 蘑菇鞭Proj : ModProjectile
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
                int num6 = NewDust(r4.TopLeft(), r4.Width, r4.Height, ModContent.DustType<蘑菇粒子>(), 0f, 0f, 100, default, 1.5f);
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
            Color originalColor = new Color(237, 160, 69);

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
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            NewDustChange(30, target.Center - new Vector2(4), new Vector2(1), ModContent.DustType<蘑菇粒子>(), 0, 4,false, 1.2F);
            Projectile.damage /= 2;
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            target.buffImmune[ModContent.BuffType<蘑菇鞭Buff>()] = false;
            target.AddBuff(ModContent.BuffType<蘑菇鞭Buff>(), 300);
        }
    }
}