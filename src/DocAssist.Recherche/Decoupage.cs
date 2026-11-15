using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

public static class Decoupage
{
    private const int LongueurMinimale = 200;

    public static List<Passage> EnPassages(string source, string markdown)
    {
        List<Passage> passages = [];
        // 1. Retirer l'en-tête du fichier : le bloc entre les deux lignes « --- » au début
        // 2. Couper le texte à chaque ligne qui commence par « ## » ou « ### »
        foreach (var morceau in Regex.Split(SansEntete(markdown), @"^(?=#{2,3} )", RegexOptions.Multiline))
        {
            string texte = morceau.Trim();

            // 3. Pour chaque morceau :
            //    - ignorer s'il fait moins de 200 caractères            
            if (texte.Length < LongueurMinimale)
                continue;

            // - Titre = la ligne de titre sans les « # », ou « Introduction » avant le premier titre
            string premiereLigne = texte.Split('\n')[0];
            string titre = premiereLigne.StartsWith('#')
                ? premiereLigne.TrimStart('#', ' ').Trim()
                : "Introduction";

            // - Source = le paramètre source ; Texte = le morceau entier
            passages.Add(new Passage { Source = source, Titre = titre, Texte = texte });

        }

        return passages;
    }

    private static string SansEntete(string markdown)
    {
        if (!markdown.StartsWith("---"))
            return markdown;

        int fin = markdown.IndexOf("\n---", 3, StringComparison.Ordinal);
        return fin < 0 ? markdown : markdown[(fin + 4)..];
    }
}