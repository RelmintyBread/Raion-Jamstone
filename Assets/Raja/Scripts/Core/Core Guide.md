# Core Game System – Detergentnation

Panduan penggunaan tiga script utama untuk mengatur alur permainan **Detergentnation** menggunakan Unity.

## Daftar Script

| Script            | Fungsi                                                                     |
| ----------------- | -------------------------------------------------------------------------- |
| `GameState.cs`    | Menyimpan daftar state atau kondisi permainan.                             |
| `GameManager.cs`  | Mengatur state global, pause, resume, victory, dan game over.              |
| `LevelManager.cs` | Mengatur level aktif, memuat scene, serta memulai dan menyelesaikan level. |

---

## 1. GameState.cs

### Fungsi

`GameState` merupakan enum yang mendefinisikan kondisi permainan.

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

### Daftar State

| State             | Keterangan                                         |
| ----------------- | -------------------------------------------------- |
| `MainMenu`        | Pemain berada di menu utama.                       |
| `Shop`            | Pemain berada di toko.                             |
| `DefensePlanning` | Pemain sedang mengatur pertahanan sebelum bermain. |
| `Playing`         | Gameplay sedang berlangsung.                       |
| `WaveBreak`       | Jeda antar-wave.                                   |
| `Paused`          | Permainan sedang dijeda.                           |
| `Victory`         | Pemain berhasil menyelesaikan level.               |
| `GameOver`        | Pemain mengalami kekalahan.                        |

### Contoh Penggunaan

```csharp
GameState currentState = GameState.Playing;
```

Untuk memeriksa state:

```csharp
if (GameManager.Instance.CurrentState == GameState.Playing)
{
    Debug.Log("Gameplay sedang berjalan");
}
```

---

## 2. GameManager.cs

### Fungsi

`GameManager` mengatur kondisi global permainan. Script ini menggunakan pola **Singleton**, sehingga dapat diakses melalui `GameManager.Instance`.

### Fitur

- Menyimpan state permainan saat ini.
- Memulai permainan.
- Menjeda dan melanjutkan permainan.
- Mengatur kondisi Victory dan Game Over.
- Tetap tersedia ketika scene berganti.

### Cara Memasang

1. Buat GameObject bernama `GameManager`.
2. Tambahkan komponen `GameManager.cs`.
3. Letakkan GameObject tersebut pada scene awal, misalnya Main Menu.
4. Pastikan hanya ada satu instance `GameManager` yang aktif.

### Daftar Method

| Method                | Fungsi                                                 |
| --------------------- | ------------------------------------------------------ |
| `StartGame()`         | Mengubah state menjadi `DefensePlanning`.              |
| `PauseGame()`         | Menjeda waktu gameplay dan menyimpan state sebelumnya. |
| `ResumeGame()`        | Melanjutkan permainan ke state sebelumnya.             |
| `GameOver()`          | Mengubah state menjadi `GameOver`.                     |
| `Victory()`           | Mengubah state menjadi `Victory`.                      |
| `SetState(GameState)` | Mengubah state permainan secara langsung.              |

### Contoh Penggunaan

**Memulai permainan:**

```csharp
GameManager.Instance.StartGame();
```

**Pause:**

```csharp
GameManager.Instance.PauseGame();
```

**Resume:**

```csharp
GameManager.Instance.ResumeGame();
```

**Game Over:**

```csharp
GameManager.Instance.GameOver();
```

**Victory:**

```csharp
GameManager.Instance.Victory();
```

**Mengubah state secara langsung:**

```csharp
GameManager.Instance.SetState(GameState.WaveBreak);
```

### Catatan

`PauseGame()` hanya dapat digunakan ketika state saat ini adalah `Playing` atau `WaveBreak`.

`SetState()` hanya mengubah state dan menampilkan log. Method ini belum otomatis mengatur perilaku gameplay, UI, atau WaveManager.

---

## 3. LevelManager.cs

### Fungsi

`LevelManager` mengatur level yang sedang dimainkan dan menangani pemuatan scene.

Script ini juga menggunakan pola **Singleton**, sehingga dapat diakses melalui `LevelManager.Instance`.

### Cara Memasang

1. Buat GameObject bernama `LevelManager`.
2. Tambahkan komponen `LevelManager.cs`.
3. Letakkan pada scene awal yang sama dengan `GameManager`.
4. Pastikan hanya ada satu instance `LevelManager` yang aktif.

### Daftar Method

