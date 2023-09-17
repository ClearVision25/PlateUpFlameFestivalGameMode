using Kitchen;
using KitchenMods;
using Unity.Entities;

namespace KitchenFireEvent
{
    public class CheckGameOverFromFireScore : RestaurantSystem, IModSystem
    {
        protected override void Initialise()
        {
            base.Initialise();

            RequireSingletonForUpdate<SFireScore>();
            RequireSingletonForUpdate<SFireGameActive>();
            RequireSingletonForUpdate<SDay>();
        }

        protected override void OnUpdate()
        {
            if (!Require(out SFireScore fireScore) || !TryGetSingleton(out SDay sDay))
                return;

            int day = sDay.Day;

            if (!HasSingleton<SGameOver>() && fireScore.Score <= 0f && fireScore.LoseWhenScoreEmpty == true && !Has<SPracticeMode>()) // Count Down End Trigger
            {
                Entity entity = base.EntityManager.CreateEntity(typeof(SGameOver), typeof(CGamePauseBlock));
                base.EntityManager.SetComponentData(entity, new SGameOver
                {
                    Reason = LossReason.Demo
                });
            }

            if (!HasSingleton<SGameOver>() && fireScore.LoseWhenScoreEmpty == false && fireScore.Score < fireScore.Target
                && !Has<SPracticeMode>() && fireScore.Date != 0 && day-1 >= fireScore.Date) // Count Up End Trigger
            {
                Entity entity = base.EntityManager.CreateEntity(typeof(SGameOver), typeof(CGamePauseBlock));
                base.EntityManager.SetComponentData(entity, new SGameOver
                {
                    Reason = LossReason.Demo
                });
            }
        }
    }
}
