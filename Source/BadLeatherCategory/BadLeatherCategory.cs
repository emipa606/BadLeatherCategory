﻿using System.Linq;
using RimWorld;
using Verse;

namespace BadLeatherCategory;

[StaticConstructorOnStartup]
public class BadLeatherCategory
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
        foreach (var raceDef in from raceDef in DefDatabase<ThingDef>.AllDefsListForReading
                 where raceDef.race is { Humanlike: true }
                 select raceDef)
        {
            if (raceDef.race.leatherDef == null)
            {
                //Log.Message($"{raceDef} has no leather");
                continue;
            }

            if (leatherBadCategory.childThingDefs.Contains(raceDef.race.leatherDef))
            {
                //Log.Message($"{raceDef.race.leatherDef} already moved");
                continue;
            }

            var nonHumanlikes = from race in DefDatabase<ThingDef>.AllDefsListForReading
                where race.race is { Humanlike: false } && race.race.leatherDef == raceDef.race.leatherDef
                select race;
            if (nonHumanlikes.Any())
            {
                Log.Message(
                    $"[BadLeatherCategory]: Note that {raceDef.race.leatherDef} is also used by non-humanlikes. {string.Join(", ", nonHumanlikes)}");
            }

            //Log.Message($"Adding {raceDef.race.leatherDef} from {raceDef} as bad");
            leathersCategory.childThingDefs.Remove(raceDef.race.leatherDef);
            raceDef.race.leatherDef.thingCategories.Remove(leathersCategory);
            leatherBadCategory.childThingDefs.Add(raceDef.race.leatherDef);
            raceDef.race.leatherDef.thingCategories.Add(leatherBadCategory);
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