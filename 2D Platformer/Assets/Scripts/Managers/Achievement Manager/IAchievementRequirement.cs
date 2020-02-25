using System;

namespace Platformer.Achievements
{
    public interface IAchievementRequirement
    {
        void FulfillRequirement(Action<AchievementRequirement> onAchievementRequirementFulfilled);
    }
}