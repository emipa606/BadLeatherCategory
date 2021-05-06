using System.Linq;
using Verse;

namespace BadLeatherCategory
{
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
                where raceDef.race != null && raceDef.race.Humanlike
                select raceDef)
            {
                if (raceDef.race.leatherDef == null)
                {
                    continue;
                }

                if (LeatherBadCategory.childThingDefs.Contains(raceDef.race.leatherDef))
                {
                    continue;
                }

                if ((from race in DefDatabase<ThingDef>.AllDefsListForReading
                    where race.race != null && !race.race.Humanlike && race.race.leatherDef == raceDef.race.leatherDef
                    select race).Any())
                {
                    continue;
                }

                LeathersCategory.childThingDefs.Remove(raceDef.race.leatherDef);
                raceDef.race.leatherDef.thingCategories.Remove(LeathersCategory);
                LeatherBadCategory.childThingDefs.Add(raceDef.race.leatherDef);
                raceDef.race.leatherDef.thingCategories.Add(LeatherBadCategory);
                counter++;
            }

            LeathersCategory.ClearCachedData();
            LeatherBadCategory.ClearCachedData();
            Log.Message($"[BadLeatherCategory]: Moved {counter} leather to the Bad Leather-category");
        }
    }
}