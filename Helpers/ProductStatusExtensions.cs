namespace Website_Progress.Helpers
{
    public static class ProductStatusExtensions
    {
        public static string GetDisplayName(this ProductStatusViewModel status)
        {
            var member = typeof(ProductStatusViewModel)
                .GetMember(status.ToString()).FirstOrDefault();
            return member?.GetCustomAttribute<DisplayAttribute>()?.Name ?? status.ToString();
        }

        public static string GetCssClass(this ProductStatusViewModel status) => status switch
        {
            ProductStatusViewModel.Production => "status-production",
            ProductStatusViewModel.Testing => "status-testing",
            ProductStatusViewModel.Manufacturing => "status-manufacturing",
            ProductStatusViewModel.Project => "status-project",
            _ => "status-development"
        };

        public static string GetIcon(this ProductStatusViewModel status) => status switch
        {
            ProductStatusViewModel.Production => "bi-check-circle-fill",
            ProductStatusViewModel.Testing => "bi-activity",
            ProductStatusViewModel.Manufacturing => "bi-gear-fill",
            ProductStatusViewModel.Project => "bi-rulers",
            _ => "bi-code-slash"
        };
    }
}