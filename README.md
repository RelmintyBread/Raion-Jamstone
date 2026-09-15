# 🧼 Detergentnation

> **2D Side-Scroller Action Tower Defense Game**

**Detergentnation** adalah game 2D side-scroller action tower defense yang dikembangkan untuk menghadapi serangan alien **Foodonians** dan mempertahankan minimarket **RnA Mart**.

Pemain berperan sebagai **Ferry**, yang harus bertahan dari berbagai wave musuh menggunakan equipment, consumables, dan defensive structures.

---

## 🎮 Game Overview

### Objective

Pertahankan **RnA Mart** dari serangan Foodonians hingga seluruh wave berhasil dikalahkan.

### Win Condition

- Semua wave berhasil diselesaikan.
- Semua enemy pada wave terakhir berhasil dikalahkan.

### Lose Condition

- HP Player habis.
- HP RnA Mart habis.

---

# 🌳 Branch Structure

Repository menggunakan pembagian branch untuk memisahkan development dan production.

```text
main
 │
 └── dev
      │
      ├── feature/programmer-1/...
      ├── feature/programmer-2/...
      └── fix/...
```

### `main`

Branch untuk versi yang sudah stabil dan siap digunakan sebagai build utama.

**Jangan melakukan development langsung di `main`.**

### `dev`

Branch utama untuk menggabungkan seluruh development dari programmer.

Semua fitur yang sudah selesai dan sudah dites akan di-merge ke `dev`.

---

# 👨‍💻 Programmer Ownership

Untuk menghindari conflict antar programmer, project dibagi berdasarkan **subsystem**.

## Programmer 1 — Player & Combat

Fokus pada seluruh sistem yang berhubungan dengan player dan gameplay combat.

### Ownership

```text
Player/
├── Player.cs
├── PlayerMovement.cs
├── PlayerCombat.cs
└── ...

Combat/
├── RangeWeapon.cs
├── MeleeWeapon.cs
└── AmmoSystem.cs

Inventory/
├── Inventory.cs
└── ConsumableSlot.cs

Interaction/
├── InteractionSystem.cs
├── HealingSystem.cs
└── RepairSystem.cs
```

### Responsibilities

- Player movement
- Player combat
- Range weapon
- Melee weapon
- Ammo & reload
- Equipment
- Inventory
- Consumables
- Health system
- Healing
- Repair
- Player interaction

### Data yang dikerjakan

```text
EquipmentData
├── RangeWeaponData
├── MeleeWeaponData
├── ArmorData
└── JetpackData

ConsumableData
```

---

# 👨‍💻 Programmer 2 — World & Game Systems

Fokus pada sistem dunia, enemy, wave, defense, economy, dan progression.

### Ownership

```text
Core/
├── GameManager.cs
├── LevelManager.cs
└── GameState.cs

Enemy/
├── Enemy.cs
├── EnemyMovement.cs
├── EnemyAttack.cs
└── EnemyTargeting.cs

Wave/
├── WaveManager.cs
└── EnemySpawner.cs

Defense/
├── DefenseManager.cs
├── Defense.cs
├── AmmoDefense.cs
├── HealthDefense.cs
└── SingleUseDefense.cs

Economy/
├── CurrencySystem.cs
├── Shop.cs
├── UpgradeSystem.cs
├── RewardSystem.cs
└── KillStreakSystem.cs

Drop/
├── DropSystem.cs
└── CandyPickup.cs
```

### Responsibilities

- Game state
- Level management
- Wave system
- Enemy spawning
- Enemy AI
- Enemy attack
- Defense system
- Minimarket system
- Candy/currency
- Shop
- Upgrade
- Reward
- Kill streak
- Enemy drops

### Data yang dikerjakan

```text
LevelData
WaveData
WaveSpawnData
EnemyData
DefenseData
```

---

# 🔗 Shared Systems

Beberapa sistem digunakan oleh kedua programmer.

## EventBus

```text
EventBus
```

EventBus digunakan sebagai communication layer antar subsystem.

Contoh:

```text
Player
  │
  │ HealthChangedEvent
  ↓
EventBus
  │
  ├── PlayerHealthUI
  └── AudioManager
```

atau:

```text
Enemy
  │
  │ EnemyKilledEvent
  ↓
EventBus
  │
  ├── KillStreakSystem
  ├── CurrencySystem
  ├── DropSystem
  ├── WaveManager
  └── AudioManager
```

### Rules

- Jangan memasukkan gameplay logic ke `EventBus`.
- Jangan mengubah struktur EventBus sembarangan.
- Event digunakan untuk **memberi tahu sistem lain bahwa sesuatu telah terjadi**.
- Hindari direct dependency ke UI atau AudioManager.

---

# 📡 Event Ownership

Event yang digunakan oleh project antara lain:

```text
EnemyKilledEvent
PlayerDamagedEvent
HealthChangedEvent
AmmoChangedEvent
CurrencyChangedEvent
KillStreakChangedEvent
WaveStartedEvent
WaveCompletedEvent
GameStateChangedEvent
EquipmentChangedEvent
DefenseChangedEvent
PurchaseEvent
UpgradeSuccessEvent
```

