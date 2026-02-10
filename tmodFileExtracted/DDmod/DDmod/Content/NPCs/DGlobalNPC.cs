using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Boss.蘑菇王;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.Projectiles;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Projectiles.Summon.Whip;
using DDmod.Content.Tiles.农场;
using DDmod.UI.HunterQuests;
using System;
using System.Reflection;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.NPCShop;
using Terraria.ID;
using DDmod.Content.Items.Tiles;
using DDmod.Content.Tiles.杂物块;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Items.Melee.Sword;
using Terraria;
using static AssGen.Assets;
using DDmod.Players;
using DDmod.Content.Biome;
using System.Linq;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Tiles;
using DDmod.Content.Items.Ranged.Make.Gun;
using DDmod.Content.Items.Series.杂物;
using DDmod.Content.Items.Ammo;
using DDmod.Worlds;

namespace DDmod.Content.NPCs
{
    public class NPCProperties
    {
        /// <summary> 火属性 </summary>
        public bool Fire;
        /// <summary> 暗影火属性 </summary>
        public bool ShadowFire;
        /// <summary> 冰属性 </summary>
        public bool Ice;
        /// <summary> 水属性 </summary>
        public bool Water;
        /// <summary> 铁属性 </summary>
        public bool Iron;
        /// <summary> 石属性 </summary>
        public bool Stone;
        /// <summary> 凝胶属性 </summary>
        public bool Gel;
        /// <summary> 血肉属性 </summary>
        public bool Meat;
        /// <summary> 草属性 </summary>
        public bool Grass;
        /// <summary> 真菌属性 </summary>
        public bool Fungi;
        /// <summary> 光属性 </summary>
        public bool Light;
        /// <summary> 纯粹光属性 </summary>
        public bool TrueLight;
        /// <summary> 控制 </summary>
        public bool Control = true;
        /// <summary> 怪物时期等级划分
        ///0:开局白天小怪
        ///1:比0危险一点的小怪
        ///2:安全一点的群系环境
        ///3:危险一点的群系环境
        ///4:非常危险的群系环境
        ///5:陨石坑
        ///6:地狱
        ///7:地牢和困难模式简单小怪
        ///8:森林各种小事件
        ///9:各种安全一点的群系
        ///10:各种危险群系包括邪恶群系跟三王前事件
        ///11:困难模式地狱
        ///12:三王后事件
        ///13:花后生成小怪
        ///14:火星人
        ///15:四柱 </summary>
        public byte Level = 255;
        public float BossLife = 1;
        public void SetStaticDefaults(NPC npc)
        {
            TrueLight = Light;
            if (npc.type is 75 or 83 or 84 or 120 or 137 or 138 or 179 or 288 or 583 or 584 or 585 or 636
               or 677)
            {
                Light = true;
                TrueLight = true;
            }
            if (npc.type is 60 or 653 or 277 or 278 or 279 or 280
                or 415 or 416 or 417 or 418 or 518 or 516
                or 517 or 654 or 655 or 59 or 151 or 419
                or 412 or 24 or 23 or 25 or 72 or 653 or 654 or 655)
            {
                Fire = true;
            }
            if (npc.type is 143 or 144 or 145 or 146 or 147 or 150
               or 154 or 161 or 169 or 184 or 206 or 243
               or 345 or 352 or 343 or 431 or 335 or 336
               or 535 or 537 or 657 or 658 or 659 or 660
               or 667 or 70 or 83 or 84 or 85)
            {
                Ice = true;
            }
            if (npc.type is 83 or 84 or 85 or 125 or 126 or 127
               or 128 or 129 or 130 or 131 or 134 or 135 or 290
               or 136 or 139 or 140 or 346 or 347 or 387
               or 388 or 392 or 393 or 394 or 395 or 179 or 341

                 or 378 or 384 or 387 or 388 or 392 or 393
                 or 394 or 395 or 399 or 467 or 473 or 474
                 or 475 or 476 or 491 or 492 or 520 or 629)
            {
                Iron = true;
            }
            if (npc.type is 245 or 246 or 247 or 248 or 249 or 437
                or 482 or 483 or 537 or 541 or 631)
            {
                Stone = true;
            }
            if (npc.type is 1 or 16 or 71 or 81 or 121 or 122
               or 141 or 183 or 204 or 50 or 225 or 244
               or 302 or 304 or 333 or 334 or 335 or 336
               or 535 or 537 or 657 or 658 or 659 or 660
               or 667 or 670 or 676 or 678 or 679 or 680 or 681 or 682 or 683
                or 684 or 685 or 686 or 687)
            {
                Gel = true;
            }
            if (npc.type is 2 or 3 or 4 or 5 or 6 or 7 or 8 or 9 or 10 or 11 or 12 or 13 or 14 or 15 or 17 or 18 or 19 or 20
                or 22 or 24 or 26 or 27 or 28 or 29 or 37 or 38 or 42 or 46 or 47 or 48 or 49 or 51 or 52 or 53 or 54 or 55
                or 57 or 58 or 61 or 62 or 63 or 64 or 65 or 66 or 67 or 69 or 73 or 74 or 76 or 86 or 87 or 88 or 89 or 90
                or 91 or 92 or 93 or 94 or 95 or 96 or 97 or 98 or 99 or 100 or 101 or 102 or 103 or 104 or 105 or 106 or 107 or 108
                or 109 or 111 or 113 or 114 or 115 or 116 or 117 or 118 or 119 or 123 or 124 or 142 or 132 or 133 or 148 or 149 or 152 or 153
                or 154 or 155 or 156 or 157 or 158 or 159 or 161 or 162 or 163 or 164 or 165 or 185 or 168 or 170 or 171 or 173 or 174 or 176
                or 177 or 178 or 180 or 181 or 182 or 183 or 186 or 187 or 188 or 189 or 190 or 191 or 192 or 193 or 194 or 195 or 196 or 198
                or 199 or 200 or 205 or 207 or 208 or 209 or 210 or 211 or 212 or 213 or 214 or 215 or 216 or 217 or 218 or 219 or 220 or 221
                or 222 or 223 or 224 or 226 or 227 or 228 or 229 or 230 or 231 or 232 or 233 or 234 or 235 or 236 or 237 or 238 or 239 or 240
                or 241 or 242 or 251 or 252 or 266 or 267 or 268 or 297 or 298 or 299 or 300 or 301 or 303 or 317 or 318 or 319 or 320 or 321
                or 331 or 332 or 337 or 338 or 339 or 340 or 343 or 351 or 353 or 354 or 355 or 356 or 350 or 357 or 358 or 359 or 360 or 361
                or 362 or 363 or 364 or 365 or 366 or 367 or 368 or 369 or 370 or 372 or 373 or 374 or 375 or 376 or 377 or 379 or 380 or 381
                or 382 or 383 or 389 or 385 or 386 or 390 or 391 or 430 or 431 or 432 or 433 or 434 or 435 or 436 or 438 or 439 or 441 or 442
                or 443 or 444 or 445 or 446 or 447 or 448 or 460 or 460 or 462 or 463 or 464 or 465 or 466 or 468 or 469 or 470 or 471 or 477
                or 478 or 479 or 480 or 484 or 485 or 486 or 487 or 489 or 490 or 494 or 495 or 496 or 497 or 498 or 499 or 500 or 501 or 502
                or 503 or 504 or 505 or 506 or 508 or 509 or 510 or 511 or 512 or 513 or 514 or 515 or 524 or 525 or 526 or 527 or 528 or 529
                or 530 or 531 or 532 or 534 or 536 or 538 or 539 or 540 or 542 or 543 or 544 or 545 or 550 or 551 or 552 or 553 or 554 or 555
                or 556 or 557 or 558 or 559 or 560 or 561 or 562 or 563 or 564 or 565 or 568 or 569 or 570 or 571 or 572 or 573 or 574 or 575
                or 576 or 577 or 578 or 579 or 580 or 581 or 582 or 563 or 564 or 565 or 566 or 586 or 587 or 588 or 589 or 590 or 591 or 592
                or 593 or 595 or 596 or 597 or 598 or 599 or 600 or 601 or 602 or 603 or 604 or 605 or 606 or 607 or 608 or 609 or 610 or 611
                or 612 or 613 or 614 or 615 or 616 or 617 or 618 or 619 or 620 or 621 or 622 or 623 or 624 or 625 or 626 or 627 or 632 or 633
                or 636 or 637 or 638 or 639 or 640 or 641 or 642 or 643 or 644 or 645 or 646 or 647 or 648 or 649 or 650 or 651 or 652 or 656
                or 661 or 663 or 656 or 668 or 669 or 671 or 672 or 673 or 674 or 675)
            {
                Meat = true;
            }

            if (npc.type is 30 or 471 or 472 or 533 or 665 or 29)
            {
                ShadowFire = true;
            }
            if (npc.type is 33 or 63 or 64 or 103 or 112 or 250 or 371 or 666)
            {
                Water = true;
            }
            if (npc.type is 43 or 56 or 175 or 166 or 204 or 262
                 or 263 or 264 or 265 or 305 or 306 or 307
                 or 308 or 309 or 310 or 311 or 312 or 313
                 or 314 or 315 or 325 or 326 or 327 or 328
                 or 329 or 330 or 344 or 348 or 349 or 546
                 or 547 or 628)
            {
                Grass = true;
            }
            if (npc.type is 160 or 254 or 255 or 256 or 257 or 258 or 259
                 or 260 or 261 or 634 or 635)
            {
                Fungi = true;
            }
            if (npc.type is 25 or 30 or 33 or 112 or 384 or 401 or 522 or 523
                or 665 or 666 || npc.boss || npc.Dnpc().BossPhysique)
            {
                Control = false;
            }
            if (!Control)
            {
                npc.buffImmune[ModContent.BuffType<Fossil>()] = true;
                npc.buffImmune[ModContent.BuffType<Freeze>()] = true;

            }
            if (Fire || ShadowFire||Ice||Water||Iron||Stone||Gel||Meat||Grass||Fungi)
                TrueLight = false;
            if (Fire || Water)
            {
                npc.buffImmune[BuffID.OnFire] = true;
                npc.buffImmune[ModContent.BuffType<地狱之火>()] = true;
                npc.buffImmune[ModContent.BuffType<炼狱之火>()] = true;
                npc.buffImmune[323] = true;
                npc.lavaImmune = true;
            }
            if (npc.type >= 87 && npc.type <= 92)
            {
                npc.buffImmune[ModContent.BuffType<Freeze>()] = true;
            }
            if (Ice)
            {
                npc.buffImmune[ModContent.BuffType<Frozen>()] = true;
                npc.buffImmune[ModContent.BuffType<Freeze>()] = true;
                npc.buffImmune[44] = true;
            }

            if (Iron || Stone)
            {
                npc.buffImmune[BuffID.OnFire] = true;
                npc.buffImmune[BuffID.OnFire3] = true;
                npc.buffImmune[BuffID.ShadowFlame] = true;
                npc.buffImmune[20] = true;
                npc.buffImmune[30] = true;
                npc.buffImmune[31] = true;
                npc.buffImmune[44] = true;
                npc.buffImmune[70] = true;
                npc.buffImmune[323] = true;
                npc.buffImmune[324] = true;
                npc.buffImmune[BuffID.CursedInferno] = true;
                npc.buffImmune[ModContent.BuffType<Frozen>()] = true;
                npc.buffImmune[ModContent.BuffType<Fossil>()] = true;
                npc.buffImmune[ModContent.BuffType<地狱之火>()] = true;
                npc.buffImmune[ModContent.BuffType<炼狱之火>()] = true;
                npc.buffImmune[344] = true;
                npc.buffImmune[ModContent.BuffType<Bleed>()] = true;
                npc.buffImmune[ModContent.BuffType<EvilEntanglement>()] = true;
                if (Stone)
                {
                    npc.buffImmune[ModContent.BuffType<Charged>()] = true;
                    npc.buffImmune[ModContent.BuffType<Charged2>()] = true;
                }
            }
            if (Light|| TrueLight)
            {
                if (TrueLight)
                {
                    npc.buffImmune[BuffID.OnFire] = true;
                    npc.buffImmune[BuffID.OnFire3] = true;
                    npc.buffImmune[20] = true;
                    npc.buffImmune[30] = true;
                    npc.buffImmune[31] = true;
                    npc.buffImmune[44] = true;
                    npc.buffImmune[70] = true;
                    npc.buffImmune[323] = true;
                    npc.buffImmune[324] = true;
                    npc.buffImmune[ModContent.BuffType<Frozen>()] = true;
                    npc.buffImmune[ModContent.BuffType<Fossil>()] = true;
                    npc.buffImmune[ModContent.BuffType<地狱之火>()] = true;
                    npc.buffImmune[ModContent.BuffType<炼狱之火>()] = true;
                    npc.buffImmune[344] = true;
                    npc.buffImmune[ModContent.BuffType<Bleed>()] = true;
                    npc.buffImmune[ModContent.BuffType<Charged>()] = true;
                    npc.buffImmune[ModContent.BuffType<Charged2>()] = true;
                }
            }
            if (ShadowFire)
            {
                npc.buffImmune[153] = true;
            }
            if (Gel)
            {
                npc.buffImmune[137] = true;
            }
        }
        public void SetLevel(NPC npc)
        {
            int type = npc.type;
            if (npc.netID < 0)
            {
                type = npc.netID;
            }
            //森林白天怪
            if (type is 1 or -3 or -7 or -4 or 302 or 316 or 333 or 334 or 335 or 336 or 624 or 628)
            {
                Level = 0;
            }
            //森林边境怪
            if (type is 73)
            {
                Level = 0;
            }
            //森林夜晚怪
            if (type is 632 or 590 or 591 or 430 or 431 or 432 or 433 or 434 or 435 or 436 or 331 or 332 or 317 or 318 or 319 or 320 or 321 or 301 or 200 or 186 or 187 or 188 or 189
                or 190 or 191 or 192 or 193 or 194 or 132 or 2 or 3 or -45 or -44 or -43 or -42 or -41 or -40 or -39 or -38 or -37 or -36 or -35 or -34 or -33 or -32 or -31 or -30 or -29 or -28 or -27 or -26)
            {
                Level = 1;
            }
            //普通地下
            if (type is 665 or 634 or 635 or 494 or 495 or 496 or 497 or 498 or 499 or 500 or 501 or 502 or 503 or 504 or 505 or 506 or 481 or 482 or 483 or 449 or 450 or 451 or 452
                or 322 or 323 or 324 or 217 or 218 or 219 or 201 or 202 or 203 or 195 or 196 or 164 or 165 or 63 or 58 or 49 or 44 or 45 or 10 or 11 or 12 or 16 or 21 or -9 or -8 or -6 or -5 or -53 or -52 or -51 or -50 or -49 or -48 or -47 or -46)
            {
                Level = 1;
            }
            //下雨白天
            if (type is 224 or 225)
            {
                Level = 0;
            }
            //下雨晚上
            if (type is -55 or -54 or 223)
            {
                Level = 1;
            }
            //雪原地表
            if (type is 147 or 161)
            {
                Level = 1;
            }
            //雪原地下
            if (type is 150 or 167 or 184 or 185)
            {
                Level = 2;
            }
            //沙漠地表
            if (type is 61 or 69 or 546)
            {
                Level = 1;
            }
            //沙漠地下
            if (type is 508 or 509 or 513 or 514 or 515 or 537 or 580 or 581 or 582)
            {
                Level = 3;
            }
            //海洋
            if (type is 64 or 65 or 67 or 220 or 221)
            {
                Level = 2;
            }
            //腐化
            if (type is 666 or -12 or -11 or 6 or 7 or 8 or 9 or 168)
            {
                Level = 3;
            }
            //猩红
            if (type is -23 or -22 or 173 or 181 or 239 or 240 or 241 or 242 or 464 or 465)
            {
                Level = 3;
            }
            //天空
            if (type is 48)
            {
                Level = 3;
            }
            //血月
            if (type is 47 or 53 or 57 or 489 or 490 or 536 or 586 or 587)
            {
                Level = 2;
            }
            //丛林地表
            if (type is 51 or 56)
            {
                Level = 2;
            }
            //丛林夜晚地表
            if (type is 52)
            {
                Level = 3;
            }
            //丛林地下
            if (type is 231 or 210 or 211 or 232 or 233 or 234 or 235 or 204 or 43 or 42 or -10 or -65 or -64 or -63 or -62 or -61 or -60 or -59 or -58 or -57 or -56 or -17 or -16)
            {
                Level = 4;
            }
            //流星
            if (type is 23)
            {
                Level = 5;
            }
            //哥布林入侵
            if (type is 26 or 27 or 28 or 29 or 30 or 111)
            {
                Level = 2;
            }
            //地牢
            if (type is 31 or 32 or 33 or 34 or 70 or 71 or 72)
            {
                Level = 7;
            }
            //地狱
            if (type is 59 or 60 or 24 or 25 or 39 or 40 or 41 or 62 or 66)
            {
                Level = 6;
            }
            //神庙
            if (type is 198 or 199 or 226)
            {
                Level = 14;
            }
            //撒旦军团一阶
            if (type is 552 or 555 or 558 or 561 or 564 or 566)
            {
                Level = 3;
            }
            //撒旦军团二阶
            if (type is 553 or 556 or 559 or 562 or 568 or 570 or 572 or 574 or 576)
            {
                Level = 11;
            }
            //撒旦军团三阶
            if (type is 554 or 557 or 560 or 563 or 565 or 567 or 569 or 571 or 573 or 575 or 577 or 578)
            {
                Level = 14;
            }
            if (Main.hardMode)
            {
                //神圣
                if (type is 545 or 475 or 75 or 80 or 84 or 86 or 120 or 122 or 137 or 138 or 171 or 244)
                {
                    Level = 9;
                }
                //困难雨天
                if (type is 250)
                {
                    Level = 8;
                }
                //困难哥布林
                if (type is 471 or 472)
                {
                    Level = 9;
                }
                //困难天空
                if (type is 87 or 88 or 89 or 90 or 91 or 92)
                {
                    Level = 8;
                }
                //困难地表
                if (type is 82 or 379 or 380 or 438)
                {
                    Level = 7;
                }
                //困难地表夜晚
                if (type is 254 or 255 or 104 or 133 or 140 or 304)
                {
                    Level = 8;
                }
                //困难血月
                if (type is 109 or 378 or 618 or 619 or 620 or 621 or 622 or 623)
                {
                    Level = 9;
                }
                //困难地下
                if (type is 631 or 480 or 256 or 257 or 258 or 259 or 260 or 261 or 238 or 172 or 163 or 141 or 110 or -15 or 77 or 85 or 93 or 95 or 96 or 97 or 102 or 103)
                {
                    Level = 9;
                }
                //困难丛林
                if (type is 476 or 236 or 237 or 205 or 152 or 153 or -21 or -20 or -19 or -18 or 157 or 175 or 176 or 177)
                {
                    Level = 10;
                }
                //困难雪原
                if (type is 629 or 206 or 154 or 155 or 169 or 197 or 243)
                {
                    Level = 9;
                }
                //困难沙漠
                if (type is 541 or 542 or 78 or 510 or 511 or 512 or 524 or 525 or 526 or 527 or 528 or 529 or 530 or 531 or 532 or 533)
                {
                    Level = 10;
                }
                //困难腐化
                if (type is 543 or 473 or 170 or 121 or 112 or -2 or -1 or 79 or 81 or 83 or 94 or 98 or 99 or 100 or 101)
                {
                    Level = 10;
                }
                //困难猩红
                if (type is 630 or 544 or 474 or -25 or -24 or 174 or 179 or 180 or 182 or 183 or 268)
                {
                    Level = 10;
                }
                //困难地狱
                if (type is 151 or 156 or 534)
                {
                    Level = 11;
                }
                //日食
                if (type is 477 or 478 or 479 or 466 or 467 or 468 or 469 or 470 or 158 or 159 or 162 or 166 or 251 or 253 or 460 or 461 or 462 or 463)
                {
                    Level = 10;
                    if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                        Level = 12;
                    if (NPC.downedPlantBoss)
                        Level = 13;
                }
                //雪人军团
                if (type is 143 or 144 or 145)
                {
                    Level = 10;
                }
                //海盗
                if (type is 662 or 212 or 213 or 214 or 215 or 216 or 252 or 491 or 492)
                {
                    Level = 10;
                }
                //花后地牢
                if (type is 269 or 270 or 271 or 272 or 273 or 274 or 275 or 276 or 277 or 278 or 279 or 280 or 281 or 282 or 283 or 284 or 285 or 286 or 287 or 288 or 289 or 290 or 291 or 292 or 293 or 294 or 295 or 296)
                {
                    Level = 13;
                }
                //南瓜月
                if (type is 337 or 338 or 325 or 326 or 329 or 330 or 305 or 306 or 307 or 308 or 309 or 310 or 311 or 312 or 313 or 314 or 315)
                {
                    Level = 13;
                }
                //霜月
                if (type is 344 or 345 or 346 or 338 or 339 or 340 or 341 or 342 or 343 or 347 or 348 or 349 or 350 or 351 or 352)
                {
                    Level = 13;
                }

                //火星人
                if (type is 392 or 393 or 394 or 395 or 520 or 381 or 382 or 383 or 384 or 385 or 386 or 387 or 388 or 389 or 390 or 391 or 399)
                {
                    Level = 14;
                }

                //四柱
                if (type is 516 or 517 or 518 or 519 or 507 or 493 or 402 or 403 or 404 or 405 or 406 or 407 or 408 or 409 or 410 or 411 or 412 or 413 or 414 or 415 or 416 or 417 or 418 or 419 or 420 or 421 or 422 or 423 or 424 or 425 or 426 or 427 or 428 or 429)
                {
                    Level = 15;
                }
            }
            //Boss

            switch (npc.type)
            {
                case 1:
                case 535:
                    if (NPC.AnyNPCs(50))
                    {
                        BossLife = 1.05f;
                    }
                    break;
                case 50:
                    BossLife = 1.05f;
                    break;
                case 4:
                case 5:
                    BossLife = 1.075f;
                    break;
                case 13:
                case 14:
                case 15:
                case 266:
                case 267:
                    BossLife = 1.1f;
                    break;
                case 210:
                case 211:
                    if (NPC.AnyNPCs(222))
                    {
                        BossLife = 1.125f;
                    }
                    break;
                case 222:
                    BossLife = 1.125f;
                    break;
                case 35:
                case 36:
                case 668:
                    BossLife = 1.15f;
                    break;
                case 113:
                case 114:
                case 115:
                case 116:
                case 117:
                case 118:
                case 119:
                case 657:
                case 658:
                case 659:
                case 660:
                    BossLife = 1.2f;
                    break;
                case 125:
                case 126:
                case 127:
                case 128:
                case 129:
                case 130:
                case 131:
                case 134:
                case 135:
                case 136:
                case 139:
                    BossLife = 1.25f;
                    break;
                case 262:
                case 263:
                case 264:
                case 265:
                    BossLife = 1.275f;
                    break;
                case 245:
                case 246:
                case 247:
                case 248:
                case 249:
                case 370:
                case 371:
                case 372:
                case 373:
                case 636:
                    BossLife = 1.3f;
                    break;
                case 439:
                case 440:
                case 454:
                case 455:
                case 456:
                case 457:
                case 458:
                case 459:
                case 521:
                case 522:
                case 523:
                    BossLife = 1.325f;
                    break;
                case 396:
                case 397:
                case 398:
                case 400:
                    BossLife = 1.4f;
                    break;
            }
        }
    }
    public class DGlobalNPC2 : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        /// <summary>中立怪</summary>
        public bool Neutrality;
    }

    public class DGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override bool IsCloneable => true;
        public static bool[] IgnoreTile = NPCID.Sets.Factory.CreateCustomSet(false);
        /// <summary>
        /// 物块怪
        /// </summary>
        public Point16 TETile;
        /// <summary>
        /// 副本怪
        /// </summary>
        public bool Copy = false;
        /// <summary>
        /// 坐的位置
        /// </summary>
        public Point SittingPoint;
        /// <summary>
        /// 可以捕捉的战斗宠物
        /// </summary>
        public bool Battlepet = false;
        public int BattlepetType = 0;
        /// <summary>
        /// 处于生成阶段
        /// </summary>
        public bool Defaults = true;
        /// <summary> 困难模式的属性强化 </summary>
        public bool LifeUP = true;
        /// <summary> 禁止Boss死亡(通常用在死亡动画) </summary>
        public bool Deathrattle = false;
        /// <summary> 心鞭Buff </summary>
        public bool HeartMarker = false;
        /// <summary> 橡果Buff </summary>
        public bool AcornMarker = false;
        /// <summary> 流星Buff </summary>
        public bool MeteorMarker = false;
        /// <summary> 魔星Buff </summary>
        public bool MagicStar = false;
        public bool 狱火爆炸 = false;
        public bool 蘑菇鞭 = false;
        public bool 绿岩鞭 = false;
        /// <summary> Boss阶段 </summary>
        public int Stage;
        /// <summary> 无敌帧 </summary>
        public int[] InvincibleFrame = new int[256];
        /// <summary> 单个弹幕无敌帧 </summary>
        public int[] InvincibleProj = new int[1000];
        /// <summary> 护甲穿透 </summary>
        public int Penetrate;

        /// <summary> 不显示伤害 </summary>
        public bool NoDamage;
        /// <summary> 免疫buff </summary>
        public bool NoBuff;
        /// <summary> npc只在一个端同步 </summary>
        public int Player;
        /// <summary> 被控制 </summary>
        public int Control;
        public int Control2;
        /// <summary> 无法移动 </summary>
        public bool NoMove;
        /// <summary> 移动速度 </summary>
        public float MoveSpeed = 1;
        /// <summary>旧旋转</summary>
        public Vector2 oldVelocity;
        /// <summary>旧旋转</summary>
        public float oldrotation;
        /// <summary>摔落伤害 </summary>
        public float FallDamage;

        /// <summary>穿透保护机制,一般用于Boss蠕虫</summary>
        public float PenetrationProtection = 0;
        public float MaxPenetrationProtection = 0;

        public Vector2 velocity;
        public Vector2[] vector = new Vector2[3];
        public bool[] Bool = new bool[5];
        public float[] Times = new float[5];

        public bool netUpdate;

        /// <summary>属于Boss的一部分</summary>
        public bool BossPhysique;
        /// <summary>无法控制</summary>
        public bool Uncontrollable;
        /// <summary>属性</summary>
        public NPCProperties Properties = new NPCProperties();
        /// <summary>npc站立</summary>
        public int Stand;
        /// <summary>中立怪</summary>
        public bool Neutrality;
        /// <summary>主人</summary>
        public int Master = 0;
        public bool MasterBool;
        /// <summary>杂鱼</summary>
        public bool[] Servant = new bool[200];
        /// <summary>哥布林</summary>
        public bool Goblins;
        //暂时废弃项目(血量数组)
        public int[] Lifes = new int[5];
        public int[] MaxLifes = new int[5];
        public bool 创建实例;
        public int ID;
        public float ArmorReduction = 1;
        public override void SetStaticDefaults()
        {
            Math.Abs(-1);
            if (IgnoreTile.Length< NPCLoader.NPCCount)
            {
                IgnoreTile = NPCID.Sets.Factory.CreateCustomSet(false);
            }
        }
        public static void LIFE(NPC npc)
        {
            if (DDWorld.晨曦)
            {
                npc.lifeMax = (int)(npc.lifeMax * npc.Dnpc().Properties.BossLife);
                npc.defense = (int)(npc.defense * npc.Dnpc().Properties.BossLife);
                float Life = 1;
                switch (npc.Dnpc().Properties.Level)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        Life = 1.05F;
                        break;
                    case 3:
                        Life = 1.075F;
                        break;
                    case 4:
                        Life = 1.1F;
                        break;
                    case 5:
                        Life = 1.1F;
                        break;
                    case 6:
                        Life = 1.125F;
                        break;
                    case 7:
                        Life = 1.15F;
                        break;
                    case 8:
                        Life = 1.175F;
                        break;
                    case 9:
                        Life = 1.175F;
                        break;
                    case 10:
                        Life = 1.2F;
                        break;
                    case 11:
                        Life = 1.2F;
                        break;
                    case 12:
                        Life = 1.25F;
                        break;
                    case 13:
                        Life = 1.275F;
                        break;
                    case 14:
                        Life = 1.3F;
                        break;
                    case 15:
                        Life = 1.325F;
                        break;
                }
                npc.lifeMax = (int)(npc.lifeMax * Life);
                npc.defense = (int)(npc.defense * Life);

                npc.life = npc.lifeMax;
                npc.defDamage = npc.damage;
                npc.defDefense = npc.defense;
            }
        }
        public override void ResetEffects(NPC npc)
        {
            HeartMarker = false;
            AcornMarker = false;
            MeteorMarker = false;
            MagicStar = false;
            狱火爆炸 = false;
            蘑菇鞭 = false;
            绿岩鞭 = false;
            Penetrate = 0;
            NoMove = false;
            MoveSpeed = 1;
        }
        /*
        public static void SendScale(NPC npc)
        {
            if (Main.netMode == 2)
            {
                ModPacket packet = DDmod.Instance.GetPacket(256);
                packet.Write((byte)DDType.NPCScale);
                packet.Write(npc.whoAmI);
                packet.Write(npc.scale);
                packet.Write(npc.width);
                packet.Write(npc.height);
            }
        }
        public static void ReceiveScale(BinaryReader binaryReader)
        {
            int whoAmI = binaryReader.ReadInt32();
            NPC npc = Main.npc[whoAmI];
            npc.scale = binaryReader.ReadFloat();
            npc.width = binaryReader.ReadInt32();
            npc.height = binaryReader.ReadInt32();
        }
        public void Send(NPC npc)
        {
            DGlobalNPC Gnpc = npc.Dnpc();
            if (Main.netMode == 2)
            {
                ModPacket packet = DDmod.Instance.GetPacket(256);
                //写入要发的包
                packet.Write((byte)DDType.NPC);
                packet.Write(npc.whoAmI);

                packet.Write(npc.localAI[0]);
                packet.Write(npc.localAI[1]);
                packet.Write(npc.localAI[2]);
                packet.Write(npc.localAI[3]);

                packet.WriteVector2(npc.Center);
                packet.WriteVector2(npc.velocity);
                packet.Write(npc.lifeMax);
                packet.Write(npc.alpha);

                packet.Write(Bool[0]);
                packet.Write(Bool[1]);
                packet.Write(Bool[2]);
                packet.Write(Bool[3]);
                packet.Write(Bool[4]);

                packet.WriteVector2(vector[0]);
                packet.WriteVector2(vector[1]);
                packet.WriteVector2(vector[2]);

                packet.Write(Times[0]);
                packet.Write(Times[1]);
                packet.Write(Times[2]);
                packet.Write(Times[3]);
                packet.Write(Times[4]);

                packet.Write(Stage);
                packet.Write(Control);
                packet.Write(Master);
                for (int a = 0; a < 200; a++)
                {
                    packet.Write(Servant[a]);
                }
                //发出去
                packet.Send(-1, -1);
            }
        }*/
        public static void Receive(BinaryReader binaryReader)
        {
            int whoAmI = binaryReader.ReadInt32();
            NPC npc = Main.npc[whoAmI];
            npc.localAI[0] = binaryReader.ReadFloat();
            npc.localAI[1] = binaryReader.ReadFloat();
            npc.localAI[2] = binaryReader.ReadFloat();
            npc.localAI[3] = binaryReader.ReadFloat();

            npc.Center = binaryReader.ReadVector2();
            npc.velocity = binaryReader.ReadVector2();
            npc.lifeMax = binaryReader.ReadInt32();
            npc.alpha = binaryReader.ReadInt32();

            DGlobalNPC Gnpc = npc.Dnpc();
            Gnpc.Bool[0] = binaryReader.ReadBoolean();
            Gnpc.Bool[1] = binaryReader.ReadBoolean();
            Gnpc.Bool[2] = binaryReader.ReadBoolean();
            Gnpc.Bool[3] = binaryReader.ReadBoolean();
            Gnpc.Bool[4] = binaryReader.ReadBoolean();

            Gnpc.vector[0] = binaryReader.ReadVector2();
            Gnpc.vector[1] = binaryReader.ReadVector2();
            Gnpc.vector[2] = binaryReader.ReadVector2();

            Gnpc.Times[0] = binaryReader.ReadFloat();
            Gnpc.Times[1] = binaryReader.ReadFloat();
            Gnpc.Times[2] = binaryReader.ReadFloat();
            Gnpc.Times[3] = binaryReader.ReadFloat();
            Gnpc.Times[4] = binaryReader.ReadFloat();

            Gnpc.Stage = binaryReader.ReadInt32();
            Gnpc.Control = binaryReader.ReadInt32();
            /*
            Gnpc.Master = binaryReader.ReadInt32();

            for (int a = 0; a < 200; a++)
            {
                Gnpc.Servant[a] = binaryReader.ReadBoolean();
            }*/
        }
        public override bool CheckActive(NPC npc)
        {
            if(npc.ModNPC!=null&&npc.ModNPC.Mod==DDmod.Instance)
            npc.chaseable =  !Neutrality;
            return true;
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            // 1. 第一个字节：所有布尔值 + 整数大小预测
            byte flags1 = 0;
            flags1 |= (byte)((Bool[0] ? 1 : 0) << 0);
            flags1 |= (byte)((Bool[1] ? 1 : 0) << 1);
            flags1 |= (byte)((Bool[2] ? 1 : 0) << 2);
            flags1 |= (byte)((Bool[3] ? 1 : 0) << 3);
            flags1 |= (byte)((Bool[4] ? 1 : 0) << 4);
            flags1 |= (byte)((Copy ? 1 : 0) << 5);
            flags1 |= (byte)((Stage > 127 || Stage < -128 ? 1 : 0) << 6); // Stage大小预测
            flags1 |= (byte)((Control > 32767 || Control < -32768 ? 1 : 0) << 7); // Control用short范围
            binaryWriter.Write(flags1);

            // 2. 写入动态整数
            if ((flags1 & 64) != 0) binaryWriter.Write((short)Stage); else binaryWriter.Write((sbyte)Stage);
            if ((flags1 & 128) != 0) binaryWriter.Write(Control); else binaryWriter.Write((short)Control);

            // 3. 第二个字节：localAI标记 + Times标记的一部分
            byte flags2 = 0;
            flags2 |= (byte)((npc.localAI[0] != 0f ? 1 : 0) << 0);
            flags2 |= (byte)((npc.localAI[1] != 0f ? 1 : 0) << 1);
            flags2 |= (byte)((npc.localAI[2] != 0f ? 1 : 0) << 2);
            flags2 |= (byte)((npc.localAI[3] != 0f ? 1 : 0) << 3);
            flags2 |= (byte)((Times[0] != 0f ? 1 : 0) << 4);
            flags2 |= (byte)((Times[1] != 0f ? 1 : 0) << 5);
            flags2 |= (byte)((Times[2] != 0f ? 1 : 0) << 6);
            flags2 |= (byte)((npc.lifeMax > 32767 || npc.lifeMax < -32768 ? 1 : 0) << 7); // lifeMax大小预测
            binaryWriter.Write(flags2);

            // 4. 写入localAI和Times[0-2]
            if ((flags2 & 1) != 0) binaryWriter.Write(npc.localAI[0]);
            if ((flags2 & 2) != 0) binaryWriter.Write(npc.localAI[1]);
            if ((flags2 & 4) != 0) binaryWriter.Write(npc.localAI[2]);
            if ((flags2 & 8) != 0) binaryWriter.Write(npc.localAI[3]);
            if ((flags2 & 16) != 0) binaryWriter.Write(Times[0]);
            if ((flags2 & 32) != 0) binaryWriter.Write(Times[1]);
            if ((flags2 & 64) != 0) binaryWriter.Write(Times[2]);

            // 5. 第三个字节：Times剩余 + Vector2标记 + alpha大小预测
            byte flags3 = 0;
            flags3 |= (byte)((Times[3] != 0f ? 1 : 0) << 0);
            flags3 |= (byte)((Times[4] != 0f ? 1 : 0) << 1);
            flags3 |= (byte)((vector[0].X != 0f ? 1 : 0) << 2);
            flags3 |= (byte)((vector[0].Y != 0f ? 1 : 0) << 3);
            flags3 |= (byte)((vector[1].X != 0f ? 1 : 0) << 4);
            flags3 |= (byte)((vector[1].Y != 0f ? 1 : 0) << 5);
            flags3 |= (byte)((vector[2].X != 0f ? 1 : 0) << 6);
            flags3 |= (byte)((npc.alpha > 127 || npc.alpha < -128 ? 1 : 0) << 7); // alpha大小预测
            binaryWriter.Write(flags3);

            // 6. 写入Times剩余和Vector2[0-1]
            if ((flags3 & 1) != 0) binaryWriter.Write(Times[3]);
            if ((flags3 & 2) != 0) binaryWriter.Write(Times[4]);
            if ((flags3 & 4) != 0) binaryWriter.Write(vector[0].X);
            if ((flags3 & 8) != 0) binaryWriter.Write(vector[0].Y);
            if ((flags3 & 16) != 0) binaryWriter.Write(vector[1].X);
            if ((flags3 & 32) != 0) binaryWriter.Write(vector[1].Y);

            // 7. 第四个字节：Vector2[2]剩余
            byte flags4 = 0;
            flags4 |= (byte)((vector[2].Y != 0f ? 1 : 0) << 0);
            // 位1-7保留给未来扩展
            binaryWriter.Write(flags4);

            // 8. 写入Vector2[2]剩余
            if ((flags3 & 64) != 0) binaryWriter.Write(vector[2].X);
            if ((flags4 & 1) != 0) binaryWriter.Write(vector[2].Y);

            // 9. 写入动态整数（使用之前的预测位）
            if ((flags2 & 128) != 0) binaryWriter.Write(npc.lifeMax);else binaryWriter.Write((short)npc.lifeMax);
            if ((flags3 & 128) != 0) binaryWriter.Write((short)npc.alpha); else binaryWriter.Write((sbyte)npc.alpha);
        }

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            // 1. 读取第一个标记
            byte flags1 = binaryReader.ReadByte();
            Bool[0] = (flags1 & 1) != 0;
            Bool[1] = (flags1 & 2) != 0;
            Bool[2] = (flags1 & 4) != 0;
            Bool[3] = (flags1 & 8) != 0;
            Bool[4] = (flags1 & 16) != 0;
            Copy = (flags1 & 32) != 0;

            // 2. 读取动态整数
            Stage = (flags1 & 64) != 0 ? binaryReader.ReadInt16() : binaryReader.ReadSByte();
            Control = (flags1 & 128) != 0 ? binaryReader.ReadInt32() : binaryReader.ReadInt16();

            // 3. 读取第二个标记
            byte flags2 = binaryReader.ReadByte();

            // 4. 读取localAI和Times[0-2]
            npc.localAI[0] = (flags2 & 1) != 0 ? binaryReader.ReadSingle() : 0f;
            npc.localAI[1] = (flags2 & 2) != 0 ? binaryReader.ReadSingle() : 0f;
            npc.localAI[2] = (flags2 & 4) != 0 ? binaryReader.ReadSingle() : 0f;
            npc.localAI[3] = (flags2 & 8) != 0 ? binaryReader.ReadSingle() : 0f;
            Times[0] = (flags2 & 16) != 0 ? binaryReader.ReadSingle() : 0f;
            Times[1] = (flags2 & 32) != 0 ? binaryReader.ReadSingle() : 0f;
            Times[2] = (flags2 & 64) != 0 ? binaryReader.ReadSingle() : 0f;

            // 5. 读取第三个标记
            byte flags3 = binaryReader.ReadByte();

            // 6. 读取Times剩余和Vector2[0-1]
            Times[3] = (flags3 & 1) != 0 ? binaryReader.ReadSingle() : 0f;
            Times[4] = (flags3 & 2) != 0 ? binaryReader.ReadSingle() : 0f;
            vector[0].X = (flags3 & 4) != 0 ? binaryReader.ReadSingle() : 0f;
            vector[0].Y = (flags3 & 8) != 0 ? binaryReader.ReadSingle() : 0f;
            vector[1].X = (flags3 & 16) != 0 ? binaryReader.ReadSingle() : 0f;
            vector[1].Y = (flags3 & 32) != 0 ? binaryReader.ReadSingle() : 0f;

            // 7. 读取第四个标记
            byte flags4 = binaryReader.ReadByte();

            // 8. 读取Vector2[2]剩余
            vector[2].X = (flags3 & 64) != 0 ? binaryReader.ReadSingle() : 0f;
            vector[2].Y = (flags4 & 1) != 0 ? binaryReader.ReadSingle() : 0f;

            // 9. 读取动态整数
            npc.lifeMax = (flags2 & 128) != 0 ? binaryReader.ReadInt32() : binaryReader.ReadInt16();
            npc.alpha = (flags3 & 128) != 0 ? binaryReader.ReadInt16() : binaryReader.ReadSByte();
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (Control > 0 || npc.HasBuff(ModContent.BuffType<Fossil>()))
            {
                return false;
            }
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            EntitySource_Parent entitySource_Parent = source as EntitySource_Parent;
            if (entitySource_Parent != null)
            {

                NPC nPC = entitySource_Parent.Entity as NPC;
                if (nPC != null)
                {
                    Master = nPC.whoAmI;
                    nPC.Dnpc().Servant[npc.whoAmI] = true;
                    MasterBool = true;
                    if (Main.netMode == 2)
                    {
                        //DDmod.SyncData(DDType.NPCMaster, npc.whoAmI);
                    }
                }
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (npc.ModNPC != null && npc.ModNPC.Mod == Mod && (npc.boss || npc.Dnpc().BossPhysique))
            {
                npc.damage = (int)(npc.damage * 0.8F);
            }
        }
        public override bool CanHitNPC(NPC npc, NPC target)
        {
            return base.CanHitNPC(npc, target);
        }
        public override void SetDefaults(NPC npc)
        {
            if (npc.boss || npc.NPCHB().MiniBoss || npc.type
            is 13 or 14 or 15 or 36 or 68
            or 114 or 128 or 129 or 130 or 131
            or 135 or 136 or 246 or 247 or 248
            or 249 or 392 or 393 or 394 or 422
            or 493 or 507 or 517 or 564 or 565
            or 576 or 577 or 618 or 476 or 473
            or 474 or 475 or 325 or 327 or 345
            or 346 or 344)
            {
                BossPhysique = true;
            }
            if (npc.type is 27 or 111 or 26 or 29 or 28
                or 73 or 620 or 471 or 30)
            {
                Goblins = true;
            }
            if (Neutrality)
            {
                npc.chaseable = false;

            }
            /*
            if (npc.type == 134)
            {
                npc.width = npc.height = 98;
            }
            if (npc.type == 135)
            {
                npc.width = npc.height = 74;
            }
            if (npc.type == 136)
            {
                npc.width = npc.height = 58;
            }*/

            Properties.SetStaticDefaults(npc);
            Properties.SetLevel(npc);
        }
        public override void SetDefaultsFromNetId(NPC npc)
        {
        }
        public override void Load()
        {
            BubbleTetxture();
        }
        public override void ModifyIncomingHit(NPC npc, ref HitModifiers modifiers)
        {
            if (npc.HasBuff(ModContent.BuffType<Fossil>()))
            {
                modifiers.Knockback *= 0;
            }
            modifiers.Defense.Base -= Penetrate;
            //JS = -0.5F;


        }
        byte JS = 1;
        public void DrawNPC(On_Main.orig_DrawNPC orig, Main main, int NPC, bool Tile)
        {
            orig(main, NPC, Tile);
        }
        public override void AI(NPC npc)
        {
            oldVelocity = npc.position - npc.oldPosition;
            /*
            MoveSpeed *= JS;
            if (JS<1)
            {
                JS += 0.01F;
            }
            else
            {
                JS = 1;
            }*/
            if (!Neutrality && npc.ModNPC != null)
            {
                npc.chaseable = true;
            }
            if (npc.aiStyle == 7)
            {
                if (npc.ai[0] != 5)
                {
                    SittingPoint = Point.Zero;
                    npc.gfxOffY = 0;
                }
                else
                {
                    if (SittingPoint != Point.Zero)
                    {
                        Tile tile = Main.tile[SittingPoint.X, SittingPoint.Y];
                        if (tile.TileType == ModContent.TileType<烈火马桶Tile>())
                        {
                            npc.SitDown(SittingPoint, out int direction, out Vector2 bottom);
                            npc.direction = direction;
                            npc.Bottom = bottom;
                        }
                    }
                }
            }
            //On.Terraria.NPC.AI += DDOn.DDmodOn.NPC_AI;
            /*if(npc.life<npc.lifeMax)
            {
                for (int a = 0; a < Lifes.Length; a++)
                {
                    if (Lifes[a]>0)
                    {
                        int li = npc.lifeMax - npc.life;
                        int ActualLife = 0;
                        if (Lifes[a]>li)
                        {
                            Lifes[a] -= li;
                            ActualLife = li;
                        }
                        else
                        {
                            Lifes[a] -= Lifes[a];
                            ActualLife = Lifes[a];
                        }
                        npc.life += ActualLife;
                        break;
                    }
                }
            }*/
        }
        public override void PostAI(NPC npc)
        {
            //备用
        }
        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if (InvincibleFrame[projectile.owner] > 0 && InvincibleProj[projectile.whoAmI] == 0)
            {
                return true;
            }
            if (npc.Dnpc().Control > 0 && projectile.hostile && (InvincibleFrame[255] <= 0))
            {
                return true;
            }
            if (projectile.type == ModContent.ProjectileType<ZombieArm>() && npc.type == NPCID.Angler)
            {
                return true;
            }
            return base.CanBeHitByProjectile(npc, projectile);
        }
        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            if (InvincibleFrame[player.whoAmI] > 0)
            {
                return false;
            }
            return base.CanBeHitByItem(npc, player, item);
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.Dplayer().ForbidSpawnNPC || DDGlobalTile.ForbidSpawn[Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType])
            {
                foreach (int key in pool.Keys)
                {
                    if (!IgnoreTile[key])
                    {
                        pool[key] = 0;
                    }
                }
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (Main.netMode == 2)
            {
                if (JS == 1)
                {
                    DDmod.SyncData(DDType.NPCMaster, npc.whoAmI);
                }
                if (JS == 5)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
                }
                JS++;
            }
            Defaults = false;
            if (TextTime > 0)
            {
                TextTime--;
                textAl += 0.05F;
            }
            else
            {
                textAl -= 0.05f;
            }
            DDHelper.MaxandMinF(ref textAl, 1, 0);
            if (npc.gfxOffY > 0)
            {
                npc.gfxOffY--;
            }

            if (npc.Dnpc().velocity != Vector2.Zero)
            {
                npc.netUpdate2 = true;
            }
            for (int A = 0; A < InvincibleFrame.Length; A++)
            {
                if (InvincibleFrame[A] > 0)
                {
                    InvincibleFrame[A]--;
                }
                else
                {
                    InvincibleFrame[A] = 0;
                }
                if (InvincibleFrame[A] > 0)
                {
                    npc.immune[A] = InvincibleFrame[A];
                }
            }
            for (int A = 0; A < InvincibleProj.Length; A++)
            {
                Projectile projectile = Main.projectile[A];
                if (InvincibleProj[A] > 0)
                {
                    InvincibleProj[A] = InvincibleFrame[projectile.owner];
                }
            }

            /*
            if (NoBuff)
            {
                for (int a = 0; a < npc.buffImmune.Length; a++)
                {
                    npc.buffImmune[a] = true;
                }
                NoBuff = false;
            }
            for (int A = 0; A < npc.buffImmune.Length; A++)
            {
                if (npc.HasBuff(A) && npc.buffImmune[A])
                {
                    npc.buffTime[npc.FindBuffIndex(A)] = 0;
                }
            }*/
            return base.PreAI(npc);
        }
        /// <summary>
        /// 鞭子效果
        /// </summary>
        public void Whip(NPC npc, Projectile projectile, HitInfo hit, int damageDone)
        {
            if (projectile.DamageType == DamageClass.Summon)
            {
                if (HeartMarker)
                {
                    NewProjectile(projectile.GetSource_FromAI(), npc.Center, new Vector2(0, -5), ModContent.ProjectileType<SummonHeart>(), 0, 0, projectile.owner);
                    if (npc.HasBuff(ModContent.BuffType<BlessingOfTheHeart>()))
                    {
                        npc.buffTime[npc.FindBuffIndex(ModContent.BuffType<BlessingOfTheHeart>())] = 0;
                    }
                }
                if (AcornMarker)
                {
                    NewProjectile(projectile.GetSource_FromAI(), npc.Center, new Vector2(Main.rand.NextFloat(-3, 3), -5), ModContent.ProjectileType<SummonAcorn>(), (int)(hit.SourceDamage * 0.8f), 0, projectile.owner);
                    if (npc.HasBuff(ModContent.BuffType<AcornMarker>()))
                    {
                        npc.buffTime[npc.FindBuffIndex(ModContent.BuffType<AcornMarker>())] = 0;
                    }
                }
                if (MagicStar)
                {
                    float A = Main.rand.NextFloat(-0.5f, 0.5f);
                    NewProjectile(projectile.GetSource_FromAI(), npc.Center - new Vector2(0, 1000).RotatedBy(A), new Vector2(0, 20).RotatedBy(A), ModContent.ProjectileType<SummonStar>(), (int)(hit.SourceDamage * 1.2f), 0, projectile.owner, npc.Center.Y);
                    if (npc.HasBuff(ModContent.BuffType<MagicStar>()))
                    {
                        npc.buffTime[npc.FindBuffIndex(ModContent.BuffType<MagicStar>())] = 0;
                    }
                }
                if (狱火爆炸)
                {
                    NewProjectile(projectile.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<狱火爆炸Proj>(), (int)(hit.SourceDamage * 3), 0, projectile.owner);
                    if (npc.HasBuff(ModContent.BuffType<狱火爆炸>()))
                    {
                        npc.buffTime[npc.FindBuffIndex(ModContent.BuffType<狱火爆炸>())] = 0;
                    }
                }
                if (蘑菇鞭)
                {
                    int proj = NewProjectile(projectile.GetSource_FromAI(), projectile.Center + npc.velocity, Vector2.Zero, ModContent.ProjectileType<蘑菇>(), (int)(hit.SourceDamage * 0.6f), 0, projectile.owner, 1, npc.whoAmI);

                    if (npc.HasBuff(ModContent.BuffType<蘑菇鞭Buff>()))
                    {
                        npc.buffTime[npc.FindBuffIndex(ModContent.BuffType<蘑菇鞭Buff>())] = 0;
                    }
                }
                if (projectile.Player().ActiveItem().type > 0 && projectile.Player().ActiveItem().DItem().SummonLamp)
                {
                    projectile.Player().Dplayer().SummonEnergy += damageDone;
                }
            }
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, HitInfo hit, int damageDone)
        {
            Whip(npc, projectile, hit, damageDone);
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref HitModifiers modifiers)
        {
            InvincibleProj[projectile.whoAmI] = InvincibleFrame[projectile.owner];

            //假设伤害100
            //防御20
            //倍率2
            //如果没有倍率伤害是90
            //有倍率伤害是80
            //damage = (int)(damage - (npc.defense / 2) * projectile.DProj().Magnification) + npc.defense / 2;
            modifiers.DefenseEffectiveness *= projectile.DProj().Magnification;

            if (projectile != null && npc.Dnpc().Control > 0 && projectile.hostile)
            {
                if (projectile.penetrate != 0 && projectile.penetrate != 1)
                {
                    InvincibleFrame[255] = 30;
                    if (projectile.tileCollide)
                    {
                        projectile.penetrate = 0;
                        InvincibleFrame[255] = 0;
                    }
                }
            }
            /*
            modifiers.ModifyHitInfo += Modifiers_ModifyHitInfo;

            void Modifiers_ModifyHitInfo(ref HitInfo info)
            {
                info.Damage = -1;
                info.SourceDamage = -1;
            }*/
        }

        public override void OnKill(NPC npc)
        {
            ///三王后大幅度提高哥布林击杀效率
            if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                int nPCInvasionGroup = GetNPCInvasionGroup(npc.type);
                if (nPCInvasionGroup <= 0 || nPCInvasionGroup != Main.invasionType)
                    return;


                int num10 = NPCID.Sets.InvasionSlotCount[npc.type];
                if (num10 > 0)
                {
                    Main.invasionSize -= num10 * 2;
                    if (Main.invasionSize < 0)
                        Main.invasionSize = 0;

                    if (Main.netMode != 1)
                        Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, nPCInvasionGroup + 3, 0);

                    if (Main.netMode == 2)
                        NetMessage.SendData(78, -1, -1, null, Main.invasionProgress, Main.invasionProgressMax, Main.invasionProgressIcon);
                }
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            ///三王后大幅度提高哥布林生成效率
            if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                if (Main.invasionType == 1)
                {
                    spawnRate = (int)(spawnRate * 0.5f);
                    maxSpawns = maxSpawns * 2;
                }
            }
            if (player.HasBuff(ModContent.BuffType<战争药水Buff>()))
            {
                    spawnRate = (int)(spawnRate * 0.5f);
                    maxSpawns = maxSpawns*2;
            }
            if (player.HasBuff(ModContent.BuffType<纷争药水Buff>()))
            {
                spawnRate = (int)(spawnRate * 0.25f);
                maxSpawns = maxSpawns * 3;
            }
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<Freeze>()) || npc.HasBuff(ModContent.BuffType<Frozen>()))
            {
                drawColor = Lighting.GetColor((int)(npc.Center.X / 16), (int)(npc.Center.Y / 16), new Color(37, 180, 255));
            }
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.ScalingArmorPenetration += 1 - ArmorReduction;
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            if ((npc.aiStyle == 1 && Main.npcFrameCount[npc.type] == 2) && npc.type != ModContent.NPCType<BraveGrassSlime>())
            {
                if (npc.velocity.Y == 0f)
                {
                    if (npc.ai[0] % 1000 > -30 && npc.ai[0] != 0)
                    {
                        float X = -0.2f;
                        if (npc.ai[1] > 0f)
                        {
                            X = -0.15f;
                        }
                        if (npc.ai[0] < -2000)
                        {
                            X = -0.3F;
                            if (npc.ai[1] > 0f)
                            {
                                X = -0.2f;
                            }
                        }
                        if (Times[4] > X)
                        {
                            Times[4] -= -X / 5;
                        }
                        else
                        {
                            Times[4] = X;
                        }
                    }
                    else
                    {
                        if (Times[3] > 10)
                        {
                            Times[3] -= 0.2F;
                            float X = -0.3F;
                            if (npc.ai[1] > 0f)
                            {
                                X = -0.2f;
                            }
                            X *= (Times[3] / 20) * (0.85F) + 0.15f;
                            if (Bool[4])
                            {
                                if (Times[4] > 0)
                                {
                                    if (Times[4] < -X)
                                    {
                                        Times[4] -= X / 5;
                                    }
                                    else
                                    {
                                        Times[4] = -X;
                                        Bool[4] = false;
                                    }
                                }
                                else
                                {
                                    if (Times[4] < -X)
                                    {
                                        Times[4] -= X / 3;
                                    }
                                    else
                                    {
                                        Times[4] = -X;
                                        Bool[4] = false;
                                    }

                                }
                            }
                            else
                            {
                                if (Times[4] > 0)
                                {
                                    if (Times[4] > X)
                                    {
                                        Times[4] -= -X / 5;
                                    }
                                    else
                                    {
                                        Times[4] = X;
                                        Bool[4] = true;
                                    }
                                }
                                else
                                {
                                    if (Times[4] > X)
                                    {
                                        Times[4] -= -X / 3;
                                    }
                                    else
                                    {
                                        Times[4] = X;
                                        Bool[4] = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (npc.ai[1] > 0f)
                            {
                                if (Times[4] > 0)
                                {
                                    DDHelper.BackAndForth(-0.1F, 0.1f, 0.05F, ref Times[4], ref Bool[4]);
                                }
                                else
                                {
                                    DDHelper.BackAndForth(-0.1F, 0.1f, 0.02F, ref Times[4], ref Bool[4]);
                                }
                            }
                            else
                            {

                                if (Times[4] > 0)
                                {
                                    DDHelper.BackAndForth(-0.15F, 0.15f, 0.05F, ref Times[4], ref Bool[4]);
                                }
                                else
                                {
                                    DDHelper.BackAndForth(-0.15F, 0.15f, 0.02F, ref Times[4], ref Bool[4]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Bool[4] = false;
                    if (npc.velocity.Y > 0)
                    {
                        if (Times[3] < 20)
                        {
                            Times[3] += 0.8F;
                        }
                        else
                        {
                            Times[3] = 20F;
                        }
                    }
                    else
                    {
                        Times[3] = 0;
                    }
                    if (Times[4] < 0.1F)
                    {
                        Times[4] += 0.1F;
                    }
                    else
                    {
                        Times[4] = 0.1F;
                    }

                }
                npc.frame.Y = 0;
                if (npc.velocity.Y != 0)
                {
                    npc.frame.Y = frameHeight;
                }
            }
            /*
            if (npc.type == 398)
            {
                if (npc.ai[0] > 0)
                {
                    npc.frame.Y = 100;
                }
            }*/
        }
        public string Text;
        public byte texture;
        public static Asset<Texture2D>[] Bubble;
        public float textAl;
        public byte TextTime;
        public Color BoxColor, TextColor;
        public void BubbleTetxture()
        {
            string T = "DDmod/Textures/TextBubbles/";
            Bubble = [
                ModContent.Request<Texture2D>(T+"默认气泡口"), ModContent.Request<Texture2D>(T + "默认气泡"),
                ModContent.Request<Texture2D>(T+"大地守卫气泡口"), ModContent.Request<Texture2D>(T + "大地守卫气泡"),
                ModContent.Request<Texture2D>(T+"星辰守卫气泡口"), ModContent.Request<Texture2D>(T + "星辰守卫气泡"),
                ];
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D TextBubblesMouth = Bubble[texture * 2].Value;
            Texture2D TextBubbles = Bubble[texture * 2 + 1].Value;
            if (BoxColor == default)
            {
                BoxColor = Color.White;
            }
            if (TextColor == default)
            {
                TextColor = Color.White;
            }
            if (Text != null && Text != "" && textAl > 0)
            {
                DDHelper.DrawTextBubbles(spriteBatch, TextBubblesMouth, TextBubbles,
                    npc.Center, Text, 1, TextColor * textAl, BoxColor * textAl, FontAssets.MouseText.Value, 300, 0);
            }
            /*
            if (npc.type == 400)
            {
                Texture2D texture = TextureAssets.Npc[npc.type].Value;
                spriteBatch.Draw(texture, npc.Center - screenPos, npc.frame, Color.White, npc.rotation, new Vector2(texture.Width / 2, texture.Height / 8), npc.scale, 0, 0f);
                return false;
            }*/
            if (npc.HasBuff(ModContent.BuffType<Freeze>()) && npc.Dnpc().NoMove)
            {
                spriteBatch.Draw(DDTextures.冻结_Glow.Value, npc.position + new Vector2(npc.width / 2, npc.height) - screenPos, null, new Color(55, 55, 55, 0), 0, new Vector2(DDTextures.冻结_Glow.Width() / 2, DDTextures.冻结_Glow.Height() - 40), ((float)npc.width / DDTextures.冻结.Width() + 0.4F) / 4, SpriteEffects.FlipHorizontally, 0f);
                spriteBatch.Draw(DDTextures.冻结.Value, npc.position + new Vector2(npc.width / 2, npc.height) - screenPos, null, new Color(55, 55, 55, 120), 0, new Vector2(DDTextures.冻结.Width() / 2, DDTextures.冻结.Height()), (float)npc.width / DDTextures.冻结.Width() + 0.4F, SpriteEffects.FlipHorizontally, 0f);
            }
            if (npc.HasBuff(ModContent.BuffType<Fossil>()) && npc.Dnpc().NoMove)
            {
                Color LightColor = Lighting.GetColor((int)(npc.Center.X / 16), (int)(npc.Center.Y / 16), new Color(155F, 155F, 255F));
                Main.spriteBatch.End();
                RasterizerState state = new RasterizerState()
                {
                    CullMode = CullMode.CullCounterClockwiseFace,
                    ScissorTestEnable = true,
                };
                if (npc.type == ModContent.NPCType<ChaosBall>())
                {
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, state, null, Main.GameViewMatrix.TransformationMatrix);
                }
                else
                {
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, state, null, Main.GameViewMatrix.TransformationMatrix);
                }
                GameShaders.Misc["渲染滤镜"].UseOpacity(2);
                GameShaders.Misc["渲染滤镜"].SetShaderTexture(DDTextures.化石);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uImageSize1"].SetValue(DDTextures.化石.Size());
                GameShaders.Misc["渲染滤镜"].UseColor(drawColor);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uColor2"].SetValue(LightColor.ToVector3() * 1.25f);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["renderTargetArea"].SetValue(new Vector2(DDTextures.化石.Width(), DDTextures.化石.Height()));
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["uWorldPosition"].SetValue(Vector2.Zero);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["position"].SetValue(Vector2.Zero);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["ImageSize"].SetValue(new Vector2(DDTextures.化石.Width() / 2, DDTextures.化石.Height() / Main.npcFrameCount[npc.type] / 2) * 2);
                GameShaders.Misc["渲染滤镜"].Shader.Parameters["upscaleFactor"].SetValue(new Vector2(0F));
                GameShaders.Misc["渲染滤镜"].Apply();
            }
            if (npc.damage > 0 && npc.aiStyle == 1 && Main.npcFrameCount[npc.type] == 2 /*&& npc.type != ModContent.NPCType<FossilSlime>()*/&& npc.type != ModContent.NPCType<BraveGrassSlime>())
            {
                SpriteEffects sprite = 0;
                if (npc.spriteDirection == 1)
                {
                    sprite = SpriteEffects.FlipHorizontally;
                }
                Color npcColor = npc.GetNPCColorTintedByBuffs(drawColor);
                if (npc.ai[1] > 0f)
                    DDHelper.MethodReflection(Main.instance.GetType(), "DrawNPC_SlimeItem", BindingFlags.NonPublic | BindingFlags.Static).Invoke(Main.instance, new object[] { npc, npc.type, npcColor, 0f });
                Texture2D texture = TextureAssets.Npc[npc.type].Value;

                if (npc.type == 138)
                {
                    spriteBatch.Draw(texture, npc.Center + new Vector2(0, npc.height / 2 + npc.gfxOffY + 2) - screenPos, npc.frame, Color.White, npc.rotation, new Vector2(texture.Width / 2, texture.Height / 2 - 2), npc.scale * new Vector2(1 - Times[4], 1 + Times[4]), 0, 0f);
                    for (int I = 0; I < npc.oldPos.Length; I++)
                    {
                        spriteBatch.Draw(texture, npc.oldPos[I] + npc.Size / 2 + new Vector2(0, npc.height / 2 + npc.gfxOffY + 2) - screenPos, npc.frame, new Color(150, 100, 150, 100) * ((npc.oldPos.Length - I) / (float)npc.oldPos.Length), npc.rotation, new Vector2(texture.Width / 2, texture.Height / 2 - 2), npc.scale * new Vector2(1 - Times[4], 1 + Times[4]), sprite, 0f);
                    }
                }
                if (npc.type == 676)
                {
                    if (!npc.IsABestiaryIconDummy)
                    {
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);
                    }

                    SpriteEffects spriteEffects = SpriteEffects.None;
                    if (npc.spriteDirection == 1)
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    Vector2 halfSize = new Vector2(TextureAssets.Npc[npc.type].Width() / 2, TextureAssets.Npc[npc.type].Height() / Main.npcFrameCount[npc.type] / 2);
                    DrawData value50 = new DrawData(TextureAssets.Npc[npc.type].Value, npc.Center + new Vector2(0, npc.height / 2 + npc.gfxOffY + 2) - screenPos, npc.frame, npc.GetAlpha(npcColor), npc.rotation, new Vector2(texture.Width / 2, texture.Height / 2 - 2), npc.scale * new Vector2(1 - Times[4], 1 + Times[4]), spriteEffects);
                    GameShaders.Misc["RainbowTownSlime"].Apply(value50);
                    value50.Draw(spriteBatch);
                    Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                    if (!npc.IsABestiaryIconDummy)
                    {
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                    }
                }
                else
                {

                    spriteBatch.Draw(texture, npc.Center + new Vector2(0, npc.height / 2 + npc.gfxOffY + 2) - screenPos, npc.frame, npc.GetAlpha(drawColor), npc.rotation, new Vector2(texture.Width / 2, texture.Height / 2 - 2), npc.scale * new Vector2(1 - Times[4], 1 + Times[4]), sprite, 0f);
                    spriteBatch.Draw(texture, npc.Center + new Vector2(0, npc.height / 2 + npc.gfxOffY + 2) - screenPos, npc.frame, npc.GetColor(drawColor), npc.rotation, new Vector2(texture.Width / 2, texture.Height / 2 - 2), npc.scale * new Vector2(1 - Times[4], 1 + Times[4]), sprite, 0f);
                }
                return false;
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<Fossil>()))
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
            if (npc.HasBuff(ModContent.BuffType<Freeze>()))
            {
                spriteBatch.Draw(DDTextures.冻结_Glow.Value, npc.position + new Vector2(npc.width / 2, npc.height) - screenPos, null, new Color(255, 255, 255, 0), 0, new Vector2(DDTextures.冻结_Glow.Width() / 2, DDTextures.冻结_Glow.Height() - 40), ((float)npc.width / DDTextures.冻结.Width() + 0.4F) / 4, 0, 0f);
                spriteBatch.Draw(DDTextures.冻结.Value, npc.position + new Vector2(npc.width / 2, npc.height) - screenPos, null, new Color(255, 255, 255, 120), 0, new Vector2(DDTextures.冻结.Width() / 2, DDTextures.冻结.Height()), (float)npc.width / DDTextures.冻结.Width() + 0.4F, 0, 0f);
            }
            if (npc.realLife == -1 || npc.realLife == npc.whoAmI)
            {
                if (!Main.gameMenu)
                {
                    bool SS = false; ;
                    Players.EntrustPlayer EntrustPlayer = Main.LocalPlayer.GetModPlayer<Players.EntrustPlayer>();
                    if (!npc.SpawnedFromStatue)
                    {
                        for (int a = 0; a < EntrustPlayer.entrust.Length; a++)
                        {
                            if (EntrustPlayer.entrust[a].EntrustNPC != null && EntrustPlayer.entrust[a].type == EntrustID.战斗任务)
                            {
                                if (EntrustPlayer.entrust[a].Accept&& EntrustPlayer.entrust[a].EntrustNPCStack < EntrustPlayer.entrust[a].MaxEntrustNPCStack && EntrustPlayer.entrust[a].EntrustNPCType(EntrustPlayer.entrust[a].EntrustNPC) == EntrustPlayer.entrust[a].EntrustNPCType(npc))
                                {
                                    SS = true;
                                }
                            }
                        }
                    }
                    if (SS)
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮2").Value;
                        spriteBatch.Draw(texture, npc.position + new Vector2(npc.width / 2, -10 - texture.Height / 2) - screenPos, null, Color.White, -MathHelper.PiOver2, texture.Size() / 2, 1, 0, 0f);
                    }
                }
            }
            if (npc.realLife == -1 || npc.realLife == npc.whoAmI)
            {
                if (Battlepet)
                {
                    Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/NPCs/可捕捉葫芦").Value;
                    spriteBatch.Draw(texture, npc.position + new Vector2(npc.width / 2, -10 - texture.Height / 2) - screenPos, null, Color.White, 0, texture.Size() / 2, 1, 0, 0f);
                }
            }
            if (npc.HasBuff(ModContent.BuffType<引雷>()))
            {
                Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/NPCs/闪电").Value;
                spriteBatch.Draw(texture, npc.position + new Vector2(npc.width / 2, -10 - texture.Height / 2) - screenPos, null, Color.White, 0, texture.Size()/2, 0.75F, 0, 0f);
                spriteBatch.Draw(texture, npc.position + new Vector2(npc.width / 2, -10 - texture.Height / 2) - screenPos, null, new Color(155,155,155,0), 0, texture.Size() / 2, 1F, 0, 0f);
            }
            return;
            if (npc.type == 125)
            {
                Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/NPCs/激光眼_Glow").Value;
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (npc.spriteDirection == 1)
                    spriteEffects = SpriteEffects.FlipHorizontally;

                spriteBatch.Draw(texture, npc.Center - screenPos - new Vector2(0, 4), npc.frame, Color.White, npc.rotation, new Vector2(55f, 107f), npc.scale, spriteEffects, 0f);
                spriteBatch.Draw(texture, npc.Center - screenPos - new Vector2(0, 4), npc.frame, new Color(255, 255, 255, 0), npc.rotation, new Vector2(55f, 107f), npc.scale, spriteEffects, 0f);

            }
            if (npc.type == 126)
            {
                Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/NPCs/魔焰眼_Glow").Value;
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (npc.spriteDirection == 1)
                    spriteEffects = SpriteEffects.FlipHorizontally;

                spriteBatch.Draw(texture, npc.Center - screenPos - new Vector2(0, 4), npc.frame, Color.White, npc.rotation, new Vector2(55f, 107f), npc.scale, spriteEffects, 0f);
                spriteBatch.Draw(texture, npc.Center - screenPos - new Vector2(0, 4), npc.frame, new Color(255, 255, 255, 0), npc.rotation, new Vector2(55f, 107f), npc.scale, spriteEffects, 0f);
            }

        }
        /// <summary>
        /// 绘制到所有npc前面(除了肉山墙)
        /// </summary>
        public static void WallDraw(Main Main)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<鬼牙头>()))
            {
                for (int a = 0; a < 200; a++)
                {
                    if (Main.npc[a].active)
                    {
                        float A = (1F - Main.npc[a].alpha / 255F);
                        if (Main.npc[a].type == ModContent.NPCType<鬼牙身>())
                        {
                            Main.spriteBatch.Draw(鬼牙身.Glow2.Value, Main.npc[a].Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * A, Main.npc[a].rotation, new Vector2(鬼牙身.Glow2.Width() / 2, 鬼牙身.Glow2.Height() / 2), Main.npc[a].scale, 0, 0f);
                            continue;
                        }
                        else if (Main.npc[a].type == ModContent.NPCType<鬼牙头>())
                        {
                            Main.spriteBatch.Draw(鬼牙头.Glow2.Value, Main.npc[a].Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * A, Main.npc[a].rotation + MathHelper.PiOver2, new Vector2(鬼牙头.Glow2.Width() / 2, 鬼牙头.Glow2.Height() / 2), Main.npc[a].scale, 0, 0f);
                            continue;
                        }
                        else if (Main.npc[a].type == ModContent.NPCType<鬼牙尾>())
                        {
                            Main.spriteBatch.Draw(鬼牙尾.Glow2.Value, Main.npc[a].Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * A, Main.npc[a].rotation, new Vector2(鬼牙尾.Glow2.Width() / 2, 鬼牙尾.Glow2.Height() / 2), Main.npc[a].scale, 0, 0f);
                            continue;
                        }
                    }
                }
            }
        }
        public override void ModifyHitNPC(NPC npc, NPC target, ref HitModifiers modifiers)
        {
        }
        public override void DrawBehind(NPC npc, int index)
        {
            if (Control > 0 || Control2 > 0)
            {
                Main.instance.DrawCacheNPCProjectiles.Add(index);
                Main.instance.DrawCacheNPCsOverPlayers.Add(index);
            }
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == 17)
            {
                Item shopItem = new(2997);
                shop.Add(new Entry(shopItem, Condition.Multiplayer));
            }
            if (shop.NpcType == 19)
            {
                /*
                Item shopItem = new(ModContent.ItemType<迷你鲨2>());
                shop.Add(new Entry(shopItem, Condition.DownedEowOrBoc));
                */
                Item shopItem = new(98);
                if(shop.TryGetEntry(98,out Entry entry))
                {
                    shop.InsertAfter(entry, new Item(ModContent.ItemType<迷你鲨设计图>()));
                    entry.Disable();
                }
                if(shop.TryGetEntry(97,out entry))
                {
                    entry.AddCondition(Condition.DownedEyeOfCthulhu);
                    shop.InsertBefore(entry,new Item(ModContent.ItemType<受潮弹>()));
                }
            }
        }
        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            for (int S = 0; S < Main.LocalPlayer.Dplayer().shops.Length; S++)
            {
                PlayerShop shop = Main.LocalPlayer.Dplayer().shops[S];
                bool BB = false;
                for (int A = 0; A < items.Length; A++)
                {
                    if (items[A] == null)
                    {
                        continue;
                    }
                    if (items[A].type == shop.ItemType && items[A].shopSpecialCurrency == -1)
                    {
                        items[A].stack = shop.Stack;
                        if (items[A].stack <= 0)
                        {
                            BB = true;
                        }
                    }
                    if (BB && A < items.Length - 1)
                    {
                        items[A] = items[A + 1];
                    }
                }
            }

        }
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            /*
            for (int A = 0; A < nextSlot; A++)
            {
                if (shop[A] == 2267)
                {
                    Main.NewText("河粉来咯");
                }
                if (shop[A] == 2268)
                {
                    Main.NewText("河粉来咯");
                }
            }*/

        }
    }

}