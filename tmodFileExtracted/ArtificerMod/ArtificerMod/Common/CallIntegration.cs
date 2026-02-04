using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;


using Terraria.GameContent;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.Graphics.Renderers;
using Terraria.DataStructures;
using Terraria.ID;

namespace ArtificerMod.Common
{
	public class CallIntegration : ModSystem
	{
		internal static List<ArtificerAbilityData> AddOnAbilities;
        internal static List<CooldownModData> AddOnCooldownMods;
        internal static List<int> AddOnCooldownAccs;

        public override void Load()
		{
            AddOnAbilities = new List<ArtificerAbilityData>();
            AddOnCooldownMods = new List<CooldownModData>();
            AddOnCooldownAccs = new List<int>();
        }

        public static void AddNewAbilityData(int accID, int cooldownTime, string abilityDesc, string placeholderTooltip, Func<Player, bool> canActivate, Func<Player, bool> useAbility)
		{
            ArtificerAbilityData abilityData = new ArtificerAbilityData()
            {
                AccessoryID = accID,
                CooldownTime = cooldownTime,
                AbilityDescription = abilityDesc,
                PlaceholderTooltipName = placeholderTooltip,
                CanActivate = canActivate,
                UseAbility = useAbility
            };

            AddOnAbilities.Add(abilityData);
        }

        public static void AddNewCooldownData(float multiplier, Func<Player, bool> shouldApply, Func<Player, bool> activityChanged)
        {
            CooldownModData abilityData = new CooldownModData()
            {
                CooldownMult = multiplier,
                ShouldApply = shouldApply,
                ActivityChanged = activityChanged
            };

            AddOnCooldownMods.Add(abilityData);
        }

        public static void AddNewCooldownAcc(int itemID)
        {
            AddOnCooldownAccs.Add(itemID);
        }

        public override void Unload()
		{
			AddOnAbilities.Clear();
			AddOnAbilities = null;
            AddOnCooldownMods.Clear();
            AddOnCooldownMods = null;
            AddOnCooldownAccs.Clear();
            AddOnCooldownAccs = null;
        }
	}

	public class ArtificerAbilityData
    {	
		/// <summary>
		/// The itemID of the item accessory that should be associated with this ability.
		/// </summary>
        public int AccessoryID { get; init; }

        /// <summary>
        /// Duration (in ticks; 60 ticks == 1 second) of Ability Cooldown that this ability should apply
        /// </summary>
        public int CooldownTime { get; init; }

        /// <summary>
        /// Tooltip describing the actiated ability
        /// </summary>
        public string AbilityDescription { get; init; }

        /// <summary>
        /// All Ability Accessory items must have a placeholder tooltip that the mod can replace with its ability description and cooldown time info. 
		/// This should match the internal name of the tooltip that is to be replaced.
        /// </summary>
        public string PlaceholderTooltipName { get; init; }

        /// <summary>
        /// Return whether or not this activated ability can be triggered (Ex: Its respective item is equipped).
        /// </summary>
        public Func<Player, bool> CanActivate { get; init; }

		/// <summary>
		/// Define the effects of this actiavted ability. Return false to prevent applying Ability Cooldown, otherwise return true.
		/// </summary>
        public Func<Player, bool> UseAbility { get; init; }
    }

    public class CooldownModData
    {
        /// <summary>
        /// Multiplier to be enacted upon Ability Cooldown times
        /// </summary>
        public float CooldownMult { get; init; }

        /// <summary>
        /// Return whether or not this multiplier should be in effect
        /// </summary>
        public Func<Player, bool> ShouldApply { get; init; }

        /// <summary>
        /// Return whether or not this multiplier has changed from active to inactive or vice. versa
        /// </summary>
        public Func<Player, bool> ActivityChanged { get; init; }
    }

