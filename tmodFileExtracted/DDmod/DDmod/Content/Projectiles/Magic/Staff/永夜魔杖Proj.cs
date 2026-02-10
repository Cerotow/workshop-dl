using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.NoContent.Config;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 永夜魔杖Proj : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.localAI[0] = -1;
        }
        List<NPC> npcs = new List<NPC>();
        List<Vector2> vectorsPo = new List<Vector2>();
        public override bool PreAI()
        {
            int i = 150;
            //if(vectors==null|| vectors.Length != i)
            {
                vectors = new Vector2[i];
                Rot = new float[i];
            }
            Player player = Main.player[Projectile.owner];
            Projectile.HoldProj(player, 32, 0, Vector2.Zero, MathHelper.PiOver4);
            Projectile.ownerHitCheck = false;

            vectors[0] = Projectile.Center+Projectile.velocity.PerfectNormalize()*34;
            npcs = new List<NPC>();
            Vector2 vector = Projectile.velocity.PerfectNormalize()*10;

            bool mw = false;
            bool mwT = false;
            for(int a =1;a< Projectile.ai[2]; a++)
            {
                if (!mw)
                {
                    NPC npc = NPCdirection.FindClosest2(vectors[a - 1], 1000, false, npcs);

                    if (vector.Length() < 10)
                    {
                        vector = vector.PerfectNormalize() * 10;
                    }
                    else if (vector == Vector2.Zero)
                    {
                        vector = Rot[a - 1].ToRotationVector2() * 10;
                    }
                    
                    if (npc != null)
                    {
                        if (Projectile.Colliding(Projectile.getRect(), npc.getRect()))
                        {
                            //vector = (npc.Center - vectors[a - 1]).PerfectNormalize() * (vectors[a - 1] - npc.Center).Length();
                            npcs.Add((NPC)npc.Clone());
                            npcs[npcs.Count - 1].Center = vectors[a-1]+vector;
                        }
                        else
                        {
                            if (npcs.Count != 0)
                            {
                                vector = (npc.Center - vectors[a - 1]).PerfectNormalize() * 10;
                            }
                        }
                    }
                    else
                    {
                        vector = vector.PerfectNormalize() * 10;
                    }
                    if (a <= 10)
                    {
                        vector *= ((float)a + 1) / 10;
                    }
                    Rot[a] = vector.ToRotation();
                    Vector2 vector2 = Collision.TileCollision(vectors[a - 1] - Projectile.Size, vector, Projectile.width, Projectile.height, true, true);
                    /*
                    if (vector.X != vector2.X)
                    {
                        vector.X = -vector.X;
                    }
                    if (vector.Y != vector2.Y)
                    {
                        vector.Y = -vector.Y;
                    }*/
                    vectors[a] = vectors[a - 1] + vector;
                    if (vector != vector2)
                    {
                        if (!mw)
                        {
                            vectors[(int)Projectile.ai[2]-1] = vectors[a];
                        }
                        mw = true;
                        mwT = true;
                        vectors[a] = Vector2.Zero;
                    }
                    if (player.Aplayer().NightEnergy)
                    {
                        if (npcs.Count >= 5)
                        {
                            if (!mw)
                            {
                                vectors[(int)Projectile.ai[2] - 1] = vectors[a];
                            }
                            mw = true;
                            vectors[a] = Vector2.Zero;
                        }
                    }
                    else
                    {
                        if (npcs.Count >= 3)
                        {
                            if (!mw)
                            {
                                vectors[(int)Projectile.ai[2] - 1] = vectors[a];
                            }
                            mw = true;
                            vectors[a] = Vector2.Zero;
                        }
                    }
                }
                else
                    if (a != Projectile.ai[2] - 1)
                {
                    vectors[a] = Vector2.Zero;
                }
            }
            if (Projectile.ai[2] >= 1)
            {
                if (mwT)
                {
                    NewDustChange4(4, vectors[(int)Projectile.ai[2] - 1] - Projectile.Size / 2, Projectile.Size, ModContent.DustType<光球粒子>(), 0, 0.4f, true, 0.4F, 2F, 100, 0, new Color(101, 36, 253, 40), 1);
                    NewDustChange4(4, vectors[(int)Projectile.ai[2] - 1] - Projectile.Size / 2, Projectile.Size, ModContent.DustType<光球粒子>(), 0, 8, true, 0.4F, 2F, 100, 0, new Color(101, 36, 253, 40), 3);
                    NewDustChange4(2, vectors[(int)Projectile.ai[2] - 1] - Projectile.Size / 2, Projectile.Size, ModContent.DustType<速度粒子>(), 6, 14, true, 1F, 2.5F, 100, 1000, new Color(101,36, 253, 40), 3);
                }
            }
            Rot[0] = Rot[1];
            if (player.statMana <= 0 && Main.myPlayer == Projectile.owner)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[2] < vectors.Length)
                Projectile.ai[2] += 5f;
            else
                Projectile.ai[2] = vectors.Length;
            if (Projectile.ai[1] >= 20)
            {
                if (Projectile.ai[0] > player.IteUseAnimation())
                {
                    Vector2 vector2 = Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1F,1F));
                    int A = player.ItemMana();
                    player.statMana -= A;
                    NPC npc = NPCdirection.FindClosest(Projectile.Center, 1000, false);
                    if (npcs.Count > 0 && player.Aplayer().NightEnergy && npc != null)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 50), vector2 * 8, ModContent.ProjectileType<永夜魔弹>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 1F);
                        }
                        SoundStyle sound = SoundID.Item4;
                        sound.Pitch = 1F;
                        sound.Volume = 0.2F;
                        PlaySound(sound, Projectile.position);
                    }
                    Projectile.ai[0] -= player.IteUseAnimation();
                }
            }
            else
            {
                Projectile.ai[0] = 0;
                Projectile.ai[1]+=0.25f;
            }
            Projectile.localNPCHitCooldown = (int)player.IteUseAnimation2()/2;
            
            int damageWithChargeAndStats = (int)(player.GetWeaponDamage(player.HeldItem)* Projectile.ai[1]/20);
            Projectile.damage = damageWithChargeAndStats/2;
            if(player.Aplayer().NightEnergy)
            {
                Projectile.damage = damageWithChargeAndStats;
            }
            return false;
        }
        public Vector2[] vectors; 
        public float[] Rot; 
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int a = 0; a < Projectile.ai[2]; a++)
            {
                projHitbox.X = (int)(vectors[a].X - projHitbox.Width / 2);
                projHitbox.Y = (int)(vectors[a].Y - projHitbox.Height / 2);
                if(projHitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref HitModifiers modifiers)
        {
            for (int a = 0; a < npcs.Count; a++)
            {
                if (npcs[a].whoAmI == target.whoAmI)
                {
                    modifiers.FinalDamage *= 0.5F + (((5 - a) * 0.2F) * 0.5F);
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge, settings, Projectile.owner);
            Projectile.netUpdate = true;
            Projectile.numHits = 2;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(!target.CanBeChasedBy())
            {
                return null; 
            }
            else
            {
                bool? R = false;
                for (int a = 0; a < npcs.Count; a++)
                {
                    if (target.whoAmI == npcs[a].whoAmI)
                    {
                        R = null;
                    }
                }
                return R;
            }
        }
        public override bool? CanDamage()
        {
            if(Projectile.ai[1] >= 60)
            {
                return null;
            }
            return null;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            float S = Projectile.scale;

            if (player.Aplayer().NightEnergy)
            {
                S *= 1.5F;
            }
                if (Projectile.velocity.X < 0)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)1, 0f);
                Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0) * (Projectile.ai[1] / 20), Projectile.rotation + MathHelper.PiOver2, Glow.Size() / 2, Projectile.scale / 4, (SpriteEffects)1, 0f);

            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(81, 6, 233, 0) * (Projectile.ai[1] / 20), Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4, 0, 0f);
            }
            Vector2 SC =new Vector2(2,1)*S / 4;
            for (int a = 0; a < Projectile.ai[2]; a++)
            {
                if (vectors[a] != Vector2.Zero)
                {
                    SC = new Vector2(2, 1) * S / 4;
                    if (a < 10)
                    {
                        SC *= ((float)a) / 10;
                    }
                    if (a == Projectile.ai[2] - 1)
                    {
                        Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vectors[a] - Main.screenPosition, null, new Color(81, 6, 233, 155) * (Projectile.ai[1] / 60), Rot[a], DDTextures.VoidStar.Size() / 2, S, 0, 0f);
                        Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vectors[a] - Main.screenPosition, null, new Color(81, 6, 233, 100).Opposite() * (Projectile.ai[1] / 60), Rot[a], DDTextures.VoidStar.Size() / 2, S / 2, 0, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vectors[a] - Main.screenPosition, null, new Color(81, 6, 233, 155) * (Projectile.ai[1] / 60), Rot[a], DDTextures.VoidStar.Size() / 2, SC, 0, 0f);
                        Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vectors[a] - Main.screenPosition, null, new Color(81, 6, 233, 100).Opposite() * (Projectile.ai[1] / 60), Rot[a], DDTextures.VoidStar.Size() / 2, SC / 2, 0, 0f);
                    }
                }
            }
            for (int a = 0; a < Projectile.ai[2]; a++)
            {
                if (vectors[a] != Vector2.Zero)
                {
                    if (a >= 10&& Math.Abs(Rot[a] - Rot[a - 1]) > 0.1F)
                    {
                        Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vectors[a - 1] - Main.screenPosition, null, new Color(81, 6, 233, 155) * (Projectile.ai[1] / 60), Rot[a], DDTextures.VoidStar.Size() / 2, S, 0, 0f);
                        Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vectors[a - 1] - Main.screenPosition, null, new Color(81, 6, 233, 100).Opposite() * (Projectile.ai[1] / 60), Rot[a], DDTextures.VoidStar.Size() / 2, S / 2, 0, 0f);
                    }
                }
            }
            for (int a = 0; a < npcs.Count; a++)
            {
                if (npcs[a].active)
                {
                    Main.spriteBatch.Draw(DDTextures.VoidStar.Value, npcs[a].Center - Main.screenPosition, null, new Color(81, 6, 233, 155) * (Projectile.ai[1] / 60), Rot[a], DDTextures.VoidStar.Size() / 2, S/2, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.VoidStar.Value, npcs[a].Center - Main.screenPosition, null, new Color(81, 6, 233, 155) * (Projectile.ai[1] / 60), Rot[a], DDTextures.VoidStar.Size() / 2, S / 2, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.VoidStar.Value, npcs[a].Center - Main.screenPosition, null, new Color(81, 6, 233, 100).Opposite() * (Projectile.ai[1] / 60), Rot[a], DDTextures.VoidStar.Size() / 2, S / 3, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.VoidStar.Value, npcs[a].Center - Main.screenPosition, null, new Color(81, 6, 233, 100).Opposite() * (Projectile.ai[1] / 60), Rot[a], DDTextures.VoidStar.Size() / 2, S / 3, 0, 0f);
                }
            }
            return false;

        }
    }
    public class 永夜魔弹 : ModProjectile
    {

        public override string Texture => "DDmod/Image/Nullpng";
        public override void Load()
        {
        }
        List<NPC> npcs = new List<NPC>();
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.extraUpdates = 3;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            ProjectileID.Sets.TrailCacheLength[Type] = 22;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override bool PreAI()
        {
            Projectile.scale = 0.6F;
            NPC npc = NPCdirection.FindClosest2(Projectile.Center, 1000, false, npcs);
            if (npc != null)
            {
                Projectile.Chase(npc, 10, 100);
            }
            else
            {
                if (
            Projectile.DProj().track>30)
                Projectile.Kill();
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange4(40, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 8, true, 0.4F, 2F, 100, 0, new Color(101, 36, 253, 40), 5);
            NewDustChange4(10, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 6, 14, true, 2F, 3.5F, 100, 1000, new Color(101, 36, 253, 40), 3);

        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void ModifyHitNPC(NPC target, ref HitModifiers modifiers)
        {
            Projectile.velocity /= 4;
            NewDustChange4(20, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 5, true, 0.4F, 1F, 100, 0, new Color(101, 36, 253, 40), 2);
            NewDustChange4(5, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 3, 8, true, 2F, 3.5F, 100, 1000, new Color(101, 36, 253, 40), 2);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            npcs.Add(target);
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = DDTextures.VoidStar.Value;
            Color color = new Color(81, 6, 233, 105);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/2, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/2, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color.Opposite(), Projectile.rotation, texture.Size() / 2, Projectile.scale/4, 0, 0);
            for (int a = 0; a < Projectile.oldPos.Length; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, null, color * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), Projectile.oldRot[a], texture.Size() / 2, Projectile.scale/2 * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), 0, 0);
            }

            return false;
        }
    }
}