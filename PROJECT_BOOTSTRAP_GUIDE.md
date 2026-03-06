# Power Prank 3D - 步骤1：项目目录与命名规范冻结

## 最终建议
- 本仓库已落地最小工程骨架：`unity-client/` 与 `game-server/`。
- 本文档与 `README.md` 完全对齐，作为步骤1冻结基线。
- 首版只覆盖既定范围，不引入未确认系统。

---

## 1) 已落地的项目目录结构

### 1.1 Unity 客户端（已创建）
```text
unity-client/
├─ Assets/
│  ├─ Scenes/
│  ├─ Scripts/
│  ├─ Localization/
│  ├─ Prefabs/
│  ├─ UI/
│  ├─ Art/
│  ├─ Audio/
│  └─ ScriptableObjects/
└─ ProjectSettings/
```

### 1.2 NestJS 服务端（已创建）
```text
game-server/
├─ src/
│  ├─ modules/
│  │  ├─ auth/
│  │  ├─ player/
│  │  ├─ level/
│  │  ├─ inventory/
│  │  └─ shop/
│  └─ config/
└─ prisma/
```

---

## 2) 命名规范冻结（统一小写蛇形）

### 2.1 通用规则
- 统一使用小写蛇形：`snake_case`
- 不允许同一概念出现多套命名
- 不允许点分 key 命名

### 2.2 场景命名（固定集合）
- `boot_scene`
- `login_scene`
- `lobby_scene`
- `game_scene`
- `shop_scene`
- `settings_scene`

### 2.3 本地化 key 命名（仅 snake_case）
正确示例：
- `ui_start_game`
- `ui_settings`
- `item_egg_name`
- `enemy_meeting_tyrant_name`
- `state_briefcase_block_name`
- `goal_break_defense_count`

禁止示例：
- `ui.button.start`
- `enemy.meeting_tyrant.name`
- `state.briefcase.block.name`

### 2.4 业务 ID 命名（仅 snake_case）

#### 敌人
- `enemy_meeting_tyrant`
- `enemy_narcissist_manager`

#### 道具
- `item_egg`
- `item_tomato`
- `item_paint_ball`
- `item_foam_sprayer`
- `item_sponge_hammer`

#### 状态
- `state_briefcase_block`
- `state_adjust_glasses`
- `state_sunglasses_paint_resist`
- `state_fix_hair`

#### 关卡
- `level_001`
- `level_002`
- `level_003`
- `level_004`
- `level_005`
- `level_006`
- `level_007`
- `level_008`
- `level_009`
- `level_010`
- `level_011`
- `level_012`

#### 商品（首版真实商品结构，冻结）
- `sku_remove_ads`
- `sku_starter_pack`
- `sku_gems_120`
- `sku_gems_300`
- `sku_prank_pack_1`
- `sku_paint_pack_1`

#### 语言
- `lang_zh_cn`
- `lang_zh_tw`
- `lang_en_us`
- `lang_ja_jp`
- `lang_ko_kr`
- `lang_de_de`
- `lang_fr_fr`
- `lang_es_es`
- `lang_pt_br`
- `lang_id_id`

---

## 3) 首版范围冻结
- 客户端：Unity
- 服务端：Node.js + NestJS
- 数据库：PostgreSQL
- 缓存：Redis
- 首版敌人：会议暴君、自恋主管
- 首版基础道具：鸡蛋、番茄、颜料球、泡沫喷枪、海绵锤
- 首版敌人状态：公文包格挡、扶正眼镜、墨镜防颜料、整理发型
- 首版内容量：1个场景流程、12关、10语言框架
- 运行时不使用 AI，不调用 API Key

---

## 4) 首版明确不做
- PVP
- 抽卡
- 战令
- 月卡
- 公会
- 排行榜
- 社交

---

## 5) 验收检查清单（步骤1）
1. 根目录存在：`README.md`、`PROJECT_BOOTSTRAP_GUIDE.md`、`unity-client/`、`game-server/`
2. Unity 必需目录存在。
3. NestJS 必需目录存在。
4. 场景命名符合固定集合。
5. 本地化 key 全部为 `snake_case`。
6. ID 与商品 ID 符合冻结清单。
