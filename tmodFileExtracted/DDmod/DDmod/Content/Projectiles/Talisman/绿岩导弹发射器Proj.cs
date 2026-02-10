using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Summon;
using DDmod.Modkey;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class 绿岩导弹发射器Proj : Talismans
    {
        public override void SetStaticDefaults()
        {
        }
        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            if (player.TPlayer().TalismanTimes % 5 == 0 && Main.myPlayer == Projectile.owner)
            {
                if (Projectile.DProj().Times[0] % 2 == 0)
                {
                    Projectile.DProj().vector[0].Y = 0;
                }
                else
                {
                    Projectile.DProj().vector[0] += new Vector2(2, 0);
                    Projectile.DProj().vector[0].Y = 4;
                }
                Projectile.DProj().Times[0]++;
                NPC npc = NPCdirection.FindClosest(player.Dplayer().MouseWorld, 1000, false);
                if (npc != null)
                {
                    int A = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(0, 12) + Projectile.DProj().vector[0], new Vector2(0, -6).RotatedBy(Projectile.rotation), ModContent.ProjectileType<绿岩导弹>(), Projectile.damage, 3, Projectile.owner, 1, npc.whoAmI);
                    Main.projectile[A].scale = 0.3F;
                }
            }
            MobileAI();
        }
        public override void End()
        {
        }
        public override void ExtraUse()
        {
            Projectile.DProj().Times[0] = 0;
            Projectile.DProj().vector[0] = new Vector2(-6,0);
        }
        public override bool MobileAI()
        {
            Player player = Main.player[Projectile.owner];
            //跟随AI
            Vector2 vector = Projectile.Player().MountedCenter;
            vector.X -= 40 * Projectile.Player().direction;
            vector.Y += Float - 20;
            vector = vector - Projectile.Center;
            DDHelper.BackAndForth(-10, 10, 0.2f, ref Float, ref FloatBool);
            float A = vector.Length();
            DDHelper.MaxandMinF(ref A, 10, 0);
            if (!player.dead)
            {
                if (vector.Length() > 1200)
                {
                    Projectile.position = Projectile.Player().position - vector.PerfectNormalize() * 1000;
                }
                vector.DirectPerfectNormalize();
                Projectile.velocity = (Projectile.velocity * 8 + vector * A) / 9;
            }
            else
            {
                if (vector.Length() > 1200)
                {
                    Projectile.Kill();
                }
                vector.DirectPerfectNormalize();
                Projectile.velocity = (Projectile.velocity * 8 + -vector * A) / 9;
            }

            Projectile.RotationSpeed(Projectile.velocity.X * 0.05F, 0.05F);
            if (Math.Abs(Projectile.velocity.X)<0.2F)
            {
                Projectile.rotation = 0;
            }
            Projectile.spriteDirection = -player.direction;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[2]+=0.1F;
            Player player = Main.player[Projectile.owner];
                SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center- Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            return false;
        }
    }
}