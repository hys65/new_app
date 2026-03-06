# Localization_Key_Rules

## 1) key 命名规则

- 所有本地化 key 必须使用**小写蛇形命名**（lower_snake_case）。
- 仅允许字符：`a-z`、`0-9`、`_`。
- 禁止使用大写字母、点号、连字符、空格、驼峰命名。
- 建议结构：`模块_语义_对象_属性`。
- 同一语义在不同模块中必须保持前缀一致。

### 正确示例

- `ui_start_game`
- `ui_settings`
- `goal_break_defense_count`
- `enemy_meeting_tyrant_name`

### 禁止示例

- `ui.start.game`
- `UI_StartGame`
- `startGameButton`

---

## 2) 游戏 ID 命名规则

- 所有 ID 使用小写蛇形命名。
- ID 一律以对象类型前缀开头，避免跨系统冲突。
- 推荐前缀：
  - 敌人：`enemy_`
  - 道具：`item_`
  - 状态：`state_`
  - 关卡：`level_`
  - 商品：`sku_`

### 2.1 敌人 ID

- `enemy_meeting_tyrant`
- `enemy_narcissist_manager`

### 2.2 道具 ID

- `item_egg`
- `item_tomato`
- `item_paint_ball`
- `item_foam_sprayer`
- `item_sponge_hammer`

### 2.3 状态 ID

- `state_briefcase_block`
- `state_adjust_glasses`
- `state_sunglasses_paint_resist`
- `state_fix_hair`

### 2.4 关卡 ID

- `level_001`
- `level_002`
- `level_012`

### 2.5 商品 ID

- `sku_remove_ads`
- `sku_starter_pack`
- `sku_gems_120`
- `sku_gems_300`
- `sku_prank_pack_1`
- `sku_paint_pack_1`

---

## 3) key 示例

## 3.1 UI 模块（至少 5）

- `ui_start_game`
- `ui_continue_game`
- `ui_back`
- `ui_confirm`
- `ui_cancel`

## 3.2 MENU 模块（至少 5）

- `menu_title_main`
- `menu_btn_play`
- `menu_btn_shop`
- `menu_btn_settings`
- `menu_btn_exit`

## 3.3 SHOP 模块（至少 5）

- `shop_title`
- `shop_tab_items`
- `shop_tab_bundles`
- `shop_btn_buy`
- `shop_restore_purchase`

## 3.4 SETTINGS 模块（至少 5）

- `settings_title`
- `settings_music`
- `settings_sound`
- `settings_vibration`
- `settings_language`

## 3.5 RESULT 模块（至少 5）

- `result_victory`
- `result_failed`
- `result_score`
- `result_retry`
- `result_next_level`

---

## 4) 游戏内容 key 示例

### 4.1 敌人名称

- `enemy_meeting_tyrant_name`
- `enemy_narcissist_manager_name`

### 4.2 道具名称

- `item_egg_name`
- `item_tomato_name`
- `item_paint_ball_name`
- `item_foam_sprayer_name`
- `item_sponge_hammer_name`

### 4.3 状态名称

- `state_briefcase_block_name`
- `state_adjust_glasses_name`
- `state_sunglasses_paint_resist_name`
- `state_fix_hair_name`

### 4.4 关卡目标

- `goal_break_briefcase`
- `goal_hit_with_egg`
- `goal_hit_with_tomato`
- `goal_strip_sunglasses`
- `goal_reach_breakdown_score`

### 4.5 教程提示

- `tutorial_swipe_to_aim`
- `tutorial_tap_to_throw`
- `tutorial_break_defense_first`
- `tutorial_use_paint_on_face`
- `tutorial_finish_before_time_up`

---

## 5) 错误提示 key（至少 10）

- `error_network`
- `error_server_timeout`
- `error_login_failed`
- `error_purchase_failed`
- `error_purchase_cancelled`
- `error_restore_failed`
- `error_no_internet`
- `error_language_load_failed`
- `error_save_data_failed`
- `error_cloud_sync_failed`
- `error_invalid_level_data`
- `error_unknown`

---

## 6) 示例 ID 汇总

### 敌人
- `enemy_meeting_tyrant`
- `enemy_narcissist_manager`

### 道具
- `item_egg`
- `item_tomato`
- `item_paint_ball`
- `item_foam_sprayer`
- `item_sponge_hammer`