### Contoh

Ketika player terkena damage:

```text
Player
  ↓
HealthSystem.TakeDamage()
  ↓
HealthChangedEvent
  ↓
EventBus
  ↓
PlayerHealthUI
```

Ketika enemy mati:

```text
Enemy
  ↓
Die()
  ↓
EnemyKilledEvent
  ↓
EventBus
  ├── KillStreakSystem
  ├── CurrencySystem
  ├── DropSystem
  └── WaveManager
```

---

# 📁 Recommended Folder Structure

```text
Assets/
└── _Project/
    │
    ├── Scripts/
    │   ├── Core/
    │   ├── Events/
    │   ├── Player/
    │   ├── Combat/
    │   ├── Enemy/
    │   ├── Wave/
    │   ├── Defense/
    │   ├── Inventory/
    │   ├── Interaction/
    │   ├── Economy/
    │   ├── Drop/
    │   ├── UI/
    │   └── Audio/
    │
    ├── ScriptableObjects/
    │   ├── Levels/
    │   ├── Waves/
    │   ├── Enemies/
    │   ├── Equipment/
    │   ├── Defense/
    │   ├── Consumables/
    │   └── Upgrades/
    │
    ├── Prefabs/
    │   ├── Characters/
    │   ├── Equipment/
    │   ├── Enemies/
    │   ├── Defense/
    │   ├── Items/
    │   ├── Environment/
    │   └── VFX/
    │
    ├── Scenes/
    ├── Animations/
    ├── Audio/
    ├── Materials/
    └── UI/
```

---

# 🌿 Git Workflow

## 1. Jangan bekerja langsung di `dev`

Sebelum mengerjakan fitur, buat feature branch dari `dev`.

```bash
git checkout dev
git pull origin dev
git checkout -b feature/programmer-1/player-movement
```

Contoh Programmer 2:

```bash
git checkout dev
git pull origin dev
git checkout -b feature/programmer-2/enemy-system
```

---

## 2. Naming Convention

Gunakan format:

```text
feature/<programmer>/<feature>
```

Contoh:

```text
feature/programmer-1/player-movement
feature/programmer-1/range-weapon
feature/programmer-1/inventory

feature/programmer-2/enemy-system
feature/programmer-2/wave-system
feature/programmer-2/defense-system
```

Untuk bug:

```text
fix/<programmer>/<bug>
```

Contoh:

```text
fix/programmer-1/player-jump
fix/programmer-2/enemy-spawn
```

---

# 💾 Commit Convention

Gunakan commit yang jelas dan spesifik.

Format:

```text
<type>: <description>
```

### Types

```text
feat      → fitur baru
fix       → perbaikan bug
refactor  → perubahan struktur kode
docs      → dokumentasi
chore     → perubahan kecil/configuration
test      → testing
```

### Examples

```bash
git commit -m "feat: add player movement"
```

```bash
git commit -m "feat: implement enemy wave spawning"
```

```bash
git commit -m "fix: prevent enemy from spawning outside bounds"
```

```bash
git commit -m "refactor: separate weapon data from weapon logic"
```

---

# 🔄 Development Workflow

Setiap mengerjakan fitur:

```text
1. Pull dev
      ↓
2. Create feature branch
      ↓
3. Develop
      ↓
4. Test in Unity
      ↓
5. Commit
      ↓
6. Push feature branch
      ↓
7. Pull Request → dev
      ↓
8. Review
      ↓
9. Merge
```

Contoh:

```bash
git checkout dev
git pull origin dev

git checkout -b feature/programmer-1/player-combat

# coding...

git add .
git commit -m "feat: add player melee attack"

git push origin feature/programmer-1/player-combat
```

Kemudian buat **Pull Request**:

```text
feature/programmer-1/player-combat
                ↓
               dev
```

---

# ⚠️ Conflict Prevention

Untuk mengurangi kemungkinan merge conflict:

### 1. Jangan mengubah file milik programmer lain

Contoh:

```text
Programmer 1
PlayerCombat.cs
```

Programmer 2 sebaiknya **tidak langsung mengedit `PlayerCombat.cs`**.

Jika membutuhkan data dari PlayerCombat, gunakan public API atau EventBus.

---

### 2. Jangan membuat duplicate system

Sebelum membuat class baru, cek apakah system tersebut sudah ada.

Contoh:

```text
❌ EnemyManager.cs
❌ EnemySystem.cs
❌ EnemyController2.cs
```

jika sebenarnya sudah ada:

```text
✅ Enemy.cs
```

---

### 3. Pull sebelum mulai bekerja

Sebelum membuat branch baru:

```bash
git checkout dev
git pull origin dev
```

Tujuannya agar branch dibuat dari versi `dev` terbaru.

---

### 4. Jangan commit file yang tidak diperlukan

Hindari commit:

```text
Library/
Temp/
Logs/
Obj/
Build/
UserSettings/
```

