using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(LittleBuddy.Main.Description)]
[assembly: AssemblyDescription(LittleBuddy.Main.Description)]
[assembly: AssemblyCompany(LittleBuddy.Main.Company)]
[assembly: AssemblyProduct(LittleBuddy.Main.Name)]
[assembly: AssemblyCopyright("Developed by " + LittleBuddy.Main.Author)]
[assembly: AssemblyTrademark(LittleBuddy.Main.Company)]
[assembly: AssemblyVersion(LittleBuddy.Main.Version)]
[assembly: AssemblyFileVersion(LittleBuddy.Main.Version)]
[assembly: MelonInfo(typeof(LittleBuddy.Main), LittleBuddy.Main.Name, LittleBuddy.Main.Version, LittleBuddy.Main.Author, LittleBuddy.Main.DownloadLink)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]