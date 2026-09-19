# Wave System – Detergentnation

Wave System digunakan untuk mengatur kemunculan enemy secara bertahap pada setiap level.

Sistem ini terdiri dari:

* `WaveManager.cs` → menjalankan dan mengontrol wave.
* `WaveData.cs` → menyimpan konfigurasi satu wave.
* `WaveSpawnData.cs` → menyimpan konfigurasi enemy yang di-spawn dalam suatu wave.
* `SpawnLocation.cs` → enum untuk menentukan lokasi spawn.
* `WaveStartCondition.cs` → enum untuk menentukan kondisi dimulainya wave.

Wave System menggunakan `LevelData` yang sudah dibuat sebelumnya untuk mendapatkan daftar wave dalam suatu level.

---

# 1. Struktur Sistem

```text
LevelData
    │
    └── Waves
         │
         ├── WaveData
         │    ├── Wave Number
         │    ├── Start Condition
         │    ├── Countdown
         │    ├── Spawn Interval
         │    └── WaveSpawnData[]
         │          │
         │          ├── EnemyData
         │          ├── Amount
         │          └── Spawn Location
         │
         └── WaveData
```

Saat game berjalan:

```text
LevelManager
     │
     ▼
WaveManager.StartWaves()
     │
     ▼
   WaveData
     │
     ▼
Spawn Wave
     │
     ▼
EnemyData
     │
     ▼
Enemy Prefab
     │
     ▼
Enemy Spawned
```

Ketika enemy dikalahkan:

```text
Enemy
  │
  ▼
EnemyKilledEvent
  │
  ▼
EventBus
  │
  ▼
WaveManager
  │
  ▼
Active Enemy Count berkurang
```

---

# 2. Struktur Folder

Disarankan menggunakan struktur:

```text
Assets/
└── _Project/
    │
    ├── Scripts/
    │   ├── Wave/
    │   │   ├── WaveManager.cs
    │   │   ├── WaveData.cs
    │   │   └── WaveSpawnData.cs
    │   │
    │   └── Enums/
    │       ├── SpawnLocation.cs
    │       └── WaveStartCondition.cs
    │
    └── ScriptableObjects/
        ├── Levels/
        │   ├── Level_01.asset
        │   └── Level_02.asset
        │
        └── Waves/
            ├── Wave_01.asset
            ├── Wave_02.asset
            └── Wave_03.asset
```

---

# 3. Enum

Enum digunakan oleh `WaveData` dan `WaveSpawnData`.

## SpawnLocation

File:

```text
SpawnLocation.cs
```

Contoh:

```csharp
public enum SpawnLocation
{
    Random,
    Left,
    Right,
    Top
}
```

Digunakan untuk menentukan dari mana enemy akan muncul.

| Value    | Fungsi                                             |
| -------- | -------------------------------------------------- |
| `Random` | Memilih spawn point secara acak dari semua lokasi. |
| `Left`   | Spawn dari spawn point sebelah kiri.               |
| `Right`  | Spawn dari spawn point sebelah kanan.              |
| `Top`    | Spawn dari spawn point bagian atas.                |

---

## WaveStartCondition

File:

```text
WaveStartCondition.cs
```

Contoh:

```csharp
public enum WaveStartCondition
{
    LevelStart,
    PreviousWaveFinishedSpawning,
    PreviousWaveAllDefeated
}
```

| Value                          | Fungsi                                                           |
| ------------------------------ | ---------------------------------------------------------------- |
| `LevelStart`                   | Wave dimulai saat level dimulai.                                 |
| `PreviousWaveFinishedSpawning` | Wave dapat dimulai setelah proses spawn wave sebelumnya selesai. |
| `PreviousWaveAllDefeated`      | Wave dimulai setelah seluruh enemy wave sebelumnya dikalahkan.   |

---

# 4. WaveSpawnData

## Fungsi

`WaveSpawnData` menyimpan konfigurasi satu kelompok enemy yang akan di-spawn.

Contohnya:

