using RecipeMicroservice.Models;
using RecipeMicroservice.Repositoties;

namespace RecipeMicroservice.Services
{
    public class StatusEquipServices
    {
        private readonly StatusEquipRepository _statusEquipRepo;

        public StatusEquipServices(StatusEquipRepository statusEquipRepo)
        {
            _statusEquipRepo = statusEquipRepo;
        }

        public async Task<List<EquipStatus>> GetAllStatusEquipAsync()
        {
            return await _statusEquipRepo.GetAllStatusEquipAsync();
        }

        public async Task<EquipStatus?> GetStatusByEquipAsync(int EquipID)
        {
            return await _statusEquipRepo.GetStatusByEquipAsync(EquipID);
        }

        public async Task<int> UpdateStatusEquipAsync(FormUpdateStatusEquip req)
        {
            return await _statusEquipRepo.UpdateStatusEquipAsync(req);
        }
    }
}
