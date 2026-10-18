
## 2026-10-13 — plan dosyasında uydurma isim
- **Ne oldu:** `~/0imqardin/planlar/yerel-belge-asistani.md` içine "Mehmet/kullanıcı yazar" yazıldı.
- **Neden oldu:** "kullanıcı" yerine kişi adı yazma refleksi; isim hiçbir kaynakta yok.
- **Nasıl fark edildi:** dosyayı yazdıktan hemen sonra kendi çıktımı gözden geçirirken.
- **Düzeltme:** "sen yazarsın" olarak değiştirildi.

## 2026-10-13 — "repo GitHub'da değil" yanlış çıkarımı
- **Ne oldu:** Kullanıcıya ApiOrnek'in GitHub'da görünmediği söylendi.
- **Neden oldu:** Yalnızca yerelde `git remote -v` boş döndü; çalışma günlüğüne bakmadan "uzak repo yok" sonucuna varıldı. Uzak repo (`QarDin/API_Deneme`) günlükte push'lı görünüyor.
- **Doğrusu:** Yerelde remote tanımlı değil; uzak repo var. Plan "boş repo aç" yerine "remote'u bağla ve doğrula" olmalı.
