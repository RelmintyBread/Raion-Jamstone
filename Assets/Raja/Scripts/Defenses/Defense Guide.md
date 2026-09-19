# Defense System – Detergentnation

Dokumentasi penggunaan sistem pertahanan (Defense System) pada game **Detergentnation** menggunakan Unity.

## 1. Overview

Defense System digunakan untuk menempatkan dan mengelola pertahanan yang membantu pemain melindungi minimarket dari serangan musuh.

Sistem ini terdiri dari beberapa script utama:

| Script                | Fungsi                                                      |
| --------------------- | ----------------------------------------------------------- |
| `DefenseType.cs`      | Enum untuk menentukan jenis defense.                        |
| `DefenseData.cs`      | Menyimpan konfigurasi defense menggunakan ScriptableObject. |
| `Defense.cs`          | Base class untuk sistem pertahanan.                         |
| `AmmoDefense.cs`      | Defense yang menggunakan amunisi.                           |
| `HealthDefense.cs`    | Defense yang memiliki health.                               |
| `SingleUseDefense.cs` | Defense yang hanya dapat diaktifkan sekali.                 |
| `DefenseManager.cs`   | Mengatur penempatan dan pengelolaan defense.                |

## 2. Jenis Defense

Jenis defense ditentukan melalui enum `DefenseType`.

```csharp
public enum DefenseType
{
    AmmoBased,
    HealthBased,
    SingleUse
}
```

| Tipe          | Deskripsi                                          | Contoh                   |
| ------------- | -------------------------------------------------- | ------------------------ |
| `AmmoBased`   | Defense menggunakan amunisi ketika menyerang.      | Soap Cannon, Soap Barrel |
| `HealthBased` | Defense memiliki health dan dapat menerima damage. | Fence                    |
| `SingleUse`   | Defense hanya dapat diaktifkan satu kali.          | Balloon Trap             |

> Catatan: Tipe di atas merupakan kategori konfigurasi. Efek serangan, deteksi musuh, dan mekanisme trigger perlu diimplementasikan pada sistem atau subclass terkait.

## 3. Membuat Defense Data

Setiap defense menggunakan `DefenseData` sebagai ScriptableObject untuk menyimpan konfigurasi.

### Langkah-langkah

1. Di Unity Project window, buka folder tempat menyimpan data defense.
2. Klik kanan pada folder tersebut.
3. Pilih menu pembuatan `DefenseData` jika tersedia melalui atribut `CreateAssetMenu` pada script.
4. Beri nama asset, misalnya `SoapCannonData`.
5. Atur konfigurasi pada Inspector.

### Konfigurasi DefenseData

| Field              | Fungsi                           |
| ------------------ | -------------------------------- |
| `defenseName`      | Nama defense.                    |
| `description`      | Deskripsi defense.               |
| `defenseType`      | Jenis defense.                   |
| `defensePrefab`    | Prefab yang akan di-instantiate. |
| `defenseSprite`    | Sprite representasi defense.     |
| `defensePointCost` | Biaya penempatan defense.        |
| `maxHealth`        | Health maksimum defense.         |
| `maxAmmo`          | Jumlah amunisi maksimum.         |
| `attackDamage`     | Nilai damage serangan.           |
| `attackCooldown`   | Jeda antarserangan.              |

Contoh konfigurasi:

**Soap Cannon**

- `defenseType`: AmmoBased
- `defensePointCost`: sesuai balancing game
- `maxHealth`: sesuai balancing game
- `maxAmmo`: sesuai balancing game
- `attackDamage`: sesuai balancing game
- `attackCooldown`: sesuai balancing game

Nilai konfigurasi dapat disesuaikan dengan kebutuhan balancing permainan.

## 4. Menyiapkan Prefab Defense

Setiap defense perlu memiliki prefab yang akan digunakan oleh `DefenseManager`.

### Langkah-langkah

