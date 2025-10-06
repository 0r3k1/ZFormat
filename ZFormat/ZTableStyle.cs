using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFormat {
    // 1. Enumeración de estilos predefinidos. El usuario elige uno al crear la tabla.
    public enum TableBorderStyle {
        Ascii,           // El estilo clásico de terminal ('+---+' con ASCII básico).
        SingleLine,      // Líneas simples (┌───┐). Buena elección por defecto.
        DoubleLine,      // Líneas dobles (╔═══╗). Estilo más formal/elegante.
        Rounded,         // Esquinas curvas (╭───╮). Estilo moderno.
        HeavyLine        // Bordes gruesos de relleno (███). Para un efecto de bloque sólido.
    }

    /// <summary>
    /// Define el conjunto de caracteres especiales (piezas) necesarios para dibujar un borde de tabla.
    /// </summary>
    public class ZTableStyle {
        // --- Líneas Base ---
        public char Horizontal { get; set; } = '═';
        public char Vertical { get; set; } = '║';
        // Nota: La propiedad 'Separator' original no se usa realmente en la lógica de borde,
        // ya que el padding lo crea la lógica de la tabla. Se omite o se deja como nota.
        public char Separator { get; set; } = ' ';

        // --- Intersecciones (Cruce en forma de T o Cruz Completa) ---
        public char Cross { get; set; } = '╬';      // Cruce central (Header Separator o fila interior)
        public char TUp { get; set; } = '╦';        // Cruce superior (Bottom de la línea Top)
        public char TDown { get; set; } = '╩';      // Cruce inferior (Header Separator o Top de la línea Bottom)
        public char TLeft { get; set; } = '╠';      // Cruce izquierdo (Final del borde izquierdo)
        public char TRight { get; set; } = '╣';     // Cruce derecho (Final del borde derecho)

        // --- Esquinas (Corners) ---
        public char TopLeft { get; set; } = '╔';
        public char TopRight { get; set; } = '╗';
        public char BottomLeft { get; set; } = '╚';
        public char BottomRight { get; set; } = '╝';

        /// <summary>
        /// Constructor estático: devuelve una instancia de ZTableStyle con un set de caracteres predefinido.
        /// </summary>
        /// <param name="style">El estilo de borde deseado.</param>
        public static ZTableStyle GetStyle(TableBorderStyle style) {
            switch(style) {
                case TableBorderStyle.Ascii:
                return new ZTableStyle {
                    // Solo caracteres ASCII básicos.
                    Horizontal = '-',
                    Vertical = '|',
                    // En ASCII, todos los cruces y esquinas suelen ser el mismo carácter '+'.
                    Cross = '+',
                    TUp = '+',
                    TDown = '+',
                    TLeft = '+',
                    TRight = '+',
                    TopLeft = '+',
                    TopRight = '+',
                    BottomLeft = '+',
                    BottomRight = '+'
                };

                case TableBorderStyle.DoubleLine:
                return new ZTableStyle {
                    // Usar caracteres de línea doble (═ y ║)
                    Horizontal = '═',
                    Vertical = '║',
                    // Caracteres de cruce para línea doble
                    Cross = '╫',
                    TUp = '╤',
                    TDown = '╧',
                    TLeft = '╟',
                    TRight = '╢',
                    // Esquinas de línea doble
                    TopLeft = '╔',
                    TopRight = '╗',
                    BottomLeft = '╚',
                    BottomRight = '╝'
                };

                case TableBorderStyle.Rounded:
                return new ZTableStyle {
                    // Línea simple (─ y │)
                    Horizontal = '─',
                    Vertical = '│',
                    // Cruces de línea simple
                    Cross = '┼',
                    TUp = '┬',
                    TDown = '┴',
                    TLeft = '├',
                    TRight = '┤',
                    // Esquinas redondeadas
                    TopLeft = '╭',
                    TopRight = '╮',
                    BottomLeft = '╰',
                    BottomRight = '╯'
                };

                case TableBorderStyle.HeavyLine:
                return new ZTableStyle {
                    // Usar el carácter de bloque relleno '█' para un estilo grueso
                    Horizontal = '█',
                    Vertical = '█',
                    // Todos los cruces también serán el bloque relleno para simplificar el dibujo
                    Cross = '█',
                    TUp = '█',
                    TDown = '█',
                    TLeft = '█',
                    TRight = '█',
                    // Esquinas
                    TopLeft = '█',
                    TopRight = '█',
                    BottomLeft = '█',
                    BottomRight = '█'
                };

                case TableBorderStyle.SingleLine:
                default:
                return new ZTableStyle {
                    // Este es el estilo por defecto, usa caracteres de línea simple (─ y │)
                    Horizontal = '─',
                    Vertical = '│',
                    Cross = '┼',
                    TUp = '┬',
                    TDown = '┴',
                    TLeft = '├',
                    TRight = '┤',
                    // Esquinas de línea simple (rectas)
                    TopLeft = '┌',
                    TopRight = '┐',
                    BottomLeft = '└',
                    BottomRight = '┘'
                };
            }
        }
    }
}