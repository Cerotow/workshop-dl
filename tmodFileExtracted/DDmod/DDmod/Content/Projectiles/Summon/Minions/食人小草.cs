using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 食人小草 : Summons
    {

        public override void Load()
        {
        }
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<食人小草Buff>(), 8, 21, 800);
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.minionSlots = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
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
            return true;
        }
        public override void Visual()
        {
            if (!Projectile.DProj().Bool[0])
            {
                for (float A = 0; A < 22; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 0, Projectile.oldVelocity.X, Projectile.oldVelocity.Y)];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale * 1.25F;
                    dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(1, 4);
                }
                Projectile.DProj().Bool[0] = true;
            }
            if (Projectile.ai[1] != 0)
            {
                frameCounter += 0.2f;
                Projectile.frame = (int)frameCounter % 3;
            }
            else if(Projectile.velocity.Y==0)
            {
                frameCounter += Math.Abs(Projectile.velocity.X) / 10;
                Projectile.frame = (int)frameCounter % 4;
            }
            else
            {
                frameCounter += 0.2f;
                Projectile.frame = (int)frameCounter % 2;
            }
        }
        float frameCounter = 0;
        public override bool AttackAI()
        {
            return false;
        }
        public override bool MobileAI()
        {
            Projectile.DProj().Magnification = 1;
            if (Projectile.ai[1]!=0)
            {
                Projectile.damage = (int)(Projectile.damage* 0.5F);
                Projectile.DProj().Magnification = 0.5F;

            }
            Projectile.rotation = 0;
            if (Projectile.velocity.Y == 0)
            {
                Projectile.ai[2]--;
            }
                if (target)
            {
                Vector2 direction = npc.Center - Projectile.Center;
                float Speed = Math.Abs(direction.X) / 15;
                if (Speed > 10)
                {
                    Speed = 10;
                }
                else if (Speed < 2f)
                {
                    Speed = 2f;
                }
                float SpeedY = Math.Abs(direction.Y) / 10;
                if (SpeedY > 15)
                {
                    SpeedY = 15;
                }
                else if (SpeedY < 6)
                {
                    SpeedY = 6;
                }
                if (Projectile.ai[1] == 0)
                {
                    if (Projectile.velocity.Y == 0)
                    {
                        if (direction.X > 0)
                        {
                            Projectile.spriteDirection = 1;
                        }
                        else
                        {
                            Projectile.spriteDirection = 0;
                        }
                    }
                    if (Projectile.velocity.Y == 0)
                    {
                        if (Math.Abs(direction.X) > 5)
                        {
                            if (direction.X > 0)
                            {
                                if (Projectile.velocity.X < Speed)
                                    Projectile.velocity.X += Speed / 10;
                            }
                            else
                            if (direction.X < 0)
                            {
                                if (Projectile.velocity.X > -Speed)
                                    Projectile.velocity.X -= Speed / 10;
                            }
                            if ((direction.Y > 50))
                            {
                                Projectile.velocity.Y = -SpeedY;
                            }
                        }
                        if (Projectile.ai[2] <= 0)
                        {
                            if ((direction.Y+40 <= Projectile.height ))
                            {
                                Projectile.velocity.Y = -SpeedY;
                                direction = direction.PerfectNormalize();
                                Projectile.velocity.X = direction.X * Speed;
                                Projectile.ai[2] = 10;
                            }
                        }
                    }
                    else
                    {
                        if (Projectile.velocity.Y < 10) Projectile.velocity.Y += 0.4F;
                    }
                    if ((direction.Y < -150 || Math.Abs(direction.X) > 500))
                    {
                        Projectile.ai[1] = 1;
                    }
                    Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0;
                }
                else
                {
                    if (SpeedY < 12f)
                    {
                        SpeedY = 12f;
                    }
                    Projectile.velocity = (Projectile.velocity * 20 + direction.PerfectNormalize() * (SpeedY)) / 21;

                    if (Projectile.velocity.X > 0)
                    {
                        Projectile.spriteDirection = 1;
                    }
                    else
                    {
                        Projectile.spriteDirection = 0;
                    }
                    Projectile.rotation = Projectile.velocity.X * 0.05f;
                    if (npc.active && npc.type > 0)
                    {
                        if ((Math.Abs(direction.Y) < 5 || Math.Abs(direction.X) < 5))
                        {
                            for (int a = 0; a < 7; a++)
                            {
                                if (Main.tile[(int)npc.Center.X / 16, (int)((Projectile.Center.Y + Projectile.height) / 16) + a].HasTile)
                                {
                                    Projectile.ai[1] = 0;
                                }
                            }
                        }
                    }
                    Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0.5f;
                }
            }
            else
            {
                Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0;
                int A = 0;
                for (int k = 0; k < 1000; k++)
                {
                    if (Main.projectile[k].active && Main.projectile[k].type == Projectile.type && k <= Projectile.whoAmI && Projectile.owner == Main.myPlayer)
                    {
                        A++;
                    }
                }
                Vector2 direction = player.Center - Projectile.Center;
                direction.X -= ((10 + 40 * A) * player.direction);
                float Speed = Math.Abs(direction.X) / 50;
                if (Speed > 4)
                {
                    Speed = 4;
                }
                else if (Speed < 0.5f)
                {
                    Speed = 0.5f;
                }
                float SpeedY = Math.Abs(direction.X + direction.Y) / 50;
                if (SpeedY > 15)
                {
                    SpeedY = 15;
                }
                else if (SpeedY < 6)
                {
                    SpeedY = 6;
                }
                if (Projectile.velocity.X > 0)
                {
                    Projectile.spriteDirection = 1;
                }
                else
                {
                    Projectile.spriteDirection = 0;
                }
                if (Projectile.ai[1] == 0)
                {
                    if (Projectile.velocity.Y == 0)
                    {
                        if (Math.Abs(direction.X) > 5)
                        {
                            if (direction.X > 0)
                            {
                                if (Projectile.velocity.X < Speed)
                                    Projectile.velocity.X += Speed / 10;
                            }
                            else
                            if (direction.X < 0)
                            {
                                if (Projectile.velocity.X > -Speed)
                                    Projectile.velocity.X -= Speed / 10;
                            }
                            if ((direction.Y > 50))
                            {
                                Projectile.velocity.Y = -SpeedY;
                            }
                        }
                        else
                        {
                            if (direction.Length() < 0.1F)
                            {
                                Projectile.velocity = new Vector2(player.direction * 0.0001F, 0);
                            }
                            else
                            Projectile.velocity.X *= 0.6F;
                        }
                        if (direction.X > 0)
                        {
                            Projectile.spriteDirection = 1;
                        }
                        else
                        {
                            Projectile.spriteDirection = 0;
                        }
                    }
                    else
                    {
                        if (Projectile.velocity.Y < 10) Projectile.velocity.Y += 0.4F;
                    }
                    if ((direction.Y < -150 || Math.Abs(direction.X) > 500))
                    {
                        Projectile.ai[1] = 1;
                    }
                }
                else
                {
                    Projectile.velocity = (Projectile.velocity * 20 + direction.PerfectNormalize() * (SpeedY)) / 21;
                    if(direction.Length()<0.1F)
                    {
                        Projectile.velocity = new Vector2(player.direction*0.0001F,0);
                    }
                    Projectile.rotation = Projectile.velocity.X * 0.05f;
                    if (player.velocity.Y == 0 && (Math.Abs(direction.Y) < 5 || Math.Abs(direction.X) < 5))
                    {
                        for (int a = 0; a < 7; a++)
                        {
                            if (Main.tile[(int)((player.Center.X - ((10 + 40 * A) * player.direction)) / 16), (int)((Projectile.Center.Y + Projectile.height) / 16) + a].HasTile)
                            {
                                Projectile.ai[1] = 0;
                            }
                        }
                    }
                }
            }
            Projectile.tileCollide = Projectile.ai[1] == 0;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            int A = 0;
            if(Projectile.ai[1] != 0)
            {
                A = 2;
            }
            else
            if (target)
            {
                A = 1;
            }
                Rectangle rectangle = new(texture.Width/3* A, texture.Height/4*Projectile.frame,texture.Width/3,texture.Height/4);
            Main.EntitySpriteDraw(texture, Projectile.position+new Vector2(Projectile.width/2,Projectile.height) - Main.screenPosition, rectangle, lightColor, Projectile.rotation,new Vector2(rectangle.Width/2, rectangle.Height-2), Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0);
            //Main.EntitySpriteDraw(DDTextures.WhitePng.Value, Projectile.position- Main.screenPosition, null, lightColor*0.3F, Projectile.rotation,Vector2.Zero, Projectile.Size/2, 0, 0);
            return false;
        }
    }
}