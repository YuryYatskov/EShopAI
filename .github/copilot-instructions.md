# Copilot instructions

## Overview
Розподілений інтернет-магазин на базі .NET 10 / Aspire 13 (README — українською мовою). Файл рішення — `src\EShopAI.sln`; проєкти тестів і конфігурація лінтера відсутні.

## Build & run
- Збирання: `dotnet build src\EShopAI.sln` (або `dotnet build` у папці окремого проєкту).
- Запуск усього: `aspire run` (використовує `src\aspire.config.json`, що вказує на AppHost) або `dotnet run --project src\EShopAI.AppHost`.
- Запуск лише API: `dotnet run --project src\EShopAI.ApiService`. Перевірте кінцеві точки через `src\EShopAI.ApiService\EShopAI.ApiService.http` (змінна хоста `@ApiService_HostAddress`).

## Architecture
- **EShopAI.AppHost** – Оркестратор Aspire (`AppHost.cs`). Реєструє `apiservice` та `webfrontend`; для фронтенду налаштовано `.WithReference(apiService)` і `.WaitFor(apiService)`, тому він має звертатися до API за іменем служби `apiservice`, а не за жорстко прописаною URL-адресою.
- **EShopAI.ApiService** – Бекенд на основі Minimal API. Дані зберігаються в пам'яті: `ProductService` зареєстровано як **Singleton**; він містить список `List<Product>`, захищений механізмом блокування, і попередньо заповнений п'ятьма товарами.
- **EShopAI.Web** – Фронтенд на Blazor Server (режим інтерактивного серверного рендерингу, кешування виводу).
- **EShopAI.ServiceDefaults** – Спільний проєкт, на який посилаються обидва застосунки; кожен із них викликає `builder.AddServiceDefaults()` та `app.MapDefaultEndpoints()` (налаштування перевірок працездатності, телеметрії, виявлення служб та механізмів забезпечення стійкості).

## Conventions
- Структура функціональних можливостей (feature layout) в `ApiService`: `Models\` (класи доменної моделі), `Services\` (бізнес-логіка), `Endpoints\` (один статичний клас на функціонал, що надає метод `Map<Feature>Endpoints(this IEndpointRouteBuilder app)`). Простори імен відповідають структурі папок (`EShopAI.ApiService.Endpoints` тощо).
- Нові кінцеві точки (endpoints): використовуйте `app.MapGroup("/api/<feature>")`, типи повернення `TypedResults` і `Results<...>`, а також реєструйте сервіс у контейнері залежностей (DI) та викликайте `Map<Feature>Endpoints()` у файлі `Program.cs` перед `MapDefaultEndpoints()`.
- Захищайте спільний змінюваний стан у сервісах-синглтонах за допомогою `Lock` (`lock (_lock)`) і використовуйте вирази для ініціалізації колекцій (collection expressions) C#.
- При додаванні кінцевої точки додавайте відповідні приклади запитів до файлу `EShopAI.ApiService.http`.
- Запланована доменна область (згідно з README): товари (Products), кошик (Shopping Cart), замовлення (Orders) — кожен із повною підтримкою CRUD-операцій.
- `prompts.txt` — це журнал запитів (промптів), використаних для створення проєкту; цей файл не є частиною програмного коду.
