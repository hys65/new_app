# Power Prank 3D

## 1) 项目简介
Power Prank 3D 是一个 **3D 休闲解压 + 轻策略互动 + 轻度收集** 移动游戏。

## 2) 首版范围
- 1 个主玩法场景流
- 12 个关卡（`level_001` ~ `level_012`）
- 敌人：`enemy_meeting_tyrant`、`enemy_narcissist_manager`
- 道具：`item_egg`、`item_tomato`、`item_paint_ball`、`item_foam_sprayer`、`item_sponge_hammer`
- 敌人状态：`state_briefcase_block`、`state_adjust_glasses`、`state_sunglasses_paint_resist`、`state_fix_hair`
- 10 语言本地化框架（首版）

## 3) 客户端目录说明（Unity）
- `unity-client/Assets/Scenes/`：场景文件
- `unity-client/Assets/Scripts/`：游戏逻辑脚本
- `unity-client/Assets/Localization/`：本地化文本与配置资源

## 4) 命名规范摘要
- 统一小写蛇形命名：`snake_case`
- 业务 ID 统一 `snake_case`（示例）：
  - `enemy_meeting_tyrant`
  - `item_paint_ball`
  - `state_sunglasses_paint_resist`
  - `level_001`
  - `sku_starter_pack`

## 5) Localization
- 正式本地化资源路径：`unity-client/Assets/Localization/localization_table.csv`
- key 命名规则：仅允许 `snake_case`（如 `ui_start_game`），禁止点分 key。
- 支持语言：`zh-CN`、`zh-TW`、`en`、`ja`、`ko`、`de`、`fr`、`es`、`pt-BR`、`ru`。
- fallback 规则：`LocalizationManager` 固定回退到 `en`（`default_locale=en`，`fallback_locale=en`）。
- 字体分组：
  - `Latin` → `en`、`de`、`fr`、`es`、`pt-BR`
  - `CJK` → `zh-CN`、`zh-TW`、`ja`、`ko`
  - `Cyrillic` → `ru`

## 6) 本地启动说明
1. 用指定 Unity 版本打开 `unity-client/`
2. 将 `LocalizationManager` 挂到启动场景对象，运行后可调用 `SetLocale(locale)` 与 `GetText(key)`。

## 7) 新成员注意事项
1. 不新增首版范围外模块。
2. 任何新 key 与 ID 必须使用 `snake_case`。
3. 不允许出现点分 key（例如 `ui.button.start`）。
4. 文档与资源命名保持一致，不使用临时本地化文件名。
