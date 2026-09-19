# Core System – Detergentnation

Dokumentasi penggunaan sistem dasar game **Detergentnation** yang terdiri dari:

- `GameState`
- `GameManager`
- `LevelManager`
- `LevelData`
- `EnemyData`

Sistem ini digunakan sebagai fondasi untuk mengatur state permainan, level, dan konfigurasi enemy.

---

# 1. Struktur Sistem

Secara sederhana, hubungan antar-script:

```text
                    GameState
                        │
                        ▼
                  GameManager
                        │
                        │
                  LevelManager
                        │
                        ▼
                   LevelData
                        │
                        ├── Scene Name
                        ├── Level Number
                        ├── Defense Points
                        └── WaveData

                   EnemyData
                        │
                        ▼
                      Enemy
```

`GameState` menentukan kondisi permainan, `GameManager` mengatur kondisi tersebut, `LevelManager` mengatur level, `LevelData` menyimpan konfigurasi level, dan `EnemyData` menyimpan konfigurasi masing-masing jenis enemy.

---

# 2. GameState

## Fungsi

`GameState` merupakan enum yang digunakan untuk menentukan kondisi permainan.

```csharp
public enum GameState
{
    MainMenu,
    Shop,
    DefensePlanning,
    Playing,
    WaveBreak,
    Paused,
    Victory,
    GameOver
}
```

## Daftar State

| State             | Fungsi                              |
| ----------------- | ----------------------------------- |
| `MainMenu`        | Pemain berada di Main Menu.         |
| `Shop`            | Pemain berada di Shop.              |
| `DefensePlanning` | Pemain melakukan persiapan defense. |
| `Playing`         | Gameplay sedang berlangsung.        |
| `WaveBreak`       | Jeda antar-wave.                    |
| `Paused`          | Game sedang di-pause.               |
| `Victory`         | Level berhasil diselesaikan.        |
| `GameOver`        | Pemain mengalami kekalahan.         |

## Cara Menggunakan

Biasanya `GameState` tidak perlu dipanggil secara langsung. State diubah melalui `GameManager`.

Contoh:

```csharp
GameManager.Instance.SetState(GameState.Playing);
```

Untuk mengecek state:

```csharp
if (GameManager.Instance.CurrentState == GameState.Playing)
{
    Debug.Log("Gameplay sedang berlangsung.");
}
```

---

# 3. GameManager

## Fungsi

`GameManager` bertanggung jawab terhadap state global game.

Fitur utama:

- Mengatur Game State.
- Memulai game.
- Pause.
- Resume.
- Victory.
- Game Over.
- Menyimpan state sebelum pause.
- Singleton.
- Tetap hidup ketika scene berganti.

## Setup di Unity

Buat GameObject:

```text
Hierarchy
└── GameManager
```

Kemudian tambahkan:

```text
GameManager.cs
```

GameObject ini cukup dibuat **satu kali**, biasanya pada scene Main Menu.

`GameManager` menggunakan Singleton dan `DontDestroyOnLoad()`, sehingga tidak perlu dibuat ulang di setiap scene.

---

## Method

### `StartGame()`

Mengubah state menjadi:

```text
DefensePlanning
```

Contoh:

```csharp
GameManager.Instance.StartGame();
```

---

### `PauseGame()`

Menghentikan waktu gameplay.

```csharp
GameManager.Instance.PauseGame();
```

Pause hanya dapat dilakukan ketika state adalah:

```text
Playing
```

atau:

```text
WaveBreak
```

---

### `ResumeGame()`

Mengembalikan game ke state sebelum pause.

```csharp
GameManager.Instance.ResumeGame();
```

Contoh:

```text
Playing
   ↓
Pause
   ↓
Paused
   ↓
Resume
   ↓
Playing
```

---

### `Victory()`

Mengubah state menjadi:

```text
Victory
```

Contoh:

```csharp
GameManager.Instance.Victory();
```

---

### `GameOver()`

Mengubah state menjadi:

```text
GameOver
```

Contoh:

```csharp
GameManager.Instance.GameOver();
```

---

### `SetState()`

Digunakan untuk mengubah state secara langsung.

```csharp
GameManager.Instance.SetState(GameState.Shop);
```

---

# 4. LevelManager

## Fungsi

`LevelManager` bertanggung jawab untuk mengatur level dan memuat scene.

Fungsi utama:

- Menyimpan level aktif.
- Memuat scene level.
- Memulai level.
- Menyelesaikan level.
- Menentukan kondisi gagal.

## Setup di Unity

Buat GameObject:

```text
Hierarchy
├── GameManager
└── LevelManager
```

Tambahkan:

```text
LevelManager.cs
```

`LevelManager` juga menggunakan Singleton dan `DontDestroyOnLoad()`.

---

## Method

### `LoadLevel()`

Versi yang menggunakan `LevelData`:

```csharp
LevelManager.Instance.LoadLevel(levelData);
```

