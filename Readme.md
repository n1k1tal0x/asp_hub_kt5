# Запуск проекта

1. Запустить докер postgres в docker-compose

2. Сделать миграцию
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

3. Если ef не найден
```
dotnet tool install --global dotnet-ef
```

# Ручки

Полный список
GET http://localhost:5191/api/products

Создать продукт
POST http://localhost:5191/api/products
Body:
```json
{
  "name": "Lorem",
  "description": "Lorem..."
}
```

Получить конкретный
GET http://localhost:5191/api/products/{id}
Ex: http://localhost:5191/api/products/1

Обновить продукт
PUT http://localhost:5191/api/products/{id}
Ex: http://localhost:5191/api/products/1
Body:
```json
{
  "id": 1,
  "name": "Updated name",
  "description": "Updated description"
}
```

Удалить продукт
DELETE http://localhost:5191/api/products/{id}
Ex: http://localhost:5191/api/products/1
