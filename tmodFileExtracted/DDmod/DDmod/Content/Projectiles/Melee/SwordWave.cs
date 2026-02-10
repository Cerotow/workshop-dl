using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class SwordWave : ModProjectile
    {
        public override string Texture => "DDmod/Content/Projectiles/Melee/SwordWave4";
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 150;
            Projectile.alpha = 0;
            Projectile.scale = 0.5F;
            Projectile.DProj().color = new Color(100,100,100,0);
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if(Projectile.ai[1]==0)
            {
                Projectile.ai[1] = Projectile.scale;
            }
            if(Projectile.extraUpdates != Projectile.DProj().Times[0])
            {
                if(Main.myPlayer == Projectile.owner)
                Projectile.DProj().Times[0] = Projectile.extraUpdates;
                else
                Projectile.extraUpdates = (int)Projectile.DProj().Times[0];
                Projectile.netUpdate = true;
            }
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.ai[1] *= player.GetAdjustedItemScale(player.ActiveItem());
                Projectile.DProj().Bool[0] = true;
                Projectile.velocity *= player.GetAdjustedItemScale(player.ActiveItem());
            }
            if (Projectile.localAI[2] < 300)
            {
                Projectile.localAI[2]+=1f;
                Projectile.ai[1] += Projectile.ai[2] / 300 * player.GetAdjustedItemScale(player.ActiveItem());
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

            Projectile.scale = Projectile.ai[1];

            Projectile.ProjScaleChange();
            for (int A = 0; A <= Length; A++)
            {
                Center[A] = Projectile.Center+Projectile.velocity.PerfectNormalize()*30 - (new Vector2(8 * Math.Abs(A - Length / 2), (A - Length / 2) * Projectile.height).RotatedBy(Projectile.rotation));
            }
            if (Projectile.ai[0] >= 3)
            {
                //Projectile.velocity = Projectile.velocity.PerfectNormalize() * 0.1F;
                if (Projectile.timeLeft > 25)
                {
                    Projectile.timeLeft = 25;
                }
            }
                return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            Projectile.velocity = oldVelocity;
            if(Projectile.timeLeft>25)
            {
                Projectile.timeLeft = 25;
            }
                return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0]++;
            if (Projectile.ai[0]>=3)
            {
                Projectile.damage = 0;
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = Projectile.DProj().color;


            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            if (Projectile.ai[0] < 3)
            {

                return null;
            }
            return false;
        }
        int Length = 8;
        Vector2[] Center = new Vector2[9];
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
            Color color = Projectile.DProj().color;
            float A = Projectile.alpha / 255F;
            color *= A;
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            //color.A = 0;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            //color.A = 0;
            int R = (int)Projectile.velocity.Length() *2*(Projectile.extraUpdates+1);
            for (int i = 1; i < R; i++)
            {
                Vector2 Sc = Projectile.scale * new Vector2(1.4F, 1) * ((R - i) / (float)R-0.1f);
                Color oldcolor = color * ((R - i) / (float)R/2);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * i * 3, null, oldcolor, Projectile.rotation, texture.Size() / 2, Sc, 0, 0);

            }
            /*
            for (int R = 0; R <= Length; R++)
            {
                Vector2 Size = Projectile.Size / 2;
                Rectangle[] vectors = new Rectangle[Length + 1];
                vectors[R] = new Rectangle((int)(Center[R].X - Size.X), (int)(Center[R].Y - Size.Y), Projectile.width, Projectile.height);
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, vectors[R].TopLeft() - Main.screenPosition, null,Color.White, 0,Vector2.Zero, Size, 0, 0);

            }*/

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