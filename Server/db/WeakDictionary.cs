#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

[Serializable]
public class WeakReference<T> : WeakReference where T : class
{
    protected WeakReference(T target)
        : base(target, false)
    {
    }

    public new T Target => (T) base.Target;

    public static WeakReference<T> Create(T target)
    {
        if (target == null)
            return WeakNullReference<T>.Singleton;

        return new WeakReference<T>(target);
    }
}

// Provides a weak reference to a null target object, which, unlike
// other weak references, is always considered to be alive. This 
// facilitates handling null dictionary values, which are perfectly
// legal.
internal class WeakNullReference<T> : WeakReference<T> where T : class
{
    public static readonly WeakNullReference<T> Singleton = new WeakNullReference<T>();

    private WeakNullReference() : base(null)
    {
    }

    public override bool IsAlive => true;
}

// Provides a weak reference to an object of the given type to be used in
// a WeakDictionary along with the given comparer.
internal sealed class WeakKeyReference<T> : WeakReference<T> where T : class
{
    public readonly int HashCode;

    public WeakKeyReference(T key, WeakKeyComparer<T> comparer)
        : base(key)
    {
        // retain the object's hash code immediately so that even
        // if the target is GC'ed we will be able to find and
        // remove the dead weak reference.
        HashCode = comparer.GetHashCode(key);
    }
}

// Compares objects of the given type or WeakKeyReferences to them
// for equality based on the given comparer. Note that we can only
// implement IEqualityComparer<T> for T = object as there is no 
// other common base between T and WeakKeyReference<T>. We need a
// single comparer to handle both types because we don't want to
// allocate a new weak reference for every lookup.
internal sealed class WeakKeyComparer<T> : IEqualityComparer<object>
    where T : class
{
    private readonly IEqualityComparer<T> _comparer;

    internal WeakKeyComparer(IEqualityComparer<T> comparer)
    {
        if (comparer == null)
            comparer = EqualityComparer<T>.Default;

        _comparer = comparer;
    }

    public int GetHashCode(object obj)
    {
        WeakKeyReference<T> weakKey = obj as WeakKeyReference<T>;
        if (weakKey != null) return weakKey.HashCode;
        return _comparer.GetHashCode((T) obj);
    }

    // Note: There are actually 9 cases to handle here.
    //
    //  Let Wa = Alive Weak Reference
    //  Let Wd = Dead Weak Reference
    //  Let S  = Strong Reference
    //  
    //  x  | y  | Equals(x,y)
    // -------------------------------------------------
    //  Wa | Wa | comparer.Equals(x.Target, y.Target) 
    //  Wa | Wd | false
    //  Wa | S  | comparer.Equals(x.Target, y)
    //  Wd | Wa | false
    //  Wd | Wd | x == y
    //  Wd | S  | false
    //  S  | Wa | comparer.Equals(x, y.Target)
    //  S  | Wd | false
    //  S  | S  | comparer.Equals(x, y)
    // -------------------------------------------------
    public new bool Equals(object x, object y)
    {
        bool xIsDead, yIsDead;
        T first = GetTarget(x, out xIsDead);
        T second = GetTarget(y, out yIsDead);

        if (xIsDead)
            return yIsDead && x == y;

        return !yIsDead && _comparer.Equals(first, second);
    }

    private static T GetTarget(object obj, out bool isDead)
    {
        WeakKeyReference<T> wref = obj as WeakKeyReference<T>;
        T target;
        if (wref != null)
        {
            target = wref.Target;
            isDead = !wref.IsAlive;
        }
        else
        {
            target = (T) obj;
            isDead = false;
        }
        return target;
    }
}

