# Taller 2 - API REST CRUD de Productos (Entity Framework Core & MySQL)

## Integrantes
* Michael Steven Parra Mantilla
* Juan Diego Guzman Leal
* Carolina Merchan Acuña



---

## Descripción del Proyecto
Desarrollo de una API REST implementando las operaciones CRUD (Crear, Consultar, Actualizar y Eliminar) para la entidad `Producto`, utilizando Entity Framework Core, MySQL como gestor de base de datos, patrón Repositorio y arquitectura modular basada en responsabilidades.

## Arquitectura del Proyecto
* **Controllers/**: `ProductoController.cs` (Endpoints HTTP REST)
* **DB/**: `AppDbContext.cs` (Contexto de base de datos - Namespace: `Taller2.Db`)
* **Interfaces/**: `IProductoRepository.cs` (Contrato de operaciones de datos)
* **Models/**: `Producto.cs` (Entidad independiente)
* **Repository/**: `ProductoRepository.cs` (Implementación de acceso a datos con EF Core)

## Endpoints Principales
* `GET /api/producto` - Lista todos los productos.
* `GET /api/producto/{id}` - Obtiene un producto por su ID.
* `POST /api/producto` - Crea un nuevo producto (con validaciones de campos vacíos y valores negativos).
* `PUT /api/producto/{id}` - Actualiza un producto existente.
* `DELETE /api/producto/{id}` - Elimina un producto por su ID.


