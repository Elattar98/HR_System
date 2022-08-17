using HR_SystemProject.Models;
using HR_SystemProject.ViewModel;

namespace HR_SystemProject.Repositories
{
    
    public class GeneralSettingsRepository : IGeneralSettingsRepository
    {
        
        HrEntity context;

        public GeneralSettingsRepository(HrEntity myDB)
        {
            context = myDB;
        }

        public GeneralSettings GetFirst()
        {
            GeneralSettings gs = context.GeneralSetting.FirstOrDefault();
            return gs;
        }

        public void Update(GeneralSettingsViewModel gs)
        {
            GeneralSettings updatedgs = GetFirst();
            updatedgs.Bouns = gs.Bouns;
            updatedgs.Discount = gs.Discount;
            updatedgs.vacation1 = (int)gs.vacation1;
            updatedgs.vacation2 = (int)gs.vacation2;
            context.SaveChanges();
        }
    }
}
