---
name: blazor-crud-component
description: Генерує стандартизовані компоненти інтерфейсу Blazor WebAssembly та типізовані HTTP-клієнти для конкретної доменної моделі.
version: 1.0.0
author: Frontend Architecture Team
compatibility: Requires blazor-wasm>=10.0
---

# Blazor Enterprise CRUD Scaffolder

## Overview
Цей компонент реалізує рівень представлення (frontend) для сутності доменної моделі в проєкті `EShopCopilot.Web`. Він містить логіку взаємодії через HTTP, обробки станів завантаження та базового відображення даних у вигляді HTML-таблиці.

## Prerequisites & Inputs
* `domain_name`: Сутність, для якої ми розробляємо інтерфейс (наприклад, `Product`, `ShoppingCart`).
* `api_route`: Базовий маршрут для бекенд-API (наприклад, `/api/products`).
* `target_project`: Має бути `EShopCopilot.Web`.

## Process Steps

1. **API Client Generation:** 
   Створити `Clients/{domain_name}ApiClient.cs`.
   *Requirement:* Впровадьте `HttpClient` через конструктор. Реалізуйте асинхронні методи (`GetFromJsonAsync`, `PostAsJsonAsync` тощо), що відповідають маршруту `{api_route}`.
   *Template Structure:*
    ```csharp
       public class {domain_name}ApiClient(HttpClient httpClient)
       {
           public async Task<{domain_name}[]> GetAllAsync() 
               => await httpClient.GetFromJsonAsync<{domain_name}[]>("{api_route}") ?? [];
           // Add GetById, Create, Update, Delete...
       }
    ```    

2. **UI Component Generation:**
   Створити `Components/Pages/{domain_name}List.razor`.
   *Requirement:* Обробляйте стани null під час отримання даних через API.
   *Template Structure:*
   ```html
   @page "/{domain_name}s"
   @inject {domain_name}ApiClient ApiClient

   <h3>{domain_name} Catalog</h3>

   @if (items == null)
   {
       <p><em>Loading...</em></p>
   }
   else
   {
       <table class="table">
           <!-- Generate standard table headers and rows dynamically based on domain properties -->
       </table>
   }

   @code {
       private {domain_name}[]? items;

       protected override async Task OnInitializedAsync()
       {
           items = await ApiClient.GetAllAsync();
       }
   }
   ```
3. **Wiring & Integration:**
   Створити кодовий фрагмент для оновлення файлу `EShopCopilot.Web` Program.cs.
   *Requirement:* Мусить містити `builder.Services.AddHttpClient<{domain_name}ApiClient>(...)` з вказаним базовим адресом ApiService.

## Conditional Routing Rules
Якщо доменна модель містить вкладені об'єкти: Відображати їх як спрощені рядки в HTML-таблиці або залишити коментар `TODO` для розробника, щоб реалізувати власний форматувальник.

## Verification Checklist
[ ] `.razor` компонент містить маршрут `@page`.
[ ] UI безпечно обробляє асинхронний стан завантаження.
[ ] ApiClient використовує ін'єкцію конструктора для `HttpClient`.
[ ] Компонент коректно відображає дані у вигляді HTML-таблиці.