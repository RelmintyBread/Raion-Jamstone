# Interactable System

Interactable System digunakan untuk menangani objek-objek yang dapat berinteraksi dengan player di dalam game.

Sistem ini terdiri dari empat script:

- **`Minimarket`** → Tempat player membuka atau mengakses Shop.
- **`SoapCenter`** → Tempat player mendapatkan atau mengisi sabun.
- **`Portal`** → Memindahkan player ke scene lain.
- **`RepairableObject`** → Objek yang memiliki HP dan dapat diperbaiki oleh player.

Secara umum, player harus berada di area trigger objek kemudian menekan tombol **E** untuk melakukan interaksi.

---

# 1. Struktur Sistem

```text
Player
  │
  │ Masuk Trigger
  ↓
Interactable Object
  │
  │ Tekan E
  ↓
Interaction
  │
  ├── Minimarket
  │       ↓
  │     Shop UI
  │
  ├── SoapCenter
  │       ↓
  │     Tambah Soap
  │
  ├── Portal
  │       ↓
  │     Load Scene
  │
  └── RepairableObject
          ↓
       Repair Object
```

---

# 2. Cara Kerja Umum

Keempat sistem menggunakan konsep yang sama.

### 2.1 Player Masuk Area Interaksi

Setiap objek memiliki `Collider2D` dengan:

```text
Is Trigger = true
```

Ketika player masuk ke collider:

```csharp
private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
        playerInRange = true;
}
```

Sistem akan mengetahui bahwa player sedang berada di dekat objek.

---

### 2.2 Player Menekan Tombol E

Pada `Update()`, sistem mengecek apakah player berada dalam jangkauan.

```csharp
if (!playerInRange)
    return;

if (Input.GetKeyDown(interactKey))
{
    Interact();
}
```

Default tombol interaksi:

```text
E
```

---

### 2.3 Player Keluar dari Area

Ketika player keluar dari trigger:

```csharp
private void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Player"))
        playerInRange = false;
}
```

Player tidak dapat melakukan interaksi sampai masuk kembali ke area objek.

---

# 3. Minimarket

## Fungsi

`Minimarket` digunakan sebagai titik interaksi untuk membuka Shop.

Alur:

```text
Player
  ↓
Masuk area Minimarket
  ↓
Tekan E
  ↓
Interact()
  ↓
OnInteract
  ↓
Buka Shop UI
```

## Setup

1. Buat GameObject `Minimarket`.
2. Tambahkan script `Minimarket`.
3. Tambahkan `Collider2D`.
4. Aktifkan:

```text
Is Trigger = true
```

5. Pastikan Player memiliki:

```text
Tag = Player
```

6. Hubungkan `On Interact` ke sistem UI Shop.

Contoh:

```text
Minimarket
├── Collider2D
└── Minimarket
```

---

# 4. SoapCenter

## Fungsi

`SoapCenter` digunakan sebagai tempat player mendapatkan atau mengisi soap.

Alur:

```text
Player
  ↓
Masuk area SoapCenter
  ↓
Tekan E
  ↓
CollectSoap()
  ↓
OnSoapCollected
  ↓
Tambah Soap
```

## Setup

1. Buat GameObject `SoapCenter`.
2. Tambahkan script `SoapCenter`.
3. Tambahkan `Collider2D`.
4. Aktifkan `Is Trigger`.
5. Pastikan Player memiliki Tag `Player`.
6. Hubungkan `On Soap Collected` dengan sistem inventory/equipment yang menangani soap.

Contoh:

```text
SoapCenter
├── Collider2D
└── SoapCenter
```

---

# 5. Portal

## Fungsi

`Portal` digunakan untuk memindahkan player dari satu scene ke scene lainnya.

Alur:

```text
Player
  ↓
Masuk area Portal
  ↓
Tekan E
  ↓
EnterPortal()
  ↓
Cek Target Scene
  ↓
SceneManager.LoadScene()
  ↓
Scene Baru
```

## Setup

