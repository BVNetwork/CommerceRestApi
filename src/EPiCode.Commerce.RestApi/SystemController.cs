using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction.RuntimeModel;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Dto;
using Mediachase.Commerce.Catalog.ImportExport;
using Mediachase.Commerce.Core;
using Mediachase.MetaDataPlus;
using Mediachase.MetaDataPlus.Configurator;

namespace EPiCode.Commerce.RestService
{
    public class SystemController : SecuredApiController
    {
        // ReSharper disable once InconsistentNaming
        protected static ILogger _log = LogManager.GetLogger();

        public class ClearCatalogAndModelsResult
        {
            public int CatalogsDeleted { get; set; }
            public int CatalogsFailed { get; set; }
            public int MetaClassesDeleted { get; set; }
            public int MetaClassesSkipped { get; set; }
            public int MetaFieldsDeleted { get; set; }
            public int MetaFieldsSkipped { get; set; }
        }

        [HttpGet]
        public ClearCatalogAndModelsResult ClearCatalogAndModels()
        {
            List<string> logList = new List<string>();
            ClearCatalogAndModelsResult result = new ClearCatalogAndModelsResult();

            ICatalogSystem catalogSystem = ServiceLocator.Current.GetInstance<ICatalogSystem>();
            CatalogDto catalogDto = catalogSystem.GetCatalogDto();
            foreach (CatalogDto.CatalogRow row in catalogDto.Catalog)
            {
                _log.Debug("Deleting catalog: " + row.Name);
                try
                {
                    CatalogContext.Current.DeleteCatalog(row.CatalogId);
                    result.CatalogsDeleted++;
                }
                catch (Exception e)
                {
                    result.CatalogsFailed++;
                    _log.Error("Failed to delete catalog: " + row.Name, e);
                }
            }

            DeleteAllMetaClasses(true, result);

            // Remove all imported inRiver Resources
            _log.Debug("Deleting inRiver Media Folder");
            DeleteFileFolder("inRiver", result);

            // Cache is invalid now, clear it
            _log.Debug("Clearing Cache");
            ClearCache();

            _log.Debug("Syncing Content Type Models");

            return result;
        }

        [HttpGet]
        public string ClearCache()
        {
            CatalogCache.Clear();
            EPiServer.CacheManager.Clear();
            return "Cache cleared";
        }

        [HttpGet]
        public string ImportCatalogXml(string filePath)
        {
            CatalogImportExport cie = new CatalogImportExport();
            cie.Import(filePath, true);
            // cie.Import(File.OpenRead(filePath), AppContext.Current.ApplicationId, string.Empty, true);
            return "Catalog Imported";
        }

        /// <summary>
        /// Tells EPiServer to sync the models from code and database.
        /// </summary>
        /// <returns>"200 Content Type Model Scan Performed" if success, 500 and Exception if not.</returns>
        [HttpGet]
        public HttpResponseMessage SyncContentModels(bool forceCommit)
        {
            _log.Information("Forcing a content type model scan and sync.");
            IContentTypeModelScanner modelScanner = ServiceLocator.Current.GetInstance<IContentTypeModelScanner>();
            modelScanner.Sync(forceCommit: forceCommit);
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                ReasonPhrase = "Content Type Model Scan Performed"
            };

            return responseMessage;
        }

        private void DeleteAllMetaClasses(bool doDelete, ClearCatalogAndModelsResult result)
        {
            MetaDataContext metaDataContext = new MetaDataContext();
            MetaClassCollection metaClassCollection = MetaClass.GetList(metaDataContext);
            List<string> logList = new List<string>();
            foreach (MetaClass metaClass in metaClassCollection)
            {
                if (doDelete && metaClass.IsSystem == false)
                {
                    _log.Debug("Deleting class: {0} - {1} (System: {2})",
                        metaClass.Name,
                        metaClass.Id,
                        metaClass.IsSystem);
                    try
                    {
                        MetaClass.Delete(metaDataContext, metaClass.Id);
                        result.MetaClassesDeleted++;
                    }
                    catch (Exception ex)
                    {
                        result.MetaClassesSkipped++;
                        _log.Error(string.Format("Cannot delete Class: {0} - {1} ({2})", metaClass.Name,
                            metaClass.Id, ex.Message), 
                            ex);
                    }
                }
                else
                {
                    result.MetaClassesSkipped++;
                    _log.Debug("NOT deleting system class: {0} - {1} (System: {2})",
                                metaClass.Name,
                                metaClass.Id,
                                metaClass.IsSystem);

                }
            }

            // List of meta data fields to keep.
            List<string> filterFields = new List<string>()
            {
                "TrackingNumber",
                "AddYourOwn"
            };
            MetaFieldCollection fields = MetaField.GetList(metaDataContext);

            foreach (MetaField field in fields)
            {

                // Do not delete: System fields, filtered fields and fields starting with underscore
                if (doDelete
                    && field.IsSystem == false
                    && filterFields.Contains(field.Name) == false
                    && field.Name.StartsWith("_") == false)
                {
                    _log.Debug("Deleting field: {0} - {1} (System: {2})",
                        field.Name,
                        field.Id,
                        field.IsSystem);
                    try
                    {
                        MetaField.Delete(metaDataContext, field.Id);
                        result.MetaFieldsDeleted++;
                    }
                    catch (Exception ex)
                    {
                        result.MetaFieldsSkipped++;
                        _log.Error(string.Format("Cannot delete Field: {0} - {1} ({2})",
                            field.Name,
                            field.Id, ex.Message),
                            ex);
                    }
                }
                else
                {
                    result.MetaFieldsSkipped++;
                    _log.Debug("NOT deleting field: {0} - {1} (System: {2})",
                        field.Name,
                        field.Id,
                        field.IsSystem);
                }
            }
        }

        public static void DeleteFileFolder(string folderName, ClearCatalogAndModelsResult result)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var contentFolder = contentRepository.GetChildren<ContentFolder>(ContentReference.GlobalBlockFolder).Where(x => x.Name == folderName);

            // ReSharper disable PossibleMultipleEnumeration
            if (contentFolder.Any())
            {
                contentRepository.Delete(contentFolder.First().ContentLink, true, AccessLevel.NoAccess);
            }
            // ReSharper restore PossibleMultipleEnumeration


        }

    }
}
