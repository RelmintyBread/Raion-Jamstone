# Enemy System – Detergentnation

Dokumentasi sistem musuh untuk game **Detergentnation** menggunakan Unity.

## 1. Overview

Enemy System bertanggung jawab untuk mengatur perilaku dasar musuh, mulai dari:

- Menerima dan mengelola damage.
- Menentukan target.
- Bergerak menuju target.
- Menyerang target ketika berada dalam jarak serang.
- Melakukan spawn musuh.
- Menghitung jumlah musuh yang masih aktif.

Sistem dibuat secara modular sehingga setiap bagian memiliki tanggung jawab masing-masing.

### Struktur Script

| Script               | Fungsi                                        |
| -------------------- | --------------------------------------------- |
| `Enemy.cs`           | Mengelola health, damage, dan kematian musuh. |
| `EnemyTargetting.cs` | Menentukan dan menyimpan target musuh.        |
| `EnemyMovement.cs`   | Mengatur pergerakan musuh menuju target.      |
| `EnemyAttack.cs`     | Mengatur serangan dan cooldown musuh.         |
| `EnemySpawner.cs`    | Mengatur proses spawn musuh.                  |

---

# 2. Enemy

`Enemy` merupakan base component yang menyimpan informasi dasar dan status kehidupan musuh.

### Tanggung Jawab

- Menyimpan nama musuh.
- Menyimpan maximum health.
- Menyimpan current health.
- Menerima damage.
- Menangani kematian.
- Menyediakan reward candy.
- Memberikan event ketika health berubah.
- Memberikan event ketika musuh mati.

### Field

| Field         | Fungsi                                      |
| ------------- | ------------------------------------------- |
| `enemyName`   | Nama musuh.                                 |
| `maxHealth`   | Health maksimum.                            |
| `candyReward` | Jumlah candy yang diberikan sebagai reward. |

### Property

```csharp
EnemyName
MaxHealth
CurrentHealth
CandyReward
IsDead
```

### Memberikan Damage

Gunakan:

```csharp
enemy.TakeDamage(10);
```

Method tersebut akan mengurangi health musuh.

Jika health mencapai `0`, musuh akan menjalankan proses kematian.

### Event

`Enemy` menyediakan dua event:

```csharp
OnEnemyDied
OnHealthChanged
```

`OnHealthChanged` dapat digunakan oleh UI untuk memperbarui health bar.

`OnEnemyDied` dapat digunakan oleh sistem lain seperti `EnemySpawner`, `WaveManager`, atau sistem reward.

---

# 3. EnemyTargetting

`EnemyTargetting` bertanggung jawab menentukan target yang akan dikejar dan diserang oleh musuh.

## Cara Kerja

Target dapat diberikan secara langsung:

```csharp
targetting.SetTarget(target);
```

atau dicari menggunakan Tag:

```csharp
targetting.FindTarget();
```

Target dapat dihapus menggunakan:

```csharp
targetting.ClearTarget();
```

### Field

| Field               | Fungsi                                             |
| ------------------- | -------------------------------------------------- |
| `target`            | Transform target musuh.                            |
| `targetTag`         | Tag yang digunakan untuk mencari target.           |
| `findTargetOnStart` | Menentukan apakah target dicari saat game dimulai. |

### Property

```csharp
Target
HasTarget
```

`HasTarget` digunakan untuk mengetahui apakah musuh sudah memiliki target.

Contoh:

```csharp
if (targetting.HasTarget)
{
    // Musuh memiliki target.
}
```

---

# 4. EnemyMovement

`EnemyMovement` mengatur pergerakan musuh menuju target.

Script ini menggunakan `Rigidbody2D`.

```text
Enemy
   |
   v
EnemyTargetting
   |
   v
Target ditemukan
   |
   v
EnemyMovement
   |
   v
Musuh bergerak menuju target
```

### Field

