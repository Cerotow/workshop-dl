using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class 阴影波 : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Melee/阴影波_Glow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 80;
            Projectile.extraUpdates =1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 150;
            Projectile.alpha = 0;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
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
            if (Projectile.timeLeft <= 10)
            {
                Projectile.extraUpdates = 1;
                if (Projectile.alpha > 0)
                {
                    Projectile.timeLeft = 6;
                    //Projectile.extraUpdates = 0;
                    Projectile.alpha -= 3;
                    Projectile.velocity *= 0.96F;
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
            //Projectile.ProjScaleChange();
            for (int A = 0; A <= Length; A++)
            {
                Center[A] = Projectile.Center - (new Vector2(Projectile.height/2 * Math.Abs(A - Length / 2)- Projectile.height/2, (A - Length / 2) * Projectile.height).RotatedBy(Projectile.rotation));
            }
            if (Projectile.alpha >= 155)
            {
                Rectangle[] vectors = new Rectangle[Length + 1];
                for (int A = 0; A <= Length; A++)
                {
                    if (Main.rand.NextBool(4))
                    {
                        Vector2 Size = Projectile.Size / 2;
                        vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                        int D = NewDust(vectors[A].TopLeft(), Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(1, 1, 1, 255), Main.rand.NextFloat(0.7F, 1.2F));
                        Main.dust[D].velocity = -Projectile.velocity * Main.rand.NextFloat(1.5F, 1.9F);
                        Main.dust[D].rotation = Projectile.velocity.ToRotation();
                        D = NewDust(vectors[A].TopLeft(), Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(255, 1, 1, 255), Main.rand.NextFloat(0.7F, 1.2F));
                        Main.dust[D].velocity = -Projectile.velocity * Main.rand.NextFloat(1.5F, 1.9F);
                        Main.dust[D].rotation = Projectile.velocity.ToRotation();
                    }
                }
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            Projectile.tileCollide = false;
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().Ecolor = new Color(255, 1, 1, 255);
            Main.projectile[A].localAI[0] = 0.3F;
            Main.projectile[A].localAI[1] = 0.5F;
            Main.projectile[A].localAI[2] = 0F;
            Main.projectile[A].scale = 0.3F;
            A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().Ecolor = new Color(1, 1, 1, 255);
            Main.projectile[A].localAI[0] = 0.15F;
            Main.projectile[A].localAI[1] = 0.25F;
            Main.projectile[A].localAI[2] = 0F;
            Main.projectile[A].scale = 0.15F;

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
        Vector2[] V => new Vector2[Projectile.oldPos.Length];
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(0, 0, 0, 255);
            Color color2 = new Color(255, 0, 0, 255);
            float A = Projectile.alpha / 255F;
            color *= A;
            color2 *= A;

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Texture2D G = Glow.Value;
            
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length/ 2);
                Main.spriteBatch.Draw(G, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, oldcolor, Projectile.rotation, G.Size() / 2, Projectile.scale/4, 0, 0);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0);

            }
            Main.spriteBatch.Draw(G, Projectile.position + Projectile.Size / 2 - Main.screenPosition, null, color2, Projectile.rotation, G.Size() / 2, Projectile.scale/4, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.position + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0);
            /*
            Rectangle[] vectors = new Rectangle[Length + 1];
            for (int E = 0; E <= Length; E++)
            {
                Vector2 Size = Projectile.Size / 2;
                vectors[E] = new Rectangle((int)(Center[E].X - Size.X), (int)(Center[E].Y - Size.Y), Projectile.width, Projectile.height);
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, vectors[E].TopLeft()- Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, Projectile.Size/2, 0, 0);
            }*/
            return false;
        }
    }
}