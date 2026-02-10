using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Summon;
using DDmod.Modkey;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class 鬼牙Proj : Talismans
    {
        public override void SetStaticDefaults()
        {
        }
        public override void Set()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.ArmorPenetration = 9999999;
            Projectile.width = Projectile.height = 100;
        }
        public override bool? CanDamage()
        {
            if (Projectile.DProj().Bool[0])
            {
                return null;
            }
                return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.DProj().Times[0] >=0&& target.whoAmI== Projectile.DProj().Times[0]&& target.CanBeChasedBy();
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            float lifeStoled = damageDone;
            if (lifeStoled < 1)
            {
                lifeStoled = 1;
            }
            target.AddBuff(30, Main.rand.Next(100, 300));
            if ((int)lifeStoled > 0 && !player.moonLeech && target.CanBeChasedBy())
            {
                NewProjectile(player.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, lifeStoled);
            }
        }
        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.DProj().Times[0] == -1 || !Main.npc[(int)Projectile.DProj().Times[0]].CanBeChasedBy())
            {
                NPC npc = NPCdirection.FindClosest(Projectile.Center, 1000, true);
                if (npc != null)
                {
                    Projectile.DProj().Times[0] = npc.whoAmI;
                }
                Projectile.RotationSpeed(Projectile.velocity.X * 0.05F, 0.05F);
                if (Math.Abs(Projectile.velocity.X) < 0.2F)
                {
                    Projectile.rotation = 0;
                }
                Projectile.spriteDirection = -player.direction;
                //跟随AI
                Vector2 vector = Projectile.Player().MountedCenter;
                vector.X -= 40 * Projectile.Player().direction;
                vector.Y += Float - 20;
                vector = vector - Projectile.Center;
                DDHelper.BackAndForth(-10, 10, 0.2f, ref Float, ref FloatBool);
                float A = vector.Length();
                DDHelper.MaxandMinF(ref A, 10, 0);
                if (!player.dead)
                {
                    if (vector.Length() > 1200)
                    {
                        Projectile.position = Projectile.Player().position - vector.PerfectNormalize() * 1000;
                    }
                    vector.DirectPerfectNormalize();
                    Projectile.velocity = (Projectile.velocity * 8 + vector * A) / 9;
                }
                else
                {
                    if (vector.Length() > 1200)
                    {
                        Projectile.Kill();
                    }
                    vector.DirectPerfectNormalize();
                    Projectile.velocity = (Projectile.velocity * 8 + -vector * A) / 9;
                }
            }
            else
            {

                Projectile.spriteDirection = 0;
                    if (Projectile.DProj().Bool[0])
                {
                    Projectile.rotation = MathHelper.Pi-1.2F;
                }
                if ((Projectile.Center - Main.npc[(int)Projectile.DProj().Times[0]].Center).Length() > 100)
                {
                    NewDustChange(60, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0.1F, 5, Scale: Main.rand.NextFloat(1.2F, 1.8F), color: new Color(220, 0, 25, 0));
                    Projectile.Center = Main.npc[(int)Projectile.DProj().Times[0]].Center - new Vector2(0, Main.npc[(int)Projectile.DProj().Times[0]].height / 2 * 0.8F);
                    NewDustChange(60, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0.1F, 5, Scale: Main.rand.NextFloat(1.2F, 1.8F), color: new Color(220, 0, 25, 0));
                }
                else
                {
                    Projectile.Center = Main.npc[(int)Projectile.DProj().Times[0]].Center - new Vector2(0, Main.npc[(int)Projectile.DProj().Times[0]].height/2*0.8F);
                    NewDustChange(3, Projectile.Center+new Vector2(0,10), Vector2.Zero, ModContent.DustType<光球粒子>(), 0.1F, 2, Scale: Main.rand.NextFloat(0.2F, 0.8F), color: new Color(220, 0, 25, 0));
                }
                Projectile.netUpdate = true;
            }
        }
        public override void End()
        {
            Projectile.DProj().Times[0] = -1;
            NewDustChange(160, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0.1F, 5,true, Main.rand.NextFloat(0.8F, 1.5F),0, new Color(220, 0, 25, 0));
        }
        public override void ExtraUse()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().Times[0] = -1;
            Projectile.velocity = Vector2.Zero;
        }
        public override bool MobileAI()
        {
             Projectile.RotationSpeed(Projectile.velocity.X * 0.05F, 0.05F);
            if (Math.Abs(Projectile.velocity.X)<0.2F)
            {
                Projectile.rotation = 0;
            }
            Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = -player.direction;
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD)
            {
                if (Main.rand.NextBool(20))
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(19, 4), 30, 1, ModContent.DustType<星光粒子>(), 0, 0, 0, new Color(220, 0, 25, 0))];
                    dust.noGravity = true;
                    dust.scale = 0.8F;
                    dust.velocity = new Vector2(0, -1).RotatedBy(Projectile.rotation) * Main.rand.NextFloat(2, 4);
                }
            }
            return base.MobileAI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[2]+=0.1F;
            Player player = Main.player[Projectile.owner];
                SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            
            Vector2 vector = Projectile.Size / 2;

            Texture2D texture = DDTextures.Circle[9].Value;
            texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center- Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            if (Projectile.DProj().Bool[0])
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(220,0, 25, 0), Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(220, 0, 25, 0), Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            }
            return false;
        }
    }
}