1. Buat GameObject untuk defense di Scene.
2. Tambahkan komponen script yang sesuai:
   - `AmmoDefense` untuk defense berbasis amunisi.
   - `HealthDefense` untuk defense berbasis health.
   - `SingleUseDefense` untuk defense sekali pakai.

3. Pastikan komponen tersebut mewarisi `Defense`.
4. Simpan GameObject sebagai prefab.
5. Masukkan prefab ke field `defensePrefab` pada asset `DefenseData`.

Pastikan prefab memiliki konfigurasi dan komponen pendukung yang diperlukan oleh implementasi defense tersebut.

## 5. Menyiapkan DefenseManager

`DefenseManager` bertanggung jawab mengatur defense point dan penempatan defense.

### Langkah-langkah

1. Buat GameObject bernama `DefenseManager` di Scene.
2. Tambahkan komponen `DefenseManager`.
3. Atur nilai `initialDefensePoints` atau field defense point yang tersedia pada Inspector.
4. Buat beberapa GameObject kosong sebagai titik penempatan defense.
5. Masukkan Transform titik tersebut ke array `defensePointsInScene`.

Contoh hierarki:

```text
Scene
├── GameManager
├── DefenseManager
├── DefensePoints
│   ├── Point_01
│   ├── Point_02
│   └── Point_03
└── RnA_Mart
```

Setiap Transform pada `defensePointsInScene` mewakili lokasi yang dapat digunakan untuk menempatkan defense.

## 6. Alur Penempatan Defense

Penempatan defense dilakukan melalui method `PlaceDefense()` pada `DefenseManager`.

Alur dasarnya:

```text
Pemain memilih defense
        |
        v
DefenseManager.CanPlace()
        |
        v
Cek defense point dan slot
        |
        v
DefenseManager.PlaceDefense()
        |
        v
Instantiate defense prefab
        |
        v
Initialize DefenseData
        |
        v
Kurangi defense point
        |
        v
Simpan defense ke daftar
```

### Method yang tersedia

| Method                           | Fungsi                                                                        |
| -------------------------------- | ----------------------------------------------------------------------------- |
| `CanPlace()`                     | Memeriksa apakah defense dapat ditempatkan berdasarkan kondisi yang tersedia. |
| `PlaceDefense(DefenseData, int)` | Menempatkan defense pada titik yang dipilih.                                  |
| `AddDefensePoints(int)`          | Menambahkan defense point.                                                    |
| `SpendDefensePoints(int)`        | Mengurangi defense point.                                                     |
| `RemoveDefense(Defense)`         | Menghapus defense dari daftar pengelolaan.                                    |
| `RemoveAllDefenses()`            | Menghapus semua defense yang dikelola.                                        |

Contoh pemanggilan:

```csharp
// Contoh: menempatkan defense pada slot pertama.
defenseManager.PlaceDefense(soapCannonData, 0);
```

`soapCannonData` merupakan referensi ke asset `DefenseData`, sedangkan `0` merupakan indeks titik penempatan pada `defensePointsInScene`.

Pastikan indeks yang digunakan valid dan defense point mencukupi sebelum memanggil method penempatan.

## 7. Mengelola Health Defense

Base class `Defense` menyediakan mekanisme dasar untuk health.

### Method utama

| Method                    | Fungsi                                     |
| ------------------------- | ------------------------------------------ |
| `Initialize(DefenseData)` | Menginisialisasi defense menggunakan data. |
| `TakeDamage(int)`         | Mengurangi health defense.                 |
| `Repair(int)`             | Memulihkan health defense.                 |
| `IsDestroyed()`           | Memeriksa apakah defense telah hancur.     |
| `DestroyDefense()`        | Menghancurkan defense.                     |

Contoh:

```csharp
// Memberikan damage kepada defense.
defense.TakeDamage(10);

// Memulihkan health defense.
defense.Repair(5);
```

Ketika health mencapai kondisi hancur, defense dapat dihancurkan melalui mekanisme `DestroyDefense()`.

## 8. Menggunakan AmmoDefense

