### 修正予定バグ###
- テキストレイアウトの遅れ
    - popup
    - dialog
- Bedに入れない。



### TopDownシーン基本構造###

SceneRoot
├── DDOL_core                 // 常駐オブジェクト（全シーン共通）
│   ├── Main Camera           // カメラ本体
│   ├── Yuji                  // プレイヤー（親）
│   │   ├── TopDown           // TopDown用移動スクリプト
│   │   └── SideScroll        // SideScroll用（Forest専用, 基本無効）
│   │       └── GroundCheck   // 地面接触判定
│   └── GameManager           // Day進行 / Bank / Cash管理など
│
└── SceneSetuper              // シーン固有の初期化/カメラ設定
    ├── Doors                 // 出入口群
    │   ├── Door (Prefab)     // 各シーンの出入口
    │   │   ├── SpawnPoint    // 出現位置
    │   │   └── TriggerPoint  // 判定トリガー
    │   └── ...               // 必要に応じて複数設置
    │
    ├── Ground                // 地面・床のコリジョン
    │
    └── Builds                // シーン固有の建物・壁・オブジェクト
        ├── Build (Prefab)    // 例：House, ShrineObjectなど
        ├── Wall (Prefab)     // 例：RightWall, TopWallなど
        └── ...               // シーンに応じて増減


### 常設UI ###
1. DiarogUI(中央下部で普段非表示。TalkやChoice時に表示)
2. YujiUI（左下から中央。HPとCashとOpenStatusButtonとOpenInventoryButtonを表示）
    1. StatusUI（MoveSpeedや防御力なんかのパラメーターとCloseStatusButton）
    2. InventoryButton（装備とか）
3. ActionUI（右下。ActionoButton有効時に有効Action（Talkとか）を表示）
4. DayUI（左上。GameManagerのDay表示とDayTime表示）
5. BankUI（DayUIの下。GamaManagaerのBank表示）
6. OpenMenuButton（右上。）
    1. MenuUI（終了ボタンだけ？）

### Omen&Punish ###
- 犬(BeyondOmen)
    - 吠える
    - 通過後、追いかけてくる（DayEvilに速度やダメージ比例）
    - 仲間を増やしながら追いかけてくる
    - 通過前に巨大化して追いかけてくる

- ハチの巣付きの木(WallOmen)
    - 犬のシンプル版

- 落とし穴(FloorOmen)
    - しばらく動けなくなる（時間やダメージがDayEvil比例）

- 雷（NonObjectOmenか、地蔵的なWallOmenか）
    - 大き目の半径に予告してから1回降る
    - 小さいのがあちこちに降り注ぐ
    - 確定で直撃する

- クレーター？地上絵？（FloorOmen）
    - 地点通過時にUFOが攫おうとしてくる（遅い）
    - 宇宙人が降ってきて銃で撃ってくる
    - 踏んだ瞬間にUFOに攫われる（一撃死）
    - 
