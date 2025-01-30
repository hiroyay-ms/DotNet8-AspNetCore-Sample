param managedIdentityName string
param storageAccountName string
param planName string
param functionName string
param location string
param functionAppRuntime string = 'dotnet-isolated'
param functionAppRuntimeVersion string = '8.0'
param maximumInstanceCount int = 100
param instanceMemoryMB int = 2048

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: managedIdentityName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' existing = {
  name: storageAccountName
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
          name: 'AzureWebJobsStorage_accountName'
          value: storageAccount.name
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
