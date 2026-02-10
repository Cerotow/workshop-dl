using DDmod.Players;

namespace DDmod.Content.Projectiles.OrnamentProjectile
{
    public class ServantOfTheHeart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Main.projFrames[Projectile.type] = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.ArmorPenetration = 20;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (target.knockBackResist > 0)
            {
                Vector2 vector = (target.Center - Main.player[Projectile.owner].Center).PerfectNormalize() * 6;
                target.velocity = vector* target.knockBackResist;
                DDmod.SyncData(DDType.NPCCenter, target.whoAmI, -1, Projectile.owner);
            }
            Projectile.localAI[2] = 30;
            Projectile.netUpdate = true;
        }
        float Length;
        public override void AI()
        {
            if(Projectile.localAI[2] > 0)
            {
                Projectile.localAI[2]--;
                Projectile.damage = 0;
            }
            else
            {
                Projectile.damage =Projectile.originalDamage;
            }
            Player player = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0f / 255f);
            NPC npc = NPCdirection.FindClosest(player.Center, 250);
            if (npc != null)
            {
                Vector2 vector = npc.Center - player.Center;
                if (vector.Length() < 250)
                {
                    if (Length < vector.Length())
                    {
                        Length += 1.5f;
                    }
                    else
                    {
                        Length -= 1.5f;
                    }
                }
            }
            else
            {
                Length -= 1.5f;
            }
            if (Length <= 0 && NPCdirection.FindClosest(player.Center, 250) == null)
            {
                Projectile.active = false;
            }

            if (Projectile.ai[0] == 0)
            {
                Projectile.localAI[0] += Projectile.ai[2];
                //float SD = 0.05F + Length / 4000;
                float SD = 0.1F;
                if (player.direction == 1)
                {
                    if(Projectile.ai[2] < SD)
                    {
                        Projectile.ai[2] += SD/10;
                    }

                }
                else
                {
                    if (Projectile.ai[2] > -SD)
                    {
                        Projectile.ai[2] -= SD / 10;
                    }
                }
            }
            else
            {
                Projectile.localAI[0] = Main.projectile[(int)Projectile.ai[1]].localAI[0];
            }
            double ro = MathHelper.TwoPi / 5 * Projectile.ai[0] + Projectile.localAI[0];
            ro %= MathHelper.TwoPi;
            Projectile.Center = player.Center + new Vector2(Length, 0).RotatedBy(ro);
            AttributesPlayer modPlayer = player.Aplayer();
            if (player.dead)
            {
                modPlayer.ServantOfTheHeart = false;
            }
            if (modPlayer.ServantOfTheHeart)
            {
                Projectile.timeLeft = 2;
            }
            if (player.direction == 1)
            {
                Projectile.rotation = (float)(ro);
            }
            else
            {
                Projectile.rotation = (float)(ro + Math.PI);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Rectangle value = new Rectangle(0, 0, 32, 32);
            Vector2 vector = Projectile.Size / 2;
            Color color = new Color(255, 100, 100, 50);
            Color color2 = color;
            color2.A = 255;
            color2 *= (1F - Projectile.localAI[2] / 60);
            Main.spriteBatch.Draw(DDTextures.VoidStar.Value, Projectile.position - Main.screenPosition + vector,null, color2, Projectile.rotation, DDTextures.VoidStar.Size()/2, Projectile.scale*0.8F, (SpriteEffects)Projectile.spriteDirection, 0f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                color2 = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f) * (1F - Projectile.localAI[2] / 60);
                Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vector2, null, color2, 1, DDTextures.VoidStar.Size() / 2, 0.8f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw((Texture2D)TextureAssets.Projectile[Projectile.type], Projectile.position - Main.screenPosition + vector, new Rectangle?(value), Color.White, Projectile.rotation, TextureAssets.Projectile[Projectile.type].Size()/2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);

            return false;
        }
    }
}