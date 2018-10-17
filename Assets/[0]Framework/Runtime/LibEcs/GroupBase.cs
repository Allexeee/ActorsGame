/*===============================================================
Product:    Cryoshock
Developer:  Dimitry Pixeye - pixeye@hbrew.store
Company:    Homebrew - http://hbrew.store
Date:       5/14/2018  6:32 PM
================================================================*/

using System;

namespace Homebrew
{
    public abstract class GroupBase : IDisposable
    {
        public int length;

        public int[] entities = new int[EngineSettings.MinEntities];
        public Composition composition;


        public Action<int> OnAdded;
        public Action<int> OnRemoved;
        public Action<int, int> OnTagsChanged;

        protected int indexLast;

        public T Get<T>(int index) where T : new()
        {
            return Storage<T>.Instance.TryGet(entities[index]);
        }

        public void Kill(int i)
        {
            var entity = GetEntityID(i);
            entity.Kill();
        }


        public void TagsChanged(int entityID)
        {
            int index = GetIndex(entityID);

            if (index == -1)
            {
                if (!entityID.Has(composition.include)) return;
                if (entityID.HasAny(composition.exclude)) return;

                TryAdd(entityID);
            }
            else
            {
                if (OnTagsChanged != null)
                    OnTagsChanged(index, entityID);

                if (!entityID.Has(composition.include) || entityID.HasAny(composition.exclude))

                    RemoveAt(index);
            }
        }


        public int GetIndex(int actorID)
        {
            for (int i = 0; i < length; i++)
            {
                if (entities[i] != actorID) continue;
                return i;
            }

            return -1;
        }

        public int GetEntityID(int index)
        {
            return entities[index];
        }

        public Actor GetActor(int index)
        {
            return Actor.entites[entities[index]];
        }

        public bool CheckTags(int entityID)
        {
            if (!entityID.Has(composition.include))
                return false;

            return !entityID.HasAny(composition.exclude);
        }


        public abstract void TryAdd(int entityID);


        public void Remove(int entityID)
        {
            int i = GetIndex(entityID);
            if (i == -1) return;
            RemoveAt(i);
        }


        public abstract void Populate();
        protected abstract void RemoveAt(int i);


        public void Dispose()
        {
            OnAdded = null;
            OnRemoved = null;
            length = 0;
            entities = new int[EngineSettings.MinEntities];
            OnDispose();
        }

        protected abstract void OnDispose();
    }


    public class Group<T> : GroupBase where T : IComponent, new()
    {
        public T[] component = new T[EngineSettings.MinComponents];
        private Storage<T> storage = Storage<T>.Instance;


        public override void TryAdd(int entityID)
        {
            if (!storage.HasComponent(entityID)) return;


            if (entities.Length <= length)
            {
                int len = length << 1;
                Array.Resize(ref entities, len);
                Array.Resize(ref component, len);
            }


            indexLast = length++;

            entities[indexLast] = entityID;
            component[indexLast] = storage.components[entityID];


            if (OnAdded != null)
                OnAdded(indexLast);
        }


        public override void Populate()
        {
            storage.groups.Add(this);

            for (int i = 0; i < Actor.lastID; i++)
            {
                var e = Actor.entites[i];
                if (e == null || !e.state.enabled) continue;

                if (composition.include.Length > 0)
                    if (!e.entity.Has(composition.include))
                        continue;

                if (composition.exclude.Length > 0)
                    if (e.entity.HasAny(composition.exclude))
                        continue;


                if (!storage.HasComponent(e.entity)) continue;


                if (length == entities.Length)
                {
                    int len = length << 1;
                    Array.Resize(ref entities, len);
                    Array.Resize(ref component, len);
                }

                int entityID = e.entity;
                indexLast = length++;


                entities[indexLast] = entityID;
                component[indexLast] = storage.components[entityID];


                if (OnAdded != null)
                    OnAdded(indexLast);
            }
        }

        protected override void RemoveAt(int i)
        {
            int l = --length;

            int next = i + 1;
            int size = l - i;

            if (OnRemoved != null)
                OnRemoved(i);

            Array.Copy(entities, next, entities, i, size);
            Array.Copy(component, next, component, i, size);
        }


        protected override void OnDispose()
        {
            component = new T[EngineSettings.MinComponents];
        }
    }

