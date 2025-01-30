param storageAccountName string
param functionName string
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


resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2023-05-01'= {
  parent: storageAccount
  name: 'default'
  properties: {}
}

resource container 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
  parent: blobService
  name: 'app-package-${functionName}'
  properties: {
    publicAccess: 'None'
  }
}
