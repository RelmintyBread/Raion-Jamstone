# Drop System

Drop System adalah sistem yang bertanggung jawab untuk menjatuhkan item setelah suatu enemy dikalahkan. Pada implementasi ini, item yang dijatuhkan berupa **Candy** yang dapat diambil oleh player untuk menambah currency.

Sistem terdiri dari dua bagian utama:

- **`DropSystem`** → Mengatur proses spawn candy.
- **`CandyPickup`** → Mengatur interaksi candy dengan player dan penambahan currency.

---

## 1. Fitur

- Menjatuhkan candy ketika enemy mati.
- Menentukan jumlah candy yang diberikan.
- Candy dapat diambil oleh player melalui collision trigger.
- Candy otomatis menghilang setelah diambil.
- Menggunakan `CurrencySystem` untuk menambahkan currency.
- `DropSystem` menggunakan Singleton sehingga dapat dipanggil dari berbagai script.

---

# 2. Cara Kerja

Alur utama sistem:

```text
Enemy Mati
    ↓
DropSystem.DropCandy()
    ↓
Instantiate Candy Prefab
    ↓
CandyPickup menerima jumlah candy
    ↓
Candy berada di dunia
    ↓
Player menyentuh Candy
    ↓
CandyPickup mendeteksi Player
    ↓
CurrencySystem.AddCurrency()
    ↓
Candy dihancurkan
```

### Penjelasan

### 2.1 Enemy Mati

Ketika enemy mati, sistem kematian enemy memanggil:

```csharp
DropSystem.Instance.DropCandy(
    transform.position
);
```

Posisi enemy digunakan sebagai lokasi munculnya candy.

---

### 2.2 DropSystem Spawn Candy

`DropSystem` kemudian melakukan `Instantiate()` terhadap prefab candy.

```csharp
GameObject candy = Instantiate(
    candyPrefab,
    position,
    Quaternion.identity
);
```

Candy akan dibuat pada posisi enemy yang mati.

---

### 2.3 Menentukan Jumlah Candy

`DropSystem` memiliki nilai default:

```csharp
[SerializeField]
private int defaultCandyAmount = 1;
```

Jika menggunakan:

```csharp
DropSystem.Instance.DropCandy(position);
```

maka candy menggunakan jumlah default.

Sedangkan jika menggunakan:

```csharp
DropSystem.Instance.DropCandy(position, 3);
```

maka candy tersebut memiliki nilai **3 candy**.

---

### 2.4 CandyPickup

Setiap prefab candy memiliki component `CandyPickup`.

Component ini bertugas mendeteksi apakah player menyentuh candy.

```csharp
private void OnTriggerEnter2D(Collider2D other)
{
    if (isCollected)
        return;

    if (!other.CompareTag("Player"))
        return;

    isCollected = true;

    CurrencySystem.Instance.AddCurrency(amount);

    Destroy(gameObject);
}
```

Jika object yang menyentuh candy memiliki Tag `Player`, candy akan dianggap diambil.

---

### 2.5 Currency Ditambahkan

Setelah candy diambil, `CandyPickup` memanggil:

```csharp
CurrencySystem.Instance.AddCurrency(amount);
```

Currency player kemudian bertambah sesuai jumlah candy.

Contoh:

```text
Candy Amount = 3
Player Currency = 10

Player mengambil Candy

10 + 3 = 13
```

Setelah currency ditambahkan, object candy dihancurkan:

```csharp
Destroy(gameObject);
```

Hal ini mencegah candy yang sama diambil berkali-kali.

---

# 3. Cara Pakai

## 3.1 Membuat Candy Prefab

Buat GameObject baru untuk candy.

Struktur sederhananya:

```text
Candy
├── SpriteRenderer
├── CircleCollider2D
└── CandyPickup
```

Tambahkan:

- `SpriteRenderer`
- `Collider2D`
- `CandyPickup`

---

## 3.2 Konfigurasi Collider

Pada `Collider2D` candy:

```text
Is Trigger = ✓
```

Collider digunakan untuk mendeteksi ketika player menyentuh candy.

---

## 3.3 Konfigurasi Rigidbody2D

Jika candy tidak perlu dipengaruhi gravitasi, tambahkan `Rigidbody2D`.

Contoh:

```text
Body Type      = Kinematic
Gravity Scale  = 0
```

Tujuannya agar candy tidak jatuh atau terpengaruh physics secara tidak diperlukan.

---

## 3.4 Konfigurasi Player

Pastikan GameObject player memiliki:

```text
Tag = Player
```

Karena `CandyPickup` menggunakan:

```csharp
other.CompareTag("Player")
```

Jika player tidak memiliki Tag `Player`, candy tidak akan terambil.

---

# 4. Membuat DropSystem

Buat GameObject baru di scene:

```text
DropSystem
```

Kemudian tambahkan component:

```text
DropSystem
```

Inspector:

```text
Drop System
├── Candy Prefab       → Candy Prefab
└── Default Candy Amount → 1
```

Drag prefab candy ke field `Candy Prefab`.

---

