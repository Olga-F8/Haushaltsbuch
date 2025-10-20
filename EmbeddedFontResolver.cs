using PdfSharp.Fonts;
using System;
using System.IO;
using System.Reflection;

public class EmbeddedFontResolver : IFontResolver
{
    // Namen, die Sie in XFont(...) verwenden: z. B. "NotoSans"
    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (familyName.Equals("NotoSans", StringComparison.OrdinalIgnoreCase))
        {
            // key string used in GetFont
            return new FontResolverInfo("NotoSans-Regular");
        }
        // Fallback
        return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
    }

    public byte[] GetFont(string faceName)
    {
        if (faceName == "NotoSans-Regular")
        {
            var asm = Assembly.GetExecutingAssembly();
            using Stream s = asm.GetManifestResourceStream("Haushaltsbuch.Fonts.NotoSans-Regular.ttf");
            if (s == null) throw new InvalidOperationException("Embedded font not found.");
            using var ms = new MemoryStream();
            s.CopyTo(ms);
            return ms.ToArray();
        }
        throw new NotSupportedException(faceName);
    }
}