
## 2026-10-13 — plan dosyasında uydurma isim
- **Ne oldu:** `~/0imqardin/planlar/yerel-belge-asistani.md` içine "Mehmet/kullanıcı yazar" yazıldı.
- **Neden oldu:** "kullanıcı" yerine kişi adı yazma refleksi; isim hiçbir kaynakta yok.
- **Nasıl fark edildi:** dosyayı yazdıktan hemen sonra kendi çıktımı gözden geçirirken.
- **Düzeltme:** "sen yazarsın" olarak değiştirildi.

## 2026-10-13 — "repo GitHub'da değil" yanlış çıkarımı
- **Ne oldu:** Kullanıcıya ApiOrnek'in GitHub'da görünmediği söylendi.
- **Neden oldu:** Yalnızca yerelde `git remote -v` boş döndü; çalışma günlüğüne bakmadan "uzak repo yok" sonucuna varıldı. Uzak repo (`QarDin/API_Deneme`) günlükte push'lı görünüyor.
- **Doğrusu:** Yerelde remote tanımlı değil; uzak repo var. Plan "boş repo aç" yerine "remote'u bağla ve doğrula" olmalı.

## 2026-10-22 — özellik adı sınıf adıyla aynı (CS0542)
- **Ne oldu:** `class Baslik` içine `public string Baslik { get; set; }` yazılınca derleyici hata verdi.
- **Neden oldu:** C#'ta bir sınıfın içindeki üye, sınıfla aynı adı taşıyamaz (aynı ad yapıcı metoda / türün kendisine ayrılmış). Küçük harfle `baslik` yazınca ad farklı sayıldığı için hata gitti.
- **Doğrusu:** Özelliğe farklı ve PascalCase bir ad ver: `Ad` (ya da `Isim`). Kullanırken `baslik.Ad` diye okunur.

## 2026-10-22 — `git commit` editör açmadı (vi yok)
- **Ne oldu:** Claude çok satırlı mesaj için `git commit` önerdi; `error: cannot run vi: No such file or directory`.
- **Neden oldu:** Git'in varsayılan editörü `vi`, sistemde kurulu değil ve `core.editor` ayarlı değil. Öneri sistemde hangi editör olduğuna bakılmadan verildi.
- **Doğrusu:** Birden çok `-m` kullan (her `-m` bir paragraf) ya da `git config --global core.editor <editör>` ayarla.

## 2026-10-22 — "Gelen.title yanlış" demek (record'a bakmadan)
- **Ne oldu:** Claude, `Gelen.title`'ın `Gelen.Title` olması gerektiğini söyledi; kullanıcı "Title yok, küçük title var" dedi ve haklıydı.
- **Neden oldu:** Record'u kendim `YeniBaslik(string Title)` diye önermiştim; kullanıcının dosyada `string title` yazdığını kontrol etmeden kendi önerimi doğru kabul ettim.
- **Doğrusu:** Satırı düzeltmeden önce dosyadaki tanıma bak. Gelenek PascalCase olduğu için record `YeniBaslik(string Title)` yapılır, kullanım `Gelen.Title` olur.
