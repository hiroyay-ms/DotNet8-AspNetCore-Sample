param managedIdentityName string
param storageAccountName string
param appInsightsName string
param planName string
param functionName string
param location string
// 関数アプリのランタイム (dotnet-isolated, java, node, powershell, python)
param functionAppRuntime string = 'dotnet-isolated'
// 関数アプリのランタイム バージョン
param functionAppRuntimeVersion string = '8.0'
// 最大インスタンス数
param maximumInstanceCount int = 100
// インスタンス メモリ (MB)
param instanceMemoryMB int = 2048

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: managedIdentityName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' existing = {
  name: storageAccountName
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: appInsightsName
}

// プランの作成 (Flex 従量課金プラン)
resource functionPlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: planName
  location: location
  kind: 'functionapp'
  sku: {
    tier: 'FlexConsumption'
    name: 'FC1'
  }
  properties: {
    reserved: true
  }
}

var deploymentStorageContainerName = 'app-package-${functionName}'

// 関数アプリの作成
// ユーザー割り当てマネージド ID を関数アプリに割り当て
// ストレージ アカウントへのアクセスには Microsoft Entra 認可を使用
// デプロイメント ストレージの設定
resource functionApp 'Microsoft.Web/sites@2023-12-01'= {
  name: functionName
  location: location
  kind: 'functionapp,linux'
  identity:{
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${managedIdentity.id}':{}
    }
  }
  properties: {
    serverFarmId: functionPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage__accountName'
          value: storageAccount.name
        }
        {
          name: 'AzureWebJobsStorage__clientId'
          value: managedIdentity.properties.clientId
        }
        {
          name: 'AzureWebJobsStorage__credential'
          value: 'managedidentity'
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: applicationInsights.properties.ConnectionString
        }
      ]
    }
    functionAppConfig: {
      deployment: {
        storage:{
          type: 'blobContainer'
          value: '${storageAccount.properties.primaryEndpoints.blob}${deploymentStorageContainerName}'
          authentication: {
            type: 'UserAssignedIdentity'
            userAssignedIdentityResourceId: managedIdentity.id
          }
        }
      }
      scaleAndConcurrency: {
        maximumInstanceCount: maximumInstanceCount
        instanceMemoryMB: instanceMemoryMB
      }
      runtime: {
        name: functionAppRuntime
        version: functionAppRuntimeVersion
      }
    }
  }
}

// ストレージ Blob データ所有者の ID を指定
var storageRoleDifinitionId = 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'

// ストレージ アカウントへ関数アプリからアクセスを許可するため、マネージド ID へロールを割り当て
resource storageRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01'= {
  name: guid(storageAccount.id, functionApp.id, storageRoleDifinitionId)
  scope: storageAccount
  properties: {
    principalId: functionApp.identity.userAssignedIdentities[managedIdentity.id].principalId
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', storageRoleDifinitionId)
    principalType: 'ServicePrincipal'
  }
}
