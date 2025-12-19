# Healthcare

Sistem manajemen booking jadwal dokter dengan arsitektur bersih yang menerapkan prinsip SOLID untuk maintainability dan scalability yang optimal.

Untuk penerapan SOLID sendiri, antara lain:<br>
1. Single Responsibility Principle (SRP), dimana penerapannya dengan service dan repository yang masing masing bertanggung jawab sesuai dengan fungsi nya masing masing
2. Open/Closed Principle (OCP)
3. Liskov Substitution Principle (LSP), dimana menerapkan turunan dari parent model.
4. Interface Segregation Principle (ISP) menerapkan implementasi dari interface.
5. Dependency Inversion Principle (DIP) diterapkan pada controller, service dan repository.

---
## Techstack
* .NET 9
* SQL Server 2019+
* Entity Framework Core
---

## Fitur
✅ Inquiry list dokter

✅ Detail dokter

✅ Detail jadwal dokter

✅ Cek jadwal dokter yang bisa di book

✅ Inquiry list pasien

✅ Inquiry list appointment

✅ Buat data appointment baru

✅ Hapus data appointment dengan ketentuan yang dapat dihapus adalah maksimal 2 jam sebelum appointment.