`levelData` berisi informasi level yang akan dimainkan.

Alurnya:

```text
LevelData
   │
   ├── Level Number
   ├── Scene Name
   └── Waves
        │
        ▼
   LevelManager
        │
        ▼
   Load Scene
```

---

### `StartLevel()`

Digunakan ketika pemain benar-benar mulai memainkan level.

```csharp
LevelManager.Instance.StartLevel();
```

State akan berubah menjadi:

```text
Playing
```

---

### `CompleteLevel()`

Dipanggil ketika level berhasil diselesaikan.

```csharp
LevelManager.Instance.CompleteLevel();
```

Alurnya:

```text
Playing
   ↓
Level selesai
   ↓
Victory
```

---

### `FailLevel()`

Dipanggil ketika kondisi kekalahan terpenuhi.

```csharp
LevelManager.Instance.FailLevel();
```

Alurnya:

```text
Playing
   ↓
Kondisi kalah
   ↓
GameOver
```

---

# 5. LevelData

## Fungsi

`LevelData` merupakan **ScriptableObject** yang menyimpan konfigurasi suatu level.

Satu asset `LevelData` mewakili satu level.

Contoh:

```text
Level_01.asset
Level_02.asset
Level_03.asset
```

Masing-masing dapat memiliki konfigurasi berbeda.

---

## Membuat LevelData

Pastikan `LevelData.cs` sudah tidak memiliki compile error.

Di Project Window:

```text
Assets
└── _Project
    └── ScriptableObjects
        └── Levels
```

Klik kanan:

```text
Create
→ Detergentnation
→ Level Data
```

Beri nama:

```text
Level_01
```

---

## Konfigurasi LevelData

Pada Inspector akan tersedia:

```text
Level Information
├── Level Number
├── Level Name
└── Scene Name

Level Configuration
├── Starting Defense Points
└── Waves
```

### Level Number

Nomor level.

Contoh:

```text
1
```

### Level Name

Nama level.

Contoh:

```text
First Encounter
```

### Scene Name

Nama scene Unity yang akan dimuat.

Contoh:

```text
Level_01
```

Jangan menulis:

```text
Level_01.unity
```

---

## Starting Defense Points

Menentukan jumlah Defense Point awal.

Contoh:

```text
5
```

---

## Waves

Berisi daftar `WaveData`.

Contoh:

```text
Waves
Size: 3

Element 0 → Wave_01
Element 1 → Wave_02
Element 2 → Wave_03
```

Urutan list menentukan urutan wave.

```text
Wave_01
   ↓
Wave_02
   ↓
Wave_03
```

`WaveData` harus sudah tersedia agar dapat dimasukkan ke dalam field ini.

---

# 6. EnemyData

## Fungsi

`EnemyData` merupakan **ScriptableObject** yang menyimpan konfigurasi suatu jenis enemy.

Tujuannya agar beberapa enemy dapat menggunakan script `Enemy` yang sama tetapi mempunyai statistik berbeda.

Contoh:

```text
Enemy_Jellien.asset
Enemy_Marshquito.asset
Enemy_Waffare.asset
```

---

## Membuat EnemyData

Buat folder:

```text
Assets
└── _Project
    └── ScriptableObjects
        └── Enemies
```

Klik kanan:

```text
Create
→ Detergentnation
→ Enemy Data
```

Contoh nama:

```text
Enemy_Jellien
```

---

## Konfigurasi EnemyData

Pada Inspector:

```text
Enemy Information
├── Enemy Name
├── Description
├── Enemy Sprite
└── Enemy Prefab

Enemy Stats
├── Max Health
├── Damage
├── Move Speed
├── Attack Range
└── Attack Cooldown

Reward
└── Candy Reward
```

### Enemy Name

Nama enemy.

```text
Jellien
```

### Description

Deskripsi enemy.

### Enemy Sprite

Sprite yang digunakan enemy.

### Enemy Prefab

Prefab enemy yang akan digunakan ketika enemy di-spawn.

### Max Health

HP maksimum enemy.

Contoh:

```text
100
```

### Damage

Damage serangan enemy.

Contoh:

```text
10
```

### Move Speed

Kecepatan bergerak enemy.

Contoh:

```text
2
```

### Attack Range

Jarak serangan enemy.

Contoh:

```text
1
```

### Attack Cooldown

Jeda antarserangan.

Contoh:

```text
1
```

### Candy Reward

Jumlah Candy yang diberikan ketika enemy dikalahkan.

Contoh:

```text
2
```

Nilai-nilai tersebut merupakan contoh dan dapat disesuaikan dengan balancing game.

---

# 7. Menggunakan EnemyData pada Enemy

Pada prefab enemy, assign `EnemyData` pada component `Enemy`.

Contoh:

```text
Enemy_Jellien
├── Enemy
│    └── Enemy Data → Enemy_Jellien
├── EnemyMovement
├── EnemyAttack
└── HealthSystem
```

