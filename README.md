# FYM Technology – Sistema de usuarios y roles

## 1. Descripción

Aplicación desarrollada como prueba técnica para la gestión de usuarios y roles mediante una API REST y un cliente web.

La solución permite registrar usuarios, autenticarlos mediante JWT, asignar roles y consultar la información almacenada en una base de datos SQL Server.

## 2. Tecnologías utilizadas

### Backend
- C#
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT (JSON Web Tokens)
- BCrypt para el hash de contraseñas
- FluentValidation
- Swagger / OpenAPI

### Frontend
- Angular 21
- TypeScript
- PrimeNG
- HTML5
- CSS3

## 3. Arquitectura

La solución backend está organizada mediante una arquitectura por capas:

- **Domain:** entidades y contratos de repositorios.
- **Application:** servicios, DTOs y validaciones.
- **Infrastructure:** persistencia, repositorios e implementación de servicios.
- **API:** controladores, configuración de JWT, Swagger y manejo global de excepciones.

Esta separación permite mantener responsabilidades independientes y facilita el mantenimiento y la evolución de la aplicación.

## 4. Funcionalidades

- Inicio de sesión con autenticación JWT.
- Registro de usuarios exclusivo para el rol SuperAdmin.
- Consulta de usuarios registrados.
- Consulta de roles disponibles.
- Asignación de roles a usuarios.
- Autorización de endpoints según los roles asignados.
- Manejo centralizado de excepciones.
- Interfaz web para consumir los servicios REST.

## 5. Modelo entidad-relación

El modelo de datos está compuesto por las entidades Users, Roles y UserRoles, que implementan una relación muchos a muchos entre usuarios y roles.

![Modelo Entidad-Relación](Documentation/Modelo-Entidad-Relacion.png)

### Descripción de las entidades

- **Users:** almacena la información de los usuarios del sistema.
- **Roles:** almacena los roles disponibles.
- **UserRoles:** tabla intermedia que relaciona usuarios con roles y registra la fecha de asignación.

## 6. Configuración de la conexión a SQL Server

La cadena de conexión se configura mediante la sección `ConnectionStrings` del archivo `appsettings.json`, ubicado en el proyecto `FymTechnology.Api`.

Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SERVIDOR;Database=FymTechnologyDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"
  }
}
```

Reemplazar `SERVIDOR` por la instancia local de SQL Server correspondiente.

**Consideraciones de seguridad:**
- `Encrypt=True` habilita el cifrado de la conexión.
- `TrustServerCertificate=True` se utiliza para entornos locales de desarrollo.
- En producción se recomienda utilizar un certificado SSL válido y configurar `TrustServerCertificate=False`.
- No publicar credenciales ni información sensible en el repositorio.

## 7. Configuración de JWT

La autenticación utiliza JSON Web Tokens (JWT).

Los parámetros de configuración incluyen:

- Issuer: `FymTechnology`
- Audience: `FymTechnologyClient`
- Expiración del token: 60 minutos.

La clave de firma debe configurarse de forma segura mediante User Secrets o variables de entorno, evitando publicarla en el repositorio.

## 8. Ejecución del proyecto
### Configuración de la URL del API en Angular

La URL base de los servicios REST se centraliza en el archivo:

`Frontend/src/environments/environment.ts`

Configuración para desarrollo:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7022/api'
};
```

Si el backend se ejecuta en una dirección o puerto diferente, se debe actualizar la propiedad `apiUrl`.

Los componentes Angular utilizan esta configuración para consumir los servicios REST, evitando tener direcciones URL duplicadas en el código.

Para producción se puede definir una configuración independiente mediante `environment.prod.ts` y el sistema de configuración de compilación de Angular.
### Backend

1. Abrir `FymTechnology.sln` en Visual Studio.
2. Configurar la cadena de conexión a SQL Server.
3. Configurar los parámetros JWT.
4. Aplicar las migraciones de Entity Framework