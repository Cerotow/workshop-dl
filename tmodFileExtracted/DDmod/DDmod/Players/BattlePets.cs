
using DDmod.Content.Items.Pet;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.ID;
using DDmod.Content.Projectiles.BattlePets;
using DDmod.Content.Achievements;
namespace DDmod.Players
{
    //战斗宠物
    public class BattlePets
    {
        public const int 空 = 0;
        public const int 迷失之眼 = 1;
        public const int 金毛幼鸟 = 2;
        public const int 牢中灵骷 = 3;

        public const int 夜晚 = 0;
        public const int 白天 = 1;
        public const int 地牢 = 2;


        public static List<Asset<Texture2D>> texture = new List<Asset<Texture2D>>();
        public int Background;
        public int WhoamI;
        public int[] Skill = new int[4];
        public int SkillCD;
        public int MaxSkillCD;
        public int Skill2CD;
        public int MaxSkill2CD;
        public int Rarity;
        /// <summary>
        /// 变体
        /// </summary>
        public byte Variant=0;
        public byte MaxVariant=0;
        /// <summary>
        /// 受伤
        /// </summary>
        public bool Wounded;
        /// <summary>
        /// 性格
        /// </summary>
        public int Nature;
        public const int 鲁莽 = 0;
        public const int 胆小 = 1;
        public const int 强壮 = 2;
        public const int 勇敢 = 3;
        public const int 暴躁 = 4;
        public const int 活泼 = 5;
        public const int 坚强 = 6;
        public const int 倔强 = 7;
        public const int 耿直 = 8;
        public int MaxNature = 8;
        public BattlePets(int type)
        {
            Type = type;
            Setdefault();
            UP(Level);
            Life = LifeMax / 2;
        }
        public static void Load()
        {
            texture = new List<Asset<Texture2D>>();
            texture.Add(ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/环境/夜晚环境"));
            texture.Add(ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/环境/白天环境"));
            texture.Add(ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/环境/地牢环境"));
        }
        /// <summary>
        /// 伤害
        /// </summary>
        public float AddDamage = 1;
        /// <summary>
        /// 血量
        /// </summary>
        public float AddLife = 1;
        /// <summary>
        /// 防御
        /// </summary>
        public float AddDefense = 1;
        public void ResetEffects()
        {
            AddDamage = 1;
            AddLife = 1;
            AddDefense = 1;
            Endurance = 0;
        }
        public void Setdefault(bool Refresh = true)
        {
            if (Type == 迷失之眼)
            {
                ProjType = ModContent.ProjectileType<迷失之眼>();
                if (Refresh)
                {
                    TrueOriginalDamage = 6;
                    TrueOriginalLifeMax = 600;
                    TrueOriginalDefense = 6;
                    Level = Main.rand.Next(1, 5);
                    Nature = Main.rand.Next(MaxNature + 1);
                    Endurance = 0;
                    Background = 夜晚;
                }
                Skill[0] = 1;
                Skill[1] = 2;
                Skill[2] = 3;
                Skill[3] = 4;
                MaxSkillCD = 600;
                MaxSkill2CD = 1800;
                Rarity = 1;
            }
            if (Type == 金毛幼鸟)
            {
                ProjType = ModContent.ProjectileType<金毛幼鸟>();
                if (Refresh)
                {
                    TrueOriginalDamage = 4;
                    TrueOriginalLifeMax = 550;
                    TrueOriginalDefense = 2;
                    Level = Main.rand.Next(1, 5);
                    Nature = Main.rand.Next(MaxNature + 1);
                    Endurance = 0;
                    Background = 白天;
                }
                Skill[0] = 5;
                Skill[1] = 6;
                Skill[2] = 7;
                Skill[3] = 8;
                MaxSkillCD = 600;
                MaxSkill2CD = 3000;
                Rarity = 0;
            }
            if (Type == 牢中灵骷)
            {
                ProjType = ModContent.ProjectileType<牢中灵骷>();
                if (Refresh)
                {
                    TrueOriginalDamage = 8;
                    TrueOriginalLifeMax = 500;
                    TrueOriginalDefense = 3;
                    Level = Main.rand.Next(1, 5);
                    Nature = Main.rand.Next(MaxNature + 1);
                    MaxVariant = 5;
                    Endurance = 0;
                    Background = 地牢;
                }
                MaxSkillCD = 600;
                MaxSkill2CD = 3000;
                Rarity = 4;
                if (Variant==0)
                {
                    Skill[0] = 9;
                    Skill[1] = 10;
                    Skill[2] = -1;
                    Skill[3] = -2;
                }
                else if (Variant == 1)
                {
                    Skill[0] = 11;
                    Skill[1] = 12;
                    Skill[2] = 13;
                    Skill[3] = 14;
                    MaxSkillCD = DDHelper.Second(22);
                    MaxSkill2CD = DDHelper.Second(60);
                    Melee = false;
                }
                else if (Variant == 2)
                {

                    Skill[0] = 15;
                    Skill[1] = 16;
                    Skill[2] = 17;
                    Skill[3] = 18;
                    MaxSkillCD = DDHelper.Second(25);
                    MaxSkill2CD = DDHelper.Second(80);
                }
                else if (Variant == 3)
                {

                    Skill[0] = 19;
                    Skill[1] = 20;
                    Skill[2] = 21;
                    Skill[3] = 22;
                    MaxSkillCD = DDHelper.Second(20);
                    MaxSkill2CD = DDHelper.Second(40);
                }
                else if (Variant == 4)
                {
                    Skill[0] = 23;
                    Skill[1] = 24;
                    Skill[2] = 25;
                    Skill[3] = 26;
                    MaxSkillCD = DDHelper.Second(20);
                    MaxSkill2CD = DDHelper.Second(100);
                    Melee = false;
                }
                else if (Variant == 5)
                {
                    Skill[0] = 27;
                    Skill[1] = 28;
                    Skill[2] = 29;
                    Skill[3] = 30;
                    MaxSkillCD = DDHelper.Second(10);
                    MaxSkill2CD = DDHelper.Second(10);
                }
            }
            if (Type > 0&& Refresh)
            {
                Damage = (int)(OriginalDamage * (1 + (float)Level * 0.1f));
                LifeMax = (int)(OriginalLifeMax * (5 + (float)Level * 0.2f));
                Defense = (int)(OriginalDefense * (1 + (float)Level * 0.1f));
            }
        }
        public void SaveData(TagCompound tag, int i)
        {
            tag.Add("BattlePetsType" + i,Type);
            tag.Add("BattlePetsLevel" + i, Level);
            tag.Add("BattlePetsLife" + i, Life);
            tag.Add("BattlePetsExp" + i, Exp);
            tag.Add("BattlePetsNature" + i, Nature);
            tag.Add("Variant" + i, Variant);

            if (Fight) tag["BattlePetsFight"+i] = true;
        }
        public void LoadData(TagCompound tag, int i)
        {
            Type = tag.Get<int>("BattlePetsType" + i);
            Setdefault();
            Level = tag.Get<int>("BattlePetsLevel" + i);
            Life = tag.Get<int>("BattlePetsLife" + i);
            Exp = tag.Get<int>("BattlePetsExp" + i);
            Nature = tag.Get<int>("BattlePetsNature" + i);
            Variant = tag.Get<byte>("Variant" + i);
            Fight = tag.ContainsKey("BattlePetsFight" + i);
            Setdefault(false);
            ResetEffects();
            if (Level >= 16)
            {
                UI.BattlePetUI.技能.Skill.Update(Skill[0],this);
            }
            if (Level >= 30)
            {
                UI.BattlePetUI.技能.Skill.Update(Skill[1], this);
            }
            if (Level >= 46)
            {
                UI.BattlePetUI.技能.Skill.Update(Skill[2], this);
                UI.BattlePetUI.技能.Skill.Update(Skill[3], this);
            }
            UP(Level);
        }
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="Position">位置</param>
        /// <param name="ScreenpPosition">屏幕位置</param>
        /// <param name="frame">帧,X进化帧,Y帧图,Z当前进化帧</param>
        /// <param name="lightColor"></param>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        /// <param name="sprite"></param>
        public int frame;
        public int framecount;
        public int projFrames;
        public void Draw(SpriteBatch spriteBatch, Vector2 Position, Vector2 ScreenpPosition, Vector3 frame, Color lightColor, Vector2 scale, float rotation, SpriteEffects sprite, bool Animation = true)
        {
            if (Animation)
                framecount++;
            if (Type == 迷失之眼)
            {
                frame.X = 4;
                frame.Y = 5;
                if (framecount > 6)
                {
                    this.frame++;
                    framecount = 0;
                }
                if (this.frame >= projFrames)
                {
                    this.frame = 0;
                }
                Texture2D texture = TextureAssets.Projectile[ProjType].Value;
                Rectangle rectangle = new((int)(texture.Width / frame.X * frame.Z), (int)(texture.Height / frame.Y * this.frame), (int)(texture.Width / frame.X), (int)(texture.Height / frame.Y));
                spriteBatch.Draw(texture, Position - ScreenpPosition, rectangle, lightColor, rotation, rectangle.Size() / 2, scale, sprite, 0);
            }
            if (Type == 金毛幼鸟)
            {
                frame.X = 4;
                frame.Y = 6;
                if (projFrames == 4)
                {
                    if (framecount > 4)
                    {
                        this.frame++;
                        framecount = 0;
                    }
                }
                if (projFrames == 5)
                {
                    if (framecount > 3)
                    {
                        this.frame++;
                        framecount = 0;
                    }
                }
                if (projFrames == 6)
                {
                    if (framecount > 2)
                    {
                        this.frame++;
                        framecount = 0;
                    }
                }
                if (this.frame >= projFrames)
                {
                    this.frame = 0;
                }
                Texture2D texture = TextureAssets.Projectile[ProjType].Value;
                Rectangle rectangle = new((int)(texture.Width / frame.X * frame.Z), (int)(texture.Height / frame.Y * this.frame), (int)(texture.Width / frame.X), (int)(texture.Height / frame.Y));
                spriteBatch.Draw(texture, Position - ScreenpPosition, rectangle, Color.White, rotation, rectangle.Size() / 2, scale, sprite, 0);
            }
            if (Type == 牢中灵骷)
            {
                frame.X = 8;
                frame.Y = 2;
                if (framecount > 6)
                {
                    this.frame++;
                    framecount = 0;
                }
                if (this.frame >= projFrames)
                {
                    this.frame = 0;
                }
                Texture2D texture = TextureAssets.Projectile[ProjType].Value;
                Rectangle rectangle = new((int)(texture.Width / frame.X * frame.Z), (int)(texture.Height / frame.Y * this.frame), (int)(texture.Width / frame.X), (int)(texture.Height / frame.Y));
                spriteBatch.Draw(texture, Position - ScreenpPosition, rectangle, Color.White, rotation, rectangle.Size() / 2, scale, sprite, 0);

            }
        }
        /// <summary> 宠物类型 </summary>
        public int Type = 0;
        public int ProjType = 0;
        /// <summary> 宠物等级 </summary>
        public int Level = 0;
        /// <summary> 宠物伤害 </summary>
        public int OriginalDamage = 0;
        public int TrueOriginalDamage = 0;
        public int Damage = 0;
        /// <summary> 宠物防御 </summary>
        public int OriginalDefense = 0;
        public int TrueOriginalDefense = 0;
        public int Defense = 0;
        /// <summary> 宠物减伤 </summary>
        public float Endurance = 0;
        /// <summary> 宠物攻击类型 </summary>
        public int AIType = 0;
        /// <summary> 宠物血量 </summary>
        public int Life;
        public int OriginalLifeMax;
        public int TrueOriginalLifeMax;
        public int LifeMax;
        /// <summary> 升级经验 </summary>
        public int Exp;
        public int MaxExp;
        /// <summary> 宠物出战 </summary>
        public bool Fight = false;
        public bool Melee = true;
        /// <summary> 设置性格属性 </summary>
        public string setNature(out float Damage, out float Life, out float Defense, out float Endurance, out float Exp, out Color color)
        {
            Damage = 1;
            Life= 1;
            Defense = 1;
            Endurance = 0;
            Exp = 1;
            color = new Color(255,255,255);
            if (Nature== BattlePets.鲁莽)
            {
                Damage += 0.2f;
                Defense -= 0.2f;
                Life -= 0.3f;
                color = new Color(255, 100, 100);
                return Language.GetTextValue("Mods.DDmod.BattlePetUI.性格.鲁莽");
            }
            if (Nature== BattlePets.胆小)
            {
                Damage -= 0.5f;
                Defense += 0.1f;
                Life += 0.1f;
                Exp -= 0.5f;
                color = new Color(100, 0, 240);
                return Language.GetTextValue("Mods.DDmod.BattlePetUI.性格.胆小");
            }
            if(Nature== BattlePets.强壮)
            {
                Damage += 0.1f;
                Defense += 0.1f;
                Life += 0.1f;
                Exp += 0.3f;
                color = new Color(50, 255, 255);
                return Language.GetTextValue("Mods.DDmod.BattlePetUI.性格.强壮");
            }
            if(Nature== BattlePets.勇敢)
            {
                Damage += 0.05f;
                Defense += 0.05f;
                Life += 0.05f;
                color = new Color(255, 255, 0);
                return Language.GetTextValue("Mods.DDmod.BattlePetUI.性格.勇敢");
            }
            if(Nature== BattlePets.暴躁)
            {
                Damage += 0.4f;
                Defense -= 0.5f;
                Life -= 0.5f;
                color = new Color(255, 0, 0);
                return Language.GetTextValue("Mods.DDmod.BattlePetUI.性格.暴躁");
            }
            if(Nature== BattlePets.活泼)
            {
                Defense -= 0.2f;
                Life += 0.5f;
                color = new Color(100, 255, 100);
                return Language.GetTextValue("Mods.DDmod.BattlePetUI.性格.活泼");
            }
            if(Nature== BattlePets.坚强)
            {
                Damage -= 0.5f;
                Defense += 0.2f;
                Life += 0.2f;
                Endurance += 0.2f;
                color = new Color(50, 150, 255);
                return Language.GetTextValue("Mods.DDmod.BattlePetUI.性格.坚强");
            }
            if(Nature== BattlePets.倔强)
            {
                Damage -= 0.3f;
                Defense += 0.3f;
                Life += 0.3f;
                color = new Color(100, 0, 0);
                return Language.GetTextValue("Mods.DDmod.BattlePetUI.性格.倔强");

            }
            if(Nature== BattlePets.耿直)
            {
                Exp -= 0.2f;
                color = new Color(255, 0, 255);
                return Language.GetTextValue("Mods.DDmod.BattlePetUI.性格.耿直");
            }
            return "";
        }
        public string Name(out int Variation)
        {
            Variation = PetsVariant(Level);
            //Level = Main.rand.Next(1, 60);
            string n = "";
            if(Wounded)
            {
                n += Language.GetTextValue("Mods.DDmod.BattlePetText.受伤");
            }
            if (Type==迷失之眼)
            {
                if(Variation==0)
                return Language.GetTextValue("Mods.DDmod.BattlePet.迷失之眼")+ n;
                else
                    return Language.GetTextValue("Mods.DDmod.BattlePet.迷失之眼"+ (Variation+1)) + n;
            }
            if (Type== 金毛幼鸟)
            {
                if (Variation == 0)
                    return Language.GetTextValue("Mods.DDmod.BattlePet.金毛幼鸟") + n;
                else
                    return Language.GetTextValue("Mods.DDmod.BattlePet.金毛幼鸟" + (Variation + 1)) + n;
            }
            if (Type== 牢中灵骷)
            {
                if (Variation == 0)
                    return Language.GetTextValue("Mods.DDmod.BattlePet.牢中灵骷") + n;
                else
                    return Language.GetTextValue("Mods.DDmod.BattlePet.牢中灵骷" + (Variation + 1)) + n;
            }
            return "";
        }
        public void PFrames(int Variation)
        {
            if (Type == 迷失之眼)
            {
                if (Variation==3)
                {
                    projFrames = 5;
                }
                else
                {
                    projFrames = 4;
                }
            }
            
            if (Type == 金毛幼鸟)
            {
                if (Variation==0)
                {
                    projFrames = 4;
                }
                else if (Variation == 3)
                {
                    projFrames = 6;
                }
                else
                {
                    projFrames = 5;

                }
            }
            
            if (Type == 牢中灵骷)
            {
                projFrames = 1;
                if (Variation >= 2)
                {
                    projFrames =2;
                }
            }

        }


        /// <summary> 开启UI后的更新 </summary>
        public void Update(int i)
        {
            WhoamI = i;
            if (Fight)
            {
                if (Type >0)
                {
                    Main.LocalPlayer.Dplayer().FightPets = i;
                }
                return;
            }
            
            if (this.Level < 0)
            {
                this.Level = 0;
            }
            if (this.Level > 120)
            {
                this.Level = 120;
            }
            ResetEffects();
            int Variation = 0;
            if (Level >= 16)
            {
                Variation = 1;
            }
            if (Level >= 30)
            {
                Variation = 2;
            }
            if (Level >= 46)
            {
                Variation = 3;
                ModContent.GetInstance<完美伙伴>().Condition.Complete();
            }
            if (Variation >= 1)
            {
                UI.BattlePetUI.技能.Skill.Update(Skill[0], Main.LocalPlayer.Dplayer().Bpets[i]);
            }
            if (Variation >= 2)
            {
                UI.BattlePetUI.技能.Skill.Update(Skill[1], Main.LocalPlayer.Dplayer().Bpets[i]);
            }
            if (Variation >= 3)
            {
                UI.BattlePetUI.技能.Skill.Update(Skill[2], Main.LocalPlayer.Dplayer().Bpets[i]);
                UI.BattlePetUI.技能.Skill.Update(Skill[3], Main.LocalPlayer.Dplayer().Bpets[i]);
            }
            PFrames(Variation);
            if (Life<0)
            {
                Life = 1;
            }
            UP(this.Level);
            if (Life >= LifeMax / 5)
            {
                Wounded = false;
            }
        }
        public void PlayerUpdate(Player player,int i)
        {
            WhoamI = i;
            if (this.Level < 0)
            {
                this.Level = 0;
            }
            if (this.Level > 120)
            {
                this.Level = 120;
            }
            ResetEffects();
            int Variation = 0;
            if (Level >= 16)
            {
                Variation = 1;
            }
            if (Level >= 30)
            {
                Variation = 2;
            }
            if (Level >= 46)
            {
                Variation = 3;
                if(Main.netMode!=2)
                ModContent.GetInstance<完美伙伴>().Condition.Complete();
            }
            if (Variation>=1)
            {
                UI.BattlePetUI.技能.Skill.Update(Skill[0], player.Dplayer().Bpets[i]);
            }
            if (Variation >= 2)
            {
                UI.BattlePetUI.技能.Skill.Update(Skill[1], player.Dplayer().Bpets[i]);
            }
            if (Variation>=3)
            {
                UI.BattlePetUI.技能.Skill.Update(Skill[2], player.Dplayer().Bpets[i]);
                UI.BattlePetUI.技能.Skill.Update(Skill[3], player.Dplayer().Bpets[i]);
            }
            PFrames(Variation);
            if (SkillCD>0)
            {
                SkillCD--;
            }
            if (Skill2CD>0)
            {
                Skill2CD--;
            }
            if (Life < 0)
            {
                Life = 0;
            }
            UP(player, this.Level);
            if (Life >= LifeMax / 5)
            {
                Wounded = false;
            }
        }
        //升级经验
        public void UP(Player player, int Level)
        {
            setNature(out float Damage, out float Life, out float Defense, out float Endurance, out float Exp2, out Color color);
            if (Level == 0)
            {
                MaxExp = 20;
            }
            else
            {
                MaxExp = 0;
            }
            int EXP = 50;
            for (int a = 0; a <= 120; a++)
            {
                if (a < Level)
                {
                    MaxExp += EXP;

                    if (a < 50)
                    {
                        EXP = (int)(EXP * 1.1f);
                    }
                    else
                    {
                        EXP = (int)(EXP * 1.05f);
                    }
                }
                if (a == 5)
                {
                    EXP = 100;
                }
                if (a == 10)
                {
                    EXP = 200;
                }
                if (a == 20)
                {
                    EXP = 600;
                }
                if (a == 30)
                {
                    EXP = 1800;
                }
                if (a == 40)
                {
                    EXP = 5400;
                }
                if (a == 50)
                {
                    EXP = 16200;
                }
            }
            MaxExp = (int)(MaxExp * Exp2);
            if (Level >= 120)
            {
                Exp = 1;
                MaxExp = 1;
            }
            else if (player == Main.LocalPlayer && MaxExp > 0 && Exp >= MaxExp&& Level < player.GetModPlayer<EntrustPlayer>().Level&& player.GetModPlayer<EntrustPlayer>().EnterWorld)
            {
                Exp -= MaxExp;
                Life = LifeMax;
                //NewProjectile(player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<UPProj>(), 0, 0, -1);
                this.Level++;
                if (player.Dplayer().ownedFightPets>=0)
                {
                    Main.projectile[player.Dplayer().ownedFightPets].DProj().Bool[4] = true;
                }
                UP(player, this.Level);
                DDmod.SyncData(DDType.Battlepets, Main.myPlayer, -1, Main.myPlayer);
            }
            Data();
            this.Damage = (int)(this.Damage * Damage);
            this.LifeMax = (int)(this.LifeMax * Life);
            this.Defense = (int)(this.Defense * Defense);
            this.Endurance += Endurance;
        }
        public int PetsVariant(int Level)
        {
            int Variation = 0;
            if (Level >= 16)
            {
                Variation = 1;
            }
            if (Level >= 30)
            {
                Variation = 2;
            }
            if (Level >= 46)
            {
                Variation = 3;
            }
            if(Level>=46&&Variant ==0)
            {
                Variant = (byte)Main.rand.Next(1, MaxVariant+1);
            }
            Setdefault(false);
            if(Variant>0)
            {
                return Variation + Variant - 1;
            }
            return Variation;
        }
        public void UP(int Level)
        {
            setNature(out float Damage, out float Life, out float Defense, out float Endurance, out float Exp2, out Color color);
            if (Level == 0)
            {
                MaxExp = 20;
            }
            else
            {
                MaxExp = 0;
            }
            int EXP = 50;
            for (int a = 0; a <= 120; a++)
            {
                if (a < Level)
                {
                    MaxExp += EXP;

                    if (a < 50)
                    {
                        EXP = (int)(EXP * 1.1f);
                    }
                    else
                    {
                        EXP = (int)(EXP * 1.05f);
                    }
                }
                if (a == 5)
                {
                    EXP = 100;
                }
                if (a == 10)
                {
                    EXP = 200;
                }
                if (a == 20)
                {
                    EXP = 600;
                }
                if (a == 30)
                {
                    EXP = 1800;
                }
                if (a == 40)
                {
                    EXP = 5400;
                }
                if (a == 50)
                {
                    EXP = 16200;
                }
            }
            MaxExp = (int)(MaxExp * Exp2);
            if (Level >= 120)
            {
                Exp = 1;
                MaxExp = 1;
            }
            //else if (MaxExp > 0 && Exp >= MaxExp && Level < Main.LocalPlayer.GetModPlayer<EntrustPlayer>().Level)
            else if (MaxExp > 0 && Exp >= MaxExp && Main.LocalPlayer.TryGetModPlayer(out EntrustPlayer player)&& Level < player.Level && player.EnterWorld)
            {
                Exp -= MaxExp;
                Life = LifeMax;
                //NewProjectile(player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<UPProj>(), 0, 0, -1);
                this.Level++;

                UP(this.Level);
            }
            Data();
            this.Damage =(int)(this.Damage*Damage);
            this.LifeMax = (int)(this.LifeMax* Life);
            this.Defense = (int)(this.Defense*Defense);
            this.Endurance += Endurance;
            if(this.Life> LifeMax)
            {
                this.Life = LifeMax;
            }
        }
        public void Data()
        {
            int Variation = PetsVariant(Level);

            OriginalDamage = TrueOriginalDamage;
            OriginalLifeMax = TrueOriginalLifeMax;
            OriginalDefense = TrueOriginalDefense;
            if(Variation==1)
            {
                OriginalDamage = (int)(TrueOriginalDamage*1.25F);
                OriginalLifeMax = (int)(TrueOriginalLifeMax*1.5F);
                OriginalDefense = (int)(TrueOriginalDefense*1.5F);
            }
            if(Variation==2)
            {
                OriginalDamage = (int)(TrueOriginalDamage * 1.5F);
                OriginalLifeMax = (int)(TrueOriginalLifeMax * 2F);
                OriginalDefense = (int)(TrueOriginalDefense * 2F);

            }
            if(Variation>=3)
            {
                OriginalDamage = (int)(TrueOriginalDamage * 1.75F);
                OriginalLifeMax = (int)(TrueOriginalLifeMax * 2.5F);
                OriginalDefense = (int)(TrueOriginalDefense * 2.5F);

            }
            Damage = (int)(OriginalDamage * (1 + (float)Level * 0.1f));
            LifeMax = (int)(OriginalLifeMax * (5 + (float)Level*0.2f));
            Defense = (int)(OriginalDefense * (1 + (float)Level * 0.1f));
            Damage = (int)(Damage * AddDamage);
            LifeMax = (int)(LifeMax * AddLife);
            Defense = (int)(Defense * AddDefense);
        }
    }
}