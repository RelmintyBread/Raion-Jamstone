# Economy System – Detergentnation

Dokumentasi sistem ekonomi pada game **Detergentnation** menggunakan Unity.

## 1. Overview

Economy System bertanggung jawab mengatur seluruh aliran currency dan transaksi dalam game.

Sistem terdiri dari:

| Script              | Fungsi                                      |
| ------------------- | ------------------------------------------- |
| `CurrencySystem.cs` | Mengelola jumlah Candy milik pemain.        |
| `Shop.cs`           | Mengelola pembelian item menggunakan Candy. |
| `ShopItemData.cs`   | Menyimpan data item yang dijual di Shop.    |
| `UpgradeSystem.cs`  | Mengelola pembelian dan level upgrade.      |
| `UpgradeData.cs`    | Menyimpan konfigurasi setiap upgrade.       |
| `UpgradeType.cs`    | Menentukan tipe upgrade.                    |
| `RewardSystem.cs`   | Memberikan reward Candy dari gameplay.      |

### Arsitektur

```text
                    ┌─────────────────┐
                    │  RewardSystem   │
                    └────────┬────────┘
                             │
                         Add Candy
                             │
                             ▼
                    ┌─────────────────┐
                    │ CurrencySystem  │
                    └───────┬─┬───────┘
                            │ │
               Spend Candy  │ │  Spend Candy
                            │ │
                  ┌─────────┘ └─────────┐
                  ▼                     ▼
           ┌─────────────┐       ┌──────────────┐
           │    Shop     │       │UpgradeSystem │
           └──────┬──────┘       └──────┬───────┘
                  │                     │
                  ▼                     ▼
             Shop Item              Upgrade
```

---

# 2. Currency System

`CurrencySystem` merupakan pusat pengelolaan Candy.

Sistem ini menggunakan Singleton sehingga sistem lain dapat mengakses currency melalui:

```csharp
CurrencySystem.Instance
```

## Tanggung Jawab

- Menyimpan jumlah Candy.
- Menambahkan Candy.
- Mengurangi Candy.
- Mengecek kemampuan membeli.
- Mengubah jumlah Candy secara langsung.
- Memberikan event ketika jumlah Candy berubah.

## Field

| Field           | Fungsi                    |
| --------------- | ------------------------- |
| `startingCandy` | Jumlah Candy awal pemain. |

## Property

```csharp
CurrentCandy
```

Digunakan untuk membaca jumlah Candy saat ini.

Contoh:

```csharp
int candy = CurrencySystem.Instance.CurrentCandy;
```

## Menambahkan Candy

```csharp
CurrencySystem.Instance.AddCurrency(50);
```

Artinya pemain mendapatkan 50 Candy.

## Mengurangi Candy

```csharp
CurrencySystem.Instance.SpendCurrency(20);
```

Method akan mengurangi 20 Candy jika saldo mencukupi.

## Mengecek Saldo

```csharp
if (CurrencySystem.Instance.CanAfford(100))
{
    Debug.Log("Candy cukup.");
}
```

## Event

```csharp
OnCurrencyChanged
```

Event dipanggil setiap kali jumlah Candy berubah.

Event ini dapat digunakan oleh UI untuk memperbarui tampilan Candy.

---

# 3. Shop Item Data

`ShopItemData` merupakan ScriptableObject yang menyimpan konfigurasi item yang dijual di Shop.

## Membuat Shop Item

Di Project Window:

```text
Right Click
→ Create
→ Detergentnation
→ Economy
→ Shop Item
```

Buat asset untuk setiap item yang ingin dijual.

## Field

| Field         | Fungsi                                              |
| ------------- | --------------------------------------------------- |
| `itemName`    | Nama item.                                          |
| `description` | Deskripsi item.                                     |
| `icon`        | Icon item untuk UI.                                 |
| `price`       | Harga item dalam Candy.                             |
| `itemPrefab`  | Prefab item yang dapat diberikan setelah pembelian. |

Contoh:

```text
Soap
├── Item Name: Soap
├── Price: 25
├── Icon: Soap Icon
└── Item Prefab: Soap Prefab
```

---

# 4. Shop

`Shop` bertanggung jawab menangani transaksi pembelian.

## Alur Pembelian

```text
Player memilih item
        │
        ▼
Shop.Purchase()
        │
        ▼
Cek item valid
        │
        ▼
Cek Candy
        │
        ▼
Candy cukup?
    ┌───┴───┐
   Tidak    Ya
    │        │
    ▼        ▼
  Gagal   Spend Candy
             │
             ▼
       OnItemPurchased
```

