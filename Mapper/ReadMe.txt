
Globale parameter inne i Js templaten

console : Standard system.io.textwriter
	Brukes til å skrive meldinger til consolet. Kjekt til debugging.
result: 
	PushTab() 
		Legger til en tab
	PopTab()	
		Fjerner en tab
	WriteComment(s As String)
		Skriver en kommentar i koden
	Comment(s As String)
		Skriver en kommentar i koden
	WriteProp(name As String, type As String, defaultValue As String)
		Skriver en standard property 
	WriteReadOnlyProp(name As String, type As String, defaultValue As String, modifier As String)
		Skriver en readonly property
			
	WriteTab(count As Integer)
		Skriver count antall taber i koden filen
	WriteTabs()
		Skriver det antall tab'er som har blitt pushet
	WriteTab()
		Skriver 1 tab

	StartBlock(s As String)
		Skriver 1 linje med kode og legger til en tab 
	StartBlock(s As String, ParamArray value As String())
		Skriver 1 linje med kode og legger til en tab. Bruker string.format

	EndBlock(s As String)
		Skriver en linje med kode PopTab()

	//Obsolete
	WriteFormatLine(s As String, ParamArray value As String())
	//Obsolete
	WriteFormat(s As String, ParamArray value As String())
	
	WriteLine(s As String, ParamArray value As String())
		Skriver en linje med kode. Bruker string.format.
		ex: result.WriteLine("Dette er {0}",["abc"])

	WriteLine(s As String)
		Skriver en linje med kode.
	Write(s As String, ParamArray value As String())
		Skriver kode med string.format. Uten linjeskift
	Write(s As String)
		Skriver kode uten linjeskift

data: Dette er infoen som ligger i *.tablesettings filen. Se denne filen

cols: Kolonnene fra data.Table.Columns uten de som er lagt inn i ExcludeCols i tablesettings

options: Egene settings som du kan sette inn i tablesettings options

 _ : Underscore bibliotek.
 Utils: Egenutviklet bibliotek 
		toParamName(s)  setter første bokstave i s til lowerCase


