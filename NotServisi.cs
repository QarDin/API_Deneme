

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
    }
}