## Mengecek Pembelian

```csharp
if (shop.CanPurchase(itemData))
{
    Debug.Log("Item dapat dibeli.");
}
```

## Membeli Item

```csharp
shop.Purchase(itemData);
```

Method akan:

1. Memastikan item valid.
2. Mengecek Candy.
3. Mengurangi Candy.
4. Memanggil `OnItemPurchased`.

## Event

```csharp
OnItemPurchased
```

Event menerima `ShopItemData` dari item yang berhasil dibeli.

Event ini dapat digunakan oleh Inventory System untuk memasukkan item ke inventory.

### Contoh integrasi

```csharp
private void OnEnable()
{
    shop.OnItemPurchased += HandleItemPurchased;
}

private void OnDisable()
{
    shop.OnItemPurchased -= HandleItemPurchased;
}

private void HandleItemPurchased(ShopItemData item)
{
    // Tambahkan item ke inventory.
}
```

> `Shop` tidak secara langsung mengelola Inventory. Tugas tersebut sebaiknya dilakukan oleh Inventory System.

---

# 5. Upgrade System

`UpgradeSystem` mengelola level upgrade yang dimiliki pemain.

Upgrade menggunakan `UpgradeData` sebagai sumber konfigurasi.

## Tanggung Jawab

- Menyimpan level setiap upgrade.
- Mengecek level saat ini.
- Mengecek level maksimum.
- Menghitung harga upgrade berikutnya.
- Mengecek apakah upgrade dapat dibeli.
- Membeli upgrade.
- Mengirim event ketika upgrade berhasil.

---

# 6. Upgrade Type

`UpgradeType` menentukan kategori upgrade.

```csharp
public enum UpgradeType
{
    PlayerDamage,
    PlayerHealth,
    PlayerMovementSpeed,
    DefenseDamage,
    DefenseHealth,
    DefenseAmmo
}
```

Contoh:

```text
PlayerDamage
→ meningkatkan damage player

PlayerHealth
→ meningkatkan health player

DefenseDamage
→ meningkatkan damage defense

DefenseHealth
→ meningkatkan health defense

DefenseAmmo
→ meningkatkan kapasitas ammo defense
```

Kategori dapat ditambahkan sesuai kebutuhan gameplay.

---

# 7. Upgrade Data

`UpgradeData` merupakan ScriptableObject yang menyimpan konfigurasi upgrade.

## Membuat Upgrade

Di Project Window:

```text
Right Click
→ Create
→ Detergentnation
→ Economy
→ Upgrade
```

## Field

| Field                   | Fungsi                          |
| ----------------------- | ------------------------------- |
| `upgradeName`           | Nama upgrade.                   |
| `description`           | Deskripsi upgrade.              |
| `icon`                  | Icon upgrade.                   |
| `upgradeType`           | Jenis upgrade.                  |
| `basePrice`             | Harga upgrade level pertama.    |
| `priceIncreasePerLevel` | Kenaikan harga setiap level.    |
| `maxLevel`              | Level maksimum.                 |
| `valuePerLevel`         | Nilai peningkatan setiap level. |

---

# 8. Perhitungan Harga Upgrade

Harga upgrade meningkat berdasarkan level saat ini.

Rumus:

```text
Price = Base Price + (Price Increase × Current Level)
```

Contoh:

```text
Base Price            = 50
Price Increase        = 25
Maximum Level         = 5
```

Maka:

| Level Saat Ini |    Harga Berikutnya |
| -------------: | ------------------: |
|              0 |                  50 |
|              1 |                  75 |
|              2 |                 100 |
|              3 |                 125 |
|              4 |                 150 |
|              5 | Tidak dapat membeli |

---

# 9. Membeli Upgrade

Gunakan:

```csharp
upgradeSystem.PurchaseUpgrade(upgradeData);
```

Sistem akan:

1. Mengecek apakah `UpgradeData` valid.
2. Mengecek apakah belum mencapai `maxLevel`.
3. Menghitung harga.
4. Mengecek Candy.
5. Mengurangi Candy.
6. Menaikkan level.
7. Menghitung nilai upgrade.
8. Memanggil `OnUpgradePurchased`.

## Mengecek Level

```csharp
int level = upgradeSystem.GetUpgradeLevel(upgradeData);
```

## Mengecek Max Level

```csharp
if (upgradeSystem.IsMaxLevel(upgradeData))
{
    Debug.Log("Upgrade sudah maksimum.");
}
```

## Mendapatkan Harga Berikutnya

```csharp
int price = upgradeSystem.GetNextUpgradePrice(upgradeData);
```

