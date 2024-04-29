using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace DataverseAIFunctions
{
    public class OnRecordCreateOrUpdate : PluginBase
    {
        protected override void ExecutePlugin()
        {
            Entity record = Service.Retrieve(PrimaryReference.LogicalName, PrimaryReference.Id, new ColumnSet("sfdemo_content", "sfdemo_operation_type", "sfdemo_category"));

            OptionSetValue opertaionType = record.GetAttributeValue<OptionSetValue>("sfdemo_operation_type");
            if(opertaionType == null || opertaionType.Value != (int)AIOperationType.AIClassify)
            {
                return;
            }

            AIClassifyRequest request = GetRequest(record);
            OrganizationResponse response = Service.Execute(request);


            Entity resultEntity = GetResultEntity((string)response.Results["Classification"]);
            Guid resultId = Service.Create(resultEntity);

            Entity updateEntity = GetUpdateEntity(new EntityReference(resultEntity.LogicalName, resultId));
            Service.Update(updateEntity);
        }

        private AIClassifyRequest GetRequest(Entity record)
        {
            string content = record.GetAttributeValue<string>("sfdemo_content");
            EntityReference categoryRef = record.GetAttributeValue<EntityReference>("sfdemo_category");
            string[] categories = GetClassificationSubcategories(categoryRef);
            return new AIClassifyRequest() 
            {
                Categories = categories,
                Text = content,
            };
        }

        private Entity GetUpdateEntity(EntityReference resultRef)
        {
            Entity updateEntity = new Entity(PrimaryReference.LogicalName, PrimaryReference.Id);
            updateEntity.Attributes["sfdemo_latestresult"] = resultRef;
            return updateEntity;
        }

        private Entity GetResultEntity(string responseText)
        {
            Entity resultEntity = new Entity("sfdemo_ai_function_content_result");
            resultEntity.Attributes["sfdemo_name"] = $"Result-{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}";
            resultEntity.Attributes["sfdemo_content"] = PrimaryReference;
            resultEntity.Attributes["sfdemo_result"] = responseText;
            return resultEntity;
        }

        private string[] GetClassificationSubcategories(EntityReference categoryRef)
        {
            QueryExpression query = new QueryExpression("sfdemo_classification_subcategory")
            {
                ColumnSet = new ColumnSet("sfdemo_name")
            };
            query.Criteria.AddCondition("sfdemo_category", ConditionOperator.Equal, categoryRef.Id);
            return Service.RetrieveMultiple(query).Entities.Select(item => item.GetAttributeValue<string>("sfdemo_name")).ToArray();
        }
    }
}
