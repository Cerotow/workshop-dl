using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Tiles.Trophy;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.Trophy
{
    public abstract class Trophy : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            DustType = 7;
            AddMapEntry(new Color(120, 85, 60));
        }

    }
    public class LifeGuardTrophy : Trophy { }
    public class MeteorAnnihilatorTrophy : Trophy { }
    public class MeteorDiggerTrophy : Trophy { }
    public class StarGuardTrophy : Trophy { }
    public class 超级蓝史莱姆纪念章 : Trophy { }
    public class 丛林暴食怪纪念章 : Trophy { }
    public class 哥布林巫师首领纪念章 : Trophy { }
    public class 鬼牙纪念章 : Trophy { }
    public class 海幽浮王纪念章 : Trophy { }
    public class 旧日秽灵纪念章 : Trophy { }
    public class 恐惧缝合体纪念章 : Trophy { }
    public class 枯萎的橡果之灵纪念章 : Trophy { }
    public class 矿洞幽魂纪念章 : Trophy { }
    public class 炼狱头颅纪念章 : Trophy { }
    public class 流星破坏者纪念章 : Trophy { }
    public class 绿岩之视纪念章 : Trophy { }
    public class 蘑菇王纪念章 : Trophy { }
    public class 天地守卫纪念章 : Trophy { }
    public class 突变喀迈拉纪念章 : Trophy { }
    public class 突变噬魂怪纪念章 : Trophy { }
    public class 妖精王纪念章 : Trophy { }
    public class 夜光蘑菇王纪念章 : Trophy { }
    public class 狱火蛇纪念章 : Trophy { }
    public class 天雷怒云纪念章Tile : Trophy { }
    public class 先祖咒魂纪念章 : Trophy { }
    public class 星尘护卫纪念章Tile : Trophy { }
    public class 星云护卫纪念章Tile : Trophy { }
    public class 日耀护卫纪念章Tile : Trophy { }
    public class 星旋护卫纪念章Tile : Trophy { }
}