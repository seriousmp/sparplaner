using System.Globalization;
using sparplaner;
var manager = new SparzielManager();
manager.Laden();
var kultur = new CultureInfo("de-DE");
bool laeuft = true;
while (laeuft)