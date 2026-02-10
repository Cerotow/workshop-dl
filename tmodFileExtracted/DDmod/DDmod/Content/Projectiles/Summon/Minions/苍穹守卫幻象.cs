using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 苍穹守卫幻象 : Summons
    {
        public override void Load()
        {
        }
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<苍穹守卫幻象Buff>(),0, 0, 720, ModContent.ProjectileType<星象>(), 2, 20, 0);
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.minionSlots = 3;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            /// <summary> 感觉有点针对召唤师了 </summary> ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void Visual()
        {

            Lighting.AddLight(Projectile.Center, new Color(125, 213, 18).ToVector3() * 0.6f);
            int r = 0;
            for(int a = 0;a< Projectile.whoAmI; a++)
            {
                Projectile Proj = Main.projectile[a];
                if(Proj.active&&Proj.type == Projectile.type&&Proj.owner==Projectile.owner)
                {
                    r++;
                }
            }
            if(r>0)
            {
                Projectile.Kill();
            }
        }
        public override bool MobileAI()
        {
            Vector2 vector = player.Center-Projectile.Center - new Vector2(0, 70);
            float SP = vector.Length()/8;
            if (SP >12) SP =12; else if (SP < 0.5F) SP = 0;
            vector.DirectPerfectNormalize();
            Projectile.velocity = (Projectile.velocity * 20 + vector * SP)/21;
            if(DistanceNPC>3000)
            {
                Projectile.Center = player.Center - new Vector2(0, 70);
            }
            Projectile.rotation += Projectile.velocity.X * 0.01F;
            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.01F;
            }
            else
            {
                Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.01F;
            }
            ManaTextures =
        [
            0,0,0,1,0,0,0,
            0,0,1,0,1,0,0,
            1,1,0,0,0,1,1,
            1,0,0,0,0,0,1,
            0,1,0,1,0,1,0,
            1,0,1,0,1,0,1,
            1,1,0,0,0,1,1,
        ];
            if (Projectile.ai[1]>0)
            {
                Projectile.ai[1]--;
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                vector = player.Center - Projectile.Center - new Vector2(0, 70);
                if (vector.Length()>600)
                {
                    for (int a = 0; a < ManaTextures.Length; a++)
                    {
                        if (ManaTextures[a] == 1)
                        {
                            Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(Projectile.rotation);
                            Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(Projectile.rotation) / 2));

                            int Proj = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                            Main.dust[Proj].velocity = velocity * 3;
                            Main.dust[Proj].customData = 3;

                        }
                    }
                    Projectile.Center = player.Center - new Vector2(0, 70);
                    for (int a = 0; a < ManaTextures.Length; a++)
                    {
                        if (ManaTextures[a] == 1)
                        {
                            Vector2 dustPO = new Vector2(a % 7, a / 7).RotatedBy(Projectile.rotation);
                            Vector2 velocity = (dustPO - (new Vector2(6).RotatedBy(Projectile.rotation) / 2));

                            int Proj = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 1.5F);
                            Main.dust[Proj].velocity = velocity * 3;
                            Main.dust[Proj].customData = 3;

                        }
                    }
                }
            }
                return false;
        }
        public byte[] ManaTextures =
        [
            0,0,0,1,0,0,0,
            0,0,1,0,1,0,0,
            1,1,0,0,0,1,1,
            1,0,0,0,0,0,1,
            0,1,0,1,0,1,0,
            1,0,1,0,1,0,1,
            1,1,0,0,0,1,1,
        ];
        public override bool AttackAI()
        {
            if (target)
            {
                Projectile.ai[0]++;
                Vector2 direction = npc.Center - Projectile.Center;
                DistanceNPC = direction.Length();
                direction = direction.PerfectNormalize();
                if (Projectile.ai[0] >= AttackSpeed)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, direction * shootSpeed, shoot, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 1);
                    }
                    SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/法杖");
                    sound.Pitch = 1.5F;
                    sound.Volume = 0.2F;
                    PlaySound(sound, Projectile.position);
                    Projectile.ai[0] = 0;
                    Projectile.ai[1] = 5;
                }
            }
            else
            {
                if (Projectile.ai[0] < AttackSpeed)
                {
                    Projectile.ai[0]++;
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 vector = Projectile.Center - Main.screenPosition + ((Projectile.rotation - MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * -6);
            for (int a = 0; a < Projectile.oldPos.Length; a++)
            {
                Main.EntitySpriteDraw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, null, new Color(0, 95, 255, 0) * (Projectile.ai[0] / AttackSpeed) * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), (SpriteEffects)1, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(0, 95, 255,255), Projectile.rotation, texture.Size()/2, Projectile.scale/4, (SpriteEffects)1, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(0, 95, 255,0), Projectile.rotation, texture.Size()/2, Projectile.scale/4, (SpriteEffects)1, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(0, 95, 255,0).Opposite(), Projectile.rotation, texture.Size()/2, Projectile.scale/6, (SpriteEffects)1, 0);

            return false;
        }
    }
}