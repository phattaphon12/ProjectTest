using RecipeMicroservice.Models;
using RecipeMicroservice.Repositoties;

namespace RecipeMicroservice.Services
{
    public class EquipService
    {
        private readonly EquipRepository _equipRepo;

        public EquipService(EquipRepository equipRepo)
        {
            _equipRepo = equipRepo;
        }

        public async Task<List<Equip>> GetAllEquipAsync()
        {
            return await _equipRepo.GetAllEquipAsync();
        }

        public async Task<List<LogEquip>> GetAllLogAsync()
        {
            return await _equipRepo.GetAllLogAsync();
        }

        public async Task<List<LogEquip>> GetLogByEquipAsync(int equipID)
        {
            return await _equipRepo.GetLogByEquipAsync(equipID);
        }

        public async Task<int> InsertLogAsync(FormLogEquip req)
        {
            return await _equipRepo.InsertLogAsync(req);
        }
    }
}
