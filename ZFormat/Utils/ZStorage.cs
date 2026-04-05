using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;

namespace ZFormat.Utils {
    public static class ZStorage {
        public static void SaveJson<T>(string fileName, T data) {
            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, jsonString);
        }

        public static T LoadJson<T>(string fileName) {
            if(!File.Exists(fileName))
                return default;
            string jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<T>(jsonString);
        }


        public static void ExportToCSV(string fileName, string[] headers, List<string[]> rows) {
            StringBuilder sb = new StringBuilder();

            // 1. Escribir Encabezados
            sb.AppendLine(string.Join(",", headers.Select(h => $"\"{h.Trim()}\"")));

            // 2. Escribir Filas con limpieza de moneda
            foreach(var row in rows) {
                var cleanedRow = row.Select(cell => {
                    string content = cell ?? "";

                    // Limpieza: Quitamos símbolos de moneda para que Excel lo vea como número
                    // También escapamos comillas dobles internas por seguridad
                    content = content.Replace("Q", "")
                                   .Replace("$", "")
                                   .Replace("\"", "\"\"")
                                   .Trim();

                    return $"\"{content}\"";
                });

                sb.AppendLine(string.Join(",", cleanedRow));
            }

            // 3. Guardar con codificación UTF-8 para no perder tildes ni la 'ñ'
            File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
        }
    }
}