using System.Linq;
using Verse;

namespace BadLeatherCategory;

[StaticConstructorOnStartup]
public class BadLeatherCategory
{
    static BadLeatherCategory()
    {
        var LeathersCategory = DefDatabase<ThingCategoryDef>.GetNamedSilentFail("Leathers");
        if (LeathersCategory == null)
        {
            Log.ErrorOnce("[BadLeatherCategory]: Could not find the Leathers-category. Will not sort bad leather.",
                "LeathersCategory".GetHashCode());
            return;
        }

        var LeatherBadCategory = DefDatabase<ThingCategoryDef>.GetNamedSilentFail("LeatherBad");
        if (LeatherBadCategory == null)
        {
            Log.ErrorOnce(
                "[BadLeatherCategory]: Could not find the LeatherBad-category. Will not sort bad leather.",
                "LeatherBadCategory".GetHashCode());
            return;
        }

        var counter = 0;
        foreach (var raceDef in from raceDef in DefDatabase<ThingDef>.AllDefsListForReading
                 where raceDef.race is { Humanlike: true }
                 select raceDef)
        {
            if (raceDef.race.leatherDef == null)
            {
                //Log.Message($"{raceDef} has no leather");
                continue;
            }

            if (LeatherBadCategory.childThingDefs.Contains(raceDef.race.leatherDef))
            {
                //Log.Message($"{raceDef.race.leatherDef} already moved");
                continue;
            }

            if ((from race in DefDatabase<ThingDef>.AllDefsListForReading
                    where race.race is { Humanlike: false } && race.race.leatherDef == raceDef.race.leatherDef
                    select race).Any())
            {
                //Log.Message($"{raceDef.race.leatherDef} also used by non-humanlike pawns. Skipping");
                continue;
            }

            //Log.Message($"Addeing {raceDef.race.leatherDef} from {raceDef} as bad");
            LeathersCategory.childThingDefs.Remove(raceDef.race.leatherDef);
            raceDef.race.leatherDef.thingCategories.Remove(LeathersCategory);
            LeatherBadCategory.childThingDefs.Add(raceDef.race.leatherDef);
            raceDef.race.leatherDef.thingCategories.Add(LeatherBadCategory);
            counter++;
        }

        LeathersCategory.ClearCachedData();
        LeatherBadCategory.ClearCachedData();
        LeathersCategory.ResolveReferences();
        LeatherBadCategory.ResolveReferences();
        Log.Message($"[BadLeatherCategory]: Moved {counter} leather to the Bad Leather-category");
    }
}