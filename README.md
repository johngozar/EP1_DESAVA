# EP1 DESAVA: cómo levantar el proyecto (solo con Visual Studio, sin scripts)

Este paquete contiene el **código fuente** de dos proyectos:

- `ServicioWCF_Productos`: el servicio WCF (backend), que se aloja en IIS.
- `ClienteConsola_Productos`: el cliente de consola que consume el servicio.

El servicio **no funciona con solo abrirlo**: hay que compilarlo y dejarlo publicado en IIS, en el puerto **8083**. Sigue los pasos en orden.

## 1. Requisitos previos (se hacen una sola vez por equipo)

### 1.1 Visual Studio con el componente de .NET Framework 4.8

1. Abre el **Instalador de Visual Studio** > **Modificar**.
2. En **Componentes individuales**, marca:
   - **.NET Framework 4.8 targeting pack**
   - **.NET Framework 4.8 SDK**
3. Pulsa **Modificar** y espera a que termine (con Visual Studio cerrado).

> Si falta este componente, la compilación falla con el error `MSB3644` ("No se encuentran los ensamblados de referencia para .NETFramework,Version=v4.8").

### 1.2 IIS con soporte para WCF

1. Abre **Panel de control > Programas > Activar o desactivar las características de Windows**.
2. Marca (los nombres pueden variar levemente según el idioma y la versión de Windows):
   - **Internet Information Services**
     - **Herramientas de administración web** > **Consola de administración de IIS**
     - **Servicios World Wide Web** > **Características de desarrollo de aplicaciones** > **ASP.NET 4.8** (esto marca también las extensiones necesarias)
   - **.NET Framework 4.8 Advanced Services** (Servicios avanzados) > **Servicios WCF** > **Activación HTTP**
3. Acepta y, si Windows lo pide, **reinicia el equipo**.

> Sin la **Activación HTTP de WCF**, IIS no reconoce los archivos `.svc`.

## 2. Compilar

1. Extrae el `.zip` (por ejemplo en `C:\Repos\EP1_DESAVA`). Antes, en las propiedades del `.zip` marca **Desbloquear** si aparece esa casilla.
2. Abre `EP1_DESAVA.sln` con Visual Studio.
3. Menú **Compilar > Recompilar solución**.
4. En la ventana de salida debe aparecer **"Correctos: 2"** (o "Succeeded: 2") y ningún error.

Se generan estos archivos (no venían en el `.zip`):

- `ServicioWCF_Productos\bin\ServicioWCF_Productos.dll`
- `ClienteConsola_Productos\bin\Debug\ClienteConsola_Productos.exe`

## 3. Publicar el servicio en IIS (copia manual de archivos)

Este proyecto no tiene el botón **Publicar** de Visual Studio, así que se copian a mano los archivos que IIS necesita.

1. Crea estas carpetas (necesitas permisos de administrador, Windows te lo pedirá):
   - `C:\inetpub\wwwroot\ServicioProductos`
   - `C:\inetpub\wwwroot\ServicioProductos\bin`
2. Copia **todo el contenido** de `ServicioWCF_Productos\bin\` a `C:\inetpub\wwwroot\ServicioProductos\bin\`.
3. Copia `Service1.svc` y `Web.config` (están en `ServicioWCF_Productos\`) a `C:\inetpub\wwwroot\ServicioProductos\`.

Resultado esperado:

```
C:\inetpub\wwwroot\ServicioProductos\
├── Service1.svc
├── Web.config
└── bin\
    └── ServicioWCF_Productos.dll  (y el .pdb)
