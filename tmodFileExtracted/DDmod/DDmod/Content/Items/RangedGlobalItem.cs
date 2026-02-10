using DDmod.Content.Items.Ammo;
using DDmod.Content.Projectiles;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Ammo;
using DDmod.Content.Projectiles.Ranged.Bow;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Players;
using Terraria;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace DDmod.Content.Items
{
    public class RangedGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool IsCloneable => true;
        /// <summary>不消耗弹药概率(100%) </summary>
        public int NoConsumesAmmo;
        public bool Bow;
        /// <summary>转换前的箭</summary>
        public int[] PrePostConvertArrows;
        /// <summary>转换后的箭</summary>
        public int PostConvertArrows;
        /// <summary>转换概率(100%) </summary>
        public int ConvertProbability = 100;
        /// <summary>技能:0蓄力重击,1三发 </summary>
        public int Skill;
        /// <summary>高光 </summary>
        public bool Glow;
        /// <summary>常驻发光(没有发光贴图默认本体发光,有发光贴图但是禁用则蓄力才发光) </summary>
        public bool ResidentGlow;
        /// <summary>光效贴图大小(没有发光贴图默认本体发光,有发光贴图但是禁用则蓄力才发光) </summary>
        public float ScaleGlow = 1;
        /// <summary>光 </summary>
        public Vector3 BowLight;
        /// <summary>弦光 </summary>
        public bool BowstringGlow;
        /// <summary>弦上长度 </summary>
        public int StringUP=3;
        /// <summary>弦下长度 </summary>
        public int StringDown=3;
        /// <summary>弦偏移 </summary>
        public int StringOffset = 3;
        /// <summary>弦颜色 </summary>
        public Color StringColor;
        /// <summary>上下偏移 </summary>
        public int YOffset;
        /// <summary>距离偏移 </summary>
        public int Offset;
        /// <summary>三发箭的上距离 </summary>
        public float AboveArrowSpacing = 0.2f;
        /// <summary>三发箭的下距离 </summary>
        public float UnderArrowSpacing = 0.2f;

        /// <summary>
        /// 设置转换的箭
        /// </summary>
        /// <param name="PrePostConvertArrows">需要转换的箭</param>
        /// <param name="PostConvertArrows">转换后的箭</param>
        /// <param name="ConvertProbability">转换概率</param>
        public void SetArrows(int[] PrePostConvertArrows = null,int PostConvertArrows = 0,int ConvertProbability = 100)
        {
            this.PrePostConvertArrows = PrePostConvertArrows;
            this.PostConvertArrows = PostConvertArrows;
            this.ConvertProbability = ConvertProbability;
        }
        /// <summary>
        /// 设置弓的偏移和弦
        /// </summary>
        /// <param name="StringUP">弦上长度,越大越长</param>
        /// <param name="StringDown">弦下长度,越大越长</param>
        /// <param name="StringOffset">弦偏移,越大越往前</param>
        /// <param name="YOffset">弓位置调整</param>
        /// <param name="Offset">弓距离调整</param>
        /// <param name="BowstringGlow">弦是否常驻高亮</param>
        public void SetString(int StringUP = 3, int StringDown = 3, int StringOffset = 3, int YOffset = 0, int Offset = 0, bool BowstringGlow = false, Color StringColor = default)
        {
            this.StringUP = StringUP;
            this.StringDown = StringDown;
            this.StringOffset = StringOffset;
            this.YOffset = YOffset;
            this.Offset = Offset;
            this.BowstringGlow = BowstringGlow;
            this.StringColor = StringColor;
        }

        /// <summary>
        /// 设置弓的效果
        /// </summary>
        /// <param name="Skill">技能:0蓄力重击,1三发</param>
        /// <param name="ResidentGlow">常驻发光(没有发光贴图默认本体发光,有发光贴图但是禁用则蓄力才发光) </param>
        /// <param name="ScaleGlow">发光贴图大小</param>
        /// <param name="BowLight">弓发出的光</param>
        public void SetBow(int Skill=0, bool ResidentGlow = false, float ScaleGlow = 1,Vector3 BowLight = default)
        {
            this.Skill = Skill;
            this.ResidentGlow = ResidentGlow;
            this.ScaleGlow = ScaleGlow;
            this.BowLight = BowLight;
        }
        public override void SetDefaults(Item item)
        {
            //重写的弓
            if (item.type == 39 || item.type == 44 || item.type == 99 || item.type == 120 || item.type == 796 ||
                item.type == 655 || item.type == 658 || item.type == 661 || item.type == 682 || item.type == 725 ||
                item.type == 796 || item.type == 923 || item.type == 2223 || item.type == 2515 || item.type == 2747 ||
                item.type == 2888 || item.type == 3019 || item.type == 3029 || item.type == 3052 || item.type == 3480 ||
                item.type == 3486 || item.type == 3492 || item.type == 3498 || item.type == 3504 || item.type == 3510 ||
                item.type == 3516 || item.type == 3854 || item.type == 3859 || item.type == 4381 || item.type == 4953 ||
                item.type == 2624 || item.type == 3540 || item.type == 5282)
            {
                Bow = true;
            }
            //蜜蜂手雷
            if (item.type == 1130)
            {
                item.useAnimation = 60;
                item.useTime = 60;
            }
            //熔火之怒
            if (item.type == ItemID.MoltenFury)
            {
                PrePostConvertArrows = new int[] { 1, 2 };
                PostConvertArrows = 41;
                ResidentGlow = true;
                BowstringGlow = true;
                Skill = 1;
                BowLight = new Color(252, 102, 25).ToVector3() / 2;
                ScaleGlow = 4;
            }
            //恶魔弓
            if (item.type == 44)
            {
                PrePostConvertArrows = new int[] { 1 };
                PostConvertArrows = 4;
                ConvertProbability = 50;
                BowstringGlow = true;
                StringUP = 7;
                StringDown = 7;
                ScaleGlow = 4;
            }
            //地狱蝙蝠弓
            if (item.type == 3019)
            {
                Offset += 4;
                BowstringGlow = true;
                ResidentGlow = true;
            }
            //血雨弓
            if (item.type == 5282)
            {
                StringUP += 2;
            }
            //血雨弓
            if (item.type == 4381)
            {
                StringUP = 9;
                StringDown = 9;
                StringOffset += 4;
                YOffset += 2;
            }
            //蜜蜂弓
            if (item.type == 2888)
            {
                item.damage = (int)(item.damage * 0.75F);
                PrePostConvertArrows = new int[] { 1, 469 };
                PostConvertArrows = ModContent.ProjectileType<蜂巢箭>();
                Offset = 8;
                StringUP += 6;
                StringDown += 6;
            }
            //暗影弓
            if (item.type == 3052)
            {
                StringUP += 2;
                StringDown += 2;
                StringOffset += 2;
                BowstringGlow = true;
                StringColor = new Color(177, 120, 255);
            }
            //空灾
            if (item.type == 3859)
            {
                Offset = 6;
                PrePostConvertArrows = new int[] { 1, 2 };
                PostConvertArrows = 710;
            }
            //海啸
            if (item.type == 2624)
            {
                item.damage = (int)(item.damage * 0.75F);
                StringUP += 1;
                StringDown += 2;
                Offset = 4;
                StringOffset = 7;
            }
            //幻想弓
            if (item.type == 3540)
            {
                item.damage = 20;
                item.useAnimation = item.useTime = 20;
                StringUP += 2;
                StringDown += 2;
                Offset = 6;
                StringOffset = 9;
                ResidentGlow = true;
                BowstringGlow = true;
                Glow = true;
            }
            //骸骨弓
            if (item.type == 682)
            {
                PrePostConvertArrows = new int[] { 1 };
                PostConvertArrows = 117;
                StringUP += 2;
                StringDown += 2;
                Offset = 0;
                StringOffset += 4;
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.稀有;
            }
            //冰雪弓
            if (item.type == 725)
            {
                PrePostConvertArrows = new int[] { 1 };
                PostConvertArrows = 120;
                ResidentGlow = true;
                BowstringGlow = true;
                BowLight = new Color(37, 137, 187).ToVector3() / 2;
            }
            //血猩弓
            if (item.type == 796)
            {
                PrePostConvertArrows = new int[] { 1 };
                PostConvertArrows = ModContent.ProjectileType<VampiricArrow>();
                ConvertProbability = 50;
                StringColor = new Color(255, 40, 40);
                BowstringGlow = true;
                ScaleGlow = 4;
                StringUP += 2;
                StringDown += 2;
                StringOffset += 2;
                Offset += 4;
            }
            //暗影弓
            if (item.type == ItemID.ShadowFlameBow)
            {
                PostConvertArrows = 495;
                StringColor = new Color(81, 6, 233);
                Skill = 1;
                BowstringGlow = true;
                AboveArrowSpacing = 0.6f;
                UnderArrowSpacing = 0.6f;
                //ScaleGlow = 4;
            }
            //脉冲弓
            if (item.type == 2223)
            {
                item.damage = (int)(item.damage * 0.6F);
                StringUP += 9;
                StringDown += 9;
                StringOffset += 4;
                Offset += 4;
                PostConvertArrows = ModContent.ProjectileType<PulseArrows>();
            }
            //地狱蝙蝠弓
            if (item.type == ItemID.HellwingBow)
            {
                //item.damage /= 2;
                StringOffset -= 2;
            }
            //代达罗斯风暴弓
            if (item.type == ItemID.DaedalusStormbow)
            {
                item.damage = 14;
                StringUP = 5;
                StringDown = 5;
                StringOffset = 7;
            }
            //幽灵凤凰
            if (item.type == 3854)
            {
                item.damage /= 2;
                PrePostConvertArrows = new int[] { 1 };
                PostConvertArrows = 2;
                StringUP = 7;
                StringDown = 13;
                StringOffset = 7;
                YOffset = 10;
                Offset = 6;
            }
            //空中灾祸
            if (item.type == 3859)
            {
                StringUP = 3;
                StringDown = 3;
                StringOffset = 7;
                StringColor = new Color(255, 255, 220);
                ResidentGlow = true;
                BowstringGlow = true;
            }
            //日暮
            if (item.type == 4953)
            {
                item.damage = 20;
                item.useAnimation = item.useTime = 20;
                StringUP += -2;
                StringDown += -2;
                StringOffset = 5;
                Offset = 6;
                PrePostConvertArrows = new int[] { 932 };
                PostConvertArrows = 1;
                BowstringGlow = true;
                ResidentGlow = true;
            }
            //迷你鲨
            if (item.type == 98)
            {
                item.damage = 8;
                item.knockBack = 0.1F;
                item.value = Item.buyPrice(0, 3, 0, 0);
            }
            if (Bow)
            {
                item.useStyle = 13;
                item.noMelee = true;
                item.UseSound = null;
                item.autoReuse = true;
                item.channel = true;
                item.noUseGraphic = true;
                if (item.useAnimation <= 100)
                {
                    item.damage = (int)(item.damage * (1 + (float)(100 - item.useAnimation) / 100));
                }
                ModifyBow.Load(item.type, new ModifyBows(), false);
            }
            //连射
            if (item.useAmmo == AmmoID.Arrow || item.useAmmo == AmmoID.Bullet)
            {
                item.autoReuse = true;
            }
            //夺命枪
            if (item.type == 800)
            {
                item.useTime -= 4;
                item.useAnimation -= 4;
                item.autoReuse = true;
            }
            //星旋机枪
            if (item.type == 3475)
            {
                item.useStyle = ItemUseStyleID.Rapier;
            }
            //手枪
            if (item.type == 164)
            {
                item.useStyle = ItemUseStyleID.Rapier;
                item.noUseGraphic = true;
                item.UseSound = null;
                item.channel = true;
            }
            //手枪
            if (item.type == ItemID.Minishark)
            {
                item.useStyle = ItemUseStyleID.Rapier;
                item.noUseGraphic = true;
                item.UseSound = null;
                item.channel = true;
            }
            //火枪
            if (item.type == ItemID.Musket)
            {
                item.shoot = ModContent.ProjectileType<Musket>();
                item.useStyle = ItemUseStyleID.Rapier;
                item.noUseGraphic = true;
                item.UseSound = null;
                item.channel = true;
                item.autoReuse = true;
                item.useTurn = false;
                item.damage *= 2;
            }
            //橡果
            if (item.type == 27)
            {
                item.notAmmo = false;
                item.ammo = 27;
                item.shoot = ModContent.ProjectileType<远程枯萎橡果>();
                item.shootSpeed = 3;
            }
            //炮弹
            if (item.type == 929)
            {
                item.notAmmo = false;
                item.ammo = 929;
                item.shoot = 162;
                item.shootSpeed = 3;
            }
            //银子弹
            if (item.type == 278)
            {
                item.damage = 8;
            }
            //钨子弹
            if (item.type == 4915)
            {
                item.damage = 10;
            }
            if (item.type == 434)
            {
                item.useAnimation = item.useTime = 26;
                Gun(item, ModContent.ProjectileType<发条步枪>());
            }
            if (item.type == 95) Gun(item, ModContent.ProjectileType<燧发枪>());
            if (item.type == 219) Gun(item, ModContent.ProjectileType<凤凰爆破枪>());
            if (item.type == 1929)
            {
                item.useAnimation = item.useTime = 5;
                item.damage = 40;
                Gun(item, ModContent.ProjectileType<链式机枪>());
            }
            if (item.type == 964) Gun(item, ModContent.ProjectileType<三发猎枪>());
            if (item.type == 4703) Gun(item, ModContent.ProjectileType<四管霰弹枪>());
            if (item.type == 1553) Gun(item, ModContent.ProjectileType<太空海豚机枪>());
            if (item.type == 800) Gun(item, ModContent.ProjectileType<夺命枪>());
            if (item.type == 2269) Gun(item, ModContent.ProjectileType<左轮手枪>());
            if (item.type == 533) Gun(item, ModContent.ProjectileType<巨兽鲨>());
            if (item.type == 679) Gun(item, ModContent.ProjectileType<战术霰弹枪>());
            if (item.type == 1254) Gun(item, ModContent.ProjectileType<狙击步枪>());
            if (item.type == 3788) Gun(item, ModContent.ProjectileType<玛瑙爆破枪>());
            if (item.type == 1870) Gun(item, ModContent.ProjectileType<红莱德枪>());
            if (item.type == 1255) 
            {
                Gun(item, ModContent.ProjectileType<维纳斯万能枪>());
                item.useAnimation = item.useTime = 40;
                item.damage = 25;
            }
            if (item.type == 534) Gun(item, ModContent.ProjectileType<霰弹枪>());
            if (item.type == 2270) Gun(item, ModContent.ProjectileType<鳄鱼机关枪>());
            if (item.type == 1265)
            {
                item.useAnimation = item.useTime = 40;
                item.damage = 12;
                Gun(item, ModContent.ProjectileType<乌兹冲锋枪>());
            }
        }
        int GunProj;
        public void Gun(Item item,int GunProj=0)
        {
            item.useStyle = ItemUseStyleID.Rapier;
            item.noUseGraphic = true;
            item.UseSound = null;
            item.channel = true;
            item.autoReuse = true;
            item.useTurn = false;
            item.reuseDelay = 0;
            item.noMelee = true;
            item.shoot = GunProj;
            this.GunProj = GunProj;
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(Rota);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Rota = reader.ReadFloat();
        }
        public override void UseItemFrame(Item item, Player player)
        {
            player.PlayerAction().WeaponType = 0;
            player.PlayerAction().WeaponTypeTimes = 0;
            if (item.useStyle == 1&&!item.channel)
            {
                float v = MathHelper.Pi;
                if (player.direction < 0)
                {
                    v = -MathHelper.PiOver2;
                }
                player.PlayerAction().PlayerArmRotation(player.itemRotation - v + MathHelper.PiOver4, Player.CompositeArmStretchAmount.Full);
            }
            if (item.useStyle == ItemUseStyleID.Shoot && !item.channel)
            {
                if (item.noMelee && item.DamageType == DamageClass.Melee)
                {
                    //return;
                }

                if (item.type != 160)
                {
                    player.ChangeDir(player.Dplayer().MouseWorld.X - player.Center.X > 0 ? 1 : -1);
                    float v = 0;
                    if (player.direction < 0)
                    {
                        v = MathHelper.Pi;
                    }
                    player.itemRotation = (player.Dplayer().MouseWorld - (player.MountedCenter + ShootOffset)).ToRotation() * player.gravDir + v - player.fullRotation - Rota * player.direction;
                    player.PlayerAction().PlayerArmRotation(player.itemRotation - MathHelper.PiOver2 * player.direction, Player.CompositeArmStretchAmount.Full);
                }

            }
        }
        public override bool? CanChooseAmmo(Item weapon, Item ammo, Player player)
        {
            return base.CanChooseAmmo(weapon, ammo, player);
        }
        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
        {
            player.Dplayer().Ammo = ammo.type;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if(item.type == 929&&player.ActiveItem().type!=929)
            {
                item.shoot = ProjectileID.CannonballFriendly;
            }
            //橡果
            if (item.type == 27 && player.ActiveItem().type != 27)
            {
                item.shoot = ModContent.ProjectileType<远程枯萎橡果>();
            }
        }
        public override void HoldItem(Item item, Player player)
        {
            //橡果
            if (item.type == 27)
            {
                item.shoot = 0;
            }
            //炮弹
            if (item.type == 929)
            {
                item.shoot = 0;
            }
            IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(item, item.type);
            int type = ModContent.ProjectileType<GlobalBow>();
            if (item.type == ItemID.DaedalusStormbow)
            {
                type = ModContent.ProjectileType<Daedalus>();
            }
            if (Bow && player.heldProj == -1 && player.ownedProjectileCounts[type] == 0 && (Main.mouseItem.type == item.type || Main.mouseItem.type == 0))
            {
                NewProjectile(Source, player.Center, Vector2.One, type, (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(item.damage), (int)(player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(item.knockBack)), player.whoAmI);
            }
            if (Rota > MaxRota)
            {
                Rota -= MathHelper.PiOver4 / 10;
            }
            else
            {
                Rota = MaxRota;
            }
        }
        public float Rota;
        public float MaxRota;
        public Vector2 ShootOffset;
        public override bool? UseItem(Item item, Player player)
        {
            return base.UseItem(item, player);
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (Bow)
            {
                return false;
            }
            if (Reload)
            {
                //夺命枪
                if (item.type == 800)
                {
                    return false;
                }
            }
            return base.CanUseItem(item, player);
        }
        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
        }
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void PostReforge(Item item)
        {
        }
        public override bool CanShoot(Item item, Player player)
        {
            return base.CanShoot(item, player);
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (Bow)
            {
                type = 0;
            }
            position += ShootOffset;
            float V = velocity.Length();
            velocity = (Main.MouseWorld - position).PerfectNormalize() * V;

            //手枪
            if (item.type == ItemID.Handgun)
            {
                type = ModContent.ProjectileType<手枪2Proj>();
            }
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            if(item.type==1254)
            {
                return true;
            }
            if(item.DItem().Sniper)
            {
                return true;
            }
            return base.AltFunctionUse(item, player);
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
        {
            if (GunProj > 0&& player.itemTime == 0)
            {
                return false;
            }
            if (Main.rand.Next(100) < NoConsumesAmmo)
            {
                return false;
            }
            return base.CanConsumeAmmo(weapon, ammo, player);
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (GunProj > 0)
            {
                type = GunProj;
                NewProjectileChange(player.GetSource_FromAI(), position, velocity, type, damage, knockback, player.whoAmI, 0, 0);
                return false;
            }
            if (item.useAmmo == AmmoID.Bullet && !item.channel)
            {
                int GoreType = Mod.Find<ModGore>("弹药壳").Type;
                Gore.NewGore(player.GetSource_Death(), player.Center + (player.itemRotation).ToRotationVector2() * 12 * player.direction, (player.itemRotation).ToRotationVector2() * -4 * player.direction, GoreType, 1.3f);
                if (Bullets <= 1)
                {
                    Reload = true;
                }
                Bullets--;
            }

            //火枪
            if (item.type == ItemID.Musket)
            {
                Projectile projectile = Main.projectile[NewProjectile(source, position, velocity, ModContent.ProjectileType<Musket>(), damage, knockback, player.whoAmI, item.useAnimation, 0f)];
                projectile.GetGlobalProjectile<RangedProjectile>().Bullet = type;
                return false;
            }
            if (item.type == ItemID.Handgun)
            {
                NewProjectile(source, position, velocity, ModContent.ProjectileType<手枪2Proj>(), damage, knockback, player.whoAmI, 0, 1f);
                return false;
            }
            if (item.type == ItemID.Minishark)
            {
                NewProjectile(source, position, velocity, ModContent.ProjectileType<迷你鲨2Proj>(), damage, knockback, player.whoAmI, 0, 1f);
                return false;
            }
            if (Bow)
            {
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
        public int Bullets = 10;
        public bool Reload;
        public int ReloadTime;
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.player[item.playerIndexTheItemIsReservedFor];
            //夺命枪
            if (item.type == 800 && player.inventory[player.selectedItem] == item)
            {
                if (Reload)
                {
                    ReloadTime++;
                    if (ReloadTime > 5)
                    {
                        ReloadTime = 0;
                        Bullets++;
                    }
                    if (Bullets >= 8)
                    {
                        Bullets = 8;
                        Reload = false;
                    }
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                for (int A = 0; A < Bullets; A++)
                {
                   
                    Color color = Color.White;
                    if (A == 0)
                    {
                        color = Color.Red;
                    }
                    int List = A / 20;
                    int List2 = A % 20;
                    Vector2 vector = player.Center - Main.screenPosition;
                    vector.Y -= player.height + 6 * List;
                    vector.X += 3 * List2 - 30;
                    spriteBatch.Draw(DDTextures.Bullet.Value, vector, null, color, 0, DDTextures.Bullet.Size() / 2, 0.5F, 0, 0);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Bow&&item.type != ItemID.DaedalusStormbow)
            {
                R += 1f;
                tooltips.Add(new TooltipLine(Mod, "蓄力武器", Language.GetTextValue("Mods.DDmod.Tooltips.RightCharge"))
                {
                    OverrideColor = new Color(255, 255, 255)
                });
            }
            if(NoConsumesAmmo>0)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria")
                    {
                        if (line.Name == "Tooltip0")
                        {
                            if (line.Text == "")
                            {
                                line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.ConsumesAmmo", NoConsumesAmmo);
                            }
                            else
                            {
                                line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.ConsumesAmmo", NoConsumesAmmo) + "\n" + line.Text;
                            }
                        }
                    }
                }
            }
        }
        public static float R;
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {

            if (line.Name == "蓄力武器" && line.Mod == "DDmod")
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
                R += 3f;
                GameShaders.Misc["渲染滤镜"].UseOpacity(2);
                GameShaders.Misc["渲染滤镜"].SetShaderTexture(ModContent.Request<Texture2D>("DDmod/Image/heatmap"));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uImageSize1"].SetValue(ModContent.Request<Texture2D>("DDmod/Image/heatmap").Size() * 5);
                GameShaders.Misc["渲染滤镜"].UseColor(Color.White);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uColor2"].SetValue(Color.White.ToVector3() * 0.75f);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["renderTargetArea"].SetValue(new Vector2(5, 5));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uWorldPosition"].SetValue(Vector2.Zero);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["position"].SetValue(new Vector2(R, 0));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["ImageSize"].SetValue(ModContent.Request<Texture2D>("DDmod/Image/heatmap").Size() * 5);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["upscaleFactor"].SetValue(new Vector2(-0.7F));
                GameShaders.Misc["渲染滤镜"].Apply();

                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), Color.White, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
                //return false;
            }
            return true;
        }
    }
}