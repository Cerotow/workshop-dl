using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Players;
using Terraria;
using Terraria.ModLoader.Config;

namespace DDmod.Content.Projectiles.Melee.Boomerang
{
    public class 花岗岩能量回旋镖Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 0;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
        }
        
        public override void AI()
        {
            Projectile.HoldBoomerang(new Vector2(0, 20), Projectile.Player().ActiveItem().shootSpeed / 2);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Rectangle value = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 vector = Projectile.Size / 2;
            SpriteEffects sprite = 0;
            if (Projectile.DProj().vector[0].X>0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color = new Color(158, 242, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(value), color, Projectile.oldRot[i], texture.Size()/2, Projectile.scale, sprite, 0f);
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(value), color, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale, sprite, 0f);
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(value), color, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale, sprite, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(value), lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, sprite, 0f);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.localAI[0] = (-oldVelocity).ToRotation();
            Projectile.DProj().Times[1] += 120;
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.localAI[0] = (-Projectile.velocity).ToRotation();
            Projectile.DProj().Times[1] += 120;
            NewDustChange2(30, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 2, 5, true, 1, 3, 100, new Color(50, 155, 255, 0));
            List<NPC> n = [target];
            if (Projectile.ai[0] == 0)
            {
                for (int a = 0; a < 5; a++)
                {
                    NPC npc = NPCdirection.FindClosest2(Projectile.Center, 300, false, n);
                    if (npc != null)
                    {
                        n.Add(npc);
                        int r = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<花岗岩能量>(), Projectile.damage, 0, -1, npc.whoAmI);
                        Main.projectile[r].DamageType = DamageClass.Melee;
                    }
                }
                Projectile.ai[0] = 1;
            }
        }
    }
}