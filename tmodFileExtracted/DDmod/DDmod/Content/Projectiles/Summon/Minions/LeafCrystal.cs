using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class LeafCrystal : Summons
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
        }
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<LeafCrystalBuff>(),0, 0, 1200, ModContent.ProjectileType<SForestBullets>(), 10, 20, 0);
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.minionSlots = 1;
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
            float SP = vector.Length()/4;
            if (SP < 3 && SP > 0.5F) SP = 3; else if (SP < 0.5F) SP = 0;
            vector.DirectPerfectNormalize();
            Projectile.velocity = (Projectile.velocity * 20 + vector * SP)/21;
            if(DistanceNPC>3000)
            {
                Projectile.Center = player.Center - new Vector2(0, 70);
            }
            Projectile.rotation = Projectile.velocity.X * 0.03F;
            return false;
        }
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
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, (SpriteEffects)1, 0);
            for (int a = 0; a < Projectile.oldPos.Length; a++)
            {
                Main.EntitySpriteDraw(Glow.Value, Projectile.oldPos[a]+Projectile.Size/2 - Main.screenPosition, null, new Color(125, 213, 18, 0) * (Projectile.ai[0] / AttackSpeed) * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), Projectile.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 2), Projectile.scale / 2 * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), (SpriteEffects)1, 0);
            }
            return false;
        }
    }
}