# Power Prank 3D 首版最小可用项目结构与命名规范

## 最终建议
- 采用“双仓并行、单文档统一规范”的最小方案：`unity-client` 与 `game-server` 两个目录，命名规则统一用 `snake_case`（ID、资源）+ `PascalCase`（C# 类型）+ `kebab-case`（NestJS 文件名）。
- 首版只覆盖：1 场景、12 关、2 敌人、5 道具、4 状态、10 语言本地化框架，不预埋 PVP/社交/抽卡/排行榜模块。
- 服务端只保留首版必要能力：账号设备标识、关卡进度、基础背包、商品配置、本地化文本下发（可选静态）。
- 统一 ID 规则后直接落库，避免后续“字符串 ID 与数值 ID 混用”导致返工。

---

## 1) Unity 项目目录树（最少够用，细化到二级）
```text
unity-client/
├─ Assets/
│  ├─ Art/
│  ├─ Audio/
│  ├─ Config/
│  ├─ Localization/
│  ├─ Materials/
│  ├─ Prefabs/
│  ├─ Resources/
│  ├─ Scenes/
│  ├─ Scripts/
│  ├─ UI/
│  └─ VFX/
├─ Packages/
├─ ProjectSettings/
└─ UserSettings/
```

### Unity `Assets/Scripts` 建议子结构（首版够用）
```text
Assets/Scripts/
├─ Core/              # 启动、全局配置、事件总线（仅基础）
├─ Gameplay/          # 关卡流程、敌人行为、道具效果
├─ Combat/            # 受击、状态处理、碰撞结果
├─ Collect/           # 轻度收集（解锁与计数）
├─ UI/                # HUD、结算、暂停
├─ Localization/      # 本地化读取与切换
├─ Data/              # ScriptableObject 与静态配置结构
└─ Network/           # 与 NestJS 的最小接口封装
```

---

## 2) Node.js / NestJS 项目目录树（最少够用，细化到二级）
```text
game-server/
├─ src/
│  ├─ app.module.ts
│  ├─ main.ts
│  ├─ common/
│  ├─ config/
│  ├─ modules/
│  └─ prisma/
├─ prisma/
│  ├─ schema.prisma
│  └─ migrations/
├─ test/
├─ package.json
├─ tsconfig.json
└─ .env.example
```

### `src/modules` 建议子结构（首版够用）
```text
src/modules/
├─ auth/          # 设备登录/游客登录（最小）
├─ player/        # 玩家基础信息
├─ level/         # 12关进度与结算记录
├─ inventory/     # 基础道具库存
├─ enemy/         # 敌人与状态配置读取
├─ shop/          # 首版商品配置与购买校验
└─ localization/  # 10语言文本下发（或版本号）
```

---

## 3) C# 命名规范（Unity）

### 规则
- 类/结构体/接口/枚举：`PascalCase`
- 方法：`PascalCase`
- 公共字段与属性：`PascalCase`
- 私有字段：`_camelCase`
- 局部变量/参数：`camelCase`
- 常量：`UPPER_SNAKE_CASE`
- 脚本文件名必须与类名一致

### 示例（至少 5 个）
- 类名：`EnemyStateController`
- 类名：`LevelProgressService`
- 接口名：`IThrowItemEffect`
- 枚举名：`EnemyStateType`
- 方法名：`ApplyPaintBallHit()`
- 私有字段：`_currentDurability`
- 属性名：`CurrentLevelId`
- 常量：`MAX_THROW_COUNT`

---

## 4) TypeScript / NestJS 命名规范

### 规则
- 文件名：`kebab-case`（例如 `level-progress.service.ts`）
- 类名：`PascalCase`
- 变量/方法：`camelCase`
- 常量：`UPPER_SNAKE_CASE`
- DTO：`xxx.dto.ts` + `PascalCase` 类名
- Controller/Service/Module 后缀必须保留

### 示例（至少 5 个）
- 文件名：`enemy-config.controller.ts`
- 文件名：`inventory.service.ts`
- 类名：`LevelProgressService`
- 类名：`CreateOrderDto`
- 方法名：`getPlayerInventory()`
- 变量名：`currentLanguage`
- 常量：`DEFAULT_LANGUAGE`

