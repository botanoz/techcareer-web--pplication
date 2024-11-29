using Core.Persistence.Repositories.Entities;
using Core.Security.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Service.Helpers.DtoConverters
{
    public static class EntityToDtoConverter
    {
        public static Object Convert(Object entity)
        {
            switch (entity)
            {
                case Category category:
                    return category;
                    
                case Company company:
                    return company;
               
                case Dictionary dictionary:
                    return dictionary;
              
                case Event evnt:
                    return evnt;
               
                case Instructor instructor:
                    return instructor;
                 
                case Job job:
                    return job;
                 
                case OperationClaim operationClaim:
                    return operationClaim;
                
                case TypOfWork typOfWork:
                   return typOfWork;
              
                case User user:
                   return user;
               
                case UserOperationClaim userOperationClaim:
                 return userOperationClaim;
            
                case VideoEducation videoEducation:
                   return videoEducation;

                case WorkPlace workPlace:
                 return workPlace;
   
                case null:
                 return null;
  
                default:
                    Console.WriteLine("Unknown type");
                    break;
            }

            return null;
        }
    }
}