Pastikan `.gitignore` Unity sudah digunakan.

---

# 🧱 Architecture

Project menggunakan pendekatan:

```text
Component-Based Architecture
+
ScriptableObject Data
+
Event-Driven Communication
```

Secara umum:

```text
                    GameManager
                         │
                    LevelManager
                         │
                    WaveManager
                         │
                    EnemySpawner
                         │
                       Enemy
```

Player:

```text
                       Player
                         │
       ┌─────────┬───────┼────────┬──────────┐
       ↓         ↓       ↓        ↓          ↓
   Movement   Combat  Equipment Inventory  Health
                  │
             ┌────┴────┐
             ↓         ↓
        RangeWeapon  MeleeWeapon
```

Defense:

```text
DefenseManager
      │
      └── Defense
           ├── AmmoDefense
           ├── HealthDefense
           └── SingleUseDefense
```

Communication:

```text
Gameplay Systems
       │
       ↓
   EventBus
       │
 ┌─────┼─────┐
 ↓     ↓     ↓
 UI   Audio Economy
```

---

# 📦 Prefab vs ScriptableObject

Gunakan prinsip:

> **Prefab = object yang hidup di scene/gameplay.**  
> **ScriptableObject = data/configuration object.**

Contoh:

```text
Water Pistol
│
├── WaterPistol Prefab
│      └── RangeWeapon
│
└── WaterPistolData
       ├── Damage
       ├── FireRate
       ├── MagazineSize
       └── Range
```

Jangan membuat class baru untuk setiap weapon jika behavior-nya sama.

Gunakan:

```text
RangeWeapon
      +
RangeWeaponData
```

untuk berbagai ranged weapon.

---

# 🧪 Testing

Sebelum membuat Pull Request ke `dev`, pastikan:

- [ ] Project dapat dibuka tanpa compile error.
- [ ] Tidak ada missing script.
- [ ] Fitur yang dibuat sudah dites.
- [ ] Tidak merusak fitur subsystem lain.
- [ ] Console tidak memiliki error baru.
- [ ] Scene dapat dimainkan.
- [ ] Prefab yang digunakan sudah benar.
- [ ] ScriptableObject reference tidak missing.
- [ ] Commit hanya berisi perubahan yang relevan.

---

# 🚀 Pull Request Checklist

Sebelum merge ke `dev`:

```text
[ ] Feature selesai
[ ] Sudah dites
[ ] Tidak ada compile error
[ ] Tidak ada missing reference
[ ] Tidak mengubah subsystem programmer lain tanpa koordinasi
[ ] Commit message jelas
[ ] Branch sudah di-push
[ ] Pull Request dibuat
```

PR description minimal menjelaskan:

```text
## What
Apa yang dibuat?

## Changes
Apa saja yang diubah?

## Testing
Bagaimana fitur tersebut dites?

## Notes
Apakah ada dependency atau hal yang perlu diperhatikan?
```

---

# 🎯 Development Principle

Selama development, prioritaskan:

1. **Readable** — kode mudah dibaca.
2. **Modular** — setiap system memiliki tanggung jawab yang jelas.
3. **Scalable** — mudah menambahkan enemy, weapon, defense, dan item baru.
4. **Low Coupling** — hindari dependency yang tidak diperlukan.
5. **Data Driven** — gunakan ScriptableObject untuk data yang dapat berubah.
6. **Event Driven** — gunakan event untuk komunikasi antar subsystem.
7. **No Duplicate Logic** — jangan membuat logic yang sama di banyak tempat.

---

# 👥 Team Responsibility

| System | Programmer 1 | Programmer 2 |
|---|:---:|:---:|
| Player | ✅ | |
| Movement | ✅ | |
| Combat | ✅ | |
| Weapons | ✅ | |
| Ammo | ✅ | |
| Equipment | ✅ | |
| Inventory | ✅ | |
| Consumables | ✅ | |
| Health | ✅ | |
| Interaction | ✅ | |
| Core / Game State | | ✅ |
| Level | | ✅ |
| Wave | | ✅ |
| Enemy | | ✅ |
| Enemy AI | | ✅ |
| Enemy Spawner | | ✅ |
| Defense | | ✅ |
| Economy | | ✅ |
| Shop | | ✅ |
| Upgrade | | ✅ |
| Reward | | ✅ |
| Kill Streak | | ✅ |
| Drops | | ✅ |
| EventBus | 🔶 Shared | 🔶 Shared |
| Events | 🔶 Shared | 🔶 Shared |
| UI | 🔶 | 🔶 |
| Audio | 🔶 | 🔶 |

---

## 📌 Golden Rule

> **Jika sebuah perubahan hanya membutuhkan file di subsystem milikmu, jangan menyentuh file subsystem programmer lain.**

Jika membutuhkan komunikasi antar subsystem:

```text
DO NOT:
System A → edit langsung System B

PREFER:
System A → EventBus → System B
```

Dengan aturan ini, development dapat dilakukan secara paralel dengan risiko merge conflict yang lebih kecil.