```text
Wave 1
└── Jellien
    ├── Amount: 5
    └── Location: Left
```

Script ini bukan ScriptableObject.

`WaveSpawnData` menggunakan:

```csharp
[System.Serializable]
```

sehingga datanya dapat ditampilkan langsung di dalam `WaveData`.

---

## Field

| Field           | Fungsi                           |
| --------------- | -------------------------------- |
| `EnemyData`     | Menentukan jenis enemy.          |
| `Amount`        | Jumlah enemy yang akan di-spawn. |
| `SpawnLocation` | Menentukan lokasi spawn enemy.   |

---

# 5. WaveData

## Fungsi

`WaveData` merupakan **ScriptableObject** yang menyimpan seluruh konfigurasi satu wave.

Contoh:

```text
Wave_01
├── Wave Number: 1
├── Is Huge Wave: No
├── Start Condition: Level Start
├── Countdown: 5
├── Spawn Interval: 1
└── Spawns
      ├── Jellien × 3 → Left
      └── Marshquito × 2 → Right
```

---

# 6. Membuat WaveData

Buat folder:

```text
Assets/_Project/ScriptableObjects/Waves
```

Klik kanan:

```text
Create
→ Detergentnation
→ Wave Data
```

Buat beberapa asset:

```text
Wave_01
Wave_02
Wave_03
```

---

# 7. Mengatur WaveData

Pilih `Wave_01`.

Inspector akan memiliki beberapa bagian.

## Wave Information

### Wave Number

Nomor wave.

Contoh:

```text
1
```

### Is Huge Wave

Centang jika wave merupakan Huge Wave.

Contoh:

```text
☐ Is Huge Wave
```

atau:

```text
☑ Is Huge Wave
```

Field ini nantinya dapat digunakan UI atau Audio System untuk menampilkan peringatan Huge Wave.

---

# 8. Mengatur Wave Start

### Start Condition

Pilih salah satu:

```text
Level Start
Previous Wave Finished Spawning
Previous Wave All Defeated
```

### Countdown Before Wave

Menentukan waktu tunggu sebelum enemy mulai di-spawn.

Contoh:

```text
Countdown Before Wave: 5
```

Artinya setelah kondisi wave terpenuhi, sistem menunggu 5 detik sebelum spawn.

---

# 9. Mengatur Spawn

### Spawn Interval

Menentukan jeda antar enemy yang di-spawn.

Contoh:

```text
Spawn Interval: 1
```

Artinya:

```text
Enemy 1
   ↓ 1 detik
Enemy 2
   ↓ 1 detik
Enemy 3
```

---

# 10. Mengatur WaveSpawnData

Pada bagian:

```text
Enemy Spawns
```

ubah:

```text
Size: 0
```

menjadi jumlah kelompok spawn yang diinginkan.

Contoh:

```text
Size: 2
```

Kemudian isi:

### Element 0

```text
Enemy Data      → Enemy_Jellien
Amount          → 3
Spawn Location  → Left
```

### Element 1

```text
Enemy Data      → Enemy_Marshquito
Amount          → 2
Spawn Location  → Right
```

Maka hasilnya:

```text
Wave_01
│
├── Jellien × 3
│      └── Left
│
└── Marshquito × 2
       └── Right
```

---

# 11. Hubungkan WaveData ke LevelData

Setelah `WaveData` selesai dibuat, buka:

```text
Level_01.asset
```

Pada bagian:

```text
Level Configuration
└── Waves
```

Masukkan wave sesuai urutan.

Contoh:

```text
Waves
Size: 3

Element 0 → Wave_01
Element 1 → Wave_02
Element 2 → Wave_03
```

Urutan tersebut menentukan urutan wave yang dimainkan.

```text
Wave_01
   ↓
Wave_02
   ↓
Wave_03
   ↓
Victory
```

---

# 12. Setup Spawn Point

`WaveManager` membutuhkan spawn point berupa `Transform`.

Di scene level, buat:

