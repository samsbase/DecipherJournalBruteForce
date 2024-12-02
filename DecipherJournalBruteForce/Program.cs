// See https://aka.ms/new-console-template for more information
char[] englishLetters = [ 'J', 'Y', 'P', 'F', 'F', 'Q', 'V', 'Y' ];
char[] spanishLetters = [ 'O', 'A', 'M', 'F', 'Y', 'Y', 'P', 'N'];
char[] germanLetters = [ 'M', 'J', '?', 'I', 'G', 'N', 'P', 'B'];
char[] spanishLatamLetters = [ 'Q', 'W', 'L', 'T', 'N', 'F', 'B', 'O' ];
char[] italianLetters = [ 'N' , 'S' , 'U' , 'L' , 'R' , 'O' , 'L' , 'Q' ];
string[] english =
    [
        "In catacombs with dusty pages",
        "As paladins or rogues or mages",
        "Through all the wars that never cease",
        "In search of hope In search of peace",
        "For all the journeys, in any weather,",
        "For adventures yet to come together,",
        "The final key is why I stay:",
        "For you... For Azeroth..."
    ];

string[] spanish =
    [
        "En catacumbas con tomos polvorientos, ",
        "como paladines, magos o pícaros de aventuras hambrientos,",
        "a través de todas las guerras que nunca cesan",
        "en busca de esperanza o de paz, esquiva presa.",
        "Por todos los viajes",
        "y aventuras que de pronto llegan. ",
        "La clave final es por qué me quedo: ",
        "Por ti... Por Azeroth..."
    ];

string[] german =
    [
        "In Katakomben mit staubigen Seiten ",
        "Als Paladin, Schurke, Magier zu jenen Zeiten",
        "Durch all die Kriege, die nie enden",
        "Die Suche nach der Hoffnung Krieg in Frieden zu wenden",
        "Für jeden Traum, den wir noch träumen,",
        "Für jedes Ziel, das wir ersehnen",
        "Der letzte Schlüssel bedeutet mir viel:",
        "Für dich... Für Azeroth..."
    ];

string[] spanishLatam =
    [
        "En catacumbas con tomos polvorientos, ",
        "como paladines, magos o pícaros de aventuras hambrientos,",
        "a través de todas las guerras que nunca cesan",
        "en busca de esperanza o de paz, esquiva presa.",
        "Por todos los viajes",
        "y aventuras que de pronto llegan. ",
        "La clave final es por qué me quedo: ",
        "Por ti... Por Azeroth..."
    ];

string[] italian =
    [
        "Nelle catacombe dagli antri polverosi",
        "Paladini, ladri o maghi coraggiosi",
        "Oltre tutte le guerre che non danno tregua",
        "In cerca di speranza Di una pace che segua",
        "Per tutti i viaggi, per qualsiasi vicenda,",
        "Per le avventure che diverranno leggenda",
        "La chiave finale io invoco:",
        "Per voi... Per Azeroth..."
    ];    

// Generate arrangements for each language
var arrangementsEnglish = GetArrangements(GetPermutations(englishLetters), Language.English);
var arrangementsSpanish = GetArrangements(GetPermutations(spanishLetters), Language.Spanish);
var arrangementsGerman = GetArrangements(GetPermutations(germanLetters), Language.German);
var arrangementsSpanishLatam = GetArrangements(GetPermutations(spanishLatamLetters), Language.SpanishLatam);
var arrangementsItalian = GetArrangements(GetPermutations(italianLetters), Language.Italian);

// Find matching arrangements based on NumberCode
var arrangementsMatching = arrangementsEnglish
    .Where(englishArrangement =>
        arrangementsSpanish.Any(spanishArrangement => spanishArrangement.NumberCode.Equals(englishArrangement.NumberCode)) &&
        arrangementsGerman.Any(germanArrangement => germanArrangement.NumberCode.Equals(englishArrangement.NumberCode)) &&
        arrangementsSpanishLatam.Any(spanishLatamArrangement => spanishLatamArrangement.NumberCode.Equals(englishArrangement.NumberCode)) &&
        arrangementsItalian.Any(italianArrangement => italianArrangement.NumberCode.Equals(englishArrangement.NumberCode))
        )                  
    .ToList();

var arrangementsMatchingOrdered = arrangementsEnglish
    .Where(englishArrangement =>
        arrangementsSpanish.Any(spanishArrangement => spanishArrangement.OrderedCode.Equals(englishArrangement.OrderedCode)) &&
        arrangementsGerman.Any(germanArrangement => germanArrangement.OrderedCode.Equals(englishArrangement.OrderedCode)) &&
        arrangementsSpanishLatam.Any(spanishLatamArrangement => spanishLatamArrangement.OrderedCode.Equals(englishArrangement.OrderedCode)) &&
        arrangementsItalian.Any(italianArrangement => italianArrangement.OrderedCode.Equals(englishArrangement.OrderedCode))
    )                  
    .ToList();


Console.WriteLine("Matching Arrangements" + arrangementsMatchingOrdered.Count);
Console.WriteLine("Matching Ordered Arrangements" + arrangementsMatchingOrdered.Count);
return;

// Helper function to get arrangements
List<Arrangement> GetArrangements(IEnumerable<string> permutations, Language language) =>
    permutations.Select(permutation => GetCode(permutation, language)).ToList();

Arrangement GetCode ( string characters, Language language )
{
    var lines = language switch
    {
        Language.English => english,
        Language.Spanish => spanish,
        Language.German => german,
        Language.SpanishLatam => spanishLatam,
        Language.Italian => italian,
        _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
    };
    var stringCountCode = "";
    var intCodeArray = new List<int>();
    for ( var i = 0 ; i < 8 ; i++ )
    {
        var count = GetCount(characters.ToLower()[i], lines[i].ToLower());
        stringCountCode += count.ToString ( );
        intCodeArray.Add(count);
    }
    
    var orderedCode = string.Join("", intCodeArray.OrderBy(x => x));
    return new Arrangement(characters,  stringCountCode, orderedCode, language ) ;
}

int GetCount (char character, string line)
{
    return line.Count ( t => t == character );
}

static List<string> GetPermutations(char[] letters)
{
    var results = new List<string>();
    Array.Sort(letters); // Sort to handle duplicates
    Permute(letters, 0, results);
    return results.ToList();
}

static void Permute(char[] letters, int index, List<string> results)
{
    if (index == letters.Length - 1)
    {
        results.Add(new string(letters));
        return;
    }

    var used = new HashSet<char>(); // To avoid duplicates at this level
    for (var i = index; i < letters.Length; i++)
    {
        if (!used.Add(letters[i])) continue; // Skip duplicates

        Swap(letters, index, i);
        Permute(letters, index + 1, results);
        Swap(letters, index, i); // Backtrack
    }
}

static void Swap(char[] letters, int i, int j)
{
    ( letters[i] , letters[j] ) = ( letters[j] , letters[i] );
}


internal record Arrangement
{
    public Arrangement ( string charArrangement, string numberCode, string orderedCode, Language language )
    {
        NumberCode = numberCode;
        CharArrangement = charArrangement;
        Language = language;
        OrderedCode = orderedCode;
    }
    public string NumberCode { get; set; }
    public string OrderedCode { get; set; }
    public string CharArrangement { get; set; }
    public Language Language { get; set; }
}

internal enum Language
{
    English,
    Spanish,
    German,
    SpanishLatam,
    Italian
}