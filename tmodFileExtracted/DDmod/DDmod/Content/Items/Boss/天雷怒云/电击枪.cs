using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Gun;

namespace DDmod.Content.Items.Boss.天雷怒云
{
    public class 电击枪 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = 8;
            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
            sound.Pitch = -0.5f;
            sound.Volume = .1f;
            Item.UseSound = sound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<枪闪电>();
            Item.shootSpeed = 20f;
            Item.GetGlobalItem<RangedGlobalItem>().ShootOffset = new Vector2(-4, -10);
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.001f;
        }
        public bool A;
        public int LS;
        public int useTime;
        public int useAnimation;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = Item.shoot;
            velocity = velocity.RotatedBy(Item.GetGlobalItem<RangedGlobalItem>().Rota * (-player.direction));
            //position += new Vector2(-4, -6);
            NewProjectile(source, position+velocity.PerfectNormalize()*24, velocity, type, damage, knockback, player.whoAmI,0,0.5F,300);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -6);
        }
    }
    public class 枪闪电 : 闪电
    {
        public override void AI()
        {
            if (V2 == null)
            {
                V2 = new List<Vector2>();
            }
            if (Projectile.ai[2] != 0)
            {
                Projectile.timeLeft = (int)Projectile.ai[2];
                Projectile.localAI[2] = Projectile.ai[2];
                Projectile.ai[2] = 0;
                int Type = ModContent.DustType<光球粒子>();
                for (int A = 0; A < 30; A++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(0, 186, 242, 0))];
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(0.5F, 2F);
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 1f);
                    dust.rotation = Projectile.rotation;
                    dust.customData = -1;
                }
            }
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity.PerfectNormalize() * 3;
                Projectile.velocity = Projectile.DProj().vector[0];
            }
            if (Projectile.timeLeft < 2)
            {
                if (Vector == null)
                {
                    Vector = new Vector2[Projectile.oldPos.Length];
                    for (int i = 0; i < Projectile.oldPos.Length; i++)
                    {
                        Vector[i] = Projectile.oldPos[i];
                    }
                }
                Projectile.timeLeft = 10000;
            }
            else
            if (Projectile.timeLeft > 1000)
            {
                Projectile.extraUpdates = 15;
                Projectile.damage = 0;
                Projectile.timeLeft = 10000;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale -= 0.004F;
                if (Projectile.scale <= 0)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.scale = Projectile.ai[1];
                V2.Add(Projectile.position);
                Projectile.DProj().Times[0]++;
                if (Projectile.DProj().Times[0] > 20 && Main.rand.NextBool(10) && Projectile.DProj().track > 3)
                {
                    Projectile.DProj().Times[0] = 0;
                    List<NPC> NONPC = new List<NPC>();
                    for(int A=0;A< Projectile.localNPCImmunity.Length;A++)
                    {
                        if(Projectile.localNPCImmunity[A]==-1)
                        {
                            NONPC.Add(Main.npc[A]);
                        }
                    }
                    NPC npc = NPCdirection.FindClosest2(Projectile.Center, 500, false, NONPC);
                    if (npc != null && npc.CanBeChasedBy(Projectile, false))
                    {
                        float A = Vector2.Subtract(npc.Center, Projectile.Center).Length() / 300;
                        if (A > 0.6F)
                        {
                            A = 0.6F;
                        }
                        Vector2 vector1 = Utils.RotatedBy(Vector2.Subtract(npc.Center, Projectile.Center).PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-A, A), default);
                        Projectile.velocity = vector1;
                    }
                    else
                    {
                        Vector2 vector1 = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-0.6F, 0.6F), default);
                        Projectile.velocity = vector1;
                    }

                    Projectile.netUpdate = true;
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Charged2>(), 30);
            Projectile.ai[0]--;
            if(Projectile.ai[0]<=-5)
            {
                Projectile.timeLeft = 2;
            }
            int Type = ModContent.DustType<光球粒子>();
            for (int A = 0; A < 20; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(0, 186, 242, 0))];
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(0.5F, 1F);
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 3f);
                dust.rotation = Projectile.rotation;
                dust.customData = -2;
            }
            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
            sound.Pitch = -0.1f;
            sound.Volume = .1f;
            PlaySound(sound, Projectile.position);
            Projectile.netUpdate = true;
        }
    }
}