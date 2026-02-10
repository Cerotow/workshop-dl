using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Summon;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.BattlePets.Proj
{
    public class 地狱骷髅头 : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] =8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = 1;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            NPC npc = NPCdirection.FindClosest(Projectile.Center, 1000, true, null);
            if (npc != null && (npc.Center - Projectile.Center).Length() > 50 && npc.active && Projectile.GetGlobalProjectile<DDGlobalProjectile>().track > 30)
            {
                Projectile.Chase(npc, 8, 21);
            }
            Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(253, 62, 3, 0), Main.rand.NextFloat(0.4F, 0.7F))];
            dust.velocity = -Projectile.velocity/10;
            dust.rotation = Projectile.rotation;

        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 6;
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectileChange(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<狱火爆炸Proj>(), Projectile.damage, 0, -1, 0,0,0.5F);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            SpriteEffects sprite = 0;
            if(Projectile.velocity.X<0)
            {
                sprite = SpriteEffects.FlipVertically;
            }
            for (int a = 0; a < Projectile.oldPos.Length; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, null, new Color(253,62, 3, 0) * 0.5f * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), Projectile.oldRot[a], texture.Size() / 2, Projectile.scale, sprite, 0);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White*0.5f, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, sprite, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(253, 62, 3, 0), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, sprite, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(253, 62, 3, 0), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, sprite, 0f);
            return false;
        }
    }

}