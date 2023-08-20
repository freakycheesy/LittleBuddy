using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(LittleBuddy.BuildInfo.Description)]
[assembly: AssemblyDescription(LittleBuddy.BuildInfo.Description)]
[assembly: AssemblyCompany(LittleBuddy.BuildInfo.Company)]
[assembly: AssemblyProduct(LittleBuddy.BuildInfo.Name)]
[assembly: AssemblyCopyright("Developed by CarrionAndon" + LittleBuddy.BuildInfo.Author)]
[assembly: AssemblyTrademark(LittleBuddy.BuildInfo.Company)]
[assembly: AssemblyVersion(LittleBuddy.BuildInfo.Version)]
[assembly: AssemblyFileVersion(LittleBuddy.BuildInfo.Version)]
[assembly: MelonInfo(typeof(LittleBuddy.BuildInfo), LittleBuddy.BuildInfo.Name, LittleBuddy.BuildInfo.Version, LittleBuddy.BuildInfo.Author, LittleBuddy.BuildInfo.DownloadLink)]
[assembly: MelonColor(System.ConsoleColor.White)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]