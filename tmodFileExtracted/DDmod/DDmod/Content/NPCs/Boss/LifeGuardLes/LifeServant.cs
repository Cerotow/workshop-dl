using DDmod.Content.Projectiles.Boss;
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.LifeGuardLes
{
    public class LifeServant : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Life Servant");
           //DisplayName.AddTranslation(7, "大地仆从");
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            float Strengthen = 1 + (NPCDowned.downedLifeGuard ? 0.25f : 0) + (NPCDowned.downedLifeGuard2 ? 0.25f : 0);
            if(Main.expertMode)
            {
                Strengthen *= 1.25F;
            }
            if(Main.masterMode)
            {
                Strengthen *= 1.25F;
            }
            NPC.damage = 2;
            NPC.lifeMax = (int)(15 * Strengthen);
            NPC.defense = (int)(3 * Strengthen);
            NPC.Dnpc().LifeUP = false;
            NPC.knockBackResist = 0f;
            NPC.width = 32;
            NPC.height = 32;
            NPC.aiStyle = -1;
            NPC.scale = 0.8f;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.Tink;
            NPC.noGravity = true;
            NPC.DeathSound = SoundID.Shatter;
            NPC.value = 0;
            NPC.Dnpc().BossPhysique = true;
            NPCID.Sets.TrailCacheLength[NPC.type] = 1;
            NPC.Dnpc().Properties.Stone = true;
            if (NPCDowned.downedLifeGuard)
            {
                NPC.Dnpc().Properties.BossLife = 1.05F;
            }
            if (NPCDowned.downedLifeGuard2)
            {
                NPC.Dnpc().Properties.BossLife = 1.1F;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.LifeServant"))
            });
        }
        public override void AI()
        {
            NPC.damage = 0;
            //让他知道他的主人
            NPC parent = Main.npc[NPC.Dnpc().Master];
            //如果他的主人是生命守卫就执行
            if (parent.type == ModContent.NPCType<LifeGuard>() && parent.active)
            {
                //数量
                int Proj = 0;
                //排序
                int G = 0;
                //遍历一遍NPC,知道这个生命守卫有了多少仆从和仆从顺序
                for (int T = 0; T < 200; T++)
                {
                    if (parent.Dnpc().Servant[T])
                    {
                        if (Main.npc[T].type == ModContent.NPCType<LifeServant>() && Main.npc[T].active)
                        {
                            Proj++;
                            if (Main.npc[T].whoAmI < NPC.whoAmI)
                            {
                                G++;
                            }
                        }
                    }
                }
                //用数量与Boss的调整距离,以免太过密集
                float E = (float)(60f * (Proj * 0.08));
                //限制最小距离,以免和Boss重叠
                if (E < 60)
                {
                    E = 60;
                }
                //让仆从围着Boss并且让Boss控制仆从旋转速度
                Vector2 vector2 = parent.Center + Utils.RotatedBy(new Vector2(0f, E), (Math.PI * 2 / Proj * G) + parent.localAI[2], default);
                NPC.Center = vector2;

                NPC.rotation = Vector2.Subtract(parent.Center, NPC.Center).ToRotation() - MathHelper.PiOver2;
                //防止他脱战
                NPC.timeLeft = 20;
                //如果他的主人正在结束动画
                if (parent.localAI[0] != 0)
                {
                    NPC.active = false;
                }
            }
            else
            {
                //如果他的主人不是生命守卫
                NPC.active = false;
            }
            NPC.TargetClosest();
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.pick > 0)
            {
                modifiers.SourceDamage *= item.pick / 10;
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 20; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, 12, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, 12, hit.HitDirection, -1f, 0, default, 1f);
                }
                Vector2 vector = Vector2.Subtract(Main.player[NPC.target].Center, NPC.Center);
                if (vector != Vector2.Zero) vector.Normalize();
                vector *= 5;
                //if (NPCDowned.downedLifeGuard2&&Main.rand.NextBool(5))
                    //NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector * 1.5F, ModContent.ProjectileType<BossHeart>(), 8, 1);
            }
        }
    }
}