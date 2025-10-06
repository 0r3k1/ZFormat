# ?? ZFormat: Biblioteca de Utilidades para Consola (C#)

**ZFormat** es una colecci��n de clases de ayuda en C# dise?ada para mejorar dram��ticamente la presentaci��n y el acabado de las aplicaciones de consola y l��nea de comandos. Proporciona herramientas para el formato de cadenas, el control del cursor (`gotoxy`), la animaci��n de carga, y la generaci��n de tablas estilizadas con bordes y colores.

## ?? ?Para Qu�� Sirve?

Su prop��sito principal es convertir las salidas de consola planas y mon��tonas en interfaces de texto din��micas y visualmente atractivas, utilizando las capacidades avanzadas de la Terminal (como los caracteres Unicode para bordes y el control de color). Es ideal para:

1.  **Reportes y Listados:** Generar tablas de datos claras y f��ciles de leer.
2.  **Interfaces de Usuario (TUI):** Crear men��s, encabezados y paneles con una est��tica retro moderna o estilo DOS.
3.  **Animaciones y Feedback:** Mostrar barras de progreso y mensajes de error/��xito con color.

---

## ?????Clases Incluidas en la DLL

La biblioteca se compone de tres clases principales:

| Clase | Descripci��n | Funcionalidades Clave |
| :--- | :--- | :--- |
| **ZstringFormat** | Utilidades de texto y consola. | `centeredString()`, `gotoxy()`, `WriteColor()`, `cargando()`, `ShowError()`. |
| **ZTableStyle** | Definici��n de estilos. | Enumeraci��n de estilos de borde (`SingleLine`, `DoubleLine`, `Rounded`, etc.). |
| **ZTableConsole** | Generador de Tablas. | Construye y renderiza tablas con encabezados, filas de datos, y una fila de totales opcional. |

---

## ?? C��mo Utilizar ZTableConsole

La clase `ZTableConsole` es el coraz��n visual de la biblioteca.

### 1. Inicializaci��n

```csharp
using ZFormat; // Aseg��rate de agregar esta referencia.

// Definir los encabezados de la tabla.
string[] headers = new string[] { "C��digo", "Descripci��n (Largo)", "Valor Num��rico" };

// Crear una nueva tabla con un estilo de borde espec��fico.
ZTableConsole miTabla = new ZTableConsole(headers, TableBorderStyle.DoubleLine);

// 2. Personalizar (Opcional)
miTabla.HeaderBackColor = ConsoleColor.DarkMagenta;
miTabla.AlternatingRowColor = ConsoleColor.DarkGreen;
miTabla.ShowTotalRow = true; // Activar la fila de totales (suma la ��ltima columna).
miTabla.TotalText = "Total General";

// 3. Agregar Datos
miTabla.AddRow(new string[] { "P001", "Disco Duro SSD 1TB", "1500" });
miTabla.AddRow(new string[] { "P002", "Memoria RAM 16GB", "850" });
miTabla.AddRow(new string[] { "P003", "Mouse Inal��mbrico", "120" });

// 4. Renderizar
// La tabla se dibuja en la posici��n actual del cursor (Console.CursorTop/Left).
// Tambi��n puedes forzar la posici��n: miTabla.RenderX = 10; miTabla.RenderY = 5;
miTabla.Render();


?? Agradecimiento Especial

Quiero expresar mi m��s sincero agradecimiento a Sara�� Parada Pineda.

Su apoyo incondicional y su creencia en mis capacidades fueron la luz que me permiti�� superar mis dudas y recuperar la confianza en m�� mismo. Es gracias a su inspiraci��n que pude volver a concentrarme en mis estudios y, finalmente, concretar el desarrollo de esta biblioteca. Este proyecto es una peque?a muestra de lo que se puede lograr cuando se tiene un apoyo tan valioso. Gracias por ser mi mayor motor e inspiraci��n.