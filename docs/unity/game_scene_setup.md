“当前仓库中不包含 .unity 二进制场景资源，需在本地 Unity Editor 中按本文手工创建。”
# Power Prank 3D：Unity 手工搭建操作清单（15~20 分钟）

> 目标：在 Unity Editor 内手工创建 `Assets/Scenes/game_scene.unity`，并跑通“拖拽投掷 → 命中敌人 → 崩坏值增长 → 结算 → Retry 重开”最小玩法闭环。

---

## 1) Unity 菜单操作步骤（先建场景）

1. 打开 Unity 项目（`unity-client`）。
2. 在顶部菜单点击：`File → New Scene`。
3. 在顶部菜单点击：`File → Save As...`。
4. 保存路径选择：`Assets/Scenes/`。
5. 文件名输入：`game_scene.unity`。
6. 点击保存。

---

## 2) 完整 Hierarchy 结构（照着搭）

```text
game_scene
├─ Systems
│  ├─ GameplayManager
│  ├─ ThrowController
│  └─ LocalizationManager
├─ Gameplay
│  ├─ MainCamera: (0, 1.8, -6)，Rotation (10, 0, 0)
│  ├─ ThrowSpawnPoint: (0, 1.2, -3.5)
│  ├─ EnemyRoot: (0, 1, 2.5)
│  │  └─ EnemyVisual
│  └─ Ground
└─ UI
   ├─ EventSystem
   └─ HUDCanvas
      ├─ HudPanel
      │  ├─ CurrentBreakdownText
      │  ├─ TargetBreakdownText
      │  ├─ TimerText
      │  └─ SelectedItemText
      └─ ResultPanel
         ├─ ResultTitleText
         └─ RetryButton
            └─ Text
```

---

## 3) 每个对象如何创建（Unity 菜单逐步）

### 3.1 根分组对象
1. `GameObject → Create Empty`，重命名为 `Systems`。
2. `GameObject → Create Empty`，重命名为 `Gameplay`。
3. `GameObject → Create Empty`，重命名为 `UI`。

### 3.2 Systems 下对象
1. 选中 `Systems`，`GameObject → Create Empty`，命名 `GameplayManager`。
2. 选中 `Systems`，`GameObject → Create Empty`，命名 `ThrowController`。
3. 选中 `Systems`，`GameObject → Create Empty`，命名 `LocalizationManager`。

### 3.3 Gameplay 下对象
1. `GameObject → Camera`，重命名 `MainCamera`，拖到 `Gameplay` 下。
2. 选中 `Gameplay`，`GameObject → Create Empty`，命名 `ThrowSpawnPoint`。
3. 选中 `Gameplay`，`GameObject → Create Empty`，命名 `EnemyRoot`。
4. 选中 `EnemyRoot`，`GameObject → 3D Object → Capsule`，重命名 `EnemyVisual`。
5. 选中 `Gameplay`，`GameObject → 3D Object → Plane`，重命名 `Ground`。

### 3.4 UI 下对象
1. `GameObject → UI → Canvas`，重命名 `HUDCanvas`，拖到 `UI` 下。
2. `GameObject → UI → Event System`，拖到 `UI` 下并命名 `EventSystem`（已有则不重复创建）。
3. 选中 `HUDCanvas`，`GameObject → UI → Panel`，重命名 `HudPanel`。
4. 在 `HudPanel` 下依次创建 4 个文本：
   - `GameObject → UI → Text` → `CurrentBreakdownText`
   - `GameObject → UI → Text` → `TargetBreakdownText`
   - `GameObject → UI → Text` → `TimerText`
   - `GameObject → UI → Text` → `SelectedItemText`
5. 选中 `HUDCanvas`，`GameObject → UI → Panel`，重命名 `ResultPanel`。
6. 在 `ResultPanel` 下创建：
   - `GameObject → UI → Text` → `ResultTitleText`
   - `GameObject → UI → Button` → `RetryButton`（按钮子对象 `Text` 保留）

