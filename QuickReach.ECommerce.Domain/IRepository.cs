using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;

namespace QuickReach.ECommerce.Domain
{
    //interface takes an TEntity type where TEntity is a class
    public interface IRepository<TEntity>
        where TEntity : EntityBase
    {
        TEntity Create(TEntity newEntity);

        TEntity Retrieve(int entityId);

        IEnumerable<TEntity> Retrieve
            (int skip = 0, int count = 10);

        IEnumerable<TEntity> Retrieve
            (string search = "", int skip = 0, int count = 10);

        TEntity Update(int entityId, TEntity entity);

        void Delete(int entityId);
    }
}
