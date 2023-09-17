using Kitchen;
using KitchenData;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenFireEvent
{
    public struct CEndOfDayCost : IApplianceProperty, IAttachableProperty, IComponentData, IModComponent
    {
        public int Amount;
    }

    [UpdateBefore(typeof(DestroyAppliancesAtNight))]
    public class ChargeOvernight : StartOfNightSystem, IModSystem
    {
        EntityQuery Charges;

        protected override void Initialise()
        {
            base.Initialise();
            Charges = GetEntityQuery(typeof(CEndOfDayCost));
        }

        protected override void OnUpdate()
        {
            if (Charges.IsEmpty || GetOrDefault<SDay>().Day < 1 || !Require(out SMoney money) || Main.PrefManager.Get<int>("messCleaningFee") == 0 )
                return;

            using NativeArray<CEndOfDayCost> charges = Charges.ToComponentDataArray<CEndOfDayCost>(Allocator.Temp);

            int totalCost = 0;
            for (int i = 0; i < charges.Length; i++)
            {
                CEndOfDayCost charge = charges[i];
                totalCost += charge.Amount * Main.PrefManager.Get<int>(Main.MESS_CLEANING_FEE);
            }
            money.Amount -= totalCost;
            Set(money);
        }
    }
}