| Field               | Fungsi                                   |
| ------------------- | ---------------------------------------- |
| `moveSpeed`         | Kecepatan pergerakan musuh.              |
| `moveTowardsTarget` | Menentukan apakah musuh sedang bergerak. |
| `targetting`        | Referensi ke `EnemyTargetting`.          |

### Mengatur Pergerakan

Pergerakan dapat dihentikan menggunakan:

```csharp
movement.SetMovement(false);
```

dan diaktifkan kembali menggunakan:

```csharp
movement.SetMovement(true);
```

Kecepatan dapat diubah menggunakan:

```csharp
movement.SetMoveSpeed(3f);
```

### Perilaku

Musuh akan menentukan arah berdasarkan posisi target.

```text
Enemy                         Target
  ● --------------------------> ●
             bergerak
```

Untuk game side-scroller, implementasi dasar ini berfokus pada pergerakan horizontal.

---

# 5. EnemyAttack

`EnemyAttack` menangani mekanisme serangan musuh.

Musuh akan menyerang ketika:

1. Memiliki target.
2. Target berada dalam attack range.
3. Cooldown serangan sudah selesai.
4. Musuh diperbolehkan menyerang.

### Field

| Field            | Fungsi                                    |
| ---------------- | ----------------------------------------- |
| `attackDamage`   | Damage yang diberikan.                    |
| `attackRange`    | Jarak maksimal untuk menyerang.           |
| `attackCooldown` | Jeda antarserangan.                       |
| `canAttack`      | Mengaktifkan atau menonaktifkan serangan. |

### Alur Serangan

```text
EnemyTargetting
       |
       v
  Target ditemukan
       |
       v
Target masuk attack range?
       |
      Ya
       |
       v
Cooldown selesai?
       |
      Ya
       |
       v
     Attack
       |
       v
Target.TakeDamage()
```

### Mengaktifkan / Menonaktifkan Attack

```csharp
enemyAttack.SetCanAttack(false);
```

Untuk mengaktifkan kembali:

```csharp
enemyAttack.SetCanAttack(true);
```

### Catatan

Implementasi dasar menggunakan:

```csharp
SendMessage("TakeDamage", attackDamage)
```

untuk mengirim damage kepada target.

Karena itu, target harus memiliki method:

```csharp
TakeDamage(int damage)
```

Pada pengembangan berikutnya, mekanisme ini dapat diganti menggunakan interface seperti `IDamageable` agar komunikasi antar-system lebih terstruktur.

---

# 6. EnemySpawner

`EnemySpawner` bertanggung jawab melakukan spawn musuh.

Spawner mendukung:

- Beberapa prefab musuh.
- Beberapa spawn point.
- Jumlah spawn.
- Spawn interval.
- Start delay.
- Maximum enemy aktif.
- Tracking musuh yang masih hidup.

## Spawn Entry

Setiap entry memiliki:

| Field           | Fungsi                           |
| --------------- | -------------------------------- |
| `enemyPrefab`   | Prefab musuh yang akan di-spawn. |
| `spawnCount`    | Jumlah musuh yang akan di-spawn. |
| `spawnInterval` | Jeda antar spawn.                |

Contoh:

```text
Spawn Entry

Jellien
Spawn Count   = 5
Spawn Interval = 1 sec
```

Artinya lima Jellien akan di-spawn dengan interval satu detik.

---

# 7. Setup Enemy Prefab

Setiap prefab musuh dasar sebaiknya memiliki komponen:

```text
EnemyPrefab
├── SpriteRenderer
├── Rigidbody2D
├── Collider2D
├── Enemy
├── EnemyTargetting
├── EnemyMovement
└── EnemyAttack
```

### Rigidbody2D

Untuk musuh yang menggunakan `EnemyMovement`:

- Body Type: `Dynamic`
- Freeze Rotation Z: aktif
- Gravity Scale: disesuaikan dengan jenis musuh

Untuk musuh terbang, pengaturan movement dan gravity dapat disesuaikan dengan kebutuhan gameplay.

---

# 8. Setup Target

