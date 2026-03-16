namespace WorldChampionships2025Analyzer.Static_data
{
	public class StringComparisonDataSets
	{
		public StringComparisonDataSets() { }

		public static HashSet<string> CountrySet => new(StringComparer.OrdinalIgnoreCase)
		{
			"CANADA","UNITED STATES","USA","ENGLAND","SCOTLAND","UKRAINE","POLAND","SWEDEN","PARAGUAY","WALES",
			"GERMANY","NORWAY","FINLAND","IRELAND","SPAIN","ITALY","FRANCE","KOREA","ARGENTINA","COLOMBIA","BOSNIA AND HERZEGOVINA",
			"BRAZIL","MEXICO","AUSTRALIA","NEW ZEALAND","NETHERLANDS","BELGIUM","CZECH REPUBLIC", "SLOVENIA", "CROATIA","JAPAN",
			"SLOVAKIA","HUNGARY","ROMANIA","BULGARIA","GEORGIA","KAZAKHSTAN","UZBEKISTAN","MOLDOVA", "MALAYSIA", "CHINESE TAIPEI", "CHILE", "INDIVIDUAL NEUTRAL ATHLETES"
		};

		public static HashSet<string> NameSet
		{
			get
			{
				var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				set.UnionWith(SeniorCompetitorsNameSet);
				set.UnionWith(JuniorCompetitorsNameSet);
				set.UnionWith(PreJuniorCompetitorsNameSet);
				return set;
			}
		}

		public static HashSet<string> SeniorCompetitorsNameSet => new(StringComparer.OrdinalIgnoreCase)
		{
			"Murillo Diego","Spina Augusto","Toyama Sogo","Lema Efrata","Tolstogan Valeriu","Kiss Levente","Pena Gonzalo",
			"Thapa Magar Niresh","Lin Chun Kit","Almeida Lautaro","Bernstein Daniel","Talhaoui Kabil Yassine","Martin Finn",
			"Linero Cruz Javier","Mcavoy Declan","Looman Zachary","Zuger Andreas","Kheng Xing Ooi","Ghosal Alexander","Mujanovic Dino",
			"Lewis Caleb","Blom Michael","Jian Yang Wong","Donde Gonzalo","Burton Joshua","Franke Matthias","Brooks Spencer","Alexander Nielsen Tristan",
			"Dancu Ioan Marius","David Mario","Cotey Bradley","Bin Bahrin Muhammad Aidil","Lindsay Akino Jordan","Falowski Kamil","Chhantyal Chamindra Bahadur",
			"Chvojka Daniel","Nicoara Radu","Mrozicki Mateusz","Neary Sean","Blicharski Maciej","Cek Andrea","Jerinic Njegos","Wahlberg Nils","Veiga Juan Pedro",
			"Vitiello Tommaso","Hansson Anton","Stige David","Lehti Janita","Bahamondes Scarlett","Burzanska Hanna","Ying Er Lee","Lin Yu-Shih","Oro Veronica",
			"Berggren Amanda","Childs Amy","Iversen Stine Hauge","Dzehovic Ajsa","Manzhos Diana","Griffith Iveta","Feley Deena","Bartus Monika","Cancedda Eleonora",
			"Kumal Niraula Sita","Atencia Aranda Ainara","Yadav Shobha","Tram Maika","Labrie Yra","Ahlskog Hou Li-Lian","Groenewald Cara","Paulo-Desmarais Sophie",
			"Wealleans Alyssa","Savitskaya Hanna","Dearmas Patricia","Berle Aurora Dønheim","Vargas Jimena","Altamirano Maria Agustina","Czuchan Patrycja","Delrock Sam",
			"Lobley Ellena","Naszer Julia Rahel","Trotter Brenda","Angarita Lucy Katherine","Sjoberg Ellen","Pecchielan Linda","Franks Christine","Gheorghita Roxana Maria",
			"Checcucci Valeria","Prunkl Jordan","Lehane Sarah","Raman Safae","Ohare Megan","Pol Palacios Valeria","Cobo Victoria","Salja Wyon Emma","Rivera Beltran Juanita",
			"Delgado Camila","Frontali Tommaso","Huang Yen-Zhen","Dearmas Gabriel","Okeric Tarik","Lucic Marin","Krauchanka Andrei","Oudolnyi Vadim","Chhantyal Begam",
			"Vogler Gideon","Loodtz Theo Lagreid","Doniesiewicz Szymon","Verbraeken Kilan","Vergara Marcelo","Amaleethan Erwin","Dariozzi Thiago","Stirling Reece",
			"Milutinovic Pavle","Tuite Shane","Haienko Artem","Nigmatullaev Nodirbek","Szabo Janos Zoltan","Alemu Seyfemichael","Vistal Alexej","Triqui Anis",
			"Dreglea Stefan","Rueda Rodrigo","Payton Harry","Tomas Jakub","Norén Anton","Odonnell Sean","Tuite James","Capolongo Samuele","Sosa Felipe","Zadina Oliver",
			"Bin Azmi Muhammad Danial","Mirzoev Jahongir","Ellis Lachlan","Oneil Sean","Tesfa Nebiyu","Rmadan Ahmed","Eckhardt Jason","Chavarria Medina Matias","Giri Sajan",
			"Van De Veerdonk Chris","Tomescu Alexandru Ioan","Rosato Damiano","Blicharski Marcin","Stupenko Roman","Lobban Jack","Leberl Zack","Lenz Tobias","Cuellar Jimmy",
			"Skvor Daniel","Moriarty Luke","Gonzalez Martos Francisco Rafael","Helin Onni","Vacariu Vlad","Dusard Nicholos Marlon","Turner Lucas","Laplante Christophe",
			"Valdez Fabian","Balboa Cristian","Ismailov Zelimhan","Castelnuovo Alessandro","Stone Richard Lloyd","Trinder James","Svraka Faris","Carracedo Trigo Manuel",
			"Turner Benjamin","Baila Alexandru Ioan","Kiviranta Tino","Sierszchulski Kamil","Moir Lewis","Helamb Adam","Horbul Yevhenii","Bien Bartosz","Cacciatore Gianfranco",
			"Vomiero Stefano","Pedersen Havar","Tarasenko Victoria","Wessels Isabella","Labrie Pia","Pender Kate","Devine Sydney","Kovaliova Veronika","Pryimak Lidiia",
			"Henwood Holly","Stridbeck Isabella","Zagar Slemensek Larisa","Mcnair Amy","Canut Camila","Cehajic Lamija","Waddell Hollie","Miller Hanna","Van Veen Helena",
			"Dilmurodova Sevara","Kosmowska Maja","Kamenetzki Daniel","Esteche Tobias","Gozzano Rodrigo","Mitchell Connor","Lypovetskiy Denys","Hassanipour Daniel","Hatta Ginryu",
			"Cammarota Antonio","Vlahovic Marko","Rueda Hernan","Schuddinck Rune","Hong Wenquan","Turner Ryan","Taylor Thomas","Larkin David","Talhaoui Kabil Bahaa",
			"Nishio Issa","Mcdermott Kieran","Stirbu Luca","Hnatko Danylo","Zagoranski Ziga","Sodikov Abduazim","Montero Miguel","Ferreyra Walter","Vilchez Lopez Jaime",
			"Jara Dennis","Hubert Will","Odlow Jon","Holroyd Mason","Chumachenko Nazar","Gingras Marie-Christine","Flores Sabrina","Hemmer Ana","Vukic Ana","Martinez Ester",
			"Listhaug Jenny Marvel","Pasanen Py","Vandenbrink Makenna","Karavaieva Anhelina","Duffour Florencia","Dixon Hannah","Runesson Ella","Rosenbaum Joanne Theresia Liz",
			"Hodnik Lourdes","Johansen Sofie Haugsleett","Semb Helene","Diefenbach Fenne","Janiszewska Katarzyna","Mukha Iryna","Wallace Rachel","Pelkey Serena","Swinburne Jemma",
			"Ulmasova Robiya","Cornea Andreea Casandra","Krucayova Teresa","Smith Taylor","Moriya Naomi","Mazzanti Elena","Bortlikova Eliska","Mettinen Nita","Osullivan Sarah",
			"Bouymejjan Lina","Luniewska Karolina","Barada Tyra","Hovelius Maria","Hanisch Wioletta","Palavecino Mailen","Ilic Ivana","Yates Cassandra","Martinez Chiara",
			"Van Der Veer Myrthe","Cadelano Stefano","Balachandran Vimal Nair","Rizzo Lautaro","Tamang Dilba","Rinchinov Altan","Hubbuck Iden","Baratov Azamat","Rannou Victor",
			"Shibasaki Takashi","Giuliano Simone","G. Emond Steve","Zsarko Daniel Peter","Garcia Nahuel Ivan","Gudyma Roman","Filippidis Filippos","Balázs Ervin","Salja Wyon Adis",
			"Alijagic Emir","Babrovich Ivan","Gomez De Olivera Thiago","Barenbaum Amit","Lingers Kayden Clyde","Olejar Samuel","Stirling Abigail","Palfi Boroka","Pampiglioni Indira",
			"Brider Isabel","Tome Nieminen Laura","Korolovych Daryna","Mcintyre Hannah","Bordiez Bianca","Rega Eliska","Ekblom Alice","Mierkiewicz Zuzanna","Gomez Esperanza",
			"Kleckova Klara","Varela Rodriguez Alba","Woodrow Anikah","Vogt Georgia","Gordionok Evelina","Kowalczyk Monika","Macrury Serena","Jilkova Barbora","Tamas Boglarka",
			"Postu Claudia Teofana","Vali Samarah","Tarasenko Davyd","Belmonte Gil Nicolas","Greene-Ryding Darren","Baila Teodor Mihai","Sheng Xihan","Gonzalez Matias","Gurung Sajan",
			"Feeley Lucas","Millan Benitez Juan Sebastian","Daverio Valentino","Nicolucci Mateo","Abe Susumu","Oranje Cody","Ulvinen Leevi","Downey Alvaro","Maggioni Mauricio",
			"Cox Darryl","Croft David","Cuellar William","Maggs Dylan","Johansson Santhe","Kruk Sebastian","Fujimoto Naoto","Valkov Egor Andreevich","Jeffery Bailey","Stender Lucas",
			"Traslavina Diego","Boyens Maximilien","Alilou Aymane","Kruk Kamil","Iversen Marius Hauge","Nunez Baron","Gurung Manoj","Putseys Yannick","Saccomanno Lucas",
			"Spencer Ii Sheldon","Couture Francois","Bewa Jeremie","Pedersen Håvar","Bianchi Bruno","Ceaser Elora","Ayala-Trujillo Luna-Silvana","Lång Johanna","Kucharska Julia",
			"Pacheco Eva","Hopfe Danielle","Haakana Mia","Binti Kamaruzzaman Nur Aqilah","Rossi Sofia","Vendlands Edvards","Hubert Noah","Formanek David","Lewandowski Nikodem",
			"Shamil Yenus","Fogarty Thomas","Budhathoki Magar Janak","Valkov Daniil","Tirelli Lepore Andrea","Lagies Tristan","Kim Vladimir","Wealleans Keara","Wendykier Michelle",
			"Schmidt Victoria","Bordiez Aldana","Stetinova Eva","Reszka Klaudia","Vrinat Alice","Kanka Filippa","Beaudoin Caroline","Takahashi Mika","Makoviichuk Sofiia",
			"Leppanen Jasmin","Domene Valverde Laura","Amores Ivana","Shan Meng Hui","Becsi Nikolett","Fabian Eszter","Acosta Ailen","Moreno Pomares Ana Maria","Pigarova Anna",
			"Li Selma","Shibasaki Rio","Lagace Genevieve","Planko Veronika","Cichon Martyna","Singleton Jaimie","Reshetnikova Alena"
		};

		public static HashSet<string> JuniorCompetitorsNameSet => new(StringComparer.OrdinalIgnoreCase)
		{
			
		};

		public static HashSet<string> PreJuniorCompetitorsNameSet => new(StringComparer.OrdinalIgnoreCase)
		{

		};

		public static Dictionary<string, string> FederationSet => new(StringComparer.OrdinalIgnoreCase)
		{
			{ "FEDERAZIONE ITALIANA TAEKWON-DO ITF", "ITALY" },
			{ "INTERNATIONAL TAEKWON-DO FOUNDATION OF NEW ZEALAND", "NEW ZEALAND" },
			{ "CANADIAN TAEKWON-DO FEDERATION INTERNATIONAL", "CANADA" },
			{ "INTERNATIONAL TAEKWON-DO FEDERATION ENGLAND", "ENGLAND" },
			{ "CZECH NATIONAL TEAKWON-UNION", "CZECH REPUBLIC" },
			{ "INTERNATIONAL TAEKWON-DO FEDERATION SCOTLAND", "SCOTLAND" },
			{ "SPANISH FEDERATION OD TAEKWON-DO ITF", "SPAIN" },
			{ "HUNGARIAN ITF TAEKWON-DFEDERATION", "HUNGARY" },
			{ "FEDERATION OF NATIONALS TAEKWON-DO ORGANIZATIONS CHILE", "CHILE" },
			{ "FEDERATION OF TAEKWON-DO OF THE ARGENTINE REPUBLIC", "ARGENTINA" },
			{ "ROMANIAN TAEKWON-DO ITF FEDERATION", "ROMANIA" },
			{ "ASSOCIATION COLOMBIANA DE TAEKWON-DO", "COLOMBIA" },
		};

		// Short tokens that often come from flag/icon OCR and should be ignored if they precede a country
		public static HashSet<string> LikelyFlagNoise => new(StringComparer.OrdinalIgnoreCase)
		{
			"PE","P","PI","I","L","O","[]","|", ":", ".", "-", "·"
		};
	}
}
