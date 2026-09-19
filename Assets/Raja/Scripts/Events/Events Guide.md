EventBus Documentation — Detergentnation

1. Apa itu EventBus?
   `EventBus` adalah sistem komunikasi berbasis event yang memungkinkan satu sistem memberi tahu sistem lain bahwa sesuatu telah terjadi, tanpa harus memiliki referensi langsung ke sistem penerima.
   Contoh:
   `HealthSystem` memberi tahu bahwa HP berubah.
   UI menerima `HealthChangedEvent` untuk memperbarui health bar.
   `Enemy` memberi tahu bahwa musuh dikalahkan.
   `RewardSystem` menerima `EnemyKilledEvent` untuk memproses reward.
   Dengan EventBus, sistem pengirim tidak perlu mengetahui siapa saja yang menerima event tersebut.
   > **Catatan:** EventBus yang digunakan di project ini bersifat static. EventBus bukan `MonoBehaviour` dan tidak perlu dipasang ke GameObject.

---

2. Alur Kerja EventBus

```text
Sistem Pengirim
    |
    | EventBus.Publish(event)
    v
   EventBus
    |
    | Mengirim event ke semua subscriber dengan tipe event yang sama
    v
Sistem Penerima
    |
    | Callback dijalankan
    v
Memperbarui data / UI / gameplay
```

Contoh alur saat musuh dikalahkan:

```text
Enemy
  -> Publish(EnemyKilledEvent)
      -> RewardSystem menerima event
      -> UI menerima event (jika subscribe)
      -> Sistem lain yang subscribe menerima event
```

---

3. Cara Menggunakan EventBus
   3.1 Subscribe — Menerima Event
   Gunakan `EventBus.Subscribe<T>()` untuk mendaftarkan method sebagai penerima event.
   Pada `MonoBehaviour`, biasanya subscribe dilakukan di `OnEnable()`.

```csharp
private void OnEnable()
{
    EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
}

private void OnEnemyKilled(EnemyKilledEvent eventData)
{
    Debug.Log("Musuh dikalahkan!");
}
```

Keterangan:
`EnemyKilledEvent` adalah tipe event yang ingin diterima.
`OnEnemyKilled` adalah callback yang dipanggil saat event diterbitkan.
`eventData` berisi data yang dikirim oleh pengirim event.
3.2 Unsubscribe — Berhenti Menerima Event
Gunakan `EventBus.Unsubscribe<T>()` untuk menghapus callback yang sebelumnya didaftarkan.
Pada `MonoBehaviour`, lakukan di `OnDisable()` agar objek yang tidak aktif tidak terus menerima event.

```csharp
private void OnDisable()
{
    EventBus.Unsubscribe<EnemyKilledEvent>(OnEnemyKilled);
}
```

Pastikan method yang digunakan untuk unsubscribe sama dengan method yang digunakan saat subscribe.
3.3 Publish — Mengirim Event
Gunakan `EventBus.Publish()` ketika suatu kejadian berlangsung.

```csharp
EventBus.Publish(new EnemyKilledEvent(enemy));
```

EventBus akan memanggil callback semua subscriber yang terdaftar untuk tipe `EnemyKilledEvent`.
3.4 Contoh Lengkap Subscriber

```csharp
using UnityEngine;

public class EnemyKillListener : MonoBehaviour
{
    private void OnEnable()
    {
        EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<EnemyKilledEvent>(OnEnemyKilled);
    }

    private void OnEnemyKilled(EnemyKilledEvent eventData)
    {
        Debug.Log("Enemy killed: " + eventData.Enemy);
    }
}
```

---

4. Daftar Event
   Event-event berikut adalah `struct` yang berfungsi sebagai wadah data. Event tidak menjalankan gameplay sendiri; event hanya membawa informasi dari pengirim kepada subscriber.
   4.1 `EnemyKilledEvent`
   Tujuan: Memberi tahu sistem bahwa sebuah enemy telah dikalahkan.
   Data:
   `Enemy`: referensi ke enemy yang dikalahkan.
   Contoh publish — dipanggil ketika enemy benar-benar mati:

```csharp
EventBus.Publish(new EnemyKilledEvent(this));
```

Contoh subscriber:

