# FYM Technology – Sistema de usuarios y roles

## Descripción

Aplicación desarrollada como prueba técnica para la gestión de usuarios y roles mediante una API REST y un cliente web.

La solución permite registrar usuarios, autenticarlos mediante JWT, asignar roles y consultar la información almacenada.

## Tecnologías utilizadas

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
- HTML y CSS

## Arquitectura

La solución backend está organizada en las siguientes capas:

- **Domain:** entidades y contratos de repositorios.
- **Application:** servicios, DTOs y validaciones.
- **Infrastructure:** persistencia, repositorios e implementación de servicios.
- **API:** controladores, configuración de JWT, Swagger y manejo global de excepciones.

## Funcionalidades

- Inicio de sesión con autenticación JWT.
- Registro de usuarios exclusivo para SuperAdmin.
- Consulta de usuarios.
- Consulta de roles.
- Asignación de roles a usuarios.
- Autorización de endpoints según los roles.
- Manejo centralizado de excepciones.
- Interfaz web para consumir los servicios REST.

## Modelo entidad-relación

El modelo de datos está compuesto por las entidades Users, Roles y UserRoles, que implementan una relación muchos a muchos entre usuarios y roles.

![Modelo Entidad-Relación](Documentation/Modelo-Entidad-Relacion.png)

## Ejecución del proyecto

### Backend

1. Abrir `FymTechnology.sln` en Visual Studio.
2. Configurar la cadena de conexión a SQL Server.
3. Configurar los parámetros JWT.
4. Aplicar las migraciones de Entity Framework Core.
5. Ejecutar el proyecto `FymTechnology.Api`.
6. Acceder a Swagger desde la URL de ejecución.

### Frontend

1. Abrir una terminal en la carpeta `Frontend`.
2. Instalar dependencias:

   ```bash
   npm install
   ```

3. Ejecutar Angular:

   ```bash
   ng serve
   ```

4. Abrir `http://localhost:4200`.

## Consideraciones de seguridad

- Las contraseñas se almacenan utilizando BCrypt.
- La autenticación se realiza mediante JWT.
- Los endpoints están protegidos mediante autorización por roles.