Kemudian script `Enemy` dapat membaca data:

```csharp
enemyData.MaxHealth
enemyData.Damage
enemyData.MoveSpeed
enemyData.AttackRange
enemyData.AttackCooldown
enemyData.CandyReward
```

Contoh:

```csharp
private void Start()
{
    currentHealth = enemyData.MaxHealth;
}
```

---

# 8. Contoh Alur Lengkap

Misalnya pemain ingin memainkan Level 1.

### Step 1 — Main Menu

Player menekan tombol Start.

```csharp
GameManager.Instance.StartGame();
```

State:

```text
MainMenu
   ↓
DefensePlanning
```

---

### Step 2 — Load Level

Gunakan `LevelData`:

```csharp
LevelManager.Instance.LoadLevel(levelData);
```

`LevelManager` membaca:

```text
LevelData
├── Level Number
├── Scene Name
├── Defense Points
└── Waves
```

Kemudian scene level dimuat.

---

### Step 3 — Defense Planning

Pemain mengatur defense berdasarkan:

```text
Starting Defense Points
```

Setelah selesai:

```csharp
LevelManager.Instance.StartLevel();
```

State:

```text
DefensePlanning
   ↓
Playing
```

---

### Step 4 — Enemy Spawn

`WaveManager` nantinya membaca `WaveData`.

Contoh:

```text
Level_01
   │
   ├── Wave_01
   │      ├── Jellien
   │      └── Jellien
   │
   ├── Wave_02
   │      ├── Marshquito
   │      └── Jellien
   │
   └── Wave_03
          └── Waffare
```

Enemy kemudian menggunakan `EnemyData` untuk mendapatkan statistiknya.

---

### Step 5 — Victory

Jika seluruh wave berhasil diselesaikan:

```csharp
LevelManager.Instance.CompleteLevel();
```

State:

```text
Playing
   ↓
Victory
```

---

### Step 6 — Game Over

Jika kondisi kekalahan terpenuhi:

```csharp
LevelManager.Instance.FailLevel();
```

State:

```text
Playing
   ↓
GameOver
```

---

# 9. Struktur Folder

Struktur yang disarankan:

```text
Assets/
└── _Project/
    │
    ├── Scripts/
    │   └── Core/
    │       ├── GameState.cs
    │       ├── GameManager.cs
    │       ├── LevelManager.cs
    │       ├── LevelData.cs
    │       └── EnemyData.cs
    │
    └── ScriptableObjects/
        ├── Levels/
        │   ├── Level_01.asset
        │   └── Level_02.asset
        │
        └── Enemies/
            ├── Enemy_Jellien.asset
            ├── Enemy_Marshquito.asset
            └── Enemy_Waffare.asset
```

---

# 10. Checklist Setup

Sebelum menjalankan game, pastikan:

### GameManager

- [ ] `GameManager.cs` sudah terpasang.
- [ ] Hanya ada satu `GameManager`.

### LevelManager

- [ ] `LevelManager.cs` sudah terpasang.
- [ ] Hanya ada satu `LevelManager`.
- [ ] Scene level sudah masuk ke Build Profiles → Scene List.

### LevelData

- [ ] `LevelData` asset sudah dibuat.
- [ ] Level Number sudah diisi.
- [ ] Level Name sudah diisi.
- [ ] Scene Name sudah sesuai.
- [ ] Defense Point sudah ditentukan.
- [ ] WaveData sudah dimasukkan.

### EnemyData

- [ ] EnemyData asset sudah dibuat.
- [ ] Enemy Name sudah diisi.
- [ ] Enemy Prefab sudah di-assign.
- [ ] Sprite sudah di-assign jika diperlukan.
- [ ] Statistik enemy sudah diisi.
- [ ] Candy Reward sudah diisi.
- [ ] Enemy prefab sudah menggunakan EnemyData yang benar.

---

# 11. Prinsip Penggunaan

Gunakan masing-masing script sesuai tanggung jawabnya:

```text
GameState
    ↓
"Game sedang dalam kondisi apa?"

GameManager
    ↓
"Bagaimana state global game berubah?"

LevelManager
    ↓
"Level mana yang sedang dimainkan?"

LevelData
    ↓
"Apa konfigurasi level tersebut?"

EnemyData
    ↓
"Apa konfigurasi enemy tersebut?"
```

Jangan memasukkan semua logic ke dalam `GameManager`.

Contohnya, `GameManager` **tidak seharusnya** mengatur detail spawn enemy. Hal tersebut menjadi tanggung jawab `WaveManager` dan `EnemySpawner`.

Begitu juga `EnemyData` hanya menyimpan konfigurasi enemy. Logic pergerakan dan serangan tetap berada pada component seperti `EnemyMovement` dan `EnemyAttack`.

Dengan pembagian ini, sistem lebih mudah dikembangkan ketika jumlah level, wave, dan jenis enemy bertambah.
