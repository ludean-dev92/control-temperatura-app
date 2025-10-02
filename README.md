**Control de Temperaturas - Aplicación Fullstack**

DEMO MVP para digitalizar el formato de Control de Temperaturas de Producto Destinado a Conservas.

**Tecnologías usadas:**
- Backend: ASP.NET Core 8, Entity Framework Core
- Frontend: Vue 3, Vite, Pinia
- Base de datos: SQL Server (Docker)
- Documentación de API: Swagger

**Estructura del proyecto**

controltemperaturaapp/
├── backend/        # Backend .NET 8
│   ├── src/
│   ├── Program.cs
│   └── ...
├── frontend/       # Frontend Vue 3
│   ├── src/
│   ├── package.json
│   └── ...
└── README.md

**Requisitos**

- .NET 8 SDK
- Node.js 18+ / npm
- Docker

** Configurar Base de Datos (Docker)**

1. Ejecutar SQL Server en Docker:

Descargar e instalar imagen de MSQSL 

cmd
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Ludean92!" -p 1433:1433 --name sqltemperature -d mcr.microsoft.com/mssql/server:2022-latest

2. Configurar conexión en `backend/src/Api/appsettings.json`:

"ConnectionStrings": {
  "Default": "Server=localhost,1433;Database=TemperatureControlDb;User Id=sa;Password=Ludean92!;TrustServerCertificate=True;"
}

3. Ejecutar migraciones para crear las tablas:

terminal>
cd backend
dotnet ef database update --project src/Infrastructure --startup-project src/Api

**Backend**

1. Entrar a la carpeta `backend` y restaurar dependencias:

Terminal>
cd backend
dotnet restore
dotnet run

2. Swagger estará disponible en:

https://localhost:5169/swagger

3. Endpoints principales:

- `POST /api/formatos` → Crear nuevo formato
- `GET /api/formatos` → Listar formatos
- `GET /api/formatos/reporte?fechaInicio=YYYY-MM-DD&fechaFin=YYYY-MM-DD` → Obtener reporte JSON
- `GET /api/formatos/reporte/excel?...` → Descargar reporte Excel
- `GET /api/formatos/reporte/pdf?...` → Descargar reporte PDF

Frontend

1. Entrar a la carpeta `frontend`:

Terminal>
cd frontend
npm install
npm run dev

2. Abrir navegador en:

http://localhost:5173

3. Funcionalidades:

- Crear y editar formatos
- Agregar registros de cada formato
- Visualizar reporte con filtros por fecha
- Exportar reporte a **Excel** o **PDF**

** Uso del reporte en Vue**
- Haz click en **Exportar Excel** o **Exportar PDF** para descargar el reporte.

** Notas importantes**

- Asegúrate de tener Docker corriendo antes de iniciar el backend.

Autor:
Luis Delgado
Demo de prueba técnica para entrevista.
