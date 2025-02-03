param storageAccountName string
param functionName string
param workspaceName string
param location string


// ストレージ アカウントの作成
// ストレージ アカウント キーへのアクセスを無効化、Azure portal で Microsoft Entra 認可を既定にするを有効化
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    allowSharedKeyAccess: false
    defaultToOAuthAuthentication: true
  }
}

// ストレージ アカウントの blob サービス
resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2023-05-01'= {
  parent: storageAccount
  name: 'default'
  properties: {}
}

// デプロイソースとなる zip ファイルを格納するためのコンテナを作成 (Flex 従量課金プラン)
resource container_app_package 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
  parent: blobService
  name: 'app-package-${functionName}'
  properties: {
    publicAccess: 'None'
  }
}

// Azure Functions で使用するコンテナを作成 (azure-webjobs-hosts)
resource container_hosts 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
  parent: blobService
  name: 'azure-webjobs-hosts'
  properties: {
    publicAccess: 'None'
  }
}

// Azure Functions で使用するコンテナを作成 (azure-webjobs-secrets)
resource container_secrets 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
  parent: blobService
  name: 'azure-webjobs-secrets'
  properties: {
    publicAccess: 'None'
  }
}

// ログの格納先となる Log Analytics ワークスペース
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' existing = {
  name: workspaceName
}

// 診断設定 (ストレージ書き込みログとメトリックの取得)
resource storageDiagnosticSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: '${storageAccountName}-logs'
  scope: blobService
  properties: {
    workspaceId: logAnalyticsWorkspace.id
    logs: [
      {
        category: 'StorageWrite'
        enabled: true
      }
    ]
    metrics: [
      {
        category: 'Transaction'
        enabled: true
      }      
    ]
  }
}