| Method                   | Fungsi                                                   |
| ------------------------ | -------------------------------------------------------- |
| `LoadLevel(string, int)` | Memuat scene berdasarkan nama dan menyimpan nomor level. |
| `StartLevel()`           | Mengubah state permainan menjadi `Playing`.              |
| `CompleteLevel()`        | Mengubah state menjadi `Victory`.                        |
| `FailLevel()`            | Mengubah state menjadi `GameOver`.                       |

### Contoh Penggunaan

**Memuat Level 1:**

```csharp
LevelManager.Instance.LoadLevel("Level1", 1);
```

Parameter:

- `"Level1"` adalah nama scene yang akan dimuat.
- `1` adalah nomor level yang disimpan.

**Memulai gameplay:**

```csharp
LevelManager.Instance.StartLevel();
```

**Menyelesaikan level:**

```csharp
LevelManager.Instance.CompleteLevel();
```

**Mengalami kekalahan:**

```csharp
LevelManager.Instance.FailLevel();
```

### Persiapan Scene

Pastikan scene yang ingin dimuat telah ditambahkan ke daftar scene build:

**Unity → File → Build Profiles → Scene List**

Pada versi Unity yang lebih lama, pengaturan ini dapat ditemukan melalui **File → Build Settings**.

Nama scene pada `LoadLevel()` harus sesuai dengan nama scene yang terdaftar.

---

## 4. Alur Penggunaan

### A. Memulai Permainan

Contoh pemanggilan dari tombol Start pada Main Menu:

```csharp
public void OnStartButtonClicked()
{
    GameManager.Instance.StartGame();

    LevelManager.Instance.LoadLevel(
        "Level1",
        1
    );
}
```

Alur:

1. `StartGame()` mengubah state menjadi `DefensePlanning`.
2. `LoadLevel()` memuat scene Level 1.
3. Setelah pemain selesai mengatur pertahanan, panggil `StartLevel()`.
4. State berubah menjadi `Playing`.

### B. Memulai Gameplay Setelah Defense Planning

Contoh pemanggilan dari tombol mulai pada fase Defense Planning:

```csharp
public void OnStartBattleClicked()
{
    LevelManager.Instance.StartLevel();
}
```

### C. Pause dan Resume

Contoh pemanggilan dari tombol UI:

```csharp
public void OnPauseButtonClicked()
{
    GameManager.Instance.PauseGame();
}

public void OnResumeButtonClicked()
{
    GameManager.Instance.ResumeGame();
}
```

### D. Menyelesaikan Level

Ketika seluruh wave berhasil diselesaikan:

```csharp
LevelManager.Instance.CompleteLevel();
```

Ketika kondisi kekalahan terpenuhi:

```csharp
LevelManager.Instance.FailLevel();
```

Kondisi pemanggilan tersebut nantinya dapat dihubungkan dengan `WaveManager`, `HealthSystem`, atau sistem gameplay lainnya.

---

## 5. Struktur Folder yang Disarankan

```text
Assets/
└── _Project/
    └── Scripts/
        └── Core/
            ├── GameState.cs
            ├── GameManager.cs
            └── LevelManager.cs
```

---

## 6. Hal yang Perlu Diperhatikan

- `GameManager` dan `LevelManager` menggunakan Singleton. Jangan membuat banyak instance aktif dari masing-masing manager.
- Keduanya menggunakan `DontDestroyOnLoad()`, sehingga tetap ada ketika scene berganti.
- Pastikan `GameManager` sudah tersedia sebelum memanggil method `LevelManager` yang mengaksesnya.
- `Time.timeScale = 0` menghentikan waktu gameplay. Untuk UI pause, gunakan pengaturan yang tidak bergantung pada waktu permainan.
- `CompleteLevel()` dan `FailLevel()` saat ini hanya mengubah state. Sistem reward, hasil level, dan perpindahan scene belum diimplementasikan.
- `StartLevel()` belum otomatis memulai wave. Hubungkan method tersebut dengan `WaveManager` ketika sistem wave sudah dibuat.

---

## 7. Ringkasan Alur Sistem

```text
Main Menu
    |
    v
GameManager.StartGame()
    |
    v
Defense Planning
    |
    v
LevelManager.LoadLevel()
    |
    v
Level Scene
    |
    v
LevelManager.StartLevel()
    |
    v
Playing
    |
    +----> CompleteLevel() ----> Victory
    |
    +----> FailLevel() --------> GameOver
```

**Kesimpulan:** `GameState` mendefinisikan kondisi permainan, `GameManager` mengatur state global, sedangkan `LevelManager` mengatur pemuatan dan siklus level. Ketiganya menjadi fondasi untuk menghubungkan sistem wave, pertahanan, UI, dan gameplay Detergentnation.