`AmmoDefense` merupakan subclass `Defense` yang mengelola amunisi dan cooldown serangan.

Method yang tersedia:

| Method             | Fungsi                                    |
| ------------------ | ----------------------------------------- |
| `CanAttack()`      | Memeriksa apakah defense dapat menyerang. |
| `TryConsumeAmmo()` | Mencoba menggunakan satu amunisi.         |
| `Reload()`         | Mengisi kembali amunisi.                  |

Contoh penggunaan:

```csharp
if (ammoDefense.CanAttack())
{
    if (ammoDefense.TryConsumeAmmo())
    {
        // Jalankan logika serangan.
    }
}
```

Contoh di atas hanya menggambarkan penggunaan API amunisi. Sistem targeting, projectile, dan pemberian damage kepada musuh belum termasuk dalam implementasi dasar ini.

## 9. Menggunakan HealthDefense

`HealthDefense` merupakan subclass `Defense` untuk defense yang memiliki health.

Class ini meneruskan mekanisme `TakeDamage()` dan `Repair()` dari base class.

Contoh:

```csharp
healthDefense.TakeDamage(15);
```

Implementasi dasar ini belum menambahkan perilaku khusus seperti efek ketika diserang atau interaksi dengan musuh.

## 10. Menggunakan SingleUseDefense

`SingleUseDefense` merupakan subclass `Defense` yang dapat diaktifkan satu kali.

Method utama:

```csharp
singleUseDefense.Activate();
```

Ketika diaktifkan, sistem akan menjalankan `OnActivated()` lalu menghancurkan defense.

Subclass dapat mengimplementasikan `OnActivated()` untuk menentukan efek khususnya.

> Catatan: Deteksi musuh, trigger otomatis, dan efek Balloon Trap belum disediakan oleh implementasi dasar.

## 11. Integrasi dengan Gameplay

Defense System dapat digunakan dalam fase persiapan pertahanan sebelum wave dimulai.

Contoh alur gameplay:

```text
Defense Planning
       |
       v
Pemain memilih defense
       |
       v
DefenseManager memeriksa penempatan
       |
       v
Defense ditempatkan
       |
       v
Defense point diperbarui
       |
       v
Wave dimulai
       |
       v
Defense digunakan untuk membantu
melindungi minimarket
```

Integrasi dengan `GameManager`, `WaveManager`, UI pemilihan defense, serta sistem musuh perlu dilakukan sesuai implementasi project.

## 12. Checklist Setup

- [ ] Setiap defense memiliki `DefenseData`.
- [ ] Setiap `DefenseData` memiliki prefab yang sesuai.
- [ ] Prefab menggunakan subclass `Defense` yang tepat.
- [ ] `DefenseManager` sudah dipasang pada Scene.
- [ ] Defense point awal sudah dikonfigurasi.
- [ ] Titik penempatan sudah dimasukkan ke `defensePointsInScene`.
- [ ] Indeks slot yang digunakan valid.
- [ ] Biaya defense sesuai dengan defense point yang tersedia.
- [ ] Sistem targeting, trigger, atau efek khusus sudah ditambahkan jika diperlukan.

## 13. Catatan Pengembangan

Implementasi dasar Defense System berfokus pada struktur class, konfigurasi data, penempatan, defense point, health, amunisi, dan aktivasi sekali pakai.

Fitur berikut masih memerlukan implementasi lanjutan apabila dibutuhkan oleh gameplay:

- Validasi kecocokan `DefenseType` dengan subclass prefab.
- Sistem targeting dan projectile untuk AmmoDefense.
- Deteksi tabrakan atau trigger untuk SingleUseDefense.
- Efek khusus setiap jenis defense.
- Integrasi event dengan sistem wave dan enemy.
- UI pemilihan, preview, dan penempatan defense.
- Aturan refund atau pengembalian defense point.
- Pencegahan inisialisasi berulang jika prefab dan manager sama-sama memanggil `Initialize()`.
