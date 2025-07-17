# Event Resource Reservation App Backend
 > Backend de aplicacion de reserva de recursos para eventos

---
Esta aplicación permite a los usuarios reservar mobiliario, equipo de iluminación y otros recursos necesarios para realizar toda clase de eventos de forma rápida y eficiente, optimizando la gestión de disponibilidad para los administradores. Está dirigida a clientes que necesiten un proveedor integral para sus eventos y permite a los organizadores optimizar recursos y mejorar la experiencia del cliente final. 



## Características Principales
* API RESTful: Proporciona endpoints para la gestión de entidades clave (recursos, reservas, usuarios, etc.).
* Autenticación y Autorización: Seguridad robusta implementada con JWT y ASP.NET Core Identity.
* Gestión de Datos: Persistencia de datos a través de PostgreSQL y Entity Framework Core.
* Documentación Interactiva: Integración con Swagger/OpenAPI para fácil exploración y prueba de endpoints.



 ## Tecnologías Clave
* .NET 8: Framework de desarrollo principal.
* PostgreSQL: Base de datos relacional.
* Entity Framework Core: ORM para interacción con la base de datos.
* JWT & ASP.NET Core Identity: Para seguridad de la API.
* Docker & Docker Compose: Para un entorno de desarrollo de base de datos rápido.
* Swagger/OpenAPI: Para documentación de la API.



## 🚀 Inicio Rápido (Setup Local)
Sigue estos pasos para poner en marcha el backend en tu máquina local.

Requisitos Previos
* .NET SDK (versión 8.0 o superior)
* Docker Desktop (para la base de datos local)
* Un editor de código o IDE (Visual Studio 2022 / VS Code recomendado)

### Pasos
1. **Clona este repositorio:**

   ```bash
    git clone https://github.com/jose-angell/EventResourceReservationAppBackend.git
    cd EventResourceReservationAppBackend
    ```
    
3. **Inicia la Base de Datos con Docker Compose:**
  Asegúrate de que Docker Desktop esté en ejecución. Desde la raíz de este proyecto:

    ```bash
    docker-compose up -d
    ```
    (Esto levantará un contenedor PostgreSQL. Espera unos segundos para que esté listo.)
  
3. **Aplica las Migraciones de la Base de Datos:**
    Navega a la carpeta principal del proyecto de API (donde está el archivo `.csproj`):

    ```bash
    cd src/EventResourceReservationAppBackend.Api # Ajusta la ruta si es diferente
    ```
    Luego, aplica las migraciones de Entity Framework Core:
    
    ```bash
    dotnet ef database update
    ```
    
4. **Ejecuta la Aplicación API:**
    Desde la misma carpeta (`src/EventResourceReservationAppBackend.Api`):

    ```bash
    dotnet run
    ```
    La API se iniciará, generalmente en `http://localhost:XXXX` (el puerto exacto se mostrará en tu terminal).
  
5. **Accede a la Documentación de Swagger:**
    Una vez que la API esté corriendo, abre tu navegador y visita:
    `http://localhost:XXXX/swagger` (reemplaza XXXX con el puerto de tu API).



## 📖 Documentación Completa
Para una guía de configuración más detallada, arquitectura, decisiones de diseño (ADRs), y más información técnica, visita la documentación centralizada del proyecto:

## 🔗 [Documentación Completa del Backend](https://jose-angell.github.io/EventReservationAppDocs/backend/setup/)
(Este enlace te llevará directamente a la sección de setup del backend en la documentación de Docusaurus.)


## 🤝 Contribución
Las contribuciones son bienvenidas. Por favor, consulta el repositorio de documentación centralizada para obtener las Guías de Contribución completas.


## 🔗 Otros Componentes del Proyecto
Este backend es parte de una solución más grande. Explora los otros repositorios:
* Frontend (React)
* Documentación Centralizada (Docusaurus)


## 📧 Contacto
Para cualquier pregunta o sugerencia, no dudes en contactarme:
* **José Ángel Gallardo Cordova**
* **Email:** gallardocordovajoseangel@gmail.com
* **LinkedIn:** [Jose Angel](www.linkedin.com/in/jose-angel-gallardo-cordova-05a347365)
* **GitHub:** [jose-angell](https://github.com/jose-angell)
