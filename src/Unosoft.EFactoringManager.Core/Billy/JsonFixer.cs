using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Billy
{
    public static class JsonFixer
    {
        public static string FixInvoiceJson(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return raw;

            // 1. Reparar caracteres mal codificados comunes
            raw = raw.Replace("󮩣a", "ónica")
                     .Replace("󮠥xpresa", "expresa")
                     .Replace("󮠤el", "del")
                     .Replace("󮠇eneral", "General")
                     .Replace("󮢬", "ación")
                     .Replace("с", "ñ");

            // 2. Detectar el bloque Status hasta el final del JSON
            var match = Regex.Match(raw, @"""Status"":.*}$", RegexOptions.Singleline);
            if (match.Success)
            {
                string broken = match.Value;

                // 3. Arreglar comillas mal colocadas: `" Campo ":` → `"Campo":`
                string fixedBlock = Regex.Replace(broken, @"""\s*([^""]+)\s*""\s*:", @"""$1"":");

                // 4. Arreglar valores string que quedaron con espacios extras
                fixedBlock = Regex.Replace(fixedBlock, @"\s+", " ");

                // 5. Reemplazar el bloque roto en el JSON original
                raw = raw.Replace(broken, fixedBlock);
            }

            return raw;
        }
    }
}
