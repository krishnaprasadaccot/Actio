using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Api.Repositories
{
    public interface IActivityRepository
    {
        Task<Models.Activity> GetAsync(Guid id);
        Task AddAsync(Models.Activity activity);
        Task<IEnumerable<Models.Activity>> BrowseAsync(Guid userId);
    }
}
