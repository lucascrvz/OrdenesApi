# OrdenesApi

Una API RESTful desarrollada con .NET 7 para la gestión de órdenes de compra y productos asociados. El sistema permite operaciones CRUD completas sobre órdenes y productos, además de contar con autenticación basada en roles y documentación interactiva mediante Swagger.

### **Objetivo**
Proporcionar una solución robusta para administrar órdenes de compra, permitiendo:

- Registrar nuevas órdenes y productos asociados.

- Consultar y modificar órdenes existentes.

- Autenticar usuarios y restringir acciones según su rol.

- Documentar claramente los endpoints disponibles mediante Swagger.

### **Tecnologías Utilizadas**

- .NET 7
- C#
- Entity Framework Core
- PostgreSQL
- Swagger para documentación de la API
- Docker para contenedorización

### **Requisitos Funcionales**

1. **Gestión de Órdenes**
   - Crear, consultar, actualizar y eliminar órdenes.

2. **Gestión de Productos**
   - Agregar un producto a una orden.
   - Consultar los productos de una orden.
   - Actualizar un producto en una orden.
   - Eliminar un producto de una orden.

3. **Documentación**
   - Proporcionar documentación completa de la API utilizando Swagger.

### **Estructura del Proyecto**

```
OrdenesApi/
├── Controllers/
│   ├── OrdersController.cs
│   └── ProductsController.cs
├── Models/
│   ├── Order.cs
│   ├── Product.cs
│   └── User.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── Services/
│   ├── OrderService.cs
│   └── ProductService.cs
├── Repositories/
│   ├── IOrderRepository.cs
│   ├── IProductRepository.cs
│   ├── OrderRepository.cs
│   └── ProductRepository.cs
├── DTOs/
│   ├── OrderDto.cs
│   ├── ProductDto.cs
│   └── UserDto.cs
├── Configurations/
│   ├── SwaggerConfig.cs
│   └── AuthConfig.cs
├── Program.cs
└── Startup.cs
```
<!-- Intrucciones de instalacion utilizando docker compose -->
### **Instrucciones de Instalación** 

1. Clonar el repositorio:
   ```bash
   git clone <URL_DEL_REPOSITORIO>
   cd OrdenesApiSolution
   ```

2. Construir y ejecutar los contenedores:
   ```bash
   docker-compose up --build
   ```
   Esto iniciara los siguientes servicios:
    - **OrdenesApi**: La API RESTful.
    - **PostgreSQL**: Base de datos para almacenar órdenes y productos.

3. Acceder a la aplicación:
   ```
   http://localhost:5000
   ```

4. Acceder a la documentación de la API:
   ```
   http://localhost:5000/swagger
   ```
### **Pruebas**
Para probar la API puedes usar herramientas como Postman, que permiten interactuar fácilmente con los endpoints y validar el correcto funcionamiento de las operaciones CRUD.

- La colección de Postman para pruebas se encuentra en el directorio `tests/PostmanCollection` del repositorio.

- Se recomienda importar esta colección en Postman para facilitar la ejecución y organización de las pruebas.

Además, el proyecto cuenta con un pipeline de CI/CD configurado en GitHub Actions que ejecuta pruebas unitarias automáticamente en cada push al repositorio. Esto garantiza que los cambios no rompan la funcionalidad existente.