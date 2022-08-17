using HR_SystemProject.Models;
using HR_SystemProject.ViewModel;

namespace HR_SystemProject.Repositories
{
    public interface IGeneralSettingsRepository
    {
        public void Update(GeneralSettingsViewModel gs);
        public GeneralSettings GetFirst();
    }
}
