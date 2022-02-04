namespace BlazorWebsite.Shared.Layouts
{
    public partial class DashboardLayout
    {
        public string ProfileStateCssClass => _isStateActive ? "Active" : "";

        private bool _isStateActive = false;

        private void ProfileIcon_ChangeActiveState()
        {
            _isStateActive = !_isStateActive;
        }
    }
}
