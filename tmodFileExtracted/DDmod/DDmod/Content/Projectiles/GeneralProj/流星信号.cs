
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.流星破坏者;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 流星信号 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft =1000;
            Projectile.extraUpdates = 5;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.DProj().vector[0] = new Vector2(0.5f, 3);
            Projectile.DProj().vector[1] = new Vector2(0.5f, 1);
            Projectile.scale = 0.1F;
        }

        public override void SetStaticDefaults()
        {

        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Color(50, 255, 57).ToVector3() * Projectile.scale * 2);
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 120)
            {
                Projectile.scale += 0.01F;
            }
            else
            {
                Projectile.velocity.Y -= 0.2F;
                Projectile.DProj().vector[0].Y -= 0.1F;
                Projectile.DProj().vector[1].Y += 0.1F;
                if (Projectile.ai[0] >= 240)
                {
                    Projectile.velocity = Vector2.Zero;
                    Main.LocalPlayer.Dplayer().PlayerShake(120, 12);
                }

                if (Projectile.ai[0] >= 600)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override void OnKill(int timeLeft)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<流星破坏者>()))
            {
                if (Main.netMode != 1)
                {
                    DNPC.NewNPCs(Projectile.GetSource_FromAI(), Projectile.Center, ModContent.NPCType<流星破坏者>(), 0);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] < 240)
            {
                Texture2D texture = DDTextures.Starlight3.Value;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(50, 255, 57, 0), MathHelper.PiOver2, texture.Size() / 2, Projectile.scale * Projectile.DProj().vector[0], 0, 0);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(50, 255, 57, 0), 0, texture.Size() / 2, Projectile.scale * Projectile.DProj().vector[1], 0, 0);
            }
            return false;
        }
    }
}
