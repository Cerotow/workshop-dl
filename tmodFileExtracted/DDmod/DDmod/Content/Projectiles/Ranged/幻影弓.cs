using DDmod.Content.Dusts;
using DDmod.Content.Items;
using DDmod.Content.Items.Ammo;
using DDmod.Content.Projectiles.Ranged.Bow;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 幻影弓 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
            Main.projFrames[Projectile.type] = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {

        }
        int r = -1;
        public override void AI()
        {
            Projectile projectile = Main.projectile[(int)Projectile.ai[0]];
            if (projectile == null || !projectile.active || projectile.type <= 0 || projectile.type != ModContent.ProjectileType<GlobalBow>())
            {
                for (int A = 0; A < 1000; A++)
                {
                    if (Main.projectile[A].active && Main.projectile[A].type == ModContent.ProjectileType<GlobalBow>() && Main.projectile[A].owner == Projectile.owner)
                    {
                        projectile = Main.projectile[A];
                        break;
                    }
                }

                if (projectile == null || !projectile.active || projectile.type <= 0 || projectile.type != ModContent.ProjectileType<GlobalBow>())
                    return;
            }
            Projectile.timeLeft = 5;
            RangedProjectile ranged = projectile.GetGlobalProjectile<RangedProjectile>();
            RangedProjectile ranged2 = Projectile.GetGlobalProjectile<RangedProjectile>();
            Player player = Projectile.Player(); 
            if (!Projectile.DProj().Bool[0])
            {
                ranged2.BowTime = ranged.BowTime;
                ranged2.BowDamage = ranged.BowDamage;
                if(ranged.Ammo[0]!=0)
                ranged2.Ammo[0] = ranged.Ammo[0];
            }
            float Charge = ranged2.BowTime / player.itemAnimationMax;
            float la = ranged2.BowTime / Projectile.Player().itemAnimationMax * 6;
            float Magnification = Charge;
            if (r == -1)
            {
                r = player.selectedItem;
            }
            if (player.selectedItem != r&&Main.myPlayer==Projectile.owner)
            {
                Projectile.Kill();
            }
            Vector2 vector = player.Dplayer().MouseWorld-Projectile.Center;
            Projectile.rotation = vector.ToRotation()-MathHelper.Pi;
            Projectile.position = player.MountedCenter + Projectile.Size / 2-new Vector2(20,20)+new Vector2(100,0).RotatedBy(((float)projectile.DProj().track/100F)+(MathHelper.TwoPi/2* Projectile.ai[1]));
            if(!Projectile.DProj().Bool[1])
            {
                Projectile.DProj().Bool[1] = true;
                NewDustChange(80, Projectile.position, Projectile.Size, 229, 1F, 8F);
            }
            if (Projectile.DProj().Bool[0]&&Projectile.owner==Main.myPlayer)
            {
                IEntitySource Source = projectile.GetSource_FromAI();
                //发射
                int Proj1 = NewProjectileChange(Source, Projectile.Center - vector.PerfectNormalize() * (-10 + la + 8), vector.PerfectNormalize() * 20 * Charge, ranged2.Ammo[0], (int)(ranged2.BowDamage[0] * Charge), 0, Projectile.owner, 0, 0, Magnification);
                Main.projectile[Proj1].scale = Projectile.scale;
                Main.projectile[Proj1].Resize((int)(Main.projectile[Proj1].OriginalWidth() * (Main.projectile[Proj1].scale)), (int)(Main.projectile[Proj1].OriginalHeight() * (Main.projectile[Proj1].scale)));
                Vector2 source = Projectile.Center - vector.PerfectNormalize() * (-10 + la + 8);
                Vector2 vector8 = vector.PerfectNormalize()  * 6f;
                int num41 = (int)(ranged2.BowDamage[0] * Charge * 0.15f);
                NPC i = NPCdirection.FindClosest(Projectile.position,1000);
                if (i != null)
                {
                    NewProjectile(Projectile.GetSource_FromAI(), source.X, source.Y, vector8.X, vector8.Y, 631, num41, 0f, Projectile.owner, i.whoAmI);
                    NewProjectile(Projectile.GetSource_FromAI(), source.X, source.Y, vector8.X, vector8.Y, 631, num41, 0f, Projectile.owner, i.whoAmI, 15f);
                }
                Projectile.DProj().Bool[0] = false;
            }
        }

        public override void OnKill(int timeLeft)
        {
            NewDustChange(80, Projectile.position, Projectile.Size, 229, 1F, 8F);
        }
        public Color color = new Color();
        public int itemtype;
        public virtual Color TrailColor(float completionRatio)
        {
            Item item = Projectile.Player().ActiveItem();
            if (color == new Color(0, 0, 0, 0) || itemtype != item.type)
            {
                Color[] colors = DDHelper.GetColors(DDTextures.Bow[item.type].Value);
                int a = 0;
                Vector4 vector4 = new Vector4(0, 0, 0, 0);
                for (int i = 0; i < colors.Length; i++)
                {
                    if (colors[i] != new Color(0, 0, 0, 0))
                    {
                        a++;
                        vector4 += colors[i].ToVector4();
                    }
                }
                vector4 /= a / 2;
                color = new Color(vector4.X, vector4.Y, vector4.Z, 255);
                itemtype = item.type;
            }
            RangedGlobalItem rangedItem = item.GetGlobalItem<RangedGlobalItem>();
            if (rangedItem.StringColor != new Color(0, 0, 0, 0))
            {
                color = rangedItem.StringColor;
            }
            if (!rangedItem.BowstringGlow)
            {
                return Lighting.GetColor((int)(Projectile.Player().Center.X / 16), (int)(Projectile.Player().Center.Y / 16), color);
            }
            return color;
        }
        public virtual float TrailWidth(float completionRatio)
        {
            Item item = Projectile.Player().ActiveItem();
            return Projectile.scale;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public Vector2[] vectors = new Vector2[3];
        public void Draw(ref Color lightColor, Vector2 Position, float rotation,float alpha)
        {
            Projectile projectile = Main.projectile[(int)Projectile.ai[0]];
            if (projectile == null || !projectile.active || projectile.type <= 0 || projectile.type != ModContent.ProjectileType<GlobalBow>())
            {
                for (int A = 0; A < 1000; A++)
                {
                    if (Main.projectile[A].active && Main.projectile[A].type == ModContent.ProjectileType<GlobalBow>() && Main.projectile[A].owner == projectile.owner)
                    {
                        projectile = Main.projectile[A];
                        break;
                    }
                }

                if (projectile == null || !projectile.active || projectile.type <= 0 || projectile.type != ModContent.ProjectileType<GlobalBow>())
                    return;
            }
            RangedProjectile ranged = projectile.GetGlobalProjectile<RangedProjectile>();
            RangedProjectile ranged2 = Projectile.GetGlobalProjectile<RangedProjectile>();
            Projectile.spriteDirection = projectile.spriteDirection;
            Item item = Projectile.Player().ActiveItem();
            if(item.type<=0)
            {
                return;
            }
            RangedGlobalItem rangedItem = item.GetGlobalItem<RangedGlobalItem>();
            Player player = Projectile.Player();
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color light = Color.White * 0.5f* alpha;
            float la = ranged.BowTime / Projectile.Player().itemAnimationMax * 6;
            float ArrowOffset = (texture.Width / 2 - (rangedItem.StringOffset + 1) + la);
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["弓弦"]);
            }
            float UP = rangedItem.StringUP;
            float Down = rangedItem.StringDown;
            if (Projectile.spriteDirection == 1)
            {
                UP = rangedItem.StringDown;
                Down = rangedItem.StringUP;
            }
            else
            {

                rotation += MathHelper.Pi;
            }
            if (rangedItem.Bow)
            {
                Vector2 vector = rotation.ToRotationVector2().RotatedBy(MathHelper.Pi* Projectile.spriteDirection);
                vectors[0] = Position - vector.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * ((DDTextures.Bow[item.type].Height() / 2 - UP) * Projectile.scale);
                vectors[1] = Position - vector.PerfectNormalize() * la * 2 * Projectile.scale;
                vectors[2] = Position + vector.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * ((DDTextures.Bow[item.type].Height() / 2 - Down) * Projectile.scale);
                TrailDrawer.Draw(vectors, -Main.screenPosition - vector.PerfectNormalize() * ((DDTextures.Bow[item.type].Width() / 2 - rangedItem.StringOffset) * Projectile.scale) + vector.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * 0, 88, null);

                Main.spriteBatch.Draw(texture, Position - Main.screenPosition - new Vector2(0, rangedItem.YOffset).RotatedBy(rotation), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), light * 0.5f, rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(Projectile.scale + la / 100, Projectile.scale * 1.25f - la / 100), (SpriteEffects)Projectile.spriteDirection, 0f);
                Main.spriteBatch.Draw(texture, Position - Main.screenPosition - new Vector2(0, rangedItem.YOffset).RotatedBy(rotation), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), new Color(68, 255, 255, 0) * 0.5F * alpha, rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(Projectile.scale + la / 100, Projectile.scale * 1.25f - la / 100), (SpriteEffects)Projectile.spriteDirection, 0f);

                light = lightColor* alpha;
                if (ranged.ShootShop)
                {
                    float AmmoRotation = rotation + MathHelper.PiOver2;
                    if (Projectile.spriteDirection == 1)
                    {
                        AmmoRotation -= MathHelper.Pi;
                    }
                    int A = ranged.Ammo[0];
                    Main.instance.LoadProjectile(A);
                    texture = TextureAssets.Projectile[A].Value;
                    if (ranged.Ammo[0] == 1 && player.Dplayer().Ammo == ModContent.ItemType<珍珠木箭>())
                    {
                        texture = RangedProjectile.珍珠木箭.Value;
                    }
                    Main.spriteBatch.Draw(texture, Position - Main.screenPosition - vector.PerfectNormalize() * (ArrowOffset - texture.Height / 2) * Projectile.scale, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), light, AmmoRotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                if (!ranged.ShootShop && ranged.BowAnimationTime > 0)
                {
                    texture = TextureAssets.Extra[65].Value;
                    if (ranged.BowAnimationTime <= 6)
                    {
                        Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / 6 * (6 - ranged.BowAnimationTime - 1), texture.Width, texture.Height / 6));
                        Main.spriteBatch.Draw(texture, Position - Main.screenPosition - new Vector2(-14 * player.direction, 2).RotatedBy(rotation), rectangle, Color.White* alpha, rotation, rectangle.Value.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Draw(ref lightColor, Projectile.Center, Projectile.rotation,0.5F);
            return false;
        }
        public Trailing TrailDrawer;
    }
}