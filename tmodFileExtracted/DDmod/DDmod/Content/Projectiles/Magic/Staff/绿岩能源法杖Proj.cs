using DDmod.Content.Projectiles.GeneralProj;
using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 绿岩能源法杖Proj : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Wifi;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
            Wifi = ModContent.Request<Texture2D>(Texture + "_Wifi");
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.HoldProj(player, 38, 0, Vector2.Zero, MathHelper.PiOver4);

            if (player.statMana <= 0 && Main.myPlayer == Projectile.owner)
            {
                Projectile.Kill();
            }
            if (Projectile.DProj().Times[1] > 0)
            {
                Projectile.DProj().Times[1]--;
            }
            else if (Projectile.DProj().Times[1] < 0)
            {
                Projectile.DProj().Times[1]++;
            }
            if (Projectile.ai[0] > player.IteUseAnimation())
            {
                Vector2 vector2 = Projectile.velocity.PerfectNormalize();
                NPC npc = NPCdirection.FindClosest(player.Dplayer().MouseWorld, 1000, false);
                if (npc != null)
                {
                    int A = player.ItemMana();
                    player.statMana -= A;
                    Projectile.DProj().Times[1] = 12;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        for (int a = 0; a < 6; a++)
                        {
                            Vector2 vector = new Vector2(Main.rand.NextFloat(200, 700) * player.direction, 1000);

                            int proj = NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld - vector, vector.PerfectNormalize() * player.ActiveItem().shootSpeed, ModContent.ProjectileType<绿岩导弹>(), Projectile.damage, Projectile.knockBack, -1, 1, npc.whoAmI);
                            Main.projectile[proj].DamageType = DamageClass.Magic;
                            Main.projectile[proj].scale = 0.5F;
                            Main.projectile[proj].DProj().track = 3;
                        }
                    }
                    SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/绿岩科技法杖");
                    sound.MaxInstances = 10;
                    sound.Pitch = 0F;
                    sound.Volume = 0.5f;
                    PlaySound(sound, Projectile.position);
                }
                else
                {

                    Projectile.DProj().Times[1] = -12;
                }
                Projectile.ai[0] -= player.IteUseAnimation();
            }
            int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
            Projectile.damage = damageWithChargeAndStats;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            SpriteEffects sprite = 0;
            Vector2 vector = new Vector2(0, 6).RotatedBy(Projectile.velocity.ToRotation());
            if(Projectile.velocity.X<0)
            {
                sprite = SpriteEffects.FlipHorizontally;
                Projectile.rotation += MathHelper.PiOver2;
                vector = new Vector2(0, -6).RotatedBy(Projectile.velocity.ToRotation());
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, sprite, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Glow.Value.Size() / 2, Projectile.scale, sprite, 0f);
            
            if (Projectile.DProj().Times[1] > 0)
            {
                texture = Wifi.Value;
                Rectangle rectangle = new Rectangle(texture.Width / 4 * (4-(int)(Math.Abs(Projectile.DProj().Times[1]))/3), 0, texture.Width / 4, texture.Height);
                Main.spriteBatch.Draw(Wifi.Value, Projectile.Center - vector - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 40, rectangle, Color.White, Projectile.velocity.ToRotation() + MathHelper.PiOver2, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            }
            else if (Projectile.DProj().Times[1] < 0)
            {
                texture = Wifi.Value;
                Rectangle rectangle = new Rectangle(texture.Width / 4 * (1 - (int)(Math.Abs(Projectile.DProj().Times[1])) / 12), 0, texture.Width / 4, texture.Height);
                Main.spriteBatch.Draw(Wifi.Value, Projectile.Center- vector - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 40, rectangle, new Color(255,0,0,100), Projectile.velocity.ToRotation() + MathHelper.PiOver2, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            }
            return false;
        }
    }
}