1. Buat GameObject `Portal`.
2. Tambahkan script `Portal`.
3. Tambahkan `Collider2D`.
4. Aktifkan `Is Trigger`.
5. Pastikan Player memiliki Tag `Player`.
6. Isi:

```text
Target Scene Name
```

dengan nama scene tujuan.

Contoh:

```text
Portal
├── Collider2D
└── Portal

Target Scene Name = DefenseScene
```

Pastikan scene tujuan tersedia pada konfigurasi scene build project.

---

# 6. RepairableObject

## Fungsi

`RepairableObject` digunakan untuk objek yang memiliki HP dan dapat diperbaiki oleh player.

Contohnya:

- Defense.
- Barricade.
- Generator.
- Base/objective.
- Struktur pertahanan lainnya.

---

## Cara Kerja

Alur repair:

```text
Player
  ↓
Masuk area objek
  ↓
Tekan E
  ↓
RequestRepair()
  ↓
OnRepairRequested
  ↓
Cek biaya repair
  ↓
Bayar Currency
  ↓
Repair()
  ↓
HP bertambah
```

### HP

Object memiliki:

```text
Max Health
Current Health
Repair Amount
```

Contoh:

```text
Max Health     = 100
Current Health = 50
Repair Amount  = 25
```

Player melakukan repair:

```text
50 + 25 = 75 HP
```

Jika HP tidak cukup untuk menambahkan seluruh repair amount:

```text
Current Health = 90
Repair Amount  = 25

90 + 25 = 115
```

Sistem membatasi HP sampai maximum:

```text
Current Health = 100
```

---

# 7. Setup RepairableObject

1. Buat GameObject yang ingin dibuat repairable.
2. Tambahkan `RepairableObject`.
3. Tambahkan `Collider2D`.
4. Aktifkan `Is Trigger`.
5. Atur nilai:

```text
Max Health
Current Health
Repair Amount
```

Contoh:

```text
RepairableObject
├── Collider2D
└── RepairableObject

Max Health     = 100
Current Health = 100
Repair Amount  = 25
```

---

# 8. Damage pada RepairableObject

Objek dapat menerima damage menggunakan:

```csharp
TakeDamage(int damage)
```

Contoh:

```csharp
repairableObject.TakeDamage(20);
```

Jika:

```text
Current Health = 100
Damage         = 20
```

hasilnya:

```text
Current Health = 80
```

HP tidak akan turun di bawah 0.

---

# 9. Repair Object

Untuk melakukan repair secara langsung:

```csharp
repairableObject.Repair();
```

Sistem akan menggunakan `Repair Amount`.

Atau jumlah repair dapat ditentukan secara manual:

```csharp
repairableObject.Repair(50);
```

Contoh:

```text
Current Health = 40

Repair(50)

40 + 50 = 90
```

---

# 10. Integrasi Currency

`RepairableObject` tidak langsung mengurangi currency.

Ketika player menekan E:

```text
RequestRepair()
      ↓
OnRepairRequested
```

Event tersebut dapat dihubungkan ke sistem ekonomi.

Contoh alur:

```text
Player tekan E
      ↓
RepairableObject
      ↓
OnRepairRequested
      ↓
Cek CurrencySystem
      ↓
Currency cukup?
    /       \
  Ya         Tidak
  ↓            ↓
Bayar       Tidak repair
  ↓
Repair()
```

Dengan cara ini, `RepairableObject` tidak perlu mengetahui detail bagaimana currency dikelola.

---

# 11. Health Changed Event

`RepairableObject` menyediakan event:

```csharp
onHealthChanged
```

Event mengirimkan:

```text
currentHealth
maxHealth
```

Event ini dapat digunakan untuk UI health bar.

Contoh:

```text
RepairableObject
      ↓
OnHealthChanged
      ↓
Health Bar UI
      ↓
Update Fill Amount
```

Dengan demikian UI tidak perlu terus-menerus mengecek HP object.

---

# 12. Inspector Configuration

## Minimarket

```text
Minimarket
├── Interaction
│   ├── Interact Key = E
│   └── On Interact
```

