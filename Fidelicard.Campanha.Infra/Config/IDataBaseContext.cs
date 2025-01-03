using System.Data;

namespace Fidelicard.Campanha.Infra.Config
{
    public interface IDatabaseContext
    {
        IDbConnection GetConnection();
    }

}