---

## 4) 每个对象要挂的脚本

1. `Systems/GameplayManager`
   - `Add Component` → `GameplayManager`
2. `Systems/ThrowController`
   - `Add Component` → `ThrowController`
3. `Systems/LocalizationManager`
   - `Add Component` → `LocalizationManager`
4. `Gameplay/EnemyVisual`
   - `Add Component` → `EnemyHitReaction`
   - 保留或添加 `Capsule Collider`
   - 保留 `Mesh Renderer`
5. `UI/HUDCanvas`
   - `Add Component` → `HudController`
6. `Gameplay/MainCamera`
   - 确保 Tag 为 `MainCamera`

---

## 5) Inspector 字段拖拽绑定（必须一项不漏）

### 5.1 ThrowController（`Systems/ThrowController`）
- `gameplayManager` → 拖入 `Systems/GameplayManager`
- `gameplayCamera` → 拖入 `Gameplay/MainCamera`
- `throwSpawnPoint` → 拖入 `Gameplay/ThrowSpawnPoint`
- `upwardFactor` → 保持默认 `0.25`

### 5.2 EnemyHitReaction（`Gameplay/EnemyVisual`）
- `targetRenderer` → 拖入 `Gameplay/EnemyVisual`（它的 Renderer）
- `visualRoot` → 拖入 `Gameplay/EnemyVisual`
- `reactionDuration` → 保持默认 `0.2`

### 5.3 HudController（`UI/HUDCanvas`）
- `gameplayManager` → `Systems/GameplayManager`
- `localizationManager` → `Systems/LocalizationManager`
- `currentBreakdownText` → `UI/HUDCanvas/HudPanel/CurrentBreakdownText`
- `targetBreakdownText` → `UI/HUDCanvas/HudPanel/TargetBreakdownText`
- `timerText` → `UI/HUDCanvas/HudPanel/TimerText`
- `selectedItemText` → `UI/HUDCanvas/HudPanel/SelectedItemText`
- `resultPanel` → `UI/HUDCanvas/ResultPanel`
- `resultTitleText` → `UI/HUDCanvas/ResultPanel/ResultTitleText`
- `retryButton` → `UI/HUDCanvas/ResultPanel/RetryButton`

### 5.4 GameplayManager（`Systems/GameplayManager`）
- `targetBreakdownValue` → `100`
- `roundDurationSeconds` → `45`
- `defaultItemIndex` → `0`
- `itemList` → 第 8 节配置完成后再回填

---

## 6) Projectile prefab 创建步骤

1. 在 `Project` 面板中确认有文件夹：`Assets/Prefabs/`（没有就右键 `Create → Folder` 新建）。
2. `Hierarchy`：`GameObject → 3D Object → Sphere`，命名 `projectile_basic`。
3. 选中 `projectile_basic`，Inspector 确认/添加组件：
   - `Transform`（默认）
   - `Sphere Collider`（默认）
   - `Rigidbody`（`Use Gravity = true`）
   - `ProjectileBehavior`（`Add Component` 添加）
4. 将 `Hierarchy` 中 `projectile_basic` 拖入 `Project/Assets/Prefabs/`。
5. 生成预制体后，`Project` 中命名为：`projectile_basic.prefab`。
6. 可删除 Hierarchy 中临时 `projectile_basic`（Prefab 已保存）。

---

## 7) GameplayItemData 创建步骤（5 个）

1. 在 `Project` 面板确保有目录：`Assets/ScriptableObjects/GameplayItems/`（没有就创建）。
2. 在该目录空白处右键：
   - `Create → PowerPrank3D → Gameplay Item Data`
3. 连续创建 5 个资源并重命名：
   - `item_egg`
   - `item_tomato`
   - `item_paint_ball`
   - `item_foam_sprayer`
   - `item_sponge_hammer`
4. 逐个点开并设置（字段名按 Inspector 原样）：

