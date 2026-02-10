using DDmod.Content.Buffs.DeBuffs;

namespace DDmod.Content.Projectiles.Magic
{
    public class MagicElectric : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 125;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.timeLeft = 125;
            Projectile.extraUpdates = 25;
        }
        Vector2[] Vector;
        public override void AI()
        {
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                //SoundStyle sound = SoundID.Thunder;
                sound.Pitch = 0.5f;
                sound.Volume = 0.1f;
                PlaySound(sound, Projectile.position);
                Projectile.DProj().vector[0] = Projectile.velocity / 3;
                Projectile.velocity = Projectile.DProj().vector[0];
            }
            if (Main.rand.NextBool(10))
            {
                Projectile.velocity = Projectile.DProj().vector[0].RotatedBy(Main.rand.NextFloat(-0.8F, 0.8F));
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
                //int Proj = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MagicElectric2>(), 0, 1, Projectile.owner, 0, 0);
                //Main.projectile[Proj].oldPos = Projectile.oldPos;
                Projectile.timeLeft = 10000;
            }
            if (Projectile.timeLeft > 1000)
            {
                Projectile.damage = 0;
                Projectile.timeLeft = 10000;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale -= 0.002F;
                if (Projectile.scale <= 0)
                {
                    Projectile.Kill();
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Charged2>(), 180);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
            //SoundStyle sound = SoundID.Thunder;
            sound.Pitch = 0.5f;
            sound.Volume = 0.2f;
            //SoundStyle sound = SoundID.Thunder;
            //sound.Pitch = 0.5f;
            PlaySound(sound, Projectile.position);
            Projectile.timeLeft = 2;
            for (int i = 0; i < 40; i++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 226)];
                dust.velocity = new Vector2(0,-1).RotatedBy(Main.rand.NextFloat(-1,1))*Main.rand.NextFloat(1,4 );
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        { }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = DDTextures.MiniVoidStar.Value;
            Color color = new Color(0, 186, 242, 0);
            Vector2 vector = Projectile.Size / 2;
            if (Vector == null)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] != Projectile.position)
                    {
                        Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, new Color(0, 186, 242, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 * ((Projectile.oldPos.Length + i) / (float)Projectile.oldPos.Length) + 0.15f, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, vector2, null, new Color(255, 70, 15, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 * ((Projectile.oldPos.Length + i) / (float)Projectile.oldPos.Length) / 2 + 0.025F, spriteEffects, 0f);
                    }
                }
            }
            else
            {
                for (int i = 0; i < Vector.Length; i++)
                {
                    if (Vector[i] != Projectile.position)
                    {
                        Vector2 vector2 = Vector[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, new Color(0, 186, 242, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 * ((Vector.Length + i) / (float)Vector.Length) + 0.15f, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, vector2, null, new Color(255, 70, 15, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 * ((Vector.Length + i) / (float)Vector.Length) / 2 + 0.025F, spriteEffects, 0f);
                    }
                }
            }
            return false;
        }
    }
    public class MagicElectric2 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1000;
            Projectile.extraUpdates = 25;
            Projectile.DProj().Times[1] = 90;
        }
        public override void AI()
        {
            if (Projectile.ai[0]==1)
            {
                for (int A = -1; A <= 1; A++)
                {
                    NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + Main.rand.NextFloat(-10, 10), Projectile.Center.Y + 4), new Vector2(0, 10).RotatedBy(A * 0.3F), ModContent.ProjectileType<MagicElectric>(), Projectile.damage * 2, 1f, Projectile.owner);
                }
                Projectile.Kill();
            }
            if (Projectile.timeLeft < 2)
            {
                Projectile.timeLeft = 10000;
            }
            if (Projectile.timeLeft > 1000)
            {
                Projectile.damage = 0;
                Projectile.timeLeft = 10000;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale -= 0.002F;
                if(Projectile.scale<=0)
                {
                    Projectile.Kill();
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = DDTextures.MiniVoidStar.Value;
            Color color = new Color(0, 186, 242, 0);
            Vector2 vector = Projectile.Size / 2;
            if(Projectile.oldPos == null)
            {
                return false;
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] != Projectile.position)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Main.spriteBatch.Draw(texture, vector2, null, new Color(0, 186, 242, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 * ((Projectile.oldPos.Length + i) / (float)Projectile.oldPos.Length) + 0.15f, spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, vector2, null, new Color(255, 70, 15, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 * ((Projectile.oldPos.Length + i) / (float)Projectile.oldPos.Length) / 2+0.025F, spriteEffects, 0f);
                }
            }
            return false;
        }
    }
}
