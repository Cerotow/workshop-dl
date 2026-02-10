
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;

namespace DDmod.Content.Projectiles.Melee.Boomerang.Chakram
{
    public class 绿岩回旋刃Proj : ModProjectile
    {
        public override void Load()
        {
            asset = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Melee/Boomerang/Chakram/绿岩回旋");
        }
        static Asset<Texture2D> asset;
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;

            Projectile.hide = true;
        }
        int T = 0;
        int T2 = 0;
        public override bool PreAI()
        {
            Projectile.HoldChakram(14, new Vector2(76)*1F);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (target.knockBackResist > 0&& !!!!!!!Projectile.DProj().Bool[0])
            {
                Vector2 vector = target.Center-Projectile.Center;
                target.velocity = (vector.PerfectNormalize()*10+Projectile.Player().velocity)*target.knockBackResist;
                DDmod.SyncData(DDType.NPCCenter, target.whoAmI, -1, Projectile.owner);
            }
            {
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
                Main.projectile[A].DProj().color = new Color(100, 255, 100, 0) * 0.3F;
                Main.projectile[A].localAI[0] = Projectile.DProj().Times[2];
                Main.projectile[A].localAI[1] = Projectile.DProj().Times[2];
                Main.projectile[A].scale = Projectile.DProj().Times[2]/4;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.localAI[0] = (Projectile.velocity).ToRotation();
            PlaySound(SoundID.Dig, Projectile.Center);
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        int A;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            texture.DrawCentre(Projectile, null, lightColor, Projectile.scale);
            texture = asset.Value;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                float r = Projectile.DProj().Times[2];
                Main.spriteBatch.Draw(texture, vector, new Rectangle(0, texture.Height/3* (i % 3), texture.Width, texture.Height / 3), new Color(105, 105, 105, 0)* r* (1-((float)i/Projectile.oldPos.Length)), Projectile.oldRot[i] + MathHelper.PiOver2, new Vector2(texture.Width, texture.Height / 3) / 2, Projectile.scale * r*1.5F * r * (1 - ((float)i / Projectile.oldPos.Length)), 0, 0f);
                //Main.spriteBatch.Draw(texture, vector, new Rectangle(0, texture.Height/3*(i%3), texture.Width, texture.Height / 3), new Color(105, 105, 105, 0)* r * (1 - ((float)i / Projectile.oldPos.Length)), Projectile.oldRot[i], new Vector2(texture.Width, texture.Height / 3) / 2, Projectile.scale * 0.75F * r * 1.5F, 0, 0f);
            }
            return false;
        }
    }
}