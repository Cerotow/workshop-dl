using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class 诡异肉块Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.timeLeft = 1800;
        }
        int A;
        public override void AI()
        {
            Projectile.damage = 0;
            Projectile.rotation += Projectile.velocity.X * 0.03F;
            Projectile.velocity.X *= 0.98f;
            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.03F;
            }
            else
            {
                Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.03F;
            }
            if (!NPC.AnyNPCs(ModContent.NPCType<恐惧缝合体>()))
                Main.LocalPlayer.Dplayer().Bossperspective(Projectile.Center, 120, false, 0.2F);
            if (Projectile.DProj().track > 30 && Projectile.velocity.Y < 10)
            {
                Projectile.velocity.Y += 0.15F;
            }
            if (!NPC.AnyNPCs(ModContent.NPCType<鬼牙头>()))
            {
                if (Main.netMode != 1)
                    DNPC.NewNPCs(Projectile.GetSource_FromAI(), Projectile.Center - Projectile.velocity.PerfectNormalize() * 1000, ModContent.NPCType<鬼牙头>(), 0);
            }
            else if (NPC.AnyNPCs(ModContent.NPCType<恐惧缝合体>()))
            {
                Projectile.Kill();
            }

            for (int a =0;a<200;a++)
            {
                if (Main.npc[a].active&& Main.npc[a].type==ModContent.NPCType<鬼牙头>() && Main.npc[a].getRect().Intersects(Projectile.getRect()))
                {
                    PlaySound(SoundID.NPCDeath1, Projectile.Center);
                    Projectile.Kill();
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(Projectile.velocity.X!=oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.2F; 
            }
            if(Projectile.velocity.Y!=oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.2F; 
            }
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange(220, Projectile.Center - new Vector2(4), new Vector2(1), 5, 1, 30, false, 3,100);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            Main.spriteBatch.Draw(texture, v, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            return false;
        }
    }
}