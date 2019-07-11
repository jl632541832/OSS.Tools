#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  缓存插件默认实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using OSS.Common.ComModels;

namespace OSS.Tools.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultCachePlug : ICachePlug
    {
        private static readonly MemoryCache _cache=new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 添加缓存
        /// 如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="slidingExpiration">相对过期的TimeSpan  如果使用固定时间  =TimeSpan.Zero</param>
        /// <param name="absoluteExpiration"> 绝对过期时间,不为空则按照绝对过期时间计算 </param>
        /// <returns>是否添加成功</returns>
        [Obsolete]
        public bool AddOrUpdate<T>(string key, T obj, TimeSpan slidingExpiration, DateTime? absoluteExpiration = null)
        {
            return Add(key, obj, slidingExpiration, absoluteExpiration, true);
        }

        /// <summary> 
        /// 添加时间段过期缓存
        /// 如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="slidingExpiration">相对过期的TimeSpan</param>
        /// <returns>是否添加成功</returns>
        public bool Set<T>(string key, T obj, TimeSpan slidingExpiration)
        {
            return Add(key, obj, slidingExpiration, null, true);
        }

        /// <summary>
        /// 添加固定过期时间缓存,如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="absoluteExpiration"> 绝对过期时间,不为空则按照绝对过期时间计算 </param>
        /// <returns>是否添加成功</returns>
        public bool Set<T>(string key, T obj, DateTime absoluteExpiration)
        {
            return Add(key, obj, TimeSpan.Zero, absoluteExpiration, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        private static bool Add<T>(string key, T obj, TimeSpan slidingExpiration, DateTime? absoluteExpiration,
            bool isUpdate)
        {
            if (slidingExpiration == TimeSpan.Zero && absoluteExpiration == null)
                throw new ArgumentNullException("slidingExpiration", "缓存过期时间不正确,需要设置固定过期时间或者相对过期时间");

            var opt = new MemoryCacheEntryOptions();
            if (absoluteExpiration != null)
                opt.AbsoluteExpiration = new DateTimeOffset(absoluteExpiration.Value);
            else
                opt.SlidingExpiration = slidingExpiration;

            _cache.Set(key, obj, opt);
            return true;
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">获取缓存对象类型</typeparam>
        /// <param name="key">key</param>
        /// <returns>获取指定key对应的值 </returns>
        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }
        


        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否成功</returns>
        public bool Remove(string key)
        {
            _cache.Remove(key);
            return true;
        }
    }
}
