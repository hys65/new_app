# Power Prank 3D 首版最小可用项目结构与命名规范

## 1) Unity 项目目录树（最少够用）
```text
unity-client/
├─ Assets/
│  ├─ Localization/
│  │  ├─ localization_table.csv
│  │  ├─ LocalizationManager.cs
│  │  ├─ LocaleConfig.json
│  │  └─ FontConfig.json
│  ├─ Scenes/
│  └─ Scripts/
├─ Packages/
└─ ProjectSettings/
```

## 2) 步骤 2：多语言框架先行（验收基线）

### 2.1 正式资源命名
- 本地化正式资源文件名固定为：`localization_table.csv`。
- 不允许出现 `localization_table_sample.csv` 这类临时命名。

### 2.2 10 语言框架固定列表
- `zh-CN`
- `zh-TW`
- `en`
- `ja`
- `ko`
- `de`
- `fr`
- `es`
- `pt-BR`
- `ru`

### 2.3 本地化运行骨架文件说明
- `LocalizationManager.cs`：
  - 加载 `localization_table.csv`
  - 按 `key` 获取文本
  - 设置当前语言
  - 缺失时按 `en` 回退
- `LocaleConfig.json`：记录支持语言、默认语言、回退语言与系统语言自动检测开关。
- `FontConfig.json`：记录语言到字体组的映射规则。

### 2.4 LocaleConfig 规范
```json
{
  "supported_locales": ["zh-CN", "zh-TW", "en", "ja", "ko", "de", "fr", "es", "pt-BR", "ru"],
  "default_locale": "en",
  "fallback_locale": "en",
  "auto_detect_system_language": true
}
```

### 2.5 FontConfig 规范
- `Latin`：`en`, `de`, `fr`, `es`, `pt-BR`
- `CJK`：`zh-CN`, `zh-TW`, `ja`, `ko`
- `Cyrillic`：`ru`
- 运行时根据 locale 选择对应字体组；未命中时回退 `Latin`。

### 2.6 key 命名规范
- 统一使用 `snake_case`。
- 禁止点分命名、驼峰命名、中文 key。
- 示例：`ui_start_game`、`menu_btn_play`、`item_egg_name`。

### 2.7 fallback 规则
- 客户端与配置统一固定：`fallback_locale = en`。
- 任意 key 在当前语言缺失时，必须回退读取 `en`。
