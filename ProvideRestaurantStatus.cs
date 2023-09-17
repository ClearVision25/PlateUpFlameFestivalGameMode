using Kitchen;
using KitchenData;
using KitchenLib.References;
using Unity.Collections;
using Unity.Entities;

namespace KitchenFireEvent
{
    internal class AddRestaurantStatus : RestaurantSystem
    {
        private bool IS_CustomApplied = false;
        private bool TC_CustomApplied = false;


        protected override void Initialise()
        {
            base.Initialise();
            RequireSingletonForUpdate<SFireScore>();
            RequireSingletonForUpdate<SFireGameActive>();
        }

        protected override void OnUpdate()
        {

            SFireScore fireScore = GetSingleton<SFireScore>();


            // Skip if Instant Service card taken
            if (Has<SIsDayFirstUpdate>() && fireScore.GameMode == FireGameMode.CoinCollection)
            {
                if (!HasStatus(RestaurantStatus.CustomersOrderImmediately) && Main.PrefManager.Get<bool>("autoAddTippingCultureInstantService") == true)
                {
                    // Set flag for effect was applied
                    IS_CustomApplied = true;
                    AddStatus(RestaurantStatus.CustomersOrderImmediately);

                }

                if (!HasStatus(RestaurantStatus.PayBasedOnPatience) && Main.PrefManager.Get<bool>("autoAddTippingCultureInstantService") == true)
                {
                    // Set flag for effect was applied
                    TC_CustomApplied = true;
                    AddStatus(RestaurantStatus.PayBasedOnPatience);

                }

                return;
            }

            // If effect is applied at start of day, reset
            if (Has<SIsDayFirstUpdate>())
            {
                if (HasStatus(RestaurantStatus.CustomersOrderImmediately) && IS_CustomApplied && Main.PrefManager.Get<bool>("autoAddTippingCultureInstantService") == false
                    || fireScore.GameMode != FireGameMode.CoinCollection)
                {
                    // Set flag for effect was applied
                    IS_CustomApplied = false;
                    RemoveStatus(RestaurantStatus.CustomersOrderImmediately);

                }

                if (HasStatus(RestaurantStatus.PayBasedOnPatience) && TC_CustomApplied && Main.PrefManager.Get<bool>("autoAddTippingCultureInstantService") == false
                    || fireScore.GameMode != FireGameMode.CoinCollection)
                {
                    // Set flag for effect was applied
                    TC_CustomApplied = false;
                    RemoveStatus(RestaurantStatus.PayBasedOnPatience);

                }
            }
        }
    }
}