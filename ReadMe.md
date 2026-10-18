# ApiOrnek

- Api yazmayi ogrendigim bir proje. Get, Post, Put, Delete islemlerini yazdim.
- C#, ASP.NET minimal API, EF Core, PostgreSQL, Docker, xUnit

## Endpointler

- `GET /notlar`, `GET /notlar/{id}`: Db den istek ile datalara erisiyor.
- `POST /notlar`: Db ye yeni ekleyecegimiz seyler icin calisiyor.
- `PUT /notlar/{id}`: Db ye ekledigimiz degeri degistirmeye olanak veriyor.
- `DELETE /notlar/{id}`: Db ye ekledigimiz datalari silmeye olanak veriyor.

## Nasil calistirilir

Programi cektikten sonra kendi .env dosyanizi yazmaniz lazim, docker db sifresini oradan aliyor.

```
cd ApiOrnek
touch .env
nano .env
```

`.env` dosyasinin icine sunu yazin:

```
POSTGRES_PASSWORD=YOUR_PASSWORD
```

Uygulamanin veritabanina baglanmasi icin baglanti dizesini kaydedin (Password, `.env` deki ile ayni olmali):

```
dotnet user-secrets set "ConnectionStrings:Db" "Host=localhost;Port=YOUR_PORT;Username=YOUR_USER_NAME;Password=YOUR_PASSWORD;Database=YOUR_DATA_BASE"
```

Port, kullanici adi ve veritabani adi compose.yaml'daki degerlerle ayni olmali. Varsayilan: 5454, QarDin, DenemeDb

Sonrasinda veritabanini ve uygulamayi baslatin:

```
docker compose up -d
dotnet run
```

Burada localhost'un ayaga kalktigini gormen gerekiyor.

## Testler

```
dotnet test ApiOrnek.Tests
```

ile terminalde testlerin verdigi sonuclari goruyoruz.