---

# 10. Upgrade Event

`UpgradeSystem` memiliki event:

```csharp
OnUpgradePurchased
```

Event memberikan:

```text
UpgradeData
New Level
Total Value
```

Contoh penggunaan:

```csharp
private void OnEnable()
{
    upgradeSystem.OnUpgradePurchased += HandleUpgradePurchased;
}

private void OnDisable()
{
    upgradeSystem.OnUpgradePurchased -= HandleUpgradePurchased;
}

private void HandleUpgradePurchased(
    UpgradeData upgrade,
    int level,
    float value)
{
    Debug.Log(
        $"{upgrade.UpgradeName} Level {level}"
    );
}
```

Event ini dapat digunakan oleh sistem Player atau Defense untuk menerapkan perubahan statistik.

---

# 11. Reward System

`RewardSystem` menangani pemberian Candy sebagai reward.

Reward dapat berasal dari:

- Enemy yang dikalahkan.
- Event gameplay.
- Bonus tertentu.
- Reward wave.
- Reward lainnya.

## Memberikan Reward

```csharp
rewardSystem.GiveReward(50);
```

Pemain mendapatkan 50 Candy.

## Reward dari Enemy

Enemy memiliki:

```csharp
CandyReward
```

Contoh:

```csharp
rewardSystem.GiveEnemyReward(enemy);
```

Sistem akan membaca `CandyReward` dari enemy kemudian memasukkannya ke `CurrencySystem`.

---

# 12. Integrasi Enemy dengan Reward

Enemy memiliki event:

```csharp
OnEnemyDied
```

Event tersebut dapat digunakan untuk memberikan reward ketika enemy mati.

Alur:

```text
Enemy
  │
  ▼
TakeDamage()
  │
  ▼
Health <= 0
  │
  ▼
OnEnemyDied
  │
  ▼
RewardSystem
  │
  ▼
GiveEnemyReward()
  │
  ▼
CurrencySystem
  │
  ▼
Candy bertambah
```

Contoh konfigurasi:

```text
Jellien
Candy Reward = 10

Waffare
Candy Reward = 20

Moshi Mos
Candy Reward = 50
```

Nilai tersebut dapat disesuaikan dengan balancing game.

---

# 13. Setup Scene

Buat GameObject untuk setiap system:

```text
Scene
│
├── GameManager
│
├── CurrencySystem
├── RewardSystem
├── Shop
├── UpgradeSystem
│
├── EnemySpawner
├── DefenseManager
│
└── UI
    ├── CurrencyDisplay
    ├── ShopPanel
    └── UpgradePanel
```

### CurrencySystem

Tambahkan:

```text
CurrencySystem.cs
```

Atur:

```text
Starting Candy
```

### RewardSystem

Tambahkan:

```text
RewardSystem.cs
```

Tidak membutuhkan konfigurasi khusus pada implementasi dasar.

### Shop

Tambahkan:

```text
Shop.cs
```

Item Shop disimpan dalam asset `ShopItemData`.

### UpgradeSystem

Tambahkan:

```text
UpgradeSystem.cs
```

Upgrade disimpan dalam asset `UpgradeData`.

---

# 14. Alur Economy System

Alur utama economy:

```text
                    GAMEPLAY
                       │
          ┌────────────┴────────────┐
          │                         │
          ▼                         ▼
      Enemy Mati              Event Reward
          │                         │
          └──────────┬──────────────┘
                     ▼
               RewardSystem
                     │
                     ▼
              CurrencySystem
                     │
                  Candy
                     │
            ┌────────┴────────┐
            │                 │
            ▼                 ▼
           Shop        UpgradeSystem
            │                 │
            ▼                 ▼
        Shop Item          Upgrade
```

---

# 15. Contoh Gameplay

Misalnya pemain mengalahkan satu musuh.

```text
Enemy Mati
    ↓
Candy Reward = 20
    ↓
RewardSystem.GiveEnemyReward()
    ↓
CurrencySystem.AddCurrency(20)
    ↓
Candy +20
```

Kemudian pemain membeli upgrade.

```text
Candy = 100
    ↓
Upgrade Price = 50
    ↓
UpgradeSystem.PurchaseUpgrade()
    ↓
CurrencySystem.SpendCurrency(50)
    ↓
Candy = 50
    ↓
Upgrade Level +1
```

Kemudian pemain membeli item Shop.

```text
Candy = 50
    ↓
Item Price = 25
    ↓
Shop.Purchase()
    ↓
CurrencySystem.SpendCurrency(25)
    ↓
Candy = 25
    ↓
OnItemPurchased
    ↓
Inventory menerima item
```

