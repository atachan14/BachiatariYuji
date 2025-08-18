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

