using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace DataverseAIFunctions
{
    public abstract class PluginBase : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Entry point
            try
            {
                InitService(serviceProvider);
                if (!TargetExists())
                {
                    Logger.Trace("TARGET NOT FOUND.");
                    return;
                };
                InitPrimaryEntity();
                Logger.Trace("INIT DONE");
                // Execute actual plugin code
                ExecutePlugin();
                Logger.Trace("EXECUTE DONE");
            }
            catch (Exception ex) 
            {
                throw new InvalidPluginExecutionException(OperationStatus.Failed, ex.Message);    
            }
        }

        protected abstract void ExecutePlugin();
        private void InitService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Logger = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            Context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            Service = ServiceFactory.CreateOrganizationService(Context.UserId);
        }
        private bool TargetExists()
        {
            return Context.InputParameters.Contains("Target");
        }
        private void InitPrimaryEntity()
        {
            switch (Context.MessageName)
            {
                case "Delete":
                    PrimaryReference = Context.InputParameters["Target"] as EntityReference;
                    PrimaryEntity = Service.Retrieve(PrimaryReference.LogicalName, PrimaryReference.Id, new ColumnSet(true));
                    break;
                default:
                    PrimaryEntity = Context.InputParameters["Target"] as Entity;
                    PrimaryReference = new EntityReference(Context.PrimaryEntityName, Context.PrimaryEntityId);
                    break;
            }
            Logger.Trace($"Primary reference: {PrimaryReference?.LogicalName} ({PrimaryReference?.Id.ToString()})");
        }

        protected Entity PrimaryEntity { get; set; }
        protected EntityReference PrimaryReference { get; set; }

        protected ITracingService Logger { get; set; }

        protected IServiceProvider ServiceProvider { get; set; }
        protected IOrganizationService Service { get; set; }
        protected IPluginExecutionContext Context { get; set; }
        protected IOrganizationServiceFactory ServiceFactory { get; set; }
    }
}
