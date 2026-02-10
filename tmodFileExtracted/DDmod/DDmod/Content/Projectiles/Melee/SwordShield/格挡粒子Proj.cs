using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.SwordShield;
using DDmod.Content.Projectiles.GeneralProj;
using Terraria;

namespace DDmod.Content.Projectiles.Melee.SwordShield
{
    public class 格挡粒子Proj : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 0;
        }
        public override bool PreAI()
        {

            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;
                Item item = Projectile.Player().ActiveItem();
                if(item.type == ModContent.ItemType<史莱姆剑盾>())
                {
                    Projectile.ai[0] = 2;
                }
                if(item.type == ModContent.ItemType<蜂巢剑盾>())
                {
                    Projectile.ai[0] = 3;
                }
                if(item.type == ModContent.ItemType<机械魔眼剑盾>())
                {
                    Projectile.ai[0] = 4;
                }
                if(item.type == ModContent.ItemType<流星剑盾>())
                {
                    Projectile.ai[0] = 5;
                }
                if(item.type == ModContent.ItemType<绿岩剑盾>())
                {
                    Projectile.ai[0] = 6;
                }
                if(item.type == ModContent.ItemType<护心晶盾>())
                {
                    Projectile.ai[0] = 7;
                }
                Projectile.netUpdate = true;
            }
            else
            {
                Projectile.Kill();
            }
            return false;
        }
        public override bool PreKill(int timeLeft)
        {
            //盾
            if (Projectile.ai[0] == 1)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/格挡");
                sound.Pitch = 0;
                PlaySound(sound, Projectile.position);
                for (int a = 0; a < 10; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<格挡粒子>());
                    Main.dust[A].velocity = (Main.dust[A].position - Projectile.Center).PerfectNormalize() * Main.rand.NextFloat(2, 3);
                    Main.dust[A].noGravity = true;
                }
            }
            //史莱姆剑盾
            if (Projectile.ai[0] == 2)
            {
                SoundStyle sound = SoundID.NPCDeath1;
                sound.Pitch = -0.2f;
                PlaySound(sound, Projectile.position);
                for (int a = 0; a < 30; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, 4,0,0, 100, new Color(78, 136, 255, 105),2.3F);
                    Main.dust[A].velocity = (Main.dust[A].position - Projectile.Center).PerfectNormalize() * Main.rand.NextFloat(2, 12);
                    Main.dust[A].noGravity = true;
                }
                if(Projectile.owner==Main.myPlayer)
                {
                    for(int a =0;a<12;a++)
                    NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center,new Vector2(0,Main.rand.NextFloat(4,8)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)),ModContent.ProjectileType<史莱姆尖刺>(),Projectile.Player().GetWeaponDamage(Projectile.Player().ActiveItem()),1,-1);
                }
            }
            //蜂巢剑盾
            if (Projectile.ai[0] == 3)
            {
                SoundStyle sound = SoundID.NPCDeath1;
                sound.Pitch = -0.2f;
                PlaySound(sound, Projectile.position);
                for (int a = 0; a < 30; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, 153, 0, 0, 100, default, 2.3F);
                    Main.dust[A].velocity = (Main.dust[A].position - Projectile.Center).PerfectNormalize() * Main.rand.NextFloat(2, 4);
                    Main.dust[A].noGravity = true;
                }
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int a = 0; a < 12; a++)
                    {
                        int t = 181;
                        if (Main.player[Projectile.owner].strongBees)
                        {
                            if (Main.rand.NextBool(3))
                                t = 556;
                        }
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, Main.rand.NextFloat(4, 8)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), t, Projectile.Player().GetWeaponDamage(Projectile.Player().ActiveItem())/4, 0, -1);

                    }
                }
            }
            //魔眼剑盾
            if (Projectile.ai[0] == 4)
            {
                SoundStyle sound = SoundID.NPCHit4;
                sound.Pitch = -0.6f;
                PlaySound(sound, Projectile.position);
                for (int a = 0; a < 30; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<格挡粒子>());
                    Main.dust[A].velocity = (Main.dust[A].position - Projectile.Center).PerfectNormalize() * Main.rand.NextFloat(2, 3);
                    Main.dust[A].noGravity = true;
                }
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int a = 0; a < 20; a++)
                    {
                        int t = ModContent.ProjectileType<咒火弹>();
                        int AI = 1;
                        Vector2 vector = new Vector2(0, Main.rand.NextFloat(2, 4)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        if (a%10==0)
                        {
                            AI = 0;
                            vector = new Vector2(0, Main.rand.NextFloat(6, 12)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)); 
                        }
                        int R= NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center,vector , t, Projectile.Player().GetWeaponDamage(Projectile.Player().ActiveItem()), 0, -1, AI);
                        Main.projectile[R].DamageType = DamageClass.Melee;
                    }
                }
            }
            //流星剑盾
            if (Projectile.ai[0] == 5)
            {
                SoundStyle sound = SoundID.NPCHit4;
                sound.Pitch = -0.6f;
                PlaySound(sound, Projectile.position);
                for (int a = 0; a < 30; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<格挡粒子>());
                    Main.dust[A].velocity = (Main.dust[A].position - Projectile.Center).PerfectNormalize() * Main.rand.NextFloat(2, 3);
                    Main.dust[A].noGravity = true;
                }
                if (Projectile.owner == Main.myPlayer)
                {
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FlameExplosion>(), Projectile.Player().GetWeaponDamage(Projectile.Player().ActiveItem()), Projectile.knockBack * 3, -1, 4, 1);
                }
            }
            //绿岩剑盾
            if (Projectile.ai[0] == 6)
            {
                SoundStyle sound = SoundID.NPCHit4;
                sound.Pitch = -0.6f;
                PlaySound(sound, Projectile.position);
                for (int a = 0; a < 30; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<格挡粒子>());
                    Main.dust[A].velocity = (Main.dust[A].position - Projectile.Center).PerfectNormalize() * Main.rand.NextFloat(2, 3);
                    Main.dust[A].noGravity = true;
                }
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int a = 0; a < Main.rand.Next(5,11); a++)
                    {
                        int T = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2()*Main.rand.Next(30,100)/100, ModContent.ProjectileType<绿岩能量>(), Projectile.Player().GetWeaponDamage(Projectile.Player().ActiveItem())/2, 0, -1, -1, 0, 3);
                        Main.projectile[T].DamageType = DamageClass.Melee;
                    }
                }
            }
            //护心晶盾
            if (Projectile.ai[0] == 7)
            {
                SoundStyle sound = SoundID.Tink;
                sound.Pitch = -0.6f;
                PlaySound(sound, Projectile.position);
                for (int a = 0; a < 30; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, 12);
                    Main.dust[A].velocity = (Main.dust[A].position - Projectile.Center).PerfectNormalize() * Main.rand.NextFloat(2, 3);
                    Main.dust[A].noGravity = true;
                }
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int a = 0; a < Main.rand.Next(8,15); a++)
                    {
                        int T = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2()*Main.rand.Next(30,100)/20, ModContent.ProjectileType<MeleeHeart>(), 0, 0, -1);
                        Main.projectile[T].DamageType = DamageClass.Melee;
                    }
                }
            }
            return base.PreKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
    }
}