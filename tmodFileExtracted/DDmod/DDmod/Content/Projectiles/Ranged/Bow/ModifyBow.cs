using DDmod.Content.Dusts;
using DDmod.Content.Items;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Content.Projectiles;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Players;
using System.Reflection.Emit;
using Terraria.Graphics.Shaders;
using Terraria.ID;
namespace DDmod
{
    public static class ModifyBowLoader
    {
        public static List<ModifyBow> bow = new List<ModifyBow>();
        public static bool SetArrows(int type, Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            int R = -1;
            for (int A = 0; A < bow.Count; A++)
            {
                if (bow[A].type == 0)
                {
                    R = A;
                }
                if (bow[A].type == type)
                {
                    return bow[A].SetArrows(item, Projectile, rangedItem, ranged);
                }
            }
            if (R != -1)
            {
                return bow[R].SetArrows(item, Projectile, rangedItem, ranged);
            }
            return false;
        }
        public static void PreModifyArrow(int type, Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged, ref float speed, ref int damage, ref float knockBack, ref int usedAmmoItemId, ref int projToShoot)
        {
            int R = -1;
            for (int A = 0; A < bow.Count; A++)
            {
                if (bow[A].type == 0)
                {
                    R = A;
                }
                if (bow[A].type == type)
                {
                    bow[A].PreModifyArrow(item, Projectile, rangedItem, ranged, ref speed, ref damage, ref knockBack, ref usedAmmoItemId, ref projToShoot);
                    return;
                }
            }
            if (R != -1) bow[R].PreModifyArrow(item, Projectile, rangedItem, ranged, ref speed, ref damage, ref knockBack, ref usedAmmoItemId, ref projToShoot);
        }
        public static void PostModifyArrow(int type, Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged, ref float speed, ref int damage, ref float knockBack, ref int usedAmmoItemId, ref int projToShoot, ref float KnockBack)
        {
            int R = -1;
            for (int A = 0; A < bow.Count; A++)
            {
                if (bow[A].type == 0)
                {
                    R = A;
                }
                if (bow[A].type == type)
                {
                    bow[A].PostModifyArrow(item, Projectile, rangedItem, ranged, ref speed, ref damage, ref knockBack, ref usedAmmoItemId, ref projToShoot, ref KnockBack);
                    return;
                }
            }
            if (R != -1) bow[R].PostModifyArrow(item, Projectile, rangedItem, ranged, ref speed, ref damage, ref knockBack, ref usedAmmoItemId, ref projToShoot, ref KnockBack);
        }
        public static void ChargeUpdate(int type, Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            int R = -1;
            for (int A = 0; A < bow.Count; A++)
            {
                if (bow[A].type == 0)
                {
                    R = A;
                }
                if (bow[A].type == type)
                {
                    bow[A].ChargeUpdate(item, Projectile, rangedItem, ranged);
                    return;
                }
            }
            if (R != -1) bow[R].ChargeUpdate(item, Projectile, rangedItem, ranged);
        }
        public static void NoChargeUpdate(int type, Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            int R = -1;
            for (int A = 0; A < bow.Count; A++)
            {
                if (bow[A].type == 0)
                {
                    R = A;
                }
                if (bow[A].type == type)
                {
                    bow[A].NoChargeUpdate(item, Projectile, rangedItem, ranged);
                    return;
                }
            }
            if (R != -1) bow[R].NoChargeUpdate(item, Projectile, rangedItem, ranged);
        }
        public static void Skill(int type, Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            int R = -1;
            for (int A = 0; A < bow.Count; A++)
            {
                if (bow[A].type == 0)
                {
                    R = A;
                }
                if (bow[A].type == type)
                {
                    bow[A].Skill(item, Projectile, rangedItem, ranged);
                    return;
                }
            }
            if (R != -1) bow[R].Skill(item, Projectile, rangedItem, ranged);
        }
        public static void Shoot(int type, Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            int R = -1;
            for (int A = 0; A < bow.Count; A++)
            {
                if (bow[A].type == 0)
                {
                    R = A;
                }
                if (bow[A].type == type)
                {
                    bow[A].Shoot(item, Projectile, rangedItem, ranged);
                    return;
                }
            }
            if (R != -1) bow[R].Shoot(item, Projectile, rangedItem, ranged);
        }
        public static void PostUpdate(int type, Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            int R = -1;
            for (int A = 0; A < bow.Count; A++)
            {
                if (bow[A].type == 0)
                {
                    R = A;
                }
                if (bow[A].type == type)
                {
                    bow[A].PostUpdate(item, Projectile, rangedItem, ranged);
                    return;
                }
            }
            if (R != -1)
                bow[R].PostUpdate(item, Projectile, rangedItem, ranged);
        }
    }
    public class ModifyBows : ModifyBow
    {
    }
    public abstract class ModifyBow
    {
        /// <summary>
        /// 加载弓
        /// </summary>
        public virtual void Load()
        {
        }
        public virtual void Unload()
        {

        }
        /// <summary>
        /// 初始化弓效果
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="modify"></param>
        /// <param name="Override"></param>
        public static void Load(int Type, ModifyBow modify, bool Override = false)
        {
            modify.type = Type;
            bool O = false;
            for (int A = 0; A < ModifyBowLoader.bow.Count; A++)
            {
                if (ModifyBowLoader.bow[A].type == Type)
                {
                    O = true;
                    if (Override)
                    {
                        ModifyBowLoader.bow[A] = modify;
                        return;
                    }
                }
            }
            if (!O)
            {
                ModifyBowLoader.bow.Add(modify);
            }
        }
        public int type = -1;
        /// <summary>
        /// 设置箭
        /// </summary>
        public virtual bool SetArrows(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            return true;
        }
        /// <summary>
        /// 修改箭矢
        /// </summary>
        public virtual void PreModifyArrow(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged, ref float speed, ref int damage, ref float knockBack, ref int usedAmmoItemId, ref int projToShoot)
        {
            //会修改箭的弓
            if (rangedItem.PostConvertArrows > 0 && Main.rand.Next(100) < rangedItem.ConvertProbability)
            {
                if (rangedItem.PrePostConvertArrows != null)
                {
                    for (int a = 0; a < rangedItem.PrePostConvertArrows.Length; a++)
                    {
                        if (projToShoot == rangedItem.PrePostConvertArrows[a])
                        {
                            projToShoot = rangedItem.PostConvertArrows;
                        }
                    }
                }
                else
                {
                    projToShoot = rangedItem.PostConvertArrows;
                }
            }
        }
        /// <summary>
        ///修改箭后
        /// </summary>
        public virtual void PostModifyArrow(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged, ref float speed, ref int damage, ref float knockBack, ref int usedAmmoItemId, ref int projToShoot, ref float KnockBack)
        {

        }
        /// <summary>
        ///蓄力时更新
        /// </summary>
        public virtual void ChargeUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            Player player = Projectile.Player();
            if (item.type == 2223)
            {
                ranged.BowTime -= player.GetTotalAttackSpeed(DamageClass.Ranged) * 0.75f * 0.5f;
            }
            if (item.type == 3854)
            {
                ranged.BowTime -= player.GetTotalAttackSpeed(DamageClass.Ranged) * 0.25f;
            }
            if (item.type == 3540)
            {
                ranged.BowTime += player.GetTotalAttackSpeed(DamageClass.Ranged);
            }
            if (item.type == 4953)
            {
                ranged.BowTime -= player.GetTotalAttackSpeed(DamageClass.Ranged) * 0.35f;
            }
            if (item.type == 3859)
            {
                ranged.BowTime -= player.GetTotalAttackSpeed(DamageClass.Ranged) * 0.35f;
            }
            //脉冲弓
            if (item.type == ItemID.PulseBow)
            {
                for (int a = 0; a < 3; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.PreviousCenter() + Projectile.velocity.PerfectNormalize() * (ranged.BowTime / Projectile.Player().itemAnimationMax * 6) + new Vector2(Main.rand.NextFloat(34, 54)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, 226, 0, 0, 0, default, 1f)];
                    dust.noGravity = true;
                    dust.scale = 0.7F;
                    dust.velocity = (Projectile.Center - dust.position) / Main.rand.NextFloat(6, 10);
                    GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;

                }
            }
        }
        float SP;
        /// <summary>
        /// 射箭但不是蓄力时更新
        /// </summary>  
        public virtual void NoChargeUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            Player player = Projectile.Player();
            if (item.type == 3540)
            {
                if (SP < 5)
                {
                    SP += 0.01F;
                }
                ranged.BowTime += player.GetTotalAttackSpeed(DamageClass.Ranged) * SP;
            }
            if (item.type == ModContent.ItemType<ShadowFury>())
            {
                if (ranged.Ammo[1] == 0)
                {
                    player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId);
                    //会修改箭的弓
                    if (rangedItem.PostConvertArrows > 0 && Main.rand.Next(100) < rangedItem.ConvertProbability)
                    {
                        if (rangedItem.PrePostConvertArrows != null)
                        {
                            for (int a = 0; a < rangedItem.PrePostConvertArrows.Length; a++)
                            {
                                if (projToShoot == rangedItem.PrePostConvertArrows[a])
                                {
                                    projToShoot = rangedItem.PostConvertArrows;
                                }
                            }
                        }
                        else
                        {
                            projToShoot = rangedItem.PostConvertArrows;
                        }
                    }
                    ranged.BowSpeed[1] = speed * Main.rand.NextFloat(0.6F, 1);
                    ranged.BowDamage[1] = damage;
                    ranged.BowKnockBack[1] = player.GetWeaponKnockback(player.inventory[player.selectedItem], knockBack);
                    ranged.BowUsedAmmoItemId[1] = usedAmmoItemId;
                    ranged.Ammo[1] = projToShoot;
                    rangedItem.AboveArrowSpacing = Main.rand.NextFloat(0, 0.15F);
                }
                if (ranged.Ammo[2] == 0)
                {
                    player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId);
                    //会修改箭的弓
                    if (rangedItem.PostConvertArrows > 0 && Main.rand.Next(100) < rangedItem.ConvertProbability)
                    {
                        if (rangedItem.PrePostConvertArrows != null)
                        {
                            for (int a = 0; a < rangedItem.PrePostConvertArrows.Length; a++)
                            {
                                if (projToShoot == rangedItem.PrePostConvertArrows[a])
                                {
                                    projToShoot = rangedItem.PostConvertArrows;
                                }
                            }
                        }
                        else
                        {
                            projToShoot = rangedItem.PostConvertArrows;
                        }
                    }
                    ranged.BowSpeed[2] = speed * Main.rand.NextFloat(0.6F, 1);
                    ranged.BowDamage[2] = damage;
                    ranged.BowKnockBack[2] = player.GetWeaponKnockback(player.inventory[player.selectedItem], knockBack);
                    ranged.BowUsedAmmoItemId[2] = usedAmmoItemId;
                    ranged.Ammo[2] = projToShoot;
                    rangedItem.UnderArrowSpacing = Main.rand.NextFloat(0, 0.15F);
                }
            }
            if (item.type == 3854)
            {
                if (ranged.Ammo[1] == 0)
                {
                    player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId);
                    //会修改箭的弓
                    if (rangedItem.PostConvertArrows > 0 && Main.rand.Next(100) < rangedItem.ConvertProbability)
                    {
                        if (rangedItem.PrePostConvertArrows != null)
                        {
                            for (int a = 0; a < rangedItem.PrePostConvertArrows.Length; a++)
                            {
                                if (projToShoot == rangedItem.PrePostConvertArrows[a])
                                {
                                    projToShoot = rangedItem.PostConvertArrows;
                                }
                            }
                        }
                        else
                        {
                            projToShoot = rangedItem.PostConvertArrows;
                        }
                    }
                    ranged.BowSpeed[1] = speed * Main.rand.NextFloat(0.4F, 0.8f);
                    ranged.BowDamage[1] = damage;
                    ranged.BowKnockBack[1] = player.GetWeaponKnockback(player.inventory[player.selectedItem], knockBack);
                    ranged.BowUsedAmmoItemId[1] = usedAmmoItemId;
                    ranged.Ammo[1] = projToShoot;
                    rangedItem.AboveArrowSpacing = Main.rand.NextFloat(-0.1f, 0.1f);
                }
                if (ranged.Ammo[2] == 0)
                {
                    player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId);
                    //会修改箭的弓
                    if (rangedItem.PostConvertArrows > 0 && Main.rand.Next(100) < rangedItem.ConvertProbability)
                    {
                        if (rangedItem.PrePostConvertArrows != null)
                        {
                            for (int a = 0; a < rangedItem.PrePostConvertArrows.Length; a++)
                            {
                                if (projToShoot == rangedItem.PrePostConvertArrows[a])
                                {
                                    projToShoot = rangedItem.PostConvertArrows;
                                }
                            }
                        }
                        else
                        {
                            projToShoot = rangedItem.PostConvertArrows;
                        }
                    }
                    ranged.BowSpeed[2] = speed * Main.rand.NextFloat(0.6F, 1);
                    ranged.BowDamage[2] = damage;
                    ranged.BowKnockBack[2] = player.GetWeaponKnockback(player.inventory[player.selectedItem], knockBack);
                    ranged.BowUsedAmmoItemId[2] = usedAmmoItemId;
                    ranged.Ammo[2] = projToShoot;
                    rangedItem.UnderArrowSpacing = -rangedItem.AboveArrowSpacing + Main.rand.NextFloat(-0.05F, 0.05F);
                }
            }
        }
        /// <summary>
        /// 蓄力完成后技能
        /// </summary>  
        public virtual void Skill(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            Player player = Projectile.Player();
            //技能1
            if (rangedItem.Skill == 0)
            {
                if (ranged.BowTime < player.itemAnimationMax * 2)
                {
                    //脉冲弓
                    if (item.type == 2223)
                    {
                        for (int a = 0; a < 3; a++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.PreviousCenter() + Projectile.velocity.PerfectNormalize() * (ranged.BowTime / Projectile.Player().itemAnimationMax * 6) + new Vector2(Main.rand.NextFloat(34, 54)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, 226, 0, 0, 0, default, 1f)];
                            dust.noGravity = true;
                            dust.scale = 0.7F;
                            dust.velocity = (Projectile.Center - dust.position) / Main.rand.NextFloat(6, 10);
                            GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;
                        }
                    }
                    //地狱蝙蝠弓
                    if (item.type == ItemID.HellwingBow)
                    {
                        int V = 0;
                        for (int A = 0; A < 1000; A++)
                        {
                            Projectile projectile = Main.projectile[A];
                            if (projectile.active && projectile.type == ModContent.ProjectileType<HellBats>() && projectile.owner == Projectile.owner && projectile.ai[0] == 0)
                            {
                                V++;
                            }
                        }
                        if (ranged.BowTime < player.itemAnimationMax * (1F))
                        {
                            if (V <= 0)
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 6, ModContent.ProjectileType<HellBats>(), Projectile.damage / 4, ranged.BowKnockBack[0], Projectile.owner, 0, 0);
                        }
                        else if (ranged.BowTime < player.itemAnimationMax * (1.15F))
                        {
                            if (V <= 1)
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 6, ModContent.ProjectileType<HellBats>(), Projectile.damage / 4, ranged.BowKnockBack[0], Projectile.owner, 0, 0);
                        }
                        else if (ranged.BowTime < player.itemAnimationMax * (1.3F))
                        {
                            if (V <= 2)
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 6, ModContent.ProjectileType<HellBats>(), Projectile.damage / 4, ranged.BowKnockBack[0], Projectile.owner, 0, 0);
                        }
                        else if (ranged.BowTime < player.itemAnimationMax * (1.45F))
                        {
                            if (V <= 3)
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 6, ModContent.ProjectileType<HellBats>(), Projectile.damage / 4, ranged.BowKnockBack[0], Projectile.owner, 0, 0);
                        }
                        else if (ranged.BowTime < player.itemAnimationMax * (1.6F))
                        {
                            if (V <= 4)
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 6, ModContent.ProjectileType<HellBats>(), Projectile.damage / 4, ranged.BowKnockBack[0], Projectile.owner, 0, 0);
                        }
                        else if (ranged.BowTime < player.itemAnimationMax * (1.75F))
                        {
                            if (V <= 5)
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 6, ModContent.ProjectileType<HellBats>(), Projectile.damage / 4, ranged.BowKnockBack[0], Projectile.owner, 0, 0);
                        }
                        else if (ranged.BowTime < player.itemAnimationMax * (1.9F))
                        {
                            if (V <= 6)
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 6, ModContent.ProjectileType<HellBats>(), Projectile.damage / 4, ranged.BowKnockBack[0], Projectile.owner, 0, 0);
                        }
                    }
                }
                if (ranged.BowTime > player.itemAnimationMax * 1.8f)
                {
                    ranged.SpecialEffect = true;
                    //恶魔弓
                    if (item.type == 44 && ranged.Ammo[0] > 0 && ranged.Ammo[0] != ModContent.ProjectileType<TrueUnholyArrow>())
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 14)];
                            dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8F, 2.2F);
                            dust.noGravity = true;
                            dust.alpha = 100;
                            dust.scale = 1.8f;
                        }
                        ranged.Ammo[0] = ModContent.ProjectileType<TrueUnholyArrow>();
                        ranged.BowDamage[0] = (int)(ranged.BowDamage[0] * 0.8F);
                    }
                    //冰雪弓
                    if (item.type == 725 && ranged.Ammo[0] > 0 && ranged.Ammo[0] != ModContent.ProjectileType<TrueFrostArrows>())
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 135)];
                            dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8F, 2.2F);
                            dust.noGravity = true;
                            dust.alpha = 100;
                            dust.scale = 1.8f;
                        }
                        ranged.Ammo[0] = ModContent.ProjectileType<TrueFrostArrows>();
                        ranged.BowSpeed[0] *= 0.8F;
                        ranged.BowDamage[0] = (int)(ranged.BowDamage[0] * 0.5F);
                    }
                    //猩红弓
                    if (item.type == 796 && ranged.Ammo[0] > 0 && ranged.Ammo[0] != ModContent.ProjectileType<TrueVampiricArrow>())
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 235)];
                            dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8F, 2.2F);
                            dust.noGravity = true;
                            dust.alpha = 100;
                            dust.scale = 1.8f;
                        }
                        ranged.Ammo[0] = ModContent.ProjectileType<TrueVampiricArrow>();
                        ranged.BowSpeed[0] *= 0.4F;
                        ranged.BowDamage[0] = (int)(ranged.BowDamage[0] * 0.6F);
                    }
                    //海啸
                    if (item.type == 2624 && ranged.Ammo[0] > 0 && ranged.Ammo[0] != ModContent.ProjectileType<水箭>())
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 33)];
                            dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8F, 2.2F);
                            dust.noGravity = true;
                            dust.alpha = 100;
                            dust.scale = 1.8f;
                        }
                        ranged.Ammo[0] = ModContent.ProjectileType<水箭>();
                        ranged.BowSpeed[0] *= 0.6F;
                        ranged.BowDamage[0] = (int)(ranged.BowDamage[0] * 3F);
                    }
                }
            }
            else if (rangedItem.Skill == 1)
            {
                if (Projectile.DProj().Times[2] <= player.itemAnimationMax)
                {
                    Projectile.DProj().Times[2] += player.GetTotalAttackSpeed(DamageClass.Ranged) * 0.75f;
                }
                if (Projectile.DProj().Times[2] > player.itemAnimationMax * 0.8F)
                {
                    ranged.SpecialEffect = true;
                    //技能2
                    if (ranged.Ammo[1] == 0)
                    {
                        player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId);
                        ranged.BowSpeed[1] = speed;
                        ranged.BowDamage[1] = damage;
                        ranged.BowKnockBack[1] = player.GetWeaponKnockback(player.inventory[player.selectedItem], knockBack);
                        ranged.BowUsedAmmoItemId[1] = usedAmmoItemId;
                        ranged.Ammo[1] = projToShoot;
                        //会修改箭的弓
                        if (rangedItem.PostConvertArrows > 0 && Main.rand.Next(100) < rangedItem.ConvertProbability)
                        {
                            if (rangedItem.PrePostConvertArrows != null)
                            {
                                for (int a = 0; a < rangedItem.PrePostConvertArrows.Length; a++)
                                {
                                    if (ranged.Ammo[1] == rangedItem.PrePostConvertArrows[a])
                                    {
                                        ranged.Ammo[1] = rangedItem.PostConvertArrows;
                                    }
                                }
                            }
                            else
                            {
                                ranged.Ammo[1] = rangedItem.PostConvertArrows;
                            }
                        }
                    }
                    if (ranged.Ammo[2] == 0)
                    {
                        player.PickAmmo(player.inventory[player.selectedItem], out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId);
                        ranged.BowSpeed[2] = speed;
                        ranged.BowDamage[2] = damage;
                        ranged.BowKnockBack[2] = player.GetWeaponKnockback(player.inventory[player.selectedItem], knockBack);
                        ranged.BowUsedAmmoItemId[2] = usedAmmoItemId;
                        ranged.Ammo[2] = projToShoot;
                        //会修改箭的弓
                        if (rangedItem.PostConvertArrows > 0 && Main.rand.Next(100) < rangedItem.ConvertProbability)
                        {
                            if (rangedItem.PrePostConvertArrows != null)
                            {
                                for (int a = 0; a < rangedItem.PrePostConvertArrows.Length; a++)
                                {
                                    if (ranged.Ammo[2] == rangedItem.PrePostConvertArrows[a])
                                    {
                                        ranged.Ammo[2] = rangedItem.PostConvertArrows;
                                    }
                                }
                            }
                            else
                            {
                                ranged.Ammo[2] = rangedItem.PostConvertArrows;
                            }
                        }
                    }
                    //暗影弓
                    if (item.type == 3052 && ranged.Ammo[0] != ModContent.ProjectileType<TrueShadowArrows>())
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 27)];
                            dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.8F, 2.2F);
                            dust.noGravity = true;
                            dust.alpha = 100;
                            dust.scale = 1.8f;
                        }
                        if (ranged.Ammo[0] > 0)
                            ranged.Ammo[0] = ModContent.ProjectileType<TrueShadowArrows>();
                        if (ranged.Ammo[1] > 0)
                            ranged.Ammo[1] = ModContent.ProjectileType<TrueShadowArrows>();
                        if (ranged.Ammo[2] > 0)
                            ranged.Ammo[2] = ModContent.ProjectileType<TrueShadowArrows>();

                        ranged.BowDamage[0] = (int)(ranged.BowDamage[0] * 0.75F);
                        ranged.BowDamage[1] = (int)(ranged.BowDamage[1] * 0.4F);
                        ranged.BowDamage[2] = (int)(ranged.BowDamage[2] * 0.4F);
                    }
                }
            }
        }
        /// <summary>
        /// 🐍
        /// </summary>
        public virtual void Shoot(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            Player player = Projectile.Player();
            IEntitySource Source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ranged.BowUsedAmmoItemId[0]);
            IEntitySource Source2 = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ranged.BowUsedAmmoItemId[1]);
            IEntitySource Source3 = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ranged.BowUsedAmmoItemId[2]);
            float la = ranged.BowTime / player.itemAnimationMax * 6;
            float Charge = ranged.BowTime / player.itemAnimationMax;
            if (Charge > 1 && rangedItem.Skill == 1) Charge = 1;
            //地狱蝙蝠弓
            if (Charge > 1 && item.type == 3019)
            {
                Charge = 1;
            }
            if (Charge > 1 && rangedItem.Skill == 0) Charge *= 1.25f;
            float Magnification = Charge;
            //恶魔弓
            if (item.type == 44 && ranged.Ammo[0] == ModContent.ProjectileType<TrueUnholyArrow>())
            {
                Magnification *= 0.8F;
            }
            //冰雪弓
            if (item.type == 725 && ranged.Ammo[0] == ModContent.ProjectileType<TrueFrostArrows>())
            {
                Magnification *= 0.5f;
            }
            //橡果弓
            if (item.type == ModContent.ItemType<AcornBow>() && ranged.Ammo[0] == ModContent.ProjectileType<NaturalAcornArrows>())
            {
                Magnification = 1.8f;
            }
            //猩红弓
            if (item.type == 796 && ranged.Ammo[0] == ModContent.ProjectileType<TrueVampiricArrow>())
            {
                Magnification = 0.6f;
            }
            //暗影弓
            if (item.type == 3052 && ranged.Ammo[0] == ModContent.ProjectileType<TrueShadowArrows>())
            {
                Magnification *= 0.4f;
            }
            //脉冲弓
            if (item.type == 2223)
            {
                Magnification *= 5f;
            }
            //海啸
            if (item.type == 2624)
            {
                Magnification *= 2f;
            }
            if (Projectile.owner == Main.myPlayer && ranged.Ammo[0] > 0)
            {
                //空中灾祸
                if (item.type == 3859)
                {
                    if (ranged.BowTime > player.itemAnimationMax * 1.8f)
                    {
                        SoundStyle sound = SoundID.DD2_BetsyScream;
                        sound.Pitch = 0.5F;
                        PlaySound(sound, Projectile.position);
                        ranged.Ammo[0] = ModContent.ProjectileType<火龙波>();
                        for (int a = -1; a <= 1; a++)
                        {
                            int Proj = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize().RotatedBy(a * 0.4f) * ranged.BowSpeed[0] * Charge / 10, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                            Main.projectile[Proj].scale = Projectile.scale;
                            Main.projectile[Proj].Resize((int)(Main.projectile[Proj].OriginalWidth() * (Main.projectile[Proj].scale)), (int)(Main.projectile[Proj].OriginalHeight() * (Main.projectile[Proj].scale)));
                        }
                        return;
                    }
                }
                //幽灵凤凰
                if (item.type == 3854)
                {
                    if (ranged.BowTime > player.itemAnimationMax * 1.8f)
                    {
                        ranged.BowSpeed[0] /= 2;
                        Charge /= 2;
                        Magnification /= 2;
                        for (int a = -1; a <= 1; a += 2)
                        {
                            int Proj = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, 706, (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                            Main.projectile[Proj].scale = Projectile.scale;
                            Main.projectile[Proj].Resize((int)(Main.projectile[Proj].OriginalWidth() * (Main.projectile[Proj].scale)), (int)(Main.projectile[Proj].OriginalHeight() * (Main.projectile[Proj].scale)));
                            Main.projectile[Proj].DProj().Bool[0] = true;
                            Main.projectile[Proj].DProj().Times[2] = a;
                            Main.projectile[Proj].netUpdate = true; ;
                        }
                        ranged.Ammo[0] = 706;
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(-10, 24 * player.direction).RotatedBy(Projectile.rotation), Projectile.velocity * 15, ModContent.ProjectileType<火束箭>(), Projectile.damage * 2, 0);
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(-10, 24 * player.direction).RotatedBy(Projectile.rotation), Projectile.velocity * 10, ModContent.ProjectileType<火束箭>(), Projectile.damage * 2, 0);
                    }
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(-10, 24 * player.direction).RotatedBy(Projectile.rotation), Projectile.velocity * 20, ModContent.ProjectileType<火束箭>(), Projectile.damage * 2, 0);
                }
                //血雨弓
                if (item.type == 4381)
                {
                    for (int a = 0; a < 6; a++)
                    {
                        NewProjectile(Projectile.GetSource_FromAI(), new Vector2(player.Dplayer().MouseWorld.X + Main.rand.Next(-100, 100), player.MountedCenter.Y - 800), new Vector2(Main.rand.NextFloat(-2, 2), 3 * Charge), ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), 0);
                    }
                }
                //日暮
                if (item.type == 4953 && ranged.Ammo[0] == 1)
                {
                    ranged.Ammo[0] = 932;
                    ranged.BowSpeed[0] *= 2;
                }
                //发射
                int Proj1 = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                Main.projectile[Proj1].scale = Projectile.scale;
                Main.projectile[Proj1].Resize((int)(Main.projectile[Proj1].OriginalWidth() * (Main.projectile[Proj1].scale)), (int)(Main.projectile[Proj1].OriginalHeight() * (Main.projectile[Proj1].scale)));
                //空中灾祸
                if (item.type == 3859)
                {
                    for (float a = 0.95F; a > 0.75F; a -= 0.05F)
                    {
                        int Proj = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize() * ranged.BowSpeed[0] * a * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                        Main.projectile[Proj].scale = Projectile.scale;
                        Main.projectile[Proj].Resize((int)(Main.projectile[Proj1].OriginalWidth() * (Main.projectile[Proj1].scale)), (int)(Main.projectile[Proj1].OriginalHeight() * (Main.projectile[Proj1].scale)));
                    }
                }
                //海啸
                if (item.type == 2624)
                {
                    if (ranged.BowTime > player.itemAnimationMax * 1.8f)
                    {
                        return;
                    }
                    for (float a = 0; a < 5; a++)
                    {
                        if (a - 2 != 0)
                        {
                            int Proj = NewProjectileChange(Source, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize().RotatedBy((a - 2) * 0.02F) * ranged.BowSpeed[0] * (100 - Math.Abs(a - 2)) / 100 * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge), ranged.BowKnockBack[0] * Charge, Projectile.owner, 0, 0, Magnification);
                            Main.projectile[Proj].scale = Projectile.scale;
                            Main.projectile[Proj].Resize((int)(Main.projectile[Proj1].OriginalWidth() * (Main.projectile[Proj1].scale)), (int)(Main.projectile[Proj1].OriginalHeight() * (Main.projectile[Proj1].scale)));
                        }
                    }
                }
                if (ranged.BowTime > player.itemAnimationMax * 1.8f)
                {
                    //骸骨弓
                    if (Main.projectile[Proj1].type == 117)
                    {
                        Main.projectile[Proj1].DProj().Bool[0] = true;
                    }
                    //脉冲弓
                    if (item.type == 2223)
                    {
                        Main.projectile[Proj1].ai[0] = 1;
                    }
                }
                //地狱蝙蝠弓
                if (item.type == 3019 && ranged.Ammo[0] == 485)
                {
                    Main.projectile[Proj1].ai[0] = Projectile.velocity.PerfectNormalize().X * ranged.BowSpeed[0] * Charge;
                    Main.projectile[Proj1].ai[1] = Projectile.velocity.PerfectNormalize().Y * ranged.BowSpeed[0] * Charge;
                    Main.projectile[Proj1].netUpdate = true;
                }
                //日暮
                if (item.type == 4953)
                {
                    Main.projectile[Proj1].ai[1] = Main.rand.NextFloat(1);
                    for (int i = 0; i < 50; i++)
                    {
                        Color fairyQueenWeaponsColor = new Color(Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255));
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 267)];
                        dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0, 6.2F);
                        dust.velocity.Y /= 2;
                        dust.velocity.RotatedBy(Projectile.rotation);
                        dust.noGravity = true;
                        dust.scale = 1.2f;
                        dust.color = fairyQueenWeaponsColor;
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        Color fairyQueenWeaponsColor = new Color(Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255));
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 267)];
                        dust.velocity = -Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(0, 8.2F);
                        dust.velocity.RotatedBy(Projectile.rotation);
                        dust.noGravity = true;
                        dust.scale = 1.2f;
                        dust.color = fairyQueenWeaponsColor;
                    }
                    if (ranged.BowTime > player.itemAnimationMax * 1.8f)
                    {
                        for (int a = -10; a <= 10; a++)
                        {
                            if (a != 0)
                            {
                                if (Math.Abs(a) % 2 == 0)
                                {
                                    float R = 0.1f * Math.Abs(a);
                                    Vector2 vector = Projectile.Center - new Vector2(15 * Math.Abs(a), 22 * a).RotatedBy(Projectile.rotation);
                                    int PP = NewProjectile(Projectile.GetSource_FromAI(), vector, (Main.MouseWorld - vector).PerfectNormalize() * ranged.BowSpeed[0] * Charge, 932, (int)(ranged.BowDamage[0] * Charge / Math.Abs((float)a / 1.2F)), 0, -1, 0, R);
                                    for (int i = 0; i < 20; i++)
                                    {
                                        Color fairyQueenWeaponsColor = Main.projectile[PP].GetFairyQueenWeaponsColor();
                                        Dust dust = Main.dust[NewDust(Main.projectile[PP].Center - new Vector2(4), 1, 1, 267)];
                                        dust.velocity = -Main.projectile[PP].velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(3, 6.2F);
                                        dust.velocity.RotatedBy(Projectile.rotation);
                                        dust.noGravity = true;
                                        dust.scale = 1.2f;
                                        dust.color = fairyQueenWeaponsColor;
                                    }
                                }
                                else
                                {
                                    float R = 0.1f * Math.Abs(a);
                                    Vector2 vector = Projectile.Center - new Vector2(15 * Math.Abs(a), 22 * a).RotatedBy(Projectile.rotation);
                                    int PP = NewProjectile(Projectile.GetSource_FromAI(), vector, (Main.MouseWorld - vector).PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge / Math.Abs((float)a / 1.2F)), 0, -1, 0, R);
                                    for (int i = 0; i < 20; i++)
                                    {
                                        Color fairyQueenWeaponsColor = Main.projectile[PP].GetFairyQueenWeaponsColor();
                                        Dust dust = Main.dust[NewDust(Main.projectile[PP].Center - new Vector2(4), 1, 1, 267)];
                                        dust.velocity = -Main.projectile[PP].velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(3, 6.2F);
                                        dust.velocity.RotatedBy(Projectile.rotation);
                                        dust.noGravity = true;
                                        dust.scale = 1.2f;
                                        dust.color = fairyQueenWeaponsColor;
                                    }

                                }
                            }
                        }
                    }
                    else if (ranged.BowTime > player.itemAnimationMax * 0.8f)
                    {
                        for (int a = -2; a <= 2; a++)
                        {
                            if (Math.Abs(a) == 1)
                            {
                                float R = Main.rand.NextFloat(1);
                                Vector2 vector = Projectile.Center - new Vector2(15 * Math.Abs(a), 16 * a).RotatedBy(Projectile.rotation);
                                int PP = NewProjectile(Projectile.GetSource_FromAI(), vector, (Main.MouseWorld - vector).PerfectNormalize() * ranged.BowSpeed[0] * Charge, 932, (int)(ranged.BowDamage[0] * Charge / Math.Abs((float)a / 1.2F)), 0, -1, 0, R);
                                for (int i = 0; i < 20; i++)
                                {
                                    Color fairyQueenWeaponsColor = Main.projectile[PP].GetFairyQueenWeaponsColor();
                                    Dust dust = Main.dust[NewDust(Main.projectile[PP].Center - new Vector2(4), 1, 1, 267)];
                                    dust.velocity = -Main.projectile[PP].velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(3, 6.2F);
                                    dust.velocity.RotatedBy(Projectile.rotation);
                                    dust.noGravity = true;
                                    dust.scale = 1.2f;
                                    dust.color = fairyQueenWeaponsColor;
                                }
                            }
                            if (Math.Abs(a) == 2)
                            {
                                float R = Main.rand.NextFloat(1);
                                Vector2 vector = Projectile.Center - new Vector2(15 * Math.Abs(a), 16 * a).RotatedBy(Projectile.rotation);
                                int PP = NewProjectile(Projectile.GetSource_FromAI(), vector, (Main.MouseWorld - vector).PerfectNormalize() * ranged.BowSpeed[0] * Charge, ranged.Ammo[0], (int)(ranged.BowDamage[0] * Charge / Math.Abs((float)a / 1.2F)), 0, -1, 0, R);
                                for (int i = 0; i < 20; i++)
                                {
                                    Color fairyQueenWeaponsColor = Main.projectile[PP].GetFairyQueenWeaponsColor();
                                    Dust dust = Main.dust[NewDust(Main.projectile[PP].Center - new Vector2(4), 1, 1, 267)];
                                    dust.velocity = -Main.projectile[PP].velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(3, 6.2F);
                                    dust.velocity.RotatedBy(Projectile.rotation);
                                    dust.noGravity = true;
                                    dust.scale = 1.2f;
                                    dust.color = fairyQueenWeaponsColor;
                                }
                            }
                        }
                    }
                }
            }
            //地狱蝙蝠弓
            if (item.type == 3019 && ranged.Ammo[0] == 485)
            {
                for (int i = 0; i < 50; i++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 6)];
                    dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0, 2.2F);
                    dust.velocity.Y /= 2;
                    dust.velocity.RotatedBy(Projectile.rotation);
                    dust.noGravity = true;
                    dust.alpha = 100;
                    dust.scale = 1.8f;
                }
            }
            //地狱蝙蝠弓
            if (item.type == 3019)
            {
                for (int A = 0; A < 1000; A++)
                {
                    Projectile projectile = Main.projectile[A];
                    if (projectile.active && projectile.type == ModContent.ProjectileType<HellBats>() && projectile.owner == Projectile.owner)
                    {
                        projectile.ai[0] = 1;
                    }
                }
            }
            if (item.type == 3540)
            {
                Projectile.DProj().Bool[1] = true;
            }
            //三发
            if (Projectile.owner == Main.myPlayer)
            {
                if (ranged.Ammo[1] > 0)
                {
                    int Proj2 = NewProjectileChange(Source2, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize().RotatedBy(rangedItem.AboveArrowSpacing) * ranged.BowSpeed[1] * Charge, ranged.Ammo[1], (int)(ranged.BowDamage[1] * Charge), ranged.BowKnockBack[1] * Charge, Projectile.owner, 0, 0, Magnification);
                    Main.projectile[Proj2].scale = Projectile.scale;
                    Main.projectile[Proj2].Resize((int)(Main.projectile[Proj2].OriginalWidth() * (Main.projectile[Proj2].scale)), (int)(Main.projectile[Proj2].OriginalHeight() * (Main.projectile[Proj2].scale)));
                    //暗影弓
                    if (item.type == 3052)
                    {
                        Main.projectile[Proj2].ai[0] = 1;
                    }
                }
                if (ranged.Ammo[2] > 0)
                {
                    int Proj3 = NewProjectileChange(Source3, Projectile.Center - Projectile.velocity.PerfectNormalize() * (-10 + la + 8), Projectile.velocity.PerfectNormalize().RotatedBy(-rangedItem.UnderArrowSpacing) * ranged.BowSpeed[2] * Charge, ranged.Ammo[2], (int)(ranged.BowDamage[2] * Charge), ranged.BowKnockBack[2] * Charge, Projectile.owner, 0, 0, Magnification);
                    Main.projectile[Proj3].scale = Projectile.scale;
                    Main.projectile[Proj3].Resize((int)(Main.projectile[Proj3].OriginalWidth() * (Main.projectile[Proj3].scale)), (int)(Main.projectile[Proj3].OriginalHeight() * (Main.projectile[Proj3].scale)));                    //暗影弓
                    if (item.type == 3052)
                    {
                        Main.projectile[Proj3].ai[0] = 1;
                    }
                }
            }
        }
        /// <summary>
        /// 在最后更新
        /// </summary>  
        public virtual void PostUpdate(Item item, Projectile Projectile, RangedGlobalItem rangedItem, RangedProjectile ranged)
        {
            if (item.type == 3540)
            {
                if (!Projectile.Player().controlUseItem)
                    SP = 0;
                Projectile.Player().phantasmTime = 5;
                int A = 0;
                for (int a = 0; a < 1000; a++)
                {
                    if (Main.projectile[a].active && Main.projectile[a].type == ModContent.ProjectileType<幻影弓>() && Main.projectile[a].owner == Projectile.owner)
                    {
                        Main.projectile[a].ai[0] = Projectile.whoAmI;
                        Main.projectile[a].ai[1] = A;
                        if (Projectile.DProj().Bool[1])
                            Main.projectile[a].DProj().Bool[0] = true;
                        A++;
                    }
                }
                if (Main.myPlayer == Projectile.owner)
                {
                    if (A < 2)
                    {
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<幻影弓>(), Projectile.damage / 2, 0, -1, Projectile.whoAmI, A);
                    }
                }
                Projectile.DProj().Bool[1] = false;
            }
        }
    }
}