
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

## 2026-10-24 — başlık id'si yanlış alana atandı (CS0266, sonra CS1061)
- **Ne oldu:** `new Not { ..., Id = gelen.baslikId }` yazılınca `Cannot implicitly convert type 'int?' to 'int'`. Record'daki ad `BaslikId` yapılınca bu kez `'YeniNot' does not contain a definition for 'baslikId'`.
- **Neden oldu:** Başlık id'si notun kendi `Id`'sine (`int`, veritabanı verir) atandı; `int?` boş olabileceği için `int`'e sığmadı. İkinci hata: record'daki ad değişti ama kullanan satır güncellenmedi, C# büyük-küçük harf ayırır.
- **Doğrusu:** `new Not { Text = gelen.Metin, BaslikId = gelen.BaslikId }`. Bir adı değiştirince onu kullanan her satırı da değiştir.

## 2026-10-24 — endpoint'te `return` unutuldu (derleme hatası yok)
- **Ne oldu:** `GET /basliklar/{id}` içinde `Results.NotFound(id);` ve `Results.Ok(gelecekDeger);` `return` olmadan yazıldı; kod derlendi.
- **Neden oldu:** `Results.Ok(...)` cevabı yalnızca oluşturur; `return` edilmezse istemciye gitmez, kaybolur. Lambda hiçbir şey döndürmediği için derleyici şikâyet etmez — sinsi hata.
- **Doğrusu:** Her dalda `return Results...`. Bir dalda `return` var, diğerinde yoksa derleyici "not all code paths return a value" der.

## 2026-10-24 — not gövdesi `/basliklar`'a gönderildi (23502 not-null)
- **Ne oldu:** `curl -X POST .../basliklar -d '{"Metin": ..., "baslikId": 2}'` → `null value in column "Title" of relation "Basliklar" violates not-null constraint`.
- **Neden oldu:** Not oluşturulacaktı ama istek başlık endpoint'ine gitti. `YeniBaslik` gövdede `Title` aradı, bulamadı → `null`; veritabanında `Title` boş olamaz. Ek ders: API boş/eksik `Title`'ı kontrol etmeden veritabanına kadar götürüyor (doğrulama eksiği).
- **Doğrusu:** Not için `POST /notlar`. Endpoint'lerde gelen veriyi kaydetmeden önce doğrula (ApiOrnek eksik listesinde).

## 2026-10-24 — var olmayan başlığa not bağlanmaya çalışıldı
- **Ne oldu:** `POST /notlar` gövdesinde `"baslikId": 2` gönderildi; not oluşmadı. `GET /basliklar/2` → 404.
- **Neden oldu:** 2 numaralı başlık yok. `BaslikId` yabancı anahtar (foreign key): veritabanı, var olmayan bir başlığa işaret eden notu kabul etmez.
- **Doğrusu:** Önce başlığı oluştur (`POST /basliklar`), dönen id'yi notta kullan. Komut doğruysa bile hedef verinin var olduğunu kontrol et.

## 2026-10-24 — `Include` sonrası JSON döngüsü (object cycle)
- **Ne oldu:** `GET /basliklar/1` (Include ile) → `JsonException: A possible object cycle was detected ... Path: $.Notes.Baslik.Notes.Baslik...`
- **Neden oldu:** `Baslik.Notes` notları tutuyor, her `Not.Baslik` de aynı başlığı geri gösteriyor. JSON'a çevirici başlık → not → başlık → not... diye sonsuza iniyor, 64 derinlikte duruyor.
- **Doğrusu:** `Not.Baslik` alanının üstüne `[JsonIgnore]` (`using System.Text.Json.Serialization;`). Hangi başlık olduğu `baslikId` ile zaten görünüyor. Kalıcı çözüm: cevap için ayrı DTO sınıfı (katmanlama günü). Kod değişince `dotnet run`'ı yeniden başlat.
- **Ara hata:** `[JsonIgnore]` önce `BaslikId`'nin üstüne kondu. `BaslikId` sadece bir sayı, döngü yaratmaz; döngüyü nesne olan `Baslik` alanı yaratıyor. Üstelik sayı gizlenince notun hangi başlıkta olduğu bilgisi de JSON'dan kaybolur.

## 2026-10-24 — servis bulunamayınca `throw new Exception()` fırlattı
- **Ne oldu:** `NotServisi.Id_Getir` içinde `if (result == null) throw new Exception();`.
- **Neden sorun:** Dönüş tipi `Not?` "bulamazsam null" diyor, metot ise patlıyordu. İşlenmemiş exception istemciye 500 (sunucu çöktü) olarak gider; oysa "not yok" normal bir sonuç, 404 olmalı. Eski alışkanlık: "ters giden her şeye throw".
- **Doğrusu:** Servis `null` döndürür; 404'e endpoint karar verir.

## 2026-10-24 — servisin içinde `Results.*` (CS0029 IResult → Not)
- **Ne oldu:** `Cannot implicitly convert type 'Microsoft.AspNetCore.Http.IResult' to 'Not'`.
- **Neden oldu:** HTTP cevabı üreten `Results.NotFound/Ok` servise yazıldı; metot `Not?` döndüreceğine söz vermişti. HTTP cevabı endpoint'in (garsonun) işi, servisin değil. Düzeltirken kalan `if (...) Results.NotFound(...)` satırı da sabahki `return` hatasının aynısıydı: oluşturulan cevap kayboluyor.
- **Doğrusu:** Servis: bul + `return result;`. `Results.*` sadece endpoint'te.

