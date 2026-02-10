using DDmod.Content.Dusts;
using DDmod.Content.Items;
using DDmod.Content.Items.Ammo;
using DDmod.Content.Items.Ranged;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Players;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Ranged.Bow
{
    public abstract  class BowTemplate : ModProjectile
    {
        public int Take;
        public int type = -1;
        public int Itemtype = -1;
        public void AC(Player player)
        {
            if (type == -1)
            {
                type = player.selectedItem;
            }
            if (type != player.selectedItem)
            {
                Projectile.Kill();
            }
            if (Itemtype == -1)
            {
                Itemtype = player.ActiveItem().type;
            }
            if (Itemtype != player.ActiveItem().type)
            {
                Projectile.Kill();
            }
        }
        bool XLB = false;
        public override bool PreAI()
        {
            //伤害
            Player player = Main.player[Projectile.owner];
            AC(player);
            int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
            Projectile.damage = damageWithChargeAndStats;
            RangedProjectile ranged = Projectile.GetGlobalProjectile<RangedProjectile>();
            ranged.RightClick = player.controlUseTile&&!player.Dplayer().NoRight;
            Item item = Projectile.Player().ActiveItem();
            player.itemTimeMax = item.useAnimation;
            player.itemAnimationMax = item.useAnimation;
            if (player.itemAnimationMax <= 0)
            {
                player.itemTimeMax = item.useAnimation;
                player.itemAnimationMax = item.useAnimation;
            }
            if (item.type <= ItemID.None)
            {
                Projectile.Kill();
                return false;
            }
            RangedGlobalItem rangedItem = item.GetGlobalItem<RangedGlobalItem>();
            Lighting.AddLight(Projectile.Center, rangedItem.BowLight);
            if (!rangedItem.Bow)
            {
                Projectile.Kill();
                return false;
            }
            Projectile.HoldProj(player, 16+rangedItem.Offset, 0, Vector2.Zero, 0, 0, true, 1, false);
            player.heldProj = Projectile.whoAmI;

            DDHelper.BackAndForth(-inertia, inertia, inertia, ref inertiaT, ref inertiaB);
            inertia *= 0.35F;
            if (!ranged.ShootShop)
            {
                Take--;
                if (Take > 0)
                {
                    player.ChangeDir(Projectile.direction);
                    player.PlayerAction().PlayerArmRotation((player.Dplayer().MouseWorld - player.ArmCenter()).ToRotation() * player.gravDir - MathHelper.PiOver2 - player.fullRotation, Player.CompositeArmStretchAmount.Full);
                    player.PlayerAction().PlayerArmRotationBack((player.Dplayer().MouseWorld - player.ArmCenter()).ToRotation() * player.gravDir - MathHelper.PiOver2 - player.fullRotation, Player.CompositeArmStretchAmount.Full);

                    Projectile.HoldProj(player, 16 + rangedItem.Offset, 0, Vector2.Zero, 0, 0, true, 1, false);
                    player.itemAnimation = 0;
                    player.itemTime = 0;
                    player.Dplayer().ProjAnimation = true;
                    if (player.nonTorch != -1|| Main.LocalPlayer.mouseInterface)
                    {
                        Projectile.Kill();
                        return false;
                    }
                }
                else
                {
                    if (player.direction == 1)
                    {
                        Projectile.HoldProj(player, 18, 0, new Vector2(1, 0).RotatedBy(-0.8f + player.fullRotation), 0, 0, true, 1, false);
                    }
                    else
                    {
                        Projectile.HoldProj(player, 18, 0, new Vector2(1, 0).RotatedBy(MathHelper.Pi + 0.8f + player.fullRotation), 0, 0, true, 1, false);
                    }
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    Projectile.Center = player.MountedCenter - new Vector2(4 * player.direction, 4 - player.gfxOffY);
                    player.heldProj = -1;
                }
                //射出僵直动画
                if (ranged.BowAnimationTime > 0)
                {
                    ranged.BowAnimationTime--;
                }
                if (!Projectile.DProj().Bool[0])
                {
                    ranged.BowTime -= 8;
                    if (ranged.BowTime < -4)
                    {
                        Projectile.DProj().Bool[0] = true;
                    }
                }
                if (Projectile.DProj().Bool[0])
                {
                    ranged.BowTime += 4f;
                    if (ranged.BowTime > 0)
                    {
                        ranged.BowTime = 0;
                    }
                }
                //僵直完成
                if (ranged.BowTime == 0 && ranged.BowAnimationTime <= 0 && (player.controlUseItem || ranged.RightClick)&& !player.HasBuff(23))
                {
                    Projectile.HoldProj(player, 16 + rangedItem.Offset, 0, Vector2.Zero, 0, 0, true, 1, false);
                    ranged.ShootShop = true;
                    ranged.BowAnimationTime = 8;
                    ranged.Ammo[0] = 0;
                    ranged.Ammo[1] = 0;
                    ranged.Ammo[2] = 0;
                    if (ranged.RightClick)
                    {      
                        ranged.Charge = true;
                    }
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                player.ChangeDir(Projectile.direction);
                Take = 180;

                //player.bodyFrame.Y = player.bodyFrame.Height * 3;
                player.itemTime = (int)(player.ActiveItem().useAnimation);
                player.itemAnimation = (int)(player.ActiveItem().useAnimation);
                float BT = (ranged.BowTime / player.itemAnimationMax/2);
                Player.CompositeArmStretchAmount amount;
                if (BT < 0.3F)
                {
                    amount = (Player.CompositeArmStretchAmount)0;
                }
                else if (BT < 0.6F)
                {
                    amount = (Player.CompositeArmStretchAmount)3;
                }
                else if (BT < 0.8F)
                {
                    amount = (Player.CompositeArmStretchAmount)2;
                }
                else
                {
                    amount = (Player.CompositeArmStretchAmount)1;
                }
                player.PlayerAction().PlayerArmRotation((player.Dplayer().MouseWorld - player.ArmCenter()).ToRotation() * player.gravDir - MathHelper.PiOver2 - player.fullRotation, amount);
                player.PlayerAction().PlayerArmRotationBack((player.Dplayer().MouseWorld - player.ArmCenter()).ToRotation() * player.gravDir - MathHelper.PiOver2 - player.fullRotation,Player.CompositeArmStretchAmount.Full);

                if (player.itemTime < 2 || player.itemAnimation < 2)
                {
                    player.itemTime = 2;
                    player.itemAnimation = 2;
                }
                if (ranged.BowTime <= 0)
                {
                    if (ModifyBowLoader.SetArrows(item.type,item, Projectile, rangedItem, ranged))
                    {
                        if (ranged.Ammo[0] == 0)
                        {
                            bool ThereAmmo = player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId);

                            if (!ThereAmmo)
                            {
                                ranged.ShootShop = false;
                                ranged.BowTime = 0;
                                Projectile.DProj().Times[2] = 0;
                                ranged.SpecialEffect = false;
                                ranged.Charge = false;
                            }
                            ModifyBowLoader.PreModifyArrow(item.type,item, Projectile, rangedItem, ranged, ref speed, ref damage, ref knockBack, ref usedAmmoItemId, ref projToShoot);

                            ranged.BowSpeed[0] = speed;
                            ranged.BowDamage[0] = damage;
                            ranged.BowKnockBack[0] = knockBack;
                            ranged.BowUsedAmmoItemId[0] = usedAmmoItemId;
                            ranged.Ammo[0] = projToShoot;
                        }
                        ranged.BowKnockBack[0] = player.GetWeaponKnockback(player.inventory[player.selectedItem], ranged.BowKnockBack[0]);
                        ModifyBowLoader.PostModifyArrow(item.type,item, Projectile, rangedItem, ranged, ref ranged.BowSpeed[0], ref ranged.BowDamage[0], ref ranged.BowKnockBack[0], ref ranged.BowUsedAmmoItemId[0], ref ranged.Ammo[0], ref ranged.BowKnockBack[0]);
                    }
                    if (ranged.Charge)
                    {
                        ranged.BowTime += player.GetTotalAttackSpeed(DamageClass.Ranged) * 0.75f;
                    }
                    else
                    {
                        ranged.BowTime += player.GetTotalAttackSpeed(DamageClass.Ranged);
                    }
                    Projectile.netUpdate = true;
                }
                else if (ranged.BowTime > 0 && ranged.BowTime < player.itemAnimationMax*2)
                {
                    if (ranged.Charge)
                    {
                        ranged.BowTime += player.GetTotalAttackSpeed(DamageClass.Ranged) *0.75f;
                        ModifyBowLoader.ChargeUpdate(item.type,item, Projectile, rangedItem, ranged);
                        if (ranged.BowTime >= player.itemAnimationMax * 1.8F)
                        {
                            if (
                            !XLB)
                            {
                                XLB = true;
                                SoundStyle sound = SoundID.Item4;
                                sound.Pitch = -1f;
                                PlaySound(sound, Projectile.position);
                            }
                        }
                        else
                        {
                            XLB = false;
                        }
                    }
                    else if(ranged.BowTime < player.itemAnimationMax)
                    {
                        ranged.BowTime += player.GetTotalAttackSpeed(DamageClass.Ranged);
                        ModifyBowLoader.NoChargeUpdate(item.type,item, Projectile, rangedItem, ranged);
                    }

                }
                if (ranged.Charge && ranged.BowTime >= player.itemAnimationMax)
                {
                    ModifyBowLoader.Skill(item.type,item, Projectile, rangedItem, ranged);
                }
                if (!ranged.Charge)
                {
                    if (ranged.BowTime > player.itemAnimationMax)
                    {
                        ranged.BowTime = player.itemAnimationMax;
                    }
                }
                else
                {
                    if (ranged.BowTime > player.itemAnimationMax * 2)
                    {
                        ranged.BowTime = player.itemAnimationMax * 2;
                    }
                }
                if ((ranged.BowTime >= player.itemAnimationMax && !ranged.Charge) || (!ranged.RightClick && ranged.BowTime >= player.itemAnimationMax*0.5f && ranged.Charge))
                {
                    ModifyBowLoader.Shoot(item.type,item, Projectile, rangedItem, ranged);
                    inertia = ranged.BowTime / Projectile.Player().itemAnimationMax * 3;
                    ranged.ShootShop = false;
                    ranged.BowTime = 0;
                    PlaySound(SoundID.Item5, Projectile.position);
                    ranged.Ammo[0] = 0;
                    ranged.Ammo[1] = 0;
                    ranged.Ammo[2] = 0;
                    Projectile.DProj().Times[2] = 0;
                    ranged.SpecialEffect = false;
                    ranged.Charge = false;
                    Projectile.netUpdate = true;
                }
            }
            if (player.dead)
            {
                Projectile.Kill();
            }
            Projectile.hide = player.heldProj >= 0;
            Projectile.spriteDirection = player.direction == 1 ? 0 : 1;
            if (Projectile.spriteDirection == 1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
            ModifyBowLoader.PostUpdate(item.type,item, Projectile, rangedItem, ranged);
            Projectile.position += Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / 56];

            /*
            if (item.type == ModContent.ItemType<樱之弓>())
            {
                if (Projectile.rotation != Projectile.oldRot[0])
                {
                    float ro = Projectile.rotation - Projectile.oldRot[0];
                    if (Math.Abs(ro) < 1)
                    {
                        rot += ro;
                    }
                }
                if (Math.Abs(rot) < MathHelper.PiOver2)
                {
                    rot += player.velocity.X * 0.03F;
                    int A = 5;
                    if (player.ZoneSandstorm)
                    {
                        A += 100;
                    }
                    if (Main.windSpeedCurrent * A > 0)
                    {
                        if (rot < Main.windSpeedCurrent * A)
                        {
                            rot -= (Main.windSpeedCurrent * A - rot) / 10;
                        }
                    }
                    if (Main.windSpeedCurrent * A < 0)
                    {
                        if (rot > Main.windSpeedCurrent * A)
                        {
                            rot += (Main.windSpeedCurrent * A + rot) / 10;
                        }
                    }
                }
                DDHelper.BackAndForth(-Math.Abs(rot) / 8, Math.Abs(rot) / 8, Math.Abs(rot) / 32,ref rot2,ref Projectile.DProj().Bool[1]);
                if (rot != 0)
                {
                    rot *= 0.92F;
                    if (rot > 0.5F)
                    {
                        rot -= Math.Abs(rot) / 5;
                    }
                    if (rot < -0.5F)
                    {
                        rot += Math.Abs(rot) / 5;
                    }
                }
            }*/
            return false;
        }
        public Color color = new Color();
        public int itemtype;
        public virtual Color TrailColor(float completionRatio)
        {
            Item item = Projectile.Player().ActiveItem();
            Player player = Projectile.Player();
            if(item.type<=0 || !item.GetGlobalItem<RangedGlobalItem>().Bow)
            {
                return Color.White;
            }
            if (color == new Color(0, 0, 0, 0)|| itemtype != item.type)
            {
                Color[] colors = DDHelper.GetColors(DDTextures.Bow[item.type].Value);
                int a = 0;
                Vector4 vector4 = new Vector4(0, 0, 0, 0);
                for (int i = 0; i < colors.Length; i++)
                {
                    if (colors[i] != new Color(0, 0, 0, 0))
                    {
                        a++;
                        vector4 += colors[i].ToVector4();
                    }
                }
                vector4 /= a / 2;
                color = new Color(vector4.X, vector4.Y, vector4.Z, 255);
                itemtype = item.type;
            }
            RangedGlobalItem rangedItem = item.GetGlobalItem<RangedGlobalItem>();
            if(rangedItem.StringColor!=new Color(0,0,0,0))
            {
                color = rangedItem.StringColor;
            }
            if (item.type == ItemID.FairyQueenRangedItem)
            {
                return Main.DiscoColor * (1 - player.immuneAlpha / 255F);
            }
            if (!rangedItem.BowstringGlow)
            {
                return Lighting.GetColor((int)(Projectile.Player().Center.X / 16), (int)(Projectile.Player().Center.Y / 16), color) * (1 - player.immuneAlpha / 255F);
            }
            return color * (1 - player.immuneAlpha / 255F);
        }
        public virtual float TrailWidth(float completionRatio)
        {
            Item item = Projectile.Player().ActiveItem();
            return Projectile.scale;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public Vector2[] vectors = new Vector2[7];
        public float rot;
        public float rot2;
        public float inertia;
        public float inertiaT;
        public bool inertiaB;
        public override bool PreDraw(ref Color lightColor)
        {
            RangedProjectile ranged = Projectile.GetGlobalProjectile<RangedProjectile>();
            Item item = Projectile.Player().ActiveItem();
            RangedGlobalItem rangedItem = item.GetGlobalItem<RangedGlobalItem>();
            if (!rangedItem.Bow) return false;
            Player player = Projectile.Player();
            /*
            Vector2 Velocity = Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), 0, default);
            Vector2 cen = Vector2.Zero;
            if (player.mount.Active)
            {
                Projectile.Center = player.RotatedRelativePoint(player.ArmCenter() - cen, reverseRotation: false, addGfxOffY: false) + Velocity * (16 + rangedItem.Offset) + new Vector2(0, player.gfxOffY);
            }
            else
            {
                Projectile.Center = player.ArmCenter() - cen + Velocity * (16 + rangedItem.Offset) + new Vector2(0, player.gfxOffY);
            }*/
            Texture2D Glow;
            Texture2D texture = DDTextures.Bow[item.type].Value;
            Color light = lightColor*(1-player.immuneAlpha/255F);
            /*
            if (item.type==ModContent.ItemType<樱之弓>())
            {
                Main.spriteBatch.Draw(樱之弓.asset.Value, Projectile.Center - Main.screenPosition + new Vector2(12*Projectile.direction, 6).RotatedBy(Projectile.rotation), null, light, rot+ rot2, new Vector2(樱之弓.asset.Width() / 2, 0), Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            }*/
            float la = ranged.BowTime / Projectile.Player().itemAnimationMax * 6;
            la *= 0.8f;
            if(la==0)
            {
                la = inertiaT;
            }
            float ArrowOffset = (texture.Width / 2 - (rangedItem.StringOffset+1) + la);
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["弓弦"]);
            }
            float UP = rangedItem.StringUP+2;
            float Down = rangedItem.StringDown+2;
            float YOffset = 0;
            if(Projectile.spriteDirection==1)
            {
                 UP = rangedItem.StringDown+2;
                 Down = rangedItem.StringUP+2;
            }
            if (rangedItem.Bow)
            {
                Vector2 Scale = new Vector2(Projectile.scale + la / 100, Projectile.scale*1.25f - la / 100);
                ranged.Scale = Scale;
                vectors[0] = Projectile.Center - Projectile.velocity.RotatedBy(MathHelper.PiOver2) * (((DDTextures.Bow[item.type].Height()*0.5f* Scale.Y - UP)) * Projectile.scale);
                vectors[1] = Projectile.Center - Projectile.velocity.RotatedBy(MathHelper.PiOver2) * (((DDTextures.Bow[item.type].Height() * 0.5f * Scale.Y - UP-10)) * Projectile.scale);
               
                vectors[2] = Projectile.Center  - Projectile.velocity * la * 1.5f * Projectile.scale - Projectile.velocity.RotatedBy(MathHelper.PiOver2) * ((( - UP+24)) * Projectile.scale);
                vectors[3] = Projectile.Center - Projectile.velocity * la * 1.3f * Projectile.scale;
                vectors[4] = Projectile.Center - Projectile.velocity * la * 1.5f * Projectile.scale + Projectile.velocity.RotatedBy(MathHelper.PiOver2) * ((( - Down+24)) * Projectile.scale);

                vectors[5] = Projectile.Center + Projectile.velocity.RotatedBy(MathHelper.PiOver2) * (((DDTextures.Bow[item.type].Height() * 0.5f * Scale.Y - Down-10)) * Projectile.scale);
                vectors[6] = Projectile.Center + Projectile.velocity.RotatedBy(MathHelper.PiOver2) * (((DDTextures.Bow[item.type].Height() * 0.5f * Scale.Y - Down)) * Projectile.scale);
                TrailDrawer.Draw(vectors, -Main.screenPosition - Projectile.velocity.PerfectNormalize() * ((DDTextures.Bow[item.type].Width() / 2 - rangedItem.StringOffset) * Projectile.scale) + Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * 0, 88, null);

                if(rangedItem.Glow)
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(0, rangedItem.YOffset).RotatedBy(Projectile.rotation), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), light, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                if (rangedItem.ResidentGlow)
                {
                    bool TrueGlow = true;
                    if (item.type == ModContent.ItemType<永夜弓>()&& !player.Aplayer().NightEnergy)
                    {
                        TrueGlow = false;
                    }
                    if (item.type == ModContent.ItemType<真永夜弓>()&& !player.Aplayer().TrueNightEnergy)
                    {
                        TrueGlow = false;
                    }
                    if (item.type == ModContent.ItemType<圣弓>()&& !player.Aplayer().HolyEnergy)
                    {
                        TrueGlow = false;
                    }
                    if (item.type == ModContent.ItemType<真圣弓>()&& !player.Aplayer().TrueHolyEnergy)
                    {
                        TrueGlow = false;
                    }
                    if (TrueGlow)
                    {
                        if (DDTextures.BowGlow[item.type] != null)
                        {
                            
                            Glow = DDTextures.BowGlow[item.type].Value;
                            light = Color.White * (1 - player.immuneAlpha / 255F);
                            Main.spriteBatch.Draw(Glow, Projectile.Center - Main.screenPosition - new Vector2(0, rangedItem.YOffset).RotatedBy(Projectile.rotation), new Rectangle?(new Rectangle(0, 0, Glow.Width, Glow.Height)), new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(Glow.Width / 2, Glow.Height / 2), Scale / rangedItem.ScaleGlow, (SpriteEffects)Projectile.spriteDirection, 0f);
                        }
                        else
                        {
                            light = Color.White * (1 - player.immuneAlpha / 255F);
                        }
                    }
                }
                else if (ranged.SpecialEffect)
                {
                    if (DDTextures.BowGlow[item.type] != null)
                    {
                        Glow = DDTextures.BowGlow[item.type].Value;
                        light = Color.White * (1 - player.immuneAlpha / 255F);
                        Main.spriteBatch.Draw(Glow, Projectile.Center - Main.screenPosition - new Vector2(0, rangedItem.YOffset).RotatedBy(Projectile.rotation), new Rectangle?(new Rectangle(0, 0, Glow.Width, Glow.Height)), new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(Glow.Width / 2, Glow.Height / 2), Scale / rangedItem.ScaleGlow, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                if (!rangedItem.Glow)
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(0, rangedItem.YOffset).RotatedBy(Projectile.rotation), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), light, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Scale, (SpriteEffects)Projectile.spriteDirection, 0f);

                light = lightColor * (1 - player.immuneAlpha / 255F);


                if (ranged.ShootShop)
                {
                    float AmmoRotation = Projectile.rotation+MathHelper.PiOver2;
                    if (Projectile.spriteDirection == 1)
                    {
                        AmmoRotation -= MathHelper.Pi;
                    }
                    int A;
                    if (ranged.Ammo[1] > 0)
                    {
                        A = ranged.Ammo[1];
                        Main.instance.LoadProjectile(A);
                        texture = TextureAssets.Projectile[A].Value;
                        if (A == 1&& player.Dplayer().Ammo == ModContent.ItemType<珍珠木箭>())
                        {
                            texture = RangedProjectile.珍珠木箭.Value;
                        }
                        if (DDGlobalProjectile.Glow[ranged.Ammo[1]] != null)
                        {
                            Glow = DDGlobalProjectile.Glow[ranged.Ammo[1]].Value;
                            light = Color.Wheat * (1 - player.immuneAlpha / 255F);
                            Main.spriteBatch.Draw(Glow, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * (ArrowOffset) * Projectile.scale, new Rectangle?(new Rectangle(0, 0, Glow.Width, Glow.Height)), DDGlobalProjectile.GlowColor[ranged.Ammo[1]], AmmoRotation + rangedItem.AboveArrowSpacing, new Vector2(Glow.Width / 2, Glow.Height * 0.8F), Projectile.scale / DDGlobalProjectile.ScaleGlow[ranged.Ammo[1]], (SpriteEffects)Projectile.spriteDirection, 0f);
                        }
                        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * (ArrowOffset) * Projectile.scale, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), light, AmmoRotation + rangedItem.AboveArrowSpacing, new Vector2(texture.Width / 2, texture.Height), Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        light = lightColor * (1 - player.immuneAlpha / 255F);
                    }
                    if (ranged.Ammo[2] > 0)
                    {
                        A = ranged.Ammo[2];
                        Main.instance.LoadProjectile(A);
                        texture = TextureAssets.Projectile[A].Value;
                        if (A == 1 && player.Dplayer().Ammo == ModContent.ItemType<珍珠木箭>())
                        {
                            texture = RangedProjectile.珍珠木箭.Value;
                        }
                        if (DDGlobalProjectile.Glow[ranged.Ammo[2]] != null)
                        {
                            Glow = DDGlobalProjectile.Glow[ranged.Ammo[2]].Value;
                            light = Color.White * (1 - player.immuneAlpha / 255F);
                            Main.spriteBatch.Draw(Glow, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * (ArrowOffset) * Projectile.scale, new Rectangle?(new Rectangle(0, 0, Glow.Width, Glow.Height)), DDGlobalProjectile.GlowColor[ranged.Ammo[2]], AmmoRotation - rangedItem.UnderArrowSpacing, new Vector2(Glow.Width / 2, Glow.Height * 0.8F), Projectile.scale / DDGlobalProjectile.ScaleGlow[ranged.Ammo[2]], (SpriteEffects)Projectile.spriteDirection, 0f);
                        }
                        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * (ArrowOffset) * Projectile.scale, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), light, AmmoRotation - rangedItem.UnderArrowSpacing, new Vector2(texture.Width / 2, texture.Height), Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        light = lightColor * (1 - player.immuneAlpha / 255F);
                    }
                    A = ranged.Ammo[0];
                    if (item.type == 3019&&A==485)
                    {
                        A = 1;
                    }
                    Main.instance.LoadProjectile(A);
                    texture = TextureAssets.Projectile[A].Value;
                    if (A == 1 && player.Dplayer().Ammo == ModContent.ItemType<珍珠木箭>())
                    {
                        texture = RangedProjectile.珍珠木箭.Value;
                    }
                    if (DDGlobalProjectile.Glow[ranged.Ammo[0]] != null)
                    {
                        Glow = DDGlobalProjectile.Glow[ranged.Ammo[0]].Value;
                        light = Color.White * (1 - player.immuneAlpha / 255F);
                        Main.spriteBatch.Draw(Glow, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * (ArrowOffset) * Projectile.scale, new Rectangle?(new Rectangle(0, 0, Glow.Width, Glow.Height)), DDGlobalProjectile.GlowColor[ranged.Ammo[2]], AmmoRotation, new Vector2(Glow.Width / 2, Glow.Height * 0.8F), Projectile.scale / DDGlobalProjectile.ScaleGlow[ranged.Ammo[2]], (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * (ArrowOffset) * Projectile.scale, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), light, AmmoRotation, new Vector2(texture.Width / 2, texture.Height), Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                    if (ranged.Charge)
                {
                    la = ranged.BowTime / Projectile.Player().itemAnimationMax/2;

                    texture = DDTextures.Bow蓄力.Value;
                    Main.spriteBatch.Draw(texture, Projectile.Player().Center - Main.screenPosition - new Vector2(0, 50 - Projectile.Player().gfxOffY), new Rectangle?(new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2)), Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 4), 1, 0, 0f);
                    Main.spriteBatch.Draw(texture, Projectile.Player().Center - Main.screenPosition - new Vector2((-texture.Width * la + 8), 54 - Projectile.Player().gfxOffY), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 2)), Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 4), 1, 0, 0f);
                }
                else
                {
                    G = false;
                }
                if (!ranged.ShootShop && ranged.BowAnimationTime>0)
                {
                    if (item.type == 3854)
                    {
                        texture = DDTextures.VoidStar.Value;
                        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(-10 * player.direction, 24 ).RotatedBy(Projectile.rotation), null, new Color(252,102,25,0) * (1 - player.immuneAlpha / 255F), 0, texture.Size()/2, (ranged.BowAnimationTime / 8f)*new Vector2(2,1F), 0, 0f);
                        texture = DDTextures.Starlight.Value; 
                        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(-10 * player.direction, 24 ).RotatedBy(Projectile.rotation), null, new Color(252,102,25,0) * (1 - player.immuneAlpha / 255F), 0, texture.Size()/2, (ranged.BowAnimationTime / 8f) * new Vector2(2, 1F), 0, 0f);
                        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(-10 * player.direction, 24 ).RotatedBy(Projectile.rotation), null, new Color(252,102,25,0) * (1 - player.immuneAlpha / 255F), 0, texture.Size()/2, (ranged.BowAnimationTime / 16f) * new Vector2(2, 1F), 0, 0f);
                    }
                    if (item.type == 3540)
                    {
                        texture = TextureAssets.Extra[65].Value;
                        if (ranged.BowAnimationTime <= 6)
                        {
                            Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / 6 * (6 - ranged.BowAnimationTime - 1), texture.Width, texture.Height / 6));
                            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(-14 * player.direction, 2).RotatedBy(Projectile.rotation), rectangle, Color.White*(1-player.immuneAlpha/255F), Projectile.rotation, rectangle.Value.Size()/2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                        }
                    }
                }
            }
            return false;
        }
        bool G;
        public Trailing TrailDrawer;
    }
}