### 状态
- `state_briefcase_block`
- `state_adjust_glasses`
- `state_sunglasses_paint_resist`
- `state_fix_hair`

### 关卡
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

### 商品
- `sku_remove_ads`
- `sku_starter_pack`
- `sku_gems_120`
- `sku_gems_300`
- `sku_prank_pack_1`
- `sku_paint_pack_1`

---

## 7) 示例本地化表（CSV）

```csv
key,zh-CN,en,ja,ru
ui_start_game,开始游戏,Start Game,ゲーム開始,Начать игру
ui_continue_game,继续游戏,Continue,続ける,Продолжить
ui_back,返回,Back,戻る,Назад
ui_confirm,确认,Confirm,確認,Подтвердить
ui_cancel,取消,Cancel,キャンセル,Отмена
menu_title_main,主菜单,Main Menu,メインメニュー,Главное меню
menu_btn_play,开始,Play,プレイ,Играть
menu_btn_shop,商店,Shop,ショップ,Магазин
menu_btn_settings,设置,Settings,設定,Настройки
menu_btn_exit,退出,Exit,終了,Выход
shop_title,商店,Shop,ショップ,Магазин
shop_tab_items,道具,Items,アイテム,Предметы
shop_tab_bundles,礼包,Bundles,バンドル,Наборы
shop_btn_buy,购买,Buy,購入,Купить
shop_restore_purchase,恢复购买,Restore Purchase,購入を復元,Восстановить покупки
settings_title,设置,Settings,設定,Настройки
settings_music,音乐,Music,音楽,Музыка
settings_sound,音效,Sound,サウンド,Звук
settings_vibration,震动,Vibration,バイブ,Вибрация
settings_language,语言,Language,言語,Язык
result_victory,胜利,Victory,勝利,Победа
result_failed,失败,Failed,失敗,Поражение
result_score,得分,Score,スコア,Счет
result_retry,重试,Retry,リトライ,Повторить
result_next_level,下一关,Next Level,次のレベル,Следующий уровень
enemy_meeting_tyrant_name,会议暴君,Meeting Tyrant,会議の暴君,Тиран совещаний
enemy_narcissist_manager_name,自恋主管,Narcissist Manager,ナルシスト上司,Самовлюбленный менеджер
item_egg_name,鸡蛋,Egg,卵,Яйцо
item_tomato_name,番茄,Tomato,トマト,Помидор
item_paint_ball_name,颜料球,Paint Ball,ペイントボール,Шарик с краской
item_foam_sprayer_name,泡沫喷枪,Foam Sprayer,フォームスプレー,Пенный распылитель
item_sponge_hammer_name,海绵锤,Sponge Hammer,スポンジハンマー,Губчатый молот
state_briefcase_block_name,公文包格挡,Briefcase Block,ブリーフケースガード,Блок портфелем
state_adjust_glasses_name,扶正眼镜,Adjust Glasses,メガネ直し,Поправляет очки
state_sunglasses_paint_resist_name,墨镜防颜料,Sunglasses Paint Resist,サングラスで塗料耐性,Солнцезащитные очки защищают от краски
state_fix_hair_name,整理发型,Fix Hair,髪型を整える,Поправляет прическу
goal_break_briefcase,击破公文包防御,Break the Briefcase Defense,ブリーフケース防御を破壊,Сломайте защиту портфеля
goal_hit_with_egg,用鸡蛋命中目标,Hit with Egg,卵で命中させる,Попадите яйцом
goal_hit_with_tomato,用番茄命中目标,Hit with Tomato,トマトで命中させる,Попадите помидором
goal_reach_breakdown_score,达到崩溃值目标,Reach Breakdown Score,崩壊スコアに到達,Достигните порога срыва
tutorial_swipe_to_aim,滑动进行瞄准,Swipe to Aim,スワイプで狙う,Проведите для прицеливания
tutorial_tap_to_throw,点击投掷道具,Tap to Throw,タップで投げる,Нажмите, чтобы бросить
error_network,网络错误,Network Error,ネットワークエラー,Ошибка сети
error_login_failed,登录失败,Login Failed,ログイン失敗,Ошибка входа
error_purchase_failed,购买失败,Purchase Failed,購入失敗,Ошибка покупки
error_no_internet,无网络连接,No Internet Connection,インターネット接続なし,Нет подключения к интернету
```
