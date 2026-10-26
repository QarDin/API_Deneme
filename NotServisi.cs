

using Microsoft.EntityFrameworkCore;

namespace ApiOrnek
{
    public class NotServisi
    {
        readonly DbNot _dbNot;
        public NotServisi(DbNot dbNot)
        {
            _dbNot = dbNot;
        }
        public Not? Id_Getir(int id)
        {
            var result = _dbNot.notlar.FirstOrDefault(a => a.Id == id);
            return result;
        }
        public async Task<List<Not>> Liste_Getir_Async()
        {
            return await _dbNot.notlar.ToListAsync();
        }
        public async Task<Not> Not_EKle_Async(YeniNot yeniNot)
        {

            var result = new Not { Text = yeniNot.Metin, BaslikId = yeniNot.BaslikId };
            _dbNot.Add(result);
            await _dbNot.SaveChangesAsync();
            return result;
        }
        public async Task<Not?> Not_Degistir_Async(int id, YeniNot yeniNot)
        {
            var result = await _dbNot.notlar.FindAsync(id);
            if (result == null) return null;
            result.Text = yeniNot.Metin;
            result.BaslikId = yeniNot.BaslikId;
            await _dbNot.SaveChangesAsync();
            return result;
        }
    }
}