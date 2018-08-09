using JerryPlat.DAL.Context;
using JerryPlat.Utils.Helpers;

namespace JerryPlat.DAL
{
    public class BaseHelper : DbContextBaseHelper<JerryPlatDbContext>
    {
    }

    public class BaseHelper<TEntity> : DbContextBaseHelper<JerryPlatDbContext, TEntity>
        where TEntity : class, new()
    {
    }
}