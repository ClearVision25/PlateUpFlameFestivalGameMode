using Kitchen;
using KitchenMods;
using System;
using Unity.Entities;

namespace KitchenFireEvent
{
    [UpdateBefore(typeof(DestroyAppliancesAtNight))]
    public class BonusForCleaning : StartOfNightSystem, IModSystem
    {
        EntityQuery Messes;

        protected override void Initialise()
        {
            base.Initialise();
            Messes = GetEntityQuery(typeof(CMess));
        }

        protected override void OnUpdate()
        {
            if (!Require(out SDay sDay) || sDay.Day < 1 || !Require(out SMoney money) || Main.PrefManager.Get<int>("messCleaningBonus") == 0)
                return;

            int messCount = Messes.CalculateEntityCount();
            int CLEAN_BONUS = Main.PrefManager.Get<int>(Main.MESS_CLEANING_BONUS);

            if (messCount == 0)
            {
                money.Amount += CLEAN_BONUS;
                Set(money);
            }

        }
    }

}

