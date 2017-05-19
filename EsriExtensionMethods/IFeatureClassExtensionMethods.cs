using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;

namespace EsriExtensionMethods
{
    public static class IFeatureClassExtensionMethods
    {
        /// <summary>
        /// Wrapper to Execute a Attribute or Spatial Query on a featureclass
        /// </summary>
        /// <param name="featureClass"></param>
        /// <param name="query"></param>
        /// <param name="recycling"></param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<IFeature> SearchWrapper(this IFeatureClass featureClass, IQueryFilter query, bool recycling)
        {
            using (var comReleaser = new ComReleaser())
            {
                var cursor = featureClass.Search(query, recycling);
                comReleaser.ManageLifetime(cursor);

                IFeature feature;
                while ((feature = cursor.NextFeature()) != null)
                {
                    yield return feature;
                }
            }
        }

    }
}