# 5. Membuat Candy Prefab

Setelah candy selesai dibuat:

1. Drag object Candy dari Hierarchy ke folder `Prefabs`.
2. Hapus object Candy dari scene jika tidak diperlukan.
3. Gunakan prefab tersebut sebagai `Candy Prefab` pada `DropSystem`.

Dengan demikian `DropSystem` dapat membuat candy secara otomatis ketika dibutuhkan.

---

# 6. Integrasi dengan Enemy

Pada bagian ketika enemy mati, panggil:

```csharp
DropSystem.Instance.DropCandy(
    transform.position
);
```

Contoh:

```csharp
private void Die()
{
    DropSystem.Instance.DropCandy(
        transform.position
    );

    Destroy(gameObject);
}
```

Alurnya menjadi:

```text
Enemy.Die()
    ↓
DropSystem.DropCandy()
    ↓
Candy Spawn
    ↓
Enemy Destroy
```

---

# 7. Drop dengan Jumlah Berbeda

Setiap enemy dapat memberikan jumlah candy yang berbeda.

Contoh enemy biasa:

```csharp
DropSystem.Instance.DropCandy(
    transform.position,
    1
);
```

Enemy kuat:

```csharp
DropSystem.Instance.DropCandy(
    transform.position,
    3
);
```

Boss:

```csharp
DropSystem.Instance.DropCandy(
    transform.position,
    10
);
```

Dengan begitu, balancing reward dapat dilakukan berdasarkan jenis enemy.

---

# 8. Struktur Sistem

Struktur sederhana:

```text
DropSystem
│
├── Candy Prefab
│   ├── SpriteRenderer
│   ├── Collider2D
│   ├── Rigidbody2D
│   └── CandyPickup
│
└── CurrencySystem
```

Hubungan antar sistem:

```text
Enemy
  │
  │ DropCandy()
  ↓
DropSystem
  │
  │ Instantiate()
  ↓
CandyPickup
  │
  │ AddCurrency()
  ↓
CurrencySystem
```

---

# 9. Pencegahan Double Pickup

`CandyPickup` memiliki:

```csharp
private bool isCollected;
```

Ketika player berhasil mengambil candy:

```csharp
isCollected = true;
```

Sistem akan mengabaikan trigger berikutnya.

Hal ini berguna untuk mencegah currency bertambah lebih dari satu kali akibat beberapa collision/trigger yang terjadi pada frame yang berdekatan.

---

# 10. Troubleshooting

| Masalah                                    | Kemungkinan Penyebab                             | Solusi                                                    |
| ------------------------------------------ | ------------------------------------------------ | --------------------------------------------------------- |
| Candy tidak muncul                         | `Candy Prefab` kosong                            | Assign prefab pada `DropSystem`                           |
| Candy tidak bisa diambil                   | Collider tidak menggunakan Trigger               | Aktifkan `Is Trigger`                                     |
| Player menyentuh tetapi candy tidak hilang | Tag player salah                                 | Set Tag menjadi `Player`                                  |
| Currency tidak bertambah                   | `CurrencySystem` tidak tersedia / method berbeda | Pastikan `CurrencySystem.Instance.AddCurrency()` tersedia |
| Candy langsung jatuh                       | Rigidbody2D menggunakan Dynamic                  | Gunakan Kinematic atau atur Gravity Scale                 |
| Candy muncul lebih dari sekali             | `DropCandy()` dipanggil beberapa kali            | Pastikan proses `Die()` hanya dijalankan sekali           |
| Candy tidak memiliki nilai                 | `CandyPickup` tidak ditemukan                    | Pastikan component `CandyPickup` ada pada prefab          |

---

# 11. Catatan Pengembangan

Implementasi saat ini menggunakan satu prefab candy dengan nilai currency yang dapat diubah melalui `SetAmount()`.

Sistem ini dapat dikembangkan lebih lanjut menjadi:

```text
DropSystem
    ↓
Drop Table
    ├── Candy
    ├── Health
    ├── Power Up
    └── Item lainnya
```

Selain itu, drop dapat dikembangkan dengan:

- Random drop chance.
- Random jumlah candy.
- Jenis item berbeda.
- Drop berdasarkan tipe enemy.
- Object pooling untuk mengurangi `Instantiate()` dan `Destroy()`.
- Animasi candy ketika jatuh.
- Magnet pickup menuju player.
- Efek suara ketika candy diambil.

---

# 12. Kesimpulan

`DropSystem` bertanggung jawab terhadap **apa dan kapan item dijatuhkan**, sedangkan `CandyPickup` bertanggung jawab terhadap **apa yang terjadi ketika player mengambil item tersebut**.

Dengan pemisahan ini, sistem menjadi lebih mudah dikembangkan:

```text
Enemy mati
    ↓
DropSystem
    ↓
Candy
    ↓
CandyPickup
    ↓
CurrencySystem
```

Pemisahan tanggung jawab ini juga memungkinkan `DropSystem` nantinya menangani berbagai jenis item tanpa harus mengubah sistem enemy secara besar-besaran.