    public class Group<T, Y> : GroupBase where T : IComponent, new() where Y : new()
    {
        public T[] component = new T[EngineSettings.MinComponents];
        public Y[] component2 = new Y[EngineSettings.MinComponents];
        Storage<T> storage = Storage<T>.Instance;
        Storage<Y> storage2 = Storage<Y>.Instance;


        public override void TryAdd(int entityID)
        {
            if (!storage.HasComponent(entityID) || !storage2.HasComponent(entityID)) return;
            if (entities.Length <= length)
            {
                int len = length << 1;

                Array.Resize(ref entities, len);
                Array.Resize(ref component, len);
                Array.Resize(ref component2, len);
            }


            indexLast = length++;

            entities[indexLast] = entityID;
            component[indexLast] = storage.components[entityID];
            component2[indexLast] = storage2.components[entityID];


            if (OnAdded != null)
                OnAdded(indexLast);
        }


        public override void Populate()
        {
            storage.groups.Add(this);
            storage2.groups.Add(this);


            for (int i = 0; i < Actor.lastID; i++)
            {
                var e = Actor.entites[i];
                if (e == null || !e.state.enabled) continue;
                if (composition.include.Length > 0)
                    if (!e.entity.Has(composition.include))
                        continue;

                if (composition.exclude.Length > 0)
                    if (e.entity.HasAny(composition.exclude))
                        continue;


                if (!storage.HasComponent(e.entity) || !storage2.HasComponent(e.entity)) continue;


                if (length == entities.Length)
                {
                    int len = length << 1;

                    Array.Resize(ref entities, len);
                    Array.Resize(ref component, len);
                    Array.Resize(ref component2, len);
                }

                int entityID = e.entity;
                indexLast = length++;


                entities[indexLast] = entityID;
                component[indexLast] = storage.components[entityID];
                component2[indexLast] = storage2.components[entityID];
            }
        }

        protected override void RemoveAt(int i)
        {
            int l    = --length;
            int next = i + 1;
            int size = l - i;

            if (OnRemoved != null)
                OnRemoved(i);

            Array.Copy(entities, next, entities, i, size);
            Array.Copy(component, next, component, i, size);
            Array.Copy(component2, next, component2, i, size);
        }


        protected override void OnDispose()
        {
            component = new T[EngineSettings.MinComponents];
            component2 = new Y[EngineSettings.MinComponents];
        }
    }

    public class Group<T, Y, U> : GroupBase where T : IComponent, new() where Y : new() where U : new()
    {
        public T[] component = new T[EngineSettings.MinComponents];
        public Y[] component2 = new Y[EngineSettings.MinComponents];
        public U[] component3 = new U[EngineSettings.MinComponents];
        Storage<T> storage = Storage<T>.Instance;
        Storage<Y> storage2 = Storage<Y>.Instance;
        Storage<U> storage3 = Storage<U>.Instance;

        protected override void RemoveAt(int i)
        {
            int l    = --length;
            int next = i + 1;
            int size = l - i;
            if (OnRemoved != null)
                OnRemoved(i);
            Array.Copy(entities, next, entities, i, size);
            Array.Copy(component, next, component, i, size);
            Array.Copy(component2, next, component2, i, size);
            Array.Copy(component3, next, component3, i, size);
        }

        public override void TryAdd(int entityID)
        {
            if (!storage.HasComponent(entityID) ||
                !storage2.HasComponent(entityID) ||
                !storage3.HasComponent(entityID)
            ) return;


            if (entities.Length <= length)
            {
                int len = length << 1;
                Array.Resize(ref entities, len);
                Array.Resize(ref component, len);
                Array.Resize(ref component2, len);
                Array.Resize(ref component3, len);
            }


            indexLast = length++;

            entities[indexLast] = entityID;
            component[indexLast] = storage.components[entityID];
            component2[indexLast] = storage2.components[entityID];
            component3[indexLast] = storage3.components[entityID];

            if (OnAdded != null)
                OnAdded(indexLast);
        }


        public override void Populate()
        {
            storage.groups.Add(this);
            storage2.groups.Add(this);
            storage3.groups.Add(this);


            for (int i = 0; i < Actor.lastID; i++)
            {
                var e = Actor.entites[i];
                if (e == null || !e.state.enabled) continue;

                if (composition.include.Length > 0)
                    if (!e.entity.Has(composition.include))
                        continue;

                if (composition.exclude.Length > 0)
                    if (e.entity.HasAny(composition.exclude))
                        continue;

                int entityID = e.entity;

                if (!storage.HasComponent(entityID) ||
                    !storage2.HasComponent(entityID) ||
                    !storage3.HasComponent(entityID)
                ) continue;


                if (length == entities.Length)
                {
                    int len = length << 1;

                    Array.Resize(ref entities, len);
                    Array.Resize(ref component, len);
                    Array.Resize(ref component2, len);
                    Array.Resize(ref component3, len);
                }

                indexLast = length++;

                entities[indexLast] = entityID;
                component[indexLast] = storage.components[entityID];
                component2[indexLast] = storage2.components[entityID];
                component3[indexLast] = storage3.components[entityID];
            }
        }


