using System.Collections;
using System.Collections.Generic;

namespace EditorColors;

public class AssociationList<TKey, TValue> : IEnumerable<(TKey, TValue)>
{
    private readonly List<(TKey, TValue)> backing = [];

    public int Count => this.backing.Count;
    
    public TValue this[TKey name]
    {
        get
        {
            foreach ((TKey name2, TValue value) in this.backing)
            {
                if (name.Equals(name2))
                {
                    return value;
                }
            }

            throw new KeyNotFoundException();
        }
        set
        {
            for(int i = 0; i < this.backing.Count; i++)
            {
                TKey name2 = this.backing[i].Item1;
                if (name.Equals(name2))
                {
                    this.backing[i] = (name2, value);
                    return;
                }
            }
            this.backing.Add((name,value));
        }
    }

    public IEnumerator<(TKey, TValue)> GetEnumerator()
    {
        return this.backing.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}