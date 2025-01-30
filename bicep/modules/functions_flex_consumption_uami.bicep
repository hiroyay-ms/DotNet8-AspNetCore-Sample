param managedIdentityName string
param storageAccountName string
param newOrExisting string
param workspaceName string
param appInsightsName string
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

// newOrExisting が 'new' の場合、Log Analytics と Application Insights を作成
// Log Analytics の作成
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' = if (newOrExisting == 'new') {
  name: workspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}

// Application Insights の作成
resource newApplicationInsights 'Microsoft.Insights/components@2020-02-02' = if (newOrExisting == 'new') {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

// 既存の Application Insights の取得
resource exisgintApplicationInsights 'Microsoft.Insights/components@2020-02-02' existing = if (newOrExisting == 'existing') {
  name: appInsightsName
}

var appInsightsConnectionString = newOrExisting == 'new' ? newApplicationInsights.properties.ConnectionString : exisgintApplicationInsights.properties.ConnectionString

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
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
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
