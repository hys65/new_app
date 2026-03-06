# Power Prank 3D

## 1) 项目简介
Power Prank 3D 是一个 **3D 休闲解压 + 轻策略互动 + 轻度收集** 移动游戏。

## 2) 首版范围
- 1 个主玩法场景流
- 12 个关卡（`level_001` ~ `level_012`）
- 敌人：`enemy_meeting_tyrant`、`enemy_narcissist_manager`
- 道具：`item_egg`、`item_tomato`、`item_paint_ball`、`item_foam_sprayer`、`item_sponge_hammer`
- 敌人状态：`state_briefcase_block`、`state_adjust_glasses`、`state_sunglasses_paint_resist`、`state_fix_hair`
- 10 语言本地化框架（仅框架与键规范）

## 3) 客户端目录说明（Unity）
- `unity-client/Assets/Scenes/`：场景文件
- `unity-client/Assets/Scripts/`：游戏逻辑脚本
- `unity-client/Assets/Localization/`：本地化文本资源
- `unity-client/Assets/Prefabs/`：预制体
- `unity-client/Assets/UI/`：UI 资源
- `unity-client/Assets/Art/`：美术资源
- `unity-client/Assets/Audio/`：音频资源
- `unity-client/Assets/ScriptableObjects/`：配置资产
- `unity-client/ProjectSettings/`：Unity 项目设置

## 4) 服务端目录说明（NestJS）
- `game-server/src/modules/auth/`：登录鉴权
- `game-server/src/modules/player/`：玩家基础数据
- `game-server/src/modules/level/`：关卡进度
- `game-server/src/modules/inventory/`：道具库存
- `game-server/src/modules/shop/`：商品与购买
- `game-server/src/config/`：运行配置
- `game-server/prisma/`：数据库模型与迁移

## 5) 命名规范摘要
- 统一小写蛇形命名：`snake_case`
- 场景固定：
  - `boot_scene`
  - `login_scene`
  - `lobby_scene`
  - `game_scene`
  - `shop_scene`
  - `settings_scene`
- 业务 ID 统一 `snake_case`（示例）：
  - `enemy_meeting_tyrant`
  - `item_paint_ball`
  - `state_sunglasses_paint_resist`
  - `level_001`
  - `sku_starter_pack`

## 6) 本地化规则摘要
- 只允许 `snake_case` key，不允许点分写法
- 正确示例：
  - `ui_start_game`
  - `ui_settings`
  - `item_egg_name`
  - `enemy_meeting_tyrant_name`
  - `state_briefcase_block_name`
  - `goal_break_defense_count`

## 7) 本地启动说明
### 7.1 Unity 客户端
1. 用指定 Unity 版本打开 `unity-client/`
2. 默认启动入口：'Assets/Scenes/boot_scene.unity'
3. 点击 Play

### 7.2 NestJS 服务端
1. `cd game-server`
2. `npm install`（占位）
3. 配置 `.env`（占位）
4. `npm run start:dev`（占位）

## 8) 首版明确不做
- PVP
- 抽卡
- 战令
- 月卡
- 公会
- 排行榜
- 社交系统

## 9) 新成员注意事项
1. 不新增首版范围外模块。
2. 任何新 key 与 ID 必须使用 `snake_case`。
3. 不允许出现点分 key（例如 `ui.button.start`）。
4. `README.md` 与 `PROJECT_BOOTSTRAP_GUIDE.md` 同步更新，保持一致。