Karena `EnemyTargetting` menggunakan Tag, target harus memiliki tag yang sesuai.

Contoh:

```text
Target
Tag = Player
```

Kemudian pada `EnemyTargetting`:

```text
Target Tag = Player
Find Target On Start = ✓
```

Jika target yang digunakan adalah minimarket, dapat dibuat tag khusus seperti:

```text
Minimarket
```

Kemudian:

```text
Target Tag = Minimarket
```

Dengan demikian musuh akan mencari object dengan tag tersebut.

---

# 9. Setup EnemySpawner di Scene

Buat GameObject:

```text
EnemySpawner
```

Kemudian tambahkan component:

```text
EnemySpawner
```

Buat beberapa spawn point:

```text
EnemySpawnPoints
├── Spawn_Left
├── Spawn_Right
└── Spawn_Top
```

Kemudian masukkan Transform tersebut ke array:

```text
Spawn Points
```

pada `EnemySpawner`.

---

# 10. Contoh Hierarchy

Struktur sederhana Scene:

```text
Scene
│
├── GameManager
├── LevelManager
│
├── EnemySpawner
│
├── EnemySpawnPoints
│   ├── Spawn_Left
│   ├── Spawn_Right
│   └── Spawn_Top
│
├── Player
│
├── RnA_Mart
│
└── DefenseManager
```

---

# 11. Memulai Spawn

Jika:

```text
Spawn On Start = true
```

maka spawner akan otomatis menjalankan proses spawn ketika Scene dimulai.

Jika spawn dikendalikan oleh `WaveManager`, lebih baik:

```text
Spawn On Start = false
```

Kemudian panggil:

```csharp
enemySpawner.StartSpawning();
```

ketika wave dimulai.

---

# 12. Alur Enemy System

Secara keseluruhan, alurnya:

```text
EnemySpawner
     |
     | Instantiate
     v
Enemy Prefab
     |
     +--------------------+
     |                    |
     v                    v
EnemyTargetting      EnemyMovement
     |                    |
     | Target             | Move
     +--------->-----------+
              |
              v
         EnemyAttack
              |
              v
          Target
              |
              v
        TakeDamage()
```

Sementara ketika enemy menerima damage:

```text
Defense / Player
       |
       v
Enemy.TakeDamage()
       |
       v
Health berkurang
       |
       v
Health <= 0 ?
       |
      Ya
       |
       v
Enemy Die
       |
       +----> OnEnemyDied
       |
       +----> Destroy Enemy
```

---

# 13. Integrasi dengan WaveManager

`EnemySpawner` dirancang agar dapat dikontrol oleh `WaveManager`.

Contoh alur:

```text
WaveManager
     |
     v
Start Wave
     |
     v
EnemySpawner.StartSpawning()
     |
     v
Enemy Spawn
     |
     v
Enemy Bergerak
     |
     v
Enemy Menyerang
     |
     v
Enemy Mati
     |
     v
OnEnemyDied
     |
     v
WaveManager menghitung enemy
     |
     v
Semua enemy selesai?
     |
    Ya
     |
     v
Wave selesai
```

`EnemySpawner` sendiri hanya menangani proses spawn dan tracking enemy aktif. Logika seperti:

- urutan wave,
- wave break,
- `Finished Spawning`,
- `All Defeated`,
- dan `Huge Wave`

sebaiknya dikelola oleh `WaveManager`.

---

# 14. Contoh Konfigurasi Enemy

Contoh konfigurasi awal untuk beberapa Foodonian:

| Enemy      | Health | Damage | Speed | Attack Range |
| ---------- | -----: | -----: | ----: | -----------: |
| Jellien    |    100 |     10 |     2 |            1 |
| Spearman   |    100 |     15 |     2 |            1 |
| Marshquito |     50 |     10 |     3 |            5 |
| Waffare    |    150 |     20 |   2.5 |            1 |
| Cheetozard |    250 |     30 |   1.5 |            5 |
| Hammerman  |    250 |     30 |   1.5 |            1 |
| Moshi Mos  |    500 |     40 |     1 |            1 |

