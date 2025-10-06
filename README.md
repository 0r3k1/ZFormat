# 📚 ZFormat: Biblioteca de Utilidades para Consola (C#)

**ZFormat** es una colección de clases de ayuda en C# diseñada para mejorar dramáticamente la presentación y el acabado de las aplicaciones de consola y línea de comandos. Proporciona herramientas para el formato de cadenas, el control del cursor (`gotoxy`), la animación de carga, y la generación de tablas estilizadas con bordes y colores.

## 🎯 ¿Para Qué Sirve?

Su propósito principal es convertir las salidas de consola planas y monótonas en interfaces de texto dinámicas y visualmente atractivas, utilizando las capacidades avanzadas de la Terminal (como los caracteres Unicode para bordes y el control de color). Es ideal para:

1.  **Reportes y Listados:** Generar tablas de datos claras y fáciles de leer.
2.  **Interfaces de Usuario (TUI):** Crear menús, encabezados y paneles con una estética retro moderna o estilo DOS.
3.  **Animaciones y Feedback:** Mostrar barras de progreso y mensajes de error/éxito con color.

---

## 🛠️ Clases Incluidas en la DLL

La biblioteca se compone de tres clases principales:

| Clase | Descripción | Funcionalidades Clave |
| :--- | :--- | :--- |
| **ZstringFormat** | Utilidades de texto y consola. | `centeredString()`, `gotoxy()`, `WriteColor()`, `cargando()`, `ShowError()`. |
| **ZTableStyle** | Definición de estilos. | Enumeración de estilos de borde (`SingleLine`, `DoubleLine`, `Rounded`, etc.). |
| **ZTableConsole** | Generador de Tablas. | Construye y renderiza tablas con encabezados, filas de datos, y una fila de totales opcional. |

---

## 🚀 Cómo Utilizar ZTableConsole

La clase `ZTableConsole` es el corazón visual de la biblioteca.

### 1. Inicialización

```csharp
using ZFormat; // Asegúrate de agregar esta referencia.

// Definir los encabezados de la tabla.
string[] headers = new string[] { "Código", "Descripción (Largo)", "Valor Numérico" };

// Crear una nueva tabla con un estilo de borde específico.
ZTableConsole miTabla = new ZTableConsole(headers, TableBorderStyle.DoubleLine);

// 2. Personalizar (Opcional)
miTabla.HeaderBackColor = ConsoleColor.DarkMagenta;
miTabla.AlternatingRowColor = ConsoleColor.DarkGreen;
miTabla.ShowTotalRow = true; // Activar la fila de totales (suma la última columna).
miTabla.TotalText = "Total General";

// 3. Agregar Datos
miTabla.AddRow(new string[] { "P001", "Disco Duro SSD 1TB", "1500" });
miTabla.AddRow(new string[] { "P002", "Memoria RAM 16GB", "850" });
miTabla.AddRow(new string[] { "P003", "Mouse Inalámbrico", "120" });

// 4. Renderizar
// La tabla se dibuja en la posición actual del cursor (Console.CursorTop/Left).
// También puedes forzar la posición: miTabla.RenderX = 10; miTabla.RenderY = 5;
miTabla.Render();

🙏 Agradecimiento Especial

Quiero expresar mi más sincero agradecimiento a Saraí Parada Pineda.

Su apoyo incondicional y su creencia en mis capacidades fueron la luz que me permitió superar mis dudas y recuperar la confianza en mí mismo. Es gracias a su inspiración que pude volver a concentrarme en mis estudios y, finalmente, concretar el desarrollo de esta biblioteca. Este proyecto es una pequeña muestra de lo que se puede lograr cuando se tiene un apoyo tan valioso. Gracias por ser mi mayor motor e inspiración.