```csharp
private void OnEnable()
{
    EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
}

private void OnDisable()
{
    EventBus.Unsubscribe<EnemyKilledEvent>(OnEnemyKilled);
}

private void OnEnemyKilled(EnemyKilledEvent eventData)
{
    Enemy enemy = eventData.Enemy;
    Debug.Log("Enemy defeated: " + enemy);
}
```

## Contoh penggunaan: `RewardSystem` memproses reward, `WaveManager` memperbarui jumlah enemy yang tersisa, atau UI memperbarui kill counter.

4.2 `PlayerDamagedEvent`
Tujuan: Memberi tahu sistem bahwa player menerima damage.
Data:
`Damage`: jumlah damage yang diterima.
`CurrentHealth`: HP player setelah menerima damage.
Contoh publish:

```csharp
EventBus.Publish(new PlayerDamagedEvent(damage, currentHealth));
```

Contoh subscriber:

```csharp
private void OnPlayerDamaged(PlayerDamagedEvent eventData)
{
    Debug.Log($"Player menerima {eventData.Damage} damage. HP: {eventData.CurrentHealth}");
}
```

## Contoh penggunaan: efek visual saat terkena serangan, sound effect, atau feedback damage.

4.3 `HealthChangedEvent`
Tujuan: Memberi tahu sistem bahwa nilai health suatu target berubah. Dapat digunakan untuk player maupun minimarket.
Data:
`Target`: GameObject yang health-nya berubah.
`CurrentHealth`: health saat ini.
`MaxHealth`: health maksimum.
Contoh publish:

```csharp
EventBus.Publish(new HealthChangedEvent(gameObject, currentHealth, maxHealth));
```

Contoh subscriber:

```csharp
private void OnHealthChanged(HealthChangedEvent eventData)
{
    float healthRatio = eventData.MaxHealth > 0
        ? eventData.CurrentHealth / eventData.MaxHealth
        : 0f;

    Debug.Log($"{eventData.Target.name} health: {healthRatio:P0}");
}
```

Contoh penggunaan: memperbarui health bar player atau minimarket.

> `PlayerDamagedEvent` berfokus pada kejadian player terkena damage, sedangkan `HealthChangedEvent` berfokus pada perubahan health target secara umum.

---

4.4 `AmmoChangedEvent`
Tujuan: Memberi tahu sistem bahwa jumlah amunisi berubah.
Data:
`CurrentAmmo`: amunisi yang tersedia di magazine/senjata.
`CarriedAmmo`: amunisi cadangan yang dibawa.
Contoh publish:

```csharp
EventBus.Publish(new AmmoChangedEvent(currentAmmo, carriedAmmo));
```

Contoh subscriber:

```csharp
private void OnAmmoChanged(AmmoChangedEvent eventData)
{
    Debug.Log($"Ammo: {eventData.CurrentAmmo} | Cadangan: {eventData.CarriedAmmo}");
}
```

## Contoh penggunaan: memperbarui tampilan ammo pada HUD.

4.5 `CurrencyChangedEvent`
Tujuan: Memberi tahu sistem bahwa jumlah Candy milik player berubah.
Data:
`CurrentCandy`: total Candy saat ini.
Contoh publish:

```csharp
EventBus.Publish(new CurrencyChangedEvent(currentCandy));
```

Contoh subscriber:

```csharp
private void OnCurrencyChanged(CurrencyChangedEvent eventData)
{
    Debug.Log("Candy: " + eventData.CurrentCandy);
}
```

## Contoh penggunaan: memperbarui jumlah Candy di HUD atau tampilan Shop.

4.6 `KillStreakChangedEvent`
Tujuan: Memberi tahu sistem bahwa kill streak atau multiplier berubah.
Data:
`CurrentStreak`: jumlah kill beruntun saat ini.
`Multiplier`: multiplier yang sedang berlaku.
Contoh publish:

```csharp
EventBus.Publish(new KillStreakChangedEvent(currentStreak, multiplier));
```

Contoh subscriber:

```csharp
private void OnKillStreakChanged(KillStreakChangedEvent eventData)
{
    Debug.Log($"Kill streak: {eventData.CurrentStreak} | Multiplier: {eventData.Multiplier}");
}
```