- `item_egg`
  - `itemId = item_egg`
  - `displayKey = item_egg_name`
  - `baseBreakdownScore = 10`
  - `feedbackType = ScalePunch`
  - `projectilePrefab = projectile_basic.prefab`
  - `throwForce = 14`

- `item_tomato`
  - `itemId = item_tomato`
  - `displayKey = item_tomato_name`
  - `baseBreakdownScore = 15`
  - `feedbackType = FlashColor`
  - `projectilePrefab = projectile_basic.prefab`
  - `throwForce = 13`

- `item_paint_ball`
  - `itemId = item_paint_ball`
  - `displayKey = item_paint_ball_name`
  - `baseBreakdownScore = 18`
  - `feedbackType = FoamTint`
  - `projectilePrefab = projectile_basic.prefab`
  - `throwForce = 12`

- `item_foam_sprayer`
  - `itemId = item_foam_sprayer`
  - `displayKey = item_foam_sprayer_name`
  - `baseBreakdownScore = 8`
  - `feedbackType = Wiggle`
  - `projectilePrefab = projectile_basic.prefab`
  - `throwForce = 16`

- `item_sponge_hammer`
  - `itemId = item_sponge_hammer`
  - `displayKey = item_sponge_hammer_name`
  - `baseBreakdownScore = 22`
  - `feedbackType = SmallKnockback`
  - `projectilePrefab = projectile_basic.prefab`
  - `throwForce = 10`

---

## 8) GameplayManager 的 itemList 配置

1. 选中 `Systems/GameplayManager`。
2. 在 Inspector 找到 `GameplayManager` 组件的 `itemList`。
3. 设置 `Size = 5`。
4. 按顺序拖入 5 个 `GameplayItemData`：
   - Element 0 → `item_egg`
   - Element 1 → `item_tomato`
   - Element 2 → `item_paint_ball`
   - Element 3 → `item_foam_sprayer`
   - Element 4 → `item_sponge_hammer`

---

## 9) HUD UI 创建与绑定步骤（最小可用）

1. `HUDCanvas` 推荐使用 `Screen Space - Overlay`。
2. `HudPanel`：放在屏幕左上区域。
3. 四个 Text 初始内容可随意（运行后脚本会覆盖）：
   - `CurrentBreakdownText`
   - `TargetBreakdownText`
   - `TimerText`
   - `SelectedItemText`
4. `ResultPanel`：
   - 编辑器中建议默认隐藏
   - 运行时由 HudController 控制显示
   - `ResultTitleText` 用于显示胜利/失败
   - `RetryButton` 文本保留默认即可（运行时会被本地化文本覆盖）
5. 回到第 5.3 节，确认 `HudController` 所有引用都已绑定。

---

## 10) 最终运行步骤（玩法闭环验证）

1. 打开 `Assets/Scenes/game_scene.unity`。
2. 点击 Unity 顶部 `Play`。
3. 在 Game 视图中按住鼠标左键拖动，松开后投掷。
4. 投掷物命中 `EnemyRoot/EnemyVisual`。
5. 命中后观察：
   - 崩坏值增加（`CurrentBreakdown`）
   - HUD 文本实时更新
   - 敌人出现受击反馈（缩放/变色/抖动等）
6. 持续投掷直到：
   - 达到目标值（胜利），或
   - 倒计时归零（失败）
7. `ResultPanel` 出现后，点击 `RetryButton`。
8. 场景重开，新一局可再次投掷。

---

## 11) 验收录屏清单（必须全部覆盖）

录屏中必须出现以下 8 项：

1. 进入 `game_scene`（Scene 或 Game 视图可识别）。
2. 鼠标拖拽并松手投掷。
3. 投掷物命中敌人。
4. 崩坏值增加（Current 数值变化）。
5. HUD 变化（至少看到倒计时和当前道具/目标值更新）。
6. 倒计时持续减少。
7. 出现一局结果（胜利或失败任一即可）。
8. 点击 `Retry` 后成功重开并可再次投掷。