---

# 16. Integrasi dengan UI

`CurrencySystem` menyediakan event:

```csharp
OnCurrencyChanged
```

UI dapat berlangganan event tersebut.

Contoh:

```csharp
private void OnEnable()
{
    CurrencySystem.Instance.OnCurrencyChanged += UpdateCurrencyUI;
}

private void OnDisable()
{
    CurrencySystem.Instance.OnCurrencyChanged -= UpdateCurrencyUI;
}

private void UpdateCurrencyUI(int currentCandy)
{
    candyText.text = currentCandy.ToString();
}
```

Dengan demikian UI tidak perlu melakukan pengecekan Candy setiap frame.

---

# 17. Checklist Setup

### Currency

- [ ] `CurrencySystem` sudah berada di Scene.
- [ ] `startingCandy` sudah diatur.
- [ ] Tidak ada lebih dari satu `CurrencySystem` aktif.

### Shop

- [ ] `Shop` sudah berada di Scene.
- [ ] `ShopItemData` sudah dibuat.
- [ ] Harga item sudah dikonfigurasi.
- [ ] Icon item sudah dikonfigurasi.
- [ ] Item memiliki prefab jika diperlukan.
- [ ] UI Shop sudah memanggil `Purchase()`.

### Upgrade

- [ ] `UpgradeSystem` sudah berada di Scene.
- [ ] `UpgradeData` sudah dibuat.
- [ ] `UpgradeType` sudah dipilih.
- [ ] Harga sudah dikonfigurasi.
- [ ] `maxLevel` sudah dikonfigurasi.
- [ ] `valuePerLevel` sudah dikonfigurasi.
- [ ] UI Upgrade sudah memanggil `PurchaseUpgrade()`.

### Reward

- [ ] `RewardSystem` sudah berada di Scene.
- [ ] Enemy memiliki `CandyReward`.
- [ ] `OnEnemyDied` terhubung ke reward handler.
- [ ] Reward masuk ke `CurrencySystem`.

---

# 18. Catatan Arsitektur

Economy System menggunakan prinsip **separation of responsibility**.

```text
CurrencySystem
→ "Berapa Candy yang dimiliki pemain?"

RewardSystem
→ "Dari mana pemain mendapatkan Candy?"

Shop
→ "Apa yang bisa dibeli pemain?"

UpgradeSystem
→ "Upgrade apa yang sudah dimiliki pemain?"
```

Dengan pembagian tersebut, sistem dapat dikembangkan secara independen.

Contohnya, apabila ingin menambahkan reward dari menyelesaikan wave, tidak perlu mengubah `CurrencySystem`. Cukup panggil:

```csharp
rewardSystem.GiveReward(waveReward);
```

Begitu juga jika ingin menambahkan item baru ke Shop, cukup membuat `ShopItemData` baru tanpa mengubah `Shop.cs`.

---

# 19. Pengembangan Selanjutnya

Beberapa fitur yang dapat ditambahkan:

### Currency

- Save/load Candy.
- Multiple currency.
- Currency limit.
- Currency reset per level.

### Shop

- Item quantity.
- Item stock.
- Item unlock condition.
- Discount.
- Shop rotation.

### Upgrade

- Upgrade tree.
- Upgrade prerequisite.
- Permanent upgrade.
- Upgrade reset/refund.
- Per-level stat modifier.

### Reward

- Wave completion reward.
- Bonus reward.
- First-time completion reward.
- Combo reward.
- Reward multiplier.

### UI

- Candy animation.
- Floating `+Candy` text.
- Purchase feedback.
- Insufficient Candy feedback.
- Upgrade preview.

---

# 20. Final Structure

Struktur folder yang direkomendasikan:

```text
Scripts
└── Economy
    ├── CurrencySystem.cs
    ├── RewardSystem.cs
    ├── Shop.cs
    ├── ShopItemData.cs
    ├── UpgradeSystem.cs
    ├── UpgradeData.cs
    └── UpgradeType.cs
```

Data:

```text
Data
└── Economy
    ├── ShopItems
    │   ├── ShopItem_01.asset
    │   └── ShopItem_02.asset
    │
    └── Upgrades
        ├── Upgrade_PlayerDamage.asset
        ├── Upgrade_PlayerHealth.asset
        └── Upgrade_DefenseDamage.asset
```

Dengan struktur tersebut, **Economy System** menjadi pusat pengelolaan Candy, Shop, Upgrade, dan Reward tanpa membuat masing-masing sistem saling bergantung secara langsung.