## Contoh penggunaan: menampilkan kill streak dan multiplier reward pada HUD.

4.7 `WaveStartedEvent`
Tujuan: Memberi tahu sistem bahwa wave baru dimulai.
Data:
`WaveNumber`: nomor wave yang dimulai.
Contoh publish:

```csharp
EventBus.Publish(new WaveStartedEvent(waveNumber));
```

Contoh subscriber:

```csharp
private void OnWaveStarted(WaveStartedEvent eventData)
{
    Debug.Log("Wave " + eventData.WaveNumber + " dimulai!");
}
```

## Contoh penggunaan: memperbarui label wave, memulai musik/efek wave, atau mengaktifkan elemen HUD.

4.8 `WaveCompletedEvent`
Tujuan: Memberi tahu sistem bahwa suatu wave telah selesai.
Data:
`WaveNumber`: nomor wave yang selesai.
Contoh publish:

```csharp
EventBus.Publish(new WaveCompletedEvent(waveNumber));
```

Contoh subscriber:

```csharp
private void OnWaveCompleted(WaveCompletedEvent eventData)
{
    Debug.Log("Wave " + eventData.WaveNumber + " selesai!");
}
```

## Contoh penggunaan: memulai fase `WaveBreak`, menampilkan hasil wave, atau menyiapkan wave berikutnya.

4.9 `GameStateChangedEvent`
Tujuan: Memberi tahu sistem bahwa state permainan berubah.
Data:
`PreviousState`: state sebelum perubahan.
`CurrentState`: state setelah perubahan.
Contoh publish:

```csharp
EventBus.Publish(new GameStateChangedEvent(previousState, newState));
```

Contoh subscriber:

```csharp
private void OnGameStateChanged(GameStateChangedEvent eventData)
{
    Debug.Log($"State: {eventData.PreviousState} -> {eventData.CurrentState}");
}
```

## Contoh penggunaan: mengatur tampilan menu, HUD, pause panel, shop, victory panel, atau game over panel.

4.10 `EquipmentChangedEvent`
Tujuan: Memberi tahu sistem bahwa equipment yang digunakan atau dipilih berubah.
Data:
`EquipmentType`: jenis equipment.
`Equipment`: data equipment yang dipilih.
Contoh publish:

```csharp
EventBus.Publish(new EquipmentChangedEvent(equipmentType, equipmentData));
```

Contoh subscriber:

```csharp
private void OnEquipmentChanged(EquipmentChangedEvent eventData)
{
    Debug.Log("Equipment berubah: " + eventData.EquipmentType);
}
```

## Contoh penggunaan: memperbarui ikon equipment di HUD atau memperbarui tampilan equipment yang sedang digunakan.

4.11 `DefenseChangedEvent`
Tujuan: Memberi tahu sistem bahwa defense dipasang atau dilepas.
Data:
`Defense`: defense yang berubah.
`IsPlaced`: `true` jika dipasang, `false` jika dilepas.
`RemainingDefensePoints`: sisa Defense Points setelah perubahan.
Contoh publish:

```csharp
EventBus.Publish(new DefenseChangedEvent(defense, isPlaced, remainingDefensePoints));
```

Contoh subscriber:

```csharp
private void OnDefenseChanged(DefenseChangedEvent eventData)
{
    string action = eventData.IsPlaced ? "dipasang" : "dilepas";
    Debug.Log($"Defense {action}. Sisa DP: {eventData.RemainingDefensePoints}");
}
```

## Contoh penggunaan: memperbarui jumlah Defense Points dan tampilan defense yang tersedia saat fase planning.

4.12 `PurchaseEvent`
Tujuan: Memberi tahu sistem bahwa pembelian berhasil dilakukan.
Data:
`PurchasedItem`: Unity Object yang dibeli.
`Price`: harga pembelian.
Contoh publish:

```csharp
EventBus.Publish(new PurchaseEvent(purchasedItem, price));
```

Contoh subscriber:

```csharp
private void OnPurchase(PurchaseEvent eventData)
{
    Debug.Log($"Pembelian berhasil: {eventData.PurchasedItem.name}, harga {eventData.Price}");
}
```

Contoh penggunaan: menampilkan notifikasi pembelian atau memperbarui riwayat transaksi.

