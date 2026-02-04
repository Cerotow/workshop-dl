using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace ArtificerMod.Common
{
    public class ArtificerSlot1 : ModAccessorySlot
	{
		public override bool IsEnabled()
		{
            // Check for viable artificer set bonuses

            // Defensive coding. All player instantces ought to have an ArtificerPlayer instance but somehow Calamity IL edits ruin things?
            if (!Player.TryGetModPlayer(out ArtificerPlayer artificer)) 
			{
				return false;
			}
			return artificer.accFlagOr || artificer.accFlagSt || artificer.accFlagTe || artificer.accFlagTa || artificer.accFlagPr
				|| artificer.accFlagMe || artificer.accFlagCh || artificer.accFlagLi || artificer.accFlagXe || artificer.accFlagAs;
		}

		public override string FunctionalTexture => "ArtificerMod/Content/Items/AccessoriesH/ArtificerEmblem";

		public override string FunctionalBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot1";
		public override string VanityBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot2";
		public override string DyeBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot3";

		public override void OnMouseHover(AccessorySlotType context)
		{
			switch (context)
			{
				case AccessorySlotType.FunctionalSlot:
					Main.hoverItemName = Language.GetTextValue("Mods.ArtificerMod.CommonItemtooltip.BonusAccessory");
					break;
			}
		}
	}

	public class ArtificerSlot2 : ModAccessorySlot
	{
		public override bool IsEnabled()
		{
            if (!Player.TryGetModPlayer(out ArtificerPlayer artificer))
            {
                return false;
            }
            return artificer.accFlagMe || artificer.accFlagCh || artificer.accFlagLi || artificer.accFlagXe || artificer.accFlagAs;
		}

		public override string FunctionalTexture => "ArtificerMod/Content/Items/AccessoriesH/ArtificerEmblem";

		public override string FunctionalBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot1";
		public override string VanityBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot2";
		public override string DyeBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot3";

		public override void OnMouseHover(AccessorySlotType context)
		{
			switch (context)
			{
				case AccessorySlotType.FunctionalSlot:
					Main.hoverItemName = Language.GetTextValue("Mods.ArtificerMod.CommonItemtooltip.BonusAccessory");
					break;
			}
		}
	}

	public class ArtificerSlot3 : ModAccessorySlot
	{
		public override bool IsEnabled()
		{
            if (!Player.TryGetModPlayer(out ArtificerPlayer artificer))
            {
                return false;
            }
            return artificer.accFlagAs;
		}

		public override string FunctionalTexture => "ArtificerMod/Content/Items/AccessoriesH/ArtificerEmblem";

		public override string FunctionalBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot1";
		public override string VanityBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot2";
		public override string DyeBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot3";

		public override void OnMouseHover(AccessorySlotType context)
		{
			switch (context)
			{
				case AccessorySlotType.FunctionalSlot:
					Main.hoverItemName = Language.GetTextValue("Mods.ArtificerMod.CommonItemtooltip.BonusAccessory");
					break;
			}
		}
	}

	public class ArtificerConfigSlot : ModAccessorySlot
	{
		public override bool IsEnabled()
		{
            if (!Player.TryGetModPlayer(out ArtificerPlayer artificer))
            {
                return false;
            }
            return (artificer.accFlagOr || artificer.accFlagSt || artificer.accFlagTe || artificer.accFlagTa || artificer.accFlagPr
				 || artificer.accFlagMe || artificer.accFlagCh || artificer.accFlagLi || artificer.accFlagXe || artificer.accFlagAs) 
				 && ModContent.GetInstance<ConfigServer>().BonusArtificerSlot;
		}

		public override string FunctionalTexture => "ArtificerMod/Content/Items/AccessoriesH/ArtificerEmblem";

		public override string FunctionalBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot1";
		public override string VanityBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot2";
		public override string DyeBackgroundTexture => "ArtificerMod/Assets/ArtificerInvSlot3";

		public override void OnMouseHover(AccessorySlotType context)
		{
			switch (context)
			{
				case AccessorySlotType.FunctionalSlot:
					Main.hoverItemName = Language.GetTextValue("Mods.ArtificerMod.CommonItemtooltip.BonusAccessory");
					break;
			}
		}
	}
}