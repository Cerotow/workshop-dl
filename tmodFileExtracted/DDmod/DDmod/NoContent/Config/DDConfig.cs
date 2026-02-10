using DDmod.Players;
using DDmod.UI;
using DDmod.Worlds;
using Humanizer;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace DDmod.NoContent.Config
{
    //影响服务器的配置
    class DDConfigServer : ModConfig
    {
        public static DDConfigServer Instance;
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [Header("$Mods.DDmod.Config.ConfigServer")]

        [DefaultValue(false)]
        public bool ForceMechanism;

        [DefaultValue(false)]
        public bool Staffdamage;

        [DefaultValue(true)]
        public bool BossAnimation;

        [DefaultValue(true)]
        public bool MeleeRework;
        [DefaultValue(true)]
        public bool MeleeRework2;
        [DefaultValue(true)]
        public bool MagicRework;
        [DefaultValue(true)]
        public bool Battlepet;
        [DefaultValue(true)]
        public bool HunterSlime;
        [DefaultValue(true)]
        public bool AdventureSlime;
        [DefaultValue(true)]
        public bool AdventureCoinDealerSlime;

        [DefaultValue(0)]
        [Range(0, 2)]
        public int MaxStrengthen = 0;
        [DefaultValue(0)]
        [Range(0, 2)]
        public int MinStrengthen = 0;
        [DefaultValue(1)]
        [Range(1, 150)]
        public int Strengthen = 1;

        [DefaultValue(0)]
        [Range(0, 3600)]
        public int ItemTime;
        public override void OnChanged()
        {
            if (!AdventureSlime)
            {
                AdventureCoinDealerSlime = false;
            }
            if (MeleeRework)
            {
                MeleeRework2 = true;
            }
        }
    }
    //影响客户端的配置
    class DDConfigClient : ModConfig
    {
        public static DDConfigClient Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Header("$Mods.DDmod.Config.ConfigClient")]

        [DefaultValue(false)]
        public bool ItemID;

        [DefaultValue(true)]
        public bool DustLightEffect;

        [DefaultValue(1)]
        public float SlashEffect = 1;

        [DefaultValue(true)]
        public bool Head;

        [DefaultValue(false)]
        public bool Somersault;

        [DefaultValue(false)]
        public bool Swim;

        [DefaultValue(true)]
        public bool Action;

        [DefaultValue(true)]
        public bool ShowWeapons;

        [DefaultValue(1)]
        public float VibrationFrequency = 1;

        [DefaultValue(100)]
        [Range(20, 100)]
        public int Brightness = 1;

        [DefaultValue(0)]
        [Range(0, 1)]
        public float EyeProtection = 0;

        [DefaultValue(true)]
        public bool Delay;

        [DefaultValue(true)]
        public bool ClickEffects;

        [DefaultValue(true)]
        public bool MoveEffects;

        [DefaultValue(true)]
        public bool SwordHit;

        public enum Dps
        {
            None,
            InternationalStandard,
            ChineseConvention,
            PureChineseCharacters,
        }

        [DefaultValue(Dps.None)]
        public Dps DpsStreamline;
        public override void OnChanged()
        {
        }

    }
    //血条配置
    class DDHealthBar : ModConfig
    {
        public static DDHealthBar Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Header("$Mods.DDmod.Config.HealthBar")]

        [DefaultValue(true)]
        public bool ShowTarget;

        [DefaultValue(true)]
        public bool ShowPercentage;

        [DefaultValue(true)]
        public bool ShowLife;

        [DefaultValue(true)]
        public bool ShowName;

        [DefaultValue(true)]
        public bool DamageandDefense;

        [DefaultValue(true)]
        public bool Kill;

        [DefaultValue(true)]
        public bool Text;

    }
    //开发者模式配置
    class DeveloperMode : ModConfig
    {
        public static DeveloperMode Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Header("$Mods.DDmod.Config.DeveloperMode")]


        [DefaultValue(false)]
        public bool lockLevel;

        [DefaultValue(0)]
        public int Level;
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
        {
            if (!DDWorld.开发者模式)
            {
                message = NetworkText.FromKey("Mods.DDmod.Config.DeveloperMode.Disable");
                return false;
            }
            return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
        }
        public override void OnChanged()
        {
            if (!DDWorld.开发者模式 && !Main.gameMenu && Main.netMode != 2)
            {
                Main.NewText(Language.GetTextValue("Mods.DDmod.Configs.DeveloperMode.Disable"));
                Level = -1;
                lockLevel = false;
            }
            //Main.LocalPlayer.GetModPlayer<EntrustPlayer>().Level = Level;

        }
    }
}