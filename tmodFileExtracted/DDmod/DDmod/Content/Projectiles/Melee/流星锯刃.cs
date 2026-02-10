using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace DDmod.Content.Projectiles.Melee
{
    public class 流星锯刃 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 260;
            Projectile.extraUpdates =2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 150;
            Projectile.alpha = 255;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Main.projFrames[Projectile.type] = 4;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.scale *= player.GetAdjustedItemScale(player.ActiveItem());
                Projectile.DProj().Bool[0] = true;
                Projectile.velocity *= player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.frame++;
            for (int A = 0; A <= Length; A++)
            {

                if (Projectile.velocity.X > 0)
                    Center[A] = Projectile.Center - (new Vector2(Projectile.height * (A - Length / 2) - Projectile.height / 2, (A - Length / 2) * Projectile.height).RotatedBy(Projectile.rotation));
                else
                    Center[A] = Projectile.Center - (new Vector2(-Projectile.height * (A - Length / 2) - Projectile.height / 2, (A - Length / 2) * Projectile.height).RotatedBy(Projectile.rotation));
                Center[A] += Projectile.velocity.PerfectNormalize() * (46/2 - Projectile.width);
            }
            if (Projectile.velocity.Length() > 2)
            {
                Rectangle[] vectors = new Rectangle[Length + 1];
                for (int A = 0; A <= Length; A++)
                {
                    if (Main.rand.NextBool(4))
                    {
                        int Type = ModContent.DustType<速度粒子>();
                        if (Main.rand.NextBool(10))
                        {
                            Type = ModContent.DustType<方块粒子>();
                        }
                        Vector2 Size = Projectile.Size / 2;
                        vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                        int D = NewDust(vectors[A].TopLeft(), Projectile.width, Projectile.height, Type, 0, 0, 100, new Color(252, 167, 34, 0), Main.rand.NextFloat(1.2F, 1.8F));
                        Main.dust[D].velocity = -Projectile.velocity / 2 * Main.rand.NextFloat(1F, 1.9F);
                        Main.dust[D].customData = 0.5F;
                        if (Type == ModContent.DustType<方块粒子>())
                        {
                            Main.dust[D].color= new Color(253, 62, 3,0);
                            Main.dust[D].scale /= 2;
                            Main.dust[D].customData = 2F;
                        }

                        Main.dust[D].rotation = Projectile.velocity.ToRotation();
                    }
                }
            }
            if(Projectile.ai[2]==1)
            {
                Projectile.velocity *= 0.92F;
                Projectile.alpha-=5;
                if(Projectile.alpha<0)
                {
                    Projectile.Kill();
                }
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            if (Projectile.ai[2]==0)
            Projectile.ai[2] = 1;
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(252, 167, 34, 255);
            Main.projectile[A].localAI[0] = 0.3F;
            Main.projectile[A].localAI[1] = 0.5F;
            Main.projectile[A].localAI[2] = 0F;
            Main.projectile[A].scale = 0.3F;

            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
                return null;
        }
        int Length = 2;
        Vector2[] Center = new Vector2[3];
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
        Vector2[] V => new Vector2[Projectile.oldPos.Length];
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects sprite = SpriteEffects.None;
            if (Projectile.velocity.X > 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Color color = Color.White;
            float A = Projectile.alpha / 255F;
            color *= A;

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle rectangle = new Rectangle(0, texture.Height / 4*((Projectile.frame/10)%4), texture.Width,texture.Height/4);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Color oldcolor = new Color(252, 167, 34, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length/ 2)* A;
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, rectangle, oldcolor, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, sprite, 0);

            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, color, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, sprite, 0);
            /*
            Rectangle[] vectors = new Rectangle[Length + 1];
            for (int E = 0; E <= Length; E++)
            {
                Vector2 Size = Projectile.Size / 2;
                vectors[E] = new Rectangle((int)(Center[E].X - Size.X), (int)(Center[E].Y - Size.Y), Projectile.width, Projectile.height);
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, vectors[E].TopLeft()- Main.screenPosition, null, Color.White, 0, Vector2.Zero, Projectile.Size/2, 0, 0);
            }*/
            return false;
        }
    }
}