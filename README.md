# ?? ZFormat: Biblioteca de Utilidades para Consola (C#)

**ZFormat** es una coleccion de clases de ayuda en C# disenada para mejorar dramaticamente la presentacion y el acabado de las aplicaciones de consola y linea de comandos. Proporciona herramientas para el formato de cadenas, el control del cursor (`gotoxy`), la animacion de carga, y la generacion de tablas estilizadas con bordes y colores.

## ?? ?Para Que Sirve?

Su proposito principal es convertir las salidas de consola planas y monotonas en interfaces de texto dinamicas y visualmente atractivas, utilizando las capacidades avanzadas de la Terminal (como los caracteres Unicode para bordes y el control de color). Es ideal para:

1.  **Reportes y Listados:** Generar tablas de datos claras y faciles de leer.
2.  **Interfaces de Usuario (TUI):** Crear menus, encabezados y paneles con una estetica retro moderna o estilo DOS.
3.  **Animaciones y Feedback:** Mostrar barras de progreso y mensajes de error/exito con color.

---

## ?????Clases Incluidas en la DLL

La biblioteca se compone de tres clases principales:

| Clase | Descripcion | Funcionalidades Clave |
| :--- | :--- | :--- |
| **ZstringFormat** | Utilidades de texto y consola. | `centeredString()`, `gotoxy()`, `WriteColor()`, `cargando()`, `ShowError()`. |
| **ZTableStyle** | Definicion de estilos. | Enumeracion de estilos de borde (`SingleLine`, `DoubleLine`, `Rounded`, etc.). |
| **ZTableConsole** | Generador de Tablas. | Construye y renderiza tablas con encabezados, filas de datos, y una fila de totales opcional. |

---

## ?? Como Utilizar ZTableConsole

La clase `ZTableConsole` es el corazon visual de la biblioteca.

### 1. Inicializacion

```csharp
using ZFormat; // Asegurate de agregar esta referencia.

// Definir los encabezados de la tabla.
string[] headers = new string[] { "Codigo", "Descripcion (Largo)", "Valor Numerico" };

// Crear una nueva tabla con un estilo de borde especifico.
ZTableConsole miTabla = new ZTableConsole(headers, TableBorderStyle.DoubleLine);

// 2. Personalizar (Opcional)
miTabla.HeaderBackColor = ConsoleColor.DarkMagenta;
miTabla.AlternatingRowColor = ConsoleColor.DarkGreen;
miTabla.ShowTotalRow = true; // Activar la fila de totales (suma la ultima columna).
miTabla.TotalText = "Total General";

// 3. Agregar Datos
miTabla.AddRow(new string[] { "P001", "Disco Duro SSD 1TB", "1500" });
miTabla.AddRow(new string[] { "P002", "Memoria RAM 16GB", "850" });
miTabla.AddRow(new string[] { "P003", "Mouse Inalambrico", "120" });

// 4. Renderizar
// La tabla se dibuja en la posicion actual del cursor (Console.CursorTop/Left).
// Tambien puedes forzar la posicion: miTabla.RenderX = 10; miTabla.RenderY = 5;
miTabla.Render();


?? Agradecimiento Especial

Quiero expresar mi mas sincero agradecimiento a Sarai Parada Pineda.

Su apoyo incondicional y su creencia en mis capacidades fueron la luz que me permitio superar mis dudas y recuperar la confianza en mi mismo. Es gracias a su inspiracion que pude volver a concentrarme en mis estudios y, finalmente, concretar el desarrollo de esta biblioteca. Este proyecto es una pequena muestra de lo que se puede lograr cuando se tiene un apoyo tan valioso. Gracias por ser mi mayor motor e inspiracion.
