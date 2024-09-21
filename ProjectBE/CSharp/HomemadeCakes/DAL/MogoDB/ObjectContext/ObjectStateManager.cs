using System.Collections.Concurrent;
using System;
using HomemadeCakes.DAL.MogoDB.Entity;
using HomemadeCakes.Common;

namespace HomemadeCakes.DAL.MogoDB.ObjectContext
{
    public class ObjectStateManager : NotificationBase, IDisposable
    {
        [CRUD(DataState.Added)]
        internal ConcurrentDictionary<string, IEntity> AddedEntities { get; } = new ConcurrentDictionary<string, IEntity>();


        [CRUD(DataState.Modified)]
        internal ConcurrentDictionary<string, IEntity> UpdatedEntities { get; } = new ConcurrentDictionary<string, IEntity>();


        [CRUD(DataState.Deleted)]
        internal ConcurrentDictionary<string, IEntity> DeletedEntities { get; } = new ConcurrentDictionary<string, IEntity>();


        internal bool HasChanges
        {
            get
            {
                if (AddedEntities.Count <= 0 && UpdatedEntities.Count <= 0)
                {
                    return DeletedEntities.Count > 0;
                }

                return true;
            }
        }

        private void RaiseHasChangesProperty(bool hasChanges)
        {
            if (hasChanges != HasChanges)
            {
                OnPropertyChanged(() => HasChanges);
            }
        }

        public void SetAdd(object entity)
        {
            if (entity is IEntity)
            {
                bool hasChanges = HasChanges;
                IEntity entity2 = entity as IEntity;
                if (!DeletedEntities.ContainsKey(entity2.Id) && !UpdatedEntities.ContainsKey(entity2.Id))
                {
                    AddedEntities.TryAdd(entity2.Id, entity2);
                    RaiseHasChangesProperty(hasChanges);
                }
            }
        }

        public void SetUpdate(object entity)
        {
            if (entity is IEntity)
            {
                bool hasChanges = HasChanges;
                IEntity entity2 = entity as IEntity;
                if (!AddedEntities.ContainsKey(entity2.Id) && !DeletedEntities.ContainsKey(entity2.Id))
                {
                    UpdatedEntities.TryAdd(entity2.Id, entity2);
                }

                RaiseHasChangesProperty(hasChanges);
            }
        }

        public void SetDelete(object entity)
        {
            if (entity is IEntity)
            {
                bool hasChanges = HasChanges;
                IEntity entity2 = entity as IEntity;
                UpdatedEntities.TryRemove(entity2.Id, out var value);
                if (!AddedEntities.TryRemove(entity2.Id, out value))
                {
                    DeletedEntities.TryAdd(entity2.Id, entity2);
                }

                RaiseHasChangesProperty(hasChanges);
            }
        }

        public virtual void Dispose()
        {
            AddedEntities.Clear();
            UpdatedEntities.Clear();
            DeletedEntities.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
