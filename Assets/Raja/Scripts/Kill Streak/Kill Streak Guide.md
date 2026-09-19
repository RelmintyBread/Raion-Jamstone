# Killstreak System

Sistem Killstreak digunakan untuk menghitung jumlah musuh yang berhasil dikalahkan secara beruntun. Pemain akan mendapatkan bonus reward ketika mencapai milestone killstreak tertentu.

## 1. Fitur

- Menghitung jumlah musuh yang mati secara beruntun.
- Mereset killstreak jika tidak ada kill dalam waktu tertentu.
- Memberikan bonus reward saat mencapai milestone.
- Menyediakan event untuk memperbarui UI.
- Mencegah enemy yang sama didaftarkan lebih dari satu kali.

## 2. Cara Kerja

Alur kerja Killstreak:

```text
EnemySpawner
    ↓
Enemy di-spawn
    ↓
RegisterEnemy(enemy)
    ↓
Enemy mati
    ↓
OnEnemyDied dipanggil
    ↓
Killstreak bertambah
    ↓
Timer di-reset
    ↓
Cek milestone
    ↓
Jika tercapai → Berikan bonus reward
```

### Penjelasan

1. **Registrasi enemy**
   Setiap enemy yang di-spawn didaftarkan ke `KillstreakSystem` melalui `RegisterEnemy()`.

2. **Enemy mati**
   Ketika enemy mati, event `OnEnemyDied` akan memanggil handler pada KillstreakSystem.

3. **Killstreak bertambah**
   Nilai `currentKillstreak` bertambah satu dan timer reset ke durasi yang ditentukan.

4. **Pengecekan milestone**
   Sistem memeriksa apakah jumlah kill saat ini sama dengan salah satu `requiredKills`.

5. **Pemberian reward**
   Jika milestone tercapai, sistem memanggil `RewardSystem.GiveReward()` untuk memberikan bonus.

6. **Reset killstreak**
   Jika timer habis sebelum ada kill berikutnya, killstreak akan direset menjadi 0.

## 3. Setup di Unity

### A. Menambahkan KillstreakSystem

1. Buat GameObject baru bernama `KillstreakSystem`.
2. Tambahkan component script `KillstreakSystem`.
3. Hubungkan referensi `RewardSystem` pada Inspector.
4. Atur nilai `Streak Reset Time`.
5. Tambahkan daftar milestone sesuai kebutuhan game.

### B. Konfigurasi Milestone

Setiap milestone memiliki dua properti:

| Property         | Fungsi                            |
| ---------------- | --------------------------------- |
| `Required Kills` | Jumlah kill yang harus dicapai    |
| `Candy Bonus`    | Jumlah bonus candy yang diberikan |

Contoh konfigurasi:

| Required Kills | Candy Bonus |
| -------------: | ----------: |
|              3 |          10 |
|              5 |          25 |
|             10 |          50 |

Dengan konfigurasi tersebut, pemain mendapatkan bonus ketika mencapai kill ke-3, ke-5, dan ke-10.

> Nilai di atas hanya contoh konfigurasi. Sesuaikan dengan balancing game.

## 4. Integrasi dengan EnemySpawner

Setiap enemy yang di-spawn harus didaftarkan ke KillstreakSystem.

Tambahkan referensi berikut pada script `EnemySpawner`:

```csharp
[SerializeField]
private KillstreakSystem killstreakSystem;
```

Kemudian, setelah enemy berhasil di-spawn:

```csharp
Enemy enemy = Instantiate(
    enemyPrefab,
    spawnPosition,
    Quaternion.identity
);

killstreakSystem.RegisterEnemy(enemy);
```

Sesuaikan `enemyPrefab` dan `spawnPosition` dengan implementasi EnemySpawner yang digunakan.

**Penting:** Pastikan semua enemy yang ingin dihitung killstreak-nya didaftarkan ke sistem.

## 5. Integrasi dengan Enemy

KillstreakSystem menggunakan event `OnEnemyDied` dari script `Enemy`.

