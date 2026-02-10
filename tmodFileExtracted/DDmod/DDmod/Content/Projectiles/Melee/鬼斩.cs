using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class 鬼斩 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 150;
            Projectile.alpha = 0;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.scale = 2.4f;
                Projectile.scale *= player.GetAdjustedItemScale(player.ActiveItem());
                Projectile.DProj().Bool[0] = true;
                Projectile.velocity *= player.GetAdjustedItemScale(player.ActiveItem());
            }
            if (Projectile.timeLeft <= 25)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.timeLeft = 2;
                    //Projectile.extraUpdates = 0;
                    Projectile.alpha -= 4;
                    Projectile.scale *= 1.01f;
                    Projectile.velocity *= 0.98F;
                }
            }
            else
            {
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += 50;
                }
                else
                {
                    Projectile.alpha = 255;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = Projectile.scale;
            }
            if (Projectile.localAI[1] == 0)
            {
                Projectile.localAI[1] = Projectile.damage;
            }

            Projectile.damage = (int)(Projectile.localAI[1]*(Projectile.alpha/255F));
            Projectile.ProjScaleChange();
            for (int A = 0; A <= Length; A++)
            {
                Center[A] = Projectile.Center - (new Vector2(20 * Math.Abs(A - Length / 2), (A - Length / 2) * 20).RotatedBy(Projectile.rotation));
            }
            return false;
        }
        
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
                return null;
        }
        int Length = 4;
        Vector2[] Center = new Vector2[5];
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle[] vectors = new Rectangle[Length + 1];
            bool B = false;
            for (int A = 0; A <= Length; A++)
            {
                Vector2 Size = Projectile.Size / 2;
                vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                if (vectors[A].Intersects(targetHitbox))
                {
                    B = true;
                }
            }
            return new bool?(B);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(220, 0, 22, 0);
            float A = Projectile.alpha / 255F;
            color *= A;

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);

            }
            for (float i = 0.1f; i < Projectile.scale; i+=0.1f)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(1.4F, 1)* i, 0, 0);
            }
            color.A = 0;

            /* Rectangle[] vectors = new Rectangle[Length+1];

             for (int r = 0; r <= Length; r++)
             {
                 Vector2 Size = Projectile.Size / 2;
                 vectors[r] = new Rectangle((int)(Center[r].X - Size.X), (int)(Center[r].Y - Size.Y), Projectile.width, Projectile.height);
                 Main.spriteBatch.Draw(DDTextures.WhitePng.Value, new Vector2((int)(Center[r].X - Size.X), (int)(Center[r].Y - Size.Y)) - Main.screenPosition, null, Color.White, 0, Vector2.Zero, new Vector2(Projectile.width, Projectile.height)/2, 0, 0);
             }*/
            return false;
        }
    }
}