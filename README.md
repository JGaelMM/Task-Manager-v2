# Kanban Task Manager

Aplicacion web de gestion de tareas estilo Kanban con tablero de 4 columnas, drag and drop, y CRUD completo.

Construida con .NET 8 en el backend, Vue 3 + Tailwind CSS en el frontend, y SQLite como base de datos. Todo orquestado con Docker.

---

## Requisitos previos

Antes de comenzar, instala las siguientes herramientas. No necesitas instalar .NET ni Node.js de forma local, Docker lo maneja todo.

| Herramienta | Descarga | Para que sirve |
|---|---|---|
| Docker Desktop | https://www.docker.com/products/docker-desktop | Ejecutar los contenedores |
| Visual Studio Code | https://code.visualstudio.com | Editar el codigo |
| Git (opcional) | https://git-scm.com | Clonar el repositorio |

Una vez instalado Docker Desktop, asegurate de que este corriendo antes de continuar. Puedes verificarlo abriendo una terminal y ejecutando:

```bash
docker --version
```

Si ves un numero de version, Docker esta listo.

---

## Instalacion y ejecucion

Sigue estos pasos en orden. Solo necesitas hacerlo una vez.

### Paso 1: Obtener el proyecto

Si tienes Git instalado:

```bash
git clone <url-del-repositorio>
cd kanban-app
```

Si no tienes Git, descarga el proyecto como ZIP desde el repositorio y descomprimelo. Luego abre una terminal dentro de la carpeta `kanban-app`.

### Paso 2: Generar la base de datos

Este comando crea la base de datos SQLite y carga los datos de ejemplo. Solo se ejecuta una vez.

```bash
docker compose build backend
docker compose run --rm backend dotnet ef migrations add InitialCreate
```

Cuando veas `Now listening on: http://[::]:8080`, presiona `Ctrl + C` para detener. Es normal.

### Paso 3: Levantar la aplicacion

```bash
docker compose up --build
```

La primera vez puede tardar entre 5 y 10 minutos porque Docker descarga las imagenes base y las dependencias. Las siguientes veces sera mucho mas rapido.

Cuando veas estas dos lineas, la aplicacion esta lista:

```
kanban-backend  | Now listening on: http://[::]:8080
kanban-frontend | INFO  Accepting connections at http://localhost:3000
```

### Paso 4: Abrir en el navegador

| Servicio | URL |
|---|---|
| Aplicacion web | http://localhost:3000 |
| API REST | http://localhost:8080/api/tasks |
| Documentacion interactiva (Swagger) | http://localhost:8080/swagger |

---

## Endpoints de la API

La API sigue convenciones REST. Puedes probarla desde el Swagger en http://localhost:8080/swagger o con cualquier cliente HTTP como Postman o Insomnia.

| Metodo | Ruta | Descripcion |
|---|---|---|
| GET | /api/tasks | Obtener todas las tareas |
| GET | /api/tasks/{id} | Obtener una tarea por ID |
| POST | /api/tasks | Crear una nueva tarea |
| PUT | /api/tasks/{id} | Actualizar una tarea completa |
| PATCH | /api/tasks/{id}/status | Cambiar solo el estado (usado por drag and drop) |
| DELETE | /api/tasks/{id} | Eliminar una tarea |

### Ejemplo: Crear una tarea

```bash
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Mi primera tarea",
    "description": "Descripcion de la tarea",
    "priority": "Alta",
    "status": "Pendiente"
  }'
```

### Ejemplo: Cambiar el estado de una tarea

```bash
curl -X PATCH http://localhost:8080/api/tasks/1/status \
  -H "Content-Type: application/json" \
  -d '{"status": "En proceso"}'
```

Los valores validos para `status` son: `Pendiente`, `En proceso`, `Revision`, `Finalizado`.
Los valores validos para `priority` son: `Alta`, `Media`, `Baja`.

---

## Stack tecnologico

| Capa | Tecnologia | Version |
|---|---|---|
| Backend | .NET / ASP.NET Core | 8.0 |
| ORM | Entity Framework Core | 8.0 |
| Base de datos | SQLite | - |
| Frontend | Vue.js | 3.4 |
| Estilos | Tailwind CSS (CDN) | 3.x |
| Build tool | Vite | 5.0 |
| Contenedores | Docker + Docker Compose | - |
