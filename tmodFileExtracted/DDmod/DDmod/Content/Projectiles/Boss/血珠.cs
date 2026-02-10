using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Particles;
using DDmod.Content.Tiles.农场;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class 血珠 : ModProjectile
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
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            Projectile.damage = 0;
            if (Projectile.velocity.Y < -0.5F)
            {
                Projectile.velocity.Y *= 0.96F;
            }
            else
            {
                Projectile.velocity.Y = -0.1f;
            }
            Projectile.ai[0]++;
            Main.LocalPlayer.Dplayer().Bossperspective(Projectile.Center, 80, false, 0.1F);
            Lighting.AddLight(Projectile.Center, new Color(255, 10, 10).ToVector3() * Projectile.ai[1]);
            if (Projectile.ai[0] < 300)
            {
                Projectile.ai[1] += 0.2F;
                NewDustChange2(2, Projectile.Center - new Vector2(4), new Vector2(1), ModContent.DustType<光球粒子>(), 1, 3, false, 1, 2, 100, new Color(166, 12, 12, 200));
                NewDustChange2(2, Projectile.Center - new Vector2(4), new Vector2(1), ModContent.DustType<冰雾>(), 1, 4, false, 0.7F, 1.1F, -600, new Color(166, 12, 12, 200));
                NewDustChange2(1, Projectile.Center - new Vector2(4), new Vector2(1), ModContent.DustType<速度粒子>(), 1, 8, false, 2, 4, 100, new Color(166, 12, 12, 200));

            }
            else
            {
                Projectile.ai[1] -= 0.5F;
                Projectile.Kill();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(Projectile.velocity.X!=oldVelocity.X)
            {
                Projectile.velocity.X = 0; 
            }
            if(Projectile.velocity.Y!=oldVelocity.Y)
            {
                Projectile.velocity.Y = 0; 
            }
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<恐惧缝合体>()))
            {
                NewDustChange(140, Projectile.Center - new Vector2(40, 104), new Vector2(50, 200), ModContent.DustType<冰雾>(), 0, 4, false, 1, -1000, new Color(166, 12, 12, 200));

                if (Main.netMode != 1)
                {
                    DNPC.NewNPCs(Projectile.GetSource_FromAI(), Projectile.Center, ModContent.NPCType<恐惧缝合体>(), 0);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            DDTileDawnSystem.Filter(new Color(220, 0, 25, 255), 0.4f, 0.05F);

            if (!NPC.AnyNPCs(ModContent.NPCType<恐惧缝合体>()))
                Main.spriteBatch.Draw(texture, v, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            return false;
        }
    }
}