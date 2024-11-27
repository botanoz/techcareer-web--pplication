using Core.Persistence.Repositories;
using Core.Security.Entities;
using TechCareer.DataAccess.Contexts;
using TechCareer.DataAccess.Repositories.Abstracts;

namespace TechCareer.DataAccess.Repositories.Concretes;

public sealed class VideoEducationRepository(BaseDbContext context) : EfRepositoryBase<VideoEducation, int,BaseDbContext>(context), IVideoEducationRepository
{
    
}