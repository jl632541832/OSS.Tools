#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  缓存插件辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;

namespace OSS.Tools.Cache
{
    /// <summary>
    /// 缓存的辅助类
    /// </summary>
    public static class CacheHelper
    {
        private static readonly DefaultToolCache defaultCache = new DefaultToolCache();

        /// <summary>
        /// 缓存模块提供者
        /// </summary>
        public static Func<string, IToolCache> CacheProvider { get; set; }

        /// <summary>
        /// 通过模块名称获取
        /// </summary>
        /// <param name="cacheModule"></param>
        /// <returns></returns>
        public static IToolCache GetCache(string cacheModule)
        {
            if (string.IsNullOrEmpty(cacheModule))
                cacheModule = "default";

            return CacheProvider?.Invoke(cacheModule) ?? defaultCache;
        }

        /// <summary> 
        /// 添加时间段过期缓存
        /// 如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="slidingExpiration">相对过期的TimeSpan</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>是否添加成功</returns>
        public static bool Set<T>(string key, T obj, TimeSpan slidingExpiration,
            string moduleName = "default")
        {
            return GetCache(moduleName).Set(key, obj, slidingExpiration);
        }

        /// <summary>
        /// 添加固定过期时间缓存,如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="absoluteExpiration"> 绝对过期时间,不为空则按照绝对过期时间计算 </param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>是否添加成功</returns>
        public static bool Set<T>(string key, T obj, DateTime absoluteExpiration,
            string moduleName = "default")
        {
            return GetCache(moduleName).Set(key, obj, absoluteExpiration);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">获取缓存对象类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>获取指定key对应的值 </returns>
        public static T Get<T>(string key, string moduleName = "default")
        {
            return GetCache(moduleName).Get<T>(key);
        }

        /// <summary>
        /// 获取缓存数据，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">没有数据时，通过此方法获取原始数据</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Task<RType> Get<RType>(string cacheKey, Func<Task<RType>> getFunc, DateTime absoluteExpiration, string moduleName = "default")
        {
            return Get(cacheKey, getFunc, TimeSpan.Zero, absoluteExpiration,null,  moduleName);
        }

        /// <summary>
        /// 获取缓存数据，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">没有数据时，通过此方法获取原始数据</param>
        /// <param name="slidingExpiration">过期时长，访问后自动延长</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Task<RType> Get<RType>(string cacheKey, Func<Task<RType>> getFunc, TimeSpan slidingExpiration, string moduleName = "default")
        {
            return Get(cacheKey, getFunc,  slidingExpiration,null, null, moduleName);
        }

        /// <summary>
        /// 获取缓存数据【同时添加缓存击穿保护】，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">没有数据时，通过此方法获取原始数据</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="hitProtectExpiration">保护到期时间</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Task<RType> ProtectedGet<RType>(string cacheKey, Func<Task<RType>> getFunc,
          
            DateTime absoluteExpiration, DateTime hitProtectExpiration, string moduleName = "default")
        {
            return Get(cacheKey, getFunc,TimeSpan.Zero, absoluteExpiration,hitProtectExpiration, moduleName);
        }

        /// <summary>
        /// 获取缓存数据【同时添加缓存击穿保护】，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">没有数据时，通过此方法获取原始数据</param>
        /// <param name="slidingExpiration">过期时长，访问后自动延长</param>
        /// <param name="hitProtectExpiration">保护到期时间</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Task<RType> ProtectedGet<RType>(string cacheKey, Func<Task<RType>> getFunc,
           TimeSpan slidingExpiration, DateTime hitProtectExpiration, string moduleName = "default")
        {
            return Get(cacheKey, getFunc, slidingExpiration, null, hitProtectExpiration, moduleName);
        }

        private static async Task<RType> Get<RType>(string cacheKey, Func<Task<RType>> createFunc
            , TimeSpan slidingExpiration, DateTime? absoluteExpiration,
            DateTime? hitProtectExpiration, string moduleName)
        {
            var obj = GetCache(moduleName).Get<HitProtectCahce<RType>>(cacheKey);
            if (!obj.Equals(null))
            {
                if (obj.IsNull)
                    return default(RType);

                return obj.Data;
            }

            var data = await createFunc.Invoke();
            if (data==null)
            {
                if (hitProtectExpiration.HasValue)
                {
                    var safeData = new HitProtectCahce<RType>();
                    Set(cacheKey, safeData, hitProtectExpiration.Value);
                }
            }
            else
            {
                var safeData = new HitProtectCahce<RType>(data);

                if (absoluteExpiration.HasValue)
                    Set(cacheKey, safeData, absoluteExpiration.Value);
                else
                    Set(cacheKey, safeData, slidingExpiration);
            }
            return data;
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>是否成功</returns>
        public static bool Remove(string key, string moduleName = "default")
        {
            return GetCache(moduleName).Remove(key);
        }

    }


    internal struct HitProtectCahce<TT>
    {
        public HitProtectCahce(TT data)
        {
            IsNull = false;
            Data = data;
        }
        public HitProtectCahce(bool isNull=true)
        {
            IsNull = isNull;
            Data = default(TT);
        }

        public TT Data { get; set; }
        public bool IsNull { get; set; }
    }
}
