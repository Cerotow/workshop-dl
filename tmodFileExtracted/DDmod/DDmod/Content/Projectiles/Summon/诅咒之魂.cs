
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Summon
{
    public class 诅咒之魂 : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;

        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                NewDustChange4(50, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 9, true, 1F, 1.8F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                Projectile.localAI[0] = 1;
            }
            Projectile.frame = (int)(Projectile.frameCounter++ / 6) % 5;
            NPC npc = Main.npc[(int)Projectile.ai[2]];
            if (!npc.active)
            {
                Projectile.Kill();
                return;
            }
            if(Projectile.timeLeft<5)
            {
                Projectile.timeLeft = 5;
                Projectile.ai[1] -= 0.05F;
                if (Projectile.ai[1]<0)
                {
                    Projectile.Kill();
                }
                return;
            }
            if (Projectile.ai[1] < 1)
            {
                Projectile.ai[1] += 0.02F;
            }
            else
            {

                Projectile.ai[0]++;
                Vector2 vector = npc.Center - Projectile.Center;
                if(vector.Length()<200)
                {
                    Projectile.velocity = -vector.PerfectNormalize() * 3;
                }
                else if (vector.Length() > 400)
                {
                    Projectile.velocity = vector.PerfectNormalize() * vector.Length() / 100;
                }
                else
                {
                    Projectile.velocity *= 0.92F;
                }
                if (Projectile.ai[0] > 120)
                {
                    NewDustChange4(100, npc.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 4);
                    Projectile.ai[0] = 0;
                    if (!npc.dontTakeDamage&&Main.myPlayer==Projectile.owner)
                    {
                        int A = npc.SimpleStrikeNPC((int)(Projectile.damage), 0, false, 1);
                        Projectile.Player().addDPS(A);
                    }
                }
                else
                {
                    NewDustSector(1, npc.Center - new Vector2(8), new Vector2(4), new Vector3(-vector.PerfectNormalize(), MathHelper.Pi), ModContent.DustType<速度粒子>(), 2, 10, true, 1, 3, 100, 1000, new Color(151, 35, 221, 0), null);
                }
                npc.AddBuff(ModContent.BuffType<永恒诅咒>(),5);
                Projectile.ai[1] = 1;
            }
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange4(50, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 9, true, 1F, 1.8F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects sprite = 0;
            Texture2D texture = NPCs.Boss.先祖咒魂.诅咒之魂.Chains.Value;
            NPC npc = Main.npc[(int)Projectile.ai[2]];
            if (npc.active)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (vector.X > 0)
                {
                    sprite = SpriteEffects.FlipHorizontally;
                }
                if ((Projectile.ai[0] / 120) < 0.85F)
                {
                    DDHelper.BackAndForth(1f, 1.5f, (Projectile.ai[0] / 120) / 20, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0], true);
                }
                else
                {
                    DDHelper.BackAndForth(1f, 1.5f, (Projectile.ai[0] / 120) / 4, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0], true);
                }
                if (Projectile.ai[1] >= 1)
                {
                    Main.spriteBatch.Draw(DDTextures.VoidStar.Value, Projectile.Center + vector * (Projectile.ai[0] / 120) - Main.screenPosition, null, new Color(100, 100, 100, 255), 0, DDTextures.VoidStar.Size() / 2, 0.4F * Projectile.DProj().Times[0], 0, 0);
                    Main.spriteBatch.Draw(DDTextures.VoidStar.Value, Projectile.Center + vector * (Projectile.ai[0] / 120) - Main.screenPosition, null, new Color(151, 35, 221, 0), 0, DDTextures.VoidStar.Size() / 2, 0.4F * Projectile.DProj().Times[0], 0, 0);
                }
                for (int a = 0; a < vector.Length() / texture.Width * Projectile.ai[1]; a++)
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center + vector.PerfectNormalize() * texture.Width * a - Main.screenPosition, null, new Color(151, 35, 221, 0) * 0.3F, vector.ToRotation(), texture.Size() / 2, Projectile.scale, 0, 0);
                }
                if (Projectile.ai[1] >= 1)
                {
                    Main.spriteBatch.Draw(DDTextures.VoidStar.Value, npc.Center - Main.screenPosition, null, new Color(100, 100, 100, 255), 0, DDTextures.VoidStar.Size() / 2, 0.4F, 0, 0);
                    Main.spriteBatch.Draw(DDTextures.VoidStar.Value, npc.Center - Main.screenPosition, null, new Color(151, 35, 221, 0), 0, DDTextures.VoidStar.Size() / 2, 0.4F, 0, 0);
                }
                texture = TextureAssets.Projectile[Projectile.type].Value;
                Rectangle rectangle = new Rectangle(0, texture.Height / 5 * Projectile.frame, texture.Width, texture.Height / 5);
                Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(0, 2) - Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, sprite, 0);

            }
            return false;
        }
    }
}