```text
SpawnPoints
├── Left
│   ├── Left_01
│   └── Left_02
│
├── Right
│   ├── Right_01
│   └── Right_02
│
└── Top
    ├── Top_01
    └── Top_02
```

Gunakan **Empty GameObject** sebagai spawn point.

Posisikan masing-masing spawn point di lokasi enemy seharusnya muncul.

---

# 13. Setup WaveManager

Di scene level, buat:

```text
Hierarchy
└── WaveManager
```

Tambahkan:

```text
WaveManager.cs
```

Pada Inspector:

### Level Data

Assign:

```text
Level_01
```

### Left Spawn Points

Masukkan:

```text
Left_01
Left_02
```

### Right Spawn Points

Masukkan:

```text
Right_01
Right_02
```

### Top Spawn Points

Masukkan:

```text
Top_01
Top_02
```

Hasilnya kurang lebih:

```text
WaveManager
├── Level Data
│      └── Level_01
│
├── Left Spawn Points
│      ├── Left_01
│      └── Left_02
│
├── Right Spawn Points
│      ├── Right_01
│      └── Right_02
│
└── Top Spawn Points
       ├── Top_01
       └── Top_02
```

---

# 14. Memastikan EnemyData Sudah Benar

Setiap `WaveSpawnData` membutuhkan `EnemyData`.

Contoh:

```text
WaveSpawnData
    │
    ▼
EnemyData: Enemy_Jellien
    │
    ▼
Enemy Prefab
    │
    ▼
Enemy
```

Pastikan `EnemyData` memiliki:

```text
EnemyData
├── Enemy Name
├── Enemy Sprite
├── Enemy Prefab
├── Max Health
├── Damage
├── Move Speed
├── Attack Range
├── Attack Cooldown
└── Candy Reward
```

Dan `Enemy Prefab` harus memiliki component:

```text
Enemy
```

---

# 15. Memulai Wave

`WaveManager` dijalankan melalui:

```csharp
WaveManager.StartWaves();
```

Contoh:

```csharp
public void StartLevel()
{
    GameManager.Instance.SetState(GameState.Playing);

    waveManager.StartWaves();
}
```

Alur:

```text
LevelManager.StartLevel()
        │
        ▼
GameState.Playing
        │
        ▼
WaveManager.StartWaves()
        │
        ▼
LevelData.Waves
        │
        ▼
WaveData
        │
        ▼
Spawn Enemy
```

---

# 16. Sistem Enemy yang Aktif

`WaveManager` menyimpan enemy yang sedang berada di arena dalam:

```csharp
HashSet<Enemy> activeEnemies;
```

Ketika enemy berhasil di-spawn:

```text
Enemy Spawned
     ↓
activeEnemies.Add(enemy)
```

Ketika enemy dikalahkan:

```text
Enemy
  ↓
EnemyKilledEvent
  ↓
EventBus
  ↓
WaveManager
  ↓
activeEnemies.Remove(enemy)
```

Dengan demikian `WaveManager` dapat mengetahui jumlah enemy yang masih hidup.

---

# 17. EnemyKilledEvent

Pastikan `Enemy` menerbitkan event ketika mati.

Contoh:

```csharp
private void Die()
{
    EventBus.Publish(new EnemyKilledEvent(this));

    Destroy(gameObject);
}
```

`WaveManager` sudah melakukan subscribe:

```csharp
private void OnEnable()
{
    EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
}
```

dan unsubscribe:

```csharp
private void OnDisable()
{
    EventBus.Unsubscribe<EnemyKilledEvent>(OnEnemyKilled);
}
```

---

# 18. Contoh Wave Lengkap

Misalnya Level 1 memiliki tiga wave.

## Wave 1

```text
Wave Number: 1
Huge Wave: No
Start Condition: Level Start
Countdown: 5s
Spawn Interval: 1s

Spawns:
    Jellien × 3 → Left
```

Alur:

```text
Level Start
    ↓
5 detik
    ↓
Jellien
    ↓ 1 detik
Jellien
    ↓ 1 detik
Jellien
```

