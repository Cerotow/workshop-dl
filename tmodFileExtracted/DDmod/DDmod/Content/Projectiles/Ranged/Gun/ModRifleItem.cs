using DDmod.Content.Items;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Ranged.Make.Gun;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Ranged.Ammo;
using DDmod.Players;
using Mono.Cecil;
using Terraria;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class Rifle : Gun
    {
        public override GunTypes Types => GunTypes.Rifle;
        public override string Texture => "DDmod/Image/Nullpng";
        public override int ID => (int)Projectile.ai[2];
    }
    public class 寒霜冲锋枪Proj :Rifle
    {
        public override int EveryShootCD => 5;
        public override int EveryMaxShootTimes => -20;
        public override int GunRecoilMaxTime => 5;

        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.01F;
            ShootPo -= new Vector2(0, 6);
            GunflamesOffset -= new Vector2(-1, 5);
            Flamesframe = 2;
            flamescolor = new Color(0, 155, 255, 100);
            AmmoPo -= new Vector2(12, 8);
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 24;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            if (projToShoot == 14)
            {
                projToShoot = ModContent.ProjectileType<冰霜弹Proj>();
            }
            NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);

        }
    }
    public class 白切枪Proj : Rifle
    {
        public override int EveryShootCD => 8;
        public override int EveryMaxShootTimes => -2;
        public override int GunRecoilMaxTime => 8;

        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.03F;
            ShootPo -= new Vector2(0, 3);
            GunflamesOffset -= new Vector2(-1, 3);
            Flamesframe = 2;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override float PositionXoffset => 16;
        public override void OnKill(int timeLeft)
        {
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);
            if(Main.rand.NextBool(10))
            {
                Pos += new Vector2(-Main.rand.Next(30,50)*player.direction,Main.rand.Next(-80,0));
                NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, ModContent.ProjectileType<白切剑Proj>(), Damage, KnockBack, Projectile.owner,0,0, Main.rand.NextFloat(0.8F, 1.3F));
            }

        }
    }
    public class 蜜蜂加特林Proj : Rifle
    {
        public override int EveryShootCD => 0;
        public override int GunRecoilMaxTime => 1;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0;
            ShootPo = new Vector2(0, 6.5F);
            GunflamesOffset = new Vector2(-9, 5);
            AmmoPo.X -= 10;
            AmmoPo.Y -= 8;
            Flamesframe = 2;
        }
        public override float DrawY => 0;
        public override float PositionYoffset => 8;
        public override float PositionXoffset => 24;
        public override void AI()
        {
            Player player = Projectile.Player();
            bool canShoot = player.channel && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;
            CanShoot = false;
            Projectile.localAI[0] += Projectile.ai[1] / 100;
            if (Projectile.localAI[0] >= player.ActiveItem().useAnimation * 2)
            {
                Projectile.frame++;
                Projectile.localAI[0] -= player.ActiveItem().useAnimation * 2;
            }
            if (Projectile.ai[1] < 600)
            {
                if (Projectile.frame >= 3)
                {
                    Projectile.frame = 0;
                    CanShoot = true;
                }
                Projectile.ai[1] += Projectile.Player().GetAttackSpeed(DamageClass.Ranged);
                if (!canShoot)
                {
                    Projectile.ai[1] += Projectile.Player().GetAttackSpeed(DamageClass.Ranged);
                }
            }
            else
            {
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
                CanShoot = true;
                Projectile.ai[1] = 600;
            }
            Maxdithering = Projectile.ai[1] / 10000;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            PlaySound(SoundID.Item41, Projectile.position);
        }
        public override void Shoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            Vector2 ShootPo = Vector2.Zero;
            if (Projectile.DProj().Times[0] == 0)
            {
                ShootPo = new Vector2(0, 6.5F);
                GunflamesOffset = new Vector2(-9, 5);
            }
            else if (Projectile.DProj().Times[0] == 1)
            {
                ShootPo = new Vector2(0, 14.5F);
                GunflamesOffset = new Vector2(-9, 13);
            }
            else if (Projectile.DProj().Times[0] == 2)
            {
                ShootPo = new Vector2(0, -2.5F);
                GunflamesOffset = new Vector2(-9, -3);
            }

            else if (Projectile.DProj().Times[0] == 3)
            {
                ShootPo = new Vector2(0, 2.5F);
                GunflamesOffset = new Vector2(-5, 1);
            }

            else if (Projectile.DProj().Times[0] == 4)
            {
                ShootPo = new Vector2(0, 10.5F);
                GunflamesOffset = new Vector2(-5, 9);
            }

            else if (Projectile.DProj().Times[0] == 5)
            {
                ShootPo = new Vector2(0, 10.5F);
                GunflamesOffset = new Vector2(-13, 9);
            }

            else if (Projectile.DProj().Times[0] == 6)
            {
                ShootPo = new Vector2(0, 2.5F);
                GunflamesOffset = new Vector2(-13, 1);
            }
            Pos.Y -= 6.5F;
            Pos += ShootPo;
            if(projToShoot==14||Main.rand.NextBool(4))
            {
                projToShoot = player.beeType();
                Damage = player.beeDamage(Damage);
                KnockBack = player.beeKB(KnockBack);
                Pos -= Projectile.velocity.PerfectNormalize() * 32;
            }
            int a= NewProjectile(Source, Pos, Projectile.velocity.PerfectNormalize() * speed, projToShoot, Damage, KnockBack, Projectile.owner);
            Main.projectile[a].DamageType = DamageClass.Ranged;

            Projectile.DProj().Times[0] = Main.rand.Next(7);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture;
            if (ID <= 0)
            {
                texture = TextureAssets.Projectile[Projectile.type].Value;
            }
            else
            {
                texture = TextureAssets.Item[ID].Value;
            }
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - new Vector2(DrawX, DrawY).RotatedBy(Projectile.rotation) - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(DrawX, DrawY).RotatedBy(Projectile.rotation) - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.FlipVertically, 0f);
            }
            Texture2D Flames = DDTextures.GunFlames.Value;
            Player player = Main.player[Projectile.owner];
            if (Gunflames > 0)
            {
                Rectangle rectangle = new Rectangle(0, Flames.Height / 4 * Flamesframe, Flames.Width, Flames.Height / 4);
                Vector2 vector = new Vector2(-texture.Width / 2 - GunflamesOffset.X, rectangle.Height / 2 - GunflamesOffset.Y * player.direction);

                Main.spriteBatch.Draw(Flames, GunflamesPo - Main.screenPosition, rectangle, flamescolor * Gunflames, Flamesrotation, vector, Projectile.scale, 0, 0f);
            }
            return false;
        }
    }
    public class MeteorRifle : Rifle
    {

        public override GunTypes Types => GunTypes.Rifle;
        public override void Set()
        {
            Projectile.width = 52;
            Projectile.height = 26;
            Maxdithering = 0.02F;
            ShootPo -= new Vector2(0, 4);
            Flamesframe = 1;
            GunflamesOffset = new Vector2(0, -3);
        }
        public override float DrawY => 0;
        public override float PositionYoffset => -2;
        public override void ExShoot(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
        }
        public override void ShootEffect(Player player, Vector2 Pos, IEntitySource Source, int projToShoot, float speed, int Damage, float KnockBack, int usedAmmoItemId)
        {
            PlaySound(SoundID.Item11, Projectile.position);
            ShootTimes++;
            if (ShootTimes > 3)
            {
                Vector2 Center = new Vector2(0, -9 * player.direction).RotatedBy(Projectile.rotation);
                ShootTimes = 0;
                if (Main.myPlayer == Projectile.owner)
                {
                    int A = NewProjectile(Source, Pos + Center - Projectile.velocity.PerfectNormalize() * 26, Projectile.velocity.PerfectNormalize() * speed, ModContent.ProjectileType<RangedGreenLaser>(), Damage * 3, KnockBack, Projectile.owner);
                    Main.projectile[A].extraUpdates = 1000;
                    Main.projectile[A].penetrate = -1;
                }
                PlaySound(SoundID.Item157, Projectile.position);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 Center = new Vector2(0, -11).RotatedBy(Projectile.rotation);
            Texture2D
                texture = TextureAssets.Item[ID].Value;
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(DDTextures.Wire.Value, Projectile.Center - Main.screenPosition + Center, new Rectangle?(new Rectangle(0, 0, (int)Projectile.SolidTileDistanceDetection2(DDTextures.Wire.Width(), 2), DDTextures.Wire.Height())), new Color(0, 255, 0), Projectile.rotation, new Vector2(0, DDTextures.Wire.Height() / 2), Projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Center = new Vector2(0, -11).RotatedBy(Projectile.rotation - MathHelper.Pi);
                Main.spriteBatch.Draw(DDTextures.Wire.Value, Projectile.Center - Main.screenPosition + Center, new Rectangle?(new Rectangle(0, 0, (int)Projectile.SolidTileDistanceDetection2(DDTextures.Wire.Width(), 2), DDTextures.Wire.Height())), new Color(0, 255, 0), Projectile.rotation, new Vector2(0, DDTextures.Wire.Height() / 2), Projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
            }
            Texture2D Flames = DDTextures.GunFlames.Value;
            Player player = Main.player[Projectile.owner];
            if (Gunflames > 0)
            {
                Rectangle rectangle = new Rectangle(0, Flames.Height / 4 * Flamesframe, Flames.Width, Flames.Height / 4);
                Vector2 vector = new Vector2(-texture.Width / 2 - GunflamesOffset.X, rectangle.Height / 2 - GunflamesOffset.Y * player.direction);

                Main.spriteBatch.Draw(Flames, GunflamesPo - Main.screenPosition, rectangle, new Color(255, 255, 255, 255) * Gunflames, Flamesrotation, vector, Projectile.scale, 0, 0f);
            }

            return false;
        }
    }
}