        protected override void OnDispose()
        {
            component = new T[EngineSettings.MinComponents];
            component2 = new Y[EngineSettings.MinComponents];
            component3 = new U[EngineSettings.MinComponents];
        }
    }

    public class Group<T, Y, U, I> : GroupBase where T : IComponent, new()
                                               where Y : new()
                                               where U : new()
                                               where I : new()
    {
        public T[] component = new T[EngineSettings.MinComponents];
        public Y[] component2 = new Y[EngineSettings.MinComponents];
        public U[] component3 = new U[EngineSettings.MinComponents];
        public I[] component4 = new I[EngineSettings.MinComponents];

        Storage<T> storage = Storage<T>.Instance;
        Storage<Y> storage2 = Storage<Y>.Instance;
        Storage<U> storage3 = Storage<U>.Instance;
        Storage<I> storage4 = Storage<I>.Instance;

        protected override void RemoveAt(int i)
        {
            int l    = --length;
            int next = i + 1;
            int size = l - i;
            if (OnRemoved != null)
                OnRemoved(i);
            Array.Copy(entities, next, entities, i, size);
            Array.Copy(component, next, component, i, size);
            Array.Copy(component2, next, component2, i, size);
            Array.Copy(component3, next, component3, i, size);
            Array.Copy(component4, next, component4, i, size);
        }

        public override void TryAdd(int entityID)
        {
            if (!storage.HasComponent(entityID) ||
                !storage2.HasComponent(entityID) ||
                !storage3.HasComponent(entityID) ||
                !storage4.HasComponent(entityID)
            ) return;

            if (entities.Length <= length)
            {
                int len = length << 1;

                Array.Resize(ref entities, len);
                Array.Resize(ref component, len);
                Array.Resize(ref component2, len);
                Array.Resize(ref component3, len);
                Array.Resize(ref component4, len);
            }

            indexLast = length++;

            entities[indexLast] = entityID;
            component[indexLast] = storage.components[entityID];
            component2[indexLast] = storage2.components[entityID];
            component3[indexLast] = storage3.components[entityID];
            component4[indexLast] = storage4.components[entityID];

            if (OnAdded != null)
                OnAdded(indexLast);
        }


        public override void Populate()
        {
            storage.groups.Add(this);
            storage2.groups.Add(this);
            storage3.groups.Add(this);
            storage4.groups.Add(this);

            for (int i = 0; i < Actor.lastID; i++)
            {
                var e = Actor.entites[i];
                if (e == null || !e.state.enabled) continue;


                int entityID = e.entity;
                if (!storage.HasComponent(entityID) ||
                    !storage2.HasComponent(entityID) ||
                    !storage3.HasComponent(entityID) ||
                    !storage4.HasComponent(entityID)
                ) continue;


                if (composition.include.Length > 0)
                    if (!e.entity.Has(composition.include))
                        continue;

                if (composition.exclude.Length > 0)
                    if (e.entity.HasAny(composition.exclude))
                        continue;


                if (length == entities.Length)
                {
                    int len = length << 1;

                    Array.Resize(ref entities, len);
                    Array.Resize(ref component, len);
                    Array.Resize(ref component2, len);
                    Array.Resize(ref component3, len);
                    Array.Resize(ref component4, len);
                }

                indexLast = length++;

                entities[indexLast] = entityID;
                component[indexLast] = storage.components[entityID];
                component2[indexLast] = storage2.components[entityID];
                component3[indexLast] = storage3.components[entityID];
                component4[indexLast] = storage4.components[entityID];
            }
        }


        protected override void OnDispose()
        {
            component = new T[EngineSettings.MinComponents];
            component2 = new Y[EngineSettings.MinComponents];
            component3 = new U[EngineSettings.MinComponents];
            component4 = new I[EngineSettings.MinComponents];
        }
    }

