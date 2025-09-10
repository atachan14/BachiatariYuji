### 修正予定バグ###


### 開発予定 ###
- Title周り
- OmenとPunish追加
- 買い物

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
4. DayUI（左上。GameDataのDay表示とDayTime表示）
5. BankUI（DayUIの下。GamaDataのBank表示）
6. OpenMenuButton（右上。）
    1. MenuUI（終了ボタンだけ？）

### Omen&Punish ###
- 犬(BeyondOmen)
    - 吠える
    - 追いかけてくる（DayEvilに速度やダメージ比例）
        - Yujiを通り抜ける判定で、座標が重なってる間1体毎にSlow付与
        - 噛みついてダメージ
    - 仲間を増やしながら追いかけてくる
    - 巨大化して（通行不可になって）追いかけてくる

- ハチの巣付きの木(WallOmen)
    - 近づくとハチを生成（ハチは犬のシンプル版）

- 落とし穴(FloorOmen)
    - Stunとダメージ（時間やダメージがDayEvil比例）

- 弓ハム
    - 範囲に入ると撃ってくる。
    - 範囲に入ると全弓ハムが撃ってくる。

- 雷（地蔵か何か）（WallOmenか何か）
    - 半径に入ると半径を覆う雷が1回降る（半径とダメージがDayEvil依存）
    - 小さいのがあちこちに降り続ける（頻度とダメージがDayEvil依存）
    - 確定で直撃する強烈な雷

- クレーター？地上絵？（FloorOmen）
    - 地点通過時にUFOが攫おうとしてくる（遅いから避けやすい。攫われるとUFOエンディング）
    - 宇宙人が降ってきて銃で撃ってくる
    - 踏んだ瞬間にUFOに攫われる


### 装備 & YujiParams案（後回し） ###
- お店全般
    - 毎日ラインナップが変わる。そういう村なので。
        - 5日先くらいまでのラインナップが予告されている。
            - 購入したいアイテムに備えて準備？
                - Cashの持ち越しは不可なので、その日に強く出れるよう装備を整える？
- 靴屋さん/靴
    - 1つだけ装備。
        - 購入したら古いのは捨てる。
    - MoveSpeedと行動妨害耐性（SlowやStun）と耐久値
        - 移動距離で耐久値が減少。
            - 耐久値に応じてMoveSpeedやCC耐性が減少。
- 闇のドラッグストア（やぶ医者？バイヤー？）
    - SpeedDrug
        - MoveSpeedが大幅に上がる。
        - Day毎にMoveSpeedとランダムなパラメーターが下がっていく。
            - 10日くらいで元に戻って、そこから悪化していく。
    - HP
    - FixedDefense
    - PercentageDefense
    - Visibility
        - 森での視界の広さ

