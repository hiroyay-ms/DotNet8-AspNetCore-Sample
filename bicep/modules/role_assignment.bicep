param storageAccountName string
param managedIdentityName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' existing = {
  name: storageAccountName
}

// ロール定義 (ストレージ BLOB データ所有者)
// 下記 URL で ID を確認
//https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles
resource blobDataOwnerDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  scope: subscription()
  name: 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
}

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: managedIdentityName
}

// ロール割り当て
// ロールの割当の名前を別のロールの割り当てに再利用するとエラーとなるため、デプロイ時には一意の名前を使用
resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(storageAccount.id, managedIdentity.id, blobDataOwnerDefinition.id)
  scope: storageAccount
  properties: {
    principalId: managedIdentity.properties.principalId
    roleDefinitionId: blobDataOwnerDefinition.id
    principalType: 'ServicePrincipal'
  }
}