---

## Wave 2

```text
Wave Number: 2
Huge Wave: No
Start Condition: Previous Wave All Defeated
Countdown: 5s
Spawn Interval: 1s

Spawns:
    Jellien × 2 → Left
    Marshquito × 2 → Right
```

Alur:

```text
Semua enemy Wave 1 mati
          ↓
       5 detik
          ↓
     Wave 2 mulai
          ↓
 Jellien → Jellien
          ↓
 Marshquito → Marshquito
```

---

## Wave 3

```text
Wave Number: 3
Huge Wave: Yes
Start Condition: Previous Wave All Defeated
Countdown: 10s
Spawn Interval: 0.5s

Spawns:
    Waffare × 5 → Right
    Jellien × 5 → Left
```

Karena `Is Huge Wave` aktif, sistem lain nantinya dapat merespons untuk menampilkan alarm atau UI.

---

# 19. Alur Keseluruhan

```text
                  LevelData
                      │
                      ▼
                 WaveManager
                      │
                      ▼
                   Wave_01
                      │
             ┌────────┴────────┐
             ▼                 ▼
       WaveSpawnData      WaveSpawnData
             │                 │
             ▼                 ▼
        EnemyData A        EnemyData B
             │                 │
             ▼                 ▼
        Enemy Prefab       Enemy Prefab
             │                 │
             └────────┬────────┘
                      ▼
                 Enemy Spawn
                      │
                      ▼
                Enemy Defeated
                      │
                      ▼
             EnemyKilledEvent
                      │
                      ▼
                  EventBus
                      │
                      ▼
                 WaveManager
                      │
                      ▼
             Next Wave / Victory
```

---

# 20. Checklist Setup

### Script

* [ ] `WaveManager.cs` sudah ada.
* [ ] `WaveData.cs` sudah ada.
* [ ] `WaveSpawnData.cs` sudah ada.
* [ ] `SpawnLocation.cs` sudah ada.
* [ ] `WaveStartCondition.cs` sudah ada.
* [ ] `EnemyKilledEvent.cs` sudah ada.
* [ ] `EventBus.cs` sudah ada.

### WaveData

* [ ] Wave asset sudah dibuat.
* [ ] Wave Number sudah diisi.
* [ ] Huge Wave sudah ditentukan.
* [ ] Start Condition sudah dipilih.
* [ ] Countdown sudah diatur.
* [ ] Spawn Interval sudah diatur.
* [ ] WaveSpawnData sudah diisi.

### LevelData

* [ ] Semua WaveData sudah dimasukkan ke `LevelData`.
* [ ] Urutan wave sudah benar.

### WaveManager

* [ ] `LevelData` sudah di-assign.
* [ ] Left Spawn Points sudah di-assign.
* [ ] Right Spawn Points sudah di-assign.
* [ ] Top Spawn Points sudah di-assign.

### Enemy

* [ ] EnemyData sudah dibuat.
* [ ] Enemy Prefab sudah di-assign.
* [ ] Enemy prefab memiliki component `Enemy`.
* [ ] Enemy menerbitkan `EnemyKilledEvent`.

---

# 21. Catatan

`WaveData` dan `EnemyData` berfungsi sebagai **data/configuration**, sedangkan `WaveManager` berfungsi sebagai **runtime controller**.

```text
ScriptableObject
      │
      ├── LevelData
      ├── WaveData
      └── EnemyData
             │
             ▼
        Runtime System
             │
             ├── LevelManager
             ├── WaveManager
             └── Enemy
```

Jangan memasukkan logic spawn langsung ke `WaveData`. `WaveData` hanya menyimpan konfigurasi. Proses menjalankan wave dan melakukan spawn dilakukan oleh `WaveManager`.

Dengan struktur ini, membuat wave baru cukup dengan membuat **WaveData baru**, mengatur `WaveSpawnData`, lalu memasukkannya ke `LevelData` tanpa perlu membuat script baru.