    public class RecipeBrowserIntegration : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("RecipeBrowser", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[5]
                {
                    "AddItemCategory",
                    Language.GetTextValue("Mods.ArtificerMod.CommonItemtooltip.RecipeBrowserFilter"),
                    "Accessories",
                    Mod.Assets.Request<Texture2D>("Content/Items/AbilityAccPH/HiddenBlade"), 

			        (Predicate<Item>)((Item item) =>
                    {
                        if (item.accessory)
                        {
                            return AbilityAccessories.IsAbilityAccessory(item);
                        }
                        return false;
                    }
                    )
                }
                );
            }
        }
    }

    public class CrossModHelper : ModSystem
    {
        internal static readonly string clickerVersion = new Version(1, 4).ToString();

        /// <summary>
        /// Allows adjusting the stats of another mod's Damage Class
        /// </summary>
        /// <param name="modName">The internal name of the mod (Ex: CaptureDiscClass, ThoriumMod)</param>
        /// <param name="className">The internal name of the class (Ex: CaptureDamage, BardDamage, HealerDamage)</param>
        /// <param name="plr">The player to recieve the stat adjustments</param>
        /// <param name="dmg">Additive damage adjustment (Ex: 0.15f => +15% damage)</param>
        /// <param name="atkSpd">Additive attack speed adjustment (Ex: 0.1tf => +1t% attack speed)</param>
        /// <param name="crit">Additive crit chance adjustment (Ex: 15f => +15% crit chance)</param>
        /// <param name="kb">Additive knockback adjustment (Ex: 0.15f => +15% knockback)</param>
        /// <returns></returns>
        public static bool AdjustClassStats(string modName, string className, Player plr, float dmg = 0f, float atkSpd = 0f, float crit = 0f, float kb = 0f)
        {
            if (ModLoader.TryGetMod(modName, out Mod otherMod))
            {
                if (otherMod.TryFind(className, out DamageClass damageClass))
                {
                    plr.GetDamage(damageClass) += dmg;
                    plr.GetAttackSpeed(damageClass) += atkSpd;
                    plr.GetCritChance(damageClass) += crit;
                    plr.GetKnockback(damageClass) += kb;

                    return true;
                }
            }

            return false;
        }

        public static bool CaptureTrailLength(float increase, Player plr)
        {
            if (ModLoader.TryGetMod("CaptureDiscClass", out Mod captureClass))
            {
                captureClass.Call("TrailLengthBoost", plr, increase);
                return true;
            }

            return false;
        }

        public static bool ThoriumBardBuffRange(float tiles, Player plr)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                thorium.Call("BonusBardEmpowermentRange", plr, (int)(tiles * 16f));
                return true;
            }

            return false;
        }

        public static bool ThoriumBardBuffTime(float increase, Player plr)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                thorium.Call("BonusBardEmpowermentDuration", plr, increase);
                return true;
            }

            return false;
        }

        public static bool ThoriumBardInspiration(float increase, Player plr)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                thorium.Call("BonusBardInspirationMax", plr, increase);
                return true;
            }

            return false;
        }

        public static bool ThoriumHealerHealing(int increase, Player plr)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                thorium.Call("BonusHealerHealBonus", plr, increase);
                return true;
            }

            return false;
        }

        /// <summary>
		/// Call to modify the players' effect threshold
		/// (2 will mean 2 less clicks required to reach the effect trigger threshold)
		/// </summary>
		/// <param name="player">The player</param>
		/// <param name="add">amount of clicks to reduce</param>
		public static bool ClickerBonusAdd(Player player, int increase)
        {
            if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass))
            {
                clickerClass.Call("SetPlayerStat", clickerVersion, player, "clickerBonusAdd", increase);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Call to modify the players' effect threshold
        /// (-0.20f will mean 20% less clicks required to reach the effect trigger threshold)
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="add">% of total clicks</param>
        public static bool ClickerBonusPercentAdd(Player player, float increase)
        {
            if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass))
            {
                clickerClass.Call("SetPlayerStat", clickerVersion, player, "clickerBonusPercentAdd", increase);
                return true;
            }

            return false;
        }

        public static bool ClickerRadius(Player player, float tiles)
        {
            if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass))
            {
                clickerClass.Call("SetPlayerStat", clickerVersion, player, "clickerRadiusAdd", tiles * 0.16f);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calls Language.GetTextValue with the provided arguments. The used key always starts with "Mods.ArtificerMod.CommonItemtooltip."
        /// </summary>
        /// <param name="keySuffix"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static string GetCrossmodText(string keySuffix, object[] arguments = null)
        {
            string key = "Mods.ArtificerMod.CommonItemtooltip." + keySuffix;

            if(arguments != null && arguments.Length > 0)
            {
                return Language.GetTextValue(key, arguments);
            }
            else
            {
                return Language.GetTextValue(key);
            }
        }

        /// <summary>
        /// Calls Language.GetTextValue with the provided arguments. The used key always starts with "Mods.ArtificerMod.CommonItemtooltip."
        /// </summary>
        /// <param name="keySuffix"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static string GetCrossmodText(string keySuffix, object argument = null)
        {
            string key = "Mods.ArtificerMod.CommonItemtooltip." + keySuffix;

            if (argument != null)
            {
                return Language.GetTextValue(key, argument);
            }
            else
            {
                return Language.GetTextValue(key);
            }
        }

        public static void MultiaddTooltips(ref List<TooltipLine> tooltips, Mod mod, List<string> newLines, string targetTip = "", bool removeTarget = false)
        {
            int lineCount = 0;
            if (targetTip == "")
            {
                foreach (var line in newLines)
                {
                    string lineName = "crossmod" + lineCount;
                    lineCount++;

                    tooltips.Add(new TooltipLine(mod, "lineName", line));
                }

                return;
            }


            TooltipLine target = tooltips.FirstOrDefault(line => line.Mod == "Terraria" && line.Name == targetTip);
            if (target.Equals(null) || target.Equals(default(TooltipLine)))
            {
                return;
            }
            int index = tooltips.IndexOf(target);
            if (index == -1)
            {
                return;
            }

            foreach (var line in newLines)
            {
                string lineName = "crossmod" + lineCount;
                lineCount++;

                tooltips.Insert(index, new TooltipLine(mod, "lineName", line));
            }

            if (removeTarget)
            {
                tooltips.Remove(target);
            }
        }
    }
}