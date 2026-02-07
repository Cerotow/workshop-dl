# ModVanillaNPC
## 设计用途
用于对原版NPC的AI绘制等代码的修改，使用AutoLoad从而避免在GNPC中写下一堆if else

## 成员
抽象属性：VanillaNPCType，所要修改的原版NPC类型

其他虚函数基本都为GNPC的虚函数去掉了npc的参数（如果需要增加新的函数可以直接在VanillaBoss.cs里面写）