Pastikan script Enemy memiliki event yang dipanggil saat enemy mati, misalnya:

```csharp
public event System.Action OnEnemyDied;
```

Ketika enemy mati, event harus dipanggil sebelum object dihancurkan:

```csharp
OnEnemyDied?.Invoke();
Destroy(gameObject);
```

Jika sistem kematian enemy sudah memiliki event tersebut, tidak perlu membuat event baru. Gunakan event yang sudah ada.

## 6. Integrasi UI

Buat GameObject UI untuk menampilkan jumlah killstreak, lalu tambahkan script `KillstreakUI`.

Contoh implementasi:

```csharp
using TMPro;
using UnityEngine;

public class KillstreakUI : MonoBehaviour
{
    [SerializeField]
    private KillstreakSystem killstreakSystem;

    [SerializeField]
    private TMP_Text killstreakText;

    private void OnEnable()
    {
        killstreakSystem.OnKillstreakChanged
            += UpdateKillstreakUI;
    }

    private void OnDisable()
    {
        killstreakSystem.OnKillstreakChanged
            -= UpdateKillstreakUI;
    }

    private void UpdateKillstreakUI(int killstreak)
    {
        killstreakText.text =
            $"Killstreak: {killstreak}";
    }
}
```

### Setup UI

1. Buat UI Text menggunakan TextMeshPro.
2. Tambahkan script `KillstreakUI`.
3. Assign `KillstreakSystem` ke field `Killstreak System`.
4. Assign TMP Text ke field `Killstreak Text`.

UI akan diperbarui setiap kali nilai killstreak berubah.

## 7. Event yang Tersedia

| Event                 | Kegunaan                                   |
| --------------------- | ------------------------------------------ |
| `OnKillstreakChanged` | Dipanggil ketika jumlah killstreak berubah |
| `OnMilestoneReached`  | Dipanggil ketika milestone tercapai        |
| `OnKillstreakReset`   | Dipanggil ketika killstreak direset        |

Event tersebut dapat digunakan untuk menambahkan fitur lain, seperti animasi UI, efek suara, atau notifikasi milestone.

## 8. Ketentuan Sistem

- Killstreak bertambah setiap kali enemy terdaftar mati.
- Timer akan di-reset setiap kali kill baru tercatat.
- Killstreak akan kembali ke 0 jika timer habis.
- Bonus reward hanya diberikan ketika milestone tercapai.
- Enemy yang sama tidak akan didaftarkan berulang kali.
- Event enemy yang terdaftar akan dilepas ketika sistem dihancurkan.

## 9. Catatan Pengembangan

Saat ini, sistem menghitung kematian semua enemy yang telah didaftarkan. Sistem belum memeriksa apakah enemy benar-benar dibunuh oleh pemain.

Jika game membutuhkan killstreak berdasarkan kill pemain saja, tambahkan validasi sumber damage atau killer pada sistem combat sebelum kill dicatat.

Selain itu, reward normal dari enemy tetap ditangani oleh `RewardSystem` atau sistem reward enemy yang sudah ada. KillstreakSystem hanya memberikan **bonus reward saat milestone tercapai**.

## 10. Troubleshooting

| Masalah                           | Kemungkinan penyebab                                                      |
| --------------------------------- | ------------------------------------------------------------------------- |
| Killstreak tidak bertambah        | Enemy belum didaftarkan melalui `RegisterEnemy()`                         |
| Killstreak tidak reset            | Timer tidak berjalan atau `Update()` tidak aktif                          |
| Bonus tidak diberikan             | Milestone belum dikonfigurasi atau referensi RewardSystem belum di-assign |
| UI tidak berubah                  | Event belum di-subscribe atau referensi UI belum di-assign                |
| Enemy terhitung lebih dari sekali | Periksa event kematian dan proses registrasi enemy                        |

---

**Kesimpulan:** KillstreakSystem menerima notifikasi kematian enemy, menghitung kill beruntun berdasarkan timer, lalu memberikan bonus reward ketika pemain mencapai milestone yang telah dikonfigurasi.