## 2026-10-25 — `Results.NotFound(404)`: durum kodu gövdeye yazıldı
- **Ne oldu:** `curl .../notlar/999` ekrana `404` bastı. Önceki hali (`Results.NotFound()`) boş görününce koda `404` argümanı eklendi.
- **Neden oldu:** `NotFound(x)` içindeki `x` cevabın **gövdesi**; durum kodunu metodun adı zaten 404 yapıyor. Ekrandaki `404` HTTP durum kodu değil, gövdeye yazılmış bir sayı. Asıl kafa karışıklığı: `curl` varsayılan olarak yalnızca gövdeyi basar, durum kodunu göstermez. Boş ekran "cevap yok" değil, "404 geldi, gövdesi boş" demekti.
- **Doğrusu:** Durum kodunu görmek için `curl -i`. Gövdeye bir şey koyacaksan istemciye işe yarayan bir mesaj koy, kodun tekrarını değil.

## 2026-10-25 — `async Task List<Not>`: iç içe tip yanlış yazıldı (CS0246)
- **Ne oldu:** `public async Task List<Not> Liste_Getir()` → `The type or namespace name 'Liste_Getir' could not be found`.
- **Neden oldu:** `Task` ile `List<Not>` arasında `<>` yok. Derleyici `Task`'ı dönüş tipi, `List<Not>`'u metot adı sandı; bu yüzden `Liste_Getir`'i bir tip adı olarak okumaya çalıştı. Hata mesajı asıl yanlışın bir kelime sağını gösteriyor.
- **Doğrusu:** Fişin içinde liste var: `Task<List<Not>>`. Genel kural: `Task<içinden çıkacak tip>`.
- **İkinci sorun (aynı metot):** `await _dbNot.notlar.ToList()`: `ToList()` senkron, sonucu zaten hazır bir liste; liste `await` edilemez. `Task.FromResult(...ToList())` ile sarmak da gerçek async değil: aşçı fırının önünde bekler, sonra fiş verir. Doğrusu `ToListAsync()` (`using Microsoft.EntityFrameworkCore;`).

## 2026-10-25 — `await` var, `return` yok (CS0161) + fişi fişe sarmak
- **Ne oldu:** `await Task.FromResult(_dbNot.notlar.ToListAsync());` → `not all code paths return a value`.
- **Neden oldu:** `await` yemeği alır ama `return` yazılmadığı için kimseye vermez. Metot `Task<List<Not>>` döndüreceğine söz verdi; söz tutulmadı. Ayrıca `ToListAsync()` zaten fiş (`Task`) veriyor; onu `Task.FromResult` içine koymak fişi bir fişe sarmak oluyor, `await` sonunda liste değil yine fiş çıkar.
- **Doğrusu:** `return await _dbNot.notlar.ToListAsync();`. `Task.FromResult` gerekmez.

## 2026-10-25 — `YeniNot` public metotta kullanıldı ama kendisi public değil (CS0051)
- **Ne oldu:** `Inconsistent accessibility: parameter type 'YeniNot' is less accessible than method 'NotServisi.Not_EKle_Async(YeniNot)'`.
- **Neden oldu:** `Program.cs`'te `record YeniNot(...)` başında erişim belirteci yok; C#'ta yazılmayan tip varsayılan olarak `internal` (sadece bu proje görür). `NotServisi` ve metodu ise `public` (herkes görebilir). Herkese açık bir kapı, herkesin göremediği bir tipi istiyor: çelişki. Bu tip daha önce sadece `Program.cs` içindeki endpoint'lerde kullanıldığı için sorun çıkmamıştı; servise taşınınca ortaya çıktı. `return result;` doğruydu, hata ondan değil.
- **Doğrusu:** `public record YeniNot(...)` (`Not` gibi). Genel kural: public bir metodun parametre ve dönüş tipleri en az onun kadar açık olmalı.

## 2026-10-26 — `FindAsync` önünde `await` yok: null kontrolü hep false
- **Ne oldu:** `if (result == null)` → `The result of the expression is always 'false' since a value of type 'ValueTask<Not?>' is never equal to 'null'`.
- **Neden oldu:** `await` yazılmadığı için `result` notun kendisi değil, fişi (`ValueTask<Not?>`). Fiş her zaman var, asla null değil; içinde not olsa da olmasa da. Bu yüzden kontrol hiçbir zaman tutmaz. Hata mesajı tipi açıkça söylüyor: `Not?` beklenen yerde `ValueTask<Not?>` görünce sebep `await` eksikliği.
- **Doğrusu:** `var result = await _dbNot.notlar.FindAsync(id);`. Genel kural: adı `...Async` ile biten metodun önüne `await`. Uyarıda `Task`/`ValueTask` tipi görüyorsan ilk bakılacak yer orası.

## 2026-10-26 — TEKRAR: servisin içinde `Results.*` + `if` içinde `return` yok (24 Ekim kaydının aynısı)
- **Ne oldu:** `Not_Degistir_Async` içinde `if (result == null) Results.NotFound();` ve `return Results.();`.
- **Neden oldu:** 24 Ekim'deki iki hata birlikte geri geldi. (1) HTTP cevabı servise yazıldı; servis `Not` döndürecek, `IResult` değil. (2) `if` içinde `return` yok: `NotFound()` oluşturulup kayboluyor, kod alt satıra geçip `result.Text`'te null ile patlıyor. Az önce konuşulan "önce kontrol et, yoksa hemen dön" adımı yazıldı ama `return` olmadan.
- **Doğrusu:** Serviste `if (result == null) return null;` ... `return result;`. 200/404 kararı endpoint'te. Fark etme sorusu: "bu satır HTTP biliyor mu?" Biliyorsa servise ait değil.
