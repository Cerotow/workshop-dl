using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Boss;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.BattlePets.Proj
{
    public class 骷髅头 : ModProjectile
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
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
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
            Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 26, 0, 0, 0, default, Main.rand.NextFloat(0.4F, 0.7F))];
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange2(40, Projectile.Center, Vector2.Zero, 26, 0, 3, false, 0.6F, 1.2F, 0);
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
                Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, null, new Color(100,0,0,0) * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), Projectile.oldRot[a], texture.Size() / 2, Projectile.scale, sprite, 0);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, sprite, 0f);
            return false;
        }
    }
}