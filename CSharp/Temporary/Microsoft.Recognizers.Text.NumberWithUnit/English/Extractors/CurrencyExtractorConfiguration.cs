﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class CurrencyExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public CurrencyExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public CurrencyExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => CurrencySuffixList;

        public override ImmutableDictionary<string, string> PrefixList => CurrencyPrefixList;

        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_CURRENCY;

        public static readonly ImmutableDictionary<string, string> CurrencySuffixList = new Dictionary<string, string>
        {
            //Reference Source: https://en.wikipedia.org/wiki/List_of_circulating_currencies

            //Abkhazian apsar
            {"Abkhazian apsar", "abkhazian apsar|apsars"},
            //Afghan afghani
            {"Afghan afghani", "afghan afghani|؋|afn|afghanis|afghani"},
            {"Pul", "pul"},
            //Euro
            {"Euro", "euros|euro|€|eur"},
            {"Cent", "cents|cent|-cents|-cent|sen"},
            //Albanian lek
            //No use of "ALL" "L"
            {"Albanian lek", "albanian lek|leks|lek"},
            {"Qindarkë", "qindarkë|qindarkës|qindarke|qindarkes"},
            //Angolan kwanza
            {"Angolan kwanza", "angolan kwanza|kz|aoa|kwanza|kwanzas|angolan kwanzas"},
            //Armenian
            //No use of "AMD"
            {"Armenian dram", "armenian drams|armenian dram"},
            //Aruban florin
            {"Aruban florin", "aruban florin|ƒ|awg|aruban florins"},
            //Bangladeshi taka
            {"Bangladeshi taka", "bangladeshi taka|৳|bdt|taka|takas|bangladeshi takas"},
            {"Paisa", "poisha|paisa"},
            //Bhutanese ngultrum
            {"Bhutanese ngultrum", "Bhutanese ngultrum|nu.|btn"},
            {"Chetrum", "chetrums|chetrum"},
            //Bolivian boliviano
            {
                "Bolivian boliviano",
                "bolivian boliviano|bob|bs.|bolivia boliviano|bolivia bolivianos|bolivian bolivianos"
            },
            //Bosnia and Herzegovina convertible mark	
            {"Bosnia and Herzegovina convertible mark", "bosnia and herzegovina convertible mark|bam"},
            {"Fening", "fenings|fenings"},
            //Botswana pula
            {"Botswana pula", "botswana pula|bwp|pula|pulas|botswana pulas"},
            {"Thebe", "thebe"},
            //Brazilian real
            {"Brazilian real", "brazilian real|r$|brl|brazil real|brazil reals|brazilian reals"},
            //Bulgarian lev	
            {"Bulgarian lev", "bulgarian lev|bgn|лв|bulgaria lev|bulgaria levs|bulgarian levs"},
            {"Stotinka", "stotinki|stotinka"},
            //Cambodian riel
            {"Cambodian riel", "cambodian riel|khr|៛|cambodia riel|cambodia riels|cambodian riels"},
            //Cape Verdean escudo
            //No use of "esc" and "$"
            {"Cape Verdean escudo", "cape verdean escudo|cve"},
            //Costa Rican colón	
            {
                "Costa Rican colón",
                "costa rican colón|costa rican colóns|crc|₡|costa rica colón|costa rica colóns|costa rican colon|costa rican colons|costa rica colon|costa rica colons"
            },
            //Salvadoran colón
            {
                "Salvadoran colón",
                "svc|salvadoran colón|salvadoran colóns|salvador colón|salvador colóns|salvadoran colon|salvadoran colons|salvador colon|salvador colons"
            },
            {"Céntimo", "céntimo"},
            //Croatian kuna
            {"Croatian kuna", "croatian kuna|kn|hrk|croatia kuna|croatian kunas|croatian kuna kunas"},
            {"Lipa", "lipa"},
            //Czech koruna
            {"Czech koruna", "czech koruna|czk|Kč|czech korunas"},
            {"Haléř", "haléř"},
            //Eritrean nakfa
            {"Eritrean nakfa", "eritrean nakfa|nfk|ern|eritrean nakfas"},
            //Ethiopian birr
            {"Ethiopian birr", "ethiopian birr|etb"},
            //Gambian dalasi
            {"Gambian dalasi", "gmd"},
            {"Butut", "bututs|butut"},
            //Georgian lari
            {"Georgian lari", "Georgian lari|lari|gel|₾"},
            {"Tetri", "tetri"},
            //Ghanaian cedi
            {"Ghanaian cedi", "Ghanaian cedi|ghs|₵|gh₵"},
            {"Pesewa", "pesewas|pesewa"},
            //Guatemalan quetzal
            {"Guatemalan quetzal", "guatemalan quetzal|gtq|guatemala quetzal"},
            //Haitian gourde
            //No use of G
            {"Haitian gourde", "haitian gourde|htg"},
            //Honduran lempira
            //No use of L
            {"Honduran lempira", "honduran lempira|hnl"},
            //Hungarian forint
            {"Hungarian forint", "hungarian forint|huf|ft|hungary forint|hungary forints|hungarian forints"},
            {"Fillér", "fillér"},
            //Iranian rial
            {"Iranian rial", "iranian rial|irr|iran rial|iran rials|iranian rials"},
            {"Yemeni rial", "yemeni rial|yer|yemeni rials"},
            {"Israeli new shekel", "₪|ils|agora"},
            //Lithuanian litas
            {"Lithuanian litas", "ltl|lithuanian litas|lithuan litas|lithuanian lit|lithuan lit" },
            //Japanese yen
            {"Japanese yen", "japanese yen|jpy|yen|-yen|¥|yens|japanese yens|japan yen|japan yens"},
            //Kazakhstani tenge
            {"Kazakhstani tenge", "Kazakhstani tenge|kzt"},
            //Kenyan shilling
            {"Kenyan shilling", "kenyan shilling|sh|kes"},
            //North Korean won
            {"North Korean won", "north korean won|kpw|north korean wons"},
            {"South Korean won", "south korean won|krw|south korean wons"},
            {"Korean won", "korean won|₩|korean wons"},
            //Kyrgyzstani som
            {"Kyrgyzstani som", "kyrgyzstani som|kgs"},
            //Uzbekitan som
            {"Uzbekitan som", "uzbekitan som|uzs"},
            //Lao kip
            {"Lao kip", "lao kip|lak|₭n|₭"},
            {"Att", "att"},
            //Lesotho loti
            {"Lesotho loti", "lesotho loti|lsl|loti"},
            {"Sente", "sente|lisente"},
            //South African rand
            {"South African rand", "south african rand|zar|south africa rand|south africa rands|south african rands"},
            //Macanese pataca
            {"Macanese pataca", "macanese pataca|mop$|mop"},
            {"Avo", "avos|avo"},
            //Macedonian denar
            {"Macedonian denar", "macedonian denar|mkd|ден"},
            {"Deni", "deni"},
            //Malagasy ariary
            {"Malagasy ariary", "malagasy ariary|mga"},
            {"Iraimbilanja", "iraimbilanja"},
            //Malawian kwacha
            {"Malawian kwacha", "malawian kwacha|mk|mwk"},
            {"Tambala", "tambala"},
            //Malaysian ringgit
            {"Malaysian ringgit", "malaysian ringgit|rm|myr|malaysia ringgit|malaysia ringgits|malaysian ringgits"},
            //Mauritanian ouguiya
            {
                "Mauritanian ouguiya",
                "mauritanian ouguiya|um|mro|mauritania ouguiya|mauritania ouguiyas|mauritanian ouguiyas"
            },
            {"Khoums", "khoums"},
            //Mongolian tögrög
            {
                "Mongolian tögrög",
                "mongolian tögrög|mnt|₮|mongolia tögrög|mongolia tögrögs|mongolian tögrögs|mongolian togrog|mongolian togrogs|mongolia togrog|mongolia togrogs"
            },
            //Mozambican metical
            {"Mozambican metical", "mozambican metical|mt|mzn|mozambica metical|mozambica meticals|mozambican meticals"},
            //Burmese kyat
            {"Burmese kyat", "Burmese kyat|ks|mmk"},
            {"Pya", "pya"},
            //Nicaraguan córdoba
            {"Nicaraguan córdoba", "nicaraguan córdoba|nio"},
            //Nigerian naira
            {"Nigerian naira", "nigerian naira|naira|ngn|₦|nigeria naira|nigeria nairas|nigerian nairas"},
            {"Kobo", "kobo"},
            //Turkish lira
            {"Turkish lira", "turkish lira|try|tl|turkey lira|turkey liras|turkish liras"},
            {"Kuruş", "kuruş"},
            //Omani rial
            {"Omani rial", "omani rial|omr|ر.ع."},
            //Panamanian balboa
            {"Panamanian balboa", "panamanian balboa|b/.|pab"},
            {"Centesimo", "centesimo|céntimo"},
            //Papua New Guinean kina
            {"Papua New Guinean kina", "papua new guinean kina|kina|pgk"},
            {"Toea", "toea"},
            //Paraguayan guaraní
            {"Paraguayan guaraní", "paraguayan guaraní|₲|pyg"},
            //Peruvian sol
            {"Peruvian sol", "peruvian sol|soles|sol|peruvian nuevo sol"},
            //Polish zloty
            {"Polish złoty", "złoty|polish złoty|zł|pln|zloty|polish zloty|poland zloty|poland złoty"},
            {"Grosz", "groszy|grosz|grosze"},
            //Qatari riyal
            {"Qatari riyal", "qatari riyal|qar|qatari riyals|qatar riyal|qatar riyals"},
            {"Saudi riyal", "saudi riyal|sar|saudi riyals"},
            {"Riyal", "riyal|riyals|rial|﷼"},
            {"Dirham", "dirham|dirhem|dirhm"},
            {"Halala", "halalas|halala"},
            //Samoan tālā
            {"Samoan tālā", "samoan tālā|tālā|tala|ws$|samoa|wst|samoan tala"},
            {"Sene", "sene"},
            //São Tomé and Príncipe dobra
            {"São Tomé and Príncipe dobra", "são tomé and príncipe dobra|dobras|dobra|std"},
            //Sierra Leonean leone
            {"Sierra Leonean leone", "sierra Leonean leone|sll|leone|le"},
            //Spain
            {"Peseta", "pesetas|peseta"},
            //Netherlands guilder
            {
                "Netherlands guilder",
                "florin|netherlands antillean guilder|ang|ƒ|nederlandse gulden|guilders|guilder|gulden|-guilders|-guilder|dutch guilders|dutch guilder|fl"
            },
            //Swazi lilangeni
            {"Swazi lilangeni", "swazi lilangeni|lilangeni|szl|emalangeni"},
            //Tajikistani somoni
            {"Tajikistani somoni", "tajikistani somoni|tjs|somoni"},
            {"Diram", "dirams|diram"},
            //Thai baht
            {"Thai baht", "thai baht|฿|thb|baht"},
            {"Satang", "satang|satangs"},
            //Tongan paʻanga
            {"Tongan paʻanga", "tongan paʻanga|paʻanga|tongan pa'anga|pa'anga"},
            {"Seniti", "seniti"},
            //Ukrainian hryvnia
            {"Ukrainian hryvnia", "ukrainian hryvnia|hyrvnia|uah|₴|ukrain hryvnia|ukrain hryvnias|ukrainian hryvnias"},
            //Vanuatu vatu
            {"Vanuatu vatu", "vanuatu vatu|vatu|vuv"},
            //Venezuelan bolívar
            {
                "Venezuelan bolívar",
                "venezuelan bolívar|venezuelan bolívars|bs.f.|vef|bolívar fuerte|venezuelan bolivar|venezuelan bolivars|venezuela bolivar|venezuela bolivarsvenezuelan bolivar|venezuelan bolivars"
            },
            //Vietnamese dong
            {"Vietnamese dong", "vietnamese dong|vnd|đồng|vietnam dong|vietnamese dongs|vietnam dongs"},
            //Zambian kwacha
            {"Zambian kwacha", "zambian kwacha|zk|zmw|zambia kwacha|kwachas|zambian kwachas"},
            //Dirham
            {"Moroccan dirham", "moroccan dirham|mad|د.م."},
            {"United Arab Emirates dirham", "united arab emirates dirham|د.إ|aed"},
            //Manat
            {"Azerbaijani manat", "azerbaijani manat|azn"},
            {"Turkmenistan manat", "turkmenistan manat|turkmenistan new manat|tmt"},
            {"Manat", "manats|manat"},
            {"Qəpik", "qəpik"},
            //Shilling
            {
                "Somali shilling",
                "somali shillings|somali shilling|shilin soomaali|-shilin soomaali|scellino|shilin|sh.so.|sos"
            },
            {"Somaliland shilling", "somaliland shillings|somaliland shilling|soomaaliland shilin"},
            {
                "Tanzanian shilling",
                "tanzanian shilling|tanzanian shillings|tsh|tzs|tanzania shilling|tanzania shillings"
            },
            {"Ugandan shilling", "ugandan shilling|ugandan shillings|sh|ugx|uganda shilling|uganda shillings"},
            //Leu
            {"Romanian leu", "romanian leu|lei|ron|romania leu"},
            {"Moldovan leu", "moldovan leu|mdl|moldova leu"},
            {"Leu", "leu"},
            {"Ban", "bani|-ban|ban"},
            //Rupee
            {"Nepalese rupee", "nepalese rupee|npr"},
            {"Pakistani rupee", "pakistani rupee|pkr"},
            {"Indian rupee", "indian rupee|inr|₹|india rupee"},
            {"Seychellois rupee", "seychellois rupee|scr|sr|sre"},
            {"Mauritian rupee", "mauritian rupee|mur"},
            {"Maldivian rufiyaa", "maldivian rufiyaa|rf|mvr|.ރ|maldive rufiyaa"},
            {"Sri Lankan rupee", "sri Lankan rupee|lkr|රු|ரூ"},
            {"Indonesian rupiah", "Indonesian rupiah|rupiah|perak|rp|idr"},
            {"Rupee", "rupee|rs"},
            //Krone
            {"Danish krone", "danish krone|dkk|denmark krone|denmark krones|danish krones"},
            {"Norwegian krone", "norwegian krone|nok|norway krone|norway krones|norwegian krones"},
            {"Faroese króna", "faroese króna|faroese krona"},
            {"Icelandic króna", "icelandic króna|isk|icelandic krona|iceland króna|iceland krona"},
            {"Swedish krona", "swedish krona|sek|swedan krona"},
            {"Krone", "kronor|krona|króna|krone|krones|kr|-kr"},
            {"Øre", "Øre|oyra|eyrir"},
            //Franc
            {
                "West African CFA franc",
                "west african cfa franc|xof|west africa cfa franc|west africa franc|west african franc"
            },
            {
                "Central African CFA franc",
                "central african cfa franc|xaf|central africa cfa franc|central african franc|central africa franc"
            },
            {"Comorian franc", "comorian franc|kmf"},
            {"Congolese franc", "congolese franc|cdf"},
            {"Burundian franc", "burundian franc|bif"},
            {"Djiboutian franc", "djiboutian franc|djf"},
            {"CFP franc", "cfp franc|xpf"},
            {"Guinean franc", "guinean franc|gnf"},
            {"Swiss franc", "swiss francs|swiss franc|chf|sfr."},
            {"Rwandan franc", "Rwandan franc|rwf|rf|r₣|frw"},
            {"Belgian franc", "belgian franc|bi.|b.fr.|bef|belgium franc"},
            {"Rappen", "rappen|-rappen"},
            {"Franc", "francs|franc|fr.|fs"},
            {"Centime", "centimes|centime|santim"},
            //Ruble
            {"Russian ruble", "russian ruble|₽|rub|russia ruble|russia ₽|russian ₽|russian rubles|russia rubles"},
            {
                "New Belarusian ruble",
                "new belarusian ruble|byn|new belarus ruble|new belarus rubles|new belarusian rubles"
            },
            {
                "Old Belarusian ruble",
                "old belarusian ruble|byr|old belarus ruble|old belarus rubles|old belarusian rubles"
            },
            {"Transnistrian ruble", "transnistrian ruble|prb|р."},
            {"Belarusian ruble", "belarusian ruble|belarus ruble|belarus rubles|belarusian rubles"},
            {"Kopek", "kopek|kopeks"},
            {"Kapyeyka", "kapyeyka"},
            {"Ruble", "rubles|ruble|br"},
            //Dinar
            {"Algerian dinar", "algerian dinar|د.ج|dzd|algerian dinars|algeria dinar|algeria dinars"},
            {"Bahraini dinar", "bahraini dinars|bahraini dinar|bhd|.د.ب"},
            {"Santeem", "santeem|santeems"},
            {"Iraqi dinar", "iraqi dinars|iraqi dinar|iraq dinars|iraq dinar|iqd|ع.د"},
            {"Jordanian dinar", "jordanian dinars|jordanian dinar|د.ا|jod|jordan dinar|jordan dinars"},
            {"Kuwaiti dinar", "kuwaiti dinars|kuwaiti dinar|kwd|د.ك"},
            {"Libyan dinar", "libyan dinars|libyan dinar|libya dinars|libya dinar|lyd"},
            {"Serbian dinar", "serbian dinars|serbian dinar|din.|rsd|дин.|serbia dinars|serbia dinar"},
            {"Tunisian dinar", "tunisian dinars|tunisian dinar|tnd|tunisia dinars|tunisia dinar"},
            {"Yugoslav dinar", "yugoslav dinars|yugoslav dinar|yun"},
            {"Dinar", "dinars|dinar|denar|-dinars|-dinar"},
            {"Fils", "fils|fulūs|-fils|-fil"},
            {"Para", "para|napa"},
            {"Millime", "millimes|millime"},
            //Peso
            {"Argentine peso", "argentine peso|ars|argetina peso|argetina pesos|argentine pesos"},
            {"Chilean peso", "chilean pesos|chilean peso|clp|chile peso|chile peso"},
            {"Colombian peso", "colombian pesos|colombian peso|cop|colombia peso|colombia pesos"},
            {
                "Cuban convertible peso",
                "cuban convertible pesos|cuban convertible peso|cuc|cuba convertible pesos|cuba convertible peso"
            },
            {"Cuban peso", "cuban pesos|cuban peso|cup|cuba pesos|cuba peso"},
            {"Dominican peso", "dominican pesos|dominican peso|dop|dominica pesos|dominica peso"},
            {"Mexican peso", "mexican pesos|mexican peso|mxn|mexico pesos|mexico peso"},
            {"Philippine peso", "piso|philippine pesos|philippine peso|₱|php"},
            {"Uruguayan peso", "uruguayan pesos|uruguayan peso|uyu"},
            {"Peso", "pesos|peso"},
            {"Centavo", "centavos|centavo"},
            //Pound
            {"Alderney pound", "alderney pounds|alderney pound|alderney £"},
            {
                "British pound",
                "british pounds|british pound|british £|gbp|pound sterling|pound sterlings|sterling|pound scot|pound scots"
            },
            {"Guernsey pound", "guernsey pounds|guernsey £|ggp"},
            {"Ascension pound", "ascension pounds|ascension pound|ascension £"},
            {"Saint Helena pound", "saint helena pounds|saint helena pound|saint helena £|shp"},
            {"Egyptian pound", "egyptian pounds|egyptian pound|egyptian £|egp|ج.م|egypt pounds|egypt pound"},
            {
                "Falkland Islands pound",
                "falkland islands pounds|falkland islands pound|falkland islands £|fkp|falkland island pounds|falkland island pound|falkland island £"
            },
            {"Gibraltar pound", "gibraltar pounds|gibraltar pound|gibraltar £|gip"},
            {"Manx pound", "manx pounds|manx pound|manx £|imp"},
            {"Jersey pound", "jersey pounds|jersey pound|jersey £|jep"},
            {
                "Lebanese pound",
                "lebanese pounds|lebanese pound|lebanese £|lebanan pounds|lebanan pound|lebanan £|lbp|ل.ل"
            },
            {
                "South Georgia and the South Sandwich Islands pound",
                "south georgia and the south sandwich islands pounds|south georgia and the south sandwich islands pound|south georgia and the south sandwich islands £"
            },
            {
                "South Sudanese pound",
                "south sudanese pounds|south sudanese pound|south sudanese £|ssp|south sudan pounds|south sudan pound|south sudan £"
            },
            {"Sudanese pound", "sudanese pounds|sudanese pound|sudanese £|ج.س.|sdg|sudan pounds|sudan pound|sudan £"},
            {"Syrian pound", "syrian pounds|syrian pound|syrian £|ل.س|syp|syria pounds|syria pound|syria £"},
            {"Tristan da Cunha pound", "tristan da cunha pounds|tristan da cunha pound|tristan da cunha £"},
            {"Pound", "pounds|pound|-pounds|-pound|£"},
            {"Pence", "pence"},
            {"Shilling", "shillings|shilling|shilingi"},
            {"Penny", "pennies|penny"},
            //Dollar
            {
                "United States dollar",
                "united states dollars|united states dollar|united states $|u.s. dollars|u.s. dollar|u s dollar|u s dollars|usd|american dollars|american dollar|us$|us dollar|us dollars|u.s dollar|u.s dollars"
            },
            {"East Caribbean dollar", "east caribbean dollars|east caribbean dollar|east Caribbean $|xcd"},
            {
                "Australian dollar",
                "australian dollars|australian dollar|australian $|australian$|aud|australia dollars|australia dollar|australia $|australia$"
            },
            {
                "Bahamian dollar",
                "bahamian dollars|bahamian dollar|bahamian $|bahamian$|bsd|bahamia dollars|bahamia dollar|bahamia $|bahamia$"
            },
            {"Barbadian dollar", "barbadian dollars|barbadian dollar|barbadian $|bbd"},
            {"Belize dollar", "belize dollars|belize dollar|belize $|bzd"},
            {
                "Bermudian dollar",
                "bermudian dollars|bermudian dollar|bermudian $|bmd|bermudia dollars|bermudia dollar|bermudia $"
            },
            {
                "British Virgin Islands dollar",
                "british virgin islands dollars|british virgin islands dollar|british virgin islands $|bvi$|virgin islands dollars|virgin islands dolalr|virgin islands $|virgin island dollars|virgin island dollar|virgin island $"
            },
            {"Brunei dollar", "brunei dollar|brunei $|bnd"},
            {"Sen", "sen"},
            {"Singapore dollar", "singapore dollars|singapore dollar|singapore $|s$|sgd"},
            {
                "Canadian dollar",
                "canadian dollars|canadian dollar|canadian $|cad|can$|c$|canada dollars|canada dolllar|canada $"
            },
            {
                "Cayman Islands dollar",
                "cayman islands dollars|cayman islands dollar|cayman islands $|kyd|ci$|cayman island dollar|cayman island doolars|cayman island $"
            },
            {"New Zealand dollar", "new zealand dollars|new zealand dollar|new zealand $|nz$|nzd|kiwi"},
            {
                "Cook Islands dollar",
                "cook islands dollars|cook islands dollar|cook islands $|cook island dollars|cook island dollar|cook island $"
            },
            {"Fijian dollar", "fijian dollars|fijian dollar|fijian $|fjd|fiji dollars|fiji dollar|fiji $"},
            {"Guyanese dollar", "guyanese dollars|guyanese dollar|gyd|gy$"},
            {
                "Hong Kong dollar",
                "hong kong dollars|hong kong dollar|hong kong $|hk$|hkd|hk dollars|hk dollar|hk $|hongkong$"
            },
            {
                "Jamaican dollar",
                "jamaican dollars|jamaican dollar|jamaican $|j$|jamaica dollars|jamaica dollar|jamaica $|jmd"
            },
            {"Kiribati dollar", "kiribati dollars|kiribati dollar|kiribati $"},
            {"Liberian dollar", "liberian dollars|liberian dollar|liberian $|liberia dollars|liberia dollar|liberia $|lrd"},
            {"Micronesian dollar", "micronesian dollars|micronesian dollar|micronesian $"},
            {
                "Namibian dollar",
                "namibian dollars|namibian dollar|namibian $|nad|n$|namibia dollars|namibia dollar|namibia $"
            },
            {"Nauruan dollar", "nauruan dollars|nauruan dollar|nauruan $"},
            {"Niue dollar", "niue dollars|niue dollar|niue $"},
            {"Palauan dollar", "palauan dollars|palauan dollar|palauan $"},
            {
                "Pitcairn Islands dollar",
                "pitcairn islands dollars|pitcairn islands dollar|pitcairn islands $|pitcairn island dollars|pitcairn island dollar|pitcairn island $"
            },
            {
                "Solomon Islands dollar",
                "solomon islands dollars|solomon islands dollar|solomon islands $|si$|sbd|solomon island dollars|solomon island dollar|solomon island $"
            },
            {"Surinamese dollar", "surinamese dollars|surinamese dollar|surinamese $|srd"},
            {"New Taiwan dollar", "new taiwan dollars|new taiwan dollar|nt$|twd|ntd"},
            {
                "Trinidad and Tobago dollar",
                "trinidad and tobago dollars|trinidad and tobago dollar|trinidad and tobago $|trinidad $|trinidad dollar|trinidad dollars|trinidadian dollar|trinidadian dollars|trinidadian $|ttd"
            },
            {"Tuvaluan dollar", "tuvaluan dollars|tuvaluan dollar|tuvaluan $"},
            {"Dollar", "dollars|dollar|$"},
            //Chinese yuan
            {"Chinese yuan", "yuan|chinese yuan|renminbi|cny|rmb|￥"},
            {"Fen", "fen"},
            {"Jiao", "jiao"},
            //Additional
            {"Finnish markka", "suomen markka|finnish markka|finsk mark|fim|markkaa|markka"},
            {"Penni", "penniä|penni"}
        }.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> CurrencyPrefixList = new Dictionary<string, string>
        {
            //Dollar Prefix
            {"Dollar", "$"},
            {"United States dollar", "united states $|us$|us $|u.s. $|u.s $"},
            {"East Caribbean dollar", "east caribbean $"},
            {"Australian dollar", "australian $|australia $"},
            {"Bahamian dollar", "bahamian $|bahamia $"},
            {"Barbadian dollar", "barbadian $|barbadin $"},
            {"Belize dollar", "belize $"},
            {"Bermudian dollar", "bermudian $"},
            {
                "British Virgin Islands dollar",
                "british virgin islands $|bvi$|virgin islands $|virgin island $|british virgin island $"
            },
            {"Brunei dollar", "brunei $|b$"},
            {"Sen", "sen"},
            {"Singapore dollar", "singapore $|s$"},
            {"Canadian dollar", "canadian $|can$|c$|c $|canada $"},
            {"Cayman Islands dollar", "cayman islands $|ci$|cayman island $"},
            {"New Zealand dollar", "new zealand $|nz$|nz $"},
            {"Cook Islands dollar", "cook islands $|cook island $"},
            {"Fijian dollar", "fijian $|fiji $"},
            {"Guyanese dollar", "gy$|gy $|g$|g $"},
            {"Hong Kong dollar", "hong kong $|hk$|hkd|hk $"},
            {"Jamaican dollar", "jamaican $|j$|jamaica $"},
            {"Kiribati dollar", "kiribati $"},
            {"Liberian dollar", "liberian $|liberia $"},
            {"Micronesian dollar", "micronesian $"},
            {"Namibian dollar", "namibian $|nad|n$|namibia $"},
            {"Nauruan dollar", "nauruan $"},
            {"Niue dollar", "niue $"},
            {"Palauan dollar", "palauan $"},
            {"Pitcairn Islands dollar", "pitcairn islands $|pitcairn island $"},
            {"Solomon Islands dollar", "solomon islands $|si$|si $|solomon island $"},
            {"Surinamese dollar", "surinamese $|surinam $"},
            {"New Taiwan dollar", "nt$|nt $"},
            {"Trinidad and Tobago dollar", "trinidad and tobago $|trinidad $|trinidadian $"},
            {"Tuvaluan dollar", "tuvaluan $"},
            {"Samoan tālā", "ws$"},
            {"Chinese yuan", "￥"},
            {"Japanese yen", "¥"},
            {"Euro", "€"},
            {"Pound", "£"},
            {"Costa Rican colón", "₡"},
            {"Turkish lira", "₺"}
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "din.",
            "kiwi",
            "kina",
            "kobo",
            "lari",
            "lipa",
            "napa",
            "para",
            "sfr.",
            "taka",
            "tala",
            "toea",
            "vatu",
            "yuan",
            "ang",
            "ban",
            "bob",
            "btn",
            "byr",
            "cad",
            "cop",
            "cup",
            "dop",
            "gip",
            "jod",
            "kgs",
            "lak",
            "lei",
            "mga",
            "mop",
            "nad",
            "omr",
            "pul",
            "sar",
            "sbd",
            "scr",
            "sdg",
            "sek",
            "sen",
            "sol",
            "sos",
            "std",
            "try",
            "yer",
            "yen",
        }.ToImmutableList();
    }
}