## SoapCenter

```text
SoapCenter
├── Interaction
│   ├── Interact Key = E
│   └── On Soap Collected
```

## Portal

```text
Portal
├── Portal Settings
│   ├── Target Scene Name
│   └── Interact Key = E
```

## RepairableObject

```text
RepairableObject
├── Health
│   ├── Max Health
│   ├── Current Health
│   └── Repair Amount
│
└── Interaction
    ├── Interact Key = E
    ├── On Repair Requested
    └── On Health Changed
```

---

# 13. Troubleshooting

| Masalah                          | Kemungkinan Penyebab                  | Solusi                                                    |
| -------------------------------- | ------------------------------------- | --------------------------------------------------------- |
| Tidak bisa interaksi             | Player tidak memiliki Tag `Player`    | Tambahkan Tag `Player`                                    |
| Trigger tidak terdeteksi         | Collider bukan Trigger                | Aktifkan `Is Trigger`                                     |
| Tombol E tidak bekerja           | Input tidak sesuai                    | Pastikan sistem input menggunakan konfigurasi yang sesuai |
| Minimarket tidak membuka Shop    | `On Interact` belum dihubungkan       | Assign method pada UnityEvent                             |
| Soap tidak bertambah             | `On Soap Collected` belum dihubungkan | Hubungkan ke inventory/equipment                          |
| Portal tidak berpindah scene     | Target scene kosong                   | Isi `Target Scene Name`                                   |
| Repair tidak menambah HP         | Object sudah full HP                  | Pastikan `Current Health < Max Health`                    |
| Repair tidak mengurangi currency | Event belum terhubung ke economy      | Hubungkan `On Repair Requested` dengan sistem currency    |
| Health Bar tidak berubah         | `On Health Changed` belum dihubungkan | Hubungkan event ke UI                                     |

---

# 14. Catatan Arsitektur

Keempat script memiliki tanggung jawab yang berbeda.

```text
Minimarket
    ↓
Shop

SoapCenter
    ↓
Inventory / Equipment

Portal
    ↓
Scene Management

RepairableObject
    ↓
Health / Repair System
```

Objek interactable hanya menangani **interaksi dan aksi spesifiknya**.

Logic sistem lain tetap berada pada sistem masing-masing.

Contohnya, `RepairableObject` tidak bertanggung jawab mengatur ekonomi:

```text
RepairableObject
    ↓
"Player ingin repair"

CurrencySystem
    ↓
"Apakah currency cukup?"

RepairableObject
    ↓
"Repair berhasil"
```

Pendekatan ini membuat setiap sistem lebih modular dan lebih mudah dikembangkan.

---

# 15. Pengembangan Selanjutnya

Sistem ini dapat dikembangkan dengan membuat sistem interaksi yang lebih terpusat:

```text
PlayerInteraction
       ↓
Detect Interactable
       ↓
Press E
       ↓
IInteractable
       ↓
┌──────────────┬──────────────┬──────────────┐
│ Minimarket   │ SoapCenter   │ Portal       │
│ Repairable   │              │              │
└──────────────┴──────────────┴──────────────┘
```

Dengan pendekatan tersebut, setiap objek tidak perlu memiliki pengecekan tombol `E` sendiri.

Player cukup memiliki satu sistem:

```text
PlayerInteraction
```

yang bertanggung jawab mendeteksi objek terdekat dan menjalankan interaksinya.

---

# Kesimpulan

Interactable System menyediakan fondasi untuk objek yang dapat digunakan player di dalam game.

```text
Player
  ↓
Detect Object
  ↓
Press E
  ↓
Interactable
  ↓
Action
```

Dengan pembagian tanggung jawab:

- `Minimarket` → interaksi Shop.
- `SoapCenter` → interaksi Soap.
- `Portal` → perpindahan Scene.
- `RepairableObject` → HP dan Repair.

Struktur ini memungkinkan fitur baru ditambahkan tanpa mengubah sistem utama player secara berlebihan.
