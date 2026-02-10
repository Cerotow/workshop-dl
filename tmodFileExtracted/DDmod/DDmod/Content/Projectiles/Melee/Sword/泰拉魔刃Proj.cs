

using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items;
using DDmod.NoContent.Config;
using DDmod.Players;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class 泰拉魔刃Proj : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.width = 20;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 34;
            Projectile.extraUpdates =10;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        int proj = 0;
        int S = 0;
        int PO = 0;
        bool POB = false;
        int POC = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

            Projectile.HoldProj(player, 24 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.06f * Projectile.DProj().Times[0], false, false);
            if (S > 4)
            {
                player.itemTime = 0;
                player.itemAnimation = 0;
                Projectile.Kill();
                return false;
            }
            else if (S > 2)
            {
                player.itemRotation = Projectile.DProj().vector[0].ToRotation();
                if(player.direction==-1)
                {
                    player.itemRotation += MathHelper.Pi;
                }
                Projectile.extraUpdates = 20;
                Projectile.MeleeProj().SwordHitbox = false;
                Projectile.DProj().Times[4] = 9999;
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), 80, Projectile.DProj().Times[4], true);
                if (!POB)
                {
                    PO ++;
                    if (PO >360)
                    {
                        POB = true;
                    }
                }
                else
                {
                    if (POC > 600)
                    {
                        PO-=2;
                    }
                    else
                    {
                        Projectile.knockBack = 0.8F;
                        POC+=2;
                    }
                    if (PO<0)
                    {
                        player.itemTime = 0;
                        player.itemAnimation = 0;
                        Projectile.Kill();
                        return false;
                    }
                }
                Projectile.position += Projectile.DProj().vector[0] * PO/2;
            }
            else if (S >= 0)
            {
                Projectile.DProj().Times[4] = 2.4f;
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), -1, Projectile.DProj().Times[4], true);
            }
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead;
            if (channeling)
            {
                Projectile.DProj().Times[2] = 0;
            }
            /*

            Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1),-1, 2.2f, true);
            */
            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0)
                {
                    PlaySound(SoundID.Item1, Projectile.position);

                    proj++;
                    S++;
                    Projectile.netUpdate = true;
                }
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (ModContent.GetInstance<DDConfigClient>().SwordHit)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/Sword");
                sound.Pitch = 0;
                PlaySound(sound, target.position);
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color*0.3F;
            Main.projectile[A].scale = 0.4F;
            if(S==3&& (!POB|| POC > 1200) &&target.knockBackResist !=0)
            {
                if (!POB)
                {
                    target.velocity = Projectile.DProj().vector[0] * 15 * (1-(float)PO / 360+(0.2F* (float)PO / 360));
                }
                else
                {

                    target.velocity = Projectile.DProj().vector[0] * 15;
                }
                Mod mod = DDmod.Instance;
                Projectile.netUpdate = true;
                if (Main.netMode == 0)
                {
                    return;
                }
                DDmod.SyncData(DDType.NPCCenter, target.whoAmI, -1, Projectile.owner);
            }

        }
        Color color = new Color(191, 255, 191, 0)*0.5F;
        public float TWidth()
        {
            return 12*Projectile.scale;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1]++;
                return false;
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }
            vector += Projectile.DProj().vector[0] * PO/2;
            DDHelper.BladeTrail(DDTextures.WhitePng, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, TWidth());
            DDHelper.BladeTrail(DDTextures.Wave, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, TWidth());
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D texture = TextureAssets.Item[4144].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
    }
}