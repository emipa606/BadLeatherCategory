using System.Linq;
using RimWorld;
using Verse;

namespace BadLeatherCategory;

[StaticConstructorOnStartup]
public static class BadLeatherCategory
{
    static BadLeatherCategory()
    {
        var leathersCategory = DefDatabase<ThingCategoryDef>.GetNamedSilentFail("Leathers");
        if (leathersCategory == null)
        {
            Log.ErrorOnce("[BadLeatherCategory]: Could not find the Leathers-category. Will not sort bad leather.",
                "LeathersCategory".GetHashCode());
            return;
        }

        var leatherBadCategory = DefDatabase<ThingCategoryDef>.GetNamedSilentFail("LeatherBad");
        if (leatherBadCategory == null)
        {
            Log.ErrorOnce(
                "[BadLeatherCategory]: Could not find the LeatherBad-category. Will not sort bad leather.",
                "LeatherBadCategory".GetHashCode());
            return;
        }

        var counter = 0;
        foreach (var race in from raceDef in DefDatabase<ThingDef>.AllDefsListForReading
                 where raceDef.race is { Humanlike: true }
                 select raceDef.race)
        {
            if (race.leatherDef == null)
            {
                continue;
            }

            if (leatherBadCategory.childThingDefs.Contains(race.leatherDef))
            {
                continue;
            }

            var nonHumanlikes = from leatherRace in DefDatabase<ThingDef>.AllDefsListForReading
                where leatherRace.race is { Humanlike: false } && leatherRace.race.leatherDef == race.leatherDef
                select leatherRace;
            if (nonHumanlikes.Any())
            {
                Log.Message(
                    $"[BadLeatherCategory]: Note that {race.leatherDef} is also used by non-humanlikes. {string.Join(", ", nonHumanlikes)}");
            }

            leathersCategory.childThingDefs.Remove(race.leatherDef);
            race.leatherDef.thingCategories.Remove(leathersCategory);
            leatherBadCategory.childThingDefs.Add(race.leatherDef);
            race.leatherDef.thingCategories.Add(leatherBadCategory);
            counter++;
        }

        if (ModLister.AnomalyInstalled)
        {
            leathersCategory.childThingDefs.Remove(ThingDefOf.Leather_Dread);
            ThingDefOf.Leather_Dread.thingCategories.Remove(leathersCategory);
            leatherBadCategory.childThingDefs.Add(ThingDefOf.Leather_Dread);
            ThingDefOf.Leather_Dread.thingCategories.Add(leatherBadCategory);
            counter++;
        }

        if (counter == 0)
        {
            Log.Message(
                "[BadLeatherCategory]: Found no leather used by humanlikes to move to the Bad Leather-category");
            return;
        }

        leathersCategory.ClearCachedData();
        leatherBadCategory.ClearCachedData();
        leathersCategory.ResolveReferences();
        leatherBadCategory.ResolveReferences();
        Log.Message($"[BadLeatherCategory]: Moved {counter} leather to the Bad Leather-category");
    }
}