    public class Group<T, Y, U, I, O> : GroupBase where T : IComponent, new()
                                                  where Y : new()
                                                  where U : new()
                                                  where I : new()
                                                  where O : new()
    {
        public T[] component = new T[EngineSettings.MinComponents];
        public Y[] component2 = new Y[EngineSettings.MinComponents];
        public U[] component3 = new U[EngineSettings.MinComponents];
        public I[] component4 = new I[EngineSettings.MinComponents];
        public O[] component5 = new O[EngineSettings.MinComponents];

        Storage<T> storage = Storage<T>.Instance;
        Storage<Y> storage2 = Storage<Y>.Instance;
        Storage<U> storage3 = Storage<U>.Instance;
        Storage<I> storage4 = Storage<I>.Instance;
        Storage<O> storage5 = Storage<O>.Instance;


        public override void TryAdd(int entityID)
        {
            if (!storage.HasComponent(entityID) ||
                !storage2.HasComponent(entityID) ||
                !storage3.HasComponent(entityID) ||
                !storage4.HasComponent(entityID) ||
                !storage5.HasComponent(entityID)
            ) return;

            if (entities.Length <= length)
            {
                int len = entityID == 0 ? EngineSettings.MinComponents : length << 1;

                Array.Resize(ref entities, len);
                Array.Resize(ref component, len);
                Array.Resize(ref component2, len);
                Array.Resize(ref component3, len);
                Array.Resize(ref component4, len);
                Array.Resize(ref component5, len);
            }

            indexLast = length++;

            entities[indexLast] = entityID;
            component[indexLast] = storage.components[entityID];
            component2[indexLast] = storage2.components[entityID];
            component3[indexLast] = storage3.components[entityID];
            component4[indexLast] = storage4.components[entityID];
            component5[indexLast] = storage5.components[entityID];


            if (OnAdded != null)
                OnAdded(indexLast);
        }


        public override void Populate()
        {
            storage.groups.Add(this);
            storage2.groups.Add(this);
            storage3.groups.Add(this);
            storage4.groups.Add(this);
            storage5.groups.Add(this);


            for (int i = 0; i < Actor.lastID; i++)
            {
                var e = Actor.entites[i];

                if (e == null || !e.state.enabled) continue;

                if (composition.include.Length > 0)
                    if (!e.entity.Has(composition.include))
                        continue;

                if (composition.exclude.Length > 0)
                    if (e.entity.HasAny(composition.exclude))
                        continue;
                int entityID = e.entity;
                if (!storage.HasComponent(entityID) ||
                    !storage2.HasComponent(entityID) ||
                    !storage3.HasComponent(entityID) ||
                    !storage4.HasComponent(entityID) ||
                    !storage5.HasComponent(entityID)
                ) continue;


                if (length == entities.Length)
                {
                    int len = length << 1;

                    Array.Resize(ref entities, len);
                    Array.Resize(ref component, len);
                    Array.Resize(ref component2, len);
                    Array.Resize(ref component3, len);
                    Array.Resize(ref component4, len);
                    Array.Resize(ref component5, len);
                }


                indexLast = length++;

                entities[indexLast] = entityID;
                component[indexLast] = storage.components[entityID];
                component2[indexLast] = storage2.components[entityID];
                component3[indexLast] = storage3.components[entityID];
                component4[indexLast] = storage4.components[entityID];
                component5[indexLast] = storage5.components[entityID];
            }
        }


        protected override void RemoveAt(int i)
        {
            int l    = --length;
            int next = i + 1;
            int size = l - i;
            if (OnRemoved != null)
                OnRemoved(i);
            Array.Copy(entities, next, entities, i, size);
            Array.Copy(component, next, component, i, size);
            Array.Copy(component2, next, component2, i, size);
            Array.Copy(component3, next, component3, i, size);
            Array.Copy(component4, next, component4, i, size);
            Array.Copy(component5, next, component5, i, size);
        }


        protected override void OnDispose()
        {
            component = new T[EngineSettings.MinComponents];
            component2 = new Y[EngineSettings.MinComponents];
            component3 = new U[EngineSettings.MinComponents];
            component4 = new I[EngineSettings.MinComponents];
            component5 = new O[EngineSettings.MinComponents];
        }
    }


