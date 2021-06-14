using Newtonsoft.Json;
using SMS.Creator.Common.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;

namespace SMS.Creator.Sync.API.Helper
{
    public static class GenericConversionHelper
    {

        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {

            List<T> list = null;
            try
            {
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(table);
                list = JsonConvert.DeserializeObject<List<T>>(JSONresult);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("GenericConversionHelper", "DataTableToList", ex);
            }
            return list.ToList();
        }



        public abstract class GenericMapper<TSource, TDestination>
        {
            public static void MapObject(TSource entity, TDestination destination)
            {
                Mapper.CreateMap<TSource, TDestination>();
                Mapper.Map<TSource, TDestination>(entity, destination);
            }

            public static TDestination MapObject(TSource entity)
            {
                Mapper.CreateMap<TSource, TDestination>();

                TDestination dto = Mapper.Map<TSource, TDestination>(entity);

                return dto;
            }

            public static void CreateMapping()
            {
                Mapper.CreateMap<TSource, TDestination>();
            }

            public static List<TDestination> MapList(IEnumerable<TSource> entities)
            {
                List<TDestination> dtoList = new List<TDestination>();

                foreach (TSource entity in entities)
                {
                    dtoList.Add(MapObject(entity));
                }

                return dtoList;
            }

        }

    }

}