---

## 5) 本地化 key 命名规范

### 规则
- 统一：`domain.section.item`（全小写 + 点分）
- 不使用中文、不使用空格
- 动词统一前缀：按钮文本优先 `ui.button.xxx`
- 敌人、道具、状态统一挂在固定域下

### 示例（至少 5 个）
- `ui.button.start`
- `ui.button.retry`
- `ui.result.victory_title`
- `enemy.meeting_tyrant.name`
- `enemy.narcissist_manager.name`
- `item.egg.name`
- `item.foam_sprayer.desc`
- `state.briefcase_block.name`
- `state.sunglasses_paint_guard.desc`
- `level.lvl_001.title`

---

## 6) ID 命名规则（敌人、道具、状态、关卡、商品、语言）

### 统一规则
- 全部使用 `snake_case` 字符串 ID
- 前缀固定：
  - 敌人：`enemy_`
  - 道具：`item_`
  - 状态：`state_`
  - 关卡：`lvl_`
  - 商品：`sku_`
  - 语言：`lang_`
- 关卡编号固定三位：`lvl_001` ~ `lvl_012`

### 示例（至少 5 个，按类别给）
- 敌人：`enemy_meeting_tyrant`、`enemy_narcissist_manager`
- 道具：`item_egg`、`item_tomato`、`item_paint_ball`、`item_foam_sprayer`、`item_sponge_hammer`
- 状态：`state_briefcase_block`、`state_adjust_glasses`、`state_sunglasses_paint_guard`、`state_fix_hair`
- 关卡：`lvl_001`、`lvl_002`、`lvl_010`、`lvl_011`、`lvl_012`
- 商品：`sku_coin_small`、`sku_coin_medium`、`sku_item_bundle_basic`、`sku_remove_ads`
- 语言：`lang_zh_cn`、`lang_zh_tw`、`lang_en_us`、`lang_ja_jp`、`lang_ko_kr`

---

## 7) README 最小模板（首版）

```md
# Power Prank 3D

## 1. 项目简介
- 类型：3D 休闲解压 + 轻策略互动 + 轻度收集
- 首版范围：1 场景、12 关、10 语言

## 2. 仓库结构
- `unity-client/`：Unity 客户端
- `game-server/`：NestJS 服务端

## 3. 本地运行
### 3.1 Unity
1. 使用指定 Unity 版本打开 `unity-client`
2. 打开 `Assets/Scenes/main_scene.unity`
3. 运行 Play

### 3.2 Server
1. `cd game-server`
2. `npm install`
3. 配置 `.env`
4. `npm run start:dev`

## 4. 命名规范
- C#：PascalCase + `_camelCase`
- TS/NestJS：kebab-case 文件名 + PascalCase 类名
- ID：统一 snake_case 字符串（如 `item_egg`）

## 5. 首版功能清单
- 敌人：会议暴君、自恋主管
- 道具：鸡蛋、番茄、颜料球、泡沫喷枪、海绵锤
- 状态：公文包格挡、扶正眼镜、墨镜防颜料、整理发型

## 6. 首版明确不做
- PVP、抽卡、战令、月卡、公会、排行榜、社交
```

---

## 8) 统一命名词表（避免一套概念多版本）
- 会议暴君：`meeting_tyrant`（不要再出现 `meeting_bully`）
- 自恋主管：`narcissist_manager`（不要再出现 `ego_manager`）
- 颜料球：`paint_ball`（不要再出现 `color_ball`）
- 泡沫喷枪：`foam_sprayer`（不要再出现 `foam_gun`）
- 公文包格挡：`briefcase_block`（不要再出现 `briefcase_guard`）

---

## 9) 本方案中容易导致后续返工的 3 个风险点
1. **ID 规则如果首版不锁死**：后续会出现配置表、存档、数据库字段不一致（如 `lvl1` / `level_1` / `lvl_001` 混用），迁移成本高。
2. **本地化 key 如果夹杂展示文案**：例如把中文直接当 key，后续 10 语言扩充会批量替换，且容易产生重复 key。
3. **客户端与服务端命名域不一致**：Unity 用 `paint_ball`，服务端用 `color_ball`，会导致结算、背包、商品映射频繁写兼容层。
