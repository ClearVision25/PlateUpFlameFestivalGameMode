using Kitchen;
using KitchenData;
using KitchenLib.References;
using Unity.Collections;
using Unity.Entities;

namespace KitchenFireEvent
{
    internal class AddMessRestaurantStatus : RestaurantSystem
    {
        private bool SF_CustomApplied = false;
        private bool CM_CustomApplied = false;


        protected override void Initialise()
        {
            base.Initialise();
        }

        protected override void OnUpdate()
        {

            // Skip if Instant Service card taken
            if (Has<SIsDayFirstUpdate>() && Main.PrefManager.Get<bool>("messSpreadFurther") == true || Main.PrefManager.Get<bool>("messConstantSpawn") == true)
            {
                if (!HasStatus(RestaurantStatus.MessRangeIncrease) && Main.PrefManager.Get<bool>("messSpreadFurther") == true)
                {
                    // Set flag for effect was applied
                    SF_CustomApplied = true;
                    AddStatus(RestaurantStatus.MessRangeIncrease);

                }

                if (!HasStatus(RestaurantStatus.HalloweenTrickConstantMess) && Main.PrefManager.Get<bool>("messConstantSpawn") == true)
                {
                    // Set flag for effect was applied
                    CM_CustomApplied = true;
                    AddStatus(RestaurantStatus.HalloweenTrickConstantMess);

                }

                return;
            }

            // If effect is applied at start of day, reset
            if (Has<SIsDayFirstUpdate>())
            {
                if (HasStatus(RestaurantStatus.MessRangeIncrease) && SF_CustomApplied && Main.PrefManager.Get<bool>("messSpreadFurther") == false)
                {
                    // Set flag for effect was applied
                    SF_CustomApplied = false;
                    RemoveStatus(RestaurantStatus.MessRangeIncrease);

                }

                if (HasStatus(RestaurantStatus.HalloweenTrickConstantMess) && CM_CustomApplied && Main.PrefManager.Get<bool>("messConstantSpawn") == false)
                {
                    // Set flag for effect was applied
                    CM_CustomApplied = false;
                    RemoveStatus(RestaurantStatus.HalloweenTrickConstantMess);

                }
            }
        }
    }
}