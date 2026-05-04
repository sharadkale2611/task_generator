namespace task_generator.Helpers
{
    public static class SubscriptionGuard
    {
        public static void EnsureFeature(bool condition, string message)
        {
            if (!condition)
                throw new Exception(message);
        }
    }
}