**Catatan:** angka pada tabel merupakan contoh konfigurasi/balancing dan bukan angka statistik resmi yang ditentukan oleh GDD.

GDD mendefinisikan Foodonian sebagai kategori Normal, Tiny, Brawler, Brute, dan Tanker, dengan contoh musuh seperti Jellien, Spearman, Marshquito, Waffare, Cheetozard, Hammerman, dan Moshi Mos.

---

# 15. Checklist Setup

### Enemy Prefab

- [ ] `Enemy` sudah ditambahkan.
- [ ] `EnemyTargetting` sudah ditambahkan.
- [ ] `EnemyMovement` sudah ditambahkan.
- [ ] `EnemyAttack` sudah ditambahkan.
- [ ] `Rigidbody2D` sudah ditambahkan.
- [ ] `Collider2D` sudah ditambahkan.
- [ ] Health sudah dikonfigurasi.
- [ ] Attack damage sudah dikonfigurasi.
- [ ] Movement speed sudah dikonfigurasi.

### Target

- [ ] Target memiliki Tag yang benar.
- [ ] `targetTag` sudah sesuai.
- [ ] Target memiliki method `TakeDamage(int)`.

### EnemySpawner

- [ ] `EnemySpawner` sudah ada di Scene.
- [ ] Prefab enemy sudah dimasukkan ke `spawnEntries`.
- [ ] `spawnCount` sudah diatur.
- [ ] `spawnInterval` sudah diatur.
- [ ] Spawn point sudah dimasukkan.
- [ ] `maxAliveEnemies` sudah diatur.

### Integration

- [ ] `WaveManager` dapat memanggil `StartSpawning()`.
- [ ] `OnEnemyDied` dapat digunakan untuk tracking wave.
- [ ] Sistem reward dapat menggunakan `CandyReward`.
- [ ] Sistem damage sudah terhubung dengan player/defense.

---

# 16. Pengembangan Selanjutnya

Enemy System ini merupakan fondasi dasar. Beberapa fitur yang dapat ditambahkan selanjutnya:

1. **EnemyData ScriptableObject**
   - Memisahkan statistik enemy dari prefab.
   - Memudahkan balancing.
   - Mendukung banyak varian enemy.

2. **IDamageable**
   - Menggantikan `SendMessage`.
   - Membuat sistem damage lebih type-safe.

3. **Ranged Attack**
   - Projectile.
   - Projectile pooling.
   - Target tracking.

4. **Enemy State Machine**
   - Idle.
   - Moving.
   - Attacking.
   - Hurt.
   - Dead.

5. **Enemy Animation**
   - Walk.
   - Attack.
   - Hurt.
   - Death.

6. **Target Prioritization**
   - Menyerang player.
   - Menyerang defense.
   - Menyerang minimarket.
   - Memilih target berdasarkan jarak/prioritas.

7. **Object Pooling**
   - Mengurangi Instantiate/Destroy ketika jumlah enemy sangat banyak.

8. **Wave Integration**
   - Integrasi penuh dengan `WaveManager`.
   - Tracking `Finished Spawning`.
   - Tracking `All Defeated`.
   - Huge Wave.

---

# 17. Responsibility Summary

Prinsip utama Enemy System:

```text
Enemy
→ "Siapa saya dan berapa HP saya?"

EnemyTargetting
→ "Siapa target saya?"

EnemyMovement
→ "Bagaimana saya bergerak menuju target?"

EnemyAttack
→ "Kapan dan bagaimana saya menyerang?"

EnemySpawner
→ "Kapan dan di mana enemy dibuat?"
```

Dengan pemisahan ini, perubahan pada satu sistem tidak perlu mengubah seluruh sistem enemy.

Contohnya, menambahkan **ranged enemy** tidak harus mengubah `EnemyMovement` atau `EnemySpawner`; cukup menambahkan implementasi attack/projectile yang sesuai pada bagian `EnemyAttack`.