> Publish event ini hanya setelah transaksi berhasil, bukan saat tombol beli baru ditekan.

---

4.13 `UpgradeSuccessEvent`
Tujuan: Memberi tahu sistem bahwa upgrade berhasil dilakukan.
Data:
`UpgradedItem`: Unity Object yang di-upgrade.
`NewLevel`: level item setelah upgrade.
`UpgradeCost`: biaya upgrade.
Contoh publish:

```csharp
EventBus.Publish(new UpgradeSuccessEvent(upgradedItem, newLevel, upgradeCost));
```

Contoh subscriber:

```csharp
private void OnUpgradeSuccess(UpgradeSuccessEvent eventData)
{
    Debug.Log($"Upgrade berhasil: {eventData.UpgradedItem.name} ke level {eventData.NewLevel}");
}
```

Contoh penggunaan: menampilkan notifikasi upgrade atau memperbarui tampilan level item.

> Publish event ini hanya setelah validasi dan proses upgrade berhasil.

---

5. Contoh Integrasi: GameManager dan GameStateChangedEvent
   Saat state berubah, `GameManager` dapat menerbitkan `GameStateChangedEvent`.

```csharp
public void SetState(GameState newState)
{
    GameState previousState = CurrentState;

    if (previousState == newState)
        return;

    CurrentState = newState;

    EventBus.Publish(
        new GameStateChangedEvent(previousState, CurrentState)
    );

    Debug.Log($"Game State: {CurrentState}");
}
```

## Dengan pola ini, UI atau sistem lain dapat mendengarkan perubahan state tanpa perlu dipanggil langsung oleh `GameManager`.

6. Praktik yang Disarankan
   Subscribe di `OnEnable()` dan unsubscribe di `OnDisable()`. Ini membantu mencegah listener tetap terdaftar ketika GameObject nonaktif.
   Publish hanya ketika kejadian benar-benar terjadi. Contohnya, publish `PurchaseEvent` setelah pembelian sukses.
   Gunakan event yang sesuai. Jangan gunakan `PlayerDamagedEvent` untuk semua perubahan health; gunakan `HealthChangedEvent` untuk perubahan health umum.
   Jaga payload tetap relevan. Kirim data yang dibutuhkan subscriber, bukan seluruh sistem jika tidak diperlukan.
   Hindari subscribe berulang tanpa unsubscribe. Callback bisa terpanggil lebih dari sekali jika terdaftar berkali-kali.
   EventBus berjalan secara sinkron. Callback subscriber dijalankan ketika `Publish()` dipanggil; jangan menganggap event otomatis diproses di frame atau thread lain.
   Hati-hati dengan referensi object. Jika event membawa `Enemy` atau `UnityEngine.Object`, subscriber sebaiknya menggunakan referensi tersebut segera dan tidak mengandalkannya setelah object dihancurkan.
   Jangan memanggil `EventBus.Clear()` sembarangan. Karena EventBus static, `Clear()` menghapus seluruh subscription dan dapat membuat sistem berhenti menerima event.

---

7. Checklist Integrasi Event
   [ ] Event struct sudah dibuat dalam file `.cs` masing-masing.
   [ ] Tipe yang dirujuk event tersedia di project (`Enemy`, `EquipmentType`, `EquipmentData`, `Defense`, dan `GameState`).
   [ ] Sistem pengirim memanggil `EventBus.Publish()` pada waktu yang tepat.
   [ ] Sistem penerima melakukan `Subscribe` dan `Unsubscribe`.
   [ ] Tidak ada subscription ganda.
   [ ] Event diuji saat gameplay berlangsung.

---

8. Ringkasan
   Method Fungsi
   `EventBus.Subscribe<T>(callback)` Mendaftarkan penerima event
   `EventBus.Unsubscribe<T>(callback)` Menghapus penerima event
   `EventBus.Publish(eventData)` Mengirim event ke subscriber
   `EventBus.Clear()` Menghapus semua subscription; gunakan dengan hati-hati
   EventBus berfungsi sebagai jalur komunikasi antar-sistem. Event hanya menyampaikan bahwa sesuatu terjadi dan data terkait; logika gameplay tetap berada pada sistem yang bertanggung jawab.
