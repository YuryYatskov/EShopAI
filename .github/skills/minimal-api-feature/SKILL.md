---
name: minimal-api-feature
description: Створює каркас повноцінної функції бекенду домену, використовуючи мінімальні API та сервіс Singleton, що знаходиться в пам'яті, в рамках проекту ApiService.
version: 1.0.0
author: Enterprise Architecture Team
compatibility: Requires dotnet>=10.0
---

# Minimal API Feature Builder

## Overview
Ця навичка генерує стандартизований фрагмент домену бекенду в `EShopAI.ApiService`. Вона гарантує, що всі нові сутності дотримуються однакової структури файлів, шаблону впровадження залежностей та правил маршрутизації без використання зовнішніх шаблонів.

## Prerequisites & Inputs
* `domain_name`: Однина назви створюваної сутності (наприклад, `Product`, `Order`).
* `target_project`: Має бути` `EShopAI.ApiService`.

## Process Steps

1. **Model Generation:** 
   Створити `Models/{domain_name}.cs`. 
   *Requirement:* Клас повинен бути `public` та містити властивість `public Guid Id { get; set; }`.

2. **Service Generation:** 
   Створити `Services/{domain_name}Service.cs`.
   *Requirement:* Реалізація стандартних CRUD-операцій для приватного, що знаходиться в пам'яті, об'єкта `List<{domain_name}>`.
   *Template Structure:*
    ```csharp
       public class {domain_name}Service 
       {
           private readonly List<{domain_name}> _items = new();
           public IEnumerable<{domain_name}> GetAll() => _items;
           public {domain_name}? GetById(Guid id) => _items.FirstOrDefault(x => x.Id == id);
           public void Create({domain_name} item) => _items.Add(item);
           // Add Update and Delete methods...
       }
    ```
        
3. **Endpoint Generation:**
   Створити `Endpoints/{domain_name}Endpoints.cs`.
   *Requirement:* Створити статичний клас з методом розширення для IEndpointRouteBuilder. Використовувати MapGroup з відповідними тегами.
   *Template Structure:*
   ```csharp
   public static class {domain_name}Endpoints
   {
       public static IEndpointRouteBuilder Map{domain_name}Endpoints(this IEndpointRouteBuilder app)
       {
           var group = app.MapGroup("/api/{domain_name}s").WithTags("{domain_name}s");
           group.MapGet("/", ({domain_name}Service service) => service.GetAll());
           // Add MapGet(id), MapPost, MapPut, MapDelete...
           return app;
       }
   }
   ```
  
4. **Wiring & Integration:**
   Надайте чіткі інструкції або фрагменти коду для оновлення користувачем `Program.cs`.
   *Requirement:* Обов'язково включайте `builder.Services.AddSingleton<{domain_name}Service>();` та `app.Map{domain_name}Endpoints();`.

## Conditional Routing Rules
Якщо для домену потрібні реляційні дані: нагадайте користувачеві, що ця навичка за замовчуванням використовує списки в пам'яті для прототипування, і пізніше знадобиться міграція бази даних.

## Verification Checklist
[ ] Кінцеві точки використовують MapGroup для префіксації маршруту.
[ ] Сервіс розроблено для реєстрації як Singleton (якщо врахувати міркування потокової безпеки для прототипування).
[ ] Модель розміщена у правильному просторі Models/ namespace.
[ ] Сервіс розміщено у правильному просторі Services/ namespace.
[ ] Кінцеві точки розміщено у правильному просторі Endpoints/ namespace.