```

## 4. Crear el sitio en el Administrador de IIS

Abre el **Administrador de IIS** (busca `inetmgr` en el menú Inicio).

**Application pool (grupo de aplicaciones):**

1. Clic derecho en **Grupos de aplicaciones > Agregar grupo de aplicaciones**.
2. Nombre `ServicioProductos`, **Versión de .NET CLR: v4.0**, modo de canalización **Integrado**. Aceptar.

**Sitio web:**

1. Clic derecho en **Sitios > Agregar sitio web**.
2. Nombre del sitio: `ServicioProductos`.
3. Grupo de aplicaciones: `ServicioProductos` (usa **Seleccionar** para elegirlo).
4. Ruta de acceso física: `C:\inetpub\wwwroot\ServicioProductos`.
5. Enlace: tipo `http`, dirección IP `Todas las no asignadas`, **puerto `8083`**. Aceptar.

Si Windows muestra un aviso de que el puerto 8083 está en uso, otro programa lo ocupa: cierra ese programa o consulta la sección 7.

## 5. Comprobar que el servicio está activo

1. Abre en el navegador: **http://localhost:8083/Service1.svc**
2. Debe aparecer la página **"Servicio de Service1"** ("Creó un servicio.").
3. Para confirmar que publica su descripción, abre también: `http://localhost:8083/Service1.svc?singleWsdl` (debe mostrar un XML).

Si no aparece, ve a la sección 7.

## 6. Ejecutar el cliente

1. En Visual Studio, clic derecho en el proyecto **ClienteConsola_Productos > Establecer como proyecto de inicio**.
2. Menú **Depurar > Iniciar sin depurar** (Ctrl+F5), para que la ventana no se cierre sola.
3. Resultado esperado en la consola:

```
=== ListarProductos() ===
1 | Laptop Lenovo | 2500
2 | Mouse Logitech | 80
3 | Teclado Redragon | 150
4 | Monitor Samsung | 900
5 | Auriculares HyperX | 320

=== ObtenerProducto(3) ===
3 | Teclado Redragon | 150
```

**Cómo comprobar que el cliente usa realmente el servicio:** en el Administrador de IIS, selecciona el sitio `ServicioProductos` y pulsa **Detener** (panel derecho). Ejecuta el cliente otra vez: debe mostrar un error de comunicación. Vuelve a **Iniciar** el sitio y el cliente funcionará de nuevo.

> Importante: **no hace falta ejecutar el servicio desde Visual Studio (F5)**. IIS lo sirve por sí solo, como servicio de Windows. Visual Studio solo compila.

## 7. Si algo falla

| Síntoma | Causa probable | Qué hacer |
|---|---|---|
| Error `MSB3644` al compilar | Falta el paquete de destino de .NET 4.8 | Sección 1.1 |
| Error `MSB4019` sobre `Microsoft.WebApplication.targets` | Falta la carga de trabajo "Desarrollo de ASP.NET y web" | Instálala desde el Instalador de Visual Studio, o pide la versión corregida del `.csproj` |
| `http://localhost:8083/Service1.svc` no abre | El sitio está detenido, o el puerto no es el 8083 | Revisa en IIS que el sitio esté **Iniciado** y el puerto sea 8083 |
| El navegador muestra el texto del `.svc` o un **404** | Falta activar WCF en IIS | Sección 1.2: **Activación HTTP** de WCF |
| **500** con "The type 'ServicioWCF_Productos.Service1' ... could not be found" | Falta la `.dll` en `bin` o está en otra carpeta | Comprueba que exista `C:\inetpub\wwwroot\ServicioProductos\bin\ServicioWCF_Productos.dll` |
| **503** | El grupo de aplicaciones está detenido | Inícialo en IIS, y comprueba que use .NET CLR v4.0 |
| **403** o error de permisos | La cuenta de IIS no puede leer la carpeta | Clic derecho en la carpeta > Propiedades > Seguridad: da **lectura** al grupo `IIS_IUSRS` |
| El cliente dice "No había ningún extremo escuchando..." | El servicio no está activo en ese puerto | Repite la sección 5 |
| El puerto 8083 está ocupado | Otro programa lo usa | Usa otro puerto en el enlace del sitio (por ejemplo 8090) y cambia el `address` del `<endpoint>` en `ClienteConsola_Productos\App.config` para que coincida |
| Cambiaste el código del servicio y el cliente sigue mostrando datos viejos | IIS sirve la copia de `C:\inetpub`, no la de tu carpeta | Recompila y vuelve a copiar los archivos de la sección 3 |

## 8. Notas

- Este paquete contiene solo código fuente: las carpetas `bin`, `obj` y `.vs` se regeneran al compilar.
- La referencia de servicio del cliente (`Connected Services`) ya viene agregada. **No hace falta agregarla de nuevo**, salvo que cambies el contrato del servicio.
- Estos pasos no se han probado en otro equipo distinto del original.