[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(Prefix + "DictionaryDebugView`2" + Suffix)]
public abstract class BaseDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    private const string Prefix = "System.Collections.Generic.Mscorlib_";
    private const string Suffix = ",mscorlib,Version=2.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089";

    private KeyCollection _keys;
    private ValueCollection _values;

    public abstract int Count { get; }
    public abstract void Clear();
    public abstract void Add(TKey key, TValue value);
    public abstract bool ContainsKey(TKey key);
    public abstract bool Remove(TKey key);
    public abstract bool TryGetValue(TKey key, out TValue value);
    public abstract IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

    public bool IsReadOnly => false;

    public ICollection<TKey> Keys => _keys ?? (_keys = new KeyCollection(this));

    public ICollection<TValue> Values => _values ?? (_values = new ValueCollection(this));

    public TValue this[TKey key]
    {
        get
        {
            TValue value;
            if (!TryGetValue(key, out value))
                throw new KeyNotFoundException();

            return value;
        }
        set { SetValue(key, value); }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        TValue value;
        if (!TryGetValue(item.Key, out value))
            return false;

        return EqualityComparer<TValue>.Default.Equals(value, item.Value);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        Copy(this, array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (!Contains(item))
            return false;

        return Remove(item.Key);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    protected abstract void SetValue(TKey key, TValue value);

    private static void Copy<T>(ICollection<T> source, T[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (arrayIndex < 0 || arrayIndex > array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));

        if ((array.Length - arrayIndex) < source.Count)
            throw new ArgumentException("Destination array is not large enough. Check array.Length and arrayIndex.");

        foreach (T item in source)
            array[arrayIndex++] = item;
    }

    private abstract class Collection<T> : ICollection<T>
    {
        protected readonly IDictionary<TKey, TValue> Dictionary;

        protected Collection(IDictionary<TKey, TValue> dictionary)
        {
            Dictionary = dictionary;
        }

        public int Count => Dictionary.Count;

        public bool IsReadOnly => true;

        public void CopyTo(T[] array, int arrayIndex)
        {
            Copy(this, array, arrayIndex);
        }

        public virtual bool Contains(T item)
        {
            return this.Any(element => EqualityComparer<T>.Default.Equals(element, item));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Dictionary.Select(GetItem).GetEnumerator();
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        public void Add(T item)
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        public void Clear()
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected abstract T GetItem(KeyValuePair<TKey, TValue> pair);
    }

    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [DebuggerTypeProxy(Prefix + "DictionaryKeyCollectionDebugView`2" + Suffix)]
    private class KeyCollection : Collection<TKey>
    {
        public KeyCollection(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }

        protected override TKey GetItem(KeyValuePair<TKey, TValue> pair)
        {
            return pair.Key;
        }

        public override bool Contains(TKey item)
        {
            return Dictionary.ContainsKey(item);
        }
    }

    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [DebuggerTypeProxy(Prefix + "DictionaryValueCollectionDebugView`2" + Suffix)]
    private class ValueCollection : Collection<TValue>
    {
        public ValueCollection(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }

        protected override TValue GetItem(KeyValuePair<TKey, TValue> pair)
        {
            return pair.Value;
        }
    }
}

public sealed class WeakDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>
    where TKey : class
{
    private readonly WeakKeyComparer<TKey> _comparer;
    private readonly Dictionary<object, TValue> _dictionary;

    public WeakDictionary()
        : this(0, null)
    {
    }

    public WeakDictionary(int capacity)
        : this(capacity, null)
    {
    }

    public WeakDictionary(IEqualityComparer<TKey> comparer)
        : this(0, comparer)
    {
    }

    public WeakDictionary(int capacity, IEqualityComparer<TKey> comparer)
    {
        _comparer = new WeakKeyComparer<TKey>(comparer);
        _dictionary = new Dictionary<object, TValue>(capacity, _comparer);
    }
    public override int Count => _dictionary.Count;

    public override void Add(TKey key, TValue value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        WeakReference<TKey> weakKey = new WeakKeyReference<TKey>(key, _comparer);
        _dictionary.Add(weakKey, value);
    }

    public override bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public override bool Remove(TKey key)
    {
        return _dictionary.Remove(key);
    }

    public override bool TryGetValue(TKey key, out TValue value)
    {
        TValue weakValue;
        if (_dictionary.TryGetValue(key, out weakValue))
        {
            value = weakValue;
            return true;
        }
        value = default(TValue);
        return false;
    }

    protected override void SetValue(TKey key, TValue value)
    {
        WeakReference<TKey> weakKey = new WeakKeyReference<TKey>(key, _comparer);
        _dictionary[weakKey] = value;
    }

    public override void Clear()
    {
        _dictionary.Clear();
    }

    public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return (from kvp in _dictionary
            let weakKey = (WeakReference<TKey>) (kvp.Key)
            let value = kvp.Value
            let key = weakKey.Target
            where weakKey.IsAlive
            select new KeyValuePair<TKey, TValue>(key, value)).GetEnumerator();
    }

    // Removes the left-over weak references for entries in the dictionary
    // whose key or value has already been reclaimed by the garbage
    // collector. This will reduce the dictionary's Count by the number
    // of dead key-value pairs that were eliminated.
    public void RemoveCollectedEntries()
    {
        List<object> toRemove = null;
        foreach (KeyValuePair<object, TValue> pair in _dictionary)
        {
            WeakReference<TKey> weakKey = (WeakReference<TKey>) (pair.Key);

            if (!weakKey.IsAlive)
            {
                if (toRemove == null)
                    toRemove = new List<object>();
                toRemove.Add(weakKey);
            }
        }

        if (toRemove != null)
        {
            foreach (object key in toRemove)
                _dictionary.Remove(key);
        }
    }
}