using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged.Bow
{
    public class MagicStarBow : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 2;
            Projectile.coldDamage = true;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.HoldProj(player, 18, 0, Vector2.Zero, 0, 0, !Projectile.DProj().Bool[1]);
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            player.heldProj = Projectile.whoAmI;
            if (Projectile.DProj().Bool[0])
            {
                if (Projectile.DProj().Times[3] == 0)
                {
                    Projectile.DProj().Times[3] = 1;
                }
                if(Projectile.DProj().Times[3]>0)
                {
                    Projectile.DProj().Times[3] -= 0.05F;
                }
                if (!Projectile.DProj().Bool[3])
                {
                    Projectile.DProj().Times[0] -= 8;
                    if (Projectile.DProj().Times[0]<-12)
                    {
                        Projectile.DProj().Bool[3] = true;
                    }
                }
                if (Projectile.DProj().Bool[3])
                {
                    Projectile.DProj().Times[0] += 4f;
                    if (Projectile.DProj().Times[0]>0)
                    {
                        Projectile.DProj().Times[0] = 0;
                    }
                }
                if (Projectile.DProj().Times[0] == 0&& Projectile.DProj().Times[3]<=0)
                {
                    Projectile.DProj().Bool[1] = true;
                }
            }
            else
            {
                if (Projectile.DProj().Times[0] < 2)
                {
                    player.itemTime = (int)(player.ActiveItem().useAnimation * 0.2f);
                    player.itemAnimation = (int)(player.ActiveItem().useAnimation * 0.2f);
                }
                else if (Projectile.DProj().Times[0] < 4)
                {

                    player.itemTime = (int)(player.ActiveItem().useAnimation * 0.3f);
                    player.itemAnimation = (int)(player.ActiveItem().useAnimation * 03f);
                }
                else if (Projectile.DProj().Times[0] < 6)
                {

                    player.itemTime = (int)(player.ActiveItem().useAnimation * 0.4f);
                    player.itemAnimation = (int)(player.ActiveItem().useAnimation * 0.4f);
                }
                else if (Projectile.DProj().Times[0] < 8)
                {

                    player.itemTime = (int)(player.ActiveItem().useAnimation * 0.5f);
                    player.itemAnimation = (int)(player.ActiveItem().useAnimation * 0.5f);
                }
                else if (Projectile.DProj().Times[0] < 10)
                {

                    player.itemTime = (int)(player.ActiveItem().useAnimation * 0.6f);
                    player.itemAnimation = (int)(player.ActiveItem().useAnimation * 0.6f);
                }
                else
                {

                    player.itemTime = (int)(player.ActiveItem().useAnimation * 0.9f);
                    player.itemAnimation = (int)(player.ActiveItem().useAnimation * 0.9f);
                }
                if (Projectile.DProj().Times[0] < 12)
                {
                    if (!channeling)
                    {
                        Projectile.DProj().Times[0] += 1.5f;
                    }
                    Projectile.DProj().Times[0] += 0.2f;
                }
            }
            if (!channeling && Projectile.DProj().Times[0] > 10 && !Projectile.DProj().Bool[0])
            {
                int Proj = NewProjectileChange(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity.PerfectNormalize() * 18, (int)Projectile.DProj().Times[1], Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 1);
                Main.projectile[(int)Projectile.DProj().Times[2]].DProj().Times[1] = Proj;
                Main.projectile[(int)Projectile.DProj().Times[2]].netUpdate = true;
                Projectile.DProj().Bool[0] = true;
                PlaySound(SoundID.Item5, Projectile.position);
            }

            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        Color color = new Color(42, 45, 153, 255);
        public Color TrailColor(float completionRatio)
        {
            return color;
        }
        public float TrailWidth(float completionRatio)
        {
            Item item = Projectile.Player().ActiveItem();
            return 1;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["弓弦"]);
            }
            Vector2[] vectors = new Vector2[] {
                Projectile.Center-Projectile.velocity.RotatedBy(MathHelper.PiOver2)*16,
                Projectile.Center - Projectile.velocity* Projectile.DProj().Times[0]*2,
                Projectile.Center + Projectile.velocity.RotatedBy(MathHelper.PiOver2) * 16};
            TrailDrawer.Draw(vectors, -Main.screenPosition-Projectile.velocity.PerfectNormalize()*8+ Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2)*0, 88, null);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);
            int A = (int)Projectile.DProj().Times[1];
            Main.instance.LoadProjectile(A);
            texture = TextureAssets.Projectile[A].Value;

            if (!Projectile.DProj().Bool[0])
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * (-10 + Projectile.DProj().Times[0]), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);
            return false;
        }
        internal Trailing TrailDrawer;
    }
}