    public class Group<T, Y, U, I, O, P> : GroupBase where T : IComponent, new()
                                                     where Y : new()
                                                     where U : new()
                                                     where I : new()
                                                     where O : new()
                                                     where P : new()
    {
        public T[] component = new T[EngineSettings.MinComponents];
        public Y[] component2 = new Y[EngineSettings.MinComponents];
        public U[] component3 = new U[EngineSettings.MinComponents];
        public I[] component4 = new I[EngineSettings.MinComponents];
        public O[] component5 = new O[EngineSettings.MinComponents];
        public P[] component6 = new P[EngineSettings.MinComponents];

        Storage<T> storage = Storage<T>.Instance;
        Storage<Y> storage2 = Storage<Y>.Instance;
        Storage<U> storage3 = Storage<U>.Instance;
        Storage<I> storage4 = Storage<I>.Instance;
        Storage<O> storage5 = Storage<O>.Instance;
        Storage<P> storage6 = Storage<P>.Instance;


        public override void TryAdd(int entityID)
        {
            if (!storage.HasComponent(entityID) ||
                !storage2.HasComponent(entityID) ||
                !storage3.HasComponent(entityID) ||
                !storage4.HasComponent(entityID) ||
                !storage5.HasComponent(entityID) ||
                !storage6.HasComponent(entityID)
            ) return;

            if (entities.Length <= length)
            {
                int len = length << 1;

                Array.Resize(ref entities, len);
                Array.Resize(ref component, len);
                Array.Resize(ref component2, len);
                Array.Resize(ref component3, len);
                Array.Resize(ref component4, len);
                Array.Resize(ref component5, len);
                Array.Resize(ref component6, len);
            }

            indexLast = length++;

            entities[indexLast] = entityID;
            component[indexLast] = storage.components[entityID];
            component2[indexLast] = storage2.components[entityID];
            component3[indexLast] = storage3.components[entityID];
            component4[indexLast] = storage4.components[entityID];
            component5[indexLast] = storage5.components[entityID];
            component6[indexLast] = storage6.components[entityID];

            if (OnAdded != null)
                OnAdded(indexLast);
        }


        public override void Populate()
        {
            storage.groups.Add(this);
            storage2.groups.Add(this);
            storage3.groups.Add(this);
            storage4.groups.Add(this);
            storage5.groups.Add(this);
            storage6.groups.Add(this);

            for (int i = 0; i < Actor.lastID; i++)
            {
                var e = Actor.entites[i];

                if (e == null || !e.state.enabled) continue;

                if (composition.include.Length > 0)
                    if (!e.entity.Has(composition.include))
                        continue;

                if (composition.exclude.Length > 0)
                    if (e.entity.HasAny(composition.exclude))
                        continue;
                int entityID = e.entity;
                if (!storage.HasComponent(entityID) ||
                    !storage2.HasComponent(entityID) ||
                    !storage3.HasComponent(entityID) ||
                    !storage4.HasComponent(entityID) ||
                    !storage5.HasComponent(entityID) ||
                    !storage6.HasComponent(entityID)
                ) continue;


                if (length == entities.Length)
                {
                    int len = length << 1;

                    Array.Resize(ref entities, len);
                    Array.Resize(ref component, len);
                    Array.Resize(ref component2, len);
                    Array.Resize(ref component3, len);
                    Array.Resize(ref component4, len);
                    Array.Resize(ref component5, len);
                    Array.Resize(ref component6, len);
                }


                indexLast = length++;

                entities[indexLast] = entityID;
                component[indexLast] = storage.components[entityID];
                component2[indexLast] = storage2.components[entityID];
                component3[indexLast] = storage3.components[entityID];
                component4[indexLast] = storage4.components[entityID];
                component5[indexLast] = storage5.components[entityID];
                component6[indexLast] = storage6.components[entityID];
            }
        }


        protected override void RemoveAt(int i)
        {
            int l    = --length;
            int next = i + 1;
            int size = l - i;

            if (OnRemoved != null)
                OnRemoved(i);

            Array.Copy(entities, next, entities, i, size);
            Array.Copy(component, next, component, i, size);
            Array.Copy(component2, next, component2, i, size);
            Array.Copy(component3, next, component3, i, size);
            Array.Copy(component4, next, component4, i, size);
            Array.Copy(component5, next, component5, i, size);
            Array.Copy(component6, next, component6, i, size);
        }


        protected override void OnDispose()
        {
            component = new T[EngineSettings.MinComponents];
            component2 = new Y[EngineSettings.MinComponents];
            component3 = new U[EngineSettings.MinComponents];
            component4 = new I[EngineSettings.MinComponents];
            component5 = new O[EngineSettings.MinComponents];
            component6 = new P[EngineSettings.MinComponents];
        }
    }
}