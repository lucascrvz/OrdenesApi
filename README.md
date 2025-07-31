# OrdenesApi
> API RESTful para gestionar órdenes de compra, productos y autenticación de usuarios con .NET 7

Una API RESTful desarrollada con .NET 7 para la gestión de órdenes de compra y productos asociados. El sistema permite operaciones CRUD completas sobre órdenes y productos, además de contar con autenticación y documentación interactiva mediante Swagger.

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
   - Crear, leer, actualizar y eliminar órdenes de compra.
   - Asociar múltiples productos a una orden.
   - Calcular automáticamente el total de la orden, incluyendo descuentos.

2. **Gestión de Productos**
   - Crear y leer productos.
   - Asociar productos a categorías.

3. **Documentación**
   - Proporcionar documentación completa de la API utilizando Swagger.

4. **Autenticación**
   - Los usuarios deben autenticarse con JWT para acceder a rutas protegidas.

### **Estructura del Proyecto**

```
OrdenesApi
├── Application
│   ├── Application.csproj
│   ├── DTOs
│   ├── Interfaces
│   └── Services
├── Domain
│   ├── Domain.csproj
│   └── Entities
├── Infrastructure
│   ├── Infrastructure.csproj
│   ├── Migrations
│   └── Repositories
├── OrdenesApi
│   ├── Controllers
│   ├── OrdenesApi.csproj
│   ├── Program.cs
│   └── Properties
│       └── launchSettings.json
├── OrdenesApiSolution.sln
├── OrdenesApi.Tests
│   ├── Controllers
│   └── OrdenesApi.Tests.csproj
└── README.md
```

### **Instrucciones de Instalación** 

Para ejecutar la API localmente, asegúrate de tener instalados los siguientes requisitos:
- Docker
- Docker Compose

1. Clonar el repositorio:
   ```bash
   git clone <URL_DEL_REPOSITORIO>
   cd OrdenesApiSolution
   ```

2. Construir y ejecutar los contenedores:
   ```bash
   docker compose up --build -d
   ```
   Esto iniciará los siguientes servicios:
    - **OrdenesApi**: La API RESTful.
    - **PostgreSQL**: Base de datos para almacenar órdenes, productos y usuarios.

3. Luego de que los contenedores estén en ejecución, puedes acceder a la API en tu navegador o mediante herramientas como Postman a través de la siguiente URL:
   ```
   http://localhost:5000
   ```

4. Para acceder a la documentación de la API generada por Swagger, abre tu navegador y navega a:
   ```
   http://localhost:5000/swagger
   ```
### **Pruebas**

El proyecto incluye pruebas unitarias con **xUnit** localizadas en el proyecto `OrdenesApi.Tests`.

Para ejecutarlas localmente:
```bash
dotnet test OrdenesApi.Tests
```
Además, se proporciona una colección de Postman en el directorio `tests/PostmanCollection`, que puede importarse para realizar pruebas manuales.

### **CI/CD**
El proyecto cuenta con un pipeline de CI/CD configurado en GitHub Actions que ejecuta pruebas unitarias automáticamente en cada push al repositorio. Esto garantiza que los cambios no rompan la funcionalidad existente.
