using System;
using Platformer.Achievements;

namespace Platformer.Achievements
{
    public interface IAchievementRequirement
    {
        void FulfillRequirement(Action<AchievementRequirement> onAchievementRequirementFulfilled);
    }
}