# ?? ZFormat: Biblioteca de Utilidades para Consola (C#)

**ZFormat** es una colecci車n de clases de ayuda en C# dise?ada para mejorar dram芍ticamente la presentaci車n y el acabado de las aplicaciones de consola y l赤nea de comandos. Proporciona herramientas para el formato de cadenas, el control del cursor (`gotoxy`), la animaci車n de carga, y la generaci車n de tablas estilizadas con bordes y colores.

## ?? ?Para Qu谷 Sirve?

Su prop車sito principal es convertir las salidas de consola planas y mon車tonas en interfaces de texto din芍micas y visualmente atractivas, utilizando las capacidades avanzadas de la Terminal (como los caracteres Unicode para bordes y el control de color). Es ideal para:

1.  **Reportes y Listados:** Generar tablas de datos claras y f芍ciles de leer.
2.  **Interfaces de Usuario (TUI):** Crear men迆s, encabezados y paneles con una est谷tica retro moderna o estilo DOS.
3.  **Animaciones y Feedback:** Mostrar barras de progreso y mensajes de error/谷xito con color.

---

## ?????Clases Incluidas en la DLL

La biblioteca se compone de tres clases principales:

| Clase | Descripci車n | Funcionalidades Clave |
| :--- | :--- | :--- |
| **ZstringFormat** | Utilidades de texto y consola. | `centeredString()`, `gotoxy()`, `WriteColor()`, `cargando()`, `ShowError()`. |
| **ZTableStyle** | Definici車n de estilos. | Enumeraci車n de estilos de borde (`SingleLine`, `DoubleLine`, `Rounded`, etc.). |
| **ZTableConsole** | Generador de Tablas. | Construye y renderiza tablas con encabezados, filas de datos, y una fila de totales opcional. |

---

## ?? C車mo Utilizar ZTableConsole

La clase `ZTableConsole` es el coraz車n visual de la biblioteca.

### 1. Inicializaci車n

```csharp
using ZFormat; // Aseg迆rate de agregar esta referencia.

// Definir los encabezados de la tabla.
string[] headers = new string[] { "C車digo", "Descripci車n (Largo)", "Valor Num谷rico" };

// Crear una nueva tabla con un estilo de borde espec赤fico.
ZTableConsole miTabla = new ZTableConsole(headers, TableBorderStyle.DoubleLine);

// 2. Personalizar (Opcional)
miTabla.HeaderBackColor = ConsoleColor.DarkMagenta;
miTabla.AlternatingRowColor = ConsoleColor.DarkGreen;
miTabla.ShowTotalRow = true; // Activar la fila de totales (suma la 迆ltima columna).
miTabla.TotalText = "Total General";

// 3. Agregar Datos
miTabla.AddRow(new string[] { "P001", "Disco Duro SSD 1TB", "1500" });
miTabla.AddRow(new string[] { "P002", "Memoria RAM 16GB", "850" });
miTabla.AddRow(new string[] { "P003", "Mouse Inal芍mbrico", "120" });

// 4. Renderizar
// La tabla se dibuja en la posici車n actual del cursor (Console.CursorTop/Left).
// Tambi谷n puedes forzar la posici車n: miTabla.RenderX = 10; miTabla.RenderY = 5;
miTabla.Render();


?? Agradecimiento Especial

Quiero expresar mi m芍s sincero agradecimiento a Sara赤 Parada Pineda.

Su apoyo incondicional y su creencia en mis capacidades fueron la luz que me permiti車 superar mis dudas y recuperar la confianza en m赤 mismo. Es gracias a su inspiraci車n que pude volver a concentrarme en mis estudios y, finalmente, concretar el desarrollo de esta biblioteca. Este proyecto es una peque?a muestra de lo que se puede lograr cuando se tiene un apoyo tan valioso. Gracias por ser mi mayor motor e inspiraci車n.