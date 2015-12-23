using System.Collections;
using System.Collections.Generic;

namespace Baku.Quma
{
    /// <summary>値の取得機能だけに制限つき辞書を表します。</summary>
    /// <typeparam name="K">ディクショナリのキーの型</typeparam>
    /// <typeparam name="V">ディクショナリの値の型</typeparam>
    public class ReadOnlyDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        /// <summary>生成元となるディクショナリでインスタンスを初期化します。</summary>
        /// <param name="dict">生成元になるディクショナリ</param>
        public ReadOnlyDictionary(Dictionary<K, V> dict)
        {
            _dict = dict;
        }

        private Dictionary<K, V> _dict;

        /// <summary>キーを指定してディクショナリ中の要素を取得します。</summary>
        /// <param name="key">キー</param>
        /// <returns>キーに対応する値</returns>
        public V this[K key] => _dict[key];

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => _dict.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();
    }

}
