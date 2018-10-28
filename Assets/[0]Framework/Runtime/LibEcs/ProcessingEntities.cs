/*===============================================================
         Product:    Cryoshock
         Developer:  Dimitry Pixeye - pixeye@hbrew.store
         Company:    Homebrew - http://hbrew.store
         Date:       25/01/2018 00:05
         ================================================================*/


using System;
using System.Collections.Generic;

namespace Homebrew
{
    internal class ProcessingEntities : IDisposable, IKernel
    {
        internal static ProcessingEntities Default;
        internal GroupBase[] GroupsBase = new GroupBase[64];
        internal int groupLength;

        internal static int Create()
        {
            int id;
            if (Actor.prevID.Count > 0)
            {
                id = Actor.prevID.Pop();
            }
            else

                id = Actor.lastID++;


            Tags.Add(id);

            return id;
        }

        internal static void Create(Actor a)
        {
            int len = Actor.entites.Length;

            if (Actor.lastID >= len)
                Array.Resize(ref Actor.entites, Actor.lastID << 1);

            if (Actor.prevID.Count > 0)
            {
                a.entity = Actor.prevID.Pop();
                Actor.entites[a.entity] = a;
            }
            else
            {
                a.entity = Actor.lastID;
                Actor.entites[Actor.lastID++] = a;
            }

            Tags.Add(a.entity);
        }


        internal GroupBase SetupGroup(Type groupType, Composition filter)
        {
            int i = groupLength - 1;
            for (; i >= 0; i--)
            {
                if (GroupsBase[i].GetType() != groupType) continue;
                if (GroupsBase[i].composition.Equals(filter))
                {
                    break;
                }
            }

            if (i != -1) return GroupsBase[i];

            i = groupLength;

            var group = Activator.CreateInstance(groupType, true) as GroupBase;

            group.composition = filter;

            List<GroupBase> groups;
            foreach (var tag in filter.include)
            {
                if (Tags.groupsInclude.TryGetValue(tag, out groups))
                {
                    groups.Add(group);
                }
                else
                {
                    groups = new List<GroupBase>(5);
                    groups.Add(group);
                    Tags.groupsInclude.Add(tag, groups);
                }
            }

            foreach (var tag in filter.exclude)
            {
                if (Tags.groupsDeclude.TryGetValue(tag, out groups))
                {
                    groups.Add(group);
                }
                else
                {
                    groups = new List<GroupBase>(5);
                    groups.Add(group);
                    Tags.groupsDeclude.Add(tag, groups);
                }
            }

            group.Populate();


            if (groupLength == GroupsBase.Length)
            {
                Array.Resize(ref GroupsBase, groupLength << 1);
            }

            GroupsBase[groupLength++] = group;

            return GroupsBase[i];
        }


        internal void CheckGroups(int entity, bool active)
        {
            if (Toolbox.applicationIsQuitting) return;

            if (active)
            {
                for (int i = 0; i < groupLength; i++)
                    if (GroupsBase[i].CheckTags(entity))
                        GroupsBase[i].TryAdd(entity);
            }
            else
            {
                for (int i = 0; i < groupLength; i++)
                    GroupsBase[i].Remove(entity);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < groupLength; i++)
                GroupsBase[i].Dispose();
        }
    }

    public struct EntityComposer
    {
        public int entity;
        private Storage[] storages;
        private int length;

        public void Init(int components = 1)
        {
            storages = new Storage[components];
            entity = ProcessingEntities.Create();
            length = 0;
        }

        public EntityComposer(int components = 1)
        {
            storages = new Storage[components];
            entity = ProcessingEntities.Create();
            length = 0;
        }

        public EntityComposer(int entityEntity, int components = 1)
        {
            storages = new Storage[components];
            entity = entityEntity;
            length = 0;
        }

        public T Add<T>() where T : new()
        {
            var storage = Storage<T>.Instance;
            storages[length++] = storage;

            return storage.GetOrCreate(entity);
        }

        public void Deploy()
        {
            foreach (var storage in storages)
            {
                storage.Deploy(entity);
            }

            storages = null;
            length = 0;
        }

        public void Deploy(params int[] tags)
        {
            Deploy();
            entity.Add(tags);
        }
    }
}